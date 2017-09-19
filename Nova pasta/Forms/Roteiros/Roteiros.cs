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
using System.Globalization;

namespace PontosX2.Forms.Roteiros
{
    public partial class Roteiros : UserControl
    {
        #region Propriedades

        public ListarRoteiros ListarRoteiros { get; set; }
        public TextosEditor TextosEditor { get; set; }
        public NumEditor NumEditor { get; set; }

        public Roteiro roteiroGUI;
        public int controladorSelecionado;
        public int painelSelecionado;
        public bool incluirRoteiro;
        string str = "";
        public int colunaOrdenacao;
        public bool ordenacaoAscendente;
       
        public ResourceManager rm;
        
        Fachada fachada = Fachada.Instance;

        //private bool tarifaAlterUsuario = true;

        #endregion

        #region Construtor

        public Roteiros()
        {
            InitializeComponent();

            //Globalizacao
            rm = fachada.carregaIdioma();
            AplicaIdioma();

        }

        #endregion

        #region Botoes

        private void btContinuar_Click(object sender, EventArgs e)
        {
            //Aplicando roteiro
            PreencheObjetoGUI();

            if (!exibirErro(true))
            {
                if (incluirRoteiro)
                    fachada.IncluirRoteiro(controladorSelecionado, roteiroGUI, chbRepetir.Checked);
                else
                    fachada.EditarRoteiro(controladorSelecionado, painelSelecionado, roteiroGUI);

                // Criando o proximo roteiro
                painelSelecionado = 0;
                incluirRoteiro = true;
                CriarNovoRoteiro(controladorSelecionado);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, 0);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
                HabilitarCamposPainelPrincipal();
            }
        }



        private void btnExcluirFraseIda_Click(object sender, EventArgs e)
        {
            if (lvFrasesIda.SelectedItems.Count > 0)
            {
                if ((fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1) || (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) > 1 && lvFrasesIda.Items.Count > 1) )
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_EXCLUIR_TEXTO_IDA"), rm.GetString("USER_CONTROL_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                        ExcluirTextoIda();
                }
            }
        }

        private void btnEditarNumero_Click(object sender, EventArgs e)
        {
            DesmarcarListViews();

            //travar combobox do painel
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);

            EditarNumero();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bool aplicou = false;

            PreencheObjetoGUI();
            if (incluirRoteiro)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_APLICAR"), rm.GetString("USER_CONTROL_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (!exibirErro(true))
                    {
                        AplicarRoteiro();
                        aplicou = true;
                    }
                    else
                        return;
                    
                }
            }
            else //Edição
            {
                if (fachada.CompararObjetosRoteiro(controladorSelecionado, painelSelecionado, roteiroGUI))
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_ALTERAR"), rm.GetString("USER_CONTROL_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        if (!exibirErro(true))
                        {
                            AplicarRoteiro();
                            aplicou = true;
                        }
                        else
                            return;
                    }
            }

            this.Visible = false;
            ListarRoteiros.Visible = true;

            //Setando na lista de mensagens, a mensagem adicionada ou editada
            if (aplicou)
                ListarRoteiros.SelecionarRoteiro(fachada.IndexRoteiro(controladorSelecionado, painelSelecionado, roteiroGUI.ID), false);
            else
                if (!incluirRoteiro)
                    ListarRoteiros.SelecionarRoteiro(fachada.IndexRoteiro(controladorSelecionado, painelSelecionado, roteiroGUI.ID), false);

            //Setando painel vazio
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

            if (incluirRoteiro)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
        }

        private void btnIncluirPainel_Click(object sender, EventArgs e)
        {

            HabilitarGUIFrase(true, true);

            //travar combobox do painel
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);

        }

        private void btnIncluirFraseVolta_Click(object sender, EventArgs e)
        {
            HabilitarGUIFrase(true, false);

            //travar combobox do painel
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
        }

        private void btnExcluirFraseVolta_Click(object sender, EventArgs e)
        {
            if (lvFrasesVolta.SelectedItems.Count > 0)
            {
                if ((fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1) || (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) > 1 && lvFrasesVolta.Items.Count > 1))
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_EXCLUIR_TEXTO_VOLTA"), rm.GetString("USER_CONTROL_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                        ExcluirTextoVolta();
                }
            }
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            if (!exibirErro(true))
            {
                AplicarRoteiro();
            }
        }

        public Boolean exibirErro(bool setarCampo)
        {
            
            if (tbNumeroRoteiro.Text == "")
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_NUMERO_VAZIO"));
                if (setarCampo)
                    tbNumeroRoteiro.Focus();
                return true;
            }

            if (tbNomeRoteiro.Text == "")
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_LABEL_VAZIO"));
                if (setarCampo)
                    tbNomeRoteiro.Focus();
                return true;
            }

            return false;
        }

        #endregion

        #region AlterarGUI

        private void Roteiros_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                //Popular combo apresentação
                PopularApresentacao();

                HabilitarBotaoApresentacao();
                HabilitarEdicaoNumero();
            }

        }

        public void HabilitarEdicaoNumero()
        {
            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                btnEditarNumero.Enabled = true;
            else
                btnEditarNumero.Enabled = false;
        }

        public void HabilitarBotaoApresentacao()
        {
            if (tabTextos.SelectedIndex == 0)
                if (lvFrasesIda.Items.Count > 0)
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);

            if (tabTextos.SelectedIndex == 1)
                if (lvFrasesVolta.Items.Count > 0)
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
        }

        public List<Frase> ExibirTodosTextos()
        {
            if (tabTextos.SelectedIndex == 0)
                return roteiroGUI.FrasesIda;
            else
                return roteiroGUI.FrasesVolta;
        }

        public bool ItemSelecionadoLista()
        {
            bool retorno = false;

            if (tabTextos.SelectedIndex == 0)
            {
                if (lvFrasesIda.SelectedItems.Count > 0)
                    retorno = true;
            }
            else
            {
                if (lvFrasesVolta.SelectedItems.Count > 0)
                    retorno = true;
            }

            return retorno;
        }

        public int IndiceSelecionadoLista()
        {
            int retorno = -1;

            if (tabTextos.SelectedIndex == 0)
            {
                if (lvFrasesIda.SelectedItems.Count > 0)
                    retorno = lvFrasesIda.SelectedItems[0].Index;
            }
            else
            {
                if (lvFrasesVolta.SelectedItems.Count > 0)
                    retorno = lvFrasesVolta.SelectedItems[0].Index;
            }

            return retorno;
        }

        public Frase FraseSelecionadaRoteiroGUI()
        {
            if (tabTextos.SelectedIndex == 0)
            {
                return roteiroGUI.FrasesIda[System.Convert.ToInt16(lvFrasesIda.SelectedItems[0].Text) - 1];
            }
            else
            {
                return roteiroGUI.FrasesVolta[System.Convert.ToInt16(lvFrasesVolta.SelectedItems[0].Text) - 1];
            }
        }

        public void SelecionarItemListView(int indice)
        {
            if (tabTextos.SelectedIndex == 0)
            {
                lvFrasesIda.Items[indice].Selected = true;
            }
            else
            {
                lvFrasesVolta.Items[indice].Selected = true;
            }
        }

        public int IndiceTextoSelecionado(bool fraseIda)
        {
            int indice = 0;

            if (fraseIda)
            {
                indice = lvFrasesIda.SelectedIndices[0];
            }
            else
            {
                indice = lvFrasesVolta.SelectedIndices[0];
            }

            return indice;
        }

        private void tbNomeRoteiro_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ContainsFocus)
            {
                roteiroGUI.LabelRoteiro = tbNomeRoteiro.Text;
            }
        }

        private void lvFrasesIda_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Link);
        }

        private void lvFrasesIda_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void lvFrasesIda_DragDrop(object sender, DragEventArgs e)
        {
            Point cp = lvFrasesIda.PointToClient(new Point(e.X, e.Y));
            ListViewItem dragToItem = lvFrasesIda.GetItemAt(cp.X, cp.Y);

            if (dragToItem != null)
            {
                ListViewItem dragItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                int itemOrigem = dragItem.Index;
                int itemDestino = dragToItem.Index;

                roteiroGUI.MoverFrases(true, itemOrigem, itemDestino);

                PopularListaTextosIda();
                SelecionarItemListView(itemDestino);
            }
        }

        public void FocarNumero()
        {
            tbNumeroRoteiro.Focus();
        }

        private void lvFrasesIda_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (((ListView)sender).ContainsFocus)
            {
                if (lvFrasesIda.SelectedItems.Count > 0)
                {
                    //Setando desenho do painel
                    if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                    {
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), roteiroGUI.FrasesIda[System.Convert.ToInt16(lvFrasesIda.SelectedItems[0].Text) - 1]);
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(roteiroGUI.FrasesIda[System.Convert.ToInt16(lvFrasesIda.SelectedItems[0].Text) - 1]);
                    }
                }
                else
                {
                    //Setando painel vazio
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
                    if (lvFrasesIda.Items.Count > 0)
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
                }
            }
        }

        private void lvFrasesVolta_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (((ListView)sender).ContainsFocus)
            {
                if (lvFrasesVolta.SelectedItems.Count > 0)
                {
                    //Setando desenho do painel
                    if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                    {
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), roteiroGUI.FrasesVolta[System.Convert.ToInt16(lvFrasesVolta.SelectedItems[0].Text) - 1]);
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(roteiroGUI.FrasesVolta[System.Convert.ToInt16(lvFrasesVolta.SelectedItems[0].Text) - 1]);
                    }
                }
                else
                {
                    //Setando painel vazio
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
                    if (lvFrasesVolta.Items.Count > 0)
                        ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
                }
            }
        }


        private void tbNumeroRoteiro_Enter(object sender, EventArgs e)
        {
            
            DesmarcarListViews();

            //Setando desenho do painel se painel for simples
            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), roteiroGUI.Numero);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(roteiroGUI.Numero);
            }
        }


        private void tabTextos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Setando painel vazio
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
            
            if (tabTextos.SelectedIndex == 0)
                if (lvFrasesIda.Items.Count>0)
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);

            if (tabTextos.SelectedIndex == 1)
                if (lvFrasesVolta.Items.Count > 0)
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);

            DesmarcarListViews();
        }

        public void DesmarcarListViews()
        {
            if (lvFrasesIda.SelectedItems.Count > 0)
                lvFrasesIda.Items[lvFrasesIda.SelectedIndices[0]].Selected = false;
            if (lvFrasesVolta.SelectedItems.Count > 0)
                lvFrasesVolta.Items[lvFrasesVolta.SelectedIndices[0]].Selected = false;

        }

        private void tbTarifa_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private string MascaraTarifa(string texto)
        {
            string valor = texto.Replace(".", string.Empty);

            if (texto == Convert.ToString(0))
                str = "";
            else
                str = texto;

            if (valor.Length == 1)
                valor = "0.0" + valor;
            else
            {
                if (valor.Length == 2)
                    valor = "0." + valor;
                else
                    if (valor.Length >= 3)
                        valor = valor.Substring(0, valor.Length - 2) + "." + valor.Substring(valor.Length - 2, 2);

            }
            return valor;
        }


        private bool IsNumeric(int Val)
        {
            return ((Val >= 48 && Val <= 57) || (Val == 8) || (Val == 46));
        }

        private void tbTarifa_KeyDown(object sender, KeyEventArgs e)
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

            //Definindo o tamaho máximo de 9 caracteres
            if (str.Length == 8 && (!(KeyCode == 8 || KeyCode == 46)))
            {
                e.Handled = true;
                return;
            }

            //Se o texto for tamanho 0 também não deve permitir digitar 0
            if (str.Length == 0 && KeyCode == 48)
            {
                e.Handled = true;
                return;
            }

            if (((KeyCode == 8) || (KeyCode == 46)) && (str.Length > 0))
            {
                str = str.Substring(0, str.Length - 1);
            }
            else if (!((KeyCode == 8) || (KeyCode == 46)))
            {
                str = str + Convert.ToChar(KeyCode);
            }

            if (str.Length == 0)
            {
                tbTarifa.Text = "0.00";
            }

            if (str.Length == 1)
            {
                tbTarifa.Text = "0.0" + str;
            }
            else
            {
                if (str.Length == 2)
                {
                    tbTarifa.Text = "0." + str;
                }
                else
                {
                    if (str.Length > 2)
                    {
                        tbTarifa.Text = str.Substring(0, str.Length - 2) + "." +
                                        str.Substring(str.Length - 2);
                    }
                }
            }

            tbTarifa.SelectionStart = tbTarifa.Text.Length + 1;
        }

        private void tbTarifa_Click(object sender, EventArgs e)
        {
            tbTarifa.SelectionStart = tbTarifa.Text.Length + 1;
        }

        private void RemoverTextoVoltarGUI()
        {
            if (tabTextos.TabPages.Count > 1)
                tabTextos.TabPages.RemoveAt(1);
            roteiroGUI.FrasesVolta.Clear();
            lvFrasesVolta.Items.Clear();
        }

        private void AdicionarTextoMultiLinhasGUI(bool ida)
        {
            Frase f = new Frase();
            f.TipoVideo = Util.Util.TipoVideo.V04;
            f.TextoAutomatico = false;
            f.LabelFrase = rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS");
            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;

            for (int j = 0; j < fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado); j++)
            {
                Texto t = new Texto(rm.GetString("ARQUIVO_PAINEL") + " " + (j + 1).ToString("00"));
                t.AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) / fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado);
                t.LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);
                f.Modelo.Textos.Add(t);
            }

            fachada.SetarFontesDefaultFrases(f, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) / fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado));

            if (ida)
                IncluirFraseIdaRoteiroGUI(f);
            else
                IncluirFraseVoltaRoteiroGUI(f);
        }

        private void cbApresentacaoPadrao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbApresentacaoPadrao.SelectedIndex != -1)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_APRESENTACAO"), rm.GetString("USER_CONTROL_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    SetApresentacaoPadraoTextos();
                }
                cbApresentacaoPadrao.SelectedIndex = -1;
            }

        }

        private void SetApresentacaoPadraoTextos()
        {
            for (int i = 0; i < roteiroGUI.FrasesIda.Count; i++)
            {
                for (int j = 0; j < roteiroGUI.FrasesIda[i].Modelo.Textos.Count; j++)
                {
                    roteiroGUI.FrasesIda[i].Modelo.Textos[j].Apresentacao = (Util.Util.Rolagem)cbApresentacaoPadrao.SelectedIndex;
                }
            }


            for (int i = 0; i < roteiroGUI.FrasesVolta.Count; i++)
            {
                for (int j = 0; j < roteiroGUI.FrasesVolta[i].Modelo.Textos.Count; j++)
                {
                    roteiroGUI.FrasesVolta[i].Modelo.Textos[j].Apresentacao = (Util.Util.Rolagem)cbApresentacaoPadrao.SelectedIndex;
                }

            }

        }

        private void tbNumeroRoteiro_TextChanged(object sender, EventArgs e)
        {
            roteiroGUI.Numero.LabelFrase = tbNumeroRoteiro.Text;
            roteiroGUI.Numero.Modelo.Textos[0].LabelTexto = tbNumeroRoteiro.Text;

            //Setando desenho do painel
            if (((TextBox)sender).ContainsFocus)
            {
                if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                {
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), roteiroGUI.Numero);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(roteiroGUI.Numero);
                }
            }
        }

        public void HabilitarCamposPainelPrincipal()
        {

            cbApresentacaoPadrao.SelectedIndex = -1;
            if (painelSelecionado == 0 || incluirRoteiro)
            {
                if (painelSelecionado == 0 && incluirRoteiro && fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado)==1)
                {
                    chbRepetir.Checked = true;
                    chbRepetir.Visible = true;
                }
                else
                {
                    chbRepetir.Checked = false;
                    chbRepetir.Visible = false;
                }
            }
            else
            {
                chbRepetir.Checked = false;
                chbRepetir.Visible = false;
            }

            tbNumeroRoteiro.Focus();

        }

        private void PopularListaTextosIda()
        {
            //Popular Lista de Textos de Ida
            lvFrasesIda.Items.Clear();
            for (int row = 0; row < roteiroGUI.FrasesIda.Count; row++)
            {
                ListViewItem item1 = new ListViewItem((row + 1).ToString());

                //Adiciona um ao indice do roteiro para exibição ao usuário
                item1.SubItems.Add(roteiroGUI.FrasesIda[row].LabelFrase);

                lvFrasesIda.Items.Add(item1);

            }
        }

        private void PopularListaTextosVolta()
        {
            //Popular Lista de Textos de Volta
            lvFrasesVolta.Items.Clear();
            for (int row = 0; row < roteiroGUI.FrasesVolta.Count; row++)
            {
                ListViewItem item1 = new ListViewItem((row + 1).ToString());

                //Adiciona um ao indice do roteiro para exibição ao usuário
                item1.SubItems.Add(roteiroGUI.FrasesVolta[row].LabelFrase);

                lvFrasesVolta.Items.Add(item1);

            }
        }

        public void PopularApresentacao()
        {
            cbApresentacaoPadrao.Items.Clear();

            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                cbApresentacaoPadrao.Items.AddRange(fachada.CarregarApresentacao().ToArray());
            else
                cbApresentacaoPadrao.Items.AddRange(fachada.CarregarApresentacaoMultiLinhas().ToArray());

        }


        public void PreencheObjetoGUI()
        {
            roteiroGUI.LabelRoteiro = tbNomeRoteiro.Text;
            roteiroGUI.Tarifa = Convert.ToInt32(tbTarifa.Text.Replace(".", string.Empty));
            roteiroGUI.IdaIgualVolta = chbIdaVolta.Checked;
            roteiroGUI.Numero.LabelFrase = tbNumeroRoteiro.Text;
            roteiroGUI.Numero.Modelo.Textos[0].LabelTexto = tbNumeroRoteiro.Text;
        }

        private void ExcluirTextoIda()
        {
            int indiceTextoIda = lvFrasesIda.SelectedItems[0].Index;

            if (indiceTextoIda == lvFrasesIda.Items.Count - 1)
                indiceTextoIda = lvFrasesIda.SelectedItems[0].Index - 1;

            roteiroGUI.FrasesIda.RemoveAt(System.Convert.ToInt16(lvFrasesIda.SelectedItems[0].Text) - 1);
            PopularListaTextosIda();

            if (lvFrasesIda.Items.Count>0)
            { 
                lvFrasesIda.Focus();
                lvFrasesIda.Items[indiceTextoIda].Selected = true;
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
            }
            else
            { 
                //Setando painel vazio
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
            }
        }

        private void ExcluirTextoVolta()
        {
            int indiceTextoVolta = lvFrasesVolta.SelectedItems[0].Index;

            if (indiceTextoVolta == lvFrasesVolta.Items.Count - 1)
                indiceTextoVolta = lvFrasesVolta.SelectedItems[0].Index - 1;

            roteiroGUI.FrasesVolta.RemoveAt(System.Convert.ToInt16(lvFrasesVolta.SelectedItems[0].Text) - 1);
            PopularListaTextosVolta();

            if (lvFrasesVolta.Items.Count > 0)
            {
                lvFrasesVolta.Focus();
                lvFrasesVolta.Items[indiceTextoVolta].Selected = true;
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).HabilitarBtnApresentacao(true);
            }
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
        }

        private void lvFrasesIda_DoubleClick(object sender, EventArgs e)
        {
            HabilitarGUIFrase(false, true);

            //travar combobox do painel
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
        }

        public void ChamarTelaEdicaoTexto(bool isIda, int indiceFrase)
        {
            if (isIda)
            {
                tabTextos.SelectedIndex = 0;

                SelecionarItemListView(indiceFrase);     

                HabilitarGUIFrase(false, true);

                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);

            }
            else
            {
                tabTextos.SelectedIndex = 1;

                SelecionarItemListView(indiceFrase);

                HabilitarGUIFrase(false, false);

                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
            }


        }

        private void HabilitarGUIFrase(bool incluir, bool fraseIda)
        {
            if (incluir)
                IncluirNovaFrase(fraseIda);
            else
                EditarFrase(fraseIda);
        }

        private void lvFrasesVolta_DoubleClick(object sender, EventArgs e)
        {
            HabilitarGUIFrase(false, false);

            ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
        }

        public Boolean AplicarRoteiro()
        {
            PreencheObjetoGUI();

            if (incluirRoteiro)
            {
                fachada.IncluirRoteiro(controladorSelecionado, roteiroGUI, chbRepetir.Checked);
                incluirRoteiro = false;
                HabilitarCamposPainelPrincipal();
                CarregarRoteiroGUI(controladorSelecionado, painelSelecionado, roteiroGUI.Indice);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);
            }
            else
            {
                fachada.EditarRoteiro(controladorSelecionado, painelSelecionado, roteiroGUI);
                HabilitarCamposPainelPrincipal();
            }

            return true;
        }

        public void AplicarRoteiroMudancaPainel()
        {
            if (incluirRoteiro)
                fachada.IncluirRoteiro(controladorSelecionado, roteiroGUI, chbRepetir.Checked);
            else
                fachada.EditarRoteiro(controladorSelecionado, painelSelecionado, roteiroGUI);
        }

        private void chbIdaVolta_Click(object sender, EventArgs e)
        {
            if (chbIdaVolta.Checked)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_EXCLUIR_TEXTOS_VOLTA"), rm.GetString("USER_CONTROL_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    RemoverTextoVoltarGUI();
                }
                else
                {
                    chbIdaVolta.Checked = false;
                }
            }
            else
            {
                tabTextos.TabPages.Add(tabVolta);

                //Criar Texto de volta obrigatório se for painel multilinhas
                if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) > 1)
                    AdicionarTextoMultiLinhasGUI(false);
            }
        }

        private void lvFrasesVolta_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Link);
        }

        private void lvFrasesVolta_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void lvFrasesVolta_DragDrop(object sender, DragEventArgs e)
        {
            Point cp = lvFrasesVolta.PointToClient(new Point(e.X, e.Y));
            ListViewItem dragToItem = lvFrasesVolta.GetItemAt(cp.X, cp.Y);

            if (dragToItem != null)
            {
                ListViewItem dragItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                int itemOrigem = dragItem.Index;
                int itemDestino = dragToItem.Index;

                roteiroGUI.MoverFrases(false, itemOrigem, itemDestino);

                PopularListaTextosVolta();
                SelecionarItemListView(itemDestino);
            }
        }

        #endregion

        #region ChamarTelaTextoEditor

        public void IncluirNovaFrase(bool fraseIda)
        {
            this.Visible = false;

            TextosEditor.isEdicao = false;
            TextosEditor.isFraseIda = fraseIda;
            TextosEditor.painelSelecionado = painelSelecionado;
            TextosEditor.controladorSelecionado = controladorSelecionado;
            TextosEditor.roteiroSelecionado = roteiroGUI.Indice;
            TextosEditor.roteiroNumero = roteiroGUI.Numero.LabelFrase;
            TextosEditor.Visible = true;
            TextosEditor.CriarNovaFrase();

            

        }

        public void EditarFrase(bool fraseIda)
        {
            if ((fraseIda) && (lvFrasesIda.SelectedItems.Count == 0))
                return;
            if ((!fraseIda) &&(lvFrasesVolta.SelectedItems.Count == 0))
                return;
            this.Visible = false;
            TextosEditor.isEdicao = true;
            TextosEditor.isFraseIda = fraseIda;
            TextosEditor.painelSelecionado = painelSelecionado;
            TextosEditor.controladorSelecionado = controladorSelecionado;
            TextosEditor.roteiroSelecionado = roteiroGUI.Indice;
            TextosEditor.roteiroNumero = roteiroGUI.Numero.LabelFrase;
            TextosEditor.Visible = true;

            if (fraseIda)
            {
                TextosEditor.fraseSelecionada = System.Convert.ToInt16(lvFrasesIda.SelectedItems[0].Text) - 1;
                TextosEditor.EditarFrase(roteiroGUI.FrasesIda[System.Convert.ToInt16(lvFrasesIda.SelectedItems[0].Text) - 1]);

            }
            else
            {
                TextosEditor.fraseSelecionada = System.Convert.ToInt16(lvFrasesVolta.SelectedItems[0].Text) - 1;
                TextosEditor.EditarFrase(roteiroGUI.FrasesVolta[System.Convert.ToInt16(lvFrasesVolta.SelectedItems[0].Text) - 1]);
            }
            
        }

        #endregion

        #region ChamarTelaNumEditor

        public void EditarNumero()
        {
            this.Visible = false;
            NumEditor.painelSelecionado = painelSelecionado;
            NumEditor.controladorSelecionado = controladorSelecionado;
            NumEditor.roteiroSelecionado = roteiroGUI.Indice;
            NumEditor.Visible = true;
            NumEditor.EditarFrase(roteiroGUI.Numero);           

        }

        #endregion

        #region EntrarNaTela

        public void CarregarRoteiroGUI(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado)
        {

            roteiroGUI = new Roteiro(fachada.CarregarRoteiro(controladorSelecionado, painelSelecionado, roteiroSelecionado), true);


            //tbIndice.Text = (roteiroGUI.ID).ToString("000");
            tbIndice.Text = (roteiroGUI.Indice).ToString("000");
            tbNumeroRoteiro.Text = roteiroGUI.Numero.LabelFrase;
            tbNomeRoteiro.Text = roteiroGUI.LabelRoteiro;
            chbIdaVolta.Checked = roteiroGUI.IdaIgualVolta;
            tbTarifa.Text = MascaraTarifa(roteiroGUI.Tarifa.ToString());
            
            PopularListaTextosIda();
            if (roteiroGUI.IdaIgualVolta)
                RemoverTextoVoltarGUI();
            else
            {
                if (tabTextos.TabPages.Count == 1)
                    tabTextos.TabPages.Add(tabVolta);
                PopularListaTextosVolta();
            }

            tabTextos.SelectedIndex = 0;
        }

        public void CriarNovoRoteiro(int controladorSelecionado)
        {
            roteiroGUI = new Roteiro();

            roteiroGUI.Indice = fachada.QuantidadeRoteiros(controladorSelecionado, 0);
            roteiroGUI.ID = fachada.QuantidadeRoteiros(controladorSelecionado, 0) + 1;

            //roteiroGUI.LabelRoteiro = rm.GetString("ROTEIRO_LABEL") + " " + (roteiroGUI.ID);
            //roteiroGUI.Numero.LabelFrase = (roteiroGUI.ID).ToString("00");
            //roteiroGUI.Numero.Modelo.Textos[0].LabelTexto = roteiroGUI.Numero.LabelFrase = (roteiroGUI.ID).ToString("00");
            roteiroGUI.LabelRoteiro = rm.GetString("ROTEIRO_LABEL") + " " + (roteiroGUI.Indice);
            roteiroGUI.Numero.LabelFrase = (roteiroGUI.Indice).ToString("00");
            roteiroGUI.Numero.Modelo.Textos[0].LabelTexto = roteiroGUI.Numero.LabelFrase = (roteiroGUI.Indice).ToString("00");
            roteiroGUI.Numero.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;            
            roteiroGUI.Ordenacao = colunaOrdenacao;
            roteiroGUI.Ascendente = ordenacaoAscendente;

            //setar a fonte padrao do painel no numero
            fachada.SetarFontesDefaultFrases(roteiroGUI.Numero, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

            //tbIndice.Text = (roteiroGUI.ID).ToString("000");
            tbIndice.Text = (roteiroGUI.Indice).ToString("000");
            tbNumeroRoteiro.Text = roteiroGUI.Numero.LabelFrase;
            tbNomeRoteiro.Text = roteiroGUI.LabelRoteiro;
            chbIdaVolta.Checked = roteiroGUI.IdaIgualVolta;
            tbTarifa.Text = MascaraTarifa(roteiroGUI.Tarifa.ToString());
            

            RemoverTextoVoltarGUI();

            //LimparListVIew Ida e Volta
            //PopularListaTextosVolta();
            //PopularListaTextosIda();

            //se for painel multilinhas adicionar a frase obrigatória
            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) > 1)
                AdicionarTextoMultiLinhasGUI(true);
            else
                PopularListaTextosIda();


        }

        #endregion

        #region ReceberFrasesDaTelaTextoEditorENumero

        public void IncluirFraseIdaRoteiroGUI(Frase frase)
        {
            roteiroGUI.FrasesIda.Add(new Frase(frase));
            PopularListaTextosIda();
        }

        public void IncluirFraseVoltaRoteiroGUI(Frase frase)
        {
            roteiroGUI.FrasesVolta.Add(new Frase(frase));
            PopularListaTextosVolta();
        }

        public void EditarFraseIdaRoteiroGUI(Frase frase)
        {
            roteiroGUI.FrasesIda[TextosEditor.fraseSelecionada] = frase;
            PopularListaTextosIda();
        }

        public void EditarFraseVoltaRoteiroGUI(Frase frase)
        {
            roteiroGUI.FrasesVolta[TextosEditor.fraseSelecionada] = frase;
            PopularListaTextosVolta();
        }

        public void EditarNumeroGUI(Frase numero)
        {
            roteiroGUI.Numero = numero;
            tbNumeroRoteiro.Text = roteiroGUI.Numero.LabelFrase;
        }


        #endregion

        #region Globalizacao

        public void AplicaIdioma()
        {

            this.groupBoxInfo.Text = rm.GetString("USER_CONTROL_ROTEIROS_GROUP_BOX_INFO");
            this.labelIndice.Text = rm.GetString("USER_CONTROL_ROTEIROS_LABEL_INDICE_ROTEIRO");
            this.btnEditarNumero.Text = rm.GetString("USER_CONTROL_ROTEIROS_BTN_EDITAR_NUMERO");
            this.labelNumeroRoteiro.Text = rm.GetString("USER_CONTROL_ROTEIROS_LABEL_NUMERO_ROTEIRO");
            this.labelNomeRoteiro.Text = rm.GetString("USER_CONTROL_ROTEIROS_LABEL_ROTEIRO");
            this.labelTarifa.Text = rm.GetString("USER_CONTROL_ROTEIROS_LABEL_TARIFA");

            this.labelApresentacaoPadrao.Text = rm.GetString("USER_CONTROL_ROTEIROS_LABEL_APRESENTACAO");
            this.chbRepetir.Text = rm.GetString("USER_CONTROL_ROTEIROS_CHECK_BOX_REPETIR");
            this.chbIdaVolta.Text = rm.GetString("USER_CONTROL_ROTEIROS_CHECK_BOX_IDA_VOLTA");

            this.tabIda.Text = rm.GetString("USER_CONTROL_ROTEIROS_TAB_IDA");
            this.tabVolta.Text = rm.GetString("USER_CONTROL_ROTEIROS_TAB_VOLTA");


            this.lvFrasesIda.Columns[0].Text = rm.GetString("USER_CONTROL_ROTEIROS_DATA_GRID_INDICE");
            this.lvFrasesIda.Columns[1].Text = rm.GetString("USER_CONTROL_ROTEIROS_DATA_GRID_TEXTO");

            this.lvFrasesVolta.Columns[0].Text = rm.GetString("USER_CONTROL_ROTEIROS_DATA_GRID_INDICE");
            this.lvFrasesVolta.Columns[1].Text = rm.GetString("USER_CONTROL_ROTEIROS_DATA_GRID_TEXTO");

            this.btnIncluirFraseIda.Text = rm.GetString("USER_CONTROL_ROTEIROS_BTN_INCLUIR");
            this.btnExcluirFraseIda.Text = rm.GetString("USER_CONTROL_ROTEIROS_BTN_EXCLUIR");

            this.btnIncluirFraseVolta.Text = rm.GetString("USER_CONTROL_ROTEIROS_BTN_INCLUIR");
            this.btnExcluirFraseVolta.Text = rm.GetString("USER_CONTROL_ROTEIROS_BTN_EXCLUIR");

            this.btnCancelar.Text = rm.GetString("USER_CONTROL_ROTEIROS_BTN_SAIR");
            this.btnAplicar.Text = rm.GetString("USER_CONTROL_ROTEIROS_BTN_APLICAR");
            this.btnContinuar.Text = rm.GetString("USER_CONTROL_ROTEIROS_BTN_CONTINUAR");

        }


        #endregion


    }
}
