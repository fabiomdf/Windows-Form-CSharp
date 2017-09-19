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
using C1.Win.C1FlexGrid;

namespace PontosX2.Forms.Mensagens
{
    public partial class ListarMensagens : UserControl
    {
        #region Propriedades

        public Mensagens EditorMensagens { get; set; }
        public int painelSelecionado;
        public int controladorSelecionado;
        Fachada fachada = Fachada.Instance;
        public ResourceManager rm;

        //Propriedades do dragdrop dbgridview
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private const int COLUNA_NOME = 0;
        private const int SEM_ORDENACAO = 255;
        private int colunaOrdenacao = COLUNA_NOME;
        private bool ordenacaoAscendente = true;
        private bool expandir = true;

        private List<Mensagem> listaMensagens;

        #endregion

        #region Construtor

        public ListarMensagens()
        {
            InitializeComponent();

            listaMensagens = new List<Mensagem>();

            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        #endregion

        #region Botoes

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            // so permite adicionar mensagens se o painel principal não for multinhas
            if (fachada.GetMultiLinhas(controladorSelecionado, 0) == 1)
                IncluirNovaMensagem();
            else
                MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_INCLUIR_MULTILINHAS"));
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (gridMensagens.Selection.r1 > 0)
            {
                if (fachada.QuantidadeMensagens(controladorSelecionado, painelSelecionado) > 1)
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_EXCLUIR"), rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        int qtdAgendamentos = fachada.QuantidadeAgendamentosMensagem(controladorSelecionado, (int)gridMensagens.Rows[gridMensagens.Selection.r1].Node.Row[1]);
                        if (qtdAgendamentos == 0)
                            excluirMensagem();
                        else
                        {
                            string msg;
                            if (qtdAgendamentos == 1)
                                msg = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_EXCLUIR_AGENDAMENTO1") + " " + qtdAgendamentos + " " + rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_EXCLUIR_AGENDAMENTO2_S");
                            else
                                msg = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_EXCLUIR_AGENDAMENTO1") + " " + qtdAgendamentos + " " + rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_EXCLUIR_AGENDAMENTO2_P");

                            if (DialogResult.Yes == MessageBox.Show(msg, rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                                excluirMensagem();
                        }
                    }
                }
                else
                    MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MBOX_EXCLUIR_ULTIMO"));
            }


            //int qtdAgendamentos = fachada.QuantidadeAgendamentosRoteiro(controladorSelecionado, (int)gridRoteiros.Rows[gridRoteiros.Selection.r1].Node.Row[1]);
            //if (qtdAgendamentos == 0)
            //    ExcluirRoteiro();
            //else
            //{
            //    string msg;
            //    if (qtdAgendamentos == 1)
            //        msg = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_AGENDAMENTO1") + " " + qtdAgendamentos + " " + rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_AGENDAMENTO2_S");
            //    else
            //        msg = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_AGENDAMENTO1") + " " + qtdAgendamentos + " " + rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_AGENDAMENTO2_P");

            //    if (DialogResult.Yes == MessageBox.Show(msg, rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            //        ExcluirRoteiro();
            //}
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            CopiarMensagem();
        }

        #endregion

        #region AlterarGUI

        public void LimparGridMensagens()
        {
            gridMensagens.BeginUpdate();

            if (gridMensagens.Rows.Count > 1)
                gridMensagens.Rows.RemoveRange(1, gridMensagens.Rows.Count - 1);

            gridMensagens.EndUpdate();
        }

        public void excluirMensagem()
        {
            int indiceMensagem = (int)gridMensagens.Rows[gridMensagens.Selection.r1].Node.Row[1]; ;

            fachada.ExcluirMensagem(controladorSelecionado, indiceMensagem);

            LimparGridMensagens();
            PopulaMensagensGrid(fachada.CarregarMensagens(controladorSelecionado, painelSelecionado));

            if (indiceMensagem == fachada.QuantidadeMensagens(controladorSelecionado, painelSelecionado))
                indiceMensagem = indiceMensagem - 1;

            SelecionarMensagem(indiceMensagem, false);

        }

        internal void SelecionarMensagem(int indiceMensagem, bool expandir)
        {
            for (int i = 1; i <= gridMensagens.Rows.Count - 1; i++)
            {
                if (gridMensagens.Rows[i].Node.Row[1].ToString() == gridMensagens.Rows[i].Node.Row[2].ToString() && indiceMensagem == (int)gridMensagens.Rows[i].Node.Row[1])
                {
                    gridMensagens.Row = i;
                    if (expandir)
                        gridMensagens.Rows[i].Node.Expanded = true;
                    gridMensagens.Focus();
                    return;
                }
            }
        }

        public void DeselectGrid()
        {
            //desmarcando o item selecionado
            gridMensagens.Select(-1, -1);
        }

        private void listViewMensagens_DoubleClick(object sender, EventArgs e)
        {
            EditarMensagem();
        }

        private void ListarMensagens_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {

                SortList(colunaOrdenacao, false);
                fachada.AtualizarIndicesMensagens(controladorSelecionado);

                //Recarrega flexfrid de mensagens
                LimparGridMensagens();
                PopulaMensagensGrid(fachada.CarregarMensagens(controladorSelecionado, painelSelecionado));

            }
        }

        private void CopiarMensagem()
        {
            if (gridMensagens.Selection.r1 > 0)
            {
                int indiceMensagem = (int)gridMensagens.Rows[gridMensagens.Selection.r1].Node.Row[1];

                //copiando a rota
                fachada.CopiarMensagem(controladorSelecionado, indiceMensagem, rm.GetString("MENSAGEM_COPIA"));

                //reordenando na lista e atualizando os indices
                SortList(colunaOrdenacao, false);
                fachada.AtualizarIndicesMensagens(controladorSelecionado);

                //Recarregar informações no listview
                LimparGridMensagens();
                PopulaMensagensGrid(fachada.CarregarMensagens(controladorSelecionado, painelSelecionado));

                SelecionarMensagem(indiceMensagem, false);
            }
        }

        private void MenuItemCopy_Click(object sender, EventArgs e)
        {
            CopiarMensagem();
        }


        public void PopulaMensagensGrid(List<Mensagem> mensagens)
        {
            listaMensagens = mensagens;

            gridMensagens.BeginUpdate();

            for (int row = 0; row < mensagens.Count; row++)
            {

                var node = gridMensagens.Rows.AddNode(0);

                node.Row[1] = row;
                node.Row[2] = row;
                node.Row[3] = (row).ToString("000");
                node.Row[4] = mensagens[row].LabelMensagem;

                if (mensagens[row].Frases.Count > 0)
                {
                    node = gridMensagens.Rows.AddNode(1);
                    node.Row[1] = row;
                    node.Row[2] = String.Empty;
                    node.Row[3] = "      " + rm.GetString("USER_CONTROL_MENSAGENS_TAB_IDA");
                    node.Row[4] = String.Empty;

                    int z = 0;
                    foreach (Frase f in mensagens[row].Frases)
                    {
                        node = gridMensagens.Rows.AddNode(2);
                        node.Row[1] = row;
                        node.Row[2] = "T" + z;
                        node.Row[3] = String.Empty;
                        node.Row[4] = f.LabelFrase;
                        z = z + 1;
                    }
                }
            }

            //Setar as colunas que dividem os textos para Bold
            SetarBold();

            //Setando para não expandir os nós
            collapseOrExpandeAll(false);

            //desmarcando o item selecionado
            gridMensagens.Select(-1, -1);

            //marcar no flexgrid a coluna que esta ordenado
            SetarColunaOrdenacao();

            gridMensagens.EndUpdate();

        }

        private void SetarColunaOrdenacao(byte colunaOrdenacaoForcada = 0)
        {
            if (listaMensagens.Count > 0)
            {
                ordenacaoAscendente = listaMensagens[0].Ascendente;
                if (colunaOrdenacaoForcada == 0)
                {
                    colunaOrdenacao = listaMensagens[0].Ordenacao;
                }
                else
                {
                    colunaOrdenacao = colunaOrdenacaoForcada;
                }


                if (colunaOrdenacao == COLUNA_NOME)
                {
                    if (!ordenacaoAscendente)
                        gridMensagens.ShowSortAt(C1.Win.C1FlexGrid.SortFlags.Ascending, 5);
                    else
                        gridMensagens.ShowSortAt(C1.Win.C1FlexGrid.SortFlags.Descending, 5);

                    gridMensagens.ShowSortPosition = ShowSortPositionEnum.Auto;

                }
                if (colunaOrdenacao == SEM_ORDENACAO)
                {
                    gridMensagens.ShowSortPosition = ShowSortPositionEnum.None;
                }
            }
        }
        private void SetarBold()
        {

            C1.Win.C1FlexGrid.CellStyle cs = gridMensagens.Styles.Add("FontBold");
            cs.Font = new Font(gridMensagens.Font.Name, gridMensagens.Font.Size, FontStyle.Bold);

            for (int i = 1; i < gridMensagens.Rows.Count; i++)
            {
                if (gridMensagens.Rows[i].Node.Row[2].Equals(String.Empty))
                {
                    gridMensagens.Rows[i].Style = cs;
                }
            }
        }

        public void collapseOrExpandeAll(bool expandido)
        {
            if (expandido)
            {
                foreach (C1.Win.C1FlexGrid.Node no in this.gridMensagens.Nodes)
                {
                    gridMensagens.SetCellImage(0, 0, (Image)global::PontosX2.Properties.Resources.collapse);
                    no.Expanded = true;
                    if (no.Nodes.Length > 0)
                        no.Nodes[0].Expanded = true;
                }
            }
            else
            {
                foreach (C1.Win.C1FlexGrid.Node no in this.gridMensagens.Nodes)
                {
                    no.Collapsed = true;
                    gridMensagens.SetCellImage(0, 0, (Image)global::PontosX2.Properties.Resources.expand);
                }
            }

            expandir = !expandido;
        }

        #endregion

        #region Globalizacao

        public void AplicaIdioma()
        {
            btnIncluir.Text = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_BTN_INCLUIR");
            btnExcluir.Text = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_BTN_EXCLUIR");
            btnCopiar.Text = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MENU_COPIAR");
            incluirToolStripMenuItem.Text = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MENU_INCLUIR");
            deleteToolStripMenuItem.Text = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MENU_EXCLUIR");
            MenuItemCopy.Text = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_MENU_COPIAR");

            gridMensagens.Cols[3].Caption = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_COLUNA_INDICE");
            gridMensagens.Cols[4].Caption = rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_COLUNA_MENSAGEM");

        }

        #endregion

        #region ChamarTelaMensagens

        private void IncluirNovaMensagem()
        {
            EditorMensagens.controladorSelecionado = controladorSelecionado;
            EditorMensagens.painelSelecionado = 0;
            EditorMensagens.incluirMensagem = true;

            EditorMensagens.colunaOrdenacao = colunaOrdenacao;
            EditorMensagens.ordenacaoAscendente = ordenacaoAscendente;
            this.Visible = false;
            EditorMensagens.CriarNovaMensagem();
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, 0);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
            EditorMensagens.Visible = true;
            EditorMensagens.HabilitarCampos();
        }

        private void EditarMensagem()
        {
            if (gridMensagens.Selection.r1 < 0)
                return;

            EditorMensagens.controladorSelecionado = controladorSelecionado;
            EditorMensagens.painelSelecionado = painelSelecionado;
            EditorMensagens.incluirMensagem = false;

            EditorMensagens.colunaOrdenacao = colunaOrdenacao;
            EditorMensagens.ordenacaoAscendente = ordenacaoAscendente;
            this.Visible = false;
            EditorMensagens.CarregarMensagemGUI(controladorSelecionado, painelSelecionado, (int)gridMensagens.Rows[gridMensagens.Selection.r1].Node.Row[1]);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
            EditorMensagens.Visible = true;
            EditorMensagens.HabilitarCampos();
        }

        #endregion

        #region SortList

        private void SortList(int coluna, bool trocarOrdenacao)
        {
            this.Cursor = Cursors.WaitCursor;

            switch (coluna)
            {
                //case COLUNA_ID:
                //    if (trocarOrdenacao)
                //        ordenacaoAscendente = !ordenacaoAscendente;
                //    colunaOrdenacao = COLUNA_ID;
                //    foreach (Painel p in fachada.CarregarPaineis(controladorSelecionado))
                //    {
                //        p.Mensagens.Sort(delegate(Mensagem a, Mensagem b)
                //        {
                //            if (ordenacaoAscendente)
                //            {
                //                return
                //                a.ID.CompareTo(b.ID);
                //            }
                //            else
                //            {
                //                return
                //                b.ID.CompareTo(a.ID);
                //            }
                //        });

                //        foreach (Mensagem m in p.Mensagens)
                //        {
                //            m.Ordenacao = COLUNA_ID;
                //            m.Ascendente = ordenacaoAscendente;
                //        }
                //    }

                //    break;

                case COLUNA_NOME:
                    if (trocarOrdenacao)
                        ordenacaoAscendente = !ordenacaoAscendente;
                    colunaOrdenacao = COLUNA_NOME;

                    foreach (Painel p in fachada.CarregarPaineis(controladorSelecionado))
                    {
                        p.Mensagens.Sort(delegate (Mensagem a, Mensagem b)
                        {
                            if (ordenacaoAscendente)
                            {
                                return
                                a.LabelMensagem.CompareTo(b.LabelMensagem);
                            }
                            else
                            {
                                return
                                b.LabelMensagem.CompareTo(a.LabelMensagem);
                            }
                        });

                        foreach (Mensagem m in p.Mensagens)
                        {
                            m.Ordenacao = COLUNA_NOME;
                            m.Ascendente = ordenacaoAscendente;
                        }
                    }

                    break;
                case SEM_ORDENACAO:
                    colunaOrdenacao = SEM_ORDENACAO;
                    break;
            }

            this.Cursor = Cursors.Default;
        }


        #endregion

        private void gridMensagens_AfterSort(object sender, C1.Win.C1FlexGrid.SortColEventArgs e)
        {
            //conversão das colunas
            int coluna = 255;
            switch (e.Col)
            {
                case 3: coluna = SEM_ORDENACAO; break;
                case 4: coluna = COLUNA_NOME; break;
            }

            //Sort na lista
            SortList(coluna, true);
            fachada.AtualizarIndicesMensagens(controladorSelecionado);

            LimparGridMensagens();
            PopulaMensagensGrid(fachada.CarregarMensagens(controladorSelecionado, painelSelecionado));

        }

        private void gridMensagens_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                C1.Win.C1FlexGrid.HitTestInfo ht = gridMensagens.HitTest();
                if (ht.Type == C1.Win.C1FlexGrid.HitTestTypeEnum.Cell)
                {
                    gridMensagens.Row = ht.Row;
                    contextMenuStrip1.Show(gridMensagens, e.Location);
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                C1.Win.C1FlexGrid.HitTestInfo ht = gridMensagens.HitTest();
                if (ht.Type == C1.Win.C1FlexGrid.HitTestTypeEnum.ColumnHeader)
                {
                    if (ht.Column == 0)
                    {
                        collapseOrExpandeAll(expandir);
                    }
                }
            }
        }

        private void gridMensagens_DoubleClick(object sender, EventArgs e)
        {
            int linha = (int)gridMensagens.Selection.r1;

            //se a linha não for válida
            if (linha < 0)
                return;

            //abrir a mensagem
            if (gridMensagens.Rows[linha].Node.Row[1].ToString() == gridMensagens.Rows[linha].Node.Row[2].ToString())
            {
                EditarMensagem();
            }

            //texto de ida - O divisor
            if (gridMensagens.Rows[linha].Node.Row[2].Equals(String.Empty))
                return;

            //editar um texto de ida da mensagem
            if (gridMensagens.Rows[linha].Node.Row[2].ToString().Substring(0, 1) == "T")
            {
                EditarMensagem();
                EditorMensagens.ChamarTelaEdicaoTexto(Convert.ToInt16(gridMensagens.Rows[linha].Node.Row[2].ToString().Substring(1, gridMensagens.Rows[linha].Node.Row[2].ToString().Length - 1)));
            }
        }

        private void gridMensagens_DragDrop(object sender, DragEventArgs e)
        {
            //find the drop position
            C1FlexGrid flex = (C1FlexGrid)sender;
            Point pt = flex.PointToClient(new Point(e.X, e.Y));
            HitTestInfo hti = gridMensagens.HitTest(pt.X, pt.Y);

            //newindex - the index where the row is to be moved to
            //oldindex - the index from where the row was moved from
            int newindex = selecionarIndice(hti.Row); //(hti.Row > 0) ? (int)(gridMensagens.Rows[hti.Row].Node.Row[1]) : hti.Row - 1; 
            int oldindex = selecionarIndice(flex.RowSel); //(int)(gridMensagens.Rows[gridMensagens.Selection.r1].Node.Row[1]); //flex.RowSel - 1; 

            if (newindex <= -1)
                newindex = flex.Rows.Count;
     
            fachada.MoverMensagem(controladorSelecionado, painelSelecionado, oldindex, newindex);
            
            colunaOrdenacao = SEM_ORDENACAO;
            LimparGridMensagens();
            PopulaMensagensGrid(fachada.CarregarMensagens(controladorSelecionado, painelSelecionado));
            SetarColunaOrdenacao((byte)SEM_ORDENACAO);
            // gridMensagens.Rows[newindex + 1].Selected = true;
            fachada.AtualizarIndicesMensagens(controladorSelecionado);
        }

        private int selecionarIndice(int linha)
        {
            int indiceRoteiro = gridMensagens.Rows[linha].Node.Index;

            if (null != gridMensagens.Rows[linha].Node.Parent)
            {
                indiceRoteiro = gridMensagens.Rows[linha].Node.Parent.Index;
            }
            return indiceRoteiro;
        }


        private void gridMensagens_DragOver(object sender, DragEventArgs e)
        {
            // e.Effect = DragDropEffects.Move;
            //check that we have the type of data we want
            if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void gridMensagens_BeforeMouseDown(object sender, BeforeMouseDownEventArgs e)
        {
            if (e.Clicks > 1)
                return;
            //start dragging when the user clicks the cell
            C1FlexGrid flex = (C1FlexGrid)sender;
            HitTestInfo hti = flex.HitTest(e.X, e.Y);

            if (hti.Type == HitTestTypeEnum.Cell)
            {
                //select the row
                int index = hti.Row;
                if (index != 0)
                {
                    flex.Select(index, 0, index, flex.Cols.Count - 1, false);

                    ////save info for target
                    //_src = flex;

                    //do drag drop
                    DragDropEffects dd = flex.DoDragDrop(flex.Clip, DragDropEffects.Move);
                    //if it worked, delete row from source (it's a move)
                    if (dd == DragDropEffects.Move)
                    {
                        //flex.Rows.Remove(index);
                        //colunaOrdenacao = SEM_ORDENACAO;
                        //LimparGridRoteiros();
                        //PopulaRoteirosGrid(fachada.CarregarRoteiros(controladorSelecionado, painelSelecionado));
                        //gridRoteiros.Select()
                    }

                    ////done, reset info
                    //_src = null;
                }


            }
        }
    }
}
