using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using Controlador;
using Persistencia;
using System.Resources;
using System.Globalization;
using System.Threading;

namespace PontosX2
{
    public partial class FormFontEditor : Form
    {
        #region proprieadades e atributos

        //Acesso à fachada
        Fachada fachada = Fachada.Instance;
        ResourceManager rm;
        VideoBitmap.VideoBitmap video = new VideoBitmap.VideoBitmap();
        //Arquivo_FNT fonte = new Arquivo_FNT();
        List<Frase> caracteres = new List<Frase>();
        Arquivo_FNT arquivoFonte = new Arquivo_FNT();
        Frase caractereCopia;

        string TituloForm;

        int celulaSelecionada;

        bool isEdicao = false;
        bool verificarAltura = true;
        bool verificarLargura = true;

        private int ALTURA_VALOR_MIN = 5;
        private int ALTURA_VALOR_MAX = 26;

        private int LARGURA_VALOR_MIN = 5;
        private int LARGURA_VALOR_MAX = 32;

        private int LARGURA_VALOR_MIN_CARACTERE = 1;
        private int LARGURA_VALOR_MAX_CARACTERE = 32;

        #endregion

        #region Construtor
        public FormFontEditor()
        {         

            //Globalização
            rm = fachada.carregaIdioma();

            InitializeComponent();

            gboxEditor.Visible = false;

            this.video.MouseDown += new System.Windows.Forms.MouseEventHandler(this.video_MouseDown);
            this.video.MouseMove += new System.Windows.Forms.MouseEventHandler(this.video_MouseMove);
            
        }

        #endregion

        #region Globalização

        private void AplicaIdioma()
        {
            if (isEdicao)
                TituloForm = rm.GetString("EDITOR_FONTES_FORM_TEXT") + " - " + System.Text.Encoding.ASCII.GetString(arquivoFonte.nomeFonte);
            else
                TituloForm = rm.GetString("EDITOR_FONTES_FORM_TEXT") + " - " + rm.GetString("EDITOR_FONTES_NOVA_FONTE");

            this.Text = TituloForm;

            newToolStripButton.ToolTipText = rm.GetString("EDITOR_FONTES_BT_NEW");
            openToolStripButton.ToolTipText = rm.GetString("EDITOR_FONTES_BT_OPEN");
            saveToolStripButton.ToolTipText = rm.GetString("EDITOR_FONTES_BT_SAVE");

            copytoolStripButton.ToolTipText = rm.GetString("EDITOR_FONTES_BT_COPY");
            pastetoolStripButton.ToolTipText = rm.GetString("EDITOR_FONTES_BT_PASTE");

            gboxEditor.Text = rm.GetString("EDITOR_FONTES_GBOX_FONTE");

            gboxInformacoes.Text = rm.GetString("EDITOR_FONTES_GBOX_INFORMACOES");
            lblNomeFonte.Text = rm.GetString("EDITOR_FONTES_LABEL_NOME");
            lblAltura.Text = rm.GetString("EDITOR_FONTES_LABEL_ALTURA");
            lblLarguraMax.Text = rm.GetString("EDITOR_FONTES_LABEL_LARGURA");

            tboxNome.Location = new Point(lblNomeFonte.Location.X + lblNomeFonte.Size.Width, tboxNome.Location.Y);
            tboxAltura.Location = new Point(lblAltura.Location.X + lblAltura.Size.Width, tboxAltura.Location.Y);
            tboxLarguraMax.Location = new Point(lblLarguraMax.Location.X + lblLarguraMax.Size.Width, tboxLarguraMax.Location.Y);

            gboxEditarCaractere.Text = rm.GetString("EDITOR_FONTES_GBOX_CARACTERE");
            lblAsc.Text = rm.GetString("EDITOR_FONTES_LABEL_ASC");
            lblLarguraCaractere.Text = rm.GetString("EDITOR_FONTES_LARGURA_CARACTERE");
            lblPintar.Text = rm.GetString("EDITOR_FONTES_LABEL_PINTAR");
            btAcender.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ACENDER");
            btApagar.ToolTipText = rm.GetString("EDITOR_FONTES_BT_APAGAR");
            btInverter.ToolTipText = rm.GetString("EDITOR_FONTES_BT_INVERTER");
            btAcenderTodos.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ACENDER_TODOS");
            btApagarTodos.ToolTipText = rm.GetString("EDITOR_FONTES_BT_APAGAR_TODOS");
            btInverterTodos.ToolTipText = rm.GetString("EDITOR_FONTES_BT_INVERTER_TODOS");            
            lblAplicarTodos.Text = rm.GetString("EDITOR_FONTES_LABEL_APLICAR_TODOS");
            btAplicarTodos.ToolTipText = rm.GetString("EDITOR_FONTES_LABEL_APLICAR_TODOS");

            labelFonte.Text = rm.GetString("EDITOR_FONTES_LABEL_FONTE");
            labelTamanho.Text = rm.GetString("EDITOR_FONTES_LABEL_FONTE_TAMANHO");
            btNegrito.ToolTipText = rm.GetString("EDITOR_FONTES_BT_NEGRITO");
            btItalico.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ITALICO");
            btSublinhado.ToolTipText = rm.GetString("EDITOR_FONTES_BT_SUBLINHADO");
            btNegrito.Text = rm.GetString("EDITOR_FONTES_BT_NEGRITO_TEXT");
            btItalico.Text = rm.GetString("EDITOR_FONTES_BT_ITALICO_TEXT");
            btSublinhado.Text = rm.GetString("EDITOR_FONTES_BT_SUBLINHADO_TEXT");

            btAlinharEsquerda.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ALINHAR_ESQUERDA");
            btAlinharMeio.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ALINHAR_MEIO");
            btAlinharDireita.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ALINHAR_DIREITA");
            btAlinharAbaixo.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ALINHAR_ABAIXO");
            btAlinharCentro.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ALINHAR_CENTRO");
            btAlinharAcima.ToolTipText = rm.GetString("EDITOR_FONTES_BT_ALINHAR_ACIMA");
            lbNivelSuavizacao.Text = rm.GetString("EDITOR_FONTES_LABEL_SUAVIZACAO");

            gboxPainel.Text = rm.GetString("EDITOR_FONTES_GBOX_PAINEL");
            gboxCaracteres.Text = rm.GetString("EDITOR_FONTES_GBOX_CARACTERES");
        }

        #endregion

        #region Botoes

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                if (!fachada.ExisteFonte(tboxNome.Text))
                    salvarFonte();
                else
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_SOBRESCREVER_FONTE"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        salvarFonte();
                }
            }
        }

        private bool Validar()
        {
            bool validou = true;

            if (tboxNome.Text.Length > 40)
            {
                validou = false;
                MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_TAMANHO_MAX_NOME"));
                tboxNome.Focus();
            }

            if (tboxNome.Text.Length <= 0)
            {
                validou = false;
                MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_NOME_VAZIO"));
                tboxNome.Focus();
            }

            try
            {
                int altura = Convert.ToInt16(tboxAltura.Text);

                if (altura < ALTURA_VALOR_MIN || altura > ALTURA_VALOR_MAX)
                {
                    validou = false;
                    verificarAltura = false;
                    MessageBox.Show(mensagemAltura());
                    tboxAltura.Text = caracteres[0].Modelo.Textos[0].AlturaPainel.ToString();
                    tboxAltura.Focus();
                }

            }
            catch
            {
                validou = false;
                verificarAltura = false;
                MessageBox.Show(mensagemAltura(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tboxAltura.Text = caracteres[0].Modelo.Textos[0].AlturaPainel.ToString();
                tboxAltura.Focus();
            }

            try
            {
                int largura = Convert.ToInt16(tboxLarguraMax.Text);

                if ((largura < LARGURA_VALOR_MIN || largura > LARGURA_VALOR_MAX) && largura != 0)
                {
                    validou = false;
                    verificarLargura = false;
                    MessageBox.Show(mensagemLargura(false));
                    if (caracteres[0].Modelo.Textos[0].LarguraPainel == caracteres[20].Modelo.Textos[0].LarguraPainel &&
                        caracteres[35].Modelo.Textos[0].LarguraPainel == caracteres[55].Modelo.Textos[0].LarguraPainel &&
                        caracteres[60].Modelo.Textos[0].LarguraPainel == caracteres[80].Modelo.Textos[0].LarguraPainel)
                        tboxLarguraMax.Text = caracteres[0].Modelo.Textos[0].LarguraPainel.ToString();
                    else
                        tboxLarguraMax.Text = "0";
                    tboxLarguraMax.Focus();
                }
            }
            catch
            {
                validou = false;
                verificarLargura = false;
                MessageBox.Show(mensagemLargura(false), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (caracteres[0].Modelo.Textos[0].LarguraPainel == caracteres[20].Modelo.Textos[0].LarguraPainel &&
                    caracteres[35].Modelo.Textos[0].LarguraPainel == caracteres[55].Modelo.Textos[0].LarguraPainel &&
                    caracteres[60].Modelo.Textos[0].LarguraPainel == caracteres[80].Modelo.Textos[0].LarguraPainel)
                    tboxLarguraMax.Text = caracteres[0].Modelo.Textos[0].LarguraPainel.ToString();
                else
                    tboxLarguraMax.Text = "0";
                tboxLarguraMax.Focus();
            }

            return validou;
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            CriarFonte();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            FormFontEditorOpen formAbriFonte = new FormFontEditorOpen();
            
            if (formAbriFonte.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CursorWait();

                    EditarFonte(formAbriFonte.caminhoFonte);

                    CursorDefault();
                }
                catch
                {
                    CursorDefault();
                    MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_FORMATO_INVALIDO_FNT"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btAcender_Click(object sender, EventArgs e)
        {
            if (btAcender.Checked)
            {
                btAcender.Checked = false;
            }
            else
            {
                btAcender.Checked = true;
                btApagar.Checked = false;
                btInverter.Checked = false;
            }
        }

        private void btApagar_Click(object sender, EventArgs e)
        {
            if (btApagar.Checked)
            {
                btApagar.Checked = false;
            }
            else
            {
                btAcender.Checked = false;
                btApagar.Checked = true;
                btInverter.Checked = false;
            }
        }

        private void btInverter_Click(object sender, EventArgs e)
        {
            if (btInverter.Checked)
            {
                btInverter.Checked = false;
            }
            else
            {
                btAcender.Checked = false;
                btApagar.Checked = false;
                btInverter.Checked = true;
            }
        }

        private void btAcenderTodos_Click(object sender, EventArgs e)
        {
            video.Apaga(0, Color.Yellow);
            caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0] = (Color[,])video.Bitmap.Clone();
            video.Desenhar();
            video.Refresh();
        }

        private void btApagarTodos_Click(object sender, EventArgs e)
        {
            video.Apaga(0, Color.Black);
            caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0] = (Color[,])video.Bitmap.Clone();
            video.Desenhar();
            video.Refresh();
        }

        private void btInverterTodos_Click(object sender, EventArgs e)
        {
            video.InverterLEDBitMap();
            caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0] = (Color[,])video.Bitmap.Clone();
            video.Desenhar();
            video.Refresh();
        }

        private void btAplicarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                CursorWait();

                AplicarTodasFrases();
                carregarCelulaSelecionada();

                CursorDefault();
            }
            catch 
            {
                CursorDefault();
                MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_ERRO_APLICAR_CARACTERES"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btNegrito_Click(object sender, EventArgs e)
        {
            if (caracteres[celulaSelecionada].Modelo.Textos[0].Negrito)
                caracteres[celulaSelecionada].Modelo.Textos[0].Negrito = false;
            else
                caracteres[celulaSelecionada].Modelo.Textos[0].Negrito = true;

            ChecarBotoes();

            if (Convert.ToInt16(tboxLarguraMax.Text) == 0)
            {
                caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel = 0;
                caracteres[celulaSelecionada].Modelo.Textos[0].Largura = 0;
            }

            fachada.PreparaBitMapCaractere(caracteres[celulaSelecionada]);

            montarVideoBitMap();

            carregarCelulaSelecionada();
        }

        private void ChecarBotoes()
        {
            switch (caracteres[celulaSelecionada].Modelo.Textos[0].AlinhamentoH)
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

            switch (caracteres[celulaSelecionada].Modelo.Textos[0].AlinhamentoV)
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


            if (caracteres[celulaSelecionada].Modelo.Textos[0].Italico)
                btItalico.Checked = true;
            else
                btItalico.Checked = false;

            if (caracteres[celulaSelecionada].Modelo.Textos[0].Negrito)
                btNegrito.Checked = true;
            else
                btNegrito.Checked = false;

            if (caracteres[celulaSelecionada].Modelo.Textos[0].Sublinhado)
                btSublinhado.Checked = true;
            else
                btSublinhado.Checked = false;

        }

        private void btItalico_Click(object sender, EventArgs e)
        {
            if (caracteres[celulaSelecionada].Modelo.Textos[0].Italico)
                caracteres[celulaSelecionada].Modelo.Textos[0].Italico = false;
            else
                caracteres[celulaSelecionada].Modelo.Textos[0].Italico = true;

            ChecarBotoes();

            if (Convert.ToInt16(tboxLarguraMax.Text) == 0)
            {
                caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel = 0;
                caracteres[celulaSelecionada].Modelo.Textos[0].Largura = 0;
            }

            fachada.PreparaBitMapCaractere(caracteres[celulaSelecionada]);
            montarVideoBitMap();

            carregarCelulaSelecionada();
        }

        private void btSublinhado_Click(object sender, EventArgs e)
        {
            if (caracteres[celulaSelecionada].Modelo.Textos[0].Sublinhado)
                caracteres[celulaSelecionada].Modelo.Textos[0].Sublinhado = false;
            else
                caracteres[celulaSelecionada].Modelo.Textos[0].Sublinhado = true;

            ChecarBotoes();

            if (Convert.ToInt16(tboxLarguraMax.Text) == 0)
            {
                caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel = 0;
                caracteres[celulaSelecionada].Modelo.Textos[0].Largura = 0;
            }

            fachada.PreparaBitMapCaractere(caracteres[celulaSelecionada]);
            montarVideoBitMap();

            carregarCelulaSelecionada();
        }

        private void btAlinharEsquerda_Click(object sender, EventArgs e)
        {
            caracteres[celulaSelecionada].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

            ChecarBotoes();

            video.listaBitMap = caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap;

            int primeiraColuna = video.GetPrimeiroPixelLarguraIndice(0, Color.Black);
            int ultimaColuna = video.GetUltimoPixelLarguraIndice(0, Color.Black);

            int primeiraLinha = video.GetPrimeiroPixelAlturaIndice(0, Color.Black);
            int ultimaLinha = video.GetUltimoPixelAlturaIndice(0, Color.Black);

            Color[,] imagemCortada = video.GetPreencherBitMap(primeiraLinha, ultimaLinha, primeiraColuna, ultimaColuna, 0);
            
            fachada.AplicarAlinhamento(caracteres[celulaSelecionada], imagemCortada);
            video.Bitmap = (Color[,])caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0].Clone();
        }

        private void log(List<String> texto)
        {
            //return;
            System.IO.File.Delete(@"C:\Users\joao.cavalcanti\Desktop\log.txt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\joao.cavalcanti\Desktop\log.txt", true))
            {
                foreach (String linha in texto)
                {
                    file.WriteLine(linha);
                }

            }
        }

        private void btAlinharMeio_Click(object sender, EventArgs e)
        {
            caracteres[celulaSelecionada].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;

            ChecarBotoes();

            video.listaBitMap = caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap;

            int primeiraColuna = video.GetPrimeiroPixelLarguraIndice(0, Color.Black);
            int ultimaColuna = video.GetUltimoPixelLarguraIndice(0, Color.Black);

            int primeiraLinha = video.GetPrimeiroPixelAlturaIndice(0, Color.Black);
            int ultimaLinha = video.GetUltimoPixelAlturaIndice(0, Color.Black);

            Color[,] imagemCortada = video.GetPreencherBitMap(primeiraLinha, ultimaLinha, primeiraColuna, ultimaColuna, 0);

            fachada.AplicarAlinhamento(caracteres[celulaSelecionada], imagemCortada);
            video.Bitmap = (Color[,])caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0].Clone();
        }

        private void btAlinharDireita_Click(object sender, EventArgs e)
        {
            caracteres[celulaSelecionada].Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Direita;

            ChecarBotoes();
            video.listaBitMap = caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap;

            int primeiraColuna = video.GetPrimeiroPixelLarguraIndice(0, Color.Black);
            int ultimaColuna = video.GetUltimoPixelLarguraIndice(0, Color.Black);

            int primeiraLinha = video.GetPrimeiroPixelAlturaIndice(0, Color.Black);
            int ultimaLinha = video.GetUltimoPixelAlturaIndice(0, Color.Black);

            Color[,] imagemCortada = video.GetPreencherBitMap(primeiraLinha, ultimaLinha, primeiraColuna, ultimaColuna, 0);

            fachada.AplicarAlinhamento(caracteres[celulaSelecionada], imagemCortada);
            video.Bitmap = (Color[,])caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0].Clone();
        }

        private void btAlinharAbaixo_Click(object sender, EventArgs e)
        {
            caracteres[celulaSelecionada].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;

            ChecarBotoes();
            video.listaBitMap = caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap;

            int primeiraColuna = video.GetPrimeiroPixelLarguraIndice(0, Color.Black);
            int ultimaColuna = video.GetUltimoPixelLarguraIndice(0, Color.Black);

            int primeiraLinha = video.GetPrimeiroPixelAlturaIndice(0, Color.Black);
            int ultimaLinha = video.GetUltimoPixelAlturaIndice(0, Color.Black);

            Color[,] imagemCortada = video.GetPreencherBitMap(primeiraLinha, ultimaLinha, primeiraColuna, ultimaColuna, 0);

            fachada.AplicarAlinhamento(caracteres[celulaSelecionada], imagemCortada);
            video.Bitmap = (Color[,])caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0].Clone();
        }

        private void btAlinharCentro_Click(object sender, EventArgs e)
        {
            caracteres[celulaSelecionada].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Centro;

            ChecarBotoes();
            video.listaBitMap = caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap;

            int primeiraColuna = video.GetPrimeiroPixelLarguraIndice(0, Color.Black);
            int ultimaColuna = video.GetUltimoPixelLarguraIndice(0, Color.Black);

            int primeiraLinha = video.GetPrimeiroPixelAlturaIndice(0, Color.Black);
            int ultimaLinha = video.GetUltimoPixelAlturaIndice(0, Color.Black);

            Color[,] imagemCortada = video.GetPreencherBitMap(primeiraLinha, ultimaLinha, primeiraColuna, ultimaColuna, 0);

            fachada.AplicarAlinhamento(caracteres[celulaSelecionada], imagemCortada);

            video.Bitmap = (Color[,])caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0].Clone();
        }

        private void btAlinharAcima_Click(object sender, EventArgs e)
        {
            caracteres[celulaSelecionada].Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Cima;

            ChecarBotoes();
            video.listaBitMap = caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap;

            int primeiraColuna = video.GetPrimeiroPixelLarguraIndice(0, Color.Black);
            int ultimaColuna = video.GetUltimoPixelLarguraIndice(0, Color.Black);

            int primeiraLinha = video.GetPrimeiroPixelAlturaIndice(0, Color.Black);
            int ultimaLinha = video.GetUltimoPixelAlturaIndice(0, Color.Black);

            Color[,] imagemCortada = video.GetPreencherBitMap(primeiraLinha, ultimaLinha, primeiraColuna, ultimaColuna, 0);

            fachada.AplicarAlinhamento(caracteres[celulaSelecionada], imagemCortada);

            video.Bitmap = (Color[,])caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0].Clone();
        }
        #endregion

        #region Criar, Editar e Salvar Fonte

        private void salvarFonte()
        {
            StringBuilder sb = new StringBuilder();
            int tempo = Environment.TickCount;
            UInt32 acumulado = 0;
            try
            {
                CursorWait();

                //Preenchendo Objeto fonte
                arquivoFonte.NomeArquivo = tboxNome.Text;
                arquivoFonte.Altura = (uint)Convert.ToInt16(tboxAltura.Text);
                arquivoFonte.nomeFonte = Encoding.ASCII.GetBytes(arquivoFonte.NomeArquivo.PadRight(arquivoFonte.nomeFonte.Length, '\0'));
                arquivoFonte.bitmaps.Clear();
                
                    acumulado = Arquivo_FNT.TamanhoFormatoFonte();
                    for (int i = 0; i < caracteres.Count; i++)
                    {
                        Persistencia.Bitmap bitmapTemp = new Persistencia.Bitmap((uint)caracteres[i].Modelo.Textos[0].listaBitMap[0].GetLength(0), (uint)caracteres[i].Modelo.Textos[0].listaBitMap[0].GetLength(1));
                        bitmapTemp.matriz = caracteres[i].ConverterColorToBoolean(caracteres[i].Modelo.Textos[0].listaBitMap[0]);
                        arquivoFonte.bitmaps.Add(bitmapTemp);
                        //arquivoFonte.TamanhoBitmapAcumulado(i);

                        Persistencia.Arquivo_FNT.CaractereInfo ci = new Persistencia.Arquivo_FNT.CaractereInfo();
                        ci.codigo = (byte)(i + 32);
                        ci.enderecoBitmap = acumulado;

                        Byte[] pixelByte = bitmapTemp.FromMatrixToArray();
                        acumulado = (UInt32)(acumulado + pixelByte.Length + sizeof(uint));

                        arquivoFonte.caracteres[i] = ci;
                    }
                
               // MessageBox.Show(sb.ToString());
                //salvar objeto fonte
                fachada.SalvarFonte(arquivoFonte);

                EditarFonte(fachada.GetDiretorio("fontes") + "\\" + tboxNome.Text.Trim() + Util.Util.ARQUIVO_EXT_FNT);
                CursorDefault();
            }
            catch
            {
                CursorDefault();
                MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_ERRO_SALVAR"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CriarFonte()
        {
            isEdicao = false;            

            celulaSelecionada = 0;

            AplicaIdioma();

            popularMapaCaracteres();

            populaListaFrasesPadrao();

            popularComboTrueType();

            PopularNivelSuavizacao();

            popularCamposGUI();

            montarVideoBitMap();            

            gboxEditor.Visible = true;
        }

        private void EditarFonte(string fileName)
        {
            isEdicao = true;
            
            celulaSelecionada = 0;

            fachada.AbrirFonte(arquivoFonte, fileName);

            AplicaIdioma();
            
            popularMapaCaracteres();            

            populaListaFrases();

            popularComboTrueType();

            PopularNivelSuavizacao();

            popularCamposGUI();

            montarVideoBitMap();

            gboxEditor.Visible = true;
        }

        #endregion

        #region Popular Componentes

        private void FormFontEditor_Load(object sender, EventArgs e)
        {
            try
            {
                CursorWait();

                CriarFonte();

                CursorDefault();
            }
            catch
            {
                CursorDefault();
                MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_ERRO_CRIAR"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void CursorWait()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.UseWaitCursor = true;
        }

        private void CursorDefault()
        {
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }



        private void PreencherListaFraseComObjetoFonte()
        {
            caracteres.Clear();
        }

        private void PopularNivelSuavizacao()
        {
            cbNivelSuavizacao.Items.Clear();
            for (int i = 0; i < 50; i++)
                cbNivelSuavizacao.Items.Add(i * 5);
        }

        private void popularCamposGUI()
        {
            if (isEdicao)
                tboxNome.Text = System.Text.Encoding.ASCII.GetString(arquivoFonte.nomeFonte);
            else
                tboxNome.Text = rm.GetString("EDITOR_FONTES_NOVA_FONTE");

            tboxAltura.Text = caracteres[celulaSelecionada].Modelo.Textos[0].AlturaPainel.ToString();
            tboxLarguraMax.Text = "0";
            tboxLargCaractere.Text = caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel.ToString();

            cboxFonte.SelectedIndex = cboxFonte.Items.IndexOf(caracteres[celulaSelecionada].Modelo.Textos[0].Fonte);
            cboxTamanho.SelectedIndex = cboxTamanho.Items.IndexOf(caracteres[celulaSelecionada].Modelo.Textos[0].Altura.ToString());
            cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(caracteres[celulaSelecionada].Modelo.Textos[0].BinaryThreshold);

            ChecarBotoes();

            dbgCaracteres.Rows[0].Cells[0].Selected = true;
        }

        private void popularCamposCelulaGUI()
        {

            tboxLargCaractere.Text = caracteres[celulaSelecionada].Modelo.Textos[0].Largura.ToString();

            cboxFonte.SelectedIndex = cboxFonte.Items.IndexOf(caracteres[celulaSelecionada].Modelo.Textos[0].Fonte);
            cboxTamanho.SelectedIndex = cboxTamanho.Items.IndexOf(caracteres[celulaSelecionada].Modelo.Textos[0].Altura.ToString());
            cbNivelSuavizacao.SelectedIndex = cbNivelSuavizacao.Items.IndexOf(caracteres[celulaSelecionada].Modelo.Textos[0].BinaryThreshold);

            ChecarBotoes();
        }

        private void populaListaFrasesPadrao()
        {

            caracteres.Clear();
            for (int i = 0; i < dbgCaracteres.RowCount; i++)
            {
                for (int j = 0; j < dbgCaracteres.ColumnCount; j++)
                {
                    if (dbgCaracteres.Rows[i].Cells[j].Value != null)
                    {
                        Frase f = new Frase(dbgCaracteres.Rows[i].Cells[j].Value.ToString());
                        f.Modelo.Textos[0].Altura = Convert.ToInt16(Util.Util.ALTURA_FONTE_DEFAULT_WINDOWS);
                        f.Modelo.Textos[0].AlturaPainel = Convert.ToInt16(Util.Util.ALTURA_FONTE_DEFAULT_WINDOWS); 
                        f.Modelo.Textos[0].Largura = 0;
                        f.Modelo.Textos[0].LarguraPainel = 0;
                        f.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                        f.Modelo.Textos[0].AlinhamentoV = retornaAlinhamentoCaractere(f.LabelFrase);
                        f.Modelo.Textos[0].FonteWindows = true;
                        f.Modelo.Textos[0].Fonte = Util.Util.FONTE_DEFAULT_WINDOWS;
                        f.Modelo.Textos[0].BinaryThreshold = 200;


                        fachada.PreparaBitMapCaractere(f);
                        caracteres.Add(f);
                    }
                    else
                        break;
                }
            }
        }
        
        private void populaListaFrases()
        {
            caracteres.Clear();

            for (int i = 0; i < arquivoFonte.caracteres.Count;i++ )
            {
                Frase f = new Frase(retornaCharCode(Convert.ToInt16(arquivoFonte.caracteres[i].codigo)).ToString());

                //atribui ao bitmap da frase a matriz booleana convertida para color
                fachada.ConverterMatrizBooleanToColor(f,arquivoFonte.bitmaps[i].matriz,0);

                f.Modelo.Textos[0].Altura = f.Modelo.Textos[0].listaBitMap[0].GetLength(0);
                f.Modelo.Textos[0].AlturaPainel = f.Modelo.Textos[0].listaBitMap[0].GetLength(0);
                f.Modelo.Textos[0].Largura = f.Modelo.Textos[0].listaBitMap[0].GetLength(1);
                f.Modelo.Textos[0].LarguraPainel = f.Modelo.Textos[0].listaBitMap[0].GetLength(1); 
                f.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                f.Modelo.Textos[0].AlinhamentoV = retornaAlinhamentoCaractere(f.LabelFrase);
                f.Modelo.Textos[0].FonteWindows = true;
                f.Modelo.Textos[0].Fonte = Util.Util.FONTE_DEFAULT_WINDOWS;
                f.Modelo.Textos[0].BinaryThreshold = 200;
                caracteres.Add(f);
            }
        }

        private Util.Util.AlinhamentoVertical retornaAlinhamentoCaractere(string caractere)
        {
            Util.Util.AlinhamentoVertical retorno = Util.Util.AlinhamentoVertical.Baixo;

            switch (caractere)
            {
                //Caracteres com alinhamento em cima
                case "\"": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "\'": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "*": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "^": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "`": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "~": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "ˆ": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "‘": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "’": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "“": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "”": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "˜": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "™": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "¨": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "©": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "ª": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "®": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "¯": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "°": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "³": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "²": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "´": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "¹": retorno = Util.Util.AlinhamentoVertical.Cima; break;
                case "º": retorno = Util.Util.AlinhamentoVertical.Cima; break;

                //caracteres com alinhamento no centro
                case "+": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "-": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case ":": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case ";": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "<": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "=": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case ">": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "‹": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "•": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "–": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "—": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "›": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "¤": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "»": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "·": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "±": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "¬": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "«": retorno = Util.Util.AlinhamentoVertical.Centro; break;
                case "÷": retorno = Util.Util.AlinhamentoVertical.Centro; break;
            }

            return retorno;
        }

        private void AplicarTodasFrases()
        {
            Frase fTemp = new Frase(caracteres[celulaSelecionada]);

            int largura = Convert.ToInt16(tboxLarguraMax.Text);

            caracteres.Clear();
            for (int i = 0; i < dbgCaracteres.RowCount; i++)
            {
                for (int j = 0; j < dbgCaracteres.ColumnCount; j++)
                {
                    if (dbgCaracteres.Rows[i].Cells[j].Value != null)
                    {
                        Frase f = new Frase(fTemp);
                        f.LabelFrase = dbgCaracteres.Rows[i].Cells[j].Value.ToString();
                        f.Modelo.Textos[0].LabelTexto = dbgCaracteres.Rows[i].Cells[j].Value.ToString();

                        f.Modelo.Textos[0].Largura = largura;
                        f.Modelo.Textos[0].LarguraPainel = largura;
 
                        fachada.PreparaBitMapCaractere(f);
                        caracteres.Add(f);
                    }
                    else
                        break;
                }
            }
        }

        private void alterarListaFrasesAlturaLargura(int altura, int largura)
        {
            foreach(Frase f in caracteres)
            {
                f.Modelo.Textos[0].AlturaPainel = altura;
                f.Modelo.Textos[0].Altura = altura;
                f.Modelo.Textos[0].Largura = largura;
                f.Modelo.Textos[0].LarguraPainel = largura;
                f.PreparaBitMapCaractere();
            }
        }

        private void popularComboTrueType()
        {
            FontFamily sansSerifFont = FontFamily.GenericSansSerif;
            InstalledFontCollection insFont = new InstalledFontCollection();
            FontFamily[] families = insFont.Families;


            cboxFonte.Items.Clear();
            foreach (FontFamily family in families)
            {
                cboxFonte.Items.Add(family.Name);
            }

            cboxTamanho.Items.Clear();
            for (int i = ALTURA_VALOR_MIN; i <= ALTURA_VALOR_MAX; i++)
                cboxTamanho.Items.Add(i.ToString());

        }

        private char retornaCharCode(int caractere)
        {
            char letra = (char)caractere;
            if (caractere > 127 && caractere < 161)
            {
                switch (caractere)
                {
                    case 128: letra = Convert.ToChar(8364); break;
                    case 130: letra = Convert.ToChar(8218); break;
                    case 131: letra = Convert.ToChar(402); break;
                    case 132: letra = Convert.ToChar(8222); break;
                    case 133: letra = Convert.ToChar(8230); break;
                    case 134: letra = Convert.ToChar(8224); break;
                    case 135: letra = Convert.ToChar(8225); break;
                    case 136: letra = Convert.ToChar(710); break;
                    case 137: letra = Convert.ToChar(8240); break;
                    case 138: letra = Convert.ToChar(352); break;
                    case 139: letra = Convert.ToChar(8249); break;
                    case 140: letra = Convert.ToChar(338); break;
                    case 142: letra = Convert.ToChar(381); break;
                    case 145: letra = Convert.ToChar(8216); break;
                    case 146: letra = Convert.ToChar(8217); break;
                    case 147: letra = Convert.ToChar(8220); break;
                    case 148: letra = Convert.ToChar(8221); break;
                    case 149: letra = Convert.ToChar(8226); break;
                    case 150: letra = Convert.ToChar(8211); break;
                    case 151: letra = Convert.ToChar(8212); break;
                    case 152: letra = Convert.ToChar(732); break;
                    case 153: letra = Convert.ToChar(8482); break;
                    case 154: letra = Convert.ToChar(353); break;
                    case 155: letra = Convert.ToChar(8250); break;
                    case 156: letra = Convert.ToChar(339); break;
                    case 158: letra = Convert.ToChar(382); break;
                    case 159: letra = Convert.ToChar(376); break;
                }
            }

            return letra;
        }

        private void popularMapaCaracteres()
        {

            dbgCaracteres.Rows.Clear();

            int caractere = 32;
            for (int row = 0; row < 10 && caractere < 256; row++)
            {
                dbgCaracteres.Rows.Add();

                for (int colunas = 0; colunas < dbgCaracteres.Columns.Count && caractere < 256; colunas++)
                {
                    char letra = (char)caractere;
                    if (caractere > 127 && caractere < 161)
                    {
                        switch (caractere)
                        {
                            case 128: letra = Convert.ToChar(8364); break;
                            case 130: letra = Convert.ToChar(8218); break;
                            case 131: letra = Convert.ToChar(402); break;
                            case 132: letra = Convert.ToChar(8222); break;
                            case 133: letra = Convert.ToChar(8230); break;
                            case 134: letra = Convert.ToChar(8224); break;
                            case 135: letra = Convert.ToChar(8225); break;
                            case 136: letra = Convert.ToChar(710); break;
                            case 137: letra = Convert.ToChar(8240); break;
                            case 138: letra = Convert.ToChar(352); break;
                            case 139: letra = Convert.ToChar(8249); break;
                            case 140: letra = Convert.ToChar(338); break;
                            case 142: letra = Convert.ToChar(381); break;
                            case 145: letra = Convert.ToChar(8216); break;
                            case 146: letra = Convert.ToChar(8217); break;
                            case 147: letra = Convert.ToChar(8220); break;
                            case 148: letra = Convert.ToChar(8221); break;
                            case 149: letra = Convert.ToChar(8226); break;
                            case 150: letra = Convert.ToChar(8211); break;
                            case 151: letra = Convert.ToChar(8212); break;
                            case 152: letra = Convert.ToChar(732); break;
                            case 153: letra = Convert.ToChar(8482); break;
                            case 154: letra = Convert.ToChar(353); break;
                            case 155: letra = Convert.ToChar(8250); break;
                            case 156: letra = Convert.ToChar(339); break;
                            case 158: letra = Convert.ToChar(382); break;
                            case 159: letra = Convert.ToChar(376); break;
                        }
                    }

                    dbgCaracteres.Rows[row].Cells[colunas].Value = letra;
                    dbgCaracteres.Rows[row].Cells[colunas].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    caractere = caractere + 1;
                }
            }
        }

        private void montarVideoBitMap()
        {
            video.Altura = caracteres[celulaSelecionada].Modelo.Textos[0].AlturaPainel;
            video.Largura = caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel;
            video.DiametroLed = 10;
            video.Bitmap = (Color[,])caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0].Clone();
            gboxPainel.Controls.Add(video);
            video.Location = new Point(gboxPainel.Width / 2 - video.Size.Width / 2, gboxPainel.Height / 2 - ((video.Size.Height / 2) - 3));
        }

        #endregion

        #region Selecionando Componente na GUI

        private void video_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point point = new Point(e.X, e.Y);

                int coluna = point.X / (video.Size.Width / video.Largura);
                int linha = video.Altura - (point.Y / (video.Size.Height / video.Altura)) - 1;

                if (coluna >= 0 && coluna < video.Largura && linha >= 0 && linha < video.Altura)
                {
                    if (btAcender.Checked)
                    {
                        video.Bitmap[linha, coluna] = Color.Yellow;
                        caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0][linha, coluna] = Color.Yellow;
                        video.Desenhar();
                        video.Refresh();
                    }

                    if (btApagar.Checked)
                    {
                        video.Bitmap[linha, coluna] = Color.Black;
                        caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0][linha, coluna] = Color.Black;
                        video.Desenhar();
                        video.Refresh();
                    }
                }
            }
        }

        private void video_MouseDown(object sender, MouseEventArgs e)
        {

            Point point = new Point(e.X, e.Y);

            int coluna = point.X / (video.Size.Width / video.Largura);
            int linha = video.Altura - (point.Y / (video.Size.Height / video.Altura)) - 1;

            if (btAcender.Checked)
            {
                video.Bitmap[linha, coluna] = Color.Yellow;
                caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0][linha, coluna] = Color.Yellow;
                video.Desenhar();
                video.Refresh();
            }

            if (btApagar.Checked)
            {
                video.Bitmap[linha, coluna] = Color.Black;
                caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0][linha, coluna] = Color.Black;
                video.Desenhar();
                video.Refresh();
            }

            if (btInverter.Checked)
            {
                if (video.Bitmap[linha, coluna] == Color.Black)
                {
                    video.Bitmap[linha, coluna] = Color.Yellow;
                    caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0][linha, coluna] = Color.Yellow;
                }
                else
                {
                    video.Bitmap[linha, coluna] = Color.Black;
                    caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0][linha, coluna] = Color.Black;
                }

                video.Desenhar();
                video.Refresh();
            }
        }


        private void carregarCelulaSelecionada()
        {
            if (dbgCaracteres.CurrentCell.Value != null)
            {
                int valor = Convert.ToInt32(dbgCaracteres.CurrentCell.Value.ToString()[0]);
                switch (valor)
                {
                    case 8364: valor = 128; break;
                    case 8218: valor = 130; break;
                    case 402: valor = 131; break;
                    case 8222: valor = 132; break;
                    case 8230: valor = 133; break;
                    case 8224: valor = 134; break;
                    case 8225: valor = 135; break;
                    case 710: valor = 136; break;
                    case 8240: valor = 137; break;
                    case 352: valor = 138; break;
                    case 8249: valor = 139; break;
                    case 338: valor = 140; break;
                    case 381: valor = 142; break;
                    case 8216: valor = 145; break;
                    case 8217: valor = 146; break;
                    case 8220: valor = 147; break;
                    case 8221: valor = 148; break;
                    case 8226: valor = 149; break;
                    case 8211: valor = 150; break;
                    case 8212: valor = 151; break;
                    case 732: valor = 152; break;
                    case 8482: valor = 153; break;
                    case 353: valor = 154; break;
                    case 8250: valor = 155; break;
                    case 339: valor = 156; break;
                    case 382: valor = 158; break;
                    case 376: valor = 159; break;
                }

                lblValorCodAsc.Text = "Alt + 0" + valor.ToString();

                celulaSelecionada = dbgCaracteres.CurrentCell.ColumnIndex + (dbgCaracteres.CurrentCell.RowIndex * dbgCaracteres.Columns.Count);

                popularCamposCelulaGUI();

                montarVideoBitMap();

                //caractereCopia = new Frase(caracteres[celulaSelecionada]);
                copytoolStripButton.Enabled = true;
                pastetoolStripButton.Enabled = true;

            }
            else
            {
                //dbgCaracteres.CurrentCell.Selected = false;
                //copytoolStripButton.Enabled = false;
                //caractereCopia = null;
                copytoolStripButton.Enabled = false;
                pastetoolStripButton.Enabled = false;
                lblValorCodAsc.Text = "";
                video.Apaga(0, Color.Black);
                video.Desenhar();
                video.Refresh();
            }
        }

        private void tboxAltura_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tboxLarguraMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


        private void tboxAltura_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Focused && verificarAltura)
            {
                try
                {
                    if (tboxAltura.Text != "")
                    {
                        int altura = Convert.ToInt16(tboxAltura.Text);
                        int largura = Convert.ToInt16(tboxLarguraMax.Text);

                        if (altura >= ALTURA_VALOR_MIN && altura <= ALTURA_VALOR_MAX)
                        {
                            if (DialogResult.Yes == MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_MUDAR_ALTURA_CARACTERES"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            {
                                CursorWait();

                                alterarListaFrasesAlturaLargura(altura, largura);
                                montarVideoBitMap();

                                //Recarregar a GUI
                                carregarCelulaSelecionada();

                                caractereCopia = null;

                                CursorDefault();
                            }
                            else
                            {
                                verificarAltura = false;
                                tboxAltura.Text = caracteres[0].Modelo.Textos[0].AlturaPainel.ToString();
                            }
                        }
                    }
                }
                catch
                {
                    CursorDefault();
                    MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_NUMERO_VALIDO"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                verificarAltura = true;
        }

        private void tboxLarguraMax_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Focused && verificarLargura)
            {
                try
                {
                    if (tboxLarguraMax.Text != "")
                    {
                        int altura = Convert.ToInt16(tboxAltura.Text);
                        int largura = Convert.ToInt16(tboxLarguraMax.Text);

                        if ((largura >= LARGURA_VALOR_MIN && largura <= LARGURA_VALOR_MAX) || largura == 0)
                        {

                            string mensagem;
                            if (largura == 0)
                                mensagem = rm.GetString("EDITOR_FONTES_MBOX_MUDAR_LARGURA_DEFAULT");
                            else
                                mensagem = rm.GetString("EDITOR_FONTES_MBOX_MUDAR_LARGURA_TODOS_IGUAIS");

                            if (DialogResult.Yes == MessageBox.Show(mensagem, rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            {
                                CursorWait();

                                alterarListaFrasesAlturaLargura(altura, largura);
                                montarVideoBitMap();

                                //Recarregar a GUI
                                carregarCelulaSelecionada();

                                caractereCopia = null;

                                CursorDefault();
                            }
                            else
                            {
                                verificarLargura = false;
                                if (caracteres[0].Modelo.Textos[0].LarguraPainel == caracteres[20].Modelo.Textos[0].LarguraPainel &&
                                    caracteres[35].Modelo.Textos[0].LarguraPainel == caracteres[55].Modelo.Textos[0].LarguraPainel &&
                                    caracteres[60].Modelo.Textos[0].LarguraPainel == caracteres[80].Modelo.Textos[0].LarguraPainel)
                                    tboxLarguraMax.Text = caracteres[0].Modelo.Textos[0].LarguraPainel.ToString();
                                else
                                    tboxLarguraMax.Text = "0";
                            }
                        }
                    }
                }
                catch
                {
                    CursorDefault();
                    MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_NUMERO_VALIDO"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tboxLarguraMax.Focus();
                }
            }
            else
                verificarLargura = true;
        }

        private void tboxAltura_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int altura = Convert.ToInt16(tboxAltura.Text);

                if (altura < ALTURA_VALOR_MIN || altura > ALTURA_VALOR_MAX)
                {
                    MessageBox.Show(mensagemAltura());
                    tboxAltura.Text = caracteres[0].Modelo.Textos[0].AlturaPainel.ToString();
                    tboxAltura.Focus();
                }

            }
            catch
            {
                MessageBox.Show(mensagemAltura(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tboxAltura.Text = caracteres[0].Modelo.Textos[0].AlturaPainel.ToString();
                tboxAltura.Focus();
            }
        }

        private string mensagemAltura()
        {
            string mensagem = rm.GetString("EDITOR_FONTES_MBOX_ALTURA_TAMANHO");
            switch(fachada.GetIdiomaFachada())
            {
                case Util.Util.Lingua.Ingles: mensagem = mensagem + " " + ALTURA_VALOR_MIN.ToString() + " and " + ALTURA_VALOR_MAX.ToString() + "!"; break;
                case Util.Util.Lingua.Espanhol: mensagem = mensagem + " " + ALTURA_VALOR_MIN.ToString() + " y " + ALTURA_VALOR_MAX.ToString() + "!"; break;
                case Util.Util.Lingua.Portugues: mensagem = mensagem + " " + ALTURA_VALOR_MIN.ToString() + " e " + ALTURA_VALOR_MAX.ToString() + "!"; break;
            }

            return mensagem;
        }

        private string mensagemLargura(bool caractere)
        {
            string mensagem = rm.GetString("EDITOR_FONTES_MBOX_LARGURA_TAMANHO");
            switch (fachada.GetIdiomaFachada())
            {
                case Util.Util.Lingua.Ingles: mensagem = mensagem + " " + (caractere? LARGURA_VALOR_MIN_CARACTERE.ToString():LARGURA_VALOR_MIN.ToString()) + " and " + (caractere ? LARGURA_VALOR_MAX_CARACTERE.ToString(): LARGURA_VALOR_MAX.ToString()) + "!"; break;
                case Util.Util.Lingua.Espanhol: mensagem = mensagem + " " + (caractere ? LARGURA_VALOR_MIN_CARACTERE.ToString() : LARGURA_VALOR_MIN.ToString()) + " y " + (caractere ? LARGURA_VALOR_MAX_CARACTERE.ToString() : LARGURA_VALOR_MAX.ToString()) + "!"; break;
                case Util.Util.Lingua.Portugues: mensagem = mensagem + " " + (caractere ? LARGURA_VALOR_MIN_CARACTERE.ToString() : LARGURA_VALOR_MIN.ToString()) + " e " + (caractere ? LARGURA_VALOR_MAX_CARACTERE.ToString() : LARGURA_VALOR_MAX.ToString()) + "!"; break;
            }

            return mensagem;
        }

        private void tboxLarguraMax_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int largura = Convert.ToInt16(tboxLarguraMax.Text);

                if ((largura < LARGURA_VALOR_MIN || largura > LARGURA_VALOR_MAX) && largura!=0)
                {
                    MessageBox.Show(mensagemLargura(false));
                    if (caracteres[0].Modelo.Textos[0].LarguraPainel == caracteres[20].Modelo.Textos[0].LarguraPainel &&
                        caracteres[35].Modelo.Textos[0].LarguraPainel == caracteres[55].Modelo.Textos[0].LarguraPainel &&
                        caracteres[60].Modelo.Textos[0].LarguraPainel == caracteres[80].Modelo.Textos[0].LarguraPainel)
                        tboxLarguraMax.Text = caracteres[0].Modelo.Textos[0].LarguraPainel.ToString();
                    else
                        tboxLarguraMax.Text = "0";
                    tboxLarguraMax.Focus();
                }
            }
            catch
            {
                MessageBox.Show(mensagemLargura(false), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (caracteres[0].Modelo.Textos[0].LarguraPainel == caracteres[20].Modelo.Textos[0].LarguraPainel &&
                    caracteres[35].Modelo.Textos[0].LarguraPainel == caracteres[55].Modelo.Textos[0].LarguraPainel &&
                    caracteres[60].Modelo.Textos[0].LarguraPainel == caracteres[80].Modelo.Textos[0].LarguraPainel)
                    tboxLarguraMax.Text = caracteres[0].Modelo.Textos[0].LarguraPainel.ToString();
                else
                    tboxLarguraMax.Text = "0";
                tboxLarguraMax.Focus();
            }
        }

        private void tboxLargCaractere_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tboxLargCaractere_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int larCaractere = Convert.ToInt16(tboxLargCaractere.Text);
                int larMaxima = (Convert.ToInt16(tboxLarguraMax.Text)==0?LARGURA_VALOR_MAX_CARACTERE:Convert.ToInt16(tboxLarguraMax.Text));


                if (larCaractere > larMaxima)
                {
                    if (Convert.ToInt16(tboxLarguraMax.Text)==0)
                        MessageBox.Show(mensagemLargura(true));
                    else
                        MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_LARGURA_CARACTERE_FONTE"));

                    tboxLargCaractere.Focus();
                }
                else
                {
                    if (larCaractere < LARGURA_VALOR_MIN_CARACTERE || larCaractere > LARGURA_VALOR_MAX_CARACTERE)
                    {
                        MessageBox.Show(mensagemLargura(true));
                        tboxLargCaractere.Focus();
                    }
                }

            }
            catch
            {
                MessageBox.Show(mensagemLargura(true), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tboxLargCaractere.Focus();
            }
        }

        private void tboxLargCaractere_TextChanged(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Focused)
            {
                try
                {
                    int larCaractere = Convert.ToInt16(tboxLargCaractere.Text);
                    int larMaxima = (Convert.ToInt16(tboxLarguraMax.Text) == 0 ? LARGURA_VALOR_MAX_CARACTERE : Convert.ToInt16(tboxLarguraMax.Text));

                    if (larCaractere >= LARGURA_VALOR_MIN_CARACTERE && larCaractere <= LARGURA_VALOR_MAX_CARACTERE && larCaractere <= larMaxima)
                    {
                        caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel = larCaractere;
                        caracteres[celulaSelecionada].Modelo.Textos[0].Largura = larCaractere;
                        //fachada.PreparaBitMapCaractere(caracteres[celulaSelecionada]);
                        //fachada.PreparaBitMapFrase()
                        fachada.RedimensionarCaractere(caracteres[celulaSelecionada]);
                        montarVideoBitMap();
                    }
                }
                catch
                {
                    MessageBox.Show(mensagemLargura(true), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tboxLargCaractere.Focus();
                }
            }
        }

        private void cbNivelSuavizacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                caracteres[celulaSelecionada].Modelo.Textos[0].BinaryThreshold = System.Convert.ToInt16(cbNivelSuavizacao.SelectedItem.ToString());

                if (Convert.ToInt16(tboxLarguraMax.Text) == 0)
                {
                    caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel = 0;
                    caracteres[celulaSelecionada].Modelo.Textos[0].Largura = 0;
                }

                fachada.PreparaBitMapCaractere(caracteres[celulaSelecionada]);
                
                montarVideoBitMap();

                carregarCelulaSelecionada();
            }
        }

        private void cboxFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                caracteres[celulaSelecionada].Modelo.Textos[0].Fonte = cboxFonte.SelectedItem.ToString();
                if (Convert.ToInt16(tboxLarguraMax.Text)==0)
                {
                    caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel = 0;
                    caracteres[celulaSelecionada].Modelo.Textos[0].Largura = 0;
                }

                fachada.PreparaBitMapCaractere(caracteres[celulaSelecionada]);
                
                montarVideoBitMap();

                carregarCelulaSelecionada();
            }
        }

        private void cboxTamanho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ToolStripComboBox)sender).Focused)
            {
                caracteres[celulaSelecionada].Modelo.Textos[0].Altura = System.Convert.ToInt16(cboxTamanho.SelectedItem.ToString());
                if (Convert.ToInt16(tboxLarguraMax.Text) == 0)
                {
                    caracteres[celulaSelecionada].Modelo.Textos[0].LarguraPainel = 0;
                    caracteres[celulaSelecionada].Modelo.Textos[0].Largura = 0;
                }
                fachada.PreparaBitMapCaractere(caracteres[celulaSelecionada]);
                
                montarVideoBitMap();

                carregarCelulaSelecionada();
            }
        }

        #endregion   

        private void dbgCaracteres_SelectionChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).Focused)
                carregarCelulaSelecionada();
        }

        private void copytoolStripButton_Click(object sender, EventArgs e)
        {
            caractereCopia = new Frase(caracteres[celulaSelecionada]);
            caractereCopia.Modelo.Textos[0].listaBitMap[0] = (Color[,])caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0].Clone();
        }

        private void pastetoolStripButton_Click(object sender, EventArgs e)
        {
            if (caractereCopia != null)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_COPY_1") + " " + caractereCopia.LabelFrase + " " + rm.GetString("EDITOR_FONTES_MBOX_COPY_2") + " " + caracteres[celulaSelecionada].LabelFrase + " ?", rm.GetString("EDITOR_FONTES_FORM_TEXT"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    Frase temp = caracteres[celulaSelecionada];
                    caracteres[celulaSelecionada] = new Frase(caractereCopia);
                    caracteres[celulaSelecionada].Modelo.Textos[0].listaBitMap[0] = (Color[,])caractereCopia.Modelo.Textos[0].listaBitMap[0].Clone();
                    caracteres[celulaSelecionada].LabelFrase = temp.LabelFrase;
                    caracteres[celulaSelecionada].Modelo.Textos[0].LabelTexto = temp.Modelo.Textos[0].LabelTexto;
                    popularCamposCelulaGUI();
                    montarVideoBitMap();
                }
            }
            else
            {
                MessageBox.Show(rm.GetString("EDITOR_FONTES_MBOX_SELECIONAR_CARACTERE"));
            }
        }
    }
}
