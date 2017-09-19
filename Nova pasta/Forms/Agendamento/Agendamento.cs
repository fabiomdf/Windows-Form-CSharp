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

namespace PontosX2.Forms.Agendamento
{
    public partial class Agendamento : UserControl
    {
        #region Propriedades

        public int controladorSelecionado;
        public int painelSelecionado;
        public bool isEdicao;
        public int indiceEvento;

        Fachada fachada = Fachada.Instance;
        public ResourceManager rm;

        List<Agendamento> listaAgendamentos;
        Evento eventoGUI;
        Arquivo_RGN regiao;

        private const int ACHOU_OPERACAO_PAINEL = 1;
        private const int ACHOU_OPERACAO_TODOS_PAINEIS = 2;

        #endregion

        #region Construtor

        public Agendamento()
        {
            InitializeComponent();

            rm = fachada.carregaIdioma();
            AplicaIdioma();

            listaAgendamentos = new List<Agendamento>();
        }

        #endregion

        #region EntrarNaTela

        private void NovoAgendamento(bool enabled)
        {
            isEdicao = false;
            panelDireita.Enabled = enabled;

            eventoGUI = new Evento(rm.GetString("USER_CONTROL_AGENDAMENTO_TBOX_NOME"));

            tboxNome.Text = eventoGUI.Titulo;
            PopularOperacao();

            if (regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
                dtData.CustomFormat = "MM/dd/yyyy";
            else
                dtData.CustomFormat = "dd/MM/yyyy";

            dtData.Value = eventoGUI.DataHora; 
            //dtHora.Value = DateTime.Parse(eventoGUI.DataHora.ToString("HH")+":00");
            dtHora.Value = eventoGUI.DataHora;

            cboxOperacao.SelectedIndex = (int)eventoGUI.Operacao;
            montarComponentes((int)eventoGUI.Operacao);
        }

        private void EditarAgendamento(bool enabled)
        {
            isEdicao = true;
            indiceEvento = lvAgendamentos.SelectedItems[0].Index;
            panelDireita.Enabled = enabled;

            eventoGUI = new Evento(fachada.CarregarEvento(controladorSelecionado, painelSelecionado, indiceEvento));

            tboxNome.Text = eventoGUI.Titulo;
            PopularOperacao();

            if (regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
                dtData.CustomFormat = "MM/dd/yyyy";
            else
                dtData.CustomFormat = "dd/MM/yyyy";

            dtData.Value = eventoGUI.DataHora;
            dtHora.Value = eventoGUI.DataHora;
            //dtHora.Value = DateTime.Parse(eventoGUI.DataHora.ToString("HH:mm")); 

            cboxOperacao.SelectedIndex = (int)eventoGUI.Operacao;
            montarComponentes((int)eventoGUI.Operacao);
        }

        #endregion

        #region PopularComponentes

        public void CarregarRegiao()
        {
            regiao = fachada.CarregarRegiao(fachada.GetNomeRegiao(this.controladorSelecionado));
        }

        private void PopularListaAgendamentos(List<Evento> eventos)
        {
            string data;
            lvAgendamentos.Items.Clear();           

            for (int row = 0; row < eventos.Count; row++)
            {                
                if (regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
                    data = eventos[row].DataHora.ToString("MM/dd/yyyy HH:mm");
                else
                    data = eventos[row].DataHora.ToString("dd/MM/yyyy HH:mm");

                ListViewItem item1 = new ListViewItem(data);

                //Adiciona um ao indice do roteiro para exibição ao usuário
                item1.SubItems.Add(eventos[row].Titulo);

                lvAgendamentos.Items.Add(item1);
            }
        }

        private void PopularOperacao()
        {
            cboxOperacao.Items.Clear();
            cboxOperacao.Items.AddRange(fachada.CarregarOperacoesAgendamento().ToArray());
        }

        private void popularListaRoteiro()
        {
            listView.Items.Clear();

            if (cboxOperacao.SelectedIndex == (int)Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO)
            {
                Roteiro r = fachada.CarregarRoteiro(controladorSelecionado, painelSelecionado, comboBox.SelectedIndex);

                int i = Math.Max(r.FrasesIda.Count, r.FrasesVolta.Count);

                for (int row = 0; row < i; row++)
                {
                    ListViewItem item1 = new ListViewItem((row + 1).ToString());

                    if (row < r.FrasesIda.Count)
                        item1.SubItems.Add(r.FrasesIda[row].LabelFrase);
                    else
                        item1.SubItems.Add("");

                    if (row < r.FrasesVolta.Count)
                        item1.SubItems.Add(r.FrasesVolta[row].LabelFrase);
                    else
                        item1.SubItems.Add("");

                    listView.Items.Add(item1);
                }
            }

            if (cboxOperacao.SelectedIndex == (int)Util.Util.TipoOperacaoEvento.SELECAO_IDA_VOLTA)
            {
                Roteiro r = fachada.CarregarRoteiro(controladorSelecionado, painelSelecionado, fachada.RetornarParamVariavelRoteiroSelecionado(controladorSelecionado));

                if (rbIda.Checked)
                {
                    for (int row = 0; row < r.FrasesIda.Count; row++)
                    {
                        ListViewItem item1 = new ListViewItem((row + 1).ToString());

                        //Adiciona um ao indice do roteiro para exibição ao usuário
                        item1.SubItems.Add(r.FrasesIda[row].LabelFrase);

                        listView.Items.Add(item1);

                    }
                }
                else
                {
                    for (int row = 0; row < r.FrasesVolta.Count; row++)
                    {
                        ListViewItem item1 = new ListViewItem((row + 1).ToString());

                        //Adiciona um ao indice do roteiro para exibição ao usuário
                        item1.SubItems.Add(r.FrasesVolta[row].LabelFrase);

                        listView.Items.Add(item1);

                    }
                }
            }

        }

        private void popularComboRoteiro()
        {
            comboBox.Items.Clear();
            List<Roteiro> roteiros = fachada.CarregarRoteiros(controladorSelecionado, painelSelecionado);
            foreach (Roteiro r in roteiros)
            {
                comboBox.Items.Add(r.Numero.LabelFrase + ": " + r.LabelRoteiro);
            }

            comboBox.SelectedIndex = eventoGUI.valorParametro;
        }

        private void popularComboMensagem()
        {
            comboBox.Items.Clear();
            List<Mensagem> mensagens = fachada.CarregarMensagens(controladorSelecionado, painelSelecionado);
            foreach (Mensagem m in mensagens)
            {
                comboBox.Items.Add((m.Indice).ToString("000") + ": " + m.LabelMensagem);
            }

            comboBox.SelectedIndex = eventoGUI.valorParametro;
        }

        private void popularListaMensagem()
        {

            listView.Items.Clear();

            Mensagem m = fachada.CarregarMensagem(controladorSelecionado, painelSelecionado, comboBox.SelectedIndex);

            for (int row = 0; row < m.Frases.Count; row++)
            {
                ListViewItem item1 = new ListViewItem((row + 1).ToString());

                //Adiciona um ao indice do roteiro para exibição ao usuário
                item1.SubItems.Add(m.Frases[row].LabelFrase);

                listView.Items.Add(item1);

            }
        }

        private void popularAlternancia()
        {
            comboBox.Items.Clear();
            List<ItemAlternancia> alternancias = fachada.CarregarAlternanciaPainel(controladorSelecionado, painelSelecionado);

            //foreach(ItemAlternancia item in alternancia.listaAlternancias)
            foreach (ItemAlternancia item in alternancias)
            {
                comboBox.Items.Add(item.NomeAlternancia);
            }

            comboBox.SelectedIndex = eventoGUI.valorParametro;
        }


        private void montarComponentes(int opcao)
        {
            switch (opcao)
            {
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO:

                    //Carregando Informações
                    popularComboRoteiro();
                    popularListaRoteiro();

                    //Reposicionando e exibindo componentes
                    gboxOperacao.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_GBOX_OP_ROTEIRO");
                    listView.Columns[1].Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LV_OPERACAO_COLUNA2_ROTEIRO");
                    listView.Columns[2].Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LV_OPERACAO_COLUNA3");
                    comboBox.Location = new Point(6, 25);
                    comboBox.Width = listView.Width;
                    rbIda.Visible = false;
                    rbVolta.Visible = false;
                    dtpHoraSaida.Visible = false;
                    comboBox.Visible = true;
                    listView.Visible = true;
                    listView.Columns[2].Width = 200;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_IDA_VOLTA:

                    //Carregando Informações
                    if (eventoGUI.valorParametro == 0)
                        rbIda.Checked = true;
                    else
                        rbVolta.Checked = true;

                    popularListaRoteiro();

                    //Reposicionando e exibindo componentes
                    gboxOperacao.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_GBOX_OP_SENTIDO");
                    listView.Columns[1].Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LV_OPERACAO_COLUNA2");
                    listView.Columns[2].Width = 0;
                    rbIda.Location = new Point(6, 25);
                    rbVolta.Location = new Point(rbIda.Location.X + rbIda.Width, 25);
                    rbIda.Visible = true;
                    rbVolta.Visible = true;
                    listView.Visible = true;
                    comboBox.Visible = false;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_ALTERNANCIA:

                    //Carregando Informações
                    popularAlternancia();

                    //Reposicionando e exibindo componentes
                    gboxOperacao.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_GBOX_OP_ALTERNANCIA");
                    comboBox.Location = new Point(6, 25);
                    comboBox.Width = listView.Width;
                    comboBox.Visible = true;
                    rbIda.Visible = false;
                    rbVolta.Visible = false;
                    dtpHoraSaida.Visible = false;
                    listView.Visible = false;
                    break;

                case (int)Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL:

                    //Carregando Informações
                    popularComboMensagem();
                    popularListaMensagem();

                    //Reposicionando e exibindo componentes
                    gboxOperacao.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_GBOX_OP_MENSAGEM");
                    listView.Columns[1].Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LV_OPERACAO_COLUNA2");
                    listView.Columns[2].Width = 0;
                    comboBox.Location = new Point(6, 25);
                    comboBox.Visible = true;
                    rbIda.Visible = false;
                    rbVolta.Visible = false;
                    dtpHoraSaida.Visible = false;
                    comboBox.Width = listView.Width;
                    listView.Visible = true;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_MSG_SECUNDARIA:

                    //Carregando Informações
                    popularComboMensagem();
                    popularListaMensagem();

                    //Reposicionando e exibindo componentes
                    gboxOperacao.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_GBOX_OP_MENSAGEM_SEC");
                    listView.Columns[1].Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LV_OPERACAO_COLUNA2");
                    listView.Columns[2].Width = 0;
                    comboBox.Location = new Point(6, 25);
                    comboBox.Visible = true;
                    rbIda.Visible = false;
                    rbVolta.Visible = false;
                    dtpHoraSaida.Visible = false;
                    listView.Visible = true;
                    comboBox.Width = listView.Width;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.ALTERACAO_HORA_SAIDA:

                    //Carregando Informações                    
                    dtpHoraSaida.Value = DateTime.Parse(eventoGUI.valorParametro + ":" + eventoGUI.valorParametro2);

                    //Reposicionando e exibindo componentes
                    gboxOperacao.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_GBOX_OP_HORASAIDA");
                    dtpHoraSaida.Location = new Point(6, 25);
                    comboBox.Visible = false;
                    rbIda.Visible = false;
                    rbVolta.Visible = false;
                    dtpHoraSaida.Visible = true;
                    listView.Visible = false;
                    break;
            }
        }

        private void PreencherObjetoEvento()
        {
            eventoGUI.Titulo = tboxNome.Text;
            eventoGUI.Operacao = (Util.Util.TipoOperacaoEvento)cboxOperacao.SelectedIndex;

            eventoGUI.DataHora = new DateTime(Convert.ToInt16(dtData.Value.ToString("yyyy")), Convert.ToInt16(dtData.Value.ToString("MM")), Convert.ToInt16(dtData.Value.ToString("dd")), Convert.ToInt16(dtHora.Value.ToString("HH")), Convert.ToInt16(dtHora.Value.ToString("mm")), 0);

            switch (cboxOperacao.SelectedIndex)
            {
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO:
                    eventoGUI.valorParametro = (ushort)comboBox.SelectedIndex;
                    eventoGUI.valorParametro2 = 0;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_IDA_VOLTA:
                    if (rbIda.Checked)
                        eventoGUI.valorParametro = 0;
                    else
                        eventoGUI.valorParametro = 1;
                    eventoGUI.valorParametro2 = 0;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_ALTERNANCIA:
                    eventoGUI.valorParametro = (ushort)comboBox.SelectedIndex;
                    eventoGUI.valorParametro2 = 0;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL:
                    eventoGUI.valorParametro = (ushort)comboBox.SelectedIndex;
                    eventoGUI.valorParametro2 = 0;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.SELECAO_MSG_SECUNDARIA:
                    eventoGUI.valorParametro = (ushort)comboBox.SelectedIndex;
                    eventoGUI.valorParametro2 = 0;
                    break;
                case (int)Util.Util.TipoOperacaoEvento.ALTERACAO_HORA_SAIDA:
                    eventoGUI.valorParametro = Convert.ToUInt16(dtpHoraSaida.Value.ToString("HH"));
                    eventoGUI.valorParametro2 = Convert.ToUInt16(dtpHoraSaida.Value.ToString("mm"));
                    break;
            }
        }

        public void RecarregarTela()
        {
            if (!panelDireita.Enabled && !isEdicao)
                tboxNome.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_TBOX_NOME");


            PreencherObjetoEvento();
            PopularOperacao();
            cboxOperacao.SelectedIndex = (int)eventoGUI.Operacao;
            montarComponentes((int)eventoGUI.Operacao);
        }
        #endregion

        #region AlteracaoGUI

        private void Agendamento_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                CarregarRegiao();
                NovoAgendamento(false);
                PopularListaAgendamentos(fachada.CarregarEventos(controladorSelecionado,painelSelecionado));
                btAdd.Focus();
            }
        }
        
        public void MudarPainel()
        {
            NovoAgendamento(false);
            PopularListaAgendamentos(fachada.CarregarEventos(controladorSelecionado, painelSelecionado));
        }

        private void cboxOperacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Focused)
            {
                eventoGUI.valorParametro = 0;
                eventoGUI.valorParametro2 = 0;
                eventoGUI.Operacao = (Util.Util.TipoOperacaoEvento)cboxOperacao.SelectedIndex;
                montarComponentes(cboxOperacao.SelectedIndex);

            }
        }

        private void Agendamento_ClientSizeChanged(object sender, EventArgs e)
        {
            comboBox.Width = listView.Width;

            //reposicionando painel
            panelSplit.Location = new Point(lvAgendamentos.Width + 17, 0);
            panelSplit.Height = lvAgendamentos.Height + 55;
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            PreencherObjetoEvento();

            int indiceEvento;
            if (isEdicao)
                indiceEvento = lvAgendamentos.SelectedItems[0].Index;
            else
                indiceEvento = -1;

            int achou = fachada.AchouObjetoEvento(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
            if (achou == ACHOU_OPERACAO_PAINEL)
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_AGENDAMENTO_MBOX_MESMA_OP_PAINEL"));
                return;
            }
            if (achou == ACHOU_OPERACAO_TODOS_PAINEIS)
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_AGENDAMENTO_MBOX_MESMA_OP_TODOS_PAINEIS"));
                return;
            }

            SalvarAgendamento();

            if (isEdicao)
            {
                indiceEvento = fachada.RetornaIndiceEvento(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
                PopularListaAgendamentos(fachada.CarregarEventos(controladorSelecionado, painelSelecionado));
                lvAgendamentos.Items[indiceEvento].Selected = true;
                EditarAgendamento(false);
            }
            else
            {
                NovoAgendamento(false);
                PopularListaAgendamentos(fachada.CarregarEventos(controladorSelecionado, painelSelecionado));
            }

            btAdd.Focus();
        }

        private void SalvarAgendamento()
        {
            if (isEdicao)
                fachada.EditarEvento(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
            else
                fachada.AdicionarEvento(controladorSelecionado, painelSelecionado, eventoGUI);
        }

        private void lvAgendamentos_DoubleClick(object sender, EventArgs e)
        {
            if (lvAgendamentos.SelectedItems.Count == 0)
            {
                NovoAgendamento(false);
                return;
            }

            EditarAgendamento(true);
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Focused)
            {
                switch (eventoGUI.Operacao)
                {
                    case Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO:
                        popularListaRoteiro();
                        break;
                    case Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL:
                        popularListaMensagem();
                        break;
                    case Util.Util.TipoOperacaoEvento.SELECAO_MSG_SECUNDARIA:
                        popularListaMensagem();
                        break;

                }
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (lvAgendamentos.SelectedItems.Count > 0)
                lvAgendamentos.Items[lvAgendamentos.SelectedIndices[0]].Selected = false;

            NovoAgendamento(true);
            tboxNome.Focus();
        }

        private void btRem_Click(object sender, EventArgs e)
        {
            if (lvAgendamentos.SelectedItems.Count > 0)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_AGENDAMENTO_MBOX_EXCLUIR"), rm.GetString("USER_CONTROL_AGENDAMENTO_MBOX_CONFIRMACAO"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    ExcluirAgendamento();
                    //NovoAgendamento(false);
                    //btRem.Focus();
                }
            }
        }

        private void listView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView.Columns[e.ColumnIndex].Width;
        }

        private void rbIda_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Focused)
                popularListaRoteiro();
        }

        private void rbVolta_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Focused)
                popularListaRoteiro();
        }

        private void lvAgendamentos_Click(object sender, EventArgs e)
        {
            if (lvAgendamentos.SelectedItems.Count == 0)
            {
                NovoAgendamento(false);
                return;
            }

            EditarAgendamento(false);
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            if (isEdicao)
                EditarAgendamento(false);
            else
                NovoAgendamento(false);
        }

        public void ExcluirAgendamento()
        {
            int indiceAgendamento = lvAgendamentos.SelectedItems[0].Index;

            if (indiceAgendamento == lvAgendamentos.Items.Count - 1)
                indiceAgendamento = lvAgendamentos.SelectedItems[0].Index - 1;

            fachada.ExcluirEvento(controladorSelecionado, painelSelecionado, lvAgendamentos.SelectedItems[0].Index);
            PopularListaAgendamentos(fachada.CarregarEventos(controladorSelecionado, painelSelecionado));

            // se possuir itens na lista
            if (indiceAgendamento >= 0)
            {
                lvAgendamentos.Focus();
                lvAgendamentos.Items[indiceAgendamento].Selected = true;
                EditarAgendamento(false);
            }
            else
            {
                //se excluir o ultimo item na lista
                NovoAgendamento(false);
                btRem.Focus();
            }
        }

        #endregion

        #region Globalização

        public void AplicaIdioma()
        {
            lblListaAgendamentos.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LABEL_LISTA");
            lvAgendamentos.Columns[0].Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LV_AGENDAMENTOS_COLUNA1");
            lvAgendamentos.Columns[1].Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LV_AGENDAMENTOS_COLUNA2");
            btAdd.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_BT_ADD");
            btRem.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_BT_DROP");

            
            lblNomeAgendamento.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LABEL_NOME");
            lblDataHora.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LABEL_DATAHORA");
            lblOperacao.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LABEL_OPERACAO");

            rbIda.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_CHECK_IDA");
            rbVolta.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_CHECK_VOLTA");

            listView.Columns[0].Text = rm.GetString("USER_CONTROL_AGENDAMENTO_LV_OPERACAO_COLUNA1");

            btCancelar.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_BT_CANCEL");
            btSalvar.Text = rm.GetString("USER_CONTROL_AGENDAMENTO_BT_APLICAR");

        }

        #endregion

    }
}
