using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Controlador;
using Nucleo;
using Persistencia;
using Persistencia.Videos;
using Util;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Resources;
using System.Threading;
using System.Reflection;
using Globalization;
using Ionic.Zip;
using System.Security.Cryptography;


namespace Controlador
{
    public class Controlador
    {        
        //VARIÁVEIS AUXILIARES
        public int NUM_PAINEIS = 1;

        public ResourceManager Rm { get; set; }
        public Arquivo_RGN Regiao { get; set; }
        public List<Painel> Paineis { get; set; }        
        public String Familia { get; set; }
        public String Versao { get; set; }
        public ParametrosFixos ParametrosFixos { get; set; }
        public ParametrosVariaveis ParametrosVariaveis { get; set; }
        public string StatusTransmissao { get; set; }
        public bool CancelarEnvioNFX;

        public int QuantidadePaineis
        {
            get { return Paineis.Count; }
            set { }

        }

        //public String Regioes { get; set; }

        //Construtor
        public Controlador()
        {
            Fachada fachada = Fachada.Instance;            

            this.Familia = "12.0.*";
            this.Versao = "12.0.*";
            this.ParametrosFixos = new ParametrosFixos();
            this.ParametrosVariaveis = new ParametrosVariaveis();
            this.Paineis = new List<Painel>();
            this.QuantidadePaineis = 0;            
            //this.Regioes = "pt-br";
            Rm = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
            //this.Idiomas = new List<Arquivo_LPK>();

        }

        public Controlador(ResourceManager rm, Arquivo_RGN argn)
        {
            
            this.Familia = "12.0.*";
            this.Versao = "12.0.*";
            this.ParametrosFixos = new ParametrosFixos();
            this.ParametrosVariaveis = new ParametrosVariaveis();
            this.Paineis = new List<Painel>();
            this.QuantidadePaineis = 0;
            this.Regiao = argn;
            this.Rm = rm;
        }

        public Controlador(Controlador OldValue)
        {
            Familia = OldValue.Familia;
            Versao = OldValue.Versao;
            ParametrosFixos = new ParametrosFixos(OldValue.ParametrosFixos);
            ParametrosVariaveis = new ParametrosVariaveis(OldValue.ParametrosVariaveis);

            this.Paineis = new List<Painel>();
            foreach (Painel p in OldValue.Paineis)
            {
                Paineis.Add(new Painel(p));
            }

            //Regioes = OldValue.Regioes;
            this.Rm = OldValue.Rm;
            this.QuantidadePaineis = OldValue.QuantidadePaineis;
            this.Regiao = OldValue.Regiao;
        }
      
        /// <summary>
        /// Cria valores de teste para popular o controlador.
        /// </summary>
        public void ControladorDefault()
        {
            this.Familia = "12.0.*";
            this.Versao = "12.0.*";
            //cria os paineis.
            for (int painel = 0; painel < NUM_PAINEIS; painel++)
            {
                Painel temp = new Painel(this.Rm);

                temp.Indice = painel;
                temp.Altura = 16;
                temp.Largura = 144;
                temp.PainelDefault(3, 3, painel);
                this.Paineis.Add(temp);
            }
        }
        public void ControladorDefault(int quantidadePaineis, int quantidadeRoteiros, int quantidadeMensagens, ResourceManager rm, Arquivo_RGN regiao)
        {            
            this.Familia = "12.0.*";
            this.Versao = "12.0.*";
            NUM_PAINEIS = quantidadePaineis;

            //cria os paineis.
            for (int painel = 0; painel < quantidadePaineis; painel++)
            {
                Painel temp = new Painel(rm);                
                temp.Indice = painel;
                temp.Altura = 16;
                temp.Largura = 144;
                temp.PainelDefault(quantidadeMensagens, quantidadeRoteiros, painel);
                temp.MensagensEspeciais = new MensagemEspecial(rm, regiao);
                temp.MensagemEmergencia = new MensagemEmergencia(rm);
                temp.MensagemSelecionada = 0;
                temp.AlternanciaSelecionada = 0;
                temp.ListaAlternancias = new List<ItemAlternancia>();
                temp.BrilhoMaximo = 100;
                temp.BrilhoMinimo = 10;
                temp.Eventos = new List<Evento>();
                temp.Motoristas = new List<Motorista>();
                if (this.Paineis.Count != quantidadePaineis)
                {
                    this.Paineis.Add(temp);
                }
            }
        }
        //RGNS
        public void CopiarArquivosRegiaoRGN(String diretorio_raiz, String diretorio_destino)
        {
            CopiarArquivos(diretorio_raiz, diretorio_destino, Util.Util.ARQUIVO_EXT_RGN);
            GerarArquivosRegiaoLST(diretorio_raiz, diretorio_destino);
        }

        public void CopiarArquivosPacotesIdiomasLPK(String diretorio_raiz, String diretorio_destino)
        {
            CopiarArquivos(diretorio_raiz, diretorio_destino, Util.Util.ARQUIVO_EXT_LPK);
            GerarArquivosIdiomasLST(diretorio_raiz, diretorio_destino);
        }

        public void CopiarArquivos(String diretorio_raiz, String diretorio_destino, String extensao)
        {
            List<String> arquivos = new List<string>();
            
            // André -> 10-12-2014
            if (!Directory.Exists(diretorio_destino))
                Directory.CreateDirectory(diretorio_destino);

            arquivos.AddRange(Directory.EnumerateFiles(diretorio_raiz, "*" + extensao, SearchOption.TopDirectoryOnly));

            foreach (string s in arquivos)
            {
                if (Path.GetFileNameWithoutExtension(s).Length > 8)
                {
                    File.Copy(s, diretorio_destino + @"\" + Path.GetFileNameWithoutExtension(s).Substring(0, 8) + Path.GetExtension(s), true);
                }
                else
                {
                    File.Copy(s, diretorio_destino + @"\" + Path.GetFileNameWithoutExtension(s) + Path.GetExtension(s), true);
                }
            }
        }
        public void CopiarTodosArquivos(String diretorio_raiz, String diretorio_destino, String extensao)
        {
            List<String> arquivos = new List<string>();

            // André -> 10-12-2014
            if (!Directory.Exists(diretorio_destino))
                Directory.CreateDirectory(diretorio_destino);

            arquivos.AddRange(Directory.EnumerateFiles(diretorio_raiz, "*" + extensao, SearchOption.AllDirectories));

            foreach (string s in arquivos)
            {
                string arquivoDestino = Path.GetDirectoryName(s).Replace(diretorio_raiz, String.Empty);

                
                if (!Directory.Exists(diretorio_destino + arquivoDestino))
                {
                    Directory.CreateDirectory(diretorio_destino + arquivoDestino);
                }

                if (Path.GetFileNameWithoutExtension(s).Length > 8)
                {
                    File.Copy(s, diretorio_destino + arquivoDestino + @"\" + Path.GetFileNameWithoutExtension(s).Substring(0, 8) + Path.GetExtension(s), true);
                }
                else
                {
                    File.Copy(s, diretorio_destino + arquivoDestino + @"\" + Path.GetFileNameWithoutExtension(s) + Path.GetExtension(s), true);
                }
            }
        }
        //FONTES
        public void CopiarArquivosFonte(String diretorio_raiz, String diretorio_destino)
        {
            List<String> arquivos = new List<string>();

            // André -> 10-12-2014
            if (!Directory.Exists(diretorio_destino))
                Directory.CreateDirectory(diretorio_destino);

            arquivos.AddRange(Directory.EnumerateFiles(diretorio_raiz, "*" + Util.Util.ARQUIVO_EXT_FNT, SearchOption.TopDirectoryOnly));

            foreach (string s in arquivos)
            {
                string arquivoDestino = RetornarNomeFonte(s, arquivos);

                File.Copy(s, diretorio_destino + @"\" + arquivoDestino, true);

            }         
        }

        private string RetornarNomeFonte(string arquivoNome, List<String> arquivos)
        {
            string arquivoDestino = arquivos.IndexOf(arquivoNome).ToString() + Util.Util.ARQUIVO_EXT_FNT;

            return arquivoDestino;
        }

        public void GerarArquivosRegiaoLST(String diretorio_raiz, String diretorio_destino)
        {
            Arquivo_LST regioes = new Arquivo_LST();
            List<String> rgns = new List<string>();

            rgns.AddRange(Directory.EnumerateFiles(diretorio_raiz, "*" + Util.Util.ARQUIVO_EXT_RGN, SearchOption.TopDirectoryOnly));

            int regiaoSelecionada = 0;
            foreach (string s in rgns)
            {
                regioes.listaPaths.Add(Path.GetFileNameWithoutExtension(s));

                if (Path.GetFileNameWithoutExtension(s) == Regiao.Nome.Trim().TrimEnd('\0'))
                    this.ParametrosVariaveis.RegiaoSelecionada = (byte)regiaoSelecionada;

                regiaoSelecionada = regiaoSelecionada + 1;
            }

            regioes.Salvar(diretorio_destino + Util.Util.ARQUIVO_LST_RGN);
        }

        public void GerarArquivosIdiomasLST(String diretorio_raiz, String diretorio_destino)
        {
            Arquivo_LST idiomas = new Arquivo_LST();
            List<String> langs = new List<string>();

            langs.AddRange(Directory.EnumerateFiles(diretorio_raiz, "*" + Util.Util.ARQUIVO_EXT_LPK));

            foreach (string s in langs)
            {
                idiomas.listaPaths.Add(Path.GetFileNameWithoutExtension(s));
            }

            idiomas.Salvar(diretorio_destino + Util.Util.ARQUIVO_LST_LPK);
        }

        public void Salvar(String ArquivoNome) 
        {
            //limpando as matrizes de bitmaps para salvar
            foreach (Painel p in this.Paineis)
            {
                //Limpando os bitmaps de Roteiros
                foreach (Roteiro r in p.Roteiros)
                {
                    r.Numero.Modelo.Textos[0].listaBitMap.Clear();

                    foreach (Frase f in r.FrasesIda)
                        foreach (Texto t in f.Modelo.Textos)
                            t.listaBitMap.Clear();

                    foreach (Frase f in r.FrasesVolta)
                        foreach (Texto t in f.Modelo.Textos)
                            t.listaBitMap.Clear();
                }

                //Limpando os bitmaps de Mensagens
                foreach (Mensagem m in p.Mensagens)
                {
                    foreach (Frase f in m.Frases)
                        foreach (Texto t in f.Modelo.Textos)
                            t.listaBitMap.Clear();
                }

                //Limpando os bitmaps de Motoristas
                foreach (Motorista m in p.Motoristas)
                {
                    m.ID.Modelo.Textos[0].listaBitMap.Clear();
                    m.Nome.Modelo.Textos[0].listaBitMap.Clear();
                }

                //Limpando os bitmaps de Mensagem Especial
                foreach (Frase f in p.MensagensEspeciais.Frases)
                    f.Modelo.Textos[0].listaBitMap.Clear();

                //Limpando os bitmaps de Mensagem Emergencia
                p.MensagemEmergencia.Frases[0].Modelo.Textos[0].listaBitMap.Clear();

            }

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);           
            System.IO.File.WriteAllText(ArquivoNome, json);

            using (ZipFile zip = new ZipFile())
            {
                zip.AddFile(ArquivoNome, "");
                zip.Save(ArquivoNome);
            }
        }

        public Controlador Abrir(String ArquivoNome)
        {
            // TODO: Fazer as devidas alterações para que a fachada utilize este método.
            String Json = String.Empty;
            Json = System.IO.File.ReadAllText(ArquivoNome);
            return JsonConvert.DeserializeObject<Controlador>(Json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diretorio_raiz"></param>
        /// <param name="quantidadePaineis"></param>
        /// <remarks> Cria os diretórios dos paineis + os arquivos de alternência Default</remarks>
        private void CriarDiretoriosPaineisNandFS(string diretorio_raiz, int quantidadePaineis)
        {
            for (int i = 0; i < quantidadePaineis; i++)
            {
                string diretorio_painel = @"\" + i.ToString("d2");

                Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_PAINEL + diretorio_painel);
                Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_PAINEL + diretorio_painel + Util.Util.diretorio_painel_msgs);
                Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_PAINEL + diretorio_painel + Util.Util.diretorio_painel_roteiros);
                Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_PAINEL + diretorio_painel + Util.Util.diretorio_painel_drivers);


                // Alteração da alternância JP, ao invés de criar alternância padrão, copiar a alternancia gerada pelo usuário para a pasta de cada painel
                if (!File.Exists(diretorio_raiz + Util.Util.DIRETORIO_PAINEL + diretorio_painel + Util.Util.ARQUIVO_ALT))
                    File.Copy(Fachada.diretorio_alt + Util.Util.ARQUIVO_ALT , diretorio_raiz + Util.Util.DIRETORIO_PAINEL + diretorio_painel + Util.Util.ARQUIVO_ALT);

                //Código Barata
                //Arquivo ALT
                //Arquivo_ALT aalt = new Arquivo_ALT();

                //aalt.ArquivoNome = diretorio_raiz + Util.Util.DIRETORIO_PAINEL + diretorio_painel + Util.Util.ARQUIVO_ALT;
                //aalt.CriarAlternanciasDefault();                              
            }
        }


        private void GerarMensagemEmergencia(ResourceManager rm, int indicePainel = 0)
        {      
            if (indicePainel == 0xff)
            {
                indicePainel = 0;
            }
            if (null == this.Paineis[indicePainel].MensagemEmergencia)
            {
                this.Paineis[indicePainel].MensagemEmergencia = new MensagemEmergencia(rm);

                //Colocado dentro do if, só deve setar o fontepath se não houver mensagem de emergência. Mas, ao criar o painel já é criada uma mensagem de emergência e a fonte deve vir do teado pelo usuário na GUI (JP)
                for (int i = 0; i < this.Paineis[indicePainel].MensagemEmergencia.Frases.Count; i++)
                {
                    this.Paineis[indicePainel].MensagemEmergencia.Frases[i].Modelo.Textos[0].Fonte = Path.GetFileNameWithoutExtension(this.Paineis[indicePainel].FontePath);
                }
            }

            this.Paineis[indicePainel].MensagemEmergencia.GerarPlaylist((uint)this.Paineis[indicePainel].Altura, (uint)this.Paineis[indicePainel].Largura, indicePainel);
        }

        private void CriarDiretoriosNandFS(string diretorio_raiz)
        {
            Directory.CreateDirectory(diretorio_raiz);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_FONTES);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_IDIOMAS);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_MSGS);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_PAINEL);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_REGIOES);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_VIDEOS);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_ROTEIROS);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_DRIVERS);
            Directory.CreateDirectory(diretorio_raiz + Util.Util.DIRETORIO_FIRMWARE);
        }

        public void GerarArquivoNandFSMainIsPontos(string arquivoDestino, bool envioNSS, BackgroundWorker background)
        {

            int progressoTransmissao = 0;
            Fachada fachada = Fachada.Instance;

            string diretorioOrigem = Fachada.diretorioOrigem;
            string diretorioRaizTemporario = Fachada.diretorio_temporario;
            string diretorio_rgn = Fachada.diretorio_rgn;
            string diretorio_lpk = Fachada.diretorio_lpk;
            string diretorio_fnt = Fachada.diretorio_fontes;
            string diretorio_NSS = Fachada.diretorio_NSS;
            string diretorioTemporario_NSS = diretorioRaizTemporario + Util.Util.DIRETORIO_NSS;
            string diretorio_fir = Fachada.diretorio_fir;

            try
            {
                if (this.CancelarEnvioNFX)
                    return;

                //Apagando a pasta temporária para evitar erros
                Rm = fachada.carregaIdioma();

                ApagarDiretorioTemp(diretorioRaizTemporario);

                this.Salvar(diretorioRaizTemporario + Util.Util.ARQUIVO_TEMP_LDX2);

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_CRIANDO_DIRETORIO_NANDFS");
                background.ReportProgress(progressoTransmissao);
                CriarDiretoriosNandFS(diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                progressoTransmissao += 5;
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_CRIANDO_DIRETORIO_PAINEIS");
                background.ReportProgress(progressoTransmissao);
                CriarDiretoriosPaineisNandFS(diretorioRaizTemporario, this.Paineis.Count);

                if (this.CancelarEnvioNFX)
                    return;

                progressoTransmissao += 5;
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_CRIANDO_MENSAGENS_EMERGENCIA");
                background.ReportProgress(progressoTransmissao);
                GerarMensagemEmergencia(Rm);

                if (this.CancelarEnvioNFX)
                    return;

                //// TODO: Lembrar de Unificar MSG com MPT.
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_CRIANDO_ARQUIVOS_REGIAO");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);
                CopiarArquivosRegiaoRGN(diretorio_rgn, diretorioRaizTemporario + Util.Util.DIRETORIO_REGIOES);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_COPIAR_ARQUIVOS_IDIOMA");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);
                CopiarArquivosPacotesIdiomasLPK(diretorio_lpk, diretorioRaizTemporario + Util.Util.DIRETORIO_IDIOMAS);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_COPIAR_ARQUIVOS_FONTE");
                progressoTransmissao += 10;
                background.ReportProgress(progressoTransmissao);
                CopiarArquivosFonte(diretorio_fnt, diretorioRaizTemporario + Util.Util.DIRETORIO_FONTES);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_COPIAR_ARQUIVOS_FIRMWARE");
                progressoTransmissao += 10;
                background.ReportProgress(progressoTransmissao);
                CopiarArquivosFirmware(diretorio_fir, diretorioRaizTemporario + Util.Util.DIRETORIO_FIRMWARE);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_PARAMETROS_FIXOS");
                progressoTransmissao += 10;
                background.ReportProgress(progressoTransmissao);
                this.ParametrosFixos.QtdPaineis = this.Paineis.Count;
                //this.ParametrosFixos.PainelAPP_NSS = (envioNSS||this.ParametrosFixos.PerifericosNaRede[(int)Util.Util.IndicePerifericosNaRede.APP]) ? this.ParametrosFixos.PainelAPP_NSS : 0xff;
                if (!(envioNSS || this.ParametrosFixos.PerifericosNaRede[(int)Util.Util.IndicePerifericosNaRede.APP]))
                    this.ParametrosFixos.PaineisAPP.Clear();

                this.ParametrosFixos.GerarArquivoFIX(diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_PARAMETROS_VARIAVEIS");
                progressoTransmissao += 10;
                background.ReportProgress(progressoTransmissao);
                this.ParametrosVariaveis.GerarArquivoVAR(diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_PAINEIS");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);

                for (int painel = 0; painel < this.Paineis.Count; painel++)
                {
                    this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_PAINEIS") + " #" + painel.ToString("00");
                    background.ReportProgress(progressoTransmissao);
                    this.Paineis[painel].Salvar(diretorioRaizTemporario, diretorio_fnt, ref this.CancelarEnvioNFX, this.ParametrosFixos.ModoApresentacaoDisplayLD6);
                    //zera os arquivos sequenciais para que a próxima vez reinicie a contagem para o nome dos arquivos.
                    Util.Util.LimpaSequenciais();

                    if (this.CancelarEnvioNFX)
                        return;
                }

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_EVENTOS");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);
                SalvarEventos(background, ref progressoTransmissao, diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                // Gerar arquivo Nand Flash File System
                Memorias.NandFFS m = new Memorias.NandFFS();
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_GERAR_NFS");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);
                m.GerarArquivoNandOtimizado(diretorioRaizTemporario, arquivoDestino);

                if (this.CancelarEnvioNFX)
                    return;

                ApagarDiretorioTemp(diretorioRaizTemporario);

                progressoTransmissao += 4;
                background.ReportProgress(progressoTransmissao);
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_TRANSFERENCIA_CONCLUIDA");
                background.ReportProgress(100);


                // Todo: Remover comentário do código de Verificação do arquivo NandFlash
                //m.VerificarNandFlashFileSystem(arquivoDestino, this.Rm);

                
            }
            catch (Exception ex)
            {
                this.StatusTransmissao = "ERROR: " + ex.Message;

                //Adicionado caso aconteca algum erro para apagar a pasta do diretorio temporario
                ApagarDiretorioTemp(diretorioRaizTemporario);

                background.ReportProgress(progressoTransmissao);
                throw new Exception(this.StatusTransmissao);


            }
        }
        public void GerarArquivoNandFSMainIsNSS(string arquivoDestino, bool envioNSS, BackgroundWorker background)
        {
            int progressoTransmissao = 0;
            Fachada fachada = Fachada.Instance;

            string diretorioOrigem = Fachada.diretorioOrigem;
            string diretorioRaizTemporario = Fachada.diretorio_temporario;
            string diretorio_rgn = Fachada.diretorio_rgn;
            string diretorio_lpk = Fachada.diretorio_lpk;
            string diretorio_fnt = Fachada.diretorio_fontes;
            string diretorio_NSS = Fachada.diretorio_NSS;
            string diretorioTemporario_NSS = diretorioRaizTemporario + Util.Util.DIRETORIO_NSS;
            string diretorio_fir = Fachada.diretorio_fir;

            try
            {
                if (this.CancelarEnvioNFX)
                    return;

                this.ParametrosFixos.QtdPaineis = this.Paineis.Count;

                //this.ParametrosFixos.PaineisAPP = (envioNSS) ? this.ParametrosFixos.PaineisAPP : 0xff;
                if (!envioNSS)
                    this.ParametrosFixos.PaineisAPP.Clear();
              

                //Apagando a pasta temporária para evitar erros
                ApagarDiretoriosTempLDX2(diretorioRaizTemporario);
                ApagarDiretoriosTempNSS(diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                this.Salvar(diretorioRaizTemporario + Util.Util.ARQUIVO_TEMP_LDX2);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_CRIANDO_DIRETORIO_NANDFS");
                background.ReportProgress(progressoTransmissao);
                CriarDiretoriosNandFS(diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                progressoTransmissao += 5;
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_CRIANDO_DIRETORIO_PAINEIS");
                background.ReportProgress(progressoTransmissao);
                CriarDiretoriosPaineisNandFS(diretorioRaizTemporario, this.Paineis.Count);

                if (this.CancelarEnvioNFX)
                    return;

                progressoTransmissao += 5;
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_CRIANDO_MENSAGENS_EMERGENCIA");
                background.ReportProgress(progressoTransmissao);
                //GerarMensagemEmergencia(Rm, this.ParametrosFixos.PainelNSS);

                //// TODO: Lembrar de Unificar MSG com MPT.
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_CRIANDO_ARQUIVOS_REGIAO");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);
                CopiarArquivosRegiaoRGN(diretorio_rgn, diretorioRaizTemporario + Util.Util.DIRETORIO_REGIOES);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_COPIAR_ARQUIVOS_IDIOMA");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);
                CopiarArquivosPacotesIdiomasLPK(diretorio_lpk, diretorioRaizTemporario + Util.Util.DIRETORIO_IDIOMAS);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_COPIAR_ARQUIVOS_FONTE");
                progressoTransmissao += 10;
                background.ReportProgress(progressoTransmissao);
                CopiarArquivosFonte(diretorio_fnt, diretorioRaizTemporario + Util.Util.DIRETORIO_FONTES);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_COPIAR_ARQUIVOS_FIRMWARE");
                progressoTransmissao += 10;
                background.ReportProgress(progressoTransmissao);
                CopiarArquivosFirmware(diretorio_fir, diretorioRaizTemporario + Util.Util.DIRETORIO_FIRMWARE);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_PARAMETROS_FIXOS");
                progressoTransmissao += 10;
                background.ReportProgress(progressoTransmissao);
                this.ParametrosFixos.GerarArquivoFIX(diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_PARAMETROS_VARIAVEIS");
                progressoTransmissao += 10;
                background.ReportProgress(progressoTransmissao);
                this.ParametrosVariaveis.GerarArquivoVAR(diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_PAINEIS");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);

                for (int painel = 0; painel < this.Paineis.Count; painel++)
                {                    
                    GerarMensagemEmergencia(Rm, painel);

                    this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_PAINEIS") + " #" + painel.ToString("00");
                    background.ReportProgress(progressoTransmissao);

                    this.Paineis[painel].Salvar(diretorioRaizTemporario, diretorio_fnt, ref this.CancelarEnvioNFX, this.ParametrosFixos.ModoApresentacaoDisplayLD6);
                    //zera os arquivos sequenciais para que a próxima vez reinicie a contagem para o nome dos arquivos.
                    Util.Util.LimpaSequenciais();

                    if (this.CancelarEnvioNFX)
                        return;
                }

                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_EVENTOS");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);
                SalvarEventos(background, ref progressoTransmissao, diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                // Gerar arquivo Nand Flash File System
                Memorias.NandFFS m = new Memorias.NandFFS();
               this.StatusTransmissao = Rm.GetString("CONTROLADOR_GERAR_NFS");
                progressoTransmissao += 5;
                background.ReportProgress(progressoTransmissao);
                m.GerarArquivoNandOtimizado(diretorioRaizTemporario, diretorioRaizTemporario + Util.Util.ARQUIVO_B12);

                if (this.CancelarEnvioNFX)
                    return;

                ApagarDiretoriosTempLDX2(diretorioRaizTemporario);

                /////////////////////////////////////////////////////////////////////////

                // 1 - VERIFICAR SE O DIRETÓRIO DO NSS EXISTE e 
                // 2 - VERIFICAR SE O USUÁRIO DESEJA ENVIAR OS ARQUIVOS NSS
                if ((Directory.Exists(diretorio_NSS)) && envioNSS)
                {
                    this.StatusTransmissao = Rm.GetString("CONTROLADOR_GERAR_NFS_APP");

                    this.StatusTransmissao = Rm.GetString("CONTROLADOR_COPIAR_ARQUIVOS_NSS");
                    progressoTransmissao += 10;
                    background.ReportProgress(progressoTransmissao);

                    if (Directory.Exists(diretorio_NSS))
                    {
                        CopiarArquivos(diretorio_NSS, diretorioTemporario_NSS, ".*");

                        if (this.CancelarEnvioNFX)
                            return;

                        string[] diretorios = Directory.GetDirectories(diretorio_NSS, "*.*", SearchOption.AllDirectories);

                        foreach (string origem in diretorios)
                        {
                            if (origem.Contains(@"\Rotas"))
                            {
                                bool isRoute = false;
                                for (int idRoteiro = 0; idRoteiro < this.Paineis[0].Roteiros.Count; idRoteiro++)
                                {
                                    if (Path.GetFileName(origem) == this.Paineis[0].Roteiros[idRoteiro].Numero.LabelFrase)
                                    {
                                        isRoute = true;
                                        break;
                                    }
                                }

                                if (isRoute)
                                {
                                    string destino = origem.Replace(diretorio_NSS, diretorioTemporario_NSS);
                                    CopiarTodosArquivos(origem, destino, ".*");
                                    isRoute = false;
                                }
                            }
                            else
                            {
                                string destino = origem.Replace(diretorio_NSS, diretorioTemporario_NSS);

                                CopiarArquivos(origem, destino, ".*");
                            }

                            if (this.CancelarEnvioNFX)
                                return;
                        }
                    }

                    if (this.CancelarEnvioNFX)
                        return;

                    if (m.isOld)
                    {
                        // Copiar o LD.B12 para o diretorio temporario do NSS
                        File.Copy(diretorioRaizTemporario + Util.Util.ARQUIVO_B12, diretorioTemporario_NSS + Util.Util.ARQUIVO_B12, true);
                    }
                    else
                    {
                        File.Copy(diretorioRaizTemporario + Util.Util.ARQUIVO_LDNFX, diretorioTemporario_NSS + Util.Util.ARQUIVO_LDNFX, true);
                    }

                    if (this.CancelarEnvioNFX)
                        return;

                    // 6 - GERAR NAND FLASH FILE SYSTEM
                    m.GerarArquivoNandOtimizado(diretorioTemporario_NSS, arquivoDestino);

                    if (this.CancelarEnvioNFX)
                        return;

                    ApagarDiretoriosTempNSS(diretorioRaizTemporario);
                }

                if (this.CancelarEnvioNFX)
                    return;

                ApagarDiretoriosTempNSS(diretorioRaizTemporario);

                if (this.CancelarEnvioNFX)
                    return;

                progressoTransmissao += 4;
                background.ReportProgress(progressoTransmissao);
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_TRANSFERENCIA_CONCLUIDA");
                background.ReportProgress(100);

            }
            catch (Exception ex)
            {
                this.StatusTransmissao = "ERROR: " + ex.Message;

                //Adicionado caso aconteca algum erro para apagar a pasta do diretorio temporario
                ApagarDiretoriosTempLDX2(diretorioRaizTemporario);
                ApagarDiretoriosTempNSS(diretorioRaizTemporario);

                background.ReportProgress(progressoTransmissao);
                throw new Exception(this.StatusTransmissao);
            }
        }

        private void CopiarArquivosNSS(String diretorio_raiz, String diretorio_destino, String extensao)
        {
            List<String> arquivos = new List<string>();

            // André -> 10-12-2014
            if (!Directory.Exists(diretorio_destino))
                Directory.CreateDirectory(diretorio_destino);

            arquivos.AddRange(Directory.EnumerateFiles(diretorio_raiz, "*" + extensao, SearchOption.AllDirectories));

            for (int idRoteiro = 0; idRoteiro < this.Paineis[0].Roteiros.Count; idRoteiro++)
            {               
                for (int i = 0; i < arquivos.Count; i++)
                {
                    if (!arquivos[i].Contains(@"\Rotas\"))
                    {
                        continue;
                    }
                    if (arquivos[i].Contains(@"\Rotas\" + this.Paineis[0].Roteiros[idRoteiro].Numero.LabelFrase))
                    {
                        arquivos.RemoveAt(i);
                    }
                }
            }

            foreach (string s in arquivos)
            {
                if (Path.GetFileNameWithoutExtension(s).Length > 8)
                {
                    File.Copy(s, diretorio_destino + @"\" + Path.GetFileNameWithoutExtension(s).Substring(0, 8) + Path.GetExtension(s), true);
                }
                else
                {
                    File.Copy(s, diretorio_destino + @"\" + Path.GetFileNameWithoutExtension(s) + Path.GetExtension(s), true);
                }
            }
        }

        private void ApagarDiretoriosTempLDX2(string diretorioRaizTemporario)
        {
            if (Directory.Exists(diretorioRaizTemporario))
            {
                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_VIDEOS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_VIDEOS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_REGIOES))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_REGIOES, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_ROTEIROS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_ROTEIROS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_DRIVERS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_DRIVERS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_PAINEL))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_PAINEL, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_MSGS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_MSGS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_IDIOMAS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_IDIOMAS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_FONTES))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_FONTES, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_FIRMWARE))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_FIRMWARE, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_NSS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_NSS, true);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_VAR))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_VAR);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_FIX))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_FIX);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_AGENDAMENTO))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_AGENDAMENTO);

                //Adicionado para excluir o arquivo do TEMP.LDX2 se tiver sido criado
                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_TEMP_LDX2))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_TEMP_LDX2);
            }

        }
        private void SalvarEventos(BackgroundWorker background, ref int progressoTransmissao, string diretorioRaizTemporario)
        {
            Arquivo_SCH arquivo = new Arquivo_SCH();
            List<Evento> lista = new List<Evento>();
            for (int painel = 0; painel < this.Paineis.Count; painel++)
            {
                this.StatusTransmissao = Rm.GetString("CONTROLADOR_SALVAR_EVENTOS") + " #" + painel.ToString("00");
                for (int indiceEvento = 0; indiceEvento < this.Paineis[painel].Eventos.Count; indiceEvento++)
                {
                    Arquivo_SCH.Agendamento evento = new Arquivo_SCH.Agendamento();
                    evento.ano = (UInt16)this.Paineis[painel].Eventos[indiceEvento].DataHora.Year;
                    evento.dia = (Byte)this.Paineis[painel].Eventos[indiceEvento].DataHora.Day;
                    evento.diaSemana = (Byte)this.Paineis[painel].Eventos[indiceEvento].DataHora.DayOfWeek;
                    evento.mes = (Byte)this.Paineis[painel].Eventos[indiceEvento].DataHora.Month;
                    evento.mascara = 0x04; // De acordo com Gustavo já é pra vir setado para Day of week; 
                    evento.horas = (Byte)this.Paineis[painel].Eventos[indiceEvento].DataHora.Hour;
                    evento.minutos = (Byte)this.Paineis[painel].Eventos[indiceEvento].DataHora.Minute;
                    evento.segundos = (Byte)this.Paineis[painel].Eventos[indiceEvento].DataHora.Second;
                    evento.operacao = (Byte)this.Paineis[painel].Eventos[indiceEvento].Operacao;
                    evento.painel = (byte)painel;
                    if (this.Paineis[painel].Eventos[indiceEvento].Operacao == Util.Util.TipoOperacaoEvento.ALTERACAO_HORA_SAIDA)
                    {
                        evento.valorParametro = (UInt16)(this.Paineis[painel].Eventos[indiceEvento].valorParametro2 << 8);
                        evento.valorParametro += (UInt16)(this.Paineis[painel].Eventos[indiceEvento].valorParametro);
                    }
                    else
                    {
                        evento.valorParametro = (UInt16)this.Paineis[painel].Eventos[indiceEvento].valorParametro;
                    }
                    // TODO: Fazer a validação do Evento ( Se já existe dentro da lista um evento que esteja em conflito. )
                    arquivo.itens.Add(evento);
                    //lista.Add(this.Paineis[painel].Eventos[indiceEvento].CompararDataHoraOperacao);
                }
                background.ReportProgress(progressoTransmissao);
            }
            arquivo.Salvar(diretorioRaizTemporario);
        }

        private void CopiarArquivosFirmware(String diretorio_raiz, String diretorio_destino)
        {
            CopiarArquivos(diretorio_raiz, diretorio_destino, Util.Util.ARQUIVO_EXT_FIR);
            CopiarArquivos(diretorio_raiz, diretorio_destino, Util.Util.ARQUIVO_EXT_OPT);
        }
        private void ApagarDiretorioTemp(string diretorioRaizTemporario)
        {
            if (Directory.Exists(diretorioRaizTemporario))
            {
                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_VIDEOS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_VIDEOS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_REGIOES))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_REGIOES, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_ROTEIROS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_ROTEIROS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_DRIVERS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_DRIVERS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_PAINEL))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_PAINEL, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_MSGS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_MSGS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_IDIOMAS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_IDIOMAS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_FONTES))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_FONTES, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_FIRMWARE))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_FIRMWARE, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_NSS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_NSS, true);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_VAR))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_VAR);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_FIX))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_FIX);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_AGENDAMENTO))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_AGENDAMENTO);

                //Adicionado para excluir o arquivo do nss.nfs se tiver sido criado
                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_NSS))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_NSS);

                //Adicionado para excluir o arquivo do nss.nfs se tiver sido criado
                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_NFX))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_NFX);

                //Adicionado para excluir o arquivo do TEMP.LDX2 se tiver sido criado
                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_TEMP_LDX2))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_TEMP_LDX2);
            }
        }
        private void ApagarDiretoriosTempNSS(string diretorioRaizTemporario)
        {
            if (Directory.Exists(diretorioRaizTemporario))
            {
                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_VIDEOS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_VIDEOS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_REGIOES))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_REGIOES, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_ROTEIROS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_ROTEIROS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_DRIVERS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_DRIVERS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_PAINEL))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_PAINEL, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_MSGS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_MSGS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_IDIOMAS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_IDIOMAS, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_FONTES))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_FONTES, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_FIRMWARE))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_FIRMWARE, true);

                if (Directory.Exists(diretorioRaizTemporario + Util.Util.DIRETORIO_NSS))
                    Directory.Delete(diretorioRaizTemporario + Util.Util.DIRETORIO_NSS, true);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_VAR))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_VAR);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_FIX))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_FIX);

                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_AGENDAMENTO))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_AGENDAMENTO);

                //Adicionado para excluir o arquivo do nss.nfs se tiver sido criado
                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_B12))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_B12);

                //Adicionado para excluir o arquivo do nss.nfs se tiver sido criado
                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_LDNFX))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_LDNFX);

                //Adicionado para excluir o arquivo do TEMP.LDX2 se tiver sido criado
                if (File.Exists(diretorioRaizTemporario + Util.Util.ARQUIVO_TEMP_LDX2))
                    File.Delete(diretorioRaizTemporario + Util.Util.ARQUIVO_TEMP_LDX2);

            }
        }

        private string RetornarArquivoFonteDefault(int altura, string diretorioFonte, string diretorioRaiz)
        {
            List<string> dir_fontes = Directory.EnumerateFiles(diretorioFonte, "*"+ Util.Util.ARQUIVO_EXT_FNT).ToList();
            
            for (int i = 0; i < dir_fontes.Count; i++ )
            {
                dir_fontes[i] = dir_fontes[i].Replace(diretorioRaiz, String.Empty);

                if (dir_fontes[i].Contains("\\\\"))
                {
                    dir_fontes[i] = dir_fontes[i].Replace("\\\\", "\\");
                }
            }
            // Pega a menor fonte
            foreach (string nome_fonte in dir_fontes)
            {
                Arquivo_FNT fonte = new Arquivo_FNT();

                fonte.Abrir(diretorioRaiz + nome_fonte);

                if (altura == fonte.Altura)
                {
                    string nomeFonteLocal = String.Empty;
                    
                    nomeFonteLocal = nome_fonte;
                    
                    return nomeFonteLocal;
                }

            }
            // Pega a menor fonte
            foreach (string nome_fonte in dir_fontes)
            {
                Arquivo_FNT fonte = new Arquivo_FNT();

                fonte.Abrir(diretorioRaiz + nome_fonte);

                if (altura >= fonte.Altura)
                {
                    string nomeFonteLocal = String.Empty;

                    nomeFonteLocal = nome_fonte;

                    return nomeFonteLocal;
                }
            }
            return null;
        }
        private void CopiarArquivosNSS(string diretorio_raiz, string diretorio_destino)
        {
            CopiarArquivos(diretorio_raiz, diretorio_destino, ".*"); // Traz todos os arquivos do NSS
        }

        internal void GerarDiretoriosFAT(string arquivoOrigem, string diretorioRaiz)
        {
            // Gerar arquivo Nand Flash File System
            Memorias.NandFFS m = new Memorias.NandFFS();
            m.GerarDiretoriosFAT(arquivoOrigem, diretorioRaiz);            
        }
        public void GerarArquivoLDX2(string arquivoOrigem, string diretorioRaiz)
        {
            // Gerar arquivo Nand Flash File System
            Memorias.NandFFS m = new Memorias.NandFFS();
            m.GerarDiretoriosFATOtimizado(arquivoOrigem, diretorioRaiz);
        }
        internal void GerarArquivoNandFS(string nomeArquivo, bool envioNSS, BackgroundWorker background)
        {
            GC.Collect();
            if (envioNSS)
            {
                GerarArquivoNandFSMainIsNSS(nomeArquivo, envioNSS, background);                
            }
            else
            {
                GerarArquivoNandFSMainIsPontos(nomeArquivo, envioNSS, background);
            }
        }
    }
}

