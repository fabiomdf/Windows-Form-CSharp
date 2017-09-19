using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Memorias
{
    public class PaginaNandFlash
    {
        public unsafe struct FormatoPaginaNandFlash
        {
            public fixed Byte dados[2048];
            public fixed Byte ci[64];
        }

        public Byte[] Dados { get; set; }
        public Byte[] CI { get; set; }


        public byte[] ToBytes()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoPaginaNandFlash)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoPaginaNandFlash* pagina = (FormatoPaginaNandFlash*)pSrc;
                   
                    for (int i = 0; i < 2048; i++)
                    {
                        pagina->dados[i] = (byte)this.Dados[i];
                    }


                    for (int i = 0; i < 64; i++)
                    {
                        pagina->ci[i] = (byte)this.CI[i];
                    }
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
                    FormatoPaginaNandFlash* pagina = (FormatoPaginaNandFlash*)pSrc;
                    

                    for (int i = 0; i < 2048; i++)
                    {
                        this.Dados[i] = pagina->dados[i];
                    }


                    for (int i = 0; i < 64; i++)
                    {
                        this.CI[i] =pagina->ci[i];
                    }
                }
            }
        }
        public PaginaNandFlash()
        {
            Dados = new byte[2048];
            CI = new byte[64];
        }
    }
}
