using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using ImportacaoLDX;
using Controlador;

namespace LDX2
{
    public class Program
    {

        public static void Main(string[] args)
        {
            #region Programa em DOS
            String versao = Environment.Version.Major.ToString();

            versao = versao + " | " +
            Environment.Version.MajorRevision.ToString() + " | " + Environment.Version.Minor.ToString() + " | " +
            Environment.Version.MinorRevision.ToString() + " | " + Environment.Version.Revision.ToString();

            try
            {
                // TODO: Qualquer problema ver o código button2_Click em FormGeradorDeArquivos.cs

                string arquivoOrigem = (args.Length == 0) ? @"c:\testeNand\Imchala3.b12" : args[0];
                string diretorioDestino = (args.Length <= 1) ? @"c:\testeNand\Programacao" : args[1];

                arquivoOrigem = arquivoOrigem.Replace("$", " ");
                diretorioDestino = diretorioDestino.Replace("$", " ");

                GerarDiretoriosFAT(arquivoOrigem, diretorioDestino);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }            
            #endregion Programa em DOS

            #region Testes para V02 e V04

           // Frase frase = new Frase("A");

           // frase.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;

           // //frase.Modelo.Textos.Clear();

           // frase.Modelo.Textos.Add(new Texto("Marcus"));

           // frase.Modelo.Textos[0].Fonte = @"83_novafonte13x8";
           // frase.Modelo.Textos[1].Fonte = @"83_novafonte13x8";

           ////frase.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;

           // //frase.Modelo.Textos[0].Fonte = @"fonte99";
           // //frase.Modelo.Textos[1].Fonte = @"fonte99";
           // frase.TipoVideo = Util.Util.TipoVideo.V02;

           // frase.Modelo.Textos[0].Apresentacao = Util.Util.Rolagem.Fixa;
           // //frase.Modelo.Textos[1].Apresentacao = Util.Util.Rolagem.Rolagem_Continua3_Esquerda ;
            

           // frase.Salvar(@"C:\TesteNand\", 16, 144);


            #endregion Testes para V02 e V04
        }
        private static void GerarDiretoriosFAT(string b12Origem, string diretorioDestino)
        {
            Memorias.NandFFS m = new Memorias.NandFFS();
            m.GerarDiretoriosFAT(b12Origem, diretorioDestino);
        }
        private static void GerarArquivoPontos(string DIRETORIO_RAIZ, string ldxOrigem, string arquivoDestino)
        {
            Console.WriteLine("Importing Data from Pontos 6 Files... Wait a moment...");
            LDX2.ImportacaoLDX importacao = new LDX2.ImportacaoLDX();

            importacao.ImportarProgramacao(ldxOrigem, DIRETORIO_RAIZ);
            if (Directory.Exists(DIRETORIO_RAIZ + "\\videos"))
            {
                Directory.Delete(DIRETORIO_RAIZ + "\\videos", true);
            }


            if (DIRETORIO_RAIZ.EndsWith("\\"))
            {
                DIRETORIO_RAIZ = DIRETORIO_RAIZ.Substring(0, DIRETORIO_RAIZ.Length - 1);
                if (DIRETORIO_RAIZ.EndsWith("\\"))
                {
                    DIRETORIO_RAIZ = DIRETORIO_RAIZ.Substring(0, DIRETORIO_RAIZ.Length - 1);
                }
            }
            Console.WriteLine("Creating a B12 File.");
            Memorias.NandFFS m = new Memorias.NandFFS();
            if (Directory.Exists(DIRETORIO_RAIZ + "\\FRT_APP"))
            {
                //m.GerarArquivoNand(DIRETORIO_RAIZ + "\\FRT_APP", DIRETORIO_RAIZ + "\\nss.nfs");
                m.GerarArquivoNandOtimizado(DIRETORIO_RAIZ + "\\FRT_APP", DIRETORIO_RAIZ + "\\nss.nfs");
                Directory.Delete(DIRETORIO_RAIZ + "\\FRT_APP", true);
            }

            //m.GerarArquivoNand(DIRETORIO_RAIZ, arquivoDestino);
            m.GerarArquivoNandOtimizado(DIRETORIO_RAIZ, arquivoDestino);

            Console.WriteLine("B12 file created.");
            
            Console.WriteLine("Deleting Temporary Files. ");            ;
            Directory.Delete(DIRETORIO_RAIZ + "\\fontes", true);            
            Directory.Delete(DIRETORIO_RAIZ + "\\idiomas", true);            
            Directory.Delete(DIRETORIO_RAIZ + "\\msgs", true);
            Directory.Delete(DIRETORIO_RAIZ + "\\paineis", true);
            Directory.Delete(DIRETORIO_RAIZ + "\\regioes", true);
            Directory.Delete(DIRETORIO_RAIZ + "\\roteiros", true);

            File.Delete(DIRETORIO_RAIZ + "\\param.fix");
            File.Delete(DIRETORIO_RAIZ + "\\param.var");

            File.Delete(DIRETORIO_RAIZ + "\\nss.nfs");

            Console.WriteLine("Temporary Files deleted. ");
            
        }


    }
}
