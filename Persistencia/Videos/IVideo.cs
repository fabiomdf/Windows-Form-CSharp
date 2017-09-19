using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia.Videos
{
    public interface IVideo
    {
        byte[] ToBytes();

        void LoadFromBytes(byte[] dados);

        void Salvar(String arquivoNome);

        IVideo Abrir(String arquivoNome);
    }
}