using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_FIR : IArquivo
    {
        public class IdentificacaoProduto
        {
            public Version versao;
            public string nomeProduto;
            public string caminhoArquivo;
        }
        public override void Salvar(string arquivoNome, object controlador)
        {
            throw new NotImplementedException();
        }

        public override object Abrir(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
                throw new CRCFileException("CRC file error.");

            Byte[] dados = this.ToBytes();

            return dados;
        }
        public IdentificacaoProduto IdentificarProduto(string arquivoNome)
        {
            IdentificacaoProduto produto = new IdentificacaoProduto();
            this.ArquivoNome = arquivoNome;
            Byte[] dados = this.ToBytes();

            produto.nomeProduto = Encoding.ASCII.GetString(dados, 1024, 20).Replace("\0", String.Empty);
            produto.versao = new Version(dados[1044], dados[1045], dados[1046]);
            produto.caminhoArquivo = arquivoNome;

            return produto;
        }

        protected override bool VerificarIntegridade(string arquivoNome)
        {
            const int posCRC1 = 1047;
            const int posCRC2 = 1048;
            this.ArquivoNome = arquivoNome;
            Byte[] dados = this.ToBytes();
            List<byte> dadosValidos = new List<byte>();
            for (int i = 0; i < dados.Length; i++)
            {
                if ((i == posCRC1) || (i == posCRC2))
                {
                    continue;
                }
                else
                {
                    dadosValidos.Add(dados[i]);
                }
            }
            ushort crc16 = (ushort)(dados[posCRC1] << 8);
            crc16 += (ushort)(dados[posCRC2]);

            return (Util.CRC16CCITT.Calcular(dadosValidos.ToArray()) == crc16);            
        }
    }
}
