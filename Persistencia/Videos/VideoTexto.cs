using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Persistencia;
using Recursos;
using Persistencia.Videos;

namespace Persistencia.Videos
{
    public class VideoTexto : IVideo //IArquivo, IVideo
    {
        public VideoV01 video;
        
        public String CaminhoFonte
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public VideoTexto()
        {
            this.video = new VideoV01();
        }


        public void Renderizar()
        {
            throw new NotImplementedException();
        }

        public void Salvar(String arquivoNome, object Controlador)
        {
            throw new NotImplementedException();
        }

        public void Salvar(String arquivoNome)
        {
            this.video.Salvar(arquivoNome);
        }
        
        /*
        public VideoV01 AbrirVideo(String nomeArquivo)
        {
            return Abrir(nomeArquivo);
        }
        */
        public int Largura;
        public int Altura;
        public Boolean[,] bitmap;

        /*
        public VideoV01 InstanciarObjetoVideo(String descricao, int alinhamento, int rolamento)
        {
            VideoV01 video = new VideoV01();
            String valor;

            // Configuração de arquivo de vídeo
            video.versao = 1;
            valor = "V01";
            video.Formato = Encoding.ASCII.GetBytes(valor.ToCharArray());

            // 0 - Fixa
            // 1 - Continua
            // 2 - Paginada
            // 3 - Contínua 2
            // 4 - Subindo
            // 5 - Descendo
            video.rolagem = (byte)rolamento;

            // 0 - Direita
            // 1 - Esquerda
            // 2 - Centralizada
            video.alinhamento = (byte)alinhamento;
            video.otimizacao = 0;
            video.inverteLeds = 0;

            /// Tempo em milisegundos
            video.tempoRolagem = 26;
            video.tempoApresentacao = 1000;
            video.tamanhoString = (UInt16)descricao.Length;
            video.reservado2 = new byte[42];

            /// CRC
            //video.crc = CRC16CCITT.Calcular(video.ToArrayBytes());
            //video.AtualizarCRC(caminhoArquivo);
            video.texto = descricao;

            return video;
        }
         */
        /*
        public VideoV01 CriarObjetoVideoVazio(int tempoApresentacao)
        {
            String descricao = String.Empty;
            VideoV01 video = new VideoV01();
            String valor;

            // Configuração de arquivo de vídeo
            video.versao = 1;
            valor = "V01";
            video.Formato = Encoding.ASCII.GetBytes(valor.ToCharArray());

            // 0 - Fixa
            // 1 - Continua
            // 2 - Paginada
            // 3 - Contínua 2
            // 4 - Subindo
            // 5 - Descendo
            video.rolagem = 0;

            // 0 - Direita
            // 1 - Esquerda
            // 2 - Centralizada
            video.alinhamento = 0;
            video.otimizacao = 0;
            video.inverteLeds = 0;

            /// Tempo em milisegundos
            video.tempoRolagem = 26;
            video.tempoApresentacao = (uint)tempoApresentacao;
            video.tamanhoString = (UInt16)descricao.Length;
            video.reservado2 = new byte[42];

            /// CRC
            //video.crc = CRC16CCITT.Calcular(video.ToArrayBytes());
            //video.AtualizarCRC(caminhoArquivo);
            video.texto = descricao;

            return video;
        }
        */

        private void InserirBit(bool valor)
        {
            bool aux = false;

            for (int indiceColunas = 0; indiceColunas < Largura; indiceColunas++)
            {
                for (int indiceLinhas = 0; indiceLinhas < Altura; indiceLinhas++)
                {
                    if ((indiceLinhas == Altura - 1) && (indiceColunas + 1 < Largura))
                    {
                        bitmap[indiceLinhas, indiceColunas] = bitmap[0, indiceColunas + 1];
                    }
                    else
                    {
                        if ((indiceLinhas + 1 < Altura))
                            bitmap[indiceLinhas, indiceColunas] = bitmap[indiceLinhas + 1, indiceColunas];
                    }

                }
            }

            bitmap[Altura - 1, Largura - 1] = valor;
        }

        /*
        /// <summary>
        /// Método Salvar - Responsável por salvar o arquivo
        /// </summary>
        /// <param name="nomeArquivo">arquivoNome - Este parametro é o caminho do arquivo mais o nome do arquivo.</param>
        /// <remarks> Lembrar de preencher o video antes de mandar salvar</remarks>
        public override void Salvar(string arquivoNome, object controlador)
        {
            VideoV01 videoLocal = this.video;

            if (null != controlador)
            {
                videoLocal = (VideoV01)controlador;
            }

            FileStream fs = File.Create(arquivoNome);
            fs.Write(videoLocal.ToArrayBytes(), 0, videoLocal.ToArrayBytes().Length);
            fs.Close();
            video.AtualizarCRC(arquivoNome);
        }
        */

        public IVideo Abrir(String arquivoNome)
        {
            this.video = this.AbrirVideo01(arquivoNome);

            return this;
        }
        
        public VideoV01 AbrirVideo01(string arquivoNome)
        {
            //if (!VerificarIntegridade(arquivoNome))
            //    throw new NotImplementedException("O arquivo está corrompido!!!");

            video = (VideoV01)video.Abrir(arquivoNome);            
            return video;
        }

        
        protected bool VerificarIntegridade(string arquivoNome)
        {
            /*
            const int posCRC1 = 62;
            const int posCRC2 = 63;

            Byte[] dados = this.video.ToArrayBytes();
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
            ushort crc16 = (ushort)(dados[posCRC1] << 8);
            crc16 += (ushort)(dados[posCRC2]);

            return (CRC16CCITT.Calcular(dadosValidos.ToArray()) == crc16);
             */
            return false;
        }


        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public void LoadFromBytes(byte[] dados)
        {
            throw new NotImplementedException();
        }
    }
}
