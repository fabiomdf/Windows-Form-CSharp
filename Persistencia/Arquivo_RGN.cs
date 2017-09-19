using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_RGN 
    {
        public enum ErroArquivo
        {
            SUCESSO,
            ERRO_CRC,
            ERRO_TAMANHO_ARQUIVO
        }


        public unsafe struct FormatoRegiao
        {
            public Byte versao;
            public Byte formatoDataHora;
            public Byte unidadeVelocidade;
            public Byte unidadeTemperatura;
            public Byte moeda;
            public Byte separadorDecimal;
            public fixed byte nome [16];
            public Byte opcaoAmPm_Ponto;
            public fixed Byte reservado [39];
            public UInt16 crc;
            public fixed byte idiomaPath [64];
        }


        //public Byte versao { get; set; }
        private Byte versao = 2;
        public Byte formatoDataHora { get; set; }
        public Byte opcaoAmPm_Ponto { get; set; }
        public Byte unidadeVelocidade { get; set; }
        public Byte unidadeTemperatura { get; set; }
        public Byte moeda { get; set; }
        public byte separadorDecimal { get; set; }
        public byte[] nome{ get; set; }
        public Byte[] reservado{ get; set; }
        public UInt16 crc{ get; set; }
        public byte[] idiomaPath{ get; set; }
        public String Nome { get { return Encoding.ASCII.GetString(nome); } }
        
        public Arquivo_RGN()
        {
            //this.versao = 2;
            this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_24H;
            this.opcaoAmPm_Ponto = (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
            this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
            this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
            this.moeda = (byte)Util.Util.Moeda.MOEDA_REAL;
            this.separadorDecimal = Encoding.ASCII.GetBytes(",")[0];

            this.nome = new byte[16];
            this.nome = Encoding.ASCII.GetBytes("SEM REGIAO      ");

            this.reservado = new byte[40];
            this.idiomaPath = new byte[64];

            this.crc = 0;

            String pathTemporario = String.Empty;
            this.idiomaPath = Encoding.ASCII.GetBytes(pathTemporario.PadRight(64, '\0'));              

        }

        public Arquivo_RGN(Util.Util.Lingua idioma)
        {

            switch(idioma)
            {
                case Util.Util.Lingua.Portugues:
                    CarregaPortugues();
                    break;

                case Util.Util.Lingua.Ingles:
                    CarregaIngles();
                    break;

                case Util.Util.Lingua.Espanhol:
                    CarregaEspanhol();
                    break;

                case Util.Util.Lingua.Frances:
                    CarregaFrances();
                    break;

            }


        }

        public int VerificarVersaoUsuario(string arquivoNome)
        {
            FileStream fs = File.OpenRead(arquivoNome);
            byte[] dados = new byte[1];
            fs.Read(dados, 0, 1);
            fs.Close();

            return dados[0];
        }

        public bool RestaurarVersao(string arquivoNome)
        {

            int versaoArquivoUsuario = VerificarVersaoUsuario(arquivoNome);

            //Comparando a versão do arquivo com a versão do arquivo do usuário
            if (this.versao == versaoArquivoUsuario)
                return false;

            //Se a versão do arquivo for anterior a versão inicial, no caso 2. Antes de chamar a função para restaurar deve-se instanciar a versão recente
            if (versaoArquivoUsuario < 2)
                return true;


            //Parte do código onde irá restaurar os arquivos das versões anteriores, foi testado e comentado porque não há versões novas de arquivo_alt

            //// Recuperando o ultimo LPK
            //Arquivo_LPK_V02 LPKRestaurado = new Arquivo_LPK_V02(this.rm);
            //LPKRestaurado.nome = arquivoNome;
            //LPKRestaurado.RestaurarVersao();

            ////Adicionando as alterações da versão 3 ao lpk v2 recuperado

            ////copiando as informações do arquivo do usuário para a nova versão
            //this.rm = LPKRestaurado.rm;
            //this.nome = LPKRestaurado.nome;
            //this.texto = LPKRestaurado.texto;

            ////adicionando as novas informações
            //this.texto.Add(rm.GetString("TEXTO_104_ARQUIVO_LPK").PadRight(TAM_MAX_FRASE, '\0'));

            return true;

        }

        private void CarregaPortugues()
        {
            //this.versao = 2;
            this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_24H;
            this.opcaoAmPm_Ponto = (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
            this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
            this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
            this.moeda = (byte)Util.Util.Moeda.MOEDA_REAL;
            this.separadorDecimal = Encoding.ASCII.GetBytes(",")[0];

            this.nome = new byte[16];
            this.nome = Encoding.ASCII.GetBytes("BRASIL          ");

            this.reservado = new byte[40];
            this.idiomaPath = new byte[64];

            this.crc = 0;

            String pathTemporario = String.Empty;
            this.idiomaPath = Encoding.ASCII.GetBytes(pathTemporario.PadRight(64, '\0'));

        }

        private void CarregaIngles()
        {
            //this.versao = 2;
            this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM;
            this.opcaoAmPm_Ponto = (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
            this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_MPH;
            this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_FAHRENHEIT;
            this.moeda = (byte)Util.Util.Moeda.MOEDA_DOLAR;
            this.separadorDecimal = Encoding.ASCII.GetBytes(".")[0];

            this.nome = new byte[16];
            this.nome = Encoding.ASCII.GetBytes("EUA             ");

            this.reservado = new byte[40];
            this.idiomaPath = new byte[64];

            this.crc = 0;

            String pathTemporario = String.Empty;
            this.idiomaPath = Encoding.ASCII.GetBytes(pathTemporario.PadRight(64, '\0'));

        }

        private void CarregaEspanhol()
        {
            //this.versao = 2;
            this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM;
            this.opcaoAmPm_Ponto = (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
            this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
            this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
            this.moeda = (byte)Util.Util.Moeda.MOEDA_PESO;
            this.separadorDecimal = Encoding.ASCII.GetBytes(".")[0];

            this.nome = new byte[16];
            this.nome = Encoding.ASCII.GetBytes("ES              ");

            this.reservado = new byte[40];
            this.idiomaPath = new byte[64];

            this.crc = 0;

            String pathTemporario = String.Empty;
            this.idiomaPath = Encoding.ASCII.GetBytes(pathTemporario.PadRight(64, '\0'));

        }

        private void CarregaFrances()
        {
            //this.versao = 2;
            this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_24H;
            this.opcaoAmPm_Ponto = (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
            this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
            this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
            this.moeda = (byte)Util.Util.Moeda.MOEDA_EURO;
            this.separadorDecimal = Encoding.ASCII.GetBytes(".")[0];

            this.nome = new byte[16];
            this.nome = Encoding.ASCII.GetBytes("FRANCE          ");

            this.reservado = new byte[40];
            this.idiomaPath = new byte[64];

            this.crc = 0;

            String pathTemporario = String.Empty;
            this.idiomaPath = Encoding.ASCII.GetBytes(pathTemporario.PadRight(64, '\0'));

        }


        public Arquivo_RGN(Arquivo_RGN oldValue)
        {
            this.versao = oldValue.versao;
            this.formatoDataHora = oldValue.formatoDataHora;
            this.opcaoAmPm_Ponto = oldValue.opcaoAmPm_Ponto;
            this.unidadeVelocidade = oldValue.unidadeVelocidade;
            this.unidadeTemperatura = oldValue.unidadeTemperatura;
            this.moeda = oldValue.moeda;
            this.separadorDecimal = oldValue.separadorDecimal;

            this.nome = new byte[oldValue.nome.Length];
            oldValue.nome.CopyTo(this.nome, 0);

            this.reservado = new byte[oldValue.reservado.Length];
            oldValue.reservado.CopyTo(this.reservado, 0);

            this.idiomaPath = new byte[oldValue.idiomaPath.Length];
            oldValue.idiomaPath.CopyTo(this.idiomaPath, 0);

            this.crc = oldValue.crc;
        }

        /// <summary>
        /// Este método cria uma região padrão, no caso BRASIL.
        /// </summary>
        public void CriarRegiaoPadrao()
        {
            //this.versao = 2;
            this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_24H;
            this.opcaoAmPm_Ponto = (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
            this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
            this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
            this.moeda = (byte)Util.Util.Moeda.MOEDA_REAL;
            this.separadorDecimal = Encoding.ASCII.GetBytes(",")[0];

            this.nome = new byte[16];
            this.nome = Encoding.ASCII.GetBytes("BRASIL          ");

            this.reservado = new byte[40];
            this.idiomaPath = new byte[64];

            this.crc = 0;

            String pathTemporario = Util.Util.TrataDiretorio(Util.Util.DIRETORIO_IDIOMAS + "ptbr.lpk");
            this.idiomaPath = Encoding.ASCII.GetBytes(pathTemporario.PadRight(64, '\0'));              

        }
        public void CriarRegiaoPadrao(Util.Util.Lingua idioma, string nomeRegiao, string idiomaPath)
        {
            Persistencia.Arquivo_RGN argn = new Persistencia.Arquivo_RGN();                                                                                          
            
            idiomaPath = Util.Util.TrataDiretorio(idiomaPath);
            //argn.separadorDecimal = (this.sdecimal == Util.Util.SeparadorDecimal.VIRGULA ? Encoding.ASCII.GetBytes(",")[0] : Encoding.ASCII.GetBytes(".")[0]);

            this.opcaoAmPm_Ponto = (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_AM_PM;

            switch (idioma)
            {
                case Util.Util.Lingua.Portugues: 
                    {
                        this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_24H;
                        this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
                        this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
                        this.moeda = (byte)Util.Util.Moeda.MOEDA_REAL;
                        this.separadorDecimal = Encoding.ASCII.GetBytes(",")[0];
                        this.nome = Encoding.ASCII.GetBytes(nomeRegiao.PadRight(16, '\0').Substring(0, 16));
                        this.idiomaPath = Encoding.ASCII.GetBytes(idiomaPath.Substring(1).PadRight(64, '\0'));
                    } 
                    break;                
                case Util.Util.Lingua.Espanhol: 
                    {
                        this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_24H;
                        this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
                        this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
                        this.moeda = (byte)Util.Util.Moeda.MOEDA_EURO;
                        this.separadorDecimal = Encoding.ASCII.GetBytes(",")[0];
                        this.nome = Encoding.ASCII.GetBytes(nomeRegiao.PadRight(16, '\0').Substring(0, 16));
                        this.idiomaPath = Encoding.ASCII.GetBytes(idiomaPath.Substring(1).PadRight(64, '\0'));                        
                    } 
                    break;
                case Util.Util.Lingua.Frances: 
                    {
                        this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_24H;
                        this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
                        this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
                        this.moeda = (byte)Util.Util.Moeda.MOEDA_EURO;
                        this.separadorDecimal = Encoding.ASCII.GetBytes(",")[0];
                        this.nome = Encoding.ASCII.GetBytes(nomeRegiao.PadRight(16, '\0').Substring(0, 16));
                        this.idiomaPath = Encoding.ASCII.GetBytes(idiomaPath.Substring(1).PadRight(64, '\0'));
                    } 
                    break;
                default: // O padrão será o Inglês
                    {
                        this.formatoDataHora = (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM;
                        this.opcaoAmPm_Ponto = (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
                        this.unidadeVelocidade = (byte)Util.Util.UnidadeVelocidade.UNIDADE_MPH;
                        this.unidadeTemperatura = (byte)Util.Util.UnidadeTemperatura.UNIDADE_FAHRENHEIT;
                        this.moeda = (byte)Util.Util.Moeda.MOEDA_DOLAR;
                        this.separadorDecimal = Encoding.ASCII.GetBytes(".")[0];
                        this.nome = Encoding.ASCII.GetBytes(nomeRegiao.PadRight(16, '\0').Substring(0, 16));
                        this.idiomaPath = Encoding.ASCII.GetBytes(idiomaPath.Substring(1).PadRight(64, '\0'));
                    } break;
            }
        }

        public void Salvar(string arquivoNome)
        {           
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();
           
        }

        /*
        
        public void Salvar(string arquivoNome, ControladorPontos.Controlador Ctrldr)
        {
            
            for(int painel = 0; painel < Ctrldr.QuantidadePaineis; painel++)
            {
                Salvar(arquivoNome);
            }

        }
        */
        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoRegiao* regiao = (FormatoRegiao*)pSrc;

                this.crc = CalcularCRC(dados);
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
                    FormatoRegiao* regiao = (FormatoRegiao*)pSrc;

                    this.versao = regiao->versao;
                    this.formatoDataHora = regiao->formatoDataHora;
                    this.unidadeVelocidade = regiao->unidadeVelocidade;
                    this.unidadeTemperatura = regiao->unidadeTemperatura;
                    this.opcaoAmPm_Ponto = regiao->opcaoAmPm_Ponto;
                    this.moeda = regiao->moeda;
                    this.separadorDecimal = regiao->separadorDecimal;
                    for (int i = 0; i < 16; i++)
                    {
                        this.nome[i] = regiao->nome[i];
                    }

                    for (int i = 0; i < 40; i++)
                    {
                        this.reservado[i] = regiao->reservado[i];
                    }

                    this.crc = regiao->crc;

                    for (int i = 0; i < 64; i++)
                    {
                        this.idiomaPath[i] = regiao->idiomaPath[i];
                    }        
                }
            }
        }

        private byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoRegiao)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoRegiao* regiao = (FormatoRegiao*)pSrc;

                    regiao->versao = this.versao;
                    regiao->formatoDataHora = this.formatoDataHora;
                    regiao->unidadeVelocidade = this.unidadeVelocidade;
                    regiao->unidadeTemperatura = this.unidadeTemperatura;
                    regiao->moeda = this.moeda;
                    regiao->separadorDecimal = this.separadorDecimal;
                    regiao->opcaoAmPm_Ponto = this.opcaoAmPm_Ponto;
                    for (int i = 0; i < 16; i++)
                    {
                        regiao->nome[i] = this.nome[i];
                    }

                    for (int i = 0; i < 40; i++)
                    {
                        regiao->reservado[i] = this.reservado[i];
                    }

                    regiao->crc = this.crc;

                    for (int i = 0; i < 64; i++)
                    {
                        regiao->idiomaPath[i] = this.idiomaPath[i];
                    } 

                    return resultado;
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

            Byte[] dadosCRC = new byte[sizeof(FormatoRegiao) - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoRegiao* regiao = (FormatoRegiao*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&regiao->crc - (int)pSrc);
                Array.Copy(dados, ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&regiao->crc - (int)pSrc,
                           sizeof(FormatoRegiao) - ((int)&regiao->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }

        public bool VerificarCRC(byte[] dados)
        {            
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoRegiao* parametros = (FormatoRegiao*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;

            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoRegiao));
            }

            return resposta;
        }
    }
}