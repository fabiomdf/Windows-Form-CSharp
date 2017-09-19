using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Videos;
using Persistencia.Erros;
using Util;

namespace Persistencia

{
    public class Arquivo_DPT
    {
        public unsafe struct FormatoMotoristaPaths
        {            
            public Byte versao;
            public Byte reservado;
            public UInt16 idMotorista;
            public UInt32 offsetIdentificacao;
            public UInt32 offsetNome;
            public fixed Byte reservado2[50];
            public UInt16 crc;
            public fixed byte pathExibicaoId[64];
            public fixed byte pathExibicaoNome[64];
            //public Byte* videos;
        }

        public string pathExibicaoID { get; set; }
        public string pathExibicaoNome { get; set; }

        //public Byte versao { get; set; }
        private Byte versao = 1;
        public UInt16 idMotorista { get; set; }
        
        public UInt16 crc { get; set; }
        
        public List<Video> lVideosId = new List<Video>();
        public List<Video> lVideosNome = new List<Video>();

        public Arquivo_DPT()
        {
            //versao = 1;
            idMotorista = 0;
            crc = 0;
        }

        public Arquivo_DPT(Arquivo_DPT oldValue)
        {
            this.versao = oldValue.versao;
            this.idMotorista = oldValue.idMotorista;
            
            this.crc = oldValue.crc;

            this.pathExibicaoID = oldValue.pathExibicaoID;
            this.pathExibicaoNome = oldValue.pathExibicaoNome;
            
            this.lVideosId = new List<Video>();
            this.lVideosNome = new List<Video>();            

            this.lVideosId = UtilPersistencia.GravaVideo(lVideosId, oldValue.lVideosId);
            this.lVideosNome = UtilPersistencia.GravaVideo(lVideosNome, oldValue.lVideosNome);

            this.pathExibicaoID = oldValue.pathExibicaoID;
            this.pathExibicaoNome = oldValue.pathExibicaoNome;             
        }
        public void GerarFormatoNovo(string arquivoNome, string diretorioRaiz)
        {
            UInt32 offsetId = 0;
            UInt32 offsetNome = 0;
            FileStream fs = File.OpenRead(arquivoNome + Util.Util.sequencial_arquivo_motoristas.ToString("X8") + Util.Util.ARQUIVO_EXT_DPT);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            fs.Close();

            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMotoristaPaths* motoristaPaths = (FormatoMotoristaPaths*)pSrc;

                    this.versao = motoristaPaths->versao;                    
                    this.idMotorista = motoristaPaths->idMotorista;
                   
                    this.crc = motoristaPaths->crc;
                    this.pathExibicaoID = Util.Util.TrataDiretorio(ArrayLDX2.ByteArrayToString(motoristaPaths->pathExibicaoId, 64));
                    this.pathExibicaoNome = Util.Util.TrataDiretorio(ArrayLDX2.ByteArrayToString(motoristaPaths->pathExibicaoNome, 64));
                }

                if (this.pathExibicaoID.Contains(":"))
                {
                    fs = File.OpenRead(this.pathExibicaoID);
                }
                else
                {
                    fs = File.OpenRead(diretorioRaiz + this.pathExibicaoID);
                }
                
                byte[] dadosVideoId = new byte[(int)fs.Length];
                fs.Read(dadosVideoId, 0, dadosVideoId.Length);
                fs.Close();

                offsetId = 64;
                Array.Resize(ref dados, (dados.Length - (2 * 64)) + dadosVideoId.Length);
                Array.Copy(dadosVideoId, 0, dados, offsetId, dadosVideoId.Length);

                if (this.pathExibicaoNome.Contains(":"))
                {
                    fs = File.OpenRead(this.pathExibicaoNome);
                }
                else
                {
                    fs = File.OpenRead(diretorioRaiz + this.pathExibicaoNome);
                }
                
                byte[] dadosVideoNome = new byte[(int)fs.Length];
                fs.Read(dadosVideoNome, 0, dadosVideoNome.Length);
                fs.Close();

                offsetNome = (UInt32)(offsetId + dadosVideoId.Length);
                Array.Resize(ref dados, dados.Length + dadosVideoNome.Length);
                Array.Copy(dadosVideoNome, 0, dados, offsetNome, dadosVideoNome.Length);
                                
                BitConverter.GetBytes(offsetId).CopyTo(dados, 4);
                BitConverter.GetBytes(offsetNome).CopyTo(dados, 8);

                fixed (byte* pSrc = dados)
                {
                    FormatoMotoristaPaths* motoristaPaths = (FormatoMotoristaPaths*)pSrc;

                    motoristaPaths->crc = CalcularCRC(dados);

                    BitConverter.GetBytes(motoristaPaths->crc).CopyTo(dados, Util.Util.CRCPosition1);
                }

                fs = File.Create(arquivoNome + Util.Util.sequencial_arquivo_motoristas.ToString("X8") + Util.Util.ARQUIVO_EXT_DPT);
                fs.Write(dados, 0, dados.Length);
                fs.Close();
            }

        }
        
        public void Salvar(string arquivoNome)
        {            
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome + Util.Util.sequencial_arquivo_motoristas.ToString("X8") + Util.Util.ARQUIVO_EXT_DPT);
            fs.Write(dados, 0, dados.Length);
            fs.Close();

        }

        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoMotoristaPaths)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoMotoristaPaths* motoristaPaths = (FormatoMotoristaPaths*)pSrc;

                    motoristaPaths->versao = this.versao;
                    motoristaPaths->idMotorista = this.idMotorista;

                    motoristaPaths->crc = this.crc;

                    ArrayLDX2.StringToByteArray(motoristaPaths->pathExibicaoId, this.pathExibicaoID, 64);
                    ArrayLDX2.StringToByteArray(motoristaPaths->pathExibicaoNome, this.pathExibicaoNome, 64);
                    
                    return resultado;
                }
            }
        }
        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoMotoristaPaths* regiao = (FormatoMotoristaPaths*)pSrc;

                this.crc = CalcularCRC(dados);
            }
        }
       
        public void Abrir(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
            {
                unsafe
                {
                    FileStream fs = File.OpenRead(arquivoNome);
                    byte[] dados = new byte[(int)fs.Length];
                    fs.Read(dados, 0, dados.Length);
                    fs.Close();

                    FromBytesToFormatoDPT(dados);
                }
            }
        }

        /// <summary>
        /// Efetua a leitura dos arquivos de vídeos levando em consideração o formato novo.
        /// </summary>
        /// <param name="arquivoNome">Você já sabe do que se trata.</param>
        /// <returns></returns>
       

        private void FromBytesToFormatoDPT(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    Arquivo_DPT.FormatoMotoristaPaths* motoristaPaths = (Arquivo_DPT.FormatoMotoristaPaths*)pSrc;

                    this.versao = motoristaPaths->versao;
                    this.idMotorista = motoristaPaths->idMotorista;

                    this.crc = motoristaPaths->crc;

                    //limpa a lista de videos
                    this.lVideosId.Clear();
                    this.lVideosNome.Clear();                    

                    //indice para leitura dos dados no arquivo.
                    int indice = sizeof(Arquivo_RPT.FormatoRoteiroPaths) - (3 * 64);

                    //efetua a leitura do tamanho do vídeo.(próximos 4 bytes após o FormatoMensagemPaths)
                    uint TamanhoArquivo = BitConverter.ToUInt32(dados, indice);

                    //indice do inicio do arquivo a ser lido
                    int indiceInicial = indice;

                    //pula o tamanho. (já no conteúdo de vídeo.)
                    indice = indice + sizeof(uint);
                    //pula a versão.
                    indice = indice + 1;

                    //aloca o suficiente pra ler o vídeo.
                    byte[] dadosVideo = new byte[TamanhoArquivo];

                    //copia os dados do arquivo de vídeo com o tamanho e versão.
                    Array.Copy(dados, indiceInicial, dadosVideo, 0, TamanhoArquivo);

                    //verifica o tipo do vídeo(v01 ou v02) ou se é Arquivo PLS..Equals("V01")
                    String tipo = Encoding.ASCII.GetString(dados, indice, 3).ToUpper();

                    CarregaVideos(tipo, lVideosId, indice, dadosVideo);
                    indice += dadosVideo.Length;
                    indice = indice + sizeof(uint);
                    indice = indice + 1;

                    CarregaVideos(tipo, lVideosNome, indice, dadosVideo);
                }
            }
        }       
        
        /// <summary>
        /// Carrega um array de bytes como vídeo.
        /// </summary>
        /// <param name="tipo">tipo do arquivo em string.</param>
        /// <param name="lLista">Lista a receber o arquivo de vídeo.</param>
        /// <param name="indice">índice de começo do vídeo.</param>
        /// <param name="dadosVideo">conjunto de bytes do vídeo.</param>
        private void CarregaVideos(String tipo,
                                   List<Video> Videos,
                                   int indice,
                                   Byte[] dadosVideo)
        {
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


        }
       
        public void CriarMotoristasPathPadrao()
        {
            //this.versao = 1;
            this.crc = 0;
            this.idMotorista = 0;  
            this.pathExibicaoID = String.Empty;
            this.pathExibicaoNome = String.Empty;
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

            Byte[] dadosCRC = new byte[dados.Length - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoMotoristaPaths* motorista = (FormatoMotoristaPaths*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&motorista->crc - (int)pSrc);
                Array.Copy(dados,
                          ((int)&motorista->crc - (int)pSrc + sizeof(UInt16)),
                          dadosCRC,
                           (int)&motorista->crc - (int)pSrc,
                           dados.Length - ((int)&motorista->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }
 
        


        public bool VerificarCRC(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMotoristaPaths* parametros = (FormatoMotoristaPaths*)pSrc;
                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }
        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoMotoristaPaths));
            }
            return resposta;
        }        
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        
        