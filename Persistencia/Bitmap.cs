using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public class Bitmap
    {
        #region Variáveis de Vinicius

        public const Int64 NUMERO_DE_COLUNAS = 120;
        public const Int64 ALTURA = 20;
        public const Int32 NUMERO_DE_LINHAS = 8;

        public List<List<Boolean>> Matriz = new List<List<bool>>(NUMERO_DE_LINHAS);
        public List<List<Boolean>> Draw = new List<List<bool>>(6);

        #endregion Variáveis de Vinicius

        public UInt32 altura;
        public UInt32 largura;
        public bool[,] matriz;

        public Bitmap()
        {
            altura = 16;
            largura = 144;
            matriz = new bool[altura, largura];
            for (int indiceAltura = 0; indiceAltura < altura; indiceAltura++)
            {
                for (int indiceLargura = 0; indiceLargura < largura; indiceLargura++)
                {
                    matriz[indiceAltura, indiceLargura] = false;
                }
            }
        }
        public Bitmap(UInt32 altura, UInt32 largura, bool valorInicial)
        {
            this.altura = altura;
            this.largura = largura;
            matriz = new bool[altura, largura];
            for (int indiceAltura = 0; indiceAltura < altura; indiceAltura++)
            {
                for (int indiceLargura = 0; indiceLargura < largura; indiceLargura++)
                {
                    matriz[indiceAltura, indiceLargura] = valorInicial;
                }
            }

        }

        public Bitmap(UInt32 altura, UInt32 largura)
        {
            this.altura = altura;
            this.largura = largura;
        }

        public Bitmap(Bitmap oldValue)
        {
            this.altura = oldValue.altura;
            this.largura = oldValue.largura;

            this.matriz = new bool[oldValue.matriz.GetLength(0), oldValue.matriz.GetLength(1)];
            this.matriz = (bool[,])oldValue.matriz.Clone();
        }
        private bool[] GetBool(byte item)
        {
            bool[] retorno = new bool[8];

            retorno[0] = Convert.ToBoolean(item & 0x01);
            retorno[1] = Convert.ToBoolean(item & 0x02);
            retorno[2] = Convert.ToBoolean(item & 0x04);
            retorno[3] = Convert.ToBoolean(item & 0x08);
            retorno[4] = Convert.ToBoolean(item & 0x10);
            retorno[5] = Convert.ToBoolean(item & 0x20);
            retorno[6] = Convert.ToBoolean(item & 0x40);
            retorno[7] = Convert.ToBoolean(item & 0x80);

            return retorno;
        }
        // É melhor ter este esforço no abrir e salvar arquivo
        public void FromArrayToMatrix(byte[] pixelBytes, int tamanho = 0)
        {
            // List<bool> c = new List<bool>();
            //BitArray b = new BitArray(pixelBytes);
            bool[] c = new bool[pixelBytes.Length * 8];


            int indiceArray = 0;
            //for (int i = 0; i < b.Length; i++)
            //{
            //    c.Add(b.Get(i));
            //}
            for (int i = 0; i < pixelBytes.Length; i++)
            {
                bool[] cPixelByte = GetBool(pixelBytes[i]);
                Array.Copy(cPixelByte, 0, c, (8 * i), 8);
                //c.AddRange(GetBool(pixelBytes[i]));
            }
            if (tamanho != 0)
            {
                Array.Resize(ref c, tamanho);
            }
            //this.matriz = new bool[altura, largura];
            //largura = (uint)(b.Length / altura);
            largura = (uint)(c.Length / altura);
            if (largura == 0) largura = 1;

            //largura = 8;
            this.matriz = new bool[altura, largura];

            for (int indiceLargura = 0; indiceLargura < largura; indiceLargura++)
            {
                for (int indiceAltura = 0; indiceAltura < altura; indiceAltura++)
                {
                    matriz[indiceAltura, indiceLargura] = c[indiceArray];
                    indiceArray++;

                    //if (indiceArray == c.Count) return;
                    if (indiceArray == c.Length) return;
                }
            }
        }

        

        public Byte[] FromMatrixToArray()
        {
            //List<Byte> teste = new List<byte>();
            //int indiceArray = 0;
            //byte b = 0;
            //for (int indiceLargura = 0; indiceLargura < largura; indiceLargura++)
            //{
            //    for (int indiceAltura = 0; indiceAltura < altura; indiceAltura++)
            //    {
            //        b += (byte)(Convert.ToInt32(matriz[indiceAltura, indiceLargura]) << (indiceArray % sizeof(byte)));

            //        indiceArray++;
            //        if (indiceArray % 8 == 0)
            //        {
            //            teste.Add(b);
            //            b = 0;
            //        }
            //    }
            //}

            long nelementos = 0;

            nelementos = matriz.GetLength(0) * matriz.GetLength(1);
            Boolean[] painelEmArray = new bool[nelementos];

            int indice_result = 0;
            for (int coluna = 0; coluna < matriz.GetLength(1); coluna++)
            {
                for (int linha = 0; linha < matriz.GetLength(0); linha++)
                {
                    painelEmArray[indice_result] = matriz[linha, coluna];
                    indice_result = indice_result + 1;
                }
            }


            //copia todos os elementos para pixelbytes            
            Byte[] teste = new byte[(nelementos % 8 == 0) ? (nelementos / 8) : (nelementos / 8) + 1];
            int indice_bmp = 0;
            byte aux = 0;
            for (int i = 0; i < painelEmArray.Length; i++)
            {
                try
                {
                    if (i < painelEmArray.Length)
                    {
                        aux = (byte)(System.Convert.ToByte(painelEmArray[i]) << 0);
                        i++;
                    }
                    if (i < painelEmArray.Length)
                    {
                        aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 1);
                        i++;
                    }
                    if (i < painelEmArray.Length)
                    {
                        aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 2);
                        i++;
                    }
                    if (i < painelEmArray.Length)
                    {
                        aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 3);
                        i++;
                    }
                    if (i < painelEmArray.Length)
                    {
                        aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 4);
                        i++;
                    }
                    if (i < painelEmArray.Length)
                    {
                        aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 5);
                        i++;
                    }
                    if (i < painelEmArray.Length)
                    {
                        aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 6);
                        i++;
                    }
                    if (i < painelEmArray.Length)
                    {
                        aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 7);
                    }
                    if (i >= painelEmArray.Length)
                    {
                        teste[indice_bmp] = aux;
                        indice_bmp++;
                        continue;
                    }
                    else
                    {
                        teste[indice_bmp] = aux;
                        indice_bmp++;
                    }
                    //aux = (byte)(System.Convert.ToByte(painelEmArray[i]) << 0);
                    //i++;
                    //aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 1);
                    //i++;
                    //aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 2);
                    //i++;
                    //aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 3);
                    //i++;
                    //aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 4);
                    //i++;
                    //aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 5);
                    //i++;
                    //aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 6);
                    //i++;
                    //aux += (byte)(System.Convert.ToByte(painelEmArray[i]) << 7);

                    //teste[indice_bmp] = aux;
                    //indice_bmp++;
                }
                catch
                {
                    teste[indice_bmp] = aux;
                    indice_bmp++;
                    continue;
                }
            }
            return teste;
        }
         /// <summary>
        /// Gera colunas vazias para a matriz, de acordo com o valor especificado em NUMERO_DE_LINHAS.
        /// </summary>
        private void InicializaColunas()
        {
            for(Int32 i = 0; i < NUMERO_DE_COLUNAS; i++)
            {
                Matriz.Add(new List<bool>());

                for(int j = 0; j < ALTURA; j++)
                {
                    Matriz[i].Add(new bool());
                }
            }
        }

        /// <summary>
        /// Inicia a Matriz como false ou como true.
        /// </summary>
        /// <param name="valor">valor booleano para inicializar todos valores com o parâmetro valor.</param>
        private void ResetMatriz(Boolean valor)
        {
            foreach (List<Boolean> lb in Matriz)
            {
                for (int i = 0; i < lb.Count; i++)
                    lb[i] = valor;
            }
        }


        public void InicializaMatriz(List<List<Boolean>> Matriz, int linhas, int colunas)
        {
            for(int i = 0; i < linhas; i++)
            {
                Matriz.Add(new List<bool>());

                for(int j = 0; j < colunas; j++)
                {
                    Matriz[i].Add(new bool());
                }
            }
        }

        
        public static String TrataDiretorio(String diretorio)
        {
            try
            {
                Int32 indice_limite = diretorio.IndexOfAny("\0".ToCharArray());

                if (indice_limite != -1)
                {
                    string temp = diretorio.Substring(0, indice_limite);

                    return temp.Replace(@"/", @"\");
                }
                else
                {
                    return diretorio;
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        /// <summary>
        /// Lê de desenho e grava em Matriz.
        /// (O desenho sofrerá crop de acordo com as dimensões de matriz)
        /// </summary>
        /// <param name="Matriz">Painel Pontos.</param>
        /// <param name="Desenho">Letra da fonte lighdot.</param>
        /// <param name="origem_linha">x - abscissa do início do desenho.</param>
        /// <param name="origem_coluna">y - ordenada do início do desenho.</param>
        private void DrawMatriz(List<List<Boolean>> Matriz, List<List<Boolean>> Desenho, int origem_linha, int origem_coluna)
        {
            for(int i = 0; i < Desenho.Count; i++)
            {
                for(int j = 0; j < Desenho[i].Count; j++)
                {
                    if ( ((i + origem_linha) >= Matriz.Count) || ((j + origem_coluna) >= Matriz[i].Count) ) continue;
                    
                    Matriz[i + origem_linha][j + origem_coluna] = Desenho[i][j];
                }
            }
        }

        /// <summary>
        /// Retorna o nº de colunas vazias à esquerda do desenho.
        /// </summary>
        /// <returns>Retorna o índice da última coluna vazia.</returns>
        private int ColunasVaziasAEsquerda(List<List<Boolean>> Matriz)
        {
            for(int i = 0; i < Matriz.Count; i++)
            {
                for(int j = 0; j < Matriz[i].Count; j++)
                {
                    if (Matriz[i][j] == true)
                        return i - 1;
                }
            }

            return 0;
        }

        private void AlinhamentoEsquerda(List<List<Boolean>> Matriz)
        {
            int decremento = ColunasVaziasAEsquerda(Matriz);

            if (decremento > 0)
            {
                for (int i = 0; i < Matriz.Count; i++)
                {
                    for (int j = 0; j < Matriz[i].Count; j++)
                    {
                        if (Matriz[i][j] == true)
                        {
                            Matriz[i][j] = false;
                            Matriz[i - decremento - 1][j] = true;
                        }
                    }
                }
            }
        }

        private int ColunasVaziasADireita(List<List<Boolean>> Matriz)
        {
            for (int i = Matriz.Count - 1; i >= 0; i--)
            {
                for (int j = Matriz[i].Count - 1; j >= 0; j--)
                {
                    if (Matriz[i][j] == true)
                        return Matriz.Count - i - 1;
                }
            }

            return 0;
        }

        private void AlinhamentoDireita(List<List<Boolean>> Matriz)
        {
            int incremento = ColunasVaziasADireita(Matriz);

            if (incremento > 0)
            {
                for (int i = 0; i < Matriz.Count - incremento; i++)
                {
                    for (int j = 0; j < Matriz[i].Count; j++)
                    {
                        if (Matriz[i][j] == true)
                        {
                            Matriz[i][j] = false;
                            Matriz[i + incremento][j] = true;
                        }
                    }
                }
            }
        }

        private void CentralizarHorizontal(List<List<Boolean>> Matriz)
        {
            //calcula a largura do desenho.
            int inicio_desenho = 0;
            int fim_desenho = 0;
            int largura_desenho = 0;
            int origem_desenho = 0;
            int ordenada_desenho = 0;

            inicio_desenho = ColunasVaziasAEsquerda(Matriz);
            fim_desenho = ColunasVaziasADireita(Matriz);
            largura_desenho = Matriz.Count - (inicio_desenho + fim_desenho);
            origem_desenho = (Matriz.Count/2) - (largura_desenho/2);
            //ordenada_desenho = RetornaOrdenada(Matriz);

            List<List<Boolean>> desenho_temp = new List<List<bool>>();

            desenho_temp = SeparaDesenho(Matriz);
            DrawMatriz(Matriz, desenho_temp, origem_desenho, 0);


        }

        /// <summary>
        /// Leia o nome da função;
        /// </summary>
        /// <param name="Matriz"></param>
        private void CentralizarVertical(List<List<Boolean>> Matriz)
        {
            int indice_superior = MaiorIndiceSuperior(SeparaDesenho(Matriz));
            int indice_inferior = MenorIndiceInferior(SeparaDesenho(Matriz));
            int altura_desenho = Matriz[0].Count - (indice_inferior + indice_superior);


            if (indice_inferior > Matriz[0].Count - 1 - indice_superior)
                AlinharAoFundo(Matriz, (((Matriz[0].Count - 8) ) / 2) - (Matriz[0].Count - 1 - indice_superior));//((Matriz[0].Count - 1 - (Matriz[0].Count - 1 - indice_superior) - indice_inferior)) / 2); //

            if (indice_inferior < Matriz[0].Count - 1 - indice_superior)
                AlinharAcima(Matriz, ((Matriz[0].Count - indice_superior - indice_inferior)) / 2);
                

        }

        private void AlinharAoFundo(List<List<Boolean>> Matriz, int deslocamento)
        {
            int indice_origem = ColunasVaziasAEsquerda(Matriz) + 1;
            int indice_fim = ColunasVaziasADireita(Matriz) - 1;
            int largura_desenho = Matriz.Count - (indice_origem + indice_fim);
            int pontos_a_remover = deslocamento;

            if(deslocamento == 0)
                pontos_a_remover = MenorIndiceInferior(Matriz);

            for (int i = indice_origem; i < largura_desenho + indice_origem; i++)
            {
                int linha = 0;

                while (linha < pontos_a_remover)
                {
                    Matriz[i].RemoveAt(0);
                    Matriz[i].Add(false);
                    linha = linha + 1;
                }
                
            }

        }
        

        private void AlinharAcima(List<List<Boolean>> Matriz, int deslocamento)
        {
            int indice_origem = ColunasVaziasAEsquerda(Matriz) + 1;
            int indice_fim = ColunasVaziasADireita(Matriz) - 1;
            int largura_desenho = Matriz.Count - (indice_origem + indice_fim);
            int pontos_a_remover = deslocamento;
                

            if (deslocamento == 0)
                 pontos_a_remover = Matriz[0].Count - MaiorIndiceSuperior(Matriz);

            for (int i = indice_origem; i < largura_desenho + indice_origem; i++)
            {
                int linha = 0;

                while (linha < pontos_a_remover)
                {
                    Matriz[i].RemoveAt(Matriz[i].Count - 1);
                    Matriz[i].Insert(0, false);
                    linha = linha + 1;
                }

            }
   
        }

        /// <summary>
        /// Parametrize a matriz do desenho que eu te retorno o indice primeiro ponto inferior do mesmo.
        /// </summary>
        /// <param name="Desenho">Matriz do desenho.</param>
        /// <returns>indice do ponto mais próximo ao fundo do painel.</returns>
        private int MenorIndiceInferior(List<List<Boolean>> Desenho)
        {
            int coluna_anterior = Desenho[0].Count;

            for (int i = 0; i < Desenho.Count; i++)
            {
                for (int j = 0; j < Desenho[i].Count; j++)
                {
                    if (Desenho[i][j] == true)
                    {
                        if (j < coluna_anterior)
                        {
                            coluna_anterior = j;
                        }
                        break;
                    }
                }
            }

            return coluna_anterior; ;
        }

        private int MaiorIndiceSuperior(List<List<Boolean>> Desenho)
        {
            int coluna_anterior = 0;//Desenho[0].Count - 1; 

            for (int i = 0; i < Desenho.Count; i++)
            {
                for (int j = Desenho[i].Count - 1; j >= 0 ; j--)
                {
                    if (Desenho[i][j] == true)
                    {
                        if (j > coluna_anterior)
                        {
                            coluna_anterior = j;
                        }
                        continue;
                    }
                }
            }

            return coluna_anterior;
   
        }

        /// <summary>
        /// Varre a matriz e copia todos os pontos ligados e desligados que formam o desenho;
        /// </summary>
        /// <param name="Matriz">Painel.</param>
        /// <returns>Retorna o desenho direto da matriz.</returns>
        private List<List<Boolean>> SeparaDesenho(List<List<Boolean>> Matriz)
        {
            List<List<Boolean> > Resultado = new List<List<bool>>();
            int inicio_desenho = ColunasVaziasAEsquerda(Matriz);
            int final_desenho = ColunasVaziasADireita(Matriz);

            int indice_resultado = 0;
            for(int i = inicio_desenho + 1; i < Matriz.Count - final_desenho; i++)
            {
                Resultado.Add(new List<bool>());

                for(int j = 0; j < Matriz[i].Count; j++)
                {
                    Resultado[indice_resultado].Add(Matriz[i][j]);
                }

                indice_resultado = indice_resultado + 1;
            }

            return Resultado;
        }

        /// <summary>
        /// Busca na matriz, a ordenada do desenho(ordenada do primeiro ponto).
        /// </summary>
        /// <param name="Matriz">Painel.</param>
        /// <returns>Ordenada do desenho na forma de inteiro.</returns>
        private int RetornaOrdenada(List<List<Boolean>> Matriz)
        {
            for(int i = 0; i < Matriz.Count; i++)
            {
                for(int j = 0; j < Matriz[i].Count; j++)
                {
                    if (Matriz[i][j] == true) return j;
                }
            }

            return 0;
        }

        //============================== código para matrizes em array.

        public void InicializaMatrix(Boolean[,] Matrix)
        {
            for (int i = 0; i < NUMERO_DE_LINHAS; i++)
                for (int j = 0; j < NUMERO_DE_COLUNAS; j++)
                {
                    Matrix[i, j] = false;
                }
        }

        private void AlinhamentoEsquerdaArray(Boolean[,] Matrix)
        {
            int inicio_desenho = ColunasVaziasAEsquerdaArray(Matrix);
            int fim_desenho = ColunasVaziasADireitaArray(Matrix);
            int inicio_matrix = 0;

            for (int colunas = inicio_desenho; colunas < fim_desenho - inicio_desenho; colunas++)
            {
                CopiaColuna(Matrix, colunas, inicio_matrix);
                //ApagaColuna(Matrix, colunas);
                inicio_matrix = inicio_matrix + 1;
            }

            for (int i = fim_desenho - inicio_desenho + 1; i < Matrix.GetLength(1); i++)
                ApagaColuna(Matrix, i);

        }


        private void AlinhamentoDireitaArray(Boolean[,] Matrix)
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


        private void CentralizarHorizontalArray(Boolean[,] Matrix)
        {
            //calcula a largura do desenho.
            int inicio_desenho = 0;
            int fim_desenho = 0;
            int largura_desenho = 0;
            int origem_desenho = 0;
            int ordenada_desenho = 0;

            inicio_desenho = ColunasVaziasAEsquerdaArray(Matrix);
            fim_desenho = ColunasVaziasADireitaArray(Matrix);
            largura_desenho = fim_desenho - inicio_desenho;
            origem_desenho = (Matrix.GetLength(1) / 2) - (largura_desenho / 2);

            int coluna_temp = origem_desenho;
            for (int colunas = inicio_desenho; colunas <= fim_desenho; colunas++)
            {
                CopiaColuna(Matrix, colunas, coluna_temp);
                coluna_temp = coluna_temp + 1;
            }

            //apaga lado esquerdo.
            for (int i = 0; i < origem_desenho - 1; i++)
                ApagaColuna(Matrix, i);

            //apaga lado direito.
            for (int i = Matrix.GetLength(1) - 1; i > origem_desenho + (fim_desenho - inicio_desenho); i--)
                ApagaColuna(Matrix, i);

        }


        private void CentralizarVerticalArray(Boolean[,] Matrix)
        {
            int indice_superior = MaiorIndiceSuperiorArray(Matrix);
            int indice_inferior = MenorIndiceInferiorArray(Matrix);
            int altura_desenho = indice_inferior - indice_superior + 1;

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

            for (int i = origem_linha - 1 - altura_desenho; i > 0; i--)
            {
                ApagaLinha(Matrix, i);
            }

        }

        
        private void AlinharAoFundoArray(Boolean[,] Matrix)
        {

            int indice_superior = MaiorIndiceSuperiorArray(Matrix);
            int indice_inferior = MenorIndiceInferiorArray(Matrix);
            int indice_linha = Matrix.GetLength(0) - 1;

            for (int linha = indice_inferior; linha >= indice_superior; linha--)
            {
                CopiaLinha(Matrix, linha, indice_linha);
                indice_linha = indice_linha - 1;
            }

            for (int i = indice_inferior; i >= 0; i--)
                ApagaLinha(Matrix, i);

        }


        private void AlinharAcimaArray(Boolean[,] Matrix)
        {
            //int pontos_a_remover = deslocamento;
            int indice_superior = MaiorIndiceSuperiorArray(Matrix);
            int indice_inferior = MenorIndiceInferiorArray(Matrix);
            int indice_superior_matrix = Matrix.GetLength(0) - 1;

            int indice_linha = 0;
            for (int linha = indice_superior; linha <= indice_inferior; linha++)
            {
                CopiaLinha(Matrix, linha, indice_linha);
                indice_linha = indice_linha + 1;
            }

            for (int i = indice_inferior - indice_superior + 1; i < Matrix.GetLength(0); i++)
                ApagaLinha(Matrix, i);
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
                        return coluna;
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
        /// Copia a coluna indicada pelo indice_fonte para indice_alvo.
        /// </summary>
        /// <param name="Matrix">Matriz em arrays.</param>
        /// <param name="indice_fonte">indice da coluna a ser copiada.</param>
        /// <param name="indice_alvo">indice da coluna a ser substituída.</param>
        private void CopiaColuna(Boolean[,] Matrix, int indice_fonte, int indice_alvo)
        {
            for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                Matrix[linha, indice_alvo] = Matrix[linha, indice_fonte];
        }

        private void CopiaLinha(Boolean[,] Matrix, int indice_fonte, int indice_alvo)
        {
            // Entra na matrix
            for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
                Matrix[indice_alvo, coluna] = Matrix[indice_fonte, coluna];
        }

        /// <summary>
        /// Apaga uma coluna, gravando valores false.
        /// </summary>
        /// <param name="Matrix">Matriz a ser alterada.</param>
        /// <param name="indice_coluna"></param>
        private void ApagaColuna(Boolean[,] Matrix, int indice_coluna)
        {
            for (int linha = 0; linha < Matrix.GetLength(0); linha++)
                Matrix[linha, indice_coluna] = false;
        }

        /// <summary>
        /// Apaga uma linha, gravando valores false.
        /// </summary>
        /// <param name="Matrix">Matriz a ser alterada.</param>
        /// <param name="indice_linha">índice da linha.</param>
        private void ApagaLinha(Boolean[,] Matrix, int indice_linha)
        {
            for (int coluna = 0; coluna < Matrix.GetLength(1); coluna++)
                Matrix[indice_linha, coluna] = false;
        }


        //==============================

        /// <summary>
        /// Desenha a matriz no arquivo.
        /// </summary>
        /// <param name="Matriz">Matriz a ser desenhada.</param>
        public void DesenhaMatriz(List<List<Boolean>> Matriz)
        {
            string arquivoNome = @"C:\ProgramData\LDX12\matriz.txt";
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
            int linha = 0;
            int coluna = Matriz[0].Count - 1;

            while(true)
            {
                if (Matriz[linha][coluna] == true)
                    dados.Add(37);
                else dados.Add(45);

                linha = linha + 1;

                if (linha == Matriz.Count)
                {
                    linha = 0;
                    coluna = coluna - 1;

                    dados.Add(13);
                    dados.Add(10);
                    
                    if (coluna == -1) break;
                }
            }

            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados.ToArray(), 0, dados.ToArray().Length);
            fs.Close();
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
                    if (((i + origem_linha) >= Matrix.GetLength(0)) || ((j + origem_coluna) >= Matrix.GetLength(1))) continue;

                    Matrix[i + origem_linha, j + origem_coluna] = Desenho[i, j];
                }
            }
        }
    }
}

