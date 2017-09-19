using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Memorias
{
    public class DirChunk
    {
        private byte[] nameBytes = new byte[8];

        public readonly int tamanho;

        public DirChunk()
        {
            unsafe
            {
                tamanho = sizeof(FormatoDirChunk);   
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

        protected unsafe struct FormatoDirChunk
        {
            public fixed Byte name[8];
        }
        public Byte[] toBytes()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoDirChunk)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoDirChunk* formatoDirChunk = (FormatoDirChunk*)pSrc;
                    for (int i = 0; i < nameBytes.Length; i++)
                    {
                        formatoDirChunk->name[i] = this.nameBytes[i];
                    }
                }

                return resultado;
            }
        }
        public void LoadFromBytes(byte[] dados)
        {
            //unsafe
            //{
            //    fixed (byte* pSrc = dados)
            //    {
            //        DirChunk.FormatoDirChunk* formatoDirChunk = (DirChunk.FormatoDirChunk*)pSrc;

            //        this.nameBytes = formatoDirChunk->name;
            //    }
            //}
        }
    }
}
