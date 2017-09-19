using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controlador;
using System.Resources;

namespace PontosX2.Forms.Motorista
{
    public partial class MotoristaEditor : UserControl
    {

        #region Propriedades

        public Motorista Motoristas { get; set; }

        public Modelo.TextoMotorista modeloTexto { get; set; }

        Fachada fachada = Fachada.Instance;

        public ResourceManager rm;

        public Frase fraseGUI;
        public bool isID;

        public int controladorSelecionado;
        public int painelSelecionado;
        public int motoristaSelecionado;

        #endregion

        #region Construtor

        public MotoristaEditor()
        {
            InitializeComponent();

            modeloTexto = new Modelo.TextoMotorista();
            this.panelModelos.Controls.Add(modeloTexto);
            this.modeloTexto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTexto.Location = new System.Drawing.Point(0, 0);
            this.modeloTexto.Size = new System.Drawing.Size(500, 200);
            this.modeloTexto.TabIndex = 0;

            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        #endregion

        #region Globalização

        public void AplicaIdioma()
        {

            this.groupBoxInfo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_GROUP_BOX_INFO");
            this.labelID.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_LABEL_ID");
            this.labelNome.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_LABEL_NOME");
            this.labelRolagem.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_LABEL_ROLAGEM"); ;
            this.labelApresentacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_LABEL_APRESENTACAO"); ;
            this.labelFonte.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_LABEL_FONTE"); ;
            this.labelTamanho.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_LABEL_TAMANHO");

            this.btAlinharAbaixo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_ALIGN_BOTTOM");
            this.btAlinharAcima.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_ALIGN_UP");
            this.btAlinharCentro.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_ALIGN_CENTER");
            this.btAlinharDireita.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_ALIGN_RIGHT");
            this.btAlinharEsquerda.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_ALIGN_LEFT");
            this.btAlinharMeio.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_ALIGN_MIDDLE");
            this.btMudarFonte.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_MUDAR_FONTE");
            this.btNegrito.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_NEGRITO");
            this.btItalico.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_ITALICO");
            this.btSublinhado.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_SUBLINHADO");
            this.btNegrito.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_NEGRITO_TEXT");
            this.btItalico.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_ITALICO_TEXT");
            this.btSublinhado.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_TOOLSTRIP_SUBLINHADO_TEXT");

            this.btCancelar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_BTN_CANCELAR");
            this.btSalvar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_BTN_SALVAR");
            this.lbNivelSuavizacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MOTORISTA_NIVEL_SUAVIZACAO");

        }

        #endregion

        #region Alteracoes GUI

        private void btCancelar_Click(object sender, EventArgs e)
        {
            //habilitar combobox do painel se for inclusão de roteiro, se for edição habilitar o combo de paineis
            if (!this.Motoristas.isEdicao)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            this.Visible = false;
            this.Motoristas.Visible = true;

            if (isID)
                this.Motoristas.SetarCampo(true);
            else
                this.Motoristas.SetarCampo(false);
            //Popular Bitmap do Painel com valor padrao
            //fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            //((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        public Boolean exibirErro(bool setarCampo)
        {

            if (tbID.Text == "")
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_ID_VAZIO"));
                if (setarCampo)
                    modeloTexto.SetarTexto();
                return true;
            }

            if (tbNome.Text == "")
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_NOME_VAZIO"));
                if (setarCampo)
                    modeloTexto.SetarTexto();
                return true;
            }

            Controlador.Motorista motTemp = new Controlador.Motorista(this.Motoristas.motoristaGUI);
            if (isID)
                motTemp.ID = fraseGUI;
            else
                motTemp.Nome = fraseGUI;

            if (fachada.AchouMotoristaID(controladorSelecionado, this.Motoristas.indiceMotorista, this.Motoristas.isEdicao, motTemp))
            {
                MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_MBOX_MESMO_ID"));
                if (setarCampo)
                    modeloTexto.SetarTexto();
                return true;
            }

            return false;
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {

            //if (exibirErro(true))
            //    return;

            //habilitar combobox do painel se for inclusão de roteiro, se for edição habilitar o combo de paineis
            if (!this.Motoristas.isEdicao)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            this.Visible = false;
            this.Motoristas.Visible = true;

            if (isID)
            {
                this.Motoristas.EditarIDGUI(fraseGUI);
                this.Motoristas.SetarCampo(true);
            }
            else
            {
                this.Motoristas.EditarNomeGUI(fraseGUI);
                this.Motoristas.SetarCampo(false);
            }


            //Popular Bitmap do Painel com valor padrao
            //fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            //((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void PopularNivelSuavizacao()
        {
            cbNivelSuavizacao.Items.Clear();
            for (int i = 0; i < 50; i++)
                cbNivelSuavizacao.Items.Add(i * 5);
        }

        private void btAlinharEsquerda_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharMeio_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharDireita_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Direita;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharAbaixo_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharCentro_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Centro;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharAcima_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Cima;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void cbNivelSuavizacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[0].BinaryThreshold = System.Convert.ToInt16(cbNivelSuavizacao.SelectedItem.ToString());
                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
            }
        }

        private void cbApresentacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[0].Apresentacao = (Util.Util.Rolagem)cbApresentacao.SelectedIndex;

                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
            }
        }

        private void tbTempoApresentacao_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbTempoApresentacao_TextChanged(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Focused)
            {
                if (tbTempoApresentacao.Text != "")
                {
                    fraseGUI.Modelo.Textos[0].TempoApresentacao = System.Convert.ToInt16(tbTempoApresentacao.Text);

                    //Setando desenho do painel
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
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
            {
                if (tbTempoRolagem.Text != "")
                {
                    fraseGUI.Modelo.Textos[0].TempoRolagem = System.Convert.ToInt16(tbTempoRolagem.Text);

                    //Setando desenho do painel
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
                }
            }
        }

        private void cbFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[0].Fonte = cbFonte.SelectedItem.ToString();
                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
            }
        }

        private void cbTamanhoFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[0].Altura = System.Convert.ToInt16(cbTamanhoFonte.SelectedItem.ToString());
                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
            }
        }

        private void btNegrito_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[0].Negrito)
                fraseGUI.Modelo.Textos[0].Negrito = false;
            else
                fraseGUI.Modelo.Textos[0].Negrito = true;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void ChecarBotoes()
        {
            switch (fraseGUI.Modelo.Textos[0].AlinhamentoH)
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

            switch (fraseGUI.Modelo.Textos[0].AlinhamentoV)
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

            if (fraseGUI.Modelo.Textos[0].FonteWindows)
            {
                btMudarFonte.Checked = true;
                if (fraseGUI.Modelo.Textos[0].Italico)
                    btItalico.Checked = true;
                else
                    btItalico.Checked = false;

                if (fraseGUI.Modelo.Textos[0].Negrito)
                    btNegrito.Checked = true;
                else
                    btNegrito.Checked = false;

                if (fraseGUI.Modelo.Textos[0].Sublinhado)
                    btSublinhado.Checked = true;
                else
                    btSublinhado.Checked = false;
            }

        }

        private void btItalico_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[0].Italico)
                fraseGUI.Modelo.Textos[0].Italico = false;
            else
                fraseGUI.Modelo.Textos[0].Italico = true;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btSublinhado_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[0].Sublinhado)
                fraseGUI.Modelo.Textos[0].Sublinhado = false;
            else
                fraseGUI.Modelo.Textos[0].Sublinhado = true;
            ChecarBotoes();
            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btMudarFonte_Click(object sender, EventArgs e)
        {
            //Carregar Fontes
            CarregarFontes();

            //Habilitar botoes de acordo com a fonte(Pontos ou Windows)
            HabilitarBotoesFontes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void HabilitarBotoesFontes()
        {
            if (fraseGUI.Modelo.Textos[0].FonteWindows)
            {
                cbTamanhoFonte.Enabled = true;
                btNegrito.Enabled = true;
                btItalico.Enabled = true;
                btSublinhado.Enabled = true;
                btMudarFonte.Checked = true;
                cbNivelSuavizacao.Enabled = true;
                cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(fraseGUI.Modelo.Textos[0].BinaryThreshold);
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
                cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(fraseGUI.Modelo.Textos[0].BinaryThreshold);

            }
        }

        #endregion

        #region Fontes

        private void CarregarFontes()
        {
            if (!fraseGUI.Modelo.Textos[0].FonteWindows)
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

        private void SetarFonteLighDotPadrao()
        {
            fraseGUI.Modelo.Textos[0].FonteWindows = false;
            fraseGUI.Modelo.Textos[0].FonteAnteriorWindows = true;

            //Setando e selecionando a fonte padrao do Pontos     
            //fachada.SetarFontesDefaultFrases(numeroGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fraseGUI.Modelo.Textos[0].FonteAnteriorWindows = false;

            //Setando e selecionando a fonte padrao do Pontos
            fraseGUI.Modelo.Textos[0].Negrito = false;
            fraseGUI.Modelo.Textos[0].Sublinhado = false;
            fraseGUI.Modelo.Textos[0].Italico = false;

            cbFonte.SelectedItem = fraseGUI.Modelo.Textos[0].Fonte;
        }

        private void CarregarFontesPontos()
        {
            cbFonte.Items.Clear();
            cbFonte.Items.AddRange(fachada.ExibeFontesPontos().ToArray());
        }

        private void SetarFonteTrueTypePadrao()
        {
            fraseGUI.Modelo.Textos[0].FonteWindows = true;
            fraseGUI.Modelo.Textos[0].FonteAnteriorWindows = false;

            //Setando e selecionando a fonte padrao do Pontos     
            //fachada.SetarFontesDefaultFrases(numeroGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[0], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fraseGUI.Modelo.Textos[0].FonteAnteriorWindows = true;

            fraseGUI.Modelo.Textos[0].Negrito = false;
            fraseGUI.Modelo.Textos[0].Sublinhado = false;
            fraseGUI.Modelo.Textos[0].Italico = false;

            cbFonte.SelectedItem = fraseGUI.Modelo.Textos[0].Fonte;
            cbTamanhoFonte.SelectedItem = fraseGUI.Modelo.Textos[0].Altura.ToString();
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

        #endregion

        #region ControleLabelUserControl

        //função chamada pelo User Control ao entrar no objeto e no change(texto, n + texto, texto duplo...)
        public void SetTextoSelecionado(string texto)
        {
            fraseGUI.Modelo.Textos[0].LabelTexto = texto;
            fraseGUI.LabelFrase = texto;
            if (isID)
                tbID.Text = texto;
            else
                tbNome.Text = texto;

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);

        }

        #endregion

        #region EntrarNaTela

        public void EditarFrase(Frase frase, bool isID, string texto)
        {
            fraseGUI = new Frase(frase);

            this.isID = isID;
            if (isID)
            {
                tbID.Text = frase.LabelFrase;
                tbNome.Text = texto;
            }
            else
            {
                tbID.Text = texto;
                tbNome.Text = frase.LabelFrase;
            }

            //Lipando os objetos tipo texto
            modeloTexto.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto);

            //Popular ComboApresentacao
            PopularApresentacao();

            //carregar na gui as propriedades do objeto texto selecionado
            CarregarGUITextoSelecionado();

            modeloTexto.Visible = true;
            //panelModelos.Controls[0].Visible = true;

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);

        }

        private void CarregarGUITextoSelecionado()
        {
            //Seta a GUI com o que esta no construtor da classe Texto
            cbApresentacao.SelectedIndex = (int)fraseGUI.Modelo.Textos[0].Apresentacao;
            tbTempoApresentacao.Text = fraseGUI.Modelo.Textos[0].TempoApresentacao.ToString();
            tbTempoRolagem.Text = fraseGUI.Modelo.Textos[0].TempoRolagem.ToString();


            if (fraseGUI.Modelo.Textos[0].FonteWindows)
            {
                CarregarFontesTrueType();
                cbFonte.SelectedItem = fraseGUI.Modelo.Textos[0].Fonte;
                cbTamanhoFonte.SelectedItem = fraseGUI.Modelo.Textos[0].Altura.ToString();
            }
            else
            {
                CarregarFontesPontos();
                cbFonte.SelectedItem = fraseGUI.Modelo.Textos[0].Fonte;
            }

            //habilitar botoes
            HabilitarBotoesFontes();

            //checar botões de acordo com o que esta setadono construtor da classe texto
            ChecarBotoes();
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

        #endregion

    }
}
