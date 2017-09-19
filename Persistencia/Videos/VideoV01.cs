using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Util;
using Persistencia.Erros;

namespace Persistencia.Videos
{
    public class VideoV01 : Video 
    {
        //indice do video numa lista de IVideos.
        public int Indice { get; set; }


        internal unsafe struct FormatoVideo01
        {
            public UInt32 Tamanho; // Tamanho do Formato de Video + Texto.Length - sizeof(Tamanho)
            public byte versao;
            public fixed byte formato[3];

            public byte animacao; //animação no código do barbudo.
            public byte alinhamento;
            public byte otimizacao;
            public byte invertido; // Inverte LEDs
            public UInt32 intervaloAnimacao; //intervaloAnimacao no código do barbudo.
            public UInt32 tempoApresentacao;
            public UInt32 tamanhoString;
            public fixed byte fontPath[42];
            public byte* texto;
        }


        public Byte versao;

        // Formato "V01"
        public string Formato;
        public Byte animacao;
        public Byte alinhamento;
        public Byte otimizacao;

        public Byte inverteLeds;
        // 4 Bytes
        public UInt32 tempoRolagem;
        // 4 Bytes
        public UInt32 tempoApresentacao;
        // 4 Bytes
        private UInt32 tamanhoString;

        //64 bytes
        public string fontPath;

        public UInt16 crc;
        //public String texto;
        public UInt32 tamanho;

        private String _texto = string.Empty;

        public String texto
        {
            get { return this._texto; }
            set
            {
                this._texto = value;
                this.tamanhoString = (uint)this.texto.Length;
            }
        }


        public VideoV01()
        {
            this.texto = @"ABCDEFGHIJKLMNOPQRSTUWVXYZ";
            this.versao = 01;
            this.Formato = @"v01";
            this.animacao = (byte)Util.Util.Rolagem.Rolagem_Continua_Esquerda;
            this.alinhamento = (byte)Util.Util.AlinhamentoHorizontal.Esquerda;
            this.otimizacao = 0;
            this.inverteLeds = 1;
            this.tempoRolagem = 26;
            this.tempoApresentacao = 2000;
            this.tamanhoString = (UInt32)texto.Length;
            this.fontPath = String.Empty;
        }

        public VideoV01(VideoV01 oldValue)
        {

            this.Indice = oldValue.Indice;   
            this.versao = oldValue.versao;
            this.Formato = oldValue.Formato;
            this.animacao = oldValue.animacao;
            this.alinhamento = oldValue.alinhamento;
            this.otimizacao = oldValue.otimizacao;
            this.inverteLeds = oldValue.inverteLeds;
            this.tempoRolagem = oldValue.tempoRolagem;
            this.tempoApresentacao = oldValue.tempoApresentacao;
            this.tamanhoString = oldValue.tamanhoString;
            this.fontPath = oldValue.fontPath;
            this.crc = oldValue.crc;
            this.tamanho = oldValue.tamanho;
            this._texto = oldValue._texto;

        }


        private void GravaExtensao()
        {
            Util.Util.SetUltimaExtensao(Util.Util.TipoVideo.V01);
        }


        private void BuildBinaryFile(Byte[] dados, String pathToFile)
        {
            FileStream fs = new FileStream(pathToFile, FileMode.Create);

            fs.Write(dados, 0, dados.Length);
            fs.Close();
        }

        protected bool VerificarIntegridade(string arquivoNome)
        {
            FileStream fs = File.OpenRead(arquivoNome);
            if (!VerificarCRC(fs))
            {
                throw new CRCFileException("CRC file error.");
            }

            if (!VerificarTamanhoArquivo(fs))
            {
                throw new SizeFileException("Size file error.");
            }
            if (!VerificarRelacaoTamanhoArquivoTexto(fs))
            {
                throw new SizeFileException("Size file error.");
            }

            fs.Close();
            return true;
        }

        private bool VerificarCRC(FileStream fs)
        {
            // TODO: Verificar a necessidade da implementação do verificar CRC para os videos
            return true;
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {                
                resposta = (fs.Length >= sizeof(FormatoVideo01));                
            }
            return resposta;
        }

        public bool VerificarRelacaoTamanhoArquivoTexto(FileStream fs)
        {
            long tamanhoArquivo = 0;

            tamanhoArquivo = (int)fs.Length;
            unsafe
            {
                if (sizeof(FormatoVideo01) + this.texto.Length != tamanhoArquivo)
                {
                    return false;
                }
            }

            return true;
        }

        public byte[] ToBytes()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoVideo01) + this.tamanhoString];

                fixed (byte* pSrc = resultado)
                {
                    FormatoVideo01* video = (FormatoVideo01*)pSrc;

                    video->versao = Convert.ToByte(this.versao);
                    Serializable.StringToByteArray(video->formato, this.Formato, 3);
                    video->animacao = Convert.ToByte(this.animacao);
                    video->alinhamento = Convert.ToByte(this.alinhamento);
                    video->otimizacao = Convert.ToByte(this.otimizacao);
                    video->invertido = Convert.ToByte(this.inverteLeds);
                    video->intervaloAnimacao = (UInt16)this.tempoRolagem;
                    video->tempoApresentacao = (UInt16)this.tempoApresentacao;
                    video->tamanhoString = (UInt16)this.tamanhoString;
                    Serializable.StringToByteArray(video->fontPath, this.fontPath, this.fontPath.Length);

                    video->Tamanho = (UInt32)resultado.Length;

                    byte* temp = (byte*) &video->texto;

                    for (int i = 0; i < video->tamanhoString; i++)
                    {
                        temp[i] = (byte)this.texto[i];
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
                    FormatoVideo01* vid = (FormatoVideo01*)pSrc;
                    this.tamanho = (UInt32)dados.Length;
                    this.versao = vid->versao;

                    this.Formato = string.Empty;
                    Byte[] temp = new byte[3];
                    for (int i = 0; i < 3; i++)
                        temp[i] = vid->formato[i];

                    this.Formato = System.Text.Encoding.ASCII.GetString(temp);

                    this.animacao = vid->animacao;
                    this.alinhamento = vid->alinhamento;
                    this.otimizacao = vid->otimizacao;
                    this.inverteLeds = vid->invertido;
                    this.tempoRolagem = vid->intervaloAnimacao;
                    this.tempoApresentacao = vid->tempoApresentacao;
                    this.tamanhoString = vid->tamanhoString;

                    this.fontPath = string.Empty;
                    temp = new byte[42];
                    for (int i = 0; i < 42; i++)
                    {
                        temp[i] = vid->fontPath[i];
                    }

                    this.fontPath = System.Text.Encoding.ASCII.GetString(temp);

                    //copio tamanho string aqui porque quero limpar o texto.
                    //quando se limpa o texto, automaticamente é alterado o tamanho string pelo setter.
                    uint _tamanhoString = this.tamanhoString;
                    
                    this.texto = String.Empty;

                    byte* temp2 = (byte*)&vid->texto;
                    
                    for (int i = 0; i < _tamanhoString; i++)
                    {
                        texto += (char)temp2[i];
                    }
                    
                }
            }
        }

        public void Salvar(string arquivoNome)
        {
            Salvar(arquivoNome, false);
        }
        public void Salvar(string arquivoNome, bool ForcarNomenclatura = false)
        {
            //coleta o cabeçalho do arquivo.
            byte[] dados = this.ToBytes();

            string arquivo = (ForcarNomenclatura)? arquivoNome : arquivoNome + Util.Util.sequencial_arquivo_videos.ToString("X8");

            FileStream fs = File.Create(arquivo + Util.Util.ARQUIVO_EXT_V01);

            if (fs.CanWrite)
                foreach (Byte b in dados)
                {
                    fs.WriteByte(b);
                }

            fs.Close();

            GravaExtensao();            
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





