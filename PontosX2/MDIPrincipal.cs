using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controlador;
using System.Resources;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PontosX2
{
    public partial class MDIPrincipal : Form
    {
        private int childFormNumber = 0;
        Fachada fachada = Fachada.Instance;
        ResourceManager rm;

        BackgroundWorker backgroundSplash = new BackgroundWorker();
        FormSplashScreen frmSplashScreen = new FormSplashScreen();

        bool achouFonte = true;
        bool cancelarSplash = false;
        bool desativado = true;
        bool ninbusInstalado = false;
        string caminhoNinbus = String.Empty;

        int qtdItemsMenuWindow;

        string arquivoAbrir = null;
        string[] listaArquivosRecentes;

        //escuta mensagem enviada pelo APP para atualizar a lista de roteiros
        protected override void WndProc(ref Message message)
        {
            //filtro para mensagem do APP atualizar a lista de roteiros
            if (message.Msg == Util.Util.RF_ATUALIZARROTA)
            {
                for (int i = 0; i < this.MdiChildren.Length; i++)
                {
                    if (this.MdiChildren[i] is Arquivo)
                        (this.MdiChildren[i] as Arquivo).AtualizarRotasAPP();
                }
            }

            //be sure to pass along all messages to the base also
            base.WndProc(ref message);
        }

        public MDIPrincipal(String[] args)
        {
            InitializeComponent();

            //Dar permissão comum na pasta de programaData
            fachada.GrantAccess();

            //Verificando versão do software do usuário
            if (!fachada.ExisteArquivoVersao())
            {
                //fachada.ApagarLDX2ProgramData();
                fachada.SetarVersaoUsuario(Application.ProductVersion);
            }
            else
            {
                string vAplicacao = Application.ProductVersion;
                string vUsuario = fachada.GetVersaoUsuario();

                var versionAplicacao = new Version(vAplicacao);
                var versionUsuario = new Version(vUsuario);

                var result = versionAplicacao.CompareTo(versionUsuario);

                //Versão do usuário menor que a versao do software apaga a pasta programData\LDX2, depois cria a pasta com a versão do software
                if (result > 0)
                {
                    //fachada.ApagarLDX2ProgramData();
                    fachada.SetarVersaoUsuario(Application.ProductVersion);
                }
            }

            //Cria todas as pastas que serão utilizadas pelo LDX2 em programData e atualiza os arquivos se necessário
            fachada.CriaDirs();

            ////Aplica o idioma na GUI do sistema
            rm = fachada.carregaIdioma();
            AplicaIdioma();

            //adicionado para inicar a aplicação clicando no arquivo .ldx2
            if (args.Length > 0)
                arquivoAbrir = args[0];
            else
                arquivoAbrir = null;

            //quantidade de itens do menu window para remover o separador colocado automaticamente quando o usuário cria um novo controlador
            qtdItemsMenuWindow = windowsMenu.DropDownItems.Count;
            

        }

        private void backgroundSplash_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Stopwatch s = new Stopwatch();
                int tempoDefault = 400;

                frmSplashScreen.Show();

                #region Splash Msg de Inicialização

                s.Start();
                frmSplashScreen.SetStatus(rm.GetString("SPLASH_MENSAGEM_INICIANDO"));
                Application.DoEvents();
                if (s.ElapsedMilliseconds < tempoDefault)
                    Thread.Sleep(tempoDefault - (int)s.ElapsedMilliseconds);

                //se o usuário clicou no fechar do splash
                cancelarSplash = frmSplashScreen.fecharSplashUsuario;
                if (cancelarSplash)
                    return;

                #endregion

                #region Verificando Chave de Ativação

                s.Start();
                frmSplashScreen.SetStatus(rm.GetString("SPLASH_MENSAGEM_ARQUIVO_ATIVACAO"));
                Application.DoEvents();
                fachada.SetLicenciadoPara(Application.CompanyName);
                desativado = !fachada.VerificarArquivoAtivacao();
                if (s.ElapsedMilliseconds < tempoDefault)
                    Thread.Sleep(tempoDefault - (int)s.ElapsedMilliseconds);

                //se o usuário clicou no fechar do splash
                cancelarSplash = frmSplashScreen.fecharSplashUsuario;
                if (cancelarSplash)
                    return;

                #endregion

                #region Splash Carregando arquivos recentes

                s.Restart();
                frmSplashScreen.SetStatus(rm.GetString("SPLASH_MENSAGEM_ARQUIVOS_RECENTES"));
                Application.DoEvents();
                listaArquivosRecentes = Util.Util.GetArquivosRecentes();
                if (s.ElapsedMilliseconds < tempoDefault)
                    Thread.Sleep(tempoDefault - (int)s.ElapsedMilliseconds);

                //se o usuário clicou no fechar do splash
                cancelarSplash = frmSplashScreen.fecharSplashUsuario;
                if (cancelarSplash)
                    return;

                #endregion

                #region Splash Abrindo Arquivo do Usuário

                if (arquivoAbrir != null)
                {
                    s.Restart();
                    frmSplashScreen.SetStatus(rm.GetString("SPLASH_MENSAGEM_ABRIR_ARQUIVO"));
                    Application.DoEvents();
                    achouFonte = frmSplashScreen.AbrirArquivo(arquivoAbrir);
                    if (s.ElapsedMilliseconds < tempoDefault)
                        Thread.Sleep(tempoDefault - (int)s.ElapsedMilliseconds);

                    //se o usuário clicou no fechar do splash
                    cancelarSplash = frmSplashScreen.fecharSplashUsuario;
                    if (cancelarSplash)
                        return;
                }

                #endregion

                #region Splash Procurando Ninbus

                s.Restart();
                frmSplashScreen.SetStatus(rm.GetString("SPLASH_MENSAGEM_NINBUS"));
                Application.DoEvents();
                ninbusInstalado = Util.Util.ExisteNinbusInstalado(out caminhoNinbus);
                if (s.ElapsedMilliseconds < tempoDefault)
                    Thread.Sleep(tempoDefault - (int)s.ElapsedMilliseconds);

                //se o usuário clicou no fechar do splash
                cancelarSplash = frmSplashScreen.fecharSplashUsuario;
                if (cancelarSplash)
                    return;

                #endregion

                #region Splash Procurando APP

                s.Restart();
                frmSplashScreen.SetStatus(rm.GetString("SPLASH_MENSAGEM_APP"));
                Application.DoEvents();
                fachada.appInstalado = Util.Util.ExisteAPPInstalado(out fachada.caminhoApp);
                if (s.ElapsedMilliseconds < tempoDefault)
                    Thread.Sleep(tempoDefault - (int)s.ElapsedMilliseconds);

                //se o usuário clicou no fechar do splash
                cancelarSplash = frmSplashScreen.fecharSplashUsuario;
                if (cancelarSplash)
                    return;

                #endregion

                #region Splash Finalizando

                s.Restart();
                frmSplashScreen.SetStatus(rm.GetString("SPLASH_MENSAGEM_FINALIZANDO"));
                Application.DoEvents();
                if (s.ElapsedMilliseconds < tempoDefault)
                    Thread.Sleep(tempoDefault - (int)s.ElapsedMilliseconds);

                //se o usuário clicou no fechar do splash
                cancelarSplash = frmSplashScreen.fecharSplashUsuario;
                if (cancelarSplash)
                    return;

                #endregion

                frmSplashScreen.Close();                

            }
            catch
            {
                frmSplashScreen.fecharSplashUsuario = true;
                cancelarSplash = true;
            }
        }

        /// <summary>
        /// Aplica o idioma como configurado na variável de idioma na fachada.
        /// </summary>
        private void AplicaIdioma()
        {
            //Nome do Form
            Version versionAplicacao = new Version(Application.ProductVersion);

            this.Text = Application.ProductName + " - " + versionAplicacao.ToString(3);

            //menu 
            this.fileMenu.Text = rm.GetString("MAIN_MENU_FILE");
            this.viewMenu.Text = rm.GetString("MAIN_MENU_VIEW");
            this.toolsMenu.Text = rm.GetString("MAIN_MENU_TOOLS");
            this.windowsMenu.Text = rm.GetString("MAIN_MENU_WINDOWS");
            this.helpMenu.Text = rm.GetString("MAIN_MENU_HELP");

            //menu arquivo
            this.newToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_NEW");
            this.openToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_OPEN");
            this.saveToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_SAVE");
            this.saveAsToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_SAVE_AS");
            this.printToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_PRINT");
            this.printPreviewToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_PRINT_PREVIEW");
            this.printSetupToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_PRINT_SETUP");
            this.exitToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_EXIT");
            this.recentFilesToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_RECENT");

            //menu view
            this.toolBarToolStripMenuItem.Text = rm.GetString("MAIN_MENU_VIEW_TOOLBAR");


            //mennu tools
            this.sendNinbustoolStripMenuItem.Text = rm.GetString("MAIN_MENU_SEND_NINBUS");
            this.importImageToolStripMenuItem.Text = rm.GetString("MAIN_MENU_FILE_IMPORT_IMAGE");
            this.importFontMenuItem.Text = rm.GetString("MAIN_MENU_FILE_IMPORT_FONT");
            this.setFareToolStripMenuItem.Text = rm.GetString("MAIN_MENU_TOOLS_FARE");
            this.newRegionToolStripMenuItem.Text = rm.GetString("MAIN_MENU_TOOLS_REGION");
            this.presentationSequenceToolStripMenuItem.Text = rm.GetString("MAIN_MENU_TOOLS_PRESENTATION_SEQUENCE");
            this.FontEditortoolStripMenuItem.Text = rm.GetString("MAIN_MENU_TOOLS_FONT_EDITOR");
            this.languageToolStripMenuItem.Text = rm.GetString("MAIN_MENU_TOOLS_LANGUAGE");
            this.atualizarFirmwaresToolStripMenuItem.Text = rm.GetString("MAIN_MENU_HELP_VERIFICAR_ATUALIZACAO");

            //hint dos botoes
            this.newToolStripButton.ToolTipText = rm.GetString("MAIN_MENU_BUTTON_NEW");
            this.openToolStripButton.ToolTipText = rm.GetString("MAIN_MENU_BUTTON_OPEN");
            this.saveToolStripButton.ToolTipText = rm.GetString("MAIN_MENU_BUTTON_SAVE");
            this.printToolStripButton.ToolTipText = rm.GetString("MAIN_MENU_BUTTON_PRINT");
            this.printPreviewToolStripButton.ToolTipText = rm.GetString("MAIN_MENU_BUTTON_PRINT_PREVIEW");
            this.toolStripBtFind.ToolTipText = rm.GetString("MAIN_MENU_BUTTON_FIND");
            this.NinbustoolStripButton.ToolTipText = rm.GetString("MAIN_MENU_SEND_NINBUS");
            this.helpToolStripButton.ToolTipText = rm.GetString("MAIN_MENU_BUTTON_HELP");


            //menu window
            this.newWindowToolStripMenuItem.Text = rm.GetString("MAIN_MENU_WINDOWS_NEW_WINDOW");
            this.cascadeToolStripMenuItem.Text = rm.GetString("MAIN_MENU_WINDOWS_CASCADE");
            this.tileHorizontalToolStripMenuItem.Text = rm.GetString("MAIN_MENU_WINDOWS_TILE_HORIZONTAL");
            this.tileVerticalToolStripMenuItem.Text = rm.GetString("MAIN_MENU_WINDOWS_TILE_VERTICAL");
            this.closeAllToolStripMenuItem.Text = rm.GetString("MAIN_MENU_WINDOWS_CLOSE_ALL");
            this.arrangeIconsToolStripMenuItem.Text = rm.GetString("MAIN_MENU_WINDOWS_ARRANGE_ALL");

            //menu help
            this.contentsToolStripMenuItem.Text = rm.GetString("MAIN_MENU_HELP_CONTENTS");
            this.releaseToolStripMenuItem.Text = rm.GetString("MAIN_MENU_HELP_REALEASE_NOTES");
            this.searchToolStripMenuItem.Text = rm.GetString("MAIN_MENU_HELP_SEARCH");
            this.aboutToolStripMenuItem.Text = rm.GetString("MAIN_MENU_HELP_ABOUT");
            this.extrairLDX2DeNFXToolStripMenuItem.Text = rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2");
            this.extrairNFXToolStripMenuItem.Text = rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX");
            this.geradorDeChavesToolStripMenuItem.Text = rm.GetString("MAIN_MENU_HELP_GERAR_CHAVE");


            englishToolStripMenuItem.Checked = (fachada.GetIdioma() == Util.Util.Lingua.Ingles) ? true : false;
            portuguêsToolStripMenuItem.Checked = (fachada.GetIdioma() == Util.Util.Lingua.Portugues) ? true : false;
            españolToolStripMenuItem.Checked = (fachada.GetIdioma() == Util.Util.Lingua.Espanhol) ? true : false;

        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            try
            {
                CursorWait();

                this.ExibeNewForm(true, rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_LABEL_CAMINHO_DESCRICAO"));

                CursorDefault();
            }
            catch
            {
                CursorDefault();
            }
        }


        private void ExibeNewForm(Boolean criarNovoArquivo, String nomeArquivo)
        {
            Form childForm = new Arquivo(criarNovoArquivo, nomeArquivo);

            childForm.MdiParent = this;

            childForm.WindowState = FormWindowState.Normal;

            childForm.Show();

        }

        private void CursorWait()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.UseWaitCursor = true;
        }

        private void CursorDefault()
        {
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                openFileDialog.Filter = rm.GetString("FILTRO_ABRIR");
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (Path.GetExtension(openFileDialog.FileName).Equals(Util.Util.ARQUIVO_EXT_LDX))
                    {
                        MessageBox.Show(this, rm.GetString("AVISO_IMPORTACAO_LDX"), String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    string FileName = openFileDialog.FileName;

                    CursorWait();

                    fachada.Abrir(FileName); //adiciona o novo controlador à fachada.

                    if (fachada.QuantidadeControladores() > 0)
                    {
                        //Checa todas as fontes do controlador
                        if (!fachada.ChecarFontesControlador(fachada.QuantidadeControladores() - 1))
                            MessageBox.Show(rm.GetString("ARQUIVO_MBOX_FONTE_NAO_ENCONTRADA"));

                        this.ExibeNewForm(false, FileName); //ao ser exibido, Arquivo já conhece o último controlador adicionado à fachada, abrindo, assim, o último arquivo aberto.
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("MAIN_MENU_MBOX_OPEN_ERROR"), rm.GetString("MAIN_MENU_MBOX_OPEN_ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //this.ExibeNewForm(true, FileName); // Deu erro ao abrir o arquivo
                    }

                    CursorDefault();
                }
            }
            catch
            {
                CursorDefault();
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.MdiChildren.Count() > 0)
                {
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    saveFileDialog.Filter = rm.GetString("FILTRO_SALVAR");

                    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        CursorWait();

                        string FileName = saveFileDialog.FileName;
                        if (((Arquivo)(this.ActiveMdiChild)).SalvarControlador(FileName))
                            ((Arquivo)(this.ActiveMdiChild)).SetarArquivoControlador(FileName);


                        CursorDefault();
                    }

                }
                else
                    MessageBox.Show(rm.GetString("MAIN_MENU_MBOX_JANELA_ATIVA"));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                CursorDefault();
            }
        }

        void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {

            string nomeArquivo = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
            //if (!Regex.IsMatch(nomeArquivo, @"^[A-Za-z0-9 _]+$"))
            //{
            //    MessageBox.Show(rm.GetString("MAIN_MENU_FILE_SAVE_MBOX_CARACTERES_ESPECIAS"));
            //    e.Cancel = true;
            //}
            //else
            //{
                if (nomeArquivo.Length > 100)
                {
                    MessageBox.Show(rm.GetString("MAIN_MENU_FILE_SAVE_MBOX_TAMANHO_NOME"));
                    e.Cancel = true;
                }
            //}
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(rm.GetString("MAIN_MENU_WINDOWS_CLOSE_ALL_MBOX"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            {
                foreach (Form childForm in MdiChildren)
                {
                    childForm.Close();
                }
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
                saveFile();
            else
                MessageBox.Show(rm.GetString("MAIN_MENU_MBOX_JANELA_ATIVA"));
        }

        //Salva um 'controlador' aberto, sendo editado em alguma das janelas MDI;
        private void SalvarComo(String ArquivoNome)
        {
            try
            {
                CursorWait();

                //se vier de um arquivo .ldx aberto
                if (Path.GetExtension(ArquivoNome).Equals(Util.Util.ARQUIVO_EXT_LDX) || Path.GetExtension(ArquivoNome).Equals(Util.Util.ARQUIVO_EXT_LDX.ToUpper()))
                    ArquivoNome += "2";

                if (((Arquivo)this.ActiveMdiChild).SalvarControlador(ArquivoNome))
                    ((Arquivo)(this.ActiveMdiChild)).SetarArquivoControlador(ArquivoNome);

                CursorDefault();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                CursorDefault();
            }

        }


        /// <summary>
        /// Recalcula os índices para as janelas MDI de acordo com a lista contida na fachada.
        /// </summary>
        /// <param name="indice">índice do último controlador removido.</param>
        public void SincronizaControladores(int indice)
        {
            //supondo que esta quantidade já esteja previamente atualizada.
            int qtdControladores = fachada.QuantidadeControladores();
            int indice_mdiChildren = indice;


            for (int controlador = indice; controlador < qtdControladores; controlador++)
            {
                if (this.MdiChildren[indice_mdiChildren] is Arquivo)
                {
                    if (controlador == indice) indice_mdiChildren = indice_mdiChildren + 1;

                    ((Arquivo)this.MdiChildren[indice_mdiChildren]).SetarControladoSelecionado(controlador);
                }

                indice_mdiChildren = indice_mdiChildren + 1;
            }
        }

        public void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
                saveFile();
            else
                MessageBox.Show(rm.GetString("MAIN_MENU_MBOX_JANELA_ATIVA"));
        }

        public bool saveFile()
        {
            try
            {
                bool saved = false;
                if (this.MdiChildren.Count() > 0)
                    if (((Arquivo)this.ActiveMdiChild).NomeArquivo != rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_LABEL_CAMINHO_DESCRICAO"))
                    {
                        this.SalvarComo(((Arquivo)this.ActiveMdiChild).NomeArquivo);
                        saved = true;
                    }
                    else
                    {
                        saveFileDialog.Filter = rm.GetString("FILTRO_SALVAR");
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            this.SalvarComo(saveFileDialog.FileName);
                            saved = true;
                        }
                        else
                            saved = false;
                    }

                return saved;
            }
            catch
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_FILE_SAVE_MBOX_ERRO"), rm.GetString("MAIN_MENU_BUTTON_SAVE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void windowsMenu_DropDownOpening(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                Form activeChild = this.ActiveMdiChild;

                ActivateMdiChild(activeChild);
            }
            else
            {
                //apagando o separador que é adicionado automaticamente quando existe uma janela ativa, porém ao fechar a janela ativa o separador não era removido.
                for (int i = qtdItemsMenuWindow; i < windowsMenu.DropDownItems.Count; i++)
                    windowsMenu.DropDownItems[i].Visible = false;
            }

        }

        private void MDIPrincipal_Load(object sender, EventArgs e)
        {
            bool erroAbrindoArquivo = false;
            this.Visible = false;

            //backgroundworker que inicializa a tela de Splash
            backgroundSplash.WorkerSupportsCancellation = true;
            backgroundSplash.WorkerReportsProgress = true;
            backgroundSplash.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundSplash_DoWork);
            //backgroundSplash.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundSplash_RunWorkerCompleted);
            backgroundSplash.RunWorkerAsync();

            //espera até o backgroundwork terminar
            while (backgroundSplash.IsBusy)
            {
                Thread.Sleep(100);
                Application.DoEvents();
            }

            //se o usuário fechar a tela de splash fecha a aplicação
            if (cancelarSplash)
            {
                Application.Exit();
                return;
            }

            //verificando se a chave de ativação é válida
            if (desativado)
            {
                FormAtivacao form = new FormAtivacao();
                if (form.ShowDialog(this) == DialogResult.Cancel)
                {
                    Application.Exit();
                    return;
                }
            }

            //carrega no menu os arquivos recentes que foram carregados no splash
            MontarMenuArquivosRecentes();

            //carrega o menu para envio do arquivo nfx pelo nimbus e o botão de ferramentas
            MontarMenuNinbus();

            //Abrir arquivo se o usuário clicou em um arquivo .ldx2. O arquivo foi carregado no controlador pelo background worker na tela de splash
            if (arquivoAbrir != null)
            {
                if (fachada.QuantidadeControladores() > 0)
                    this.ExibeNewForm(false, arquivoAbrir); //ao ser exibido, Arquivo já conhece o último controlador adicionado à fachada, abrindo, assim, o último arquivo aberto.
                else
                {
                    MessageBox.Show(rm.GetString("MAIN_MENU_MBOX_OPEN_ERROR"), rm.GetString("MAIN_MENU_MBOX_OPEN_ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    erroAbrindoArquivo = true;
                    //this.ExibeNewForm(true, arquivoAbrir); // Deu erro ao abrir o arquivo
                }

            }

            this.WindowState = FormWindowState.Maximized;
            this.Visible = true;
            this.Activate();

            if (!achouFonte && !erroAbrindoArquivo)
                MessageBox.Show(rm.GetString("ARQUIVO_MBOX_FONTE_NAO_ENCONTRADA"));


            //habilitando os menus de geração de chaves, extração e recuperação do NFX
            bool exibir = fachada.LiberarPOS();
            extrairLDX2DeNFXToolStripMenuItem.Visible = exibir;
            extrairNFXToolStripMenuItem.Visible = exibir;
            geradorDeChavesToolStripMenuItem.Visible = exibir;
        }

        private void MontarMenuNinbus()
        {
            if (ninbusInstalado)
            {
                NinbustoolStripSeparator.Visible = true;
                NinbustoolStripButton.Visible = true;
                sendNinbustoolStripMenuItem.Visible = true;
                separadorNinbus.Visible = true;
            }
            else
            {
                NinbustoolStripSeparator.Visible = false;
                NinbustoolStripButton.Visible = false;
                sendNinbustoolStripMenuItem.Visible = false;
                separadorNinbus.Visible = false;
            }
        }

        private void MontarMenuArquivosRecentes()
        {
            try
            {
                recentFilesToolStripMenuItem.DropDownItems.Clear();

                foreach (String arquivo in listaArquivosRecentes)
                {
                    if (System.IO.File.Exists(arquivo))
                    {
                        recentFilesToolStripMenuItem.DropDownItems.Add(arquivo);
                        recentFilesToolStripMenuItem.DropDownItems[recentFilesToolStripMenuItem.DropDownItems.Count - 1].Click += OnClick;
                    }
                }
                if (listaArquivosRecentes.Count() == 0)
                {
                    toolStripSeparator5.Visible = false;
                    recentFilesToolStripMenuItem.Visible = false;
                }

            }
            catch
            {
                recentFilesToolStripMenuItem.Visible = false;
                toolStripSeparator5.Visible = false;
            }
        }

        private void OnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                String FileName = ((sender as ToolStripMenuItem).Text);

                if (!File.Exists(FileName))
                {
                    MessageBox.Show(rm.GetString("MAIN_MENU_RECENT_FILE_MBOX"), rm.GetString("MAIN_MENU_BUTTON_OPEN"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CursorWait();

                fachada.Abrir(FileName); //adiciona o novo controlador à fachada.

                if (fachada.QuantidadeControladores() > 0)
                {
                    //Checa todas as fontes do controlador
                    if (!fachada.ChecarFontesControlador(fachada.QuantidadeControladores() - 1))
                        MessageBox.Show(rm.GetString("ARQUIVO_MBOX_FONTE_NAO_ENCONTRADA"));

                    this.ExibeNewForm(false, FileName); //ao ser exibido, Arquivo já conhece o último controlador adicionado à fachada, abrindo, assim, o último arquivo aberto.
                }
                else
                {
                    //TODO: Exibir mensagem ao usuário informando que deu problema na abertura do arquivo e foi aberto um novo controlador
                    this.ExibeNewForm(true, FileName); // Deu erro ao abrir o arquivo
                }

                CursorDefault();
            }
            catch
            {
                CursorDefault();
            }
        }

        private void setFareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                if (((Arquivo)(this.ActiveMdiChild)).PodeEditarTarifas())
                {
                    FormChangeFare fcf = new FormChangeFare();
                    if (fcf.ShowDialog() == DialogResult.OK)
                    {
                        if (fcf.TodasTarifas)
                        {
                            ((Arquivo)(this.ActiveMdiChild)).MudarTarifaTodosPaineis(fcf.Tarifa);
                            MessageBox.Show(rm.GetString("MAIN_MENU_TOOLS_FARE_MBOX_TODAS_TARIFAS_ALTERADAS"));
                        }
                        else
                        {
                            int qtdAlterados = ((Arquivo)(this.ActiveMdiChild)).MudarTarifaDePara(fcf.TarifaDe, fcf.TarifaPara);
                            MessageBox.Show(rm.GetString("MAIN_MENU_TOOLS_FARE_MBOX_QTD_ROTEIROS") + "[" + qtdAlterados.ToString() + "]");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("MAIN_MENU_TOOLS_FARE_MBOX_EDITANDO_ROTEIRO"));
                }
            }
            else
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_MBOX_JANELA_ATIVA"));
            }

        }
    
        private void newRegionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_TOOLS_REGION_MBOX"));
            }
            else
            {
                FormLanguagesAndRegions flr = new FormLanguagesAndRegions();

                flr.ShowDialog();
            }
        }

        private void presentationSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_TOOLS_PRESENTATION_SEQUENCE_MBOX"));
            }
            else 
            { 
                FormPresentationSequence fps = new FormPresentationSequence();
                fps.ShowDialog();
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {          
            if (this.MdiChildren.Count() > 0)
            {
                switch (((Arquivo)(this.ActiveMdiChild)).GetTabControlSelecionado())
                {
                    case 1:
                        if (MessageBox.Show(rm.GetString("MAIN_MENU_FILE_PRINT_MBOX_TEXTOS_ROTEIRO"), rm.GetString("MAIN_MENU_FILE_PRINT_MBOX_TEXTOS_TITULO_ROTEIRO"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            ((Arquivo)(this.ActiveMdiChild)).ImprimirRoteiros(true); 
                        else
                            ((Arquivo)(this.ActiveMdiChild)).ImprimirRoteiros(false);
                        break;
                    case 2:
                        if (MessageBox.Show(rm.GetString("MAIN_MENU_FILE_PRINT_MBOX_TEXTOS_MENSAGEM"), rm.GetString("MAIN_MENU_FILE_PRINT_MBOX_TEXTOS_TITULO_MENSAGEM"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            ((Arquivo)(this.ActiveMdiChild)).ImprimirMensagens(true);
                        else
                            ((Arquivo)(this.ActiveMdiChild)).ImprimirMensagens(false);
                        break;
                    case 3:
                        ((Arquivo)(this.ActiveMdiChild)).ImprimirMotoristas();
                        break;
                    default:
                        MessageBox.Show(rm.GetString("MAIN_MENU_FILE_PRINT_MBOX_ABA"));
                        break;
                }
            }
            else
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_MBOX_JANELA_ATIVA"));
            }

        }

        private void importImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_TOOLS_IMPORT_IMAGE_MBOX"));
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                openFileDialog.Filter = Util.Util.FILTRO_ARQUIVO_IMAGEM;
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string FileName = openFileDialog.FileName;
                    if (File.Exists(FileName))
                    {
                        bool copiarImagem = true;
                        if (File.Exists(Fachada.diretorio_imagens + Path.GetFileName(FileName)))
                            if (MessageBox.Show(this, rm.GetString("MAIN_MENU_FILE_IMPORT_IMAGE_SOBRESCREVER"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo) == DialogResult.No)
                                copiarImagem = false;

                        if (copiarImagem)
                        {
                            if (!Directory.Exists(Fachada.diretorio_imagens))
                            {
                                Directory.CreateDirectory(Fachada.diretorio_imagens);
                            }


                            File.Copy(FileName, Fachada.diretorio_imagens + Path.GetFileName(FileName), true);
                            MessageBox.Show(this, rm.GetString("MAIN_MENU_FILE_IMPORT_IMAGE_COPIADA"));
                        }
                    }
                }
            }
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            FormSobre fs = new FormSobre();
            fs.ShowDialog();
        }

        private void SetarIdioma(Util.Util.Lingua idioma)        
        {
            fachada = Fachada.Instance;

            englishToolStripMenuItem.Checked = (idioma == Util.Util.Lingua.Ingles) ? true : false;
            portuguêsToolStripMenuItem.Checked = (idioma == Util.Util.Lingua.Portugues) ? true : false;
            españolToolStripMenuItem.Checked = (idioma == Util.Util.Lingua.Espanhol) ? true : false;

            fachada.SetarIdioma(idioma);
            this.rm = fachada.carregaIdioma();
            this.AplicaIdioma();
            this.Refresh();

            //setando o arquivo de alternancia altern.alt com a nova lingua
            fachada.AtualizarLinguaArquivoAlternancia(idioma);

            //Atualizar todas as alternancias dos paineis dos controladores para a nova lingua
            fachada.AtualizarIdiomaAlternanciaPadraoPainelTodosControladores();

            for (int i = 0; i < this.MdiChildren.Length; i++)
            {
                if (this.MdiChildren[i] is Arquivo)
                {
                    (this.MdiChildren[i] as Arquivo).rm = this.rm;
                    (this.MdiChildren[i] as Arquivo).AplicaIdioma(true);

                }
                this.MdiChildren[i].Refresh();
            }
        }
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetarIdioma(Util.Util.Lingua.Ingles);
        }

        private void portuguêsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetarIdioma(Util.Util.Lingua.Portugues);
        }

        private void españolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetarIdioma(Util.Util.Lingua.Espanhol);
        }



        private void FontEditortoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_TOOLS_FONT_EDITOR_MBOX"));
            }
            else
            {
                FormFontEditor ffe = new FormFontEditor();
                ffe.ShowDialog();
            }
        }

        private void MDIPrincipal_Shown(object sender, EventArgs e)
        {
            //rm = fachada.carregaIdioma();
            //AplicaIdioma();
        }

        public int Localizar(bool isRoteiro, int startIndex, string texto, bool matchWholeWord, bool ignoreCase, bool findTexts)
        {
            int indice = - 1;
            if (isRoteiro)
                indice = ((Arquivo)(this.ActiveMdiChild)).LocalizarRoteiro(startIndex, texto, matchWholeWord, ignoreCase, findTexts);
            else
                indice = ((Arquivo)(this.ActiveMdiChild)).LocalizarMensagem(startIndex, texto, matchWholeWord, ignoreCase, findTexts);

            return indice;

        }

        public int QuantidadeRoteiros()
        {
            return ((Arquivo)(this.ActiveMdiChild)).QuantidadeRoteiros();
        }


        public int QuantidadeMensagens()
        {
            return ((Arquivo)(this.ActiveMdiChild)).QuantidadeMensagens();
        }

        private void toolStripBtFind_Click(object sender, EventArgs e)
        {
            
            if (this.MdiChildren.Count() > 0)
            {
                switch (((Arquivo)(this.ActiveMdiChild)).GetTabControlSelecionado())
                {
                    case 1:
                        {
                            FormSearchRoute fsr = new FormSearchRoute();
                            fsr.isRoteiro = true;
                            fsr.ShowDialog(this);
                        }
                        break;
                    case 2:
                        {
                            FormSearchRoute fsr = new FormSearchRoute();
                            fsr.isRoteiro = false;
                            fsr.ShowDialog(this);
                        }
                        break;
                    default:
                        MessageBox.Show(rm.GetString("MAIN_MENU_FILE_FIND_MBOX_ABA"));
                        break;
                }
            }
            else
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_MBOX_JANELA_ATIVA"));
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSobre fs = new FormSobre();
            fs.ShowDialog();
        }

        private void MDIPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (this.MdiChildren.Length > 0)
                {

                    if (this.MdiChildren.Length == 1)
                    {
                        var result = MessageBox.Show(this, rm.GetString("ARQUIVO_MBOX_SALVAR_AO_FECHAR"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            if (!saveFile())
                                e.Cancel = true;
                        }
                    }
                    else
                    { 
                        var result = MessageBox.Show(this, rm.GetString("MDI_PRINCIPAL_MBOX_FECHAR"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo);
                        if (result != DialogResult.Yes)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }
        
        private void geradorDeChavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAtivacao form = new FormAtivacao(true);
            form.ShowDialog(this);
        }

        private void extrairLDX2DeNFXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_MBOX_MSG"), rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_MBOX_TITLE"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information)== DialogResult.OK)
            {
                // File OPEN
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                openFileDialog.Filter = rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_FILTRO");
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string ArquivoOrigem = openFileDialog.FileName;
                    string DiretorioDestino = String.Empty;

                    
                    // Destino
                    if (MessageBox.Show(this, rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_DIRETORIO"), rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_MBOX_TITLE"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        if (fbd.ShowDialog(this) == DialogResult.OK)
                        {
                            try
                            {
                                DiretorioDestino = fbd.SelectedPath;
                                fachada.ExtrairArquivoLDX2(ArquivoOrigem, DiretorioDestino + Util.Util.DIRETORIO_TEMPORARIO);
                                bool copiar = true;

                                if (File.Exists(DiretorioDestino + Util.Util.ARQUIVO_TEMP_LDX2))
                                    if (MessageBox.Show(this, rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_SOBRESCREVER"), rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_MBOX_TITLE"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                                        copiar = false;

                                if (copiar)
                                {
                                    File.Copy(DiretorioDestino + Util.Util.DIRETORIO_TEMPORARIO + Util.Util.ARQUIVO_TEMP_LDX, (DiretorioDestino + Util.Util.DIRETORIO_TEMPORARIO + Util.Util.ARQUIVO_TEMP_LDX).Replace(Util.Util.DIRETORIO_TEMPORARIO + Util.Util.ARQUIVO_TEMP_LDX, "\\"+ Util.Util.ARQUIVO_TEMP_LDX2), true);
                                    MessageBox.Show(rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_MBOX_SUCESSO"));
                                }

                            }
                            catch
                            {
                                MessageBox.Show(rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_MBOX_ERRO"), rm.GetString("MAIN_MENU_HELP_EXTRAIR_LDX2_MBOX_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                Directory.Delete(DiretorioDestino + Util.Util.DIRETORIO_TEMPORARIO, true);
                            }


                        }
                    }
                }
            }
        }

        private void importFontMenuItem_Click(object sender, EventArgs e)
        {

            if (this.MdiChildren.Count() > 0)
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_TOOLS_IMPORT_FONT_MBOX"));
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                openFileDialog.Filter = rm.GetString("MAIN_MENU_FILE_IMPORT_FONT_FILTRO");
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string FileName = openFileDialog.FileName;

                    if (File.Exists(FileName))
                    {
                        bool copiarFonte = true;
                        if (File.Exists(Fachada.diretorio_fontes + Path.GetFileName(FileName)))
                            if (MessageBox.Show(this, rm.GetString("MAIN_MENU_FILE_IMPORT_FONT_SOBRESCREVER"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo) == DialogResult.No)
                                copiarFonte = false;

                        if (copiarFonte)
                        {
                            try
                            {
                                Persistencia.Arquivo_FNT arquivo = new Persistencia.Arquivo_FNT();
                                arquivo.Abrir(FileName);

                                if (arquivo != null)
                                {
                                    if (!Directory.Exists(Fachada.diretorio_fontes))
                                    {
                                        Directory.CreateDirectory(Fachada.diretorio_fontes);
                                    }

                                    File.Copy(FileName, Fachada.diretorio_fontes + Path.GetFileName(FileName), true);
                                    MessageBox.Show(this, rm.GetString("MAIN_MENU_FILE_IMPORT_FONT_COPIADA"));
                                }
                            }
                            catch
                            {
                                MessageBox.Show(rm.GetString("MAIN_MENU_FILE_IMPORT_FONT_FORMATO_INVALIDO"));
                            }
                        }
                    }
                }
            }
        }

        private void sendNinbustoolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnviarArquivoNFXNinbus();
        }

        private void NinbustoolStripButton_Click(object sender, EventArgs e)
        {
            EnviarArquivoNFXNinbus();
        }

        private void EnviarArquivoNFXNinbus()
        {
            try
            {
                CursorWait();

                //Verificando se o ninbus ainda esta instalado na máquina
                ninbusInstalado = Util.Util.ExisteNinbusInstalado(out caminhoNinbus);
                if (!ninbusInstalado)
                {
                    CursorDefault();
                    MessageBox.Show(rm.GetString("MDI_PRINCIPAL_MBOX_NINBUS_DESINSTALADO"));
                    return;
                }

                //Verificando se o ninbus já está em execução
                Process[] ninbus = System.Diagnostics.Process.GetProcessesByName(Util.Util.NOME_ARQUIVO_NINBUS);
                if (ninbus.Length > 0)
                {
                    CursorDefault();
                    MessageBox.Show(rm.GetString("MDI_PRINCIPAL_MBOX_NINBUS_EM_EXECUCAO"));
                    return;
                }

                //abrindo arquivo .NFX
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                openFileDialog.Filter = rm.GetString("ARQUIVO_TABPAGE_TRANSMISSAO_ARQUIVO_BINARIO");

                CursorDefault();

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    CursorWait();

                    string FileName = openFileDialog.FileName;

                    if (File.Exists(FileName))
                    {
                        //verificando se o ninbus server application esta rodando
                        Process[] ninbusServer = System.Diagnostics.Process.GetProcessesByName(Util.Util.NOME_ARQUIVO_NINBUS_SERVER);
                        if (ninbusServer.Length == 0)
                        {
                            //abrindo ninbus server application
                            Process ninbusServerProcess = new Process();
                            ninbusServerProcess.StartInfo.FileName = caminhoNinbus + Util.Util.NOME_ARQUIVO_NINBUS_SERVER + Util.Util.ARQUIVO_EXT_EXE;
                            ninbusServerProcess.Start();
                        }


                        //abrindo ninbus para envio do arquivo nfx
                        Process ninbusProcess = new Process();
                        ninbusProcess.StartInfo.Arguments = FileName.Replace(" ","$") + " " + Convert.ToByte(fachada.GetIdiomaFachada()).ToString();
                        ninbusProcess.StartInfo.FileName = caminhoNinbus + Util.Util.NOME_ARQUIVO_NINBUS + Util.Util.ARQUIVO_EXT_EXE;
                        ninbusProcess.Start();
                    }

                    CursorDefault();
                }
            }
            catch
            {
                CursorDefault();
            }
        }

        private void extrairNFXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX_MBOX_MSG"), rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX_MBOX_TITLE"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                // File OPEN
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                openFileDialog.Filter = rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX_FILTRO");
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string ArquivoOrigem = openFileDialog.FileName;
                    string DiretorioDestino = String.Empty;


                    // Destino
                    if (MessageBox.Show(this, rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX_DIRETORIO"), rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX_MBOX_TITLE"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        if (fbd.ShowDialog(this) == DialogResult.OK)
                        {
                            try
                            {
                                DiretorioDestino = fbd.SelectedPath + Util.Util.DIRETORIO_NFX;

                                if (Directory.Exists(DiretorioDestino))
                                    Directory.Delete(DiretorioDestino, true);

                                fachada.ExtrairArquivoLDX2(ArquivoOrigem, DiretorioDestino);

                                MessageBox.Show(rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX_MBOX_SUCESSO"));
                            }
                            catch
                            {
                                MessageBox.Show(rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX_MBOX_ERRO"), rm.GetString("MAIN_MENU_HELP_EXTRAIR_NFX_MBOX_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Directory.Delete(DiretorioDestino + Util.Util.DIRETORIO_TEMPORARIO, true);
                            }
                        }
                    }
                }
            }
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReleaseNotes frn = new FormReleaseNotes();
            frn.ShowDialog();
        }

        private void atualizarFirmwaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAtualizaFirmwares faf = new FormAtualizaFirmwares();
            faf.ShowDialog();
        }

        private void verificarAtualizaçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Version versao = new Version(Application.ProductVersion);
                fachada.RealizarDownloadNovasVersoesSoftware(versao.Major.ToString(), Application.ProductName, Application.ProductVersion);
                fachada.RealizarDownloadNovasVersoesFirmware();
                MessageBox.Show(rm.GetString("MAIN_MENU_HELP_VERIFICAR_ATUALIZACAO_MBOX_SUCESSO"));
            }
            catch
            {
                MessageBox.Show(rm.GetString("MAIN_MENU_HELP_VERIFICAR_ATUALIZACAO_MBOX_ERRO"), rm.GetString("MAIN_MENU_HELP_VERIFICAR_ATUALIZACAO_MBOX_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
