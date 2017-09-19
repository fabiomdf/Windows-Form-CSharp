using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistencia;
using System.IO;
using System.Reflection;
using System.Resources;
using Globalization;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using Nucleo;
using Ionic.Zip;
using System.Collections;
using System.Security.AccessControl;
using System.Security.Principal;
using Util;
using System.Diagnostics;

namespace Controlador
{
    public class Fachada
    {
        #region Propriedades

        //public Persistencia.Arquivo_LPK alpk;
        private List<Controlador> controlador;
        private Util.Util.Lingua lingua = Util.Util.Lingua.Ingles;
        public bool appInstalado = false;
        public string caminhoApp = String.Empty;
        private string licenciadoPara;
        public string GetLicenciadoPara()
        {
            return this.licenciadoPara;
        }

        public void SetLicenciadoPara(string licensa)
        {
            this.licenciadoPara = licensa;
        }

        #endregion

        #region SINGLETON

        private static readonly Fachada instance = new Fachada();

        private Fachada()
        {

            //alpk = new Persistencia.Arquivo_LPK(carregaIdioma());

            controlador = new List<Controlador>();

            diretorio_raiz = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            diretorio_rgn = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_REGIOES;

            diretorio_lpk = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_IDIOMAS;

            diretorio_alt = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_ALT;

            diretorio_fontes = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_FONTES;

            diretorio_temporario = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_TEMPORARIO;

            diretorio_fir = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_FIRMWARE;

            diretorio_imagens = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_BITMAP;

            diretorio_NSS = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_NSS;

            diretorio_idiomaGUI = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_IDIOMA_GUI;

            diretorio_reports = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_REPORTS;

            diretorio_aplicacao = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            diretorio_rotas_app = diretorio_raiz + Util.Util.DIRETORIO_ARQUIVOS_ROTA;

            diretorio_downloads = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_DOWNLOADS;

            Util.Util.diretorio_NSS = diretorio_NSS;

            //CriaDirs();
            //GetIdioma();
        }



        public static Fachada Instance
        {
            get { return instance; }
        }
        #endregion

        #region Ativacao

        public bool ValidarChaveAtivacao(string empresa, string produto, string versao, string chaveInformada)
        {
            string dadosGeraChave = empresa + produto + versao;
            byte[] chaveCalculada = Util.AtivacaoSenha.ConverterSenhaTexto(dadosGeraChave);

            string chaveConvertida = ConvertChaveByteToString(chaveCalculada);

            if (chaveConvertida.Equals(chaveInformada))
            {
                //GerarArquivoAtivacao(empresa, produto, versao, chaveConvertida);
                return true;
            }

            return false;
        }

        public void CadastrarChaveAtivacao(string empresa, string versao, string produto, string nome, string telefone, string email)
        {
            AtivacaoSenha.DadosAtivaProduto dados = new AtivacaoSenha.DadosAtivaProduto();
            dados.empresa = empresa;
            dados.versao = versao;
            dados.produto = produto;
            dados.nome = nome;
            dados.telefone = telefone;
            dados.email = email;
            
            Util.AtivacaoSenha.AtivarProduto(dados);          
        }
        public void AtualizarSistemas(string familia, string produto, string softwareOuFirmware, string versaoArquivo)
        {
            AtualizacaoSoftware.DadosDownloadProduto dadosDownload = new AtualizacaoSoftware.DadosDownloadProduto();
            dadosDownload.getversion = "True";
            dadosDownload.produto = produto;
            dadosDownload.versao = familia;
            dadosDownload.sw_fw = softwareOuFirmware;
            string versaoSOPA = Util.AtualizacaoSoftware.VerificarVersaoDownloadProduto(dadosDownload);

            Version comparaVersaoSopa = new Version(versaoSOPA);
            Version comparaVersaoArquivo = new Version(versaoArquivo);
            

            
            if (comparaVersaoSopa.CompareTo(comparaVersaoArquivo) > 0)
            {
                dadosDownload.getversion = "False";
                string url = Util.AtualizacaoSoftware.VerificarDownloadProduto(dadosDownload);
                Process.Start(url);
                Uri uri = new Uri(url);
                //// Código para remover "?file=" do PathAndQuery
                ////string filename = System.IO.Path.GetFileName(uri.PathAndQuery).Substring(6);
                //string filename = "\\" + produto + "_" + familia + "_" + softwareOuFirmware + "_" + versaoArquivo;

                //Util.AtualizacaoSoftware.ExecutarDownloadSincrono(@"" + url, @"" + diretorio_downloads + "\\" + filename);
            }
           
        }

        public string VerificarAtualizarSistemas(string familia, string produto, string softwareOuFirmware, string versaoArquivo)
        {
            AtualizacaoSoftware.DadosDownloadProduto dadosDownload = new AtualizacaoSoftware.DadosDownloadProduto();
            dadosDownload.getversion = "True";
            dadosDownload.produto = produto;
            dadosDownload.versao = familia;
            dadosDownload.sw_fw = softwareOuFirmware;
            string versaoSOPA = Util.AtualizacaoSoftware.VerificarVersaoDownloadProduto(dadosDownload);

            Version comparaVersaoSopa = new Version(versaoSOPA);
            Version comparaVersaoArquivo = new Version(versaoArquivo);



            if (comparaVersaoSopa.CompareTo(comparaVersaoArquivo) > 0)
            {
                dadosDownload.getversion = "False";
                string url = Util.AtualizacaoSoftware.VerificarDownloadProduto(dadosDownload);

                return url;
                //Process.Start(url);
                //Uri uri = new Uri(url);
                //// Código para remover "?file=" do PathAndQuery
                ////string filename = System.IO.Path.GetFileName(uri.PathAndQuery).Substring(6);
                //string filename = "\\" + produto + "_" + familia + "_" + softwareOuFirmware + "_" + versaoArquivo;

                //Util.AtualizacaoSoftware.ExecutarDownloadSincrono(@"" + url, @"" + diretorio_downloads + "\\" + filename);
            }
            return string.Empty;
        }
        public void RealizarDownloadNovasVersoesSoftware(string familia, string produto, string versaoArquivo)
        {
            AtualizarSistemas(familia, produto, "SW", versaoArquivo);
        }
        public void RealizarDownloadNovasVersoesFirmware(string familia, string produto, string versaoArquivo)
        {
            AtualizarSistemas(familia, produto, "FW", versaoArquivo);
        }
        public void RealizarDownloadNovasVersoesFirmware()
        {
            List<Arquivo_FIR.IdentificacaoProduto> lista = CarregarListaFirmwares();
            //lista[0].versao.Major

            foreach (Arquivo_FIR.IdentificacaoProduto produto in lista)
            {
                AtualizarSistemas(produto.versao.Major.ToString(), produto.nomeProduto, "FW", produto.versao.ToString());
            }
        }

        public void GerarArquivoAtivacao(string empresa, string produto, string versao, string chave)
        {
            string ArquivoNome = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO;
            //if (File.Exists(ArquivoNome))
            //{
            //    return;
            //}

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(empresa);
            sb.AppendLine(produto);
            sb.AppendLine(versao);
            sb.AppendLine(chave);

            // Gerar arquivo da Ativação
            File.WriteAllText(ArquivoNome, (sb.ToString()));

            // Zipar
            ZipFile zip = new ZipFile();
            zip.AddFile(ArquivoNome, "");
            zip.Save(ArquivoNome);
        }
        public void CriarArquivoTrial(string empresa, string versao, string produto, string nome, string telefone, string email)
        {
            string arquivoTrial = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO_3DIAS;
            if (!Directory.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP))
            {
                Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_APP);
            }
            if (!File.Exists(arquivoTrial))
            {
                string[] conteudo = new string[9];
                conteudo[0] = DateTime.Now.Year.ToString();
                conteudo[1] = DateTime.Now.Month.ToString();
                conteudo[2] = DateTime.Now.Day.ToString();
                conteudo[3] = empresa;
                conteudo[4] = versao;
                conteudo[5] = produto;
                conteudo[6] = nome;
                conteudo[7] = telefone;
                conteudo[8] = email;


                File.WriteAllLines(arquivoTrial, conteudo);
                // Zipar arquivo
                ZipFile zip = new ZipFile();
                zip.AddFile(arquivoTrial, "");
                zip.Save(arquivoTrial);
            }
        }

        //public void CriarArquivoTrial()
        //{
        //    string arquivoTrial = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO_3DIAS;

        //    if (!File.Exists(arquivoTrial))
        //    {
        //        string[] conteudo = new string[3];
        //        conteudo[0] = DateTime.Now.Year.ToString();
        //        conteudo[1] = DateTime.Now.Month.ToString();
        //        conteudo[2] = DateTime.Now.Day.ToString();

        //        File.WriteAllLines(arquivoTrial, conteudo);
        //        // Zipar arquivo
        //        ZipFile zip = new ZipFile();
        //        zip.AddFile(arquivoTrial, "");
        //        zip.Save(arquivoTrial);
        //    }
        //}

        public void ApagarArquivoTrial()
        {
            string arquivoTrial = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO_3DIAS;
            if (File.Exists(arquivoTrial))
                File.Delete(arquivoTrial);
        }

        public DateTime GetValidadeArquivoTrial()
        {
            string arquivoTrial = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO_3DIAS;
            string pastaTemp = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_TEMPORARIO;
            // Descompactar
            ZipFile zipFile = ZipFile.Read(arquivoTrial);
            zipFile.ExtractAll(pastaTemp, ExtractExistingFileAction.OverwriteSilently);

            string[] data = File.ReadAllLines(pastaTemp + Util.Util.ARQUIVO_ATIVACAO_3DIAS);

            DateTime dt = new DateTime(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]));

            if (data.Length > 3)
            {
                try
                {
                    // empresa; versao; produto; nome; telefone; email;
                    CadastrarChaveAtivacao(data[3], data[4], data[5], data[6], data[7], data[8]);
                }
                catch
                {

                }

            }

            if (File.Exists(pastaTemp + Util.Util.ARQUIVO_ATIVACAO_3DIAS))
                File.Delete(pastaTemp + Util.Util.ARQUIVO_ATIVACAO_3DIAS);

            return dt;
        }

        public Boolean arquivoTrialCriado()
        {            
            if (File.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO_3DIAS))
                return true;
            else
                return false;

        }

        public Boolean arquivoAtivacaoCriado()
        {
            if (File.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO))
                return true;
            else
                return false;
        }

        public DateTime GetValidade()
        {
            string caminho = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO_3DIAS;
            string caminhoTemporario = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_TEMPORARIO;

            string ArquivoNome = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO;

            if (File.Exists(ArquivoNome))
            {
                return DateTime.Now;
            }

            if (!File.Exists(caminho))
            {
                string[] conteudo = new string[3];
                conteudo[0] = DateTime.Now.Year.ToString();
                conteudo[1] = DateTime.Now.Month.ToString();
                conteudo[2] = DateTime.Now.Day.ToString();

                File.WriteAllLines(caminho, conteudo);
                // Zipar arquivo
                ZipFile zip = new ZipFile();
                zip.AddFile(caminho, "");
                zip.Save(caminho);

                return DateTime.Now;
            }

            // Descompactar
            ZipFile zipFile = ZipFile.Read(caminho);
            zipFile.ExtractAll(caminhoTemporario, ExtractExistingFileAction.OverwriteSilently);

            string[] data = File.ReadAllLines(caminhoTemporario + Util.Util.ARQUIVO_ATIVACAO_3DIAS);
            
            DateTime dt = new DateTime(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]));

            if (File.Exists(caminhoTemporario + Util.Util.ARQUIVO_ATIVACAO_3DIAS))
                File.Delete(caminhoTemporario + Util.Util.ARQUIVO_ATIVACAO_3DIAS);

            return dt;
        }

        public void SetarTurnOffUSBControlador(int controladorSelecionado, bool USBDesligada)
        {
            controlador[controladorSelecionado].ParametrosVariaveis.TurnOffUSB = USBDesligada;
        }

        public Util.Util.ValidacaoAtivacao SituacaoChave()
        {            

            if (VerificarArquivoAtivacao())
                return Util.Util.ValidacaoAtivacao.Ativado;
            
            
            if (arquivoTrialCriado())
            {
                DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if ((now - GetValidadeArquivoTrial()).TotalDays >= Util.Util.DIAS_CHAVE_TRIAL)
                {
                    return Util.Util.ValidacaoAtivacao.Expirado;
                }
            }

            return Util.Util.ValidacaoAtivacao.EmAtivacao;
        }

        public int DiasExpirarChaveTrial()
        {
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            int dias = Util.Util.DIAS_CHAVE_TRIAL - Convert.ToInt16((now - GetValidadeArquivoTrial()).TotalDays);
            return dias;
        }

        public string GerarChaveAtivacao(string empresa, string produto, string versao)
        {
            string dadosGeraChave = empresa + produto + versao;
            byte[] chaveCalculada = Util.AtivacaoSenha.ConverterSenhaTexto(dadosGeraChave);

            string chaveConvertida = ConvertChaveByteToString(chaveCalculada);

            return chaveConvertida;
        }

        public bool VerificarArquivoAtivacao()
        {
            try
            {
                string caminho = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO;
                string caminhoTemporario = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_TEMPORARIO;

                if (!File.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_ATIVACAO))
                {
                    return false;
                }

                ZipFile zipFile = ZipFile.Read(caminho);
                zipFile.ExtractAll(caminhoTemporario, ExtractExistingFileAction.OverwriteSilently);
                zipFile.Dispose();

                string[] sb = File.ReadAllLines(caminhoTemporario + Util.Util.ARQUIVO_ATIVACAO, ASCIIEncoding.ASCII);

                string empresa = sb[0];
                string produto = sb[1];
                string versao = sb[2];
                string chave = sb[3];

                if (File.Exists(caminhoTemporario + Util.Util.ARQUIVO_ATIVACAO))
                {
                    File.Delete(caminhoTemporario + Util.Util.ARQUIVO_ATIVACAO);
                }


                // Verificar arquivo da Ativação
                if (ValidarChaveAtivacao(empresa, produto, versao, chave))
                {
                    this.licenciadoPara = empresa;
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private string ConvertChaveByteToString(byte[] chaveCalculada)
        {
            string chave = string.Empty;

            foreach (byte b in chaveCalculada)
            {
                chave += b.ToString("X2");
            }

            return chave;
        }
        #endregion

        #region DIRETORIOS

        public static String diretorioOrigem;

        public static String diretorio_NSS;

        public static String diretorio_raiz;

        public static String diretorio_rgn;

        public static String diretorio_fir;        

        public static String diretorio_lpk;

        public static String diretorio_alt;

        public static String diretorio_fontes;

        public static String diretorio_temporario;

        public static String diretorio_imagens;

        public static String diretorio_idiomaGUI;

        public static String diretorio_reports;
        
        public static String diretorio_aplicacao;

        public static String diretorio_rotas_app;

        public static String diretorio_downloads;


        /// <summary>
        /// Cria os diretórios padrão para rodar o sistema.
        /// </summary>
        /// 

        public String GetDiretorio(string diretorio)
        {
            string retorno = "";
            switch (diretorio)
            {
                case "fontes":
                    retorno = diretorio_fontes.Substring(0, diretorio_fontes.Length - 1);
                               retorno = retorno.Replace("\\\\", "\\"); 
                               break; 
            }
            return retorno;
        }

        private void GerarLinguasRegioesDefault(Util.Util.Lingua idioma, String nomeArquivoSemExtensao)
        {
            Arquivo_RGN arquivoREGION = new Arquivo_RGN(idioma);

            string RGN_fileName = nomeArquivoSemExtensao + Util.Util.ARQUIVO_EXT_RGN;
            string destinoRGNFile = System.IO.Path.Combine(diretorio_rgn, RGN_fileName);
            string idiomaPath = Util.Util.DIRETORIO_IDIOMAS + nomeArquivoSemExtensao + Util.Util.ARQUIVO_EXT_LPK;
            arquivoREGION.CriarRegiaoPadrao(idioma, nomeArquivoSemExtensao, idiomaPath);

            if (!File.Exists(destinoRGNFile))
            {
                arquivoREGION.Salvar(destinoRGNFile);
            }
        }

        public void CriaDirs()
        {
            #region Criar Diretorio LDX2

            //diretório do aplicativo em programData.
            if (!Directory.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP))
            {
                Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_APP);
            }

            #endregion

            #region Idioma GUI

            Directory.CreateDirectory(diretorio_idiomaGUI);

            //se o diretorio existir e o arquivo não, copiar o arquvo
            if (!File.Exists(diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI))
            {
                string linguaInstalacao = File.ReadAllText(diretorio_aplicacao + Util.Util.DIRETORIO_IDIOMA_GUI + Util.Util.ARQUIVO_IDIOMA_GUI);
                File.WriteAllText(diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI, linguaInstalacao);
            }
            else
            {
                //Se os arquivos existirem e as linguas forem iguais não copia
                string linguaProgramData = File.ReadAllText(diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI);
                string linguaInstalacao = File.ReadAllText(diretorio_aplicacao + Util.Util.DIRETORIO_IDIOMA_GUI + Util.Util.ARQUIVO_IDIOMA_GUI);

                if (Convert.ToByte(linguaProgramData) != Convert.ToByte(linguaInstalacao))
                {
                    //se o arquivo já existir no programData, comparar com a data de instalação do sw para copiar a lingua selecionada no setup de instalação
                    FileInfo fileProgramData = new FileInfo(diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI);
                    FileInfo fileInstalacao = new FileInfo(diretorio_aplicacao + Util.Util.DIRETORIO_IDIOMA_GUI + Util.Util.ARQUIVO_IDIOMA_GUI);

                    if (fileProgramData.LastWriteTime <= fileInstalacao.LastAccessTime)
                        File.WriteAllText(diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI, linguaInstalacao);
                }
            }
            

            //Carregando na fachada o Idioma Setado no arquivo idiomaGUI
            GetIdioma();

            #endregion

            #region Idiomas LPK

            //idiomas
            if (!Directory.Exists(diretorio_lpk))
            {
                Directory.CreateDirectory(diretorio_lpk);
            }                

            //Criando LPK portugues
            string LPK_fileName = Util.Util.LPK_PT_BR + Util.Util.ARQUIVO_EXT_LPK;            
            string destinoFilePortugues = System.IO.Path.Combine(diretorio_lpk, LPK_fileName);
            Arquivo_LPK arquivo = new Arquivo_LPK(new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass))), Util.Util.LPK_PT_BR);
            if (!File.Exists(destinoFilePortugues))
                arquivo.Salvar(destinoFilePortugues);
            else
                //Se a versão do arquivo do usuário for antiga, abrir o objeto antigo, preencher o novo objeto e adicionar as alterações da versão nova.
                if (arquivo.RestaurarVersao(destinoFilePortugues))
                    arquivo.Salvar(destinoFilePortugues);


            //Criando LPK Ingles
            LPK_fileName = Util.Util.LPK_EN_US + Util.Util.ARQUIVO_EXT_LPK;            
            string destinoFileIngles = System.IO.Path.Combine(diretorio_lpk, LPK_fileName);
            arquivo = new Arquivo_LPK(new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass))), Util.Util.LPK_EN_US);
            if (!File.Exists(destinoFileIngles))
                arquivo.Salvar(destinoFileIngles);
            else
                if (arquivo.RestaurarVersao(destinoFileIngles))
                    arquivo.Salvar(destinoFileIngles);


            //Criando LPK Espanhol
            LPK_fileName = Util.Util.LPK_ES_ES + Util.Util.ARQUIVO_EXT_LPK;            
            string destinoFileEspanhol = System.IO.Path.Combine(diretorio_lpk, LPK_fileName);
            arquivo = new Arquivo_LPK(new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass))), Util.Util.LPK_ES_ES);
            if (!File.Exists(destinoFileEspanhol))
                arquivo.Salvar(destinoFileEspanhol);
            else
                if (arquivo.RestaurarVersao(destinoFileEspanhol))
                    arquivo.Salvar(destinoFileEspanhol);


            #endregion

            #region Regiões
            //regiões
            if (!Directory.Exists(diretorio_rgn))
            {
                Directory.CreateDirectory(diretorio_rgn);
            }
            
            //Criando região Portugues
            string RGN_fileName_PT_BR = Util.Util.RGN_PT_BR + Util.Util.ARQUIVO_EXT_RGN;
            string destinoRGNFile = System.IO.Path.Combine(diretorio_rgn, RGN_fileName_PT_BR);
            Arquivo_RGN arquivoREGION = new Arquivo_RGN(Util.Util.Lingua.Portugues);
            string idiomaPathPt = "\\" + Directory.GetParent(destinoFilePortugues).Name + "\\" + Path.GetFileNameWithoutExtension(RGN_fileName_PT_BR) + Util.Util.ARQUIVO_EXT_LPK;
            arquivoREGION.CriarRegiaoPadrao(Util.Util.Lingua.Portugues, Path.GetFileNameWithoutExtension(RGN_fileName_PT_BR), idiomaPathPt);
            if (!File.Exists(destinoRGNFile))
                arquivoREGION.Salvar(destinoRGNFile);
            else
                if (arquivoREGION.RestaurarVersao(destinoRGNFile))
                    arquivoREGION.Salvar(destinoRGNFile);

            //Criando região Ingles
            string RGN_fileName_EN_US = Util.Util.RGN_EN_US + Util.Util.ARQUIVO_EXT_RGN;
            destinoRGNFile = System.IO.Path.Combine(diretorio_rgn, RGN_fileName_EN_US);
            arquivoREGION = new Arquivo_RGN(Util.Util.Lingua.Ingles);
            string idiomaPathEn = "\\" + Directory.GetParent(destinoFileIngles).Name + "\\" + Path.GetFileNameWithoutExtension(RGN_fileName_EN_US) + Util.Util.ARQUIVO_EXT_LPK;
            arquivoREGION.CriarRegiaoPadrao(Util.Util.Lingua.Ingles, Path.GetFileNameWithoutExtension(RGN_fileName_EN_US), idiomaPathEn);
            if (!File.Exists(destinoRGNFile))
                arquivoREGION.Salvar(destinoRGNFile);
            else
                if (arquivoREGION.RestaurarVersao(destinoRGNFile))
                    arquivoREGION.Salvar(destinoRGNFile);

            //Criando região Espanhol
            string RGN_fileName_ES_ES = Util.Util.RGN_ES_ES + Util.Util.ARQUIVO_EXT_RGN;
            destinoRGNFile = System.IO.Path.Combine(diretorio_rgn, RGN_fileName_ES_ES);
            arquivoREGION = new Arquivo_RGN(Util.Util.Lingua.Espanhol);
            string idiomaPathEs = "\\" + Directory.GetParent(destinoFileEspanhol).Name + "\\" + Path.GetFileNameWithoutExtension(RGN_fileName_ES_ES) + Util.Util.ARQUIVO_EXT_LPK;
            arquivoREGION.CriarRegiaoPadrao(Util.Util.Lingua.Espanhol, Path.GetFileNameWithoutExtension(RGN_fileName_ES_ES), idiomaPathEs);

            if (!File.Exists(destinoRGNFile))
                arquivoREGION.Salvar(destinoRGNFile);
            else
                if (arquivoREGION.RestaurarVersao(destinoRGNFile))
                    arquivoREGION.Salvar(destinoRGNFile);

            //Verificando as regiões salvas pelo usuário
            string[] filesRGN = Directory.GetFiles(diretorio_rgn, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(Util.Util.ARQUIVO_EXT_RGN, StringComparison.OrdinalIgnoreCase)).ToArray();
            foreach (string s in filesRGN)
            {
                string fileName = System.IO.Path.GetFileName(s);
                
                //Se for os arquivos diferentes dos três default(PT-BR, EN-US, ES-ES)
                if (fileName != RGN_fileName_PT_BR && fileName != RGN_fileName_EN_US && fileName != RGN_fileName_ES_ES)
                {
                    string destFile = System.IO.Path.Combine(diretorio_rgn, fileName);
                    string idiomaPath = "";
                    switch (this.lingua)
                    {
                        case Util.Util.Lingua.Portugues: idiomaPath = idiomaPathPt; break;
                        case Util.Util.Lingua.Ingles: idiomaPath = idiomaPathEn; break;
                        case Util.Util.Lingua.Espanhol: idiomaPath = idiomaPathEs; break;
                    }

                    arquivoREGION = new Arquivo_RGN(this.lingua);
                    arquivoREGION.CriarRegiaoPadrao(this.lingua, Path.GetFileNameWithoutExtension(fileName), idiomaPath);
                    
                    if (arquivoREGION.RestaurarVersao(destFile))
                        arquivoREGION.Salvar(destFile);
                    //System.IO.File.Copy(s, destFile, true);
                }
            }





            //Lista de regiões
            Arquivo_LST regioes = new Arquivo_LST();
            List<String> rgns = new List<string>();
            rgns.AddRange(Directory.EnumerateFiles(diretorio_rgn, "*" + Util.Util.ARQUIVO_EXT_RGN, SearchOption.TopDirectoryOnly));
            foreach (string s in rgns)
            {
                regioes.listaPaths.Add(Path.GetFileNameWithoutExtension(s));
            }

            regioes.Salvar(diretorio_rgn + Util.Util.ARQUIVO_LST_RGN);

            #endregion

            #region Fontes

            //fontes

            //Lista de fontes da pasta de instalação
            string[] filesLdX2 = System.IO.Directory.GetFiles(diretorio_aplicacao + Util.Util.DIRETORIO_FONTES);

            if (!Directory.Exists(diretorio_fontes))
            {
                Directory.CreateDirectory(diretorio_fontes);

                //copiar todas as fontes para ProgramData
                foreach (string s in filesLdX2)
                {
                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(diretorio_fontes, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }

            }
            else
            {
                string[] filesProgramData = Directory.GetFiles(diretorio_fontes, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(Util.Util.ARQUIVO_EXT_FNT, StringComparison.OrdinalIgnoreCase)).ToArray();

                Arquivo_FNT fonte = new Arquivo_FNT();

                //Restaurar fontes do programData para a versão atual de arquivo_fnt
                foreach (string s in filesProgramData)
                {
                    //Caso seja necessário restaurar o arquivo para a versão atual, o objeto será devidamente preenchido dentro da função RestaurarVersão
                    if (fonte.RestaurarVersao(s))
                        fonte.Salvar(s);
                }

                //Copiando as fontes do software para ProgramData
                for (int i = 0; i < filesLdX2.Count(); i++)
                {
                    if (!File.Exists(System.IO.Path.Combine(diretorio_fontes, System.IO.Path.GetFileName(filesLdX2[i]))))
                        System.IO.File.Copy(filesLdX2[i], System.IO.Path.Combine(diretorio_fontes, System.IO.Path.GetFileName(filesLdX2[i])), true);
                }
            }

            #endregion

            #region Alternancia

            //alternacia
            if (!Directory.Exists(diretorio_alt))
            {
                Directory.CreateDirectory(diretorio_alt);
            }
            
            
            Arquivo_ALT arquivoAlternancia = new Arquivo_ALT(carregaIdioma());
            string destinoFileALT = diretorio_alt + Util.Util.ARQUIVO_PAINEL_ALTERNANCIA;
            arquivoAlternancia.ArquivoNome = destinoFileALT;

            if (!File.Exists(destinoFileALT))
                arquivoAlternancia.CriarAlternanciasDefault();
            else
            { 
                if (!arquivoAlternancia.RestaurarVersao(destinoFileALT))
                {
                    //atualizar arquivo de alternancia
                    AtualizarLinguaArquivoAlternancia(arquivoAlternancia);
                }
            }


            #endregion

            #region Firmware

            //sempre copiar os arquivos de firmware na pasta de instalação em programData, uso do delete para evitar o envio de lixo na criação do NFX
            if (!Directory.Exists(diretorio_fir))           
                Directory.CreateDirectory(diretorio_fir);

            //copiando os arquivos de onde o Pontos esta instalado
            string[] files = Directory.GetFiles(diretorio_aplicacao + Util.Util.DIRETORIO_FIRMWARE, "*" + Util.Util.ARQUIVO_EXT_FIR);
            foreach (string s in files)
            {
                CopiarFirmwareParaProgramData(s);
            }
            files = Directory.GetFiles(diretorio_aplicacao + Util.Util.DIRETORIO_FIRMWARE, "*" + Util.Util.ARQUIVO_EXT_OPT);

            foreach (string s in files)
            {
                string fileName = Path.GetFileName(s);
                string destFile = Path.Combine(diretorio_fir, fileName);
                File.Copy(s, destFile, true);
            }

            #endregion

            #region Imagens

            //imagens bmp
            string[] imagensLDX2 = System.IO.Directory.GetFiles(diretorio_aplicacao + Util.Util.DIRETORIO_BITMAP);

            if (!Directory.Exists(diretorio_imagens))
            {
                Directory.CreateDirectory(diretorio_imagens);

                //copiar arquivos de alternancia

                foreach (string s in imagensLDX2)
                {
                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(diretorio_imagens, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }

            }
            else
            {
                //Copiando as imagens do software para ProgramData
                for (int i = 0; i < imagensLDX2.Count(); i++)
                {
                    if (!File.Exists(System.IO.Path.Combine(diretorio_imagens, System.IO.Path.GetFileName(imagensLDX2[i]))))
                        System.IO.File.Copy(imagensLDX2[i], System.IO.Path.Combine(diretorio_imagens, System.IO.Path.GetFileName(imagensLDX2[i])), true);
                }
                
            }
            
            #endregion

            #region Relatorio            

            if (!Directory.Exists(diretorio_reports))
            {
                Directory.CreateDirectory(diretorio_reports);
            }

            // Arquivos de Reports 
            string[] reportfilesLDX2 = System.IO.Directory.GetFiles(diretorio_aplicacao + Util.Util.DIRETORIO_REPORTS);
            foreach (string s in reportfilesLDX2)
            {
                string fileName = System.IO.Path.GetFileName(s);
                string destFile = System.IO.Path.Combine(diretorio_reports, fileName);
                System.IO.File.Copy(s, destFile, true);
            }           

            #endregion

            #region Diretorio Temporário

            //Temporário
            Directory.CreateDirectory(diretorio_temporario);

            #endregion

            #region Download
            diretorio_downloads = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_DOWNLOADS;

            //sempre copiar os arquivos de firmware na pasta de instalação em programData, uso do delete para evitar o envio de lixo na criação do NFX
            if (!Directory.Exists(diretorio_downloads))
                Directory.CreateDirectory(diretorio_downloads);

            #endregion Download
        }
       
        public void ApagarLDX2ProgramData()
        {
            if (Directory.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP))
                Directory.Delete(diretorio_raiz + Util.Util.DIRETORIO_APP, true);
            
        }

        public bool GrantAccess()
        {
            //diretório do aplicativo em programData.
            if (!Directory.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP))
            {
                Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_APP);
            }

            DirectoryInfo dInfo = new DirectoryInfo(diretorio_raiz + Util.Util.DIRETORIO_APP);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
            return true;
        }

        #endregion

        #region Alternancia

        public int QuantidadeAgendamentosAlternancia(int controladorSelecionado, int painelSelecionado)
        {
            int quantidade = 0;

            for (int i = 0; i < this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.Count; i++)
            {
                if (this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_ALTERNANCIA)
                    quantidade++;
            }

            return quantidade;
        }

        public void AtualizarLinguaArquivoAlternancia(Util.Util.Lingua novaLingua)
        {
            ResourceManager rmSoftware = carregaIdioma();
            ResourceManager rmAlternancia = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));

            Arquivo_ALT alternancia = new Arquivo_ALT(rmSoftware);
            alternancia.ArquivoNome = diretorio_alt + Util.Util.ARQUIVO_PAINEL_ALTERNANCIA;

            //Setando o resource manager do arquivo de alternancia original
            Util.Util.Lingua linguaAlternancia = this.RetornarLinguaAlternancia(alternancia);
            switch (linguaAlternancia)
            {
                case Util.Util.Lingua.Ingles:
                    rmAlternancia = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Portugues:
                    rmAlternancia = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Espanhol:
                    rmAlternancia = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
            }

            //pegando todo o arquivo de recurso
            List<DictionaryEntry> listaAlternancia = rmAlternancia.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true).OfType<DictionaryEntry>().ToList();


            //alterando a lista e deixando apenas os recursos de arquivo de alternancia        
            int i = 0;
            while (i < listaAlternancia.Count)
            {
                if (listaAlternancia[i].Key.ToString().Contains("ARQUIVO_ALT"))
                    i++;
                else
                    listaAlternancia.RemoveAt(i);
            }

            //alterando a lista padrao de alternancia de cada painel substituindo os valores da alternancia para a lingua do software
            foreach (ItemAlternancia ia in alternancia.listaAlternancias)
            {
                string value = ia.NomeAlternancia.Replace(rmAlternancia.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO"), "").Trim();
                var chave = listaAlternancia.FirstOrDefault(dictionaryEntry => dictionaryEntry.Value.ToString().Trim() == value);

                if (chave.Key != null)
                    ia.NomeAlternancia = rmSoftware.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO") + " " + rmSoftware.GetString(chave.Key.ToString());
            }

            alternancia.Salvar(alternancia.ArquivoNome);


        }

        public bool LiberarPOS()
        {
            return File.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_LIBERACAO);            
        }

        public void AtualizarLinguaArquivoAlternancia(Arquivo_ALT alternancia)
        {
            
            Util.Util.Lingua linguaAlternancia = this.RetornarLinguaAlternancia(alternancia);
            if (linguaAlternancia != this.lingua)
            {
                ResourceManager rmAlternancia = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                ResourceManager rmSoftware = carregaIdioma();

                switch (linguaAlternancia)
                {
                    case Util.Util.Lingua.Ingles:
                        rmAlternancia = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                        break;
                    case Util.Util.Lingua.Portugues:
                        rmAlternancia = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                        break;
                    case Util.Util.Lingua.Espanhol:
                        rmAlternancia = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                        break;
                }

                //pegando todo o arquivo de recurso
                List<DictionaryEntry> listaAlternancia = rmAlternancia.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true).OfType<DictionaryEntry>().ToList();


                //alterando a lista e deixando apenas os recursos de arquivo de alternancia        
                int i = 0;
                while (i < listaAlternancia.Count)
                {
                    if (listaAlternancia[i].Key.ToString().Contains("ARQUIVO_ALT"))
                        i++;
                    else
                        listaAlternancia.RemoveAt(i);
                }

                //alterando a lista padrao de alternancia de cada painel substituindo os valores da alternancia para a lingua do software
                foreach (ItemAlternancia ia in alternancia.listaAlternancias)
                {
                    string value = ia.NomeAlternancia.Replace(rmAlternancia.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO"), "").Trim();
                    var chave = listaAlternancia.FirstOrDefault(dictionaryEntry => dictionaryEntry.Value.ToString().Trim() == value);

                    if (chave.Key != null)
                        ia.NomeAlternancia = rmSoftware.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO") + " " + rmSoftware.GetString(chave.Key.ToString());
                }

                alternancia.Salvar(alternancia.ArquivoNome);

            }   
        }

        public Arquivo_ALT CarregarAlternancia()
        {
            //return CarregarAlternancia(false);
            Arquivo_ALT alternancia = new Arquivo_ALT(this.carregaIdioma());
            alternancia.Abrir(diretorio_alt + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + Util.Util.ARQUIVO_ALT);
            return alternancia;
        }

        public Util.Util.Lingua RetornarLinguaAlternancia(Arquivo_ALT alternancia)
        {
            bool setou = false;
            Util.Util.Lingua linguaAlternancia = this.GetIdiomaFachada();
            alternancia.Abrir(alternancia.ArquivoNome);


            ResourceManager rm = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
            if (alternancia.listaAlternancias[0].NomeAlternancia.Contains(rm.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO")))
            {
                setou = true;
                linguaAlternancia = Util.Util.Lingua.Ingles;
            }

            if (!setou)
            {
                rm = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                if (alternancia.listaAlternancias[0].NomeAlternancia.Contains(rm.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO")))
                {
                    setou = true;
                    linguaAlternancia = Util.Util.Lingua.Espanhol;
                }
            }

            if (!setou)
            {
                rm = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                if (alternancia.listaAlternancias[0].NomeAlternancia.Contains(rm.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO")))
                {
                    setou = true;
                    linguaAlternancia = Util.Util.Lingua.Portugues;
                }
            }

            return linguaAlternancia;
        }

        public List<ItemAlternancia> CarregarAlternanciaPadraoPainel()
        {
            Arquivo_ALT alternancia = new Arquivo_ALT(this.carregaIdioma());
            alternancia.Abrir(diretorio_alt + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + Util.Util.ARQUIVO_ALT);

            while (alternancia.listaAlternancias.Count > Util.Util.INDICE_ALTERNANCIA_PADRAO_PAINEL)
            {
                alternancia.listaAlternancias.RemoveAt(alternancia.listaAlternancias.Count - 1);
            }

            return alternancia.listaAlternancias;
        }

        public List<ItemAlternancia> CarregarAlternanciaPadraoPainelMultiLinhas()
        {
            Arquivo_ALT alternancia = new Arquivo_ALT(this.carregaIdioma());
            alternancia.Abrir(diretorio_alt + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + Util.Util.ARQUIVO_ALT);

            while (alternancia.listaAlternancias.Count > Util.Util.INDICE_ALTERNANCIA_PADRAO_PAINEL_MULTI_LINHAS)
            {
                alternancia.listaAlternancias.RemoveAt(alternancia.listaAlternancias.Count - 1);
            }

            return alternancia.listaAlternancias;
        }

        public List<ItemAlternancia> CarregarAlternanciaPainel(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].ListaAlternancias;
        }

        public void SalvarAlternancia(Arquivo_ALT aalt, int painelSelecionado)
        {
            string diretorioAlternacia = diretorio_alt + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                painelSelecionado.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS) +
                Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS;
            diretorioAlternacia = diretorioAlternacia.Replace("\\\\", "\\");

            if (!Directory.Exists(diretorioAlternacia))
            {
                Directory.CreateDirectory(diretorioAlternacia);
            }
            aalt.Salvar(diretorioAlternacia + Util.Util.ARQUIVO_ALT);
        }

        public void SalvarAlternancia(Arquivo_ALT aalt)
        {
            aalt.Salvar(diretorio_alt + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + Util.Util.ARQUIVO_ALT);
        }

        public void CriarAlterananciaDefault(Arquivo_ALT aalt)
        {
            aalt.CriarAlternanciasDefault();
        }

        public List<String> CarregarTiposExibicao()
        {
            List<String> lista = new List<String>();
            ResourceManager rm = carregaIdioma();

            lista.Clear();
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_ROTEIRO"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_NUMERO"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_MENSAGEM"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_HORA_SAIDA"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_HORA"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_DATA_HORA"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_TARIFA"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_SAUDACAO"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_TEMPERATURA"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_HORA_TEMPERATURA"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_VELOCIDADE"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_MENSAGEM_SECUNDARIA"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_ID_MOTORISTA"));
            lista.Add(rm.GetString("ARQUIVO_ALT_TIPO_EXIBICAO_NOME_MOTORISTA"));

            return lista;
        }

        public void RestaurarArquivoAlternancia()
        {
           // File.Copy(Directory.GetCurrentDirectory() + Util.Util.DIRETORIO_ALT + Util.Util.ARQUIVO_PAINEL_ALTERNANCIA, diretorio_alt + Util.Util.ARQUIVO_PAINEL_ALTERNANCIA, true);
            Arquivo_ALT arquivoAlternancia = new Arquivo_ALT(carregaIdioma());

            string destinoFileALT = diretorio_alt + Util.Util.ARQUIVO_PAINEL_ALTERNANCIA;

            arquivoAlternancia.ArquivoNome = destinoFileALT;

            arquivoAlternancia.CriarAlternanciasDefault();
        }

        public void SetarListaAlternanciaPainel(int controladorSelecionado, int painelSelecionado, List<ItemAlternancia> listaAlternancias)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].ListaAlternancias.Clear();
            foreach (ItemAlternancia item in listaAlternancias)
            {
                this.controlador[controladorSelecionado].Paineis[painelSelecionado].ListaAlternancias.Add(new ItemAlternancia(item));
            }
        }


        #endregion

        #region RGN

        public String GetNomeRegiao(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].Regiao.Nome.Trim().TrimEnd('\0');
        }
        public int GetIndiceRegiao(int controladorSelecionado)
        {
            List<String> lista = ListaRgns();
            int resposta = 0;
            try
            {
                resposta = lista.IndexOf(this.controlador[controladorSelecionado].Regiao.Nome.Trim().TrimEnd('\0'));
            }
            catch
            { }
            return resposta;            
        }
        public void SalvarRGN(Arquivo_RGN argn)
        {
            argn.Salvar(diretorio_rgn + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + Encoding.ASCII.GetString(argn.nome).Trim().TrimEnd('\0') + Util.Util.ARQUIVO_EXT_RGN);
        }

        public List<String> ListaRgns()
        {
            List<String> regioes = new List<String>();
            //var arquivos_fonte = Directory.EnumerateFiles(diretorio_rgn, "*.rgn", SearchOption.TopDirectoryOnly);
            var arquivos_fonte = Directory.GetFiles(diretorio_rgn, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(Util.Util.ARQUIVO_EXT_RGN, StringComparison.OrdinalIgnoreCase));

            foreach (string arquivo in arquivos_fonte)
            {
                Persistencia.Arquivo_RGN argn = new Persistencia.Arquivo_RGN();

                argn.Abrir(arquivo);
                regioes.Add(Encoding.ASCII.GetString(argn.nome).Trim().TrimEnd('\0'));
            }

            return regioes;
        }

        public void setarRgnControlador(int controladorSelecionado, String nomeRegiao)
        {
            this.controlador[controladorSelecionado].Regiao = CarregarRegiao(nomeRegiao);
            this.controlador[controladorSelecionado].ParametrosVariaveis.RegiaoSelecionada = (byte)GetIndiceRegiao(controladorSelecionado);
            //this.controlador[controladorSelecionado].CONTROLADOR_MODIFICADO = true;
        }

        public Arquivo_RGN CarregarRegiao(String nomeRegiao)
        {
            nomeRegiao = nomeRegiao.Replace("\0", String.Empty);

            Persistencia.Arquivo_RGN argn = new Persistencia.Arquivo_RGN();
            if (!File.Exists(diretorio_rgn + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + nomeRegiao + Util.Util.ARQUIVO_EXT_RGN))
            {
                CriaDirs();    
            }            
            return (Arquivo_RGN)argn.Abrir(diretorio_rgn + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + nomeRegiao + Util.Util.ARQUIVO_EXT_RGN);
        }


        #endregion

        #region LPK

        public void SalvarLPK(Arquivo_LPK alpk)
        {
            alpk.Salvar(diretorio_lpk + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + alpk.nome + Util.Util.ARQUIVO_EXT_LPK);
        }

        public List<String> ListaLPKs()
        {
            var arquivos_fonte = Directory.EnumerateFiles(diretorio_lpk, "*" + Util.Util.ARQUIVO_EXT_LPK, SearchOption.AllDirectories);
            List<String> retorno = new List<String>();
            Arquivo_LPK alpk = new Persistencia.Arquivo_LPK(carregaIdioma());

            if (arquivos_fonte.Count() == 0)
            {
                //this.alpk.Salvar(diretorio_lpk + Util.Util.NOME_IDIOMA + Util.Util.ARQUIVO_EXT_LPK);
                alpk.Salvar(diretorio_lpk + Util.Util.NOME_IDIOMA + Util.Util.ARQUIVO_EXT_LPK);
            //==============================================================================================================
            //Enumerate files aparentemente já monitora o novo arquivo e atribui os nomes novos à variável de arquivo_fonte.
            //==============================================================================================================
            }


            foreach (string arquivo in arquivos_fonte)
            {
               
                //this.alpk.Abrir(arquivo);
                alpk.Abrir(arquivo);
                int indice_barra_zero = alpk.nome.IndexOf('\0');

                if (indice_barra_zero > 0)
                    //retorno.Add(this.alpk.nome.Trim().Substring(0, indice_barra_zero));
                    retorno.Add(alpk.nome.Trim().Substring(0, indice_barra_zero));
                else 
                    //retorno.Add(this.alpk.nome.Trim());
                    retorno.Add(alpk.nome.Trim());
            }

            return retorno;
        }

        public List<String> ExibeLPK(String ArquivoNome)
        {
            List<String> retorno = new List<string>();
            String diretorio = diretorio_lpk +
                               ArquivoNome +
                               Util.Util.ARQUIVO_EXT_LPK;

            Arquivo_LPK alpk = new Persistencia.Arquivo_LPK(carregaIdioma());
            //this.alpk.Abrir(diretorio);
            alpk.Abrir(diretorio);

            //foreach (String texto in this.alpk.texto)
            foreach (String texto in alpk.texto)
            {
                retorno.Add(texto);
            }

            return retorno;
        }

        public List<String> ExibeLPK(String ArquivoNome, Arquivo_LPK arquivo_lpk)
        {
            List<String> retorno = new List<string>();
            String diretorio = diretorio_lpk +
                               ArquivoNome +
                               Util.Util.ARQUIVO_EXT_LPK;

            arquivo_lpk.Abrir(diretorio);

            foreach (String texto in arquivo_lpk.texto)
            {
                retorno.Add(texto);
            }

            return retorno;
        }

        public string[] CarregarFrasesLpkDefault()
        {
            Arquivo_LPK alpk = new Persistencia.Arquivo_LPK(carregaIdioma());
            return alpk.CarregarFrasesDefault();
        }

        public string[] CarregarFrasesLpkDefault(Arquivo_LPK arquivo_lpk)
        {
            return arquivo_lpk.CarregarFrasesDefault();
        }

        #endregion

        #region Controlador

        public void SetCancelarTransmissaoNFX(int controladorSelecionado, bool cancelar)
        {
            this.controlador[controladorSelecionado].CancelarEnvioNFX = cancelar;
        }

        public bool GetCancelarEnvioNFX(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].CancelarEnvioNFX;
        }

        public void AtualizarIdiomaAlternanciaPadraoPainelTodosControladores()
        {
            for (int i = 0; i < this.controlador.Count; i++)
                AtualizarIdiomaAlternanciaPadrao(i);
        }


        public bool ChecarFontesControlador(int controladorSelecionado)
        {
            bool achouFonte = true;

            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                //Refazendo todos os textos dos roteiros
                foreach (Roteiro r in p.Roteiros)
                {
                    if (!ChecarFonteFrase(r.Numero))
                        achouFonte = false;

                    foreach (Frase f in r.FrasesIda)
                        if (!ChecarFonteFrase(f))
                            achouFonte = false;

                    foreach (Frase f in r.FrasesVolta)
                        if (!ChecarFonteFrase(f))
                            achouFonte = false;
                }

                //Refazendo todos os textos das mensagens
                foreach (Mensagem m in p.Mensagens)
                    foreach (Frase f in m.Frases)
                        if (!ChecarFonteFrase(f))
                            achouFonte = false;

                //Refazendo a mensagem de emergencia
                foreach (Frase f in p.MensagemEmergencia.Frases)
                    if (!ChecarFonteFrase(f))
                        achouFonte = false;

                //Refazendo as mensagens especiais
                foreach (Frase f in p.MensagensEspeciais.Frases)
                    if (!ChecarFonteFrase(f))
                        achouFonte = false;

                if (!achouFonte)
                    p.FontePath = Util.Util.TrataDiretorio(Util.Util.DIRETORIO_FONTES + p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel].Modelo.Textos[0].Fonte + Util.Util.ARQUIVO_EXT_FNT);
            }

            return achouFonte;
        }


        public void MudarTarifaTodosRoteiros(int controladoSelecionado, int tarifa)
        {
            foreach (Painel p in controlador[controladoSelecionado].Paineis)
            {
                foreach (Roteiro r in p.Roteiros)
                {
                    r.Tarifa = tarifa;
                }
            }
        }

        public int MudarTarifaDePara(int controladoSelecionado, int tarifaDe, int tarifaPara)
        {
            int qtdAlterados = 0;
            foreach (Painel p in controlador[controladoSelecionado].Paineis)
            {
                foreach (Roteiro r in p.Roteiros)
                {
                    if (r.Tarifa == tarifaDe)
                    { 
                        r.Tarifa = tarifaPara;
                        qtdAlterados = qtdAlterados + 1;
                    }
                }
            }
            return qtdAlterados;
        }

        public int QuantidadeControladores()
        {
            return this.controlador.Count();
        }

        public void ControladorDefault(ResourceManager rm, Arquivo_RGN regiao)
        {
            Controlador cTemp = new Controlador(rm, regiao);

            //cria os paineis.
            for (int painel = 0; painel < cTemp.NUM_PAINEIS; painel++)
            {
                Painel temp = new Painel(rm, regiao);

                temp.Indice = painel;
                temp.Altura = 16;
                temp.Largura = 144;
                temp.PainelDefault(1, 1, painel);

                //Incluindo a lista de alternancias padrão ao painel
                temp.ListaAlternancias = this.CarregarAlternanciaPadraoPainel();
                
                cTemp.Paineis.Add(temp);
            }

            this.controlador.Add(cTemp);
        } 

        public void Salvar(String ArquivoNome, int indiceControlador)
        {
            this.controlador[indiceControlador].Salvar(ArquivoNome);
        }       

        public void Abrir(String ArquivoNome)
        {
            try
            {
                Controlador cTemp;

                //Verificando se o arquivo é zipado ou não
                if (ZipFile.IsZipFile(ArquivoNome))
                {

                    //deletando arquivos .ldx2 se houver na pasta
                    Directory.EnumerateFiles(diretorio_raiz + Util.Util.DIRETORIO_APP, "*" + Util.Util.ARQUIVO_EXT_LDX2_MAIUSCULO).ToList().ForEach(x => File.Delete(x));

                    //Extraindo arquivo zip para a pasta LDX2 em programdata
                    using (ZipFile zipFile = ZipFile.Read(ArquivoNome))
                    {
                        zipFile.ExtractAll(diretorio_raiz + Util.Util.DIRETORIO_APP, ExtractExistingFileAction.OverwriteSilently);
                    }

                    //Abrindo o controlador com o arquivo descompactado
                    string[] files = System.IO.Directory.GetFiles(diretorio_raiz + Util.Util.DIRETORIO_APP, "*" + Util.Util.ARQUIVO_EXT_LDX2_MAIUSCULO);

                    cTemp = AbrirControlador(diretorio_raiz + Util.Util.DIRETORIO_APP + Path.GetFileName(files[0]));

                    //Apagando o arquivo temporario
                    File.Delete(diretorio_raiz + Util.Util.DIRETORIO_APP + Path.GetFileName(files[0]));
                }
                else
                    //Abrindo o arquivo sem ser zipado
                    cTemp = AbrirControlador(ArquivoNome);



                //o json não serializa resourcemanager
                cTemp.Rm = this.carregaIdioma();

                
                foreach (Painel p in cTemp.Paineis)
                {
                    //o json não serializa resourcemanager
                    p.rm = this.carregaIdioma();
                    p.MensagemEmergencia.rm = this.carregaIdioma();
                    p.MensagensEspeciais.rm = this.carregaIdioma();

                    //carregando a regiao das mensagens especiais
                    p.MensagensEspeciais.regiao = new Arquivo_RGN(cTemp.Regiao);

                    if (p.MensagemEmergencia.Frases.Count > 1)
                        p.MensagemEmergencia.Frases.RemoveAt(0);

                    int i = 0;
                    foreach (Roteiro r in p.Roteiros)
                    {
                        if (r.Numero.Modelo.Textos.Count > 1)
                            r.Numero.Modelo.Textos.RemoveAt(0);

                        //verifica se vem de arquivo anterior ao Uso do ID e da ordenação. Se o ID for igual a zero é porque não estava sendo usado. O valor mínimo é 1
                        //Refaz os id's, os indices, coluna de ordenação e tipo da ordenação
                        if (r.ID == 0)
                        {
                            r.ID = i + 1;
                            r.Indice = i;
                            r.Ascendente = true;
                            r.Ordenacao = 0;
                        }

                        ValidarFrasesV04(r.FrasesIda, p.Altura, p.Largura);
                        ValidarFrasesV04(r.FrasesVolta, p.Altura, p.Largura);

                        i++;
                    }

                    

                    //verifica se vem de arquivo anterior ao Uso do ID e da ordenação. Se o ID for igual a zero é porque não estava sendo usado. O valor mínimo é 1
                    //Refaz os id's, os indices, coluna de ordenação e tipo da ordenação
                    i = 0;
                    foreach (Mensagem m in p.Mensagens)
                    {
                        if (m.ID == 0)
                        {
                            m.ID = i + 1;
                            m.Indice = i;
                            m.Ascendente = true;
                            m.Ordenacao = 0;
                        }

                        ValidarFrasesV04(m.Frases, p.Altura, p.Largura);

                        i++;
                    }

                    foreach (Motorista m in p.Motoristas)
                    {
                        if (m.ID.Modelo.Textos.Count > 1)
                            m.ID.Modelo.Textos.RemoveAt(0);
                        if (m.Nome.Modelo.Textos.Count > 1)
                            m.Nome.Modelo.Textos.RemoveAt(0);
                    }

                    //lista de mensagens especiais
                    if (p.MensagensEspeciais.Frases.Count != p.MensagensEspeciais.QTD_FRASES)
                    {
                        p.MensagensEspeciais = new MensagemEspecial(p.MensagensEspeciais.rm, p.MensagensEspeciais.regiao);

                        //apenas setando a altura do texto para a função de verificação das fontes carregar a fonte correta e exibir ao usuário que não achou a fonte correta
                        foreach (Frase f in p.MensagensEspeciais.Frases)
                            f.Modelo.Textos[0].Altura = p.Altura;
                        //SetarFontesDefaultFrases(f, p.Altura);
                    }

                    
                    //Incluindo a lista de alternancias padrão ao painel se não houver itens na lista recuperada
                    if (p.ListaAlternancias.Count == 0)
                        p.ListaAlternancias = this.CarregarAlternanciaPadraoPainel();
                }

                //Tratando as horas do parametrofixo. Alguns arquivos .ldx2 antigos possuiam o campo de boa dia, boa tarde e boa noite com valor 0 sobrescrevendo assim o construtor que vem com bom dia=4, boa tarde=12 e boa noite =18
                if (cTemp.ParametrosFixos.HoraInicioDia == 0 && cTemp.ParametrosFixos.HoraInicioTarde == 0 && cTemp.ParametrosFixos.HoraInicioNoite == 0)
                {
                    cTemp.ParametrosFixos.HoraInicioDia = 4;
                    cTemp.ParametrosFixos.HoraInicioTarde = 12;
                    cTemp.ParametrosFixos.HoraInicioNoite = 18;
                }

                this.controlador.Add(cTemp);

                //verificando e atualizando se necessário a lingua da alternancia padrao de cada painel para a lingua setada no software
                AtualizarIdiomaAlternanciaPadrao(this.controlador.Count - 1);
            }
            catch (FileNotFoundException fnfe)
            {
                string message = fnfe.FileName;                

                message = "File : " + message;
            }
            catch
            {
                //Verificando se o arquivo é zipado ou não
                if (ZipFile.IsZipFile(ArquivoNome))
                {

                    //deletando arquivos .ldx2 se houver na pasta
                    Directory.EnumerateFiles(diretorio_raiz + Util.Util.DIRETORIO_APP, "*" + Util.Util.ARQUIVO_EXT_LDX2_MAIUSCULO).ToList().ForEach(x => File.Delete(x));

                    //Extraindo arquivo zip para a pasta LDX2 em programdata
                    using (ZipFile zipFile = ZipFile.Read(ArquivoNome))
                    {
                        zipFile.ExtractAll(diretorio_raiz + Util.Util.DIRETORIO_APP, ExtractExistingFileAction.OverwriteSilently);
                    }

                    //Abrindo o controlador com o arquivo descompactado
                    string[] files = System.IO.Directory.GetFiles(diretorio_raiz + Util.Util.DIRETORIO_APP, "*" + Util.Util.ARQUIVO_EXT_LDX2_MAIUSCULO);

                    //cTemp = AbrirControlador(diretorio_raiz + Util.Util.DIRETORIO_APP + Path.GetFileName(files[0]));
                    AbrirControladorVersaoAntigaLDX2(diretorio_raiz + Util.Util.DIRETORIO_APP + Path.GetFileName(files[0]));

                    //Apagando o arquivo temporario
                    File.Delete(diretorio_raiz + Util.Util.DIRETORIO_APP + Path.GetFileName(files[0]));


                    ////Abrindo o controlador com o arquivo descompactado
                    //AbrirControladorVersaoAntigaLDX2(diretorio_raiz + Util.Util.DIRETORIO_APP + Path.GetFileName(ArquivoNome));

                    ////Apagando o arquivo temporario
                    //File.Delete(diretorio_raiz + Util.Util.DIRETORIO_APP + Path.GetFileName(ArquivoNome));
                }
                else
                    //Abrindo o arquivo sem ser zipado
                    AbrirControladorVersaoAntigaLDX2(ArquivoNome);                

                foreach (Painel p in this.controlador[controlador.Count - 1].Paineis)
                {
                    //Recarregando os Resources Managers
                    p.rm = this.carregaIdioma();
                    p.MensagemEmergencia.rm = this.carregaIdioma();
                    p.MensagensEspeciais.rm = this.carregaIdioma();
                    p.MensagensEspeciais.regiao = this.controlador[controlador.Count - 1].Regiao;

                    //Verificando se possui Roteiro
                    if (p.Roteiros.Count == 0)
                    {
                        Roteiro rTemp = new Roteiro((1).ToString("00"));
                        rTemp.ID = 1;
                        rTemp.Indice = 0;
                        rTemp.LabelRoteiro = p.rm.GetString("ROTEIRO_LABEL") + " " + 1;
                        rTemp.Ascendente = true;
                        rTemp.Ordenacao = 0;
                        //apenas setando a altura do texto para a função de verificação das fontes carregar a fonte correta e exibir ao usuário que não achou a fonte correta
                        rTemp.Numero.Modelo.Textos[0].Altura = p.Altura;
                        //SetarFontesDefaultFrases(rTemp.Numero, p.Altura);
                        p.Roteiros.Add(rTemp);
                    }

                    //Verificando se possui Mensagem
                    if (p.Mensagens.Count == 0)
                    {
                        Mensagem mTemp = new Mensagem(p.rm.GetString("MENSAGEM_LABEL").ToString());
                        mTemp.ID = 1;
                        mTemp.Indice = 0;
                        mTemp.Ascendente = true;
                        mTemp.Ordenacao = 0;
                        //apenas setando a altura do texto para a função de verificação das fontes carregar a fonte correta e exibir ao usuário que não achou a fonte correta
                        mTemp.Frases[0].Modelo.Textos[0].Altura = p.Altura;
                        //SetarFontesDefaultFrases(mTemp.Frases[0], p.Altura);
                        p.Mensagens.Add(mTemp);
                    }

                    //lista de mensagens especiais
                    if (p.MensagensEspeciais.Frases.Count != p.MensagensEspeciais.QTD_FRASES)
                    {
                        p.MensagensEspeciais = new MensagemEspecial(p.MensagensEspeciais.rm, p.MensagensEspeciais.regiao);

                        //apenas setando a altura do texto para a função de verificação das fontes carregar a fonte correta e exibir ao usuário que não achou a fonte correta
                        foreach (Frase f in p.MensagensEspeciais.Frases)
                            f.Modelo.Textos[0].Altura = p.Altura;
                            //SetarFontesDefaultFrases(f, p.Altura);
                    }
                    else
                    {
                        //apenas setando a altura do texto para a função de verificação das fontes carregar a fonte correta e exibir ao usuário que não achou a fonte correta
                        foreach (Frase f in p.MensagensEspeciais.Frases)
                            if (f.Modelo.Textos[0].Altura == 0)
                                f.Modelo.Textos[0].Altura = p.Altura;
                    }

                    //verificando a mensagem de emergência
                    if (p.MensagemEmergencia.Frases[0].Modelo.Textos[0].Altura == 0)
                    {
                        p.MensagemEmergencia = new MensagemEmergencia(p.MensagemEmergencia.rm);
                        p.MensagemEmergencia.Frases[0].Modelo.Textos[0].Altura = p.Altura;
                    }
                        //SetarFontesDefaultFrases(p.MensagemEmergencia.Frases[0], p.Altura);


                    //Incluindo a lista de alternancias padrão ao painel se não houver itens na lista recuperada
                    if (p.ListaAlternancias.Count == 0)
                        p.ListaAlternancias = this.CarregarAlternanciaPadraoPainel();

                    
                }

                //Tratando as horas do parametrofixo. Alguns arquivos .ldx2 antigos possuiam o campo de boa dia, boa tarde e boa noite com valor 0 sobrescrevendo assim o construtor que vem com bom dia=4, boa tarde=12 e boa noite =18
                if (controlador[controlador.Count - 1].ParametrosFixos.HoraInicioDia == 0 && controlador[controlador.Count - 1].ParametrosFixos.HoraInicioTarde == 0 && controlador[controlador.Count - 1].ParametrosFixos.HoraInicioNoite == 0)
                {
                    controlador[controlador.Count - 1].ParametrosFixos.HoraInicioDia = 4;
                    controlador[controlador.Count - 1].ParametrosFixos.HoraInicioTarde = 12;
                    controlador[controlador.Count - 1].ParametrosFixos.HoraInicioNoite = 18;
                }


                //verificando e atualizando se necessário a lingua da alternancia padrao de cada painel para a lingua setada no software
                AtualizarIdiomaAlternanciaPadrao(this.controlador.Count - 1);
            }
        }

        public bool RetornarModoApresentacaoLD6(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.ModoApresentacaoDisplayLD6;
        }

        public Controlador AbrirControlador(String ArquivoNome)
        {
            if (ArquivoNome.EndsWith(Util.Util.ARQUIVO_EXT_LDX) || ArquivoNome.EndsWith(Util.Util.ARQUIVO_EXT_LDX.ToUpper()))
            {
                return ImportarControladorLDX(ArquivoNome);
            }
            //TODO: Verificar a possibilidade de usar o método abrir dentro da classe Controlador.
            String Json = String.Empty;

            Json = System.IO.File.ReadAllText(ArquivoNome);

            return JsonConvert.DeserializeObject<Controlador>(Json);
        }

        public void AbrirControladorVersaoAntigaLDX2(String ArquivoNome)
        {
            if (Path.GetExtension(ArquivoNome).Equals(Util.Util.ARQUIVO_EXT_LDX))
            {
                // Todo: Verificar se será necessário criar uma exceção
                return;
            }
            TextReader texto = File.OpenText(ArquivoNome);
            string textao = System.IO.File.ReadAllText(ArquivoNome);
            JsonTextReader jtr = new JsonTextReader(texto);

            switch (this.lingua)
            {
                case Util.Util.Lingua.Portugues:
                    controlador.Add(new Controlador(this.carregaIdioma(), this.CarregarRegiao(Util.Util.RGN_PT_BR)));
                    break;
                case Util.Util.Lingua.Ingles:
                    controlador.Add(new Controlador(this.carregaIdioma(), this.CarregarRegiao(Util.Util.RGN_EN_US)));
                    break;
                case Util.Util.Lingua.Espanhol:
                    controlador.Add(new Controlador(this.carregaIdioma(), this.CarregarRegiao(Util.Util.RGN_ES_ES)));
                    break;
            }


            controlador[controlador.Count - 1].Paineis.Clear();

            while (jtr.TokenType != JsonToken.EndObject)
            {
                jtr.Read();
                switch (jtr.TokenType)
                {
                    case JsonToken.PropertyName:
                        {
                            if (VerificarJsonValue("NUM_PAINEIS", jtr))
                            {
                                controlador[controlador.Count - 1].NUM_PAINEIS = GetJsonInteiro(jtr);
                            }

                            //if (VerificarJsonValue("CONTROLADOR_MODIFICADO", jtr))
                            //{
                            //    controlador[controlador.Count - 1].CONTROLADOR_MODIFICADO = GetJsonBooleano(jtr);
                            //}

                            if (VerificarJsonValue("Rm", jtr))
                            {
                                while (jtr.TokenType != JsonToken.EndObject)
                                {
                                    jtr.Read();
                                }
                                jtr.Read();

                            }
                            if (VerificarJsonValue("Regiao", jtr))
                            {
                                string textinho = textao.Substring(textao.IndexOf("Regiao") + 9); // 6 do Length de Regiao + 3 das aspas, dois pontos e espaço.
                                textinho = textinho.Substring(0, textinho.IndexOf('}') + 1); // +1 para que o fecha chave entre também.
                                while (jtr.TokenType != JsonToken.EndObject)
                                {
                                    jtr.Read();
                                }
                                jtr.Read();
                                controlador[controlador.Count - 1].Regiao = (Arquivo_RGN)JsonConvert.DeserializeObject<Arquivo_RGN>(textinho);
                                //controlador[controlador.Count - 1].Regiao = (null == jtr.Value) ? CarregarRegiao(Util.Util.NOME_REGIAO) : (Arquivo_RGN)JsonConvert.DeserializeObject<Arquivo_RGN>(textinho);                                    

                            }
                            if (VerificarJsonValue("Familia", jtr))
                            {
                                controlador[controlador.Count - 1].Familia = GetJsonString(jtr);
                            }

                            if (VerificarJsonValue("ParametrosFixos", jtr))
                            {
                                ExecutarJsonRead_N_Vezes(2, jtr);

                                if (VerificarJsonValue("somenteHora", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.somenteHora = CarregarJsonOpcoesApresentacao(jtr);
                                }
                                if (VerificarJsonValue("dataHora", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.dataHora = CarregarJsonOpcoesApresentacao(jtr);
                                }
                                if (VerificarJsonValue("horaSaida", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.horaSaida = CarregarJsonOpcoesApresentacao(jtr);
                                }
                                if (VerificarJsonValue("temperatura", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.temperatura = CarregarJsonOpcoesApresentacao(jtr);
                                }
                                if (VerificarJsonValue("tarifa", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.tarifa = CarregarJsonOpcoesApresentacao(jtr);
                                }
                                if (VerificarJsonValue("horaTemperatura", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.horaTemperatura = CarregarJsonOpcoesApresentacao(jtr);
                                }
                                if (VerificarJsonValue("QtdRoteiros", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.QtdRoteiros = GetJsonInteiro(jtr);
                                    GetJsonInteiro(jtr);
                                }
                                if (VerificarJsonValue("QtdMensagens", jtr))
                                {
                                    ExecutarJsonRead_N_Vezes(3, jtr);
                                }                                
                                if (VerificarJsonValue("FrasesFixasLCD", jtr))
                                {
                                    ExecutarJsonRead_N_Vezes(3, jtr);
                                }                                
                                if (VerificarJsonValue("BloqueioFuncoes", jtr))
                                {
                                    jtr.Read();
                                    if (jtr.TokenType == JsonToken.StartArray)
                                    {
                                        while (jtr.TokenType != JsonToken.EndArray)
                                        {
                                            jtr.Read();
                                        }

                                    }
                                    if ((null == jtr.Value) || (jtr.TokenType == JsonToken.Integer))
                                    {
                                        jtr.Read();
                                    }
                                    //controlador[controlador.Count - 1].ParametrosFixos.BloqueioFuncoes = Convert.ToInt32(jtr.Value.ToString());
                                }
                                if (VerificarJsonValue("FormatoRotulo", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.FormatoRotulo = GetJsonInteiro(jtr);
                                    GetJsonInteiro(jtr);
                                }
                                if (VerificarJsonValue("HoraInicioDia", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.HoraInicioDia = GetJsonInteiro(jtr);
                                }
                                if (VerificarJsonValue("HoraInicioTarde", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.HoraInicioTarde = GetJsonInteiro(jtr);
                                }
                                if (VerificarJsonValue("HoraInicioNoite", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.HoraInicioNoite = GetJsonInteiro(jtr);
                                }

                                if (VerificarJsonValue("CaminhoMensagensEspeciais", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.CaminhoMensagensEspeciais = GetJsonInteiro(jtr);
                                    GetJsonInteiro(jtr);
                                }
                                if (VerificarJsonValue("ApresentacaoMensagensEspeciais", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.ApresentacaoMensagensEspeciais = GetJsonInteiro(jtr);
                                    GetJsonInteiro(jtr);
                                }
                                if (VerificarJsonValue("TextoBomDia", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.TextoBomDia = GetJsonString(jtr);
                                    GetJsonString(jtr);
                                }
                                if (VerificarJsonValue("TextoBoaTarde", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.TextoBoaTarde = GetJsonString(jtr);
                                    GetJsonString(jtr);
                                }
                                if (VerificarJsonValue("TextoBoaNoite", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.TextoBoaNoite = GetJsonString(jtr);
                                    GetJsonString(jtr);
                                }
                                if (VerificarJsonValue("QtdPaineis", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosFixos.QtdPaineis = GetJsonInteiro(jtr);
                                }
                                if (VerificarJsonValue("BomDiaPath", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.BomDiaPath = GetJsonString(jtr);
                                    GetJsonString(jtr);
                                }
                                if (VerificarJsonValue("BoaTardePath", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.BoaTardePath = GetJsonString(jtr);
                                    GetJsonString(jtr);
                                }
                                if (VerificarJsonValue("BoaNoitePath", jtr))
                                {
                                    //controlador[controlador.Count - 1].ParametrosFixos.BoaNoitePath = GetJsonString(jtr);
                                    GetJsonString(jtr);
                                }
                                jtr.Read();
                                //controlador[controlador.Count - 1].ParametrosFixos = (ParametrosFixos)JsonConvert.DeserializeObject<ParametrosFixos>(textinho);
                            }

                            if (VerificarJsonValue("ParametrosVariaveis", jtr))
                            {
                                ExecutarJsonRead_N_Vezes(2, jtr);

                                if (VerificarJsonValue("RoteiroSelecionado", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosVariaveis.RoteiroSelecionado = (uint)GetJsonInteiro(jtr);
                                }
                                if (VerificarJsonValue("HoraSaida", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosVariaveis.HoraSaida = GetJsonByte(jtr);
                                }
                                if (VerificarJsonValue("MinutosSaida", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosVariaveis.MinutosSaida = GetJsonByte(jtr);
                                }
                                if (VerificarJsonValue("RegiaoSelecionada", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosVariaveis.RegiaoSelecionada = GetJsonByte(jtr);
                                }
                                if (VerificarJsonValue("SentidoIda", jtr))
                                {
                                    controlador[controlador.Count - 1].ParametrosVariaveis.SentidoIda = GetJsonBooleano(jtr);
                                }

                                jtr.Read();
                            }

                            if (VerificarJsonValue("Paineis", jtr))
                            {
                                ExecutarJsonRead_N_Vezes(3, jtr);

                                for (int i = 0; i < controlador[controlador.Count - 1].NUM_PAINEIS; i++)
                                {
                                    Painel p = new Painel();
                                    if (VerificarJsonValue("Indice", jtr))
                                    {
                                        p.Indice = GetJsonInteiro(jtr);
                                    }
                                    if (VerificarJsonValue("Altura", jtr))
                                    {
                                        p.Altura = GetJsonInteiro(jtr);
                                    }
                                    if (VerificarJsonValue("Largura", jtr))
                                    {
                                        p.Largura = GetJsonInteiro(jtr);
                                    }
                                    if (VerificarJsonValue("Mensagens", jtr))
                                    {
                                        p.Mensagens.Clear();
                                        jtr.Read();
                                        if (jtr.TokenType == JsonToken.StartArray)
                                        {
                                            while (jtr.TokenType != JsonToken.EndArray)
                                            {
                                                jtr.Read();
                                                if (jtr.TokenType == JsonToken.EndArray)
                                                {
                                                    continue;
                                                }
                                                jtr.Read();

                                                Mensagem m = new Mensagem();
                                                if (VerificarJsonValue("ID", jtr))
                                                {
                                                    m.ID = GetJsonInteiro(jtr);
                                                }
                                                if (VerificarJsonValue("Indice", jtr))
                                                {
                                                    m.Indice = GetJsonInteiro(jtr);
                                                }
                                                if (VerificarJsonValue("LabelMensagem", jtr))
                                                {
                                                    m.LabelMensagem = GetJsonString(jtr);
                                                }
                                                if (VerificarJsonValue("Frases", jtr))
                                                {
                                                    m.Frases.AddRange(CarregarJsonFrases(jtr));
                                                    
                                                    

                                                    ValidarFrasesV04(m.Frases, p.Altura, p.Largura);

                                                    //jtr.Read();
                                                    //if (jtr.TokenType == JsonToken.StartArray)
                                                    //{
                                                    //    while (jtr.TokenType != JsonToken.EndArray)
                                                    //    {
                                                    //        jtr.Read();
                                                    //        if (jtr.TokenType == JsonToken.EndArray)
                                                    //        {
                                                    //            continue;
                                                    //        }

                                                    //        Frase f = CarregarJsonFrase(jtr);

                                                    //        if (jtr.TokenType == JsonToken.EndObject)
                                                    //        {
                                                    //            m.Frases.Add(f);
                                                    //        }
                                                    //    }
                                                    //    jtr.Read();

                                                    //}

                                                    if (jtr.TokenType == JsonToken.EndObject)
                                                    {
                                                        if (m.ID == 0)
                                                        {
                                                            m.ID = p.Mensagens.Count + 1;
                                                            m.Indice = p.Mensagens.Count;
                                                            m.Ascendente = true;
                                                            m.Ordenacao = 0;
                                                        }
                                                        p.Mensagens.Add(m);
                                                        //jtr.Read();
                                                    }
                                                }

                                            }

                                        }
                                    }
                                    jtr.Read();
                                    if (VerificarJsonValue("Roteiros", jtr))
                                    {
                                        jtr.Read();
                                        if (jtr.TokenType == JsonToken.StartArray)
                                        {
                                            while (jtr.TokenType != JsonToken.EndArray)
                                            {
                                                jtr.Read();
                                                if (jtr.TokenType == JsonToken.EndArray)
                                                {
                                                    continue;
                                                }
                                                jtr.Read();

                                                Roteiro r = new Roteiro();
                                                if (VerificarJsonValue("ID", jtr))
                                                {
                                                    r.ID = GetJsonInteiro(jtr);
                                                }
                                                if (VerificarJsonValue("Indice", jtr))
                                                {
                                                    r.Indice = GetJsonInteiro(jtr);
                                                }

                                                if (VerificarJsonValue("Numero", jtr))
                                                {
                                                    jtr.Read();
                                                    r.Numero = CarregarJsonFrase(jtr);
                                                }

                                                jtr.Read();

                                                if (VerificarJsonValue("IdaIgualVolta", jtr))
                                                {
                                                    r.IdaIgualVolta = GetJsonBooleano(jtr);
                                                }
                                                if (VerificarJsonValue("LabelRoteiro", jtr))
                                                {
                                                    r.LabelRoteiro = GetJsonString(jtr);
                                                }
                                                if (VerificarJsonValue("Tarifa", jtr))
                                                {
                                                    r.Tarifa = GetJsonInteiro(jtr);
                                                }
                                                if (VerificarJsonValue("FrasesIda", jtr))
                                                {
                                                    r.FrasesIda.AddRange(CarregarJsonFrases(jtr));
                                                    ValidarFrasesV04(r.FrasesIda, p.Altura, p.Largura);
                                                }                                                
                                                if (VerificarJsonValue("FrasesVolta", jtr))
                                                {
                                                    r.FrasesVolta.AddRange(CarregarJsonFrases(jtr));
                                                    ValidarFrasesV04(r.FrasesVolta, p.Altura, p.Largura);
                                                }
                                                if (jtr.TokenType == JsonToken.EndObject)
                                                {
                                                    if (r.ID == 0)
                                                    {
                                                        r.ID = p.Roteiros.Count + 1;
                                                        r.Indice = p.Roteiros.Count;
                                                        r.Ascendente = true;
                                                        r.Ordenacao = 0;
                                                    }
                                                    p.Roteiros.Add(r);
                                                }

                                            }

                                        }
                                    }
                                    jtr.Read();

                                    if (VerificarJsonValue("AlternanciaSelecionada", jtr))
                                    {
                                        p.AlternanciaSelecionada = GetJsonByte(jtr);
                                    }

                                    if (VerificarJsonValue("MensagensEspeciais", jtr))
                                    {
                                        ExecutarJsonRead_N_Vezes(2, jtr);

                                        if (VerificarJsonValue("Frases", jtr))
                                        {
                                            p.MensagensEspeciais.Frases.AddRange(CarregarJsonFrases(jtr));
                                        }
                                        p.MensagensEspeciais = new MensagemEspecial(carregaIdioma(), this.CarregarRegiao(this.GetNomeRegiao(controlador.Count - 1)));
                                    }
                                    jtr.Read();
                                    if (VerificarJsonValue("MensagemEmergencia", jtr))
                                    {
                                        p.MensagemEmergencia.Frases.Clear();
                                        jtr.Read();
                                        jtr.Read();
                                        if (VerificarJsonValue("Frases", jtr))
                                        {
                                            p.MensagemEmergencia.Frases.AddRange(CarregarJsonFrases(jtr));                                            
                                        }
                                    }

                                    jtr.Read();
                                    if (VerificarJsonValue("MensagemBoaNoite", jtr))
                                    {
                                        ExecutarJsonRead_N_Vezes(2, jtr);
                                    }
                                    if (VerificarJsonValue("MensagemBoaTarde", jtr))
                                    {
                                        ExecutarJsonRead_N_Vezes(2, jtr);
                                    }
                                    if (VerificarJsonValue("MensagemBomDia", jtr))
                                    {
                                        ExecutarJsonRead_N_Vezes(2, jtr);
                                    }
                                    if (VerificarJsonValue("BloqueioAlternancias", jtr))
                                    {
                                        ExecutarJsonRead_N_Vezes(2, jtr);
                                    }
                                    if (VerificarJsonValue("FontePath", jtr))
                                    {
                                        p.FontePath = GetJsonString(jtr);
                                    }
                                    if (VerificarJsonValue("MensagemSelecionada", jtr))
                                    {
                                        p.MensagemSelecionada = GetJsonInteiro(jtr);
                                    }

                                    controlador[controlador.Count - 1].Paineis.Add(p);

                                }

                            }
                        }
                        if (VerificarJsonValue("Versao", jtr))
                        {
                            controlador[controlador.Count - 1].Versao = GetJsonString(jtr);
                        }
                        if (VerificarJsonValue("StatusTransmissao", jtr))
                        {
                            controlador[controlador.Count - 1].StatusTransmissao = GetJsonString(jtr);
                        }
                        if (VerificarJsonValue("QuantidadePaineis", jtr))
                        {
                            controlador[controlador.Count - 1].QuantidadePaineis = GetJsonInteiro(jtr);
                        }
                        if (VerificarJsonValue("Fontes", jtr))
                        {
                            ExecutarJsonRead_N_Vezes(2, jtr);
                        }
                        //if (VerificarJsonValue("Regioes", jtr))
                        //{
                        //    controlador[controlador.Count - 1].Regioes = GetJsonString(jtr);
                        //}

                        break;
                }
                    
            }
            jtr.Close();

        }
        /// <summary>
        /// Método criado para atribuir valores para as propriedades AlturaPainel e LarguraPainel de arquivos antigos.
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="altura"></param>
        /// <param name="largura"></param>
        public void ValidarFrasesV04(List<Frase> lista, int altura, int largura)
        {
            int primeiraMetadeAltura = altura / 2;
            int segundaMetadeAltura = altura - primeiraMetadeAltura;
            int primeiraMetadeLargura = largura / 2;
            int segundaMetadeLargura = largura - primeiraMetadeLargura;
            int umTercoLargura = largura / 4;
            int doisTercosLargura = largura - umTercoLargura;

            foreach (Frase f in lista)
            {
                if (f.TipoVideo == Util.Util.TipoVideo.V04)
                {
                    switch (f.Modelo.TipoModelo)
                    {
                        case Util.Util.TipoModelo.NúmeroTexto:
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? altura : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? umTercoLargura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? altura : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[1].LarguraPainel;
                            }
                            break;
                        case Util.Util.TipoModelo.NúmeroTextoDuplo: 
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? altura : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? umTercoLargura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? primeiraMetadeAltura : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[1].LarguraPainel;
                                f.Modelo.Textos[2].AlturaPainel = (f.Modelo.Textos[2].AlturaPainel == 0) ? segundaMetadeAltura : f.Modelo.Textos[2].AlturaPainel;
                                f.Modelo.Textos[2].LarguraPainel = (f.Modelo.Textos[2].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[2].LarguraPainel;
                            }
                            break;
                        case Util.Util.TipoModelo.Texto: 
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? altura : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? largura : f.Modelo.Textos[0].LarguraPainel;
                            }
                            break;
                        case Util.Util.TipoModelo.TextoDuplo: 
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? primeiraMetadeAltura : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? largura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? segundaMetadeAltura : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? largura : f.Modelo.Textos[1].LarguraPainel;
                            }
                            break;
                        case Util.Util.TipoModelo.TextoDuploNúmero: 
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? primeiraMetadeAltura : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? altura : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? umTercoLargura : f.Modelo.Textos[1].LarguraPainel;
                                f.Modelo.Textos[2].AlturaPainel = (f.Modelo.Textos[2].AlturaPainel == 0) ? segundaMetadeAltura : f.Modelo.Textos[2].AlturaPainel;
                                f.Modelo.Textos[2].LarguraPainel = (f.Modelo.Textos[2].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[2].LarguraPainel;                                
                            }
                            break;
                        case Util.Util.TipoModelo.TextoDuploTextoDuplo: 
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? primeiraMetadeAltura : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? primeiraMetadeLargura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? primeiraMetadeAltura : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? segundaMetadeLargura : f.Modelo.Textos[1].LarguraPainel;
                                f.Modelo.Textos[2].AlturaPainel = (f.Modelo.Textos[2].AlturaPainel == 0) ? segundaMetadeAltura : f.Modelo.Textos[2].AlturaPainel;
                                f.Modelo.Textos[2].LarguraPainel = (f.Modelo.Textos[2].LarguraPainel == 0) ? primeiraMetadeLargura : f.Modelo.Textos[2].LarguraPainel;
                                f.Modelo.Textos[3].AlturaPainel = (f.Modelo.Textos[3].AlturaPainel == 0) ? segundaMetadeAltura : f.Modelo.Textos[3].AlturaPainel;
                                f.Modelo.Textos[3].LarguraPainel = (f.Modelo.Textos[3].LarguraPainel == 0) ? segundaMetadeLargura : f.Modelo.Textos[3].LarguraPainel;
                            }
                            break;
                        case Util.Util.TipoModelo.TextoNúmero: 
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? altura : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? altura : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? umTercoLargura : f.Modelo.Textos[1].LarguraPainel;
                            }
                            break;
                        case Util.Util.TipoModelo.TextoTriplo:
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1 : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? largura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2 : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? largura : f.Modelo.Textos[1].LarguraPainel;
                                f.Modelo.Textos[2].AlturaPainel = (f.Modelo.Textos[2].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3 : f.Modelo.Textos[2].AlturaPainel;
                                f.Modelo.Textos[2].LarguraPainel = (f.Modelo.Textos[2].LarguraPainel == 0) ? largura : f.Modelo.Textos[2].LarguraPainel;
                    }
                            break;
                        case Util.Util.TipoModelo.NumeroTextoTriplo:
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? altura : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? umTercoLargura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1 : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[1].LarguraPainel;
                                f.Modelo.Textos[2].AlturaPainel = (f.Modelo.Textos[2].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2 : f.Modelo.Textos[2].AlturaPainel;
                                f.Modelo.Textos[2].LarguraPainel = (f.Modelo.Textos[2].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[2].LarguraPainel;
                                f.Modelo.Textos[3].AlturaPainel = (f.Modelo.Textos[3].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3 : f.Modelo.Textos[3].AlturaPainel;
                                f.Modelo.Textos[3].LarguraPainel = (f.Modelo.Textos[3].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[3].LarguraPainel;
                }
                            break;
                        case Util.Util.TipoModelo.TextoTriploNumero:
                            {
                                f.Modelo.Textos[0].AlturaPainel = (f.Modelo.Textos[0].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO1 : f.Modelo.Textos[0].AlturaPainel;
                                f.Modelo.Textos[0].LarguraPainel = (f.Modelo.Textos[0].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[0].LarguraPainel;
                                f.Modelo.Textos[1].AlturaPainel = (f.Modelo.Textos[1].AlturaPainel == 0) ? altura : f.Modelo.Textos[1].AlturaPainel;
                                f.Modelo.Textos[1].LarguraPainel = (f.Modelo.Textos[1].LarguraPainel == 0) ? umTercoLargura : f.Modelo.Textos[1].LarguraPainel;
                                f.Modelo.Textos[2].AlturaPainel = (f.Modelo.Textos[2].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO2 : f.Modelo.Textos[2].AlturaPainel;
                                f.Modelo.Textos[2].LarguraPainel = (f.Modelo.Textos[2].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[2].LarguraPainel;
                                f.Modelo.Textos[3].AlturaPainel = (f.Modelo.Textos[3].AlturaPainel == 0) ? Util.Util.ALTURA_PAINEL_TEXTO_TRIPLO3 : f.Modelo.Textos[3].AlturaPainel;
                                f.Modelo.Textos[3].LarguraPainel = (f.Modelo.Textos[3].LarguraPainel == 0) ? doisTercosLargura : f.Modelo.Textos[3].LarguraPainel;
            }
                            break;
        }
                }
            }
        }

        private byte GetJsonByte(JsonTextReader jtr)
        {
            jtr.Read();            
            byte resposta = Convert.ToByte(jtr.Value.ToString());
            jtr.Read();
            return resposta;
        }

        private int GetJsonInteiro(JsonTextReader jtr)
        {
            jtr.Read();
            int resposta = Convert.ToInt32(jtr.Value.ToString());
            jtr.Read();
            return resposta;
        }
        private bool GetJsonBooleano(JsonTextReader jtr)
        {
            jtr.Read();
            bool resposta = Convert.ToBoolean(jtr.Value.ToString());
            jtr.Read();
            return resposta;
        }
        private string GetJsonString(JsonTextReader jtr)
        {
            jtr.Read();
            string resposta = jtr.Value.ToString();           
            jtr.Read();
            return resposta;
        }
        private Frase[] CarregarJsonFrases(JsonTextReader jtr)
        {
            List<Frase> frases = new List<Frase>();

            jtr.Read();
            if (jtr.TokenType == JsonToken.StartArray)
            {
                while (jtr.TokenType != JsonToken.EndArray)
                {
                    jtr.Read();
                    if (jtr.TokenType == JsonToken.EndArray)
                    {
                        continue;
                    }

                    Frase f = CarregarJsonFrase(jtr);

                    if (jtr.TokenType == JsonToken.EndObject)
                    {
                        frases.Add(f);
                        //jtr.Read();
                    }
                }
                jtr.Read();
            }

            return frases.ToArray();
        }
        private Frase CarregarJsonFrase(JsonTextReader jtr)
        {
            Frase f = new Frase();

            jtr.Read();

            if (VerificarJsonValue("Indice", jtr))
            {
                f.Indice = GetJsonInteiro(jtr);                
            }
            if (VerificarJsonValue("LabelFrase", jtr))
            {
                f.LabelFrase = GetJsonString(jtr);
            }
            if (VerificarJsonValue("TextoAutomatico", jtr))
            {
                f.TextoAutomatico = GetJsonBooleano(jtr);
            }
            
            if (VerificarJsonValue("TipoVideo", jtr))
            {
                f.TipoVideo = (Util.Util.TipoVideo)GetJsonByte(jtr);
            }
            
            if (VerificarJsonValue("Modelo", jtr))
            {
                ExecutarJsonRead_N_Vezes(2, jtr);

                if (VerificarJsonValue("Textos", jtr))
                {
                    jtr.Read();
                    if (jtr.TokenType == JsonToken.StartArray)
                    {
                        while (jtr.TokenType != JsonToken.EndArray)
                        {
                            jtr.Read();
                            if (jtr.TokenType == JsonToken.EndArray)
                            {
                                continue;
                            }

                            Texto textoLocal = CarregarJsonTextos(jtr);

                           
                            

                            if (jtr.TokenType == JsonToken.EndObject)
                            {
                                f.Modelo.Textos.Add(textoLocal);
                            }
                        }
                        jtr.Read();
                        if (VerificarJsonValue("TipoModelo", jtr))
                        {                            
                            f.Modelo.TipoModelo = (Util.Util.TipoModelo)GetJsonByte(jtr);
                        }
                                            
                    }
                }
            }
            jtr.Read();
            return f;
        }
        private bool VerificarJsonValue(string valor, JsonTextReader jtr)
        {
            bool resposta = false;
            if ((null != jtr.Value) && (valor == jtr.Value.ToString()))
            {
                resposta = true;
            }
            return resposta;
        }
        private Texto CarregarJsonTextos(JsonTextReader jtr)
        {
            jtr.Read();
            Texto textoLocal = new Texto();
            if (VerificarJsonValue("LabelTexto", jtr))
            {
                textoLocal.LabelTexto = GetJsonString(jtr);
            }
            
            if (VerificarJsonValue("Apresentacao", jtr))
            {
                textoLocal.Apresentacao = (Util.Util.Rolagem)GetJsonByte(jtr);
            }
            
            if (VerificarJsonValue("FonteWindows", jtr))
            {
                textoLocal.FonteWindows = GetJsonBooleano(jtr);                
            }
            
            if (VerificarJsonValue("FonteAnteriorWindows", jtr))
            {
                textoLocal.FonteAnteriorWindows = GetJsonBooleano(jtr);
            }            
            if (VerificarJsonValue("Altura", jtr))
            {
                textoLocal.Altura = GetJsonInteiro(jtr);
            }
            
            if (VerificarJsonValue("Negrito", jtr))
            {
                textoLocal.Negrito = GetJsonBooleano(jtr);                
            }            
            if (VerificarJsonValue("Italico", jtr))
            {
                textoLocal.Italico = GetJsonBooleano(jtr);
            }
            
            if (VerificarJsonValue("Sublinhado", jtr))
            {
                textoLocal.Sublinhado = GetJsonBooleano(jtr);
            }
            
            if (VerificarJsonValue("AlinhamentoH", jtr))
            {
                textoLocal.AlinhamentoH = (Util.Util.AlinhamentoHorizontal)GetJsonByte(jtr);
            }            
            if (VerificarJsonValue("AlinhamentoV", jtr))
            {
                textoLocal.AlinhamentoV = (Util.Util.AlinhamentoVertical)GetJsonByte(jtr);
            }
            
            if (VerificarJsonValue("TempoRolagem", jtr))
            {
                textoLocal.TempoRolagem = GetJsonInteiro(jtr);
            }
            
            if (VerificarJsonValue("TempoApresentacao", jtr))
            {
                textoLocal.TempoApresentacao = GetJsonInteiro(jtr);
            }
            
            if (VerificarJsonValue("InverterLed", jtr))
            {
                textoLocal.InverterLed = GetJsonBooleano(jtr);
            }            
            if (VerificarJsonValue("Largura", jtr))
            {                
                textoLocal.Largura = GetJsonInteiro(jtr);
            }            
            if (VerificarJsonValue("Fonte", jtr))
            {
                textoLocal.Fonte = GetJsonString(jtr);
            }            
            if (VerificarJsonValue("CaminhoFonte", jtr))
            {
                // Caminho Fonte é Read Only
                ExecutarJsonRead_N_Vezes(2, jtr);                
            }
            
            return textoLocal;
        }
        private void ExecutarJsonRead_N_Vezes(int vezes, JsonTextReader jtr)
        {
            for (int i = 0; i < vezes; i++)
            {
                jtr.Read();
            }
        }
        private unsafe Util.Util.OpcoesApresentacao CarregarJsonOpcoesApresentacao(JsonTextReader jtr)
        {            
            Util.Util.OpcoesApresentacao opcoes = new Util.Util.OpcoesApresentacao();

            ExecutarJsonRead_N_Vezes(2, jtr);      

            if (VerificarJsonValue("animacao", jtr))
            {
                opcoes.animacao = GetJsonByte(jtr);
            }            
            if (VerificarJsonValue("alinhamento", jtr))
            {
                opcoes.alinhamento = GetJsonByte(jtr);
            }            
            if (VerificarJsonValue("reservado", jtr))
            {
                ExecutarJsonRead_N_Vezes(5, jtr);
            }            
            if (VerificarJsonValue("intervaloAnimacao", jtr))
            {
                opcoes.intervaloAnimacao = (uint)GetJsonInteiro(jtr);
            }            
            if (VerificarJsonValue("tempoApresentacao", jtr))
            {
                opcoes.tempoApresentacao = (uint)GetJsonInteiro(jtr);
            }            
            jtr.Read();
            return opcoes;
        }

        public void RemoveControlador(int indiceControlador)
        {
            this.controlador.RemoveAt(indiceControlador);
        }

        
        public String[] CarregarImagens()
        {
            List<String> lista = new List<string>();

            foreach (string s in Directory.GetFiles(diretorio_imagens))
            {
                lista.Add(Path.GetFileNameWithoutExtension(s));
            }
            
            return lista.ToArray();
        }
        public string StatusEnvioControlador(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].StatusTransmissao;
        }

        public void SetResourceManagerControlador(int controladorSelecionado, ResourceManager rm)
        {
            this.controlador[controladorSelecionado].Rm = rm;
        }
        public void GerarConfig(int controladorSelecionado, int quantidadePaineis, int quantidadeRoteiros, int quantidadeMensagens)
        {

            //this.controlador[controladorSelecionado] = new Controlador(carregaIdioma(), CarregarRegiao(Util.Util.NOME_REGIAO));
            this.controlador[controladorSelecionado] = new Controlador(carregaIdioma(), CarregarRegiao(this.controlador[controladorSelecionado].Regiao.Nome));
            //Arquivo_RGN regiao = new Arquivo_RGN(this.controlador[controladorSelecionado].Regiao);

            //this.controlador[controladorSelecionado] = new Controlador(carregaIdioma(), regiao);

            //cria os paineis.
            for (int painel = 0; painel < quantidadePaineis; painel++)
            {
                //Painel temp = new Painel(carregaIdioma(), CarregarRegiao(Util.Util.NOME_REGIAO));
                Painel temp = new Painel(carregaIdioma(), CarregarRegiao(this.controlador[controladorSelecionado].Regiao.Nome));
                //Painel temp = new Painel(carregaIdioma(), regiao);

                temp.Indice = painel;
                temp.Altura = 16;
                temp.Largura = 128;
                temp.PainelDefault(quantidadeMensagens, quantidadeRoteiros, painel);
                SetarFontesDefaultPainel(temp);
                
                this.controlador[controladorSelecionado].Paineis.Add(temp);                
            }
            SetarListaPadraoAlternanciasPainel(controladorSelecionado);
            //this.controlador[controladorSelecionado].ControladorDefault(quantidadePaineis, quantidadeRoteiros, quantidadeMensagens, carregaIdioma(),CarregarRegiao(Util.Util.NOME_REGIAO));
        }

        public void GerarConfig(int controladorSelecionado, int quantidadePaineis, int quantidadeRoteiros, int quantidadeMensagens, int quantidadeMotoristas)
        {

            this.controlador[controladorSelecionado] = new Controlador(carregaIdioma(), CarregarRegiao(this.controlador[controladorSelecionado].Regiao.Nome));

            //cria os paineis.
            for (int painel = 0; painel < quantidadePaineis; painel++)
            {
                
                Painel temp = new Painel(carregaIdioma(), CarregarRegiao(this.controlador[controladorSelecionado].Regiao.Nome));

                temp.Indice = painel;
                temp.Altura = 16;
                temp.Largura = 96;

                //painel principal
                if (painel == 0)
                {
                    temp.Largura = 144;
                    temp.BrilhoMaximo = 80;
                    temp.BrilhoMinimo = 30;
                }

                //penultimo painel
                if ((painel == quantidadePaineis - 2) && (quantidadePaineis > 2))
                {
                    temp.Altura = 26;
                    temp.Largura = 192;
                }

                //ultimo painel
                if ((painel == quantidadePaineis - 1) && (painel != 0))
                {
                    temp.Largura = 112;
                    temp.BrilhoMaximo = 70;
                    temp.BrilhoMinimo = 20;
                }
                
                temp.PainelDefault(quantidadeMensagens, quantidadeRoteiros, quantidadeMotoristas, painel, quantidadePaineis);

                for (int indiceRoteiro = 0; indiceRoteiro < temp.Roteiros.Count; indiceRoteiro++)
                {
                    ValidarFrasesV04(temp.Roteiros[indiceRoteiro].FrasesIda, temp.Altura, temp.Largura);
                    ValidarFrasesV04(temp.Roteiros[indiceRoteiro].FrasesVolta, temp.Altura, temp.Largura);
                }
                for (int indiceMensagem = 0; indiceMensagem < temp.Mensagens.Count; indiceMensagem++)
                {
                    ValidarFrasesV04(temp.Mensagens[indiceMensagem].Frases, temp.Altura, temp.Largura);
                }

                SetarFontesDefaultPainel(temp);

                this.controlador[controladorSelecionado].Paineis.Add(temp);
            }

            SetarListaPadraoAlternanciasPainel(controladorSelecionado);
        }

        public void ImprimirArquivo(int controladorSelecionado, int painelSelecionado, int Opcao, bool imprimirTextos)
        {
            ResourceManager rm = carregaIdioma();
            Controlador Ctrl = this.controlador[controladorSelecionado];
            //string FileName = diretorio_raiz + Util.Util.DIRETORIO_APP+ Util.Util.DIRETORIO_REPORTS +"\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".html";
            string FileName = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_REPORTS + Util.Util.ARQUIVO_RELATORIO;
            
            // Armazenando informações dentro do arquivo			
            TextWriter output = new StreamWriter(FileName, false, Encoding.UTF8);

            //tratando para exibir de forma diferente se for para imprimir os textos internos
            string tabela, linha;
            if (imprimirTextos)
            {
                tabela = "<table class=\"table\">";
                linha = "<tr style='background-color: #f9f9f9'>";
            } else {
                tabela = "<table class=\"table table-striped\">";
                linha = "<tr>";
            }

            
            #region HEADER
            output.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            output.WriteLine("<head runat=\"server\">");
            output.WriteLine("<title>Itinerário Pontos X2 - http://www.frt.com.br</title>");
            string cssPath = "<link href=\"StyleSheet.css\" rel=\"stylesheet\" type=\"text/css\" />";
            //string cssPath = "<link href=\"bootstrap.css\" rel=\"stylesheet\" type=\"text/css\" />";
            output.WriteLine(cssPath);
            output.WriteLine("</head>");
            #endregion

            output.WriteLine("<body>");
            if (Opcao == 1)
            {
                #region Roteiros
                output.WriteLine("<div class=\"row\">");
                output.WriteLine("<div class=\"col-md-12\">");
                output.WriteLine("<div class=\"table-responsive\">");
                output.WriteLine("<h2>" + rm.GetString("ARQUIVO_TABPAGE_ROTEIRO") + "</h2>");
                output.WriteLine("<br>");
                output.WriteLine(tabela);

                output.WriteLine("<thead>");
                // Cabeçalho da tabela
                output.WriteLine("<tr>");
                // Cabeçalho Indice
                output.WriteLine("<th colspan=\"1\" >");
                output.WriteLine(rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_COLUNA_INDICE") + "</th>");
                // Cabeçalho Numero
                output.WriteLine("<th  colspan=\"1\" >");
                output.WriteLine(rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_COLUNA_NUMERO") + "</th>");
                // Cabeçalho Label do Roteiro
                output.WriteLine("<th  colspan=\"1\" >");
                output.WriteLine(rm.GetString("USER_CONTROL_LISTAR_ROTEIROS_COLUNA_LABEL") + "</th>");
                output.WriteLine("</tr>");
                output.WriteLine("</thead>");
               
                output.WriteLine("<tbody>");
                for (int indiceRoteiro = 0; indiceRoteiro < Ctrl.Paineis[painelSelecionado].Roteiros.Count; indiceRoteiro++)
                {
                    // Conteudo da tabela
                    output.WriteLine(linha);

                    // Conteudo Indice
                    output.WriteLine("<td  colspan=\"1\">");
                    output.WriteLine(Ctrl.Paineis[painelSelecionado].Roteiros[indiceRoteiro].Indice.ToString("000") + "</td>");
                    // Conteudo Numero
                    output.WriteLine("<td  colspan=\"1\">");
                    output.WriteLine(Ctrl.Paineis[painelSelecionado].Roteiros[indiceRoteiro].Numero.LabelFrase + "</td>");
                    // Conteudo Label do Roteiro 
                    output.WriteLine("<td  colspan=\"1\">");
                    output.WriteLine(Ctrl.Paineis[painelSelecionado].Roteiros[indiceRoteiro].LabelRoteiro + "</td>");

                    output.WriteLine("</tr>");

                    if (imprimirTextos)
                    {
                        //Textos de ida
                        for (int indiceTextos = 0; indiceTextos < Ctrl.Paineis[painelSelecionado].Roteiros[indiceRoteiro].FrasesIda.Count; indiceTextos++)
                        {
                            //imprimir cabeçalho do texto de ida
                            if (indiceTextos == 0)
                            {
                                output.WriteLine("<tr>");
                                output.WriteLine("<td  colspan=\"1\"></td>");
                                output.WriteLine("<td  colspan=\"2\"><b>"+ rm.GetString("USER_CONTROL_ROTEIROS_TAB_IDA") + "</b></td>");
                                output.WriteLine("</tr>");
                            }

                            // Imprimir os textos de ida
                            output.WriteLine("<tr>");
                            output.WriteLine("<td  colspan=\"2\"></td>");
                            output.WriteLine("<td  colspan=\"1\">");
                            output.WriteLine(Ctrl.Paineis[painelSelecionado].Roteiros[indiceRoteiro].FrasesIda[indiceTextos].LabelFrase + "</td>");
                            output.WriteLine("</tr>");
                        }

                        //Textos de volta
                        for (int indiceTextos = 0; indiceTextos < Ctrl.Paineis[painelSelecionado].Roteiros[indiceRoteiro].FrasesVolta.Count; indiceTextos++)
                        {
                            //imprimir cabeçalho do texto de ida
                            if (indiceTextos == 0)
                            {
                                output.WriteLine("<tr>");
                                output.WriteLine("<td  colspan=\"1\"></td>");
                                output.WriteLine("<td  colspan=\"2\"><b>" + rm.GetString("USER_CONTROL_ROTEIROS_TAB_VOLTA") + "</b></td>");
                                output.WriteLine("</tr>");
                            }

                            // Imprimir os textos de ida
                            output.WriteLine("<tr>");
                            output.WriteLine("<td  colspan=\"2\"></td>");
                            output.WriteLine("<td  colspan=\"1\">");
                            output.WriteLine(Ctrl.Paineis[painelSelecionado].Roteiros[indiceRoteiro].FrasesVolta[indiceTextos].LabelFrase + "</td>");
                            output.WriteLine("</tr>");
                        }
                    }
                }
                output.WriteLine("</tbody>");
                output.WriteLine("</table>");
                output.WriteLine("</div>");
                output.WriteLine("</div>");
                output.WriteLine("</div>");
                #endregion
            }

            if (Opcao == 2)
            {
                #region Mensagens

                output.WriteLine("<div class=\"row\">");
                output.WriteLine("<div class=\"col-md-12\">");
                output.WriteLine("<div class=\"table-responsive\">");
                output.WriteLine("<h2>" + rm.GetString("ARQUIVO_TABPAGE_MENSAGEM") + "</h2>");
                output.WriteLine("<br>");
                output.WriteLine(tabela);

                output.WriteLine("<thead>");
                // Cabeçalho da tabela
                output.WriteLine("<tr>");
                // Cabeçalho Indice
                output.WriteLine("<th colspan=\"1\" >");
                output.WriteLine(rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_COLUNA_INDICE") + "</th>");
                // Cabeçalho Mensagem
                output.WriteLine("<th  colspan=\"1\" >");
                output.WriteLine(rm.GetString("USER_CONTROL_LISTAR_MENSAGENS_COLUNA_MENSAGEM") + "</th>");
                output.WriteLine("</thead>");


                output.WriteLine("<tbody>");
                for (int indiceMensagem = 0; indiceMensagem < Ctrl.Paineis[painelSelecionado].Mensagens.Count; indiceMensagem++)
                {
                    // Conteudo da tabela
                    output.WriteLine(linha);

                    // Conteudo Indice
                    output.WriteLine("<td  colspan=\"1\">");
                    output.WriteLine(Ctrl.Paineis[painelSelecionado].Mensagens[indiceMensagem].Indice.ToString("000") + "</td>");
                    // Conteudo Numero
                    output.WriteLine("<td  colspan=\"1\">");
                    output.WriteLine(Ctrl.Paineis[painelSelecionado].Mensagens[indiceMensagem].LabelMensagem + "</td>");


                    output.WriteLine("</tr>");

                    if (imprimirTextos)
                    {
                        //Textos de ida
                        for (int indiceTextos = 0; indiceTextos < Ctrl.Paineis[painelSelecionado].Mensagens[indiceMensagem].Frases.Count; indiceTextos++)
                        {
                            //imprimir cabeçalho do texto de ida
                            if (indiceTextos == 0)
                            {
                                output.WriteLine("<tr>");
                                output.WriteLine("<td  colspan=\"1\">&emsp;<b>" + rm.GetString("USER_CONTROL_MENSAGENS_TAB_IDA") + "</b></td>");
                                output.WriteLine("<td  colspan=\"1\"></td>");
                                output.WriteLine("</tr>");
                            }

                            // Imprimir os textos de ida
                            output.WriteLine("<tr>");
                            output.WriteLine("<td  colspan=\"1\"></td>");
                            output.WriteLine("<td  colspan=\"1\">");
                            output.WriteLine(Ctrl.Paineis[painelSelecionado].Mensagens[indiceMensagem].Frases[indiceTextos].LabelFrase + "</td>");
                            output.WriteLine("</tr>");
                        }
                    }
                }
                output.WriteLine("</tbody>");
                output.WriteLine("</table>");
                output.WriteLine("</div>");
                output.WriteLine("</div>");
                output.WriteLine("</div>");

                #endregion
            }

            if (Opcao == 3)
            {
                #region Motorista

                output.WriteLine("<div class=\"row\">");
                output.WriteLine("<div class=\"col-md-12\">");
                output.WriteLine("<div class=\"table-responsive\">");
                output.WriteLine("<h2>" + rm.GetString("ARQUIVO_TABPAGE_MOTORISTA") + "</h2>");
                output.WriteLine("<br>");
                output.WriteLine("<table class=\"table table-striped\">");

                output.WriteLine("<thead>");
                // Cabeçalho da tabela
                output.WriteLine("<tr>");
                // Cabeçalho Indice
                output.WriteLine("<th colspan=\"1\" >");
                output.WriteLine(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_LV_MOTORISTAS_COLUNA_0") + "</th>");
                // Cabeçalho Mensagem
                output.WriteLine("<th  colspan=\"1\" >");
                output.WriteLine(rm.GetString("USER_CONTROL_LISTAR_MOTORISTA_LV_MOTORISTAS_COLUNA_1") + "</th>");
                output.WriteLine("</thead>");

                output.WriteLine("<tbody>");
                for (int indiceMotorista = 0; indiceMotorista < Ctrl.Paineis[0].Motoristas.Count; indiceMotorista++)
                {
                    // Conteudo da tabela
                    output.WriteLine("<tr>");

                    // Conteudo Indice
                    output.WriteLine("<td  colspan=\"1\">");
                    output.WriteLine(Ctrl.Paineis[0].Motoristas[indiceMotorista].ID.LabelFrase + "</td>");
                    // Conteudo Numero
                    output.WriteLine("<td  colspan=\"1\">");
                    output.WriteLine(Ctrl.Paineis[0].Motoristas[indiceMotorista].Nome.LabelFrase + "</td>");


                    output.WriteLine("</tr>");
                }
                output.WriteLine("</tbody>");
                output.WriteLine("</table>");
                output.WriteLine("</div>");
                output.WriteLine("</div>");
                output.WriteLine("</div>");

                #endregion
            }


            output.WriteLine("</body>");
            output.WriteLine("</html>");

            // Armazenando informações dentro do arquivo			

            output.Close();
            System.Diagnostics.Process.Start(FileName);
        }

        public Controlador ImportarControladorLDX(String arquivoNome)
        {
            Fachada fachada = Fachada.instance;

            string regiao = Util.Util.RGN_EN_US;
            switch (this.lingua)
            {
                case Util.Util.Lingua.Ingles:
                    regiao = Util.Util.RGN_EN_US;
                    break;
                case Util.Util.Lingua.Espanhol:
                    regiao = Util.Util.RGN_ES_ES;
                    break;
                case Util.Util.Lingua.Portugues:
                    regiao = Util.Util.RGN_PT_BR;
                    break;
            }

            Controlador cTemp = new Controlador(this.carregaIdioma(), this.CarregarRegiao(regiao));

            bool existeVelocidade = Util.Util.VerificarExistenciaVelocidade(arquivoNome);
            // Recuperando informações dentro do arquivo			
            TextReader input = new StreamReader(arquivoNome, Encoding.UTF8, true);

            int tempoMensagem = (ushort)Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "TempoMensagem"));
            int tempoRolagem = (ushort)Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "TempoRolagem"));
            int tempoRoteiro = (ushort)Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "TempoRoteiro"));

            int QuantidadePaineis = Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "QtdPaineis"));
            int QtdRoteiros = Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "QtdRoteiros"));
            int QtdMensagem = Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "QtdMensagens"));

            // Carregar os Parametros Fixos
            cTemp.ParametrosFixos.HoraInicioDia = Convert.ToByte(Util.Util.retornarAtributoLinha(input, "HoraInicioDia"));
            cTemp.ParametrosFixos.HoraInicioTarde = Convert.ToByte(Util.Util.retornarAtributoLinha(input, "HoraInicioTarde"));
            cTemp.ParametrosFixos.HoraInicioNoite = Convert.ToByte(Util.Util.retornarAtributoLinha(input, "HoraInicioNoite"));

            //cTemp.ParametrosFixos.ApresentacaoMensagensEspeciais = 0;

            // Carregar o Bloqueio de funções - [TecladoInfo] - AjusteDireito, AjusteEsquerdo, AlternaRoteiroCom, IdaVolta, MensagemDireito, MensagemEsquerdo, OK, RoteiroDireito, RoteiroEsquerdo e SelecionaPainel.
            cTemp.ParametrosFixos.BloqueioFuncoes = new bool[Util.Util.QUANTIDADE_DE_FUNCOES_BLOQUEAVEIS];
            //parser.control.MascaraTeclado

            //cTemp.ParametrosFixos.CaminhoMensagensEspeciais = 0;

            //cTemp.ParametrosFixos.dataHora.
                       

            cTemp.Regiao = CarregarRegiao(regiao);

            input.Close();

            input = new StreamReader(arquivoNome, Encoding.UTF8, true);

            // Carregar os Roteiros IDA / VOLTA /Número e Carregar as mensagens
            for (int indicePainel = 0; indicePainel < QuantidadePaineis; indicePainel++)
            {
                bool ativado = Convert.ToBoolean(Util.Util.retornarAtributoLinha(input, "Ativado"));

                if (ativado)
                {
                    Painel painelTemp = new Painel(this.carregaIdioma(), this.CarregarRegiao(regiao));
                    painelTemp.Altura = Convert.ToInt32(Util.Util.retornarAtributoLinha(input, "Altura"));
                    painelTemp.Largura = Convert.ToInt32(Util.Util.retornarAtributoLinha(input, "Largura"));
                    
                    painelTemp.AlternanciaSelecionada = RetornarAlternancia(Util.Util.retornarAtributoLinha(input, "Apresentacao"));
                    cTemp.ParametrosVariaveis.RoteiroSelecionado = (ushort)Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "RoteiroSelecionado"));
                    painelTemp.MensagemSelecionada = Convert.ToInt32(Util.Util.retornarAtributoLinha(input, "MensagemSelecionada"));
                    painelTemp.MensagemSecundariaSelecionada = Convert.ToInt32(Util.Util.retornarAtributoLinha(input, "MensagemSelecionada2"));

                    /* ROTEIROS */
                    for (int indiceRoteiro = 0; indiceRoteiro < QtdRoteiros; indiceRoteiro++)
                    {
                        // NUMERO
                        string formatacaoTipo = Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.FormatacaoTipo");

                        bool usoSuperior = (formatacaoTipo == "FormatacaoTipo3") || (formatacaoTipo == "FormatacaoTipo4");
                        bool usoNumero = (formatacaoTipo == "FormatacaoTipo2")||(formatacaoTipo == "FormatacaoTipo4");

                        Util.Util.AlinhamentoHorizontal ahLocal = Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal"));
                        Util.Util.AlinhamentoHorizontal ahLocal_Numero = (usoNumero) ? Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal_Numero")) : Util.Util.AlinhamentoHorizontal.Centralizado;
                        Util.Util.AlinhamentoHorizontal ahLocal_Superior = (usoSuperior)? Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoHorizontal_Superior")): Util.Util.AlinhamentoHorizontal.Centralizado;

                        Util.Util.AlinhamentoVertical avLocal = Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical"));
                        Util.Util.AlinhamentoVertical avLocal_Numero = (usoNumero) ? Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical_Numero")) : Util.Util.AlinhamentoVertical.Centro; 
                        Util.Util.AlinhamentoVertical avLocal_Superior = (usoSuperior) ? Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.AlinhamentoVertical_Superior")) : Util.Util.AlinhamentoVertical.Centro;

                        Util.Util.Rolagem rolagemLocal = Util.Util.RetornaRolagemLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.RolagemTipo"));

                        string numero = Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Numero.Imagem[0].Rotulo")/*.Trim()*/;
                        if (numero.Length > 16)
                            numero = numero.Substring(0, 16);

                        Roteiro r = new Roteiro(numero);
                        r.IdaIgualVolta = false;
                        r.ID = indiceRoteiro + 1;
                        r.Indice = indiceRoteiro;
                        r.Ascendente = true;
                        r.Ordenacao = 255;
                        r.Numero.Modelo.Textos[0].Apresentacao = rolagemLocal;
                        r.Numero.Modelo.Textos[0].AlinhamentoH = ahLocal;
                        r.Numero.Modelo.Textos[0].AlinhamentoV = avLocal;

                        //setando a fonte default do numero
                        fachada.SetarFontesDefaultFrases(r.Numero, painelTemp.Altura);


                        int quantidadeFrasesIda = Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].QtdFrases"));

                        for (int indiceIda = 0; indiceIda < quantidadeFrasesIda; indiceIda++)
                        {
                            formatacaoTipo = Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].FormatacaoTipo");                           
                            
                            usoSuperior = (formatacaoTipo == "FormatacaoTipo3") || (formatacaoTipo == "FormatacaoTipo4");
                            usoNumero = (formatacaoTipo == "FormatacaoTipo2") || (formatacaoTipo == "FormatacaoTipo4");

                            ahLocal = Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal"));
                            ahLocal_Numero = (usoNumero) ? Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Numero")) : Util.Util.AlinhamentoHorizontal.Centralizado;
                            ahLocal_Superior = (usoSuperior) ? Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoHorizontal_Superior")) : Util.Util.AlinhamentoHorizontal.Centralizado;

                            avLocal = Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical"));
                            avLocal_Numero = (usoNumero) ? Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Numero")) : Util.Util.AlinhamentoVertical.Centro;
                            avLocal_Superior = (usoSuperior) ? Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].AlinhamentoVertical_Superior")) : Util.Util.AlinhamentoVertical.Centro;


                            rolagemLocal = Util.Util.RetornaRolagemLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].RolagemTipo"));                            
                            painelTemp.MultiLinhas = ((painelTemp.Altura == 7)&& (quantidadeFrasesIda == 8))? quantidadeFrasesIda: 1;
                            
                            // IDA
                            string ida = Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + indiceIda.ToString() + "].Imagem[0].Rotulo").Trim();
                            Frase f = new Frase();
                            switch (formatacaoTipo)
                            {
                                case "FormatacaoTipo1": // Somente texto
                                    {
                                        if (painelTemp.MultiLinhas > 1)
                                        {
                                            if (indiceRoteiro == 0)
                                            {
                                                painelTemp.Altura *= quantidadeFrasesIda;
                                            }
                                            f.TipoVideo = Util.Util.TipoVideo.V04;
                                            f.TextoAutomatico = false;                                            
                                            f.LabelFrase = carregaIdioma().GetString("ARQUIVO_PAINEL_MULTI_LINHAS");                                                                                        
                                                                                     
                                            for (indiceIda = 0; indiceIda < quantidadeFrasesIda; indiceIda++)
                                            {
                                                f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                                f.Modelo.Textos.Add(new Texto(ida));
                                                f.Modelo.Textos[indiceIda].AlinhamentoH = ahLocal;
                                                f.Modelo.Textos[indiceIda].AlinhamentoV = avLocal;
                                                f.Modelo.Textos[indiceIda].Altura = painelTemp.Altura / painelTemp.MultiLinhas;
                                                f.Modelo.Textos[indiceIda].Largura = painelTemp.Largura;
                                                f.Modelo.Textos[indiceIda].AlturaPainel = painelTemp.Altura / painelTemp.MultiLinhas;
                                                f.Modelo.Textos[indiceIda].LarguraPainel = painelTemp.Largura;
                                                if (indiceIda + 1 < quantidadeFrasesIda)
                                                {
                                                    ida = Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Ida[" + (indiceIda + 1).ToString() + "].Imagem[0].Rotulo").Trim();
                                                }
                                            }
                                            //carregando a lista de alternancia padrao de um painel multilinhas
                                            painelTemp.ListaAlternancias = CarregarAlternanciaPadraoPainelMultiLinhas();
                                        }
                                        else
                                        {
                                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                            f.Modelo.Textos.Add(new Texto(ida));
                                            f.Modelo.Textos[0].AlinhamentoH = ahLocal;
                                            f.Modelo.Textos[0].AlinhamentoV = avLocal;
                                        }
                                    }
                                    break;
                                case "FormatacaoTipo2": // Numero + Texto
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
                                        f.Modelo.Textos.Add(new Texto(numero));
                                        f.Modelo.Textos.Add(new Texto(ida));
                                        f.Modelo.Textos[0].AlinhamentoH = ahLocal_Numero;
                                        f.Modelo.Textos[0].AlinhamentoV = avLocal_Numero;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal;
                                    }
                                    break;
                                case "FormatacaoTipo3": // Texto Duplo
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
                                        f.Modelo.Textos.Add(new Texto(ida.Split(new char[1] { '•' })[0].Trim()));
                                        f.Modelo.Textos.Add(new Texto(ida.Split(new char[1] { '•' })[1].Trim()));
                                        f.Modelo.Textos[0].AlinhamentoH = ahLocal_Superior;
                                        f.Modelo.Textos[0].AlinhamentoV = avLocal_Superior;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal;
                                    }
                                    break;
                                case "FormatacaoTipo4": // Numero + TextoDuplo
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTextoDuplo;
                                        f.Modelo.Textos.Add(new Texto(numero));
                                        f.Modelo.Textos.Add(new Texto(ida.Split(new char[1] { '•' })[0].Trim()));
                                        f.Modelo.Textos.Add(new Texto(ida.Split(new char[1] { '•' })[1].Trim()));
                                        f.Modelo.Textos[0].AlinhamentoH = ahLocal_Numero;
                                        f.Modelo.Textos[0].AlinhamentoV = avLocal_Numero;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal_Superior;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal_Superior;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal;
                                    }
                                    break;
                                default: // Somente texto
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                        f.Modelo.Textos.Add(new Texto(ida));
                                    }
                                    break;
                            }
                            f.LabelFrase = (painelTemp.MultiLinhas == 1) ? ida : f.LabelFrase;
                            fachada.SetarFontesDefaultFrases(f, (painelTemp.MultiLinhas == 1) ? painelTemp.Altura : painelTemp.Altura / painelTemp.MultiLinhas);
                            for (int i = 0; i < f.Modelo.Textos.Count; i++)
                            {
                                f.Modelo.Textos[i].Apresentacao = rolagemLocal;
                            }
                            r.FrasesIda.Add(f);                           
                        }

                        int quantidadeFrasesVolta = Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].QtdFrases"));
                        for (int indiceVolta = 0; indiceVolta < quantidadeFrasesVolta; indiceVolta++)
                        {
                            formatacaoTipo = Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].FormatacaoTipo");

                            usoSuperior = (formatacaoTipo == "FormatacaoTipo3") || (formatacaoTipo == "FormatacaoTipo4");
                            usoNumero = (formatacaoTipo == "FormatacaoTipo2") || (formatacaoTipo == "FormatacaoTipo4");

                            ahLocal = Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal"));
                            ahLocal_Numero = (usoNumero) ? Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal_Numero")) : Util.Util.AlinhamentoHorizontal.Centralizado;
                            ahLocal_Superior = (usoSuperior) ? Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoHorizontal_Superior")) : Util.Util.AlinhamentoHorizontal.Centralizado;

                            avLocal = Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical"));
                            avLocal_Numero = (usoNumero) ? Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical_Numero")) : Util.Util.AlinhamentoVertical.Centro;
                            avLocal_Superior = (usoSuperior) ? Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].AlinhamentoVertical_Superior")) : Util.Util.AlinhamentoVertical.Centro;

                            rolagemLocal = Util.Util.RetornaRolagemLD6(Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].RolagemTipo"));

                            string volta = Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + indiceVolta.ToString() + "].Imagem[0].Rotulo").Trim();
                            Frase f = new Frase();

                            switch (formatacaoTipo)
                            {
                                case "FormatacaoTipo1": // Somente texto
                                    {                                       
                                        if (painelTemp.MultiLinhas > 1)
                                        {                                           
                                            f.TipoVideo = Util.Util.TipoVideo.V04;
                                            f.TextoAutomatico = false;
                                            f.LabelFrase = carregaIdioma().GetString("ARQUIVO_PAINEL_MULTI_LINHAS");

                                            for (indiceVolta = 0; indiceVolta < quantidadeFrasesVolta; indiceVolta++)
                                            {
                                                f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                                f.Modelo.Textos.Add(new Texto(volta));
                                                f.Modelo.Textos[indiceVolta].AlinhamentoH = ahLocal;
                                                f.Modelo.Textos[indiceVolta].AlinhamentoV = avLocal;
                                                f.Modelo.Textos[indiceVolta].Altura = painelTemp.Altura / painelTemp.MultiLinhas;
                                                f.Modelo.Textos[indiceVolta].Largura = painelTemp.Largura;
                                                f.Modelo.Textos[indiceVolta].AlturaPainel = painelTemp.Altura / painelTemp.MultiLinhas;
                                                f.Modelo.Textos[indiceVolta].LarguraPainel = painelTemp.Largura;
                                                if (indiceVolta + 1 < quantidadeFrasesVolta)
                                                {
                                                    volta = Util.Util.retornarAtributoLinha(input, "Roteiro[" + indiceRoteiro + "].Volta[" + (indiceVolta + 1).ToString() + "].Imagem[0].Rotulo").Trim();
                                                }
                                            }
                                            //carregando a lista de alternancia padrao de um painel multilinhas
                                            painelTemp.ListaAlternancias = CarregarAlternanciaPadraoPainelMultiLinhas();
                                        }


                                        else
                                        {
                                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                            f.Modelo.Textos.Add(new Texto(volta));
                                            f.Modelo.Textos[0].AlinhamentoH = ahLocal;
                                            f.Modelo.Textos[0].AlinhamentoV = avLocal;
                                        }
                                    }
                                    break;
                                case "FormatacaoTipo2": // Numero + Texto
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
                                        f.Modelo.Textos.Add(new Texto(numero));
                                        f.Modelo.Textos.Add(new Texto(volta));
                                        f.Modelo.Textos[0].AlinhamentoH = ahLocal_Numero;
                                        f.Modelo.Textos[0].AlinhamentoV = avLocal_Numero;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal;
                                    }
                                    break;
                                case "FormatacaoTipo3": // Texto Duplo
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
                                        f.Modelo.Textos.Add(new Texto(volta.Split(new char[1] { '•' })[0].Trim()));
                                        f.Modelo.Textos.Add(new Texto(volta.Split(new char[1] { '•' })[1].Trim()));
                                        f.Modelo.Textos[0].AlinhamentoH = ahLocal_Superior;
                                        f.Modelo.Textos[0].AlinhamentoV = avLocal_Superior;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal;

                                    }
                                    break;
                                case "FormatacaoTipo4": // Numero + TextoDuplo
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTextoDuplo;
                                        f.Modelo.Textos.Add(new Texto(numero));
                                        f.Modelo.Textos.Add(new Texto(volta.Split(new char[1] { '•' })[0].Trim()));
                                        f.Modelo.Textos.Add(new Texto(volta.Split(new char[1] { '•' })[1].Trim()));
                                        f.Modelo.Textos[0].AlinhamentoH = ahLocal_Numero;
                                        f.Modelo.Textos[0].AlinhamentoV = avLocal_Numero;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal_Superior;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal_Superior;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal;
                                    }
                                    break;
                                default: // Somente texto
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                        f.Modelo.Textos.Add(new Texto(volta));
                                    }
                                    break;
                            }
                            f.LabelFrase = (painelTemp.MultiLinhas == 1) ? volta : f.LabelFrase;
                            fachada.SetarFontesDefaultFrases(f, (painelTemp.MultiLinhas == 1) ? painelTemp.Altura: painelTemp.Altura / painelTemp.MultiLinhas);
                            for (int i = 0; i < f.Modelo.Textos.Count; i++)
                            {
                                f.Modelo.Textos[i].Apresentacao = rolagemLocal;
                            }
                            r.FrasesVolta.Add(f);
                        }

                        //Setando o label do roteiro. Se for o painel principal
                        if (indicePainel == 0)
                        { 
                            if (r.FrasesIda.Count == 0)
                                r.LabelRoteiro = r.Numero.LabelFrase;
                            else
                                r.LabelRoteiro = (r.FrasesIda[0].LabelFrase.Length > 16 ? r.FrasesIda[0].LabelFrase.Substring(0, 16) : r.FrasesIda[0].LabelFrase);
                        }
                        else
                            r.LabelRoteiro = cTemp.Paineis[0].Roteiros[indiceRoteiro].LabelRoteiro;
                        

                        painelTemp.Roteiros.Add(r);
                    }

                    /* MENSAGEM DE EMERGENCIA */
                    painelTemp.MensagemEmergencia = new MensagemEmergencia(this.carregaIdioma());                    
                    painelTemp.MensagemEmergencia.Frases.Clear();
                    Frase emergencia;
                    if (indicePainel == 0)
                        emergencia = new Frase(Util.Util.retornarAtributoLinha(input, "Mensagem[0].Ida[0].Imagem[0].Rotulo"));
                    else
                        emergencia = new Frase(cTemp.Paineis[0].MensagemEmergencia.Frases[0].LabelFrase);

                    painelTemp.MensagemEmergencia.Frases.Add(emergencia);

                    //setando a fonte default da mensagem de emergência
                    fachada.SetarFontesDefaultFrases(painelTemp.MensagemEmergencia.Frases[0], painelTemp.Altura);


                    /* MENSAGENS */
                    for (int indiceMensagem = 1; indiceMensagem < QtdMensagem; indiceMensagem++)
                    {
                        Mensagem m = new Mensagem();
                        m.ID = indiceMensagem;
                        m.Indice = indiceMensagem - 1;
                        m.Ascendente = true;
                        m.Ordenacao = 255;

                        int quantidadeFrases = Convert.ToInt16(Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].QtdFrases"));

                        for (int indiceFrase = 0; indiceFrase < quantidadeFrases; indiceFrase++)
                        {
                            string formatacaoTipo = Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceFrase.ToString() + "].FormatacaoTipo");                            

                            bool usoSuperior = (formatacaoTipo == "FormatacaoTipo3");

                            Util.Util.AlinhamentoHorizontal ahLocal = Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceFrase.ToString() + "].AlinhamentoHorizontal"));
                            Util.Util.AlinhamentoHorizontal ahLocal_Superior = (usoSuperior) ? Util.Util.RetornaAlinhamentoHorizontalLD6(Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceFrase.ToString() + "].AlinhamentoHorizontal_Superior")) : Util.Util.AlinhamentoHorizontal.Centralizado;

                            Util.Util.AlinhamentoVertical avLocal = Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceFrase.ToString() + "].AlinhamentoVertical"));
                            Util.Util.AlinhamentoVertical avLocal_Superior = (usoSuperior) ? Util.Util.RetornaAlinhamentoVerticalLD6(Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceFrase.ToString() + "].AlinhamentoVertical_Superior")) : Util.Util.AlinhamentoVertical.Centro;

                            Util.Util.Rolagem rolagemLocal = Util.Util.RetornaRolagemLD6(Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceFrase.ToString() + "].RolagemTipo"));
                            
                            // Mensagem
                            string mensagem = Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + indiceFrase.ToString() + "].Imagem[0].Rotulo");
                            
                            //m.LabelMensagem = mensagem;
                            Frase f = new Frase();
                            
                            switch (formatacaoTipo)
                            {
                                case "FormatacaoTipo1": // Somente texto
                                    {                                       
                                        if (painelTemp.MultiLinhas > 1)
                                        {                                        
                                            f.TipoVideo = Util.Util.TipoVideo.V04;
                                            f.TextoAutomatico = false;                                            
                                            f.LabelFrase = carregaIdioma().GetString("ARQUIVO_PAINEL_MULTI_LINHAS");                                                                                        
                                                                                     
                                            for (indiceFrase = 0; indiceFrase < quantidadeFrases; indiceFrase++)
                                            {
                                                f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                                f.Modelo.Textos.Add(new Texto(mensagem));
                                                f.Modelo.Textos[indiceFrase].AlinhamentoH = ahLocal;
                                                f.Modelo.Textos[indiceFrase].AlinhamentoV = avLocal;
                                                f.Modelo.Textos[indiceFrase].Altura = painelTemp.Altura / painelTemp.MultiLinhas;
                                                f.Modelo.Textos[indiceFrase].Largura = painelTemp.Largura;
                                                f.Modelo.Textos[indiceFrase].AlturaPainel = painelTemp.Altura / painelTemp.MultiLinhas;
                                                f.Modelo.Textos[indiceFrase].LarguraPainel = painelTemp.Largura;
                                                if (indiceFrase + 1 < quantidadeFrases)
                                                {
                                                    mensagem = Util.Util.retornarAtributoLinha(input, "Mensagem[" + indiceMensagem + "].Ida[" + (indiceFrase+1).ToString() + "].Imagem[0].Rotulo").Trim();
                                                }
                                            }
                                            //carregando a lista de alternancia padrao de um painel multilinhas
                                            painelTemp.ListaAlternancias = CarregarAlternanciaPadraoPainelMultiLinhas();
                                        }
                                        else
                                        {
                                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                            f.Modelo.Textos.Add(new Texto(mensagem));
                                            f.Modelo.Textos[0].Apresentacao = rolagemLocal;
                                            f.Modelo.Textos[0].AlinhamentoV = avLocal;
                                            f.Modelo.Textos[0].AlinhamentoH = ahLocal;
                                            f.LabelFrase = mensagem;
                                        }
                                    }
                                    break;
                                case "FormatacaoTipo3":// Texto Duplo
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
                                        f.Modelo.Textos.Add(new Texto(mensagem.Split(new char[1] { '•' })[0]));
                                        f.Modelo.Textos.Add(new Texto(mensagem.Split(new char[1] { '•' })[1]));
                                        f.Modelo.Textos[0].Apresentacao = rolagemLocal;
                                        f.Modelo.Textos[0].AlinhamentoV = avLocal_Superior;
                                        f.Modelo.Textos[0].AlinhamentoH = ahLocal_Superior;
                                        f.Modelo.Textos[1].Apresentacao = rolagemLocal;
                                        f.Modelo.Textos[1].AlinhamentoV = avLocal;
                                        f.Modelo.Textos[1].AlinhamentoH = ahLocal;
                                        
                                        f.LabelFrase = mensagem;
                                    }
                                    break;
                                default: // Somente texto
                                    {
                                        f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                                        f.Modelo.Textos.Add(new Texto(mensagem));
                                        f.Modelo.Textos[0].Apresentacao = rolagemLocal;
                                        f.Modelo.Textos[0].AlinhamentoV = avLocal;
                                        f.Modelo.Textos[0].AlinhamentoH = ahLocal;                                    
                                        f.LabelFrase = mensagem;
                                    }
                                    break;
                            }
                            f.LabelFrase = (painelTemp.MultiLinhas == 1) ? mensagem : f.LabelFrase;
                            fachada.SetarFontesDefaultFrases(f, (painelTemp.MultiLinhas == 1) ? painelTemp.Altura : painelTemp.Altura / painelTemp.MultiLinhas);
                            m.Frases.Add(f);
                        }
                        if (m.Frases.Count > 0)
                        {
                            //se for o painel principal
                            if (indicePainel == 0)
                                m.LabelMensagem = (m.Frases[0].LabelFrase.Length > 16 ? m.Frases[0].LabelFrase.Substring(0, 16) : m.Frases[0].LabelFrase);
                            else
                                m.LabelMensagem = cTemp.Paineis[0].Mensagens[m.Indice].LabelMensagem;

                            painelTemp.Mensagens.Add(m);
                        }                        
                        
                    }

                    //Verificando os parametros variaves e fixos de roteiros e mensagens do arquivo em busca de inconsistências
                    if (cTemp.ParametrosVariaveis.RoteiroSelecionado >= painelTemp.Roteiros.Count || cTemp.ParametrosVariaveis.RoteiroSelecionado < 0)
                        cTemp.ParametrosVariaveis.RoteiroSelecionado = 0;


                    if (painelTemp.MensagemSelecionada > painelTemp.Mensagens.Count || painelTemp.MensagemSelecionada <= 0)
                        painelTemp.MensagemSelecionada = 0;
                    else
                        painelTemp.MensagemSelecionada = painelTemp.MensagemSelecionada - 1; // Removendo a Mensagem de Emergência


                    if (painelTemp.MensagemSecundariaSelecionada > painelTemp.Mensagens.Count || painelTemp.MensagemSecundariaSelecionada <= 0)
                        painelTemp.MensagemSecundariaSelecionada = 0;
                    else
                        painelTemp.MensagemSecundariaSelecionada = painelTemp.MensagemSecundariaSelecionada - 1; // Removendo a Mensagem de Emergência



                    /* MENSAGENS ESPECIAIS DE Bom dia, Boa tarde e boa noite */
                    for (int indiceSaudacao = 0; indiceSaudacao < 3; indiceSaudacao++)
                    {
                        string saudacao = Util.Util.retornarAtributoLinha(input, "Saudacao[" + indiceSaudacao + "].Ida[0].Imagem[0].Rotulo").Trim();
                        painelTemp.MensagensEspeciais.Frases[indiceSaudacao] = new Frase(saudacao);
                    }

                    /* MENSAGENS ESPECIAIS DE Domingo, Segunda, Terça, Quarta, Quinta, Sexta e Sábado */
                    for (int indiceDiasSemana = 0; indiceDiasSemana < 7; indiceDiasSemana++)
                    {
                        string diasSemana = Util.Util.retornarAtributoLinha(input, "MensagemEspecial.DiasDaSemana[" + indiceDiasSemana + "].Imagem[0].Rotulo").Trim();
                        painelTemp.MensagensEspeciais.Frases[3 + ((indiceDiasSemana + 1) % 7)] = new Frase(diasSemana);
                    }

                    /* DEMAIS MENSAGENS ESPECIAIS */
                    string hora = "";
                    string data = "";
                    string formatoHora;
                    string unidade_Temperatura;
                    string temperatura;

                    if (cTemp.Regiao.unidadeTemperatura == (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS)
                    {
                        unidade_Temperatura = "°C";
                        temperatura = "30 " + unidade_Temperatura;
                    }
                    else
                    {
                        unidade_Temperatura = "°F";
                        temperatura = "86 " + unidade_Temperatura;
                    }

                    if (cTemp.Regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
                    {
                        if (cTemp.Regiao.opcaoAmPm_Ponto == (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_AM_PM)
                        {
                            formatoHora = " pm";
                        }
                        else
                        {
                            formatoHora = ".";
                        }

                        hora = DateTime.Now.ToString("hh:mm");
                    }
                    else
                    {
                        hora = DateTime.Now.ToString("HH:mm");
                        formatoHora = "";
                    }

                    hora = hora + formatoHora;

                    if (cTemp.Regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
                        data = DateTime.Now.ToString("MM/dd/yyyy");
                    else
                        data = DateTime.Now.ToString("dd/MM/yyyy");

                    painelTemp.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora] = new Frase(hora);
                    painelTemp.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora] = new Frase(painelTemp.MensagensEspeciais.Frases[3 + (int)DateTime.Now.DayOfWeek].LabelFrase + " " + data + " " + hora);


                    string saida = Util.Util.retornarAtributoLinha(input, "MensagemEspecial.NumerosECaracteresEspeciais[13].Imagem[0].Rotulo").Trim();
                    painelTemp.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida] = new Frase(saida);

                    painelTemp.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura] = new Frase(hora + " " + temperatura);
                    painelTemp.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura] = new Frase(data + " " + hora + " " + temperatura);

                    foreach (Frase f in painelTemp.MensagensEspeciais.Frases)
                    {
                        fachada.SetarFontesDefaultFrases(f, painelTemp.Altura);
                    }
                    painelTemp.Indice = cTemp.Paineis.Count;

                    cTemp.Paineis.Add(painelTemp);
                }

            }

            // Verificar esta remoção;
            foreach (Painel p in cTemp.Paineis)
            {
                // parser.
                if (p.MensagemEmergencia.Frases.Count > 1)
                    p.MensagemEmergencia.Frases.RemoveAt(0);
                foreach (Roteiro r in p.Roteiros)
                {
                    if (r.Numero.Modelo.Textos.Count > 1)
                        r.Numero.Modelo.Textos.RemoveAt(0);
                }
            }

            cTemp.ParametrosFixos.MinutosInverterLED = Byte.Parse(Util.Util.retornarAtributoLinha(input, "TempoInversaoBits"));
            

            return cTemp;
        }

        private int RetornarAlternancia(String apresentacao)
        {
            switch (apresentacao)
            {
                case "ApresentaRoteiro":
                    return 0;
                case "ApresentaMensagem":
                    return 1;
                case "AlternaRoteiro_Saudacao":
                    return 2;
                case "AlternaRoteiro_DataHora":
                    return 3;
                case "AlternaRoteiro_HoraPartida":
                    return 4;
                case "AlternaRoteiro_Mensagem":
                    return 5;
                case "ApresentaNumeroRoteiro":
                    return 6;
                case "AlternaRoteiro_MensagemSaudacao":
                    return 7;
                case "AlternaRoteiro_MensagemHoraPartida":
                    return 8;
                case "AlternaRoteiro_MensagemDataHora":
                    return 9;
                case "AlternaRoteiro_Tarifa":
                    return 10;
                case "ApresentaHora":
                    return 0;
                case "AlternaMensagem_Hora":
                    return 0;
                case "AlternaMensagem_Hora_Temp":
                    return 0;
                case "Apenas_Temperatura":
                    return 0;
                case "Apenas_Velocidade":
                    return 0;
                case "AlternaRoteiro_Velocidade":
                    return 0;
                case "AlternaMensagem_Velocidade":
                    return 0;
                case "AlternaRoteiro_Temperatura":
                    return 0;
                case "AlternaMensagem_Temperatura":
                    return 0;
                case "AlternaRoteiro_Hora_Temp":
                    return 0;
                case "AlternaMensagem_Hora_Temp2":
                    return 0;
                case "AlternaNumeroVelocidade":
                    return 0;
                case "RoteiroDataHoraMsg1Msg2":
                    return 0;
                case "RoteiroSaudacaoDataHora":
                    return 0;
                case "NumeroHora":
                    return 0;
                default:
                    return 0;
            }
        }

        // Versão antiga da importação do LDX

        //public Controlador ImportarControladorLDX2(String ArquivoNome)
        //{
        //    Fachada fachada = Fachada.instance;
        //    Controlador cTemp = new Controlador();
        //    ParserLDX parser = new ParserLDX();
        //    parser.CarregarControlador(ArquivoNome);
            
        //    int tempoMensagem = parser.GetTempoMensagem();
        //    int tempoRolagem = parser.GetTempoRolagem();
        //    int tempoRoteiro = parser.GetTempoRoteiro();

        //    // Carregar os Parametros Fixos
        //    cTemp.ParametrosFixos.HoraInicioDia = parser.GetHoraInicioDia();
        //    cTemp.ParametrosFixos.HoraInicioTarde = parser.GetHoraInicioTarde();
        //    cTemp.ParametrosFixos.HoraInicioNoite = parser.GetHoraInicioNoite();
            
        //    cTemp.ParametrosFixos.ApresentacaoMensagensEspeciais = 0;
            
        //    // Carregar o Bloqueio de funções - [TecladoInfo] - AjusteDireito, AjusteEsquerdo, AlternaRoteiroCom, IdaVolta, MensagemDireito, MensagemEsquerdo, OK, RoteiroDireito, RoteiroEsquerdo e SelecionaPainel.
        //    cTemp.ParametrosFixos.BloqueioFuncoes = new bool[16];
        //    //parser.control.MascaraTeclado

        //    cTemp.ParametrosFixos.CaminhoMensagensEspeciais = 0; 
            
        //    // Carregar os Parametros Variaveis
        //    cTemp.ParametrosVariaveis.RoteiroSelecionado = parser.RetornarRoteiroSelecionado();

        //    string regiao = Util.Util.RGN_EN_US;
        //    switch (this.lingua)
        //    {
        //        case Util.Util.Lingua.Ingles: regiao = Util.Util.RGN_EN_US;
        //            break;
        //        case Util.Util.Lingua.Espanhol: regiao = Util.Util.RGN_ES_ES;
        //            break;
        //        case Util.Util.Lingua.Portugues: regiao = Util.Util.RGN_PT_BR;
        //            break;
        //    }
        //    cTemp.Regiao = CarregarRegiao(regiao);
            
            
        //    // Carregar os Roteiros IDA / VOLTA /Número e Carregar as mensagens
        //    for (int indicePainel = cTemp.QuantidadePaineis; indicePainel < parser.quantidadePaineis; indicePainel++)
        //    {
        //        Painel painelTemp = new Painel();
        //        painelTemp.Altura = parser.RetornarAlturaPainel(indicePainel);
        //        painelTemp.Largura = parser.RetornarLarguraPainel(indicePainel);
        //        painelTemp.MensagemSelecionada = parser.RetornarMensagemSelecionada(indicePainel);
        //        painelTemp.MensagemSecundariaSelecionada = parser.RetornarMensagemSelecionada2(indicePainel);
                
        //        /* MENSAGEM DE EMERGENCIA */
        //        painelTemp.MensagemEmergencia = new MensagemEmergencia(this.carregaIdioma());
        //        int quantidadeFrasesMensagemEmergencia = parser.RetornarQuantidadeFrasesMensagem(0, 0);
        //        for (int indiceFrase = 0; indiceFrase < quantidadeFrasesMensagemEmergencia; indiceFrase++)
        //        {                    
        //            string mensagemEmergencia = parser.RetornaLabelMensagem(indicePainel, 0, indiceFrase, 0);
        //            painelTemp.MensagemEmergencia.Frases.Add(new Frase(mensagemEmergencia));                    
        //        }

        //        /* MENSAGEM ESPECIAL */
        //        painelTemp.MensagensEspeciais = new MensagemEspecial(this.carregaIdioma(), this.CarregarRegiao(regiao));
        //        /* Bom dia, Boa tarde e boa noite */
        //        for (int indiceSaudacao = 0; indiceSaudacao < 3; indiceSaudacao++)
        //        {
        //            string saudacao = parser.RetornaLabelSaudacao(indicePainel, indiceSaudacao);
        //            painelTemp.MensagensEspeciais.Frases[indiceSaudacao] = new Frase(saudacao);
        //        }
        //        /* Domingo, Segunda, Terça, Quarta, Quinta, Sexta e Sábado */
        //        for (int indiceDiasSemana = 0; indiceDiasSemana < 7; indiceDiasSemana++)
        //        {
        //            string diasSemana = parser.RetornaLabelDiaSemana(indicePainel, indiceDiasSemana).Trim();
        //            painelTemp.MensagensEspeciais.Frases[3 + ((indiceDiasSemana + 1) % 7)] = new Frase(diasSemana);
        //        }
                              
        //        string hora = "";
        //        string data = "";
        //        string formatoHora;
        //        string unidade_Temperatura;
        //        string temperatura;

        //        if (cTemp.Regiao.unidadeTemperatura == (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS)
        //        {
        //            unidade_Temperatura = "°C";
        //            temperatura = "30 " + unidade_Temperatura;
        //        }
        //        else
        //        { 
        //            unidade_Temperatura = "°F";
        //            temperatura = "86 " + unidade_Temperatura;
        //        }

        //        if (cTemp.Regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
        //        {
        //            if (cTemp.Regiao.opcaoAmPm_Ponto == (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_AM_PM)
        //            {
        //                formatoHora = " pm";
        //            }
        //            else
        //            {
        //                formatoHora = ".";
        //            }

        //            hora = DateTime.Now.ToString("hh:mm");
        //        }
        //        else
        //        {
        //            hora = DateTime.Now.ToString("HH:mm");
        //            formatoHora = "";
        //        }

        //        hora = hora + formatoHora;

        //        if (cTemp.Regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
        //            data = DateTime.Now.ToString("MM/dd/yyyy");
        //        else
        //            data = DateTime.Now.ToString("dd/MM/yyyy");

        //        painelTemp.MensagensEspeciais.Frases[10] = new Frase(hora);
        //        painelTemp.MensagensEspeciais.Frases[11] = new Frase(painelTemp.MensagensEspeciais.Frases[3 + DateTime.Now.Day].LabelFrase + " " + data + " " + hora);

        //        string saida = parser.RetornaLabelSaida(indicePainel).Trim();
        //        painelTemp.MensagensEspeciais.Frases[12] = new Frase(saida);

        //        painelTemp.MensagensEspeciais.Frases[16] = new Frase(hora + " " + temperatura);
        //        painelTemp.MensagensEspeciais.Frases[17] = new Frase(data + " " + hora + " " + temperatura);

        //        foreach (Frase f in painelTemp.MensagensEspeciais.Frases)
        //        {
        //            fachada.SetarFontesDefaultFrases(f, painelTemp.Altura);
        //        }


        //        int quantidadeRoteiro = parser.RetornarQuantidadeRoteiros(indicePainel);
        //        int quantidadeMensagem = parser.retornarQuantidadeMensagens(indicePainel);
                
        //        painelTemp.AlternanciaSelecionada = parser.RetornarAlternancia(indicePainel);

        //        for (int indiceRoteiro = 0; indiceRoteiro < quantidadeRoteiro; indiceRoteiro++)
        //        {
                    
        //            int quantidadeFrasesIda = parser.RetornarQuantidadeFrasesIdaRoteiros(indicePainel, indiceRoteiro, true);
        //            int quantidadeFrasesVolta = parser.RetornarQuantidadeFrasesIdaRoteiros(indicePainel, indiceRoteiro, false);                    
        //            // NUMERO
        //            string numero = parser.RetornarLabelNumero(indicePainel, indiceRoteiro, 0);
        //            Roteiro r = new Roteiro(numero);
        //            r.IdaIgualVolta = false;

        //            for (int indiceFrase = 0; indiceFrase < quantidadeFrasesIda; indiceFrase++ )
        //            {
        //                // IDA
        //                string ida = parser.RetornarLabelRoteiro(indicePainel, indiceRoteiro, indiceFrase, 0, true);
        //                Frase f = new Frase();
        //                switch (parser.RetornarFormatacaoTipoFrasesRoteiro(indicePainel, indiceRoteiro, indiceFrase, true))
        //                {
        //                    case 0: // Somente texto
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
        //                            f.Modelo.Textos.Add(new Texto(ida));
        //                        }
        //                        break;
        //                    case 1: // Numero + Texto
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
        //                            f.Modelo.Textos.Add(new Texto(numero));
        //                            f.Modelo.Textos.Add(new Texto(ida));
        //                        }
        //                        break;
        //                    case 2: // Texto Duplo
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
        //                            f.Modelo.Textos.Add(new Texto(ida.Split(new char[1] { '•' })[0]));
        //                            f.Modelo.Textos.Add(new Texto(ida.Split(new char[1] { '•' })[1]));
                                    
        //                        }
        //                        break;
        //                    case 3: // Numero + TextoDuplo
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTextoDuplo;
        //                            f.Modelo.Textos.Add(new Texto(numero));
        //                            f.Modelo.Textos.Add(new Texto(ida.Split(new char[1] { '•' })[0]));
        //                            f.Modelo.Textos.Add(new Texto(ida.Split(new char[1] { '•' })[1]));
        //                        }
        //                        break;
        //                    default: // Somente texto
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
        //                            f.Modelo.Textos.Add(new Texto(ida));
        //                        }
        //                        break;
        //                }
        //                f.LabelFrase = ida;
        //                fachada.SetarFontesDefaultFrases(f, painelTemp.Altura);
        //                r.FrasesIda.Add(f);
        //                r.LabelRoteiro += " - " + ida;
        //            }

        //            for (int indiceFrase = 0; indiceFrase < quantidadeFrasesVolta; indiceFrase++)
        //            {
        //                // VOLTA
        //                string volta = parser.RetornarLabelRoteiro(indicePainel, indiceRoteiro, indiceFrase, 0, false);
        //                Frase f = new Frase();
        //                switch (parser.RetornarFormatacaoTipoFrasesRoteiro(indicePainel, indiceRoteiro, indiceFrase, false))
        //                {
        //                    case 0: // Somente texto
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
        //                            f.Modelo.Textos.Add(new Texto(volta));
        //                        }
        //                        break;
        //                    case 1: // Numero + Texto
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
        //                            f.Modelo.Textos.Add(new Texto(numero));
        //                            f.Modelo.Textos.Add(new Texto(volta));
        //                        }
        //                        break;
        //                    case 2: // Texto Duplo
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
        //                            f.Modelo.Textos.Add(new Texto(volta.Split(new char[1] { '•' })[0]));
        //                            f.Modelo.Textos.Add(new Texto(volta.Split(new char[1] { '•' })[1]));

        //                        }
        //                        break;
        //                    case 3: // Numero + TextoDuplo
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTextoDuplo;
        //                            f.Modelo.Textos.Add(new Texto(numero));
        //                            f.Modelo.Textos.Add(new Texto(volta.Split(new char[1] { '•' })[0]));
        //                            f.Modelo.Textos.Add(new Texto(volta.Split(new char[1] { '•' })[1]));
        //                        }
        //                        break;
        //                    default: // Somente texto
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
        //                            f.Modelo.Textos.Add(new Texto(volta));
        //                        }
        //                        break;
        //                }
        //                f.LabelFrase = volta;
        //                fachada.SetarFontesDefaultFrases(f, painelTemp.Altura);
        //                r.FrasesVolta.Add(f);
        //            }

        //            painelTemp.Roteiros.Add(r);  
                    
                    
        //        }
        //        for (int indiceMensagem = 1; indiceMensagem < quantidadeMensagem; indiceMensagem++)
        //        {                    
        //            int quantidadeFrases = parser.RetornarQuantidadeFrasesMensagem(indicePainel, indiceMensagem);
        //            for (int indiceFrase = 0; indiceFrase < quantidadeFrases; indiceFrase++)
        //            {                        
        //                // Mensagem
        //                string mensagem = parser.RetornaLabelMensagem(indicePainel, indiceMensagem, indiceFrase, 0);
        //                Frase f = new Frase();
        //                switch (parser.RetornarFormatacaoTipoFrasesRoteiro(indicePainel, indiceMensagem, indiceFrase, true))
        //                {
        //                    case 0: // Somente texto
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
        //                            f.Modelo.Textos.Add(new Texto(mensagem));
        //                        }
        //                        break;
        //                    case 2: // Texto Duplo
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
        //                            f.Modelo.Textos.Add(new Texto(mensagem.Split(new char[1] { '•' })[0]));
        //                            f.Modelo.Textos.Add(new Texto(mensagem.Split(new char[1] { '•' })[1]));

        //                        }
        //                        break;
        //                     default: // Somente texto
        //                        {
        //                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
        //                            f.Modelo.Textos.Add(new Texto(mensagem));
        //                        }
        //                        break;
        //                }

        //                Mensagem m = new Mensagem();
        //                fachada.SetarFontesDefaultFrases(f, painelTemp.Altura);
        //                m.Frases.Add(f);
        //                painelTemp.Mensagens.Add(m);                        
        //            }
        //        }
                

        //        cTemp.Paineis.Add(painelTemp);                
        //    }
            
        //    // Verificar esta remoção;
        //    foreach (Painel p in cTemp.Paineis)
        //    {
        //       // parser.
        //        if (p.MensagemEmergencia.Frases.Count > 1)
        //            p.MensagemEmergencia.Frases.RemoveAt(0);
        //        foreach (Roteiro r in p.Roteiros)
        //        {
        //            if (r.Numero.Modelo.Textos.Count > 1)
        //                r.Numero.Modelo.Textos.RemoveAt(0);
        //        }
        //    }
        //    return cTemp;
        //}



        public Controlador CarregarControlador(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado];
        }

        public void SetarResourceManagerControlador(int controladorSelecionado)
        {
            this.controlador[controladorSelecionado].Rm = this.carregaIdioma();
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                p.rm = this.carregaIdioma();
                p.MensagemEmergencia.rm = this.carregaIdioma();
                p.MensagensEspeciais.rm = this.carregaIdioma();
            }
        }

        #endregion Controlador

        #region Paineis

        public void CopiarPainel(int controladorSelecionado, int painelSelecionado)
        {
            Painel p = new Painel(this.controlador[controladorSelecionado].Paineis[painelSelecionado]);
            p.Indice = this.controlador[controladorSelecionado].Paineis.Count;
            this.controlador[controladorSelecionado].Paineis.Add(p);
        }

        public void AtualizarAlturaTextosPainelMultilinhas(Painel p)
        {
            foreach (Roteiro r in p.Roteiros)
            {
                foreach (Frase f in r.FrasesIda)
                {
                    foreach (Texto t in f.Modelo.Textos)
                        t.AlturaPainel = p.Altura / p.MultiLinhas;
                }

                foreach (Frase f in r.FrasesVolta)
                {
                    foreach (Texto t in f.Modelo.Textos)
                        t.AlturaPainel = p.Altura / p.MultiLinhas;
                }
            }
        }

        public void AtualizarLarguraTextosPainelMultilinhas(Painel p)
        {
            foreach (Roteiro r in p.Roteiros)
            {
                foreach (Frase f in r.FrasesIda)
                {
                    foreach (Texto t in f.Modelo.Textos)
                        t.LarguraPainel = p.Largura;
                }

                foreach (Frase f in r.FrasesVolta)
                {
                    foreach (Texto t in f.Modelo.Textos)
                        t.LarguraPainel = p.Largura;
                }
            }
        }

        public void ConverterPainelMultiLinhas(Painel p, ResourceManager rm)
        {

            if (p.MultiLinhas == 1)
            {
                //Removendo os textos das frases dos roteiros
                foreach (Roteiro r in p.Roteiros)
                {
                    r.FrasesIda.Clear();
                    r.FrasesVolta.Clear();
                }

                //carregando a lista de alternancia padrao de um painel normal
                p.ListaAlternancias = CarregarAlternanciaPadraoPainel();
            }
            else
            {
                //adicionando os textos as frases do roteiro de acordo com a quantidade de paineis multilinhas
                foreach (Roteiro r in p.Roteiros)
                {
                    r.FrasesIda.Clear();
                    r.FrasesVolta.Clear();

                    Frase f = new Frase();
                    f.TipoVideo = Util.Util.TipoVideo.V04;
                    f.TextoAutomatico = false;
                    f.LabelFrase = rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS");
                    f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;

                    for (int i = 0; i < p.MultiLinhas; i++)
                    {
                        Texto t = new Texto(rm.GetString("ARQUIVO_PAINEL") + " " + (i + 1).ToString("00"));
                        t.AlturaPainel = p.Altura / p.MultiLinhas;
                        t.LarguraPainel = p.Largura;
                        f.Modelo.Textos.Add(t);
                    }

                    r.FrasesIda.Add(f);

                    if (!r.IdaIgualVolta)
                        r.FrasesVolta.Add(new Frase(f));
                }

                //carregando a lista de alternancia padrao de um painel multilinhas
                p.ListaAlternancias = CarregarAlternanciaPadraoPainelMultiLinhas();
            }
        }

        public Util.Util.Lingua RetornarLinguaAlternanciaPainel(int controladorSelecionado)
        {
            bool setou = false;
            Util.Util.Lingua linguaAlternancia = this.GetIdiomaFachada();


            ResourceManager rm = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
            if (this.controlador[controladorSelecionado].Paineis[0].ListaAlternancias[0].NomeAlternancia.Contains(rm.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO")))
            { 
                setou = true;
                linguaAlternancia = Util.Util.Lingua.Ingles;
            }

            if (!setou)
            { 
                rm = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                if (this.controlador[controladorSelecionado].Paineis[0].ListaAlternancias[0].NomeAlternancia.Contains(rm.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO")))
                {
                    setou = true;
                    linguaAlternancia = Util.Util.Lingua.Espanhol;
                }
            }

            if (!setou)
            { 
                rm = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                if (this.controlador[controladorSelecionado].Paineis[0].ListaAlternancias[0].NomeAlternancia.Contains(rm.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO")))
                {
                    setou = true;
                    linguaAlternancia = Util.Util.Lingua.Portugues;
                }
            }

            return linguaAlternancia;
        }

        public void AtualizarIdiomaAlternanciaPadrao(int controladorSelecionado)
        {            

            Util.Util.Lingua linguaAlternanciaPainel = RetornarLinguaAlternanciaPainel(controladorSelecionado);

            if (linguaAlternanciaPainel != this.lingua)
            { 

                ResourceManager rmAlternancia = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                ResourceManager rmSoftware = carregaIdioma();

                switch (linguaAlternanciaPainel)
                {
                    case Util.Util.Lingua.Ingles:
                        rmAlternancia = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                        break;
                    case Util.Util.Lingua.Portugues:
                        rmAlternancia = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                        break;
                    case Util.Util.Lingua.Espanhol:
                        rmAlternancia = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                        break;
                }

                //pegando todo o arquivo de recurso
                List<DictionaryEntry> listaAlternancia = rmAlternancia.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true).OfType<DictionaryEntry>().ToList();


                //alterando a lista e deixando apenas os recursos de arquivo de alternancia        
                int i = 0;
                while (i < listaAlternancia.Count)
                {
                    if (listaAlternancia[i].Key.ToString().Contains("ARQUIVO_ALT"))
                        i++;
                    else
                        listaAlternancia.RemoveAt(i);
                }

                //alterando a lista padrao de alternancia de cada painel substituindo os valores da alternancia para a lingua do software
                foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
                {
                    foreach (ItemAlternancia ia in p.ListaAlternancias)
                    {
                        string value = ia.NomeAlternancia.Replace(rmAlternancia.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO"), "").Trim();
                        var chave = listaAlternancia.FirstOrDefault(dictionaryEntry => dictionaryEntry.Value.ToString().Trim() == value);
                       
                        if (chave.Key != null)
                          ia.NomeAlternancia = rmSoftware.GetString("ARQUIVO_ALT_ALTERNA_ROTEIRO") + " " + rmSoftware.GetString(chave.Key.ToString());
                    }
                }
            }   
        }

        public void SetarBrilhoMinPainel(int controladorSelecionado, int painelSelecionado, byte BrilhoMin)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].BrilhoMinimo = BrilhoMin;
        }

        public void SetarBrilhoMaxPainel(int controladorSelecionado, int painelSelecionado, byte BrilhoMax)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].BrilhoMaximo = BrilhoMax;
        }

        public byte GetBrilhoMinPainel(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].BrilhoMinimo;
        }

        public byte GetBrilhoMaxPainel(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].BrilhoMaximo;
        }

        public int GetMultiLinhas(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].MultiLinhas;
        }

        public void SetMultiLinhas(int controladorSelecionado, int painelSelecionado, int MultiLinhas)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].MultiLinhas = MultiLinhas;
        }

        public void SetarListaPadraoAlternanciasPainel(int controladorSelecionado)
        {
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                if (p.ListaAlternancias.Count == 0)
                    p.ListaAlternancias = this.CarregarAlternanciaPadraoPainel();
            }
        }

        public int QuantidadePaineis(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis.Count;
        }

        public void SetarAlternanciaSimulacao(int controladorSelecionado, int painelSelecionado, int alternanciaSelecionada)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].AlternanciaSelecionada = alternanciaSelecionada;
        }

        public int RetornatAlternanciaSimulacao(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].AlternanciaSelecionada;
        }

        public void SetarMensagemSimulacao(int controladorSelecionado, int painelSelecionado, int mensagemSelecionada)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagemSelecionada = mensagemSelecionada;
        }

        public int RetornarMensagemSimulacao(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagemSelecionada;
        }

        public int RetornarMensagemSecundariaSimulacao(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagemSecundariaSelecionada;
        }

        public void SetarMensagemSimulacaoTodosPaineis(int controladorSelecionado, int mensagemSelecionada)
        {
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
                p.MensagemSelecionada = mensagemSelecionada;
        }

        public void SetarMensagemSecundariaSimulacao(int controladorSelecionado, int painelSelecionado, int mensagemSelecionada)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagemSecundariaSelecionada = mensagemSelecionada;
        }

        public List<Painel> CarregarPaineis(int controladorSelecionado)
        {

            return this.controlador[controladorSelecionado].Paineis;
                                
        }

        public Painel CarregarPainel(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado];
        }

        public void IncluirPainel(int controladorSelecionado, Painel painel)
        {
            this.controlador[controladorSelecionado].Paineis.Add(painel);
        }

        public void ExcluirPainel(int controladorSelecionado, int painelSelecionado)
        {
            this.controlador[controladorSelecionado].Paineis.RemoveAt(painelSelecionado);

            //rebalanceia os índices dos Paineis.
            for (int painel = 0; painel < this.controlador[controladorSelecionado].Paineis.Count; painel++)
            {
                this.controlador[controladorSelecionado].Paineis[painel].Indice = painel;
            }

        }

        public void SetAlturaPainel(int controladorSelecionado, int painelSelecionado, int altura)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Altura = altura * this.controlador[controladorSelecionado].Paineis[painelSelecionado].MultiLinhas;
        }

        public void SetLarguraPainel(int controladorSelecionado, int painelSelecionado, int largura)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Largura = largura;
        }
        
        public int GetLarguraPainel(int controladorSelecionado, int painelSelecionado)
        {
           return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Largura;
        }

        public int GetAlturaPainel(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Altura;
        }

        public List<Color[,]> PreparaBitMapPainel(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].PrepararBitMapPainel();
        }

        public void SetarFontesDefaultPainel(Painel p)
        {
            //Setando roteiro
            for (int i = 0; i < p.Roteiros.Count(); i++)
            {
                Roteiro rTemp = new Roteiro(p.Roteiros[i], true);

                foreach (Frase f in rTemp.FrasesIda)
                {
                    foreach (Texto t in f.Modelo.Textos)
                        t.FonteAnteriorWindows = t.FonteWindows;

                    SetarFontesDefaultFrases(f, (p.MultiLinhas == 1 ? p.Altura : p.Altura / p.MultiLinhas));
                }

                foreach (Frase f in rTemp.FrasesVolta)
                {
                    foreach (Texto t in f.Modelo.Textos)
                        t.FonteAnteriorWindows = t.FonteWindows;

                    SetarFontesDefaultFrases(f, (p.MultiLinhas == 1 ? p.Altura : p.Altura / p.MultiLinhas));
                }

                //Setando numero
                rTemp.Numero.Modelo.Textos[0].FonteAnteriorWindows = rTemp.Numero.Modelo.Textos[0].FonteWindows;
                SetarFontesDefaultFrases(rTemp.Numero, (p.MultiLinhas == 1 ? p.Altura : p.Altura / p.MultiLinhas));

                p.Roteiros[i] = rTemp;
            }

            //Setando mensagens 
            for (int i = 0; i < p.Mensagens.Count(); i++)
            {
                Mensagem mTemp = new Mensagem(p.Mensagens[i], true);

                foreach (Frase f in mTemp.Frases)
                {
                    foreach (Texto t in f.Modelo.Textos)
                        t.FonteAnteriorWindows = t.FonteWindows;

                    SetarFontesDefaultFrases(f, (p.MultiLinhas == 1 ? p.Altura : p.Altura / p.MultiLinhas));
                }

                p.Mensagens[i] = mTemp;
            }

            //Setando mensagens especiais
            foreach (Frase f in p.MensagensEspeciais.Frases)
            {
                foreach (Texto t in f.Modelo.Textos)
                    t.FonteAnteriorWindows = t.FonteWindows;

                SetarFontesDefaultFrases(f, (p.MultiLinhas == 1 ? p.Altura : p.Altura / p.MultiLinhas));
            }

            //Setando motoristas
            foreach (Motorista m in p.Motoristas)
            {
                m.ID.Modelo.Textos[0].FonteAnteriorWindows = m.ID.Modelo.Textos[0].FonteWindows;
                m.Nome.Modelo.Textos[0].FonteAnteriorWindows = m.Nome.Modelo.Textos[0].FonteWindows;

                SetarFontesDefaultFrases(m.ID, (p.MultiLinhas == 1 ? p.Altura : p.Altura / p.MultiLinhas));
                SetarFontesDefaultFrases(m.Nome, (p.MultiLinhas == 1 ? p.Altura : p.Altura / p.MultiLinhas));
            }

            //Setando mensagens de emergencia
            p.MensagemEmergencia.Frases[0].Modelo.Textos[0].FonteAnteriorWindows = p.MensagemEmergencia.Frases[0].Modelo.Textos[0].FonteWindows;
            SetarFontesDefaultFrases(p.MensagemEmergencia.Frases[0], (p.MultiLinhas == 1 ? p.Altura : p.Altura / p.MultiLinhas));

            p.FontePath = Util.Util.TrataDiretorio(Util.Util.DIRETORIO_FONTES + p.Roteiros[0].Numero.Modelo.Textos[0].Fonte + Util.Util.ARQUIVO_EXT_FNT);
            
        }

        public void SetarFontePath(int controladorSelecionado, int painelSelecionado, string fonte)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].FontePath = Util.Util.TrataDiretorio(Util.Util.DIRETORIO_FONTES + fonte + Util.Util.ARQUIVO_EXT_FNT);
        }

        //public void ConverterRoteirosMensagensV04emV02(Painel p)
        //{
        //    //Converter as Frases dos Roteiros do Painel
        //    for (int i = 0; i < p.Roteiros.Count(); i++)
        //    {
        //         foreach (Frase f in p.Roteiros[i].FrasesIda)
        //            ConverterFraseV04emV02(f);
        //         foreach (Frase f in p.Roteiros[i].FrasesVolta)
        //            ConverterFraseV04emV02(f);
        //    }

        //    //Converter as Frases das Mensagens do Painel
        //    for (int i = 0; i < p.Mensagens.Count(); i++)
        //    {
        //        foreach (Frase f in p.Mensagens[i].Frases)
        //            ConverterFraseV04emV02(f);
        //    }
        //}

        public void RedimensionarRoteirosMensagens(Painel p, int alturaAnterior, int larguraAnterior)
        {
            //Converter as Frases dos Roteiros do Painel
            for (int i = 0; i < p.Roteiros.Count(); i++)
            {
                foreach (Frase f in p.Roteiros[i].FrasesIda)
                    RedimensionarFrase(f, alturaAnterior , larguraAnterior, p.Altura, p.Largura);
                foreach (Frase f in p.Roteiros[i].FrasesVolta)
                    RedimensionarFrase(f, alturaAnterior, larguraAnterior, p.Altura, p.Largura);
            }

            //Converter as Frases das Mensagens do Painel
            for (int i = 0; i < p.Mensagens.Count(); i++)
            {
                foreach (Frase f in p.Mensagens[i].Frases)
                    RedimensionarFrase(f, alturaAnterior, larguraAnterior, p.Altura, p.Largura);
            }
        }

        public void MoverPainel(int controladoSelecionado, int posicaoInicial, int posicaoFinal)
        {
            Painel painelInicial = new Painel(this.controlador[controladoSelecionado].Paineis[posicaoInicial]);
            painelInicial.Indice = posicaoFinal;

            Painel painelFinal = new Painel(this.controlador[controladoSelecionado].Paineis[posicaoFinal]);
            painelFinal.Indice = posicaoInicial;


            this.controlador[controladoSelecionado].Paineis[posicaoInicial] = painelFinal;
            this.controlador[controladoSelecionado].Paineis[posicaoFinal] = painelInicial;
        }

        #endregion Paineis

        #region Roteiros

        public int LocalizarRoteiros(int controladorSelecionado, int painelSelecionado,int startIndex, string texto, bool matchWholeWord, bool ignoreCase, bool findTexts)
        {
            for(int i = startIndex; i < controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros.Count; i++)
            {
                //Buscar nos labels dos roteiros
                if (!findTexts)
                {
                    if (matchWholeWord)
                    {
                        if (String.Compare(controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].LabelRoteiro, texto, ignoreCase) == 0)
                            return i;
                    }
                    else
                    {
                        if (ignoreCase)
                        {
                            if (controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].LabelRoteiro.ToLower().Contains(texto.ToLower()))
                                return i;
                        }
                        else
                        {
                            if (controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].LabelRoteiro.Contains(texto))
                                return i;
                        }
                    }
                }
                else //Buscar nos labels dos textos
                {
                    //buscando nas frases de ida
                    for(int j=0; j < controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].FrasesIda.Count; j++)
                    {
                        if (matchWholeWord)
                        {
                            if (String.Compare(controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].FrasesIda[j].LabelFrase, texto, ignoreCase) == 0)
                                return i;
                        }
                        else
                        {
                            if (ignoreCase)
                            {
                                if (controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].FrasesIda[j].LabelFrase.ToLower().Contains(texto.ToLower()))
                                    return i;
                            }
                            else
                            {
                                if (controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].FrasesIda[j].LabelFrase.Contains(texto))
                                    return i;
                            }
                        }
                    }

                    //buscando nas frases de volta
                    for (int j = 0; j < controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].FrasesVolta.Count; j++)
                    {
                        if (matchWholeWord)
                        {
                            if (String.Compare(controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].FrasesVolta[j].LabelFrase, texto, ignoreCase) == 0)
                                return i;
                        }
                        else
                        {
                            if (ignoreCase)
                            {
                                if (controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].FrasesVolta[j].LabelFrase.ToLower().Contains(texto.ToLower()))
                                    return i;
                            }
                            else
                            {
                                if (controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[i].FrasesVolta[j].LabelFrase.Contains(texto))
                                    return i;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        public List<Roteiro> CarregarRoteiros(int controladorSelecionado, int painelSelecionado)
        {
            foreach (Roteiro r in this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros)
            {
                if (this.appInstalado)
                {
                    //buscando o arquivo .rota na pasta do APP
                    if (File.Exists(Fachada.diretorio_rotas_app + r.Numero.LabelFrase + Util.Util.ARQUIVO_EXT_ROTA))
                    {
                        if (r.EnvioRoteiroAPP == Util.Util.EnvioRoteiroAPP.NaoTemRotaAPP)
                            r.EnvioRoteiroAPP = Util.Util.EnvioRoteiroAPP.EnviarRotaAPP;
                    }
                    else
                        r.EnvioRoteiroAPP = Util.Util.EnvioRoteiroAPP.NaoTemRotaAPP;
                }
                else
                    r.EnvioRoteiroAPP = Util.Util.EnvioRoteiroAPP.NaoTemRotaAPP;
            }

            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros; 
        }

        public Roteiro CarregarRoteiro(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroSelecionado];
        }


        public void ExcluirRoteiro(int controladorSelecionado, int roteiroSelecionado)
        {
            bool setou = false;
            Roteiro roteiroSimulacao = this.controlador[controladorSelecionado].Paineis[0].Roteiros[(int)this.controlador[controladorSelecionado].ParametrosVariaveis.RoteiroSelecionado];
            Roteiro roteiroExcluido = this.controlador[controladorSelecionado].Paineis[0].Roteiros[roteiroSelecionado];

            //remover a mensagem de todos os paineis
            for (int paineis = 0; paineis < this.controlador[controladorSelecionado].Paineis.Count(); paineis++)
            {

                //removendo o objeto da lista de roteiros
                this.controlador[controladorSelecionado].Paineis[paineis].Roteiros.RemoveAt(roteiroSelecionado);

                //removendo os agendamentos relacionados ao roteiro
                int indiceEvento = 0;
                while (indiceEvento < this.controlador[controladorSelecionado].Paineis[paineis].Eventos.Count)
                {
                    if (this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO && this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].valorParametro == roteiroSelecionado)
                        this.controlador[controladorSelecionado].Paineis[paineis].Eventos.RemoveAt(indiceEvento);
                    else
                        indiceEvento++;
                }

                //Setando o novo indice do roteiro na simulacao
                if (!setou)
                {
                    int indiceRoteiroSimulacao = IndexRoteiro(controladorSelecionado, paineis, roteiroSimulacao.ID);
                    this.SetarRoteiroSimulacao(controladorSelecionado, indiceRoteiroSimulacao);
                    setou = true;
                }

                //rebalanceado os indices dos roteiros em cada painel
                for (int roteiro = 0; roteiro < this.controlador[controladorSelecionado].Paineis[paineis].Roteiros.Count(); roteiro++)
                {
                    this.controlador[controladorSelecionado].Paineis[paineis].Roteiros[roteiro].Indice = roteiro;
                    if (this.controlador[controladorSelecionado].Paineis[paineis].Roteiros[roteiro].ID > roteiroExcluido.ID)
                        this.controlador[controladorSelecionado].Paineis[paineis].Roteiros[roteiro].ID = this.controlador[controladorSelecionado].Paineis[paineis].Roteiros[roteiro].ID - 1;
                }

                //rebalancear os indices dos agendamentos
                indiceEvento = 0;
                while (indiceEvento < this.controlador[controladorSelecionado].Paineis[paineis].Eventos.Count)
                {
                    if (this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO)
                    {
                        if (this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].valorParametro > roteiroExcluido.Indice)
                            this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].valorParametro = Convert.ToUInt16(this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].valorParametro - 1);
                    }

                    indiceEvento++;
                }
            }           
        }

        public void IncluirRoteiro(int controladorSelecionado, Roteiro roteiro, bool copiarTextos)
        {

            int i = 0;
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                //Painel principal
                if (i == 0)
                    p.Roteiros.Add(new Roteiro(roteiro, true));
                else
                {
                    Roteiro rTemp = new Roteiro(roteiro, copiarTextos);

                    //Se for um painel simples
                    if (p.MultiLinhas == 1)
                    {
                        foreach (Frase f in rTemp.FrasesIda)
                        {
                            if (copiarTextos)
                                RedimensionarFrase(f, this.controlador[controladorSelecionado].Paineis[0].Altura, this.controlador[controladorSelecionado].Paineis[0].Largura, p.Altura, p.Largura);

                            SetarFontesDefaultFrases(f, p.Altura);
                        }

                        foreach (Frase f in rTemp.FrasesVolta)
                        {
                            if (copiarTextos)
                                RedimensionarFrase(f, this.controlador[controladorSelecionado].Paineis[0].Altura, this.controlador[controladorSelecionado].Paineis[0].Largura, p.Altura, p.Largura);

                            SetarFontesDefaultFrases(f, p.Altura);
                        }

                        SetarFontesDefaultFrases(rTemp.Numero, p.Altura);

                    }

                    //se for um painel multilinhas
                    if (p.MultiLinhas > 1)
                    {
                        rTemp.FrasesIda.Clear();
                        rTemp.FrasesVolta.Clear();

                        Frase f = new Frase();
                        f.TipoVideo = Util.Util.TipoVideo.V04;
                        f.TextoAutomatico = false;
                        f.LabelFrase = p.MensagensEspeciais.rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS");
                        f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;

                        for (int j = 0; j < p.MultiLinhas; j++)
                        {
                            Texto t = new Texto(p.MensagensEspeciais.rm.GetString("ARQUIVO_PAINEL") + " " + (j + 1).ToString("00"));
                            t.AlturaPainel = p.Altura / p.MultiLinhas;
                            t.LarguraPainel = p.Largura;
                            f.Modelo.Textos.Add(t);
                        }

                        SetarFontesDefaultFrases(f, p.Altura / p.MultiLinhas);

                        rTemp.FrasesIda.Add(f);

                        if (!rTemp.IdaIgualVolta)
                            rTemp.FrasesVolta.Add(new Frase(f));

                        SetarFontesDefaultFrases(rTemp.Numero, p.Altura / p.MultiLinhas);
                    }

                    

                    p.Roteiros.Add(rTemp);
                }
                i++;
            }
        }

        public void CopiarRoteiro(int controladorSelecionado, int roteiroSelecionado, string copia)
        {
            int i = 0;
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                Roteiro r = new Roteiro(p.Roteiros[roteiroSelecionado], true);
                r.Indice = this.QuantidadeRoteiros(controladorSelecionado, i);
                r.ID = this.QuantidadeRoteiros(controladorSelecionado, i) + 1;
                r.LabelRoteiro = r.LabelRoteiro + " - " + copia;
                p.Roteiros.Add(r);
                i++;
            }
        }

        public void EditarRoteiro(int controladorSelecionado, int painelSelecionado, Roteiro roteiro)
        {
            
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                p.Roteiros[roteiro.Indice].LabelRoteiro = roteiro.LabelRoteiro;
                p.Roteiros[roteiro.Indice].Numero.LabelFrase = roteiro.Numero.LabelFrase;
                p.Roteiros[roteiro.Indice].Numero.Modelo.Textos[0].LabelTexto = roteiro.Numero.Modelo.Textos[0].LabelTexto; 
                p.Roteiros[roteiro.Indice].Tarifa = roteiro.Tarifa;
            }
       
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiro.Indice] = new Roteiro(roteiro, true);
        }

        public int QuantidadeRoteiros(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros.Count();
        }

        public string NumeroRoteiro(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroSelecionado].Numero.LabelFrase;
        }
                
        public bool CompararObjetosRoteiro(int controladorSelecionado, int painelSelecionado, Roteiro roteiroGUI)
        {
            return roteiroGUI.CompararObjetosRoteiro(roteiroGUI, this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroGUI.Indice]); 
        }
        public void MoverRoteiro(int controladorSelecionado, int painelSelecionado, int indiceOrigem, int indiceDestino)
        {
            if (indiceOrigem < 0)
            { return; }

            if (indiceDestino < 0)
            { return; }

            if (indiceDestino >= this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros.Count)
            {
                indiceDestino = this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros.Count - 1;
            }

            Roteiro copia = new Roteiro(this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[indiceOrigem], true); // Lembrar de usar o contrutor de cópia de Mensagem e de Roteiro
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros.RemoveAt(indiceOrigem);
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros.Insert(indiceDestino, copia);
        }
        //public void MoverPainel(int controladorSelecionado, int indiceOrigem, int indiceDestino)
        //{
        //    if (indiceOrigem < 0)
        //    { return; }

        //    if (indiceDestino < 0)
        //    { return; }

        //    if (indiceDestino >= this.controlador[controladorSelecionado].Paineis.Count)
        //    {
        //        indiceDestino = this.controlador[controladorSelecionado].Paineis.Count - 1;
        //    }

        //    Painel copia = new Painel(this.controlador[controladorSelecionado].Paineis[indiceOrigem]); 
        //    this.controlador[controladorSelecionado].Paineis.RemoveAt(indiceOrigem);
        //    this.controlador[controladorSelecionado].Paineis.Insert(indiceDestino, copia);
        //}

        public void AtualizarIndicesRoteiros(int controladorSelecionado)
        {
            int indiceRoteiroSimulacao = RetornarParamVariavelRoteiroSelecionado(controladorSelecionado);
            bool setou = false;            

            for (int i = 0; i < this.controlador[controladorSelecionado].Paineis.Count; i++)
            {
                //refaz os indices dos agendamentos para não perder a referencia na reordenação
                AtualizarIndicesRoteirosAgendamento(controladorSelecionado, i);

                //Buscando o objeto que tinha o indice da Simulação e remontando os indices
                for (int j = 0; j < this.controlador[controladorSelecionado].Paineis[i].Roteiros.Count; j++)
                {
                    //Setando o roteiro em parametro variavel
                    if (!setou)
                    { 
                        if (this.controlador[controladorSelecionado].Paineis[i].Roteiros[j].Indice == indiceRoteiroSimulacao)
                        { 
                            this.controlador[controladorSelecionado].ParametrosVariaveis.RoteiroSelecionado = (uint)j;
                            setou = true;
                        }
                    }

                    this.controlador[controladorSelecionado].Paineis[i].Roteiros[j].Indice = j;
                }
            }
        }

        public void AtualizarIndicesRoteirosAgendamento(int controladorSelecionado, int painelSelecionado)
        {
            for (int i = 0; i < this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.Count; i++)
            {
                if (this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO)
                    this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i].valorParametro = (ushort)IndiceRoteiroAfterSortList(controladorSelecionado, painelSelecionado, this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i].valorParametro);
            }
        }

        public int QuantidadeAgendamentosRoteiro(int controladorSelecionado, int roteiroSelecionado)
        {
            int quantidade = 0;

            for (int indicePainel = 0; indicePainel < this.controlador[controladorSelecionado].Paineis.Count; indicePainel++)
            {
                for (int i = 0; i < this.controlador[controladorSelecionado].Paineis[indicePainel].Eventos.Count; i++)
                {
                    if (this.controlador[controladorSelecionado].Paineis[indicePainel].Eventos[i].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO)
                    {
                        if (this.controlador[controladorSelecionado].Paineis[indicePainel].Eventos[i].valorParametro == roteiroSelecionado)
                            quantidade = quantidade + 1;
                    }
                }
            }
            return quantidade;
        }

        public int IndexRoteiro(int controladorSelecionado, int painelSelecionado, int ID)
        {
            int indice = 0;
            bool achou = false;

            foreach (Roteiro r in this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros)
            {
                if (r.ID == ID)
                {
                    achou = true;
                    break;
                }

                indice++;
            }

            if (!achou)
                indice = 0;

            return indice;

        }

        public int IndiceRoteiroAfterSortList(int controladorSelecionado, int painelSelecionado, int IndiceVelho)
        {
            int indice = 0;
            bool achou = false;

            foreach (Roteiro r in this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros)
            {
                if (r.Indice == IndiceVelho)
                {
                    achou = true;
                    break;
                }

                indice++;
            }

            if (!achou)
                indice = 0;

            return indice;

        }

        #endregion Roteiros

        #region Mensagens

        public void AtualizarIndicesMensagensAgendamento(int controladorSelecionado, int painelSelecionado)
        {
            for (int i = 0; i < this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.Count; i++)
            {
                if (this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL || this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_MSG_SECUNDARIA)
                    this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i].valorParametro = (ushort)IndiceMensagemAfterSortList(controladorSelecionado, painelSelecionado, this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i].valorParametro);
            }
        }

        public int LocalizarMensagens(int controladorSelecionado, int painelSelecionado, int startIndex, string texto, bool matchWholeWord, bool ignoreCase, bool findTexts)
        {
            for (int i = startIndex; i < controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens.Count; i++)
            {
                //Buscar nos labels das mensagens
                if (!findTexts)
                {
                    if (matchWholeWord)
                    {
                        if (String.Compare(controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[i].LabelMensagem, texto, ignoreCase) == 0)
                            return i;
                    }
                    else
                    {
                        if (ignoreCase)
                        {
                            if (controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[i].LabelMensagem.ToLower().Contains(texto.ToLower()))
                                return i;
                        }
                        else
                        {
                            if (controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[i].LabelMensagem.Contains(texto))
                                return i;
                        }
                    }
                }
                else //Buscar nos labels dos textos
                {
                    //buscando nas frases
                    for (int j = 0; j < controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[i].Frases.Count; j++)
                    {
                        if (matchWholeWord)
                        {
                            if (String.Compare(controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[i].Frases[j].LabelFrase, texto, ignoreCase) == 0)
                                return i;
                        }
                        else
                        {
                            if (ignoreCase)
                            {
                                if (controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[i].Frases[j].LabelFrase.ToLower().Contains(texto.ToLower()))
                                    return i;
                            }
                            else
                            {
                                if (controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[i].Frases[j].LabelFrase.Contains(texto))
                                    return i;
                            }
                        }
                    }                   
                }
            }

            return -1;
        }

        public void MoverMensagem(int controladorSelecionado, int painelSelecionado, int indiceOrigem, int indiceDestino)
        {
            if (indiceOrigem < 0)
            { return; }

            if (indiceDestino < 0)
            { return; }

            if (indiceDestino >= this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens.Count)
            {
                indiceDestino = this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens.Count - 1;
            }
            Mensagem copia = new Mensagem(this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[indiceOrigem], true); // Lembrar de usar o contrutor de cópia de Mensagem e de Roteiro
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens.RemoveAt(indiceOrigem);
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens.Insert(indiceDestino, copia);
        }

        public void CopiarMensagem(int controladorSelecionado, int mensagemSelecionado, string copia)
        {
            int i = 0;
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                Mensagem m = new Mensagem(p.Mensagens[mensagemSelecionado], true);
                m.Indice = this.QuantidadeMensagens(controladorSelecionado, i);
                m.ID = this.QuantidadeMensagens(controladorSelecionado, i) + 1;
                m.LabelMensagem = m.LabelMensagem + " - " + copia;
                p.Mensagens.Add(m);
                i++;
            }
        }

        public List<Mensagem> CarregarMensagens(int controladorSelecionado, int painelSelecionado) 
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens;
        }

        public void AtualizarIndicesMensagens(int controladorSelecionado)
        {
            for (int i = 0; i < this.controlador[controladorSelecionado].Paineis.Count; i++)
            {
                int indiceAnteriorMensagemSimulacao = RetornarMensagemSimulacao(controladorSelecionado, i);
                int indiceAnteriorMensagemSecSimulacao = RetornarMensagemSecundariaSimulacao(controladorSelecionado, i);

                //refaz os indices dos agendamentos para não perder a referencia na reordenação
                AtualizarIndicesMensagensAgendamento(controladorSelecionado, i);

                //Buscando o objeto que tinha o indice da Simulação e remontando os indices
                for (int j = 0; j < this.controlador[controladorSelecionado].Paineis[i].Mensagens.Count; j++)
                {
                    //Setando a mensagem secundária
                    if (this.controlador[controladorSelecionado].Paineis[i].Mensagens[j].Indice == indiceAnteriorMensagemSecSimulacao)
                        this.controlador[controladorSelecionado].Paineis[i].MensagemSecundariaSelecionada = j;

                    //Setando a mensagem principal
                    if (this.controlador[controladorSelecionado].Paineis[i].Mensagens[j].Indice == indiceAnteriorMensagemSimulacao)
                       this.controlador[controladorSelecionado].Paineis[i].MensagemSelecionada = j;

                    this.controlador[controladorSelecionado].Paineis[i].Mensagens[j].Indice = j;
                }
            }
        }

        public int IndiceMensagemAfterSortList(int controladorSelecionado, int painelSelecionado, int IndiceVelho)
        {
            int indice = 0;
            bool achou = false;

            foreach (Mensagem m in this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens)
            {
                if (m.Indice == IndiceVelho)
                {
                    achou = true;
                    break;
                }

                indice++;
            }

            if (!achou)
                indice = 0;

            return indice;

        }

        public Mensagem CarregarMensagem(int controladorSelecionado, int painelSelecionado, int mensagemSelecionada)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[mensagemSelecionada];
        }

        public void ExcluirMensagem(int controladorSelecionado, int mensagemSelecionada)
        {
            //remover a mensagem de todos os paineis
            for (int paineis = 0; paineis < this.controlador[controladorSelecionado].Paineis.Count(); paineis++)
            {

                //Carregando objetos que estavam em simulacao
                Mensagem msgSimulacao = this.controlador[controladorSelecionado].Paineis[paineis].Mensagens[this.controlador[controladorSelecionado].Paineis[paineis].MensagemSelecionada];
                Mensagem msgSecundariaSimulacao = this.controlador[controladorSelecionado].Paineis[paineis].Mensagens[this.controlador[controladorSelecionado].Paineis[paineis].MensagemSecundariaSelecionada];

                //Carregando objeto que será excluido
                Mensagem msgExcluida = this.controlador[controladorSelecionado].Paineis[paineis].Mensagens[mensagemSelecionada];

                //removendo o objeto da lista de mensagens
                this.controlador[controladorSelecionado].Paineis[paineis].Mensagens.RemoveAt(mensagemSelecionada);

                //removendo os agendamentos relacionados a mensagem
                int indiceEvento = 0;
                while (indiceEvento < this.controlador[controladorSelecionado].Paineis[paineis].Eventos.Count)
                {
                    if ((this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL || this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_MSG_SECUNDARIA) && this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].valorParametro == mensagemSelecionada)
                        this.controlador[controladorSelecionado].Paineis[paineis].Eventos.RemoveAt(indiceEvento);
                    else
                        indiceEvento++;
                }

                //Setando o novo indice da mensagem na simulacao
                int indiceMensagemSimulacao = IndexMensagem(controladorSelecionado, paineis, msgSimulacao.ID);
                this.SetarMensagemSimulacao(controladorSelecionado, paineis, indiceMensagemSimulacao);

                //Setando o novo indice na simulação da mensagem secundária
                int indiceMensagemSecSimulacao = IndexMensagem(controladorSelecionado, paineis, msgSecundariaSimulacao.ID);
                this.SetarMensagemSecundariaSimulacao(controladorSelecionado, paineis, indiceMensagemSecSimulacao);

                //rebalanceado os indices das mensagens em cada painel
                for (int mensagem = 0; mensagem < this.controlador[controladorSelecionado].Paineis[paineis].Mensagens.Count(); mensagem++)
                {
                    this.controlador[controladorSelecionado].Paineis[paineis].Mensagens[mensagem].Indice = mensagem;
                    if (this.controlador[controladorSelecionado].Paineis[paineis].Mensagens[mensagem].ID > msgExcluida.ID)
                        this.controlador[controladorSelecionado].Paineis[paineis].Mensagens[mensagem].ID = this.controlador[controladorSelecionado].Paineis[paineis].Mensagens[mensagem].ID - 1;
                }

                //rebalancear os indices dos agendamentos
                indiceEvento = 0;
                while (indiceEvento < this.controlador[controladorSelecionado].Paineis[paineis].Eventos.Count)
                {
                    if (this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL || this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_MSG_SECUNDARIA)
                    {
                        if (this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].valorParametro > msgExcluida.Indice)
                            this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].valorParametro = Convert.ToUInt16(this.controlador[controladorSelecionado].Paineis[paineis].Eventos[indiceEvento].valorParametro - 1);
                    }

                    indiceEvento++;
                }

            }
        }

        public int QuantidadeMensagens(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens.Count();
        }

        public int QuantidadeAgendamentosMensagem(int controladorSelecionado, int mensagemSelecionada)
        {
            int quantidade = 0;

            for (int indicePainel = 0; indicePainel < this.controlador[controladorSelecionado].Paineis.Count; indicePainel++)
            {
                for (int i = 0; i < this.controlador[controladorSelecionado].Paineis[indicePainel].Eventos.Count; i++)
                {
                    if (this.controlador[controladorSelecionado].Paineis[indicePainel].Eventos[i].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL || this.controlador[controladorSelecionado].Paineis[indicePainel].Eventos[i].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_MSG_SECUNDARIA)
                    {
                        if (this.controlador[controladorSelecionado].Paineis[indicePainel].Eventos[i].valorParametro == mensagemSelecionada)
                            quantidade = quantidade + 1;
                    }
                }
            }
            return quantidade;
        }

        public void IncluirMensagem(int controladorSelecionado, Mensagem mensagem, bool copiarTextos)
        {

            int i = 0;
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                //Painel principal
                if (i == 0)
                    p.Mensagens.Add(new Mensagem(mensagem, true));
                else
                {
                    Mensagem mTemp;
                    if (!copiarTextos)
                    {
                        //Removendo as outras Frases, deve incluir pelo menos um Texto
                        while (mensagem.Frases.Count > 1)
                            mensagem.Frases.RemoveAt(mensagem.Frases.Count - 1);

                        mTemp = new Mensagem(mensagem, true);
                        foreach (Frase f in mTemp.Frases)
                        {
                            RedimensionarFrase(f, this.controlador[controladorSelecionado].Paineis[0].Altura, this.controlador[controladorSelecionado].Paineis[0].Largura, p.Altura, p.Largura);
                            SetarFontesDefaultFrases(f, p.Altura);
                        }

                        p.Mensagens.Add(mTemp);
                    }
                    else
                    { 
                        mTemp = new Mensagem(mensagem, true);
                        foreach (Frase f in mTemp.Frases)
                        {
                            RedimensionarFrase(f, this.controlador[controladorSelecionado].Paineis[0].Altura, this.controlador[controladorSelecionado].Paineis[0].Largura, p.Altura, p.Largura);
                            SetarFontesDefaultFrases(f, p.Altura);
                        }

                        p.Mensagens.Add(mTemp);
                    }
                }
 
                i++;
            }
        }

        public bool CompararObjetosMensagem(int controladorSelecionado, int painelSelecionado, Mensagem mensagemGUI)
        {
            return mensagemGUI.CompararObjetosMensagem(mensagemGUI, this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[mensagemGUI.Indice]);
        }

        public void EditarMensagem(int controladorSelecionado, int painelSelecionado, Mensagem mensagem)
        {
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                p.Mensagens[mensagem.Indice].LabelMensagem = mensagem.LabelMensagem;
            }

            //this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiro.Indice] = new Roteiro(roteiro, true);

            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens[mensagem.Indice] = new Mensagem(mensagem, true);
        }

        public int IndexMensagem(int controladorSelecionado, int painelSelecionado, int ID)
        {
            int indice = 0;
            bool achou = false;

            foreach (Mensagem m in this.controlador[controladorSelecionado].Paineis[painelSelecionado].Mensagens)
            {
                if (m.ID == ID)
                {
                    achou = true;
                    break;
                }

                indice++;
            }

            if (!achou)
                indice = 0;

            return indice;
        }

        #endregion Mensagens

        #region Frase

        public void CriarFraseVoltaPainelMultiLinha(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado, ResourceManager rm)
        {
            Frase f = new Frase();
            f.TipoVideo = Util.Util.TipoVideo.V04;
            f.TextoAutomatico = false;
            f.LabelFrase = rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS");
            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;

            for (int j = 0; j < this.controlador[controladorSelecionado].Paineis[painelSelecionado].MultiLinhas; j++)
            {
                Texto t = new Texto(rm.GetString("ARQUIVO_PAINEL") + " " + (j + 1).ToString("00"));
                t.AlturaPainel = this.controlador[controladorSelecionado].Paineis[painelSelecionado].Altura / this.controlador[controladorSelecionado].Paineis[painelSelecionado].MultiLinhas;
                t.LarguraPainel = this.controlador[controladorSelecionado].Paineis[painelSelecionado].Largura;
                f.Modelo.Textos.Add(t);
            }

            SetarFontesDefaultFrases(f, this.controlador[controladorSelecionado].Paineis[painelSelecionado].Altura / this.controlador[controladorSelecionado].Paineis[painelSelecionado].MultiLinhas);

            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroSelecionado].FrasesVolta.Add(f);
        }

        public bool ChecarFonteFrase(Frase f)
        {
            return f.ChecarFontes(diretorio_fontes);
        }

        public void RedimensionarFrase(Frase f, int alturaAnterior, int larguraAnterior, int alturaNova, int larguraNova)
        {
            int larguraNumero, larguraTexto1, larguraTexto2, larguraTexto3, larguraTexto4, alturaNumero, alturaTexto1, alturaTexto2, alturaTexto3, alturaTexto4;

            switch (f.Modelo.TipoModelo)
            {
                case Util.Util.TipoModelo.Texto:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        f.Modelo.Textos[0].AlturaPainel = alturaNova;
                        f.Modelo.Textos[0].LarguraPainel = larguraNova;
                    }
                    break;

                case Util.Util.TipoModelo.NúmeroTexto:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        //calculando as novas larguras e alturas mantendo as proporções
                        larguraNumero = Convert.ToInt16(((double)f.Modelo.Textos[0].LarguraPainel / (double)larguraAnterior) * larguraNova);
                        larguraTexto1  = larguraNova - larguraNumero;

                        //se os novos valores forem menores que os valores mínimos possiveis para cada frame
                        if (larguraNumero < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraNumero = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto1 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        if (larguraTexto1 < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraTexto1 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraNumero = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        f.Modelo.Textos[0].AlturaPainel = alturaNova;
                        f.Modelo.Textos[0].LarguraPainel = larguraNumero;

                        f.Modelo.Textos[1].AlturaPainel = alturaNova;
                        f.Modelo.Textos[1].LarguraPainel = larguraTexto1;
                    }
                    break;

                case Util.Util.TipoModelo.TextoNúmero:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        larguraNumero = Convert.ToInt16(((double)f.Modelo.Textos[1].LarguraPainel / (double)larguraAnterior) * larguraNova);
                        larguraTexto1 = larguraNova - larguraNumero;

                        //se os novos valores forem menores que os valores mínimos possiveis para cada frame
                        if (larguraNumero < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraNumero = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto1 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        if (larguraTexto1 < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraTexto1 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraNumero = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        f.Modelo.Textos[0].AlturaPainel = alturaNova;
                        f.Modelo.Textos[0].LarguraPainel = larguraTexto1;

                        f.Modelo.Textos[1].AlturaPainel = alturaNova;
                        f.Modelo.Textos[1].LarguraPainel = larguraNumero;
                    }
                    break;

                case Util.Util.TipoModelo.TextoDuplo:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        alturaTexto1 = Convert.ToInt16(((double)f.Modelo.Textos[0].AlturaPainel / (double)alturaAnterior) * alturaNova);
                        alturaTexto2 = alturaNova - alturaTexto1;

                        //se os novos valores forem menores que os valores mínimos possiveis para cada frame
                        if (alturaTexto1 < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                        {
                            alturaTexto1 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto2 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                        }

                        if (alturaTexto2 < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                        {
                            alturaTexto1 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto2 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                        }

                        f.Modelo.Textos[0].AlturaPainel = alturaTexto1;
                        f.Modelo.Textos[0].LarguraPainel = larguraNova;

                        f.Modelo.Textos[1].AlturaPainel = alturaTexto2;
                        f.Modelo.Textos[1].LarguraPainel = larguraNova;
                    }
                    break;
                case Util.Util.TipoModelo.TextoDuploNúmero:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        alturaNumero = alturaNova;
                        alturaTexto1 = Convert.ToInt16(((double)f.Modelo.Textos[0].AlturaPainel / (double)alturaAnterior) * alturaNova);
                        alturaTexto2 = alturaNova - alturaTexto1;

                        larguraNumero = Convert.ToInt16(((double)f.Modelo.Textos[1].LarguraPainel / (double)larguraAnterior) * larguraNova);
                        larguraTexto1 = larguraNova - larguraNumero;
                        larguraTexto2 = larguraNova - larguraNumero;

                        //se os novos valores forem menores que os valores mínimos possiveis para cada frame
                        if (alturaTexto1 < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                        {
                            alturaTexto1 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto2 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                        }

                        if (alturaTexto2 < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                        {
                            alturaTexto1 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto2 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                        }

                        if (larguraNumero < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraNumero = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto1 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto2 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        if (larguraTexto1 < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraTexto1 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto2 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraNumero = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        f.Modelo.Textos[0].AlturaPainel = alturaTexto1;
                        f.Modelo.Textos[0].LarguraPainel = larguraTexto1;

                        f.Modelo.Textos[1].AlturaPainel = alturaNumero;
                        f.Modelo.Textos[1].LarguraPainel = larguraNumero;

                        f.Modelo.Textos[2].AlturaPainel = alturaTexto2;
                        f.Modelo.Textos[2].LarguraPainel = larguraTexto2;
                    }
                    break;

                case Util.Util.TipoModelo.NúmeroTextoDuplo:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        alturaNumero = alturaNova;
                        alturaTexto1 = Convert.ToInt16(((double)f.Modelo.Textos[1].AlturaPainel / (double)alturaAnterior) * alturaNova);
                        alturaTexto2 = alturaNova - alturaTexto1;

                        larguraNumero = Convert.ToInt16(((double)f.Modelo.Textos[0].LarguraPainel / (double)larguraAnterior) * larguraNova);
                        larguraTexto1 = larguraNova - larguraNumero;
                        larguraTexto2 = larguraNova - larguraNumero;

                        //se os novos valores forem menores que os valores mínimos possiveis para cada frame
                        if (alturaTexto1 < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                        {
                            alturaTexto1 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto2 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                        }

                        if (alturaTexto2 < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                        {
                            alturaTexto1 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto2 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                        }

                        if (larguraNumero < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraNumero = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto1 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto2 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        if (larguraTexto1 < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraTexto1 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto2 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraNumero = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }


                        f.Modelo.Textos[0].AlturaPainel = alturaNumero;
                        f.Modelo.Textos[0].LarguraPainel = larguraNumero;

                        f.Modelo.Textos[1].AlturaPainel = alturaTexto1;
                        f.Modelo.Textos[1].LarguraPainel = larguraTexto1;

                        f.Modelo.Textos[2].AlturaPainel = alturaTexto2;
                        f.Modelo.Textos[2].LarguraPainel = larguraTexto2;
                    }
                    break;

                case Util.Util.TipoModelo.TextoDuploTextoDuplo:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        alturaTexto1 = Convert.ToInt16(((double)f.Modelo.Textos[0].AlturaPainel / (double)alturaAnterior) * alturaNova);
                        alturaTexto2 = alturaTexto1;
                        alturaTexto3 = alturaNova - alturaTexto1;
                        alturaTexto4 = alturaTexto3;

                        larguraTexto1 = Convert.ToInt16(((double)f.Modelo.Textos[0].LarguraPainel / (double)larguraAnterior) * larguraNova);
                        larguraTexto2 = larguraNova - larguraTexto1;
                        larguraTexto3 = larguraTexto1;
                        larguraTexto4 = larguraTexto2;

                        //se os novos valores forem menores que os valores mínimos possiveis para cada frame
                        if (alturaTexto1 < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                        {
                            alturaTexto1 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto2 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto3 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto4 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                        }

                        if (alturaTexto3 < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                        {
                            alturaTexto1 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto2 = alturaNova - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto3 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            alturaTexto4 = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                        }

                        if (larguraTexto1 < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraTexto1 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto3 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto2 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto4 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        if (larguraTexto2 < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                        {
                            larguraTexto1 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto3 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto2 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            larguraTexto4 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                        }

                        f.Modelo.Textos[0].AlturaPainel = alturaTexto1;
                        f.Modelo.Textos[0].LarguraPainel = larguraTexto1;

                        f.Modelo.Textos[1].AlturaPainel = alturaTexto2;
                        f.Modelo.Textos[1].LarguraPainel = larguraTexto2;

                        f.Modelo.Textos[2].AlturaPainel = alturaTexto3;
                        f.Modelo.Textos[2].LarguraPainel = larguraTexto3;

                        f.Modelo.Textos[3].AlturaPainel = alturaTexto4;
                        f.Modelo.Textos[3].LarguraPainel = larguraTexto4;
                    }
                    break;

                case Util.Util.TipoModelo.TextoTriplo:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        //se foi alterada a largura
                        if (alturaAnterior == alturaNova)
                        {
                            f.Modelo.Textos[0].LarguraPainel = larguraNova;
                            f.Modelo.Textos[1].LarguraPainel = larguraNova;
                            f.Modelo.Textos[2].LarguraPainel = larguraNova;
                        }
                        else
                        {
                            //se a altura for diferente de 26, transformar para Texto
                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                            f.Modelo.Textos[0].LabelTexto = f.Modelo.Textos[0].LabelTexto + " | " + f.Modelo.Textos[1].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto;
                            f.Modelo.Textos[0].AlturaPainel = alturaNova;
                            f.Modelo.Textos[0].LarguraPainel = larguraNova;

                            f.Modelo.Textos.RemoveAt(2);
                            f.Modelo.Textos.RemoveAt(1);
                        }
                    }

                    if (f.TipoVideo == Util.Util.TipoVideo.V02)
                    {
                        if (alturaAnterior != alturaNova)
                        {
                            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                            f.Modelo.Textos[0].LabelTexto = f.Modelo.Textos[0].LabelTexto + " | " + f.Modelo.Textos[1].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto;
                            f.Modelo.Textos[0].AlturaPainel = 0;
                            f.Modelo.Textos[0].LarguraPainel = 0;
                            f.Modelo.Textos.RemoveAt(2);
                            f.Modelo.Textos.RemoveAt(1);
                        }
                    }

                    break;

                case Util.Util.TipoModelo.NumeroTextoTriplo:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        //se foi alterada a largura
                        if (alturaAnterior == alturaNova)
                        {
                            //calculando as novas larguras e alturas mantendo as proporções
                            larguraNumero = Convert.ToInt16(((double)f.Modelo.Textos[0].LarguraPainel / (double)larguraAnterior) * larguraNova);
                            larguraTexto1 = larguraNova - larguraNumero;
                            larguraTexto2 = larguraTexto1;
                            larguraTexto3 = larguraTexto1;

                            //se os novos valores forem menores que os valores mínimos possiveis para cada frame
                            if (larguraNumero < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                            {
                                larguraNumero = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto1 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto2 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto3 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            }

                            if (larguraTexto1 < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                            {
                                larguraTexto1 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto2 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto3 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraNumero = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            }

                            
                            f.Modelo.Textos[0].LarguraPainel = larguraNumero;
                            f.Modelo.Textos[1].LarguraPainel = larguraTexto1;
                            f.Modelo.Textos[2].LarguraPainel = larguraTexto2;
                            f.Modelo.Textos[3].LarguraPainel = larguraTexto3;
                        }
                        else
                        {
                            //se a altura for diferente de 26, transformar para Texto
                            f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
                            f.Modelo.Textos[0].AlturaPainel = alturaNova;
                            f.Modelo.Textos[1].LabelTexto = f.Modelo.Textos[1].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto + " | " + f.Modelo.Textos[3].LabelTexto;
                            f.Modelo.Textos[1].AlturaPainel = alturaNova;

                            f.Modelo.Textos.RemoveAt(3);
                            f.Modelo.Textos.RemoveAt(2);

                        }
                    }

                    if (f.TipoVideo == Util.Util.TipoVideo.V02)
                    {
                        if (alturaAnterior != alturaNova)
                        {
                            f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
                            f.Modelo.Textos[1].LabelTexto = f.Modelo.Textos[1].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto + " | " + f.Modelo.Textos[3].LabelTexto;
                            f.Modelo.Textos[0].AlturaPainel = 0;
                            f.Modelo.Textos[0].LarguraPainel = 0;
                            f.Modelo.Textos[1].AlturaPainel = 0;
                            f.Modelo.Textos[1].LarguraPainel = 0;
                            f.Modelo.Textos.RemoveAt(3);
                            f.Modelo.Textos.RemoveAt(2);
                        }

                    }

                    break;

                case Util.Util.TipoModelo.TextoTriploNumero:

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        //se foi alterada a largura
                        if (alturaAnterior == alturaNova)
                        {
                            //calculando as novas larguras e alturas mantendo as proporções
                            larguraNumero = Convert.ToInt16(((double)f.Modelo.Textos[1].LarguraPainel / (double)larguraAnterior) * larguraNova);
                            larguraTexto1 = larguraNova - larguraNumero;
                            larguraTexto2 = larguraTexto1;
                            larguraTexto3 = larguraTexto1;

                            //se os novos valores forem menores que os valores mínimos possiveis para cada frame
                            if (larguraNumero < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                            {
                                larguraNumero = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto1 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto2 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto3 = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            }

                            if (larguraTexto1 < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                            {
                                larguraTexto1 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto2 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraTexto3 = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                                larguraNumero = larguraNova - Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            }


                            f.Modelo.Textos[0].LarguraPainel = larguraTexto1;
                            f.Modelo.Textos[1].LarguraPainel = larguraNumero;
                            f.Modelo.Textos[2].LarguraPainel = larguraTexto2;
                            f.Modelo.Textos[3].LarguraPainel = larguraTexto3;
                        }
                        else
                        {
                            //se a altura for diferente de 26, transformar para Texto
                            f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoNúmero;
                            f.Modelo.Textos[0].AlturaPainel = alturaNova;
                            f.Modelo.Textos[0].LabelTexto = f.Modelo.Textos[0].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto + " | " + f.Modelo.Textos[3].LabelTexto;
                            f.Modelo.Textos[1].AlturaPainel = alturaNova;

                            f.Modelo.Textos.RemoveAt(3);
                            f.Modelo.Textos.RemoveAt(2);

                        }
                    }

                    if (f.TipoVideo == Util.Util.TipoVideo.V02)
                    {
                        if (alturaAnterior != alturaNova)
                        {
                            f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoNúmero;
                            f.Modelo.Textos[0].LabelTexto = f.Modelo.Textos[0].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto + " | " + f.Modelo.Textos[3].LabelTexto;
                            f.Modelo.Textos[0].AlturaPainel = 0;
                            f.Modelo.Textos[0].LarguraPainel = 0;
                            f.Modelo.Textos[1].AlturaPainel = 0;
                            f.Modelo.Textos[1].LarguraPainel = 0;
                            f.Modelo.Textos.RemoveAt(3);
                            f.Modelo.Textos.RemoveAt(2);
                        }
                    }

                    break;


            }
        }

        public void AplicarModoApresentacaoLD6(int controladorSelecionado, bool modoApresentacaoDisplayLD6)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.ModoApresentacaoDisplayLD6 = modoApresentacaoDisplayLD6;
        }

        //public void ConverterFraseV04emV02(Frase f)
        //{
        //    if (f.TipoVideo == Util.Util.TipoVideo.V02)
        //    {
        //        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
        //        {
        //            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
        //            f.Modelo.Textos[0].LabelTexto = f.Modelo.Textos[0].LabelTexto + " | " + f.Modelo.Textos[1].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto;
        //            f.Modelo.Textos.RemoveAt(2);
        //            f.Modelo.Textos.RemoveAt(1);
        //        }

        //        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
        //        {
        //            f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
        //            f.Modelo.Textos[1].LabelTexto = f.Modelo.Textos[1].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto + " | " + f.Modelo.Textos[3].LabelTexto;
        //            f.Modelo.Textos.RemoveAt(3);
        //            f.Modelo.Textos.RemoveAt(2);
        //        }

        //        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
        //        {
        //            f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoNúmero;
        //            f.Modelo.Textos[0].LabelTexto = f.Modelo.Textos[0].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto + " | " + f.Modelo.Textos[3].LabelTexto;
        //            f.Modelo.Textos.RemoveAt(3);
        //            f.Modelo.Textos.RemoveAt(2);
        //        }
        //    }

        //    if (f.TipoVideo == Util.Util.TipoVideo.V04)
        //    {
        //        f.TipoVideo = Util.Util.TipoVideo.V02;
        //        for (int j = 0; j < f.Modelo.Textos.Count; j++)
        //        {
        //            f.Modelo.Textos[j].Apresentacao = f.Modelo.Textos[0].Apresentacao;
        //            f.Modelo.Textos[j].TempoApresentacao = f.Modelo.Textos[0].TempoApresentacao;
        //            f.Modelo.Textos[j].TempoRolagem = f.Modelo.Textos[0].TempoRolagem;
        //            f.Modelo.Textos[j].AlturaPainel = 0;
        //            f.Modelo.Textos[j].LarguraPainel = 0;
        //            f.Modelo.Textos[j].listaBitMap.Clear();
        //        }

        //        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
        //        {
        //            f.Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
        //            f.Modelo.Textos[0].LabelTexto = f.Modelo.Textos[0].LabelTexto + " | " + f.Modelo.Textos[1].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto;
        //            f.Modelo.Textos.RemoveAt(2);
        //            f.Modelo.Textos.RemoveAt(1);
        //        }

        //        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
        //        {
        //            f.Modelo.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
        //            f.Modelo.Textos[1].LabelTexto = f.Modelo.Textos[1].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto + " | " + f.Modelo.Textos[3].LabelTexto;
        //            f.Modelo.Textos.RemoveAt(3);
        //            f.Modelo.Textos.RemoveAt(2);
        //        }

        //        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
        //        {
        //            f.Modelo.TipoModelo = Util.Util.TipoModelo.TextoNúmero;
        //            f.Modelo.Textos[0].LabelTexto = f.Modelo.Textos[0].LabelTexto + " | " + f.Modelo.Textos[2].LabelTexto + " | " + f.Modelo.Textos[3].LabelTexto;
        //            f.Modelo.Textos.RemoveAt(3);
        //            f.Modelo.Textos.RemoveAt(2);
        //        }
        //    }

        //}

        public void SetarFontesDefaultFrases(Frase f, int altura) 
        {
            f.SetarFontesDefaultTextos(altura, diretorio_fontes);
        }

        public int GetAlturaTexto(Frase f, int altura, int textoSelecionado)
        {
            return f.GetAlturaTexto(altura, textoSelecionado);
        }

        public int QuantidadeFrasesIda(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroSelecionado].FrasesIda.Count();
        }

        public int QuantidadeFrasesVolta(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroSelecionado].FrasesVolta.Count();
        }

        public void IncluirFraseIda(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado, Frase frase)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroSelecionado].FrasesIda.Add(new Frase(frase));
        }

        public void IncluirFraseVolta(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado, Frase frase)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroSelecionado].FrasesVolta.Add(new Frase(frase));
        }

        public void PreparaBitMapFrase(int altura, int largura, Frase frase)
        {
             frase.PrepararMatrizFrase(altura, largura);
        }

        public void PreparaBitMapCaractere(Frase frase)
        {
            frase.PreparaBitMapCaractere();
        }

        public void RedimensionarCaractere(Frase frase)
        {
            frase.RedimensionarCaractere();
        }

        public void AplicarAlinhamento(Frase frase, Color[,] imagem)
        {
            frase.AplicarAlinhamento(imagem);
        }

        public void InveterLEDBitMapFrase(Frase frase)
        {
            frase.InverterLEDBitMap();
        }

        public bool CompararObjetosFrase(Frase frase1, Frase frase2)
        {
            return frase1.CompararObjetosFrase(frase1, frase2);
        }
        
        public void ConverterMatrizBooleanToColor(Frase frase, Boolean[,] matrizBoolean, int indiceTexto)
        {
            frase.ConverterBooleanToColor(matrizBoolean, indiceTexto);
        }

        #endregion Frase

        #region Fontes
        public List<String> ExibeFontesPontos()
        {
            var arquivos_fonte = Directory.EnumerateFiles(diretorio_fontes, "*" + Util.Util.ARQUIVO_EXT_FNT, SearchOption.AllDirectories);
            List<String> retorno = new List<String>();

            foreach (string arquivo in arquivos_fonte)
            {
                retorno.Add(Path.GetFileNameWithoutExtension(arquivo)); 
            }

            retorno.Reverse();

            return retorno;
        }

        public void SalvarFonte(Arquivo_FNT fonte)
        {
            fonte.Salvar(diretorio_fontes + fonte.NomeArquivo.Trim() + Util.Util.ARQUIVO_EXT_FNT);
        }

        public void AbrirFonte(Arquivo_FNT fonte, string fileName)
        {
            fonte.Abrir(fileName);
        }

        public Boolean ExisteFonte(string fileName)
        {
            bool existe = false;
            if (File.Exists(diretorio_fontes + fileName.Trim() + Util.Util.ARQUIVO_EXT_FNT))
                existe = true;

            return existe;
        }


        #endregion Fontes

        #region Texto

        public int QuantidadeTextos(int controladorSelecionado, int painelSelecionado, int roteiroSelecionado, int fraseSelecionada)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Roteiros[roteiroSelecionado].FrasesIda[fraseSelecionada].Modelo.Textos.Count();
        }

        public void SetarFonteDefaultTexto(Texto t, int altura)
        {
            t.SetarArquivoFonteDefaultTexto(altura, diretorio_fontes);
        }

        public string GetTextoSemImagem(string labelFrase)
        {
            return ProcessamentoDeImagens.GetTextoSemImagem(labelFrase);
        }

        #endregion Texto

        #region Globalização

        public ResourceManager carregaIdioma()
        {
            ResourceManager rmStrings = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));

            switch (this.lingua)
            {
                case Util.Util.Lingua.Ingles:
                    rmStrings = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Portugues:
                    rmStrings = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Espanhol:
                    rmStrings = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Frances:
                    rmStrings = new ResourceManager("Globalization.Frances", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Italiano:
                    rmStrings = new ResourceManager("Globalization.Italiano", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
            }


            return rmStrings;

        }

        #endregion Globalização

        #region Apresentacao

        public List<String> CarregarApresentacao()
        {
            List<String> lista = new List<String>();
            ResourceManager rm = carregaIdioma();

            lista.Clear();
            lista.Add(rm.GetString("COMBO_APRESENTACAO_FIXA"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_CONTINUA_ESQUERDA"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_PAGINADA_ESQUERDA"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_CONTINUA_2_ESQUERDA"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_CIMA"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_BAIXO"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_SURGIMENTO_FORA_HOR"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_SURGIMENTO_FORA_VERT"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_SURGIMENTO_FORA_AMBOS"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_SURGIMENTO_DENTRO_HOR"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_SURGIMENTO_DENTRO_VERT"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_SURGIMENTO_DENTRO_AMBOS"));
            lista.Add(rm.GetString("COMBO_APRESENTACAO_ROLAGEM_CONTINUA_3_ESQUERDA"));

            return lista;
        }


        public List<String> CarregarApresentacaoMultiLinhas()
        {
            List<String> lista = new List<String>();
            ResourceManager rm = carregaIdioma();

            lista.Clear();
            lista.Add(rm.GetString("COMBO_APRESENTACAO_FIXA"));

            return lista;
        }

        #endregion

        #region TipoModelo

        public List<String> CarregarTipoModeloMensagens()
        {
            List<String> lista = new List<String>();
            ResourceManager rm = carregaIdioma();

            lista.Clear();
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO_TEXTO_DUPLO"));

            return lista;
        }

        public List<String> CarregarTipoModeloMensagensTextoTriplo()
        {
            List<String> lista = new List<String>();
            ResourceManager rm = carregaIdioma();

            lista.Clear();
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO_TEXTO_DUPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_TRIPLO"));

            return lista;
        }

        public List<String> CarregarTipoModelo()
        {
            List<String> lista = new List<String>();
            ResourceManager rm = carregaIdioma();

            lista.Clear();
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO"));
            lista.Add(rm.GetString("COMBO_MODELO_N_TEXTO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_N"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_N_TEXTO_DUPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO_N"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO_TEXTO_DUPLO"));

            return lista;
        }

        public List<String> CarregarTipoModeloTextoTriplo()
        {
            List<String> lista = new List<String>();
            ResourceManager rm = carregaIdioma();

            lista.Clear();
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO"));
            lista.Add(rm.GetString("COMBO_MODELO_N_TEXTO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_N"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_N_TEXTO_DUPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO_N"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_DUPLO_TEXTO_DUPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_TRIPLO"));            
            lista.Add(rm.GetString("COMBO_MODELO_N_TEXTO_TRIPLO"));
            lista.Add(rm.GetString("COMBO_MODELO_TEXTO_TRIPLO_N"));

            return lista;
        }
        
        #endregion

        #region MensagemEspecial

        public MensagemEspecial CarregarMensagemEspecial(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagensEspeciais;
        }

        public void EditarMensagemEspecial(int controladorSelecionado, int painelSelecionado, MensagemEspecial mensagemEspecial)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagensEspeciais = new MensagemEspecial(mensagemEspecial);
            
            /* Data e Hora*/
            int fraseSelecionada = (int)Util.Util.IndiceMensagensEspeciais.DataHora;
            this.controlador[controladorSelecionado].ParametrosFixos.dataHora.alinhamento = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].AlinhamentoH;
            this.controlador[controladorSelecionado].ParametrosFixos.dataHora.animacao = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].Apresentacao;
            this.controlador[controladorSelecionado].ParametrosFixos.dataHora.intervaloAnimacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoRolagem;
            this.controlador[controladorSelecionado].ParametrosFixos.dataHora.tempoApresentacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoApresentacao;

            /* Hora de Saída */
            fraseSelecionada = (int)Util.Util.IndiceMensagensEspeciais.HoraSaida;
            this.controlador[controladorSelecionado].ParametrosFixos.horaSaida.alinhamento = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].AlinhamentoH; ;
            this.controlador[controladorSelecionado].ParametrosFixos.horaSaida.animacao = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].Apresentacao;
            this.controlador[controladorSelecionado].ParametrosFixos.horaSaida.intervaloAnimacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoRolagem;
            this.controlador[controladorSelecionado].ParametrosFixos.horaSaida.tempoApresentacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoApresentacao;

            /* Hora Temperatura */
            fraseSelecionada = (int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura;
            this.controlador[controladorSelecionado].ParametrosFixos.horaTemperatura.alinhamento = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].AlinhamentoH; ;
            this.controlador[controladorSelecionado].ParametrosFixos.horaTemperatura.animacao = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].Apresentacao;
            this.controlador[controladorSelecionado].ParametrosFixos.horaTemperatura.intervaloAnimacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoRolagem;
            this.controlador[controladorSelecionado].ParametrosFixos.horaTemperatura.tempoApresentacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoApresentacao;

            /* Somente Hora */
            fraseSelecionada = (int)Util.Util.IndiceMensagensEspeciais.SomenteHora;
            this.controlador[controladorSelecionado].ParametrosFixos.somenteHora.alinhamento = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].AlinhamentoH; ;
            this.controlador[controladorSelecionado].ParametrosFixos.somenteHora.animacao = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].Apresentacao;
            this.controlador[controladorSelecionado].ParametrosFixos.somenteHora.intervaloAnimacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoRolagem;
            this.controlador[controladorSelecionado].ParametrosFixos.somenteHora.tempoApresentacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoApresentacao;

            /* Tarifa */
            fraseSelecionada = (int)Util.Util.IndiceMensagensEspeciais.Tarifa;
            this.controlador[controladorSelecionado].ParametrosFixos.tarifa.alinhamento = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].AlinhamentoH; ;
            this.controlador[controladorSelecionado].ParametrosFixos.tarifa.animacao = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].Apresentacao;
            this.controlador[controladorSelecionado].ParametrosFixos.tarifa.intervaloAnimacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoRolagem;
            this.controlador[controladorSelecionado].ParametrosFixos.tarifa.tempoApresentacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoApresentacao;

            /* Temperatura */
            fraseSelecionada = (int)Util.Util.IndiceMensagensEspeciais.Temperatura;
            this.controlador[controladorSelecionado].ParametrosFixos.temperatura.alinhamento = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].AlinhamentoH; ;
            this.controlador[controladorSelecionado].ParametrosFixos.temperatura.animacao = (byte)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].Apresentacao;
            this.controlador[controladorSelecionado].ParametrosFixos.temperatura.intervaloAnimacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoRolagem;
            this.controlador[controladorSelecionado].ParametrosFixos.temperatura.tempoApresentacao = (uint)mensagemEspecial.Frases[fraseSelecionada].Modelo.Textos[0].TempoApresentacao;

        }

        public bool CompararObjetosMensagemEspecial(int controladorSelecionado, int painelSelecionado, MensagemEspecial mensagemEspecial)
        {
            return mensagemEspecial.CompararObjetosMensagem(mensagemEspecial, this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagensEspeciais);
        }

        public void CriarMensagemEspecial(int controladorSelecionado) 
        {
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                p.MensagensEspeciais = new MensagemEspecial(this.controlador[controladorSelecionado].Rm, this.controlador[controladorSelecionado].Regiao);

                //Setando as mensagens especiais para a altura de cada painel
                foreach (Frase f in p.MensagensEspeciais.Frases)
                    SetarFontesDefaultFrases(f, p.Altura);
            }
        }


        #endregion

        #region MensagemEmergencia

        public MensagemEmergencia CarregarMensagemEmergencia(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagemEmergencia;
        }

        public void EditarMensagemEmergencia(int controladorSelecionado, int painelSelecionado, MensagemEmergencia mensagemEmergencia)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagemEmergencia = new MensagemEmergencia(mensagemEmergencia);
        }

        public bool CompararObjetosMensagemEmergencia(int controladorSelecionado, int painelSelecionado, MensagemEmergencia mensagemEmergencia)
        {
            return mensagemEmergencia.CompararObjetosMensagem(mensagemEmergencia, this.controlador[controladorSelecionado].Paineis[painelSelecionado].MensagemEmergencia);
        }

        public void CriarMensagemEmergencia(int controladorSelecionado)
        {
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                p.MensagemEmergencia = new MensagemEmergencia(this.controlador[controladorSelecionado].Rm);

                //Setando a mensagem de emergencia para a altura de cada painel
                foreach (Frase f in p.MensagemEmergencia.Frases)
                    SetarFontesDefaultFrases(f, p.Altura);
            }
        }

        #endregion

        #region ParametroFixo

        public void SetarSegundosTimeoutFalhaRede(int controladorSelecionado, ushort segundos)
        {
            controlador[controladorSelecionado].ParametrosFixos.timeoutFalhaRede = segundos;
        }

        public void SetarBaudRateSerialControlador(int controladorSelecionado, byte baudRate)
        {
            controlador[controladorSelecionado].ParametrosFixos.baudRate = baudRate;
        }

        public void SetarMinutosInverterLED(int controladorSelecionado, byte minutos)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.MinutosInverterLED = minutos;
        }

        public byte RetornarMinutosInverterLED(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.MinutosInverterLED;
        }

        public void SetarHoraBomDia(int controladorSelecionado, int horaBomDia)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.HoraInicioDia = horaBomDia;
        }

        public void SetarHoraBoaTarde(int controladorSelecionado, int horaBoaTarde)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.HoraInicioTarde = horaBoaTarde;
        }

        public void SetarHoraBoaNoite(int controladorSelecionado, int horaBoaNoite)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.HoraInicioNoite = horaBoaNoite;
        }

        public int RetornarHoraBomDia(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.HoraInicioDia;
        }

        public int RetornarHoraBoaTarde(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.HoraInicioTarde;
        }

        public int RetornarHoraBoaNoite(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.HoraInicioNoite;
        }

        public bool ExistePerifericoSelecionado(int controladorSelecionado)
        {
            bool existe = false;

            for (int i = 0; i < this.controlador[controladorSelecionado].ParametrosFixos.PerifericosNaRede.Length; i++)
            {
                if (this.controlador[controladorSelecionado].ParametrosFixos.PerifericosNaRede[i])
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        public bool[] RetornarPerifericos(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.PerifericosNaRede;
        }

        public void EditarBloqueioFuncoes(int controladorSelecionado, bool[] funcoes)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.BloqueioFuncoes = funcoes;
        }


        public void SetarPaineisAPP(int controladorSelecionado, List<int> paineisAPP)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP = paineisAPP;
        }


        public List<int> RetornarPaineisAPP(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP;
        }

        public void RemoverPainelAPP(int controladorSelecionado, int painel)
        {
            for (int i = 0; i < this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.Count; i++)
            {
                //remove o indice do painel app excluido
                if (this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP[i] == painel)
                {
                    this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.RemoveAt(i);
                    break;
                }
            }

            //se não houver mais paineis app
            if (this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.Count == 0)
                this.controlador[controladorSelecionado].ParametrosFixos.PerifericosNaRede[(int)Util.Util.IndicePerifericosNaRede.APP] = false;
            else
            {
                //rebalenceia o indice do painel app se for maior que o indice removido
                for (int i = 0; i < this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.Count; i++)
                {                    
                    if (this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP[i] > painel)
                        this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP[i] = this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP[i] - 1;
                }
            }
        }

        public void RemoverPainelMultilinhaAPP(int controladorSelecionado, int painel)
        {
            for (int i = 0; i < this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.Count; i++)
            {
                //remove o indice do painel app excluido
                if (this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP[i] == painel)
                {
                    this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.RemoveAt(i);
                    break;
                }
            }

            //se não houver mais paineis app
            if (this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.Count == 0)
                this.controlador[controladorSelecionado].ParametrosFixos.PerifericosNaRede[(int)Util.Util.IndicePerifericosNaRede.APP] = false;

        }

        public void MoverPainelAPP(int controladorSelecionado, int posicaoInicial, int posicaoFinal)
        {
            int indiceInicial = this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.IndexOf(posicaoInicial);
            int indiceFinal = this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.IndexOf(posicaoFinal);

            if (indiceInicial != -1)
                this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP[indiceInicial] = posicaoFinal;

            if (indiceFinal != -1)
                this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP[indiceFinal] = posicaoInicial;

            this.controlador[controladorSelecionado].ParametrosFixos.PaineisAPP.Sort();

        }

        #endregion

        #region ParametrosVariaveis

        public int RetornarParamVariavelRoteiroSelecionado(int controladorSelecionado)
        {
            return (int)this.controlador[controladorSelecionado].ParametrosVariaveis.RoteiroSelecionado;
        }

        public int RetornarParamVariavelMotoristaSelecionado(int controladorSelecionado)
        {
            return (int)this.controlador[controladorSelecionado].ParametrosVariaveis.motoristaSelecionado;
        }

        public bool RetornarParamVariavelSentidaIda(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosVariaveis.SentidoIda;
        }

        public void SetarRoteiroSimulacao(int controladorSelecionado, int roteiroSelecionado)
        {
            this.controlador[controladorSelecionado].ParametrosVariaveis.RoteiroSelecionado = (UInt32)roteiroSelecionado;
        }

        public void SetarMotoristaSimulacao(int controladorSelecionado, int motoristaSelecionado)
        {
            this.controlador[controladorSelecionado].ParametrosVariaveis.motoristaSelecionado = (UInt16)motoristaSelecionado;
        }

        public void SetarRoteiroSimulacaoIdaVolta(int controladorSelecionado, bool roteiroIda)
        {
            this.controlador[controladorSelecionado].ParametrosVariaveis.SentidoIda = roteiroIda;
        }

        #endregion

        #region Senhas

        public void EditarSenhaAntiRoubo(int controladorSelecionado, string senhaAntiRoubo)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.SenhaAntiRoubo = senhaAntiRoubo;
        }

        public void EditarSenhaAcessoEspecial(int controladorSelecionado, string senhaAcessoEspecial)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.SenhaAcessoEspecial = senhaAcessoEspecial;
        }

        public void HabilitarUsoSenha(int controladorSelecionado, bool habilita)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.HabilitaSenha = habilita;
        }

        public void HabilitarUsoLock(int controladorSelecionado, bool habilita)
        {
            this.controlador[controladorSelecionado].ParametrosFixos.HabilitaLock = habilita;
        }

        public bool RetornarHabilitaUsoSenha(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.HabilitaSenha;
        }

        public string RetornarSpecialAccess(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.SenhaAcessoEspecial;
        }

        public string RetornarAntiTheft(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.SenhaAntiRoubo;
        }

        public bool RetornarHabilitaUsoLock(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.HabilitaLock;
        }

        public bool[] RetornarBloqueioFuncoes(int controladorSelecionado)
        {
            return this.controlador[controladorSelecionado].ParametrosFixos.BloqueioFuncoes;

        }

        #endregion

        #region Versao

        public void SetarVersaoUsuario(string versao)
        {
            File.WriteAllText(diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_VERSAO, versao);
        }

        public string GetVersaoUsuario()
        {
            return File.ReadAllText(diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_VERSAO);
        }

        public bool ExisteArquivoVersao()
        {
            bool existe = true;

            if (!File.Exists(diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.ARQUIVO_VERSAO))
                existe = false;

            return existe;
        }

        #endregion

        #region Idioma
        public void SetarIdioma(Util.Util.Lingua idioma)
        {
            this.lingua = idioma;

            File.WriteAllText(diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI, Convert.ToByte(idioma).ToString());

        }

        public Util.Util.Lingua GetIdioma()
        {
            //diretório do aplicativo em programData.
            if (!File.Exists(diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI))
            {
                File.Copy(diretorio_aplicacao + Util.Util.DIRETORIO_IDIOMA_GUI + Util.Util.ARQUIVO_IDIOMA_GUI, diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI, true);
            }

            string text = File.ReadAllText(diretorio_idiomaGUI + Util.Util.ARQUIVO_IDIOMA_GUI);

            switch ((Util.Util.Lingua)Convert.ToByte(text))
            {
                case Util.Util.Lingua.Portugues: this.lingua = Util.Util.Lingua.Portugues; break;
                case Util.Util.Lingua.Espanhol: this.lingua = Util.Util.Lingua.Espanhol; break;
                case Util.Util.Lingua.Ingles: this.lingua = Util.Util.Lingua.Ingles; break;
                //case Util.Util.Lingua.Frances: this.lingua = Util.Util.Lingua.Frances; break;
                default: this.lingua = Util.Util.Lingua.Ingles; break;
            }
            return this.lingua;
        }


        public Util.Util.Lingua GetIdiomaFachada()
        {
            return this.lingua;
        }

        #endregion

        #region NANDFS

        public void GerarDiretoriosFAT(int controladorSelecionado, string arquivoOrigem, string diretorioRaiz)
        {
            this.controlador[controladorSelecionado].GerarDiretoriosFAT(arquivoOrigem, diretorioRaiz);
        }

        public void ExtrairArquivoLDX2(string arquivoOrigem, string diretorioRaiz)
        {
            Memorias.NandFFS m = new Memorias.NandFFS();
            m.GerarDiretoriosFATOtimizado(arquivoOrigem, diretorioRaiz);                           
        }

        public void GerarNandFS(int indiceControlador, string nomeArquivo, bool envioNSS, BackgroundWorker background)
        {
            diretorioOrigem = diretorio_raiz + Util.Util.DIRETORIO_APP;
            diretorio_NSS = diretorioOrigem + Util.Util.DIRETORIO_NSS;
            diretorio_imagens = diretorio_raiz + Util.Util.DIRETORIO_APP + Util.Util.DIRETORIO_BITMAP;

            List<string> lista = new List<string>();

            nomeArquivo = nomeArquivo.Replace("\\\\", "\\");
            diretorioOrigem = diretorioOrigem.Replace("\\\\", "\\");
            diretorio_temporario = diretorio_temporario.Replace("\\\\", "\\");
            diretorio_rgn = diretorio_rgn.Replace("\\\\", "\\");
            diretorio_lpk = diretorio_lpk.Replace("\\\\", "\\");
            diretorio_fontes = diretorio_fontes.Replace("\\\\", "\\");
            diretorio_NSS = diretorio_NSS.Replace("\\\\", "\\");
            diretorio_fir = diretorio_fir.Replace("\\\\", "\\");
            diretorio_imagens = diretorio_imagens.Replace("\\\\", "\\");
            //this.controlador[indiceControlador].Regiao = this.CarregarRegiao(Util.Util.NOME_REGIAO);
            this.controlador[indiceControlador].GerarArquivoNandFS(nomeArquivo, envioNSS, background);           
        }

        #endregion NANDFS

        #region Motoristas
        
        public List<Motorista> CarregarMotoristas(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Motoristas;
        }


        public Motorista CarregarMotorista(int controladorSelecionado, int painelSelecionado, int motoristaSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Motoristas[motoristaSelecionado];
        }

        public void ExcluirMotorista(int controladorSelecionado, int motoristaSelecionado)
        {
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                p.Motoristas.RemoveAt(motoristaSelecionado);
            }
            
        }

        public void AdicionarMotorista(int controladorSelecionado, Motorista motoristaGUI)
        {
            int i = 0;
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                if (i == 0)
                    p.Motoristas.Add(new Motorista(motoristaGUI));
                else
                { 
                    Motorista mTemp = new Motorista(motoristaGUI);

                    SetarFontesDefaultFrases(mTemp.ID, p.Altura);
                    SetarFontesDefaultFrases(mTemp.Nome, p.Altura);

                    p.Motoristas.Add(mTemp);
                }

                i++;

                //p.Motoristas = p.Motoristas.OrderBy(o => o.Nome.LabelFrase).ToList();
            }

        }

        public void EditarMotorista(int controladorSelecionado, int painelSelecionado, int indiceMotorista, Motorista motoristaGUI)
        {
            int i = 0;
            foreach (Painel p in this.controlador[controladorSelecionado].Paineis)
            {
                if (i != painelSelecionado)
                { 
                    p.Motoristas[indiceMotorista].ID.LabelFrase = motoristaGUI.ID.LabelFrase;
                    p.Motoristas[indiceMotorista].ID.Modelo.Textos[0].LabelTexto = motoristaGUI.ID.Modelo.Textos[0].LabelTexto;
                    p.Motoristas[indiceMotorista].Nome.LabelFrase = motoristaGUI.Nome.LabelFrase;
                    p.Motoristas[indiceMotorista].Nome.Modelo.Textos[0].LabelTexto = motoristaGUI.Nome.Modelo.Textos[0].LabelTexto;

                    //p.Motoristas = p.Motoristas.OrderBy(o => o.Nome.LabelFrase).ToList();
                }
                else
                { 
                    p.Motoristas[indiceMotorista] = new Motorista(motoristaGUI);
                    //p.Motoristas = p.Motoristas.OrderBy(o => o.Nome.LabelFrase).ToList();
                }
                i++;
            }           
        }

        public bool AchouMotoristaID(int controladorSelecionado, int motoristaSelecionado, bool isEdicao, Motorista motoristaGUI)
        {
            bool achou = false;

            if (isEdicao)
            {
                for (int i = 0; i < this.controlador[controladorSelecionado].Paineis[0].Motoristas.Count; i++)
                {
                    if (i != motoristaSelecionado)
                    {
                        if (motoristaGUI.AchouID(motoristaGUI, this.controlador[controladorSelecionado].Paineis[0].Motoristas[i]))
                        {
                            achou = true;
                            break;
                        }
                    }
                }
            }

            if (!isEdicao)
            {
                for (int i = 0; i < this.controlador[controladorSelecionado].Paineis[0].Motoristas.Count; i++)
                {
                    if (motoristaGUI.AchouID(motoristaGUI, this.controlador[controladorSelecionado].Paineis[0].Motoristas[i]))
                    {
                        achou = true;
                        break;
                    }                 
                }
            }

            return achou;
        }

        public int IndexMotorista(int controladorSelecionado, int painelSelecionado, string ID)
        {
            int indice = 0;
            bool achou = false;

            foreach (Motorista m in this.controlador[controladorSelecionado].Paineis[painelSelecionado].Motoristas)
            {
                if (m.ID.LabelFrase == ID)
                {
                    achou = true;
                    break;
                }

                indice++;
            }

            if (!achou)
                indice = 0;

            return indice;
        }

        public bool CompararObjetosMotorista(int controladorSelecionado, int painelSelecionado, int motoristaSelecionado, Motorista motoristaGUI)
        {
            return motoristaGUI.CompararObjetosMotoristas(motoristaGUI, this.controlador[controladorSelecionado].Paineis[painelSelecionado].Motoristas[motoristaSelecionado]);
        }

        #endregion

        #region Agendamento

        public void ExcluirEventoAlternancia(int controladorSelecionado, int painelSelecionado)
        {
            //removendo os agendamentos relacionados a mensagem
            int indiceEvento = 0;
            while (indiceEvento < this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.Count)
            {
                if (this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[indiceEvento].Operacao == Util.Util.TipoOperacaoEvento.SELECAO_ALTERNANCIA)
                    this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.RemoveAt(indiceEvento);
                else
                    indiceEvento++;
            }
        }

        public List<String> CarregarOperacoesAgendamento()
        {
            List<String> lista = new List<String>();
            ResourceManager rm = carregaIdioma();

            lista.Clear();
            lista.Add(rm.GetString("COMBO_OPERACOES_AGENDAMENTO_ROTEIRO"));
            lista.Add(rm.GetString("COMBO_OPERACOES_AGENDAMENTO_SENTIDO"));
            lista.Add(rm.GetString("COMBO_OPERACOES_AGENDAMENTO_ALTERNANCIA"));
            lista.Add(rm.GetString("COMBO_OPERACOES_AGENDAMENTO_MENSAGEM"));
            lista.Add(rm.GetString("COMBO_OPERACOES_AGENDAMENTO_MENSAGEM_SEC"));
            lista.Add(rm.GetString("COMBO_OPERACOES_AGENDAMENTO_HORA_SAIDA"));

            return lista;
        }

        public List<Evento> CarregarEventos(int controladorSelecionado, int painelSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos;
        }

        public void AdicionarEvento(int controladorSelecionado, int painelSelecionado, Evento evento)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.Add(new Evento(evento));
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos = this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.OrderBy(o => o.DataHora).ToList();
        }

        public void EditarEvento(int controladorSelecionado, int painelSelecionado, int eventoSelecionado, Evento evento)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[eventoSelecionado] = new Evento(evento);
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos = this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.OrderBy(o => o.DataHora).ToList();
        }

        public void ExcluirEvento(int controladorSelecionado, int painelSelecionado, int eventoSelecionado)
        {
            this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.RemoveAt(eventoSelecionado);
        }

        public Evento CarregarEvento(int controladorSelecionado, int painelSelecionado, int eventoSelecionado)
        {
            return this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[eventoSelecionado];
        }

        public bool CompararObjetoEvento(int controladorSelecionado, int painelSelecionado, int eventoSelecionado, Evento eventoGUI)
        {
            return eventoGUI.CompararTodoObjetosEvento(eventoGUI, this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[eventoSelecionado]);
        }

        public int AchouObjetoEvento(int controladorSelecionado, int painelSelecionado, int indiceEvento, Evento eventoGUI)
        {
            int achou = 0;

            switch (eventoGUI.Operacao)
            {
                case Util.Util.TipoOperacaoEvento.SELECAO_ROTEIRO:
                    achou = AchouObjetoEventoTodosPaineis(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
                    break;
                case Util.Util.TipoOperacaoEvento.SELECAO_IDA_VOLTA:
                    achou = AchouObjetoEventoTodosPaineis(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
                    break;
                case Util.Util.TipoOperacaoEvento.SELECAO_ALTERNANCIA:
                    achou = AchouObjetoEventoPainel(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
                    break;
                case Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL:
                    achou = AchouObjetoEventoPainel(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
                    break;
                case Util.Util.TipoOperacaoEvento.SELECAO_MSG_SECUNDARIA:
                    achou = AchouObjetoEventoPainel(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
                    break;
                case Util.Util.TipoOperacaoEvento.ALTERACAO_HORA_SAIDA:
                    achou = AchouObjetoEventoTodosPaineis(controladorSelecionado, painelSelecionado, indiceEvento, eventoGUI);
                    break;
            }

            return achou;
        }

        private int AchouObjetoEventoPainel(int controladorSelecionado, int painelSelecionado, int indiceEvento, Evento eventoGUI)
        {
            int achou = 0;

            for (int i = 0; i < this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos.Count; i++)
            {
                if (i != indiceEvento)
                {
                    if (!eventoGUI.CompararDataHoraOperacao(eventoGUI, this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos[i]))
                    {
                        achou = 1;
                        break;
                    }
                }
            }
            return achou;
        }

        private int AchouObjetoEventoTodosPaineis(int controladorSelecionado, int painelSelecionado, int indiceEvento, Evento eventoGUI)
        {
            int achou = 0;

            for (int i = 0; i < this.controlador[controladorSelecionado].Paineis.Count; i++)
            { 
                for (int j=0; j < this.controlador[controladorSelecionado].Paineis[i].Eventos.Count; j++)
                {
                    //não verificar ele mesmo se for edição do objeto evento
                    if (i == painelSelecionado && indiceEvento == j)
                        continue;
                    
                    if (!eventoGUI.CompararDataHoraOperacao(eventoGUI, this.controlador[controladorSelecionado].Paineis[i].Eventos[j]))
                    {
                        achou = 2;
                        break;
                    }
                    
                }

                if (achou != 0)
                    break;
            }

            return achou;

        }

        public int RetornaIndiceEvento(int controladorSelecionado, int painelSelecionado, int eventoSelecionado, Evento eventoGUI)
        {
            int indice = 0;

            foreach (Evento e in this.controlador[controladorSelecionado].Paineis[painelSelecionado].Eventos)
            {
                if (!eventoGUI.CompararTodoObjetosEvento(eventoGUI, e))
                    break;

                indice++;
            }

            return indice;
        }

        #endregion

        #region Firmware

        public List<Arquivo_FIR.IdentificacaoProduto> CarregarListaFirmwares()
        {
            string caminhoFirmware = diretorio_fir;

            string[] arquivos = Directory.GetFiles(caminhoFirmware, "*.fir");

            List<Arquivo_FIR.IdentificacaoProduto> lista = new List<Arquivo_FIR.IdentificacaoProduto>();

            foreach (string arquivo in arquivos)
            {
                Arquivo_FIR firmware = new Arquivo_FIR();
                lista.Add(firmware.IdentificarProduto(arquivo));

            }
            return lista;
        }
        public Arquivo_FIR.IdentificacaoProduto IdentificarProduto(string nomeArquivo)
        {
            Arquivo_FIR firmware = new Arquivo_FIR();
            return firmware.IdentificarProduto(nomeArquivo);
        }

        public int VerificarFirmware(string nomeArquivo)
        {
            string caminhoFirmware = diretorio_fir;

            string[] arquivos = Directory.GetFiles(caminhoFirmware, "*.fir");

            List<Arquivo_FIR.IdentificacaoProduto> lista = CarregarListaFirmwares();

            Arquivo_FIR firmware = new Arquivo_FIR();
            Arquivo_FIR.IdentificacaoProduto produto = firmware.IdentificarProduto(nomeArquivo);
            int indice = -1;
            foreach (Arquivo_FIR.IdentificacaoProduto arquivo in lista)
            {
                if (arquivo.nomeProduto == produto.nomeProduto)
                {
                    indice = lista.IndexOf(arquivo);
                    break;
                }                
            }
            if (lista.Count == 0)
                return 0;

            if ((indice == -1)&& (lista.Count>0))
            {
                //throw new Exception("Produto não encontrado");
                return 0;
            }
            return produto.versao.CompareTo(lista[indice].versao);
        }
        public void CopiarFirmwareTurbusParaProgramData(string origem)
        {                                  
            string destino = diretorio_fir;
            destino += Util.Util.ARQUIVO_FIR_LDX2;
            
            if (File.Exists(destino))
            {
                // Renomear o firmware.fir para _firmware.fir
                File.Copy(destino, diretorio_fir + "_" + Util.Util.ARQUIVO_FIR_LDX2, true);
                File.Delete(destino);
            }
            // Copiar TURBUS para ProgramData com o nome firmware.fir;
            if (VerificarFirmware(origem) >= 0)
            {
                File.Copy(origem, destino, true);
            }
        }

        public void CopiarFirmwareParaProgramData(string origem)
        {
            string destino = diretorio_fir;
            destino += Path.GetFileName(origem);
            if (VerificarFirmware(origem) >= 0)
            {
                File.Copy(origem, destino, true);
            }
        }

        public void CopiarFirmwareProgramDataParaDiretorio(string destino)
        {
            string origem = diretorio_fir;

            destino += "\\" + Util.Util.DIRETORIO_APP_NFX;

            if (!Directory.Exists(destino))
            {
                Directory.CreateDirectory(destino);
            }
            destino += Util.Util.DIRETORIO_FIRMWARE;

            if (!Directory.Exists(destino))
            {
                Directory.CreateDirectory(destino);
            }

            string[] arquivos = Directory.GetFiles(origem);

            foreach(string arquivo in arquivos)
            {
                File.Copy(arquivo, destino + Path.GetFileName(arquivo), true);
            }       
        }

        #endregion Firmware

    }
}
