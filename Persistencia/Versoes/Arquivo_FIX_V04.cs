﻿/* Versão 1
    - Remoção das opções de apresentação - As opções de apresentação foram movidas para arquivo_cfg
    - Remoção dos labels de bom dia, boa tarde e boa noite - Esses labels estão nas mensagens especiais no painel.cfg
 
   Versão 2
    -  Adição do tempo para inverterLed
    -  Adição de painelNSS
    -  Foram revomidos 2 bytes de reservado1. Antes era reservado1[3] e passou a ser reservado1[1]
 * Versão 3
    -  Adição de Mensagem Secundária em Funções Bloqueaveis
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using Recursos;
using Util;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_FIX_V04 //: IArquivo
    {
        public struct funcoesBloqueadas
        {
            public UInt32 funcoes1;
            public UInt32 funcoes2;
        }

       
        public enum FuncoesBloqueadas
        {
            SELECAO_ROTEIRO = (1 << 0),
            SELECAO_MENSAGEM = (1 << 1),
            SELECAO_SENTIDO = (1 << 2),
            SELECAO_ALTERNANCIA = (1 << 3),
            SELECAO_PAINEL = (1 << 4),
            AJUSTE_HORA_SAIDA = (1 << 5),
            AJUSTE_RELOGIO = (1 << 6),
            SELECAO_REGIAO = (1 << 7),
            IDENTIFICAR_PAINEIS = (1 << 8),
            NOVA_CONFIGURACAO = (1 << 9),
            COLETAR_DUMP = (1 << 10),
            COLETAR_LOG = (1 << 11),
            APAGAR_ARQUIVOS = (1 << 12),
            ACENDER_PAINEIS = (1 << 13),
            CONFIG_FABRICA = (1 << 14),
            SELECAO_MENSAGEM_SEC = (1 << 15),
            MODO_TESTE = (1 << 16),
            AJUSTE_BRILHO = (1 << 17),
            SELECAO_MOTORISTA = (1 << 18)
        }

        public enum PerifericosSelecionados
        {
            CATRACA = (1 << 0),
            SENSOR_TEMPERATURA = (1 << 1),
            SENSOR_VELOCIDADE = (1 << 2),
            APP = (1 << 3)
        }

        public unsafe struct FormatoParametrosFixos
        {
            public Byte versao;
            public Byte horaBomDia;
            public Byte horaBoaTarde;
            public Byte horaBoaNoite;

            public UInt32 funcao1;
            public UInt32 funcao2;

            public UInt32 qntPaineis; //1 linha
            public Byte ativaSenhaAntiFurto;
            public Byte tempoInverterLed;
            public Byte painelAPP_NSS;
            public Byte opcaoLock;
            public UInt32 segundosGeracao;
            public fixed Byte  versaoSoftware[3];
            public byte reservado2; //catraca ou validador
            public fixed Byte versaoHardware[3];
            public byte reservado3;
            public UInt16 perifericoNaRede;
            public fixed Byte reservado4[28];

            public UInt16 crc;

            public fixed byte senhaAcessoEspecial[32];
            public fixed byte senhaAntiFurto[32];
        }

        //public Byte versao { get; set; }
        private Byte versao = 4;
        public Byte horaBomDia { get; set; }
        public Byte horaBoaTarde { get; set; }
        public Byte horaBoaNoite { get; set; }
        public Byte tempoInverterLed { get; set; }
        public Byte painelAPP_NSS { get; set; }
        public UInt16 perifericoNaRede { get; set; } //catraca ou validador
        public funcoesBloqueadas funcoes;
        public bool ativaSenhaAntiFurto { get; set; }
        public bool ativaLock { get; set; }
        public UInt32 qntPaineis { get; set; }
        public Byte[] reservado4 { get; set; }

        public UInt16 crc { get; set; }


        public byte[] senhaAcessoEspecial = new byte[32];
        public byte[] senhaAntiFurto = new byte[32];

        public UInt32 segundosGeracao;
        public Byte[]  versaoSoftware = new byte[3];
        public Byte[] versaoHardware = new byte[3];

        // Quantidade de funções pré-definidas pelo FIRMWARE do controlador
        public static int QUANTIDADE_FUNCOES_DEFINIDAS = 19;

        // Quantidade de periféricos na rede
        public static int QUANTIDADE_PERIFERICOS_NA_REDE = 4;


        // Indices de funções pré-definidas pelo FIRMWARE do controlador
        public const int SELECAO_ROTEIRO = (1 << 0);
        public const int SELECAO_MENSAGEM = (1 << 1);
        public const int SELECAO_SENTIDO = (1 << 2);
        public const int SELECAO_ALTERNANCIA = (1 << 3);
        public const int SELECAO_PAINEL = (1 << 4);
        public const int AJUSTE_HORA_SAIDA = (1 << 5);
        public const int AJUSTE_RELOGIO = (1 << 6);
        public const int SELECAO_REGIAO = (1 << 7);
        public const int IDENTIFICAR_PAINEIS = (1 << 8);
        public const int NOVA_CONFIGURACAO = (1 << 9);
        public const int COLETAR_DUMP = (1 << 10);
        public const int COLETAR_LOG = (1 << 11);
        public const int APAGAR_ARQUIVOS = (1 << 12);
        public const int ACENDER_PAINEIS = (1 << 13);
        public const int CONFIG_FABRICA = (1 << 14);
        public const int SELECAO_MENSAGEMSEC = (1 << 15);
        public const int MODO_TESTE = (1 << 16);
        public const int AJUSTE_BRILHO = (1 << 17);
        public const int SELECAO_MOTORISTA = (1 << 18);


        // As funções inicializam todas liberadas.
        public bool[] BloqueioDeFuncoes = new bool[QUANTIDADE_FUNCOES_DEFINIDAS];

        //Perifericos na rede
        public bool[] Perifericos = new bool[QUANTIDADE_PERIFERICOS_NA_REDE];

        public void InicializaBloqueioDeFuncoes()
        {
            for (int indiceFuncao = 0; indiceFuncao < QUANTIDADE_FUNCOES_DEFINIDAS; indiceFuncao++)
            {
                BloqueioDeFuncoes[indiceFuncao] = false;
            }
        }

        public void InicializaPerifericosNaRede()
        {
            for (int indicePeriferico = 0; indicePeriferico < QUANTIDADE_PERIFERICOS_NA_REDE; indicePeriferico++)
            {
                Perifericos[indicePeriferico] = false;
            }
        }

        public UInt32 ConvertToUInt32()
        {
            UInt32 funcoesTemp = 0;

            for (int indiceFuncao = 0; indiceFuncao < QUANTIDADE_FUNCOES_DEFINIDAS; indiceFuncao++)
            {
                funcoesTemp += (Convert.ToUInt32(BloqueioDeFuncoes[indiceFuncao]) << indiceFuncao);
            }
            return funcoesTemp;
        }



        public UInt16 ConvertToUint16()
        {
            UInt32 perifericos = 0;
            for (int indicePeriferico = 0; indicePeriferico < QUANTIDADE_PERIFERICOS_NA_REDE; indicePeriferico++)
               perifericos += (Convert.ToUInt32(Perifericos[indicePeriferico]) << indicePeriferico);

            return Convert.ToUInt16(perifericos);
        }

        public bool[] ConvertToBoolArray()
        {
            bool[] bloqueioDeFuncoesTemp = new bool[QUANTIDADE_FUNCOES_DEFINIDAS];

            for (int indiceFuncao = 0; indiceFuncao < QUANTIDADE_FUNCOES_DEFINIDAS; indiceFuncao++)
            {
                bloqueioDeFuncoesTemp[indiceFuncao] = Convert.ToBoolean(funcoes.funcoes1 & (UInt16)(Math.Pow(2, indiceFuncao)));
            }

            return bloqueioDeFuncoesTemp;
        }
        public Arquivo_FIX_V04()
        {
            this.reservado4 = new byte[46];
            InicializaBloqueioDeFuncoes();
            InicializaPerifericosNaRede();
        }

        public Arquivo_FIX_V04(Arquivo_FIX_V04 oldValue)
        {
            this.versao = oldValue.versao;
            this.horaBomDia = oldValue.horaBomDia;
            this.horaBoaTarde = oldValue.horaBoaTarde;
            this.horaBoaNoite = oldValue.horaBoaNoite;

            this.funcoes.funcoes1 = oldValue.funcoes.funcoes1;
            this.funcoes.funcoes2 = oldValue.funcoes.funcoes2;

            this.qntPaineis = oldValue.qntPaineis;

            this.reservado4 = new byte[oldValue.reservado4.Length];

            for (int i = 0; i < oldValue.reservado4.Length; i++)
            {
                this.reservado4[i] = oldValue.reservado4[i];
            }

            this.crc = oldValue.crc;
            this.tempoInverterLed = oldValue.tempoInverterLed;
            this.painelAPP_NSS = oldValue.painelAPP_NSS;
            this.perifericoNaRede = oldValue.perifericoNaRede;


            this.senhaAcessoEspecial = new byte[oldValue.senhaAcessoEspecial.Length];
            this.senhaAntiFurto = new byte[oldValue.senhaAntiFurto.Length];

            for (int i = 0; i < 20; i++)
            {
                this.senhaAcessoEspecial[i] = oldValue.senhaAcessoEspecial[i];
                this.senhaAntiFurto[i] = oldValue.senhaAntiFurto[i];
            }

        }


        public void CriarParametrosFixosPadrao()
        {
            //this.versao = 3;
            this.horaBomDia = 4;
            this.horaBoaTarde = 12;
            this.horaBoaNoite = 18;

            this.funcoes.funcoes1 = 0;
            this.funcoes.funcoes2 = 0;

            this.qntPaineis = 3;

            this.reservado4 = new byte[46];

            for (int i = 0; i < 46; i++)
            {
                this.reservado4[i] = 0x00;
            }
            this.crc = 0;
            this.tempoInverterLed = 0;
            this.painelAPP_NSS = 0;
            this.ativaSenhaAntiFurto = false;
            this.ativaLock = false;

            this.perifericoNaRede = 0;

            this.senhaAcessoEspecial = new byte[32];
            this.senhaAntiFurto = new byte[32];            
            for (int i = 0; i < 20; i++)
            {
                this.senhaAcessoEspecial[i] = (byte)0;//(char) i;
                this.senhaAntiFurto[i] = (byte)0;//(char) i;               
            }

        }

        public void Salvar(string arquivoNome)
        {
            AtualizarCRC();

            if ((Util.Util.ValidarHora(this.horaBomDia)) && (Util.Util.ValidarHora(this.horaBoaTarde)) &&(Util.Util.ValidarHora(this.horaBoaNoite))&&(this.qntPaineis >= 1))
            {
                byte[] dados = this.toByteArray();
                FileStream fs = File.Create(arquivoNome);
                fs.Write(dados, 0, dados.Length);
                fs.Close();
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

                    FromBytesToFormatoParametrosFixos(dados);
                    return this;
                }
            }
            return null;
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
            FromBytesToFormatoParametrosFixos(dados);
            if (!Util.Util.ValidarHora(this.horaBomDia)) // Hora de Bom dia fora do intervalo coerente
            {
                throw new FileLoadException("Good morning time error."); 
            }
            if (!Util.Util.ValidarHora(this.horaBoaTarde))// Hora de Boa Tarde fora do intervalo coerente
            {
                throw new FileLoadException("Good afternoon time error.");
            }
            if (!Util.Util.ValidarHora(this.horaBoaNoite))// Hora de Boa Noite fora do intervalo coerente
            {
                throw new FileLoadException("Good night time error.");
            }
            if (this.qntPaineis < 1) //É necessário verificar se é pelo menos igual a 1?
            {
                throw new FileLoadException("The count signs is zero or less.");
            }
            if ((this.painelAPP_NSS >= this.qntPaineis) && (this.painelAPP_NSS != 0xff)) //Verifica se o painel NSS indicado está dentro da quantidade de paineis
            {
                throw new FileLoadException("The NSS sign is out of range.");
            }
     
            fs.Close();
            return resposta;
        }

        public bool VerificarCRC(byte[] dados)
        {                                    
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoParametrosFixos* parametros = (FormatoParametrosFixos*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }
        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;

            unsafe
            {
                resposta = (fs.Length == sizeof(FormatoParametrosFixos));
            }            

            return resposta;
        }

        private void FromBytesToFormatoParametrosFixos(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoParametrosFixos* parametros = (FormatoParametrosFixos*)pSrc;

                    this.versao = parametros->versao;
                    this.horaBomDia = parametros->horaBomDia;
                    this.horaBoaTarde = parametros->horaBoaTarde;
                    this.horaBoaNoite = parametros->horaBoaNoite;
                    this.qntPaineis = parametros->qntPaineis;
                    this.tempoInverterLed = parametros->tempoInverterLed;
                    this.painelAPP_NSS = parametros->painelAPP_NSS;
                    this.perifericoNaRede = parametros->perifericoNaRede;
                    
                    for (int i = 0; i < 46; i++)
                    {
                        this.reservado4[i] = parametros->reservado4[i];
                    }
                    this.crc = parametros->crc;

                    this.segundosGeracao = parametros->segundosGeracao;

                    for (int i = 0; i < 3; i++)
                    {
                        this.versaoSoftware[i] = parametros->versaoSoftware[i];
                        this.versaoHardware[i] = parametros->versaoHardware[i];
                    }
                    this.ativaSenhaAntiFurto = Convert.ToBoolean(parametros->ativaSenhaAntiFurto);
                    this.ativaLock = Convert.ToBoolean(parametros->opcaoLock);

                    for (int i = 0; i < 32; i++)
                    {
                        this.senhaAcessoEspecial[i] = parametros->senhaAcessoEspecial[i];
                        this.senhaAntiFurto[i] = parametros->senhaAntiFurto[i];
                    }
                }
            }
        }

        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoParametrosFixos* parametros = (FormatoParametrosFixos*)pSrc;

                this.crc = CalcularCRC(dados);
            }
        }

        private byte[] toByteArray()
        {         
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoParametrosFixos)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoParametrosFixos* parametros = (FormatoParametrosFixos*)pSrc;

                    parametros->versao = (Byte)this.versao;

                    parametros->horaBomDia = this.horaBomDia;
                    parametros->horaBoaTarde = this.horaBoaTarde;
                    parametros->horaBoaNoite = this.horaBoaNoite;
                    parametros->tempoInverterLed = this.tempoInverterLed;
                    parametros->painelAPP_NSS = this.painelAPP_NSS;
                    parametros->funcao1 = this.funcoes.funcoes1;
                    parametros->funcao2 = this.funcoes.funcoes2;
                    parametros->ativaSenhaAntiFurto = Convert.ToByte(this.ativaSenhaAntiFurto);
                    parametros->opcaoLock = Convert.ToByte(this.ativaLock);
                    parametros->qntPaineis = this.qntPaineis;
                    parametros->perifericoNaRede = this.perifericoNaRede;

                    for (int i = 0; i < 46; i++)
                    {
                        parametros->reservado4[i] = this.reservado4[i];
                    }
                    
                    parametros->crc = this.crc;


                    this.segundosGeracao = (uint) (DateTime.UtcNow - new DateTime(2000, 1, 1, 0, 0, 0, 0)).TotalSeconds;

                    parametros->segundosGeracao = this.segundosGeracao;
                    String versao = Environment.Version.Major.ToString();

                    versao = versao + " | "+
                    Environment.Version.MajorRevision.ToString() + " | " + Environment.Version.Minor.ToString() + " | " +
                    Environment.Version.MinorRevision.ToString() + " | " + Environment.Version.Revision.ToString(); 
                    //parametros->versaoSoftware
        
                    for (int i = 0; i < 3; i++)
                    {
                        parametros->versaoSoftware[i] = this.versaoSoftware[i];
                        parametros->versaoHardware[i] = this.versaoHardware[i];
                    }


                    for (int i = 0; i < 32; i++)
                    {
                        parametros->senhaAcessoEspecial[i] = this.senhaAcessoEspecial[i];
                        parametros->senhaAntiFurto[i] = this.senhaAntiFurto[i];
                    }

                    BitConverter.GetBytes(CalcularCRC(resultado)).CopyTo(resultado, 62);


                    return resultado;
                }
            }
        }
        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            
            int tamanho;

            Byte[] dadosCRC = new byte[sizeof(FormatoParametrosFixos) - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoParametrosFixos* parametros = (FormatoParametrosFixos*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&parametros->crc - (int)pSrc);
                Array.Copy(dados, ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&parametros->crc - (int)pSrc,
                           sizeof(FormatoParametrosFixos) - ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }
    }
}
