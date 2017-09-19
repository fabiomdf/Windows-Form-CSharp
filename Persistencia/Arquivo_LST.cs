using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Erros;
using Util;


namespace Persistencia
{
    public class Arquivo_LST //: IArquivo
    {
        public unsafe struct FormatoListaArquivos
        {
            public Byte versao;
            public fixed Byte reservado [3];
            public UInt32 qtdArquivos;
            public fixed Byte reservado2 [54];
            public UInt16 crc;
        }

        //public Byte versao { get; set; }
        private Byte versao = 1;
        public byte[] reservado;
        public UInt32 qtdArquivos { get; set; }
        public byte[] reservado2;
        public UInt16 crc { get; set; }
        // Colocar somente o nome do arquivo, esquecer o path e a extensão.
        public List<String> listaPaths { get; set; }

        public Arquivo_LST()
        {
            this.reservado = new byte[3];
            this.qtdArquivos = 0;
            this.reservado2 = new byte[54];
            this.crc = 0;
            this.listaPaths = new List<string>();
        }

        public Arquivo_LST(Arquivo_LST oldValue)
        {
            this.versao = oldValue.versao;

            this.reservado = new byte[oldValue.reservado.Length];
            oldValue.reservado.CopyTo(this.reservado, 0);

            this.qtdArquivos = oldValue.qtdArquivos;

            this.reservado2 = new byte[oldValue.reservado2.Length];
            oldValue.reservado2.CopyTo(this.reservado2, 0);

            this.crc = oldValue.crc;

            this.listaPaths = new List<string>();

            foreach (String sOld in oldValue.listaPaths)
                this.listaPaths.Add(sOld);

        }

        public int QTDArquivos
        {
            get { return this.listaPaths.Count; }
            set { this.qtdArquivos = (uint)value; }
        }

        public byte[] paths
        {
            get
            {
                qtdArquivos = (UInt16)this.listaPaths.Count;
                byte[] array = new byte[qtdArquivos * Util.Util.ARQUIVO_LST_PATH_SIZE];
               
                int indiceFrase = 0;
                foreach (var str in listaPaths)
                {
                    char[] charArray = str.PadRight(Util.Util.ARQUIVO_LST_PATH_SIZE, '\0').ToCharArray();
                    for (int i = 0; i < Util.Util.ARQUIVO_LST_PATH_SIZE; i++)
                    {
                        array[(indiceFrase * Util.Util.ARQUIVO_LST_PATH_SIZE) + i] = (Byte)charArray[i];
                    }
                    indiceFrase++;
                }

                return array;
            }
        }

        public void Default()
        {
            //versao = 1;
            reservado = new byte[3];
            qtdArquivos = 1;
            reservado2 = new byte[54];
            this.listaPaths = new List<string>();
            listaPaths.Add("roteiros/rot1");
        }

        public void Salvar(string arquivoNome)//, object controlador)
        {            
            AtualizarCRC();
            byte[] dados = this.toByteArray();

            dados = Util.Util.CalcularCRC2(dados);

            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();
        }



        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoListaArquivos* formatoListaArquivos = (FormatoListaArquivos*)pSrc;

                this.crc = CalcularCRC(dados);
            }           
        }
     
        private byte[] toByteArray()
        {            
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoListaArquivos) + (this.QTDArquivos * Util.Util.ARQUIVO_LST_PATH_SIZE)]; 

                fixed (byte* pSrc = resultado)
                {
                    FormatoListaArquivos* listaArquivos = (FormatoListaArquivos*)pSrc;

                    paths.CopyTo(resultado, sizeof(FormatoListaArquivos));

                    listaArquivos->versao = this.versao;
                    
                    listaArquivos->qtdArquivos = this.qtdArquivos;

                    listaArquivos->crc = this.crc;
                    
                    return resultado;
                }
            }
        }

        private void FromBytesToFormatoPainelCfg(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoListaArquivos* listaArquivos = (FormatoListaArquivos*) pSrc;
                    Byte[] temporario = new byte[listaArquivos->qtdArquivos];

                    this.versao = listaArquivos->versao;
                    
                    this.qtdArquivos = listaArquivos->qtdArquivos;

                    this.crc = listaArquivos->crc;
                    for (int i = 0; i < this.qtdArquivos; i++)
                    {
                        this.listaPaths.Add(string.Empty);
                        this.listaPaths[i] = ArrayLDX2.ByteArrayToString(pSrc + sizeof(FormatoListaArquivos) + (i * Util.Util.ARQUIVO_LST_PATH_SIZE), Util.Util.ARQUIVO_LST_PATH_SIZE);
                    }

                }
            }
        }

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

                    FromBytesToFormatoPainelCfg(dados);
                    return this;
                }
            }
            return null;
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
        
        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            int tamanho;

            Byte[] dadosCRC = new byte[sizeof(FormatoListaArquivos) - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoListaArquivos* regiao = (FormatoListaArquivos*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&regiao->crc - (int)pSrc);
                Array.Copy(dados, ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&regiao->crc - (int)pSrc,
                           sizeof(FormatoListaArquivos) - ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }



        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {             
                resposta =  (fs.Length >= sizeof (FormatoListaArquivos));
            }
            return resposta;
        }

        public bool VerificarCRC(byte[] dados)
        {
            byte[] temp = new byte[2];
            byte[] dados2 = new byte[(int)dados.Length];
            
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoListaArquivos* vid = (FormatoListaArquivos*)pSrc;

                    dados2 = Util.Util.CalcularCRC2(dados);

                    temp[0] = dados2[Util.Util.CRCPosition1];
                    temp[1] = dados2[Util.Util.CRCPosition2];

                    return (vid->crc == BitConverter.ToUInt16(temp, 0));
                }
            }
        }
        public bool VerificarRelacaoTamanhoArquivoLista(string arquivoNome)
        {
            long tamanhoArquivo = 0;
            
            FileStream fs = File.OpenRead(arquivoNome);
            tamanhoArquivo = (int) fs.Length;
            fs.Close();
            long qntBytesImagem = 0;

            for (int i =0 ;i<this.qtdArquivos;i++)
            {
                qntBytesImagem += listaPaths[i].Length;
            }

            //Verifica se o tamanho do arquivo está de acordo com o tamanho da imagem
            unsafe
            {
                if (sizeof (FormatoListaArquivos) + qntBytesImagem != tamanhoArquivo)
                {
                    return false;
                }
            }

            return true;
        }                    
    }
}
