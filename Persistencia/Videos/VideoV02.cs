using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Persistencia.Videos
{
    [Serializable]
    public class VideoV02 : Video
    {
        //indice do video numa lista de IVideos.
        public int Indice { get { return this._indice; } set { this._indice = value; } }

        internal unsafe struct FormatoVideo02
        {
            public UInt32 Tamanho; // Tamanho do Formato de Video + PixelBytes.Length - sizeof(Tamanho)
            public byte versao;           //0
            public fixed byte formato[3]; //3

            public byte animacao;          //4  //animação no código do barbudo.
            public byte alinhamento;      //5
            public fixed byte reservado1[2]; //7 //8 bytes até aqui.

            public UInt32 intervaloAnimacao;       //12      //intervaloAnimacao no código do barbudo.
            public UInt32 tempoApresentacao;  //16
            public UInt32 altura;             //20
            public UInt32 largura;            //24
            public byte* pixelBytes;
        }

        public Byte versao;
        // Formato "V02"
        public Byte[] Formato;

        public Byte animacao;
        public Byte alinhamento;
        public Byte[] reservado1;

        public UInt32 tempoRolagem;
        public UInt32 tempoApresentacao;
        public UInt32 Altura;
        public UInt32 Largura;
        public byte[] reservado2;
        public byte[] pixelBytes;
        public int _indice;
        public UInt32 tamanho;

        public VideoV02()
        {
            versao = 1;
            this.Formato = new byte[3] { (byte)'v', (byte)'0', (byte)'2' };
            animacao = 0;
            alinhamento = (byte) Util.Util.AlinhamentoHorizontal.Centralizado;
            this.reservado1 = new byte[2] { 0, 0 };
            //inverteLeds = 0;
            tempoRolagem = 10;
            tempoApresentacao = 3;
            Altura = 8;
            Largura = 128;
            this.reservado2 = new byte[38];
            this.pixelBytes = new byte[0];

            //==
            this._indice = -1;
        }

        //construtor de cópia produnda (deep copy) para v02
        public VideoV02(VideoV02 oldValue)
        {

            this.versao = oldValue.versao;

            this.Formato = new byte[oldValue.Formato.Length];
            oldValue.Formato.CopyTo(this.Formato, 0);

            this.animacao = oldValue.animacao;
            this.alinhamento = oldValue.alinhamento;

            this.reservado1 = new byte[oldValue.reservado1.Length];
            oldValue.reservado1.CopyTo(this.reservado1, 0);

            this.tempoRolagem = oldValue.tempoRolagem;
            this.tempoApresentacao = oldValue.tempoApresentacao;
            this.Altura = oldValue.Altura;
            this.Largura = oldValue.Largura;
            
            this.reservado2 = new byte[oldValue.reservado2.Length];
            oldValue.reservado2.CopyTo(this.reservado2, 0);

            this.pixelBytes = new byte[oldValue.pixelBytes.Length];
            oldValue.pixelBytes.CopyTo(this.pixelBytes, 0);

            this._indice = oldValue.Indice;
            this.tamanho = oldValue.tamanho;

            //tentativa do uso do reflection para construtores de cópia.
            //FieldInfo[] fields_of_class = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            
            //foreach(FieldInfo fi in fields_of_class)
            //{
            //    fi.SetValue(this, fi.GetValue(oldValue));
            //}

            // //public MyClass( MyClass rhs )
            // //{
            // //// get all the fields in the class
            // //FieldInfo[] fields_of_class = this.GetType().GetFields( 
            // //BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );

            // //// copy each value over to 'this'
            // //foreach( FieldInfo fi in fields_of_class )
            // //{
            // //fi.SetValue( this, fi.GetValue( rhs ) );
            // //}
            // //}
            // //       
        }


        public void Salvar(string arquivoNome)
        {
            Salvar(arquivoNome, false);

        }
        public void Salvar(string arquivoNome, bool ForcarNomenclatura = false)
        {
            //coleta o cabeçalho do arquivo.
            byte[] dados = this.ToBytes();

            string arquivo = (ForcarNomenclatura) ? arquivoNome : arquivoNome + Util.Util.sequencial_arquivo_videos.ToString("X8");

            FileStream fs = File.Create(arquivo + Util.Util.ARQUIVO_EXT_V02);

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
            Util.Util.SetUltimaExtensao(Util.Util.TipoVideo.V02);
        }

        private void BuildBinaryFile(Byte[] dados, String pathToFile)
        {
            if (!pathToFile.EndsWith(Util.Util.ARQUIVO_EXT_V02))
            {
                pathToFile = pathToFile + Util.Util.ARQUIVO_EXT_V02;
            }
            FileStream fs = new FileStream(pathToFile, FileMode.Create);

            fs.Write(dados, 0, dados.Length);
            fs.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivoNome"></param>o
        /// <returns></returns>
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

        public bool VerificarIntegridade(string arquivoNome)
        {
            // TODO: Implementar a verificação do V02;
            return true;
        }


        public bool VerificarTamanhoArquivo(string arquivoNome)
        {
            bool resposta = false;
            unsafe
            {
                FileStream fs = File.OpenRead(arquivoNome);
                resposta = (fs.Length >= sizeof(FormatoVideo02));
                fs.Close();
            }
            return resposta;
        }

        public bool VerificarRelacaoTamanhoArquivoImagem(string arquivoNome)
        {
            long tamanhoArquivo = 0;

            FileStream fs = File.OpenRead(arquivoNome);
            tamanhoArquivo = (int)fs.Length;
            fs.Close();
            this.Abrir(arquivoNome);

            long qntBytesImagem = this.Altura * this.Largura;
            qntBytesImagem = (qntBytesImagem / 8) + (qntBytesImagem % 8 == 0 ? 0 : 1);

            //Verifica se o tamanho do arquivo está de acordo com o tamanho da imagem
            unsafe
            {
                if (this.Altura < 5)
                {
                    return false;
                }
                if (this.Largura < 8)
                {
                    return false;
                }
                if (sizeof(FormatoVideo02) + qntBytesImagem != tamanhoArquivo)
                {
                    return false;
                }
            }

            return true;
        }

        public byte[] ToBytes()
        {
            long qntBytesImagem = this.Altura * this.Largura;
            qntBytesImagem = (qntBytesImagem / 8) + (qntBytesImagem % 8 == 0 ? 0 : 1);

            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoVideo02) + qntBytesImagem - sizeof(uint)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoVideo02* video = (FormatoVideo02*)pSrc;

                    video->versao = Convert.ToByte(this.versao);
                    for (int i = 0; i < 3; i++)
                    {
                        video->formato[i] = this.Formato[i];
                    }
                    video->animacao = Convert.ToByte(this.animacao);
                    video->alinhamento = Convert.ToByte(this.alinhamento);
                    video->largura = this.Largura;
                    video->altura = this.Altura;
                    video->intervaloAnimacao = (UInt16)this.tempoRolagem;
                    video->tempoApresentacao = (UInt16)this.tempoApresentacao;

                    //código original. todo: a testar.
                    //video->Tamanho = (uint)(resultado.Length + this.pixelBytes.Length);
                    video->Tamanho = (uint)(resultado.Length);

                    byte* temp = (byte*)&video->pixelBytes;

                    for (int i = 0; i < this.pixelBytes.Length; i++)
                    {
                        temp[i] = (byte)this.pixelBytes[i];
                    }
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
                    tamanho = (UInt32)dados.Length;
                    FormatoVideo02* vid = (FormatoVideo02*)pSrc;

                    this.versao = vid->versao;

                    this.Formato = new byte[3];

                    for (int i = 0; i < 3; i++)
                        Formato[i] = vid->formato[i];


                    this.animacao = vid->animacao;
                    this.alinhamento = vid->alinhamento;
                    this.tempoRolagem = vid->intervaloAnimacao;
                    this.tempoApresentacao = vid->tempoApresentacao;
                    this.Largura = vid->largura;
                    this.Altura = vid->altura;
                    this.pixelBytes = new byte[dados.Length - sizeof(FormatoVideo02) + sizeof(UInt32)];

                    byte* temp = (byte*)&vid->pixelBytes;

                    for (int i = 0; i < this.pixelBytes.Length; i++)
                    {
                        this.pixelBytes[i] = temp[i];
                    }
                }
            }
        }
    }
}

