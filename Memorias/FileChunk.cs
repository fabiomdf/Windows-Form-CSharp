using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Memorias
{
    public class FileChunk
    {
        private Byte[] nameBytes = new byte[8];
        private Byte[] extensionBytes = new byte[3];
        public Byte RESERVADO1;
        public UInt32 size;
        public readonly int tamanho;


        public FileChunk()
        {
            unsafe
            {
                tamanho = sizeof(FormatoFileChunk);   
            }             
        }

        public string name
        {
            get { return Encoding.ASCII.GetString(nameBytes); }
            set
            {
                if ((value.Length > 8) || (value.Length == 0))
                    throw new FormatException();

                nameBytes = Encoding.ASCII.GetBytes(value);
            }
        }

        public string extension
        {
            get { return Encoding.ASCII.GetString(extensionBytes); }
            set
            {
                if ((value.Length > 8) || (value.Length == 0))
                    throw new Exception();

                extensionBytes = Encoding.ASCII.GetBytes(value);
            }
        }

        protected unsafe struct FormatoFileChunk
        {
            public fixed Byte name[8];
            public fixed Byte extension[3];
            public Byte RESERVADO1;
            public UInt32 size;
        }

        public Byte[] toBytes()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoFileChunk)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoFileChunk* formatoFileChunk = (FormatoFileChunk*)pSrc;
                    for (int i = 0; i < nameBytes.Length; i++)
                    {
                        formatoFileChunk->name[i] = this.nameBytes[i];
                    }
                    for (int i = 0; i < extensionBytes.Length; i++)
                    {
                        formatoFileChunk->extension[i] = this.extensionBytes[i];
                    }
                    formatoFileChunk->RESERVADO1 = this.RESERVADO1;
                    formatoFileChunk->size = this.size;
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
                    FileChunk.FormatoFileChunk* formatoFileChunk = (FileChunk.FormatoFileChunk*)pSrc;
                    for (int i = 0; i < nameBytes.Length; i++)
                    {
                        this.nameBytes[i] = formatoFileChunk->name[i];
                    }
                    for (int i = 0; i < extensionBytes.Length; i++)
                    {
                        this.extensionBytes[i] = formatoFileChunk->extension[i];
                    }
                    this.RESERVADO1 = formatoFileChunk->RESERVADO1;
                    this.size = formatoFileChunk->size;
                }
            }
        }
    }
}
