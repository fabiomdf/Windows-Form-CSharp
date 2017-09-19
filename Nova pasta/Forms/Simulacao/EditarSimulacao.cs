using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controlador;
using System.Resources;
using Persistencia;
using System.Collections;
using System.Threading;

namespace PontosX2.Forms.Simulacao
{
    public partial class EditorSimulacao : UserControl
    {
        #region Propriedades e Atributos

        Fachada fachada = Fachada.Instance;
        public ResourceManager rm;
        //Arquivo_ALT alternancia;

        Controlador.Controlador controlador;

        public int controladorSelecionado;
        public int painelSelecionado;
        string strBomDia = "";
        string strBoaTarde = "";
        string strBoaNoite = "";

        #endregion

        #region Construtor

        public EditorSimulacao()
        {
            InitializeComponent();

            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        #endregion

        #region Globalizacao

        public void AplicaIdioma()
        {

            this.gboxAlternancia.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_ALTERNANCIA");
            this.gboxRoteiro.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_ROTEIRO");
            this.gboxMensagem.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_MENSAGEM");
            this.gboxMotorista.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_MOTORISTA");

            this.rbIda.Text = rm.GetString("EDITAR_SIMULACAO_RB_IDA");
            this.rbVolta.Text = rm.GetString("EDITAR_SIMULACAO_RB_VOLTA");

            this.lvRoteiros.Columns[0].Text = rm.GetString("EDITAR_SIMULACAO_LV_ROTEIROS_INDICE");
            this.lvRoteiros.Columns[1].Text = rm.GetString("EDITAR_SIMULACAO_LV_ROTEIROS_TEXTO");

            this.lvMensagens.Columns[0].Text = rm.GetString("EDITAR_SIMULACAO_LV_MENSAGENS_INDICE");
            this.lvMensagens.Columns[1].Text = rm.GetString("EDITAR_SIMULACAO_LV_MENSAGENS_TEXTO");

            this.lvSecMensagens.Columns[0].Text = rm.GetString("EDITAR_SIMULACAO_LV_MENSAGENS_INDICE");
            this.lvSecMensagens.Columns[1].Text = rm.GetString("EDITAR_SIMULACAO_LV_MENSAGENS_TEXTO");

            this.btSimular.Text = rm.GetString("EDITAR_SIMULACAO_BOTAO_SIMULAR");
            this.btnAplicar.Text = rm.GetString("EDITAR_SIMULACAO_BOTAO_APLICAR");

            this.gboxSaudacao.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_SAUDACAO");
            this.labelBomDia.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_BOM_DIA");
            this.labelBoaTarde.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_BOA_TARDE");
            this.labelBoaNoite.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_BOA_NOITE");

            this.gboxInverterLed.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_INVERTER_LED");
            this.lblMinInverterLed.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_MIN_INVERTER_LED");
            this.lblInverterLed.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_INVERTER_LED");
            this.gboxSecMensagem.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_MENSAGEM_SECUNDARIA");

            this.groupBoxBaudrate.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_BAUDRATE");
            int indiceAnterior = comboBoxBaudRate.SelectedIndex;
            this.comboBoxBaudRate.Items.Clear();
            this.comboBoxBaudRate.Items.Add(rm.GetString("EDITAR_SIMULACAO_COMBOBOX_BAUDRATE_ITEM1"));
            this.comboBoxBaudRate.Items.Add(rm.GetString("EDITAR_SIMULACAO_COMBOBOX_BAUDRATE_ITEM2"));

            comboBoxBaudRate.SelectedIndex = indiceAnterior;

            this.groupBoxTimeOutSemComunicacao.Text = rm.GetString("EDITAR_SIMULACAO_GBOX_TIMEOUT_SEM_COMUNICACAO");
            this.labelTimeoutFalhaRede.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_TIMEOUT_SEM_COMUNICACAO");
            this.labelTimeOutSegundos.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_TIMEOUT_SEM_COMUNICACAO_SEGUNDOS");


            this.checkBoxTurnUSBOff.Text = rm.GetString("EDITAR_SIMULACAO_CHECK_TURN_OFF_USB");
            this.groupBoxTurnOffUSB.Text = rm.GetString("EDITAR_SIMULACAO_CHECK_TURN_ONOFF_USB");
        }

        #endregion

        #region EntrarNaTela

        public void EditarSimulacao()
        {

            controlador = fachada.CarregarControlador(controladorSelecionado);

            popularComboAlternancia(controlador.Paineis[painelSelecionado].AlternanciaSelecionada);

            popularComboMotoristas((int)controlador.ParametrosVariaveis.motoristaSelecionado);

            popularComboRoteiros((int)controlador.ParametrosVariaveis.RoteiroSelecionado);

            popularComboMensagens(controlador.Paineis[painelSelecionado].MensagemSelecionada);

            popularComboSecMensagens(controlador.Paineis[painelSelecionado].MensagemSecundariaSelecionada);

            rbIda.Checked = controlador.ParametrosVariaveis.SentidoIda;
            rbVolta.Checked = !controlador.ParametrosVariaveis.SentidoIda;

            popularListFrasesRoteiro();

            popularListFrasesMensagem();

            popularListFrasesMensagemSecundaria();

            popularHoraSaudacao(controlador.ParametrosFixos.HoraInicioDia, controlador.ParametrosFixos.HoraInicioTarde, controlador.ParametrosFixos.HoraInicioNoite);

            popularMinutosInverterLED(controlador.ParametrosFixos.MinutosInverterLED);

            comboBoxBaudRate.SelectedIndex = controlador.ParametrosFixos.baudRate;
            textBoxTimeoutFalhaRede.Text = controlador.ParametrosFixos.timeoutFalhaRede.ToString();

            HabilitarCamposGUI();
        }

        #endregion EntrarNaTela

        #region AlterarGUI

        private void HabilitarCamposGUI()
        {
            if (controlador.Paineis[painelSelecionado].MultiLinhas == 1)
            {
                btBloquearAlternancia.Enabled = true;
                gboxMotorista.Enabled = true;
                gboxSaudacao.Enabled = true;
                gboxMensagem.Enabled = true;
                gboxSecMensagem.Enabled = true;
            }
            else
            {
                btBloquearAlternancia.Enabled = false;
                gboxMotorista.Enabled = false;
                gboxSaudacao.Enabled = false;
                gboxMensagem.Enabled = false;
                gboxSecMensagem.Enabled = false;
            }
        }


        private void btSimular_Click(object sender, EventArgs e)
        {
            if (ValidarCampos(true))
            {              

                SetarParametrosNaFachada();

                FormSimulacao fs = new FormSimulacao();
                fs.controladorSelecionado = this.controladorSelecionado;
                fs.isSimulacao = true;
                fs.ShowDialog();
            }
        }

        private void tboxBomDia_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void tboxBoaTarde_KeyPress(object sender, KeyPressEventArgs e)
        {
           e.Handled = true;
        }

        private void tboxBoaNoite_KeyPress(object sender, KeyPressEventArgs e)
        {
           e.Handled = true;
        }

        private bool IsNumeric(int Val)
        {
            return ((Val >= 48 && Val <= 57) || (Val == 8) || (Val == 46));
        }

        private void tboxBomDia_KeyDown(object sender, KeyEventArgs e)
        {
            int KeyCode = 0;

            //transformando o numpad em algarismos
            switch (e.KeyValue)
            {
                case 96: KeyCode = 48; break;
                case 97: KeyCode = 49; break;
                case 98: KeyCode = 50; break;
                case 99: KeyCode = 51; break;
                case 100: KeyCode = 52; break;
                case 101: KeyCode = 53; break;
                case 102: KeyCode = 54; break;
                case 103: KeyCode = 55; break;
                case 104: KeyCode = 56; break;
                case 105: KeyCode = 57; break;
            }

            if (KeyCode == 0)
                KeyCode = e.KeyValue;

            if (!IsNumeric(KeyCode))
            {
                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = true;
            }

            //Definindo o tamaho máximo de 2 caracteres
            if (strBomDia.Length == 2 && (!(KeyCode == 8 || KeyCode == 46)))
            {
                e.Handled = true;
                return;
            }

            //Se o texto for tamanho 0 também não deve permitir digitar 0
            if (strBomDia.Length == 0 && KeyCode == 48)
            {
                e.Handled = true;
                return;
            }

            if (((KeyCode == 8) || (KeyCode == 46)) && (strBomDia.Length > 0))
            {
                strBomDia = strBomDia.Substring(0, strBomDia.Length - 1);
            }
            else if (!((KeyCode == 8) || (KeyCode == 46)))
            {
                char caractere = Convert.ToChar(KeyCode);

                if (strBomDia.Length > 0)
                {
                    if (Convert.ToUInt16(strBomDia) == 0 || Convert.ToUInt16(strBomDia) == 1)
                        strBomDia = strBomDia + caractere;
                    else
                    {
                        if (Convert.ToUInt16(strBomDia) == 2 && (caractere == '0' || caractere == '1' || caractere == '2' || caractere == '3'))
                            strBomDia = strBomDia + caractere;
                        else
                        {
                            e.Handled = true;
                            return;
                        }

                    }
                }
                else
                    strBomDia = strBomDia + caractere;
            }

            if (strBomDia.Length == 0)
            {
                tboxBomDia.Text = "00";
            }

            if (strBomDia.Length == 1)
            {
                tboxBomDia.Text = "0" + strBomDia;
            }

            if (strBomDia.Length == 2)
            {
                tboxBomDia.Text = strBomDia;
            }

            tboxBomDia.SelectionStart = tboxBomDia.Text.Length + 1;
        }

        private void tboxBomDia_Click(object sender, EventArgs e)
        {
            tboxBomDia.SelectionStart = tboxBomDia.Text.Length + 1;
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos(true))
            {
                SetarParametrosNaFachada();
            }
        }

        public bool ValidarCampos(bool exibirMbox)
        {
            bool retorno = true;
            if (Convert.ToInt16(tboxBomDia.Text) >= Convert.ToInt16(tboxBoaTarde.Text))
            {
                retorno = false;
                if (exibirMbox)
                { 
                    MessageBox.Show(rm.GetString("EDITAR_SIMULACAO_MBOX_BDIA_MAIOR_BTARDE"));
                    tboxBomDia.Focus();
                }
 
            }

            if (Convert.ToInt16(tboxBoaTarde.Text) >= Convert.ToInt16(tboxBoaNoite.Text))
            {
                retorno = false;
                if (exibirMbox)
                { 
                    MessageBox.Show(rm.GetString("EDITAR_SIMULACAO_MBOX_BTARDE_MAIOR_BNOITE"));
                    tboxBoaTarde.Focus();
                }
                
            }

            return retorno;
        }

        public void SetarParametrosNaFachada()
        {            
            //Setando alternancia na fachada
            fachada.SetarAlternanciaSimulacao(controladorSelecionado, painelSelecionado, cboxAlternancia.SelectedIndex);

            //Setando motorista na fachada
            int motorista = (cboxMotorista.SelectedIndex<0?0:cboxMotorista.SelectedIndex);
            fachada.SetarMotoristaSimulacao(controladorSelecionado, motorista);

            //Setando na fachada o roteiro da simulação
            fachada.SetarRoteiroSimulacao(controladorSelecionado, cboxRoteiros.SelectedIndex);

            //Setando na fachada o sentido do roteiro
            fachada.SetarRoteiroSimulacaoIdaVolta(controladorSelecionado, rbIda.Checked);

            //Setando na fachada a mensagem da simulação
            fachada.SetarMensagemSimulacao(controladorSelecionado, painelSelecionado, cboxMensagens.SelectedIndex);

            //Setando na fachada a mensagem secundaria da simulação
            fachada.SetarMensagemSecundariaSimulacao(controladorSelecionado, painelSelecionado, cboxSecMensagem.SelectedIndex);

            //Setando na fachada a hora do bom dia
            fachada.SetarHoraBomDia(controladorSelecionado, Convert.ToInt16(tboxBomDia.Text));

            //Setando na fachada a hora do boa tarde
            fachada.SetarHoraBoaTarde(controladorSelecionado, Convert.ToInt16(tboxBoaTarde.Text));

            //Setando na fachada a hora do boa noite
            fachada.SetarHoraBoaNoite(controladorSelecionado, Convert.ToInt16(tboxBoaNoite.Text));

            //Setando na fachada o tempo para inverter o LED
            fachada.SetarMinutosInverterLED(controladorSelecionado, Convert.ToByte(tboxMinInverterLed.Text));

            //Setando na fachada o tempo para animar quando sem rede
            fachada.SetarSegundosTimeoutFalhaRede(controladorSelecionado, Convert.ToUInt16(textBoxTimeoutFalhaRede.Text));

            //Setando na fachada o BaudRate da Serial do Controlador.
            fachada.SetarBaudRateSerialControlador(controladorSelecionado, Convert.ToByte(comboBoxBaudRate.SelectedIndex));

            //Setar se vai desligada a porta USB
            fachada.SetarTurnOffUSBControlador(controladorSelecionado, checkBoxTurnUSBOff.Checked);

        }

        public bool MudouParametros()
        {
            bool mudou = false;

            if (controlador.Paineis[painelSelecionado].AlternanciaSelecionada != cboxAlternancia.SelectedIndex)
                return true;

            if (controlador.ParametrosVariaveis.RoteiroSelecionado != cboxRoteiros.SelectedIndex)
                return true;

            int motorista = (cboxMotorista.SelectedIndex < 0 ? 0 : cboxMotorista.SelectedIndex);
            if (controlador.ParametrosVariaveis.motoristaSelecionado != motorista)
                return true;

            if (controlador.ParametrosVariaveis.SentidoIda != rbIda.Checked)
                return true;

            if (controlador.Paineis[painelSelecionado].MensagemSelecionada != cboxMensagens.SelectedIndex)
                return true;

            if (controlador.Paineis[painelSelecionado].MensagemSecundariaSelecionada != cboxSecMensagem.SelectedIndex)
                return true;

            if (controlador.ParametrosFixos.HoraInicioDia != Convert.ToInt16(tboxBomDia.Text))
                return true;

            if (controlador.ParametrosFixos.HoraInicioTarde != Convert.ToInt16(tboxBoaTarde.Text))
                return true;

            if (controlador.ParametrosFixos.HoraInicioNoite != Convert.ToInt16(tboxBoaNoite.Text))
                return true;

            if (controlador.ParametrosFixos.MinutosInverterLED != Convert.ToInt16(tboxMinInverterLed.Text))
                return true;

            return mudou;
        }

        private void tboxBomDia_Enter(object sender, EventArgs e)
        {
            tboxBomDia.SelectionStart = tboxBomDia.Text.Length + 1;
        }

        private void tboxBoaTarde_Enter(object sender, EventArgs e)
        {
            tboxBoaTarde.SelectionStart = tboxBoaTarde.Text.Length + 1;
        }

        private void tboxBoaTarde_Click(object sender, EventArgs e)
        {
            tboxBoaTarde.SelectionStart = tboxBoaTarde.Text.Length + 1;
        }

        private void tboxBoaNoite_Enter(object sender, EventArgs e)
        {
            tboxBoaNoite.SelectionStart = tboxBoaNoite.Text.Length + 1;
        }

        private void tboxBoaNoite_Click(object sender, EventArgs e)
        {
            tboxBoaNoite.SelectionStart = tboxBoaNoite.Text.Length + 1;
        }

        private void tboxBoaTarde_KeyDown(object sender, KeyEventArgs e)
        {
            int KeyCode = 0;

            //transformando o numpad em algarismos
            switch (e.KeyValue)
            {
                case 96: KeyCode = 48; break;
                case 97: KeyCode = 49; break;
                case 98: KeyCode = 50; break;
                case 99: KeyCode = 51; break;
                case 100: KeyCode = 52; break;
                case 101: KeyCode = 53; break;
                case 102: KeyCode = 54; break;
                case 103: KeyCode = 55; break;
                case 104: KeyCode = 56; break;
                case 105: KeyCode = 57; break;
            }

            if (KeyCode == 0)
                KeyCode = e.KeyValue;

            if (!IsNumeric(KeyCode))
            {
                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = true;
            }

            //Definindo o tamaho máximo de 2 caracteres
            if (strBoaTarde.Length == 2 && (!(KeyCode == 8 || KeyCode == 46)))
            {
                e.Handled = true;
                return;
            }

            //Se o texto for tamanho 0 também não deve permitir digitar 0
            if (strBoaTarde.Length == 0 && KeyCode == 48)
            {
                e.Handled = true;
                return;
            }

            if (((KeyCode == 8) || (KeyCode == 46)) && (strBoaTarde.Length > 0))
            {
                strBoaTarde = strBoaTarde.Substring(0, strBoaTarde.Length - 1);
            }
            else if (!((KeyCode == 8) || (KeyCode == 46)))
            {
                char caractere = Convert.ToChar(KeyCode);

                if (strBoaTarde.Length > 0)
                {
                    if (Convert.ToUInt16(strBoaTarde) == 0 || Convert.ToUInt16(strBoaTarde) == 1)
                        strBoaTarde = strBoaTarde + caractere;
                    else
                    {
                        if (Convert.ToUInt16(strBoaTarde) == 2 && (caractere == '0' || caractere == '1' || caractere == '2' || caractere == '3'))
                            strBoaTarde = strBoaTarde + caractere;
                        else
                        {
                            e.Handled = true;
                            return;
                        }

                    }
                }
                else
                    strBoaTarde = strBoaTarde + caractere;
            }

            if (strBoaTarde.Length == 0)
            {
                tboxBoaTarde.Text = "00";
            }

            if (strBoaTarde.Length == 1)
            {
                tboxBoaTarde.Text = "0" + strBoaTarde;
            }

            if (strBoaTarde.Length == 2)
            {
                tboxBoaTarde.Text = strBoaTarde;
            }

            tboxBoaTarde.SelectionStart = tboxBoaTarde.Text.Length + 1;
        }

        private void tboxBoaNoite_KeyDown(object sender, KeyEventArgs e)
        {
            int KeyCode = 0;

            //transformando o numpad em algarismos
            switch (e.KeyValue)
            {
                case 96: KeyCode = 48; break;
                case 97: KeyCode = 49; break;
                case 98: KeyCode = 50; break;
                case 99: KeyCode = 51; break;
                case 100: KeyCode = 52; break;
                case 101: KeyCode = 53; break;
                case 102: KeyCode = 54; break;
                case 103: KeyCode = 55; break;
                case 104: KeyCode = 56; break;
                case 105: KeyCode = 57; break;
            }

            if (KeyCode == 0)
                KeyCode = e.KeyValue;

            if (!IsNumeric(KeyCode))
            {
                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = true;
            }

            //Definindo o tamaho máximo de 2 caracteres
            if (strBoaNoite.Length == 2 && (!(KeyCode == 8 || KeyCode == 46)))
            {
                e.Handled = true;
                return;
            }

            //Se o texto for tamanho 0 também não deve permitir digitar 0
            if (strBoaNoite.Length == 0 && KeyCode == 48)
            {
                e.Handled = true;
                return;
            }

            if (((KeyCode == 8) || (KeyCode == 46)) && (strBoaNoite.Length > 0))
            {
                strBoaNoite = strBoaNoite.Substring(0, strBoaNoite.Length - 1);
            }
            else if (!((KeyCode == 8) || (KeyCode == 46)))
            {
                char caractere = Convert.ToChar(KeyCode);

                if (strBoaNoite.Length > 0)
                {
                    if (Convert.ToUInt16(strBoaNoite) == 0 || Convert.ToUInt16(strBoaNoite) == 1)
                        strBoaNoite = strBoaNoite + caractere;
                    else
                    {
                        if (Convert.ToUInt16(strBoaNoite) == 2 && (caractere == '0' || caractere == '1' || caractere == '2' || caractere == '3'))
                            strBoaNoite = strBoaNoite + caractere;
                        else
                        {
                            e.Handled = true;
                            return;
                        }

                    }
                }
                else
                    strBoaNoite = strBoaNoite + caractere;
            }

            if (strBoaNoite.Length == 0)
            {
                tboxBoaNoite.Text = "00";
            }

            if (strBoaNoite.Length == 1)
            {
                tboxBoaNoite.Text = "0" + strBoaNoite;
            }

            if (strBoaNoite.Length == 2)
            {
                tboxBoaNoite.Text = strBoaNoite;
            }

            tboxBoaNoite.SelectionStart = tboxBoaNoite.Text.Length + 1;
        }

        private void cboxSecMensagem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Focused)
            {
                popularListFrasesMensagemSecundaria();
            }
        }

        private void tboxMinInverterLed_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tboxMinInverterLed_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Convert.ToByte(tboxMinInverterLed.Text);
            }
            catch
            {
                if (tboxMinInverterLed.Text != "")
                {
                    MessageBox.Show(rm.GetString("EDITAR_SIMULACAO_MBOX_INVERTER_LED"));
                    tboxMinInverterLed.Text = "0";
                }

                tboxMinInverterLed.Text = "0";
                tboxMinInverterLed.SelectAll();
            }
        }

        private void popularComboSecMensagens(int mensagemSelecionada)
        {
            cboxSecMensagem.Items.Clear();
            foreach (Mensagem m in controlador.Paineis[painelSelecionado].Mensagens)
            {
                //cboxSecMensagem.Items.Add((m.ID).ToString("000") + ": " + m.LabelMensagem);
                cboxSecMensagem.Items.Add((m.Indice).ToString("000") + ": " + m.LabelMensagem);
            }

            cboxSecMensagem.SelectedIndex = mensagemSelecionada;
        }

        private void popularListFrasesMensagemSecundaria()
        {
            lvSecMensagens.Items.Clear();

            for (int row = 0; row < controlador.Paineis[painelSelecionado].Mensagens[cboxSecMensagem.SelectedIndex].Frases.Count; row++)
            {
                ListViewItem item1 = new ListViewItem((row + 1).ToString());

                //Adiciona um ao indice do roteiro para exibição ao usuário
                item1.SubItems.Add(controlador.Paineis[painelSelecionado].Mensagens[cboxSecMensagem.SelectedIndex].Frases[row].LabelFrase);

                lvSecMensagens.Items.Add(item1);

            }
        }

        private void popularMinutosInverterLED(byte Minutos)
        {
            tboxMinInverterLed.Text = Minutos.ToString();

            reposicionarInverterLED();
        }

        public void reposicionarInverterLED()
        {
            tboxMinInverterLed.Location = new Point(lblInverterLed.Location.X + lblInverterLed.Size.Width, tboxMinInverterLed.Location.Y);
            lblMinInverterLed.Location = new Point(tboxMinInverterLed.Location.X + tboxMinInverterLed.Size.Width, lblMinInverterLed.Location.Y);
        }

        private void popularHoraSaudacao(int horaBDia, int horaBTarde, int horaBNoite)
        {
            strBomDia = horaBDia.ToString();
            strBoaTarde = horaBTarde.ToString();
            strBoaNoite = horaBNoite.ToString();

            tboxBomDia.Text = horaBDia.ToString("00");
            tboxBoaTarde.Text = horaBTarde.ToString("00");
            tboxBoaNoite.Text = horaBNoite.ToString("00");

            reposicionarSaudacoes();
        }

        public void reposicionarSaudacoes()
        {
            tboxBomDia.Location = new Point(labelBomDia.Location.X + labelBomDia.Size.Width, tboxBomDia.Location.Y);
            tboxBoaTarde.Location = new Point(labelBoaTarde.Location.X + labelBoaTarde.Size.Width, tboxBoaTarde.Location.Y);
            tboxBoaNoite.Location = new Point(labelBoaNoite.Location.X + labelBoaNoite.Size.Width, tboxBoaNoite.Location.Y);

            labelMinbDia.Location = new Point(tboxBomDia.Location.X + tboxBomDia.Size.Width-3, labelMinbDia.Location.Y);
            labelMinbTarde.Location = new Point(tboxBoaTarde.Location.X + tboxBoaTarde.Size.Width-3, labelMinbTarde.Location.Y);
            labelMinbNoite.Location = new Point(tboxBoaNoite.Location.X + tboxBoaNoite.Size.Width-3, labelMinbNoite.Location.Y);

            labelMinbDia.SendToBack();
            labelMinbTarde.SendToBack();
            labelMinbNoite.SendToBack();

        }

        private void EditarSimulacao_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                EditarSimulacao();
            }
        }

        public void popularComboAlternancia(int alternanciaSelecionada)
        {
            cboxAlternancia.Items.Clear();

            foreach(ItemAlternancia item in controlador.Paineis[painelSelecionado].ListaAlternancias)
            {
                cboxAlternancia.Items.Add(item.NomeAlternancia);
            }

            cboxAlternancia.SelectedIndex = alternanciaSelecionada;
        }

        public int GetAlternanciaSelecionada()
        {
            return cboxAlternancia.SelectedIndex;
        }

        private void popularComboMotoristas(int motoristaSelecionado)
        {
            cboxMotorista.Items.Clear();
            foreach (Controlador.Motorista m in controlador.Paineis[painelSelecionado].Motoristas)
            {
                cboxMotorista.Items.Add(m.ID.LabelFrase + ": " + m.Nome.LabelFrase);
            }

            if (cboxMotorista.Items.Count>0)
                cboxMotorista.SelectedIndex = motoristaSelecionado;
        }

        private void popularComboRoteiros(int roteiroSelecionado)
        {
            cboxRoteiros.Items.Clear();
            foreach(Roteiro r in controlador.Paineis[painelSelecionado].Roteiros)
            {
                cboxRoteiros.Items.Add(r.Numero.LabelFrase + ": " + r.LabelRoteiro);
            }

            cboxRoteiros.SelectedIndex = roteiroSelecionado;
        }

        private void popularComboMensagens(int mensagemSelecionada)
        {
            cboxMensagens.Items.Clear();
            foreach (Mensagem m in controlador.Paineis[painelSelecionado].Mensagens)
            {
                //cboxMensagens.Items.Add((m.ID).ToString("000") + ": " + m.LabelMensagem);
                cboxMensagens.Items.Add((m.Indice).ToString("000") + ": " + m.LabelMensagem);
            }

            cboxMensagens.SelectedIndex = mensagemSelecionada;
        }

        private void popularListFrasesRoteiro()
        {
            lvRoteiros.Items.Clear();

            //atualiza o label dos textos do roteiro se os de ida são iguais aos textos de volta
            AtualizarLabelTextoRoteiro();

            if (rbIda.Checked)
            {
                for (int row = 0; row < controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].FrasesIda.Count; row++)
                {
                    ListViewItem item1 = new ListViewItem((row + 1).ToString());

                    //Adiciona um ao indice do roteiro para exibição ao usuário
                    item1.SubItems.Add(controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].FrasesIda[row].LabelFrase);

                    lvRoteiros.Items.Add(item1);

                }
            }
            else
            {
                // Se o roteiro estiver configurado para os textos de volta serem diferentes dos de ida
                if (!controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].IdaIgualVolta)
                {
                    for (int row = 0; row < controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].FrasesVolta.Count; row++)
                    {
                        ListViewItem item1 = new ListViewItem((row + 1).ToString());

                        //Adiciona um ao indice do roteiro para exibição ao usuário
                        item1.SubItems.Add(controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].FrasesVolta[row].LabelFrase);

                        lvRoteiros.Items.Add(item1);

                    }                    
                }

                // Se o roteiro estiver configurado para os textos de volta serem iguais aos de ida, exibir os textos de ida
                if (controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].IdaIgualVolta)
                {
                    for (int row = 0; row < controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].FrasesIda.Count; row++)
                    {
                        ListViewItem item1 = new ListViewItem((row + 1).ToString());

                        //Adiciona um ao indice do roteiro para exibição ao usuário
                        item1.SubItems.Add(controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].FrasesIda[row].LabelFrase);

                        lvRoteiros.Items.Add(item1);

                    }                    
                }
            }
        }

        private void popularListFrasesMensagem()
        {
            lvMensagens.Items.Clear();

            for (int row = 0; row < controlador.Paineis[painelSelecionado].Mensagens[cboxMensagens.SelectedIndex].Frases.Count; row++)
            {
                ListViewItem item1 = new ListViewItem((row + 1).ToString());

                //Adiciona um ao indice do roteiro para exibição ao usuário
                item1.SubItems.Add(controlador.Paineis[painelSelecionado].Mensagens[cboxMensagens.SelectedIndex].Frases[row].LabelFrase);

                lvMensagens.Items.Add(item1);

            }
        }

        public void AtualizarLabelTextoRoteiro()
        {
            if (controlador.Paineis[painelSelecionado].Roteiros[cboxRoteiros.SelectedIndex].IdaIgualVolta)
                lblTextoRoteiro.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_TEXTOS_ROTEIRO_IGUAIS");
            else
                lblTextoRoteiro.Text = rm.GetString("EDITAR_SIMULACAO_LABEL_TEXTOS_ROTEIRO_DIFERENTES");
        }

        private void cboxRoteiros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Focused)
            {
                popularListFrasesRoteiro();
            }
        }

        private void cboxMensagens_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Focused)
            {
                popularListFrasesMensagem();
            }
        }

        private void rbIda_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Focused)
            {
                popularListFrasesRoteiro();
            }
        }

        private void rbVolta_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Focused)
            {
                popularListFrasesRoteiro();
            }
        }

        #endregion


        private void EditorSimulacao_ClientSizeChanged(object sender, EventArgs e)
        {
            gboxAlternancia.Width = gboxMotorista.Width;
            cboxAlternancia.Width = cboxMotorista.Width - 24;
            btBloquearAlternancia.Location = new Point(cboxAlternancia.Width + 7, btBloquearAlternancia.Location.Y);
        }

        private void btBloquearAlternancia_Click(object sender, EventArgs e)
        {
            if (ValidarCampos(true))
            {
                SetarParametrosNaFachada();

                string nomeAlternancia = controlador.Paineis[painelSelecionado].ListaAlternancias[controlador.Paineis[painelSelecionado].AlternanciaSelecionada].NomeAlternancia;

                FormHabilitaAlternancia janela = new FormHabilitaAlternancia();
                janela.controladorSelecionado = this.controladorSelecionado;
                janela.painelSelecionado = this.painelSelecionado;

                if (janela.ShowDialog(this) == DialogResult.OK)
                {
                    controlador.Paineis[painelSelecionado].AlternanciaSelecionada = -1;

                    for (int i = 0; i < controlador.Paineis[painelSelecionado].ListaAlternancias.Count; i++)
                    {
                        if (controlador.Paineis[painelSelecionado].ListaAlternancias[i].NomeAlternancia == nomeAlternancia)
                        {
                            controlador.Paineis[painelSelecionado].AlternanciaSelecionada = i;
                            break;
                        }
                    }

                    if (controlador.Paineis[painelSelecionado].AlternanciaSelecionada == -1)
                        controlador.Paineis[painelSelecionado].AlternanciaSelecionada = 0;

                    popularComboAlternancia(controlador.Paineis[painelSelecionado].AlternanciaSelecionada);
                }
            }
        }

        private void textBoxTimeoutFalhaRede_TextChanged(object sender, EventArgs e)
        {
            //// Fazer a validação
            //controlador.ParametrosFixos.timeoutFalhaRede = UInt16.Parse(textBoxTimeoutFalhaRede.Text);

            try
            {
                Convert.ToByte(textBoxTimeoutFalhaRede.Text);
            }
            catch
            {
                if (textBoxTimeoutFalhaRede.Text != "")
                {
                    MessageBox.Show(rm.GetString("EDITAR_SIMULACAO_MBOX_INVERTER_LED"));
                    textBoxTimeoutFalhaRede.Text = "0";
                }

                textBoxTimeoutFalhaRede.Text = "0";
                textBoxTimeoutFalhaRede.SelectAll();
            }
        }

        private void comboBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            controlador.ParametrosFixos.baudRate = (byte)comboBoxBaudRate.SelectedIndex;
        }

        private void textBoxTimeoutFalhaRede_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void checkBoxTurnUSBOff_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
