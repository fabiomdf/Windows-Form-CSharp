using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia.Videos
{
    public class VideoV03 : IVideo
    {
        internal unsafe struct FormatoVideo03
        {
            public UInt32 Tamanho;
            public byte versao;
            public fixed byte formato[3];

            public byte alinhamento;
            public fixed byte reservado1[3];
            public UInt32 qntImagens;
            public UInt32 altura;
            public UInt32 largura;

            public UInt32 delayAnimacao;
            public UInt32 tempoApresentacao;

            public byte** bitmaps;
        }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public void LoadFromBytes(byte[] dados)
        {
            throw new NotImplementedException();
        }

        public void Salvar(string arquivoNome)
        {
            throw new NotImplementedException();
        }

        public IVideo Abrir(string arquivoNome)
        {
            throw new NotImplementedException();
        }
    }
}
