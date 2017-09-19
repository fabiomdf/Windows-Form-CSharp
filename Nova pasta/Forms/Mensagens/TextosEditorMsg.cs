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


namespace PontosX2.Forms.Mensagens
{
    public partial class TextosEditorMsg : UserControl
    {
        
        #region Propriedades
        public Mensagens Mensagens { get; set; }
        public ModeloMsg.TextoMsg modeloTexto { get; set; }
        public ModeloMsg.TextoDuploMsg modeloTextoDuplo { get; set; }
        public ModeloMsg.TextoDuploTextoDuploMsg modeloTextoDuploTextoDuplo { get; set; }
        public ModeloMsg.TextoTriploMsg modeloTextoTriplo { get; set; }

        Fachada fachada = Fachada.Instance;

        public ResourceManager rm;

        public Frase fraseGUI;
        public int controladorSelecionado;
        public int painelSelecionado;
        public int mensagemSelecionada;
        public int fraseSelecionada;

        public int textoSelecionado;

        //Flag pra saber se é inclusão de frases
        public bool isEdicao;

        private Frase fraseContinuar;

        #endregion

        #region Construtor
        public TextosEditorMsg()
        {
            InitializeComponent();

            modeloTexto = new ModeloMsg.TextoMsg();
            this.panelModelos.Controls.Add(modeloTexto);
            this.modeloTexto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTexto.Location = new System.Drawing.Point(0, 0);
            this.modeloTexto.Size = new System.Drawing.Size(500, 200);
            this.modeloTexto.TabIndex = 0;

            modeloTextoDuplo = new ModeloMsg.TextoDuploMsg();
            this.panelModelos.Controls.Add(modeloTextoDuplo);
            this.modeloTextoDuplo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoDuplo.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoDuplo.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoDuplo.TabIndex = 0;

            modeloTextoDuploTextoDuplo = new ModeloMsg.TextoDuploTextoDuploMsg();
            this.panelModelos.Controls.Add(modeloTextoDuploTextoDuplo);
            this.modeloTextoDuploTextoDuplo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoDuploTextoDuplo.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoDuploTextoDuplo.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoDuploTextoDuplo.TabIndex = 0;

            modeloTextoTriplo = new ModeloMsg.TextoTriploMsg();
            this.panelModelos.Controls.Add(modeloTextoTriplo);
            this.modeloTextoTriplo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoTriplo.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoTriplo.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoTriplo.TabIndex = 0;

            rm = fachada.carregaIdioma();
            AplicaIdioma();

            cbListaImages.Items.Clear();
            cbListaImages.Items.AddRange(fachada.CarregarImagens());
        }

        #endregion

        #region Globalizacao
        public void AplicaIdioma()
        {

            this.groupBoxInfo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_GROUP_BOX_INFO");
            this.labelIndiceMensagem.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_LABEL_INDICE");
            this.labelTexto.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_LABEL_TEXT");
            this.chkTextoAutomatico.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TEXTO_AUTOMATICO");

            this.labelModelo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_LABEL_MODELO");
            this.labelApresentacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_LABEL_PRESENTATION");
            this.labelRolagem.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_LABEL_ROLAGEM");
            this.labelFonte.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_LABEL_FONTE");
            this.labelTamanho.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_LABEL_TAMANHO");

            this.btAlinharAbaixo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_ALIGN_BOTTOM");
            this.btAlinharAcima.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_ALIGN_UP");
            this.btAlinharCentro.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_ALIGN_CENTER");
            this.btAlinharDireita.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_ALIGN_RIGHT");
            this.btAlinharEsquerda.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_ALIGN_LEFT");
            this.btAlinharMeio.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_ALIGN_MIDDLE");
            this.btMudarFonte.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_MUDAR_FONTE");
            this.btNegrito.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_NEGRITO");
            this.btItalico.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_ITALICO");
            this.btSublinhado.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_SUBLINHADO");
            this.btNegrito.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_NEGRITO_TEXT");
            this.btItalico.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_ITALICO_TEXT");
            this.btSublinhado.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_SUBLINHADO_TEXT");

            this.btnCancelar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_BTN_CANCELAR");
            this.btnSalvar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_BTN_SALVAR");
            this.btnContinuar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_BTN_CONTINUAR");

            this.lbNivelSuavizacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_NIVEL_SUAVIZACAO");
            this.chkAgrupar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_MESCLAR");
            this.lbImage.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_LABEL_IMAGEM");
            this.btInsertImage.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_MENSAGENS_TOOLSTRIP_IMAGEM");

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

        private void btAlinharEsquerda_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharMeio_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharDireita_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Direita;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharAbaixo_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharCentro_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoV = Util.Util.AlinhamentoVertical.Centro;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btAlinharAcima_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoV = Util.Util.AlinhamentoVertical.Cima;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        public void ChecarBotoes()
        {
            switch (fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoH)
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

            switch (fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoV)
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

            if (fraseGUI.Modelo.Textos[textoSelecionado].FonteWindows)
            {
                btMudarFonte.Checked = true;
                if (fraseGUI.Modelo.Textos[textoSelecionado].Italico)
                    btItalico.Checked = true;
                else
                    btItalico.Checked = false;

                if (fraseGUI.Modelo.Textos[textoSelecionado].Negrito)
                    btNegrito.Checked = true;
                else
                    btNegrito.Checked = false;

                if (fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado)
                    btSublinhado.Checked = true;
                else
                    btSublinhado.Checked = false;
            }

        }

        private void btSublinhado_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado)
                fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado = false;
            else
                fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado = true;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btItalico_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[textoSelecionado].Italico)
                fraseGUI.Modelo.Textos[textoSelecionado].Italico = false;
            else
                fraseGUI.Modelo.Textos[textoSelecionado].Italico = true;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void btNegrito_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[textoSelecionado].Negrito)
                fraseGUI.Modelo.Textos[textoSelecionado].Negrito = false;
            else
                fraseGUI.Modelo.Textos[textoSelecionado].Negrito = true;
            ChecarBotoes();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        private void HabilitarBotoesFontes()
        {
            if (fraseGUI.Modelo.Textos[textoSelecionado].FonteWindows)
            {
                cbTamanhoFonte.Enabled = true;
                btNegrito.Enabled = true;
                btItalico.Enabled = true;
                btSublinhado.Enabled = true;
                btMudarFonte.Checked = true;
                cbNivelSuavizacao.Enabled = true;
                cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(fraseGUI.Modelo.Textos[textoSelecionado].BinaryThreshold);
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
                cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(fraseGUI.Modelo.Textos[textoSelecionado].BinaryThreshold);
            }

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

        private void btCancelar_Click(object sender, EventArgs e)
        {
            //habilitar combobox do painel se for inclusão de roteiro, se for edição habilitar o combo de paineis
            if (this.Mensagens.incluirMensagem)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            //Popular Bitmap do Painel com valor padrao
            //((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fachada.PreparaBitMapPainel(controladorSelecionado, painelSelecionado));
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

            this.Mensagens.DesmarcarListViews();

            this.Visible = false;
            this.Mensagens.Visible = true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            if (isEdicao)
                this.Mensagens.EditarFraseMensagemGUI(fraseGUI);
            else
                this.Mensagens.IncluirFraseMensagemGUI(fraseGUI);

            //habilitar combobox do painel se for inclusão de roteiro, se for edição habilitar o combo de paineis
            if (this.Mensagens.incluirMensagem)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            //Popular Bitmap do Painel com valor padrao
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

            this.Mensagens.DesmarcarListViews();

            this.Visible = false;
            this.Mensagens.Visible = true;
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {

            if (isEdicao)
                this.Mensagens.EditarFraseMensagemGUI(fraseGUI);
            else
                this.Mensagens.IncluirFraseMensagemGUI(fraseGUI);

            isEdicao = false;

            fraseContinuar = new Frase(fraseGUI);
            mensagemSelecionada = Convert.ToInt16(tbIndice.Text);

            //Criar Uma nova frase
            CriarNovaFrase();
        }

        #endregion Botoes

        #region Fontes
        private void CarregarFontes()
        {
            if (!fraseGUI.Modelo.Textos[textoSelecionado].FonteWindows)
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
            fraseGUI.Modelo.Textos[textoSelecionado].FonteWindows = true;
            fraseGUI.Modelo.Textos[textoSelecionado].FonteAnteriorWindows = false;

            //Setando e selecionando a fonte padrao do Pontos     
            //fachada.SetarFontesDefaultFrases(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            // fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[textoSelecionado], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[textoSelecionado], fachada.GetAlturaTexto(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), textoSelecionado));

            fraseGUI.Modelo.Textos[textoSelecionado].FonteAnteriorWindows = true;

            //Setar Fonte Padrao do windows e tamanho
            fraseGUI.Modelo.Textos[textoSelecionado].Negrito = false;
            fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado = false;
            fraseGUI.Modelo.Textos[textoSelecionado].Italico = false;

            cbFonte.SelectedItem = fraseGUI.Modelo.Textos[textoSelecionado].Fonte;
            cbTamanhoFonte.SelectedItem = fraseGUI.Modelo.Textos[textoSelecionado].Altura.ToString();
        }

        private void SetarFonteLighDotPadrao()
        {
            fraseGUI.Modelo.Textos[textoSelecionado].FonteWindows = false;
            fraseGUI.Modelo.Textos[textoSelecionado].FonteAnteriorWindows = true;

            //setar altura e fonte padrao para o painel selecionado
            //fachada.SetarFontesDefaultFrases(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            //fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[textoSelecionado], fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[textoSelecionado], fachada.GetAlturaTexto(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), textoSelecionado));

            fraseGUI.Modelo.Textos[textoSelecionado].FonteAnteriorWindows = false;

            //Setando e selecionando a fonte padrao do Pontos            
            fraseGUI.Modelo.Textos[textoSelecionado].Negrito = false;
            fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado = false;
            fraseGUI.Modelo.Textos[textoSelecionado].Italico = false;

            cbFonte.SelectedItem = fraseGUI.Modelo.Textos[textoSelecionado].Fonte;
        }

        #endregion Fontes

        #region EntrarNaTela

        public void CriarNovaFrase()
        {
            //ja cria a frase do tipo texto
            if (fraseContinuar == null)
            {
                fraseGUI = new Frase(rm.GetString("USER_CONTROL_MODELOS_TEXTO_01"));

                cbModelo.SelectedIndex = (int)fraseGUI.Modelo.TipoModelo;
                CriarListaTextos();

                //setar altura e fonte padrao para o painel selecionado
                fachada.SetarFontesDefaultFrases(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
            }
            else
            {
                fraseGUI = new Frase(fraseContinuar);
                fraseGUI.TextoAutomatico = true;
                switch (fraseGUI.Modelo.TipoModelo)
                {
                    case Util.Util.TipoModelo.Texto:
                        cbModelo.SelectedIndex = 0;
                        fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                        break;
                    case Util.Util.TipoModelo.TextoDuplo:
                        cbModelo.SelectedIndex = 1;
                        fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                        fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                        break;
                    case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                        cbModelo.SelectedIndex = 2;
                        fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                        fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                        fraseGUI.Modelo.Textos[2].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_03");
                        fraseGUI.Modelo.Textos[3].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_04");
                        break;
                    case Util.Util.TipoModelo.TextoTriplo:
                        cbModelo.SelectedIndex = 3;
                        fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                        fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                        fraseGUI.Modelo.Textos[2].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_03");
                        break;
                }
                EdicaoListaTextos();
            }

            tbIndice.Text = (mensagemSelecionada + 1).ToString("000");
            tbLabelFrase.Text = "";

            //Popular ComboApresentacao
            PopularApresentacao();

            //carregar na gui as propriedades do objeto texto selecionado
            CarregarGUITextoSelecionado();

            //Setando o check do texto automaticante
            chkTextoAutomatico.Checked = fraseGUI.TextoAutomatico;
            chkAgrupar.Checked = (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02 ? true : false);
            SetLabelFrase();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);

        }

        public void EditarFrase(Frase frase)
        {
            fraseGUI = new Frase(frase);

            tbIndice.Text = (mensagemSelecionada + 1).ToString("000");
            tbLabelFrase.Enabled = !(fraseGUI.TextoAutomatico);
            tbLabelFrase.Text = fraseGUI.LabelFrase;
            chkTextoAutomatico.Checked = fraseGUI.TextoAutomatico;
            chkAgrupar.Checked = (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02 ? true : false);

            switch (fraseGUI.Modelo.TipoModelo)
            {
                case Util.Util.TipoModelo.Texto:
                    cbModelo.SelectedIndex = 0;
                    EdicaoListaTextos();
                    break;
                case Util.Util.TipoModelo.TextoDuplo:
                    cbModelo.SelectedIndex = 1;
                    EdicaoListaTextos();
                    break;
                case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                    cbModelo.SelectedIndex = 2;
                    EdicaoListaTextos();
                    break;
                case Util.Util.TipoModelo.TextoTriplo:
                    cbModelo.SelectedIndex = 3;
                    EdicaoListaTextos();
                    break;
            }

            //Popular ComboApresentacao
            PopularApresentacao();

            //carregar na gui as propriedades do objeto texto selecionado
            CarregarGUITextoSelecionado();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);

            //Habilitando os trackbars se v04
            if (!chkAgrupar.Checked)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).ReposicionarVideoMapNoPainel(true);
        }

        #endregion EntrarNaTela

        #region AlterarGUI

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[textoSelecionado].BinaryThreshold = System.Convert.ToInt16(cbNivelSuavizacao.SelectedItem.ToString());
                //Setando desenho do painel                
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
            }
        }

        private void btInsertImage_Click(object sender, EventArgs e)
        {
            if (null == cbListaImages.SelectedItem)
                return;

            string texto = Util.Util.ABRE_TAG + cbListaImages.SelectedItem.ToString() + Util.Util.FECHA_TAG;

            switch (cbModelo.SelectedIndex)
            {
                case 0:
                    {
                        this.modeloTexto.InserirTexto(texto);
                        this.modeloTexto.Focus();
                    }
                    break;

                case 1:
                    {
                        this.modeloTextoDuplo.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoDuplo.Focus(textoSelecionado);
                    }
                    break;

                case 2:
                    {

                        this.modeloTextoDuploTextoDuplo.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoDuploTextoDuplo.Focus(textoSelecionado);
                    }
                    break;
                case 3:
                    {

                        this.modeloTextoTriplo.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoTriplo.Focus(textoSelecionado);
                    }
                    break;
            }
        }

        private void chkAgrupar_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Focused)
            {
                if (chkAgrupar.Checked)
                {
                    fraseGUI.TipoVideo = Util.Util.TipoVideo.V02;
                    for (int i = 1; i < fraseGUI.Modelo.Textos.Count; i++)
                    {
                        fraseGUI.Modelo.Textos[i].Apresentacao = fraseGUI.Modelo.Textos[0].Apresentacao;
                        fraseGUI.Modelo.Textos[i].TempoApresentacao = fraseGUI.Modelo.Textos[0].TempoApresentacao;
                        fraseGUI.Modelo.Textos[i].TempoRolagem = fraseGUI.Modelo.Textos[0].TempoRolagem;
                    }
                }
                else
                { 
                    fraseGUI.TipoVideo = Util.Util.TipoVideo.V04;
                    int larguraTotalFrase = fraseGUI.CalcularLarguraTotal();
                    int larguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);
                    if ((larguraTotalFrase > larguraPainel - 5) || (!fraseGUI.VerificarLarguraMinimaFrase()))
                    {
                        List<Frase> listaFrase = new List<Frase>();
                        listaFrase.Add(fraseGUI);
                        fachada.ValidarFrasesV04(listaFrase, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado));
                    }
                }

                this.textoSelecionado = 0;

                //Se o evento de chamada das users controls for troca de Textos, deverá carregar todas as informações na GUI, não precisa carregar tudo se for apenas mudança no texto
                CarregarGUITextoSelecionado();

                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
            }
        }

        private void TextosEditorMsg_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                fraseContinuar = null;
                textoSelecionado = 0;
                CarregarTipoModelo();
            }
        }

        private void CarregarTipoModelo()
        {
            cbModelo.Items.Clear();
            if (fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) == Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO && fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                cbModelo.Items.AddRange(fachada.CarregarTipoModeloMensagensTextoTriplo().ToArray());
            else
                cbModelo.Items.AddRange(fachada.CarregarTipoModeloMensagens().ToArray());
        }

        private void cbApresentacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
                {
                    for (int i = 0; i < fraseGUI.Modelo.Textos.Count; i++)
                        fraseGUI.Modelo.Textos[i].Apresentacao = (Util.Util.Rolagem)cbApresentacao.SelectedIndex;
                }
                else
                fraseGUI.Modelo.Textos[textoSelecionado].Apresentacao = (Util.Util.Rolagem)cbApresentacao.SelectedIndex;

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
                    if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
                    {
                        for (int i = 0; i < fraseGUI.Modelo.Textos.Count; i++)
                            fraseGUI.Modelo.Textos[i].TempoApresentacao = System.Convert.ToInt16(tbTempoApresentacao.Text);
                    }
                    else
                    fraseGUI.Modelo.Textos[textoSelecionado].TempoApresentacao = System.Convert.ToInt16(tbTempoApresentacao.Text);
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
                    if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
                    {
                        for (int i = 0; i < fraseGUI.Modelo.Textos.Count; i++)
                            fraseGUI.Modelo.Textos[i].TempoRolagem = System.Convert.ToInt16(tbTempoRolagem.Text);
                    }
                    else
                        fraseGUI.Modelo.Textos[textoSelecionado].TempoRolagem = System.Convert.ToInt16(tbTempoRolagem.Text);

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
                fraseGUI.Modelo.Textos[textoSelecionado].Fonte = cbFonte.SelectedItem.ToString();

                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
            }
        }

        private void cbTamanhoFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[textoSelecionado].Altura = System.Convert.ToInt16(cbTamanhoFonte.SelectedItem.ToString());

                //Setando desenho do painel
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
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

        public void RecarregarTipoModelo()
        {
            int indice = cbModelo.SelectedIndex;
            CarregarTipoModelo();
            cbModelo.SelectedIndex = indice;
        }

        private void PopularNivelSuavizacao()
        {
            cbNivelSuavizacao.Items.Clear();
            for (int i = 0; i < 50; i++)
                cbNivelSuavizacao.Items.Add(i * 5);
        }

        private void chkTextoAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Focused)
            {
                fraseGUI.TextoAutomatico = chkTextoAutomatico.Checked;
                SetLabelFrase();
            }
        }

        private void tbLabelFrase_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Focused)
                SetLabelFrase();
        }

        private void CarregarGUITextoSelecionado()
        {
            //Seta a GUI com o que esta no construtor da classe Texto
            cbApresentacao.SelectedIndex = (int)fraseGUI.Modelo.Textos[textoSelecionado].Apresentacao;
            tbTempoApresentacao.Text = fraseGUI.Modelo.Textos[textoSelecionado].TempoApresentacao.ToString();
            tbTempoRolagem.Text = fraseGUI.Modelo.Textos[textoSelecionado].TempoRolagem.ToString();


            if (fraseGUI.Modelo.Textos[textoSelecionado].FonteWindows)
            {
                CarregarFontesTrueType();
                cbFonte.SelectedItem = fraseGUI.Modelo.Textos[textoSelecionado].Fonte;
                cbTamanhoFonte.SelectedItem = fraseGUI.Modelo.Textos[textoSelecionado].Altura.ToString();
            }
            else
            {
                CarregarFontesPontos();
                cbFonte.SelectedItem = fraseGUI.Modelo.Textos[textoSelecionado].Fonte;
            }

            //habilitar botoes
            HabilitarBotoesFontes();

            //checar botões de acordo com o que esta setadono construtor da classe texto
            ChecarBotoes();

        }

        private void SetLabelFrase()
        {
            if (fraseGUI.TextoAutomatico)
            {
                tbLabelFrase.Enabled = false;
                string labelFrase = "";
                for (int i = 0; i < fraseGUI.Modelo.Textos.Count(); i++)
                    if (i == 0)
                        labelFrase = fraseGUI.Modelo.Textos[i].LabelTexto;
                    else
                        labelFrase = labelFrase + " | " + fraseGUI.Modelo.Textos[i].LabelTexto;
                if (labelFrase != tbLabelFrase.Text)
                {
                    //tbLabelFrase.Text = (labelFrase.Length > 16 ? labelFrase.Substring(0, 16) : labelFrase);
                    //fraseGUI.LabelFrase = (labelFrase.Length > 16 ? labelFrase.Substring(0, 16) : labelFrase); 
                    //tbLabelFrase.Text = fachada.GetTextoSemImagem(labelFrase);
                    //fraseGUI.LabelFrase = fachada.GetTextoSemImagem(labelFrase);
                    string texto = fachada.GetTextoSemImagem(labelFrase);
                    tbLabelFrase.Text = (texto.Length > 50 ? texto.Substring(0, 50) : texto);
                    fraseGUI.LabelFrase = (texto.Length > 50 ? texto.Substring(0, 50) : texto);
                }
            }
            else
            {
                tbLabelFrase.Enabled = true;
                fraseGUI.LabelFrase = tbLabelFrase.Text;
            }
        }

        private void HabilitarUseControl()
        {
            modeloTextoTriplo.Visible = false;
            modeloTextoDuplo.Visible = false;
            modeloTextoDuploTextoDuplo.Visible = false;
            modeloTexto.Visible = true;

            modeloTexto.CarregarTextos(rm.GetString("USER_CONTROL_MODELOS_TEXTO_01"));

            this.textoSelecionado = 0;
        }

        private void CriarListaTextos()
        {
            modeloTextoDuplo.Visible = false;
            modeloTextoDuploTextoDuplo.Visible = false;
            modeloTexto.Visible = false;
            modeloTextoTriplo.Visible = false;


            //Lipando os objetos tipo texto
            fraseGUI.Modelo.Textos.Clear();

            //Setando o TEXTO 01 como o label da mensagem caso seja o primeiro texto inserido
            string labelTexto01 = "";
            if (Mensagens.mensagemGUI.Frases.Count == 0)
                labelTexto01 = Mensagens.mensagemGUI.LabelMensagem;
            else
                labelTexto01 = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");


            switch (cbModelo.SelectedIndex)
            {
                case 0:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    modeloTexto.Visible = true;
                    modeloTexto.CarregarTextos(labelTexto01);
                    break;

                case 1:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    modeloTextoDuplo.Visible = true;
                    modeloTextoDuplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"));
                    break;

                case 2:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuploTextoDuplo;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_04")));
                    modeloTextoDuploTextoDuplo.Visible = true;
                    modeloTextoDuploTextoDuplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_04"));
                    break;

                case 3:
                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoTriplo;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                    modeloTextoTriplo.Visible = true;
                    modeloTextoTriplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"));
                    break;
            }

            //Mesmo não selecionando um item no user control dos textos, selecionar o item 0 como default
            this.textoSelecionado = 0;
        }

        private void EdicaoListaTextos()
        {
            int modelo_anterior = 0;

            switch (fraseGUI.Modelo.TipoModelo)
            {
                case Util.Util.TipoModelo.Texto:
                    modelo_anterior = 0;
                    break;
                case Util.Util.TipoModelo.TextoDuplo:
                    modelo_anterior = 1;
                    break;
                case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                    modelo_anterior = 2;
                    break;
                case Util.Util.TipoModelo.TextoTriplo:
                    modelo_anterior = 3;
                    break;
            }

            modeloTextoDuplo.Visible = false;
            modeloTextoDuploTextoDuplo.Visible = false;
            modeloTexto.Visible = false;
            modeloTextoTriplo.Visible = false;

            //Setando o TEXTO 01 como o label do roteiro caso seja o primeiro texto inserido
            string labelTexto01 = "";
            if (fraseSelecionada == 0)
                labelTexto01 = Mensagens.mensagemGUI.LabelMensagem;
            else
                labelTexto01 = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");


            switch (cbModelo.SelectedIndex)
            {
                case 0: //Texto
                    if (cbModelo.SelectedIndex == modelo_anterior)
                    {
                        modeloTexto.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto);
                        modeloTexto.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        modeloTexto.Visible = true;
                        modeloTexto.CarregarTextos(labelTexto01);
                    }
                    break;

                case 1: //TextoDuplo
                    if (cbModelo.SelectedIndex == modelo_anterior)
                    {
                        modeloTextoDuplo.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto, fraseGUI.Modelo.Textos[1].LabelTexto);
                        modeloTextoDuplo.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                        modeloTextoDuplo.Visible = true;
                        modeloTextoDuplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"));
                    }
                    break;

                case 2: //TextoDuploTextoDuplo
                    if (cbModelo.SelectedIndex == modelo_anterior)
                    {
                        modeloTextoDuploTextoDuplo.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto, fraseGUI.Modelo.Textos[2].LabelTexto, fraseGUI.Modelo.Textos[1].LabelTexto, fraseGUI.Modelo.Textos[3].LabelTexto);
                        modeloTextoDuploTextoDuplo.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuploTextoDuplo;
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_04")));
                        modeloTextoDuploTextoDuplo.Visible = true;
                        modeloTextoDuploTextoDuplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_04"));
                    }
                    break;

                case 3: //TextoTriplo
                    if (cbModelo.SelectedIndex == modelo_anterior)
                    {
                        modeloTextoTriplo.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto, fraseGUI.Modelo.Textos[1].LabelTexto, fraseGUI.Modelo.Textos[2].LabelTexto);
                        modeloTextoTriplo.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoTriplo;
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                        modeloTextoTriplo.Visible = true;
                        modeloTextoTriplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"));
                    }
                    break;
            }

            //Mesmo não selecionando um item no user control dos textos, selecionar o item 0 como default
            this.textoSelecionado = 0;
        }


        private void cbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                int modelo_anterior = 0;

                switch (fraseGUI.Modelo.TipoModelo)
                {
                    case Util.Util.TipoModelo.Texto:
                        modelo_anterior = 0;
                        break;
                    case Util.Util.TipoModelo.TextoDuplo:
                        modelo_anterior = 1;
                        break;
                    case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                        modelo_anterior = 2;
                        break;
                    case Util.Util.TipoModelo.TextoTriplo:
                        modelo_anterior = 3;
                        break;
                }

                if (cbModelo.SelectedIndex != modelo_anterior)
                {
                    if (isEdicao)
                        EdicaoListaTextos();
                    else
                        CriarListaTextos();

                    //setar altura e fonte padrao para o painel selecionado
                    fachada.SetarFontesDefaultFrases(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

                    //Popular ComboApresentacao
                    PopularApresentacao();

                    //carregar na gui as propriedades do objeto texto selecionado
                    CarregarGUITextoSelecionado();

                    //Setando o check do texto automaticante
                    chkTextoAutomatico.Checked = true;
                    SetLabelFrase();

                    //Setando desenho do painel
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                    ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
                }
            }
        }

        public void HabilitarBotoesAlinhamentoAjusteV04()
        {
            // ChecarBotoes();
            if (fraseGUI.Modelo.Textos[this.textoSelecionado].listaBitMap.Count > 1)
                HabilitarAlinhamentoHorizontal(false);
            else
                HabilitarAlinhamentoHorizontal(true);
        }
        #endregion

        #region ControleLabelsUserControl

        //função chamada pelo User Control ao entrar no objeto e no change(texto, n + texto, texto duplo...)
        public void SetTextoSelecionado(int textoSelecionado, string texto, bool onEnterText)
        {
            this.textoSelecionado = textoSelecionado;
            fraseGUI.Modelo.Textos[textoSelecionado].LabelTexto = texto;

            //Se o evento de chamada das users controls for troca de Textos, deverá carregar todas as informações na GUI, não precisa carregar tudo se for apenas mudança no texto
            if (onEnterText)
                CarregarGUITextoSelecionado();

            SetLabelFrase();

            //Setando desenho do painel
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);
        }

        #endregion



    }
}
