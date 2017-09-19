using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public interface IFrame
    {
        void Play();

        void Stop();

        void Clear();
    }
}
