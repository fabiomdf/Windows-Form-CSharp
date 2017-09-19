using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Persistencia.Erros
{
    public class SizeFileException : FileLoadException
    {
        private string message;

        public SizeFileException(string message)
            : base(message)
        {
            // TODO: Complete member initialization
            this.message = message;
        }

    }
}
