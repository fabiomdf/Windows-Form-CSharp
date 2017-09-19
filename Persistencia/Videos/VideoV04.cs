using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Persistencia.Videos
{
    public class VideoV04 : Video
    {
        public unsafe struct FormatoFrame
        {
            public UInt32 x;
            public UInt32 y;
            public UInt32 height; // Lembrando que o height e o width definem o espaço para desenho. 
            public UInt32 width;
            public byte* video;
        }

        internal unsafe struct FormatoVideo04
        {
            public UInt32 Tamanho; // Tamanho do Formato de Video + Texto.Length - sizeof(Tamanho)
            public byte versao;
            public fixed byte formato[3];
            public UInt32 qntFrames;
            public FormatoFrame* Frame;
        }

        public Size tamanhoDesenho;
        public System.Drawing.Point posicao;
        public UInt32 Tamanho;
        public byte versao;
        public byte[] formato = new byte[3]{86, 48 ,52};
        public UInt32 qntFrames;
        //public FormatoFrame[] Frame;
        
        public List<IVideo> listaVideos { get; set; }
        //public List<String> listaPaths { get; set; }
        public List<FormatoFrame> listaFrames { get; set; }
        public string DiretorioRaiz = String.Empty;

        public VideoV04(VideoV04 oldValue)
        {
            this.tamanhoDesenho = new Size();
            this.tamanhoDesenho.Height = oldValue.tamanhoDesenho.Height;
            this.tamanhoDesenho.Width = oldValue.tamanhoDesenho.Width;

            this.posicao = new Point();
            this.posicao.X = oldValue.posicao.X;
            this.posicao.Y = oldValue.posicao.Y;

            this.Tamanho = oldValue.Tamanho;
            this.versao = oldValue.versao;
            this.formato = new byte[oldValue.formato.Length];
            oldValue.formato.CopyTo(this.formato, 0);
            this.qntFrames = oldValue.qntFrames;

        }
        public VideoV04()
        {
            this.versao = 1;
            this.posicao = new Point();
            this.posicao.X = 0;
            this.posicao.Y = 0;
            this.listaFrames = new List<FormatoFrame>();
            this.listaVideos = new List<IVideo>();
        }        
        public byte[] ToBytes()
        {
            List<byte> dados = new List<byte>();

            unsafe
            {
                long quantidadeBytes = (long)(sizeof(FormatoVideo04) - sizeof(UInt32));
                quantidadeBytes += (listaFrames.Count * (sizeof(FormatoFrame) - sizeof(UInt32)));
                foreach (VideoV02 video in listaVideos)
                {
                    quantidadeBytes += video.ToBytes().Length;
                }
                Byte[] resultado = new Byte[quantidadeBytes];


                dados.AddRange(BitConverter.GetBytes((uint)quantidadeBytes));
                dados.Add(this.versao);
                //dados.AddRange(BitConverter.GetBytes());
                dados.AddRange(this.formato);
                dados.AddRange(BitConverter.GetBytes((uint)listaFrames.Count));
                

                int indice = (sizeof(FormatoVideo04) - sizeof(UInt32));
                for (int i = 0; i < listaFrames.Count; i++)
                {
                    dados.AddRange(BitConverter.GetBytes(listaFrames[i].x));
                    dados.AddRange(BitConverter.GetBytes(listaFrames[i].y));
                    dados.AddRange(BitConverter.GetBytes(listaFrames[i].height));
                    dados.AddRange(BitConverter.GetBytes(listaFrames[i].width));
                    IVideo video = listaVideos[i];

                    if (video is VideoV01)
                    {
                        dados.AddRange((video as VideoV01).ToBytes());
                    }
                    else if (video is VideoV02)
                    {
                        dados.AddRange((video as VideoV02).ToBytes());
                    }
                    else if (video is VideoV03)
                    {
                        dados.AddRange((video as VideoV03).ToBytes());
                    }
                    else if (video is Arquivo_PLS)
                    {
                        dados.AddRange((video as Arquivo_PLS).ToBytes());
                    }
                }

                resultado = dados.ToArray();
                return resultado;
            }
        }

        public void LoadFromBytes(byte[] dados)
        {
            if (null == this.listaVideos)
            {
                this.listaVideos = new List<IVideo>();
            }
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoVideo04* listaArquivos = (FormatoVideo04*)pSrc;
                    Byte[] temporario = new byte[listaArquivos->qntFrames];

                    listaArquivos->Tamanho = (UInt32)dados.Length;

                    this.versao = listaArquivos->versao;

                    uint qtdFrames = listaArquivos->qntFrames;

                    // Limpa a lista
                    this.listaVideos.Clear();
                    int indice = sizeof(FormatoVideo04) - sizeof(UInt32);
                    for (int i = 0; i < qtdFrames; i++)
                    {
                        FormatoFrame temporaria = new FormatoFrame();

                        temporaria.x = BitConverter.ToUInt32(dados, indice); 
                        indice += sizeof(uint);

                        temporaria.y = BitConverter.ToUInt32(dados, indice);
                        indice += sizeof(uint);

                        temporaria.height = BitConverter.ToUInt32(dados, indice);
                        indice += sizeof(uint);

                        temporaria.width = BitConverter.ToUInt32(dados, indice);
                        indice += sizeof(uint);

                        

                        listaFrames.Add(temporaria);

                        // Pegar o tamanho do Arquivo
                        uint tamanhoArquivo = BitConverter.ToUInt32(dados, indice);
                        byte[] dadosVideo = new byte[tamanhoArquivo];

                        Array.Copy(dados, indice, dadosVideo, 0, tamanhoArquivo);
                        
                        // índice do tamanho
                        indice += sizeof(uint);
                        // índice da versão
                        indice++; 

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
                        else if (Encoding.ASCII.GetString(dados, indice, 3).ToUpper().Equals("PLS"))
                        {
                            // Instancia o video a partir do formato
                            Arquivo_PLS video = new Arquivo_PLS();
                            // Carrega através do ARRAY
                            video.LoadFromBytes(dadosVideo);
                            // Adiciona o video    
                            listaVideos.Add(video);
                        }

                        indice = indice + dadosVideo.Length - (sizeof(uint) + 1/*tamanho da versão*/);
                    }

                }
            }
        }

        //public void Salvar(string arquivoNome)
        //{
        //    byte[] dados = this.ToBytes();

        //    FileStream fs = File.Create(arquivoNome + Util.Util.ARQUIVO_EXT_V04);
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

            FileStream fs = File.Create(arquivo + Util.Util.ARQUIVO_EXT_V04);

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
            Util.Util.SetUltimaExtensao(Util.Util.TipoVideo.V04);
        }

        public IVideo Abrir(string arquivoNome)
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

        public uint qtdFrames
        {
            get
            {
                return (uint)listaFrames.Count;
            }
        }
    }
}
