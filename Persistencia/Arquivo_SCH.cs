using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Persistencia.Erros;
using Util;

namespace Persistencia
{
    public class Arquivo_SCH
    {
  /*      typedef enum {
                         SELECAO_ROTEIRO,
                         SELECAO_IDA_VOLTA,
                         SELECAO_ALTERNANCIA,
                         SELECAO_MSG_PRINCIPAL,
                         SELECAO_MSG_SECUNDARIA,
                         ALTERACAO_HORA_SAIDA
           } Operacao;
*/

        public unsafe struct Agendamento
        {
            public Byte segundos;
            public Byte minutos;
            public Byte horas;
            public Byte dia;
            public Byte mes;
            public Byte diaSemana;
            public UInt16 ano;
            public Byte mascara;
            public Byte painel;
            public fixed Byte RESERVADO[1];
            public Byte operacao;
            public UInt16 valorParametro;
            public fixed Byte RESERVADO2[2];
        }
        public unsafe struct FormatoAgenda
        {
            public Byte versao;
            public Byte reservado;
            public UInt16 qntItens;
            public fixed Byte RESERVADO[58];
            public UInt16 crc;
            public Agendamento* itens;
        }
        
        //public Byte versao { get; set; }
        private Byte versao = 1;
        public Byte reservado { get; set; }
        public UInt16 qntItens { get; set; }
        public Byte[] RESERVADO { get; set; } // Length = 60
        public UInt16 crc { get; set; }                 
        
        public List<Agendamento> itens = new List<Agendamento>();
             

        public Arquivo_SCH()
        {
            //versao = 1;
            reservado = 0;
            crc = 0;
        }


        public Arquivo_SCH(Arquivo_SCH oldValue)
        {
            this.versao = oldValue.versao;
            this.reservado = oldValue.reservado;
           
            this.crc = oldValue.crc;
        }

        public void Salvar(string arquivoNome)//, object controlador)
        {           
            AtualizarCRC();
            byte[] dados = this.toByteArray();

            FileStream fs = File.Create(arquivoNome + Util.Util.ARQUIVO_AGENDAMENTO);
            fs.Write(dados, 0, dados.Length);

            fs.Close();

        }
        
        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoAgenda* agenda = (FormatoAgenda*)pSrc;

                this.crc = CalcularCRC(dados);
            }
        }


        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoAgenda)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoAgenda* agenda = (FormatoAgenda*)pSrc;

                    agenda->versao = this.versao;
                    agenda->reservado = this.reservado;

                    agenda->qntItens = (UInt16)this.itens.Count;
                    
                    agenda->crc = this.crc;

                    unsafe
                    {
                        Byte[] resultado2 = new Byte[this.itens.Count * sizeof(Agendamento)];
                        
                        Agendamento* temp = (Agendamento*)&agenda->itens;

                        for (int i = 0; i < this.itens.Count; i++)
                        {
                            resultado2[(i* sizeof(Agendamento))] = this.itens[i].segundos;
                            resultado2[(i* sizeof(Agendamento)) + 1] = this.itens[i].minutos;
                            resultado2[(i* sizeof(Agendamento)) + 2] = this.itens[i].horas;
                            resultado2[(i* sizeof(Agendamento)) + 3] = this.itens[i].dia;
                            resultado2[(i* sizeof(Agendamento)) + 4] = this.itens[i].mes;
                            resultado2[(i* sizeof(Agendamento)) + 5] = this.itens[i].diaSemana;                            
                            BitConverter.GetBytes(this.itens[i].ano).CopyTo(resultado2, (i * sizeof(Agendamento)) + 6);
                            resultado2[(i* sizeof(Agendamento)) + 8] = this.itens[i].mascara;
                            resultado2[(i* sizeof(Agendamento)) + 9] = this.itens[i].painel;
                            resultado2[(i* sizeof(Agendamento)) + 10] = 0xff;
                            resultado2[(i* sizeof(Agendamento)) + 11] = this.itens[i].operacao;
                            BitConverter.GetBytes(this.itens[i].valorParametro).CopyTo(resultado2, (i* sizeof(Agendamento)) + 12);
                            resultado2[(i * sizeof(Agendamento)) + 14] = 0xff; // 2 Bytes reservados.
                            resultado2[(i * sizeof(Agendamento)) + 15] = 0xff; // 2 Bytes reservados.
                        }

                        //byte* temp2 = (byte*)&agenda->itens;


                        
                        //for (int i = 0; i<resultado2.Length;i++)
                        //{
                        //    resultado2[i] = temp2[i];
                        //}

                        Array.Resize<byte>(ref resultado, (resultado.Length + resultado2.Length) - sizeof(Int32));
                        resultado2.CopyTo(resultado, 64);

                        ushort crc16 = CalcularCRC(resultado);
                        byte[] dadosCRC = BitConverter.GetBytes(crc16);
                        dadosCRC.CopyTo(resultado, 62);                        
                    }

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
                    FormatoAgenda* roteiros = (FormatoAgenda*)pSrc;

                    this.versao = roteiros->versao;
                    this.reservado = roteiros->reservado;
                  

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
            Byte[] dadosCRC = new byte[dados.Length - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoAgenda* agenda = (FormatoAgenda*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&agenda->crc - (int)pSrc);
                Array.Copy(dados, ((int)&agenda->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&agenda->crc - (int)pSrc,
                           dados.Length - ((int)&agenda->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }


        public bool VerificarCRC(Byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoAgenda* parametros = (FormatoAgenda*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoAgenda));
            }
            return resposta;
        }
    }    
}
