using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_OPT : IArquivo
    {
        //  ATUALIZAR_1	00	//Atualizar somente da mesma família e mais novo
        //  ATUALIZAR_2,	01	//Atualizar qualquer versão da mesma família
        //  ATUALIZAR_3,	02	//Atualizar somente do mesmo produto e versão mais nova
        //  ATUALIZAR_4,	03	//Atualizar qualquer do mesmo produto
        //  FORCAR_ATUALIZACAO  04	//Forçar atualização
       
        private byte version = 1;
        private byte option = 0;
        private ushort crc = 0x2e3e;
        public byte Option
        {
            get { return option; }
            set
            {
                if (value > 4)
                {
                    throw new InvalidOptionException("Invalid Option error.");
                }
                option = value;
            }
        }

        public override void Salvar(string arquivoNome, object controlador)
        {
            if (option >4)
            {
                throw new InvalidOptionException("Invalid Option error.");
            }

            Byte[] dados = new byte[2];
            dados[0] = version;
            dados[1] = option;

            crc = Util.CRC16CCITT.Calcular(dados);
            Array.Resize(ref dados, 4);
          
            dados[2] = (byte) crc;
            dados[3] = (byte) (crc >> 8);

            /* Criar arquivo */
            FileStream fs = File.Create(arquivoNome);

            /* Alimentar o arquivo*/
            fs.Write(dados, 0, dados.Length);

            /* Fechar arquivo */
            fs.Close();
        }

        public override object Abrir(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
            {
                throw new CRCFileException("CRC file error.");
            }

            Byte[] dados = new byte[2];
            ushort crcLocal = 0;

            dados = this.ToBytes();
            /* Criar arquivo */
            //FileStream fs = File.OpenRead(arquivoNome);

            //fs.Read(dados, 0, 2);

            ///* Fechar arquivo */
            //fs.Close();

            version = dados[0];
            option = dados[1];            
            crc = BitConverter.ToUInt16(dados, 2);

            return option;
        }

        protected override bool VerificarIntegridade(string arquivoNome)
        {
            const int posCRC1 = 2;
            const int posCRC2 = 3;
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
            ushort crc16 = (ushort) (dados[posCRC1] << 8);
            crc16 += (ushort) (dados[posCRC2]);

            return (CRC16CCITT.Calcular(dadosValidos.ToArray()) == crc16);

        }
    }
}
