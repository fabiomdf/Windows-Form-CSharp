using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistencia;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using System.Text.RegularExpressions;


namespace Nucleo
{
    public static class ProcessamentoDeImagens
    {
        //        static Regex pattern = new Regex(@"{{:([A-Za-z0-9\-\ ]+):}}");

        //static Regex pattern = new Regex(Util.Util.ABRE_TAG + @"([A-z0-9\-\ ]+)" + Util.Util.FECHA_TAG);      
        static Regex pattern = new Regex(Util.Util.ABRE_TAG + @"([\w\W\s\S\D\d])+?" + Util.Util.FECHA_TAG);
        public static String diretorioBitmap = @"C:\Temp\";
        /// <summary>
        /// Monta a string com caracteres vindos direto da fonte.
        /// </summary>
        /// <param name="texto">String contendo texto desejado.</param>
        /// <param name="afnt">Arquivo fonte com os caracteres que formarão o texto no painel.</param>
        /// <returns>Matriz de booleanos com o texto desenhado com caracteres de afnt.</returns>
        public static List<Boolean[,]> StrToMatrix(String texto, String fnt)
        {            
            Arquivo_FNT afnt = new Arquivo_FNT();
            afnt.Abrir(fnt);

            return StrToMatrix(texto, afnt);            
        }
        ///// <summary>
        ///// Alimenta um Arquivo v02 com seu pixel bytes.
        ///// cria uma imagem dentro do arquivo v02.
        ///// </summary>
        ///// <param name="v02">Arquivo V02 cuja imagem deve ser gerada.</param>
        ///// <param name="texto">texto a partir do qual a imagem será gerada.</param>
        ///// <param name="fonte">fonte usada para criação da imagem.</param>
        //public static void preparaPixelBytes(ref Persistencia.Videos.VideoV02 v02,
        //                               String texto,
        //                               Arquivo_FNT fonte,
        //                               Arquivo_FNT fonteTexto1,
        //                               Arquivo_FNT fonteTexto2,
        //                               string numeroRota,
        //                               string largura_numero)
        //{
        //    string tipoArquivo = texto.Substring(0, 4);
        //    string formatoTipo = texto.Substring(4, 1);
        //    string alinhamentoH = texto.Substring(5, 1);
        //    string alinhamentoV = texto.Substring(6, 1);
        //    int iLargura_numero = 0;

        //    if (largura_numero != string.Empty)
        //    {
        //        iLargura_numero = System.Convert.ToInt32(largura_numero);
        //    }

        //    String texto_1 = string.Empty;
        //    String texto_2 = string.Empty;

        //    List<Boolean[,]> conteudoNumero = new List<bool[,]>();
        //    List<Boolean[,]> conteudoTexto1 = new List<bool[,]>();
        //    List<Boolean[,]> conteudoTexto2 = new List<bool[,]>();

        //    //Instancia o painel principal.
        //    Boolean[,] Painel = new bool[v02.Altura, v02.Largura];

        //    //todo: testar outros alinhamentos. trabalhar primeiro em texto duplo + número.
        //    // Verifica o formato do arquivo que será gerado.
        //    if (tipoArquivo.Equals(".v02"))
        //    {
        //        switch (formatoTipo)
        //        {
        //            case " ": // Default: Numero + texto
        //                {
        //                    // Carrega o texto
        //                    texto_1 = texto.Substring(8);
        //                    conteudoTexto1 = StrToMatrix(texto_1, fonte);
        //                    conteudoNumero = StrToMatrix(numeroRota, fonte); //Lista de caracteres que formam o numero.

        //                    //Prepara uma matrix para receber o número de acordo com a largura indicada.
        //                    Boolean[,] NumeroTemp = new bool[v02.Altura, iLargura_numero];

        //                    //escreve o numero em um painel com a largura que ele precisa.
        //                    DesenhaNoPainel(conteudoNumero, NumeroTemp, 0, 0);

        //                    //Prepara o alinhamento para o número.
        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, NumeroTemp);

        //                    // Plotar Matrix Numero na Matrix Principal
        //                    DrawMatrix(Painel, NumeroTemp, 0, 0);

        //                    //primeiro desenha, depois alinha centralizando horizontal e vertical.
        //                    DesenhaNoPainel(conteudoTexto1, Painel, 0, iLargura_numero);

        //                    //DesenhaMatrix(Painel, @"C:\teste\painel.txt");
        //                    //Process.Start(@"c:\teste\");

        //                    // Plotar Matrix Texto 1 na Matrix Principal
        //                    //int origem_texto = LarguraListaCaracteres(conteudoNumero); //calcula a origem do texto.


        //                }
        //                break;

        //            #region TEXTO_DUPLO
        //            case "0": // Tipo 0: Texto Duplo
        //                {
        //                    // Carrega o texto da primeira linha
        //                    texto_1 = texto.Substring(8, texto.LastIndexOf('|') - 8);
        //                    // Carrega o texto da segunda linha
        //                    texto_2 = texto.Substring(texto.LastIndexOf('|') + 1);
        //                    conteudoTexto1 = StrToMatrix(texto_1, fonte);
        //                    conteudoTexto2 = StrToMatrix(texto_2, fonte);

        //                    //painel temporário para o primeiro texto.
        //                    Boolean[,] texto_1_temp = new bool[v02.Altura / 2, v02.Largura];

        //                    DesenhaNoPainel(conteudoTexto1, texto_1_temp, 0, 0);

        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, texto_1_temp);

        //                    //============ texto 2
        //                    Boolean[,] texto_2_temp = new bool[v02.Altura / 2, v02.Largura];

        //                    DesenhaNoPainel(conteudoTexto2, texto_2_temp, 0, 0);

        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, texto_2_temp);

        //                    //Plota no painel principal.
        //                    DrawMatrix(Painel, texto_1_temp, (int)v02.Altura / 2, 0);
        //                    DrawMatrix(Painel, texto_2_temp, 0, 0);

        //                    //                            DesenhaMatrix(Painel, @"C:\teste\painel.txt");
        //                    //                            Process.Start(@"c:\teste\");

        //                }
        //                break;
        //            #endregion

        //            case "1": // Tipo 1: Numero + Texto Duplo
        //                {
        //                    // Carrega o texto da primeira linha
        //                    texto_1 = texto.Substring(8, texto.LastIndexOf('|') - 8);
        //                    // Carrega o texto da segunda linha
        //                    texto_2 = texto.Substring(texto.LastIndexOf('|') + 1);

        //                    conteudoNumero = StrToMatrix(numeroRota, fonte);
        //                    conteudoTexto1 = StrToMatrix(texto_1, fonte);
        //                    conteudoTexto2 = StrToMatrix(texto_2, fonte);

        //                    //====== preparo do numero.

        //                    //Prepara uma matrix para receber o número de acordo com a largura indicada.
        //                    Boolean[,] NumeroTemp = new bool[v02.Altura, iLargura_numero];

        //                    //escreve o numero em um painel com a largura que ele precisa.
        //                    DesenhaNoPainel(conteudoNumero, NumeroTemp, 0, 0);

        //                    //Prepara o alinhamento para o número.
        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, NumeroTemp);

        //                    //====== fim numero

        //                    //====== texto 1
        //                    //painel temporário para o primeiro texto.
        //                    Boolean[,] texto_1_temp = new bool[v02.Altura / 2, v02.Largura - iLargura_numero];

        //                    DesenhaNoPainel(conteudoTexto1, texto_1_temp, 0, 5);

        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, texto_1_temp);

        //                    //====== fim texto 1

        //                    //====== texto 2
        //                    Boolean[,] texto_2_temp = new bool[v02.Altura / 2, v02.Largura - iLargura_numero];

        //                    DesenhaNoPainel(conteudoTexto2, texto_2_temp, 0, 0);

        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, texto_2_temp);

        //                    //====== fim texto 2

        //                    //aplica o numero.
        //                    DrawMatrix(Painel, NumeroTemp, 0, 0);

        //                    //aplica texto 1
        //                    DrawMatrix(Painel, texto_1_temp, (int)(v02.Altura / 2), iLargura_numero);

        //                    //Aplica texto 2
        //                    DrawMatrix(Painel, texto_2_temp, 0, iLargura_numero);

        //                    //DesenhaMatrix(Painel, @"C:\teste\painel.txt");
        //                    //Process.Start(@"c:\teste\");


        //                }
        //                break;
        //            case "2": // Tipo 2: Texto Duplo + Numero
        //                {
        //                    // Carrega o texto da primeira linha
        //                    texto_1 = texto.Substring(8, texto.LastIndexOf('|') - 8);

        //                    // Carrega o texto da segunda linha
        //                    texto_2 = texto.Substring(texto.LastIndexOf('|') + 1);

        //                    conteudoNumero = StrToMatrix(numeroRota, fonte);
        //                    conteudoTexto1 = StrToMatrix(texto_1, fonte);
        //                    conteudoTexto2 = StrToMatrix(texto_2, fonte);

        //                    //====== preparo do numero.

        //                    //Prepara uma matrix para receber o número de acordo com a largura indicada.
        //                    Boolean[,] NumeroTemp = new bool[v02.Altura, iLargura_numero];

        //                    //escreve o numero em um painel com a largura que ele precisa.
        //                    DesenhaNoPainel(conteudoNumero, NumeroTemp, 0, 0);

        //                    //Prepara o alinhamento para o número.
        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, NumeroTemp);

        //                    //====== fim numero

        //                    //====== texto 1
        //                    //painel temporário para o primeiro texto.
        //                    Boolean[,] texto_1_temp = new bool[v02.Altura / 2, v02.Largura - iLargura_numero];

        //                    DesenhaNoPainel(conteudoTexto1, texto_1_temp, 0, 0);

        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, texto_1_temp);

        //                    //====== fim texto 1

        //                    //====== texto 2
        //                    Boolean[,] texto_2_temp = new bool[v02.Altura / 2, v02.Largura - iLargura_numero];

        //                    DesenhaNoPainel(conteudoTexto2, texto_2_temp, 0, 0);

        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, texto_2_temp);

        //                    //====== fim texto 2

        //                    //aplica o numero.
        //                    DrawMatrix(Painel, NumeroTemp, 0, System.Convert.ToInt16(v02.Largura - iLargura_numero));

        //                    //aplica texto 1
        //                    DrawMatrix(Painel, texto_1_temp, (int)(v02.Altura / 2), 0);

        //                    //Aplica texto 2
        //                    DrawMatrix(Painel, texto_2_temp, 0, 0);

        //                    //DesenhaMatrix(Painel, @"C:\teste\painel.txt");
        //                    //Process.Start(@"c:\teste\");


        //                }
        //                break;
        //            case "3": // Tipo 3: Somente Texto; OBS.: Este não será utilizado devido ao V01
        //                {
        //                    texto_1 = texto.Substring(8);
        //                    conteudoTexto1 = StrToMatrix(texto_1, fonte);

        //                    //temporário para caso se precise fazer alguma operação intermediária.
        //                    Boolean[,] texto_1_temp = new bool[v02.Altura, v02.Largura];

        //                    //desenha no temporário.
        //                    DesenhaNoPainel(conteudoTexto1, texto_1_temp, 0, 0);

        //                    //aplica alinhamento.
        //                    AplicaAlinhamento(alinhamentoH, alinhamentoV, texto_1_temp);

        //                    //grava no v02 propriamente dito.
        //                    DrawMatrix(Painel, texto_1_temp, 0, 0);
        //                }

        //                break;
        //        }


        //        long nelementos = 0;
        //        Boolean[] painelEmArray = new bool[nelementos];

        //        nelementos = Painel.GetLength(0) * Painel.GetLength(1);
        //        painelEmArray = UnidimensionaMatriz(Painel);

        //        //copia todos os elementos para pixelbytes
        //        v02.pixelBytes = new byte[nelementos / 8];
        //        int indice_bmp = 0;
        //        for (int i = 0; i < painelEmArray.Length; i++)
        //        {
        //            byte aux = (byte)(System.Convert.ToByte(painelEmArray[i]) << 0);
        //            i++;
        //            aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 1);
        //            i++;
        //            aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 2);
        //            i++;
        //            aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 3);
        //            i++;
        //            aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 4);
        //            i++;
        //            aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 5);
        //            i++;
        //            aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 6);
        //            i++;
        //            aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 7);

        //            v02.pixelBytes[indice_bmp] = aux;

        //            indice_bmp++;


        //            /*for (int e = 0; e < pbytesTemp[i].Count(); e++)
        //            {
        //                byte aux = (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 0);
        //                e++;
        //                aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 1);
        //                e++;
        //                aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 2);
        //                e++;
        //                aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 3);
        //                e++;
        //                aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 4);
        //                e++;
        //                aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 5);
        //                e++;
        //                aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 6);
        //                e++;
        //                aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 7);

        //                v02.pixelBytes[indice_bmp] = aux;

        //                indice_bmp++;
        //            }
        //             */
        //        }
        //    }
        //}//fim de preparaPixelBytes!!!






        ///// <summary>
        ///// Prepara um Video02 com atributos de um vídeo 01.
        ///// Apenas uma linha, com uma determinada fonte.
        ///// </summary>
        ///// <param name="v02">Arquivo Video02 a ser modificado.</param>
        ///// <param name="Texto">Texto a ser desenhado no V02.</param>
        ///// <param name="diretorio_fonte">Diretório do arquivo fonte a ser usado.</param>
        //public static void PreparaVideo02ApenasTexto(ref Persistencia.Videos.VideoV02 v02,
        //                                             String Texto,
        //                                             String diretorio_fonte)
        //{
        //    //todo: falta pôr os alinhamentos e formatação.

        //    preparaPixelBytes(ref v02,
        //                      Util.Util.FORMATAÇÃO_SAUDAÇÃO + Texto,
        //                      RetornaFonte(diretorio_fonte),
        //                      RetornaFonte(diretorio_fonte),
        //                      RetornaFonte(diretorio_fonte),
        //                      "0",
        //                      "0");
        //}

        /// <summary>
        /// Prepara os pixels bytes usando fontes truetype (usado por exemplo, para caracteres indiandos lokães)
        /// Prepare o arquivo v02 antes de chamá-la (com altura e largura já definidas.)
        /// </summary>
        public static void PreparaPixelBytesTrueType(ref Persistencia.Videos.VideoV02 v02,
                                                     String alinhamentoH,
                                                     Util.Util.AlinhamentoVertical alinhamentoV,
                                                     String Texto,
                                                     Font fonte
                                                     )
        {

            System.Drawing.Bitmap bTemp;
            Boolean[,] Result;
            Color textColor = Color.FromArgb(0, 0, 0); //cor do texto
            Color backColor = Color.FromArgb(255, 255, 255); //cor da fonte

            //converte o bitmap diretamente usando o método estático
            bTemp = TextImage.MakeTextBitmap(Texto, fonte, textColor, backColor, 200);//.Save("result.bmp");
            Result = new Boolean[bTemp.Height, bTemp.Width];

            //for (int altura = 0; altura < bTemp.Height; altura++)            
            for (int altura = bTemp.Height - 1; altura >= 0 ; altura--)            
            {
               for (int largura = 0; largura < bTemp.Width; largura++)
                {
                    //if (bTemp.GetPixel(largura, altura) == Color.White)
                    if (VerificaBranco(bTemp.GetPixel(largura, bTemp.Height - altura - 1)))
                        Result[altura, largura] = false;
                    else
                        Result[altura, largura] = true;
                }
            }

            //temporário para caso se precise fazer alguma operação intermediária.
            Boolean[,] texto_temp = new bool[v02.Altura, v02.Largura];

            //desenha no temporário.
            
            DrawMatrix(texto_temp, Result, 0, 0);
            
            //todo: verificar tamanho do painel. desenho ainda não está desenhado no painel.
            //AplicaAlinhamento(alinhamentoH, alinhamentoV, texto_temp);
            Util.Util.AlinhamentoHorizontal alinhamentoHorizontal = 0;

            alinhamentoHorizontal = (Util.Util.AlinhamentoHorizontal)Util.Util.RetornaAlinhamentoHorizontal(alinhamentoH);
            AplicarAlinhamento(alinhamentoHorizontal, alinhamentoV, texto_temp);
            //DesenhaMatrix(texto_temp);
            EstufaBits(ref v02, texto_temp);
            
        }

        private static Boolean VerificaBranco(Color c)
        {
            return ((c.A == 0xFF) & (c.R == 0xFF) & (c.G == 0xFF) & (c.B == 0xFF));
        }

        public static void DesenhaMatrix(Boolean[,] Matrix)
        {
            string arquivoNome = @"C:\teste\MATRIX.txt";
            List<Byte> dados = new List<Byte>();

            //escrita de cima pra baixo.
            //for(int i = 0; i < Matriz.Count; i++)
            //{
            //    for(int j = 0; j < Matriz[i].Count; j++)
            //    {
            //        if (Matriz[i][j] == true)
            //            dados.Add(37);
            //        else dados.Add(45);
            //    }

            //    dados.Add(13);
            //    dados.Add(10);
            //}

            //escrita da esquerda para a direita.

            //int linha = 0;
            //int coluna = Matrix.GetLength(1) - 1;

            //while (true)
            //{
            //    if (Matrix[linha, coluna] == true)
            //        dados.Add(37);
            //    else dados.Add(45);

            //    linha = linha + 1;

            //    if (linha == Matrix.GetLength(0))
            //    {
            //        linha = 0;
            //        coluna = coluna - 1; &

            //        dados.Add(13);
            //        dados.Add(10);

            //        if (coluna == -1) break;
            //    }

            //}

            /*
            int linha = 0;
            int coluna = 0;

            while (true)
            {
                if (Matrix[linha, coluna] == true)
                    dados.Add(37);
                else dados.Add(45);

                coluna = coluna + 1;

                if (coluna == Matrix.GetLength(1))
                {
                    linha = linha + 1;
                    coluna = 0;

                    dados.Add(13);
                    dados.Add(10);

                    if (linha == Matrix.GetLength(0)) break;
                }
            }
            */

            for (int i = 0; i < Matrix.GetLength(0); i++)
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

        public static bool ContemImagem(String texto)
        {
            //Testa o padrão
            Match matcher = pattern.Match(texto);
            String textoSemImagem = texto;

            return (matcher.Success);
        }

        /// <summary>
        /// Monta a string com caracteres vindos direto da fonte.
        /// </summary>
        /// <param name="texto">String contendo texto desejado.</param>
        /// <param name="afnt">Arquivo fonte com os caracteres que formarão o texto no painel.</param>
        /// <returns>Matriz de booleanos com o texto desenhado com caracteres de afnt.</returns>
        public static List<Boolean[,]> StrToMatrix(String texto, Arquivo_FNT afnt)
        {                          
            //Testa o padrão
            Match matcher = pattern.Match(texto);
             String textoSemImagem = texto;

             if (matcher.Success)
             {
                 string tag = matcher.Value.ToString();
                 textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);

                 while (matcher.Success)
                 {
                     matcher = pattern.Match(textoSemImagem);
                     if (matcher.Success)
                     {
                         tag = matcher.Value.ToString();
                         textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                     }
                 }

             }
            
            List<Boolean[,]> Result = new List<Boolean[,]>();
            byte[] asciiBytes = new byte[textoSemImagem.Length]; // Encoding.ASCII.GetBytes(texto);

            for (int i = 0; i < textoSemImagem.Length; i++)
            {
                if ((uint)textoSemImagem[i] > 255)
                {
                    //asciiBytes = Encoding.ASCII.GetBytes(textoSemImagem);
                    //break;
                    byte valor = 63;
                    switch ((uint)textoSemImagem[i])
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
                    asciiBytes[i] = valor;
                }
                else
                    asciiBytes[i] = (byte)textoSemImagem[i];
            }

            //System.Text.Encoding.ASCII.GetString(asciiBytes[8]);

            foreach (byte b in asciiBytes)
            {
                // Código abaixo para a Feira de São Paulo 
               // Result.Add(ResizeArray(AplicarAlinhamento(Util.Util.AlinhamentoHorizontal.Centralizado, Util.Util.AlinhamentoVertical.Centro, RotateMatrix(ResizeArray(afnt.bitmaps[b - 32].matriz, 16, 16), 16)), 16, 17));
                // Código correto para o funcionamento normal
                Result.Add(afnt.bitmaps[b - 32].matriz);
            }
            matcher = pattern.Match(texto);
            textoSemImagem = texto;
            int numeroImagens = 0;
            if (matcher.Success)
            {
                string tag = matcher.Value.ToString();                
                int indice = texto.IndexOf(tag);
                
                textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                
                Boolean[,] auxiliar = BitmapToMatrix(tag);

                if (null != auxiliar)
                {
                    Result.Insert(indice + numeroImagens, auxiliar);
                    numeroImagens++;
                }
                
                while (matcher.Success)
                {
                    matcher = pattern.Match(textoSemImagem);
                    if (matcher.Success)
                    {
                        tag = matcher.Value.ToString();
                        indice = textoSemImagem.IndexOf(tag);

                        textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);

                        auxiliar = BitmapToMatrix(tag);

                        if (null != auxiliar)
                        {
                            Result.Insert(indice + numeroImagens, auxiliar);
                            numeroImagens++;
                        }
                    }
                }
            }

            return Result;
        }
        public static string GetTextoSemImagem(string labelTexto)
        {
            //Testa o padrão
            Match matcher = pattern.Match(labelTexto);
            String textoSemImagem = labelTexto;
            List<String> textos = new List<string>();
            if (matcher.Success)
            {
                string tag = matcher.Value.ToString();
                
                textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                
                while (matcher.Success)
                {
                    string[] separador = new string[1];
                    separador[0] = tag;
                    //textos.Clear();
                    matcher = pattern.Match(textoSemImagem);
                    if (matcher.Success)
                    {
                        tag = matcher.Value.ToString();
                        textos.AddRange(textoSemImagem.Split(separador, StringSplitOptions.None));
                        textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);                        
                    }
                }

            }
            return textoSemImagem;
        }
        public static List<string> GetTextosSemImagem(string labelTexto)
        {
            //Testa o padrão
            Match matcher = pattern.Match(labelTexto);
            String textoSemImagem = labelTexto;
            List<String> textos = new List<string>();
            if (matcher.Success)
            {
                string tag = matcher.Value.ToString();
                string[] separador = new string[1];
                separador[0] = tag;

                textos.AddRange(textoSemImagem.Split(separador, StringSplitOptions.None));
                textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);

                while (matcher.Success)
                {
                    
                    //textos.Clear();
                    matcher = pattern.Match(textoSemImagem);
                    if (matcher.Success)
                    {
                        string tag2 = matcher.Value.ToString();
                        if (tag != tag2)
                        {
                            separador[0] = tag2;
                            textos.AddRange(textoSemImagem.Split(separador, StringSplitOptions.None));
                            
                        }
                        textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag2, String.Empty, 1);
                    }
                }

            }

            if (textos.Count == 0)
                textos.Add(textoSemImagem);
            
            
            for (int i = textos.Count -1; i >= 0; i--)
            {
                if (String.IsNullOrEmpty(textos[i]))
                {
                    textos.RemoveAt(i);
                }
                if (i < textos.Count)
                {
                    if ((textos[i].Contains("{{:")) && (textos[i].Contains(":}}")) && (!textos[i].Contains("{{:}}") && (!textos[i].Contains("{{::}}"))))
                    {
                        string aux = (string) textos[i].Clone();
                        textos.RemoveAt(i);
                        textos.InsertRange(i, GetTextosSemImagem(aux));
                    }
                }
            }
            return textos;
        }
        public static Boolean[,] StrToMatrixTrueType(String labelTexto, String fonteTexto, int alturaTexto, bool italicoTexto, bool negritoTexto, bool sublinhadoTexto, int binaryThresholdTexto, Util.Util.AlinhamentoVertical av, Util.Util.AlinhamentoHorizontal ah)
        {
            List<Boolean[,]> listaImagensTextos = new List<Boolean[,]>();
            List<Boolean[,]> listaImagensBitmaps = new List<Boolean[,]>();

            //Testa o padrão
            Match matcher = pattern.Match(labelTexto);

            List<string> listaTextos = GetTextosSemImagem(labelTexto);
            //List<int> listaPosicoesImagens = GetPosicoesImagens(labelTexto, matcher, listaTextos);
            String textoSemImagem = (listaTextos.Count > 0)?listaTextos[0]:String.Empty;
            int alturaLocal = alturaTexto;
            Boolean[,] texto_temp;
            int countEsquerda = 0;
            int countDireita = 0;

            for (int indiceTexto = 0; indiceTexto < listaTextos.Count; indiceTexto++)
            {
                textoSemImagem = listaTextos[indiceTexto];
                if (String.IsNullOrEmpty(textoSemImagem))
                {
                    continue;
                }

                System.Drawing.Bitmap bTemp = new System.Drawing.Bitmap(1, 1);
                Boolean[,] Result;
                Color textColor = Color.FromArgb(0, 0, 0); //cor do texto
                Color backColor = Color.FromArgb(255, 255, 255); //cor da fonte
                FontStyle fsTexto = new FontStyle();

                fsTexto = FontStyle.Regular;

                InstalledFontCollection ifc = new InstalledFontCollection();
                for (int i = 0; i < ifc.Families.Length; i++)
                {
                    if (ifc.Families[i].Name == fonteTexto)
                    {
                        if (ifc.Families[i].IsStyleAvailable(FontStyle.Regular))
                        {
                            fsTexto = FontStyle.Regular;
                        }
                        else if (ifc.Families[i].IsStyleAvailable(FontStyle.Bold))
                        {
                            fsTexto = FontStyle.Bold;
                        }
                        else if (ifc.Families[i].IsStyleAvailable(FontStyle.Italic))
                        {
                            fsTexto = FontStyle.Italic;
                        }
                        else if (ifc.Families[i].IsStyleAvailable(FontStyle.Underline))
                        {
                            fsTexto = FontStyle.Underline;
                        }
                        break;
                    }
                }

                if (italicoTexto)
                    fsTexto = fsTexto | FontStyle.Italic;

                if (negritoTexto)
                    fsTexto = fsTexto | FontStyle.Bold;

                if (sublinhadoTexto)
                    fsTexto = fsTexto | FontStyle.Underline;



                //converte o bitmap diretamente usando o método estático
                bTemp = TextImage.MakeTextBitmap(textoSemImagem, new Font(fonteTexto, alturaTexto, fsTexto), textColor, backColor, binaryThresholdTexto);
                Result = new Boolean[bTemp.Height, bTemp.Width];

                //for (int altura = 0; altura < bTemp.Height; altura++)            
                for (int altura = bTemp.Height - 1; altura >= 0; altura--)
                {
                    for (int largura = 0; largura < bTemp.Width; largura++)
                    {
                        //if (bTemp.GetPixel(largura, altura) == Color.White)
                        if (VerificaBranco(bTemp.GetPixel(largura, bTemp.Height - altura - 1)))
                            Result[altura, largura] = false;
                        else
                            Result[altura, largura] = true;
                    }
                }

                if (Result.LongLength == 1)
                {
                    Result[0, 0] = false;
                }

                //temporário para caso se precise fazer alguma operação intermediária.
                texto_temp = new bool[bTemp.Height, bTemp.Width];

                //desenha no temporário.
                DrawMatrix(texto_temp, Result, 0, 0);

                listaImagensTextos.Add(texto_temp);
            }

            matcher = pattern.Match(labelTexto);
            textoSemImagem = labelTexto;
            int numeroImagens = 0;
            int numeroTextos = 0;

            

            if (matcher.Success)
            {
                string tag = matcher.Value.ToString();

                int indice = CalcularProximoIndice(tag, textoSemImagem, ref numeroImagens, GetTextoSemImagem(labelTexto), listaTextos);
                
                textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);

                Boolean[,] auxiliar = null;
               // if (File.Exists(diretorioBitmap + matcher.Groups[1].Value + Util.Util.ARQUIVO_EXT_BMP))
                    auxiliar= BitmapToMatrix(tag/*, binaryThresholdTexto*/); // Ficou decidido em reunião ficar fixo o smooth level


                if (null != auxiliar)
                {
                    listaImagensBitmaps.Add(auxiliar);
                    //if (indice > listaImagens.Count)
                    //{
                    //    listaImagens.Insert(listaImagens.Count, auxiliar);
                    //}
                    //else
                    //{
                    //    listaImagens.Insert(indice, auxiliar);
                    //    numeroImagens++;
                    //}

                }

                while (matcher.Success)
                {
                    matcher = pattern.Match(textoSemImagem);
                    if (matcher.Success)
                    {
                        tag = matcher.Value.ToString();

                        indice = CalcularProximoIndice(tag, textoSemImagem, ref numeroImagens, GetTextoSemImagem(labelTexto), listaTextos);
                        textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                        
                        auxiliar = BitmapToMatrix(tag/*, binaryThresholdTexto*/); // Ficou decidido em reunião ficar fixo o smooth level
                        if (null != auxiliar)
                        {
                            listaImagensBitmaps.Add(auxiliar);
                            //if (indice > listaImagens.Count)
                            //{
                            //    listaImagens.Insert(listaImagens.Count, auxiliar);
                            //}
                            //else
                            //{
                            //    listaImagens.Insert(indice, auxiliar);
                            //    numeroImagens++;
                            //}
                        }
                    }
                }
            }

            matcher = pattern.Match(labelTexto);            
            textoSemImagem = labelTexto;
            numeroImagens = 0;
            numeroTextos = 0;

            List<Boolean[,]> listaImagens = new List<Boolean[,]>();

            if (listaTextos.Count > 0)
            {
                if (String.IsNullOrEmpty(listaTextos[0]))
                {
                    listaTextos.RemoveAt(0);
                }
                if (listaTextos.Count > 0)
                {
                    if (textoSemImagem.StartsWith(listaTextos[0]))
                    {
                        listaImagens.Add(listaImagensTextos[0]);
                        listaImagensTextos.RemoveAt(0);
                        textoSemImagem = Util.Util.SubstituirString(textoSemImagem, listaTextos[0], String.Empty, 1);
                        listaTextos.RemoveAt(0);
                    }
                }
            }
            if (matcher.Success)
            {
                string tag = matcher.Value.ToString();

                if (textoSemImagem.StartsWith(tag))
                {
                    listaImagens.Add(listaImagensBitmaps[0]);
                    listaImagensBitmaps.RemoveAt(0);
                    textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                }
                else
                {
                    listaImagens.Add(listaImagensTextos[0]);
                    listaImagensTextos.RemoveAt(0);
                    textoSemImagem = Util.Util.SubstituirString(textoSemImagem, listaTextos[0], String.Empty, 1);
                    listaTextos.RemoveAt(0);
                }
                
                while (matcher.Success)
                {
                    matcher = pattern.Match(textoSemImagem);
                    if (matcher.Success)
                    {
                        tag = matcher.Value.ToString();

                        if (textoSemImagem.StartsWith(tag))
                        {
                            listaImagens.Add(listaImagensBitmaps[0]);
                            listaImagensBitmaps.RemoveAt(0);
                            textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                        }
                        else
                        {
                            if (listaImagensTextos.Count > 0)
                            {
                                listaImagens.Add(listaImagensTextos[0]);
                                listaImagensTextos.RemoveAt(0);
                                
                            }
                            textoSemImagem = Util.Util.SubstituirString(textoSemImagem, listaTextos[0], String.Empty, 1);
                            listaTextos.RemoveAt(0);
                        }
                    }
                }
            }
            if (listaTextos.Count > 0)
            {
                if (String.IsNullOrEmpty(listaTextos[0]))
                {
                    listaTextos.RemoveAt(0);
                }
                if (listaTextos.Count > 0)
                {
                    if (textoSemImagem.StartsWith(listaTextos[0]))
                    {
                        listaImagens.Add(listaImagensTextos[0]);
                        listaImagensTextos.RemoveAt(0);
                        textoSemImagem = Util.Util.SubstituirString(textoSemImagem, listaTextos[0], String.Empty, 1);
                        listaTextos.RemoveAt(0);
                    }
                }
            }


            int larguraLocal = 0;

            foreach (bool[,] b in listaImagens)
            {
                larguraLocal += b.GetLength(1);
                alturaLocal = Math.Max(alturaLocal, b.GetLength(0));
                //if ((b.GetLength(0) > 1) && (b.GetLength(1) > 1))
                //{
                //    AplicaAlinhamentoVertical(av, b);
                //}
            }
            // Aplicando alinhamento nos objs
            for (int i = 0; i < listaImagens.Count; i++)
            {
                //if ()
                listaImagens[i] = ResizeArray(listaImagens[i], alturaLocal, listaImagens[i].GetLength(1));
                if (listaImagens[i].GetLength(1) > 1)
                {
                    

                    if (labelTexto.StartsWith(" "))
                    {
                        countEsquerda = labelTexto.Length - labelTexto.TrimStart().Length;
                    }
                    if (labelTexto.EndsWith(" "))
                    {
                        countDireita = labelTexto.Length - labelTexto.TrimEnd().Length;
                    }
                    
                    countEsquerda *= 3;
                    countDireita *= 3;

                    listaImagens[i] = AplicarAlinhamentoVertical(av, listaImagens[i]);
                    //listaImagens[i] = AplicarAlinhamentoHorizontal(ah, listaImagens[i], countEsquerda, countDireita);
                }
            }
            texto_temp = new bool[alturaLocal, larguraLocal + countEsquerda + countDireita];

            //DesenhaNoPainel(listaImagens, texto_temp, av, 0, 0);

            DesenhaNoPainel(listaImagens, texto_temp, 0, countEsquerda);

            //texto_temp = AplicarAlinhamentoHorizontal(ah, texto_temp, countEsquerda, countDireita);

            return texto_temp;
        }

        //TODO: Verificar se vai impactar na geração dos outros textos com fonte truetype
        public static Boolean[,] AplicarAlinhamento(Util.Util.AlinhamentoHorizontal AlinhamentoHorizontal, Util.Util.AlinhamentoVertical AlinhamentoVertical, string LabelTexto, bool isFonteWindows, string caminhoFonte, Boolean[,] Matrix)
        {
            int countEsquerda = 0;
            int countDireita = 0;
            int tamanhoEspaco = 3;

            // Para Fonte TrueType e alinhamento a esquerda não é necessário aplicar o alinhamento, pois dá problemas na geração da Matriz.
            if ((isFonteWindows) && (AlinhamentoHorizontal == Util.Util.AlinhamentoHorizontal.Esquerda))
            {
                Matrix = AplicarAlinhamentoVertical(AlinhamentoVertical, Matrix);
                return Matrix;
            }

            bool existePixels = false;
            for (int largura = 0; largura < Matrix.GetLength(1); largura++)
            {
                for (int altura = 0; altura < Matrix.GetLength(0); altura++)
                {
                    existePixels = Matrix[altura, largura];
                    if (existePixels)
                    {
                        break;
                    }
                }
                if (existePixels)
                {
                    break;
                }
            }
            if (LabelTexto.StartsWith(" "))
            {
                countEsquerda = LabelTexto.Length - LabelTexto.TrimStart().Length;
            }
            if (LabelTexto.EndsWith(" "))
            {
                countDireita = LabelTexto.Length - LabelTexto.TrimEnd().Length;
            }
            if (String.IsNullOrEmpty(LabelTexto.Trim()))
            {
                countEsquerda = 0;
                countDireita = 0;
            }
            if (!isFonteWindows)
            {
                Arquivo_FNT afnt = new Arquivo_FNT();
                afnt.Abrir(caminhoFonte);
                // Verificar a quantidade de colunas que o espaço dessa fonte ocupa.
                tamanhoEspaco = afnt.bitmaps[0].matriz.GetLength(1);
            }
            countEsquerda *= tamanhoEspaco;
            countDireita *= tamanhoEspaco;

            if (existePixels)
            {
                Matrix = AplicarAlinhamentoHorizontal(AlinhamentoHorizontal, Matrix, countEsquerda, countDireita);
                Matrix = AplicarAlinhamentoVertical(AlinhamentoVertical, Matrix);
            }

            return Matrix;
        }
        private static int CalcularProximoIndice(string tag, string labelTexto, ref int numeroImagens, string textoSemTags, List<string> listaTextos)
        {
          int indice = labelTexto.IndexOf(tag);

            if ((indice > 0) && (indice < textoSemTags.Length))
            {
                int acumulado = 0;
                for (int i = 0; i < listaTextos.Count; i++)
                {
                    acumulado = listaTextos[i].Length;

                    if (indice < acumulado)
                    {
                        acumulado = i;
                        break;
                    }
                }                
                return (numeroImagens + acumulado - 1);
            }
            //indice = (indice - listaTextos[numeroTextos].Length) + numeroImagens;


            return indice;
        }

        /// Funcionando, mas está muito feio.
        public static Boolean[,] StrToMatrixTrueType2(String labelTexto, String fonteTexto, int alturaTexto, bool italicoTexto, bool negritoTexto, bool sublinhadoTexto, int binaryThresholdTexto, Util.Util.AlinhamentoVertical av)
        {
            List<Boolean[,]> listaImagens = new List<Boolean[,]>();

            //Testa o padrão
            Match matcher = pattern.Match(labelTexto);

            List<string> listaTextos = GetTextosSemImagem(labelTexto);
            List<int> listaPosicoesImagens = GetPosicoesImagens(labelTexto, matcher, listaTextos);
            String textoSemImagem = (listaTextos.Count > 0) ? listaTextos[0] : String.Empty;
            int alturaLocal = alturaTexto;
            Boolean[,] texto_temp;

            for (int indiceTexto = 0; indiceTexto < listaTextos.Count; indiceTexto++)
            {
                textoSemImagem = listaTextos[indiceTexto];
                if (String.IsNullOrEmpty(textoSemImagem))
                {
                    continue;
                }

                System.Drawing.Bitmap bTemp = new System.Drawing.Bitmap(1, 1);
                Boolean[,] Result;
                Color textColor = Color.FromArgb(0, 0, 0); //cor do texto
                Color backColor = Color.FromArgb(255, 255, 255); //cor da fonte
                FontStyle fsTexto = new FontStyle();

                fsTexto = FontStyle.Regular;

                InstalledFontCollection ifc = new InstalledFontCollection();
                for (int i = 0; i < ifc.Families.Length; i++)
                {
                    if (ifc.Families[i].Name == fonteTexto)
                    {
                        if (ifc.Families[i].IsStyleAvailable(FontStyle.Regular))
                        {
                            fsTexto = FontStyle.Regular;
                        }
                        else if (ifc.Families[i].IsStyleAvailable(FontStyle.Bold))
                        {
                            fsTexto = FontStyle.Bold;
                        }
                        else if (ifc.Families[i].IsStyleAvailable(FontStyle.Italic))
                        {
                            fsTexto = FontStyle.Italic;
                        }
                        else if (ifc.Families[i].IsStyleAvailable(FontStyle.Underline))
                        {
                            fsTexto = FontStyle.Underline;
                        }
                        break;
                    }
                }

                if (italicoTexto)
                    fsTexto = fsTexto | FontStyle.Italic;

                if (negritoTexto)
                    fsTexto = fsTexto | FontStyle.Bold;

                if (sublinhadoTexto)
                    fsTexto = fsTexto | FontStyle.Underline;



                //converte o bitmap diretamente usando o método estático
                bTemp = TextImage.MakeTextBitmap(textoSemImagem, new Font(fonteTexto, alturaTexto, fsTexto), textColor, backColor, binaryThresholdTexto);
                Result = new Boolean[bTemp.Height, bTemp.Width];

                //for (int altura = 0; altura < bTemp.Height; altura++)            
                for (int altura = bTemp.Height - 1; altura >= 0; altura--)
                {
                    for (int largura = 0; largura < bTemp.Width; largura++)
                    {
                        //if (bTemp.GetPixel(largura, altura) == Color.White)
                        if (VerificaBranco(bTemp.GetPixel(largura, bTemp.Height - altura - 1)))
                            Result[altura, largura] = false;
                        else
                            Result[altura, largura] = true;
                    }
                }

                if (Result.LongLength == 1)
                {
                    Result[0, 0] = false;
                }

                //temporário para caso se precise fazer alguma operação intermediária.
                texto_temp = new bool[bTemp.Height, bTemp.Width];

                //desenha no temporário.
                DrawMatrix(texto_temp, Result, 0, 0);

                listaImagens.Add(texto_temp);
            }
            matcher = pattern.Match(labelTexto);
            textoSemImagem = labelTexto;
            int numeroImagens = 0;

            if (matcher.Success)
            {
                string tag = matcher.Value.ToString();
                int indice = labelTexto.IndexOf(tag);

                //indice = listaPosicoesImagens[numeroImagens];

                textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);

                Boolean[,] auxiliar = BitmapToMatrix(tag, binaryThresholdTexto);

                if (null != auxiliar)
                {
                    if (indice + numeroImagens > listaImagens.Count)
                    {
                        listaImagens.Insert(listaImagens.Count, auxiliar);
                    }
                    else
                    {
                        listaImagens.Insert(indice + numeroImagens, auxiliar);
                        numeroImagens++;
                    }

                }

                while (matcher.Success)
                {
                    matcher = pattern.Match(textoSemImagem);
                    if (matcher.Success)
                    {
                        tag = matcher.Value.ToString();
                        indice = textoSemImagem.IndexOf(tag);
                        // indice = listaPosicoesImagens[numeroImagens];
                        textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                        auxiliar = BitmapToMatrix(tag, binaryThresholdTexto);

                        if (null != auxiliar)
                        {
                            if (indice + numeroImagens > listaImagens.Count)
                            {
                                listaImagens.Insert(listaImagens.Count, auxiliar);
                            }
                            else
                            {
                                listaImagens.Insert(indice + numeroImagens, auxiliar);
                                numeroImagens++;
                            }
                        }
                    }
                }
            }
            int larguraLocal = 0;

            foreach (bool[,] b in listaImagens)
            {
                larguraLocal += b.GetLength(1);
                alturaLocal = Math.Max(alturaLocal, b.GetLength(0));
                //if ((b.GetLength(0) > 1) && (b.GetLength(1) > 1))
                //{
                //    AplicaAlinhamentoVertical(av, b);
                //}
            }
            //// Aplicando alinhamento nos objs
            //for (int i = 0; i < listaImagens.Count; i++)
            //{
            //    //if ()
            //    listaImagens[i] = ResizeArray(listaImagens[i], alturaLocal, listaImagens[i].GetLength(1));
            //    if (listaImagens[i].GetLength(1) > 1)
            //    {
            //        listaImagens[i] = AplicaAlinhamentoVertical(av, listaImagens[i]);
            //    }
            //}
            texto_temp = new bool[alturaLocal, larguraLocal];

            //DesenhaNoPainel(listaImagens, texto_temp, av, 0, 0);

            DesenhaNoPainel(listaImagens, texto_temp, 0, 0);

            return texto_temp;
        }

        private static List<int> GetPosicoesImagens(string labelTexto, Match matcher, List<string> listaTextos)
        {
            List<int> lista = new List<int>();
            String textoSemImagem = labelTexto;
            List<int> listaValores = new List<int>();

            int acumulado = 0;
            for (int i = 0; i < listaTextos.Count; i++)
            {
                acumulado += listaTextos[i].Length;
                listaValores.Add(acumulado);
            }

            if (matcher.Success)
            {
                string tag = matcher.Value.ToString();
                int indice = labelTexto.IndexOf(tag);

                //if (indice >= listaValores[0])
                //    indice = 1;


                lista.Add(indice);

                textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                                 

                while (matcher.Success)
                {
                    matcher = pattern.Match(textoSemImagem);
                    if (matcher.Success)
                    {
                        tag = matcher.Value.ToString();
                        indice = textoSemImagem.IndexOf(tag);
                        //if (indice >= listaValores[0])
                        //    indice++;
                        lista.Add(indice);
                        textoSemImagem = Util.Util.SubstituirString(textoSemImagem, tag, String.Empty, 1);
                    }
                }
            }
            int indiceListaTexto = 0;
            int contadorStrings = 0;

            if (lista.Count > 0)
            {
                lista.Clear();
                for (int i = 0; i < listaTextos.Count; i++)
                {
                    if (String.IsNullOrEmpty(listaTextos[i]))
                    {
                        lista.Add(i);
                    }
                    else if ((i>0)&&((!String.IsNullOrEmpty(listaTextos[i])) && (!String.IsNullOrEmpty(listaTextos[i - 1]))))
                    {
                        lista.Add(i);
                    }


                   if (lista.Count == indiceListaTexto)
                        break;
                }
            }
            
            return lista;
        }

        public static Boolean[,] BitmapToMatrix(String tag, int binarize = 200)
        {
            System.Drawing.Bitmap bTemp;
            Boolean[,] Result;
            Color textColor = Color.FromArgb(0, 0, 0); //cor do texto
            Color backColor = Color.FromArgb(255, 255, 255); //cor da fonte                        
            
            //Testa o padrão
            Match matcher = pattern.Match(tag);

            if (matcher.Success)
            {
                String nomeArquivo = tag.Substring(3, tag.Length-6) + Util.Util.ARQUIVO_EXT_BMP;

                if (!File.Exists(diretorioBitmap + nomeArquivo))
                {

                    //converte o bitmap diretamente usando o método estático
                    //bTemp = TextImage.MakeTextBitmap(" ", new Font("Arial", 16, FontStyle.Regular), textColor, backColor, 200);
                    //Result = new Boolean[bTemp.Height, bTemp.Width];
                    //Result = new Boolean[1,1];

                    ////for (int altura = 0; altura < bTemp.Height; altura++)            
                    //for (int altura = bTemp.Height - 1; altura >= 0; altura--)
                    //{
                    //    for (int largura = 0; largura < bTemp.Width; largura++)
                    //    {
                    //        //if (bTemp.GetPixel(largura, altura) == Color.White)
                    //        if (VerificaBranco(bTemp.GetPixel(largura, bTemp.Height - altura - 1)))
                    //            Result[altura, largura] = false;
                    //        else
                    //            Result[altura, largura] = true;
                    //    }
                    //}

                    //if (Result.LongLength == 1)
                    //{
                    //    Result[0, 0] = false;
                    //}

                    ////temporário para caso se precise fazer alguma operação intermediária.
                    //Boolean[,] espaco = new bool[bTemp.Height, bTemp.Width];

                    ////desenha no temporário.
                    //DrawMatrix(espaco, Result, 0, 0);
                    return new Boolean[1, 1];
                }

                //converte o bitmap diretamente usando o método estático
                bTemp = new System.Drawing.Bitmap(diretorioBitmap + nomeArquivo);
                bTemp = TextImage.Binarize(bTemp, binarize);

                Result = new Boolean[bTemp.Height, bTemp.Width];

                //for (int altura = 0; altura < bTemp.Height; altura++)            
                for (int altura = bTemp.Height - 1; altura >= 0; altura--)
                {
                    for (int largura = 0; largura < bTemp.Width; largura++)
                    {
                        //if (bTemp.GetPixel(largura, altura) == Color.White)
                        if (VerificaBranco(bTemp.GetPixel(largura, bTemp.Height - altura - 1)))
                            Result[altura, largura] = false;
                        else
                            Result[altura, largura] = true;
                    }
                }

                if (Result.LongLength == 1)
                {
                    Result[0, 0] = false;
                }

                //temporário para caso se precise fazer alguma operação intermediária.
                Boolean[,] texto_temp = new bool[bTemp.Height, bTemp.Width];

                //desenha no temporário.
                DrawMatrix(texto_temp, Result, 0, 0);
                return texto_temp;
            }

            return null;
        }

        private static Boolean[,] AplicarAlinhamento(Util.Util.AlinhamentoHorizontal AlinhamentoHorizontal,
                                             Util.Util.AlinhamentoVertical AlinhamentoVertical,
                                             Boolean[,] Matrix)
        {
            bool existePixels = false;
            for (int largura = 0; largura < Matrix.GetLength(1); largura++)
            {
                for (int altura = 0; altura < Matrix.GetLength(0); altura++)
                {
                    existePixels = Matrix[altura, largura];
                    if (existePixels)
                    {
                        break;
                    }
                }
                if (existePixels)
                {
                    break;
                }
            }


            if (existePixels)
            {
                //Matrix = AplicarAlinhamentoHorizontal(AlinhamentoHorizontal, Matrix);
                Matrix = AplicarAlinhamentoVertical(AlinhamentoVertical, Matrix);
            }

            return Matrix;
        }

        public static Boolean[,] AplicarAlinhamentoHorizontal(Util.Util.AlinhamentoHorizontal AlinhamentoH, Boolean[,] Matrix, int espacosEsquerda, int espacosDireita)
        {
            if ((Matrix.GetLength(1) == espacosEsquerda) && (Matrix.GetLength(1) == espacosDireita) && (espacosDireita == espacosEsquerda))
                return Matrix;

            //if (espacosDireita == espacosEsquerda)
            //{
            //    espacosDireita
            //}

            switch (AlinhamentoH)
            {
                case Util.Util.AlinhamentoHorizontal.Centralizado:
                    CentralizarHorizontalArray(Matrix, espacosEsquerda, espacosDireita);
                    break;

                case Util.Util.AlinhamentoHorizontal.Direita:
                    AlinharDireitaArray(Matrix, espacosDireita);
                    break;

                case Util.Util.AlinhamentoHorizontal.Esquerda:
                    AlinharEsquerdaArray(Matrix, espacosEsquerda);
                    break;
            }

            return Matrix;
        }
        private static void AlinharEsquerdaArray(Boolean[,] Matrix, int espacosEsquerda)
        {
            int inicio_desenho = ColunasVaziasAEsquerdaArray(Matrix);

            inicio_desenho = inicio_desenho - espacosEsquerda;

            int fim_desenho = ColunasVaziasADireitaArray(Matrix);
            int inicio_matrix = 0;

            if (inicio_desenho + fim_desenho == 0)
                return;

            for (int colunas = inicio_desenho; colunas <= fim_desenho - inicio_desenho; colunas++)
            {
                CopiarColuna(Matrix, colunas, inicio_matrix);
                inicio_matrix = inicio_matrix + 1;
            }

            for (int i = fim_desenho - inicio_desenho; i < Matrix.GetLength(1); i++)
                ApagarColuna(Matrix, i);

        }
        private static void AlinharDireitaArray(Boolean[,] Matrix, int espacosDireita)
        {
            int indice_ultima_coluna = ColunasVaziasADireitaArray(Matrix);
            indice_ultima_coluna = indice_ultima_coluna + espacosDireita;

            int colunas_alvo = Matrix.GetLength(1) - 1;

            if (indice_ultima_coluna < Matrix.GetLength(1) - 1)
            {
                for (int colunas = indice_ultima_coluna; colunas >= 0; colunas--)
                {
                    CopiarColuna(Matrix, colunas, colunas_alvo);
                    ApagarColuna(Matrix, colunas);
                    colunas_alvo = colunas_alvo - 1;
                }
            }
        }

        private static void CentralizarHorizontalArray(Boolean[,] Matrix, int espacosEsquerda, int espacosDireita)
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
        private static Boolean[,] AplicarAlinhamentoVertical(Util.Util.AlinhamentoVertical AlinhamentoV, Boolean[,] Matrix)
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
 
        /// <summary>
        /// Apaga uma linha, gravando valores false.
        /// </summary>
        /// <param name="Matrix">Matriz a ser alterada.</param>
        /// <param name="indice_linha">índice da linha.</param>
        private static void ApagarLinha(Boolean[,] Matrix, int indice_linha)
        {
            for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
                Matrix[indice_linha, coluna] = false;
        }
        private static void CopiarLinha(Boolean[,] Matrix, int indice_fonte, int indice_alvo)
        {
            //if (indice_alvo < 0)
            //    return;
            //if (indice_fonte < 0)
            //    return;
            if (indice_fonte < Matrix.GetLength(0))
            {
                for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
                    Matrix[indice_alvo, coluna] = Matrix[indice_fonte, coluna];
            }
        }       
        /// <summary>
        /// Apaga uma coluna, gravando valores false.
        /// </summary>
        /// <param name="Matrix">Matriz a ser alterada.</param>
        /// <param name="indice_coluna"></param>
        private static void ApagarColuna(Boolean[,] Matrix, int indice_coluna)
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
        private static void CopiarColuna(Boolean[,] Matrix, int indice_fonte, int indice_alvo)
        {
            //if (indice_alvo < 0)
            //    return;
            //if (indice_fonte < 0)
            //    return;
            if (indice_fonte < Matrix.GetLength(1))
            {
                for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                    Matrix[linha, indice_alvo] = Matrix[linha, indice_fonte];
            }
        }
        // RotateMatrix
        public static bool[,] RotateMatrix(bool[,] matrix, int n)
        {
            bool[,] ret = new bool[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    ret[i, j] = matrix[n - j - 1, i];
                }
            }

            return ret;
        }
        // RESIZE
        public static bool[,] ResizeArray(bool[,] original, int rows, int cols)
        {
            var newArray = new bool[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
        }
        /// <summary>
        /// Retorna um arquivo de fonte a partir do nome do arquivo sem a extensão;
        /// </summary>
        /// <param name="nome_arquivo_fonte">String contendo o nome do arquivo sem a extensão ".fnt"</param>
        /// <returns>Objeto Arquivo_FNT</returns>
        public static Arquivo_FNT RetornaFonte(String nome_arquivo_fonte)
        {
            Arquivo_FNT afnt = new Arquivo_FNT();
            // TODO: retirar esse IF abaixo assim que as frases estejam corrijidas / Ou abrir/salvar arquivo.
            if (nome_arquivo_fonte.EndsWith("\\.fnt"))
            {
                nome_arquivo_fonte = nome_arquivo_fonte.Replace("\\.fnt", "\\FRT 05x08 Bold x.fnt");
                
            }
            //afnt.Abrir(DIRETORIO_RAIZ + DIRETORIO_FONTES + @"\" + nome_arquivo_fonte + Util.Util.ARQUIVO_EXT_FNT);
            afnt.Abrir(nome_arquivo_fonte);

            return afnt;
        }


        //para facilitar a chamada à draw matrix.
        private static void DesenhaNoPainel(List<Boolean[,]> conteudo, Boolean[,] Matrix, int origem_abs = 0, int origem_ord = 0)
        {
            int origem_proximo = 0;

            //Matrix[0, 0] = true;

            foreach (Boolean[,] caractere in conteudo)
            {
                if (origem_proximo == 0)
                    DrawMatrix(Matrix, caractere, origem_abs, origem_ord);
                else
                    //DrawMatrix(Matrix, caractere, origem_ord + origem_proximo, origem_abs);
                    DrawMatrix(Matrix, caractere, origem_abs, origem_ord + origem_proximo);

                origem_proximo = origem_proximo + caractere.GetLength(1);
            }
        }
        //para facilitar a chamada à draw matrix.
        private static void DesenhaNoPainel(List<Boolean[,]> conteudo, Boolean[,] Matrix, Util.Util.AlinhamentoVertical av, int origem_abs = 0, int origem_ord = 0)
        {
            int origem_proximo = 0;

            //Matrix[0, 0] = true;

            foreach (Boolean[,] caractere in conteudo)
            {
                if (origem_proximo == 0)
                    DrawMatrix(Matrix, caractere, origem_abs, origem_ord);
                else
                    //DrawMatrix(Matrix, caractere, origem_ord + origem_proximo, origem_abs);
                    DrawMatrix(Matrix, caractere, origem_abs, origem_ord + origem_proximo);

                AplicarAlinhamentoVertical(av, Matrix);
                origem_proximo = origem_proximo + caractere.GetLength(1);
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
        private static void DrawMatrix(Boolean[,] Matrix, Boolean[,] Desenho, int origem_linha, int origem_coluna)
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

        /// <summary>
        /// Gera uma matriz de um array apenas.
        /// </summary>
        /// <param name="matriz">Matriz a ser redimensionada.</param>
        /// <returns>Array de valores booleanos.</returns>
        private static Boolean[] UnidimensionaMatriz(Boolean[,] matriz)
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


        /// <summary>
        /// Usada após processamento da imagem, para traduzir os bytes para hardware.
        /// </summary>
        private static void EstufaBits(ref Persistencia.Videos.VideoV02 v02,
                                       Boolean[,] Painel)
        {
            
            long nelementos = 0;
            Boolean[] painelEmArray = new bool[nelementos];

            nelementos = Painel.GetLength(0) * Painel.GetLength(1);
            painelEmArray = UnidimensionaMatriz(Painel);

            //copia todos os elementos para pixelbytes
            v02.pixelBytes = new byte[nelementos / 8];
            int indice_bmp = 0;
            for (int i = 0; i < painelEmArray.Length; i++)
            {
                byte aux = (byte)(System.Convert.ToByte(painelEmArray[i]) << 0);
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


                /*for (int e = 0; e < pbytesTemp[i].Count(); e++)
                {
                    byte aux = (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 0);
                    e++;
                    aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 1);
                    e++;
                    aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 2);
                    e++;
                    aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 3);
                    e++;
                    aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 4);
                    e++;
                    aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 5);
                    e++;
                    aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 6);
                    e++;
                    aux += (byte)(System.Convert.ToByte(pbytesTemp[i][e]) << 7);

                    v02.pixelBytes[indice_bmp] = aux;

                    indice_bmp++;
                }
                 */
            }

        }

        
        #region FUNÇÕES_DE_ALINHAMENTO
              

        private static void AlinhamentoEsquerdaArray(Boolean[,] Matrix)
        {
            int inicio_desenho = ColunasVaziasAEsquerdaArray(Matrix);
            int fim_desenho = ColunasVaziasADireitaArray(Matrix);
            int inicio_matrix = 0;

            for (int colunas = inicio_desenho; colunas <= fim_desenho - inicio_desenho; colunas++)
            {
                CopiaColuna(Matrix, colunas, inicio_matrix);
                //ApagaColuna(Matrix, colunas);
                inicio_matrix = inicio_matrix + 1;
            }

            for (int i = fim_desenho - inicio_desenho; i < Matrix.GetLength(1); i++)
                ApagaColuna(Matrix, i);

        }

        private static void AlinhamentoDireitaArray(Boolean[,] Matrix)
        {
            int indice_ultima_coluna = ColunasVaziasADireitaArray(Matrix);
            int colunas_alvo = Matrix.GetLength(1) - 1;

            for (int colunas = indice_ultima_coluna; colunas >= 0; colunas--)
            {
                CopiaColuna(Matrix, colunas, colunas_alvo);
                ApagaColuna(Matrix, colunas);
                colunas_alvo = colunas_alvo - 1;
            }
        }

        private static void CentralizarVerticalArray(Boolean[,] Matrix)
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
                    //if ((metade < 0) || (indice_desenho == Matrix.GetLength(0))) break;
                    CopiaLinha(Matrix, indice_desenho, metade);
                    metade = metade + 1;
                    indice_desenho = indice_desenho + 1;
                }


                for (int i = altura_desenho + origem_linha; i < Matrix.GetLength(0); i++)
                {
                    ApagaLinha(Matrix, i);
                }

                for (int i = origem_linha - 1; i >= 0; i--)
                {
                    ApagaLinha(Matrix, i);
                }

            }

            if (indice_superior < Matrix.GetLength(0) - indice_inferior - 1)
            {
                int origem_linha = (Matrix.GetLength(0) / 2) + (altura_desenho / 2) - 1;

                int indice_desenho = indice_inferior;
                int metade = origem_linha;
                for (int linha = 0; linha < altura_desenho; linha++)
                {
                    CopiaLinha(Matrix, indice_desenho, metade);
                    metade = metade - 1;
                    indice_desenho = indice_desenho - 1;
                }

                for (int i = origem_linha + 1; i < Matrix.GetLength(0); i++)
                {
                    ApagaLinha(Matrix, i);
                }

                for (int i = metade; i >= 0; i--)
                {
                    ApagaLinha(Matrix, i);
                }
            }
        }

        private static void AlinharAcimaArray(Boolean[,] Matrix)
        {
            int indice_superior = MaiorIndiceSuperiorArray(Matrix);
            int indice_inferior = MenorIndiceInferiorArray(Matrix);
            int indice_linha = Matrix.GetLength(0) - 1;

            //para caso não precise processar.
            if (indice_inferior == Matrix.GetLength(0) - 1) return;

            for (int linha = indice_inferior; linha >= indice_superior; linha--)
            {
                CopiaLinha(Matrix, linha, indice_linha);
                indice_linha = indice_linha - 1;
            }

            for (int i = indice_linha; i >= 0; i--)
                ApagaLinha(Matrix, i);

        }
        
        private static void AlinharAoFundoArray(Boolean[,] Matrix)
        {
            //int pontos_a_remover = deslocamento;
            int indice_superior = MaiorIndiceSuperiorArray(Matrix);
            int indice_inferior = MenorIndiceInferiorArray(Matrix);

            //para caso não precise processar.
            if (indice_superior == 0) return;

            int indice_linha = 0;
            for (int linha = indice_superior; linha <= indice_inferior; linha++)
            {
                CopiaLinha(Matrix, linha, indice_linha);
                indice_linha = indice_linha + 1;
            }

            for (int i = indice_linha; i < Matrix.GetLength(0); i++)
                ApagaLinha(Matrix, i);


        }

        #endregion FUNÇÕES_DE_ALINHAMENTO

        #region MANIPULAÇÃO_DE_MATRIZES

        /// <summary>
        /// Retorna o índice da última coluna vazia à direita.
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        private static int ColunasVaziasADireitaArray(Boolean[,] Matrix)
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
        private static int ColunasVaziasAEsquerdaArray(Boolean[,] Matrix)
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
        private static void ApagaColuna(Boolean[,] Matrix, int indice_coluna)
        {
            for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                Matrix[linha, indice_coluna] = false;
        }


        /// <summary>
        /// Copia a coluna indicada pelo indice_fonte para indice_alvo.
        /// </summary>
        /// <param name="Matrix">Matriz em arrays.</param>
        /// <param name="indice_fonte">indice da coluna a ser copiada.</param>
        /// <param name="indice_alvo">indice da coluna a ser substituída.</param>
        private static void CopiaColuna(Boolean[,] Matrix, int indice_fonte, int indice_alvo)
        {
            for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                Matrix[linha, indice_alvo] = Matrix[linha, indice_fonte];
        }

        private static void CopiaLinha(Boolean[,] Matrix, int indice_fonte, int indice_alvo)
        {
            if (indice_fonte < 0)
                return;

            if (indice_alvo < 0)
                return;

            for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
                Matrix[indice_alvo, coluna] = Matrix[indice_fonte, coluna];
        }
        

        /// <summary>
        /// Apaga uma linha, gravando valores false.
        /// </summary>
        /// <param name="Matrix">Matriz a ser alterada.</param>
        /// <param name="indice_linha">índice da linha.</param>
        private static void ApagaLinha(Boolean[,] Matrix, int indice_linha)
        {
            for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
                Matrix[indice_linha, coluna] = false;
        }
        
        private static int MenorIndiceInferiorArray(Boolean[,] Matrix)
        {
            for (int i = Matrix.GetLength(0) - 1; i >= 0; i--)
                for (int j = 0; j < Matrix.GetLength(1); j++)
                    if (Matrix[i, j] == true) return i;

            return -1;
        }

        private static int MaiorIndiceSuperiorArray(Boolean[,] Matrix)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
                for (int j = 0; j < Matrix.GetLength(1); j++)
                    if (Matrix[i, j] == true) return i;

            return -1;
        }

        #endregion MANIPULAÇÃO_DE_MATRIZES

    }
}
