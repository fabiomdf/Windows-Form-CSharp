using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Persistencia
{
    public class Arquivo_B11:IArquivo
    {
        public override void Salvar(string arquivoNome, object controlador)
        {
            throw new NotImplementedException();
        }

        public override object Abrir(string arquivoNome)
        {
            throw new NotImplementedException();
        }

        protected override bool VerificarIntegridade(string arquivoNome)
        {
            const int posCRC1 = 62;
            const int posCRC2 = 63;

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

            return (CRC16CCITT.Calcular(dadosValidos.ToArray()) == crc16);            
        }
    }
}
