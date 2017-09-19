using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;
using Persistencia.Erros;


namespace Persistencia
{
    public class Arquivo_VAR //: IArquivo    
    {
        private Byte versao = 3;
        public Byte horaSaida;
        public Byte minutosSaida;
        public Byte regiao;
        public bool sentidoIda;
        public UInt16 crc;
        public UInt32 roteiro;
        public Byte baudRate;
        public UInt16 motorista;
        public bool TurnOffUSB;

        public Arquivo_VAR ()
        {
           CarregarProgramacaoPadrao();
        }

        public Arquivo_VAR(Arquivo_VAR oldValue)
        {
            this.versao = oldValue.versao;
            this.horaSaida = oldValue.horaSaida;
            this.minutosSaida = oldValue.minutosSaida;
            this.regiao = oldValue.regiao;
            this.sentidoIda = oldValue.sentidoIda;
            this.crc = oldValue.crc;
            this.roteiro = oldValue.roteiro;
            this.baudRate = oldValue.baudRate;
        }

        private void CarregarProgramacaoPadrao()
        {
           // versao = 1;
            horaSaida = 2;
            minutosSaida = 30;
            regiao = 0;
            sentidoIda = true;
            crc = 0;
            // Verificar se o roteiro abaixo existe
            roteiro = 0;
            motorista = 0;
            TurnOffUSB = false;
            AtualizarCRC();
        }

        public unsafe struct FormatoParametrosVariaveis
        {
            public Byte versao;
            public Byte horaSaida;
            public Byte minutosSaida;
            public Byte regiao;
            public UInt16 motorista;

            public bool TurnOffUSB;
            public fixed Byte reservado [1];           
            
            public bool sentidoIda;
            public fixed Byte reservado2 [53];

            public UInt16 crc;
            public UInt32 roteiro;
        }

        public void Salvar(string arquivoNome)
        {
            AtualizarCRC();
                        
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();
            
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
            if (!Util.Util.ValidarHora(this.horaSaida)) // Hora de Partida fora do intervalo coerente
            {
                throw new FileLoadException("Departure time error. [Hour]");
            }

            if (!Util.Util.ValidarMinuto(this.minutosSaida)) // Minuto de Partida fora do intervalo coerente
            {
                throw new FileLoadException("Departure time error. [Minute]");
            }
            fs.Close();
            return resposta;
        }
        /*
        public void Salvar(string arquivoNome, ControladorPontos.Controlador Ctrdlr)
        {
            versao = 1;
            horaSaida = Ctrdlr.ParametrosVariaveis.HoraSaida;
            minutosSaida = Ctrdlr.ParametrosVariaveis.MinutosSaida;
            regiao = Ctrdlr.ParametrosVariaveis.RegiaoSelecionada;
            sentidoIda = Ctrdlr.ParametrosVariaveis.SentidoIda;
            crc = 0;
            roteiro = Ctrdlr.ParametrosVariaveis.RoteiroSelecionado;

            Salvar(arquivoNome);
        }
        */
        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoParametrosVariaveis* parametros = (FormatoParametrosVariaveis*) pSrc;

                this.crc = CalcularCRC(dados);
            }
        }


        public Byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoParametrosVariaveis)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoParametrosVariaveis* parametros = (FormatoParametrosVariaveis*)pSrc;
                   
                    parametros->versao = (Byte)this.versao;
                    parametros->horaSaida = (Byte)this.horaSaida;
                    parametros->minutosSaida = (Byte)this.minutosSaida;
                    parametros->regiao = (Byte)this.regiao;
                    parametros->sentidoIda = (bool)this.sentidoIda;
                    parametros->roteiro = (UInt32) this.roteiro;
                    parametros->motorista = this.motorista;
                    //parametros->baudRate = this.baudRate;
                    parametros->TurnOffUSB = this.TurnOffUSB;
                    parametros->crc = this.crc;

                }

                return resultado;
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

                    FromBytesToFormatoParametrosVariaveis(dados);
                    return this;
                }
            }
            return null;
        }

        private void FromBytesToFormatoParametrosVariaveis(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    Arquivo_VAR.FormatoParametrosVariaveis* parametros = (Arquivo_VAR.FormatoParametrosVariaveis*)pSrc;

                    this.versao = parametros->versao;
                    this.horaSaida = parametros->horaSaida;
                    this.minutosSaida = parametros->minutosSaida;
                    this.regiao = parametros->regiao;
                    this.sentidoIda = parametros->sentidoIda;
                    this.roteiro = parametros->roteiro;
                    this.motorista = parametros->motorista;
                    //this.baudRate = parametros->baudRate;
                    this.TurnOffUSB = parametros->TurnOffUSB;
                }
            }
        }

        

        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            Byte[] dadosCRC = new byte[sizeof (FormatoParametrosVariaveis) - sizeof (UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoParametrosVariaveis* parametros = (FormatoParametrosVariaveis*) pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int) &parametros->crc - (int) pSrc);
                Array.Copy(dados, ((int) &parametros->crc - (int) pSrc + sizeof (UInt16)), dadosCRC,
                           (int) &parametros->crc - (int) pSrc,
                           sizeof (FormatoParametrosVariaveis) - ((int) &parametros->crc - (int) pSrc + sizeof (UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }

        public bool VerificarCRC(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoParametrosVariaveis* parametros = (FormatoParametrosVariaveis*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length == sizeof(FormatoParametrosVariaveis));
            }
            return resposta;
        }
    }
}
