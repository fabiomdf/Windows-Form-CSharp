using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Erros
{
    public class CRCFileException : FileLoadException
    {
        private string message;

        public CRCFileException(string message)
            : base(message)
        {
            // TODO: Complete member initialization
            this.message = message;
        }
    }
}
