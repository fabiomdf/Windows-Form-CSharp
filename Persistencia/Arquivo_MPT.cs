using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Videos;
using Util;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_MPT 
    {
        public unsafe struct FormatoMensagemPaths
        {
            /*
                typedef struct {
		        U8 versao;
		        U8 reservado1[1];
		        U16 idMensagem;
		        U8 reservado2[58];
		        U16 crc;
		        U8* video;
	            } FormatoMensagemPaths
             */
            public Byte versao;
            public Byte reservado;
            public UInt16 idMensagem;
            public fixed Byte reservado2 [58];
            public UInt16 crc;
            public fixed Byte pathVideo [64];
        }
        //public Byte versao { get; set; }
        private Byte versao = 1;
        public Byte reservado { get; set; }
        public UInt16 idMensagem { get; set; }
        public Byte[] reservado2{ get; set; }
        public UInt16 crc { get; set; }
        public string pathVideo{ get; set; }

        public List<Videos.Video> Videos = new List<Video>();

        public Arquivo_MPT()
        {
            //this.versao = 0;
            this.reservado = 0;
            this.idMensagem = 0;
            this.reservado2 = new byte[58];
            this.crc = 0;
            this.pathVideo = String.Empty;
        }

        public Arquivo_MPT(Arquivo_MPT oldValue)
        {
            this.versao = oldValue.versao;
            this.reservado = oldValue.reservado;
            this.idMensagem = oldValue.idMensagem;

            this.reservado2 = new byte[oldValue.reservado2.Length];
            oldValue.reservado2.CopyTo(this.reservado2, 0);

            this.crc = oldValue.crc;
            this.pathVideo = oldValue.pathVideo;

            this.Videos = UtilPersistencia.GravaVideo(this.Videos, oldValue.Videos);

        }


        public void GerarFormatoNovo(string arquivoNome, string diretorioRaiz)
        {
            FileStream fs = File.OpenRead(arquivoNome + Util.Util.sequencial_arquivo_mensagens.ToString("X8") + Util.Util.ARQUIVO_EXT_MPT);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            fs.Close();

            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMensagemPaths* mensagemPaths = (FormatoMensagemPaths*)pSrc;

                    this.versao = mensagemPaths->versao;
                    this.reservado = mensagemPaths->reservado;
                    this.idMensagem = mensagemPaths->idMensagem;
                    for (int i = 0; i < 58; i++)
                    {
                        this.reservado2[i] = mensagemPaths->reservado2[i];
                    }
                    this.crc = mensagemPaths->crc;
                    this.pathVideo = Util.Util.TrataDiretorio(ArrayLDX2.ByteArrayToString(mensagemPaths->pathVideo, 64));
                }
                //fs = File.OpenRead(diretorioRaiz + this.pathVideo);
                fs = File.OpenRead(this.pathVideo);
                byte[] dadosVideo = new byte[(int)fs.Length];
                fs.Read(dadosVideo, 0, dadosVideo.Length);
                fs.Close();

                Array.Resize(ref dados, (dados.Length - 64) + dadosVideo.Length);
                Array.Copy(dadosVideo, 0, dados, 64, dadosVideo.Length);

                fixed (byte* pSrc = dados)
                {
                    FormatoMensagemPaths* mensagemPaths = (FormatoMensagemPaths*)pSrc;

                    mensagemPaths->crc = CalcularCRC(dados);                 
                }

                fs = File.Create(arquivoNome + Util.Util.sequencial_arquivo_mensagens.ToString("X8") + Util.Util.ARQUIVO_EXT_MPT);
                fs.Write(dados, 0, dados.Length);
                fs.Close();
            }

        }

        /// <summary>
        /// Efetua a leitura dos arquivos de vídeos levando em consideração o formato novo.
        /// </summary>
        public void AbrirFormatoNovo(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
            {
                unsafe
                {
                    FileStream fs = File.OpenRead(arquivoNome);
                    byte[] dados = new byte[(int)fs.Length];
                    fs.Read(dados, 0, dados.Length);
                    fs.Close();

                    FromBytesToFormatoMPT(dados);
                }
            }
        }

        /// <summary>
        /// Traduz os bytes de dados arrayados para o formato novo.
        /// </summary>
        /// <param name="dados">Arquivo já lido do disco.</param>
        private void FromBytesToFormatoMPT(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMensagemPaths* mensagemPaths = (FormatoMensagemPaths*)pSrc;

                    this.versao = mensagemPaths->versao;
                    this.reservado = mensagemPaths->reservado;
                    this.idMensagem = mensagemPaths->idMensagem;
                    for (int i = 0; i < 58; i++)
                    {
                        this.reservado2[i] = mensagemPaths->reservado2[i];
                    }
                    this.crc = mensagemPaths->crc;
                    //a princípio, vamos pular esses 64 bytes.
                    //this.pathVideo = ArrayLDX2.ByteArrayToString(mensagemPaths->pathVideo, 64);

                    //limpa a lista de videos
                    this.Videos.Clear();

                    //indice para leitura dos dados no arquivo.
                    int indice = sizeof (FormatoMensagemPaths) - 64;

                    //efetua a leitura do tamanho do vídeo.(próximos 4 bytes após o FormatoMensagemPaths)
                    uint TamanhoArquivo = BitConverter.ToUInt32(dados, indice);

                    //indice do inicio do arquivo a ser lido
                    int indiceInicial = indice;

                    //pula o tamanho. (já no conteúdo de vídeo.)
                    indice = indice + sizeof (uint);
                    //pula a versão.
                    indice = indice + 1;

                    //aloca o suficiente pra ler o vídeo.
                    byte[] dadosVideo = new byte[TamanhoArquivo];

                    //copia os dados do arquivo de vídeo com o tamanho e versão.
                    Array.Copy(dados, indiceInicial, dadosVideo, 0, TamanhoArquivo);

                    //verifica o tipo do vídeo(v01 ou v02) ou se é Arquivo PLS..Equals("V01")
                    String tipo = Encoding.ASCII.GetString(dados, indice, 3).ToUpper();

                    //todo: tentar substituir as strings e utilizar tipovideo.
                    
                    //caso o vídeo seja apenas vídeo e não PLS então será apenas UM arquivo de vídeo.
                    switch (tipo)
                    {
                        case "V01":
                            {
                                // Instancia o video a partir do formato
                                VideoV01 video = new VideoV01();
                                // Carrega através do ARRAY
                                video.LoadFromBytes(dadosVideo);
                                // Adiciona o video    
                                Videos.Add(video);

                            }
                            break;

                        case "V02":
                            {
                                // Instancia o video a partir do formato
                                VideoV02 video = new VideoV02();
                                // Carrega através do ARRAY
                                video.LoadFromBytes(dadosVideo);
                                // Adiciona o video    
                                Videos.Add(video);
                                
                            }
                            break;

                        case "V03":
                            {

                            }
                            break;

                        case "V04":
                            {

                            }
                            break;

                        case "PLS":
                            {
                                //Instancia o PLS.
                                Arquivo_PLS please = new Arquivo_PLS();

                                please.LoadFromBytes(dadosVideo);

                                foreach (Video video in please.listaVideos)
                                {
                                    Videos.Add(video);
                                }

                            }
                            break;

                    }

                    indice += dadosVideo.Length;
                }

            }
   
        }

        public void Salvar(string arquivoNome)
        {
            if (String.IsNullOrEmpty(this.pathVideo))
            {
                throw new FormatException();
            }
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome + Util.Util.sequencial_arquivo_mensagens.ToString("X8") + Util.Util.ARQUIVO_EXT_MPT);
            fs.Write(dados, 0, dados.Length);
            fs.Close();
        }

        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoMensagemPaths* regiao = (FormatoMensagemPaths*)pSrc;

                this.crc = CalcularCRC(dados);
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
        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoMensagemPaths)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoMensagemPaths* mensagemPaths = (FormatoMensagemPaths*)pSrc;

                    mensagemPaths->versao = this.versao;
                    mensagemPaths->reservado = this.reservado;
                    mensagemPaths->idMensagem = this.idMensagem;
                    for (int i = 0; i < 58; i++)
                    {
                        mensagemPaths->reservado2[i] = this.reservado2[i];
                    }
                    mensagemPaths->crc = this.crc;
                    ArrayLDX2.StringToByteArray(mensagemPaths->pathVideo, this.pathVideo, 64);
                    
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
                    FormatoMensagemPaths* mensagemPaths = (FormatoMensagemPaths*) pSrc;

                    this.versao = mensagemPaths->versao;
                    this.reservado = mensagemPaths->reservado;
                    this.idMensagem = mensagemPaths->idMensagem;
                    for (int i = 0; i < 58; i++)
                    {
                        this.reservado2[i] = mensagemPaths->reservado2[i];
                    }
                    this.crc = mensagemPaths->crc;
                    this.pathVideo = ArrayLDX2.ByteArrayToString(mensagemPaths->pathVideo, 64);
                }
            }
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

        private bool VerificarCRC(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMensagemPaths* parametros = (FormatoMensagemPaths*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }
        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            int tamanho;

            Byte[] dadosCRC = new byte[dados.Length - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoMensagemPaths* regiao = (FormatoMensagemPaths*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&regiao->crc - (int)pSrc);
                Array.Copy(dados, ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&regiao->crc - (int)pSrc,
                           dados.Length - ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }
        public void CriarMensagemPathPadrao()
        {
            //this.versao = 1;
            this.reservado = 1;
            this.idMensagem = 0;

            this.reservado2 = new byte[58];

            for (int i = 0; i < 58; i++)
            {
                this.reservado2[i] = 0x00;
            }
            this.crc = 0;
            this.pathVideo = @"video/000.V01";
        }

        public bool VerificarCRC(string caminhoMPT)
        {
            FileStream fs = File.OpenRead(caminhoMPT);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            fs.Close();

            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMensagemPaths* parametros = (FormatoMensagemPaths*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {                 
                resposta = (fs.Length >= sizeof(FormatoMensagemPaths));
            }
            return resposta;
        }
    }
}
