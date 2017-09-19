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
using System.Drawing.Text;
using System.Resources;

namespace PontosX2.Forms.Roteiros
{
    public partial class NumEditor : UserControl
    {

        #region Propriedades

        public Roteiros Roteiros { get; set; }

        public Modelo.Texto modeloTexto { get; set; }

        Fachada fachada = Fachada.Instance;

        public ResourceManager rm;

        public Frase numeroGUI;

        public int controladorSelecionado;
        public int painelSelecionado;
        public int roteiroSelecionado;

        #endregion

        #region Construtor

        public NumEditor()
        {
            InitializeComponent();


            modeloTexto = new Modelo.Texto();
            this.panelModelos.Controls.Add(modeloTexto);
            this.modeloTexto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTexto.Location = new System.Drawing.Point(0, 0);
            this.modeloTexto.Size = new System.Drawing.Size(500, 200);
            this.modeloTexto.TabIndex = 0;

            rm = fachada.carregaIdioma();
            AplicaIdioma();

        }

        #endregion

        #region Globalizacao

        public void AplicaIdioma()
        {

            this.groupBoxInfo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_GROUP_BOX_INFO");
            this.labelIndice.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_LABEL_INDICE_ROTEIRO");
            this.labelNumero.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_LABEL_NUMERO_ROTEIRO");
            this.labelRolagem.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_ROLAGEM"); ;
            this.labelApresentacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_APRESENTACAO"); ;
            this.labelFonte.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_FONTE"); ;
            this.labelTamanho.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_TAMANHO");

            this.btAlinharAbaixo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_ALIGN_BOTTOM");
            this.btAlinharAcima.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_ALIGN_UP");
            this.btAlinharCentro.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_ALIGN_CENTER");
            this.btAlinharDireita.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_ALIGN_RIGHT");
            this.btAlinharEsquerda.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_ALIGN_LEFT");
            this.btAlinharMeio.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_ALIGN_MIDDLE");
            this.btMudarFonte.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_MUDAR_FONTE");
            this.btNegrito.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_NEGRITO");
            this.btItalico.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_ITALICO");
            this.btSublinhado.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_SUBLINHADO");
            this.btNegrito.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_NEGRITO_TEXT");
            this.btItalico.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_ITALICO_TEXT");
            this.btSublinhado.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_SUBLINHADO_TEXT");

            this.btCancelar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_BTN_CANCELAR");
            this.btSalvar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_BTN_SALVAR");
            this.lbNivelSuavizacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_NIVEL_SUAVIZACAO");

        }

        #endregion

        #region Botoes

        public void HabilitarAlinhamentoHorizontal(bool habilitar)
        {
            ChecarBotoes();

            btAlinharEsquerda.Enabled = habilitar;
            btAlinharDireita.Enabled = habilitar;
            btAlinharMeio.Enabled = habilitar;
        }

        public void EnableButtonAlinhamentoEsquerda(bool enabled)
        {
            btAlinharEsquerda.Enabled = enabled;
        }

        public void EnableButtonAlinhamentoDireita(bool enabled)
        {
            btAlinharDireita.Enabled = enabled;
        }


        private void btNegrito_Click(object sender, EventArgs e)
        {
            if (numeroGUI.Modelo.Textos[0].Negrito)
                numeroGUI.Modelo.Textos[0].Negrito = false;
            else
                numeroGUI.Modelo.Textos[0].Negrito = true;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void btItalico_Click(object sender, EventArgs e)
        {
            if (numeroGUI.Modelo.Textos[0].Italico)
                numeroGUI.Modelo.Textos[0].Italico = false;
            else
                numeroGUI.Modelo.Textos[0].Italico = true;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void btSublinhado_Click(object sender, EventArgs e)
        {
            if (numeroGUI.Modelo.Textos[0].Sublinhado)
                numeroGUI.Modelo.Textos[0].Sublinhado = false;
            else
                numeroGUI.Modelo.Textos[0].Sublinhado = true;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void btMudarFonte_Click(object sender, EventArgs e)
        {

            //Carregar Fontes
            CarregarFontes();

            //Habilitar botoes de acordo com a fonte(Pontos ou Windows)
            HabilitarBotoesFontes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);

        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            if (!exibirErro())
            {
                //habilitar combobox do painel se for inclusão de roteiro, se for edição habilitar o combo de paineis
                if (this.Roteiros.incluirRoteiro)
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
                else
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

                this.Roteiros.EditarNumeroGUI(numeroGUI);

                //Popular Bitmap do Painel com valor padrao
                //((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fachada.PreparaBitMapPainel(controladorSelecionado, painelSelecionado));
                //((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

                this.Visible = false;
                this.Roteiros.Visible = true;

                this.Roteiros.FocarNumero();
            }
        }

        public Boolean exibirErro()
        {
            if (numeroGUI.LabelFrase == "")
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_NUMERO_VAZIO"));
                modeloTexto.SetarFocus();
                return true;
            }

            return false;
        }

        public void ChecarBotoes()
        {
            switch (numeroGUI.Modelo.Textos[0].AlinhamentoH)
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

            switch (numeroGUI.Modelo.Textos[0].AlinhamentoV)
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

            if (numeroGUI.Modelo.Textos[0].FonteWindows)
            {
                btMudarFonte.Checked = true;
                if (numeroGUI.Modelo.Textos[0].Italico)
                    btItalico.Checked = true;
                else
                    btItalico.Checked = false;

                if (numeroGUI.Modelo.Textos[0].Negrito)
                    btNegrito.Checked = true;
                else
                    btNegrito.Checked = false;

                if (numeroGUI.Modelo.Textos[0].Sublinhado)
                    btSublinhado.Checked = true;
                else
                    btSublinhado.Checked = false;
            }

        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            //habilitar combobox do painel se for inclusão de roteiro, se for edição habilitar o combo de paineis
            if (this.Roteiros.incluirRoteiro)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            this.Visible = false;
            this.Roteiros.Visible = true;

            this.Roteiros.FocarNumero();
            //Popular Bitmap do Painel com valor padrao
            // ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fachada.PreparaBitMapPainel(controladorSelecionado,painelSelecionado));
            //((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();
        }

        private void btAlinharEsquerda_Click(object sender, EventArgs e)
        {
            numeroGUI.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void btAlinharMeio_Click(object sender, EventArgs e)
        {
            numeroGUI.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void btAlinharDireita_Click(object sender, EventArgs e)
        {
            numeroGUI.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Direita;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void btAlinharAbaixo_Click(object sender, EventArgs e)
        {
            numeroGUI.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void btAlinharCentro_Click(object sender, EventArgs e)
        {
            numeroGUI.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Centro;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void btAlinharAcima_Click(object sender, EventArgs e)
        {
            numeroGUI.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Cima;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        }

        private void HabilitarBotoesFontes()
        {
            if (numeroGUI.Modelo.Textos[0].FonteWindows)
            {
                cbTamanhoFonte.Enabled = true;
                btNegrito.Enabled = true;
                btItalico.Enabled = true;
                btSublinhado.Enabled = true;
                btMudarFonte.Checked = true;
                cbNivelSuavizacao.Enabled = true;
                cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(numeroGUI.Modelo.Textos[0].BinaryThreshold);
            }
            else
            {
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
                cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(numeroGUI.Modelo.Textos[0].BinaryThreshold);
                
            }

        }

        #endregion

        #region AlterarGUI

        private void cbNivelSuavizacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                numeroGUI.Modelo.Textos[0].BinaryThreshold = System.Convert.ToInt16(cbNivelSuavizacao.SelectedItem.ToString());
                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
            }
        }

        private void SetarTipoVideo()
        {
            numeroGUI.TipoVideo = Util.Util.TipoVideo.V02;
        }

        private void cbTamanhoFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                numeroGUI.Modelo.Textos[0].Altura = System.Convert.ToInt16(cbTamanhoFonte.SelectedItem.ToString());
                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
            }
        }

        private void tbTempoApresentacao_TextChanged(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Focused)
            {
                if (tbTempoApresentacao.Text != "")
                {
                    numeroGUI.Modelo.Textos[0].TempoApresentacao = System.Convert.ToInt16(tbTempoApresentacao.Text);

                    //Setando desenho do painel
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
                }
            }
        }


        private void tbTempoApresentacao_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbTempoRolagem_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbTempoRolagem_TextChanged(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Focused)
            {
                if (tbTempoRolagem.Text != "")
                {
                    numeroGUI.Modelo.Textos[0].TempoRolagem = System.Convert.ToInt16(tbTempoRolagem.Text);

                    //Setando desenho do painel
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
                }
            }
        }

        private void cbFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                numeroGUI.Modelo.Textos[0].Fonte = cbFonte.SelectedItem.ToString();
                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
            }
        }

        private void cbApresentacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                numeroGUI.Modelo.Textos[0].Apresentacao = (Util.Util.Rolagem)cbApresentacao.SelectedIndex;

                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
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
            //Seta a GUI com o que esta no construtor da classe Texto
            cbApresentacao.SelectedIndex = (int)numeroGUI.Modelo.Textos[0].Apresentacao;
            tbTempoApresentacao.Text = numeroGUI.Modelo.Textos[0].TempoApresentacao.ToString();
            tbTempoRolagem.Text = numeroGUI.Modelo.Textos[0].TempoRolagem.ToString();


            if (numeroGUI.Modelo.Textos[0].FonteWindows)
            {
                CarregarFontesTrueType();
                cbFonte.SelectedItem = numeroGUI.Modelo.Textos[0].Fonte;
                cbTamanhoFonte.SelectedItem = numeroGUI.Modelo.Textos[0].Altura.ToString();
            }
            else
            {
                CarregarFontesPontos();
                cbFonte.SelectedItem = numeroGUI.Modelo.Textos[0].Fonte;
            }

            //habilitar botoes
            HabilitarBotoesFontes();

            //checar botões de acordo com o que esta setadono construtor da classe texto
            ChecarBotoes();

        }

        #endregion

        #region Fontes

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

            PopularNivelSuavizacao();
        }

        private void SetarFonteTrueTypePadrao()
        {
            numeroGUI.Modelo.Textos[0].FonteWindows = true;
            numeroGUI.Modelo.Textos[0].FonteAnteriorWindows = false;

            //Setando e selecionando a fonte padrao do Pontos     
            //fachada.SetarFontesDefaultFrases(numeroGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fachada.SetarFonteDefaultTexto(numeroGUI.Modelo.Textos[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            numeroGUI.Modelo.Textos[0].FonteAnteriorWindows = true;

            numeroGUI.Modelo.Textos[0].Negrito = false;
            numeroGUI.Modelo.Textos[0].Sublinhado = false;
            numeroGUI.Modelo.Textos[0].Italico = false;

            cbFonte.SelectedItem = numeroGUI.Modelo.Textos[0].Fonte;
            cbTamanhoFonte.SelectedItem = numeroGUI.Modelo.Textos[0].Altura.ToString();
        }

        private void SetarFonteLighDotPadrao()
        {

            numeroGUI.Modelo.Textos[0].FonteWindows = false;
            numeroGUI.Modelo.Textos[0].FonteAnteriorWindows = true;

            //Setando e selecionando a fonte padrao do Pontos     
            //fachada.SetarFontesDefaultFrases(numeroGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fachada.SetarFonteDefaultTexto(numeroGUI.Modelo.Textos[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            numeroGUI.Modelo.Textos[0].FonteAnteriorWindows = false;

            //Setando e selecionando a fonte padrao do Pontos
            numeroGUI.Modelo.Textos[0].Negrito = false;
            numeroGUI.Modelo.Textos[0].Sublinhado = false;
            numeroGUI.Modelo.Textos[0].Italico = false;

            cbFonte.SelectedItem = numeroGUI.Modelo.Textos[0].Fonte;
        }
        private void PopularNivelSuavizacao()
        {
            cbNivelSuavizacao.Items.Clear();
            for (int i = 0; i < 50; i++)
                cbNivelSuavizacao.Items.Add(i * 5);
        }
        private void CarregarFontes()
        {
            if (!numeroGUI.Modelo.Textos[0].FonteWindows)
            {
                CarregarFontesTrueType();
                SetarFonteTrueTypePadrao();
            }
            else
            {
                CarregarFontesPontos();
                SetarFonteLighDotPadrao();
            }
        }

        #endregion

        #region EntrarNaTela

        public void EditarFrase(Frase numero)
        {
            numeroGUI = new Frase(numero);

            tbIndice.Text = (roteiroSelecionado + 1).ToString("000");
            tbNumero.Text = numeroGUI.LabelFrase;

            //Lipando os objetos tipo texto
            modeloTexto.CarregarTextos(numeroGUI.Modelo.Textos[0].LabelTexto);

            //Popular ComboApresentacao
            PopularApresentacao();

            //carregar na gui as propriedades do objeto texto selecionado
            CarregarGUITextoSelecionado();

            modeloTexto.Visible = true;
            //panelModelos.Controls[0].Visible = true;

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
            
        }

        #endregion

        #region ControleLabelUserControl

        //função chamada pelo User Control ao entrar no objeto e no change(texto, n + texto, texto duplo...)
        public void SetTextoSelecionado(string texto)
        {
            numeroGUI.Modelo.Textos[0].LabelTexto = texto;
            numeroGUI.LabelFrase = texto;
            tbNumero.Text = texto;

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), numeroGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(numeroGUI);
        
        }

        #endregion

        
    }
}
