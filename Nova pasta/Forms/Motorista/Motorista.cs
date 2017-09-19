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

namespace PontosX2.Forms.Motorista
{
    public partial class Motorista : UserControl
    {
        #region Propriedades

        public int controladorSelecionado;
        public int painelSelecionado;
        public bool isEdicao;
        public int indiceMotorista;
        private const int COLUNA_ID = 0;
        private const int COLUNA_NOME = 1;
        private int colunaOrdenacao = COLUNA_NOME;
        private bool ordenacaoAscendente = true;        
        private const char setaAscendente = '↓';
        private const char setaDescendente = '↑';

        public MotoristaEditor MotoristaEditor { get; set; }

        Fachada fachada = Fachada.Instance;
        public ResourceManager rm;



        public Controlador.Motorista motoristaGUI;
        private int indiceMotParmVariavel;
        private Controlador.Motorista motoristaParamVariavel;
        public List<Controlador.Motorista> listaMotorista;

        #endregion

        #region Construtor

        public Motorista()
        {
            InitializeComponent();

            listaMotorista = new List<Controlador.Motorista>();

            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        #endregion

        #region Globalização

        public void AplicaIdioma()
        {
            gboxMotorista.Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_GBOX_INFO");
            btEditarID.Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_BT_EDITAR_ID");
            btEditarNome.Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_BT_EDITAR_NOME");
            btCancelar.Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_BT_CANCELAR");
            btAplicar.Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_BT_APLICAR");
            btAdd.Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_BT_ADICIONAR");
            btRem.Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_BT_REMOVER");
            AtualizarColunasMotoristas();

        }

        private void AtualizarColunasMotoristas()
        {
            if (colunaOrdenacao == COLUNA_ID)
            {
                if (ordenacaoAscendente)
                    lvMotoristas.Columns[0].Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_LV_MOTORISTAS_COLUNA_0") + " " + setaAscendente;
                else
                    lvMotoristas.Columns[0].Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_LV_MOTORISTAS_COLUNA_0") + " " + setaDescendente;

                lvMotoristas.Columns[1].Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_LV_MOTORISTAS_COLUNA_1");
            }

            if (colunaOrdenacao == COLUNA_NOME)
            {
                if (ordenacaoAscendente)
                    lvMotoristas.Columns[1].Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_LV_MOTORISTAS_COLUNA_1") + " " + setaAscendente;
                else
                    lvMotoristas.Columns[1].Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_LV_MOTORISTAS_COLUNA_1") + " " + setaDescendente;

                lvMotoristas.Columns[0].Text = rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_LV_MOTORISTAS_COLUNA_0");
            }
        }

        #endregion

        #region EntrarNaTela

        private void NovoMotorista(bool enabled)
        {
            isEdicao = false;
            gboxMotorista.Enabled = enabled;

            motoristaGUI = new Controlador.Motorista(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_NOVO_ID"), rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_NOVO_NOME"));
            motoristaGUI.Ordenacao = colunaOrdenacao;
            motoristaGUI.Ascendente = ordenacaoAscendente;

            //setar a fonte padrao para o tamanho do painel
            fachada.SetarFontesDefaultFrases(motoristaGUI.ID, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fachada.SetarFontesDefaultFrases(motoristaGUI.Nome, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

            tboxID.Text = motoristaGUI.ID.LabelFrase;
            tboxNome.Text = motoristaGUI.Nome.LabelFrase;
        }

        public void EditarMotorista(bool enabled)
        {
            isEdicao = true;
            gboxMotorista.Enabled = enabled;

            motoristaGUI = new Controlador.Motorista(fachada.CarregarMotorista(controladorSelecionado, painelSelecionado, indiceMotorista));
            
            tboxID.Text = motoristaGUI.ID.LabelFrase;
            tboxNome.Text = motoristaGUI.Nome.LabelFrase;
        }

        #endregion

        public void HabilitarUserControl()
        {
            //Habilitar lista de motorista e carregar os labels como um cadastro de novo motorista
            HabilitarListaMotoristas(true);
            NovoMotorista(false);

            //carregando lista de motoristas
            listaMotorista = fachada.CarregarMotoristas(controladorSelecionado, painelSelecionado);
            PopularListaMotoristas();

            btAdd.Focus();
        }

        private void CarregarMotoristaParametroVariavel()
        {
            indiceMotParmVariavel = fachada.RetornarParamVariavelMotoristaSelecionado(controladorSelecionado);
            if (indiceMotParmVariavel <= listaMotorista.Count && listaMotorista.Count > 0)
                motoristaParamVariavel = fachada.CarregarMotorista(controladorSelecionado, painelSelecionado, indiceMotParmVariavel);
            else
            { 
                motoristaParamVariavel = null;
                indiceMotParmVariavel = 0;
            }
        }

        private void SetarMotoristaParametroVariavel()
        {
            if (motoristaParamVariavel!=null)
            {
                indiceMotParmVariavel = fachada.IndexMotorista(controladorSelecionado, painelSelecionado, motoristaParamVariavel.ID.LabelFrase);
                fachada.SetarMotoristaSimulacao(controladorSelecionado, indiceMotParmVariavel);
            }
            else
                fachada.SetarMotoristaSimulacao(controladorSelecionado, 0);
            
        }

        #region PopularGUI
        
        public void PopularListaMotoristas()
        {
            lvMotoristas.Items.Clear();

            for (int row = 0; row < listaMotorista.Count; row++)
            {
                ListViewItem item1 = new ListViewItem(listaMotorista[row].ID.LabelFrase);

                //Adiciona um ao indice do roteiro para exibição ao usuário
                item1.SubItems.Add(listaMotorista[row].Nome.LabelFrase);

                lvMotoristas.Items.Add(item1);
            }

            if (listaMotorista.Count>0)
            {
                ordenacaoAscendente = listaMotorista[0].Ascendente;
                colunaOrdenacao = listaMotorista[0].Ordenacao;
            }
            else
            {
                ordenacaoAscendente = true;
                colunaOrdenacao = COLUNA_NOME;
            }

            AtualizarColunasMotoristas();
        }

        #endregion

        private void btRem_Click(object sender, EventArgs e)
        {
            if (lvMotoristas.SelectedItems.Count > 0)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_EXCLUIR"), rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_CONFIRMACAO"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {

                    //carregando motorista que está em parametro variavel
                    CarregarMotoristaParametroVariavel();

                    ExcluirMotorista();

                    //Ajustando o indice setado em parametro variavel do motorista
                    SetarMotoristaParametroVariavel();

                    //NovoMotorista(false);
                    //btRem.Focus();
                }
            }
        }

        #region AlteraçãoGUI

        public void CarregarMotoristaGUI()
        {
            motoristaGUI = new Controlador.Motorista(fachada.CarregarMotorista(controladorSelecionado, painelSelecionado, indiceMotorista));
        }

        private void ExcluirMotorista() 
        {
            int indiceMotorista = lvMotoristas.SelectedItems[0].Index;

            if (indiceMotorista == lvMotoristas.Items.Count - 1)
                indiceMotorista = lvMotoristas.SelectedItems[0].Index - 1;

            fachada.ExcluirMotorista(controladorSelecionado, lvMotoristas.SelectedItems[0].Index);
            listaMotorista = fachada.CarregarMotoristas(controladorSelecionado, painelSelecionado);
            PopularListaMotoristas();

            // se possuir itens na lista
            if (indiceMotorista >= 0)
            { 
                lvMotoristas.Focus();
                lvMotoristas.Items[indiceMotorista].Selected = true;
            }
            else
            { 
                //se excluir o ultimo item na lista
                NovoMotorista(false);
                btRem.Focus();
            }

        }

        public void MudarPainel()
        {
            listaMotorista = fachada.CarregarMotoristas(controladorSelecionado, painelSelecionado);
            PopularListaMotoristas();

            if (isEdicao)
            {
                if (gboxMotorista.Enabled)
                {
                    indiceMotorista = fachada.IndexMotorista(controladorSelecionado, painelSelecionado, motoristaGUI.ID.LabelFrase);
                    lvMotoristas.Items[indiceMotorista].Selected = true;
                    EditarMotorista(true);
                    tboxID.Focus();
                }
                else
                {
                    EditarMotorista(false);
                    lvMotoristas.Items[indiceMotorista].Selected = true;
                }

            }
            else
                NovoMotorista(false);
            
        }

        public void RecarregarTela()
        {
            if (!gboxMotorista.Enabled)
            {
                if (!isEdicao)
                    NovoMotorista(false);                    
            }
        }

        public bool UsuarioEditando()
        {
            return gboxMotorista.Enabled;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            // só permite adicionar motorista se o painel principal não for multinhas
            if (fachada.GetMultiLinhas(controladorSelecionado, 0) == 1)
            {
                if (lvMotoristas.SelectedItems.Count > 0)
                    lvMotoristas.Items[lvMotoristas.SelectedIndices[0]].Selected = false;

                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, 0);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).LimparPaineis();

                HabilitarListaMotoristas(false);

                NovoMotorista(true);
                tboxID.Focus();
            }
            else
                MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_INCLUIR_MULTILINHAS"));
        }

        #endregion

        private void btCancelar_Click(object sender, EventArgs e)
        {
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).LimparPaineis();
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            HabilitarListaMotoristas(true);

            if (isEdicao)
            {
                 EditarMotorista(false);
                 lvMotoristas.Items[indiceMotorista].Selected = true;
                 lvMotoristas.Focus();
            }
            else
                NovoMotorista(false);
        }

        public void AplicarMotorista()
        {
            PreencherObjetoMotorista();

            //Se achar um motorista com o mesmo ID não termina o cadastro
            //if (fachada.AchouMotoristaID(controladorSelecionado, indiceMotorista, isEdicao, motoristaGUI))
            //    return false;

            if (isEdicao)
                fachada.EditarMotorista(controladorSelecionado, painelSelecionado, indiceMotorista, motoristaGUI);
            else
                fachada.AdicionarMotorista(controladorSelecionado, motoristaGUI);

            //carregando motorista que está em parametro variavel
            CarregarMotoristaParametroVariavel();

            //ordenando a lista de motoristas
            SortList(colunaOrdenacao, false);

            //Ajustando o indice setado em parametro variavel do motorista
            SetarMotoristaParametroVariavel();

            //return true;
        }

        public void btAplicar_Click(object sender, EventArgs e)
        {
            if (exibirErro(true))
                return;

            AplicarMotorista();

            //recuperando lista de motoristas da fachada
            listaMotorista = fachada.CarregarMotoristas(controladorSelecionado, painelSelecionado);

            //carregando a lista de motoristas no listview
            PopularListaMotoristas();

            //recuperando o novo indice do motorista inserido após a ordenação
            indiceMotorista = fachada.IndexMotorista(controladorSelecionado, painelSelecionado, motoristaGUI.ID.LabelFrase);

            //carregando a gui com as informações
            EditarMotorista(false);

            ((Arquivo)Parent.Parent.Parent.Parent.Parent).LimparPaineis();
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            HabilitarListaMotoristas(true);
            lvMotoristas.Items[indiceMotorista].Selected = true;
            btAdd.Focus();
        }

        public Boolean exibirErro(bool setarCampo)
        {

            if (tboxID.Text == "")
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_ID_VAZIO"));
                if (setarCampo)
                    tboxID.Focus();
                return true;
            }

            if (tboxNome.Text == "")
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_NOME_VAZIO"));
                if (setarCampo)
                    tboxNome.Focus();
                return true;
            }

            if (fachada.AchouMotoristaID(controladorSelecionado, indiceMotorista, isEdicao, motoristaGUI))
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_MESMO_ID"));
                if (setarCampo)
                    tboxID.Focus();
                return true;
            }

            return false;
        }


        public void AplicarMudancaDePainel()
        {
            //Se achar um motorista com o mesmo ID não termina o cadastro
            if (!fachada.AchouMotoristaID(controladorSelecionado, indiceMotorista, isEdicao, motoristaGUI))
            {
                //fachada.EditarMotorista(controladorSelecionado, painelSelecionado, indiceMotorista, motoristaGUI);
                AplicarMotorista();
            }
        }


        public void PreencherObjetoMotorista()
        {
            motoristaGUI.ID.LabelFrase = tboxID.Text;
            motoristaGUI.ID.Modelo.Textos[0].LabelTexto = tboxID.Text;
            motoristaGUI.Nome.LabelFrase = tboxNome.Text;
            motoristaGUI.Nome.Modelo.Textos[0].LabelTexto = tboxNome.Text;
        }

        private void tboxID_TextChanged(object sender, EventArgs e)
        {
            motoristaGUI.ID.LabelFrase = tboxID.Text;
            motoristaGUI.ID.Modelo.Textos[0].LabelTexto = tboxID.Text;

            //Setando desenho do painel
            if (((TextBox)sender).ContainsFocus)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), motoristaGUI.ID);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(motoristaGUI.ID);
            }
        }

        private void tboxNome_TextChanged(object sender, EventArgs e)
        {
            motoristaGUI.Nome.LabelFrase = tboxNome.Text;
            motoristaGUI.Nome.Modelo.Textos[0].LabelTexto = tboxNome.Text;

            //Setando desenho do painel
            if (((TextBox)sender).ContainsFocus)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), motoristaGUI.Nome);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(motoristaGUI.Nome);
            }
        }

        private void btEditarID_Click(object sender, EventArgs e)
        {
            EditarID();
        }

        private void btEditarNome_Click(object sender, EventArgs e)
        {
            EditarNome();
        }

        public void EditarID()
        {
            this.Visible = false;
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);

            MotoristaEditor.painelSelecionado = painelSelecionado;
            MotoristaEditor.controladorSelecionado = controladorSelecionado;
            MotoristaEditor.Visible = true;
            MotoristaEditor.EditarFrase(motoristaGUI.ID,true,motoristaGUI.Nome.LabelFrase);
        }

        public void EditarNome()
        {
            this.Visible = false;
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);

            MotoristaEditor.painelSelecionado = painelSelecionado;
            MotoristaEditor.controladorSelecionado = controladorSelecionado;
            MotoristaEditor.Visible = true;
            MotoristaEditor.EditarFrase(motoristaGUI.Nome, false, motoristaGUI.ID.LabelFrase);
        }

        public void EditarIDGUI(Frase id)
        {
            motoristaGUI.ID = id;
            tboxID.Text = motoristaGUI.ID.LabelFrase;            
        }

        public void EditarNomeGUI(Frase nome)
        {
            motoristaGUI.Nome = nome;
            tboxNome.Text = motoristaGUI.Nome.LabelFrase;            
        }

        public void SetarCampo(bool isID)
        {
            if (isID)
                tboxID.Focus();
            else
                tboxNome.Focus();
        }

        public void SelecionarItemListView()
        {
            lvMotoristas.Items[indiceMotorista].Selected = true;
        }

        private void lvMotoristas_DoubleClick(object sender, EventArgs e)
        {
            if (lvMotoristas.SelectedItems.Count == 0)
            {
                NovoMotorista(false);
                return;
            }

            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);            

            indiceMotorista = lvMotoristas.SelectedItems[0].Index;

            HabilitarListaMotoristas(false);

            EditarMotorista(true);
            tboxID.Focus();
        }

        private void lvMotoristas_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (((ListView)sender).ContainsFocus)
            { 
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).LimparPaineis();

                if (lvMotoristas.SelectedItems.Count == 0)
                {
                    NovoMotorista(false);
                    return;
                }

                indiceMotorista = lvMotoristas.SelectedItems[0].Index;
                EditarMotorista(false);
            }
        }


        private void tboxID_Enter(object sender, EventArgs e)
        {
            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), motoristaGUI.ID);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(motoristaGUI.ID);
            }
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
        }

        private void tboxNome_Enter(object sender, EventArgs e)
        {
            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), motoristaGUI.Nome);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(motoristaGUI.Nome);
            }
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
    }

        public void HabilitarListaMotoristas(bool enabled)
        {
            lvMotoristas.Enabled = enabled;
            btRem.Enabled = enabled;
            btAdd.Enabled = enabled;
        }

        private void lvMotoristas_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listaMotorista.Count>0)
            {
                //carregando motorista que está em parametro variavel
                CarregarMotoristaParametroVariavel();

                SortList(e.Column, true);
                PopularListaMotoristas();

                //Ajustando o indice setado em parametro variavel do motorista
                SetarMotoristaParametroVariavel();

                //recuperando o novo indice do motorista inserido após a ordenação
                if (isEdicao)
                { 
                    indiceMotorista = fachada.IndexMotorista(controladorSelecionado, painelSelecionado, motoristaGUI.ID.LabelFrase);
                    lvMotoristas.Items[indiceMotorista].Selected = true;
                }
            }
        }

        private void SortList(int coluna, bool trocarOrdenacao)
        {
            this.Cursor = Cursors.WaitCursor;

            switch (coluna)
            {
                case COLUNA_ID:
                    if (trocarOrdenacao)
                        ordenacaoAscendente = !ordenacaoAscendente;
                    colunaOrdenacao = COLUNA_ID;
                    foreach (Painel p in fachada.CarregarPaineis(controladorSelecionado)) 
                    { 
                        p.Motoristas.Sort(delegate(Controlador.Motorista a, Controlador.Motorista b)
                        {
                            if (ordenacaoAscendente)
                            {
                                return
                                a.ID.LabelFrase.CompareTo(b.ID.LabelFrase);
                            }
                            else
                            {
                                return
                                b.ID.LabelFrase.CompareTo(a.ID.LabelFrase);
                            }
                        });

                        foreach (Controlador.Motorista m in p.Motoristas)
                        {
                            m.Ordenacao = COLUNA_ID;
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
                        p.Motoristas.Sort(delegate(Controlador.Motorista a, Controlador.Motorista b)
                        {
                            if (ordenacaoAscendente)
                            {
                                return
                                a.Nome.LabelFrase.CompareTo(b.Nome.LabelFrase);
                            }
                            else
                            {
                                return
                                b.Nome.LabelFrase.CompareTo(a.Nome.LabelFrase);
                            }
                        });

                        foreach (Controlador.Motorista m in p.Motoristas)
                        {
                            m.Ordenacao = COLUNA_NOME;
                            m.Ascendente = ordenacaoAscendente;
                        }
                    }

                    break;
            }

            this.Cursor = Cursors.Default;
        }

    }
}
