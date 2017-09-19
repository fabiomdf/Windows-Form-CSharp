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
using System.Drawing.Text;
using Persistencia;
using System.IO;



namespace PontosX2.Forms.MensagensEspeciais
{
    public partial class TextosEditorMsgEsp : UserControl
    {

        #region Propriedades

        Fachada fachada = Fachada.Instance;

        public ResourceManager rm;
        private Arquivo_RGN regiao;

        public MensagemEspecial mensagemEspecialGUI;
        public MensagemEmergencia mensagemEmergenciaGUI;
        public Frase fraseFontePainelGUI;
        public int controladorSelecionado;
        public int painelSelecionado;
        private int textoSelecionado;

        #endregion

        #region Construtor

        public TextosEditorMsgEsp()
        {
            InitializeComponent();
            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        #endregion

        #region Globalizacao
        public void AplicaIdioma()
        {
            this.labelApresentacao.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_LABEL_APRESENTACAO");
            this.labelRolagem.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_LABEL_ROLAGEM");
            this.labelFonte.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_LABEL_FONTE");
            this.labelTamanho.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_LABEL_TAMANHO");

            this.btAlinharAbaixo.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_ALIGN_BOTTOM");
            this.btAlinharAcima.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_ALIGN_UP");
            this.btAlinharCentro.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_ALIGN_CENTER");
            this.btAlinharDireita.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_ALIGN_RIGHT");
            this.btAlinharEsquerda.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_ALIGN_LEFT");
            this.btAlinharMeio.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_ALIGN_MIDDLE");
            this.btMudarFonte.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_MUDAR_FONTE");
            this.btNegrito.ToolTipText = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_NEGRITO");
            this.btItalico.ToolTipText = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_ITALICO");
            this.btSublinhado.ToolTipText = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_SUBLINHADO");
            this.btNegrito.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_NEGRITO_TEXT");
            this.btItalico.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_ITALICO_TEXT");
            this.btSublinhado.Text = rm.GetString("MENSAGENS_ESPECIAIS_TOOLSTRIP_SUBLINHADO_TEXT");

            this.gboxEmergencia.Text = rm.GetString("MENSAGENS_ESPECIAIS_GBOX_EMERGENCIA");
            if (Directory.Exists(Fachada.diretorio_NSS))
            {                
                this.gboxEmergencia.Text = rm.GetString("MENSAGENS_ESPECIAIS_GBOX_STOP");
            }

            this.gBoxSaudacoes.Text = rm.GetString("MENSAGENS_ESPECIAIS_GBOX_SAUDACOES");
            this.gBoxDiasSemana.Text = rm.GetString("MENSAGENS_ESPECIAIS_GBOX_SEMANA");
            this.gboxFontePainel.Text = rm.GetString("MENSAGENS_ESPECIAIS_GBOX_FONTE_PAINEL");

            this.labelHora.Text = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_HORA");
            this.labelDataHora.Text = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_DATA_HORA");
            this.labelHoraSaida.Text = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_HORA_SAIDA");
            this.labelTemperatura.Text = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_TEMPERATURA");
            this.labelVelocidade.Text = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_VELOCIDADE");
            this.labelTarifa.Text = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_TARIFA");
            this.labelHoraTemp.Text = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_HORA_TEMPERATURA");
            this.labelDataHoraTemp.Text = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_DATAHORA_TEMPERATURA");

            this.btnCancelar.Text = rm.GetString("MENSAGENS_ESPECIAIS_BTN_CANCELAR");
            this.btnSalvar.Text = rm.GetString("MENSAGENS_ESPECIAIS_BTN_SALVAR");

            this.lbNivelSuavizacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_NIVEL_SUAVIZACAO");

        }

        #endregion

        #region Botoes

        public void HabilitarAlinhamentoHorizontal(bool habilitar)
        {
           if (textoSelecionado == -1 || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.BomDia || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.BoaTarde || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.BoaNoite ||
               textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.SomenteHora || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.DataHora || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Temperatura ||
               textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraSaida || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Velocidade || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Tarifa ||
               textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura)
           { 
                ChecarBotoes();

                btAlinharEsquerda.Enabled = habilitar;
                btAlinharDireita.Enabled = habilitar;
                btAlinharMeio.Enabled = habilitar;
           }
        }

        public void EnableButtonAlinhamentoEsquerda(bool enabled)
        {
            btAlinharEsquerda.Enabled = enabled;
        }

        public void EnableButtonAlinhamentoDireita(bool enabled)
        {
            btAlinharDireita.Enabled = enabled;
        }

        private void btAlinharEsquerda_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
            else
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);

            }
            else
                if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraSaida)
                {
                    Frase fraseTemp = new Frase(mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida]);
                    fraseTemp.LabelFrase = fraseTemp.LabelFrase + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].LabelFrase;
                    fraseTemp.Modelo.Textos[0].LabelTexto = fraseTemp.Modelo.Textos[0].LabelTexto + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseTemp);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseTemp);
                }
                else
                {
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                }
        }

        private void btAlinharMeio_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;
            else
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
            }
            else
                if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraSaida)
                {
                    Frase fraseTemp = new Frase(mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida]);
                    fraseTemp.LabelFrase = fraseTemp.LabelFrase + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].LabelFrase;
                    fraseTemp.Modelo.Textos[0].LabelTexto = fraseTemp.Modelo.Textos[0].LabelTexto + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;

                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseTemp);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseTemp);
                }
                else
                {
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                }
        }

        private void btAlinharDireita_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Direita;
            else
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Direita;

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
            }
            else
                if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraSaida)
                {
                    Frase fraseTemp = new Frase(mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida]);
                    fraseTemp.LabelFrase = fraseTemp.LabelFrase + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].LabelFrase;
                    fraseTemp.Modelo.Textos[0].LabelTexto = fraseTemp.Modelo.Textos[0].LabelTexto + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;

                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseTemp);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseTemp);
                }
                else
                {
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                }
        }

        private void btAlinharAbaixo_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            else
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);

            }
            else
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
            }
        }

        private void btAlinharCentro_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Centro;
            else
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Centro;

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
            }
            else
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
            }
        }

        private void btAlinharAcima_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Cima;
            else
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Cima;

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
            }
            else
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
            }
        }

        public void ChecarBotoes()
        {

            switch ((textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].AlinhamentoH : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoH))
            {
                case Util.Util.AlinhamentoHorizontal.Centralizado:
                    btAlinharEsquerda.Checked = false;
                    btAlinharDireita.Checked = false;
                    btAlinharMeio.Checked = true;
                    break;
                case Util.Util.AlinhamentoHorizontal.Direita:
                    btAlinharEsquerda.Checked = false;
                    btAlinharDireita.Checked = true;
                    btAlinharMeio.Checked = false;
                    break;
                case Util.Util.AlinhamentoHorizontal.Esquerda:
                    btAlinharEsquerda.Checked = true;
                    btAlinharDireita.Checked = false;
                    btAlinharMeio.Checked = false;
                    break;

            }

            switch ((textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].AlinhamentoV : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoV))
            {
                case Util.Util.AlinhamentoVertical.Baixo:
                    btAlinharCentro.Checked = false;
                    btAlinharAcima.Checked = false;
                    btAlinharAbaixo.Checked = true;
                    break;
                case Util.Util.AlinhamentoVertical.Centro:
                    btAlinharCentro.Checked = true;
                    btAlinharAcima.Checked = false;
                    btAlinharAbaixo.Checked = false;
                    break;
                case Util.Util.AlinhamentoVertical.Cima:
                    btAlinharCentro.Checked = false;
                    btAlinharAcima.Checked = true;
                    btAlinharAbaixo.Checked = false;
                    break;

            }

            if ((textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteWindows : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteWindows))
            {
                btMudarFonte.Checked = true;
                if ((textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Italico : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Italico))
                    btItalico.Checked = true;
                else
                    btItalico.Checked = false;

                if ((textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Negrito : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Negrito))
                    btNegrito.Checked = true;
                else
                    btNegrito.Checked = false;

                if ((textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Sublinhado : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Sublinhado))
                    btSublinhado.Checked = true;
                else
                    btSublinhado.Checked = false;
            }

        }

        private void btSublinhado_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
            {
                if (mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Sublinhado)
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Sublinhado = false;
                else
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Sublinhado = true;
            }
            else
            {
                if (mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Sublinhado)
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Sublinhado = false;
                else
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Sublinhado = true;
            }

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
            }
            else
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
            }
        }

        private void btItalico_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
            {
                if (mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Italico)
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Italico = false;
                else
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Italico = true;
            }
            else
            {
                if (mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Italico)
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Italico = false;
                else
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Italico = true;
            }

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
            }
            else
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
            }
        }

        private void btNegrito_Click(object sender, EventArgs e)
        {
            if (textoSelecionado == -1)
            {
                if (mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Negrito)
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Negrito = false;
                else
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Negrito = true;
            }
            else
            {
                if (mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Negrito)
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Negrito = false;
                else
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Negrito = true;
            }

            ChecarBotoes();

            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
            }
            else
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
            }
        }

        private void HabilitarBotoes()
        {

            if ((textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteWindows : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteWindows))
            {
                btMudarFonte.Enabled = true;
                btAlinharAbaixo.Enabled = true;
                btAlinharAcima.Enabled = true;
                btAlinharCentro.Enabled = true;
                btAlinharDireita.Enabled = true;
                btAlinharEsquerda.Enabled = true;
                btAlinharMeio.Enabled = true;
                cbTamanhoFonte.Enabled = true;
                btNegrito.Enabled = true;
                btItalico.Enabled = true;
                btSublinhado.Enabled = true;
                btMudarFonte.Checked = true;
                cbNivelSuavizacao.Enabled = true;
                cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf((textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].BinaryThreshold : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].BinaryThreshold));
            }
            else
            {
                if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.SomenteHora || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.DataHora || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraSaida ||
                    textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Temperatura || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Velocidade || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Tarifa ||
                    textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura ||
                    textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Domingo || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Segunda || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Terca ||
                    textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Quarta || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Quinta || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Sexta ||
                    textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Sabado)
                {
                    btMudarFonte.Enabled = false;
                    btAlinharAbaixo.Enabled = false;
                    btAlinharAcima.Enabled = false;
                    btAlinharCentro.Enabled = false;
                    btAlinharDireita.Enabled = false;
                    btAlinharEsquerda.Enabled = false;
                    btAlinharMeio.Enabled = false;
                }
                else
                {
                    btMudarFonte.Enabled = true;
                    btAlinharAbaixo.Enabled = true;
                    btAlinharAcima.Enabled = true;
                    btAlinharCentro.Enabled = true;
                    btAlinharDireita.Enabled = true;
                    btAlinharEsquerda.Enabled = true;
                    btAlinharMeio.Enabled = true;
                }

                cbTamanhoFonte.Items.Clear();
                cbTamanhoFonte.Enabled = false;
                btNegrito.Enabled = false;
                btItalico.Enabled = false;
                btSublinhado.Enabled = false;
                btNegrito.Checked = false;
                btSublinhado.Checked = false;
                btItalico.Checked = false;
                btMudarFonte.Checked = false;
                cbNivelSuavizacao.Enabled = false;
            }

        }

        private void btMudarFonte_Click(object sender, EventArgs e)
        {
            //Carregar Fontes
            CarregarFontes();

            //Habilitar botoes de acordo com a fonte(Pontos ou Windows)
            HabilitarBotoes();

            //Setando desenho do painel
            if (textoSelecionado == -1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
            }
            else
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
            }
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            if (fachada.CompararObjetosMensagemEspecial(controladorSelecionado, painelSelecionado, mensagemEspecialGUI) || (fachada.CompararObjetosMensagemEmergencia(controladorSelecionado, painelSelecionado, mensagemEmergenciaGUI)))
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("MENSAGENS_ESPECIAIS_MBOX_APPLY"), rm.GetString("MENSAGENS_ESPECIAIS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    Salvar();
                else
                    EditarMensagensEspeciais(fachada.CarregarMensagemEspecial(controladorSelecionado, painelSelecionado), fachada.CarregarMensagemEmergencia(controladorSelecionado, painelSelecionado));

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        public void Salvar()
        {
            fachada.EditarMensagemEspecial(controladorSelecionado, painelSelecionado, mensagemEspecialGUI);
            fachada.EditarMensagemEmergencia(controladorSelecionado, painelSelecionado, mensagemEmergenciaGUI);
            fachada.SetarFontePath(controladorSelecionado, painelSelecionado, mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel].Modelo.Textos[0].Fonte);
        }

        #endregion Botoes

        #region Fontes
        private void CarregarFontes()
        {
            if (!(textoSelecionado == -1 ? mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteWindows : mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteWindows))
            {
                CarregarFontesTrueType();
                SetarFonteTrueTypePadrao();
                PopularNivelSuavizacao();
            }
            else
            {
                CarregarFontesPontos();
                SetarFonteLighDotPadrao();
            }
        }
        private void PopularNivelSuavizacao()
        {
            cbNivelSuavizacao.Items.Clear();
            for (int i = 0; i < 50; i++)
                cbNivelSuavizacao.Items.Add(i * 5);
        }
        private void CarregarFontesPontos()
        {
            cbFonte.Items.Clear();
            cbFonte.Items.AddRange(fachada.ExibeFontesPontos().ToArray());
        }

        private void CarregarFontesTrueType()
        {
            FontFamily sansSerifFont = FontFamily.GenericSansSerif;
            InstalledFontCollection insFont = new InstalledFontCollection();
            FontFamily[] families = insFont.Families;


            cbFonte.Items.Clear();
            foreach (FontFamily family in families)
            {
                cbFonte.Items.Add(family.Name);
            }

            cbTamanhoFonte.Items.Clear();
            for (int i = Util.Util.ALTURA_MINIMA_FONTE_TRUETYPE; i <= Util.Util.ALTURA_MAXIMA_FONTE_TRUETYPE; i++)
                cbTamanhoFonte.Items.Add(i.ToString());

        }

        private void SetarFonteTrueTypePadrao()
        {
            //Setar Fonte Padrao do windows e tamanho
            if (textoSelecionado == -1)
            {
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteWindows = true;
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteAnteriorWindows = false;

                //Setando e selecionando a fonte padrao do Pontos     
                //fachada.SetarFontesDefaultFrases(mensagemEmergenciaGUI.Frases[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
                fachada.SetarFonteDefaultTexto(mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteAnteriorWindows = true;

                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Negrito = false;
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Sublinhado = false;
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Italico = false;

                cbFonte.SelectedItem = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Fonte;
                cbTamanhoFonte.SelectedItem = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Altura.ToString();
            }
            else
            {
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteWindows = true;
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteAnteriorWindows = false;

                //Setando e selecionando a fonte padrao do Pontos     
                //fachada.SetarFontesDefaultFrases(mensagemEspecialGUI.Frases[textoSelecionado], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
                fachada.SetarFonteDefaultTexto(mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteAnteriorWindows = true;

                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Negrito = false;
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Sublinhado = false;
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Italico = false;

                cbFonte.SelectedItem = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                cbTamanhoFonte.SelectedItem = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura.ToString();
            }

        }

        private void SetarFonteLighDotPadrao()
        {

            //Setando e selecionando a fonte padrao do Pontos
            if (textoSelecionado == -1)
            {
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteWindows = false;
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteAnteriorWindows = true;

                //Setando e selecionando a fonte padrao do Pontos     
                //fachada.SetarFontesDefaultFrases(mensagemEmergenciaGUI.Frases[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
                fachada.SetarFonteDefaultTexto(mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteAnteriorWindows = false;

                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Negrito = false;
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Sublinhado = false;
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Italico = false;


                cbFonte.SelectedItem = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Fonte;
            }
            else
            {
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteWindows = false;
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteAnteriorWindows = true;

                //Setando e selecionando a fonte padrao do Pontos     
                //fachada.SetarFontesDefaultFrases(mensagemEspecialGUI.Frases[textoSelecionado], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
                fachada.SetarFonteDefaultTexto(mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteAnteriorWindows = false;

                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Negrito = false;
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Sublinhado = false;
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Italico = false;

                cbFonte.SelectedItem = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
            }
        }

        #endregion Fontes

        public void habilitarMensagemEmergencia()
        {
            if (Directory.Exists(Fachada.diretorio_NSS))
                gboxEmergencia.Enabled = true;
            else
            {
                if (painelSelecionado == 0)
                    gboxEmergencia.Enabled = true;
                else
                {
                    tboxEmergencia.Text = "";
                    gboxEmergencia.Enabled = false;
                    if (textoSelecionado < 0 && fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado) == 1)
                        textoSelecionado = 0;
                }
            }

        }

        #region EntrarNaTela

        public void EditarMensagensEspeciais(MensagemEspecial mensagemEspecial, MensagemEmergencia mensagemEmergencia)
        {
            mensagemEspecialGUI = new MensagemEspecial(mensagemEspecial);
            mensagemEmergenciaGUI = new MensagemEmergencia(mensagemEmergencia);

            //Popular ComboApresentacao
            PopularApresentacao();

            //carregar na GUI os textos
            CarregarTextosGUI();

            //habilitar mensagem de emergência ou mensagem de parada se houver a pasta do nss
            habilitarMensagemEmergencia();

            //Pegando a regiao da fachada
            regiao = fachada.CarregarRegiao(fachada.GetNomeRegiao(controladorSelecionado));

            //Setando o campo na GUI e carregando os dados no painel
            SetarCampo();
        }

        #endregion EntrarNaTela

        #region AlterarGUI

        private void SetarCampo()
        {
            switch (textoSelecionado)
            {
                case -1: tboxEmergencia.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.BomDia: tboxBomDia.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.BoaTarde: tboxBoaTarde.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.BoaNoite: tboxBoaNoite.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Domingo: tboxDomingo.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Segunda: tboxSegunda.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Terca: tboxTerca.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Quarta: tboxQuarta.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Quinta: tboxQuinta.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Sexta: tboxSexta.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Sabado: tboxSabado.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.SomenteHora: tboxHora.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.DataHora: tboxDataHora.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.HoraSaida: tboxHoraSaida.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Temperatura: tboxTemp.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Velocidade: tboxVelocidade.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.Tarifa: tboxTarifa.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura: tboxHoraTemp.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura: tboxDataHoraTemp.Focus(); break;
                case (int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel: tboxFontePainel.Focus(); break;
            }
        }


        private void CarregarTextosGUI()
        {
            tboxBomDia.Text       = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.BomDia].Modelo.Textos[0].LabelTexto;
            tboxBoaTarde.Text     = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.BoaTarde].Modelo.Textos[0].LabelTexto;
            tboxBoaNoite.Text     = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.BoaNoite].Modelo.Textos[0].LabelTexto;
            tboxDomingo.Text      = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Domingo].Modelo.Textos[0].LabelTexto;
            tboxSegunda.Text      = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Segunda].Modelo.Textos[0].LabelTexto;
            tboxTerca.Text        = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Terca].Modelo.Textos[0].LabelTexto;
            tboxQuarta.Text       = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Quarta].Modelo.Textos[0].LabelTexto;
            tboxQuinta.Text       = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Quinta].Modelo.Textos[0].LabelTexto;
            tboxSexta.Text        = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Sexta].Modelo.Textos[0].LabelTexto;
            tboxSabado.Text       = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Sabado].Modelo.Textos[0].LabelTexto;
            tboxHora.Text         = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;
            tboxDataHora.Text     = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].Modelo.Textos[0].LabelTexto;
            tboxHoraSaida.Text    = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].LabelTexto;
            tboxTemp.Text         = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura].Modelo.Textos[0].LabelTexto;
            tboxVelocidade.Text   = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade].Modelo.Textos[0].LabelTexto;
            tboxTarifa.Text       = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa].Modelo.Textos[0].LabelTexto;
            tboxHoraTemp.Text     = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura].Modelo.Textos[0].LabelTexto;
            tboxDataHoraTemp.Text = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura].Modelo.Textos[0].LabelTexto;
            tboxFontePainel.Text  = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel].Modelo.Textos[0].LabelTexto;
            lblHora.Text          = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;


            tboxEmergencia.Text = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].LabelTexto;

        }

        private void cbApresentacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                if (textoSelecionado == -1)
                {
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Apresentacao = (Util.Util.Rolagem)cbApresentacao.SelectedIndex;
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
                }
                else
                {
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Apresentacao = (Util.Util.Rolagem)cbApresentacao.SelectedIndex;
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                }
            }
        }

        private void tbTempoApresentacao_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbTempoApresentacao_TextChanged(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Focused)
                if (tbTempoApresentacao.Text != "")
                {
                    if (textoSelecionado == -1)
                    {
                        mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].TempoApresentacao = System.Convert.ToInt16(tbTempoApresentacao.Text);
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
                    }
                    else
                    {
                        mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].TempoApresentacao = System.Convert.ToInt16(tbTempoApresentacao.Text);
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                    }
                }
        }

        private void tbTempoRolagem_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbTempoRolagem_TextChanged(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Focused)
                if (tbTempoRolagem.Text != "")
                {
                    if (textoSelecionado == -1)
                    {
                        mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].TempoRolagem = System.Convert.ToInt16(tbTempoRolagem.Text);
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
                    }
                    else
                    {
                        mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].TempoRolagem = System.Convert.ToInt16(tbTempoRolagem.Text);
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                    }
                }
        }

        private void cbFonte_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (((ToolStripComboBox)sender).Focused)
            {
                if (textoSelecionado == -1)
                {
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Fonte = cbFonte.SelectedItem.ToString();
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
                }
                else
                {
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte = cbFonte.SelectedItem.ToString();
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);

                    //Setando a fonte padrao para os outros textos
                    if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel)
                    {
                        //setando a fonte default para as outras mensagens especiais
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Domingo].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Segunda].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Terca].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Quarta].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Quinta].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Sexta].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Sabado].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;

                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloSeparadorDecimal].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloVelocidade].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloTemperatura].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloTarifa].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloAM_Espaco].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloPM_Ponto].Modelo.Textos[0].Fonte = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;


                        //setando a altura da fonte escolhida para as outras mensagens especiais
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Domingo].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Segunda].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Terca].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Quarta].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Quinta].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Sexta].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Sabado].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;

                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloSeparadorDecimal].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloVelocidade].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloTemperatura].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloTarifa].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloAM_Espaco].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloPM_Ponto].Modelo.Textos[0].Altura = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura;
                    }
                }
            }
        }

        private void cbTamanhoFonte_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (((ToolStripComboBox)sender).Focused)
            {
                if (textoSelecionado == -1)
                {
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Altura = System.Convert.ToInt16(cbTamanhoFonte.SelectedItem.ToString());
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
                }
                else
                {
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura = System.Convert.ToInt16(cbTamanhoFonte.SelectedItem.ToString());
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                }
            }

        }

        private void PopularApresentacao()
        {
            cbApresentacao.Items.Clear();
            cbApresentacao.Items.AddRange(fachada.CarregarApresentacao().ToArray());
        }

        public void RecarregarApresentacao()
        {
            int indice = cbApresentacao.SelectedIndex;
            cbApresentacao.Items.Clear();
            cbApresentacao.Items.AddRange(fachada.CarregarApresentacao().ToArray());
            cbApresentacao.SelectedIndex = indice;
        }

        private void CarregarGUITextoSelecionado()
        {
            //Se for mensagem de emergencia
            if (textoSelecionado == -1)
            {
                //Seta a GUI com o que esta no construtor da classe Texto
                cbApresentacao.SelectedIndex = (int)mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Apresentacao;
                tbTempoApresentacao.Text = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].TempoApresentacao.ToString();
                tbTempoRolagem.Text = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].TempoRolagem.ToString();


                if (mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].FonteWindows)
                {
                    CarregarFontesTrueType();
                    cbFonte.SelectedItem = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Fonte;
                    cbTamanhoFonte.SelectedItem = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Altura.ToString();
                    cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].BinaryThreshold);
                }
                else
                {
                    CarregarFontesPontos();
                    cbFonte.SelectedItem = mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].Fonte;
                    cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].BinaryThreshold);
                }
            }
            else //Se for mensagens especiais
            {
                //Seta a GUI com o que esta no construtor da classe Texto
                cbApresentacao.SelectedIndex = (int)mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Apresentacao;
                tbTempoApresentacao.Text = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].TempoApresentacao.ToString();
                tbTempoRolagem.Text = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].TempoRolagem.ToString();


                if (mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].FonteWindows)
                {
                    CarregarFontesTrueType();
                    cbFonte.SelectedItem = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                    cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].BinaryThreshold);
                    cbTamanhoFonte.SelectedItem = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Altura.ToString();                    
                }
                else
                {
                    CarregarFontesPontos();
                    cbFonte.SelectedItem = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].Fonte;
                    cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].BinaryThreshold);
                }

            }

            //habilitando os combos
            HabilitarCombos();

            //habilitar botoes
            HabilitarBotoes();

            //checar botões de acordo com o que esta setado no construtor da classe texto
            ChecarBotoes();

        }


        private void HabilitarCombos()
        {
            if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.SomenteHora || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.DataHora || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraSaida ||
                textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Temperatura || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Velocidade || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Tarifa ||
                textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura||
                textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Domingo || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Segunda || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Terca ||
                textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Quarta || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Quinta || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Sexta ||
                textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Sabado)
            {
                cbApresentacao.Enabled = false;
                tbTempoApresentacao.Enabled = false;
                tbTempoRolagem.Enabled = false;

                if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel)
                    cbFonte.Enabled = true;
                else
                    cbFonte.Enabled = false;
            }
            else 
            {
                cbApresentacao.Enabled = true;
                tbTempoApresentacao.Enabled = true;
                tbTempoRolagem.Enabled = true;
                cbFonte.Enabled = true;
            }
        }

        //função chamada pelo User Control ao entrar no objeto e no change(texto, n + texto, texto duplo...)
        public void SetTextoSelecionado(int textoSelecionado, string texto, bool onEnterText)
        {
            this.textoSelecionado = textoSelecionado;

            //Se for mensagem de emergencia
            if (textoSelecionado == -1)
            {
                mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].LabelTexto = texto;
                mensagemEmergenciaGUI.Frases[0].LabelFrase = texto;

                if (onEnterText)
                    CarregarGUITextoSelecionado();

                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);

            }
            else //Se for mensagens especiais
            {
                mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].LabelTexto = texto;
                mensagemEspecialGUI.Frases[textoSelecionado].LabelFrase = texto;

                
                //Se estiver alterando os dias da semana, refletir na data hora
                if (!onEnterText)
                { 
                    
                    if ((textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Domingo && (int)DateTime.Now.DayOfWeek == 0)  || (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Segunda && (int)DateTime.Now.DayOfWeek == 1) || 
                        (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Terca   && (int)DateTime.Now.DayOfWeek == 2)  || (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Quarta  && (int)DateTime.Now.DayOfWeek == 3) ||
                        (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Quinta  && (int)DateTime.Now.DayOfWeek == 4)  || (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Sexta   && (int)DateTime.Now.DayOfWeek == 5) ||
                        (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Sabado  && (int)DateTime.Now.DayOfWeek == 6))
                    {
                        string data;
                        if (regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
                            data = DateTime.Now.ToString("MM/dd/yyyy");
                        else
                            data = DateTime.Now.ToString("dd/MM/yyyy");

                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].Modelo.Textos[0].LabelTexto = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].LabelTexto + " " + data + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;
                        mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].LabelFrase                  = mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].LabelTexto + " " + data + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;
                        tboxDataHora.Text = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].LabelFrase;
                    }
                }


                //Se o evento de chamada das users controls for troca de Textos, deverá carregar todas as informações na GUI, não precisa carregar tudo se for apenas mudança no texto
                if (onEnterText)
                    CarregarGUITextoSelecionado();

                if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.HoraSaida)
                {
                    Frase fraseTemp = new Frase(mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida]);
                    fraseTemp.LabelFrase = fraseTemp.LabelFrase + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].LabelFrase;
                    fraseTemp.Modelo.Textos[0].LabelTexto = fraseTemp.Modelo.Textos[0].LabelTexto + " " + mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;

                    //((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseTemp), fraseTemp);
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseTemp);

                    Util.Util.AlinhamentoHorizontal alinhamento = mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].AlinhamentoH;
                    mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].AlinhamentoH = fraseTemp.Modelo.Textos[0].AlinhamentoH;

                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseTemp);

                    mensagemEspecialGUI.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].AlinhamentoH = alinhamento;

                }
                else
                {
                    if (textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Domingo || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Segunda || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Terca ||
                        textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Quarta || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Quinta || textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Sexta ||
                        textoSelecionado == (int)Util.Util.IndiceMensagensEspeciais.Sabado)
                    {

                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                        if (mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].listaBitMap.Count > 1)
                            mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;

                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                    }
                    else
                    {
                        //Setando desenho do painel
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                    }
                }
            }
        }

        private void TextosEditorMsgEsp_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (Directory.Exists(Fachada.diretorio_NSS))
                    textoSelecionado = -1;
                else
                {
                    if (painelSelecionado == 0 || fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado)>1)
                        textoSelecionado = -1;
                    else
                        textoSelecionado = 0;
                }
                
                EditarMensagensEspeciais(fachada.CarregarMensagemEspecial(controladorSelecionado, painelSelecionado), fachada.CarregarMensagemEmergencia(controladorSelecionado, painelSelecionado));
            }
        }

        #endregion

        #region Eventos TextChange e Enter dos TextBox

        private void tboxBomDia_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.BomDia, tboxBomDia.Text, false);
            }
        }

        private void tboxBomDia_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.BomDia, tboxBomDia.Text, true);
        }

        private void tboxBoaTarde_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.BoaTarde, tboxBoaTarde.Text, false);
            }
        }

        private void tboxBoaTarde_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.BoaTarde, tboxBoaTarde.Text, true);
        }

        private void tboxBoaNoite_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.BoaNoite, tboxBoaNoite.Text, false);
            }
        }

        private void tboxBoaNoite_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.BoaNoite, tboxBoaNoite.Text, true);
        }

        private void tboxDomingo_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Domingo, tboxDomingo.Text, false);
            }
        }

        private void tboxDomingo_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Domingo, tboxDomingo.Text, true);
        }

        private void tboxSegunda_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Segunda, tboxSegunda.Text, false);
            }
        }

        private void tboxSegunda_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Segunda, tboxSegunda.Text, true);
        }

        private void tboxTerca_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Terca, tboxTerca.Text, false);
            }
        }

        private void tboxTerca_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Terca, tboxTerca.Text, true);
        }

        private void tboxQuarta_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Quarta, tboxQuarta.Text, false);
            }
        }

        private void tboxQuarta_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Quarta, tboxQuarta.Text, true);
        }

        private void tboxQuinta_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Quinta, tboxQuinta.Text, false);
            }
        }

        private void tboxQuinta_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Quinta, tboxQuinta.Text, true);
        }

        private void tboxSexta_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Sexta, tboxSexta.Text, false);
            }
        }

        private void tboxSexta_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Sexta, tboxSexta.Text, true);
        }

        private void tboxSabado_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Sabado, tboxSabado.Text, false);
            }
        }

        private void tboxSabado_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Sabado, tboxSabado.Text, true);
        }

        private void tboxHora_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.SomenteHora, tboxHora.Text, false);
            }
        }

        private void tboxHora_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.SomenteHora, tboxHora.Text, true);
        }

        private void tboxDataHora_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.DataHora, tboxDataHora.Text, false);
            }
        }

        private void tboxDataHora_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.DataHora, tboxDataHora.Text, true);
        }

        private void tboxHoraSaida_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.HoraSaida, tboxHoraSaida.Text, false);
            }
        }

        private void tboxHoraSaida_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.HoraSaida, tboxHoraSaida.Text, true);
        }

        private void tboxTemp_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Temperatura, tboxTemp.Text, false);
            }
        }

        private void tboxTemp_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Temperatura, tboxTemp.Text, true);
        }

        private void tboxVelocidade_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Velocidade, tboxVelocidade.Text, false);
            }
        }

        private void tboxVelocidade_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Velocidade, tboxVelocidade.Text, true);
        }

        private void tboxTarifa_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Tarifa, tboxTarifa.Text, false);
            }
        }

        private void tboxTarifa_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.Tarifa, tboxTarifa.Text, true);
        }

        private void tboxHoraTemp_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura, tboxHoraTemp.Text, false);
            }
        }

        private void tboxHoraTemp_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura, tboxHoraTemp.Text, true);
        }

        private void tboxDataHoraTemp_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura, tboxDataHoraTemp.Text, false);
            }
        }

        private void tboxDataHoraTemp_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura, tboxDataHoraTemp.Text, true);
        }

        private void tboxEmergencia_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado(-1, tboxEmergencia.Text, false);
            }
        }

        private void tboxEmergencia_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado(-1, tboxEmergencia.Text, true);
        }

        private void tboxFontePainel_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel, tboxFontePainel.Text, false);
            }
        }

        private void tboxFontePainel_Enter(object sender, EventArgs e)
        {
            SetTextoSelecionado((int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel, tboxFontePainel.Text, true);
        }

        private void cbNivelSuavizacao_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (((ToolStripComboBox)sender).Focused)
            {
                if (textoSelecionado == -1)
                {
                    mensagemEmergenciaGUI.Frases[0].Modelo.Textos[0].BinaryThreshold = System.Convert.ToInt16(cbNivelSuavizacao.SelectedItem.ToString());
                    //Setando desenho do painel
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEmergenciaGUI.Frases[0]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEmergenciaGUI.Frases[0]);
                }
                else
                {
                    mensagemEspecialGUI.Frases[textoSelecionado].Modelo.Textos[0].BinaryThreshold = System.Convert.ToInt16(cbNivelSuavizacao.SelectedItem.ToString());
                    //Setando desenho do painel
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemEspecialGUI.Frases[textoSelecionado]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemEspecialGUI.Frases[textoSelecionado]);
                }

            }
        }

        #endregion

    }
}
