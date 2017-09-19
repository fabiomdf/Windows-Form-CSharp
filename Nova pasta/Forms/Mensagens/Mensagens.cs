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

namespace PontosX2.Forms.Mensagens
{
    public partial class Mensagens : UserControl
    {
        #region Proprieades
        public ListarMensagens ListarMensagens { get; set; }
        public TextosEditorMsg TextosEditorMsg { get; set; }
        Fachada fachada = Fachada.Instance;

        public Mensagem mensagemGUI;
        public int controladorSelecionado;
        public int painelSelecionado;
        public bool incluirMensagem;
        public ResourceManager rm;

        public int colunaOrdenacao;
        public bool ordenacaoAscendente;

        private bool exibirTelaPrimeiraVez;

        #endregion

        #region Construtor
        public Mensagens()
        {
            InitializeComponent();

            exibirTelaPrimeiraVez = true;

            //Popular combo apresentação
            PopularApresentacao();

            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        #endregion

        #region ChamarTelaEditorTextosMensagens
        public void IncluirNovaFrase()
        {
            this.Visible = false;

            TextosEditorMsg.isEdicao = false;
            TextosEditorMsg.painelSelecionado = painelSelecionado;
            TextosEditorMsg.controladorSelecionado = controladorSelecionado;
            TextosEditorMsg.mensagemSelecionada = mensagemGUI.Indice;
            TextosEditorMsg.Visible = true;
            TextosEditorMsg.CriarNovaFrase();


        }

        public void EditarFrase()
        {
            if (listViewMensagens.SelectedItems.Count == 0)
                return;
            this.Visible = false;
            TextosEditorMsg.isEdicao = true;
            TextosEditorMsg.painelSelecionado = painelSelecionado;
            TextosEditorMsg.controladorSelecionado = controladorSelecionado;
            TextosEditorMsg.mensagemSelecionada = mensagemGUI.Indice;
            TextosEditorMsg.Visible = true;
            TextosEditorMsg.fraseSelecionada = System.Convert.ToInt16(listViewMensagens.SelectedItems[0].Text) - 1;
            TextosEditorMsg.EditarFrase(mensagemGUI.Frases[System.Convert.ToInt16(listViewMensagens.SelectedItems[0].Text) - 1]);

        }

        #endregion

        #region ReceberFrasesTelaTextoEditorMensagens
        public void IncluirFraseMensagemGUI(Frase frase)
        {
            mensagemGUI.Frases.Add(new Frase(frase));
            PopularListaTextos();
        }

        public void EditarFraseMensagemGUI(Frase frase)
        {
            mensagemGUI.Frases[TextosEditorMsg.fraseSelecionada] = frase;
            PopularListaTextos();
        }
        #endregion

        #region EntrarNaTela

        public void CriarNovaMensagem()
        {
            mensagemGUI = new Mensagem("*FRT*");
            mensagemGUI.Ordenacao = colunaOrdenacao;
            mensagemGUI.Ascendente = ordenacaoAscendente;

            mensagemGUI.Indice = fachada.QuantidadeMensagens(controladorSelecionado, 0);
            mensagemGUI.ID = fachada.QuantidadeMensagens(controladorSelecionado, 0) + 1;

            //tbIndice.Text = (mensagemGUI.ID).ToString("000");
            tbIndice.Text = (mensagemGUI.Indice).ToString("000");
            tboxNomeMensagem.Text = mensagemGUI.LabelMensagem;

            //setar a fonte padrao do painel na criação da nova mensagem
            fachada.SetarFontesDefaultFrases(mensagemGUI.Frases[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

            //Popular Lista de Textos
            PopularListaTextos();

        }

        public void CarregarMensagemGUI(int controladorSelecionado, int painelSelecionado, int mensagemSelecionada)
        {

            mensagemGUI = new Mensagem(fachada.CarregarMensagem(controladorSelecionado, painelSelecionado, mensagemSelecionada), true);
            //tbIndice.Text = (mensagemGUI.ID).ToString("000");
            tbIndice.Text = (mensagemGUI.Indice).ToString("000");
            tboxNomeMensagem.Text = mensagemGUI.LabelMensagem;
            PopularListaTextos();
        }

        #endregion

        #region AlterarGUI

        public void AplicarMensagem()
        {
            PreencheObjetoGUI();
            if (incluirMensagem)
            {
                fachada.IncluirMensagem(controladorSelecionado, mensagemGUI, chbRepetir.Checked);
                incluirMensagem = false;
                HabilitarCampos();
                CarregarMensagemGUI(controladorSelecionado, painelSelecionado, mensagemGUI.Indice);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
            }
            else
            {
                fachada.EditarMensagem(controladorSelecionado, painelSelecionado, mensagemGUI);
            }

        }

        private void excluirMensagem()
        {
            int indiceTexto = listViewMensagens.SelectedItems[0].Index;

            if (indiceTexto == listViewMensagens.Items.Count - 1)
                indiceTexto = listViewMensagens.SelectedItems[0].Index - 1;

            mensagemGUI.Frases.RemoveAt(System.Convert.ToInt16(listViewMensagens.SelectedItems[0].Text) - 1);
            PopularListaTextos();

            if (listViewMensagens.Items.Count > 0)
            {
                listViewMensagens.Focus();
                listViewMensagens.Items[indiceTexto].Selected = true;
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
            }
            else
                //Setando painel vazio
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();


            ////Setando painel vazio
            //((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

            //if (listViewMensagens.Items.Count > 0)
            //    ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
        }

        public bool ItemSelecionadoLista()
        {
            bool retorno = false;

            if (listViewMensagens.SelectedItems.Count > 0)
                retorno = true;

            return retorno;
        }

        public List<Frase> ExibirTodosTextos()
        {
            return mensagemGUI.Frases;
        }

        public void SelecionarItemListView(int indice)
        {
            listViewMensagens.Items[indice].Selected = true;
        }

        private void cbApresentacaoPadrao_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (exibirTelaPrimeiraVez)
                exibirTelaPrimeiraVez = false;
            else
            { */
            if (cbApresentacaoPadrao.SelectedIndex != -1)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_APRESENTACAO"), rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    //Muda a forma de apresentação para todos os textos da mensagem
                    SetApresentacaoPadraoTextos();
                }

                cbApresentacaoPadrao.SelectedIndex = -1;

            }
            // }
        }

        private void SetApresentacaoPadraoTextos()
        {
            for (int i = 0; i < mensagemGUI.Frases.Count; i++)
            {
                for (int j = 0; j < mensagemGUI.Frases[i].Modelo.Textos.Count; j++)
                {
                    mensagemGUI.Frases[i].Modelo.Textos[j].Apresentacao = (Util.Util.Rolagem)cbApresentacaoPadrao.SelectedIndex;
                }
            }

        }

        private void listViewMensagens_DoubleClick(object sender, EventArgs e)
        {
            EditarFrase();

            //travar combobox do painel
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
        }

        public void ChamarTelaEdicaoTexto(int indiceFrase)
        {
            SelecionarItemListView(indiceFrase);
            EditarFrase();
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
        }

        private void PopularListaTextos()
        {
            //Popular Lista de Textos de Ida
            listViewMensagens.Items.Clear();
            for (int row = 0; row < mensagemGUI.Frases.Count; row++)
            {
                ListViewItem item1 = new ListViewItem((row + 1).ToString());

                //Adiciona um ao indice do roteiro para exibição ao usuário
                item1.SubItems.Add(mensagemGUI.Frases[row].LabelFrase);

                listViewMensagens.Items.Add(item1);

            }
        }

        public void PopularApresentacao()
        {
            cbApresentacaoPadrao.Items.Clear();
            cbApresentacaoPadrao.Items.AddRange(fachada.CarregarApresentacao().ToArray());
        }


        private void Mensagens_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                HabilitarBotaoApresentacao();
            }
        }

        public void HabilitarBotaoApresentacao()
        {
            if (listViewMensagens.Items.Count > 0)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
        }

        public void HabilitarCampos()
        {
            cbApresentacaoPadrao.SelectedIndex = -1;

            if (painelSelecionado == 0 || incluirMensagem)
            {
                //tboxNomeMensagem.ReadOnly = false;
                if (painelSelecionado == 0 && incluirMensagem)
                {
                    chbRepetir.Visible = true;
                    chbRepetir.Checked = true;
                }
                else
                {
                    chbRepetir.Visible = false;
                    chbRepetir.Checked = false;
                }
            }
            else
            {
                //tboxNomeMensagem.ReadOnly = true;
                chbRepetir.Checked = false;
                chbRepetir.Visible = false;
            }

            tboxNomeMensagem.Focus();

        }

        private void listViewMensagens_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (((ListView)sender).ContainsFocus)
            {
                if (listViewMensagens.SelectedItems.Count > 0)
                {
                    //Setando desenho do painel
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), mensagemGUI.Frases[System.Convert.ToInt16(listViewMensagens.SelectedItems[0].Text) - 1]);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(mensagemGUI.Frases[System.Convert.ToInt16(listViewMensagens.SelectedItems[0].Text) - 1]);
                }
                else
                {
                    //Setando painel vazio
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

                    if (listViewMensagens.Items.Count > 0)
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
                }
            }
        }

        public void DesmarcarListViews()
        {
            if (listViewMensagens.SelectedItems.Count > 0)
                listViewMensagens.Items[listViewMensagens.SelectedIndices[0]].Selected = false;
        }

        private void tboxNomeMensagem_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                mensagemGUI.LabelMensagem = tboxNomeMensagem.Text;
            }
        }

        private void listViewMensagens_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Link);
        }

        private void listViewMensagens_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void listViewMensagens_DragDrop(object sender, DragEventArgs e)
        {
            Point cp = listViewMensagens.PointToClient(new Point(e.X, e.Y));
            ListViewItem dragToItem = listViewMensagens.GetItemAt(cp.X, cp.Y);

            if (dragToItem != null)
            {
                ListViewItem dragItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                int itemOrigem = dragItem.Index;
                int itemDestino = dragToItem.Index;

                mensagemGUI.MoverFrases(itemOrigem, itemDestino);

                PopularListaTextos();
                SelecionarItemListView(itemDestino);
            }
        }

        #endregion

        #region Globalizacao
        public void AplicaIdioma()
        {


            this.groupBoxInfo.Text = rm.GetString("USER_CONTROL_MENSAGENS_GROUP_BOX_TITLE");
            this.labelIndiceMensagem.Text = rm.GetString("USER_CONTROL_MENSAGENS_LABEL_INDICE_MENSAGEM");
            this.lblNomeMensagem.Text = rm.GetString("USER_CONTROL_MENSAGENS_LABEL_MENSAGEM");
            this.labelApresentaçãoPadrão.Text = rm.GetString("USER_CONTROL_MENSAGENS_LABEL_APRESENTACAO");
            this.chbRepetir.Text = rm.GetString("USER_CONTROL_MENSAGENS_CHECK_BOX_REPETIR");

            this.listViewMensagens.Columns[0].Text = rm.GetString("USER_CONTROL_MENSAGENS_DATA_GRID_INDICE");
            this.listViewMensagens.Columns[1].Text = rm.GetString("USER_CONTROL_MENSAGENS_DATA_GRID_TEXTO");

            this.tabIda.Text = rm.GetString("USER_CONTROL_MENSAGENS_TAB_IDA");

            this.btnIncluirPainel.Text = rm.GetString("USER_CONTROL_MENSAGENS_BTN_INCLUIR");
            this.btnExcluirPainel.Text = rm.GetString("USER_CONTROL_MENSAGENS_BTN_EXCLUIR");

            this.btnCancelar.Text = rm.GetString("USER_CONTROL_MENSAGENS_BTN_CANCELAR");
            this.btnAplicar.Text = rm.GetString("USER_CONTROL_MENSAGENS_BTN_APLICAR");
            this.btnContinuar.Text = rm.GetString("USER_CONTROL_MENSAGENS_BTN_CONTINUAR");

        }
        #endregion

        #region Botoes

        private void btnContinuar_Click(object sender, EventArgs e)
        {

            if (fachada.GetMultiLinhas(controladorSelecionado, 0) > 1)
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_INCLUIR_MULTILINHAS"));
                return;
            }

            //Aplicando a mensagem
            PreencheObjetoGUI();

            if (!exibirErro())
            {
                if (incluirMensagem)
                    fachada.IncluirMensagem(controladorSelecionado, mensagemGUI, chbRepetir.Checked);
                else
                    fachada.EditarMensagem(controladorSelecionado, painelSelecionado, mensagemGUI);

                //Criando a próxima mensagem
                painelSelecionado = 0;
                incluirMensagem = true;
                CriarNovaMensagem();
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, 0);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
                HabilitarCampos();
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (!exibirErro())
                AplicarMensagem();
        }

        public Boolean exibirErro()
        {

            if (tboxNomeMensagem.Text == "")
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_LABEL_VAZIO"));
                tboxNomeMensagem.Focus();
                return true;
            }

            return false;
        }

        public void PreencheObjetoGUI()
        {
            mensagemGUI.LabelMensagem = tboxNomeMensagem.Text;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bool aplicou = false;

            PreencheObjetoGUI();

            if (incluirMensagem)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_APLICAR"), rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    if (!exibirErro())
                    {
                        AplicarMensagem();
                        aplicou = true;
                    }
                    else
                        return;
            }
            else //Edição
            {
                if (fachada.CompararObjetosMensagem(controladorSelecionado, painelSelecionado, mensagemGUI))
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_ALTERAR"), rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        if (!exibirErro())
                        {
                            AplicarMensagem();
                            aplicou = true;
                        }
                        else
                            return;
            }

            this.Visible = false;
            ListarMensagens.Visible = true;

            //Setando na lista de mensagens, a mensagem adicionada ou editada
            if (aplicou)
                ListarMensagens.SelecionarMensagem(fachada.IndexMensagem(controladorSelecionado, painelSelecionado, mensagemGUI.ID), false);
            else
                if (!incluirMensagem)
                    ListarMensagens.SelecionarMensagem(fachada.IndexMensagem(controladorSelecionado, painelSelecionado, mensagemGUI.ID), false);

            //Setando painel vazio
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

            if (incluirMensagem)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
        }

        private void btnExcluirPainel_Click(object sender, EventArgs e)
        {
            if (listViewMensagens.SelectedItems.Count > 0)
            {
                if (listViewMensagens.Items.Count > 1)
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_EXCLUIR_TEXTO"), rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                        excluirMensagem();
                }
                else
                    MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_EXCLUIR_TEXTO_ULTIMO"));
            }
        }

        private void btnIncluirPainel_Click(object sender, EventArgs e)
        {
            IncluirNovaFrase();

            //travar combobox do painel
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
        }

        #endregion


    }
}
