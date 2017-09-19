using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Memorias
{
    public class NandFlash        
    {
        // Constantes relacionadas a memória que será utilizada MT29F

        #region Constantes

        private const uint TAMANHO_PAGINA = 2048;
        private const uint TAMANHO_CI = 64;
        private const uint PAGINAS_POR_BLOCO = 64;
        private const uint NUMERO_BLOCOS = 2048;
        private const long TAMANHO_TOTAL = (TAMANHO_PAGINA + TAMANHO_CI) * PAGINAS_POR_BLOCO * NUMERO_BLOCOS;

        private string nomeArquivoDestino = "teste.dat";
        
        #endregion Constantes

        FileStream fs;
        private List<byte> dados;

        public void Inicializar()
        {
            
            this.Abrir();
            for (long i = 0; i < TAMANHO_TOTAL; i++)
            {
                dados.Add(0xff);
            }
            this.Fechar();
            
        }
        
        public void Abrir()
        {
            fs = File.Open(nomeArquivoDestino, FileMode.Create);
        }

        public void Fechar()
        {
            fs.Close();
        }

        public uint GetNumBlocks()
        {
            return NUMERO_BLOCOS;
        }

        public uint GetPagesPerBlock()
        {
            return PAGINAS_POR_BLOCO;
        }

        public uint GetPageDataSize()
        {
            return TAMANHO_PAGINA;
        }

        public uint GetPageSpareSize()
        {
            return TAMANHO_CI;
        }

        public byte EraseBlock(uint numeroBloco)
        {
            try
            {
                long endereco = CalcularEnderecoDoBloco(numeroBloco);
                fs.Seek(endereco, SeekOrigin.Begin);

                for (int i = 0; i < TAMANHO_PAGINA; i++)
                {
                    fs.WriteByte(0xff);
                }
                for (int i = 0; i < TAMANHO_CI; i++)
                {
                    fs.WriteByte(0xff);
                }
                return 0;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private long CalcularEnderecoRealSpare(uint address)
        {
            long resposta = CalcularEnderecoRealPage(address) + TAMANHO_CI;

            return resposta;
        }
        private long CalcularEnderecoRealPage(uint address)
        {
            long resposta = ((address/TAMANHO_PAGINA)*(TAMANHO_PAGINA + TAMANHO_CI));
            
            return resposta;
        }
        private long CalcularEnderecoDoBloco(uint blockNumber)
        {
            long resposta = (blockNumber * PAGINAS_POR_BLOCO * (TAMANHO_PAGINA + TAMANHO_CI));

            return resposta;
        }
    }
}
