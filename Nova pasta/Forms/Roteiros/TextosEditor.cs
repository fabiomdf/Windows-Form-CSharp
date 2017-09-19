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
    public partial class TextosEditor : UserControl
    {

        #region Proprieadades

        public Roteiros Roteiros { get; set; }

        public Modelo.Texto modeloTexto { get; set; }
        public Modelo.NumeroTexto modeloNumeroTexto { get; set; }
        public Modelo.NumeroTextoDuplo modeloNumeroTextoDuplo { get; set; }
        public Modelo.TextoDuplo modeloTextoDuplo { get; set; }
        public Modelo.TextoDuploNumero modeloTextoDuploNumero { get; set; }
        public Modelo.TextoDuploTextoDuplo modeloTextoDuploTextoDuplo { get; set; }
        public Modelo.TextoNumero modeloTextoNumero { get; set; }
        public Modelo.TextoTriplo modeloTextoTriplo { get; set; }
        public Modelo.NumeroTextoTriplo modeloNumeroTextoTriplo { get; set; }
        public Modelo.TextoTriploNumero modeloTextoTriploNumero { get; set; }

        Fachada fachada = Fachada.Instance;

        public ResourceManager rm;

        public Frase fraseGUI;
        public int controladorSelecionado;
        public int painelSelecionado;
        public int roteiroSelecionado;
        public string roteiroNumero;
        public int fraseSelecionada;

        public int textoSelecionado;

        //Flag pra saber se é inclusão de frases ou edição do número
        public bool isFraseIda;
        public bool isEdicao;

        private Frase fraseContinuar;

        #endregion

        #region Construtor

        public TextosEditor()
        {
            InitializeComponent();


            modeloTexto = new Modelo.Texto();
            this.panelModelos.Controls.Add(modeloTexto);
            this.modeloTexto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTexto.Location = new System.Drawing.Point(0, 0);
            this.modeloTexto.Size = new System.Drawing.Size(500, 200);
            this.modeloTexto.TabIndex = 0;

            modeloNumeroTexto = new Modelo.NumeroTexto();
            this.panelModelos.Controls.Add(modeloNumeroTexto);
            this.modeloNumeroTexto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloNumeroTexto.Location = new System.Drawing.Point(0, 0);
            this.modeloNumeroTexto.Size = new System.Drawing.Size(500, 200);
            this.modeloNumeroTexto.TabIndex = 0;

            modeloTextoNumero = new Modelo.TextoNumero();
            this.panelModelos.Controls.Add(modeloTextoNumero);
            this.modeloTextoNumero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoNumero.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoNumero.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoNumero.TabIndex = 0;

            modeloTextoDuplo = new Modelo.TextoDuplo();
            this.panelModelos.Controls.Add(modeloTextoDuplo);
            this.modeloTextoDuplo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoDuplo.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoDuplo.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoDuplo.TabIndex = 0;

            modeloNumeroTextoDuplo = new Modelo.NumeroTextoDuplo();
            this.panelModelos.Controls.Add(modeloNumeroTextoDuplo);
            this.modeloNumeroTextoDuplo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloNumeroTextoDuplo.Location = new System.Drawing.Point(0, 0);
            this.modeloNumeroTextoDuplo.Size = new System.Drawing.Size(500, 200);
            this.modeloNumeroTextoDuplo.TabIndex = 0;

            modeloTextoDuploNumero = new Modelo.TextoDuploNumero();
            this.panelModelos.Controls.Add(modeloTextoDuploNumero);
            this.modeloTextoDuploNumero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoDuploNumero.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoDuploNumero.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoDuploNumero.TabIndex = 0;

            modeloTextoDuploTextoDuplo = new Modelo.TextoDuploTextoDuplo();
            this.panelModelos.Controls.Add(modeloTextoDuploTextoDuplo);
            this.modeloTextoDuploTextoDuplo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoDuploTextoDuplo.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoDuploTextoDuplo.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoDuploTextoDuplo.TabIndex = 0;

            modeloTextoTriplo = new Modelo.TextoTriplo();
            this.panelModelos.Controls.Add(modeloTextoTriplo);
            this.modeloTextoTriplo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoTriplo.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoTriplo.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoTriplo.TabIndex = 0;

            modeloNumeroTextoTriplo = new Modelo.NumeroTextoTriplo();
            this.panelModelos.Controls.Add(modeloNumeroTextoTriplo);
            this.modeloNumeroTextoTriplo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloNumeroTextoTriplo.Location = new System.Drawing.Point(0, 0);
            this.modeloNumeroTextoTriplo.Size = new System.Drawing.Size(500, 200);
            this.modeloNumeroTextoTriplo.TabIndex = 0;

            modeloTextoTriploNumero = new Modelo.TextoTriploNumero();
            this.panelModelos.Controls.Add(modeloTextoTriploNumero);
            this.modeloTextoTriploNumero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.modeloTextoTriploNumero.Location = new System.Drawing.Point(0, 0);
            this.modeloTextoTriploNumero.Size = new System.Drawing.Size(500, 200);
            this.modeloTextoTriploNumero.TabIndex = 0;

            rm = fachada.carregaIdioma();
            AplicaIdioma();

            cbListaImages.Items.Clear();
            cbListaImages.Items.AddRange(fachada.CarregarImagens());

            
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

        private void PopularNivelSuavizacao()
        {
            cbNivelSuavizacao.Items.Clear();
            for (int i = 0; i < 50; i++)
                cbNivelSuavizacao.Items.Add(i*5);
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

        private void btSalvar_Click(object sender, EventArgs e)
        {

           if (isFraseIda)
            {
                if (isEdicao)
                    this.Roteiros.EditarFraseIdaRoteiroGUI(fraseGUI);
                else
                    this.Roteiros.IncluirFraseIdaRoteiroGUI(fraseGUI);
            }
            else
            {
                if (isEdicao)
                    this.Roteiros.EditarFraseVoltaRoteiroGUI(fraseGUI);
                else
                    this.Roteiros.IncluirFraseVoltaRoteiroGUI(fraseGUI);
            }         

            //habilitar combobox do painel se for inclusão de roteiro, se for edição habilitar o combo de paineis
            if (this.Roteiros.incluirRoteiro)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            //Popular Bitmap do Painel com valor padrao
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

            this.Roteiros.DesmarcarListViews();

            this.Visible = false;
            this.Roteiros.Visible = true;
        }

        private void btNegrito_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[textoSelecionado].Negrito)
                fraseGUI.Modelo.Textos[textoSelecionado].Negrito = false;
            else
                fraseGUI.Modelo.Textos[textoSelecionado].Negrito = true;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        private void btItalico_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[textoSelecionado].Italico)
                fraseGUI.Modelo.Textos[textoSelecionado].Italico = false;
            else
                fraseGUI.Modelo.Textos[textoSelecionado].Italico = true;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        private void btSublinhado_Click(object sender, EventArgs e)
        {
            if (fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado)
                fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado = false;
            else
                fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado = true;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        private void btContinuar_Click(object sender, EventArgs e)
        {

            if (isFraseIda)
            {
                if (isEdicao)
                    this.Roteiros.EditarFraseIdaRoteiroGUI(fraseGUI);
                else
                    this.Roteiros.IncluirFraseIdaRoteiroGUI(fraseGUI);
            }
            else
            {
                if (isEdicao)
                    this.Roteiros.EditarFraseVoltaRoteiroGUI(fraseGUI);
                else
                    this.Roteiros.IncluirFraseVoltaRoteiroGUI(fraseGUI);
            }


            isEdicao = false;

            fraseContinuar = new Frase(fraseGUI);
            roteiroSelecionado = Convert.ToInt16(tbIndice.Text);

            //Criar Uma nova frase
            CriarNovaFrase();
        }

        private void btMudarFonte_Click(object sender, EventArgs e)
        {
            //Carregar Fontes
            CarregarFontes();

            //Habilitar botoes de acordo com a fonte(Pontos ou Windows)
            HabilitarBotoesFontes();

            //Setando desenho do painel
            DesenharTextoPainel();

        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            //habilitar combobox do painel se for inclusão de roteiro, se for edição habilitar o combo de paineis
            if (this.Roteiros.incluirRoteiro)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(false, painelSelecionado);
            else
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).TravarPainel(true, painelSelecionado);

            //Popular Bitmap do Painel com valor padrao
            ((Arquivo)Parent.Parent.Parent.Parent.Parent).DesenharPainel();

            this.Roteiros.DesmarcarListViews();

            this.Visible = false;
            this.Roteiros.Visible = true;
        }

        private void btAlinharEsquerda_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        private void btAlinharMeio_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        private void btAlinharDireita_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Direita;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        private void btAlinharAbaixo_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        private void btAlinharCentro_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoV = Util.Util.AlinhamentoVertical.Centro;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        private void btAlinharAcima_Click(object sender, EventArgs e)
        {
            fraseGUI.Modelo.Textos[textoSelecionado].AlinhamentoV = Util.Util.AlinhamentoVertical.Cima;
            ChecarBotoes();

            //Setando desenho do painel
            DesenharTextoPainel();
        }

        #endregion

        #region Globalizacao

        public void AplicaIdioma()
        {

            this.groupBoxInfo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_GROUP_BOX_INFO");
            this.labelIndice.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_LABEL_INDICE_ROTEIRO");
            this.labelNumero.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_LABEL_NUMERO_ROTEIRO");
            this.labelFrase.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_LABEL_FRASE");
            this.chkTextoAutomatico.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TEXTO_AUTOMATICO");

            this.labelMultiLinha.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_MULTILINHA");
            this.labelModelo.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_MODELO");
            this.labelRolagem.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_ROLAGEM");
            this.labelApresentacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_APRESENTACAO");
            this.labelFonte.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_LABEL_FONTE");
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
            this.btContinuar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_BTN_CONTINUAR");

            this.lbNivelSuavizacao.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_NIVEL_SUAVIZACAO");            
            this.chkAgrupar.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_MESCLAR");
            
            this.lbImage.Text = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_LABEL_IMAGEM");
            this.btInsertImage.ToolTipText = rm.GetString("USER_CONTROL_TEXTO_EDITOR_ROTEIROS_TOOLSTRIP_IMAGEM");
        }
        #endregion

        #region Fontes

        public void EnableButtonAlinhamentoEsquerda(bool enabled)
        {
            btAlinharEsquerda.Enabled = enabled;
        }

        public void EnableButtonAlinhamentoDireita(bool enabled)
        {
            btAlinharDireita.Enabled = enabled;
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
            fraseGUI.Modelo.Textos[textoSelecionado].FonteWindows = true;
            fraseGUI.Modelo.Textos[textoSelecionado].FonteAnteriorWindows = false;

            //Setando e selecionando a fonte padrao do Pontos     
            if (fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado)==1)
                fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[textoSelecionado], fachada.GetAlturaTexto(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), textoSelecionado));
            else
                fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[textoSelecionado], fraseGUI.Modelo.Textos[textoSelecionado].AlturaPainel);

            fraseGUI.Modelo.Textos[textoSelecionado].FonteAnteriorWindows = true;

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

            //Setando e selecionando a fonte padrao do Pontos     
            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[textoSelecionado], fachada.GetAlturaTexto(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), textoSelecionado));
            else
                fachada.SetarFonteDefaultTexto(fraseGUI.Modelo.Textos[textoSelecionado], fraseGUI.Modelo.Textos[textoSelecionado].AlturaPainel);

            fraseGUI.Modelo.Textos[textoSelecionado].FonteAnteriorWindows = false;

            fraseGUI.Modelo.Textos[textoSelecionado].Negrito = false;
            fraseGUI.Modelo.Textos[textoSelecionado].Sublinhado = false;
            fraseGUI.Modelo.Textos[textoSelecionado].Italico = false;

            cbFonte.SelectedItem = fraseGUI.Modelo.Textos[textoSelecionado].Fonte;
        }

        private void CarregarFontes()
        {
            if (!fraseGUI.Modelo.Textos[textoSelecionado].FonteWindows)
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

        private void CarregarFontesPontos()
        {
            cbFonte.Items.Clear();
            cbFonte.Items.AddRange(fachada.ExibeFontesPontos().ToArray());
        }

        #endregion

        #region EntrarNaTela

        public void CriarNovaFrase()
        {
            //Se estiver criando a primeira Frase
            if (fraseContinuar == null)
            {

                //Se for painel simples
                if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                {
                    fraseGUI = new Frase();

                    cbModelo.SelectedIndex = (int)fraseGUI.Modelo.TipoModelo;
                    CriarListaTextos();

                    //Setando a fonte e altura para o tamanho do painel na criação de uma nova frase
                    fachada.SetarFontesDefaultFrases(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));
                }

                //se for painel multilinhas
                if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) > 1)
                {
                    fraseGUI = new Frase();
                    fraseGUI.TipoVideo = Util.Util.TipoVideo.V04;
                    fraseGUI.TextoAutomatico = false;
                    fraseGUI.LabelFrase = rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS");
                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;

                    for (int i = 0; i < fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado); i++)
                    {
                        Texto t = new Texto(rm.GetString("ARQUIVO_PAINEL") + " " + (i + 1).ToString("00"));
                        t.AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) / fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado);
                        t.LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);
                        fraseGUI.Modelo.Textos.Add(t);
                    }

                    //desablitando os usercontrol dos outros modelos
                    cbModelo.SelectedIndex = (int)fraseGUI.Modelo.TipoModelo;
                    modeloTexto.Visible = true;
                    modeloNumeroTexto.Visible = false;
                    modeloNumeroTextoDuplo.Visible = false;
                    modeloTextoDuplo.Visible = false;
                    modeloTextoDuploNumero.Visible = false;
                    modeloTextoDuploTextoDuplo.Visible = false;
                    modeloTextoNumero.Visible = false;
                    
                    //carregando o texto no usercontrol
                    textoSelecionado = 0;
                    modeloTexto.CarregarTextos(fraseGUI.Modelo.Textos[textoSelecionado].LabelTexto);

                    //Setando a fonte e altura para o tamanho do painel na criação de uma nova frase
                    fachada.SetarFontesDefaultFrases(fraseGUI, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado)/ fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado));
                }

            }


            //Se estiver criando frase a partir do botão continuar, usa o modelo anterior para criar a nova frase
            if (fraseContinuar != null)
            { 
                //se for painel simples
                if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                {
                    fraseGUI = new Frase(fraseContinuar);
                    fraseGUI.TextoAutomatico = true;
                    cbModelo.SelectedIndex = (int)fraseGUI.Modelo.TipoModelo;
                    switch (fraseGUI.Modelo.TipoModelo)
                    {
                        case Util.Util.TipoModelo.Texto:
                            fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            break;

                        case Util.Util.TipoModelo.NúmeroTexto:
                            fraseGUI.Modelo.Textos[0].LabelTexto = roteiroNumero.ToString();
                            fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            break;

                        case Util.Util.TipoModelo.TextoNúmero:
                            fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            fraseGUI.Modelo.Textos[1].LabelTexto = roteiroNumero.ToString();
                            break;

                        case Util.Util.TipoModelo.TextoDuplo:
                            fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                            break;

                        case Util.Util.TipoModelo.NúmeroTextoDuplo:
                            fraseGUI.Modelo.Textos[0].LabelTexto = roteiroNumero.ToString();
                            fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            fraseGUI.Modelo.Textos[2].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");

                            break;

                        case Util.Util.TipoModelo.TextoDuploNúmero:
                            fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            fraseGUI.Modelo.Textos[1].LabelTexto = roteiroNumero.ToString();
                            fraseGUI.Modelo.Textos[2].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                            break;

                        case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                            fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                            fraseGUI.Modelo.Textos[2].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_03");
                            fraseGUI.Modelo.Textos[3].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_04");
                            break;

                        case Util.Util.TipoModelo.TextoTriplo:
                            fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                            fraseGUI.Modelo.Textos[2].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_03");
                            break;

                        case Util.Util.TipoModelo.NumeroTextoTriplo:
                            fraseGUI.Modelo.Textos[0].LabelTexto = roteiroNumero.ToString();
                            fraseGUI.Modelo.Textos[1].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            fraseGUI.Modelo.Textos[2].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                            fraseGUI.Modelo.Textos[3].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_03");
                            break;

                        case Util.Util.TipoModelo.TextoTriploNumero:
                            fraseGUI.Modelo.Textos[0].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
                            fraseGUI.Modelo.Textos[1].LabelTexto = roteiroNumero.ToString(); 
                            fraseGUI.Modelo.Textos[2].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
                            fraseGUI.Modelo.Textos[3].LabelTexto = rm.GetString("USER_CONTROL_MODELOS_TEXTO_03");
                            break;
                    }

                    EdicaoListaTextos();
                }

                //se for painel multilinhas
                if (fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado) > 1)
                {
                    fraseGUI = new Frase(fraseContinuar);

                    fraseGUI.LabelFrase = rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS");

                    for (int i = 0; i < fraseGUI.Modelo.Textos.Count; i++)
                    {
                        fraseGUI.Modelo.Textos[i].LabelTexto = rm.GetString("ARQUIVO_PAINEL") + " " + (i + 1).ToString("00");
                    }

                    //carregando o texto no usercontrol
                    textoSelecionado = 0;
                    modeloTexto.CarregarTextos(fraseGUI.Modelo.Textos[textoSelecionado].LabelTexto);
                }
                
            }

            if (fraseGUI.TextoAutomatico)
                tbLabelFrase.Text = "";
            else
                tbLabelFrase.Text = fraseGUI.LabelFrase;

            tbIndice.Text = (roteiroSelecionado + 1).ToString("000");
            tbNumero.Text = roteiroNumero;


            //Popular ComboApresentacao
            PopularApresentacao();

            //Popular ComboPainelMultiLinhas
            PopularComboMultiLinhas();

            //Posicionar os componentes se for um painel simples ou multilinha
            PosisionarComponentesPainelMultiLinhas();

            //carregar na gui as propriedades do objeto texto selecionado
            CarregarGUITextoSelecionado();

            //Setando o check do texto automaticante
            chkTextoAutomatico.Checked = fraseGUI.TextoAutomatico;
            chkAgrupar.Checked = (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02 ? true : false);

            //Setar label de frase
            SetLabelFrase();

            //Desenhar o texto no painel
            DesenharTextoPainel();

            //centralizando o painel dos User Controls
            panelModelos.Top = (panel1.Height) / 2 - (panelModelos.Size.Height - 35) / 2;

        }

        public void EditarFrase(Frase frase)
        {
            fraseGUI = new Frase(frase);

            tbIndice.Text = (roteiroSelecionado + 1).ToString("000");
            tbNumero.Text = roteiroNumero;
            //Setando o check e texto
            tbLabelFrase.Enabled = !(fraseGUI.TextoAutomatico);
            tbLabelFrase.Text = fraseGUI.LabelFrase;
            chkTextoAutomatico.Checked = fraseGUI.TextoAutomatico;
            chkAgrupar.Checked = (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02 ? true : false);

            cbModelo.SelectedIndex = (int)fraseGUI.Modelo.TipoModelo;
            EdicaoListaTextos();

            //Popular ComboApresentacao
            PopularApresentacao();

            //Popular ComboPainelMultiLinhas
            PopularComboMultiLinhas();

            //Posicionar os componentes se for um painel simples ou multilinha
            PosisionarComponentesPainelMultiLinhas();

            //carregar na gui as propriedades do objeto texto selecionado
            CarregarGUITextoSelecionado();

            //Desenhando o texto no painel
            DesenharTextoPainel();

            //Habilitando os trackbars se v04
            if (!chkAgrupar.Checked)
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).ReposicionarVideoMapNoPainel(true);

            //centralizando o painel dos User Controls
            panelModelos.Top = (panel1.Height) / 2 - (panelModelos.Size.Height - 35) / 2;
        }

        #endregion

        #region ControleLabelUserControl

        //função chamada pelo User Control ao entrar no objeto e no change(texto, n + texto, texto duplo...)
        public void SetTextoSelecionado(int Selecionado, string texto, bool onEnterText)
        {
            if (fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado)==1)
                this.textoSelecionado = Selecionado;

            fraseGUI.Modelo.Textos[textoSelecionado].LabelTexto = texto;

            //Se o evento de chamada das users controls for troca de Textos, deverá carregar todas as informações na GUI, não precisa carregar tudo se for apenas mudança no texto
            if (onEnterText)
                CarregarGUITextoSelecionado();

            SetLabelFrase();

            DesenharTextoPainel();
        }

        #endregion

        #region AlterarGUI

        private void DesenharTextoPainel()
        {
            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
            {
                fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), fraseGUI);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(fraseGUI);

            }
            else
            {
                Frase f = PrepararFrasePainelMultilinhas(cbMultinha.SelectedIndex);

                fachada.PreparaBitMapFrase(f.Modelo.Textos[0].AlturaPainel, f.Modelo.Textos[0].LarguraPainel, f);
                ((Arquivo)Parent.Parent.Parent.Parent.Parent).PreencherBitMap(f);

            }
        }

        private Frase PrepararFrasePainelMultilinhas(int painelMultilinhas)
        {
            Frase f = new Frase(fraseGUI);
            f.Modelo.Textos.Clear();
            f.Modelo.Textos.Add(new Texto(fraseGUI.Modelo.Textos[painelMultilinhas]));

            return f;
        }


        private void PopularComboMultiLinhas()
        {
            cbMultinha.Items.Clear();
            for (int i = 1; i <= fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado); i++)
                cbMultinha.Items.Add(i);

            cbMultinha.SelectedIndex = 0;
        }

        private void PosisionarComponentesPainelMultiLinhas()
        {
            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
            {
                cbMultinha.Visible = false;
                labelMultiLinha.Visible = false;

                chkAgrupar.Visible = true;
                labelModelo.Visible = true;
                cbModelo.Visible = true;
                chkTextoAutomatico.Enabled = true;
            }
            else
            {
                cbMultinha.Visible = true;
                labelMultiLinha.Visible = true;

                chkAgrupar.Visible = false;
                labelModelo.Visible = false;
                cbModelo.Visible = false;
                chkTextoAutomatico.Enabled = false;
            }

            Rectangle rect = labelMsRolagem.Bounds;

            Point pt = new Point(rect.Left + 30, rect.Top + 3);

            chkAgrupar.Location = pt;
        }


        private void btInsertImage_Click(object sender, EventArgs e)
        {
            if (null == cbListaImages.SelectedItem)
                return;

            string texto = Util.Util.ABRE_TAG + cbListaImages.SelectedItem.ToString() + Util.Util.FECHA_TAG;

            switch (cbModelo.SelectedIndex)
            {
                case (int)Util.Util.TipoModelo.Texto: // Texto
                    {
                        this.modeloTexto.InserirTexto(texto);
                        this.modeloTexto.Focus();
                    }
                    break;

                case (int) Util.Util.TipoModelo.NúmeroTexto: // Número + Texto
                    {
                        this.modeloNumeroTexto.InserirTexto(textoSelecionado, texto);
                        this.modeloNumeroTexto.Focus(textoSelecionado);
                    }
                    break;

                case (int) Util.Util.TipoModelo.TextoNúmero: // Texto + Número
                    {
                        this.modeloTextoNumero.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoNumero.Focus(textoSelecionado);
                    }
                    break;
                case (int) Util.Util.TipoModelo.TextoDuplo: // Texto Duplo
                    {
                        this.modeloTextoDuplo.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoDuplo.Focus(textoSelecionado);
                    }
                    break;

                case (int) Util.Util.TipoModelo.NúmeroTextoDuplo: // Número + Texto Duplo
                    {
                        this.modeloNumeroTextoDuplo.InserirTexto(textoSelecionado, texto);
                        this.modeloNumeroTextoDuplo.Focus(textoSelecionado);
                    }
                    break;

                case (int) Util.Util.TipoModelo.TextoDuploNúmero: // Texto Duplo + Número
                    {
                        this.modeloTextoDuploNumero.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoDuploNumero.Focus(textoSelecionado);
                    }
                    break;

                case (int) Util.Util.TipoModelo.TextoDuploTextoDuplo: // Texto Duplo + Texto Duplo
                    {
                        this.modeloTextoDuploTextoDuplo.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoDuploTextoDuplo.Focus(textoSelecionado);
                    }
                    break;

                case (int)Util.Util.TipoModelo.TextoTriplo: 
                    {
                        this.modeloTextoTriplo.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoTriplo.Focus(textoSelecionado);
                    }
                    break;

                case (int)Util.Util.TipoModelo.NumeroTextoTriplo: 
                    {
                        this.modeloNumeroTextoTriplo.InserirTexto(textoSelecionado, texto);
                        this.modeloNumeroTextoTriplo.Focus(textoSelecionado);
                    }
                    break;

                case (int)Util.Util.TipoModelo.TextoTriploNumero: 
                    {
                        this.modeloTextoTriploNumero.InserirTexto(textoSelecionado, texto);
                        this.modeloTextoTriploNumero.Focus(textoSelecionado);
                    }
                    break;
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

        private void cbNivelSuavizacao_SelectedIndexChanged(object sender, EventArgs e)
        {
  
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[textoSelecionado].BinaryThreshold = System.Convert.ToInt16(cbNivelSuavizacao.SelectedItem.ToString());

                //Setando desenho do painel
                DesenharTextoPainel();                
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
                    if ((larguraTotalFrase > larguraPainel-5) || (!fraseGUI.VerificarLarguraMinimaFrase()))
                    {
                        List<Frase> listaFrase = new List<Frase>();
                        listaFrase.Add(fraseGUI);
                        fachada.ValidarFrasesV04(listaFrase, fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado));
                    }
                }

                this.textoSelecionado = 0;

                //Se o evento de chamada das users controls for troca de Textos, deverá carregar todas as informações na GUI, não precisa carregar tudo se for apenas mudança no texto
                CarregarGUITextoSelecionado();

                DesenharTextoPainel();
                
            }
        }

        private void CarregarTipoModelo()
        {
            cbModelo.Items.Clear();
            if (fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) == Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO && fachada.GetMultiLinhas(controladorSelecionado,painelSelecionado) == 1)
                cbModelo.Items.AddRange(fachada.CarregarTipoModeloTextoTriplo().ToArray());
            else
                cbModelo.Items.AddRange(fachada.CarregarTipoModelo().ToArray());
        }

        public void RecarregarTipoModelo()
        {
            int indice = cbModelo.SelectedIndex;
            CarregarTipoModelo();
            cbModelo.SelectedIndex = indice;
        }

        private void CriarListaTextos()
        {
            modeloTexto.Visible = false;
            modeloNumeroTexto.Visible = false;
            modeloNumeroTextoDuplo.Visible = false;
            modeloTextoDuplo.Visible = false;
            modeloTextoDuploNumero.Visible = false;
            modeloTextoDuploTextoDuplo.Visible = false;
            modeloTextoNumero.Visible = false;
            modeloTextoTriplo.Visible = false;
            modeloNumeroTextoTriplo.Visible = false;
            modeloTextoTriploNumero.Visible = false;

            //Lipando os objetos tipo texto
            fraseGUI.Modelo.Textos.Clear();

            //Setando o TEXTO 01 como o label do roteiro caso seja o primeiro texto inserido
            string labelTexto01 = "";
            if (isFraseIda)
            {
                if (Roteiros.roteiroGUI.FrasesIda.Count == 0)
                    labelTexto01 = Roteiros.roteiroGUI.LabelRoteiro;
                else
                    labelTexto01 = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
            }
            else
            {
                if (Roteiros.roteiroGUI.FrasesVolta.Count == 0)
                    labelTexto01 = Roteiros.roteiroGUI.LabelRoteiro;
                else
                    labelTexto01 = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
            }

            switch (cbModelo.SelectedIndex)
            {
                case (int)Util.Util.TipoModelo.Texto:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    modeloTexto.Visible = true;
                    modeloTexto.CarregarTextos(labelTexto01);
                    break;

                case (int)Util.Util.TipoModelo.NúmeroTexto:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
                    fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    
                    modeloNumeroTexto.Visible = true;
                    modeloNumeroTexto.CarregarTextos(roteiroNumero.ToString(), labelTexto01);
                    break;

                case (int)Util.Util.TipoModelo.TextoNúmero:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoNúmero;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                    modeloTextoNumero.Visible = true;
                    modeloTextoNumero.CarregarTextos(roteiroNumero.ToString(), labelTexto01);
                    break;

                case (int)Util.Util.TipoModelo.TextoDuplo:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    modeloTextoDuplo.Visible = true;
                    modeloTextoDuplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"));
                    break;

                case (int)Util.Util.TipoModelo.NúmeroTextoDuplo:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTextoDuplo;
                    fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    modeloNumeroTextoDuplo.Visible = true;
                    modeloNumeroTextoDuplo.CarregarTextos(roteiroNumero.ToString(), labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"));
                    break;

                case (int)Util.Util.TipoModelo.TextoDuploNúmero:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuploNúmero;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    modeloTextoDuploNumero.Visible = true;
                    modeloTextoDuploNumero.CarregarTextos(roteiroNumero.ToString(), labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"));
                    break;

                case (int)Util.Util.TipoModelo.TextoDuploTextoDuplo:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuploTextoDuplo;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_04")));
                    modeloTextoDuploTextoDuplo.Visible = true;
                    modeloTextoDuploTextoDuplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_04"));
                    break;

                case (int)Util.Util.TipoModelo.TextoTriplo:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoTriplo;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                    modeloTextoTriplo.Visible = true;
                    modeloTextoTriplo.CarregarTextos(labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"));

                    break;

                case (int)Util.Util.TipoModelo.NumeroTextoTriplo:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.NumeroTextoTriplo;
                    fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                    modeloNumeroTextoTriplo.Visible = true;
                    modeloNumeroTextoTriplo.CarregarTextos(roteiroNumero.ToString(), labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"));
                    break;

                case (int)Util.Util.TipoModelo.TextoTriploNumero:

                    fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoTriploNumero;
                    fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                    fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                    fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                    modeloTextoTriploNumero.Visible = true;
                    modeloTextoTriploNumero.CarregarTextos(labelTexto01, roteiroNumero.ToString(), rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"));
                    break;

            }

            //Mesmo não selecionando um item no user control dos textos, selecionar o item 0 como default
            this.textoSelecionado = 0;
        }

        private void EdicaoListaTextos()
        {
            modeloTexto.Visible = false;
            modeloNumeroTexto.Visible = false;
            modeloNumeroTextoDuplo.Visible = false;
            modeloTextoDuplo.Visible = false;
            modeloTextoDuploNumero.Visible = false;
            modeloTextoDuploTextoDuplo.Visible = false;
            modeloTextoNumero.Visible = false;
            modeloTextoTriplo.Visible = false;
            modeloNumeroTextoTriplo.Visible = false;
            modeloTextoTriploNumero.Visible = false;

            //Setando o TEXTO 01 como o label do roteiro caso seja o primeiro texto inserido
            string labelTexto01 = "";
            if (isFraseIda)
            {
                if (fraseSelecionada == 0)
                    labelTexto01 = Roteiros.roteiroGUI.LabelRoteiro;
                else
                    labelTexto01 = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
            }
            else
            {
                if (fraseSelecionada == 0)
                    labelTexto01 = Roteiros.roteiroGUI.LabelRoteiro;
                else
                    labelTexto01 = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
            }

            switch (cbModelo.SelectedIndex)
            {
                case (int)Util.Util.TipoModelo.Texto:

                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
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

                case (int)Util.Util.TipoModelo.NúmeroTexto:

                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
                    {
                        modeloNumeroTexto.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto, fraseGUI.Modelo.Textos[1].LabelTexto);
                        modeloNumeroTexto.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
                        fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        modeloNumeroTexto.Visible = true;
                        modeloNumeroTexto.CarregarTextos(roteiroNumero.ToString(), labelTexto01);
                    }

                    break;

                case (int)Util.Util.TipoModelo.TextoNúmero:


                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
                    {
                        modeloTextoNumero.CarregarTextos(fraseGUI.Modelo.Textos[1].LabelTexto, fraseGUI.Modelo.Textos[0].LabelTexto);
                        modeloTextoNumero.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoNúmero;
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                        modeloTextoNumero.Visible = true;
                        modeloTextoNumero.CarregarTextos(roteiroNumero.ToString(), labelTexto01);
                    }

                    break;

                case (int)Util.Util.TipoModelo.TextoDuplo:


                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
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

                case (int)Util.Util.TipoModelo.NúmeroTextoDuplo:

                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
                    {
                        modeloNumeroTextoDuplo.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto, fraseGUI.Modelo.Textos[1].LabelTexto, fraseGUI.Modelo.Textos[2].LabelTexto);
                        modeloNumeroTextoDuplo.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTextoDuplo;
                        fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                        modeloNumeroTextoDuplo.Visible = true;
                        modeloNumeroTextoDuplo.CarregarTextos(roteiroNumero.ToString(), labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"));
                    }

                    break;

                case (int)Util.Util.TipoModelo.TextoDuploNúmero:

                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
                    {
                        modeloTextoDuploNumero.CarregarTextos(fraseGUI.Modelo.Textos[1].LabelTexto, fraseGUI.Modelo.Textos[0].LabelTexto, fraseGUI.Modelo.Textos[2].LabelTexto);
                        modeloTextoDuploNumero.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuploNúmero;
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                        modeloTextoDuploNumero.Visible = true;
                        modeloTextoDuploNumero.CarregarTextos(roteiroNumero.ToString(), labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"));
                    }

                    break;

                case (int)Util.Util.TipoModelo.TextoDuploTextoDuplo:

                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
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

                case (int)Util.Util.TipoModelo.TextoTriplo:

                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
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

                case (int)Util.Util.TipoModelo.NumeroTextoTriplo:

                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
                    {
                        modeloNumeroTextoTriplo.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto, fraseGUI.Modelo.Textos[1].LabelTexto, fraseGUI.Modelo.Textos[2].LabelTexto, fraseGUI.Modelo.Textos[3].LabelTexto);
                        modeloNumeroTextoTriplo.Visible = true;
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.NumeroTextoTriplo;
                        fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                        modeloNumeroTextoTriplo.Visible = true;
                        modeloNumeroTextoTriplo.CarregarTextos(roteiroNumero.ToString(), labelTexto01, rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"));
                    }

                    break;

                case (int)Util.Util.TipoModelo.TextoTriploNumero:
                    if (cbModelo.SelectedIndex == (int)fraseGUI.Modelo.TipoModelo)
                    {
                        modeloTextoTriploNumero.CarregarTextos(fraseGUI.Modelo.Textos[0].LabelTexto, fraseGUI.Modelo.Textos[1].LabelTexto, fraseGUI.Modelo.Textos[2].LabelTexto, fraseGUI.Modelo.Textos[3].LabelTexto);
                        modeloTextoTriploNumero.Visible = true;                        
                    }
                    else
                    {
                        fraseGUI.Modelo.Textos.Clear();
                        fraseGUI.Modelo.TipoModelo = Util.Util.TipoModelo.TextoTriploNumero;
                        fraseGUI.Modelo.Textos.Add(new Texto(labelTexto01));
                        fraseGUI.Modelo.Textos.Add(new Texto(roteiroNumero.ToString()));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_02")));
                        fraseGUI.Modelo.Textos.Add(new Texto(rm.GetString("USER_CONTROL_MODELOS_TEXTO_03")));
                        modeloTextoTriploNumero.Visible = true;
                        modeloTextoTriploNumero.CarregarTextos(labelTexto01, roteiroNumero.ToString(), rm.GetString("USER_CONTROL_MODELOS_TEXTO_02"), rm.GetString("USER_CONTROL_MODELOS_TEXTO_03"));
                    }
                    break;
            }

            //Mesmo não selecionando um item no user control dos textos, selecionar o item 0 como default
            this.textoSelecionado = 0;
        }

        private void comboModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (((ToolStripComboBox)sender).Focused)
            {
                int modelo_anterior = (int)fraseGUI.Modelo.TipoModelo;

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

                    DesenharTextoPainel();

                }              
            }
           
        }

        private void PopularApresentacao()
        {
            cbApresentacao.Items.Clear();

            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                cbApresentacao.Items.AddRange(fachada.CarregarApresentacao().ToArray());
            else
                cbApresentacao.Items.AddRange(fachada.CarregarApresentacaoMultiLinhas().ToArray());

        }

        public void RecarregarApresentacao()
        {
            int indice = cbApresentacao.SelectedIndex;
            cbApresentacao.Items.Clear();

            if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                cbApresentacao.Items.AddRange(fachada.CarregarApresentacao().ToArray());
            else
                cbApresentacao.Items.AddRange(fachada.CarregarApresentacaoMultiLinhas().ToArray());

            cbApresentacao.SelectedIndex = indice;

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
                    DesenharTextoPainel();
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
                    if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
                    {
                        for (int i = 0; i < fraseGUI.Modelo.Textos.Count; i++)
                            fraseGUI.Modelo.Textos[i].TempoRolagem = System.Convert.ToInt16(tbTempoRolagem.Text);
                    }
                    else
                        fraseGUI.Modelo.Textos[textoSelecionado].TempoRolagem = System.Convert.ToInt16(tbTempoRolagem.Text);

                    //Setando desenho do painel
                    DesenharTextoPainel();
                }
            }
        }

        private void cbFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[textoSelecionado].Fonte = cbFonte.SelectedItem.ToString();

                //Setando desenho do painel
                DesenharTextoPainel();
            }
        }

        private void cbApresentacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02) 
                { 
                    for(int i=0;i<fraseGUI.Modelo.Textos.Count;i++)
                        fraseGUI.Modelo.Textos[i].Apresentacao = (Util.Util.Rolagem)cbApresentacao.SelectedIndex;
                }
                else
                    fraseGUI.Modelo.Textos[textoSelecionado].Apresentacao = (Util.Util.Rolagem)cbApresentacao.SelectedIndex;

                //Setando desenho do painel
                DesenharTextoPainel();
            }
        }

        private void cbTamanhoFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                fraseGUI.Modelo.Textos[textoSelecionado].Altura = System.Convert.ToInt16(cbTamanhoFonte.SelectedItem.ToString());

                //Setando desenho do painel
                DesenharTextoPainel();
            }
        }

        private void TextosEditor_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                this.textoSelecionado = 0;
                fraseContinuar = null;
                CarregarTipoModelo();
            }
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

        private void cbMultinha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                textoSelecionado = cbMultinha.SelectedIndex;

                //alterando o texto do textbox
                modeloTexto.CarregarTextos(fraseGUI.Modelo.Textos[textoSelecionado].LabelTexto);

                //carregar na gui as propriedades do objeto texto selecionado
                CarregarGUITextoSelecionado();

                //Setando desenho do painel
                DesenharTextoPainel();


            }
        }
        #endregion


    }
}
