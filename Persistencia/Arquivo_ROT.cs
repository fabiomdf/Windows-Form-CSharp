using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Erros;
using Util;

namespace Persistencia
{
    public class Arquivo_ROT //: IArquivo
    {
        public unsafe struct FormatoRoteiros
        {
            public Byte versao;
            public Byte reservado;
            public UInt16 id;
            public UInt32 tarifa; // valor da tarifa multiplicado por 100 ex:11,22 é salvo como 1122 
            public fixed Byte labelNumero[20];
            public fixed Byte labelRoteiro[16];
            public fixed Byte labelRoteiroVolta[16];
            public fixed Byte reservado2[2];
            public UInt16 crc;
        }
        
        //public Byte versao { get; set; }
        private Byte versao = 1;
        public Byte reservado { get; set; }
        public UInt16 id { get; set; }
        public UInt32 tarifa { get; set; } // valor da tarifa multiplicado por 100 ex:11,22 é salvo como 1122 
        public string labelNumero { get; set; }
        public string labelRoteiro { get; set; }
        public string labelRoteiroVolta { get; set; }
        public Byte[] reservado2 { get; set; }
        public UInt16 crc { get; set; }

        public Arquivo_ROT()
        {
            //versao = 1;
            reservado = 0;
            id = 0;
            tarifa = 0;
            labelNumero = "0";
            labelRoteiro = "Lighdot 12";
            labelRoteiroVolta = "Lighdot 12";
            reservado2 = new byte[14];
            crc = 0;
        }


        public Arquivo_ROT(Arquivo_ROT oldValue)
        {
            this.versao = oldValue.versao;
            this.reservado = oldValue.reservado;
            this.id = oldValue.id;
            this.tarifa = oldValue.tarifa;
            this.labelNumero = oldValue.labelNumero;
            this.labelRoteiro = oldValue.labelRoteiro;
            this.labelRoteiroVolta = oldValue.labelRoteiroVolta;

            this.reservado2 = new byte[oldValue.reservado2.Length];
            oldValue.reservado2.CopyTo(this.reservado2, 0);

            this.crc = oldValue.crc;
        }

        public void Salvar(string arquivoNome)//, object controlador)
        {
            if (String.IsNullOrEmpty(this.labelNumero))
            {
                throw new FormatException();
            }
            if (String.IsNullOrEmpty(this.labelRoteiro))
            {
                throw new FormatException();
            }
            AtualizarCRC();
            byte[] dados = this.toByteArray();

            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);

            fs.Close();

        }

        /*
        public void Salvar(string diretorio, ControladorPontos.Controlador Ctrdl)
        {
            //for(int painel = 0; painel < Ctrdl.QuantidadePaineis; painel++)
            int painel = 0;
            {
                for(int roteiro = 0; roteiro < Ctrdl.Paineis[painel].Roteiros.Count; roteiro++)
                {
                    labelNumero = Ctrdl.Paineis[painel].Roteiros[roteiro].LabelNumero;
                    labelRoteiro = Ctrdl.Paineis[painel].Roteiros[roteiro].LabelRoteiroIda;
                    tarifa = (uint)Ctrdl.Paineis[painel].Roteiros[roteiro].Tarifa;
                   
                    Salvar(diretorio + @"\" + roteiro.ToString("d3") + ".rot");
                }
            }
        }
        */
        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoRoteiros* regiao = (FormatoRoteiros*)pSrc;

                this.crc = CalcularCRC(dados);
            }
        }


        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoRoteiros)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoRoteiros* roteiros = (FormatoRoteiros*)pSrc;

                    roteiros->versao = this.versao;
                    roteiros->reservado = this.reservado;
                    roteiros->id = this.id;
                    roteiros->tarifa = this.tarifa; // valor da tarifa multiplicado por 100 ex:11,22 é salvo como 1122 

                    ArrayLDX2.StringToByteArray(roteiros->labelNumero, this.labelNumero, 20);
                    ArrayLDX2.StringToByteArray(roteiros->labelRoteiro, this.labelRoteiro, 16);
                    ArrayLDX2.StringToByteArray(roteiros->labelRoteiroVolta, this.labelRoteiroVolta, 16);

                    for (int i = 0; i < 2; i++)
                    {
                        roteiros->reservado2[i] = this.reservado2[i];
                    }

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
                    FormatoRoteiros* roteiros = (FormatoRoteiros*)pSrc;

                    this.versao = roteiros->versao;
                    this.reservado = roteiros->reservado;
                    this.id = roteiros->id;
                    this.tarifa = roteiros->tarifa; // valor da tarifa multiplicado por 100 ex:11,22 é salvo como 1122 

                    this.labelNumero = ArrayLDX2.ByteArrayToString(roteiros->labelNumero, 20);
                    this.labelRoteiro = ArrayLDX2.ByteArrayToString(roteiros->labelRoteiro, 16);
                    this.labelRoteiroVolta = ArrayLDX2.ByteArrayToString(roteiros->labelRoteiroVolta, 16);

                    for (int i = 0; i < 2; i++)
                    {
                        this.reservado2[i] = roteiros->reservado2[i];
                    }

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

            Byte[] dadosCRC = new byte[sizeof(FormatoRoteiros) - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoRoteiros* regiao = (FormatoRoteiros*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&regiao->crc - (int)pSrc);
                Array.Copy(dados, ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&regiao->crc - (int)pSrc,
                           sizeof(FormatoRoteiros) - ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }
        public void CriarRoteirosPadrao()
        {
            //this.versao = 1;
            this.reservado = 0;
            this.id = 0;

            this.tarifa = 1111; // valor da tarifa multiplicado por 100 ex:11,22 é salvo como 1122 

            this.labelNumero = "00";
            this.labelRoteiro = "Pontos X2";
            this.labelRoteiroVolta = "Pontos X2";
            reservado2 = new byte[14];
            for (int i = 0; i < 14; i++)
            {
                this.reservado2[i] = 0X00;
            }
            this.crc = 0;
        }


        public bool VerificarCRC(Byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoRoteiros* parametros = (FormatoRoteiros*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length == sizeof(FormatoRoteiros));
            }
            return resposta;
        }
    }
}
