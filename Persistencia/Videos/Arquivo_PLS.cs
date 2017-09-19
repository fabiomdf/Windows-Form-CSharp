using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Videos;
using Persistencia.Erros;

namespace Persistencia.Videos
{
    public class Arquivo_PLS : Video
    {
        public unsafe struct FormatoPlaylist
        {
            public UInt32 Tamanho; // Tamanho do Formato de Video + Texto.Length - sizeof(Tamanho)
            public Byte versao;
            public fixed Byte formato[3];
            public UInt32 qtdArquivos;
            public byte* videos;
        }

        public Byte versao { get; set; }
        public byte[] formato;
        public UInt32 qtdArquivos { get; set; }

        // listaPaths ira mudar para lista de Vídeos 
        public List<Video> listaVideos { get; set; }
        public List<String> listaPaths { get; set; }
        public string DiretorioRaiz = String.Empty;

        public Arquivo_PLS(string diretorioRaiz)
        {
            this.DiretorioRaiz = diretorioRaiz;
            versao = 1;
            this.formato = new byte[3] { (byte)'P', (byte)'L', (byte)'S' };
            this.qtdArquivos = 0;
            this.listaVideos = new List<Video>();
            this.listaPaths = new List<string>();
        }

        /// <summary>
        /// Construtor sem parâmetro para não ficar parametrizando o maldito diretório.
        /// </summary>
        public Arquivo_PLS()
        {
            versao = 1;
            this.formato = new byte[3] { (byte)'P', (byte)'L', (byte)'S' };
            this.qtdArquivos = 0;
            this.listaVideos = new List<Video>();
            this.listaPaths = new List<string>();
        }


        public Arquivo_PLS(Arquivo_PLS oldValue)
        {
            this.versao = oldValue.versao;

            this.formato = new byte[oldValue.formato.Length];
            oldValue.formato.CopyTo(this.formato, 0);

            this.qtdArquivos = oldValue.qtdArquivos;

            this.listaVideos = new List<Video>();
            foreach(Video iv in oldValue.listaVideos)
            {
                //IVideo ivTemp = iv;

                if (iv is VideoV01)
                    this.listaVideos.Add(new VideoV01((VideoV01)iv));

                if (iv is VideoV02)
                    this.listaVideos.Add(new VideoV02((VideoV02)iv));

                //if (iv is VideoV03)
                //    this.listaVideos.Add(new VideoV03((VideoV03)iv));

                if (iv is VideoV04)
                    this.listaVideos.Add(new VideoV04((VideoV04)iv));

            }

            foreach(String s in oldValue.listaPaths)
            {
                this.listaPaths.Add(s);
            }

        }

        public int QTDArquivos
        {
            get { return this.listaVideos.Count; }
            set { this.qtdArquivos = (uint)value; }
        }


        public void Default()
        {
            versao = 1;
            this.formato = new byte[3] { (byte)'P', (byte)'L', (byte)'S' };
            qtdArquivos = 1;
            this.listaVideos = new List<Video>();
            this.listaPaths = new List<String>();
        }


        private void CarregarListaVideos()
        {
            listaVideos.Clear();
            foreach (string caminho in listaPaths)
            {

                if (caminho.EndsWith(".v01"))
                {
                    VideoV01 video = new VideoV01();

                    if (caminho.Contains(":"))
                    {
                        video.Abrir(caminho);
                    }
                    else
                    {
                        video.Abrir(DiretorioRaiz + Util.Util.TrataDiretorioFWParaWindows(Util.Util.ARQUIVO_SEPARADOR_DIRETORIO + caminho));
                    }
                    listaVideos.Add(video);
                }
                else if (caminho.EndsWith(".v02"))
                {
                    VideoV02 video = new VideoV02();

                    if (caminho.Contains(":"))
                    {
                        //se for mais de um video
                        video.Abrir(caminho);
                    }
                    else
                    {
                        // se for apenas um video
                        video.Abrir(DiretorioRaiz + Util.Util.TrataDiretorioFWParaWindows(Util.Util.ARQUIVO_SEPARADOR_DIRETORIO + caminho));
                    }
                    listaVideos.Add(video);
                }
                else if (caminho.EndsWith(".v04"))
                {
                    VideoV04 video = new VideoV04();

                    if (caminho.Contains(":"))
                    {
                        video.Abrir(caminho);
                    }
                    else
                    {
                        video.Abrir(DiretorioRaiz + Util.Util.TrataDiretorioFWParaWindows(Util.Util.ARQUIVO_SEPARADOR_DIRETORIO + caminho));
                    }
                    listaVideos.Add(video);
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
           // CRC foi removido do PLS, assim como dos arquivos de Video.
           return true;
       }

       public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoPlaylist));
            }
            return resposta;
        }

        public bool VerificarRelacaoTamanhoArquivoLista(string arquivoNome)
        {
            // TODO: Implementar a verificãção do tamnaho do arquivo Lista
            return true;
        }

        public byte[] ToBytes()
        {
            CarregarListaVideos();
            List<byte> dados = new List<byte>();
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoPlaylist) - sizeof(UInt32)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoPlaylist* listaArquivos = (FormatoPlaylist*)pSrc;

                    //paths.CopyTo(resultado, sizeof(FormatoPlaylist));

                    listaArquivos->versao = this.versao;

                    listaArquivos->qtdArquivos = this.qtdArquivos;

                    for (int i = 0; i < this.formato.Count(); i++)
                        listaArquivos->formato[i] = this.formato[i];

                    dados.AddRange(resultado);

                    foreach (IVideo video in listaVideos)
                    {
                        if (video is VideoV01)
                        {
                            dados.AddRange((video as VideoV01).ToBytes());
                        }
                        else if (video is VideoV02)
                        {
                            dados.AddRange((video as VideoV02).ToBytes());
                        }
                        else if (video is VideoV04)
                        {
                            dados.AddRange((video as VideoV04).ToBytes());
                        }
                    }
                    listaArquivos->Tamanho = (UInt32)dados.Count;

                    resultado = dados.ToArray();
                    BitConverter.GetBytes((UInt32)resultado.Length).CopyTo(resultado, 0);
                }

                return resultado;

            }
        }

        public void LoadFromBytes(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoPlaylist* listaArquivos = (FormatoPlaylist*)pSrc;
                    Byte[] temporario = new byte[listaArquivos->qtdArquivos];

                    listaArquivos->Tamanho = (UInt32)dados.Length;

                    this.versao = listaArquivos->versao;

                    this.qtdArquivos = listaArquivos->qtdArquivos;

                    // Limpa a lista
                    this.listaVideos.Clear();
                    int indice = sizeof(FormatoPlaylist)-sizeof(UInt32);
                    for (int i = 0; i < this.qtdArquivos; i++)
                    {
                        int indiceInicial = indice;
                        // Pegar o tamanho do Arquivo
                        uint tamanhoArquivo = BitConverter.ToUInt32(dados, indice);
                        byte[] dadosVideo = new byte[tamanhoArquivo];
                        // Atualiza o indice com o tamanho do arquivo
                        indice += sizeof(uint);
                        // Pular a versão
                        indice++;

                        Array.Copy(dados, indiceInicial, dadosVideo, 0, tamanhoArquivo);


                        // Pegar o formato do Arquivo Ex.: V02
                        if (Encoding.ASCII.GetString(dados, indice, 3).ToUpper().Equals("V01"))
                        {
                            // Instancia o video a partir do formato
                            VideoV01 video = new VideoV01();
                            // Carrega através do ARRAY
                            video.LoadFromBytes(dadosVideo);
                            // Adiciona o video    
                            listaVideos.Add(video);
                        }
                        else if (Encoding.ASCII.GetString(dados, indice, 3).ToUpper().Equals("V02"))
                        {
                            // Instancia o video a partir do formato
                            VideoV02 video = new VideoV02();
                            // Carrega através do ARRAY
                            video.LoadFromBytes(dadosVideo);
                            // Adiciona o video    
                            listaVideos.Add(video);
                        }
                        else if (Encoding.ASCII.GetString(dados, indice, 3).ToUpper().Equals("V04"))
                        {
                            // Instancia o video a partir do formato
                            VideoV04 video = new VideoV04();
                            // Carrega através do ARRAY
                            video.LoadFromBytes(dadosVideo);
                            // Adiciona o video    
                            listaVideos.Add(video);
                        }

                        indice = indice + dadosVideo.Length;
                        indice = indice - sizeof (uint);
                        indice = indice - 1;
                    }

                }
            }
        }

        //public void Salvar(string arquivoNome)
        //{
        //    byte[] dados = this.ToBytes();

        //    FileStream fs = File.Create(arquivoNome + Util.Util.ARQUIVO_EXT_PLS);
        //    fs.Write(dados, 0, dados.Length);
        //    fs.Close();
        //}
        public void Salvar(string arquivoNome)
        {
            Salvar(arquivoNome, false);

        }
        public void Salvar(string arquivoNome, bool ForcarNomenclatura = false)
        {
            //coleta o cabeçalho do arquivo.
            byte[] dados = this.ToBytes();

            string arquivo = (ForcarNomenclatura) ? arquivoNome : arquivoNome + Util.Util.sequencial_arquivo_videos.ToString("X8");

            FileStream fs = File.Create(arquivo + Util.Util.ARQUIVO_EXT_PLS);

            if (fs.CanWrite)
                foreach (Byte b in dados)
                {
                    fs.WriteByte(b);
                }

            fs.Close();

            GravaExtensao();
        }
        private void GravaExtensao()
        {
            Util.Util.SetUltimaExtensao(Util.Util.TipoVideo.PLS);
        }
        public IVideo Abrir(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
            {
                unsafe
                {
                    FileStream fs = File.OpenRead(arquivoNome);
                    byte[] dados = new byte[(int)fs.Length];
                    fs.Read(dados, 0, dados.Length);
                    fs.Close();

                    LoadFromBytes(dados);
                    return this;
                }
            }
            return null;
        }
    }
}