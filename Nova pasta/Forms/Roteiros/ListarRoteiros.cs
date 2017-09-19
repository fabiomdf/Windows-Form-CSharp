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
using System.Diagnostics;
using System.IO;
using C1.Win.C1FlexGrid;

namespace PontosX2.Forms.Roteiros
{
    public partial class ListarRoteiros : UserControl
    {
        #region Propriedades

        public Roteiros EditorRoteiros { get; set; }
       
        public int painelSelecionado;
        public int controladorSelecionado;
        public int roteiroSelecionado;
        Fachada fachada = Fachada.Instance;
        public ResourceManager rm;

        private const int COLUNA_NUMERO = 0;
        private const int COLUNA_NOME = 1;
        private const int SEM_ORDENACAO = 255;
        private int colunaOrdenacao = SEM_ORDENACAO;//COLUNA_NUMERO;
        private bool ordenacaoAscendente = true;
        private bool expandir = true;

        List<Roteiro> listaRoteiros;

        //Propriedades do dragdrop dbgridview
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        #endregion

        #region Construtor

        public ListarRoteiros()
        {
            InitializeComponent();

            listaRoteiros = new List<Roteiro>();

            rm = fachada.carregaIdioma();
            AplicaIdioma();            
        }

        #endregion

        #region Botoes

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            IncluirRoteiro();            
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            CopiarRoteiro();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (gridRoteiros.Selection.r1 > 0)
            {
                if (fachada.QuantidadeRoteiros(controladorSelecionado, painelSelecionado) > 1)
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR"), rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        int qtdAgendamentos = fachada.QuantidadeAgendamentosRoteiro(controladorSelecionado, (int)gridRoteiros.Rows[gridRoteiros.Selection.r1].Node.Row[1]);
                        if (qtdAgendamentos == 0)
                            ExcluirRoteiro();
                        else
                        {
                            string msg;
                            if (qtdAgendamentos == 1)
                                msg = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_AGENDAMENTO1") + " " + qtdAgendamentos + " " + rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_AGENDAMENTO2_S");
                            else
                                msg = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_AGENDAMENTO1") + " " + qtdAgendamentos + " " + rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_AGENDAMENTO2_P");

                            if (DialogResult.Yes == MessageBox.Show(msg, rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                                ExcluirRoteiro();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MBOX_EXCLUIR_ULTIMO"));
                }
            }
        }

        #endregion

        #region ChamarTelaRoteiros

        private void IncluirRoteiro()
        {
            EditorRoteiros.controladorSelecionado = controladorSelecionado;
            EditorRoteiros.painelSelecionado = 0;
            EditorRoteiros.incluirRoteiro = true;            
            EditorRoteiros.colunaOrdenacao = colunaOrdenacao;
            EditorRoteiros.ordenacaoAscendente = ordenacaoAscendente;
            this.Visible = false;
            EditorRoteiros.CriarNovoRoteiro(controladorSelecionado);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, 0);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
            EditorRoteiros.Visible = true;
            EditorRoteiros.HabilitarCamposPainelPrincipal();
        }

        private void EditarRoteiro()
        {
            if (gridRoteiros.Selection.r1 < 0)
                return;
            EditorRoteiros.controladorSelecionado = controladorSelecionado;
            EditorRoteiros.painelSelecionado = painelSelecionado;
            EditorRoteiros.incluirRoteiro = false;
            EditorRoteiros.colunaOrdenacao = colunaOrdenacao;
            EditorRoteiros.ordenacaoAscendente = ordenacaoAscendente;
            this.Visible = false;
            EditorRoteiros.CarregarRoteiroGUI(controladorSelecionado, painelSelecionado, gridRoteiros.Selection.r1);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
            EditorRoteiros.Visible = true;
            EditorRoteiros.HabilitarCamposPainelPrincipal();
        }

        #endregion

        #region AlterarGUI


        private void EditarRoteiroAPP()
        {
            try
            {
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).CursorWait();

                Roteiro roteiroTemp = fachada.CarregarRoteiro(controladorSelecionado, painelSelecionado, (int)gridRoteiros.Rows[gridRoteiros.Selection.r1].Node.Row[1]);
                roteiroSelecionado = roteiroTemp.Indice;

                //Verificando se o app ainda esta instalado na máquina
                fachada.appInstalado = Util.Util.ExisteAPPInstalado(out fachada.caminhoApp);
                if (!fachada.appInstalado)
                {
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).CursorDefault();
                    MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_ROTEIRO_MBOX_APP_DESINSTALADO"));
                    return;
                }

                //Verificando se o app já está em execução
                Process[] app = Process.GetProcessesByName(Util.Util.NOME_APP_APP);
                if (app.Length > 0)
                {
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).CursorDefault();
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_ROTEIRO_MBOX_APP_EM_EXECUCAO"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        foreach (Process p in app)
                            p.Kill();
                    }
                    else
                        return;
                }

                //abrindo ninbus para envio do arquivo nfx
                Process appProcess = new Process();
                appProcess.StartInfo.Arguments = "0 "+ Convert.ToByte(fachada.GetIdiomaFachada()).ToString() + " "+roteiroTemp.Numero.LabelFrase + " " + roteiroTemp.LabelRoteiro.Replace(" ","$");
                appProcess.StartInfo.FileName = fachada.caminhoApp + Util.Util.NOME_ARQUIVO_APP + Util.Util.ARQUIVO_EXT_EXE;
                appProcess.Start();


                ((Arquivo)Parent.Parent.Parent.Parent.Parent).CursorDefault();
            }
            catch
            {
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).CursorDefault();
            }
        }

        private void ListarRoteiros_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                SortList(colunaOrdenacao, false);
                fachada.AtualizarIndicesRoteiros(controladorSelecionado);

                //Recarregar informações no flexgrid
                LimparGridRoteiros();
                PopulaRoteirosGrid(fachada.CarregarRoteiros(controladorSelecionado, painelSelecionado));

                HabilitarAPP();
            }
        }

        private void HabilitarAPP()
        {
            if (fachada.appInstalado)
                btnApp.Visible = true;
            else
                btnApp.Visible = false;
        }

        public void LimparGridRoteiros()
        {
            gridRoteiros.BeginUpdate();

            if (gridRoteiros.Rows.Count > 1)
                gridRoteiros.Rows.RemoveRange(1, gridRoteiros.Rows.Count - 1);

            gridRoteiros.EndUpdate();
        }

        private void SetarBold()
        {

            C1.Win.C1FlexGrid.CellStyle cs = gridRoteiros.Styles.Add("FontBold");
            cs.Font = new Font(gridRoteiros.Font.Name, gridRoteiros.Font.Size, FontStyle.Bold);

            for (int i = 1; i < gridRoteiros.Rows.Count; i++)
            {
                if (gridRoteiros.Rows[i].Node.Row[2].Equals(String.Empty))
                {
                    gridRoteiros.Rows[i].Style = cs;
                }
            }
        }

        public void PopulaRoteirosGrid(List<Roteiro> roteiros)
        {
            listaRoteiros = roteiros;

            gridRoteiros.BeginUpdate();

            for (int row = 0; row < roteiros.Count; row++)
            {

                var node = gridRoteiros.Rows.AddNode(0);

                node.Row[1] = row;
                node.Row[2] = row;
                node.Row[3] = (row).ToString("000");
                node.Row[4] = roteiros[row].Numero.LabelFrase;
                node.Row[5] = roteiros[row].LabelRoteiro;

                //adicionando imagem no node se tiver rota do APP
                if (fachada.appInstalado)
                {
                    if (roteiros[row].EnvioRoteiroAPP == Util.Util.EnvioRoteiroAPP.EnviarRotaAPP)
                        node.Image = (Image)global::PontosX2.Properties.Resources.APP_Icone_16;

                    if (roteiros[row].EnvioRoteiroAPP == Util.Util.EnvioRoteiroAPP.NaoEnviarRotaAPP)
                        node.Image = (Image)global::PontosX2.Properties.Resources.APP_Icone_16_vermelho;
                }


                if (roteiros[row].FrasesIda.Count > 0)
                {
                    node = gridRoteiros.Rows.AddNode(1);
                    node.Row[1] = row;
                    node.Row[2] = String.Empty;
                    node.Row[3] = String.Empty;
                    node.Row[4] = rm.GetString("USER_CONTROL_ROTEIROS_TAB_IDA"); 
                    node.Row[5] = String.Empty;
                    
                    int z = 0;
                    foreach (Frase f in roteiros[row].FrasesIda)
                    {
                        node = gridRoteiros.Rows.AddNode(2);
                        node.Row[1] = row;
                        node.Row[2] = "I"+z;
                        node.Row[3] = String.Empty;
                        node.Row[4] = String.Empty;
                        node.Row[5] = f.LabelFrase;
                        z = z + 1;
                    }
                }

                if (roteiros[row].FrasesVolta.Count > 0)
                {
                    node = gridRoteiros.Rows.AddNode(1);
                    node.Row[1] = row;
                    node.Row[2] = String.Empty;
                    node.Row[3] = String.Empty;
                    node.Row[4] = rm.GetString("USER_CONTROL_ROTEIROS_TAB_VOLTA");
                    node.Row[5] = String.Empty;

                    int z = 0;
                    foreach (Frase f in roteiros[row].FrasesVolta)
                    {
                        node = gridRoteiros.Rows.AddNode(2);
                        node.Row[1] = row;
                        node.Row[2] = "V" + z;
                        node.Row[3] = String.Empty;
                        node.Row[4] = String.Empty;
                        node.Row[5] = f.LabelFrase;
                        z = z + 1;
                    }
                }
            }          

            //Setar as colunas que dividem os textos para Bold
            SetarBold();

            //Setando para não expandir os nós
            collapseOrExpandeAll(false);

            //desmarcando o item selecionado
            gridRoteiros.Select(-1, -1);

            //marcar no flexgrid a coluna que esta ordenado
            SetarColunaOrdenacao();

            gridRoteiros.EndUpdate();

        }

        public void collapseOrExpandeAll(bool expandido)
        {
            if (expandido)
            {
                foreach (C1.Win.C1FlexGrid.Node no in this.gridRoteiros.Nodes)
                {
                    gridRoteiros.SetCellImage(0, 0, (Image)global::PontosX2.Properties.Resources.collapse);
                    no.Expanded = true;
                    if (no.Nodes.Length > 0)
                        no.Nodes[0].Expanded = true;
                    if (no.Nodes.Length > 1)
                        no.Nodes[1].Expanded = true;

                }
            }
            else
            {
                foreach (C1.Win.C1FlexGrid.Node no in this.gridRoteiros.Nodes)
                {                  
                    no.Collapsed = true;
                    gridRoteiros.SetCellImage(0, 0, (Image)global::PontosX2.Properties.Resources.expand);
                }
            }
        }

        private void MenuItemCopy_Click(object sender, EventArgs e)
        {
            CopiarRoteiro();
        }

        private void CopiarRoteiro()
        {
            if (gridRoteiros.Selection.r1 > 0)
            {
                int indiceRoteiro = (int)gridRoteiros.Rows[gridRoteiros.Selection.r1].Node.Row[1];

                //copiando a rota
                fachada.CopiarRoteiro(controladorSelecionado, indiceRoteiro, rm.GetString("ROTEIRO_COPIA"));

                //reordenando na lista e atualizando os indices
                SortList(colunaOrdenacao, false);
                fachada.AtualizarIndicesRoteiros(controladorSelecionado);

                //Recarregar informações no listview
                LimparGridRoteiros();
                PopulaRoteirosGrid(fachada.CarregarRoteiros(controladorSelecionado, painelSelecionado));

                SelecionarRoteiro(indiceRoteiro, false);
            }
        }

        public void ExcluirRoteiro()
        {
            int indiceRoteiro = (int)gridRoteiros.Rows[gridRoteiros.Selection.r1].Node.Row[1];

            fachada.ExcluirRoteiro(controladorSelecionado, indiceRoteiro);

            LimparGridRoteiros();
            PopulaRoteirosGrid(fachada.CarregarRoteiros(controladorSelecionado, painelSelecionado));

            if (indiceRoteiro == fachada.QuantidadeRoteiros(controladorSelecionado, painelSelecionado))
                indiceRoteiro = indiceRoteiro - 1;

            SelecionarRoteiro(indiceRoteiro, false);

        }

        internal void SelecionarRoteiro(int indiceRoteiro, bool expandir)
        {
            for (int i = 1; i <= gridRoteiros.Rows.Count - 1; i++)
            {
                if (gridRoteiros.Rows[i].Node.Row[1].ToString() == gridRoteiros.Rows[i].Node.Row[2].ToString() && indiceRoteiro == (int)gridRoteiros.Rows[i].Node.Row[1])
                {
                    gridRoteiros.Row = i;
                    if (expandir)
                        gridRoteiros.Rows[i].Node.Expanded = true;
                    gridRoteiros.Focus();
                    return;
                }
            }
        }
      
        private void SetarColunaOrdenacao(byte colunaOrdenacaoForcada = 0)
        {
            if (listaRoteiros.Count > 0)
            {
                ordenacaoAscendente = listaRoteiros[0].Ascendente;
                if (colunaOrdenacaoForcada == 0)
                {
                    colunaOrdenacao = listaRoteiros[0].Ordenacao;
                }
                else
                {
                    colunaOrdenacao = colunaOrdenacaoForcada;
                }


                if (colunaOrdenacao == COLUNA_NUMERO)
                {
                    if (!ordenacaoAscendente)
                        gridRoteiros.ShowSortAt(C1.Win.C1FlexGrid.SortFlags.Ascending, 4);
                    else
                        gridRoteiros.ShowSortAt(C1.Win.C1FlexGrid.SortFlags.Descending, 4);
                    gridRoteiros.ShowSortPosition = ShowSortPositionEnum.Auto;

                }

                if (colunaOrdenacao == COLUNA_NOME)
                {
                    if (!ordenacaoAscendente)
                        gridRoteiros.ShowSortAt(C1.Win.C1FlexGrid.SortFlags.Ascending, 5);
                    else
                        gridRoteiros.ShowSortAt(C1.Win.C1FlexGrid.SortFlags.Descending, 5);

                    gridRoteiros.ShowSortPosition = ShowSortPositionEnum.Auto;

                }
                if (colunaOrdenacao == SEM_ORDENACAO)
                {                    
                    gridRoteiros.ShowSortPosition = ShowSortPositionEnum.None;
                }
            }
        }

        #endregion

        #region Globalizacao

        public void AplicaIdioma()
        {           

            btnIncluir.Text = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_BTN_INCLUIR");
            btnExcluir.Text = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_BTN_REMOVER");
            btnCopiar.Text = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MENU_COPIAR");

            this.gridRoteiros.Cols[3].Caption = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_COLUNA_INDICE");
            this.gridRoteiros.Cols[4].Caption = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_COLUNA_NUMERO");
            this.gridRoteiros.Cols[5].Caption = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_COLUNA_LABEL");

            incluirToolStripMenuItem.Text = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MENU_INCLUIR");
            deleteToolStripMenuItem.Text = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MENU_EXCLUIR");
            MenuItemCopy.Text = rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_MENU_COPIAR");
        }

        #endregion

        #region SortList

        private void SortList(int coluna, bool trocarOrdenacao)
        {
            this.Cursor = Cursors.WaitCursor;

            switch (coluna)
            {
                case COLUNA_NUMERO:
                    if (trocarOrdenacao)
                        ordenacaoAscendente = !ordenacaoAscendente;
                    colunaOrdenacao = COLUNA_NUMERO;
                    foreach (Painel p in fachada.CarregarPaineis(controladorSelecionado))
                    {
                        p.Roteiros.Sort(delegate(Roteiro a, Roteiro b)
                        {
                            if (ordenacaoAscendente)
                            {
                                return
                                a.Numero.LabelFrase.CompareTo(b.Numero.LabelFrase);
                            }
                            else
                            {
                                return
                                b.Numero.LabelFrase.CompareTo(a.Numero.LabelFrase);
                            }
                        });

                        foreach (Roteiro m in p.Roteiros)
                        {
                            m.Ordenacao = COLUNA_NUMERO;
                            m.Ascendente = ordenacaoAscendente;
                        }
                    }

                    break;

                case COLUNA_NOME:
                    if (trocarOrdenacao)
                        ordenacaoAscendente = !ordenacaoAscendente;
                    colunaOrdenacao = COLUNA_NOME;

                    foreach (Painel p in fachada.CarregarPaineis(controladorSelecionado))
                    {
                        p.Roteiros.Sort(delegate(Roteiro a, Roteiro b)
                        {
                            if (ordenacaoAscendente)
                            {
                                return
                                a.LabelRoteiro.CompareTo(b.LabelRoteiro);
                            }
                            else
                            {
                                return
                                b.LabelRoteiro.CompareTo(a.LabelRoteiro);
                            }
                        });

                        foreach (Roteiro m in p.Roteiros)
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

        private void gridRoteiros_DoubleClick(object sender, EventArgs e)
        {

            int linha = (int)gridRoteiros.Selection.r1;

            //se a linha não for válida
            if (linha < 0)
                return;

            //abrir o roteiro
            if (gridRoteiros.Rows[linha].Node.Row[1].ToString() == gridRoteiros.Rows[linha].Node.Row[2].ToString())
            {
                EditorRoteiros.controladorSelecionado = controladorSelecionado;
                EditorRoteiros.painelSelecionado = painelSelecionado;
                EditorRoteiros.incluirRoteiro = false;
                EditorRoteiros.colunaOrdenacao = colunaOrdenacao;
                EditorRoteiros.ordenacaoAscendente = ordenacaoAscendente;
                this.Visible = false;
                EditorRoteiros.CarregarRoteiroGUI(controladorSelecionado, painelSelecionado, (int)gridRoteiros.Rows[linha].Node.Row[1]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
                EditorRoteiros.Visible = true;
                EditorRoteiros.HabilitarCamposPainelPrincipal();
            }

            //texto de ida ou volta - O divisor
            if (gridRoteiros.Rows[linha].Node.Row[2].Equals(String.Empty))
                return;

            //editar um texto de ida
            if (gridRoteiros.Rows[linha].Node.Row[2].ToString().Substring(0, 1) == "I")
            {
                EditorRoteiros.controladorSelecionado = controladorSelecionado;
                EditorRoteiros.painelSelecionado = painelSelecionado;
                EditorRoteiros.incluirRoteiro = false;
                EditorRoteiros.colunaOrdenacao = colunaOrdenacao;
                EditorRoteiros.ordenacaoAscendente = ordenacaoAscendente;
                this.Visible = false;
                EditorRoteiros.CarregarRoteiroGUI(controladorSelecionado, painelSelecionado, (int)gridRoteiros.Rows[linha].Node.Row[1]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
                EditorRoteiros.Visible = true;
                EditorRoteiros.HabilitarCamposPainelPrincipal();

                EditorRoteiros.ChamarTelaEdicaoTexto(true, Convert.ToInt16(gridRoteiros.Rows[linha].Node.Row[2].ToString().Substring(1, gridRoteiros.Rows[linha].Node.Row[2].ToString().Length - 1)));
            }

            //editar um texto de volta
            if (gridRoteiros.Rows[linha].Node.Row[2].ToString().Substring(0, 1) == "V")
            {
                EditorRoteiros.controladorSelecionado = controladorSelecionado;
                EditorRoteiros.painelSelecionado = painelSelecionado;
                EditorRoteiros.incluirRoteiro = false;
                EditorRoteiros.colunaOrdenacao = colunaOrdenacao;
                EditorRoteiros.ordenacaoAscendente = ordenacaoAscendente;
                this.Visible = false;
                EditorRoteiros.CarregarRoteiroGUI(controladorSelecionado, painelSelecionado, (int)gridRoteiros.Rows[linha].Node.Row[1]);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
                EditorRoteiros.Visible = true;
                EditorRoteiros.HabilitarCamposPainelPrincipal();
                EditorRoteiros.ChamarTelaEdicaoTexto(false, Convert.ToInt16(gridRoteiros.Rows[linha].Node.Row[2].ToString().Substring(1, gridRoteiros.Rows[linha].Node.Row[2].ToString().Length - 1)));
            }
        }

        private void gridRoteiros_AfterSort(object sender, C1.Win.C1FlexGrid.SortColEventArgs e)
        {
            //conversão das colunas
            int coluna = 255;

            switch (e.Col)
            {
                case 3: coluna = SEM_ORDENACAO; break;
                case 4: coluna = COLUNA_NUMERO; break;
                case 5: coluna = COLUNA_NOME; break;
            }

            //Sort na lista
            SortList(coluna, true);
            fachada.AtualizarIndicesRoteiros(controladorSelecionado);

            LimparGridRoteiros();
            PopulaRoteirosGrid(fachada.CarregarRoteiros(controladorSelecionado, painelSelecionado));
        }

        private void gridRoteiros_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                C1.Win.C1FlexGrid.HitTestInfo ht = gridRoteiros.HitTest();
                if (ht.Type == C1.Win.C1FlexGrid.HitTestTypeEnum.Cell)
                {
                    gridRoteiros.Row = ht.Row;
                    contextMenuStrip1.Show(gridRoteiros, e.Location);
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                C1.Win.C1FlexGrid.HitTestInfo ht = gridRoteiros.HitTest();
                if (ht.Type == C1.Win.C1FlexGrid.HitTestTypeEnum.ColumnHeader)
                {
                    if (ht.Column == 0)
                    {
                        if (expandir)
                            collapseOrExpandeAll(true);
                        else
                            collapseOrExpandeAll(false);

                        expandir = !expandir;
                    }
                }

                if (ht.Type == C1.Win.C1FlexGrid.HitTestTypeEnum.Cell)
                {
                    if (ht.Column == 0 && gridRoteiros.Rows[ht.Row].Node.Row[1].ToString() == gridRoteiros.Rows[ht.Row].Node.Row[2].ToString())
                    {
                        //string valor = gridRoteiros.Rows[ht.Row].Node.Row[6].ToString();
                        Roteiro rTemp = fachada.CarregarRoteiro(controladorSelecionado, painelSelecionado, (int)gridRoteiros.Rows[ht.Row].Node.Row[1]);
                        if (rTemp.EnvioRoteiroAPP == Util.Util.EnvioRoteiroAPP.EnviarRotaAPP)
                        {
                            gridRoteiros.Rows[ht.Row].Node.Image = (Image)global::PontosX2.Properties.Resources.APP_Icone_16_vermelho;
                            rTemp.EnvioRoteiroAPP = Util.Util.EnvioRoteiroAPP.NaoEnviarRotaAPP;
                        }
                        else
                        {
                            if (rTemp.EnvioRoteiroAPP == Util.Util.EnvioRoteiroAPP.NaoEnviarRotaAPP)
                            {
                                gridRoteiros.Rows[ht.Row].Node.Image = (Image)global::PontosX2.Properties.Resources.APP_Icone_16;
                                rTemp.EnvioRoteiroAPP = Util.Util.EnvioRoteiroAPP.EnviarRotaAPP;
                            }
                        }
                    }                    
                }
            }
        }

        public void DeselectGrid()
        {
            //desmarcando o item selecionado
            gridRoteiros.Select(-1, -1);
        }

        private void btnApp_Click(object sender, EventArgs e)
        {
            if (gridRoteiros.Selection.r1 > 0)
                EditarRoteiroAPP();
        }

        private void gridRoteiros_DragDrop(object sender, DragEventArgs e)
        {                       
            //find the drop position
            C1FlexGrid flex = (C1FlexGrid)sender;
            Point pt = flex.PointToClient(new Point(e.X, e.Y));
            HitTestInfo hti = flex.HitTest(pt.X, pt.Y);

            //newindex - the index where the row is to be moved to
            //oldindex - the index from where the row was moved from
            int newindex = selecionarIndice(hti.Row);
            int oldindex = selecionarIndice(flex.RowSel);

            if (newindex <= -1)
                newindex = flex.Rows.Count;


            int quantidadePaineis = fachada.QuantidadePaineis(controladorSelecionado);    

            for (int indicePainelSelecionado = 0; indicePainelSelecionado < quantidadePaineis; indicePainelSelecionado++)
            {                
                fachada.MoverRoteiro(controladorSelecionado, indicePainelSelecionado, oldindex, newindex);
            }
            
            
            LimparGridRoteiros();
            PopulaRoteirosGrid(fachada.CarregarRoteiros(controladorSelecionado, painelSelecionado));
            colunaOrdenacao = SEM_ORDENACAO;
            SetarColunaOrdenacao((byte)colunaOrdenacao);
            fachada.AtualizarIndicesRoteiros(controladorSelecionado);
        }

        private int selecionarIndice(int linha)
        {         
            int indiceRoteiro = gridRoteiros.Rows[linha].Node.Index;

            if (null != gridRoteiros.Rows[linha].Node.Parent)
            {
                indiceRoteiro = gridRoteiros.Rows[linha].Node.Parent.Index;
            }
            return indiceRoteiro;
        }

        private void gridRoteiros_DragOver(object sender, DragEventArgs e)
        {
            // e.Effect = DragDropEffects.Move;
            //check that we have the type of data we want
            if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void gridRoteiros_BeforeMouseDown(object sender, BeforeMouseDownEventArgs e)
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
