using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Controlador;
using System.Resources;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace PontosX2
{
    public partial class Arquivo : Form
    {
        #region Propriedades

        [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool Repaint);

        //Propriedades do dragdrop dbgridview
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private const int colunaPainel = 0;
        private const int colunaAltura = 1;
        private const int colunaLargura = 2;
        private const int colunaBrilhoMin = 3;
        private const int colunaBrilhoMax = 4;
        private const int colunaMultiLinhas = 5;

        private const int tab_Arquivos = 0;
        private const int tab_Roteiros = 1;
        private const int tab_Mensagens = 2;
        private const int tab_Motoristas = 3;
        private const int tab_MensagensEsp = 4;
        private const int tab_Agendamento = 5;
        private const int tab_Simulacao = 6;
        private const int tab_Trasmitir = 7;

        private const int bloqueio_roteiro = 0;
        private const int bloqueio_mensagem = 1;
        private const int bloqueio_ida_volta = 2;
        private const int bloqueio_alterna_roteiro = 3;
        private const int bloqueio_seleciona_painel = 4;
        private const int bloqueio_ajusta_hora_saida = 5;
        private const int bloqueio_ajusta_relogio = 6;
        private const int bloqueio_identifica_painel = 7;
        private const int bloqueio_nova_config = 8;
        private const int bloqueio_coleta_dump = 9;
        private const int bloqueio_coleta_log = 10;
        private const int bloqueio_apagar_config = 11;
        private const int bloqueio_acender_paineis = 12;
        private const int bloqueio_config_fabrica = 13;
        private const int bloqueio_mens_secundaria = 14;
        private const int bloqueio_modo_teste = 15;
        private const int bloqueio_ajuste_brilho = 16;
        private const int bloqueio_seleciona_motorista = 17;

        private bool exibirMboxRegiao;
        private int ultimoIndexRegiao;
        private int ultimoIndexPainelExibido = 0;
        private Frase fraseGUI;

        public Forms.Roteiros.Roteiros EditorRoteiros { get; set; }
        public Forms.Roteiros.TextosEditor TextosEditor { get; set; }
        public Forms.Roteiros.NumEditor NumEditor { get; set; }
        public Forms.Mensagens.Mensagens EditorMensagens { get; set; }
        public Forms.Mensagens.TextosEditorMsg TextosEditorMsg { get; set; }
        public Forms.MensagensEspeciais.TextosEditorMsgEsp TextosEditorMsgEsp { get; set; }
        public Forms.Motorista.MotoristaEditor MotoristaEditor { get; set; }

        Fachada fachada = Fachada.Instance;
        private int painelSelecionado = 0;
        public int controladorSelecionado = 0;
        public Boolean criarNovo = true;
        public String NomeArquivo = String.Empty;
        public ResourceManager rm;
        bool ultimaSituacaoPainel;
        int indiceSelecionadoListaRoteiros;

        List<VideoBitmap.VideoBitmap> listaVideoBitMap;

        //Lista de frases que serão exibidas pelo botão apresentar quando estiver visivel os textos de roteiro ou mensagem
        List<Frase> listaFrasesApresentacao = null;

        TrackBar tbarAltura = new TrackBar();
        TrackBar tbarLargura = new TrackBar();
        C1.Win.C1Input.C1RangeSlider rangeAltura = new C1.Win.C1Input.C1RangeSlider();

        ToolTip toolTipTrackBar = new ToolTip();


        private int currentRow;
        private bool resetRow = false;
        private bool tbarClicked = false;

        BackgroundWorker background = new BackgroundWorker();

        BackgroundWorker backgroundApresentacao1 = new BackgroundWorker();
        BackgroundWorker backgroundApresentacao2 = new BackgroundWorker();
        BackgroundWorker backgroundApresentacao3 = new BackgroundWorker();
        BackgroundWorker backgroundApresentacao4 = new BackgroundWorker();

        //controle da exibição da exibição
        AutoResetEvent esperarApresentacao1 = new AutoResetEvent(false);
        AutoResetEvent esperarApresentacao2 = new AutoResetEvent(false);
        AutoResetEvent esperarApresentacao3 = new AutoResetEvent(false);
        AutoResetEvent esperarApresentacao4 = new AutoResetEvent(false);

        bool CancelarApresentacao = false;

        #endregion

        #region Construtor

        public Arquivo(Boolean criarNovoArquivo, String nomeArquivo)
        {
            InitializeComponent();

            #region Carregando UserControls

            listaVideoBitMap = new List<VideoBitmap.VideoBitmap>();
            listaVideoBitMap.Add(new VideoBitmap.VideoBitmap());

            // Instanciando objeto Roteiros e suas propriedades
            EditorRoteiros = new Forms.Roteiros.Roteiros();
            EditorRoteiros.Visible = false;
            EditorRoteiros.Dock = DockStyle.Fill;
            EditorRoteiros.Location = new System.Drawing.Point(6, 6);
            EditorRoteiros.Size = new System.Drawing.Size(736, 361);

            // Passa objeto ListarRoteiros para que o objeto Roteiros possa acioná-lo
            EditorRoteiros.ListarRoteiros = listarRoteiros1;

            // Adicionando objeto Roteiros a lista de controles de tabPageRoteiro
            tabPageRoteiro.Controls.Add(EditorRoteiros);

            // Passa objeto Roteiros para que o objeto ListarRoteiros possa acioná-lo
            listarRoteiros1.EditorRoteiros = EditorRoteiros;

            // Instanciando objeto TextosEditor e suas propriedades
            TextosEditor = new Forms.Roteiros.TextosEditor();
            TextosEditor.Visible = false;
            TextosEditor.Dock = DockStyle.Fill;
            TextosEditor.Location = new System.Drawing.Point(6, 6);
            TextosEditor.Size = new System.Drawing.Size(736, 361);

            // Instanciando objeto NumEditor e suas propriedades
            NumEditor = new Forms.Roteiros.NumEditor();
            NumEditor.Visible = false;
            NumEditor.Dock = DockStyle.Fill;
            NumEditor.Location = new System.Drawing.Point(6, 6);
            NumEditor.Size = new System.Drawing.Size(736, 361);

            // Passa objeto ListarRoteiros para que o objeto Roteiros possa acioná-lo
            TextosEditor.Roteiros = EditorRoteiros;

            // Passa objeto ListarRoteiros para que o objeto Roteiros possa acioná-lo
            NumEditor.Roteiros = EditorRoteiros;

            // Adicionando objeto Roteiros a lista de controles de tabPageRoteiro
            tabPageRoteiro.Controls.Add(TextosEditor);

            // Adicionando objeto Roteiros a lista de controles de tabPageRoteiro
            tabPageRoteiro.Controls.Add(NumEditor);

            // Passa objeto Roteiros para que o objeto ListarRoteiros possa acioná-lo
            EditorRoteiros.TextosEditor = TextosEditor;

            // Passa objeto Roteiros para que o objeto ListarRoteiros possa acioná-lo
            EditorRoteiros.NumEditor = NumEditor;

            //Instanciando a edição dos Labels de ID e Nome do Motorista
            MotoristaEditor = new Forms.Motorista.MotoristaEditor();
            MotoristaEditor.Visible = false;
            MotoristaEditor.Dock = DockStyle.Fill;
            MotoristaEditor.Location = new System.Drawing.Point(6, 6);
            MotoristaEditor.Size = new System.Drawing.Size(736, 361);

            //Passando a Lista de Motorista como referencia para a edição dos motoristas
            MotoristaEditor.Motoristas = editarMotorista;

            //Passando o Editor de Motorista como parametro para lista de motorista
            editarMotorista.MotoristaEditor = MotoristaEditor;

            //adicionando o editor de motorista para a tabpage de motorista
            tabPageMotorista.Controls.Add(MotoristaEditor);

            // Instanciando objeto Roteiros e suas propriedades
            EditorMensagens = new Forms.Mensagens.Mensagens();
            EditorMensagens.Visible = false;
            EditorMensagens.Dock = DockStyle.Fill;
            EditorMensagens.Location = new System.Drawing.Point(6, 6);
            EditorMensagens.Size = new System.Drawing.Size(736, 361);

            // Passa objeto ListarRoteiros para que o objeto Roteiros possa acioná-lo
            EditorMensagens.ListarMensagens = listarMensagens1;

            // Adicionando objeto Roteiros a lista de controles de tabPageRoteiro
            tabPageMensagem.Controls.Add(EditorMensagens);

            // Passa objeto Roteiros para que o objeto ListarRoteiros possa acioná-lo
            listarMensagens1.EditorMensagens = EditorMensagens;

            // Instanciando objeto TextosEditor e suas propriedades
            TextosEditorMsg = new Forms.Mensagens.TextosEditorMsg();
            TextosEditorMsg.Visible = false;
            TextosEditorMsg.Dock = DockStyle.Fill;
            TextosEditorMsg.Location = new System.Drawing.Point(6, 6);
            TextosEditorMsg.Size = new System.Drawing.Size(736, 361);

            // Passa objeto ListarRoteiros para que o objeto Roteiros possa acioná-lo
            TextosEditorMsg.Mensagens = EditorMensagens;

            // Adicionando objeto Roteiros a lista de controles de tabPageRoteiro
            tabPageMensagem.Controls.Add(TextosEditorMsg);

            // Passa objeto Roteiros para que o objeto ListarRoteiros possa acioná-lo
            EditorMensagens.TextosEditorMsg = TextosEditorMsg;

            #endregion

            #region Carregando o Idioma

            //Carregando idioma do software
            rm = fachada.carregaIdioma();

            //setando a região de acordo com o idioma escolhido na GUI pelo usuário
            string regTemp = Util.Util.RGN_EN_US;
            switch (fachada.GetIdioma())
            {
                case Util.Util.Lingua.Espanhol: regTemp = Util.Util.RGN_ES_ES; break;
                case Util.Util.Lingua.Ingles: regTemp = Util.Util.RGN_EN_US; break;
                case Util.Util.Lingua.Portugues: regTemp = Util.Util.RGN_PT_BR; break;
            }

            #endregion

            #region Definindo Controlador

            //Retorna o numero de controladores
            controladorSelecionado = fachada.QuantidadeControladores();

            this.criarNovo = criarNovoArquivo;
            if (criarNovo)
            {
                fachada.ControladorDefault(rm, fachada.CarregarRegiao(regTemp));
                foreach (Painel p in fachada.CarregarPaineis(controladorSelecionado))
                    fachada.SetarFontesDefaultPainel(p);
            }
            else
            {
                this.controladorSelecionado = this.controladorSelecionado - 1;
            }

            #endregion

            #region Populando Tela

            //Setando o controlador para outras telas
            SetarControladoSelecionado(this.controladorSelecionado);

            // Exibe painéis do controlador.
            PopulaPaineisDataGrid(fachada.CarregarPaineis(controladorSelecionado));
            PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));

            //setando tamanho do componente VideoBitmap do painel principal
            DesenharPainel();

            //Carregar Roteiros do Painel Principal
            CarregarRoteirosPainel();

            //Carregar Mensagens do Painel Principal
            CarregarMensagensPainel();

            //Carrega regiões
            CarregarRegioes();

            #endregion

            #region Verificando Região

            // se for arquivo .ldx vai carregar a região da lingua selecionada no software
            if (nomeArquivo.EndsWith(Util.Util.ARQUIVO_EXT_LDX))
            {
                fachada.setarRgnControlador(controladorSelecionado, regTemp);
            }

            //setar a região da fachada
            if (cbRegiao.Items.Count > 0)
            {
                exibirMboxRegiao = false;
                if (criarNovo)
                {
                    if (cbRegiao.Items.IndexOf(regTemp) == -1)
                        cbRegiao.SelectedIndex = 0;
                    else
                        cbRegiao.SelectedIndex = cbRegiao.Items.IndexOf(regTemp);

                    fachada.setarRgnControlador(controladorSelecionado, cbRegiao.SelectedItem.ToString());
                }
                else
                {
                    if (cbRegiao.Items.IndexOf(fachada.GetNomeRegiao(controladorSelecionado)) == -1)
                    {
                        //refazendo as mensagens especias e de emergência porque não foi encontrada a região do usuário
                        MessageBox.Show(rm.GetString("ARQUIVO_MBOX_REGIAO_NAO_ENCONTRADA1") +" [" + fachada.GetNomeRegiao(controladorSelecionado) + "] "+rm.GetString("ARQUIVO_MBOX_REGIAO_NAO_ENCONTRADA2"));

                        cbRegiao.SelectedIndex = cbRegiao.Items.IndexOf(regTemp);
                        fachada.setarRgnControlador(controladorSelecionado, cbRegiao.SelectedItem.ToString());
                        fachada.CriarMensagemEmergencia(controladorSelecionado);
                        fachada.CriarMensagemEspecial(controladorSelecionado);
                    }
                    else
                        cbRegiao.SelectedIndex = cbRegiao.Items.IndexOf(fachada.GetNomeRegiao(controladorSelecionado));
                }
            }

            #endregion

            #region Backgroundworkers

            //Propriedades do BackGroundWorker para atualizar a progressbar de transmissao do nandfs
            background.DoWork += new DoWorkEventHandler(background_DoWork);
            background.ProgressChanged += new ProgressChangedEventHandler(background_ProgressChanged);
            background.RunWorkerCompleted += new RunWorkerCompletedEventHandler(background_RunWorkerCompleted);
            background.WorkerReportsProgress = true;

            //Propriedades do BackGroundWorker para desenhar no painel
            backgroundApresentacao1.DoWork += new DoWorkEventHandler(backgroundApresentacao1_DoWork);
            backgroundApresentacao1.WorkerReportsProgress = true;
            backgroundApresentacao1.WorkerSupportsCancellation = true;

            backgroundApresentacao2.DoWork += new DoWorkEventHandler(backgroundApresentacao2_DoWork);
            backgroundApresentacao2.WorkerReportsProgress = true;
            backgroundApresentacao2.WorkerSupportsCancellation = true;

            backgroundApresentacao3.DoWork += new DoWorkEventHandler(backgroundApresentacao3_DoWork);
            backgroundApresentacao3.WorkerReportsProgress = true;
            backgroundApresentacao3.WorkerSupportsCancellation = true;

            backgroundApresentacao4.DoWork += new DoWorkEventHandler(backgroundApresentacao4_DoWork);
            backgroundApresentacao4.WorkerReportsProgress = true;
            backgroundApresentacao4.WorkerSupportsCancellation = true;

            #endregion

            #region TrackBars

            tbarAltura.Scroll += new System.EventHandler(this.tbarAltura_Scroll);
            tbarAltura.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbarAltura_MouseDown);
            tbarAltura.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarAltura_MouseUp);
            tbarAltura.MouseHover += new System.EventHandler(this.tbarAltura_MouseHover);
            tbarAltura.TickStyle = TickStyle.None;
            tbarAltura.SendToBack();
            tbarAltura.LargeChange = 0;
            tbarLargura.SmallChange = 0;
            tbarAltura.Orientation = Orientation.Vertical;

            rangeAltura.Width = 23;
            rangeAltura.LowerValueChanged += new System.EventHandler(this.rangeAltura_LowerValueChanged);
            rangeAltura.UpperValueChanged += new System.EventHandler(this.rangeAltura_UpperValueChanged);
            rangeAltura.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rangeAltura_MouseDown);
            rangeAltura.MouseHover += new System.EventHandler(this.rangeAltura_MouseHover);
            rangeAltura.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rangeAltura_MouseUp);
            rangeAltura.Orientation = Orientation.Vertical;
            rangeAltura.VisualStyle = C1.Win.C1Input.VisualStyle.System;
            rangeAltura.SendToBack();

            tbarLargura.Scroll += new System.EventHandler(this.tbarLargura_Scroll);
            tbarLargura.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbarLargura_MouseDown);
            tbarLargura.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbarLargura_MouseUp);
            tbarLargura.MouseHover += new System.EventHandler(this.tbarLargura_MouseHover);
            tbarLargura.TickStyle = TickStyle.None;
            tbarLargura.SendToBack();
            tbarLargura.LargeChange = 0;
            tbarLargura.SmallChange = 0;
            tbarLargura.Orientation = Orientation.Horizontal;

            // Tooltip das trackbars
            toolTipTrackBar.AutoPopDelay = 5000;
            toolTipTrackBar.InitialDelay = 1000;
            toolTipTrackBar.ReshowDelay = 500;
            toolTipTrackBar.ShowAlways = true;

            #endregion

            #region Propriedades e Eventos da GUI

            this.lbCarminho.Text = nomeArquivo;
            this.Text = nomeArquivo;
            this.NomeArquivo = nomeArquivo;

            button1.Enabled = false;
            button2.Enabled = false;

            //fixando o splitcontainer do painel para o usuário não alterar a proporção
            splitContainer1.IsSplitterFixed = true;

            // Propriedade para o dataGridView
            dataGridViewPaineis.ReadOnly = false;

            panel1.Controls.Add(panelVideoBitmap);

            //Aplicando o resourcemanage a GUI e todas user control
            AplicaIdioma(false);

            cbPainel.MouseWheel += new MouseEventHandler(cbPainel_MouseWheel);

            InicializarTeclado();

            #endregion

        }

        #endregion

        #region Eventos da GUI

        void cbPainel_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void Arquivo_Shown(object sender, EventArgs e)
        {
            ReposicionarVideoMapNoPainel(false);
        }

        private void tabControlArquivo_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControlArquivo.SelectedIndex == tab_Trasmitir && background.IsBusy)
            {
                MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_GERANDO_NFX"));
                e.Cancel = true;
                
            }

            if (textosEditorMsgEsp1.Visible)
            {
                if (fachada.CompararObjetosMensagemEspecial(controladorSelecionado, painelSelecionado, textosEditorMsgEsp1.mensagemEspecialGUI) || fachada.CompararObjetosMensagemEmergencia(controladorSelecionado, painelSelecionado, textosEditorMsgEsp1.mensagemEmergenciaGUI))
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_TABCONTROL_MSG_ESPECIAL"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        textosEditorMsgEsp1.Salvar();

                DesenharPainel();
            }

            if (EditorRoteiros.Visible)
            {
                EditorRoteiros.PreencheObjetoGUI();
                if (EditorRoteiros.incluirRoteiro)
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_APLICAR"), rm.GetString("USER_CONTROL_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        if (!EditorRoteiros.exibirErro(true))
                            EditorRoteiros.AplicarRoteiro();
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                }
                else //Edição
                {
                    if (fachada.CompararObjetosRoteiro(controladorSelecionado, painelSelecionado, EditorRoteiros.roteiroGUI))
                        if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_ALTERAR"), rm.GetString("USER_CONTROL_ROTEIROS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            if (!EditorRoteiros.exibirErro(true))
                                EditorRoteiros.AplicarRoteiro();
                            else
                            {
                                e.Cancel = true;
                                return;
                            }
                }

                EditorRoteiros.Visible = false;
                listarRoteiros1.Visible = true;
                cbPainel.Enabled = true;
                DesenharPainel();
            }

            if (NumEditor.Visible)
            {

                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_ALTERAR"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (!NumEditor.exibirErro())
                    {
                        NumEditor.Roteiros.EditarNumeroGUI(NumEditor.numeroGUI);
                        if (!EditorRoteiros.exibirErro(true))
                            EditorRoteiros.AplicarRoteiro();
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                NumEditor.Visible = false;
                EditorRoteiros.Visible = false;
                listarRoteiros1.Visible = true;
                cbPainel.Enabled = true;
                DesenharPainel();
            }


            if (TextosEditor.Visible)
            {

                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_ROTEIROS_MBOX_ALTERAR"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (EditorRoteiros.exibirErro(true))
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (TextosEditor.isFraseIda)
                    {
                        if (TextosEditor.isEdicao)
                            TextosEditor.Roteiros.EditarFraseIdaRoteiroGUI(TextosEditor.fraseGUI);
                        else
                            TextosEditor.Roteiros.IncluirFraseIdaRoteiroGUI(TextosEditor.fraseGUI);
                    }
                    else
                    {
                        if (TextosEditor.isEdicao)
                            TextosEditor.Roteiros.EditarFraseVoltaRoteiroGUI(TextosEditor.fraseGUI);
                        else
                            TextosEditor.Roteiros.IncluirFraseVoltaRoteiroGUI(TextosEditor.fraseGUI);
                    }

                    //EditorRoteiros.PreencheObjetoGUI();
                    EditorRoteiros.AplicarRoteiro();

                }

                TextosEditor.Visible = false;
                EditorRoteiros.Visible = false;
                listarRoteiros1.Visible = true;
                cbPainel.Enabled = true;
                DesenharPainel();
            }

            if (EditorMensagens.Visible)
            {
                EditorMensagens.PreencheObjetoGUI();

                if (EditorMensagens.incluirMensagem)
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_APLICAR"), rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        if (!EditorMensagens.exibirErro())
                            EditorMensagens.AplicarMensagem();
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                }
                else //Edição
                {
                    if (fachada.CompararObjetosMensagem(controladorSelecionado, painelSelecionado, EditorMensagens.mensagemGUI))
                        if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_ALTERAR"), rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            if (!EditorMensagens.exibirErro())
                                EditorMensagens.AplicarMensagem();
                            else
                            {
                                e.Cancel = true;
                                return;
                            }
                }

                EditorMensagens.Visible = false;
                listarMensagens1.Visible = true;
                cbPainel.Enabled = true;
                DesenharPainel();
            }

            if (TextosEditorMsg.Visible)
            {

                if (DialogResult.Yes == MessageBox.Show(rm.GetString("USER_CONTROL_MENSAGENS_MBOX_ALTERAR"), rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (EditorMensagens.exibirErro())
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (TextosEditorMsg.isEdicao)
                        TextosEditorMsg.Mensagens.EditarFraseMensagemGUI(TextosEditorMsg.fraseGUI);
                    else
                        TextosEditorMsg.Mensagens.IncluirFraseMensagemGUI(TextosEditorMsg.fraseGUI);

                    //EditorMensagens.PreencheObjetoGUI();
                    EditorMensagens.AplicarMensagem();
                }

                TextosEditorMsg.Visible = false;
                EditorMensagens.Visible = false;
                listarMensagens1.Visible = true;
                cbPainel.Enabled = true;
                DesenharPainel();
            }

            if (editorSimulacao.Visible)
            {
                if (editorSimulacao.MudouParametros() && editorSimulacao.ValidarCampos(false))
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_TABCONTROL_SIMULACAO"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        editorSimulacao.SetarParametrosNaFachada();

                //listaVideoBitMap[0].Bitmap = fachada.PreparaBitMapPainel(controladorSelecionado, painelSelecionado)[0];
                // DesenharPainel();
            }

            if (editarMotorista.Visible)
            {
                if (editarMotorista.UsuarioEditando())
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_TABCONTROL_MOTORISTA"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        if (editarMotorista.exibirErro(true))
                        {
                            e.Cancel = true;
                            return;
                        }

                        editarMotorista.AplicarMotorista();
                    }

                cbPainel.Enabled = true;
                LimparPaineis();
            }

            if (MotoristaEditor.Visible)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_TABCONTROL_MOTORISTA"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (MotoristaEditor.exibirErro(true))
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (MotoristaEditor.isID)
                        MotoristaEditor.Motoristas.EditarIDGUI(MotoristaEditor.fraseGUI);
                    else
                        MotoristaEditor.Motoristas.EditarNomeGUI(MotoristaEditor.fraseGUI);

                    MotoristaEditor.Motoristas.AplicarMotorista();

                }

                MotoristaEditor.Visible = false;
                editarMotorista.Visible = true;
                cbPainel.Enabled = true;
                LimparPaineis();
            }

        }

        private void cbRegiao_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (exibirMboxRegiao)
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_REGIAO"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        CursorWait();

                        fachada.setarRgnControlador(controladorSelecionado, cbRegiao.SelectedItem.ToString());
                        fachada.CriarMensagemEmergencia(controladorSelecionado);
                        fachada.CriarMensagemEspecial(controladorSelecionado);
                        exibirMboxRegiao = true;
                        ultimoIndexRegiao = cbRegiao.SelectedIndex;

                        CursorDefault();
                    }
                    else
                    {
                        exibirMboxRegiao = false;
                        cbRegiao.SelectedIndex = ultimoIndexRegiao;
                    }
                }
                else
                {
                    exibirMboxRegiao = true;
                    ultimoIndexRegiao = cbRegiao.SelectedIndex;
                }
            }
            catch
            {
                CursorDefault();
            }

        }



        private void dataGridViewPaineis_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                switch (e.ColumnIndex)
                {
                    case colunaAltura:
                        {
                            if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_ALTURA_PAINEL"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            {
                                CursorWait();

                                resetRow = true;
                                currentRow = e.RowIndex;

                                int alturaAnteriorPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado);
                                int larguraAnteriorPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);

                                //setando a altura na fachada
                                fachada.SetAlturaPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex, System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value));

                                //carregando informações na GUI
                                PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));
                                dataGridViewPaineis.Rows[currentRow].Selected = true;
                                dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[currentRow].Cells[e.ColumnIndex];
                                cbPainel.SelectedIndex = dataGridViewPaineis.CurrentCell.RowIndex;


                                if (fachada.GetMultiLinhas(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex) == 1)
                                    //Redimensionar os Textos do Roteiro e Mensagem para a nova altura
                                    fachada.RedimensionarRoteirosMensagens(fachada.CarregarPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex), alturaAnteriorPainel, larguraAnteriorPainel);
                                else
                                    //se for multilinhas redefenir a altura do painel de todos os textos
                                    fachada.AtualizarAlturaTextosPainelMultilinhas(fachada.CarregarPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex));


                                //Setar Fonte padrão para todos os textos do painel de acordo com a nova altura
                                fachada.SetarFontesDefaultPainel(fachada.CarregarPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex));

                                SetarPainelSelecionado();
                                DesenharPainel();

                                CursorDefault();
                            }
                            else
                            {
                                // dataGridViewPaineis.CurrentCell.Value = fachada.GetAlturaPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);
                                dataGridViewPaineis.Rows[dataGridViewPaineis.CurrentCell.RowIndex].Cells[colunaAltura].Value = System.Convert.ToString((fachada.GetAlturaPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex)/fachada.GetMultiLinhas(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex)));
                            }
                        }
                        break;

                    case colunaLargura:
                        {
                            if (ValidaLargura())
                            {
                                if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_LARGURA_PAINEL"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {
                                    CursorWait();

                                    resetRow = true;
                                    currentRow = e.RowIndex;

                                    int alturaAnteriorPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado);
                                    int larguraAnteriorPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);

                                    //setando na fachada a nova largura
                                    fachada.SetLarguraPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex, System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value));

                                    //populando na GUI
                                    PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));
                                    dataGridViewPaineis.Rows[currentRow].Selected = true;
                                    dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[currentRow].Cells[e.ColumnIndex];
                                    cbPainel.SelectedIndex = dataGridViewPaineis.CurrentCell.RowIndex;


                                    if (fachada.GetMultiLinhas(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex) == 1)
                                        //Redimensionar todos os textos do Roteiro e Mensagem
                                        fachada.RedimensionarRoteirosMensagens(fachada.CarregarPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex), alturaAnteriorPainel, larguraAnteriorPainel);
                                    else
                                        //se for multilinhas redefenir a largura do painel de todos os textos
                                        fachada.AtualizarLarguraTextosPainelMultilinhas(fachada.CarregarPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex));

                                    SetarPainelSelecionado();
                                    DesenharPainel();

                                    CursorDefault();
                                }
                                else
                                {
                                    dataGridViewPaineis.CurrentCell.Value = fachada.GetLarguraPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);
                                }
                            }
                            else
                            {
                                dataGridViewPaineis.CurrentCell.Value = fachada.GetLarguraPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);
                            }
                        }
                        break;

                    case colunaBrilhoMin:
                        {
                            if (ValidaBrilhoMin())
                                fachada.SetarBrilhoMinPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex, System.Convert.ToByte(dataGridViewPaineis.CurrentCell.Value));
                            else
                                dataGridViewPaineis.CurrentCell.Value = fachada.GetBrilhoMinPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);
                        }
                        break;

                    case colunaBrilhoMax:
                        {
                            if (ValidaBrilhoMax())
                                fachada.SetarBrilhoMaxPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex, System.Convert.ToByte(dataGridViewPaineis.CurrentCell.Value));
                            else
                                dataGridViewPaineis.CurrentCell.Value = fachada.GetBrilhoMaxPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);
                        }
                        break;

                    case colunaMultiLinhas:
                        {

                            if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_PAINEL_MBOX_PARA_MULTI_LINHAS"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            {
                                CursorWait();

                                resetRow = true;
                                currentRow = e.RowIndex;

                                fachada.SetMultiLinhas(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex, System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value));
                                fachada.SetAlturaPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex, System.Convert.ToInt16(dataGridViewPaineis.Rows[currentRow].Cells[colunaAltura].Value));

                                //remove o painelAPP se o usuário setou o painel para ser multilinhas
                                if (fachada.GetMultiLinhas(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex) > 1)
                                    fachada.RemoverPainelMultilinhaAPP(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);

                                PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));
                                dataGridViewPaineis.Rows[currentRow].Selected = true;
                                dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[currentRow].Cells[e.ColumnIndex];
                                cbPainel.SelectedIndex = dataGridViewPaineis.CurrentCell.RowIndex;

                                //Alterar todo o roteiro para refletir o painel multilinhas
                                fachada.ConverterPainelMultiLinhas(fachada.CarregarPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex), rm);

                                //Setar Fonte padrão para todos os textos do painel de acordo com a nova altura
                                fachada.SetarFontesDefaultPainel(fachada.CarregarPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex));

                                SetarPainelSelecionado();
                                DesenharPainel();

                                CursorDefault();
                            }
                            else
                            {
                                dataGridViewPaineis.Rows[dataGridViewPaineis.CurrentCell.RowIndex].Cells[colunaMultiLinhas].Value = System.Convert.ToString(fachada.GetMultiLinhas(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex));
                            }
                        }
                        break;
                }
            }
            catch
            {
                CursorDefault();
            }
        }

        public void BloquearAbasPainelMultiLinhas()
        {
            if (fachada.GetMultiLinhas(controladorSelecionado, cbPainel.SelectedIndex) > 1)
            {
                tabControlArquivo.Controls[tab_Mensagens].Enabled = false;
                tabControlArquivo.Controls[tab_MensagensEsp].Enabled = false;
                tabControlArquivo.Controls[tab_Agendamento].Enabled = false;
                tabControlArquivo.Controls[tab_Motoristas].Enabled = false;

            }
            else
            {
                tabControlArquivo.Controls[tab_Mensagens].Enabled = true;
                tabControlArquivo.Controls[tab_MensagensEsp].Enabled = true;
                tabControlArquivo.Controls[tab_Agendamento].Enabled = true;
                tabControlArquivo.Controls[tab_Motoristas].Enabled = true;
            }
        }

        private void Arquivo_ClientSizeChanged(object sender, EventArgs e)
        {
            if (TextosEditor.Visible || TextosEditorMsg.Visible)
                ReposicionarVideoMapNoPainel(true);
            else
                ReposicionarVideoMapNoPainel(false);
        }

        private void dataGridViewPaineis_SelectionChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).ContainsFocus)
            {
                if (resetRow)
                {
                    resetRow = false;
                    dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[currentRow].Cells[2];
                    dataGridViewPaineis.Rows[currentRow].Selected = true;
                    cbPainel.SelectedIndex = currentRow;

                }

                if (dataGridViewPaineis.SelectedRows.Count > 0)
                    cbPainel.SelectedIndex = dataGridViewPaineis.SelectedRows[0].Index;

                SetarPainelSelecionado();

                //setando tamanho do componente VideoBitmap do painel principal
                DesenharPainel();
            }
        }

        private void Arquivo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(this, rm.GetString("ARQUIVO_MBOX_SALVAR_AO_FECHAR"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (!((MDIPrincipal)(this.Parent.Parent)).saveFile())
                        e.Cancel = true;
                }

            }

            //se o usuário estiver apresentando textos no painel
            if (btnApresentar.Enabled && btnApresentar.Text == rm.GetString("ARQUIVO_BTN_APRESENTAR_CANCELAR") && !e.Cancel)
            {
                //cancelar a apresentacao
                btnApresentar_Click(sender, e);

                //espera o cancelamento
                while (ApresentacaoPainelIsBusy())
                {
                    Thread.Sleep(5);
                    Application.DoEvents();
                }
            }

            //se o usuário estiver gerando arquivo .NFX
            if (background.IsBusy)
            {
                CancelarNFX();

                //espera o cancelamento
                while (background.IsBusy)
                {
                    Thread.Sleep(5);
                    Application.DoEvents();
                }
            }
        }

        private void cbPainel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).ContainsFocus)
            {
                //bloquear abas
                //BloquearAbasPainelMultiLinhas();

                //setando tamanho do componente VideoBitmap do painel
                DesenharPainel();

                //Carrega as abas com as informações do painel selecionado
                listarRoteiros1.painelSelecionado = cbPainel.SelectedIndex;
                listarRoteiros1.controladorSelecionado = controladorSelecionado;
                if (listarRoteiros1.Visible)
                {
                    CarregarRoteirosPainel();
                }

                //Carrega Aba de mensagens
                listarMensagens1.painelSelecionado = cbPainel.SelectedIndex;
                listarMensagens1.controladorSelecionado = controladorSelecionado;
                if (listarMensagens1.Visible)
                {
                    CarregarMensagensPainel();
                }


                //Carrega Aba de MensagensEspeciais
                if (textosEditorMsgEsp1.Visible)
                {
                    if (painelSelecionado != cbPainel.SelectedIndex)
                    {
                        //se o painel anterior era normal, então pergunta se quer salvar
                        if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                        {
                            if (fachada.CompararObjetosMensagemEspecial(controladorSelecionado, painelSelecionado, textosEditorMsgEsp1.mensagemEspecialGUI) || (fachada.CompararObjetosMensagemEmergencia(controladorSelecionado, painelSelecionado, textosEditorMsgEsp1.mensagemEmergenciaGUI)))
                            {
                                if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_PAINEL_MSG_ESPECIAL"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {
                                    textosEditorMsgEsp1.Salvar();
                                }
                            }
                        }


                        textosEditorMsgEsp1.painelSelecionado = cbPainel.SelectedIndex;
                        textosEditorMsgEsp1.controladorSelecionado = controladorSelecionado;
                        painelSelecionado = cbPainel.SelectedIndex;
                        //Só recarrega a GUI se o painel selecionado for normal, se for multilinhas fica travado e a exibição do painel em branco
                        if (fachada.GetMultiLinhas(controladorSelecionado, cbPainel.SelectedIndex) == 1)
                            textosEditorMsgEsp1.EditarMensagensEspeciais(fachada.CarregarMensagemEspecial(controladorSelecionado, cbPainel.SelectedIndex), fachada.CarregarMensagemEmergencia(controladorSelecionado, cbPainel.SelectedIndex));

                    }
                }

                //Carrega Aba de Simulacao
                if (editorSimulacao.Visible)
                {
                    if (painelSelecionado != cbPainel.SelectedIndex)
                    {
                        if (editorSimulacao.MudouParametros() && editorSimulacao.ValidarCampos(false))
                            if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_PAINEL_SIMULACAO"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                editorSimulacao.SetarParametrosNaFachada();

                        editorSimulacao.painelSelecionado = cbPainel.SelectedIndex;
                        editorSimulacao.controladorSelecionado = controladorSelecionado;
                        editorSimulacao.EditarSimulacao();
                    }
                }

                //Carrega Aba de Agendamento
                if (editorAgendamento.Visible)
                {
                    if (painelSelecionado != cbPainel.SelectedIndex)
                    {
                        editorAgendamento.painelSelecionado = cbPainel.SelectedIndex;
                        editorAgendamento.controladorSelecionado = controladorSelecionado;
                        editorAgendamento.MudarPainel();
                    }
                }

                //Fazer a parte de motoristas
                if (editarMotorista.Visible)
                {
                    if (painelSelecionado != cbPainel.SelectedIndex)
                    {
                        if (editarMotorista.UsuarioEditando())
                        {
                            editarMotorista.isEdicao = true;

                            editarMotorista.PreencherObjetoMotorista();
                            //Comparar Objetos para ver se precisa salvar
                            if (fachada.CompararObjetosMotorista(controladorSelecionado, painelSelecionado, editarMotorista.indiceMotorista, editarMotorista.motoristaGUI))
                            {
                                if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_PAINEL_MOTORISTAS"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {
                                    if (!editarMotorista.exibirErro(false))
                                        editarMotorista.AplicarMudancaDePainel();
                                    else
                                        editarMotorista.CarregarMotoristaGUI();
                                }
                                else
                                    editarMotorista.CarregarMotoristaGUI();
                            }
                        }
                        else
                            editarMotorista.HabilitarListaMotoristas(true);

                        editarMotorista.painelSelecionado = cbPainel.SelectedIndex;
                        editarMotorista.controladorSelecionado = controladorSelecionado;
                        editarMotorista.MudarPainel();
                    }
                }

                //Carrega a tela de edição de roteiros
                if (EditorRoteiros.Visible)
                {

                    if (painelSelecionado != cbPainel.SelectedIndex)
                    {

                        EditorRoteiros.incluirRoteiro = false;

                        //Carregar objeto roteiro da GUI com os campos da GUI
                        EditorRoteiros.PreencheObjetoGUI();

                        if (fachada.CompararObjetosRoteiro(controladorSelecionado, painelSelecionado, EditorRoteiros.roteiroGUI))
                            if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_PAINEL_ROTEIROS"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                if (!EditorRoteiros.exibirErro(false))
                                    EditorRoteiros.AplicarRoteiroMudancaPainel();

                        EditorRoteiros.incluirRoteiro = false;
                        EditorRoteiros.painelSelecionado = cbPainel.SelectedIndex;
                        EditorRoteiros.PopularApresentacao();
                        EditorRoteiros.CarregarRoteiroGUI(controladorSelecionado, cbPainel.SelectedIndex, EditorRoteiros.roteiroGUI.Indice);
                        EditorRoteiros.HabilitarCamposPainelPrincipal();

                    }

                    EditorRoteiros.HabilitarBotaoApresentacao();
                    EditorRoteiros.DesmarcarListViews();
                    EditorRoteiros.HabilitarEdicaoNumero();
                }

                //Carrega a tela de edição de mensagens
                if (EditorMensagens.Visible)
                {

                    if (painelSelecionado != cbPainel.SelectedIndex)
                    {
                        EditorMensagens.incluirMensagem = false;

                        EditorMensagens.PreencheObjetoGUI();

                        if (fachada.CompararObjetosMensagem(controladorSelecionado, painelSelecionado, EditorMensagens.mensagemGUI))
                            if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MUDAR_PAINEL_MENSAGENS"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                if (!EditorMensagens.exibirErro())
                                    EditorMensagens.AplicarMensagem();

                        EditorMensagens.painelSelecionado = cbPainel.SelectedIndex;
                        EditorMensagens.HabilitarCampos();
                        EditorMensagens.CarregarMensagemGUI(controladorSelecionado, cbPainel.SelectedIndex, EditorMensagens.mensagemGUI.Indice);
                    }

                    EditorMensagens.HabilitarBotaoApresentacao();
                    EditorMensagens.DesmarcarListViews();
                }

                SetarPainelSelecionado();

                //Seleciona o painel no dbgridview se o tabcontrol de arquivo estiver selecionado
                if (tabControlArquivo.SelectedIndex == tab_Arquivos)
                {
                    dataGridViewPaineis.Rows[painelSelecionado].Selected = true;
                    dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[painelSelecionado].Cells[0];
                }
            }
        }

        private void tabControlArquivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlArquivo.SelectedIndex == tab_Trasmitir)
            {
                //Fachada fachada = Fachada.Instance;
                labelCaminhoUsuario.Text = "";
                labelCaminhoNFS.Visible = false;
                progressBarTransmissao.Value = 0;
                labelStatus.Text = "";                

                if (Directory.Exists(Fachada.diretorio_NSS))
                {
                    cbPainelNSS.Visible = true;
                    cbSendNSS.Visible = true;
                    gboxPainelNss.Visible = true;
                    chkPerifericos.Visible = false;
                }
                else
                {
                    cbPainelNSS.Visible = false;
                    cbSendNSS.Visible = false;
                    gboxPainelNss.Visible = false;
                    chkPerifericos.Visible = true;
                    habilitarCheckPerifericos();
                }

                CarregarDadosTransmissao();

            }

            if (tabControlArquivo.SelectedIndex == tab_Motoristas)
            {
                editarMotorista.HabilitarUserControl();
            }

        }

        private void CarregarDadosTransmissao()
        {
            bool[] funcoesBloquadas = fachada.RetornarBloqueioFuncoes(controladorSelecionado);

            cbHabilitaUsoLock.Checked = fachada.RetornarHabilitaUsoLock(controladorSelecionado);
            cbHabilitaUsoSenha.Checked = fachada.RetornarHabilitaUsoSenha(controladorSelecionado);
            chkHabilitaModoLD6.Checked = fachada.RetornarModoApresentacaoLD6(controladorSelecionado);
            tbAntiTheft.Text = fachada.RetornarAntiTheft(controladorSelecionado);
            tbSpecialAccess.Text = fachada.RetornarSpecialAccess(controladorSelecionado);

            if (funcoesBloquadas.Length != 0)
            {        
                //habilitar a imagem de roteiro e marca o check      
                habilitarRoute(!funcoesBloquadas[bloqueio_roteiro]);

                //apenas checa a mensagem
                clbBloqueioFuncoes.SetItemChecked(bloqueio_mensagem, funcoesBloquadas[bloqueio_mensagem]);

                //habilita as imagens e marca o check 
                habilitarRoundTrip(!funcoesBloquadas[bloqueio_ida_volta]);
                habilitarMessageType(!funcoesBloquadas[bloqueio_alterna_roteiro]);
                habilitarSelectSign(!funcoesBloquadas[bloqueio_seleciona_painel]);

                clbBloqueioFuncoes.SetItemChecked(bloqueio_ajusta_hora_saida, funcoesBloquadas[bloqueio_ajusta_hora_saida]);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_ajusta_relogio, funcoesBloquadas[bloqueio_ajusta_relogio]);

                for (int i = 8; i < funcoesBloquadas.Length; i++)
                {
                    // a função 7 é seleção de região dentro do controlador.
                    clbBloqueioFuncoes.SetItemChecked(i - 1, funcoesBloquadas[i]);
                }

                //verifica se todos os itens estiverem checados para habilitar a imagem de settings ou não
                habilitarSettings(!TodosItensSettingsChecados(), false);

                //verifica se a mensagem e a mensagem secundaria estão selecionadas para habilitar a imagem
                habilitarMessage(!TodosItensMensagensChecados(), false);

            }
            else
            {
                habilitarRoute(true);
                habilitarMessage(true, false);
                habilitarRoundTrip(true);
                habilitarMessageType(true);
                habilitarSelectSign(true);
                habilitarSettings(true , false);
            }
        }

        private bool TodosItensMensagensChecados()
        {
            if (clbBloqueioFuncoes.GetItemChecked(bloqueio_mensagem) && clbBloqueioFuncoes.GetItemChecked(bloqueio_mens_secundaria))
                return true;
            else
                return false;
        }

        private bool TodosItensSettingsChecados()
        {
            bool checados = true;

            for(int i = bloqueio_ajusta_hora_saida; i <= bloqueio_seleciona_motorista; i++)
            {
                // Foi solicitado pelo pós-venda que deixassemos desbloqueados Emparelhamento (bloqueio_identifica_painel), Novas Configurações e Coleta de DUMP
                if ((i>= bloqueio_identifica_painel)&&(i<=bloqueio_coleta_dump))
                {
                    continue;
                }
                if (i != bloqueio_mens_secundaria)
                {
                    if (!clbBloqueioFuncoes.GetItemChecked(i))
                    {
                        checados = false;
                        break;
                    }
                }
            }

            return checados;
        }

        private void habilitarImagemSettings(bool habilitar)
        {

        }
        
        private void tbarAltura_MouseHover(object sender, EventArgs e)
        {
            toolTipTrackBar.SetToolTip(tbarAltura, rm.GetString("ARQUIVO_VIDEOBITMAP_TRACKBAR_ALTURA_SUPERIOR") + " " + (fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - tbarAltura.Value).ToString() + "\n" + rm.GetString("ARQUIVO_VIDEOBITMAP_TRACKBAR_ALTURA_INFERIOR") + " " + tbarAltura.Value.ToString());
        }

        private void tbarLargura_MouseHover(object sender, EventArgs e)
        {
            toolTipTrackBar.SetToolTip(tbarLargura, rm.GetString("ARQUIVO_VIDEOBITMAP_TRACKBAR_LARGURA_ESQUERDA") + " " + tbarLargura.Value.ToString() + "\n" + rm.GetString("ARQUIVO_VIDEOBITMAP_TRACKBAR_LARGURA_DIREITA") + " " + (fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value).ToString());
        }

        private void tbarAltura_Scroll(object sender, EventArgs e)
        {
            if (tbarClicked)
                return;

            AtualizarPainel();
        }

        private void tbarAltura_MouseUp(object sender, MouseEventArgs e)
        {
            if (!tbarClicked)
                return;

            tbarClicked = false;

            AtualizarPainel();
        }

        private void tbarAltura_MouseDown(object sender, MouseEventArgs e)
        {
            tbarClicked = true;

            double dblValue;

            // Jump to the clicked location
            dblValue = ((double)e.Y / (double)tbarAltura.Height) * fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado);
            if (dblValue < Util.Util.ALTURA_MINIMA_TEXTOS_V04)
                dblValue = Util.Util.ALTURA_MINIMA_TEXTOS_V04;

            if (dblValue > tbarAltura.Maximum)
                dblValue = tbarAltura.Maximum;

            tbarAltura.Value = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - Convert.ToInt32(dblValue);


        }

        private void rangeAltura_MouseUp(object sender, MouseEventArgs e)
        {
            if (!tbarClicked)
                return;

            tbarClicked = false;

            AtualizarPainel();
        }

        private void rangeAltura_MouseHover(object sender, EventArgs e)
        {
            toolTipTrackBar.SetToolTip(rangeAltura, rm.GetString("ARQUIVO_VIDEOBITMAP_TRACKBAR_ALTURA_SUPERIOR") + " " + (fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado)-Convert.ToInt16(rangeAltura.UpperValue) + "\n" + rm.GetString("ARQUIVO_VIDEOBITMAP_TRACKBAR_ALTURA_MEIO") + " " + (Convert.ToInt16(rangeAltura.UpperValue)- Convert.ToInt16(rangeAltura.LowerValue)).ToString() + "\n" + rm.GetString("ARQUIVO_VIDEOBITMAP_TRACKBAR_ALTURA_INFERIOR") + " " + (Convert.ToInt16(rangeAltura.LowerValue)).ToString()));
        }

        private void rangeAltura_MouseDown(object sender, MouseEventArgs e)
        {
            tbarClicked = true;
        }

        private void rangeAltura_UpperValueChanged(object sender, EventArgs e)
        {
            if (!tbarClicked)
                return;

            if (rangeAltura.LowerValue + Util.Util.ALTURA_MINIMA_TEXTOS_V04 >= rangeAltura.UpperValue)
                    rangeAltura.UpperValue = rangeAltura.LowerValue + Util.Util.ALTURA_MINIMA_TEXTOS_V04;

        }

        private void rangeAltura_LowerValueChanged(object sender, EventArgs e)
        {
            if (!tbarClicked)
                return;

            if (rangeAltura.LowerValue + Util.Util.ALTURA_MINIMA_TEXTOS_V04 >= rangeAltura.UpperValue)
                    rangeAltura.LowerValue = rangeAltura.UpperValue - Util.Util.ALTURA_MINIMA_TEXTOS_V04;
        }

        private void tbarLargura_MouseUp(object sender, MouseEventArgs e)
        {
            if (!tbarClicked)
                return;

            tbarClicked = false;

            AtualizarPainel();
        }

        private void tbarLargura_MouseDown(object sender, MouseEventArgs e)
        {
            tbarClicked = true;

            double dblValue;

            // Jump to the clicked location
            dblValue = ((double)e.X / (double)tbarLargura.Width) * fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);//(tbarLargura.Maximum - tbarLargura.Minimum);
            if (dblValue < Util.Util.LARGURA_MINIMA_TEXTOS_V04)
                dblValue = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
            if (dblValue > tbarLargura.Maximum)
                dblValue = tbarLargura.Maximum;
            tbarLargura.Value = Convert.ToInt32(dblValue);

        }

        private void tbarLargura_Scroll(object sender, EventArgs e)
        {
            if (tbarClicked)
                return;

            AtualizarPainel();
        }

        private void cbHabilitaUsoLock_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).ContainsFocus)
            { 
                if (cbHabilitaUsoLock.Checked)
                {
                    MessageBox.Show(this, rm.GetString("ARQUIVO_TEXTO_ALERTA_LOCK"), rm.GetString("ARQUIVO_TITULO_ALERTA_LOCK"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                fachada.HabilitarUsoLock(controladorSelecionado, cbHabilitaUsoLock.Checked);
            }
        }

        private void cbSendNSS_CheckedChanged(object sender, EventArgs e)
        {
            cbPainelNSS.Visible = cbSendNSS.Checked;
            //lbPainelNSS.Visible = cbSendNSS.Checked;
            gboxPainelNss.Visible = cbSendNSS.Checked;
        }

        private void cbHabilitaUsoSenha_CheckedChanged(object sender, EventArgs e)
        {
            fachada.HabilitarUsoSenha(controladorSelecionado, cbHabilitaUsoSenha.Checked);
        }

        private void pbRouteLeft_Click(object sender, EventArgs e)
        {
            habilitarRoute(null != pbRouteLeft.Image);

            SetarFuncoesFachada();
        }

        private void Arquivo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                fachada.RemoveControlador(this.controladorSelecionado);
                ((MDIPrincipal)(this.Parent.Parent)).SincronizaControladores(this.controladorSelecionado);
            }
        }

        private void dataGridViewPaineis_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dataGridViewPaineis.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = dataGridViewPaineis.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if ((e.Effect == DragDropEffects.Move) && (rowIndexOfItemUnderMouseToDrop >= 0) && (rowIndexOfItemUnderMouseToDrop < dataGridViewPaineis.RowCount))
            {
                //mover o painel e seu indice
                fachada.MoverPainel(controladorSelecionado, rowIndexFromMouseDown, rowIndexOfItemUnderMouseToDrop);

                //mover o painelAPP
                fachada.MoverPainelAPP(controladorSelecionado, rowIndexFromMouseDown, rowIndexOfItemUnderMouseToDrop);

                //Popula na GUI
                LimpaDataGrid();
                PopulaPaineisDataGrid(fachada.CarregarPaineis(controladorSelecionado));
                PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));


                dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[rowIndexOfItemUnderMouseToDrop].Cells[0];
                dataGridViewPaineis.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
                cbPainel.SelectedIndex = rowIndexOfItemUnderMouseToDrop;
                SetarPainelSelecionado();
                //DesenharPainel();
            }
        }


        private void dataGridViewPaineis_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridViewPaineis_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dataGridViewPaineis.DoDragDrop(
                    dataGridViewPaineis.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewPaineis_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                rowIndexFromMouseDown = dataGridViewPaineis.HitTest(e.X, e.Y).RowIndex;
                if (rowIndexFromMouseDown != -1)
                {

                    // Remember the point where the mouse down occurred. 
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                                   e.Y - (dragSize.Height / 2)),
                                        dragSize);
                }
                else
                    // Reset the rectangle if the mouse is not over an item in the ListBox.
                    dragBoxFromMouseDown = Rectangle.Empty;
            }


        }

        private void tabControlArquivo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.Shift)
            {
                if (e.KeyCode == Keys.A)
                    habilitarControlesGeracao(true);

                if (e.KeyCode == Keys.S)
                    habilitarControlesGeracao(false);
            }
        }

        private void pbSettingsRight_Click(object sender, EventArgs e)
        {
            habilitarSettings(null != pbSettingsRight.Image, true);

            SetarFuncoesFachada();
        }
        private void cbPainelNSS_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetarPainelAPP();
        }

        private void SetarPainelAPP()
        {
            List<int> painelNSS = new List<int>();
            painelNSS.Add((cbPainelNSS.SelectedIndex == -1) ? 0 : cbPainelNSS.SelectedIndex);
            fachada.SetarPaineisAPP(controladorSelecionado, painelNSS);
        }

        private void pbSelectSign_Click(object sender, EventArgs e)
        {
            habilitarSelectSign(null != pbSelectSign.Image);

            SetarFuncoesFachada();
        }

        private void pbSettingsLeft_Click(object sender, EventArgs e)
        {
            habilitarSettings(null != pbSettingsLeft.Image, true);

            SetarFuncoesFachada();
        }

        private void pbMessageType_Click(object sender, EventArgs e)
        {
            habilitarMessageType(null != pbMessageType.Image);

            SetarFuncoesFachada();
        }

        private void pbMessageLeft_Click(object sender, EventArgs e)
        {
            habilitarMessage(null != pbMessageLeft.Image, true);

            SetarFuncoesFachada();
        }

        private void pbMessageRight_Click(object sender, EventArgs e)
        {
            habilitarMessage(null != pbMessageRight.Image, true);

            SetarFuncoesFachada();
        }

        private void pbRoundTrip_Click(object sender, EventArgs e)
        {
            habilitarRoundTrip(null != pbRoundTrip.Image);

            SetarFuncoesFachada();
        }

        private void pbRouteRight_Click(object sender, EventArgs e)
        {
            //Seta a imagem e marca o check
            habilitarRoute(null != pbRouteRight.Image);

            SetarFuncoesFachada();
        }

        private void clbBloqueioFuncoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bool[] funcoesBloquadas = new bool[clbBloqueioFuncoes.Items.Count + 1];

            //foreach (int f in clbBloqueioFuncoes.CheckedIndices)
            //{
            //    // Este if serve para pular a função de escolha da região que será bloqueada
            //    if (f > 6)
            //    {
            //        funcoesBloquadas[f + 1] = true;
            //    }
            //    else
            //    {
            //        funcoesBloquadas[f] = true;
            //    }
            //}

            //fachada.EditarBloqueioFuncoes(controladorSelecionado, funcoesBloquadas);

            SetarFuncoesFachada();

            habilitarRoute(!clbBloqueioFuncoes.GetItemChecked(bloqueio_roteiro));
            habilitarMessage(!TodosItensMensagensChecados(), false);
            habilitarRoundTrip(!clbBloqueioFuncoes.GetItemChecked(bloqueio_ida_volta));
            habilitarMessageType(!clbBloqueioFuncoes.GetItemChecked(bloqueio_alterna_roteiro));
            habilitarSelectSign(!clbBloqueioFuncoes.GetItemChecked(bloqueio_seleciona_painel));
            habilitarSettings(!TodosItensSettingsChecados(), false);

        }

        private void SetarFuncoesFachada()
        {
            bool[] funcoesBloquadas = new bool[clbBloqueioFuncoes.Items.Count + 1];

            foreach (int f in clbBloqueioFuncoes.CheckedIndices)
            {
                // Este if serve para pular a função de escolha da região que será bloqueada
                if (f > 6)
                {
                    funcoesBloquadas[f + 1] = true;
                }
                else
                {
                    funcoesBloquadas[f] = true;
                }
            }

            fachada.EditarBloqueioFuncoes(controladorSelecionado, funcoesBloquadas);
        }

        private void tbAntiTheft_TextChanged(object sender, EventArgs e)
        {
            fachada.EditarSenhaAntiRoubo(controladorSelecionado, tbAntiTheft.Text);
        }

        private void tbSpecialAccess_TextChanged(object sender, EventArgs e)
        {
            fachada.EditarSenhaAcessoEspecial(controladorSelecionado, tbSpecialAccess.Text);
        }

        private void tbAntiTheft_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }

        private void tbSpecialAccess_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }
        #endregion

        #region AlterarGUI

        private void habilitarRoute(bool habilita)
        {
            if (!pbControlador.Controls.Contains(pbRouteLeft))
                pbControlador.Controls.Add(pbRouteLeft);

            if (!pbControlador.Controls.Contains(pbRouteRight))
                pbControlador.Controls.Add(pbRouteRight);

            if (habilita)
            {
                pbRouteLeft.Image = (Image)null;
                pbRouteRight.Image = (Image)null;
            }
            else
            {
                pbRouteLeft.Image = (Image)global::PontosX2.Properties.Resources.xis;
                pbRouteRight.Image = (Image)global::PontosX2.Properties.Resources.xis;
            }

            clbBloqueioFuncoes.SetItemChecked(bloqueio_roteiro, !habilita);

            //Posicionamento das imagens de acordo com a lingua selecionada se a imagem X estiver sendo exibida
            if (pbRouteLeft.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbRouteLeft.Location = new Point(59, 193);
                else
                    pbRouteLeft.Location = new Point(59, 195);

            if (pbRouteRight.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbRouteRight.Location = new Point(119, 193);
                else
                    pbRouteRight.Location = new Point(120, 195);

            pbRouteLeft.BackColor = Color.Transparent;
            pbRouteRight.BackColor = Color.Transparent;
        }

        private void habilitarMessage(bool habilita, bool setarChecks)
        {
            if (!pbControlador.Controls.Contains(pbMessageLeft))
                pbControlador.Controls.Add(pbMessageLeft);

            if (!pbControlador.Controls.Contains(pbMessageRight))
                pbControlador.Controls.Add(pbMessageRight);

            if (habilita)
            {
                pbMessageLeft.Image = (Image)null;
                pbMessageRight.Image = (Image)null;
            }
            else
            {
                pbMessageLeft.Image = (Image)global::PontosX2.Properties.Resources.xis;
                pbMessageRight.Image = (Image)global::PontosX2.Properties.Resources.xis;
            }

            if (setarChecks)
            {
                clbBloqueioFuncoes.SetItemChecked(bloqueio_mensagem, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_mens_secundaria, !habilita);
            }

            //Posicionamento das imagens de acordo com a lingua selecionada se a imagem X estiver sendo exibida
            if (pbMessageLeft.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbMessageLeft.Location = new Point(330, 193);
                else
                    pbMessageLeft.Location = new Point(328, 195);

            if (pbMessageRight.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbMessageRight.Location = new Point(393, 193);
                else
                    pbMessageRight.Location = new Point(389, 195);


            pbMessageLeft.BackColor = Color.Transparent;
            pbMessageRight.BackColor = Color.Transparent;
        }

        private void InicializarTeclado()
        {
            habilitarRoute(true);
            habilitarMessage(true, true);
            habilitarRoundTrip(true);
            habilitarMessageType(true);
            habilitarSelectSign(true);
            habilitarSettings(true, true);
        }

        private void habilitarRoundTrip(bool habilita)
        {
            if (!pbControlador.Controls.Contains(pbRoundTrip))
                pbControlador.Controls.Add(pbRoundTrip);
            
            if (habilita)
                pbRoundTrip.Image = (Image)null;
            else
                pbRoundTrip.Image = (Image)global::PontosX2.Properties.Resources.xis;

            clbBloqueioFuncoes.SetItemChecked(bloqueio_ida_volta, !habilita);

            //Posicionamento das imagens de acordo com a lingua selecionada se a imagem X estiver sendo exibida
            if (pbRoundTrip.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbRoundTrip.Location = new Point(183, 193);
                else
                    pbRoundTrip.Location = new Point(183, 195);

            pbRoundTrip.BackColor = Color.Transparent;
        }

        private void habilitarMessageType(bool habilita)
        {
            if (!pbControlador.Controls.Contains(pbMessageType))
                pbControlador.Controls.Add(pbMessageType);

            if (habilita)
                pbMessageType.Image = (Image)null;
            else
                pbMessageType.Image = (Image)global::PontosX2.Properties.Resources.xis;

            clbBloqueioFuncoes.SetItemChecked(bloqueio_alterna_roteiro, !habilita);

            //Posicionamento das imagens de acordo com a lingua selecionada se a imagem X estiver sendo exibida
            if (pbMessageType.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbMessageType.Location = new Point(259, 193);
                else
                    pbMessageType.Location = new Point(257, 195);

            pbMessageType.BackColor = Color.Transparent;
        }

        private void habilitarSelectSign(bool habilita)
        {
            if (!pbControlador.Controls.Contains(pbSelectSign))
                pbControlador.Controls.Add(pbSelectSign);

            if (habilita)
                pbSelectSign.Image = (Image)null;
            else
                pbSelectSign.Image = (Image)global::PontosX2.Properties.Resources.xis;

            clbBloqueioFuncoes.SetItemChecked(bloqueio_seleciona_painel, !habilita);

            //Posicionamento das imagens de acordo com a lingua selecionada se a imagem X estiver sendo exibida
            if (pbSelectSign.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbSelectSign.Location = new Point(463, 193);
                else
                    pbSelectSign.Location = new Point(461, 195);

            pbSelectSign.BackColor = Color.Transparent;
        }

        private void habilitarSettings(bool habilita, bool setarChecks)
        {
            if (!pbControlador.Controls.Contains(pbSettingsLeft))
                pbControlador.Controls.Add(pbSettingsLeft);

            if (!pbControlador.Controls.Contains(pbSettingsRight))
                pbControlador.Controls.Add(pbSettingsRight);

            if (habilita)
            {
                pbSettingsLeft.Image = (Image)null;
                pbSettingsRight.Image = (Image)null;
            }
            else
            {
                pbSettingsLeft.Image = (Image)global::PontosX2.Properties.Resources.xis;
                pbSettingsRight.Image = (Image)global::PontosX2.Properties.Resources.xis;
            }

            if (setarChecks)
            {
                clbBloqueioFuncoes.SetItemChecked(bloqueio_ajusta_hora_saida, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_ajusta_relogio, !habilita);
                //clbBloqueioFuncoes.SetItemChecked(bloqueio_identifica_painel, !habilita);
                //clbBloqueioFuncoes.SetItemChecked(bloqueio_nova_config, !habilita);
                //clbBloqueioFuncoes.SetItemChecked(bloqueio_coleta_dump, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_coleta_log, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_apagar_config, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_acender_paineis, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_config_fabrica, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_modo_teste, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_ajuste_brilho, !habilita);
                clbBloqueioFuncoes.SetItemChecked(bloqueio_seleciona_motorista, !habilita);
            }

            //Posicionamento das imagens de acordo com a lingua selecionada se a imagem X estiver sendo exibida
            if (pbSettingsLeft.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbSettingsLeft.Location = new Point(414, 73);
                else
                    pbSettingsLeft.Location = new Point(405, 77); 

            if (pbSettingsRight.Image != (Image)null)
                if (fachada.GetIdiomaFachada() == Util.Util.Lingua.Ingles)
                    pbSettingsRight.Location = new Point(473, 73);
                else
                    pbSettingsRight.Location = new Point(470, 77); 

            pbSettingsLeft.BackColor = Color.Transparent;
            pbSettingsRight.BackColor = Color.Transparent;
        }

        public int GetTabControlSelecionado()
        {
            return tabControlArquivo.SelectedIndex;
        }

        private bool ValidaLargura()
        {
            try
            {
                System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value);
                if (System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value) > 256 || System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value) < 16)
                {
                    MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MBOX_LARGURA_INVALIDA"));
                    return false;
                }

                return true;
            }
            catch
            {
                MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MBOX_LARGURA_INVALIDA"));
                return false;

            }
        }

        private bool ValidaBrilhoMin()
        {
            try
            {
                System.Convert.ToByte(dataGridViewPaineis.CurrentCell.Value);
                if (System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value) < 1 || System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value) > 100)
                {
                    MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MBOX_BRILHO_NUMERO"));
                    return false;
                }

                byte brilhoMax = fachada.GetBrilhoMaxPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);
                if (System.Convert.ToByte(dataGridViewPaineis.CurrentCell.Value) > brilhoMax)
                {
                    MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MBOX_BRILHO_MENOR"));
                    return false;
                }

                return true;
            }
            catch
            {
                MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MBOX_BRILHO_NUMERO"));
                return false;

            }
        }

        private bool ValidaBrilhoMax()
        {
            try
            {
                System.Convert.ToByte(dataGridViewPaineis.CurrentCell.Value);
                if (System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value) < 1 || System.Convert.ToInt16(dataGridViewPaineis.CurrentCell.Value) > 100)
                {
                    MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MBOX_BRILHO_NUMERO"));
                    return false;
                }

                byte brilhoMin = fachada.GetBrilhoMinPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);
                if (System.Convert.ToByte(dataGridViewPaineis.CurrentCell.Value) < brilhoMin)
                {
                    MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MBOX_BRILHO_MAIOR"));
                    return false;
                }

                return true;
            }
            catch
            {
                MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MBOX_BRILHO_NUMERO"));
                return false;

            }
        }

        private void LimpaDataGrid()
        {
            dataGridViewPaineis.Rows.Clear();
        }

        public void CarregarRegioes()
        {
            List<String> rgns = null;

            rgns = fachada.ListaRgns();

            if (rgns != null)
            {
                if (rgns.Count > 0)
                {
                    cbRegiao.Items.Clear();
                    cbRegiao.Items.AddRange(rgns.ToArray());
                }
            }

        }

        private void travarUserControl(bool travar, bool enviandoNFX)
        {
            if (travar)
            {
                ultimaSituacaoPainel = cbPainel.Enabled;                
                cbPainel.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                tbarAltura.Enabled = false;
                tbarLargura.Enabled = false;
                rangeAltura.Enabled = false;
                if (enviandoNFX)
                {
                    painelPerifecos.Enabled = false;
                    gbLockKeyboard.Enabled = false;
                    gbLockFunctions.Enabled = false;
                    gbPasswords.Enabled = false;
                }
                else
                {
                    tabControlArquivo.Enabled = false;
                }


            }
            else
            {
                cbPainel.Enabled = ultimaSituacaoPainel;
               
                tbarAltura.Enabled = true;
                tbarLargura.Enabled = true;
                rangeAltura.Enabled = true;
                if (enviandoNFX)
                {
                    painelPerifecos.Enabled = true;
                    gbLockKeyboard.Enabled = true;
                    gbLockFunctions.Enabled = true;
                    gbPasswords.Enabled = true;
                }
                else
                {
                    tabControlArquivo.Enabled = true;
                }
            }

        }

        #endregion

        #region Botoes

        private void btAvancado_Click(object sender, EventArgs e)
        {
            if (dataGridViewPaineis.Columns[colunaBrilhoMin].Visible)
            {
                dataGridViewPaineis.Columns[colunaBrilhoMin].Visible = false;
                dataGridViewPaineis.Columns[colunaBrilhoMax].Visible = false;
                dataGridViewPaineis.Columns[colunaMultiLinhas].Visible = false;
                btAvancado.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_BUTTON_OPCOES_1");
                btAvancado.Image = Properties.Resources.zoom_in;
            }
            else
            {
                dataGridViewPaineis.Columns[colunaBrilhoMin].Visible = true;
                dataGridViewPaineis.Columns[colunaBrilhoMax].Visible = true;
                dataGridViewPaineis.Columns[colunaMultiLinhas].Visible = true;
                btAvancado.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_BUTTON_OPCOES_2");
                btAvancado.Image = Properties.Resources.zoom_out;
            }
        }

        private void btGerar_Click(object sender, EventArgs e)
        {
            try
            {
                //Alterando o cursor
                CursorWait();
                
                //Gerando configuração
                int qtdPaineis = Convert.ToInt32(tbPaineis.Text);
                int qtdRoteiros = Convert.ToInt32(tbRoteiros.Text);
                int qtdMensagens = Convert.ToInt32(tbMensagens.Text);
                int qtdMotoristas = Convert.ToInt32(tbMotoristas.Text);
                fachada.GerarConfig(controladorSelecionado, qtdPaineis, qtdRoteiros, qtdMensagens, qtdMotoristas);

                //Populando a GUI.
                painelSelecionado = 0;
                PopulaPaineisDataGrid(fachada.CarregarPaineis(controladorSelecionado));
                PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));

                //Setando o painel nas user control
                SetarPainelSelecionado();

                //Seleciona o painel no dbgridview 
                dataGridViewPaineis.Rows[painelSelecionado].Selected = true;
                dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[painelSelecionado].Cells[0];

                //setando tamanho do componente VideoBitmap do painel principal
                DesenharPainel();

                //Carregar Roteiros do Painel Principal
                CarregarRoteirosPainel();

                //Carregar Mensagens do Painel Principal
                CarregarMensagensPainel();

                //Carrega regiões
                CarregarRegioes();

                //Setando a região selecionada
                exibirMboxRegiao = false;
                cbRegiao.SelectedIndex = cbRegiao.Items.IndexOf(fachada.GetNomeRegiao(controladorSelecionado));

                //Desabilitando o teste de carga
                habilitarControlesGeracao(false);

                //Cursor Normal
                CursorDefault();


            } catch (Exception ex) {

                MessageBox.Show(ex.Message);
                CursorDefault();
            }
        }


        private void ApresentarPainel()
        {
            travarUserControl(true, false);

            CancelarApresentacao = false;

            listaFrasesApresentacao = null;

            //Limpando o painel
            for (int i = 0; i < listaVideoBitMap.Count; i++)
                listaVideoBitMap[i].Apaga(0, Color.Black);


            //Se estiver na tela de edição do roteiro
            if (EditorRoteiros.Visible)
            {
                //Se não houver nenhum item da lista selecionado, deverá listar todos os textos
                if (!EditorRoteiros.ItemSelecionadoLista() && fraseGUI == null)
                {
                    listaFrasesApresentacao = EditorRoteiros.ExibirTodosTextos();

                    foreach (Frase f in listaFrasesApresentacao)
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), f);
                }
            }

            //Se estiver na tela de edição de mensagens
            if (EditorMensagens.Visible)
            {
                if (!EditorMensagens.ItemSelecionadoLista() && fraseGUI == null)
                {
                    listaFrasesApresentacao = EditorMensagens.ExibirTodosTextos();

                    foreach (Frase f in listaFrasesApresentacao)
                        fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), f);
                }
            }

            int indice = 0;

            while (!CancelarApresentacao)
            {

                //Se não houver nenhum item da lista selecionado, deverá listar todos os textos
                if (listaFrasesApresentacao != null)
                {
                    if (indice == listaFrasesApresentacao.Count)
                        indice = 0;

                    MethodInvoker invoker = delegate
                    {
                        ReposicionarVideoMapNoPainel(listaFrasesApresentacao[indice]);
                    };

                    this.Invoke(invoker);

                    if (EditorRoteiros.Visible)
                        EditorRoteiros.SelecionarItemListView(indice);

                    if (EditorMensagens.Visible)
                        EditorMensagens.SelecionarItemListView(indice);

                    indice = indice + 1;

                }

                if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
                {
                    listaVideoBitMap[0].Cancelar = false;
                    backgroundApresentacao1.RunWorkerAsync();
                }

                if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V04)
                {
                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
                    {
                        listaVideoBitMap[0].Cancelar = false;
                        backgroundApresentacao1.RunWorkerAsync();
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
                    {
                        listaVideoBitMap[0].Cancelar = false;
                        backgroundApresentacao1.RunWorkerAsync();

                        listaVideoBitMap[1].Cancelar = false;
                        backgroundApresentacao2.RunWorkerAsync();
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
                    {
                        listaVideoBitMap[0].Cancelar = false;
                        backgroundApresentacao1.RunWorkerAsync();

                        listaVideoBitMap[1].Cancelar = false;
                        backgroundApresentacao2.RunWorkerAsync();

                        listaVideoBitMap[2].Cancelar = false;
                        backgroundApresentacao3.RunWorkerAsync();
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
                    {
                        listaVideoBitMap[0].Cancelar = false;
                        backgroundApresentacao1.RunWorkerAsync();

                        listaVideoBitMap[1].Cancelar = false;
                        backgroundApresentacao2.RunWorkerAsync();

                        listaVideoBitMap[2].Cancelar = false;
                        backgroundApresentacao3.RunWorkerAsync();

                        listaVideoBitMap[3].Cancelar = false;
                        backgroundApresentacao4.RunWorkerAsync();
                    }
                }

                while (backgroundApresentacao1.IsBusy || backgroundApresentacao2.IsBusy || backgroundApresentacao3.IsBusy || backgroundApresentacao4.IsBusy)
                {
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
            }

        }

        private void CancelarApresentacaoPainel()
        {
            CancelarApresentacao = true;

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
            {
                listaVideoBitMap[0].Cancelar = true;
                backgroundApresentacao1.CancelAsync();
            }

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V04)
            {
                if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
                {
                    listaVideoBitMap[0].Cancelar = true;
                    backgroundApresentacao1.CancelAsync();
                }

                if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
                {
                    listaVideoBitMap[0].Cancelar = true;
                    backgroundApresentacao1.CancelAsync();

                    listaVideoBitMap[1].Cancelar = true;
                    backgroundApresentacao2.CancelAsync();
                }

                if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
                {
                    listaVideoBitMap[0].Cancelar = true;
                    backgroundApresentacao1.CancelAsync();

                    listaVideoBitMap[1].Cancelar = true;
                    backgroundApresentacao2.CancelAsync();

                    listaVideoBitMap[2].Cancelar = true;
                    backgroundApresentacao3.CancelAsync();
                }

                if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
                {
                    listaVideoBitMap[0].Cancelar = true;
                    backgroundApresentacao1.CancelAsync();

                    listaVideoBitMap[1].Cancelar = true;
                    backgroundApresentacao2.CancelAsync();

                    listaVideoBitMap[2].Cancelar = true;
                    backgroundApresentacao3.CancelAsync();

                    listaVideoBitMap[3].Cancelar = true;
                    backgroundApresentacao4.CancelAsync();
                }
            }

            if (listaFrasesApresentacao == null)
            {
                PreencherBitMap(fraseGUI);
            }
            else
            {
                DesenharPainel();

                if (EditorRoteiros.Visible)
                {
                    EditorRoteiros.DesmarcarListViews();
                    EditorRoteiros.HabilitarBotaoApresentacao();
                }

                if (EditorMensagens.Visible)
                {
                    EditorMensagens.DesmarcarListViews();
                    EditorMensagens.HabilitarBotaoApresentacao();
                }

            }

            travarUserControl(false, false);
        }

        private void ApresentarPainelMultiLinhas()
        {
            Forms.Simulacao.FormSimulacao fs = new Forms.Simulacao.FormSimulacao();

            //Se estiver na tela de edição do roteiro
            if (EditorRoteiros.Visible)
            {
                indiceSelecionadoListaRoteiros = EditorRoteiros.IndiceSelecionadoLista();
                
                //Se não houver nenhum item da lista selecionado, deverá listar todos os textos
                if (indiceSelecionadoListaRoteiros == -1)
                {
                    fs.listaFrasesRoteiroPainelMultilinhas = EditorRoteiros.ExibirTodosTextos();
                    EditorRoteiros.SelecionarItemListView(0);
                }
                else
                    fs.listaFrasesRoteiroPainelMultilinhas.Add(EditorRoteiros.FraseSelecionadaRoteiroGUI());
            }

            //Se estiver na tela de edição dos textos do roteiro
            if (TextosEditor.Visible)
            {
                fs.listaFrasesRoteiroPainelMultilinhas.Add(TextosEditor.fraseGUI);
            }

            fs.controladorSelecionado = this.controladorSelecionado;
            fs.painelSelecionado = painelSelecionado;
            fs.isSimulacao = false;

            fs.ShowDialog(this);
        }

        public void SelecionarItemListRoteiros(int indice)
        {
            EditorRoteiros.SelecionarItemListView(indice);
        }

        public void CancelarApresentacaoPainelMultiLinhas()
        {
            btnApresentar.Text = rm.GetString("ARQUIVO_BTN_APRESENTAR_EXIBIR");

            if (EditorRoteiros.Visible)
            {
                if (indiceSelecionadoListaRoteiros == -1)
                    EditorRoteiros.DesmarcarListViews();
            }

        }

        private void btnApresentar_Click(object sender, EventArgs e)
        {
            var lista = fachada.CarregarListaFirmwares();
            if (btnApresentar.Text != rm.GetString("ARQUIVO_BTN_APRESENTAR_CANCELAR"))
            {
                btnApresentar.Text = rm.GetString("ARQUIVO_BTN_APRESENTAR_CANCELAR");

                if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                    ApresentarPainel();
                else
                    ApresentarPainelMultiLinhas();
            }

            else
            {
                btnApresentar.Text = rm.GetString("ARQUIVO_BTN_APRESENTAR_EXIBIR");

                if (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) == 1)
                    CancelarApresentacaoPainel();
                else
                    CancelarApresentacaoPainelMultiLinhas();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ultimoIndexPainelExibido = ultimoIndexPainelExibido + 1;

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
            {
                listaVideoBitMap[0].Bitmap = (Color[,])listaVideoBitMap[0].listaBitMap[ultimoIndexPainelExibido].Clone();


                if (listaVideoBitMap[0].listaBitMap.Count - 1 == ultimoIndexPainelExibido)
                    button2.Enabled = false;
                else
                    button2.Enabled = true;

                button1.Enabled = true;
            }

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V04)
            {
                int maiorIndice = 0;
                for (int i = 0; i < listaVideoBitMap.Count; i++)
                {
                    if (ultimoIndexPainelExibido <= listaVideoBitMap[i].listaBitMap.Count - 1)
                        listaVideoBitMap[i].Bitmap = (Color[,])listaVideoBitMap[i].listaBitMap[ultimoIndexPainelExibido].Clone();
                    else
                        listaVideoBitMap[i].Bitmap = (Color[,])listaVideoBitMap[i].listaBitMap[listaVideoBitMap[i].listaBitMap.Count - 1].Clone();

                    if (listaVideoBitMap[i].listaBitMap.Count > maiorIndice)
                        maiorIndice = listaVideoBitMap[i].listaBitMap.Count;
                }

                if (maiorIndice - 1 == ultimoIndexPainelExibido)
                    button2.Enabled = false;
                else
                    button2.Enabled = true;

                button1.Enabled = true;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ultimoIndexPainelExibido = ultimoIndexPainelExibido - 1;

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
            {
                listaVideoBitMap[0].Bitmap = (Color[,])listaVideoBitMap[0].listaBitMap[ultimoIndexPainelExibido].Clone();
            }

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V04)
            {
                for (int i = 0; i < listaVideoBitMap.Count; i++)
                {
                    if (ultimoIndexPainelExibido <= listaVideoBitMap[i].listaBitMap.Count - 1)
                        listaVideoBitMap[i].Bitmap = (Color[,])listaVideoBitMap[i].listaBitMap[ultimoIndexPainelExibido].Clone();
                    else
                        listaVideoBitMap[i].Bitmap = (Color[,])listaVideoBitMap[i].listaBitMap[listaVideoBitMap[i].listaBitMap.Count - 1].Clone();
                }
            }

            if (ultimoIndexPainelExibido == 0)
                button1.Enabled = false;
            else
                button1.Enabled = true;

            button2.Enabled = true;
        }

        private void btnExcluirPainel_Click(object sender, EventArgs e)
        {
            RemoverPainel();
        }

        private void RemoverPainel()
        {
            try
            {
                if (dataGridViewPaineis.RowCount > 1 && dataGridViewPaineis.SelectedRows.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_EXCLUIR_PAINEL"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        CursorWait();

                        ExcluirPainel();

                        CursorDefault();
                    }
                }
                else
                    MessageBox.Show(rm.GetString("ARQUIVO_MBOX_EXCLUIR_PAINEL_ULTIMO"));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                CursorDefault();
            }
        }

        private void btnIncluirPainel_Click(object sender, EventArgs e)
        {
            AdicionarPainel();
        }

        private void AdicionarPainel()
        {
            try
            {
                if (fachada.QuantidadePaineis(controladorSelecionado) < 16)
                {
                    CursorWait();

                    IncluirPainel();
                    dataGridViewPaineis.Rows[dataGridViewPaineis.RowCount - 1].Selected = true;
                    dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[dataGridViewPaineis.RowCount - 1].Cells[0];
                    cbPainel.SelectedIndex = dataGridViewPaineis.RowCount - 1;
                    SetarPainelSelecionado();
                    //setando tamanho do componente VideoBitmap do painel principal
                    DesenharPainel();

                    CursorDefault();
                }
                else
                {
                    MessageBox.Show(rm.GetString("ARQUIVO_MBOX_MAXIMO_DE_PAINEIS"));
                }
            }
            catch
            {
                CursorDefault();
            }
        }

        private void EnviarNFX()
        {

            //Setando flag de cancelamento da geração do arquivo NFX
            fachada.SetCancelarTransmissaoNFX(controladorSelecionado, false);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (!cbSendNSS.Visible)
                cbSendNSS.Checked = false;

            if (Util.Util.IdentificarDispositivoUSB() == string.Empty)
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            else
                saveFileDialog.InitialDirectory = Util.Util.IdentificarDispositivoUSB();

            saveFileDialog.Filter = rm.GetString("ARQUIVO_TABPAGE_TRANSMISSAO_ARQUIVO_BINARIO");
            saveFileDialog.OverwritePrompt = false;

            if ((cbSendNSS.Visible) && (cbSendNSS.Checked))
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

                if (Util.Util.IdentificarDispositivoUSB() == string.Empty)
                    folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                else
                    folderBrowserDialog.SelectedPath = Util.Util.IdentificarDispositivoUSB();

                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string FileName = folderBrowserDialog.SelectedPath + Util.Util.DIRETORIO_NSS + Util.Util.ARQUIVO_NSS;

                    // Cria o diretório LDx2 para uso do firmware;
                    if (!Directory.Exists(FileName.Replace(Path.GetFileName(FileName), String.Empty)))
                    {
                        Directory.CreateDirectory(FileName.Replace(Path.GetFileName(FileName), String.Empty));
                    }


                    if (File.Exists(FileName))
                    {
                        if (MessageBox.Show(this, Path.GetFileName(FileName) + " " + rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_MBOX_SALVAR_SOBRESCREVER"), rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_MBOX_TYPE_SALVAR_SOBRESCREVER"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                        {
                            labelCaminhoUsuario.Text = FileName;
                            labelCaminhoNFS.Visible = true;

                            //chamando o backgroundWorker (ira gerar o nandfs e atualizar o progressBar)
                            background.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        labelCaminhoUsuario.Text = FileName;
                        labelCaminhoNFS.Visible = true;

                        //chamando o backgroundWorker (ira gerar o nandfs e atualizar o progressBar)
                        background.RunWorkerAsync();
                    }
                }

                return;
            }


            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string nomeArquivo = Path.GetFileName(saveFileDialog.FileName);
                if (nomeArquivo.Length > 12)
                {
                    //Se o nome do arquivo for maior que 8 caracteres exibir mensagem ao usuário. Exibe ruim no display do controlador.
                    MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_MBOX_TAMANHO_ARQUIVO"));
                    EnviarNFX();
                }
                else
                {
                    // Cria o diretório LDx2 para uso do firmware;
                    if (!Directory.Exists(saveFileDialog.FileName.Replace(Path.GetFileName(saveFileDialog.FileName), String.Empty) + Util.Util.DIRETORIO_APP_NFX))
                    {
                        Directory.CreateDirectory(saveFileDialog.FileName.Replace(Path.GetFileName(saveFileDialog.FileName), String.Empty) + Util.Util.DIRETORIO_APP_NFX);
                    }

                    saveFileDialog.FileName = saveFileDialog.FileName.Replace(Path.GetFileName(saveFileDialog.FileName), String.Empty) + Util.Util.DIRETORIO_APP_NFX + Path.GetFileName(saveFileDialog.FileName);

                    if (File.Exists(saveFileDialog.FileName))
                    {
                        if (MessageBox.Show(this, Path.GetFileName(saveFileDialog.FileName) + " " + rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_MBOX_SALVAR_SOBRESCREVER"), rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_MBOX_TYPE_SALVAR_SOBRESCREVER"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                        {
                            //string FileName = saveFileDialog.FileName;
                            labelCaminhoUsuario.Text = saveFileDialog.FileName;
                            labelCaminhoNFS.Visible = true;

                            //chamando o backgroundWorker (ira gerar o nandfs e atualizar o progressBar)
                            background.RunWorkerAsync();                            
                        }
                    }
                    else
                    {
                        //string FileName = saveFileDialog.FileName;
                        labelCaminhoUsuario.Text = saveFileDialog.FileName;
                        labelCaminhoNFS.Visible = true;

                        //chamando o backgroundWorker (ira gerar o nandfs e atualizar o progressBar)
                        background.RunWorkerAsync();                        
                    }
                }
            }
        }


        private void btnSalvar_Click(object sender, EventArgs e)
        {
                //salvar arquivo NFX
                if (btnSalvar.Text == rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_BTN_ENVIAR"))
                {
                    if (VerificarFuncoesBloqueadas())
                        EnviarNFX();
                }
                else
                    CancelarNFX();

        }

        private bool VerificarFuncoesBloqueadas()
        {
            bool verificado = true;

            if (clbBloqueioFuncoes.CheckedIndices.Count > 0 && tbSpecialAccess.Text == "")
            {
                MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_MBOX_VERIFICAR_FUNCOES1"));
                tbSpecialAccess.Focus();
                return false;
            }

            if (clbBloqueioFuncoes.CheckedIndices.Count > 0 && tbSpecialAccess.Text.Length < tbSpecialAccess.MaxLength)
            {
                MessageBox.Show(rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_MBOX_VERIFICAR_FUNCOES2") +" " + tbSpecialAccess.MaxLength + " "+ rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_MBOX_VERIFICAR_FUNCOES3"));
                tbSpecialAccess.Focus();
                return false;
            }

            return verificado;
        }

        private void CancelarNFX()
        {
            CursorWait();

            //Setando flag de cancelamento da geração do arquivo NFX
            fachada.SetCancelarTransmissaoNFX(controladorSelecionado, true);
        }

        public void HabilitarBtnApresentacao(bool habilitar)
        {
            btnApresentar.Enabled = habilitar;
        }

        #endregion

        #region Mensagens

        private void CarregarMensagensPainel()
        {
            //Carregar os Objetos de Mensagens da GUI no flexgrid do user control ListarMensagens
            listarMensagens1.LimparGridMensagens();
            listarMensagens1.PopulaMensagensGrid(fachada.CarregarMensagens(listarMensagens1.controladorSelecionado, listarMensagens1.painelSelecionado));
        }

        public int LocalizarMensagem(int startIndex, string texto, bool matchWholeWord, bool ignoreCase, bool findTexts)
        {
            int indice = -1;
            indice = fachada.LocalizarMensagens(controladorSelecionado, painelSelecionado, startIndex, texto, matchWholeWord, ignoreCase, findTexts);

            listarMensagens1.collapseOrExpandeAll(false);
            if (indice >= 0)
                listarMensagens1.SelecionarMensagem(indice, true);
            else             
                listarMensagens1.DeselectGrid();                

            return indice;
        }

        public int QuantidadeMensagens()
        {
            return fachada.QuantidadeMensagens(controladorSelecionado, painelSelecionado);
        }
        #endregion

        #region Roteiro

        public int LocalizarRoteiro(int startIndex, string texto, bool matchWholeWord, bool ignoreCase, bool findTexts)
        {
            int indice = -1;
            indice = fachada.LocalizarRoteiros(controladorSelecionado, painelSelecionado, startIndex ,texto, matchWholeWord, ignoreCase, findTexts);

            listarRoteiros1.collapseOrExpandeAll(false);
            if (indice >= 0)
                listarRoteiros1.SelecionarRoteiro(indice, true);
            else
                listarRoteiros1.DeselectGrid();

            return indice;
        }

        private void CarregarRoteirosPainel()
        {
            //Carregar os Objetos de Roteiros da GUI no flexgrid do user control ListarRoteiros
            listarRoteiros1.LimparGridRoteiros();
            listarRoteiros1.PopulaRoteirosGrid(fachada.CarregarRoteiros(listarRoteiros1.controladorSelecionado, listarRoteiros1.painelSelecionado));
        }

        public int QuantidadeRoteiros()
        {
            return fachada.QuantidadeRoteiros(controladorSelecionado, painelSelecionado);
        }

        public void AtualizarRotasAPP()
        {
            if (listarRoteiros1.Visible)
            {
                listarRoteiros1.LimparGridRoteiros();
                listarRoteiros1.PopulaRoteirosGrid(fachada.CarregarRoteiros(listarRoteiros1.controladorSelecionado, listarRoteiros1.painelSelecionado));
                listarRoteiros1.SelecionarRoteiro(listarRoteiros1.roteiroSelecionado, false);
            }
        }

        #endregion

        #region Geracao Automatica de Configuração

        private void habilitarControlesGeracao(bool habilita)
        {
            tbPaineis.Visible = habilita;
            tbMensagens.Visible = habilita;
            tbRoteiros.Visible = habilita;
            tbMotoristas.Visible = habilita;
            lbPaineis.Visible = habilita;
            lbRoteiros.Visible = habilita;
            lbMensagens.Visible = habilita;
            lbMotoristas.Visible = habilita;
            btGerar.Visible = habilita;

        }

        #endregion

        #region Globalizacao

        public void AplicaIdioma(bool alteracaoIdioma)
        {

            this.btnApresentar.Text = rm.GetString("ARQUIVO_BTN_APRESENTAR_EXIBIR");
            this.tabPageArquivo.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO");
            this.labelCaminho.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_LABEL_CAMINHO");
            this.labelConfiguracaoPaineis.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_LABEL_CONFIGURACAO_PAINEIS");
            this.labelRegiao.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_LABEL_REGIAO");
            this.dataGridViewPaineis.Columns[0].HeaderText = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_DATAGRID_COLUNA_PAINEIS");
            this.dataGridViewPaineis.Columns[1].HeaderText = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_DATAGRID_COLUNA_ALTURA");
            this.dataGridViewPaineis.Columns[2].HeaderText = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_DATAGRID_COLUNA_LARGURA");
            this.dataGridViewPaineis.Columns[3].HeaderText = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_DATAGRID_COLUNA_BRILHO_MIN");
            this.dataGridViewPaineis.Columns[4].HeaderText = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_DATAGRID_COLUNA_BRILHO_MAX");
            this.dataGridViewPaineis.Columns[5].HeaderText = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_DATAGRID_COLUNA_MULTI_LINHAS");
            this.btnIncluirPainel.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_BUTTON_INCLUIR");
            this.btnExcluirPainel.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_BUTTON_REMOVER");
            this.btnCopiarPainel.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_BUTTON_COPIAR");
            if (dataGridViewPaineis.Columns[colunaBrilhoMin].Visible)
                this.btAvancado.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_BUTTON_OPCOES_2");
            else
                this.btAvancado.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_BUTTON_OPCOES_1");
            this.toolStripMenuIncluir.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MENU_INCLUIR");
            this.toolStripMenuExcluir.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MENU_EXCLUIR");
            this.toolStripMenuCopiar.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_MENU_COPIAR");


            this.tabPageRoteiro.Text = rm.GetString("ARQUIVO_TABPAGE_ROTEIRO");
            this.tabPageMensagem.Text = rm.GetString("ARQUIVO_TABPAGE_MENSAGEM");
            this.tabPageMsgEspeciais.Text = rm.GetString("ARQUIVO_TABPAGE_MENSAGENSESPECIAIS");
            this.tabPageSimulacao.Text = rm.GetString("ARQUIVO_TABPAGE_SIMULACAO");
            this.tabPageAgendamento.Text = rm.GetString("ARQUIVO_TABPAGE_AGENDAMENTO");
            this.tabPageMotorista.Text = rm.GetString("ARQUIVO_TABPAGE_MOTORISTA");

            this.tabPageTransmissao.Text = rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR");
            this.labelCaminhoNFS.Text = rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_CAMINHO");
            this.groupBoxTrnasmitir.Text = rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_GROUP_TRANSMITIR");
            this.btnSalvar.Text = rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_BTN_ENVIAR");
            this.pbControlador.Image = (Image)rm.GetObject("controlador");
            this.pbControlador.SizeMode = PictureBoxSizeMode.StretchImage;

            this.cbSendNSS.Text = rm.GetString("SEND_NSS");
            this.gboxPainelNss.Text = rm.GetString("LABEL_DEFINE_NSS");
            this.chkPerifericos.Text = rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_CHECK_PERIFERICOS");
            this.chkHabilitaModoLD6.Text = rm.GetString("TEXTO_HABILITA_MODO_LD6");

            this.gbLockKeyboard.Text = rm.GetString("TEXTO_LOCK_KEYBOARD");

            this.gbPasswords.Text = rm.GetString("TEXTO_PASSWORDS");
            this.cbHabilitaUsoSenha.Text = rm.GetString("TEXTO_ENABLE_PASSWORDS");

            this.cbHabilitaUsoLock.Text = rm.GetString("TEXTO_ENABLE_LOCK");

            this.lbAntiTheft.Text = rm.GetString("TEXTO_ANTI_THEFT");
            this.lbSpecialAccess.Text = rm.GetString("TEXTO_SPECIAL_ACCESS");
            this.lbUseOnlyNumbers.Text = rm.GetString("TEXTO_USE_ONLY_NUMBERS");
            this.labelStatus.Text = "";

            this.gbLockFunctions.Text = rm.GetString("TEXTO_LOCK_FUNCTIONS");
            this.clbBloqueioFuncoes.Items.Clear();
            for (int i = 0; i < Util.Util.QUANTIDADE_DE_FUNCOES_BLOQUEAVEIS; i++)
            {
                if (i == 7) // Seleciona Região
                { continue; }

                this.clbBloqueioFuncoes.Items.Add(rm.GetString("TEXTO_FUNCOES_" + i.ToString()));

            }

            //Adicionado posteriormente para ser atualizado com a mudança da lingua no menu

            if (criarNovo)
            {
                this.lbCarminho.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_LABEL_CAMINHO_DESCRICAO");
                this.Text = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_LABEL_CAMINHO_DESCRICAO");
                this.NomeArquivo = rm.GetString("ARQUIVO_TABPAGE_ARQUIVO_LABEL_CAMINHO_DESCRICAO");
            }
            else
            {
                this.Text = this.NomeArquivo;
                this.lbCarminho.Text = this.NomeArquivo;
            }

            //alteração de idioma pelo usuário remontar todos os componentes
            if (alteracaoIdioma)
            {

                //setando o idioma do controlador e outras classes
                fachada.SetarResourceManagerControlador(controladorSelecionado);

                PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));

                //se estiver selecionada a aba de transmissão, recarregar os checks
                if (tabControlArquivo.SelectedIndex == tab_Trasmitir)
                    CarregarDadosTransmissao();

                //aplicando idioma na lista de roteiros
                listarRoteiros1.rm = fachada.carregaIdioma();
                listarRoteiros1.AplicaIdioma();
                if (listarRoteiros1.Visible)
                    CarregarRoteirosPainel();

                listarRoteiros1.Refresh();

                //aplicando idioma no roteiro
                EditorRoteiros.rm = fachada.carregaIdioma();
                EditorRoteiros.AplicaIdioma();
                EditorRoteiros.PopularApresentacao();
                EditorRoteiros.Refresh();

                //aplicando idioma ao numero do roteiro
                NumEditor.rm = fachada.carregaIdioma();
                NumEditor.AplicaIdioma();
                NumEditor.RecarregarApresentacao();
                NumEditor.Refresh();

                //aplicando idioma ao editor de roteiros
                TextosEditor.rm = fachada.carregaIdioma();
                TextosEditor.AplicaIdioma();
                TextosEditor.RecarregarTipoModelo();
                TextosEditor.RecarregarApresentacao();
                TextosEditor.Refresh();

                //aplicando idioma na lista de mensagens
                listarMensagens1.rm = fachada.carregaIdioma();
                listarMensagens1.AplicaIdioma();
                if (listarMensagens1.Visible)
                    CarregarMensagensPainel();

                listarMensagens1.Refresh();

                //aplicando idioma na mensagem
                EditorMensagens.rm = fachada.carregaIdioma();
                EditorMensagens.AplicaIdioma();
                EditorMensagens.PopularApresentacao();
                EditorMensagens.Refresh();

                TextosEditorMsg.rm = fachada.carregaIdioma();
                TextosEditorMsg.AplicaIdioma();
                TextosEditorMsg.RecarregarTipoModelo();
                TextosEditorMsg.RecarregarApresentacao();
                TextosEditorMsg.Refresh();

                //aplicando idioma as mensagens especiais
                textosEditorMsgEsp1.rm = fachada.carregaIdioma();
                textosEditorMsgEsp1.AplicaIdioma();
                if (textosEditorMsgEsp1.Visible)
                    textosEditorMsgEsp1.RecarregarApresentacao();
                textosEditorMsgEsp1.Refresh();

                //aplicando idioma a tela de simulação
                editorSimulacao.rm = fachada.carregaIdioma();
                editorSimulacao.AplicaIdioma();
                if (editorSimulacao.Visible)
                {
                    editorSimulacao.reposicionarInverterLED();
                    editorSimulacao.reposicionarSaudacoes();
                    editorSimulacao.popularComboAlternancia(editorSimulacao.GetAlternanciaSelecionada());
                    editorSimulacao.AtualizarLabelTextoRoteiro();
                }
                editorSimulacao.Refresh();

                //aplicando idioma a tela de agendamento
                editorAgendamento.rm = fachada.carregaIdioma();
                editorAgendamento.AplicaIdioma();
                if (editorAgendamento.Visible)
                    editorAgendamento.RecarregarTela();
                editorAgendamento.Refresh();

                //aplicando idioma a tela de motorista
                editarMotorista.rm = fachada.carregaIdioma();
                editarMotorista.AplicaIdioma();
                if (editarMotorista.Visible)
                    editarMotorista.RecarregarTela();
                editarMotorista.Refresh();

                //aplicando idioma ao editor de motorista
                MotoristaEditor.rm = fachada.carregaIdioma();
                MotoristaEditor.AplicaIdioma();
                if (MotoristaEditor.Visible)
                    MotoristaEditor.RecarregarApresentacao();
                MotoristaEditor.Refresh();
            }
        }

        #endregion

        #region BackgroundWorker

        private void background_DoWork(object sender, DoWorkEventArgs e)
        {
            MethodInvoker bloquearTela = delegate
            {
                travarUserControl(true, true);
                btnSalvar.Text = rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_BTN_ENVIAR_CANCELAR");
            };

            Invoke(bloquearTela);

            fachada.GerarNandFS(controladorSelecionado, labelCaminhoUsuario.Text, cbSendNSS.Checked, background);

        }

        private void background_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBarTransmissao.Value = e.ProgressPercentage;
            labelStatus.Text = fachada.StatusEnvioControlador(controladorSelecionado);

        }

        void background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            labelStatus.Text = fachada.StatusEnvioControlador(controladorSelecionado);

            MethodInvoker desbloquearTela = delegate
            {
                travarUserControl(false, true);
                btnSalvar.Text = rm.GetString("ARQUIVO_TABPAGE_TRANSMITIR_BTN_ENVIAR");

                //cancelado pelo usuário
                if (fachada.GetCancelarEnvioNFX(controladorSelecionado))
                {
                    progressBarTransmissao.Value = 0;
                    labelCaminhoUsuario.Text = "";
                    labelStatus.Text = "";
                    labelCaminhoNFS.Visible = false;
                    CursorDefault();
                }
                else
                    GerarNFX_APP();
            };

            Invoke(desbloquearTela);
        }

        private void GerarNFX_APP()
        {
            try
            {
                bool[] perifericos = fachada.RetornarPerifericos(controladorSelecionado);

                if (!perifericos[(int)Util.Util.IndicePerifericosNaRede.APP])
                    return;

                CursorWait();

                fachada.appInstalado = Util.Util.ExisteAPPInstalado(out fachada.caminhoApp);
                if (!fachada.appInstalado)
                {
                    CursorDefault();
                    MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_ROTEIRO_MBOX_APP_DESINSTALADO"));
                    return;
                }

                //Verificando se o ninbus já está em execução
                Process[] app = Process.GetProcessesByName(Util.Util.NOME_APP_APP);
                if (app.Length > 0)
                {
                    CursorDefault();
                    MessageBox.Show(rm.GetString("USER_CONTROL_LISTAR_ROTEIRO_MBOX_APP_EM_EXECUCAO"));
                    return;
                }

                //montando lista de roteiros
                string listaRoteiros = "";
                foreach(Roteiro r in fachada.CarregarRoteiros(controladorSelecionado, painelSelecionado))
                {
                    if (r.EnvioRoteiroAPP == Util.Util.EnvioRoteiroAPP.EnviarRotaAPP)
                        listaRoteiros = listaRoteiros + r.Numero.LabelFrase.Replace(" ","$") + " ";
                }

                //montando o caminho do app
                listaRoteiros = listaRoteiros.Substring(0, listaRoteiros.Length - 1);
                string caminho = Path.GetDirectoryName(labelCaminhoUsuario.Text);
                caminho = caminho.Substring(0, caminho.Length - Util.Util.DIRETORIO_APP_NFX.Length);

                //abrindo app para envio da lista de roteiros
                Process appProcess = new Process();
                appProcess.StartInfo.Arguments = "1 " + Convert.ToByte(fachada.GetIdiomaFachada()).ToString() + " " + caminho.Replace(" ","$") + " " + listaRoteiros;
                appProcess.StartInfo.FileName = fachada.caminhoApp + Util.Util.NOME_ARQUIVO_APP + Util.Util.ARQUIVO_EXT_EXE;
                appProcess.Start();

                CursorDefault();
            }
            catch
            {
                CursorDefault();
            }
        }

        private bool ApresentacaoPainelIsBusy()
        {
            if (backgroundApresentacao1.IsBusy || backgroundApresentacao2.IsBusy || backgroundApresentacao3.IsBusy || backgroundApresentacao4.IsBusy)
                return true;
            else
                return false;
        }

        private void backgroundApresentacao1_DoWork(object sender, DoWorkEventArgs e)
        {
            listaVideoBitMap[0].Apresentar(fraseGUI.Modelo.Textos[0]);

        }

        private void backgroundApresentacao2_DoWork(object sender, DoWorkEventArgs e)
        {
            listaVideoBitMap[1].Apresentar(fraseGUI.Modelo.Textos[1]);

        }

        private void backgroundApresentacao3_DoWork(object sender, DoWorkEventArgs e)
        {
            listaVideoBitMap[2].Apresentar(fraseGUI.Modelo.Textos[2]);

        }

        private void backgroundApresentacao4_DoWork(object sender, DoWorkEventArgs e)
        {
            listaVideoBitMap[3].Apresentar(fraseGUI.Modelo.Textos[3]);

        }

        #endregion

        #region Controlador

        public Boolean SalvarControlador(String ArquivoNome)
        {
            //verificar as janelas abertas e perguntar ao usuário se deseja salvar as informações, pois só estava salvando o que estava na fachada e o usuário pode estar editando algum informação

            string msgBox = "";


            //Tela de edição de roteiros
            if (EditorRoteiros.Visible)
            {
                if (EditorRoteiros.incluirRoteiro)
                {
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_INCLUIR_ROTEIRO");
                    if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        if (!EditorRoteiros.exibirErro(true))
                            EditorRoteiros.AplicarRoteiro();
                        else
                            return false;
                }
                else //Edição
                {
                    EditorRoteiros.PreencheObjetoGUI();
                    if (fachada.CompararObjetosRoteiro(controladorSelecionado, painelSelecionado, EditorRoteiros.roteiroGUI))
                    {
                        msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_ROTEIRO");
                        if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            if (!EditorRoteiros.exibirErro(true))
                                EditorRoteiros.AplicarRoteiro();
                            else
                                return false;
                    }
                }
            }

            //tela de edição do numero do roteiro
            if (NumEditor.Visible)
            {
                if (EditorRoteiros.incluirRoteiro)
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_INCLUIR_ROTEIRO");
                else
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_ROTEIRO");

                if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (!NumEditor.exibirErro())
                    {
                        NumEditor.Roteiros.EditarNumeroGUI(NumEditor.numeroGUI);
                        if (!EditorRoteiros.exibirErro(true))
                        {
                            EditorRoteiros.AplicarRoteiro();
                            cbPainel.Enabled = false;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }

            //tela de edição dos textos do roteiro
            if (TextosEditor.Visible)
            {

                if (EditorRoteiros.incluirRoteiro)
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_INCLUIR_ROTEIRO");
                else
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_ROTEIRO");

                if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (EditorRoteiros.exibirErro(true))
                        return false;

                    if (TextosEditor.isFraseIda)
                    {
                        if (TextosEditor.isEdicao)
                            TextosEditor.Roteiros.EditarFraseIdaRoteiroGUI(TextosEditor.fraseGUI);
                        else
                            TextosEditor.Roteiros.IncluirFraseIdaRoteiroGUI(TextosEditor.fraseGUI);
                    }
                    else
                    {
                        if (TextosEditor.isEdicao)
                            TextosEditor.Roteiros.EditarFraseVoltaRoteiroGUI(TextosEditor.fraseGUI);
                        else
                            TextosEditor.Roteiros.IncluirFraseVoltaRoteiroGUI(TextosEditor.fraseGUI);
                    }

                    EditorRoteiros.AplicarRoteiro();
                    cbPainel.Enabled = false;
                }
            }


            //tela de edição das mensagens
            if (EditorMensagens.Visible)
            {

                if (EditorMensagens.incluirMensagem)
                {
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_INCLUIR_MENSAGEM");
                    if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        if (!EditorMensagens.exibirErro())
                            EditorMensagens.AplicarMensagem();
                        else
                            return false;
                }
                else //Edição
                {
                    EditorMensagens.PreencheObjetoGUI();
                    if (fachada.CompararObjetosMensagem(controladorSelecionado, painelSelecionado, EditorMensagens.mensagemGUI))
                    {
                        msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_MENSAGEM");
                        if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            if (!EditorMensagens.exibirErro())
                                EditorMensagens.AplicarMensagem();
                            else
                                return false;
                    }
                }

            }

            //tela de edição dos textos das mensagens
            if (TextosEditorMsg.Visible)
            {

                if (EditorMensagens.incluirMensagem)
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_INCLUIR_MENSAGEM");
                else
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_MENSAGEM");

                if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("USER_CONTROL_MENSAGENS_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (EditorMensagens.exibirErro())
                        return false;

                    if (TextosEditorMsg.isEdicao)
                        TextosEditorMsg.Mensagens.EditarFraseMensagemGUI(TextosEditorMsg.fraseGUI);
                    else
                        TextosEditorMsg.Mensagens.IncluirFraseMensagemGUI(TextosEditorMsg.fraseGUI);

                    EditorMensagens.AplicarMensagem();
                    cbPainel.Enabled = false;
                }

            }

            //tela de edição do motorista
            if (editarMotorista.Visible)
            {
                if (editarMotorista.UsuarioEditando())
                {
                    if (!editarMotorista.isEdicao)
                        msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_INCLUIR_MOTORISTA");
                    else
                        msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_MOTORISTA");

                    if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        if (editarMotorista.exibirErro(true))
                            return false;

                        //Aplicando o Motorista
                        editarMotorista.AplicarMotorista();

                        //recuperando lista de motoristas da fachada
                        editarMotorista.listaMotorista = fachada.CarregarMotoristas(controladorSelecionado, painelSelecionado);

                        //carregando a lista de motoristas no listview
                        editarMotorista.PopularListaMotoristas();

                        //recuperando o novo indice do motorista inserido após a ordenação
                        editarMotorista.indiceMotorista = fachada.IndexMotorista(controladorSelecionado, painelSelecionado, editarMotorista.motoristaGUI.ID.LabelFrase);

                        //carregando a gui com as informações
                        editarMotorista.EditarMotorista(true);

                        //Liberando a seleção de painel
                        TravarPainel(true, editarMotorista.painelSelecionado);

                        //selecionando item no listview
                        editarMotorista.SelecionarItemListView();

                        //setando o campo ID
                        editarMotorista.SetarCampo(true);
                    }

                }
            }

            //tela de edição dos textos do motorista
            if (MotoristaEditor.Visible)
            {
                if (!editarMotorista.isEdicao)
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_INCLUIR_MOTORISTA");
                else
                    msgBox = rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_MOTORISTA");

                if (DialogResult.Yes == MessageBox.Show(msgBox, rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (MotoristaEditor.exibirErro(true))
                        return false;

                    if (MotoristaEditor.isID)
                        MotoristaEditor.Motoristas.EditarIDGUI(MotoristaEditor.fraseGUI);
                    else
                        MotoristaEditor.Motoristas.EditarNomeGUI(MotoristaEditor.fraseGUI);

                    //Aplica o motorista
                    MotoristaEditor.Motoristas.AplicarMotorista();

                    //recuperando lista de motoristas da fachada
                    editarMotorista.listaMotorista = fachada.CarregarMotoristas(controladorSelecionado, painelSelecionado);

                    //carregando a lista de motoristas no listview
                    editarMotorista.PopularListaMotoristas();

                    //recuperando o novo indice do motorista inserido após a ordenação
                    editarMotorista.indiceMotorista = fachada.IndexMotorista(controladorSelecionado, painelSelecionado, editarMotorista.motoristaGUI.ID.LabelFrase);

                    //carregando a gui com as informações
                    editarMotorista.EditarMotorista(true);

                    //selecionando item no listview
                    editarMotorista.SelecionarItemListView();

                }
            }

            //Editor de Mensagens especiais
            if (textosEditorMsgEsp1.Visible)
            {
                if (fachada.CompararObjetosMensagemEspecial(controladorSelecionado, painelSelecionado, textosEditorMsgEsp1.mensagemEspecialGUI) || fachada.CompararObjetosMensagemEmergencia(controladorSelecionado, painelSelecionado, textosEditorMsgEsp1.mensagemEmergenciaGUI))
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_MENSAGEM_ESP"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        textosEditorMsgEsp1.Salvar();
                    else
                        textosEditorMsgEsp1.EditarMensagensEspeciais(fachada.CarregarMensagemEspecial(controladorSelecionado, painelSelecionado), fachada.CarregarMensagemEmergencia(controladorSelecionado, painelSelecionado));


            }

            //tela de simulação
            if (editorSimulacao.Visible)
            {
                if (editorSimulacao.MudouParametros() && editorSimulacao.ValidarCampos(false))
                    if (DialogResult.Yes == MessageBox.Show(rm.GetString("ARQUIVO_MBOX_BTN_SALVAR_EDICAO_SIMULACAO"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        editorSimulacao.SetarParametrosNaFachada();
            }

            fachada.Salvar(ArquivoNome, this.controladorSelecionado);
            this.criarNovo = false;

            return true;
        }

        public void SetarArquivoControlador(String nomeArquivo)
        {

            this.Text = nomeArquivo;
            this.lbCarminho.Text = nomeArquivo;
            this.NomeArquivo = nomeArquivo;

        }

        public void SetarControladoSelecionado(int controladorSelecionado)
        {
            this.controladorSelecionado = controladorSelecionado;

            //Telas de Roteiro
            NumEditor.controladorSelecionado = controladorSelecionado;
            listarRoteiros1.controladorSelecionado = controladorSelecionado;
            EditorRoteiros.controladorSelecionado = controladorSelecionado;
            TextosEditor.controladorSelecionado = controladorSelecionado;

            //Telas de Mensagens
            listarMensagens1.controladorSelecionado = controladorSelecionado;
            TextosEditorMsg.controladorSelecionado = controladorSelecionado;
            EditorMensagens.controladorSelecionado = controladorSelecionado;

            //Tela de Mensagem Especial
            textosEditorMsgEsp1.controladorSelecionado = controladorSelecionado;

            //Tela de edição da simulação
            editorSimulacao.controladorSelecionado = controladorSelecionado;

            //Tela de Agendamento
            editorAgendamento.controladorSelecionado = controladorSelecionado;

            //Tela de Motorista
            editarMotorista.controladorSelecionado = controladorSelecionado;
            MotoristaEditor.controladorSelecionado = controladorSelecionado;

        }

        #endregion

        #region Painel e Controlador

        private void PopulaPaineisDataGrid(List<Painel> paineis)
        {
            dataGridViewPaineis.Rows.Clear();
            for (int row = 0; row < paineis.Count; row++)
            {
                dataGridViewPaineis.Rows.Add();

                dataGridViewPaineis.Rows[row].Cells[colunaPainel].Value = paineis[row].Indice + 1;
                dataGridViewPaineis.Rows[row].Cells[colunaPainel].ReadOnly = true; //ineditável para o índice.

                dataGridViewPaineis.Rows[row].Cells[colunaAltura].Value = (paineis[row].MultiLinhas == 1 ? System.Convert.ToString(paineis[row].Altura) : System.Convert.ToString(paineis[row].Altura / paineis[row].MultiLinhas));
                dataGridViewPaineis.Rows[row].Cells[colunaLargura].Value = paineis[row].Largura;

                if (paineis[row].BrilhoMinimo >= 1 && paineis[row].BrilhoMinimo <= 100)
                    dataGridViewPaineis.Rows[row].Cells[colunaBrilhoMin].Value = paineis[row].BrilhoMinimo;
                else
                    dataGridViewPaineis.Rows[row].Cells[colunaBrilhoMin].Value = 10;

                if (paineis[row].BrilhoMaximo >= 1 && paineis[row].BrilhoMaximo <= 100)
                    dataGridViewPaineis.Rows[row].Cells[colunaBrilhoMax].Value = paineis[row].BrilhoMaximo;
                else
                    dataGridViewPaineis.Rows[row].Cells[colunaBrilhoMax].Value = 100;

                dataGridViewPaineis.Rows[row].Cells[colunaMultiLinhas].Value = System.Convert.ToString(paineis[row].MultiLinhas);
            }
        }

        private void PopulaPaineisComboBox(List<Painel> paineis)
        {
            cbPainel.Items.Clear();
            cbPainelNSS.Items.Clear();

            string labelPainel;

            foreach (Painel painel in paineis)
            {

                if (painel.MultiLinhas > 1)
                    labelPainel = rm.GetString("ARQUIVO_PAINEL") + " " + (painel.Indice + 1).ToString("d3") + " " + (painel.Altura / painel.MultiLinhas).ToString("d2") + " X " + painel.Largura.ToString("d2") +
                                    " (" + rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS") + " " + painel.MultiLinhas + "x)";
                else
                    labelPainel = rm.GetString("ARQUIVO_PAINEL") + " " + (painel.Indice + 1).ToString("d3") + " " + painel.Altura.ToString("d2") + " X " + painel.Largura.ToString("d2");

                cbPainel.Items.Add(labelPainel);
                cbPainelNSS.Items.Add(labelPainel);
            }

            cbPainel.SelectedIndex = painelSelecionado;

            //setando o Painel NSS
            if (Directory.Exists(Fachada.diretorio_NSS))
            {
                if (cbPainelNSS.SelectedIndex < 0)
                    cbPainelNSS.SelectedIndex = cbPainelNSS.Items.Count - 1;
            }

        }

        private void IncluirPainel()
        {

            Painel pTemp = new Painel(this.rm);
            pTemp.Indice = dataGridViewPaineis.RowCount;

            pTemp.AlternanciaSelecionada = 0;
            IncluirDadosDoPainelPrincipal(pTemp);

            //Incluindo a lista de alternancias padrão ao painel
            pTemp.ListaAlternancias = fachada.CarregarAlternanciaPadraoPainel();

            //Incluir o objeto painil na fachada
            fachada.IncluirPainel(this.controladorSelecionado, pTemp);

            //Setando as fonte default para a altura do painel
            fachada.SetarFontesDefaultPainel(fachada.CarregarPainel(controladorSelecionado, pTemp.Indice));

            //Popula componenetes da GUI
            LimpaDataGrid();
            PopulaPaineisDataGrid(fachada.CarregarPaineis(controladorSelecionado));
            PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));

        }

        private void IncluirDadosDoPainelPrincipal(Painel pTemp)
        {
            //Incluir dados do painel princiapl no novo painel

            //Retorna da fachada o painel Principal - Indice 0
            Painel painelPrincipal = fachada.CarregarPainel(controladorSelecionado, 0);

            bool copiarFrase = (painelPrincipal.MultiLinhas == 1 ? true : false);

            //Incluir roteiros do painel principal ao novo painel
            for (int i = 0; i < painelPrincipal.Roteiros.Count(); i++)
            {
                Roteiro rTemp = new Roteiro(painelPrincipal.Roteiros[i], copiarFrase);

                foreach (Frase f in rTemp.FrasesIda)
                    fachada.RedimensionarFrase(f, painelPrincipal.Altura, painelPrincipal.Largura, pTemp.Altura, pTemp.Largura);
                foreach (Frase f in rTemp.FrasesVolta)
                    fachada.RedimensionarFrase(f, painelPrincipal.Altura, painelPrincipal.Largura, pTemp.Altura, pTemp.Largura);

                pTemp.Roteiros.Add(rTemp);
            }

            //Incluir mensagens do painel principal ao novo painel
            for (int i = 0; i < painelPrincipal.Mensagens.Count(); i++)
            {
                Mensagem mTemp = new Mensagem(painelPrincipal.Mensagens[i], true);
                foreach (Frase f in mTemp.Frases)
                    fachada.RedimensionarFrase(f, painelPrincipal.Altura, painelPrincipal.Largura, pTemp.Altura, pTemp.Largura);

                pTemp.Mensagens.Add(mTemp);
            }

            //Inlcuindo Mensagens especiais do painel principal ao novo painel
            pTemp.MensagensEspeciais = new MensagemEspecial(painelPrincipal.MensagensEspeciais);

            //Incluindo Mensagem de emergencia do painel principal ao novo painel
            pTemp.MensagemEmergencia = new MensagemEmergencia(painelPrincipal.MensagemEmergencia);

            //Incluindo Motoristas do painel principal ao novo painel
            for (int i = 0; i < painelPrincipal.Motoristas.Count; i++)
            {
                Motorista m = new Motorista(painelPrincipal.Motoristas[i]);
                pTemp.Motoristas.Add(m);
            }

        }

        private void ExcluirPainel()
        {
            int indicePainel = dataGridViewPaineis.CurrentCell.RowIndex;


            if (indicePainel == dataGridViewPaineis.Rows.Count - 1)
               indicePainel = dataGridViewPaineis.CurrentCell.RowIndex - 1;

            //Remove da Fachada
            fachada.ExcluirPainel(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);

            //Remove painel do parametro fixo
            fachada.RemoverPainelAPP(controladorSelecionado, dataGridViewPaineis.CurrentCell.RowIndex);

            //Popula na GUI
            painelSelecionado = indicePainel;
            PopulaPaineisDataGrid(fachada.CarregarPaineis(controladorSelecionado));
            PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));

            SetarPainelSelecionado();
            DesenharPainel();

        }

        public void TravarPainel(bool travarPainel, int painelSelecionado)
        {
            if (cbPainel.SelectedIndex != painelSelecionado)
            {
                cbPainel.SelectedIndex = painelSelecionado;
                SetarPainelSelecionado();
                DesenharPainel();
            }
            cbPainel.Enabled = travarPainel;
        }

        public void ReposicionarVideoMapNoPainel(bool edicao)
        {
            if (fraseGUI == null)
            {

                panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                panelVideoBitmap.Width = listaVideoBitMap[0].Width + 3;
                panelVideoBitmap.Height = listaVideoBitMap[0].Height + 3;

                listaVideoBitMap[0].Location = new Point(0, 0);

                panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
                {

                    panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                    listaVideoBitMap[0].Location = new Point(0, 0);
                    panelVideoBitmap.Width = listaVideoBitMap[0].Width + 3;
                    panelVideoBitmap.Height = listaVideoBitMap[0].Height + 3;

                    panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                    panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;

                }

                if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V04)
                {
                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
                    {
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);

                        panelVideoBitmap.Width = listaVideoBitMap[0].Width + 3;
                        panelVideoBitmap.Height = listaVideoBitMap[0].Height + 3;

                        listaVideoBitMap[0].Location = new Point(0, 0);

                        panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                        panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero)
                    {
                        if (edicao) // Mostra o trackbar de Largura dos Paineis
                        {
                            //Setando as configurações do trackbar de Altura
                            tbarLargura.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width - 16 + 1;
                            tbarLargura.Minimum = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            tbarLargura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(1) + listaVideoBitMap[1].listaBitMap[0].GetLength(1) - Util.Util.LARGURA_MINIMA_TEXTOS_V04;

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(tbarLargura);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 1 + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + tbarLargura.Height + 3;

                            //setando a localização dos componentes na tela
                            tbarLargura.Location = new Point(8, 23);
                            listaVideoBitMap[0].Location = new Point(0, tbarLargura.Height);
                            listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width + 1, tbarLargura.Height);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, (panel1.Height / 2 - panelVideoBitmap.Size.Height / 2) - 15);

                            panelVideoBitmap.BorderStyle = BorderStyle.None;

                            //setando o trackbar para receber o valor de largura do texto
                            tbarLargura.Value = fraseGUI.Modelo.Textos[0].LarguraPainel;
                        }
                        else // Não mostra o trackbar de Largura dos Paineis
                        {

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + 3;

                            //setando a localização dos componentes na tela
                            listaVideoBitMap[0].Location = new Point(0, 0);
                            listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;

                        }

                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
                    {
                        if (edicao)
                        {
                            //Setando as configurações do trackbar de Altura
                            tbarAltura.Height = listaVideoBitMap[0].Height + listaVideoBitMap[1].Height + 4 + 1;
                            tbarAltura.Minimum = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            tbarAltura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(0) + listaVideoBitMap[1].listaBitMap[0].GetLength(0) - Util.Util.ALTURA_MINIMA_TEXTOS_V04;

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(tbarAltura);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + tbarAltura.Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + listaVideoBitMap[1].Height + 1 + 3;

                            //setando a localização dos componentes na tela
                            tbarAltura.Location = new Point(23, -2);
                            listaVideoBitMap[0].Location = new Point(tbarAltura.Width, 0);
                            listaVideoBitMap[1].Location = new Point(tbarAltura.Width, listaVideoBitMap[0].Height + 1);
                            panelVideoBitmap.Location = new Point((panel1.Width / 2 - panelVideoBitmap.Size.Width / 2) - 15, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.None;

                            //setando o trackbar para receber o valor de largura do texto
                            tbarAltura.Value = fraseGUI.Modelo.Textos[1].AlturaPainel;
                        }
                        else
                        {
                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + listaVideoBitMap[1].Height + 3;

                            //setando a localização dos componentes na tela
                            listaVideoBitMap[0].Location = new Point(0, 0);
                            listaVideoBitMap[1].Location = new Point(0, listaVideoBitMap[0].Height);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;

                        }
                    }


                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo)
                    {
                        if (edicao)
                        {
                            //Setando as configurações do trackbar de Altura e de Largura
                            tbarAltura.Height = listaVideoBitMap[0].Height + 4 + 1;
                            tbarAltura.Minimum = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            tbarAltura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(0) - Util.Util.ALTURA_MINIMA_TEXTOS_V04;

                            tbarLargura.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width - 16 + 1;
                            tbarLargura.Minimum = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            tbarLargura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(1) + listaVideoBitMap[1].listaBitMap[0].GetLength(1) - Util.Util.LARGURA_MINIMA_TEXTOS_V04;

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(tbarAltura);
                            panelVideoBitmap.Controls.Add(tbarLargura);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = tbarAltura.Width + listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 1 + 3;
                            panelVideoBitmap.Height = tbarLargura.Height + listaVideoBitMap[0].Height + 1 + 3;

                            //setando a localização dos componentes na tela
                            tbarLargura.Location = new Point(tbarAltura.Width + 8, 23);
                            tbarAltura.Location = new Point(23, tbarLargura.Height - 2);
                            listaVideoBitMap[0].Location = new Point(tbarAltura.Width, tbarLargura.Height);
                            listaVideoBitMap[1].Location = new Point(tbarAltura.Width + listaVideoBitMap[0].Width + 1, tbarLargura.Height);
                            listaVideoBitMap[2].Location = new Point(tbarAltura.Width + listaVideoBitMap[0].Width + 1, tbarLargura.Height + listaVideoBitMap[1].Height + 1);
                            panelVideoBitmap.Location = new Point((panel1.Width / 2 - panelVideoBitmap.Size.Width / 2) - 15, (panel1.Height / 2 - panelVideoBitmap.Size.Height / 2) - 15);

                            panelVideoBitmap.BorderStyle = BorderStyle.None;

                            //setando o trackbar para receber o valor de largura do texto
                            tbarAltura.Value = fraseGUI.Modelo.Textos[2].AlturaPainel;
                            tbarLargura.Value = fraseGUI.Modelo.Textos[0].LarguraPainel;
                        }
                        else
                        {
                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + 3;

                            //setando a localização dos componentes na tela
                            listaVideoBitMap[0].Location = new Point(0, 0);
                            listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                            listaVideoBitMap[2].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;
                        }
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero)
                    {
                        if (edicao)
                        {
                            //Setando as configurações do trackbar de Altura e de Largura
                            tbarAltura.Height = listaVideoBitMap[1].Height + 4 + 1;
                            tbarAltura.Minimum = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            tbarAltura.Maximum = listaVideoBitMap[1].listaBitMap[0].GetLength(0) - Util.Util.ALTURA_MINIMA_TEXTOS_V04;

                            tbarLargura.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width - 16 + 1;
                            tbarLargura.Minimum = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            tbarLargura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(1) + listaVideoBitMap[1].listaBitMap[0].GetLength(1) - Util.Util.LARGURA_MINIMA_TEXTOS_V04;

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(tbarAltura);
                            panelVideoBitmap.Controls.Add(tbarLargura);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = tbarAltura.Width + listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 1 + 3;
                            panelVideoBitmap.Height = tbarLargura.Height + listaVideoBitMap[1].Height + 1 + 3;

                            //setando a localização dos componentes na tela
                            tbarLargura.Location = new Point(tbarAltura.Width + 8, 23);
                            tbarAltura.Location = new Point(23, tbarLargura.Height - 2);
                            listaVideoBitMap[0].Location = new Point(tbarAltura.Width, tbarLargura.Height);
                            listaVideoBitMap[1].Location = new Point(tbarAltura.Width + listaVideoBitMap[0].Width + 1, tbarLargura.Height);
                            listaVideoBitMap[2].Location = new Point(tbarAltura.Width, tbarLargura.Height + listaVideoBitMap[0].Height + 1);
                            panelVideoBitmap.Location = new Point((panel1.Width / 2 - panelVideoBitmap.Size.Width / 2) - 15, (panel1.Height / 2 - panelVideoBitmap.Size.Height / 2) - 15);

                            panelVideoBitmap.BorderStyle = BorderStyle.None;

                            //setando o trackbar para receber o valor de largura do texto
                            tbarAltura.Value = fraseGUI.Modelo.Textos[2].AlturaPainel;
                            tbarLargura.Value = fraseGUI.Modelo.Textos[0].LarguraPainel;
                        }
                        else
                        {

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[1].Height + 3;

                            //setando a localização dos componentes na tela
                            listaVideoBitMap[0].Location = new Point(0, 0);
                            listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                            listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;

                        }
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo)
                    {
                        if (edicao)
                        {
                            //Setando as configurações do trackbar de Altura e de Largura
                            tbarAltura.Height = listaVideoBitMap[0].Height + listaVideoBitMap[2].Height + 4 + 1;
                            tbarAltura.Minimum = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            tbarAltura.Maximum = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - Util.Util.ALTURA_MINIMA_TEXTOS_V04;

                            tbarLargura.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width - 16 + 1;
                            tbarLargura.Minimum = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            tbarLargura.Maximum = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - Util.Util.LARGURA_MINIMA_TEXTOS_V04;

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                            panelVideoBitmap.Controls.Add(tbarAltura);
                            panelVideoBitmap.Controls.Add(tbarLargura);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = tbarAltura.Width + listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 1 + 3;
                            panelVideoBitmap.Height = tbarLargura.Height + listaVideoBitMap[0].Height + listaVideoBitMap[2].Height + 1 + 3;

                            //setando a localização dos componentes na tela
                            tbarLargura.Location = new Point(tbarAltura.Width + 8, 23);
                            tbarAltura.Location = new Point(23, tbarLargura.Height - 2);
                            listaVideoBitMap[0].Location = new Point(tbarAltura.Width, tbarLargura.Height);
                            listaVideoBitMap[1].Location = new Point(tbarAltura.Width + listaVideoBitMap[0].Width + 1, tbarLargura.Height);
                            listaVideoBitMap[2].Location = new Point(tbarAltura.Width, tbarLargura.Height + listaVideoBitMap[0].Height + 1);
                            listaVideoBitMap[3].Location = new Point(tbarAltura.Width + listaVideoBitMap[0].Width + 1, tbarLargura.Height + listaVideoBitMap[1].Height + 1);
                            panelVideoBitmap.Location = new Point((panel1.Width / 2 - panelVideoBitmap.Size.Width / 2) - 15, (panel1.Height / 2 - panelVideoBitmap.Size.Height / 2) - 15);

                            panelVideoBitmap.BorderStyle = BorderStyle.None;

                            //setando o trackbar para receber o valor de largura do texto
                            tbarAltura.Value = fraseGUI.Modelo.Textos[2].AlturaPainel;
                            tbarLargura.Value = fraseGUI.Modelo.Textos[0].LarguraPainel;
                        }
                        else
                        {
                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + listaVideoBitMap[2].Height + 3;

                            //setando a localização dos componentes na tela
                            listaVideoBitMap[0].Location = new Point(0, 0);
                            listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                            listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);
                            listaVideoBitMap[3].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;

                        }
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
                    {
                        //TODO: fazer a edição quando tiver o componente de trackbar
                        if (edicao)
                        {
                            //Setando as configurações do trackbar de Altura
                            rangeAltura.Height = listaVideoBitMap[0].Height + listaVideoBitMap[1].Height + listaVideoBitMap[2].Height - 16;
                            rangeAltura.Minimum = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            rangeAltura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(0) + listaVideoBitMap[1].listaBitMap[0].GetLength(0) + +listaVideoBitMap[2].listaBitMap[0].GetLength(0) - Util.Util.ALTURA_MINIMA_TEXTOS_V04;

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(rangeAltura);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + rangeAltura.Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + 1 + listaVideoBitMap[1].Height + 1 + listaVideoBitMap[2].Height + 3;

                            //setando a localização dos componentes na tela
                            rangeAltura.Location = new Point(0, 9);
                            listaVideoBitMap[0].Location = new Point(rangeAltura.Width + 1, 0);
                            listaVideoBitMap[1].Location = new Point(rangeAltura.Width + 1, listaVideoBitMap[0].Height + 1);
                            listaVideoBitMap[2].Location = new Point(rangeAltura.Width + 1, listaVideoBitMap[0].Height + 1 + listaVideoBitMap[1].Height + 1);
                            panelVideoBitmap.Location = new Point((panel1.Width / 2 - panelVideoBitmap.Size.Width / 2) - 15, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.None;

                            //setando o trackbar para receber o valor de largura do texto
                            rangeAltura.LowerValue = fraseGUI.Modelo.Textos[2].AlturaPainel;
                            rangeAltura.UpperValue = fraseGUI.Modelo.Textos[2].AlturaPainel + fraseGUI.Modelo.Textos[1].AlturaPainel;
                        }
                        else
                        {
                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + listaVideoBitMap[1].Height + listaVideoBitMap[2].Height + 3;

                            //setando a localização dos componentes na tela
                            listaVideoBitMap[0].Location = new Point(0, 0);
                            listaVideoBitMap[1].Location = new Point(0, listaVideoBitMap[0].Height);
                            listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height + listaVideoBitMap[1].Height);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;
                        }
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
                    {
                        if (edicao)
                        {
                            //Setando as configurações do trackbar de Altura e de Largura
                            rangeAltura.Height = listaVideoBitMap[1].Height + listaVideoBitMap[2].Height + listaVideoBitMap[3].Height - 16;
                            rangeAltura.Minimum = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            rangeAltura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(0) - Util.Util.ALTURA_MINIMA_TEXTOS_V04;

                            tbarLargura.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width - 16 + 1;
                            tbarLargura.Minimum = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            tbarLargura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(1) + listaVideoBitMap[1].listaBitMap[0].GetLength(1) - Util.Util.LARGURA_MINIMA_TEXTOS_V04;

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                            panelVideoBitmap.Controls.Add(rangeAltura);
                            panelVideoBitmap.Controls.Add(tbarLargura);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = rangeAltura.Width + listaVideoBitMap[0].Width + 1 + listaVideoBitMap[1].Width + 3;
                            panelVideoBitmap.Height = tbarLargura.Height + listaVideoBitMap[0].Height + 2 + 3;

                            //setando a localização dos componentes na tela
                            tbarLargura.Location = new Point(rangeAltura.Width + 8, 23);
                            rangeAltura.Location = new Point(0, tbarLargura.Height + 10);
                            listaVideoBitMap[0].Location = new Point(rangeAltura.Width + 1, tbarLargura.Height);
                            listaVideoBitMap[1].Location = new Point(rangeAltura.Width + 1 + listaVideoBitMap[0].Width + 1, tbarLargura.Height);
                            listaVideoBitMap[2].Location = new Point(rangeAltura.Width + 1 + listaVideoBitMap[0].Width + 1, tbarLargura.Height + listaVideoBitMap[1].Height + 1);
                            listaVideoBitMap[3].Location = new Point(rangeAltura.Width + 1 + listaVideoBitMap[0].Width + 1, tbarLargura.Height + listaVideoBitMap[1].Height + 1 + listaVideoBitMap[2].Height + 1);
                            panelVideoBitmap.Location = new Point((panel1.Width / 2 - panelVideoBitmap.Size.Width / 2) - 15, (panel1.Height / 2 - panelVideoBitmap.Size.Height / 2) - 15);

                            panelVideoBitmap.BorderStyle = BorderStyle.None;

                            //setando o trackbar para receber o valor de largura do texto
                            rangeAltura.LowerValue = fraseGUI.Modelo.Textos[3].AlturaPainel;
                            rangeAltura.UpperValue = fraseGUI.Modelo.Textos[3].AlturaPainel + fraseGUI.Modelo.Textos[2].AlturaPainel;
                            tbarLargura.Value = fraseGUI.Modelo.Textos[0].LarguraPainel;
                        }
                        else
                        {
                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[0].Height + 3;

                            //setando a localização dos componentes na tela
                            listaVideoBitMap[0].Location = new Point(0, 0);
                            listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                            listaVideoBitMap[2].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                            listaVideoBitMap[3].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height + listaVideoBitMap[2].Height);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;
                        }
                    }

                    if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
                    {
                        if (edicao)
                        {
                            //Setando as configurações do trackbar de Altura e de Largura
                            rangeAltura.Height = listaVideoBitMap[0].Height + listaVideoBitMap[2].Height + listaVideoBitMap[3].Height - 16;
                            rangeAltura.Minimum = Util.Util.ALTURA_MINIMA_TEXTOS_V04;
                            rangeAltura.Maximum = listaVideoBitMap[1].listaBitMap[0].GetLength(0) - Util.Util.ALTURA_MINIMA_TEXTOS_V04;

                            tbarLargura.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width - 16 + 1;
                            tbarLargura.Minimum = Util.Util.LARGURA_MINIMA_TEXTOS_V04;
                            tbarLargura.Maximum = listaVideoBitMap[0].listaBitMap[0].GetLength(1) + listaVideoBitMap[1].listaBitMap[0].GetLength(1) - Util.Util.LARGURA_MINIMA_TEXTOS_V04;

                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                            panelVideoBitmap.Controls.Add(rangeAltura);
                            panelVideoBitmap.Controls.Add(tbarLargura);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = rangeAltura.Width + listaVideoBitMap[0].Width + 1 + listaVideoBitMap[1].Width + 3;
                            panelVideoBitmap.Height = tbarLargura.Height + listaVideoBitMap[1].Height + 2 + 3;

                            //setando a localização dos componentes na tela
                            tbarLargura.Location = new Point(rangeAltura.Width + 8, 23);
                            rangeAltura.Location = new Point(0, tbarLargura.Height + 10);
                            listaVideoBitMap[0].Location = new Point(rangeAltura.Width + 1, tbarLargura.Height);
                            listaVideoBitMap[1].Location = new Point(rangeAltura.Width + 1 + listaVideoBitMap[0].Width + 1, tbarLargura.Height);
                            listaVideoBitMap[2].Location = new Point(rangeAltura.Width + 1, tbarLargura.Height + listaVideoBitMap[0].Height + 1);
                            listaVideoBitMap[3].Location = new Point(rangeAltura.Width + 1, tbarLargura.Height + listaVideoBitMap[0].Height + 1 + listaVideoBitMap[2].Height + 1);
                            panelVideoBitmap.Location = new Point((panel1.Width / 2 - panelVideoBitmap.Size.Width / 2) - 15, (panel1.Height / 2 - panelVideoBitmap.Size.Height / 2) - 15);

                            panelVideoBitmap.BorderStyle = BorderStyle.None;

                            //setando o trackbar para receber o valor de largura do texto
                            rangeAltura.LowerValue = fraseGUI.Modelo.Textos[3].AlturaPainel;
                            rangeAltura.UpperValue = fraseGUI.Modelo.Textos[3].AlturaPainel + fraseGUI.Modelo.Textos[2].AlturaPainel;
                            tbarLargura.Value = fraseGUI.Modelo.Textos[0].LarguraPainel;
                        }
                        else
                        {
                            //Adicionando os componentes ao panel
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);

                            //setando altura e largura do panel
                            panelVideoBitmap.Width = listaVideoBitMap[0].Width + listaVideoBitMap[1].Width + 3;
                            panelVideoBitmap.Height = listaVideoBitMap[1].Height + 3;

                            //setando a localização dos componentes na tela
                            listaVideoBitMap[0].Location = new Point(0, 0);
                            listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                            listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);
                            listaVideoBitMap[3].Location = new Point(0, listaVideoBitMap[0].Height + listaVideoBitMap[2].Height);
                            panelVideoBitmap.Location = new Point(panel1.Width / 2 - panelVideoBitmap.Size.Width / 2, panel1.Height / 2 - panelVideoBitmap.Size.Height / 2);

                            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;
                        }
                    }
                }
            }
        }

        private void AtualizarPainel()
        {
            Frase frasePainelNovo = new Frase(fraseGUI);

            if (frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero)
            {
                frasePainelNovo.Modelo.Textos[0].LarguraPainel = tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[1].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;

                frasePainelNovo.Modelo.Textos[0].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado);
                frasePainelNovo.Modelo.Textos[1].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado);
            }

            if (frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
            {
                frasePainelNovo.Modelo.Textos[0].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);
                frasePainelNovo.Modelo.Textos[1].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);

                frasePainelNovo.Modelo.Textos[0].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - tbarAltura.Value;
                frasePainelNovo.Modelo.Textos[1].AlturaPainel = tbarAltura.Value;
            }

            if (frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo)
            {
                frasePainelNovo.Modelo.Textos[0].LarguraPainel = tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[1].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[2].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;

                frasePainelNovo.Modelo.Textos[0].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado);
                frasePainelNovo.Modelo.Textos[1].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - tbarAltura.Value;
                frasePainelNovo.Modelo.Textos[2].AlturaPainel = tbarAltura.Value;

            }

            if (frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero)
            {
                frasePainelNovo.Modelo.Textos[0].LarguraPainel = tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[1].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[2].LarguraPainel = tbarLargura.Value;

                frasePainelNovo.Modelo.Textos[0].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - tbarAltura.Value;
                frasePainelNovo.Modelo.Textos[1].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado);
                frasePainelNovo.Modelo.Textos[2].AlturaPainel = tbarAltura.Value;

            }

            if (frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo)
            {
                frasePainelNovo.Modelo.Textos[0].LarguraPainel = tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[1].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[2].LarguraPainel = tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[3].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;

                frasePainelNovo.Modelo.Textos[0].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - tbarAltura.Value;
                frasePainelNovo.Modelo.Textos[1].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - tbarAltura.Value;
                frasePainelNovo.Modelo.Textos[2].AlturaPainel = tbarAltura.Value;
                frasePainelNovo.Modelo.Textos[3].AlturaPainel = tbarAltura.Value;

            }

            //TODO: fazer a altura os 3 textos

            if (frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
            {
                frasePainelNovo.Modelo.Textos[0].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);
                frasePainelNovo.Modelo.Textos[1].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);
                frasePainelNovo.Modelo.Textos[2].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado);

                frasePainelNovo.Modelo.Textos[0].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - Convert.ToInt16(rangeAltura.UpperValue);
                frasePainelNovo.Modelo.Textos[1].AlturaPainel = Convert.ToInt16(rangeAltura.UpperValue) - Convert.ToInt16(rangeAltura.LowerValue);
                frasePainelNovo.Modelo.Textos[2].AlturaPainel = Convert.ToInt16(rangeAltura.LowerValue);
            }


            if (frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
            {
                frasePainelNovo.Modelo.Textos[0].LarguraPainel = tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[1].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[2].LarguraPainel = tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[3].LarguraPainel = tbarLargura.Value;

                frasePainelNovo.Modelo.Textos[0].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - Convert.ToInt16(rangeAltura.UpperValue);
                frasePainelNovo.Modelo.Textos[2].AlturaPainel = Convert.ToInt16(rangeAltura.UpperValue) - Convert.ToInt16(rangeAltura.LowerValue);
                frasePainelNovo.Modelo.Textos[3].AlturaPainel = Convert.ToInt16(rangeAltura.LowerValue);
            }


            if (frasePainelNovo.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
            {
                frasePainelNovo.Modelo.Textos[0].LarguraPainel = tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[1].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[2].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;
                frasePainelNovo.Modelo.Textos[3].LarguraPainel = fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado) - tbarLargura.Value;

                frasePainelNovo.Modelo.Textos[1].AlturaPainel = fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) - Convert.ToInt16(rangeAltura.UpperValue);
                frasePainelNovo.Modelo.Textos[2].AlturaPainel = Convert.ToInt16(rangeAltura.UpperValue) - Convert.ToInt16(rangeAltura.LowerValue);
                frasePainelNovo.Modelo.Textos[3].AlturaPainel = Convert.ToInt16(rangeAltura.LowerValue);
            }
            

            // A fraseGUI em textoeditor de roteiro e mensagem deve ter o tamanho dos paineis ajustados
            fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), frasePainelNovo);
            PreencherBitMap(frasePainelNovo);

            if (TextosEditor.Visible)
            {
                TextosEditor.fraseGUI = new Frase(frasePainelNovo);
                TextosEditor.HabilitarBotoesAlinhamentoAjusteV04();
            }

            if (TextosEditorMsg.Visible)
            {
                TextosEditorMsg.fraseGUI = new Frase(frasePainelNovo);
                TextosEditorMsg.HabilitarBotoesAlinhamentoAjusteV04();
            }

        }

        internal void SetarListaAlternanciaPainel(List<Persistencia.ItemAlternancia> lista)
        {
            fachada.SetarListaAlternanciaPainel(controladorSelecionado, painelSelecionado, lista);
        }

        public void PreencherBitMap(Frase frase)
        {
            //verifica se houve alteração nos paineis entre o objeto FraseGUI e o Novo objeto que foi modificado pelo usuário
            bool alterou = VerificarAlteracaoPaineisBitmap(frase);

            if (alterou)
            {
                listaVideoBitMap.Clear();
                panelVideoBitmap.Controls.Clear();
            }

            fraseGUI = new Frase(frase);

            ultimoIndexPainelExibido = 0;

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
            {
                if (alterou)
                    listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fachada.GetAlturaPainel(controladorSelecionado, cbPainel.SelectedIndex), fachada.GetLarguraPainel(controladorSelecionado, cbPainel.SelectedIndex)));

                listaVideoBitMap[0].Bitmap = (Color[,])frase.Modelo.Textos[0].listaBitMap[ultimoIndexPainelExibido].Clone();
                listaVideoBitMap[0].listaBitMap = frase.Modelo.Textos[0].listaBitMap;
            }

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V04)
            {
                if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
                {
                    if (alterou)
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[0].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[0].listaBitMap[0].GetLength(1)));

                    listaVideoBitMap[0].listaBitMap = fraseGUI.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[0].Bitmap = (Color[,])fraseGUI.Modelo.Textos[0].listaBitMap[ultimoIndexPainelExibido].Clone();
                }

                if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[0].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[1].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                    }

                    listaVideoBitMap[0].listaBitMap = fraseGUI.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[0].Bitmap = (Color[,])fraseGUI.Modelo.Textos[0].listaBitMap[ultimoIndexPainelExibido].Clone();

                    listaVideoBitMap[1].listaBitMap = fraseGUI.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[1].Bitmap = (Color[,])fraseGUI.Modelo.Textos[1].listaBitMap[ultimoIndexPainelExibido].Clone();

                }

                if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[0].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[1].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[2].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                    }

                    listaVideoBitMap[0].listaBitMap = fraseGUI.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[0].Bitmap = (Color[,])fraseGUI.Modelo.Textos[0].listaBitMap[ultimoIndexPainelExibido].Clone();

                    listaVideoBitMap[1].listaBitMap = fraseGUI.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[1].Bitmap = (Color[,])fraseGUI.Modelo.Textos[1].listaBitMap[ultimoIndexPainelExibido].Clone();

                    listaVideoBitMap[2].listaBitMap = fraseGUI.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[2].Bitmap = (Color[,])fraseGUI.Modelo.Textos[2].listaBitMap[ultimoIndexPainelExibido].Clone();
                }

                if (fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero || fraseGUI.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[0].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[1].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[2].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(fraseGUI.Modelo.Textos[3].listaBitMap[0].GetLength(0), fraseGUI.Modelo.Textos[3].listaBitMap[0].GetLength(1)));
                    }

                    listaVideoBitMap[0].listaBitMap = fraseGUI.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[0].Bitmap = (Color[,])fraseGUI.Modelo.Textos[0].listaBitMap[ultimoIndexPainelExibido].Clone();

                    listaVideoBitMap[1].listaBitMap = fraseGUI.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[1].Bitmap = (Color[,])fraseGUI.Modelo.Textos[1].listaBitMap[ultimoIndexPainelExibido].Clone();

                    listaVideoBitMap[2].listaBitMap = fraseGUI.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[2].Bitmap = (Color[,])fraseGUI.Modelo.Textos[2].listaBitMap[ultimoIndexPainelExibido].Clone();

                    listaVideoBitMap[3].listaBitMap = fraseGUI.Modelo.Textos[3].listaBitMap;
                    listaVideoBitMap[3].Bitmap = (Color[,])fraseGUI.Modelo.Textos[3].listaBitMap[ultimoIndexPainelExibido].Clone();
                }
            }

            if (alterou)
            {
                if (TextosEditor.Visible || TextosEditorMsg.Visible)
                    ReposicionarVideoMapNoPainel(true);
                else
                    ReposicionarVideoMapNoPainel(false);
            }

            //Bloqueando os botoes de navegação
            button1.Enabled = false;

            //Setando o botão apresentar e o objeto frase que será exibido ao apresentar
            btnApresentar.Enabled = true;

            //Habilitar botoes de navegação lateral do painel e botoes de alinhamento horizontal em cada usercontrol se o texto for maior que o painel
            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V02)
            {
                if (listaVideoBitMap[0].listaBitMap.Count > 1)
                {
                    button2.Enabled = true;

                    if (NumEditor.Visible)
                        NumEditor.HabilitarAlinhamentoHorizontal(false);

                    if (TextosEditor.Visible)
                        TextosEditor.HabilitarAlinhamentoHorizontal(false);

                    if (TextosEditorMsg.Visible)
                        TextosEditorMsg.HabilitarAlinhamentoHorizontal(false);

                    if (textosEditorMsgEsp1.Visible)
                        textosEditorMsgEsp1.HabilitarAlinhamentoHorizontal(false);
                }
                else
                {
                    button2.Enabled = false;

                    if (NumEditor.Visible)
                        NumEditor.HabilitarAlinhamentoHorizontal(true);

                    if (TextosEditor.Visible)
                        TextosEditor.HabilitarAlinhamentoHorizontal(true);

                    if (TextosEditorMsg.Visible)
                        TextosEditorMsg.HabilitarAlinhamentoHorizontal(true);

                    if (textosEditorMsgEsp1.Visible)
                        textosEditorMsgEsp1.HabilitarAlinhamentoHorizontal(true);
                }
            }

            if (fraseGUI.TipoVideo == Util.Util.TipoVideo.V04)
            {
                bool habilitarBotao = false;
                for (int i = 0; i < listaVideoBitMap.Count; i++)
                {
                    if (listaVideoBitMap[i].listaBitMap.Count > 1)
                    {
                        habilitarBotao = true;
                        break;
                    }
                }
                button2.Enabled = habilitarBotao;

                if (TextosEditor.Visible)
                    if (TextosEditor.fraseGUI.Modelo.Textos[TextosEditor.textoSelecionado].listaBitMap.Count > 1)
                        TextosEditor.HabilitarAlinhamentoHorizontal(false);
                    else
                        TextosEditor.HabilitarAlinhamentoHorizontal(true);

                if (TextosEditorMsg.Visible)
                    if (TextosEditorMsg.fraseGUI.Modelo.Textos[TextosEditorMsg.textoSelecionado].listaBitMap.Count > 1)
                        TextosEditorMsg.HabilitarAlinhamentoHorizontal(false);
                    else
                        TextosEditorMsg.HabilitarAlinhamentoHorizontal(true);
            }

        }

        public void ReposicionarVideoMapNoPainel(Frase f)
        {
            bool alterou = VerificarAlteracaoPaineisBitmap(f);

            fraseGUI = new Frase(f);

            if (alterou)
            {
                listaVideoBitMap.Clear();
                panelVideoBitmap.Controls.Clear();
            }

            if (f.TipoVideo == Util.Util.TipoVideo.V02)
            {
                if (alterou)
                {
                    listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                    panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                    listaVideoBitMap[0].Location = new Point(0, 0);

                    listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                }

                listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;

            }

            if (f.TipoVideo == Util.Util.TipoVideo.V04)
            {
                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        listaVideoBitMap[0].Location = new Point(0, 0);

                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero)
                {

                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);

                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(0, listaVideoBitMap[0].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                }


                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero)
                {

                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));

                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);

                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[3].listaBitMap[0].GetLength(0), f.Modelo.Textos[3].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);
                        listaVideoBitMap[3].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[3].Apaga(0, f.Modelo.Textos[3].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[3].listaBitMap = f.Modelo.Textos[3].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(0, listaVideoBitMap[0].Height);
                        listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height + listaVideoBitMap[1].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[3].listaBitMap[0].GetLength(0), f.Modelo.Textos[3].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                        listaVideoBitMap[3].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height + listaVideoBitMap[2].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[3].Apaga(0, f.Modelo.Textos[3].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[3].listaBitMap = f.Modelo.Textos[3].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[3].listaBitMap[0].GetLength(0), f.Modelo.Textos[3].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);
                        listaVideoBitMap[3].Location = new Point(0, listaVideoBitMap[0].Height + listaVideoBitMap[2].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[3].Apaga(0, f.Modelo.Textos[3].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[3].listaBitMap = f.Modelo.Textos[3].listaBitMap;
                }
            }
        }

        private bool VerificarAlteracaoPaineisBitmap(Frase frase)
        {
            //verificação se foi alterado algum painel
            bool alterou = false;

            if (fraseGUI != null)
            {
                if (fraseGUI.TipoVideo != frase.TipoVideo)
                    alterou = true;

                if (!alterou)
                {
                    if (frase.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        if (fraseGUI.Modelo.TipoModelo != frase.Modelo.TipoModelo)
                            alterou = true;
                        else
                        {
                            for (int i = 0; i < frase.Modelo.Textos.Count; i++)
                            {
                                if (frase.Modelo.Textos[i].listaBitMap[0].GetLength(0) != fraseGUI.Modelo.Textos[i].listaBitMap[0].GetLength(0) || frase.Modelo.Textos[i].listaBitMap[0].GetLength(1) != fraseGUI.Modelo.Textos[i].listaBitMap[0].GetLength(1))
                                {
                                    alterou = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
                alterou = true;

            return alterou;
        }

        public void DesenharPainel()
        {

            int alturaPainel = (fachada.GetMultiLinhas(controladorSelecionado, cbPainel.SelectedIndex) > 1 ? fachada.GetAlturaPainel(controladorSelecionado, cbPainel.SelectedIndex) / fachada.GetMultiLinhas(controladorSelecionado, cbPainel.SelectedIndex) : fachada.GetAlturaPainel(controladorSelecionado, cbPainel.SelectedIndex));
            //int alturaPainel = (fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) > 1 ? fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado) / fachada.GetMultiLinhas(controladorSelecionado, painelSelecionado) : fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado));

            panelVideoBitmap.Controls.Clear();
            listaVideoBitMap.Clear();
            listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(alturaPainel, fachada.GetLarguraPainel(controladorSelecionado, cbPainel.SelectedIndex)));
            //listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(alturaPainel, fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado)));
            fraseGUI = null;
            ReposicionarVideoMapNoPainel(false);
            btnApresentar.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        public void LimparPaineis()
        {
            for (int i = 0; i < listaVideoBitMap.Count; i++)
            {
                listaVideoBitMap[i].Apaga(0, Color.Black);
                listaVideoBitMap[i].Desenhar();
                listaVideoBitMap[i].Refresh();
            }
            fraseGUI = null;
            btnApresentar.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void SetarPainelSelecionado()
        {
            painelSelecionado = cbPainel.SelectedIndex;

            if (dataGridViewPaineis.RowCount > 0)
            {
                dataGridViewPaineis.CurrentCell = dataGridViewPaineis.Rows[painelSelecionado].Cells[dataGridViewPaineis.CurrentCell.ColumnIndex];
                dataGridViewPaineis.Rows[painelSelecionado].Selected = true;
            }

            NumEditor.painelSelecionado = painelSelecionado;
            listarRoteiros1.painelSelecionado = painelSelecionado;
            listarMensagens1.painelSelecionado = painelSelecionado;
            textosEditorMsgEsp1.painelSelecionado = painelSelecionado;
            EditorMensagens.painelSelecionado = painelSelecionado;
            EditorRoteiros.painelSelecionado = painelSelecionado;
            editorSimulacao.painelSelecionado = painelSelecionado;
            editorAgendamento.painelSelecionado = painelSelecionado;
            editarMotorista.painelSelecionado = painelSelecionado;
            MotoristaEditor.painelSelecionado = painelSelecionado;

            //Bloquear Abas que nao podem ter se for um painel multilinhas
            BloquearAbasPainelMultiLinhas();

        }

        #endregion

        #region Relatorios

        public void ImprimirRoteiros(bool imprimirTextos)
        {
            fachada.ImprimirArquivo(controladorSelecionado, painelSelecionado,  tab_Roteiros, imprimirTextos);
        }

        public void ImprimirMensagens(bool imprimirTextos)
        {
            fachada.ImprimirArquivo(controladorSelecionado, painelSelecionado, tab_Mensagens, imprimirTextos);
        }

        public void ImprimirMotoristas()
        {
            fachada.ImprimirArquivo(controladorSelecionado, painelSelecionado, tab_Motoristas, false);
        }

        #endregion

        #region Tarifas

        public void MudarTarifaTodosPaineis(int tarifa)
        {
            fachada.MudarTarifaTodosRoteiros(controladorSelecionado, tarifa);
        }

        public int MudarTarifaDePara(int tarifaDe, int tarifaPara)
        {
            return fachada.MudarTarifaDePara(controladorSelecionado, tarifaDe, tarifaPara);
        }

        public bool PodeEditarTarifas()
        {
            bool editar = true;

            if (EditorRoteiros.Visible || NumEditor.Visible || TextosEditor.Visible)
                editar = false;

            return editar;
        }

        #endregion

        #region Cursor

        public void CursorWait()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.UseWaitCursor = true;
        }

        public void CursorDefault()
        {
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        #endregion

        private void cbPerifericos_Click(object sender, EventArgs e)
        {
            FormPerifericoAnexo form = new FormPerifericoAnexo();
            form.controladorSelecionado = this.controladorSelecionado;
            form.ShowDialog();

            habilitarCheckPerifericos();
        }

        private void cbPerifericos_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Focused)
                habilitarCheckPerifericos();
        }

        private void habilitarCheckPerifericos()
        {
            chkPerifericos.Checked = fachada.ExistePerifericoSelecionado(controladorSelecionado);
        }

        private void Arquivo_Load(object sender, EventArgs e)
        {
            VerificarJanela();
        }

        private void VerificarJanela()
        {
            //tamanho minimo para a tela de configuraçao ser exibida corretamente maximizada
            if (Screen.PrimaryScreen.Bounds.Height < Util.Util.ALTURA_MINIMA_JANELA_PRINCIPAL || Screen.PrimaryScreen.Bounds.Width < Util.Util.LARGURA_MINIMA_JANELA_PRINCIPAL)
            {                
                this.MaximizeBox = false;
                this.MaximumSize = new Size(Util.Util.LARGURA_MINIMA_JANELA_ARQUIVO, Util.Util.ALTURA_MINIMA_JANELA_ARQUIVO);
                bool Result = MoveWindow(this.Handle, this.Left, this.Top, Util.Util.LARGURA_MINIMA_JANELA_ARQUIVO, Util.Util.ALTURA_MINIMA_JANELA_ARQUIVO, true);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
            else
            {
                this.MaximizeBox = true;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.MinimumSize = new Size(Util.Util.LARGURA_MINIMA_JANELA_ARQUIVO, Util.Util.ALTURA_MINIMA_JANELA_ARQUIVO);
            }
        }

        private void toolStripMenuIncluir_Click(object sender, EventArgs e)
        {
            //AdicionarPainel();
        }

        private void toolStripMenuExcluir_Click(object sender, EventArgs e)
        {
            //RemoverPainel();
        }

        private void toolStripMenuCopiar_Click(object sender, EventArgs e)
        {
            //CopiarPainel();
        }

        private void CopiarPainel()
        {
            int painel = painelSelecionado;

            //Copiar painel na fachada
            fachada.CopiarPainel(controladorSelecionado, painelSelecionado);

            //Popula paineis na GUI
            LimpaDataGrid();
            PopulaPaineisDataGrid(fachada.CarregarPaineis(controladorSelecionado));
            PopulaPaineisComboBox(fachada.CarregarPaineis(controladorSelecionado));

            //Setando o painel selecionado    
            cbPainel.SelectedIndex = painel;        
            SetarPainelSelecionado();

            //setando tamanho do componente VideoBitmap do painel principal
            DesenharPainel();

        }

        private void dataGridViewPaineis_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Right && !dataGridViewPaineis.IsCurrentCellInEditMode)
            //{
            //    rowIndexFromMouseDown = dataGridViewPaineis.HitTest(e.X, e.Y).RowIndex;
            //    if (rowIndexFromMouseDown != -1)
            //    {
            //        //Setando o painel selecionado
            //        cbPainel.SelectedIndex = rowIndexFromMouseDown;
            //        SetarPainelSelecionado();

            //        //setando tamanho do componente VideoBitmap do painel principal
            //        DesenharPainel();

            //        //exibindo o Menu de contexto
            //        contextMenuPainel.Show(dataGridViewPaineis, e.Location);
            //    }

            //}
        }

        private void btnCopiarPainel_Click(object sender, EventArgs e)
        {
            CopiarPainel();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHabilitaModoLD6.Focused)
            {
                fachada.AplicarModoApresentacaoLD6(controladorSelecionado, chkHabilitaModoLD6.Checked);
            }
        }
    }
}
