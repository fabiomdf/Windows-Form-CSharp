using Persistencia;
using Persistencia.Videos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using Nucleo;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Controlador
{
    public class Frase
    {
        public int Indice { get; set; }
        public string LabelFrase { get; set; }
        public bool TextoAutomatico { get; set; }
        public Util.Util.TipoVideo TipoVideo { get; set; }
        public Modelo Modelo { get; set; }


        public Frase() {
            this.Modelo = new Modelo();
            this.TextoAutomatico = true;
            this.TipoVideo = Util.Util.TipoVideo.V02;
            ProcessamentoDeImagens.diretorioBitmap =  Fachada.diretorio_imagens;
        }

        public Frase(string numeroLabel)
        {
            this.Modelo = new Modelo(numeroLabel);
            this.TextoAutomatico = true;
            this.TipoVideo = Util.Util.TipoVideo.V02;
            this.LabelFrase = numeroLabel;
            ProcessamentoDeImagens.diretorioBitmap = Fachada.diretorio_imagens;
        }

        public Frase(Frase frase_antiga) 
        {
            this.Indice = frase_antiga.Indice;
            this.LabelFrase = frase_antiga.LabelFrase;
            this.TextoAutomatico = frase_antiga.TextoAutomatico;
            this.Modelo = new Modelo(frase_antiga.Modelo);
            this.TipoVideo = frase_antiga.TipoVideo;
            ProcessamentoDeImagens.diretorioBitmap = Fachada.diretorio_imagens;
        }

        public Boolean VerificarLarguraMinimaFrase()
        {
            Boolean retorno = true;

            foreach(Texto t in this.Modelo.Textos)
            {
                if (t.Largura < 5)
                    retorno = false;
            }

            return retorno;
        }




        public bool CompararObjetosFrase(Frase frase1, Frase frase2)
        {
            bool alterou = false;

            if ((frase1.Indice != frase2.Indice) || (frase1.LabelFrase != frase2.LabelFrase) || (frase1.TextoAutomatico != frase2.TextoAutomatico) || (frase1.TipoVideo != frase2.TipoVideo))
                alterou = true;

            if (!alterou)
            {
                if (frase1.Modelo.CompararObjetosModelo(frase1.Modelo, frase2.Modelo))
                    alterou = true;
            }

            return alterou;
        }

        public void CalculaLarguraFrase()
        {
            
            Arquivo_FNT fonte = new Arquivo_FNT();
            List<Boolean[,]> Texto = new List<bool[,]>();
            
            if(this.Modelo.Textos != null)
            for (int i = 0; i < this.Modelo.Textos.Count; i++)
            {
                if (this.Modelo.Textos[i].FonteWindows)
                {
                    if (!String.IsNullOrEmpty(this.Modelo.Textos[i].LabelTexto))
                    {
                        Texto.Add(StrToMatrixTrueType(this.Modelo.Textos[i]));
                    }

                    int largura = 0;
                    foreach (Boolean[,] caractere in Texto)
                    {
                        largura = largura + caractere.GetLength(1);

                    }
                    this.Modelo.Textos[i].Largura = largura;
                }
                else
                {
                    //procura a fonte.
                    fonte = Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[i].CaminhoFonte);

                    Texto = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[i].LabelTexto, fonte);

                    int largura = 0;
                    int altura = 0;
                    foreach (Boolean[,] caractere in Texto)
                    {
                        largura = largura + caractere.GetLength(1);
                        if (caractere.GetLength(0) > altura)
                         altura = caractere.GetLength(0);
                    }

                    this.Modelo.Textos[i].Largura = largura;
                    this.Modelo.Textos[i].Altura = altura;
                }

                Texto.Clear();
               
            }
        }

        public int CalcularLarguraTotal()
        {
            int largura = 0;

            
            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
            {
                largura = this.Modelo.Textos[0].Largura;
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero)
            { 
                for (int i = 0; i < this.Modelo.Textos.Count; i++)                
                    largura = largura + this.Modelo.Textos[i].Largura;
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
            {
                if (this.Modelo.Textos[0].Largura > this.Modelo.Textos[1].Largura)
                    largura = this.Modelo.Textos[0].Largura;
                else
                    largura = this.Modelo.Textos[1].Largura;
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo)
            {
                if (this.Modelo.Textos[2].Largura > this.Modelo.Textos[1].Largura)
                    largura = this.Modelo.Textos[0].Largura + this.Modelo.Textos[2].Largura;
                else
                    largura = this.Modelo.Textos[0].Largura + this.Modelo.Textos[1].Largura;
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero)
            {
                if (this.Modelo.Textos[2].Largura > this.Modelo.Textos[0].Largura)
                    largura = this.Modelo.Textos[1].Largura + this.Modelo.Textos[2].Largura;
                else
                    largura = this.Modelo.Textos[1].Largura + this.Modelo.Textos[0].Largura;
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo)
            {
                int larguraColuna1 = Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[2].Largura);
                int larguraColuna2 = Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[3].Largura);

                largura = larguraColuna1 + larguraColuna2;
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
            {
                largura = Math.Max(Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[1].Largura),this.Modelo.Textos[2].Largura);
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
            {
                largura = this.Modelo.Textos[0].Largura + Math.Max(Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[2].Largura), this.Modelo.Textos[3].Largura);
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
            {
                largura = this.Modelo.Textos[1].Largura + Math.Max(Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[2].Largura), this.Modelo.Textos[3].Largura);
            }

            return largura;  
        }

       public void Salvar(string nomeArquivo, uint altura, uint largura)
       {
            switch (this.TipoVideo)
            {
                case Util.Util.TipoVideo.V01: break;
                case Util.Util.TipoVideo.V02:
                    {
                        GerarVideo02(nomeArquivo, altura, largura);
                    }
                    break;
                case Util.Util.TipoVideo.V03: break;
                case Util.Util.TipoVideo.V04:
                    {
                        GerarVideo04(nomeArquivo, altura, largura);
                    }
                    break;
                case Util.Util.TipoVideo.PLS: break;
            }
        }

        private void GerarVideo04(string nomeArquivo, uint altura, uint largura)
        {
            string diretorio_raiz = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string diretorio_temporario = diretorio_raiz +
                                     Util.Util.DIRETORIO_APP +
                                     Util.Util.DIRETORIO_TEMPORARIO +
                                     Util.Util.DIRETORIO_VIDEOS;

            VideoV04 v04 = new VideoV04();

            PrepararPixelBytes(ref v04, altura, largura);

            v04.Salvar(nomeArquivo);
        }

        private void GerarVideo02(string nomeArquivo, uint altura, uint largura)
        {
            string diretorio_raiz = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);            
            string diretorio_temporario = diretorio_raiz +
                                     Util.Util.DIRETORIO_APP +
                                     Util.Util.DIRETORIO_TEMPORARIO + 
                                     Util.Util.DIRETORIO_VIDEOS;

            VideoV02 v02 = new VideoV02();
            v02.Altura = altura;
            v02.Largura = largura;
            
            v02.tempoApresentacao = (uint)this.Modelo.Textos[0].TempoApresentacao;
            v02.tempoRolagem = (uint)this.Modelo.Textos[0].TempoRolagem;
            v02.animacao = (byte)this.Modelo.Textos[0].Apresentacao;
            
            PrepararPixelBytes(ref v02, (int)altura, (int) largura);

            //Colocado abaixo de PrepararPixelBytes pois o alinhamento horizontal pode ser alterado a esquerda pela função PrepararMatrizFrase
            v02.alinhamento = (byte)this.Modelo.Textos[0].AlinhamentoH;

            v02.Salvar(nomeArquivo);
        }

        public void GerarVideo02ApenasImagem(string nomeArquivo, uint altura)
        {

            VideoV02 v02 = new VideoV02();

            v02.tempoApresentacao = (uint)this.Modelo.Textos[0].TempoApresentacao;
            v02.tempoRolagem = (uint)this.Modelo.Textos[0].TempoRolagem;
            v02.animacao = (byte)this.Modelo.Textos[0].Apresentacao;
            v02.alinhamento = (byte)this.Modelo.Textos[0].AlinhamentoH;

            PrepararPixelBytesApenasImagem(ref v02, (int)altura);

            v02.Salvar(nomeArquivo);
        }

        public void SetarFontesDefaultTextos(int altura, string diretorioFonte) 
        {

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.Texto || this.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero)
            {
                for (int i = 0; i < this.Modelo.Textos.Count; i++)
                    this.Modelo.Textos[i].SetarArquivoFonteDefaultTexto(altura, diretorioFonte); 
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
            {
                for (int i = 0; i < this.Modelo.Textos.Count; i++)
                    this.Modelo.Textos[i].SetarArquivoFonteDefaultTexto(altura/2, diretorioFonte);
            }

            
            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo)
            {
                this.Modelo.Textos[0].SetarArquivoFonteDefaultTexto(altura, diretorioFonte);
                this.Modelo.Textos[1].SetarArquivoFonteDefaultTexto(altura/2, diretorioFonte);
                this.Modelo.Textos[2].SetarArquivoFonteDefaultTexto(altura/2, diretorioFonte);
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero)
            {
                this.Modelo.Textos[0].SetarArquivoFonteDefaultTexto(altura/2, diretorioFonte);
                this.Modelo.Textos[1].SetarArquivoFonteDefaultTexto(altura, diretorioFonte);
                this.Modelo.Textos[2].SetarArquivoFonteDefaultTexto(altura/2, diretorioFonte);
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo)
            {
                for (int i = 0; i < this.Modelo.Textos.Count; i++)
                    this.Modelo.Textos[i].SetarArquivoFonteDefaultTexto(altura/2, diretorioFonte);
            }  
            
            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
            {
                if (altura == Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO)
                {
                    this.Modelo.Textos[0].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1, diretorioFonte);
                    this.Modelo.Textos[1].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2, diretorioFonte);
                    this.Modelo.Textos[2].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3, diretorioFonte);
        }
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
            {
                if (altura == Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO)
                {
                    this.Modelo.Textos[0].SetarArquivoFonteDefaultTexto(altura, diretorioFonte);
                    this.Modelo.Textos[1].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1, diretorioFonte);
                    this.Modelo.Textos[2].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2, diretorioFonte);
                    this.Modelo.Textos[3].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3, diretorioFonte);
                }
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
            {
                if (altura == Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO)
                {
                    this.Modelo.Textos[0].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1, diretorioFonte);
                    this.Modelo.Textos[1].SetarArquivoFonteDefaultTexto(altura, diretorioFonte);
                    this.Modelo.Textos[2].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2, diretorioFonte);
                    this.Modelo.Textos[3].SetarArquivoFonteDefaultTexto(Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3, diretorioFonte);
                }
            }
        }

        public bool ChecarFontes(string diretorioFonte)
        {
            bool achouFonte = true;

            for (int i = 0; i < this.Modelo.Textos.Count; i++)
            {
                if (!this.Modelo.Textos[i].ChecarFontes(diretorioFonte))
                    achouFonte = false;
                    
            }

            return achouFonte;
        }

        public int GetAlturaTexto(int altura, int textoSelecionado)
        {
            int retornoAltura = altura;

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
                retornoAltura = altura / 2;


            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo)
            {
                switch (textoSelecionado) 
                {
                    case 0: retornoAltura = altura; break;
                    case 1: retornoAltura = altura/2; break;
                    case 2: retornoAltura = altura/2; break;
                }
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero)
            {
                switch (textoSelecionado)
                {
                    case 0: retornoAltura = altura / 2; break;
                    case 1: retornoAltura = altura; break;
                    case 2: retornoAltura = altura / 2; break;
                }
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo)
            {
                retornoAltura = altura / 2;
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
            {
                switch (textoSelecionado)
                {
                    case 0: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1; break;
                    case 1: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2; break;
                    case 2: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3; break;
                }
            }

            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
            {
                switch (textoSelecionado)
                {
                    case 0: retornoAltura = altura; break;
                    case 1: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1; break;
                    case 2: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2; break;
                    case 3: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3; break;
                }
            }


            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
            {
                switch (textoSelecionado)
                {
                    case 0: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1; break;
                    case 1: retornoAltura = altura; break;                    
                    case 2: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2; break;
                    case 3: retornoAltura = Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3; break;
                }
            }

            return retornoAltura;
        }

        private Boolean[] UnidimensionaMatriz(Boolean[,] matriz)
        {
            int numero_elementos = matriz.GetLength(0) * matriz.GetLength(1);
            Boolean[] Result = new Boolean[numero_elementos];

            int indice_result = 0;

            for (int coluna = 0; coluna < matriz.GetLength(1); coluna++)
            {
                for (int linha = 0; linha < matriz.GetLength(0); linha++)
                {
                    Result[indice_result] = matriz[linha, coluna];
                    indice_result = indice_result + 1;
                }
            }

            return Result;
        }       
        

        private Boolean[,] StrToMatrixTrueType(Texto texto)
        {
            return ProcessamentoDeImagens.StrToMatrixTrueType(texto.LabelTexto, texto.Fonte, texto.Altura, texto.Italico, texto.Negrito, texto.Sublinhado, texto.BinaryThreshold, texto.AlinhamentoV, texto.AlinhamentoH);            
        }

        private static Boolean VerificaBranco(Color c)
        {
            return ((c.A == 0xFF) & (c.R == 0xFF) & (c.G == 0xFF) & (c.B == 0xFF));
        }

        /// <summary>
        /// Usada após processamento da imagem, para traduzir os bytes para hardware.
        /// </summary>
        private void EstufaBits(ref Persistencia.Videos.VideoV02 v02,
                                       Boolean[,] Painel)       
        {
            v02.Altura = (uint)Painel.GetLength(0);
            v02.Largura = (uint)Painel.GetLength(1);

            long nelementos = 0;
            Boolean[] painelEmArray = new bool[nelementos];

            nelementos = Painel.GetLength(0) * Painel.GetLength(1);
            painelEmArray = UnidimensionaMatriz(Painel);

            //copia todos os elementos para pixelbytes            
            v02.pixelBytes = new byte[(nelementos % 8 == 0) ? (nelementos / 8) : (nelementos / 8) + 1];
            int indice_bmp = 0;
            byte aux = 0;
            for (int i = 0; i < painelEmArray.Length; i++)
            {
                try
                {
                    aux = (byte)(System.Convert.ToByte(painelEmArray[i]) << 0);
                    i++;
                    aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 1);
                    i++;
                    aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 2);
                    i++;
                    aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 3);
                    i++;
                    aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 4);
                    i++;
                    aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 5);
                    i++;
                    aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 6);
                    i++;
                    aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 7);

                    v02.pixelBytes[indice_bmp] = aux;

                    indice_bmp++;
                }
                catch
                {
                    v02.pixelBytes[indice_bmp] = aux;
                    indice_bmp++;
                    continue;
                }
            }

        }

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
                for (int j = 0; j < Desenho.GetLength(1); j++)
                {
                    if (((j + origem_coluna) >= Matrix.GetLength(1)) || ((i + origem_linha) >= Matrix.GetLength(0))) continue;

                    Matrix[i + origem_linha, j + origem_coluna] = Desenho[i, j];
                }
            }
        }
       
        private void DesenharNoPainel(List<Boolean[,]> conteudo, Boolean[,] Matrix, int origem_abs = 0, int origem_ord = 0)
        {
            int origem_proximo = 0;

            foreach (Boolean[,] caractere in conteudo)
            {
                if (origem_proximo == 0)
                    DrawMatrix(Matrix, caractere, origem_abs, origem_ord);
                else
                    DrawMatrix(Matrix, caractere, origem_abs, origem_ord + origem_proximo);

                origem_proximo = origem_proximo + caractere.GetLength(1);
            }
        }



        private Boolean[,] AplicarAlinhamento(Texto texto, Boolean[,] Matrix)
            {


            Util.Util.AlinhamentoHorizontal AlinhamentoHorizontal = texto.AlinhamentoH;
            Util.Util.AlinhamentoVertical AlinhamentoVertical = texto.AlinhamentoV;

            return ProcessamentoDeImagens.AplicarAlinhamento(AlinhamentoHorizontal, AlinhamentoVertical, texto.LabelTexto, texto.FonteWindows, texto.CaminhoFonte, Matrix);
            //int countEsquerda = 0;
            //int countDireita = 0;
            //int tamanhoEspaco = 3;

            //bool existePixels = false;
            //for (int largura = 0; largura < Matrix.GetLength(1); largura++)
            //{
            //    for (int altura = 0; altura < Matrix.GetLength(0); altura++)
            //    {
            //        existePixels = Matrix[altura, largura];
            //        if (existePixels)
            //        {
            //            break;
            //        }
            //    }
            //    if (existePixels)
            //    {
            //        break;
            //    }
            //}
            // if (texto.LabelTexto.StartsWith(" "))
            //{
            //    countEsquerda = texto.LabelTexto.Length - texto.LabelTexto.TrimStart().Length;
            //}
            //if (texto.LabelTexto.EndsWith(" "))
            //{
            //    countDireita = texto.LabelTexto.Length - texto.LabelTexto.TrimEnd().Length;
            //}
            //if (!texto.FonteWindows)            
            //{
            //    Arquivo_FNT afnt = new Arquivo_FNT();
            //    afnt.Abrir(texto.CaminhoFonte);
            //    // Verificar a quantidade de colunas que o espaço dessa fonte ocupa.
            //    tamanhoEspaco = afnt.bitmaps[0].matriz.GetLength(1);                
            //}
            //countEsquerda *= tamanhoEspaco;
            //countDireita *= tamanhoEspaco;

            //if (existePixels)
            //{
            //    Matrix = AplicarAlinhamentoHorizontal(AlinhamentoHorizontal, Matrix, countEsquerda, countDireita);
            //    Matrix = AplicarAlinhamentoVertical(AlinhamentoVertical, Matrix);
            //}

            //return Matrix;
            }

        private Boolean[,] AplicarAlinhamentoHorizontal(Util.Util.AlinhamentoHorizontal AlinhamentoH, Boolean[,] Matrix, int espacosEsquerda, int espacosDireita)
        {
            return ProcessamentoDeImagens.AplicarAlinhamentoHorizontal(AlinhamentoH, Matrix, espacosEsquerda, espacosDireita);
        }

        /// <summary>
        /// Retorna o índice da última coluna vazia à direita.
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        private int ColunasVaziasADireitaArray(Boolean[,] Matrix)
        {

            for (int coluna = Matrix.GetLength(1) - 1; coluna >= 0; coluna--)
            {
                for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                {
                    if (Matrix[linha, coluna] == true)
                        return coluna + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Retorna o índice da última coluna vazia à esquerda.
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        private int ColunasVaziasAEsquerdaArray(Boolean[,] Matrix)
        {
            for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
            {
                for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                {
                    if (Matrix[linha, coluna] == true)
                        return coluna;
                }
            }

            return -1;
        }

        /// <summary>
        /// Apaga uma coluna, gravando valores false.
        /// </summary>
        /// <param name="Matrix">Matriz a ser alterada.</param>
        /// <param name="indice_coluna"></param>
        private void ApagarColuna(Boolean[,] Matrix, int indice_coluna)
        {
            if (indice_coluna < Matrix.GetLength(1))
            {
                for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                    Matrix[linha, indice_coluna] = false;
            }
        }


        /// <summary>
        /// Copia a coluna indicada pelo indice_fonte para indice_alvo.
        /// </summary>
        /// <param name="Matrix">Matriz em arrays.</param>
        /// <param name="indice_fonte">indice da coluna a ser copiada.</param>
        /// <param name="indice_alvo">indice da coluna a ser substituída.</param>
        private void CopiarColuna(Boolean[,] Matrix, int indice_fonte, int indice_alvo)
        {
            if (indice_fonte < Matrix.GetLength(1))
            {
                for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                    Matrix[linha, indice_alvo] = Matrix[linha, indice_fonte];
            }
        }

        private void CopiarLinha(Boolean[,] Matrix, int indice_fonte, int indice_alvo)
        {
            if (indice_fonte < Matrix.GetLength(0))
            {
                for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
                     Matrix[indice_alvo, coluna] = Matrix[indice_fonte, coluna];
            }
        }

        private void AlinharDireitaArray(Boolean[,] Matrix, int espacosDireita)
        {
            int indice_ultima_coluna = ColunasVaziasADireitaArray(Matrix);
            indice_ultima_coluna = indice_ultima_coluna + espacosDireita;

            int colunas_alvo = Matrix.GetLength(1) - 1;

            if (indice_ultima_coluna < Matrix.GetLength(1)-1) 
            { 
                for (int colunas = indice_ultima_coluna; colunas >= 0; colunas--)
                {
                    CopiarColuna(Matrix, colunas, colunas_alvo);
                    ApagarColuna(Matrix, colunas);
                    colunas_alvo = colunas_alvo - 1;
                }
            }
        }
        
        private void CentralizarHorizontalArray(Boolean[,] Matrix, int espacosEsquerda, int espacosDireita)
        {
            //calcula a largura do desenho.
            int inicio_desenho = 0;
            int fim_desenho = 0;
            int largura_desenho = 0;
            int origem_desenho = 0;

            inicio_desenho = ColunasVaziasAEsquerdaArray(Matrix);
            fim_desenho = ColunasVaziasADireitaArray(Matrix);
            inicio_desenho = inicio_desenho - espacosEsquerda;
            fim_desenho = fim_desenho + espacosDireita;

            largura_desenho = fim_desenho - inicio_desenho;
            origem_desenho = (Matrix.GetLength(1) / 2) - (largura_desenho / 2);

            int coluna_temp = Matrix.GetLength(1) - origem_desenho;

            if (Matrix.GetLength(1) > largura_desenho + 1) //se não houver espaço, não há necessidade em tentar centralizar;
            {
                if (inicio_desenho < Matrix.GetLength(1) - fim_desenho) //deve-se ir para à direita.
                {
                    for (int colunas = fim_desenho; colunas >= inicio_desenho; colunas--)
                    {
                        coluna_temp = coluna_temp - 1;
                        if ((coluna_temp >= 0) & (colunas >= 0))
                            CopiarColuna(Matrix, colunas, coluna_temp);
                    }
                    //apaga lado esquerdo.
                    for (int i = 0; i < coluna_temp; i++)
                        ApagarColuna(Matrix, i);

                    return;
                }

                coluna_temp = origem_desenho;
                if (inicio_desenho > Matrix.GetLength(1) - fim_desenho) //deve-se ir à esquerda.
                {
                    for (int colunas = inicio_desenho; colunas <= fim_desenho; colunas++)
                    {
                        CopiarColuna(Matrix, colunas, coluna_temp);
                        coluna_temp = coluna_temp + 1;
                    }

                    //apaga lado direito.
                    for (int i = Matrix.GetLength(1) - 1; i > origem_desenho + (fim_desenho - inicio_desenho); i--)
                        ApagarColuna(Matrix, i);
                }
            }
        }

        private Boolean[,] AplicarAlinhamentoVertical(Util.Util.AlinhamentoVertical AlinhamentoV, Boolean[,] Matrix)
        {
            switch (AlinhamentoV)
            {
                case Util.Util.AlinhamentoVertical.Centro:
                    CentralizarVerticalArray(Matrix);
                    break;

                case Util.Util.AlinhamentoVertical.Baixo:
                    AlinharAoFundoArray(Matrix);
                    break;

                case Util.Util.AlinhamentoVertical.Cima:
                    AlinharAcimaArray(Matrix);
                    break;

            }

            return Matrix;
        }

        private void CentralizarVerticalArray(Boolean[,] Matrix)
        {
            int indice_superior = MaiorIndiceSuperiorArray(Matrix); //indice de onde começa o desenho (índice de 'baixo')
            int indice_inferior = MenorIndiceInferiorArray(Matrix); //indice do 'teto' do desenho;
            int altura_desenho = indice_inferior - indice_superior + 1;


            if (indice_superior > Matrix.GetLength(0) - indice_inferior - 1)
            {
                int origem_linha = ((Matrix.GetLength(0) / 2) - (altura_desenho / 2));
                int indice_desenho = indice_superior;
                int metade = origem_linha;

                for (int linha = 0; linha < altura_desenho; linha++)
                {
                    CopiarLinha(Matrix, indice_desenho, metade);
                    metade = metade + 1;
                    indice_desenho = indice_desenho + 1;
                }


                for (int i = altura_desenho + origem_linha; i < Matrix.GetLength(0); i++)
                {
                    ApagarLinha(Matrix, i);
                }

                for (int i = origem_linha - 1; i >= 0; i--)
                {
                    ApagarLinha(Matrix, i);
                }

            }

            if (indice_superior < Matrix.GetLength(0) - indice_inferior - 1)
            {
                int origem_linha = (Matrix.GetLength(0) / 2) + (altura_desenho / 2) - 1;

                int indice_desenho = indice_inferior;
                int metade = origem_linha;
                for (int linha = 0; linha < altura_desenho; linha++)
                {
                    CopiarLinha(Matrix, indice_desenho, metade);
                    metade = metade - 1;
                    indice_desenho = indice_desenho - 1;
                }

                for (int i = origem_linha + 1; i < Matrix.GetLength(0); i++)
                {
                    ApagarLinha(Matrix, i);
                }

                for (int i = metade; i >= 0; i--)
                {
                    ApagarLinha(Matrix, i);
                }
            }
        }

        /// <summary>
        /// Apaga uma linha, gravando valores false.
        /// </summary>
        /// <param name="Matrix">Matriz a ser alterada.</param>
        /// <param name="indice_linha">índice da linha.</param>
        private void ApagarLinha(Boolean[,] Matrix, int indice_linha)
        {
            for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
                Matrix[indice_linha, coluna] = false;
        }

        private int MenorIndiceInferiorArray(Boolean[,] Matrix)
        {
            for (int i = Matrix.GetLength(0) - 1; i >= 0; i--)
                for (int j = 0; j < Matrix.GetLength(1); j++)
                    if (Matrix[i, j] == true) return i;

            return -1;
        }

        private int MaiorIndiceSuperiorArray(Boolean[,] Matrix)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
                for (int j = 0; j < Matrix.GetLength(1); j++)
                    if (Matrix[i, j] == true) return i;

            return -1;
        }

        private void AlinharAcimaArray(Boolean[,] Matrix)
        {
            int indice_superior = MaiorIndiceSuperiorArray(Matrix);
            int indice_inferior = MenorIndiceInferiorArray(Matrix);
            int indice_linha = Matrix.GetLength(0) - 1;

            //para caso não precise processar.
            if (indice_inferior == Matrix.GetLength(0) - 1) return;

            for (int linha = indice_inferior; linha >= indice_superior; linha--)
            {
                CopiarLinha(Matrix, linha, indice_linha);
                indice_linha = indice_linha - 1;
            }

            for (int i = indice_linha; i >= 0; i--)
                ApagarLinha(Matrix, i);

        }

        private void AlinharAoFundoArray(Boolean[,] Matrix)
        {
            //int pontos_a_remover = deslocamento;
            int indice_superior = MaiorIndiceSuperiorArray(Matrix);
            int indice_inferior = MenorIndiceInferiorArray(Matrix);

            //para caso não precise processar.
            if (indice_superior == 0) return;

            int indice_linha = 0;
            for (int linha = indice_superior; linha <= indice_inferior; linha++)
            {
                CopiarLinha(Matrix, linha, indice_linha);
                indice_linha = indice_linha + 1;
            }

            for (int i = indice_linha; i < Matrix.GetLength(0); i++)
                ApagarLinha(Matrix, i);           
        }

        public void PrepararPixelBytes(ref VideoV02 v02, int altura, int largura)
        {
            Boolean[,] Painel = PrepararMatrizFrase(altura, largura);
            if (v02.animacao == (byte) Util.Util.Rolagem.Rolagem_Continua3_Esquerda)
            {
                if (this.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
                {
                    Painel = PrepararMatrixFrasev02ApenasImagem(altura);
                }
                else
                {
                    Painel = RetirarEspacosVaziosMatrix(Painel);
                }
                
            }

            if (Painel.GetLength(1) == largura + 1)
            {
                bool colunaVazia = true;

                for (int i = 0; i < Painel.GetLength(0); i++)
                {
                    if (Painel[i, largura] == true)
                    {
                        colunaVazia = false;
                        break;
                    }
                }
                if (colunaVazia)
                {
                    Painel = ProcessamentoDeImagens.ResizeArray(Painel, Painel.GetLength(0), largura);
                }
            }
            EstufaBits(ref v02, Painel);            
        }

        private bool[,] RetirarEspacosVaziosMatrix(bool[,] painel)
        {
            int larguraFinal = 0;
            int altura = painel.GetLength(0);
            int largura = painel.GetLength(1);            
            int quantidadeEspacosEsquerda = RetornarEspacosEsquerda(painel);
            int quantidadeEspacosDireita = RetornarEspacosDireita(painel);

            larguraFinal = largura - quantidadeEspacosEsquerda - quantidadeEspacosDireita;
            bool[,] retorno = new bool[altura, larguraFinal];

            for (int i=quantidadeEspacosEsquerda; i<largura-quantidadeEspacosDireita; i++)
                for (int j = 0; j<altura; j++)
                {
                    retorno[j, i - quantidadeEspacosEsquerda] = painel[j, i];
                }            

            return retorno;
        }

        private int RetornarEspacosEsquerda(bool[,] painel)
        {
            int altura = painel.GetLength(0);
            int largura = painel.GetLength(1);

            for (int i = 0; i < largura; i++)
                for (int j = 0; j < altura; j++)
                {
                    if (painel[j, i])
                    {
                        return i;
                    }
                }
            return 0;
        }

        private int RetornarEspacosDireita(bool[,] painel)
        {
            int altura = painel.GetLength(0);
            int largura = painel.GetLength(1);

            for (int i = largura - 1; i >0 ; i--)
                for (int j = 0; j < altura; j++)
                {
                    if (painel[j, i])
                    {
                        return (largura - i - 1);
                    }
                }
            return 0;
        }

        public void PrepararPixelBytesApenasImagem(ref VideoV02 v02, int altura)
        {
            Boolean[,] Painel = PrepararMatrixFrasev02ApenasImagem(altura);
            EstufaBits(ref v02, Painel);      
        }

        public Boolean[,] PrepararMatrixFrasev02ApenasImagem(int altura)
        {
            List<Boolean[,]> conteudoTexto1 = new List<bool[,]>();

            this.CalculaLarguraFrase();

            Boolean[,] Painel = new bool[altura, this.Modelo.Textos[0].Largura];

            if (this.Modelo.Textos[0].FonteWindows == true)
                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
            else
                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

            DesenharNoPainel(conteudoTexto1, Painel, 0, 0);

            return Painel;

        }

        public Boolean[,] PrepararMatrizFrase(int altura, int largura)
        {

            List<Boolean[,]> conteudoTexto1 = new List<bool[,]>();
            List<Boolean[,]> conteudoTexto2 = new List<bool[,]>();
            List<Boolean[,]> conteudoTexto3 = new List<bool[,]>();
            List<Boolean[,]> conteudoTexto4 = new List<bool[,]>();

            this.CalculaLarguraFrase();

            int larguraTotal = this.CalcularLarguraTotal();
            if (larguraTotal < largura)
                larguraTotal = largura;

            //Instancia o painel principal.
            Boolean[,] Painel = new bool[1, 1];
            
            // Verifica o formato do arquivo que será gerado.
            //PrepararTextos();
            if (this.TipoVideo.Equals(Util.Util.TipoVideo.V02))
            {
                
                Painel = new bool[altura, larguraTotal]; 
                // Prepara o TEXTO 1
                if (this.Modelo.Textos[0].FonteWindows == true)
                    conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                else
                    conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                switch (this.Modelo.TipoModelo)
                {
                    case Util.Util.TipoModelo.NúmeroTextoDuplo:
                        #region NúmeroTextoDuplo
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            //====== preparo do numero.

                            //Prepara uma matrix para receber o número de acordo com a largura indicada.
                            Boolean[,] NumeroTemp = new bool[altura, this.Modelo.Textos[0].Largura];

                            //escreve o numero em um painel com a largura que ele precisa.
                            DesenharNoPainel(conteudoTexto1, NumeroTemp, 0, 0);

                            //Prepara o alinhamento para o número.
                            AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);

                            //====== fim numero

                            if (altura > this.Modelo.Textos[2].Altura) 
                            {
                                int alturaLocal = (conteudoTexto2.Count > 0) ? Math.Max(this.Modelo.Textos[2].Altura, conteudoTexto2[0].GetLength(0)) : this.Modelo.Textos[2].Altura;

                                foreach (bool[,] matriz in conteudoTexto2)
                                {
                                    alturaLocal = Math.Max(alturaLocal, matriz.GetLength(0));
                                }
                                
                                //====== texto 2
                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_2_temp = new bool[altura - alturaLocal, larguraTotal - this.Modelo.Textos[0].Largura];

                                DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                //====== fim texto 2

                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[alturaLocal, larguraTotal - this.Modelo.Textos[0].Largura];

                                DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //====== fim texto 2

                                //aplica o numero.
                                DrawMatrix(Painel, NumeroTemp, 0, 0);

                                //aplica texto 1
                                DrawMatrix(Painel, texto_2_temp, alturaLocal, this.Modelo.Textos[0].Largura);

                                //Aplica texto 2
                                DrawMatrix(Painel, texto_3_temp, 0, this.Modelo.Textos[0].Largura);
                            }
                            else 
                            {
                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[this.Modelo.Textos[2].Altura, larguraTotal - this.Modelo.Textos[0].Largura];

                                DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //====== fim texto 2

                                //aplica o numero.
                                DrawMatrix(Painel, NumeroTemp, 0, 0);

                                //Aplica texto 2
                                DrawMatrix(Painel, texto_3_temp, 0, this.Modelo.Textos[0].Largura);
                            }

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[2].CriarListaBitMap(Painel, altura, largura);
                        }
                        break;
                        #endregion NúmeroTextoDuplo
                    case Util.Util.TipoModelo.Texto:
                        #region Texto
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            DesenharNoPainel(conteudoTexto1, Painel, 0, 0);
                            
                            AplicarAlinhamento(this.Modelo.Textos[0], Painel);
                            

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                        }
                        break;
                        #endregion Texto
                    case Util.Util.TipoModelo.TextoDuplo:
                        #region TEXTO_DUPLO
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            if (altura > this.Modelo.Textos[1].Altura)
                            {
                                int alturaLocal = (conteudoTexto2.Count > 0) ? Math.Max(this.Modelo.Textos[1].Altura, conteudoTexto2[0].GetLength(0)) : this.Modelo.Textos[1].Altura;                                

                                foreach (bool[,] matriz in conteudoTexto2)
                                {
                                    alturaLocal = Math.Max(alturaLocal, matriz.GetLength(0));
                                }

                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_1_temp = new bool[altura - alturaLocal, larguraTotal];

                                DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);

                                //============ texto 2
                              
                                Boolean[,] texto_2_temp = new bool[alturaLocal, larguraTotal];

                                DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                //Plota no painel principal.                            
                                DrawMatrix(Painel, texto_1_temp, alturaLocal, 0);
                                DrawMatrix(Painel, texto_2_temp, 0, 0);
                            }
                            else //Se o texto inferior
                            {
                                Boolean[,] texto_2_temp = new bool[this.Modelo.Textos[1].Altura, larguraTotal];

                                DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                DrawMatrix(Painel, texto_2_temp, 0, 0);
                            }

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                                                    
                        }
                        break;
                        #endregion TEXTO_DUPLO
                    case Util.Util.TipoModelo.TextoDuploNúmero:
                        #region TextoDuploNúmero
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            //====== preparo do numero.

                            //Prepara uma matrix para receber o número de acordo com a largura indicada.
                            Boolean[,] NumeroTemp = new bool[altura, this.Modelo.Textos[1].Largura];

                            //escreve o numero em um painel com a largura que ele precisa.
                            DesenharNoPainel(conteudoTexto2, NumeroTemp, 0, 0);

                            //Prepara o alinhamento para o número.
                            AplicarAlinhamento(this.Modelo.Textos[1], NumeroTemp);

                            //====== fim numero

                            if (altura > this.Modelo.Textos[2].Altura)
                            {
                                int alturaLocal = (conteudoTexto3.Count > 0) ? Math.Max(this.Modelo.Textos[2].Altura, conteudoTexto3[0].GetLength(0)) : this.Modelo.Textos[2].Altura;

                                foreach (bool[,] matriz in conteudoTexto3)
                                {
                                    alturaLocal = Math.Max(alturaLocal, matriz.GetLength(0));
                                }

                                //====== texto 2
                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_2_temp = new bool[altura - alturaLocal, larguraTotal - this.Modelo.Textos[1].Largura];

                                DesenharNoPainel(conteudoTexto1, texto_2_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[0], texto_2_temp);

                                //====== fim texto 2

                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[alturaLocal, larguraTotal - this.Modelo.Textos[1].Largura];

                                DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //====== fim texto 3

                                //aplica o numero.
                                DrawMatrix(Painel, NumeroTemp, 0, System.Convert.ToInt16(larguraTotal - this.Modelo.Textos[1].Largura));

                                //aplica texto 2
                                DrawMatrix(Painel, texto_2_temp, alturaLocal, 0);

                                //Aplica texto 3
                                DrawMatrix(Painel, texto_3_temp, 0, 0);
                            }
                            else 
                            {

                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[this.Modelo.Textos[2].Altura, larguraTotal - this.Modelo.Textos[1].Largura];

                                DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);
                                //====== fim texto 3

                                //aplica o numero.
                                DrawMatrix(Painel, NumeroTemp, 0, System.Convert.ToInt16(larguraTotal - this.Modelo.Textos[1].Largura));

                                //Aplica texto 3
                                DrawMatrix(Painel, texto_3_temp, 0, 0);   
                            }

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[2].CriarListaBitMap(Painel, altura, largura);

                        }
                        break;
                        #endregion TextoDuploNúmero
                    case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                        #region TextoDuploTextoDuplo
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[3].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            if (this.Modelo.Textos[3].FonteWindows == true)
                                conteudoTexto4.Add(StrToMatrixTrueType(this.Modelo.Textos[3]));
                            else
                                conteudoTexto4 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[3].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[3].CaminhoFonte));

                            //====== preparo do numero.

                            int alturaLinha2 = Math.Max(this.Modelo.Textos[2].Altura,this.Modelo.Textos[3].Altura);
                            int larguraColuna2 = Math.Max(this.Modelo.Textos[1].Largura,this.Modelo.Textos[3].Largura);

                            if (altura > alturaLinha2) {

                                int alturaLocal = (conteudoTexto2.Count > 0) ? Math.Max(alturaLinha2, conteudoTexto2[0].GetLength(0)) : alturaLinha2;

                                foreach (bool[,] matriz in conteudoTexto2)
                                {
                                    alturaLocal = Math.Max(alturaLocal, matriz.GetLength(0));
                                }
                                //Prepara uma matrix para receber o número de acordo com a largura indicada.
                                Boolean[,] Texto_1_temp = new bool[altura - alturaLocal, larguraTotal - larguraColuna2];

                                //escreve o numero em um painel com a largura que ele precisa.
                                DesenharNoPainel(conteudoTexto1, Texto_1_temp, 0, 0);

                                //Prepara o alinhamento para o número.
                                AplicarAlinhamento(this.Modelo.Textos[0], Texto_1_temp);

                                //====== fim numero

                                //====== texto 2
                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_2_temp = new bool[altura - alturaLocal, larguraColuna2];

                                DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                //====== fim texto 2

                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[alturaLocal, larguraTotal - larguraColuna2];

                                DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //====== fim texto 3

                                //====== texto 4
                                Boolean[,] texto_4_temp = new bool[alturaLocal, larguraColuna2];

                                DesenharNoPainel(conteudoTexto4, texto_4_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[3], texto_4_temp);
                                //====== fim texto 4

                                //aplica texto 0;
                                DrawMatrix(Painel, Texto_1_temp, alturaLocal, 0);

                                //aplica texto 1
                                DrawMatrix(Painel, texto_2_temp, alturaLocal, larguraTotal - larguraColuna2);

                                //Aplica texto 2
                                DrawMatrix(Painel, texto_3_temp, 0, 0);

                                //aplica texto 3
                                DrawMatrix(Painel, texto_4_temp, 0, larguraTotal - larguraColuna2);
                            }
                            else 
                            {
                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[alturaLinha2, larguraTotal - larguraColuna2];

                                DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //====== fim texto 3

                                //====== texto 4
                                Boolean[,] texto_4_temp = new bool[alturaLinha2, larguraColuna2];

                                DesenharNoPainel(conteudoTexto4, texto_4_temp, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[3], texto_4_temp);
                                //====== fim texto 4


                                //Aplica texto 2
                                DrawMatrix(Painel, texto_3_temp, 0, 0);

                                //aplica texto 3
                                DrawMatrix(Painel, texto_4_temp, 0, larguraTotal - larguraColuna2); 
                            }

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[2].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[3].CriarListaBitMap(Painel, altura, largura);

                        }
                        break;
                        #endregion TextoDuploTextoDuplo
                    case Util.Util.TipoModelo.TextoNúmero:
                        #region Texto + Número
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));


                            Boolean[,] texto = new bool[altura, larguraTotal - Modelo.Textos[1].Largura];

                            DesenharNoPainel(conteudoTexto1, texto, 0, 0);

                            AplicarAlinhamento(this.Modelo.Textos[0], texto);

                            //Prepara uma matrix para receber o número de acordo com a largura indicada.
                            Boolean[,] numero = new bool[altura, this.Modelo.Textos[1].Largura];

                            //escreve o numero em um painel com a largura que ele precisa.
                            DesenharNoPainel(conteudoTexto2, numero, 0, 0);

                            //Prepara o alinhamento para o número.
                            AplicarAlinhamento(this.Modelo.Textos[1], numero);

                            DrawMatrix(Painel, texto, 0, 0);
                            DrawMatrix(Painel, numero, 0, larguraTotal - Modelo.Textos[1].Largura);

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                        }
                        break;
                        #endregion Número + Texto
                    case Util.Util.TipoModelo.NúmeroTexto:
                        #region Número + Texto
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            Boolean[,] NumeroTemp = new bool[altura, this.Modelo.Textos[0].Largura];
                            Boolean[,] texto_1_temp = new bool[altura, larguraTotal - this.Modelo.Textos[0].Largura];

                            DesenharNoPainel(conteudoTexto1, NumeroTemp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, texto_1_temp, 0, 0);

                            //Prepara o alinhamento para o número.
                            AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);
                            AplicarAlinhamento(this.Modelo.Textos[1], texto_1_temp);

                            //Plotar Matrix Numero na Matrix Principal
                            DrawMatrix(Painel, NumeroTemp, 0, 0);

                            //primeiro desenha, depois alinha centralizando horizontal e vertical.
                            DrawMatrix(Painel, texto_1_temp, 0, this.Modelo.Textos[0].Largura);

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                        }
                        break;
                        #endregion Número + Texto
                    case Util.Util.TipoModelo.TextoTriplo:
                        #region TextoTriplo
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            //gerando a imagem do texto 2
                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            //gerando a imagem do texto 3
                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            //pegando a altura dos 3 textos
                            int altura1 = (conteudoTexto1.Count > 0) ? Math.Max(this.Modelo.Textos[0].Altura, conteudoTexto1[0].GetLength(0)) : this.Modelo.Textos[0].Altura;
                            int altura2 = (conteudoTexto2.Count > 0) ? Math.Max(this.Modelo.Textos[1].Altura, conteudoTexto2[0].GetLength(0)) : this.Modelo.Textos[1].Altura;
                            int altura3 = (conteudoTexto3.Count > 0) ? Math.Max(this.Modelo.Textos[2].Altura, conteudoTexto3[0].GetLength(0)) : this.Modelo.Textos[2].Altura;

                            foreach (bool[,] matriz in conteudoTexto1)
                            {
                                altura1 = Math.Max(altura1, matriz.GetLength(0));
                            }

                            foreach (bool[,] matriz in conteudoTexto2)
                            {
                                altura2 = Math.Max(altura2, matriz.GetLength(0));
                            }

                            foreach (bool[,] matriz in conteudoTexto3)
                            {
                                altura3 = Math.Max(altura3, matriz.GetLength(0));
                            }


                            //Se a soma dos 2 campos inferiores forem menor que a altura, exibir os 3 campos
                            if (altura > (altura2 + altura3))
                            {
                                //adicionando uma linha em branco entre os textos
                                if (altura1 + altura2 + altura3 <= (altura - 2))
                                {
                                    altura2 = altura2 + 1;
                                    altura3 = altura3 + 1;
                                }
                                else
                                {
                                    if (altura1 + altura2 + altura3 <= (altura - 1))
                                        altura3 = altura3 + 1;
                                }

                                //painel do primeiro texto
                                Boolean[,] texto_1_temp = new bool[altura - altura2 - altura3, larguraTotal];
                                DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);


                                //painel do segundo texto
                                Boolean[,] texto_2_temp = new bool[altura2, larguraTotal];
                                DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                //painel do terceiro texto
                                Boolean[,] texto_3_temp = new bool[altura3, larguraTotal];
                                DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //Plota no painel principal.                            
                                DrawMatrix(Painel, texto_1_temp, altura2 + altura3, 0);
                                DrawMatrix(Painel, texto_2_temp, altura3, 0);
                                DrawMatrix(Painel, texto_3_temp, 0, 0);
                            }
                            else //Se não cabem os 3 textos
                            {
                                if (altura > altura3) //Se só cabem os 2 ultimos textos
                                {
                                    //painel do segundo texto
                                    Boolean[,] texto_2_temp = new bool[altura2, larguraTotal];
                                    DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                    //painel do terceiro texto
                                    Boolean[,] texto_3_temp = new bool[altura3, larguraTotal];
                                    DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                    DrawMatrix(Painel, texto_2_temp, altura3, 0);
                                    DrawMatrix(Painel, texto_3_temp, 0, 0);
                                }
                                else //Se só cabe o terceiro texto
                                {
                                    //painel do terceiro texto
                                    Boolean[,] texto_3_temp = new bool[altura3, larguraTotal];
                                    DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);
                                    DrawMatrix(Painel, texto_3_temp, 0, 0);
                                }
                            }

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[2].CriarListaBitMap(Painel, altura, largura);

                        }
                        break;
                    #endregion
                    case Util.Util.TipoModelo.TextoTriploNumero:
                        #region TextoTriploNumero
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[3].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            //gerando a imagem do numero
                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            //gerando a imagem do texto 2
                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            //gerando a imagem do texto 3
                            if (this.Modelo.Textos[3].FonteWindows == true)
                                conteudoTexto4.Add(StrToMatrixTrueType(this.Modelo.Textos[3]));
                            else
                                conteudoTexto4 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[3].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[3].CaminhoFonte));


                            //pegando a altura dos 3 textos
                            int altura1 = (conteudoTexto1.Count > 0) ? Math.Max(this.Modelo.Textos[0].Altura, conteudoTexto1[0].GetLength(0)) : this.Modelo.Textos[0].Altura;
                            int altura2 = (conteudoTexto3.Count > 0) ? Math.Max(this.Modelo.Textos[2].Altura, conteudoTexto3[0].GetLength(0)) : this.Modelo.Textos[2].Altura;
                            int altura3 = (conteudoTexto4.Count > 0) ? Math.Max(this.Modelo.Textos[3].Altura, conteudoTexto4[0].GetLength(0)) : this.Modelo.Textos[3].Altura;

                            foreach (bool[,] matriz in conteudoTexto1)
                            {
                                altura1 = Math.Max(altura1, matriz.GetLength(0));
                            }

                            foreach (bool[,] matriz in conteudoTexto3)
                            {
                                altura2 = Math.Max(altura2, matriz.GetLength(0));
                            }

                            foreach (bool[,] matriz in conteudoTexto4)
                            {
                                altura3 = Math.Max(altura3, matriz.GetLength(0));
                            }

                            //Painel do Número
                            Boolean[,] numero = new bool[altura, this.Modelo.Textos[1].Largura];
                            DesenharNoPainel(conteudoTexto2, numero, 0, 0);
                            AplicarAlinhamento(this.Modelo.Textos[1], numero);

                            //Se a soma dos 2 campos inferiores forem menor que a altura, exibir os 3 campos
                            if (altura > (altura2 + altura3))
                            {
                                //adicionando uma linha em branco entre os textos
                                if (altura1 + altura2 + altura3 <= (altura - 2))
                                {
                                    altura2 = altura2 + 1;
                                    altura3 = altura3 + 1;
                                }
                                else
                                {
                                    if (altura1 + altura2 + altura3 <= (altura - 1))
                                        altura3 = altura3 + 1;
                                }

                                //painel do primeiro texto
                                Boolean[,] texto_1_temp = new bool[altura - altura2 - altura3, larguraTotal - this.Modelo.Textos[1].Largura];
                                DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);


                                //painel do segundo texto
                                Boolean[,] texto_2_temp = new bool[altura2, larguraTotal - this.Modelo.Textos[1].Largura];
                                DesenharNoPainel(conteudoTexto3, texto_2_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[2], texto_2_temp);

                                //painel do terceiro texto
                                Boolean[,] texto_3_temp = new bool[altura3, larguraTotal - this.Modelo.Textos[1].Largura];
                                DesenharNoPainel(conteudoTexto4, texto_3_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[3], texto_3_temp);

                                //Desenha o painel completo.
                                DrawMatrix(Painel, numero, 0, larguraTotal - this.Modelo.Textos[1].Largura);
                                DrawMatrix(Painel, texto_1_temp, altura2 + altura3, 0);
                                DrawMatrix(Painel, texto_2_temp, altura3, 0);
                                DrawMatrix(Painel, texto_3_temp, 0, 0);
                            }
                            else //Se não cabem os 3 textos
                            {
                                if (altura > altura3) //Se só cabem os 2 ultimos textos
                                {
                                    //painel do segundo texto
                                    Boolean[,] texto_2_temp = new bool[altura2, larguraTotal - this.Modelo.Textos[1].Largura];
                                    DesenharNoPainel(conteudoTexto3, texto_2_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[2], texto_2_temp);

                                    //painel do terceiro texto
                                    Boolean[,] texto_3_temp = new bool[altura3, larguraTotal - this.Modelo.Textos[1].Largura];
                                    DesenharNoPainel(conteudoTexto4, texto_3_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[3], texto_3_temp);

                                    //Desenha o painel completo.
                                    DrawMatrix(Painel, numero, 0, larguraTotal - this.Modelo.Textos[1].Largura);
                                    DrawMatrix(Painel, texto_2_temp, altura3, 0);
                                    DrawMatrix(Painel, texto_3_temp, 0, 0);
                                }
                                else //Se só cabe o terceiro texto
                                {
                                    //painel do terceiro texto
                                    Boolean[,] texto_3_temp = new bool[altura3, larguraTotal - this.Modelo.Textos[1].Largura];
                                    DesenharNoPainel(conteudoTexto4, texto_3_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[3], texto_3_temp);

                                    //Desenha o painel completo.
                                    DrawMatrix(Painel, numero, 0, larguraTotal - this.Modelo.Textos[1].Largura);
                                    DrawMatrix(Painel, texto_3_temp, 0, 0);
                                }
                            }

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[2].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[3].CriarListaBitMap(Painel, altura, largura);

                        }
                        break;
                    #endregion
                    case Util.Util.TipoModelo.NumeroTextoTriplo:
                        #region NumeroTextoTriplo
                        {
                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                            {
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                this.Modelo.Textos[3].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            //gerando a imagem do texto 1
                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            //gerando a imagem do texto 2
                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            //gerando a imagem do texto 3
                            if (this.Modelo.Textos[3].FonteWindows == true)
                                conteudoTexto4.Add(StrToMatrixTrueType(this.Modelo.Textos[3]));
                            else
                                conteudoTexto4 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[3].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[3].CaminhoFonte));


                            //pegando a altura dos 3 textos
                            int altura1 = (conteudoTexto2.Count > 0) ? Math.Max(this.Modelo.Textos[1].Altura, conteudoTexto2[0].GetLength(0)) : this.Modelo.Textos[1].Altura;
                            int altura2 = (conteudoTexto3.Count > 0) ? Math.Max(this.Modelo.Textos[2].Altura, conteudoTexto3[0].GetLength(0)) : this.Modelo.Textos[2].Altura;
                            int altura3 = (conteudoTexto4.Count > 0) ? Math.Max(this.Modelo.Textos[3].Altura, conteudoTexto4[0].GetLength(0)) : this.Modelo.Textos[3].Altura;

                            foreach (bool[,] matriz in conteudoTexto2)
                            {
                                altura1 = Math.Max(altura1, matriz.GetLength(0));
                            }

                            foreach (bool[,] matriz in conteudoTexto3)
                            {
                                altura2 = Math.Max(altura2, matriz.GetLength(0));
                            }

                            foreach (bool[,] matriz in conteudoTexto4)
                            {
                                altura3 = Math.Max(altura3, matriz.GetLength(0));
                            }

                            //Painel do Número
                            Boolean[,] numero = new bool[altura, this.Modelo.Textos[0].Largura];
                            DesenharNoPainel(conteudoTexto1, numero, 0, 0);
                            AplicarAlinhamento(this.Modelo.Textos[0], numero);

                            //Se a soma dos 2 campos inferiores forem menor que a altura, exibir os 3 campos
                            if (altura > (altura2 + altura3))
                            {
                                //adicionando uma linha em branco entre os textos
                                if (altura1 + altura2 + altura3 <= (altura - 2))
                                {
                                    altura2 = altura2 + 1;
                                    altura3 = altura3 + 1;
                                }
                                else
                                {
                                    if (altura1 + altura2 + altura3 <= (altura - 1))
                                        altura3 = altura3 + 1;
                                }

                                //painel do primeiro texto
                                Boolean[,] texto_1_temp = new bool[altura - altura2 - altura3, larguraTotal - this.Modelo.Textos[0].Largura];
                                DesenharNoPainel(conteudoTexto2, texto_1_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[1], texto_1_temp);


                                //painel do segundo texto
                                Boolean[,] texto_2_temp = new bool[altura2, larguraTotal - this.Modelo.Textos[0].Largura];
                                DesenharNoPainel(conteudoTexto3, texto_2_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[2], texto_2_temp);

                                //painel do terceiro texto
                                Boolean[,] texto_3_temp = new bool[altura3, larguraTotal - this.Modelo.Textos[0].Largura];
                                DesenharNoPainel(conteudoTexto4, texto_3_temp, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[3], texto_3_temp);

                                //Desenha o painel completo.
                                DrawMatrix(Painel, numero, 0, 0);
                                DrawMatrix(Painel, texto_1_temp, altura2 + altura3, Modelo.Textos[0].Largura);
                                DrawMatrix(Painel, texto_2_temp, altura3, Modelo.Textos[0].Largura);
                                DrawMatrix(Painel, texto_3_temp, 0, Modelo.Textos[0].Largura);
                            }
                            else //Se não cabem os 3 textos
                            {
                                if (altura > altura3) //Se só cabem os 2 ultimos textos
                                {
                                    //painel do segundo texto
                                    Boolean[,] texto_2_temp = new bool[altura2, larguraTotal - this.Modelo.Textos[0].Largura];
                                    DesenharNoPainel(conteudoTexto3, texto_2_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[2], texto_2_temp);

                                    //painel do terceiro texto
                                    Boolean[,] texto_3_temp = new bool[altura3, larguraTotal - this.Modelo.Textos[0].Largura];
                                    DesenharNoPainel(conteudoTexto4, texto_3_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[3], texto_3_temp);

                                    //Desenha o painel completo.
                                    DrawMatrix(Painel, numero, 0, 0);
                                    DrawMatrix(Painel, texto_2_temp, altura3, this.Modelo.Textos[0].Largura);
                                    DrawMatrix(Painel, texto_3_temp, 0, this.Modelo.Textos[0].Largura);
                                }
                                else //Se só cabe o terceiro texto
                                {
                                    //painel do terceiro texto
                                    Boolean[,] texto_3_temp = new bool[altura3, larguraTotal - this.Modelo.Textos[0].Largura];
                                    DesenharNoPainel(conteudoTexto4, texto_3_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[3], texto_3_temp);

                                    //Desenha o painel completo.
                                    DrawMatrix(Painel, numero, 0, 0);
                                    DrawMatrix(Painel, texto_3_temp, 0, this.Modelo.Textos[0].Largura);
                                }
                            }

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[1].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[2].CriarListaBitMap(Painel, altura, largura);
                            this.Modelo.Textos[3].CriarListaBitMap(Painel, altura, largura);

                        }
                        break;
                    #endregion
                    default: // Default: Texto
                        #region Texto
                        {
                            //painel temporário para o primeiro texto.
                            Boolean[,] texto_1_temp = new bool[altura, largura];

                            //Forçando o alinhamento a esquerda se o texto for maior que o painel para o segundo painel ficar alinhado a esquerda
                            if (larguraTotal > largura)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);
                            AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);

                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);
                        }
                        break;
                        #endregion Texto

                }
            }

            if (this.TipoVideo.Equals(Util.Util.TipoVideo.V04))
            {

                switch (this.Modelo.TipoModelo)
                {
                    case Util.Util.TipoModelo.NúmeroTextoDuplo:
                        #region NúmeroTextoDuplo
                        {
                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            //Setando a largura e a altura dos paineis no objeto Texto
                            this.Modelo.Textos[0].LarguraPainel = (this.Modelo.Textos[0].LarguraPainel == 0 ? this.Modelo.Textos[0].Largura : this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].LarguraPainel = (this.Modelo.Textos[1].LarguraPainel == 0 ? largura - this.Modelo.Textos[0].Largura : this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].LarguraPainel = (this.Modelo.Textos[2].LarguraPainel == 0 ? largura - this.Modelo.Textos[0].Largura : this.Modelo.Textos[2].LarguraPainel);

                            this.Modelo.Textos[0].AlturaPainel = altura;
                            if (altura >= (this.Modelo.Textos[2].Altura + Util.Util.ALTURA_MINIMA_TEXTOS_V04))
                            {
                                this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? altura - this.Modelo.Textos[2].Altura : this.Modelo.Textos[1].AlturaPainel);
                                this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? this.Modelo.Textos[2].Altura : this.Modelo.Textos[2].AlturaPainel);
                            }
                            else
                            {
                                this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[1].AlturaPainel);
                                this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? altura - Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[2].AlturaPainel);
                            }

                            Boolean[,] NumeroTemp = new bool[altura, Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] texto_2_temp = new bool[this.Modelo.Textos[1].AlturaPainel, Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[1].LarguraPainel)];
                            Boolean[,] texto_3_temp = new bool[this.Modelo.Textos[2].AlturaPainel, Math.Max(this.Modelo.Textos[2].Largura, this.Modelo.Textos[2].LarguraPainel)];

                            DesenharNoPainel(conteudoTexto1, NumeroTemp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                            AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);
                            AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);
                            AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                            this.Modelo.Textos[0].CriarListaBitMap(NumeroTemp, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(texto_2_temp, this.Modelo.Textos[1].AlturaPainel, this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].CriarListaBitMap(texto_3_temp, this.Modelo.Textos[2].AlturaPainel, this.Modelo.Textos[2].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[2].listaBitMap.Count > 1)
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                        }
                        break;
                        #endregion NúmeroTextoDuplo
                    case Util.Util.TipoModelo.Texto:
                        #region Texto
                        {
                            //painel simples
                            if (this.Modelo.Textos.Count == 1)
                            {
                                Painel = new bool[altura, larguraTotal];
                                
                                if (this.Modelo.Textos[0].FonteWindows == true)
                                    conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                                else
                                    conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                                this.Modelo.Textos[0].LarguraPainel = largura;
                                this.Modelo.Textos[0].AlturaPainel = altura;

                                DesenharNoPainel(conteudoTexto1, Painel, 0, 0);
                                AplicarAlinhamento(this.Modelo.Textos[0], Painel);
                                this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);

                                //Forçando alinhamento a esquerda se o texto for maior que o painel
                                if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                    this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                            }

                            //painel multilinhas
                            if (this.Modelo.Textos.Count > 1)
                            {
                                for(int i=0; i<this.Modelo.Textos.Count;i++)
                                {
                                    Boolean[,] texto_1_temp = new bool[this.Modelo.Textos[i].AlturaPainel, Math.Max(this.Modelo.Textos[i].Largura, this.Modelo.Textos[i].LarguraPainel)];
                                    conteudoTexto1.Clear();

                                    if (this.Modelo.Textos[i].FonteWindows == true)
                                        conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[i]));
                                    else
                                        conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[i].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[i].CaminhoFonte));

                                    DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);
                                    AplicarAlinhamento(this.Modelo.Textos[i], texto_1_temp);
                                    this.Modelo.Textos[i].CriarListaBitMap(texto_1_temp, this.Modelo.Textos[i].AlturaPainel, this.Modelo.Textos[i].LarguraPainel);

                                    //Forçando alinhamento a esquerda se o texto for maior que o painel
                                    if (this.Modelo.Textos[i].listaBitMap.Count > 1)
                                        this.Modelo.Textos[i].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                                }
                            }
                        }
                        break;
                        #endregion Texto
                    case Util.Util.TipoModelo.TextoDuplo:
                        #region TEXTO_DUPLO
                        {

                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            this.Modelo.Textos[0].LarguraPainel = largura;
                            this.Modelo.Textos[1].LarguraPainel = largura;
                            if (altura >= (this.Modelo.Textos[1].Altura + Util.Util.ALTURA_MINIMA_TEXTOS_V04)) 
                            { 
                                this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? altura - this.Modelo.Textos[1].Altura : this.Modelo.Textos[0].AlturaPainel);
                                this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? this.Modelo.Textos[1].Altura : this.Modelo.Textos[1].AlturaPainel);
                            }
                            else
                            {
                                this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[0].AlturaPainel);
                                this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? altura - Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[1].AlturaPainel);
                            }

                            Boolean[,] texto_1_temp = new bool[this.Modelo.Textos[0].AlturaPainel, Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] texto_2_temp = new bool[this.Modelo.Textos[1].AlturaPainel, Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[1].LarguraPainel)];

                            DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);

                            AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);
                            AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                            this.Modelo.Textos[0].CriarListaBitMap(texto_1_temp, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(texto_2_temp, this.Modelo.Textos[1].AlturaPainel, this.Modelo.Textos[1].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                        }
                        break;
                        #endregion TEXTO_DUPLO
                    case Util.Util.TipoModelo.TextoDuploNúmero:
                        #region TextoDuploNúmero
                        {

                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));


                            //Setando a largura e a altura dos paineis no objeto Texto
                            this.Modelo.Textos[0].LarguraPainel = (this.Modelo.Textos[0].LarguraPainel == 0 ? largura - this.Modelo.Textos[1].Largura : this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].LarguraPainel = (this.Modelo.Textos[1].LarguraPainel == 0 ? this.Modelo.Textos[1].Largura : this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].LarguraPainel = (this.Modelo.Textos[2].LarguraPainel == 0 ? largura - this.Modelo.Textos[1].Largura : this.Modelo.Textos[2].LarguraPainel);

                            this.Modelo.Textos[1].AlturaPainel = altura;
                            if (altura >= (this.Modelo.Textos[2].Altura + Util.Util.ALTURA_MINIMA_TEXTOS_V04))
                            {
                                this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? altura - this.Modelo.Textos[2].Altura : this.Modelo.Textos[0].AlturaPainel);
                                this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? this.Modelo.Textos[2].Altura : this.Modelo.Textos[2].AlturaPainel);
                            }
                            else
                            {
                                this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[0].AlturaPainel);
                                this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? altura - Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[2].AlturaPainel);
                            }

                            
                            Boolean[,] texto_2_temp = new bool[this.Modelo.Textos[0].AlturaPainel, Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] NumeroTemp = new bool[altura, Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[1].LarguraPainel)];
                            Boolean[,] texto_3_temp = new bool[this.Modelo.Textos[2].AlturaPainel, Math.Max(this.Modelo.Textos[2].Largura, this.Modelo.Textos[2].LarguraPainel)];

                            
                            DesenharNoPainel(conteudoTexto1, texto_2_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, NumeroTemp, 0, 0);
                            DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                            AplicarAlinhamento(this.Modelo.Textos[0], texto_2_temp);
                            AplicarAlinhamento(this.Modelo.Textos[1], NumeroTemp);                            
                            AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                            
                            this.Modelo.Textos[0].CriarListaBitMap(texto_2_temp, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(NumeroTemp, this.Modelo.Textos[1].AlturaPainel, this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].CriarListaBitMap(texto_3_temp, this.Modelo.Textos[2].AlturaPainel, this.Modelo.Textos[2].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[2].listaBitMap.Count > 1)
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                        }
                        break;
                        #endregion TextoDuploNúmero
                    case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                        #region TextoDuploTextoDuplo
                        {
                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            if (this.Modelo.Textos[3].FonteWindows == true)
                                conteudoTexto4.Add(StrToMatrixTrueType(this.Modelo.Textos[3]));
                            else
                                conteudoTexto4 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[3].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[3].CaminhoFonte));


                            this.Modelo.Textos[0].LarguraPainel = (this.Modelo.Textos[0].LarguraPainel == 0 ? largura/2 : this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].LarguraPainel = (this.Modelo.Textos[1].LarguraPainel == 0 ? largura - this.Modelo.Textos[0].LarguraPainel : this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].LarguraPainel = (this.Modelo.Textos[2].LarguraPainel == 0 ? largura/2 : this.Modelo.Textos[2].LarguraPainel);
                            this.Modelo.Textos[3].LarguraPainel = (this.Modelo.Textos[3].LarguraPainel == 0 ? largura - this.Modelo.Textos[2].LarguraPainel : this.Modelo.Textos[3].LarguraPainel);


                            if (altura >= (Math.Max(this.Modelo.Textos[2].Altura,this.Modelo.Textos[3].Altura) + Util.Util.ALTURA_MINIMA_TEXTOS_V04))
                            {

                                this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? altura - Math.Max(this.Modelo.Textos[2].Altura, this.Modelo.Textos[3].Altura) : this.Modelo.Textos[0].AlturaPainel);
                                this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? altura - Math.Max(this.Modelo.Textos[2].Altura, this.Modelo.Textos[3].Altura) : this.Modelo.Textos[1].AlturaPainel); ;
                                this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? Math.Max(this.Modelo.Textos[2].Altura, this.Modelo.Textos[3].Altura) : this.Modelo.Textos[2].AlturaPainel);
                                this.Modelo.Textos[3].AlturaPainel = (this.Modelo.Textos[3].AlturaPainel == 0 ? Math.Max(this.Modelo.Textos[2].Altura, this.Modelo.Textos[3].Altura) : this.Modelo.Textos[3].AlturaPainel);
                            }
                            else
                            {
                                this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[0].AlturaPainel);
                                this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[1].AlturaPainel);
                                this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? altura - Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[2].AlturaPainel);
                                this.Modelo.Textos[3].AlturaPainel = (this.Modelo.Textos[3].AlturaPainel == 0 ? altura - Util.Util.ALTURA_MINIMA_TEXTOS_V04 : this.Modelo.Textos[3].AlturaPainel);
                            }


                            Boolean[,] texto_1_temp = new bool[this.Modelo.Textos[0].AlturaPainel, Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] texto_2_temp = new bool[this.Modelo.Textos[1].AlturaPainel, Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[1].LarguraPainel)];
                            Boolean[,] texto_3_temp = new bool[this.Modelo.Textos[2].AlturaPainel, Math.Max(this.Modelo.Textos[2].Largura, this.Modelo.Textos[2].LarguraPainel)];
                            Boolean[,] texto_4_temp = new bool[this.Modelo.Textos[3].AlturaPainel, Math.Max(this.Modelo.Textos[3].Largura, this.Modelo.Textos[3].LarguraPainel)];


                            DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto4, texto_4_temp, 0, 0);

                            AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);
                            AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);
                            AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);
                            AplicarAlinhamento(this.Modelo.Textos[3], texto_4_temp);


                            this.Modelo.Textos[0].CriarListaBitMap(texto_1_temp, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(texto_2_temp, this.Modelo.Textos[1].AlturaPainel, this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].CriarListaBitMap(texto_3_temp, this.Modelo.Textos[2].AlturaPainel, this.Modelo.Textos[2].LarguraPainel);
                            this.Modelo.Textos[3].CriarListaBitMap(texto_4_temp, this.Modelo.Textos[3].AlturaPainel, this.Modelo.Textos[3].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[2].listaBitMap.Count > 1)
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[3].listaBitMap.Count > 1)
                                this.Modelo.Textos[3].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                           
                        }
                        break;
                        #endregion TextoDuploTextoDuplo
                    case Util.Util.TipoModelo.TextoNúmero:
                        #region Texto + Número
                        {
                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            //Setando a largura e a altura dos paineis no objeto Texto
                            this.Modelo.Textos[0].LarguraPainel = (this.Modelo.Textos[0].LarguraPainel == 0 ? largura - this.Modelo.Textos[1].Largura : this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].LarguraPainel = (this.Modelo.Textos[1].LarguraPainel == 0 ? this.Modelo.Textos[1].Largura : this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[0].AlturaPainel = altura;
                            this.Modelo.Textos[1].AlturaPainel = altura;

                            Boolean[,] texto = new bool[altura, Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] numero = new bool[altura, Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[1].LarguraPainel)];

                            DesenharNoPainel(conteudoTexto1, texto, 0, 0);
                            DesenharNoPainel(conteudoTexto2, numero, 0, 0);

                            AplicarAlinhamento(this.Modelo.Textos[0], texto);                            
                            AplicarAlinhamento(this.Modelo.Textos[1], numero);

                            this.Modelo.Textos[0].CriarListaBitMap(texto, altura, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(numero, altura, this.Modelo.Textos[1].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;
                        }
                        break;
                        #endregion Número + Texto
                    case Util.Util.TipoModelo.NúmeroTexto:
                        #region Número + Texto
                        {
                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            //Setando a largura e a altura dos paineis no objeto Texto
                            this.Modelo.Textos[0].LarguraPainel = (this.Modelo.Textos[0].LarguraPainel == 0?this.Modelo.Textos[0].Largura:this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].LarguraPainel = (this.Modelo.Textos[1].LarguraPainel == 0 ? largura - this.Modelo.Textos[0].LarguraPainel : this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[0].AlturaPainel = altura;
                            this.Modelo.Textos[1].AlturaPainel = altura;

                            Boolean[,] NumeroTemp = new bool[altura, Math.Max(this.Modelo.Textos[0].Largura,this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] texto_1_temp = new bool[altura, Math.Max(this.Modelo.Textos[1].Largura,this.Modelo.Textos[1].LarguraPainel)];

                            DesenharNoPainel(conteudoTexto1, NumeroTemp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, texto_1_temp, 0, 0);

                            //Prepara o alinhamento para o número.
                            AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);
                            AplicarAlinhamento(this.Modelo.Textos[1], texto_1_temp);

                            this.Modelo.Textos[0].CriarListaBitMap(NumeroTemp, altura, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(texto_1_temp, altura, this.Modelo.Textos[1].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                        }
                        break;
                        #endregion Número + Texto
                    case Util.Util.TipoModelo.TextoTriplo:
                        #region TEXTO_TRIPLO
                        {
                            //texto 1
                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            //texto 2
                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            //texto 3
                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            this.Modelo.Textos[0].LarguraPainel = largura;
                            this.Modelo.Textos[1].LarguraPainel = largura;
                            this.Modelo.Textos[2].LarguraPainel = largura;

                            //TODO: FAZER QUANDO TIVER O TRACKBAR
                            this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1 : this.Modelo.Textos[0].AlturaPainel);
                            this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2 : this.Modelo.Textos[1].AlturaPainel);
                            this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3 : this.Modelo.Textos[2].AlturaPainel);
                            //if (altura >= (this.Modelo.Textos[1].Altura + 2))
                            //{
                            //    this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? altura - this.Modelo.Textos[1].Altura : this.Modelo.Textos[0].AlturaPainel);
                            //    this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? this.Modelo.Textos[1].Altura : this.Modelo.Textos[1].AlturaPainel);
                            //}
                            //else
                            //{
                            //    this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? 2 : this.Modelo.Textos[0].AlturaPainel);
                            //    this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? altura - 2 : this.Modelo.Textos[1].AlturaPainel);
                            //}

                            Boolean[,] texto_1_temp = new bool[this.Modelo.Textos[0].AlturaPainel, Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] texto_2_temp = new bool[this.Modelo.Textos[1].AlturaPainel, Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[1].LarguraPainel)];
                            Boolean[,] texto_3_temp = new bool[this.Modelo.Textos[2].AlturaPainel, Math.Max(this.Modelo.Textos[2].Largura, this.Modelo.Textos[2].LarguraPainel)];

                            DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);

                            AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);
                            AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);
                            AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                            this.Modelo.Textos[0].CriarListaBitMap(texto_1_temp, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(texto_2_temp, this.Modelo.Textos[1].AlturaPainel, this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].CriarListaBitMap(texto_3_temp, this.Modelo.Textos[2].AlturaPainel, this.Modelo.Textos[2].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[2].listaBitMap.Count > 1)
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                        }
                        break;
                    #endregion TEXTO_TRIPLO
                    case Util.Util.TipoModelo.NumeroTextoTriplo:
                        #region NúmeroTextoTriplo
                        {
                            //numero
                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            //texto 1
                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            //texto 2
                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            //texto 3
                            if (this.Modelo.Textos[3].FonteWindows == true)
                                conteudoTexto4.Add(StrToMatrixTrueType(this.Modelo.Textos[3]));
                            else
                                conteudoTexto4 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[3].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[3].CaminhoFonte));


                            //Setando a largura e a altura dos paineis no objeto Texto
                            this.Modelo.Textos[0].LarguraPainel = (this.Modelo.Textos[0].LarguraPainel == 0 ? this.Modelo.Textos[0].Largura : this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].LarguraPainel = (this.Modelo.Textos[1].LarguraPainel == 0 ? largura - this.Modelo.Textos[0].Largura : this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].LarguraPainel = (this.Modelo.Textos[2].LarguraPainel == 0 ? largura - this.Modelo.Textos[0].Largura : this.Modelo.Textos[2].LarguraPainel);
                            this.Modelo.Textos[3].LarguraPainel = (this.Modelo.Textos[3].LarguraPainel == 0 ? largura - this.Modelo.Textos[0].Largura : this.Modelo.Textos[3].LarguraPainel);

                            //TODO: fazer o calculo quando tiver com o trackbar
                            this.Modelo.Textos[0].AlturaPainel = altura;
                            this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1 : this.Modelo.Textos[1].AlturaPainel);
                            this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2 : this.Modelo.Textos[2].AlturaPainel);
                            this.Modelo.Textos[3].AlturaPainel = (this.Modelo.Textos[3].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3 : this.Modelo.Textos[3].AlturaPainel);
                            //if (altura >= (this.Modelo.Textos[2].Altura + 2))
                            //{
                            //    this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? altura - this.Modelo.Textos[2].Altura : this.Modelo.Textos[1].AlturaPainel);
                            //    this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? this.Modelo.Textos[2].Altura : this.Modelo.Textos[2].AlturaPainel);
                            //}
                            //else
                            //{
                            //    this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? 2 : this.Modelo.Textos[1].AlturaPainel);
                            //    this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? altura - 2 : this.Modelo.Textos[2].AlturaPainel);
                            //}

                            Boolean[,] NumeroTemp = new bool[altura, Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] texto_2_temp = new bool[this.Modelo.Textos[1].AlturaPainel, Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[1].LarguraPainel)];
                            Boolean[,] texto_3_temp = new bool[this.Modelo.Textos[2].AlturaPainel, Math.Max(this.Modelo.Textos[2].Largura, this.Modelo.Textos[2].LarguraPainel)];
                            Boolean[,] texto_4_temp = new bool[this.Modelo.Textos[3].AlturaPainel, Math.Max(this.Modelo.Textos[3].Largura, this.Modelo.Textos[3].LarguraPainel)];

                            DesenharNoPainel(conteudoTexto1, NumeroTemp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, texto_2_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto4, texto_4_temp, 0, 0);

                            AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);
                            AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);
                            AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);
                            AplicarAlinhamento(this.Modelo.Textos[3], texto_4_temp);

                            this.Modelo.Textos[0].CriarListaBitMap(NumeroTemp, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(texto_2_temp, this.Modelo.Textos[1].AlturaPainel, this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].CriarListaBitMap(texto_3_temp, this.Modelo.Textos[2].AlturaPainel, this.Modelo.Textos[2].LarguraPainel);
                            this.Modelo.Textos[3].CriarListaBitMap(texto_4_temp, this.Modelo.Textos[3].AlturaPainel, this.Modelo.Textos[3].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[2].listaBitMap.Count > 1)
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[3].listaBitMap.Count > 1)
                                this.Modelo.Textos[3].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                        }
                        break;
                    #endregion NúmeroTextoTriplo
                    case Util.Util.TipoModelo.TextoTriploNumero:
                        #region TextoTriploNúmero
                        {
                            //texto 1
                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            //numero
                            if (this.Modelo.Textos[1].FonteWindows == true)
                                conteudoTexto2.Add(StrToMatrixTrueType(this.Modelo.Textos[1]));
                            else
                                conteudoTexto2 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[1].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[1].CaminhoFonte));

                            //texto 2
                            if (this.Modelo.Textos[2].FonteWindows == true)
                                conteudoTexto3.Add(StrToMatrixTrueType(this.Modelo.Textos[2]));
                            else
                                conteudoTexto3 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[2].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[2].CaminhoFonte));

                            //texto 3
                            if (this.Modelo.Textos[3].FonteWindows == true)
                                conteudoTexto4.Add(StrToMatrixTrueType(this.Modelo.Textos[3]));
                            else
                                conteudoTexto4 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[3].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[3].CaminhoFonte));


                            //Setando a largura e a altura dos paineis no objeto Texto
                            this.Modelo.Textos[0].LarguraPainel = (this.Modelo.Textos[0].LarguraPainel == 0 ? largura - this.Modelo.Textos[1].Largura : this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].LarguraPainel = (this.Modelo.Textos[1].LarguraPainel == 0 ? this.Modelo.Textos[1].Largura : this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].LarguraPainel = (this.Modelo.Textos[2].LarguraPainel == 0 ? largura - this.Modelo.Textos[1].Largura : this.Modelo.Textos[2].LarguraPainel);
                            this.Modelo.Textos[3].LarguraPainel = (this.Modelo.Textos[3].LarguraPainel == 0 ? largura - this.Modelo.Textos[1].Largura : this.Modelo.Textos[3].LarguraPainel);

                            //TODO: fazer o calculo quando tiver com o trackbar
                            this.Modelo.Textos[0].AlturaPainel = (this.Modelo.Textos[0].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1 : this.Modelo.Textos[0].AlturaPainel); 
                            this.Modelo.Textos[1].AlturaPainel = altura;
                            this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2 : this.Modelo.Textos[2].AlturaPainel);
                            this.Modelo.Textos[3].AlturaPainel = (this.Modelo.Textos[3].AlturaPainel == 0 ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3 : this.Modelo.Textos[3].AlturaPainel);
                            //if (altura >= (this.Modelo.Textos[2].Altura + 2))
                            //{
                            //    this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? altura - this.Modelo.Textos[2].Altura : this.Modelo.Textos[1].AlturaPainel);
                            //    this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? this.Modelo.Textos[2].Altura : this.Modelo.Textos[2].AlturaPainel);
                            //}
                            //else
                            //{
                            //    this.Modelo.Textos[1].AlturaPainel = (this.Modelo.Textos[1].AlturaPainel == 0 ? 2 : this.Modelo.Textos[1].AlturaPainel);
                            //    this.Modelo.Textos[2].AlturaPainel = (this.Modelo.Textos[2].AlturaPainel == 0 ? altura - 2 : this.Modelo.Textos[2].AlturaPainel);
                            //}

                            
                            Boolean[,] texto_2_temp = new bool[this.Modelo.Textos[0].AlturaPainel, Math.Max(this.Modelo.Textos[0].Largura, this.Modelo.Textos[0].LarguraPainel)];
                            Boolean[,] NumeroTemp = new bool[altura, Math.Max(this.Modelo.Textos[1].Largura, this.Modelo.Textos[1].LarguraPainel)];
                            Boolean[,] texto_3_temp = new bool[this.Modelo.Textos[2].AlturaPainel, Math.Max(this.Modelo.Textos[2].Largura, this.Modelo.Textos[2].LarguraPainel)];
                            Boolean[,] texto_4_temp = new bool[this.Modelo.Textos[3].AlturaPainel, Math.Max(this.Modelo.Textos[3].Largura, this.Modelo.Textos[3].LarguraPainel)];

                            
                            DesenharNoPainel(conteudoTexto1, texto_2_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto2, NumeroTemp, 0, 0);
                            DesenharNoPainel(conteudoTexto3, texto_3_temp, 0, 0);
                            DesenharNoPainel(conteudoTexto4, texto_4_temp, 0, 0);

                            
                            AplicarAlinhamento(this.Modelo.Textos[0], texto_2_temp);
                            AplicarAlinhamento(this.Modelo.Textos[1], NumeroTemp);
                            AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);
                            AplicarAlinhamento(this.Modelo.Textos[3], texto_4_temp);

                            
                            this.Modelo.Textos[0].CriarListaBitMap(texto_2_temp, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
                            this.Modelo.Textos[1].CriarListaBitMap(NumeroTemp, this.Modelo.Textos[1].AlturaPainel, this.Modelo.Textos[1].LarguraPainel);
                            this.Modelo.Textos[2].CriarListaBitMap(texto_3_temp, this.Modelo.Textos[2].AlturaPainel, this.Modelo.Textos[2].LarguraPainel);
                            this.Modelo.Textos[3].CriarListaBitMap(texto_4_temp, this.Modelo.Textos[3].AlturaPainel, this.Modelo.Textos[3].LarguraPainel);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[1].listaBitMap.Count > 1)
                                this.Modelo.Textos[1].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[2].listaBitMap.Count > 1)
                                this.Modelo.Textos[2].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                            if (this.Modelo.Textos[3].listaBitMap.Count > 1)
                                this.Modelo.Textos[3].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                        }
                        break;
                    #endregion TextoTriploNúmero
                    default: // Default: Texto
                        #region Texto
                        {
                            Painel = new bool[altura, larguraTotal];

                            if (this.Modelo.Textos[0].FonteWindows == true)
                                conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
                            else
                                conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[0].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[0].CaminhoFonte));

                            this.Modelo.Textos[0].LarguraPainel = largura;
                            this.Modelo.Textos[0].AlturaPainel = altura;

                            DesenharNoPainel(conteudoTexto1, Painel, 0, 0);
                            AplicarAlinhamento(this.Modelo.Textos[0], Painel);
                            this.Modelo.Textos[0].CriarListaBitMap(Painel, altura, largura);

                            //Forçando alinhamento a esquerda se o texto for maior que o painel
                            if (this.Modelo.Textos[0].listaBitMap.Count > 1)
                                this.Modelo.Textos[0].AlinhamentoH = Util.Util.AlinhamentoHorizontal.Esquerda;

                        }
                        break;
                        #endregion Texto

                }
            }

            return Painel;
        }

        public void InverterLEDBitMap()
        {
            foreach(Texto t in this.Modelo.Textos)
            {
                for(int i=0; i< t.listaBitMap.Count;i++)
                {
                    for (int j = 0; j < t.listaBitMap[0].GetLength(0); j++)
                    { 
                        for (int z = 0; z < t.listaBitMap[0].GetLength(1); z++)
                        { 
                            if (t.listaBitMap[i][j, z] == Color.Yellow)
                                t.listaBitMap[i][j, z] = Color.Black;
                            else
                                t.listaBitMap[i][j, z] = Color.Yellow;
                        }
                    }
                }
            }
    
        }

        public void PreparaBitMapCaractere()
        {
           
            List<Boolean[,]> conteudoTexto1 = new List<bool[,]>();
            
            conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
            
            Boolean[,] painel;
            if (this.Modelo.Textos[0].LarguraPainel > 0)
                painel = new bool[this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel];
            else
            {
                painel = new bool[this.Modelo.Textos[0].AlturaPainel, conteudoTexto1[0].GetLength(1) + 1]; //Adicionando uma coluna vazia a direita porque a imagem de truetype é cortada
                this.Modelo.Textos[0].Largura = painel.GetLength(1);
                this.Modelo.Textos[0].LarguraPainel = painel.GetLength(1);
            }

            DesenharNoPainel(conteudoTexto1, painel, 0, 0);
            AplicarAlinhamento(this.Modelo.Textos[0], painel);

            this.Modelo.Textos[0].CriarListaBitMap(painel, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
        }

        public void RedimensionarCaractere()
        {
            //List<Boolean[,]> conteudoTexto1 = new List<bool[,]>();

            //conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[0]));
            if (this.Modelo.Textos[0].LarguraPainel > 0)
            {
               
                //Boolean[,] painel = new Boolean[this.Modelo.Textos[0].listaBitMap[0].GetLength(0), this.Modelo.Textos[0].listaBitMap[0].GetLength(1)];

                //painel = (Boolean[,])this.Modelo.Textos[0].listaBitMap[0].Clone();

                this.Modelo.Textos[0].CriarListaBitMap(ProcessamentoDeImagens.ResizeArray(this.ConverterColorToBoolean(this.Modelo.Textos[0].listaBitMap[0]), this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel), this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);
            }

                //if (this.Modelo.Textos[0].LarguraPainel > 0)
                //    painel = 
                //else
                //{
                //    painel = new bool[this.Modelo.Textos[0].AlturaPainel, conteudoTexto1[0].GetLength(1) + 1]; //Adicionando uma coluna vazia a direita porque a imagem de truetype é cortada
                //    this.Modelo.Textos[0].Largura = painel.GetLength(1);
                //    this.Modelo.Textos[0].LarguraPainel = painel.GetLength(1);
                //}

                //DesenharNoPainel(conteudoTexto1, painel, 0, 0);
                //AplicarAlinhamento(this.Modelo.Textos[0], painel);


        }

        public void AplicarAlinhamento(Color[,] imagem)
        {
            List<Boolean[,]> conteudoTexto1 = new List<bool[,]>();

            conteudoTexto1.Add(ConverterColorToBoolean(imagem));

            Boolean[,] painel;

                painel = new bool[this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel];


            DesenharNoPainel(conteudoTexto1, painel, 0, 0);
            AplicarAlinhamento(this.Modelo.Textos[0], painel);

            this.Modelo.Textos[0].CriarListaBitMap(painel, this.Modelo.Textos[0].AlturaPainel, this.Modelo.Textos[0].LarguraPainel);

        }

        private void log(List<String> texto)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\joao.cavalcanti\Desktop\log.txt", true))
            {
                foreach(String linha in texto) {  
                    file.WriteLine(linha); 
                }  
                
            }
        }

        public Boolean[,] ConverterColorToBoolean(Color[,] matrizColor)
        {
            Boolean[,] matrizBool = new Boolean[matrizColor.GetLength(0), matrizColor.GetLength(1)];
            for (int i = matrizColor.GetLength(0)-1; i >= 0 ; i--)
            { 
                for (int j = 0; j < matrizColor.GetLength(1); j++)
                { 
                    if (matrizColor[i, j] == Color.Yellow)
                    {
                        matrizBool[i, j] = true;
                    }
                    else
                    {
                        matrizBool[i, j] = false;
                    }
                }
            }
            return matrizBool;
        }

        public void ConverterBooleanToColor(Boolean[,] matrizBoolean, int indiceTexto)
        {
            Color[,] matrizColor = new Color[matrizBoolean.GetLength(0), matrizBoolean.GetLength(1)];
            for (int i = matrizBoolean.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < matrizBoolean.GetLength(1); j++)
                {
                    if (matrizBoolean[i, j] == true)
                    {
                        matrizColor[i, j] = Color.Yellow;
                    }
                    else
                    {
                        matrizColor[i, j] = Color.Black;
                    }
                }
            }

            this.Modelo.Textos[indiceTexto].listaBitMap.Clear();
            this.Modelo.Textos[indiceTexto].listaBitMap.Add(matrizColor);
        }
        
        /// <summary>
        /// Prepara os pixels bytes usando fontes truetype (usado por exemplo, para caracteres indianos lokães)
        /// Prepare o arquivo v02 antes de chamá-la (com altura e largura já definidas.)
        /// </summary>
        public void PreparaPixelBytesTrueType(ref Persistencia.Videos.VideoV02 v02)
        {

            Boolean[,] Painel = new bool[v02.Altura, v02.Largura];

            if (this.Modelo.Textos[0].FonteWindows == true)
            {
                //todo: testar outros alinhamentos. trabalhar primeiro em texto duplo + número.
                // Verifica o formato do arquivo que será gerado.
                if (this.TipoVideo.Equals(Util.Util.TipoVideo.V02))
                {
                    switch (this.Modelo.TipoModelo)
                    {
                        case Util.Util.TipoModelo.NúmeroTextoDuplo:
                            #region NúmeroTextoDuplo
                            {
                                Boolean[,] conteudoTexto1 = StrToMatrixTrueType(this.Modelo.Textos[0]);
                                Boolean[,] conteudoTexto2 = StrToMatrixTrueType(this.Modelo.Textos[1]);
                                Boolean[,] conteudoTexto3 = StrToMatrixTrueType(this.Modelo.Textos[2]);

                                //====== preparo do numero.

                                //Prepara uma matrix para receber o número de acordo com a largura indicada.
                                Boolean[,] NumeroTemp = new bool[v02.Altura, conteudoTexto1.GetLength(1) ];

                                //escreve o numero em um painel com a largura que ele precisa.
                                DrawMatrix(NumeroTemp, conteudoTexto1, 0, 0);

                                //Prepara o alinhamento para o número.
                                AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);

                                //====== fim numero

                                //====== texto 2
                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_2_temp = new bool[v02.Altura / 2, v02.Largura - conteudoTexto1.GetLength(1)];

                                DrawMatrix(texto_2_temp, conteudoTexto2, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                //====== fim texto 2

                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[v02.Altura / 2, v02.Largura - conteudoTexto1.GetLength(1)];

                                DrawMatrix(texto_3_temp, conteudoTexto3, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //====== fim texto 2

                                //aplica o numero.
                                DrawMatrix(Painel, NumeroTemp, 0, 0);

                                //aplica texto 1
                                DrawMatrix(Painel, texto_2_temp, (int)(v02.Altura / 2), conteudoTexto1.GetLength(1));

                                //Aplica texto 2
                                DrawMatrix(Painel, texto_3_temp, 0, conteudoTexto1.GetLength(1));
                            }
                            break;
                            #endregion NúmeroTextoDuplo
                        case Util.Util.TipoModelo.Texto:
                            #region Texto
                            {
                                Boolean[,] conteudoTexto1 =
                                           conteudoTexto1 = StrToMatrixTrueType(this.Modelo.Textos[0]);

                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_1_temp = new bool[v02.Altura, v02.Largura];

                                DrawMatrix(texto_1_temp, conteudoTexto1, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);

                                DrawMatrix(Painel, texto_1_temp, 0, 0);
                            }
                            break;
                            #endregion Texto
                        case Util.Util.TipoModelo.TextoDuplo:
                            #region texto_duplo
                            {
                                Boolean[,] conteudoTexto1 = StrToMatrixTrueType(this.Modelo.Textos[0]);
                                Boolean[,] conteudoTexto2 = StrToMatrixTrueType(this.Modelo.Textos[1]);

                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_1_temp = new bool[v02.Altura / 2, v02.Largura];

                                DrawMatrix(texto_1_temp, conteudoTexto1, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);

                                //============ texto 2
                                Boolean[,] texto_2_temp = new bool[v02.Altura / 2, v02.Largura];

                                DrawMatrix(texto_2_temp, conteudoTexto2, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                //Plota no painel principal.
                                DrawMatrix(Painel, texto_1_temp, (int)v02.Altura / 2, 0);
                                DrawMatrix(Painel, texto_2_temp, 0, 0);

                            }
                            break;
                            #endregion texto_duplo
                        case Util.Util.TipoModelo.TextoDuploNúmero:
                            #region TextoDuploNúmero
                            {
                                Boolean[,] conteudoTexto1 = StrToMatrixTrueType(this.Modelo.Textos[0]);
                                Boolean[,] conteudoTexto2 = StrToMatrixTrueType(this.Modelo.Textos[1]);
                                Boolean[,] conteudoTexto3 = StrToMatrixTrueType(this.Modelo.Textos[2]);

                                //====== preparo do numero.

                                //Prepara uma matrix para receber o número de acordo com a largura indicada.
                                Boolean[,] NumeroTemp = new bool[v02.Altura, conteudoTexto1.GetLength(1)];

                                //escreve o numero em um painel com a largura que ele precisa.
                                DrawMatrix(NumeroTemp, conteudoTexto1, 0, 0);

                                //Prepara o alinhamento para o número.
                                AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);

                                //====== fim numero

                                //====== texto 2
                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_2_temp = new bool[v02.Altura / 2, v02.Largura - conteudoTexto1.GetLength(1)];

                                DrawMatrix(texto_2_temp, conteudoTexto2, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                //====== fim texto 2

                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[v02.Altura / 2, v02.Largura - conteudoTexto1.GetLength(1)];

                                DrawMatrix(texto_3_temp, conteudoTexto3, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //====== fim texto 3

                                //aplica o numero.
                                DrawMatrix(Painel, NumeroTemp, 0, System.Convert.ToInt16(v02.Largura - conteudoTexto1.GetLength(1)));

                                //aplica texto 2
                                DrawMatrix(Painel, texto_2_temp, (int)(v02.Altura / 2), 0);

                                //Aplica texto 3
                                DrawMatrix(Painel, texto_3_temp, 0, 0);
                            }
                            break;
                            #endregion TextoDuploNúmero
                        case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                            #region TextoDuploTextoDuplo
                            {
                                Boolean[,] conteudoTexto1 = StrToMatrixTrueType(this.Modelo.Textos[0]);
                                Boolean[,] conteudoTexto2 = StrToMatrixTrueType(this.Modelo.Textos[1]);
                                Boolean[,] conteudoTexto3 = StrToMatrixTrueType(this.Modelo.Textos[2]);
                                Boolean[,] conteudoTexto4 = StrToMatrixTrueType(this.Modelo.Textos[3]);

                                //====== preparo do texto 1 (esquerda superior)

                                Boolean[,] texto_1_temp = new bool[v02.Altura / 2, v02.Largura / 2];

                                DrawMatrix(texto_1_temp, conteudoTexto1, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);

                                //====== fim texto 1

                                //====== texto 2
                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_2_temp = new bool[v02.Altura / 2, v02.Largura / 2];

                                DrawMatrix(texto_2_temp, conteudoTexto2, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], texto_2_temp);

                                //====== fim texto 2

                                //====== texto 3
                                Boolean[,] texto_3_temp = new bool[v02.Altura / 2, v02.Largura / 2];

                                DrawMatrix(texto_3_temp, conteudoTexto3, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[2], texto_3_temp);

                                //====== fim texto 3

                                //====== texto 4
                                Boolean[,] texto_4_temp = new bool[v02.Altura / 2, v02.Largura / 2];

                                DrawMatrix(texto_4_temp, conteudoTexto4, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[3], texto_4_temp);

                                //====== fim texto 4

                                //aplica o texto superior esquerdo.
                                DrawMatrix(Painel, texto_1_temp, (int)(v02.Altura / 2), 0);

                                //aplica texto superior direito.
                                DrawMatrix(Painel, texto_2_temp, (int)(v02.Altura / 2), (int)(v02.Largura / 2));

                                //Aplica texto inferior esquerdo.
                                DrawMatrix(Painel, texto_3_temp, 0, 0);

                                //Aplica texto inferior direito.
                                DrawMatrix(Painel, texto_4_temp, 0, (int)(v02.Largura / 2));
                            }
                            break;
                            #endregion TextoDuploTextoDuplo
                        case Util.Util.TipoModelo.TextoNúmero:
                            #region Texto + Número
                            {
                                Boolean[,] conteudoTexto1 = StrToMatrixTrueType(this.Modelo.Textos[0]);
                                Boolean[,] conteudoTexto2 = StrToMatrixTrueType(this.Modelo.Textos[1]);

                                //Prepara uma matrix para receber o número de acordo com a largura indicada.
                                Boolean[,] NumeroTemp = new bool[v02.Altura, conteudoTexto1.GetLength(1)];

                                //escreve o numero em um painel com a largura que ele precisa.
                                DrawMatrix(NumeroTemp, conteudoTexto1, 0, 0);

                                //Prepara o alinhamento para o número.
                                AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);

                                Boolean[,] TextoTemp = new bool[v02.Altura, v02.Largura - conteudoTexto1.GetLength(1)];
                                DrawMatrix(TextoTemp, conteudoTexto2, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], TextoTemp);

                                //desenha no painel.
                                DrawMatrix(Painel, TextoTemp, 0, 0);
                                DrawMatrix(Painel, NumeroTemp, 0, (int)(v02.Largura - conteudoTexto1.GetLength(1)));
                            }
                            break;
                            #endregion Número + Texto
                        case Util.Util.TipoModelo.NúmeroTexto:
                            #region Número + Texto
                            {
                                Boolean[,] conteudoTexto1 = StrToMatrixTrueType(this.Modelo.Textos[0]);
                                Boolean[,] conteudoTexto2 = StrToMatrixTrueType(this.Modelo.Textos[1]);

                                //Prepara uma matrix para receber o número de acordo com a largura indicada.
                                Boolean[,] NumeroTemp = new bool[v02.Altura, conteudoTexto1.GetLength(1)];

                                //escreve o numero em um painel com a largura que ele precisa.
                                DrawMatrix(NumeroTemp, conteudoTexto1, 0, 0);

                                //Prepara o alinhamento para o número.
                                AplicarAlinhamento(this.Modelo.Textos[0], NumeroTemp);

                                
                                Boolean[,] TextoTemp = new bool[v02.Altura, v02.Largura - conteudoTexto1.GetLength(1)];
                                DrawMatrix(TextoTemp, conteudoTexto2, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[1], TextoTemp);
                                
                                //desenha no painel.
                                DrawMatrix(Painel, NumeroTemp, 0, 0);
                                DrawMatrix(Painel, TextoTemp, 0, conteudoTexto1.GetLength(1));
                            }
                            break;
                            #endregion Número + Texto
                        default: // Default: Texto
                            #region Texto
                            {
                                Boolean[,] conteudoTexto1 =
                                           conteudoTexto1 = StrToMatrixTrueType(this.Modelo.Textos[0]);

                                //painel temporário para o primeiro texto.
                                Boolean[,] texto_1_temp = new bool[v02.Altura, v02.Largura];

                                DrawMatrix(texto_1_temp, conteudoTexto1, 0, 0);

                                AplicarAlinhamento(this.Modelo.Textos[0], texto_1_temp);

                                DrawMatrix(Painel, texto_1_temp, 0, 0);
                            }
                            break;
                            #endregion Texto
                    }

                    EstufaBits(ref v02, Painel);
                    //DesenharMatrix(Painel, @"c:\teste\caga.txt");
                }
            }
        }

        private void DesenharMatrix(Boolean[,] Matrix, string arquivoNome)
        {
            List<Byte> dados = new List<Byte>();
            int linha = 0;
            int coluna = 0;
            for (int i = Matrix.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (Matrix[i, j] == true)
                        dados.Add(37);
                    else dados.Add(45);

                }

                dados.Add(13);
                dados.Add(10);
            }

            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados.ToArray(), 0, dados.ToArray().Length);
            fs.Close();

        }

        private void PrepararPixelBytes(ref VideoV04 v04, uint altura, uint largura)
        {
            int larguraLocal = 0;
            int alturaLocal = 0;
            bool isMultilinhas = (this.Modelo.Textos.Count > 1) && (this.Modelo.TipoModelo == Util.Util.TipoModelo.Texto);

            
            v04.listaVideos = new List<IVideo>();
            List<Boolean[,]> conteudoTexto1 = new List<bool[,]>();
            List<Boolean[,]> conteudoTexto2 = new List<bool[,]>();
            List<Boolean[,]> conteudoTexto3 = new List<bool[,]>();
            List<Boolean[,]> conteudoTexto4 = new List<bool[,]>();

            this.CalculaLarguraFrase();

            List<VideoV02> v02Texto = new List<VideoV02>();
            v02Texto.Add(new VideoV02()); // Adiciona o texto1
            switch (this.Modelo.TipoModelo)
            {
                case Util.Util.TipoModelo.NúmeroTexto:
                    v02Texto.Add(new VideoV02());
                    break;
                case Util.Util.TipoModelo.TextoDuplo:
                    v02Texto.Add(new VideoV02());
                    break;
                case Util.Util.TipoModelo.TextoNúmero:
                    v02Texto.Add(new VideoV02());
                    break;
                case Util.Util.TipoModelo.NúmeroTextoDuplo:
                    {
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                    }
                    break;
                case Util.Util.TipoModelo.TextoDuploNúmero:
                    {
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                    }
                    break;
                case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                    {
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                    }
                    break;
                case Util.Util.TipoModelo.TextoTriplo:
                    {
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                    }
                    break;
                case Util.Util.TipoModelo.TextoTriploNumero:
                    {
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                    }
                    break;
                case Util.Util.TipoModelo.NumeroTextoTriplo:
                    {
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                        v02Texto.Add(new VideoV02());
                    }
                    break;
            }
            if (isMultilinhas)
            {
                for (int i = 0; i < this.Modelo.Textos.Count - 1; i++)
                {
                    v02Texto.Add(new VideoV02());
                }
            }

            for (int j = 0; j < v02Texto.Count; j++)
            {
                Boolean[,] Painel = new bool[this.Modelo.Textos[j].Altura, this.Modelo.Textos[j].Largura];

                v02Texto[j].alinhamento = (byte)(this.Modelo.Textos[j].AlinhamentoH);
                v02Texto[j].Altura = (uint)this.Modelo.Textos[j].AlturaPainel;
                v02Texto[j].animacao = (byte)this.Modelo.Textos[j].Apresentacao;
                v02Texto[j].Largura = (uint)Math.Max(this.Modelo.Textos[j].Largura, this.Modelo.Textos[j].LarguraPainel);
                v02Texto[j].tempoApresentacao = (uint)this.Modelo.Textos[j].TempoApresentacao;
                v02Texto[j].tempoRolagem = (uint)this.Modelo.Textos[j].TempoRolagem;

                //conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[j].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[j].CaminhoFonte));

                if (this.Modelo.Textos[j].FonteWindows == true)
                {
                    conteudoTexto1.Clear();
                    conteudoTexto1.Add(StrToMatrixTrueType(this.Modelo.Textos[j]));
                }
                else
                    conteudoTexto1 = Nucleo.ProcessamentoDeImagens.StrToMatrix(this.Modelo.Textos[j].LabelTexto, Nucleo.ProcessamentoDeImagens.RetornaFonte(this.Modelo.Textos[j].CaminhoFonte));


                larguraLocal = Math.Max(this.Modelo.Textos[j].Largura, this.Modelo.Textos[j].LarguraPainel);
                alturaLocal = this.Modelo.Textos[j].AlturaPainel;


                //painel temporário para o primeiro texto.
                Boolean[,] texto_1_temp = new bool[alturaLocal, larguraLocal];
                DesenharNoPainel(conteudoTexto1, texto_1_temp, 0, 0);
                AplicarAlinhamento(this.Modelo.Textos[j], texto_1_temp);
                if (this.Modelo.Textos[j].Apresentacao == Util.Util.Rolagem.Rolagem_Continua3_Esquerda)
                {
                    texto_1_temp = RetirarEspacosVaziosMatrix(texto_1_temp);
                }
                long nelementos = 0;
                Boolean[] painelEmArray = new bool[nelementos];

                nelementos = texto_1_temp.GetLength(0) * texto_1_temp.GetLength(1);
                painelEmArray = UnidimensionaMatriz(texto_1_temp);
                VideoV02 v02temp = v02Texto[j];
                EstufaBits(ref v02temp, texto_1_temp);
                v02Texto[j] = v02temp;
            }

            v04.listaVideos.AddRange(v02Texto.ToArray());


            #region Texto1
            v04.listaFrames = new List<VideoV04.FormatoFrame>();

            VideoV04.FormatoFrame formatoFrame = new VideoV04.FormatoFrame();
            formatoFrame.height = (uint)this.Modelo.Textos[0].AlturaPainel;
            formatoFrame.width = (uint)this.Modelo.Textos[0].LarguraPainel;
            formatoFrame.x = 0;
            formatoFrame.y = 0;


            v04.listaFrames.Add(formatoFrame);
            #endregion Texto1

            #region Texto2

            formatoFrame = new VideoV04.FormatoFrame();

            if ((this.Modelo.TipoModelo != Util.Util.TipoModelo.Texto))
            {
                //  Texto 2
                formatoFrame.height = (uint)this.Modelo.Textos[1].AlturaPainel;
                formatoFrame.width = (uint)this.Modelo.Textos[1].LarguraPainel;
                formatoFrame.x = 0;
                formatoFrame.y = 0;

                switch (this.Modelo.TipoModelo)
                {
                    case Util.Util.TipoModelo.NúmeroTexto:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[0].LarguraPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoDuplo:
                        {
                            formatoFrame.y = (uint)this.Modelo.Textos[0].AlturaPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoNúmero:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[0].LarguraPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.NúmeroTextoDuplo:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[0].LarguraPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoDuploNúmero:
                        {
                            formatoFrame.x = (uint)Math.Max(this.Modelo.Textos[0].LarguraPainel, this.Modelo.Textos[2].LarguraPainel);
                        }
                        break;
                    case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[0].LarguraPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoTriplo:
                        {
                            formatoFrame.y = (uint)this.Modelo.Textos[0].AlturaPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.NumeroTextoTriplo:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[0].LarguraPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoTriploNumero:
                        {
                            formatoFrame.x = (uint)Math.Max(Math.Max(this.Modelo.Textos[0].LarguraPainel, this.Modelo.Textos[2].LarguraPainel), this.Modelo.Textos[3].LarguraPainel);
                        }
                        break;
                }

                v04.listaFrames.Add(formatoFrame);
            }
            #endregion Texto2

            #region Texto3
            if ((this.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo) || (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero) || (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo) ||
                (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo) || (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero) || (this.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo))
            {
                formatoFrame = new VideoV04.FormatoFrame();

                formatoFrame.height = (uint)this.Modelo.Textos[2].AlturaPainel;
                formatoFrame.width = (uint)this.Modelo.Textos[2].LarguraPainel;
                formatoFrame.x = 0;

                switch (this.Modelo.TipoModelo)
                {
                    case Util.Util.TipoModelo.NúmeroTextoDuplo:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[0].LarguraPainel;
                            formatoFrame.y = (uint)this.Modelo.Textos[1].AlturaPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoDuploNúmero:
                        {
                            formatoFrame.y = (uint)this.Modelo.Textos[0].AlturaPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                        {
                            formatoFrame.y = (uint)this.Modelo.Textos[0].AlturaPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoTriplo:
                        {
                            formatoFrame.y = (uint)(this.Modelo.Textos[0].AlturaPainel + this.Modelo.Textos[1].AlturaPainel);
                        }
                        break;
                    case Util.Util.TipoModelo.NumeroTextoTriplo:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[0].LarguraPainel;
                            formatoFrame.y = (uint)this.Modelo.Textos[1].AlturaPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.TextoTriploNumero:
                        {
                            formatoFrame.y = (uint)this.Modelo.Textos[0].AlturaPainel;
                        }
                        break;
                }
                v04.listaFrames.Add(formatoFrame);
            }
            #endregion Texto3

            #region Texto4
            if (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo || (this.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero) || (this.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo))
            {
                formatoFrame = new VideoV04.FormatoFrame();
                formatoFrame.height = (uint)this.Modelo.Textos[3].AlturaPainel;
                formatoFrame.width = (uint)this.Modelo.Textos[3].LarguraPainel;

                switch (this.Modelo.TipoModelo)
                {
                    case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[2].LarguraPainel;
                            formatoFrame.y = (uint)this.Modelo.Textos[1].AlturaPainel;
                        }
                        break;
                    case Util.Util.TipoModelo.NumeroTextoTriplo:
                        {
                            formatoFrame.x = (uint)this.Modelo.Textos[0].LarguraPainel;
                            formatoFrame.y = (uint)(this.Modelo.Textos[1].AlturaPainel + this.Modelo.Textos[2].AlturaPainel);
                        }
                        break;
                    case Util.Util.TipoModelo.TextoTriploNumero:
                        {
                            formatoFrame.x = 0;
                            formatoFrame.y = (uint)(this.Modelo.Textos[0].AlturaPainel + this.Modelo.Textos[2].AlturaPainel);
                        }
                        break;
                }

                v04.listaFrames.Add(formatoFrame);
            }
            #endregion Texto4

            #region Multilinhas

            v04.qntFrames = (uint) this.Modelo.Textos.Count;
            if (isMultilinhas)
            {
                int alturaLocalAcumulado = this.Modelo.Textos[0].AlturaPainel;

                for (int i = 1; i < this.Modelo.Textos.Count; i++)
                {
                    formatoFrame = new VideoV04.FormatoFrame();
                    formatoFrame.height = (uint)this.Modelo.Textos[1].AlturaPainel;
                    formatoFrame.width = (uint)this.Modelo.Textos[1].LarguraPainel;
                    formatoFrame.x = 0;
                    formatoFrame.y = (uint)alturaLocalAcumulado;
                    alturaLocalAcumulado += this.Modelo.Textos[i].AlturaPainel;
                    v04.listaFrames.Add(formatoFrame);
                }

            }
            #endregion Multilinhas
        }

        private void PrepararTextos()
        {
            int qtdEspacos = 0;
            for (int i = 0; i<this.Modelo.Textos.Count; i++)
            {
                if (this.Modelo.Textos[0].AlinhamentoH == Util.Util.AlinhamentoHorizontal.Esquerda)
                {
                    qtdEspacos = this.Modelo.Textos[i].LabelTexto.Length;
                    this.Modelo.Textos[i].LabelTexto = this.Modelo.Textos[i].LabelTexto.TrimStart().PadRight(qtdEspacos, ' ');
                }
            }
            
        }
    }
}
