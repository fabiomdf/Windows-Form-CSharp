using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_FNT
    {
        private const int RESERVADO = 3;
        private const int RESERVADO2 = 4;
        
        public unsafe struct FormatoFonte
        {
            public Byte versao;
            public fixed Byte reservado[RESERVADO]; //4
            public UInt32 altura;                 
            public fixed Byte reservado2[4]; //12

            //byte 1: Italico;
            //byte 2: Negrito;
            //byte 3 em diante, reservado;
            public Byte flags;            
            public fixed byte nomeFonte[49];     //13 + 49 
            public UInt16 crc;

            //public fixed CaractereInfo caractereInfo[224];
            public fixed byte caractereInfo[224 * 8];//sizeof(CaractereInfo)];            
        }

        public unsafe struct CaractereInfo
        {
            public byte codigo;
            public fixed byte reservado[3];
            public UInt32 enderecoBitmap;

        }
        
        public uint[] ListaSizeBits = new uint[224]; 
        //public Byte Versao { get; set; }
        private Byte Versao = 1;
        public Byte[] reservado { get; set; }
        public UInt32 Altura { get; set; }
        public Byte[] reservado2 { get; set; }

        //byte 1: Italico;
        //byte 2: Negrito;
        //byte 3 em diante, reservado;
        public Byte flags { get; set; }
        public Byte[] nomeFonte { get; set; } //49
        public UInt16 crc { get; set; }

        //public CaractereInfo[] caractereInfo { get; set; }
        public List<CaractereInfo> caracteres { get; set; }
        public List<Bitmap> bitmaps { get; set; }
        public static UInt32 TamanhoFormatoFonte()
        {
            unsafe
            {
                return (UInt32)sizeof(Persistencia.Arquivo_FNT.FormatoFonte);
            }
        }
        public Arquivo_FNT()
        {
            InicializaListaSizeBits();

            //Versao = 1;
            reservado = new byte[3];
            Altura  = 8;
            reservado2 = new byte[4];

            //byte 1: Italico;
            //byte 2: Negrito;
            //byte 3 em diante, reservado;
            flags  = 0;
            nomeFonte = new byte[49];
            crc = 0;
            //caractereInfo = new CaractereInfo[224];
            caracteres = new List<CaractereInfo>(224);
            this.bitmaps = new List<Bitmap>();

                unsafe
                {
                   
                    for (int i = 0; i < 224; i++)
                    {
                        CaractereInfo ci = new CaractereInfo();

                        ci.codigo = (byte)(i + 32);
                        //ci.reservado = new byte[3];

                        //if (i == 0)
                        //    ci.enderecoBitmap = (UInt32)sizeof(FormatoFonte);
                        //else
                        //    ci.enderecoBitmap = TamanhoBitmapAcumulado(i);

                        this.caracteres.Add(ci);
                    }
                }
        }
        

        public Arquivo_FNT(Arquivo_FNT oldValue)
        {
            ListaSizeBits = new uint[oldValue.ListaSizeBits.Length]; 
            oldValue.ListaSizeBits.CopyTo(this.ListaSizeBits, 0);

            this.Versao = oldValue.Versao;

            this.reservado = new Byte[oldValue.reservado.Count()];
            oldValue.reservado.CopyTo(this.reservado, 0);
        
            this.Altura = oldValue.Altura;
         
            this.reservado2 = new Byte[oldValue.reservado2.Count()];
            oldValue.reservado2.CopyTo(this.reservado2, 0);
            
            this.flags = oldValue.flags;

            this.nomeFonte = new Byte[oldValue.nomeFonte.Length];
            oldValue.nomeFonte.CopyTo(this.nomeFonte, 0);

            this.crc = oldValue.crc;

            this.caracteres = new List<CaractereInfo>();
            foreach(CaractereInfo cOld in oldValue.caracteres)
            {
                this.caracteres.Add(cOld);
            }
             
            foreach(Bitmap bOld in oldValue.bitmaps)
            {
                this.bitmaps.Add(new Bitmap(bOld));
            }

        }



        public int VerificarVersaoUsuario(string arquivoNome)
        {
            FileStream fs = File.OpenRead(arquivoNome);
            byte[] dados = new byte[1];
            fs.Read(dados, 0, 1);
            fs.Close();

            return dados[0];
        }

        public bool RestaurarVersao(string arquivoNome)
        {

            int versaoArquivoUsuario = VerificarVersaoUsuario(arquivoNome);

            //Comparando a versão do arquivo com a versão do arquivo do usuário
            if (this.Versao == versaoArquivoUsuario)
                return false;


            //Parte do código onde irá restaurar os arquivos das versões anteriores, foi testado e comentado porque não há versões novas de arquivo_alt

            //// Recuperando o ultimo LPK
            //Arquivo_LPK_V02 LPKRestaurado = new Arquivo_LPK_V02(this.rm);
            //LPKRestaurado.nome = arquivoNome;
            //LPKRestaurado.RestaurarVersao();

            ////Adicionando as alterações da versão 3 ao lpk v2 recuperado

            ////copiando as informações do arquivo do usuário para a nova versão
            //this.rm = LPKRestaurado.rm;
            //this.nome = LPKRestaurado.nome;
            //this.texto = LPKRestaurado.texto;

            ////adicionando as novas informações
            //this.texto.Add(rm.GetString("TEXTO_104_ARQUIVO_LPK").PadRight(TAM_MAX_FRASE, '\0'));

            return true;

        }


        /// <summary>
        /// Retorna o somatório dos tamanhos dos bitmaps anteriores ao do índice parametrizado.
        /// </summary>
        /// <param name="indice_bitmap">Indice do bitmap para o qual se deseja saber o offset.</param>
        /// <returns>Endereço do bitmap parametrizado.</returns>
        public UInt32 TamanhoBitmapAcumulado(int indice_bitmap)
        {
            unsafe
            {
                UInt32 resultado = (UInt32)sizeof(FormatoFonte);

                for (int i = 0; i < indice_bitmap; i++)
                {
                    //result = result + this.bitmaps[i].tamanho;

                    Byte[] pixelByte = bitmaps[i].FromMatrixToArray();
                    resultado = (UInt32)(resultado + pixelByte.Length + sizeof(uint));

                }
                CaractereInfo ci = new CaractereInfo();
                ci.codigo = (byte)(indice_bitmap + 32);
                ci.enderecoBitmap = resultado;

                this.caracteres[indice_bitmap] = ci;
                //for (int i = 0; i < indice_bitmap; i++)
                //{
                //    result = result + this.bitmaps[i].;
                //}

                return resultado;
            }
            
        }
       
        public CaractereInfo this[char index]
        {
            get
            {
                int indice = (int)index - 32;

                if ((indice < 0) | (indice > this.caracteres.Count - 1)) 
                    throw new IndexOutOfRangeException(indice.ToString());
                else
                {
                    return caracteres[indice];
                }
                
            }
        }

        public void Salvar(string arquivoNome)//, object controlador)
        {/*
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();
          */

            AtualizarCRC();
            byte[] dados = this.FonteToByteArray();
            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();

        }

        public void AtualizarCRC()
        {
            /*
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoFonte* parametros = (FormatoFonte*)pSrc;

                this.crc = CalcularCRC(dados);
            }
             */

            const int posCRC1 = 62;
            const int posCRC2 = 63;

            Byte[] dados = FonteToByteArray();

            List<byte> dadosValidos = new List<byte>();

            for (int i = 0; i < dados.Length; i++)
            {
                if ((i == posCRC1) || (i == posCRC2))
                {
                    continue;
                }
                else
                {
                    dadosValidos.Add(dados[i]);
                }

            }
            ushort crc16 = CRC16CCITT.Calcular(dadosValidos.ToArray());

            dados[posCRC1] = (byte)(crc16 >> 8);
            dados[posCRC2] = (byte)(crc16);

            this.crc = crc16;

        }


        public bool VerificarIntegridade(string arquivoNome)
        {
            bool resposta = false;
            FileStream fs = File.OpenRead(arquivoNome);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            resposta = VerificarCRC(dados);
            if (!resposta)
            {
                throw new CRCFileException("CRC file error.");
            }
            resposta = VerificarTamanhoArquivo(fs);
            if (!resposta)
            {
                throw new SizeFileException("Size file error.");
            }

            fs.Close();
            return resposta;
        }
        public string NomeArquivo = String.Empty;
        public object Abrir(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
            {
                unsafe
                {
                    FileStream fs = File.OpenRead(arquivoNome);
                    byte[] dados = new byte[(int)fs.Length];
                    fs.Read(dados, 0, dados.Length);
                    fs.Close();
                    this.NomeArquivo = arquivoNome;
                    FromBytesToFormatoFonte(dados);
                    return this;
                }
            }
            return null;
        }

        public object AbrirSimples(string arquivoNome)
        {
            unsafe
            {
                FileStream fs = File.OpenRead(arquivoNome);
                byte[] dados = new byte[(int)fs.Length];
                fs.Read(dados, 0, dados.Length);
                fs.Close();
                this.NomeArquivo = arquivoNome;
                FromBytesToFormatoFonteSimples(dados);
                return this;
            }
        }

        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoFonte)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoFonte* formatoFonte = (FormatoFonte*)pSrc;

                    formatoFonte->versao = this.Versao;

                    for (int i = 0; i < 3; i++)
                    formatoFonte->reservado[i] = this.reservado[i];

                    formatoFonte->altura = this.Altura;

                    for (int i = 0; i < 4; i++)
                        formatoFonte->reservado2[i] = this.reservado2[i];

                    formatoFonte->flags = this.flags;

                    for (int i = 0; i < 49; i++)
                        formatoFonte->nomeFonte[i] = this.nomeFonte[i];

                    formatoFonte->crc = this.crc;

                    for (int caracInfo = 0; caracInfo < 224; caracInfo++)
                    {
                        CaractereInfo* caracteresInfo = ((CaractereInfo*) formatoFonte->caractereInfo);
                        caracteresInfo[caracInfo].codigo = this.caracteres[caracInfo].codigo;
                        caracteresInfo[caracInfo].enderecoBitmap = this.caracteres[caracInfo].enderecoBitmap;
                    }

                }

                return resultado;
            }
        }

        /// <summary>
        /// Gera o array de byte com os dados de cabeçalho(FormatoFonte + os bitmaps.)
        /// </summary>
        /// <returns></returns>
        private byte[] FonteToByteArray()
        {
            byte[] dados = this.toByteArray();
            List<byte> bmps = new List<byte>();
            int indice = 0;
            foreach (Bitmap bmp in this.bitmaps)
            {
                Byte[] pixelByte = bmp.FromMatrixToArray();
                ListaSizeBits[indice] = (uint)(bmp.matriz.GetLength(0)*bmp.matriz.GetLength(1));
                bmps.AddRange(BitConverter.GetBytes(ListaSizeBits[indice]));
                indice++;
                bmps.AddRange(pixelByte);
            }

            byte[] result = new byte[dados.Count() + bmps.Count];
            
            dados.CopyTo(result, 0);
            bmps.ToArray().CopyTo(result, dados.Count());
            
            return result;
        }


        private void FromBytesToFormatoFonte(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoFonte* formatoFonte = (FormatoFonte*)pSrc;

                    this.Versao = formatoFonte->versao;

                    this.Altura = formatoFonte->altura;

                    this.flags = formatoFonte->flags;

                    for (int i = 0; i < 49; i++)
                        this.nomeFonte[i] = formatoFonte->nomeFonte[i];

                    this.crc = formatoFonte->crc;

                    int indiceInfoCaractere = 0;

                    this.caracteres.Clear();

                    //carrega as informações do cabeçalho de caracteres.
                    for (int i = 0; i < 224; i++)
                    {
                        CaractereInfo c = new CaractereInfo();
                        CaractereInfo* caracteresInfo = ((CaractereInfo*)formatoFonte->caractereInfo);
                        c.codigo = caracteresInfo[i].codigo;
                        c.enderecoBitmap = caracteresInfo[i].enderecoBitmap;
                        this.caracteres.Add(c);

                    }
                }
            }
            //caracteres.Sort(delegate(CaractereInfo a, CaractereInfo b)
            //                    {
            //                        return a.codigo.CompareTo(b.codigo);
            //                    });
            // Ordenar os caracteres pelo código, pois algumas fontes do Pontos 6 não estão em ordem
            caracteres.Sort((a, b) => a.codigo.CompareTo(b.codigo));

            //carrega as informações sobre os bitmaps.
            this.bitmaps.Clear();
            for (int bitmap = 0; bitmap < 224; bitmap++)
            {
                Bitmap b = new Bitmap();
                b = CarregarBitmap(this.NomeArquivo, caracteres[bitmap].enderecoBitmap, bitmap);
                this.bitmaps.Add(b);
            }



        }

        private void FromBytesToFormatoFonteSimples(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoFonte* formatoFonte = (FormatoFonte*)pSrc;

                    this.Versao = formatoFonte->versao;

                    this.Altura = formatoFonte->altura;

                    this.flags = formatoFonte->flags;

                    for (int i = 0; i < 49; i++)
                        this.nomeFonte[i] = formatoFonte->nomeFonte[i];

                    this.crc = formatoFonte->crc;
                }
            }
        }

        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            int tamanho;
            List<byte> dadosBitmaps = new List<byte>();

            Byte[] dadosCRC = new byte[dados.Length - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoFonte* parametros = (FormatoFonte*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&parametros->crc - (int)pSrc);
                Array.Copy(dados, ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&parametros->crc - (int)pSrc,
                           sizeof(FormatoFonte) - ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)));

                for (int i = sizeof(FormatoFonte); i<dados.Length;i++)
                {
                    dadosCRC[i - 2] = dados[i];
                }

                //return CRC16CCITT.Calcular(dadosCRC);
            }
            
            //foreach (Bitmap bmp in this.bitmaps)
            //{
            //    dadosBitmaps.AddRange(bmp.FromMatrixToArray());                
            //}
            
            //tamanho = dadosCRC.Length;

            //Array.Resize(ref dadosCRC, dadosCRC.Length + dadosBitmaps.Count);

            //byte[] aux = dadosBitmaps.ToArray();
            //Array.Copy(aux, 0, dadosCRC, tamanho, aux.Length);

            return CRC16CCITT.Calcular(dadosCRC);
        }
        private void InicializaListaSizeBits()
        {
            for (int i = 0; i < ListaSizeBits.Length; i++)
            {
                ListaSizeBits[i] = 0;
            }
        }

        private Bitmap CarregarBitmap(String arquivoNome, long enderecoBitmap, int indice)
        {
            #region Código Antigo
            //Bitmap b = new Bitmap();
            //FileStream fs = File.OpenRead(arquivoNome);
            //try
            //{                
            //    byte[] dados = new byte[4];

            //    fs.Seek(enderecoBitmap, SeekOrigin.Current);
            //    fs.Read(dados, 0, dados.Length);

            //    b.altura = Altura;

            //    uint tamanho = 0;
            //    for (int size = 0; size < 4; size++)
            //    {
            //        tamanho = tamanho + (uint)(dados[size] << (8 * size));
            //    }
            //    ListaSizeBits[indice] = tamanho;

            //    bool maisUm = (tamanho % 8 > 0);
            //    tamanho = tamanho / 8;
            //    tamanho = (maisUm) ? tamanho + 1 : tamanho;

            //    dados = new byte[tamanho];
            //    fs.Read(dados, 0, dados.Length);
            //    b.FromArrayToMatrix(dados);

            //}
            //catch { }
            //finally
            //{
            //    fs.Close();
            //}
            //return b;
            #endregion Código Antigo
            #region Código Novo
            Bitmap b = new Bitmap();
            // FileStream fs = File.OpenRead(arquivoNome);
            byte[] conteudoArquivo = File.ReadAllBytes(arquivoNome);
            try
            {
                byte[] dados = new byte[4];
                uint tamanho = BitConverter.ToUInt32(conteudoArquivo, (int) enderecoBitmap);

                b.altura = Altura;
                          
                ListaSizeBits[indice] = tamanho;
                tamanho = (tamanho % 8 > 0) ? (tamanho / 8) + 1 : (tamanho / 8);

                dados = new byte[tamanho];
                Array.Copy(conteudoArquivo, enderecoBitmap + 4, dados, 0, tamanho);
                b.FromArrayToMatrix(dados, (int)ListaSizeBits[indice]);

            }
            catch { }
            #endregion Código Novo
            return b;

        }

        public bool VerificarCRC(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoFonte* parametros = (FormatoFonte*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            } 
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoFonte));
            }
            return resposta;
        }

        public bool verificarConsistenciaDados(string pathFonte)
        {
            long tamanhoArquivo = 0;
            long qntTotalBytes = 0;
            
            FileStream fs = File.OpenRead(pathFonte);
            tamanhoArquivo = fs.Length;
            fs.Close();

            for (int i = 0; i < 224; i++)
            {
                //Verifica se o código do caractere está de acordo com a posição
                if (this.caracteres[i].codigo != (char) (i + 0x20))
                {
                    return false;
                }
                long tamanho = bitmaps[i].altura*bitmaps[i].largura;
                
                //this->read((U32) & bitmap->tamanho, sizeof (tamanho), (U8*) &tamanho);
                
                qntTotalBytes += (tamanho/8) + (tamanho%8 == 0 ? 0 : 1);
                
                // Incrementa com o tamanho (4 bytes) para o total de bytes
                qntTotalBytes += sizeof(UInt32);

                //Verifica se o endereço do bitmap do caractere está dentro dos limites
                if ((this.caracteres[i].enderecoBitmap) >= (tamanhoArquivo - sizeof(UInt32)))
                {
                    return false;
                }
            }

            //Verifica se o tamanho do arquivo está de acordo com os caracteres presentes neste arquivo
            unsafe
            {
                if (sizeof (FormatoFonte) + qntTotalBytes != tamanhoArquivo)
                {
                    return false;
                }
            }

            return true;
        }

        public static unsafe CaractereInfo RetornaNovoCaracterInfo(int indice, uint acumulado)
        {
            Persistencia.Arquivo_FNT.CaractereInfo ci = new Persistencia.Arquivo_FNT.CaractereInfo();
            ci.codigo = (byte)(indice + 32);
            ci.enderecoBitmap = acumulado;
            return ci;
        }
    }
}
