using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Erros;
using Util;

namespace Persistencia
{
    public class Arquivo_DRV
    {        
        public unsafe struct FormatoMotoristas
        {             
            public Byte versao;
            public Byte reservado;
            public UInt16 id;            
            public fixed Byte labelIdentificacao[20];
            public fixed Byte labelNome[20];
            public fixed Byte reservado2[18];
            public UInt16 crc;
        }
        //public Byte versao { get; set; }
        private Byte versao = 1;
        public UInt16 id { get; set; }
        public string labelIdentificacao { get; set; }
        public string labelNome { get; set; }
       
        public UInt16 crc { get; set; }

        public Arquivo_DRV()
        {
            //versao = 1;            
            id = 0;
            labelIdentificacao = "0";
            labelNome = "Lighdot 12";
            crc = 0;
        }


        public Arquivo_DRV(Arquivo_DRV oldValue)
        {
            this.versao = oldValue.versao;            
            this.id = oldValue.id;
            this.labelIdentificacao = oldValue.labelIdentificacao;
            this.labelNome = oldValue.labelNome;

            this.crc = oldValue.crc;
        }

        public void Salvar(string arquivoNome)//, object controlador)
        {
            if (String.IsNullOrEmpty(this.labelIdentificacao))
            {
                throw new FormatException();
            }
            if (String.IsNullOrEmpty(this.labelNome))
            {
                throw new FormatException();
            }
            AtualizarCRC();
            byte[] dados = this.toByteArray();

            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);

            fs.Close();

        }

        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoMotoristas* regiao = (FormatoMotoristas*)pSrc;

                this.crc = CalcularCRC(dados);
            }
        }


        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoMotoristas)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoMotoristas* roteiros = (FormatoMotoristas*)pSrc;

                    roteiros->versao = this.versao;                    
                    roteiros->id = this.id;

                    ArrayLDX2.StringToByteArray(roteiros->labelIdentificacao, this.labelIdentificacao, 20);
                    ArrayLDX2.StringToByteArray(roteiros->labelNome, this.labelNome, 20);
                   
                    roteiros->crc = this.crc;

                    return resultado;
                }
            }
        }


        public object Abrir(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
            {
                unsafe
                {
                    FileStream fs = File.OpenRead(arquivoNome);
                    byte[] dados = new byte[(int)fs.Length];
                    fs.Read(dados, 0, dados.Length);
                    fs.Close();

                    FromBytesToFormatoPainelCfg(dados);
                    return this;
                }
            }
            return null;
        }


        private void FromBytesToFormatoPainelCfg(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMotoristas* roteiros = (FormatoMotoristas*)pSrc;

                    this.versao = roteiros->versao;
                    this.id = roteiros->id;

                    this.labelIdentificacao = ArrayLDX2.ByteArrayToString(roteiros->labelIdentificacao, 20);
                    this.labelNome = ArrayLDX2.ByteArrayToString(roteiros->labelNome, 20);

                    this.crc = roteiros->crc;
                }
            }
        }

        public bool VerificarIntegridade(string arquivoNome)
        {
            bool resposta = false;
            FileStream fs = File.OpenRead(arquivoNome);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            resposta = VerificarCRC(dados);
            if (!resposta)
            {
                throw new CRCFileException("CRC file error.");
            }
            resposta = VerificarTamanhoArquivo(fs);
            if (!resposta)
            {
                throw new SizeFileException("Size file error.");
            }

            fs.Close();
            return resposta;
        }
        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            int tamanho;

            Byte[] dadosCRC = new byte[sizeof(FormatoMotoristas) - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoMotoristas* motorista = (FormatoMotoristas*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&motorista->crc - (int)pSrc);
                Array.Copy(dados, ((int)&motorista->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&motorista->crc - (int)pSrc,
                           sizeof(FormatoMotoristas) - ((int)&motorista->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }
        public void CriarMotoristaPadrao()
        {
            //this.versao = 1;
            this.id = 0;

            this.labelIdentificacao = "00";
            this.labelNome = "Pontos X2";
          
            this.crc = 0;
        }


        public bool VerificarCRC(Byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoMotoristas* motoristas = (FormatoMotoristas*)pSrc;

                    return (motoristas->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length == sizeof(FormatoMotoristas));
            }
            return resposta;
        }

    }
}
