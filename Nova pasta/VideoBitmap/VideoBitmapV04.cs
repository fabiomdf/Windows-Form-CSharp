using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using Controlador;
using System.Collections.Generic;
using System.Diagnostics;


namespace PontosX2.VideoBitmap
{

    /// <summary>
    /// Summary description for UserControl.
    /// </summary>
    [ToolboxBitmap(typeof(VideoBitmapV04))]
    partial class VideoBitmapV04 : System.Windows.Forms.UserControl
    {
        #region Constantes

        //public const int WS_EX_CLIENTEDGE = unchecked((int)0x00000200);
        //public const int WS_BORDER = unchecked((int)0x00800000);



        private const int larguraInicial = 128;
        private const int alturaInicial = 13;
        private const int diametroLedInicial = 3;
        private const int espacamentoVertInicial = 2;
        private const int espacamentoHorzInicial = 1;

        private readonly Color corBordaAcesoInicial = Color.DarkGoldenrod;
        private readonly Color corInteriorAcesoInicial = Color.White;//Yellow;

        private readonly Color corBordaApagadoInicial = Color.FromArgb(64, 64, 64);
        private readonly Color corInteriorApagadoInicial = Color.DarkGray;

        private readonly Color corPainelInicial = Color.Black;

        #endregion

        #region Atributos


        private BorderStyle borderStyle;

        // Largura do painel, em colunas de leds
        private int largura;

        // Altura do painel, em linhas de leds
        private int altura;

        // Diâmetro do LED, em pixels
        private int diametroLed;

        // Espacamento vertical entre os leds, em pixels
        private int espacamentoVert;

        // Espacamento horizontal entre leds, em pixels
        private int espacamentoHorz;

        // Cores usadas para desenhar um LED aceso
        private Color corBordaAceso;
        private Color corInteriorAceso;

        // Cores usadas para desenhar um LED apagado
        private Color corBordaApagado;
        private Color corInteriorApagado;

        // Cor de fundo do painel
        private Color corPainel;

        // Colunas do bitmap que será desenhado
        //private ulong[] bitmap;
        //private long[] bitmap;
        private Color[,] bitmap;
        public List<Color[,]> listaBitMap;

        // Bitmaps dos LEDs aceso e apagado, respectivamente
        private Bitmap ledAceso;
        private Bitmap ledApagado;

        // Bitmap onde o painel será renderizado
        private Bitmap bmPainel;

        //indica se a rolagem da frase será com wrap(true) ou não(false).
        private bool _rolaWrap = false;

        // Matriz de colunas com apenas um bit ligado, usadas para otimizar
        // o desempenho da função Desenhar
        private long[] bits;

        //Tipo Frase de onde serão coletados os bitmaps a serem desenhados.

        private bool _existeProximaImagem;
        private bool _existeAnteriorImagem;

        private int _indiceImagemExibidaAtual;

        private bool _navega_wrap;

        private bool _exibindo = false;

        private bool _parar = false;

        private bool _pausa_ultima_imagem = true;

        private bool bJaAdicionou = false;

        public bool Cancelar = false;

        private int qtdColunasAdicionadas = 0;
        #endregion

        #region Propriedades
        public bool JaAdicionou
        {
            get
            {
                return bJaAdicionou;
            }
            set
            {
                bJaAdicionou = value;
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle &= (~NativeMethods.WS_EX_CLIENTEDGE);
                cp.Style &= (~NativeMethods.WS_BORDER);

                switch (borderStyle)
                {
                    case BorderStyle.Fixed3D:
                        cp.ExStyle |= NativeMethods.WS_EX_CLIENTEDGE;
                        break;

                    case BorderStyle.FixedSingle:
                        cp.Style |= NativeMethods.WS_BORDER;
                        break;
                }

                return cp;
            }
        }

        public BorderStyle BorderStyle
        {
            get
            {
                return borderStyle;
            }
            set
            {
                if (borderStyle != value)
                {
                    if (!Enum.IsDefined(typeof(BorderStyle), value))
                    {
                        throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
                    }

                    borderStyle = value;
                    UpdateStyles();
                }
            }
        }

        // Largura do painel, em colunas de leds
        [Category("Pontos6")]
        [Description("Largura do painel, em colunas de LEDs.")]
        public int Largura
        {
            get
            {
                return largura;
            }
            set
            {
                // Gera um novo array de colunas
                largura = value;
                bitmap = new Color[altura, largura];

                for (int l = 0; l < altura; l++)
                    for (int j = 0; j < largura; j++)
                    {
                        bitmap[l, j] = Color.Black;
                    }
                // Cria o painel novamente
                CriarPainel();

                Desenhar();

                // Redesenha o bitmap
                this.Refresh();
            }
        }

        // Altura do painel, em linhas de leds
        [Category("Pontos6")]
        [Description("Altura do painel, em linhas de LEDs.")]
        public int Altura
        {
            get
            {
                return altura;
            }
            set
            {
                altura = value;

                bitmap = new Color[altura, largura];

                for (int l = 0; l < altura; l++)
                    for (int j = 0; j < largura; j++)
                    {
                        bitmap[l, j] = Color.Black;
                    }

                // Prepara a matriz de bits usadas no desenho das colunas
                bits = new long[altura];
                for (int k = 0; k < altura; k++)
                {
                    bits[k] = 1 << k;
                }

                // Cria o painel novamente
                CriarPainel();

                // Desenha o bitmap
                Desenhar();

                // Redesenha o bitmap
                this.Refresh();
            }
        }

        // Diâmetro do LED, em pixels
        [Category("Pontos6")]
        [Description("Diâmetro dos LEDs, em pixels.")]
        public int DiametroLed
        {
            get
            {
                return diametroLed;
            }
            set
            {
                diametroLed = value;

                // Recria os bitmaps
                CriarLedAceso();
                CriarLedApagado();
                CriarPainel();

                Desenhar();

                // Redesenha o bitmap
                this.Refresh();
            }
        }

        // Espacamento vertical entre os leds, em pixels
        [Category("Pontos6")]
        [Description("Distância vertical entre os LEDs, em pixels.")]
        public int EspacamentoVert
        {
            get
            {
                return espacamentoVert;
            }
            set
            {
                espacamentoVert = value;

                // Cria novamente os bitmaps
                CriarLedAceso();
                CriarLedApagado();
                CriarPainel();

                Desenhar();

                // Redesenha o bitmap
                this.Refresh();
            }
        }

        // Espacamento horizontal entre leds, em pixels
        [Category("Pontos6")]
        [Description("Distância horizontal entre os LEDs, em pixels.")]
        public int EspacamentoHorz
        {
            get
            {
                return espacamentoHorz;
            }
            set
            {
                espacamentoHorz = value;

                // Cria os bitmaps
                CriarLedAceso();
                CriarLedApagado();
                CriarPainel();

                Desenhar();

                // Redesenha o bitmap
                this.Refresh();
            }
        }

        // Bitmap que será desenhado
        [Category("Pontos6")]
        [Description("Colunas do bitmap que será renderizado no painel.")]
        public Color[,] Bitmap
        {
            get
            {
                return bitmap;
            }
            set
            {
                bitmap = value;

                //Desenhar();
                //Invoke(new MethodInvoker(Desenhar));
                Desenhar();
                // this.Refresh();
                //Invoke(new MethodInvoker(Refresh));
                Refresh();
            }

        }

        /// <summary>
        /// Indica se ainda existe imagens.
        /// </summary>
        public bool ExisteProximaImagem
        {
            get
            {
                return this._existeProximaImagem;
            }

        }

        /// <summary>
        /// Indica se a imagem exibida é a primeira.
        /// </summary>
        public bool ExisteAnteriorImagem
        {
            get
            {
                return this._existeAnteriorImagem;
            }
        }

        public bool WrapNavegacao
        {
            get
            {
                return this._navega_wrap;
            }
            set
            {
                this._navega_wrap = value;
            }
        }


        public bool WrapRolagem
        {
            get
            {
                return this._rolaWrap;
            }
            set
            {
                this._rolaWrap = value;
            }
        }




        public bool Parar
        {
            get
            {
                return this._parar;
            }

            set
            {
                this._parar = value;
            }
        }

        #endregion //propriedades.
        public bool ParaUltimaImagem
        {
            set { this._pausa_ultima_imagem = value; }
            get { return this._pausa_ultima_imagem; }
        }


        public void PausaUltimaImagem(int tempo)
        {
            if (this._pausa_ultima_imagem == true)
                Thread.Sleep(tempo);
        }





        public void Apaga(int tempoDeEspera)
        {
            // Boolean[,] bitmapApagado = new bool[this.altura, this.largura];

            // bitmap = bitmapApagado;

            for (int j = 0; j < largura; j++)
            {
                for (int i = 0; i < Altura; i++)
                {
                    this.Bitmap[i, j] = Color.Black;
                }
            }
            // this.Bitmap = bitmapApagado;
            //Invoke(new MethodInvoker(Desenhar));
           // Invoke(new MethodInvoker(Refresh));
            Thread.Sleep(tempoDeEspera);
        }




        public bool Exibindo
        {
            get
            {
                return this._exibindo;
            }
            set
            {
                this._exibindo = value;
            }
        }


        public VideoBitmapV04()
        {
            long coluna = 0;
            int i;

            // This call is required by the Windows.Forms Designer.
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            // Carrega os valores padrão para os parâmetros de desenho
            largura = larguraInicial;
            altura = alturaInicial;
            diametroLed = diametroLedInicial;
            espacamentoVert = espacamentoVertInicial;
            espacamentoHorz = espacamentoHorzInicial;
            bitmap = new Color[altura, largura];
            corBordaAceso = corBordaAcesoInicial;
            corInteriorAceso = corInteriorAcesoInicial;
            corBordaApagado = corBordaApagadoInicial;
            corInteriorApagado = corInteriorApagadoInicial;
            corPainel = corPainelInicial;
            borderStyle = BorderStyle.None;
            _indiceImagemExibidaAtual = 0;


            for (int l = 0; l < altura; l++)
                for (int j = 0; j < largura; j++)
                {
                    bitmap[l, j] = Color.Black;
                }
            //cria uma frase default
            //_frase = new Frase();


            // Prepara um padrão para as colunas
            for (i = 0; i < altura; i++)
            {
                if (i % 2 == 0)
                    coluna |= (uint)(1 << i);
            }
            for (int j = 0; j < altura; j++)
            {
                for (i = 0; i < largura; i++)
                {
                    //bitmap[j, i] = (i % 2 != 0) ? Color.Black : bitmap[j, i];
                }
            }

            // Cria o bitmap do led aceso
            CriarLedAceso();

            // Cria o bitmap do led apagado
            CriarLedApagado();

            // Cria o bitmap do painel
            CriarPainel();

            // Prepara a matriz de bits usadas no desenho das colunas
            bits = new long[altura];
            for (int k = 0; k < altura; k++)
            {
                bits[k] = 1 << k;
            }

            // Desenha o padrão de colunas inicial no painel
            Desenhar();
        }

        public VideoBitmapV04(int altura, int largura)
        {
            long coluna = 0;
            int i;

            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Carrega os valores padrão para os parâmetros de desenho
            this.largura = largura;
            this.altura = altura;
            diametroLed = diametroLedInicial;
            espacamentoVert = espacamentoVertInicial;
            espacamentoHorz = espacamentoHorzInicial;
            bitmap = new Color[altura, largura];
            corBordaAceso = corBordaAcesoInicial;
            corInteriorAceso = corInteriorAcesoInicial;
            corBordaApagado = corBordaApagadoInicial;
            corInteriorApagado = corInteriorApagadoInicial;
            corPainel = corPainelInicial;
            borderStyle = BorderStyle.None;
            _indiceImagemExibidaAtual = 0;


            for (int l = 0; l < altura; l++)
                for (int j = 0; j < largura; j++)
                {
                    bitmap[l, j] = Color.Black;
                }

            // Prepara um padrão para as colunas
            for (i = 0; i < altura; i++)
            {
                if (i % 2 == 0)
                    coluna |= (uint)(1 << i);
            }

            // Cria o bitmap do led aceso
            CriarLedAceso();

            // Cria o bitmap do led apagado
            CriarLedApagado();

            // Cria o bitmap do painel
            CriarPainel();

            // Prepara a matriz de bits usadas no desenho das colunas
            bits = new long[altura];
            for (int k = 0; k < altura; k++)
            {
                bits[k] = 1 << k;
            }

            // Desenha o padrão de colunas inicial no painel
            Desenhar();
        }

        ///// <summary>
        ///// Clean up any resources being used.
        ///// </summary>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (components != null)
        //            components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        /// <summary>
        /// Cria o bitmap do LED aceso
        /// </summary>
        private void CriarLedAceso()
        {
            SolidBrush brush;
            Pen pen;
            Graphics gr;

            brush = new SolidBrush(corInteriorAceso);
            pen = new Pen(corBordaAceso);
            ledAceso = new Bitmap(diametroLed + espacamentoHorz,
                diametroLed + espacamentoVert);
            gr = Graphics.FromImage(ledAceso);
            gr.Clear(corPainel);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.DrawEllipse(pen, 0, 0, diametroLed, diametroLed);
            gr.FillEllipse(brush, 0, 0, diametroLed, diametroLed);
        }
        public void CriarLedAceso(Color cor)
        {
            SolidBrush brush;
            Pen pen;
            Graphics gr;

            brush = new SolidBrush(cor);
            pen = new Pen(corBordaAceso);
            ledAceso = new Bitmap(diametroLed + espacamentoHorz,
                diametroLed + espacamentoVert);
            gr = Graphics.FromImage(ledAceso);
            gr.Clear(corPainel);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.DrawEllipse(pen, 0, 0, diametroLed, diametroLed);
            gr.FillEllipse(brush, 0, 0, diametroLed, diametroLed);
        }
        /// <summary>
        /// Cria o bitmap do LED apagado
        /// </summary>
        private void CriarLedApagado()
        {
            SolidBrush brush;
            Pen pen;
            Graphics gr;

            brush = new SolidBrush(corInteriorApagado);
            pen = new Pen(corBordaApagado);
            ledApagado = new Bitmap(diametroLed + espacamentoHorz,
                diametroLed + espacamentoVert);
            gr = Graphics.FromImage(ledApagado);
            gr.Clear(corPainel);
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            gr.DrawEllipse(pen, 0, 0, diametroLed, diametroLed);
            gr.FillEllipse(brush, 0, 0, diametroLed - 1, diametroLed - 1);
        }

        /// <summary>
        /// Cria o bitmap do painel
        /// </summary>
        private void CriarPainel()
        {
            // Cria o bitmap onde será desenhado o painel
            bmPainel = new Bitmap(largura * (diametroLed + espacamentoHorz),
                (altura * (diametroLed + espacamentoVert))+1);
            
        }

        /// <summary>
        /// Desenha o painel, usando as colunas do bitmap e as imagens dos leds
        /// aceso e apagado criadas anteriormente.
        /// </summary>
        public void Desenhar()
        {
            Graphics gr;
            long coluna;

            gr = Graphics.FromImage(bmPainel);

            // Para cada coluna...
            for (ushort x = 0; x < largura; x++)
            {
                // Para cada ponto da coluna...
                for (ushort y = 0; y < altura; y++)
                {
                    // Verifica se o bit da potência está ligado
                    if (bitmap[y, x] == Color.Black)
                    {
                        gr.DrawImage(ledApagado, x * (diametroLed + espacamentoHorz),
                        (altura - y - 1) * (diametroLed + espacamentoVert) + 1);
                    }
                    else
                    {
                        CriarLedAceso(bitmap[y, x]);
                        gr.DrawImage(ledAceso, x * (diametroLed + espacamentoHorz),
                            (altura - y - 1) * (diametroLed + espacamentoVert) + 1);
                    }
                }
            }

            this.Width = bmPainel.Size.Width;
            this.Height = bmPainel.Size.Height - 1;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {

            // Calling the base class OnPaint
            base.OnPaint(pe);

            // Desenha o bitmap do painel no controle
            Graphics gr = pe.Graphics;
            gr.DrawImage(bmPainel, 0, -1);

            //this.Width = bmPainel.Size.Width;
            //this.Height = bmPainel.Size.Height-1;
        }
        public void InserirBit()
        {
            Color valor = Color.Yellow;
            InserirBit(valor);
        }
        public void InserirBit(Color valor)
        {
            for (int indiceColunas = 0; indiceColunas < largura; indiceColunas++)
            {
                for (int indiceLinhas = 0; indiceLinhas < altura; indiceLinhas++)
                {
                    if ((indiceLinhas == altura - 1) && (indiceColunas + 1 < largura))
                    {
                        bitmap[indiceLinhas, indiceColunas] = bitmap[0, indiceColunas + 1];
                    }
                    else
                    {
                        if ((indiceLinhas + 1 < altura))
                            bitmap[indiceLinhas, indiceColunas] = bitmap[indiceLinhas + 1, indiceColunas];
                    }
                }
            }

            bitmap[altura - 1, largura - 1] = valor;
        }

        public void InserirByte(Byte valor)
        {
            Color cor = Color.Yellow;
            InserirByte(valor, cor);
        }

        public void InserirByte(Byte valor, Color cor)
        {
            for (int indiceBit = 1; indiceBit <= 128; indiceBit *= 2)
            {
                if (Convert.ToBoolean(valor & indiceBit))
                {
                    InserirBit(cor);
                }
                else
                {
                    InserirBit(Color.Black);
                }
            }
        }

        public void ApresentarFrase(Texto texto)
        {
            Apaga(0);

            while (!Cancelar)
            {
                Apresentar(texto);
            }
        }

        private int GetUltimoPixelLargura()
        {
            int ultimoPixel = 0;

            for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count; indiceBitMap++)
            {
                for (int col = 0; col < listaBitMap[indiceBitMap].GetLength(1); col++)
                {
                    for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0); linha++)
                    {
                        if (listaBitMap[indiceBitMap][linha, col] != Color.Black)
                        {
                            ultimoPixel = (indiceBitMap * listaBitMap[indiceBitMap].GetLength(1)) + col;
                            break;
                        }
                    }
                }
            }
            return ultimoPixel;
        }

        private int GetPrimeiroPixelLargura()
        {
            int primeiroPixel = 0;

            for (int col = 0; col < listaBitMap[0].GetLength(1); col++)
            {
                for (int linha = 0; linha < listaBitMap[0].GetLength(0); linha++)
                {
                    if (listaBitMap[0][linha, col] != Color.Black)
                    {
                        primeiroPixel = col;
                        return primeiroPixel;
                    }
                }
            }

            return primeiroPixel;
        }

        private void EsperarTempo(int tempo)
        {
            for (int j = 0; j < tempo; j++)
            {
                Thread.Sleep(1);
                if (Cancelar)
                    break;
            }
        }

        private void Apresentar(Texto texto)
        {
            int primeiroPixel;
            int ultimoPixel;
            int indiceListBitMap;
            int incremento;
            Color[,] matrix;

            Stopwatch s = new Stopwatch();
            long TempoRolagem;

            switch (texto.Apresentacao)
            {
                #region Fixa

                case Util.Util.Rolagem.Fixa:

                    for (int i = 0; i < listaBitMap.Count; i++)
                    {
                        bitmap = listaBitMap[i];
                        Invoke(new MethodInvoker(Desenhar));
                        Invoke(new MethodInvoker(Refresh));
                        EsperarTempo(texto.TempoApresentacao);
                    }
                    break;

                #endregion

                #region Continua1

                case Util.Util.Rolagem.Rolagem_Continua_Esquerda:

                    Apaga(0);

                    primeiroPixel = GetPrimeiroPixelLargura();
                    ultimoPixel = GetUltimoPixelLargura();
                    indiceListBitMap = 0;

                    for (int col = primeiroPixel; (col <= ultimoPixel) && (!Cancelar); col++)
                    {
                        s.Restart();
                        if ((col % listaBitMap[0].GetLength(1)) == 0 && col > 0)
                            indiceListBitMap = indiceListBitMap + 1;

                        for (int linha = 0; linha < listaBitMap[0].GetLength(0); linha++)
                            InserirBit(listaBitMap[indiceListBitMap][linha, col - (listaBitMap[0].GetLength(1) * indiceListBitMap)]);

                        Invoke(new MethodInvoker(Desenhar));
                        Invoke(new MethodInvoker(Refresh));

                        s.Stop();

                        TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                        EsperarTempo(System.Convert.ToInt32(TempoRolagem));
                    }

                    if (!Cancelar)
                        EsperarTempo(texto.TempoApresentacao);

                    break;

                #endregion

                #region Continua2

                case Util.Util.Rolagem.Rolagem_Continua2_Esquerda:

                    primeiroPixel = GetPrimeiroPixelLargura();
                    ultimoPixel = GetUltimoPixelLargura();
                    indiceListBitMap = 0;

                    for (int col = primeiroPixel; (col <= ultimoPixel) && (!Cancelar); col++)
                    {
                        s.Restart();
                        if ((col % listaBitMap[0].GetLength(1)) == 0 && col > 0)
                            indiceListBitMap = indiceListBitMap + 1;

                        for (int linha = 0; linha < listaBitMap[0].GetLength(0); linha++)
                            InserirBit(listaBitMap[indiceListBitMap][linha, col - (listaBitMap[0].GetLength(1) * indiceListBitMap)]);

                        Invoke(new MethodInvoker(Desenhar));
                        Invoke(new MethodInvoker(Refresh));

                        s.Stop();

                        TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                        EsperarTempo(System.Convert.ToInt32(TempoRolagem));
                    }

                    for (int col = 0; col < listaBitMap[0].GetLength(1) && (!Cancelar); col++)
                    {
                        s.Restart();
                        for (int linha = 0; linha < listaBitMap[0].GetLength(0); linha++)
                            InserirBit(Color.Black);

                        Invoke(new MethodInvoker(Desenhar));
                        Invoke(new MethodInvoker(Refresh));

                        s.Stop();

                        TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                        EsperarTempo(System.Convert.ToInt32(TempoRolagem));
                    }

                    break;

                #endregion

                #region Continua3

                case Util.Util.Rolagem.Rolagem_Continua3_Esquerda:

                    primeiroPixel = GetPrimeiroPixelLargura();
                    ultimoPixel = GetUltimoPixelLargura();
                    indiceListBitMap = 0;

                    for (int col = primeiroPixel; (col <= ultimoPixel) && (!Cancelar); col++)
                    {
                        s.Restart();
                        if ((col % listaBitMap[0].GetLength(1)) == 0 && col > 0)
                            indiceListBitMap = indiceListBitMap + 1;

                        for (int linha = 0; linha < listaBitMap[0].GetLength(0); linha++)
                            InserirBit(listaBitMap[indiceListBitMap][linha, col - (listaBitMap[0].GetLength(1) * indiceListBitMap)]);

                        Invoke(new MethodInvoker(Desenhar));
                        Invoke(new MethodInvoker(Refresh));

                        s.Stop();

                        TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                        EsperarTempo(System.Convert.ToInt32(TempoRolagem));
                    }

                    for (int col = 0; col < 16 && (!Cancelar); col++)
                    {
                        s.Restart();
                        for (int linha = 0; linha < listaBitMap[0].GetLength(0); linha++)
                            InserirBit(Color.Black);

                        Invoke(new MethodInvoker(Desenhar));
                        Invoke(new MethodInvoker(Refresh));

                        s.Stop();

                        TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                        EsperarTempo(System.Convert.ToInt32(TempoRolagem));
                    }

                    break;

                #endregion

                #region Paginada

                case Util.Util.Rolagem.Rolagem_Paginada_Esquerda:



                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);

                        for (int col = 0; col < listaBitMap[indiceBitMap].GetLength(1) && (!Cancelar); col++)
                        {
                            s.Restart();
                            for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0) && (!Cancelar); linha++)
                                InserirBit(listaBitMap[indiceBitMap][linha, col]);

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));
                        }
                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;

                #endregion

                #region Rolagem Pra Cima

                case Util.Util.Rolagem.Rolagem_Cima:

                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);

                        for (int linha = listaBitMap[indiceBitMap].GetLength(0) - 1; linha >= 0 && (!Cancelar); linha--)
                        {
                            s.Restart();
                            Apaga(0);

                            matrix = GetPreencherBitMap(linha, listaBitMap[indiceBitMap].GetLength(0) - 1, 0, listaBitMap[indiceBitMap].GetLength(1) - 1, indiceBitMap);

                            //preenchendo com a matrix pintada
                            for (int i = 0; i < matrix.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrix.GetLength(1); j++)
                                    bitmap[i, j] = matrix[i, j];
                            }

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));
                        }

                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;

                #endregion

                #region Rolagem Pra Baixo

                case Util.Util.Rolagem.Rolagem_Baixo:


                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);

                        for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0) && (!Cancelar); linha++)
                        {
                            s.Restart();
                            Apaga(0);

                            matrix = GetPreencherBitMap(0, linha, 0, listaBitMap[indiceBitMap].GetLength(1) - 1, indiceBitMap);  // Color[linha, listaBitMap[indiceBitMap].GetLength(1)];

                            //preenchendo com a matrix pintada
                            incremento = 1;
                            for (int i = matrix.GetLength(0) - 1; i >= 0; i--)
                            {
                                for (int j = 0; j < matrix.GetLength(1); j++)
                                    bitmap[listaBitMap[indiceBitMap].GetLength(0) - incremento, j] = matrix[i, j];
                                incremento = incremento + 1;
                            }

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));
                        }

                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;

                #endregion

                #region Horizontal Fade Out

                case Util.Util.Rolagem.Surgimento_Fora_Horizontal:

                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);
                        EsperarTempo(System.Convert.ToInt32(texto.TempoRolagem));

                        int meioPainelSubir = listaBitMap[indiceBitMap].GetLength(1) / 2;
                        int meioPainelDescer = meioPainelSubir - 1;

                        while ((meioPainelDescer >= 0 && meioPainelSubir < listaBitMap[indiceBitMap].GetLength(1)) && (!Cancelar))
                        {
                            s.Restart();

                            if (meioPainelSubir < listaBitMap[indiceBitMap].GetLength(1))
                            {
                                //Subindo até o inicio do painel
                                for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0) && (!Cancelar); linha++)
                                    bitmap[linha, meioPainelSubir] = listaBitMap[indiceBitMap][linha, meioPainelSubir];

                                meioPainelSubir = meioPainelSubir + 1;
                            }

                            if (meioPainelDescer >= 0)
                            {
                                //Descendo até o inicio do painel
                                for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0) && (!Cancelar); linha++)
                                    bitmap[linha, meioPainelDescer] = listaBitMap[indiceBitMap][linha, meioPainelDescer];

                                meioPainelDescer = meioPainelDescer - 1;
                            }

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));

                        }

                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;

                #endregion

                #region Horizontal Fade In

                case Util.Util.Rolagem.Surgimento_Dentro_Horizontal:

                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);
                        EsperarTempo(System.Convert.ToInt32(texto.TempoRolagem));

                        int meioPainelSubir = listaBitMap[indiceBitMap].GetLength(1) / 2;
                        int meioPainelDescer = meioPainelSubir - 1;

                        int indiceSubindo = 0;
                        int indiceDescendo = listaBitMap[indiceBitMap].GetLength(1) - 1;

                        while ((meioPainelDescer >= indiceSubindo && meioPainelSubir <= indiceDescendo) && (!Cancelar))
                        {
                            s.Restart();

                            if (meioPainelSubir <= indiceDescendo)
                            {
                                //Subindo até o inicio do painel
                                for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0) && (!Cancelar); linha++)
                                    bitmap[linha, indiceDescendo] = listaBitMap[indiceBitMap][linha, indiceDescendo];

                                indiceDescendo = indiceDescendo - 1;
                            }

                            if (meioPainelDescer >= 0)
                            {
                                //Descendo até o inicio do painel
                                for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0) && (!Cancelar); linha++)
                                    bitmap[linha, indiceSubindo] = listaBitMap[indiceBitMap][linha, indiceSubindo];

                                indiceSubindo = indiceSubindo + 1;
                            }

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));

                        }

                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;

                #endregion

                #region Vertical Fade Out

                case Util.Util.Rolagem.Surgimento_Fora_Vertical:

                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);
                        EsperarTempo(System.Convert.ToInt32(texto.TempoRolagem));

                        int meioPainelSubir = listaBitMap[indiceBitMap].GetLength(0) / 2;
                        int meioPainelDescer = meioPainelSubir - 1;

                        while ((meioPainelDescer >= 0 && meioPainelSubir < listaBitMap[indiceBitMap].GetLength(0)) && (!Cancelar))
                        {
                            s.Restart();

                            if (meioPainelSubir < listaBitMap[indiceBitMap].GetLength(0))
                            {
                                //Subindo até o inicio do painel
                                for (int col = 0; col < listaBitMap[indiceBitMap].GetLength(1) && (!Cancelar); col++)
                                    bitmap[meioPainelSubir, col] = listaBitMap[indiceBitMap][meioPainelSubir, col];

                                meioPainelSubir = meioPainelSubir + 1;
                            }

                            if (meioPainelDescer >= 0)
                            {
                                //Descendo até o inicio do painel
                                for (int col = 0; col < listaBitMap[indiceBitMap].GetLength(1) && (!Cancelar); col++)
                                    bitmap[meioPainelDescer, col] = listaBitMap[indiceBitMap][meioPainelDescer, col];

                                meioPainelDescer = meioPainelDescer - 1;
                            }

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));

                        }

                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;

                #endregion

                #region Vertical Fade In

                case Util.Util.Rolagem.Surgimento_Dentro_Vertical:

                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);
                        EsperarTempo(System.Convert.ToInt32(texto.TempoRolagem));

                        int meioPainelSubir = listaBitMap[indiceBitMap].GetLength(0) / 2;
                        int meioPainelDescer = meioPainelSubir - 1;

                        int indiceSubindo = 0;
                        int indiceDescendo = listaBitMap[indiceBitMap].GetLength(0) - 1;

                        while ((meioPainelDescer >= indiceSubindo && meioPainelSubir <= indiceDescendo) && (!Cancelar))
                        {
                            s.Restart();

                            if (meioPainelSubir <= indiceDescendo)
                            {
                                //Subindo até o inicio do painel
                                for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(1) && (!Cancelar); linha++)
                                    bitmap[indiceDescendo, linha] = listaBitMap[indiceBitMap][indiceDescendo, linha];

                                indiceDescendo = indiceDescendo - 1;
                            }

                            if (meioPainelDescer >= 0)
                            {
                                //Descendo até o inicio do painel
                                for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(1) && (!Cancelar); linha++)
                                    bitmap[indiceSubindo, linha] = listaBitMap[indiceBitMap][indiceSubindo, linha];

                                indiceSubindo = indiceSubindo + 1;
                            }

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));

                        }

                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;

                #endregion

                #region Both Fade In
                case Util.Util.Rolagem.Surgimento_Dentro_Ambos:

                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);
                        EsperarTempo(System.Convert.ToInt32(texto.TempoRolagem));


                        int meioPainelSubirVer = listaBitMap[indiceBitMap].GetLength(0) / 2;
                        int meioPainelDescerVer = meioPainelSubirVer - 1;

                        int indiceSubindoVer = 0;
                        int indiceDescendoVer = listaBitMap[indiceBitMap].GetLength(0) - 1;

                        int meioPainelSubirHor = listaBitMap[indiceBitMap].GetLength(1) / 2;
                        int meioPainelDescerHor = meioPainelSubirHor - 1;

                        int indiceSubindoHor = 0;
                        int indiceDescendoHor = listaBitMap[indiceBitMap].GetLength(1) - 1;

                        int fatorLarguraAltura = (listaBitMap[indiceBitMap].GetLength(1) / listaBitMap[indiceBitMap].GetLength(0)) / 2;
                        int incrementoFatorLargura = 0;

                        while ((meioPainelDescerVer >= indiceSubindoVer && meioPainelSubirVer <= indiceDescendoVer) && (meioPainelDescerHor >= indiceSubindoHor && meioPainelSubirHor <= indiceDescendoHor) && (!Cancelar))
                        {
                            s.Restart();

                            //Vai pintar uma linha de acordo com a proporção entre a largura e a altura do painel
                            if (incrementoFatorLargura == fatorLarguraAltura)
                            {
                                if (meioPainelSubirVer <= indiceDescendoVer)
                                {
                                    //Subindo até o inicio do painel
                                    for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(1) && (!Cancelar); linha++)
                                        bitmap[indiceDescendoVer, linha] = listaBitMap[indiceBitMap][indiceDescendoVer, linha];

                                    indiceDescendoVer = indiceDescendoVer - 1;
                                }

                                if (meioPainelDescerVer >= 0)
                                {
                                    //Descendo até o inicio do painel
                                    for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(1) && (!Cancelar); linha++)
                                        bitmap[indiceSubindoVer, linha] = listaBitMap[indiceBitMap][indiceSubindoVer, linha];

                                    indiceSubindoVer = indiceSubindoVer + 1;
                                }
                                incrementoFatorLargura = 0;
                            }

                            //Pinta coluna do painel
                            if (meioPainelSubirHor <= indiceDescendoHor)
                            {
                                //Subindo até o inicio do painel
                                for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0) && (!Cancelar); linha++)
                                    bitmap[linha, indiceDescendoHor] = listaBitMap[indiceBitMap][linha, indiceDescendoHor];

                                indiceDescendoHor = indiceDescendoHor - 1;
                            }

                            if (meioPainelDescerHor >= 0)
                            {
                                //Descendo até o inicio do painel
                                for (int linha = 0; linha < listaBitMap[indiceBitMap].GetLength(0) && (!Cancelar); linha++)
                                    bitmap[linha, indiceSubindoHor] = listaBitMap[indiceBitMap][linha, indiceSubindoHor];

                                indiceSubindoHor = indiceSubindoHor + 1;
                            }

                            incrementoFatorLargura = incrementoFatorLargura + 1;

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));

                        }

                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;


                #endregion

                #region Both Fade Out
                case Util.Util.Rolagem.Surgimento_Fora_Ambos:

                    for (int indiceBitMap = 0; indiceBitMap < listaBitMap.Count && (!Cancelar); indiceBitMap++)
                    {
                        Apaga(0);
                        EsperarTempo(System.Convert.ToInt32(texto.TempoRolagem));

                        int meioPainelVer = listaBitMap[indiceBitMap].GetLength(0) / 2;

                        int meioPainelHor = listaBitMap[indiceBitMap].GetLength(1) / 2;

                        double fatorLargura = (listaBitMap[indiceBitMap].GetLength(1) / (double)(listaBitMap[indiceBitMap].GetLength(1) / 2));
                        double fatorAltura = (listaBitMap[indiceBitMap].GetLength(0) / (double)(listaBitMap[indiceBitMap].GetLength(1) / 2));

                        incremento = 0;

                        int linhaInicial = meioPainelVer;
                        int linhaFinal = meioPainelVer;
                        int colInicial = meioPainelHor;
                        int colFinal = meioPainelHor;

                        while ((linhaInicial > 0 || linhaFinal < listaBitMap[indiceBitMap].GetLength(0) - 1 || colInicial > 0 || colFinal < listaBitMap[indiceBitMap].GetLength(1) - 1) && (!Cancelar))
                        {
                            s.Restart();

                            linhaInicial = (int)Math.Ceiling(meioPainelVer - (fatorAltura * incremento));
                            linhaFinal = (int)Math.Ceiling(meioPainelVer + (fatorAltura * incremento));
                            colInicial = (int)Math.Ceiling(meioPainelHor - (fatorLargura * incremento));
                            colFinal = (int)Math.Ceiling(meioPainelHor + (fatorLargura * incremento));

                            linhaInicial = (linhaInicial >= 0 ? linhaInicial : 0);
                            linhaFinal = (linhaFinal < listaBitMap[indiceBitMap].GetLength(0) ? linhaFinal : listaBitMap[indiceBitMap].GetLength(0) - 1);
                            colInicial = (colInicial >= 0 ? colInicial : 0);
                            colFinal = (colFinal < listaBitMap[indiceBitMap].GetLength(1) ? colFinal : listaBitMap[indiceBitMap].GetLength(1) - 1);

                            PreencherBitMap(linhaInicial, linhaFinal, colInicial, colFinal, indiceBitMap);

                            incremento = incremento + 1;

                            Invoke(new MethodInvoker(Desenhar));
                            Invoke(new MethodInvoker(Refresh));

                            s.Stop();

                            TempoRolagem = (s.ElapsedMilliseconds > texto.TempoRolagem ? 0 : texto.TempoRolagem - s.ElapsedMilliseconds);
                            EsperarTempo(System.Convert.ToInt32(TempoRolagem));

                        }

                        EsperarTempo(texto.TempoApresentacao);
                    }

                    break;


                #endregion
            }

        }

        private void PreencherMatrixNaPosicao(Color[,] matrix, int linhaInicial, int colunaInicial)
        {
            int interacaoLinha = linhaInicial;
            for (int linha = 0; linha < matrix.GetLength(0); linha++)
            {
                int interacaoColuna = colunaInicial;
                for (int coluna = 0; coluna < matrix.GetLength(1); coluna++)
                {
                    bitmap[interacaoLinha, interacaoColuna] = matrix[linha, coluna];
                    interacaoColuna = interacaoColuna + 1;
                }
                interacaoLinha = interacaoLinha + 1;
            }

        }

        private void ApagarMatrixNaPosicao(Color[,] matrix, int linhaInicial, int colunaInicial)
        {
            int interacaoLinha = linhaInicial;
            for (int linha = 0; linha < matrix.GetLength(0); linha++)
            {
                int interacaoColuna = colunaInicial;
                for (int coluna = 0; coluna < matrix.GetLength(1); coluna++)
                {
                    bitmap[interacaoLinha, interacaoColuna] = Color.Black;
                    interacaoColuna = interacaoColuna + 1;
                }
                interacaoLinha = interacaoLinha + 1;
            }

        }


        private void PreencherBitMap(int linhaOrigem, int linhaDestino, int colunaOrigem, int colunaDestino, int indexListaBitMap)
        {
            for (int linha = linhaOrigem; linha <= linhaDestino; linha++)
                for (int coluna = colunaOrigem; coluna <= colunaDestino; coluna++)
                    bitmap[linha, coluna] = listaBitMap[indexListaBitMap][linha, coluna];
        }

        private Color[,] GetPreencherBitMap(int linhaOrigem, int linhaDestino, int colunaOrigem, int colunaDestino, int indexListaBitMap)
        {
            Color[,] matrix = new Color[(linhaDestino - linhaOrigem) + 1, (colunaDestino - colunaOrigem) + 1];
            int incrementoLinha = 0;
            for (int linha = linhaOrigem; linha <= linhaDestino; linha++)
            {
                int incrementoColuna = 0;
                for (int coluna = colunaOrigem; coluna <= colunaDestino; coluna++)
                {
                    matrix[incrementoLinha, incrementoColuna] = listaBitMap[indexListaBitMap][linha, coluna];
                    incrementoColuna = incrementoColuna + 1;
                }
                incrementoLinha = incrementoLinha + 1;
            }

            return matrix;
        }
    }





}
