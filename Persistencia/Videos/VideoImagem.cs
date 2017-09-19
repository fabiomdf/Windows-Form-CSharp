using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Persistencia.Videos
{
    public class VideoImagem : IVideo
    {
        public VideoV02 video;

        public void Renderizar()
        {
            throw new NotImplementedException();
        }
        public VideoImagem()
        {
 
        }

        public VideoV02 AbrirArquivo(string videoPath)
        {
            video = (VideoV02)video.Abrir(videoPath);            
            return video;
        }

        public void SalvarArquivoVideo(String nomeArquivo)
        {
            FileStream fs = File.Create(nomeArquivo);
            fs.Write(video.ToBytes(), 0, video.ToBytes().Length);
            fs.Close();
        }
        
        public VideoV02 InstanciarObjetoVideo(byte[] pixelBytes, int animacao, int alinhamento, UInt32 intervaloAnimacao, UInt32 tempoApresentacao, UInt32 altura, UInt32 largura)
        {
            VideoV02 video = new VideoV02();
            
            // Configuração de arquivo de vídeo
            video.animacao = (byte)animacao;
            video.alinhamento = (byte)alinhamento;
                
            video.reservado1 = new byte[2];

            video.tempoRolagem = intervaloAnimacao;

            video.tempoApresentacao = tempoApresentacao;
            video.Altura = altura;
            video.Largura = largura;        
        
            video.reservado2 = new byte[38];                
        
            video.pixelBytes = pixelBytes;


            /// CRC
            //video.crc = CRC16CCITT.Calcular(video.ToArrayBytes());
            //video.AtualizarCRC(caminhoArquivo);

            return video;
        }
        
	    public byte readAnimacao()
        {
            throw new NotImplementedException();
        }
	    public byte readAlinhamento()
        {
            throw new NotImplementedException();
        }
	    public UInt32 readIntervaloAnimacao()
        {
            throw new NotImplementedException();
        }
	    public UInt32 readTempoApresentacao()
        {
            throw new NotImplementedException();
        }
    	public UInt32 readAltura()
        {
            throw new NotImplementedException();
        }
    	public UInt32 readLargura()
        {
            throw new NotImplementedException();
        }
        public byte ReadPixelByteAt(UInt32 offset)
        {
            int offSetLocal = (int)(offset / 8);

            if (offset % 8 != 0)
            {
                offSetLocal++;
            }

            return video.pixelBytes[offSetLocal];            
        }
        public byte VerificarConsistencia()
        {
            throw new NotImplementedException();
        }

        private byte rolagem;
        private byte alinhamento;
	    private UInt32 tempoRolagem;
	    private UInt32 tempoApresentacao;
	    private UInt32 altura;
	    private UInt32 largura;
	    private UInt16 crc16;



        public void Salvar(string ArquivoNome)
        {
            throw new NotImplementedException();
        }

        public IVideo Abrir(string ArquivoNome)
        {
            throw new NotImplementedException();
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
