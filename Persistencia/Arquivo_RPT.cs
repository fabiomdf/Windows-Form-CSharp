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
    public class Arquivo_RPT
    {
        public unsafe struct FormatoRoteiroPaths
        {           
            public Byte versao;
            public Byte reservado;
            public UInt16 idRoteiro;
            public fixed Byte reservado2[58];
            public UInt16 crc;

            public fixed byte pathExibicaoIda[64];
            public fixed byte pathExibicaoVolta[64];
            public fixed byte pathNumeroRoteiro[64];
        }

        //public Byte versao { get; set; }
        private Byte versao = 1;
        public Byte reservado { get; set; }
        public UInt16 idRoteiro { get; set; }
        public Byte[] reservado2 { get; set; }
        public UInt16 crc { get; set; }

        public string pathExibicaoIda { get; set; }
        public string pathExibicaoVolta { get; set; }
        public string pathNumeroRoteiro { get; set; }

        
        public List<Video> lVideosIda = new List<Video>();
        public List<Video> lVideosVolta = new List<Video>();
        public List<Video> lVideosNumero = new List<Video>();

        public Arquivo_RPT()
        {
            //versao = 1;
            reservado = 0;
            idRoteiro = 0;
            reservado2 = new byte[58];
            crc = 0;
        }

        public Arquivo_RPT(Arquivo_RPT oldValue)
        {
            this.versao = oldValue.versao;
            this.reservado = oldValue.reservado;
            this.idRoteiro = oldValue.idRoteiro;

            this.reservado2 = new byte[oldValue.reservado2.Length];
            oldValue.reservado2.CopyTo(this.reservado2, 0);

            this.crc = oldValue.crc;

            this.pathExibicaoIda = oldValue.pathExibicaoIda;
            this.pathExibicaoVolta = oldValue.pathExibicaoVolta;
            this.pathNumeroRoteiro = oldValue.pathNumeroRoteiro;

            this.lVideosIda = new List<Video>();
            this.lVideosVolta = new List<Video>();
            this.lVideosNumero = new List<Video>();

            this.lVideosIda = UtilPersistencia.GravaVideo(lVideosIda, oldValue.lVideosIda);
            this.lVideosVolta = UtilPersistencia.GravaVideo(lVideosVolta, oldValue.lVideosVolta);
            this.lVideosNumero = UtilPersistencia.GravaVideo(lVideosNumero, oldValue.lVideosNumero);

        }

        public void GerarFormatoNovo(string arquivoNome, string diretorioRaiz)
        {
            UInt32 offsetIda = 0;
            UInt32 offsetVolta = 0;
            UInt32 offsetNumero = 0;
            FileStream fs = File.OpenRead(arquivoNome + Util.Util.sequencial_arquivo_roteiros.ToString("X8") + Util.Util.ARQUIVO_EXT_RPT);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            fs.Close();

            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoRoteiroPaths* roteiroPaths = (FormatoRoteiroPaths*)pSrc;

                    this.versao = roteiroPaths->versao;
                    this.reservado = roteiroPaths->reservado;
                    this.idRoteiro = roteiroPaths->idRoteiro;
                    for (int i = 0; i < 58; i++)
                    {
                        this.reservado2[i] = roteiroPaths->reservado2[i];
                    }
                    this.crc = roteiroPaths->crc;

                    this.pathExibicaoIda = Util.Util.TrataDiretorio(ArrayLDX2.ByteArrayToString(roteiroPaths->pathExibicaoIda, 64));
                    this.pathExibicaoVolta = Util.Util.TrataDiretorio(ArrayLDX2.ByteArrayToString(roteiroPaths->pathExibicaoVolta, 64));
                    this.pathNumeroRoteiro = Util.Util.TrataDiretorio(ArrayLDX2.ByteArrayToString(roteiroPaths->pathNumeroRoteiro, 64));
                }

                if (this.pathExibicaoIda.Contains(":"))
                {
                    fs = File.OpenRead(this.pathExibicaoIda);
                }
                else
                {
                    fs = File.OpenRead(diretorioRaiz + this.pathExibicaoIda);
                }
                
                byte[] dadosVideoIda = new byte[(int)fs.Length];
                fs.Read(dadosVideoIda, 0, dadosVideoIda.Length);
                fs.Close();

                offsetIda = 64;
                Array.Resize(ref dados, (dados.Length - (3 * 64)) + dadosVideoIda.Length);
                Array.Copy(dadosVideoIda, 0, dados, offsetIda, dadosVideoIda.Length);

                if (this.pathExibicaoVolta.Contains(":"))
                {
                    fs = File.OpenRead(this.pathExibicaoVolta);
                }
                else
                {
                    fs = File.OpenRead(diretorioRaiz + this.pathExibicaoVolta);
                }
                
                byte[] dadosVideoVolta = new byte[(int)fs.Length];
                fs.Read(dadosVideoVolta, 0, dadosVideoVolta.Length);
                fs.Close();

                offsetVolta = (UInt32)(offsetIda + dadosVideoIda.Length);
                Array.Resize(ref dados, dados.Length + dadosVideoVolta.Length);
                Array.Copy(dadosVideoVolta, 0, dados, offsetVolta, dadosVideoVolta.Length);

                if (this.pathNumeroRoteiro.Contains(":"))
                {
                    fs = File.OpenRead(this.pathNumeroRoteiro);
                }
                else
                {
                    fs = File.OpenRead(diretorioRaiz + this.pathNumeroRoteiro);
                }
                byte[] dadosVideoNumero = new byte[(int)fs.Length];
                fs.Read(dadosVideoNumero, 0, dadosVideoNumero.Length);
                fs.Close();

                offsetNumero = (UInt32)(offsetVolta + dadosVideoVolta.Length);
                Array.Resize(ref dados, dados.Length + dadosVideoNumero.Length);
                Array.Copy(dadosVideoNumero, 0, dados, offsetNumero, dadosVideoNumero.Length);

                BitConverter.GetBytes(offsetIda).CopyTo(dados, 4);
                BitConverter.GetBytes(offsetVolta).CopyTo(dados, 8);
                BitConverter.GetBytes(offsetNumero).CopyTo(dados, 12);

                fixed (byte* pSrc = dados)
                {
                    FormatoRoteiroPaths* roteiroPaths = (FormatoRoteiroPaths*)pSrc;

                    roteiroPaths->crc = CalcularCRC(dados);

                    BitConverter.GetBytes(roteiroPaths->crc).CopyTo(dados, Util.Util.CRCPosition1);
                }

                fs = File.Create(arquivoNome + Util.Util.sequencial_arquivo_roteiros.ToString("X8") + Util.Util.ARQUIVO_EXT_RPT);
                fs.Write(dados, 0, dados.Length);
                fs.Close();
            }

        }
        public void Salvar(string arquivoNome)
        {
         
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome + Util.Util.sequencial_arquivo_roteiros.ToString("X8") + Util.Util.ARQUIVO_EXT_RPT);
            fs.Write(dados, 0, dados.Length);
            fs.Close();

        }

        /*
        public void Salvar(string diretorio_fonte, String diretorio_saida ,ControladorPontos.Controlador Ctrdl)
        {
            List<string> rots = Directory.EnumerateFiles(diretorio_fonte, "*.rot").ToList();

            for (int painel = 0; painel < Ctrdl.QuantidadePaineis; painel++)
            {
                foreach (String s in rots)
                {
                    Arquivo_RPT arpt = new Arquivo_RPT();

                    arpt.CriarRoteirosPathPadrao();

                    //criar um arquivo de vídeo para cada entidade.
                    arpt.pathExibicaoIda = "videos/" + painel.ToString("d3") + ".v01";
                    arpt.pathExibicaoVolta = "videos/" + painel.ToString("d3") + ".v01";
                    arpt.pathNumeroRoteiro = "videos/" + painel.ToString("d3") + ".v01";

                    arpt.Salvar(diretorio_saida + @"\" + painel.ToString("d2") + @"\roteiros\" +
                                s.Substring(s.Length - 7, 3) + ".rpt");
                }
            }
        }
        */
        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoRoteiroPaths)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoRoteiroPaths* roteiroPaths = (FormatoRoteiroPaths*)pSrc;

                    roteiroPaths->versao = this.versao;
                    roteiroPaths->reservado = this.reservado;
                    roteiroPaths->idRoteiro = this.idRoteiro;
                    for (int i = 0; i < 58; i++)
                    {
                        roteiroPaths->reservado2[i] = this.reservado2[i];
                    }
                    roteiroPaths->crc = this.crc;
                    ArrayLDX2.StringToByteArray(roteiroPaths->pathExibicaoIda, this.pathExibicaoIda, 64);
                    ArrayLDX2.StringToByteArray(roteiroPaths->pathExibicaoVolta, this.pathExibicaoVolta, 64);
                    ArrayLDX2.StringToByteArray(roteiroPaths->pathNumeroRoteiro, this.pathNumeroRoteiro, 64);

                    return resultado;
                }
            }
        }
        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoRoteiroPaths* roteiroPath = (FormatoRoteiroPaths*)pSrc;

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
        private void FromBytesToFormatoPainelCfg(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoRoteiroPaths* roteiroPaths = (FormatoRoteiroPaths*)pSrc;

                    this.versao = roteiroPaths->versao;
                    this.reservado = roteiroPaths->reservado;
                    this.idRoteiro = roteiroPaths->idRoteiro;

                    for (int i = 0; i < 58; i++)
                    {
                        this.reservado2[i] = roteiroPaths->reservado2[i];
                    }
                    this.crc = roteiroPaths->crc;

                    this.pathExibicaoIda = ArrayLDX2.ByteArrayToString(roteiroPaths->pathExibicaoIda, 64);
                    this.pathExibicaoVolta = ArrayLDX2.ByteArrayToString(roteiroPaths->pathExibicaoVolta, 64);
                    this.pathNumeroRoteiro = ArrayLDX2.ByteArrayToString(roteiroPaths->pathNumeroRoteiro, 64);
                }
            }
        }


        /// <summary>
        /// Efetua a leitura dos arquivos de vídeos levando em consideração o formato novo.
        /// </summary>
        /// <param name="arquivoNome">Você já sabe do que se trata.</param>
        /// <returns></returns>
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

                    FromBytesToFormatoRPT(dados);
                }
            }
        }

        private void FromBytesToFormatoRPT(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    Arquivo_RPT.FormatoRoteiroPaths* roteiroPaths = (Arquivo_RPT.FormatoRoteiroPaths*)pSrc;

                    this.versao = roteiroPaths->versao;
                    this.reservado = roteiroPaths->reservado;
                    this.idRoteiro = roteiroPaths->idRoteiro;

                    for (int i = 0; i < 58; i++)
                    {
                        this.reservado2[i] = roteiroPaths->reservado2[i];
                    }

                    this.crc = roteiroPaths->crc;

                    //limpa a lista de videos
                    this.lVideosIda.Clear();
                    this.lVideosVolta.Clear();
                    this.lVideosNumero.Clear();

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

                    CarregaVideos(tipo, lVideosIda, indice, dadosVideo);
                    indice += dadosVideo.Length;
                    indice = indice + sizeof(uint);
                    indice = indice + 1;

                    CarregaVideos(tipo, lVideosVolta, indice, dadosVideo);
                    indice += dadosVideo.Length;
                    indice = indice + sizeof(uint);
                    indice = indice + 1;

                    CarregaVideos(tipo, lVideosNumero, indice, dadosVideo);
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

            Byte[] dadosCRC = new byte[dados.Length - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoRoteiroPaths* roteiros = (FormatoRoteiroPaths*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&roteiros->crc - (int)pSrc);
                Array.Copy(dados,
                          ((int)&roteiros->crc - (int)pSrc + sizeof(UInt16)),
                          dadosCRC,
                           (int)&roteiros->crc - (int)pSrc,
                           dados.Length - ((int)&roteiros->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }
        public void CriarRoteirosPathPadrao()
        {
            //this.versao = 1;
            this.reservado = 0;
            this.idRoteiro = 0;
            this.reservado2 = new byte[58];
            for (int i = 0; i < 58; i++)
            {
                this.reservado2[i] = 0x00;
            }
            this.crc = 0;

            this.pathExibicaoIda = String.Empty;
            this.pathExibicaoVolta = String.Empty;
            this.pathNumeroRoteiro = String.Empty;
        }


        public bool VerificarCRC(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoRoteiroPaths* roteiros = (FormatoRoteiroPaths*)pSrc;
                    return (roteiros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoRoteiroPaths));
            }
            return resposta;
        }
    }
}
