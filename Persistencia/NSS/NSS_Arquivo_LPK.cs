using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Persistencia.NSS
{
    public class NSS_Arquivo_LPK
    {
        public abstract class Serializable
        {
            public static unsafe void StringToByteArray(Byte* array, string str, int length)
            {
                if (str.Length < length)
                {
                    str = str.PadRight(length, '\0');
                }
                char[] charArray = str.ToCharArray();
                for (int i = 0; i < length; i++)
                {
                    array[i] = (Byte)charArray[i];
                }
            }

            public abstract Byte[] ToByteArray();
        }

        public unsafe struct FormatoIdioma
        {
            public Byte versao;              //1 byte para versão
            public fixed Byte reservado1[3];       //3 bytes reservados
            public fixed Byte nome[16];             //16 bytes para o nome do idioma
            public fixed Byte reservado2[42];         //42 bytes reservados
            public UInt16 crc;              //2 bytes para crc
            public fixed Byte textos[32 * 4];         //32 bytes para cada texto
        }
        
        public Byte versao;              //1 byte para versão

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Byte[] reservado1;       //3 bytes reservados

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public String nome;             //16 bytes para o nome do idioma

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 42)]
        public Byte[] reservado2;         //42 bytes reservados
        public UInt16 crc;              //2 bytes para crc
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public string[] textos;         //32 bytes para cada texto

        //Ordem dos textos 

        //REMOVA_PENDRIVE,        String Remova Pendrive
        //REGISTRANDO_LOG,        String Registrando LOG
        //SEM_SINAL_GPS,          String Sem sinal GPS
        //ROTA_DP                 String Rota: 

        public Byte[] ToArrayBytes()
        {
            Byte[] dados = new byte[192];
            Byte[] dadosTemp;

            // posicao 0 -> versao
            dados[0] = versao;

            // posicao 1-3 -> reservado1

            // posicao 4-20 -> nome
            dadosTemp = Encoding.Default.GetBytes(nome);
            Array.Copy(dadosTemp, 0, dados, 4, dadosTemp.Length);

            // posicao 21-63 -> reservado2
            // posicao 64-65 -> crc
            dadosTemp = BitConverter.GetBytes(crc);
            Array.Copy(dadosTemp, 0, dados, 64, dadosTemp.Length);

            // posicao 66-97   -> texto01 
            dadosTemp = Encoding.Default.GetBytes(textos[0]);
            Array.Copy(dadosTemp, 0, dados, 66, dadosTemp.Length);

            // posicao 98-129  -> texto02
            dadosTemp = Encoding.Default.GetBytes(textos[1]);
            Array.Copy(dadosTemp, 0, dados, 98, dadosTemp.Length);

            // posicao 130-161 -> texto03
            dadosTemp = Encoding.Default.GetBytes(textos[2]);
            Array.Copy(dadosTemp, 0, dados, 130, dadosTemp.Length);

            // posicao 162-193 -> texto04
            dadosTemp = Encoding.Default.GetBytes(textos[3]);
            Array.Copy(dadosTemp, 0, dados, 166, dadosTemp.Length);

            return dados;
        }

        public byte[] ToByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoIdioma)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoIdioma* idioma = (FormatoIdioma*)pSrc;

                    idioma->versao = (Byte)this.versao;
                    idioma->crc = (UInt16)this.crc;

                    Serializable.StringToByteArray(idioma->nome, this.nome, 16);

                    Serializable.StringToByteArray(idioma->textos + (32 * 0), this.textos[0], 32);
                    Serializable.StringToByteArray(idioma->textos + (32 * 1), this.textos[1], 32);
                    Serializable.StringToByteArray(idioma->textos + (32 * 2), this.textos[2], 32);
                    Serializable.StringToByteArray(idioma->textos + (32 * 3), this.textos[3], 32);
                }

                return resultado;
            }
        }


        public void UpdateCRC()
        {
            unsafe
            {                
                Byte[] dados = ToByteArray();
                Byte[] dadosCRC = new byte[sizeof(FormatoIdioma) - sizeof(UInt16)];

                fixed (byte* pSrc = dados)
                {
                    FormatoIdioma* idioma = (FormatoIdioma*)pSrc;

                    Array.Copy(dados, 0, dadosCRC, 0, (int)&idioma->crc - (int)pSrc);
                    Array.Copy(dados, ((int)&idioma->crc - (int)pSrc + sizeof(UInt16)), dadosCRC, (int)&idioma->crc - (int)pSrc,
                               sizeof(FormatoIdioma) - ((int)&idioma->crc - (int)pSrc + sizeof(UInt16)));

                    this.crc = Util.CRC16CCITT.Calcular(dadosCRC);                    
                }
            }
        }

        public void Abrir(string idioma)
        {
            throw new NotImplementedException();
        }
    }
}
