using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Persistencia;
using Nucleo;
using System.Drawing;
using System.Drawing.Text;

namespace Controlador
{
    public class Texto
    {
        public string LabelTexto { get; set;}
        public Util.Util.Rolagem Apresentacao { get; set; }
        private string _fonte;
        public int BinaryThreshold  = 200;
        public bool FonteWindows { get; set; }
        public bool FonteAnteriorWindows { get; set; }
        public int Altura { get; set; }
        public bool Negrito { get; set; }
        public bool Italico { get; set; }
        public bool Sublinhado { get; set; }
        public Util.Util.AlinhamentoHorizontal AlinhamentoH { get; set; }
        public Util.Util.AlinhamentoVertical AlinhamentoV { get; set; }
        public int TempoRolagem { get; set; }
        public int TempoApresentacao { get; set; }
        public bool InverterLed { get; set; }
        public int Largura { get; set; }
        public int AlturaPainel { get; set; }
        public int LarguraPainel { get; set; }
        public List<Color[,]> listaBitMap { get; set; }

        public Texto() 
        {
            this.FonteWindows = false;
            this.FonteAnteriorWindows = false;
            this.AlinhamentoH = Util.Util.AlinhamentoHorizontal.Centralizado;
            this.AlinhamentoV = Util.Util.AlinhamentoVertical.Centro;
            this.InverterLed = false;
            this.Apresentacao = Util.Util.Rolagem.Fixa;
            this.TempoRolagem = 26;
            this.TempoApresentacao = 2000;
            this.listaBitMap = new List<Color[,]>();
        }

        public Texto(string texto):this()
        {
            this.LabelTexto = texto;
        }

        public Texto(Texto texto_antigo)
        {
            this.LabelTexto = texto_antigo.LabelTexto;
            this.Apresentacao = texto_antigo.Apresentacao;
            this.Fonte = texto_antigo.Fonte;
            this.FonteWindows = texto_antigo.FonteWindows;
            this.FonteAnteriorWindows = texto_antigo.FonteAnteriorWindows;
            this.Altura = texto_antigo.Altura;
            this.Largura = texto_antigo.Largura;
            this.Negrito = texto_antigo.Negrito;
            this.Italico = texto_antigo.Italico;
            this.AlinhamentoH = texto_antigo.AlinhamentoH;
            this.AlinhamentoV = texto_antigo.AlinhamentoV;
            this.TempoRolagem = texto_antigo.TempoRolagem;
            this.TempoApresentacao = texto_antigo.TempoApresentacao;
            this.InverterLed = texto_antigo.InverterLed;
            this.Sublinhado = texto_antigo.Sublinhado;
            this.listaBitMap = new List<Color[,]>();
            foreach (Color[,] t in texto_antigo.listaBitMap)
            {
                this.listaBitMap.Add(t);
            }
            this.BinaryThreshold = texto_antigo.BinaryThreshold;
            this.AlturaPainel = texto_antigo.AlturaPainel;
            this.LarguraPainel = texto_antigo.LarguraPainel;
        }

        public String Fonte
        {
            get { return this._fonte ; }
            set { this._fonte = value; }
        }
        public String CaminhoFonte
        {
            get { return Fachada.diretorio_fontes + this._fonte + Util.Util.ARQUIVO_EXT_FNT; }        
        }



        public bool CompararObjetosTexto(Texto texto1, Texto texto2)
        {
            bool alterou = false;


            if ((texto1.LabelTexto != texto2.LabelTexto) || (texto1.Apresentacao != texto2.Apresentacao) || (texto1.Fonte != texto2.Fonte) || (texto1.FonteWindows != texto2.FonteWindows) || (texto1.Negrito != texto2.Negrito) ||
                (texto1.Altura != texto2.Altura) || (texto1.Italico != texto2.Italico) || (texto1.AlinhamentoV != texto2.AlinhamentoV) || (texto1.AlinhamentoH != texto2.AlinhamentoH) ||
                (texto1.TempoRolagem != texto2.TempoRolagem) || (texto1.TempoApresentacao != texto2.TempoApresentacao) || (texto1.InverterLed != texto2.InverterLed) || (texto1.Sublinhado != texto2.Sublinhado) ||
                (texto1.BinaryThreshold!=texto2.BinaryThreshold))
            {

                alterou = true;
                if (((texto1.LabelTexto == texto2.LabelTexto) && (texto1.Apresentacao == texto2.Apresentacao) && (texto1.Fonte == texto2.Fonte) && (texto1.FonteWindows == texto2.FonteWindows) && (texto1.Negrito == texto2.Negrito) &&
                    (texto1.Altura == texto2.Altura) && (texto1.Italico == texto2.Italico) && (texto1.AlinhamentoV == texto2.AlinhamentoV) && (texto1.TempoRolagem == texto2.TempoRolagem) && 
                    (texto1.TempoApresentacao == texto2.TempoApresentacao) && (texto1.InverterLed == texto2.InverterLed) && (texto1.Sublinhado == texto2.Sublinhado) &&
                    (texto1.BinaryThreshold == texto2.BinaryThreshold)) && (texto1.AlinhamentoH == Util.Util.AlinhamentoHorizontal.Esquerda) && (texto1.listaBitMap.Count>1) && (texto2.listaBitMap.Count == 0))
                {
                    alterou = false;
                }
            }
            
            return alterou;
        }

        private void SetarLargura()
        {
            Arquivo_FNT fonte = new Arquivo_FNT();
            List<Boolean[,]> Texto = new List<bool[,]>();            

            if (this.FonteWindows)
            {
                if (!String.IsNullOrEmpty(this.LabelTexto.Trim()))
                {
                    /* Código Antigo*/
                    //Texto.Add(StrToMatrixTrueType(this));
                    Texto.Add(Nucleo.ProcessamentoDeImagens.StrToMatrixTrueType(this.LabelTexto, this.Fonte, this.Altura, this.Italico, this.Negrito, this.Sublinhado, this.BinaryThreshold, this.AlinhamentoV, this.AlinhamentoH));
                }
            }
            else
            {
                //procura a fonte.
                fonte = Nucleo.ProcessamentoDeImagens.RetornaFonte(this.CaminhoFonte);

                Texto = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.LabelTexto, fonte);
            }

            int largura = 0;
            //int altura = 0;
            foreach (Boolean[,] caractere in Texto)
            {
                largura = largura + caractere.GetLength(1);
                //if (caractere.GetLength(0) > altura)
                    //altura = caractere.GetLength(0);
            }

            this.Largura = largura;
           // this.Altura = altura;
            
        }
        
        /* Código Antigo*/
        ////TODO: retirar a função de frase
        //private Boolean[,] StrToMatrixTrueType(Texto texto)
        //{
        //    System.Drawing.Bitmap bTemp;
        //    Boolean[,] Result;
        //    Color textColor = Color.FromArgb(0, 0, 0); //cor do texto
        //    Color backColor = Color.FromArgb(255, 255, 255); //cor da fonte
        //    FontStyle fsTexto = new FontStyle();

        //    fsTexto = FontStyle.Regular;

        //    InstalledFontCollection ifc = new InstalledFontCollection();
        //    for (int i = 0; i < ifc.Families.Length; i++)
        //    {
        //        if (ifc.Families[i].Name == texto.Fonte)
        //        {
        //            if (ifc.Families[i].IsStyleAvailable(FontStyle.Regular))
        //            {
        //                fsTexto = FontStyle.Regular;
        //            }
        //            else if (ifc.Families[i].IsStyleAvailable(FontStyle.Bold))
        //            {
        //                fsTexto = FontStyle.Bold;
        //            }
        //            else if (ifc.Families[i].IsStyleAvailable(FontStyle.Italic))
        //            {
        //                fsTexto = FontStyle.Italic;
        //            }
        //            else if (ifc.Families[i].IsStyleAvailable(FontStyle.Underline))
        //            {
        //                fsTexto = FontStyle.Underline;
        //            }
        //            break;
        //        }
        //    }

        //    if (texto.Italico)
        //        fsTexto = fsTexto | FontStyle.Italic;

        //    if (texto.Negrito)
        //        fsTexto = fsTexto | FontStyle.Bold;

        //    if (texto.Sublinhado)
        //        fsTexto = fsTexto | FontStyle.Underline;



        //    //converte o bitmap diretamente usando o método estático
        //    bTemp = TextImage.MakeTextBitmap(texto.LabelTexto, new Font(texto.Fonte, texto.Altura, fsTexto), textColor, backColor, this.BinaryThreshold);
        //    Result = new Boolean[bTemp.Height, bTemp.Width];

        //    //for (int altura = 0; altura < bTemp.Height; altura++)            
        //    for (int altura = bTemp.Height - 1; altura >= 0; altura--)
        //    {
        //        for (int largura = 0; largura < bTemp.Width; largura++)
        //        {
        //            //if (bTemp.GetPixel(largura, altura) == Color.White)
        //            if (VerificaBranco(bTemp.GetPixel(largura, bTemp.Height - altura - 1)))
        //                Result[altura, largura] = false;
        //            else
        //                Result[altura, largura] = true;
        //        }
        //    }

        //    if (Result.LongLength == 1)
        //    {
        //        Result[0, 0] = false;
        //    }

        //    //temporário para caso se precise fazer alguma operação intermediária.
        //    Boolean[,] texto_temp = new bool[bTemp.Height, bTemp.Width];

        //    //desenha no temporário.
        //    DrawMatrix(texto_temp, Result, 0, 0);

        //    //todo: verificar tamanho do painel. desenho ainda não está desenhado no painel.
        //    //AplicarAlinhamento(texto.AlinhamentoH, texto.AlinhamentoV, texto_temp);

        //    //EstufaBits(ref v02, texto_temp);

        //    return texto_temp;
        //}

        //TODO: retirar de frase
        /// <summary>
        /// Lê de desenho e grava em Matrix(Versão Array).
        /// (O desenho sofrerá crop de acordo com as dimensões de matrix)
        /// </summary>
        /// <param name="Matriz">Painel Pontos.</param>
        /// <param name="Desenho">Letra da fonte lighdot.</param>
        /// <param name="origem_linha">x - abscissa do início do desenho.</param>
        /// <param name="origem_coluna">y - ordenada do início do desenho.</param>
        private void DrawMatrix(Boolean[,] Matrix, Boolean[,] Desenho, int origem_linha, int origem_coluna)
        {
            for (int i = 0; i < Desenho.GetLength(0); i++)
            {
                //for (int j = Desenho.GetLength(1) - 1; j >= 0 ; j--)
                for (int j = 0; j < Desenho.GetLength(1); j++)
                {
                    if (((j + origem_coluna) >= Matrix.GetLength(1)) || ((i + origem_linha) >= Matrix.GetLength(0))) continue;

                    Matrix[i + origem_linha, j + origem_coluna] = Desenho[i, j];
                    //Matrix[i + origem_linha, j + origem_coluna] = Desenho[i, j];
                }
            }
        }

        //TODO: retirar de frase
        private static Boolean VerificaBranco(Color c)
        {
            return ((c.A == 0xFF) & (c.R == 0xFF) & (c.G == 0xFF) & (c.B == 0xFF));
        }

        public bool ChecarFontes(string diretorioFonte)
        {
            bool achouFonte = false;
            bool setouFonte = false;
            string fonteNova = "";

            if (!this.FonteWindows) //Se for fonte do Pontos
            {
                
                List<string> dir_fontes = Directory.EnumerateFiles(diretorioFonte, "*" + Util.Util.ARQUIVO_EXT_FNT).ToList();

                dir_fontes.Reverse();

                // tenta recuperar a fonte que estava carregada no controlador
                foreach (string nome_fonte in dir_fontes)
                {
                    if (this.Fonte == Path.GetFileNameWithoutExtension(nome_fonte))
                    {
                        achouFonte = true;
                        break;
                    }
                }

                //tenta recuperar a fonte padrão do Pontos para cada altura de texto
                if (!achouFonte)
                {
                    switch (this.Altura)
                    {
                        case 16:
                            if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_16 + Util.Util.ARQUIVO_EXT_FNT))
                            {
                                this.Fonte = Util.Util.FONTE_DEFAULT_16;
                                this.SetarLargura();
                                return false;
                            }
                            break;
                        case 14:
                            if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_14 + Util.Util.ARQUIVO_EXT_FNT))
                            {
                                this.Fonte = Util.Util.FONTE_DEFAULT_14;
                                this.SetarLargura();
                                return false;
                            }
                            break;
                        case 13:
                            if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_13 + Util.Util.ARQUIVO_EXT_FNT))
                            {
                                this.Fonte = Util.Util.FONTE_DEFAULT_13;
                                this.SetarLargura();
                                return false;
                            }
                            break;
                        case 11:
                            if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_11 + Util.Util.ARQUIVO_EXT_FNT))
                            {
                                this.Fonte = Util.Util.FONTE_DEFAULT_11;
                                this.SetarLargura();
                                return false;
                            }
                            break;
                        case 8:
                            if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_8 + Util.Util.ARQUIVO_EXT_FNT))
                            {
                                this.Fonte = Util.Util.FONTE_DEFAULT_8;
                                this.SetarLargura();
                                return false;
                            }
                            break;
                        case 7:
                            if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_7 + Util.Util.ARQUIVO_EXT_FNT))
                            {
                                this.Fonte = Util.Util.FONTE_DEFAULT_7;
                                this.SetarLargura();
                                return false;
                            }
                            break;
                        case 5:
                            if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_5 + Util.Util.ARQUIVO_EXT_FNT))
                            {
                                this.Fonte = Util.Util.FONTE_DEFAULT_5;
                                this.SetarLargura();
                                return false;
                            }
                            break;

                    }
                }

                //se o usuário exclui a fonte pre carregada no controlador e também a fonte padrão para cada altura do texto, o sistema irá buscar a primeira fonte com a mesma altura
                if (!achouFonte)
                {
                                       
                    int alturaNova = Util.Util.ALTURA_MAXIMA_PAINEL;                    

                    //buscando a primeira fonte com a mesma altura
                    foreach (string nome_fonte in dir_fontes)
                    {
                        Arquivo_FNT fonte = new Arquivo_FNT();

                        fonte.AbrirSimples(nome_fonte);

                        if (this.Altura == fonte.Altura)
                        {
                            this.Fonte = Path.GetFileNameWithoutExtension(nome_fonte);
                            this.Altura = (int)fonte.Altura;
                            this.SetarLargura();
                            setouFonte = true;
                            break;
                        }

                    }

                    if (!setouFonte)
                    {
                        
                        foreach (string nome_fonte in dir_fontes)
                        {
                            Arquivo_FNT fonte = new Arquivo_FNT();

                            fonte.AbrirSimples(nome_fonte);

                            // Pega a primeira fonte menor que a altura anterior
                            if (this.Altura >= fonte.Altura)
                            {
                                this.Fonte = Path.GetFileNameWithoutExtension(nome_fonte);
                                this.Altura = (int)fonte.Altura;
                                this.SetarLargura();
                                setouFonte = true;
                                break;
                            }
                            else
                            {
                                // Vai setando a fonte imediatamente superior ao tamanho anterior se não achar nenhuma menor
                                if (alturaNova>=(int)fonte.Altura)
                                {
                                    alturaNova = (int)fonte.Altura;
                                    fonteNova = Path.GetFileNameWithoutExtension(nome_fonte);
                                }
                            }
                        }
                    }
                    if (!setouFonte)
                    {
                        //Seta a fonte imediatamente superior se não achar nenhuma fonte de mesma altura ou menor
                        this.Altura = alturaNova;
                        this.Fonte = fonteNova;
                        this.SetarLargura();
                    }
                }
               
            }
            else // Se for fonte TrueType
            {
                InstalledFontCollection insFont = new InstalledFontCollection();
                FontFamily[] families = insFont.Families;

                foreach (FontFamily family in families)
                {
                    if (this.Fonte == family.Name)
                    {
                        achouFonte = true;
                        break;
                    }
                }

                if (!achouFonte)
                {
                    fonteNova = "";
                    foreach (FontFamily family in families)
                    {
                        if (fonteNova=="")
                            fonteNova = family.Name;
                        
                        if (Util.Util.FONTE_DEFAULT_WINDOWS == family.Name)
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_WINDOWS;
                            this.SetarLargura();
                            setouFonte = true;
                            break;
                        }
                    }

                    if (!setouFonte)
                    {
                        //Seta a fonte imediatamente superior se não achar nenhuma fonte de mesma altura ou menor
                        this.Fonte = fonteNova;
                        this.SetarLargura();
                    }
                }
            }

            return achouFonte;
        }

        public void SetarArquivoFonteDefaultTexto(int altura, string diretorioFonte)
        {
            //Se for fonte Pontos
            if (!this.FonteWindows)
            {

                bool setouFonte = false;               

                //tenta recuperar a fonte padrão do Pontos para cada altura de texto
                switch (altura)
                {
                    case 26:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_26 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_26;
                            this.Altura = altura;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;
                    case 16:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_16 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_16;
                            this.Altura = altura;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;
                    case 14:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_14 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_14;
                            this.Altura = altura;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;
                    case 13:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_13 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_13;
                            this.Altura = altura;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;
                    case 11:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_11 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_11;
                            this.Altura = altura;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;
                    case 9:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_8 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_8;
                            // painel de altura 9 são os dois inferiores do texto triplo. A fonte é de altura 8
                            this.Altura = 8;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;
                    case 8:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_8 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_8;
                            this.Altura = altura;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;
                    case 7:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_7 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_7;
                            this.Altura = altura;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;
                    case 5:
                        if (File.Exists(diretorioFonte + Util.Util.FONTE_DEFAULT_5 + Util.Util.ARQUIVO_EXT_FNT))
                        {
                            this.Fonte = Util.Util.FONTE_DEFAULT_5;
                            this.Altura = altura;
                            this.SetarLargura();
                            setouFonte = true;
                        }
                        break;

                }



                if (!setouFonte)
                {

                    //Listando as fontes do diretorio de fontes em programData
                    List<string> dir_fontes = Directory.EnumerateFiles(diretorioFonte, "*" + Util.Util.ARQUIVO_EXT_FNT).ToList();

                    //Invertendo a ordem da lista para as fontes maiores aparecem primeiro
                    dir_fontes.Reverse();

                    int alturaNova = Util.Util.ALTURA_MAXIMA_PAINEL;
                    string fonteNova = "";

                    //buscando a primeira fonte com a mesma altura
                    foreach (string nome_fonte in dir_fontes)
                    {
                        Arquivo_FNT fonte = new Arquivo_FNT();

                        fonte.AbrirSimples(nome_fonte);

                        if (altura == fonte.Altura)
                        {
                            this.Fonte = Path.GetFileNameWithoutExtension(nome_fonte);
                            this.Altura = (int)fonte.Altura;
                            this.SetarLargura();
                            setouFonte = true;
                            break;
                        }

                    }

                    if (!setouFonte)
                    {

                        foreach (string nome_fonte in dir_fontes)
                        {
                            Arquivo_FNT fonte = new Arquivo_FNT();

                            fonte.AbrirSimples(nome_fonte);

                            // Pega a primeira fonte menor que a altura anterior
                            if (altura >= fonte.Altura)
                            {
                                this.Fonte = Path.GetFileNameWithoutExtension(nome_fonte);
                                this.Altura = (int)fonte.Altura;
                                this.SetarLargura();
                                setouFonte = true;
                                break;
                            }
                            else
                            {
                                // Vai setando a fonte imediatamente superior ao tamanho anterior se não achar nenhuma menor
                                if (alturaNova >= (int)fonte.Altura)
                                {
                                    alturaNova = (int)fonte.Altura;
                                    fonteNova = Path.GetFileNameWithoutExtension(nome_fonte);
                                }
                            }
                        }
                    }
                    if (!setouFonte)
                    {
                        //Seta a fonte imediatamente superior se não achar nenhuma fonte de mesma altura ou menor
                        this.Altura = alturaNova;
                        this.Fonte = fonteNova;
                        this.SetarLargura();
                    }
                }
            }
            else  //Fonte windows
            {
                //buscando fontes instaladas no windows para setar no texto
                string primeraFonte = "";
                string textoFonte = "";
                string padraoFonte = "";
                InstalledFontCollection insFont = new InstalledFontCollection();
                FontFamily[] families = insFont.Families;

                foreach (FontFamily family in families)
                {
                    if (this.Fonte == family.Name)
                    {
                        textoFonte = family.Name;
                    }

                    if (Util.Util.FONTE_DEFAULT_WINDOWS == family.Name)
                    {
                        padraoFonte = family.Name;
                    }

                    if (primeraFonte == "")
                        primeraFonte = family.Name;

                }

                if (this.FonteAnteriorWindows==this.FonteWindows)
                {
                    if (altura < 5)
                        this.Altura = 5;
                    else
                        this.Altura = altura;

                    if (textoFonte=="")
                    {
                        if (padraoFonte == "")
                            this.Fonte = primeraFonte;
                        else
                            this.Fonte = padraoFonte;
                    }
                    

                }
                if (this.FonteAnteriorWindows!=this.FonteWindows)
                {
                    if (padraoFonte=="")
                        this.Fonte = primeraFonte;
                    else
                        this.Fonte = padraoFonte;

                    if (altura!=this.Altura)
                    {
                        if (altura < 5)
                            this.Altura = 5;
                        else
                            this.Altura = altura;
                    }
                }


                this.SetarLargura();
            }
        }

        public int AlturaFonte(string fonte, string diretorioFonte)
        {
            Arquivo_FNT font = new Arquivo_FNT();

            font.Abrir(diretorioFonte + font + Util.Util.ARQUIVO_EXT_FNT);

            return (int)font.Altura;
        }

        

        public void CriarListaBitMap(Boolean[,] Painel, int altura, int largura)
        {
            List<Color[,]> listaCores = new List<Color[,]>();

            double paineis = (double)Painel.GetLength(1) / (double)largura;
            int quantidadePaineis = Convert.ToInt32(Math.Ceiling(paineis));

            for (int z = 0; z < quantidadePaineis; z++)
            {
                listaCores.Add(new Color[altura, largura]);

                for (int i = Painel.GetLength(0) - 1; i >= 0; i--)
                {
                    int j = 0;
                    while (j < largura)
                    {
                        if (z > 0)
                        {
                            if ((largura * z) + j > Painel.GetLength(1) - 1)
                                listaCores[z][i, j] = Color.Black;
                            else
                                if (Painel[i, (largura * z) + j] == true)
                                    listaCores[z][i, j] = Color.Yellow;
                                else
                                    listaCores[z][i, j] = Color.Black;
                        }
                        else
                            if (Painel[i, j] == true)
                                listaCores[z][i, j] = Color.Yellow;
                            else
                                listaCores[z][i, j] = Color.Black;

                        j++;
                    }

                }
            }
            // Implementação para remover a coluna vazia da última imagem
            if (Painel.GetLength(1) % (double)largura == 1)
            {
                bool achouAceso = false;
                for (int i = 0; i < altura; i++)
                {
                    achouAceso = listaCores[listaCores.Count - 1][0, i] == Color.Yellow;
                    if (achouAceso)
                        break;
                }
                if ((!achouAceso) && (listaCores.Count > 1))
                {
                    listaCores.RemoveAt(listaCores.Count - 1);
                }
            }
            this.listaBitMap = listaCores;
        }
    }
}
