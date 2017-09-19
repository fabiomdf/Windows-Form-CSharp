using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;



namespace Persistencia
{
    public class ItemAlternancia
    {

        public unsafe struct FormatoItemAlternancia
        {
            public byte qntExibicoes;
            public fixed byte exibicoes [7];
            public fixed byte nomeAlternancia [32];
        }
        
        public ItemAlternancia()
        {
            this.exibicoes = new List<TipoExibicao>();
        }

        public ItemAlternancia(ItemAlternancia oldValue)
        {
            this.exibicoes = new List<TipoExibicao>();
            foreach (TipoExibicao te in oldValue.exibicoes)
                this.exibicoes.Add(te);

            this.nomeAlternancia = oldValue.nomeAlternancia;

        }

        public Byte qntExibicoes
        {
            get { return (byte) exibicoes.Count; }
        }

		private List<TipoExibicao> exibicoes; // Lembrar que o COUNT no MÁXIMO = 7

        public List<TipoExibicao> Exibicoes
        {
            get { return exibicoes; }
            set 
            {                 
                if (value.Count > 7)
                {
                    throw new Exception("Não é possível adicionar mais um tipo de exibição!");
                }
                else
                {
                    exibicoes = value;
                }
            }
        }

		private String nomeAlternancia; // Nome LCD Lembrar que LENGTH no MÁXIMO = 2X16 = 32

        public String NomeAlternancia
        {
            get { return nomeAlternancia; }
            set
            {
                if (value.Length > 32)
                {
                    throw new FormatException();
                }
                else
                {
                    nomeAlternancia = value.PadRight(32, ' ');
                    if (String.IsNullOrEmpty(nomeAlternancia))
                        throw new FormatException();
                }
            }
        }


        public byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoItemAlternancia)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoItemAlternancia* fItemAlternancia = (FormatoItemAlternancia*)pSrc;

                    fItemAlternancia->qntExibicoes = this.qntExibicoes;
                    
                    for(int i = 0; i < this.exibicoes.Count; i++)
                    {
                        fItemAlternancia->exibicoes[i] = (byte)this.exibicoes[i];
                    }

                    ArrayLDX2.StringToByteArray(fItemAlternancia->nomeAlternancia, this.NomeAlternancia, 32);

                    return resultado;
                }
            }
        }


        public void fromByteArray(byte[] conteudo)
        {
            unsafe
            {

                fixed (byte* pSrc = conteudo)
                {
                    FormatoItemAlternancia* fItemAlternancia = (FormatoItemAlternancia*)pSrc;

                    for (int i = 0; i < fItemAlternancia->qntExibicoes; i++)
                    {
                        exibicoes.Add((TipoExibicao)fItemAlternancia->exibicoes[i]);
                    }

                    byte[] nome = new byte[32];

                    for (int i = 0; i < 32; i++)
                    {
                        nome[i] = fItemAlternancia->nomeAlternancia[i];
                    }
                    nomeAlternancia = Encoding.ASCII.GetString(nome);
                }
            }
        }       
    }
}
