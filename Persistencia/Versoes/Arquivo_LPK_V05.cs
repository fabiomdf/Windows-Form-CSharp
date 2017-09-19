using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Util;
using System.Resources;
using Persistencia.Erros;

namespace Persistencia
{
    public class Arquivo_LPK_V05
    {
        private const int NSS_LPK_SIZE = 192; // Tamanho do arquivo de Idiomas do NSS (APP)
        private const int QTD_FRASES = 109;
        private const int TAM_MAX_FRASE = 16;

        private int versao = 5;
        public List<string> texto;
        public string nome;
        public UInt16 crc;
        public ResourceManager rm;

        public enum ErroArquivo
        {
            SUCESSO,
            ERRO_CRC,
            ERRO_TAMANHO_ARQUIVO
        }

        public unsafe struct FormatoIdioma
        {
            public Byte versao;
            public fixed Byte reservado[3];
            public fixed Byte nome[TAM_MAX_FRASE];
            public fixed Byte reservado2[42];
            public UInt16 crc;
            public fixed Byte texto[TAM_MAX_FRASE * QTD_FRASES];
        }

        public Arquivo_LPK_V05(ResourceManager rm, String nome)
        {
            //this.versao = 3;
            this.nome = nome;
            this.rm = rm;
            texto = new List<string>();
            texto.AddRange(CarregarFrasesDefault());
        }

        public Arquivo_LPK_V05(ResourceManager rm)
        {
            //this.versao = 3;
            nome = Util.Util.NOME_IDIOMA;
            this.rm = rm;
            texto = new List<string>();
            texto.AddRange(CarregarFrasesDefault());
        }

        public string[] CarregarFrasesDefault()
        {
            return CarregarFrases(this.rm);
        }

        private string[] CarregarFrases(System.Resources.ResourceManager rm)
        {
            List<string> textoLocal = new List<string>();

            for (int i = 0; i < QTD_FRASES; i++)
            {
                textoLocal.Add(rm.GetString("TEXTO_" + i.ToString() + "_ARQUIVO_LPK").PadRight(TAM_MAX_FRASE, '\0'));
            }

            return textoLocal.ToArray();
        }

        public unsafe void stringToByteArray(Byte* array, List<string> listaFrases, int length)
        {
            int indiceFrase = 0;
            foreach (var str in listaFrases)
            {
                char[] charArray = str.PadRight(length, '\0').ToCharArray();
                for (int i = 0; i < length; i++)
                {
                    array[(indiceFrase * TAM_MAX_FRASE) + i] = (Byte)charArray[i];
                }
                indiceFrase++;
            }
        }
        public void FromBytesToFormatoIdioma(Byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoIdioma* idioma = (FormatoIdioma*)pSrc;

                    this.versao = idioma->versao;
                    this.nome = String.Empty;
                    for (int i = 0; i < TAM_MAX_FRASE; i++)
                    {
                        this.nome += (char)idioma->nome[i];
                    }
                    this.texto.Clear();
                    for (int indiceFrase = 0; indiceFrase < QTD_FRASES; indiceFrase++)
                    {
                        String Aux = string.Empty;
                        for (int i = 0; i < TAM_MAX_FRASE; i++)
                        {
                            Aux += (char)idioma->texto[(indiceFrase * TAM_MAX_FRASE) + i];
                        }
                        this.texto.Add(Aux);
                    }
                }
            }
        }
        public Byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoIdioma)];

                FormatoIdioma idioma = new FormatoIdioma();
                idioma.versao = (Byte)this.versao;
            }
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoIdioma)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoIdioma* idioma = (FormatoIdioma*)pSrc;

                    idioma->versao = (Byte)this.versao;

                    this.nome = this.nome.PadRight(16, '\0');

                    for (int i = 0; i < this.nome.Length; i++)
                    {
                        idioma->nome[i] = (byte)this.nome[i];
                    }

                    stringToByteArray(idioma->texto, this.texto, 16);
                    idioma->crc = this.crc;

                }

                return resultado;
            }

        }

        public void Salvar(string arquivoNome)//, object controlador)
        {
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();
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

                    FromBytesToFormatoIdioma(dados);
                    return this;
                }
            }
            return null;

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

            //Comparando a versão do arquivo_lpk com a versão do arquivo do usuário, se for a mesma versão não restaura
            if (this.versao == versaoArquivoUsuario)
                return false;


            //Parte do código onde irá restaurar o arquivo das versões anteriores
            Arquivo_LPK_V04 LPKRestaurado = new Arquivo_LPK_V04(this.rm);
            LPKRestaurado.nome = Path.GetFileNameWithoutExtension(arquivoNome);
            LPKRestaurado.RestaurarVersao(arquivoNome);

            ////copiando as informações do arquivo do usuário para a nova versão
            this.rm = LPKRestaurado.rm;
            this.nome = LPKRestaurado.nome;
            this.texto = LPKRestaurado.texto;

            //adicionando as novas informações da versao 4 para a versão 5
            this.texto.Add(rm.GetString("TEXTO_108_ARQUIVO_LPK").PadRight(TAM_MAX_FRASE, '\0'));

            return true;

        }

        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoIdioma* parametros = (FormatoIdioma*)pSrc;

                this.crc = CalcularCRC(dados);
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
                FormatoIdioma* parametros = (FormatoIdioma*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&parametros->crc - (int)pSrc);
                Array.Copy(dados, ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&parametros->crc - (int)pSrc,
                           dados.Length - ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }

        public bool VerificarCRC(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoIdioma* parametros = (FormatoIdioma*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            if (fs.Length == NSS_LPK_SIZE)
            {
                return true;
            }
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoIdioma));
            }
            return resposta;
        }

    }
}
