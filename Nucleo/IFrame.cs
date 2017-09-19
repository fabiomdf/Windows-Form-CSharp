using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucleo
{
    public interface IFrame
    {
        void Play();

        void Stop();

        void Clear();

        void Salvar(String arquivoNome);

        IFrame Abrir(String arquivoNome);
        
    }

}
