namespace LPontos2
{
    partial class FormAtivacao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAtivacao));
            this.lbVersao = new System.Windows.Forms.Label();
            this.textBoxChave4 = new System.Windows.Forms.TextBox();
            this.textBoxChave3 = new System.Windows.Forms.TextBox();
            this.textBoxChave2 = new System.Windows.Forms.TextBox();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btValidar = new System.Windows.Forms.Button();
            this.textBoxChave1 = new System.Windows.Forms.TextBox();
            this.textBoxEmpresa = new System.Windows.Forms.TextBox();
            this.lbChave = new System.Windows.Forms.Label();
            this.lbEmpresa = new System.Windows.Forms.Label();
            this.lbTexto = new System.Windows.Forms.Label();
            this.pboxChave = new System.Windows.Forms.PictureBox();
            this.btGerar = new System.Windows.Forms.Button();
            this.groupBoxValidarAtivacao = new System.Windows.Forms.GroupBox();
            this.groupBoxCadastro = new System.Windows.Forms.GroupBox();
            this.labelFecharDDD = new System.Windows.Forms.Label();
            this.labelAbrirDDD = new System.Windows.Forms.Label();
            this.tboxDDD = new System.Windows.Forms.TextBox();
            this.cboxDDI = new System.Windows.Forms.ComboBox();
            this.labelCadastroEmail = new System.Windows.Forms.Label();
            this.tbCadastroTelefone = new System.Windows.Forms.TextBox();
            this.tbCadastroNomeContato = new System.Windows.Forms.TextBox();
            this.labelCadastroEmpresa = new System.Windows.Forms.Label();
            this.labelCadastroContato = new System.Windows.Forms.Label();
            this.labelCadastroTelefone = new System.Windows.Forms.Label();
            this.tbCadastroEmpresa = new System.Windows.Forms.TextBox();
            this.btEnviar = new System.Windows.Forms.Button();
            this.tbCadastroEmail = new System.Windows.Forms.TextBox();
            this.buttonCancelarEnvio = new System.Windows.Forms.Button();
            this.radioButtonSim = new System.Windows.Forms.RadioButton();
            this.radioButtonNao = new System.Windows.Forms.RadioButton();
            this.labelTemChaveAtivacao = new System.Windows.Forms.Label();
            this.gboxChave3dias = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbValidade = new System.Windows.Forms.Label();
            this.btContinuar = new System.Windows.Forms.Button();
            this.btCancelar3dias = new System.Windows.Forms.Button();
            this.imageListBandeiras = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pboxChave)).BeginInit();
            this.groupBoxValidarAtivacao.SuspendLayout();
            this.groupBoxCadastro.SuspendLayout();
            this.gboxChave3dias.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbVersao
            // 
            this.lbVersao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbVersao.BackColor = System.Drawing.Color.White;
            this.lbVersao.Location = new System.Drawing.Point(7, 505);
            this.lbVersao.Name = "lbVersao";
            this.lbVersao.Size = new System.Drawing.Size(183, 21);
            this.lbVersao.TabIndex = 19;
            this.lbVersao.Text = "versão: 12.2.7.0";
            this.lbVersao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxChave4
            // 
            this.textBoxChave4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxChave4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxChave4.Location = new System.Drawing.Point(241, 71);
            this.textBoxChave4.MaxLength = 80;
            this.textBoxChave4.Name = "textBoxChave4";
            this.textBoxChave4.Size = new System.Drawing.Size(156, 20);
            this.textBoxChave4.TabIndex = 13;
            this.textBoxChave4.TextChanged += new System.EventHandler(this.textBoxChave4_TextChanged);
            // 
            // textBoxChave3
            // 
            this.textBoxChave3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxChave3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxChave3.Location = new System.Drawing.Point(82, 71);
            this.textBoxChave3.MaxLength = 80;
            this.textBoxChave3.Name = "textBoxChave3";
            this.textBoxChave3.Size = new System.Drawing.Size(156, 20);
            this.textBoxChave3.TabIndex = 12;
            this.textBoxChave3.TextChanged += new System.EventHandler(this.textBoxChave3_TextChanged);
            // 
            // textBoxChave2
            // 
            this.textBoxChave2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxChave2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxChave2.Location = new System.Drawing.Point(241, 45);
            this.textBoxChave2.MaxLength = 80;
            this.textBoxChave2.Name = "textBoxChave2";
            this.textBoxChave2.Size = new System.Drawing.Size(156, 20);
            this.textBoxChave2.TabIndex = 11;
            this.textBoxChave2.TextChanged += new System.EventHandler(this.textBoxChave2_TextChanged);
            // 
            // btCancelar
            // 
            this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancelar.Location = new System.Drawing.Point(322, 97);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 24);
            this.btCancelar.TabIndex = 15;
            this.btCancelar.Text = "&Cancelar";
            // 
            // btValidar
            // 
            this.btValidar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btValidar.Location = new System.Drawing.Point(241, 97);
            this.btValidar.Name = "btValidar";
            this.btValidar.Size = new System.Drawing.Size(75, 24);
            this.btValidar.TabIndex = 14;
            this.btValidar.Text = "&Validar";
            this.btValidar.Click += new System.EventHandler(this.buttonValidar_Click);
            // 
            // textBoxChave1
            // 
            this.textBoxChave1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxChave1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxChave1.Location = new System.Drawing.Point(82, 45);
            this.textBoxChave1.MaxLength = 80;
            this.textBoxChave1.Name = "textBoxChave1";
            this.textBoxChave1.Size = new System.Drawing.Size(156, 20);
            this.textBoxChave1.TabIndex = 10;
            this.textBoxChave1.TextChanged += new System.EventHandler(this.textBoxChave1_TextChanged);
            // 
            // textBoxEmpresa
            // 
            this.textBoxEmpresa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEmpresa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxEmpresa.Location = new System.Drawing.Point(82, 19);
            this.textBoxEmpresa.MaxLength = 50;
            this.textBoxEmpresa.Name = "textBoxEmpresa";
            this.textBoxEmpresa.Size = new System.Drawing.Size(315, 20);
            this.textBoxEmpresa.TabIndex = 8;
            // 
            // lbChave
            // 
            this.lbChave.Location = new System.Drawing.Point(9, 42);
            this.lbChave.Name = "lbChave";
            this.lbChave.Size = new System.Drawing.Size(70, 23);
            this.lbChave.TabIndex = 15;
            this.lbChave.Text = "Chave:";
            this.lbChave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbEmpresa
            // 
            this.lbEmpresa.Location = new System.Drawing.Point(9, 19);
            this.lbEmpresa.Name = "lbEmpresa";
            this.lbEmpresa.Size = new System.Drawing.Size(70, 23);
            this.lbEmpresa.TabIndex = 13;
            this.lbEmpresa.Text = "Empresa:";
            this.lbEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbTexto
            // 
            this.lbTexto.AutoSize = true;
            this.lbTexto.Location = new System.Drawing.Point(115, 50);
            this.lbTexto.Name = "lbTexto";
            this.lbTexto.Size = new System.Drawing.Size(204, 13);
            this.lbTexto.TabIndex = 11;
            this.lbTexto.Text = "Informe a empresa e a chave de ativação";
            // 
            // pboxChave
            // 
            this.pboxChave.Image = ((System.Drawing.Image)(resources.GetObject("pboxChave.Image")));
            this.pboxChave.Location = new System.Drawing.Point(10, 70);
            this.pboxChave.Name = "pboxChave";
            this.pboxChave.Size = new System.Drawing.Size(99, 97);
            this.pboxChave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pboxChave.TabIndex = 9;
            this.pboxChave.TabStop = false;
            // 
            // btGerar
            // 
            this.btGerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btGerar.Location = new System.Drawing.Point(160, 97);
            this.btGerar.Name = "btGerar";
            this.btGerar.Size = new System.Drawing.Size(75, 24);
            this.btGerar.TabIndex = 22;
            this.btGerar.Text = "&Gerar";
            this.btGerar.Click += new System.EventHandler(this.btGerar_Click);
            // 
            // groupBoxValidarAtivacao
            // 
            this.groupBoxValidarAtivacao.Controls.Add(this.btGerar);
            this.groupBoxValidarAtivacao.Controls.Add(this.textBoxChave1);
            this.groupBoxValidarAtivacao.Controls.Add(this.lbEmpresa);
            this.groupBoxValidarAtivacao.Controls.Add(this.lbChave);
            this.groupBoxValidarAtivacao.Controls.Add(this.textBoxEmpresa);
            this.groupBoxValidarAtivacao.Controls.Add(this.textBoxChave4);
            this.groupBoxValidarAtivacao.Controls.Add(this.btValidar);
            this.groupBoxValidarAtivacao.Controls.Add(this.textBoxChave3);
            this.groupBoxValidarAtivacao.Controls.Add(this.btCancelar);
            this.groupBoxValidarAtivacao.Controls.Add(this.textBoxChave2);
            this.groupBoxValidarAtivacao.Location = new System.Drawing.Point(118, 237);
            this.groupBoxValidarAtivacao.Name = "groupBoxValidarAtivacao";
            this.groupBoxValidarAtivacao.Size = new System.Drawing.Size(406, 131);
            this.groupBoxValidarAtivacao.TabIndex = 23;
            this.groupBoxValidarAtivacao.TabStop = false;
            this.groupBoxValidarAtivacao.Visible = false;
            // 
            // groupBoxCadastro
            // 
            this.groupBoxCadastro.Controls.Add(this.labelFecharDDD);
            this.groupBoxCadastro.Controls.Add(this.labelAbrirDDD);
            this.groupBoxCadastro.Controls.Add(this.tboxDDD);
            this.groupBoxCadastro.Controls.Add(this.cboxDDI);
            this.groupBoxCadastro.Controls.Add(this.labelCadastroEmail);
            this.groupBoxCadastro.Controls.Add(this.tbCadastroTelefone);
            this.groupBoxCadastro.Controls.Add(this.tbCadastroNomeContato);
            this.groupBoxCadastro.Controls.Add(this.labelCadastroEmpresa);
            this.groupBoxCadastro.Controls.Add(this.labelCadastroContato);
            this.groupBoxCadastro.Controls.Add(this.labelCadastroTelefone);
            this.groupBoxCadastro.Controls.Add(this.tbCadastroEmpresa);
            this.groupBoxCadastro.Controls.Add(this.btEnviar);
            this.groupBoxCadastro.Controls.Add(this.tbCadastroEmail);
            this.groupBoxCadastro.Controls.Add(this.buttonCancelarEnvio);
            this.groupBoxCadastro.Location = new System.Drawing.Point(118, 63);
            this.groupBoxCadastro.Name = "groupBoxCadastro";
            this.groupBoxCadastro.Size = new System.Drawing.Size(406, 158);
            this.groupBoxCadastro.TabIndex = 24;
            this.groupBoxCadastro.TabStop = false;
            this.groupBoxCadastro.Visible = false;
            // 
            // labelFecharDDD
            // 
            this.labelFecharDDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFecharDDD.Location = new System.Drawing.Point(223, 70);
            this.labelFecharDDD.Name = "labelFecharDDD";
            this.labelFecharDDD.Size = new System.Drawing.Size(10, 23);
            this.labelFecharDDD.TabIndex = 24;
            this.labelFecharDDD.Text = ")";
            // 
            // labelAbrirDDD
            // 
            this.labelAbrirDDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAbrirDDD.Location = new System.Drawing.Point(172, 70);
            this.labelAbrirDDD.Name = "labelAbrirDDD";
            this.labelAbrirDDD.Size = new System.Drawing.Size(10, 23);
            this.labelAbrirDDD.TabIndex = 23;
            this.labelAbrirDDD.Text = "(";
            // 
            // tboxDDD
            // 
            this.tboxDDD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tboxDDD.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tboxDDD.Location = new System.Drawing.Point(183, 70);
            this.tboxDDD.MaxLength = 8;
            this.tboxDDD.Name = "tboxDDD";
            this.tboxDDD.Size = new System.Drawing.Size(40, 20);
            this.tboxDDD.TabIndex = 11;
            this.tboxDDD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tboxDDD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxDDD_KeyPress);
            // 
            // cboxDDI
            // 
            this.cboxDDI.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboxDDI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxDDI.FormattingEnabled = true;
            this.cboxDDI.ItemHeight = 14;
            this.cboxDDI.Items.AddRange(new object[] {
            "+93",
            "+355",
            "+213",
            "+1-684",
            "+376",
            "+244",
            "+1-264",
            "+672",
            "+1-268",
            "+54",
            "+374",
            "+297",
            "+61",
            "+43",
            "+994",
            "+1-242",
            "+973",
            "+880",
            "+1-246",
            "+375",
            "+32",
            "+501",
            "+229",
            "+1-441",
            "+975",
            "+591",
            "+387",
            "+267",
            "+55",
            "+673",
            "+359",
            "+226",
            "+257",
            "+855",
            "+237",
            "+1",
            "+238",
            "+1-345",
            "+236",
            "+235",
            "+56",
            "+86",
            "+61",
            "+57",
            "+269",
            "+682",
            "+506",
            "+385",
            "+53",
            "+357",
            "+420",
            "+243",
            "+45",
            "+253",
            "+1-767",
            "+1-809",
            "+1-829",
            "+1-849",
            "+593",
            "+20",
            "+503",
            "+240",
            "+291",
            "+372",
            "+251",
            "+298",
            "+679",
            "+358",
            "+33",
            "+241",
            "+220",
            "+995",
            "+49",
            "+233",
            "+350",
            "+30",
            "+299",
            "+1-473",
            "+1-671",
            "+502",
            "+44-1481",
            "+224",
            "+245",
            "+592",
            "+509",
            "+504",
            "+852",
            "+36",
            "+354",
            "+91",
            "+62",
            "+98",
            "+964",
            "+353",
            "+44-1624",
            "+972",
            "+39",
            "+1-876",
            "+81",
            "+44-1534",
            "+962",
            "+7",
            "+254",
            "+686",
            "+383",
            "+965",
            "+996",
            "+856",
            "+371",
            "+961",
            "+266",
            "+231",
            "+218",
            "+423",
            "+370",
            "+352",
            "+853",
            "+389",
            "+261",
            "+265",
            "+60",
            "+960",
            "+223",
            "+356",
            "+692",
            "+222",
            "+230",
            "+52",
            "+691",
            "+373",
            "+377",
            "+976",
            "+382",
            "+1-664",
            "+212",
            "+258",
            "+95",
            "+264",
            "+674",
            "+977",
            "+599",
            "+31",
            "+687",
            "+64",
            "+505",
            "+227",
            "+234",
            "+850",
            "+47",
            "+968",
            "+92",
            "+680",
            "+970",
            "+507",
            "+675",
            "+595",
            "+51",
            "+63",
            "+48",
            "+351",
            "+1-787",
            "+1-939",
            "+974",
            "+242",
            "+262",
            "+40",
            "+7",
            "+250",
            "+1-758",
            "+685",
            "+378",
            "+239",
            "+966",
            "+221",
            "+381",
            "+248",
            "+232",
            "+65",
            "+421",
            "+386",
            "+677",
            "+252",
            "+27",
            "+82",
            "+34",
            "+94",
            "+249",
            "+597",
            "+268",
            "+46",
            "+41",
            "+963",
            "+886",
            "+992",
            "+255",
            "+66",
            "+228",
            "+676",
            "+1-868",
            "+216",
            "+90",
            "+993",
            "+1-649",
            "+688",
            "+256",
            "+380",
            "+971",
            "+44",
            "+1",
            "+598",
            "+998",
            "+678",
            "+379",
            "+58",
            "+84",
            "+212",
            "+967",
            "+260",
            "+263"});
            this.cboxDDI.Location = new System.Drawing.Point(82, 70);
            this.cboxDDI.Name = "cboxDDI";
            this.cboxDDI.Size = new System.Drawing.Size(90, 20);
            this.cboxDDI.TabIndex = 10;
            this.cboxDDI.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboxDDI_DrawItem);
            // 
            // labelCadastroEmail
            // 
            this.labelCadastroEmail.Location = new System.Drawing.Point(9, 96);
            this.labelCadastroEmail.Name = "labelCadastroEmail";
            this.labelCadastroEmail.Size = new System.Drawing.Size(70, 23);
            this.labelCadastroEmail.TabIndex = 22;
            this.labelCadastroEmail.Text = "Email:";
            this.labelCadastroEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbCadastroTelefone
            // 
            this.tbCadastroTelefone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCadastroTelefone.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbCadastroTelefone.Location = new System.Drawing.Point(234, 70);
            this.tbCadastroTelefone.MaxLength = 16;
            this.tbCadastroTelefone.Name = "tbCadastroTelefone";
            this.tbCadastroTelefone.Size = new System.Drawing.Size(163, 20);
            this.tbCadastroTelefone.TabIndex = 12;
            this.tbCadastroTelefone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbCadastroTelefone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCadastroTelefone_KeyPress);
            // 
            // tbCadastroNomeContato
            // 
            this.tbCadastroNomeContato.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCadastroNomeContato.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbCadastroNomeContato.Location = new System.Drawing.Point(82, 45);
            this.tbCadastroNomeContato.MaxLength = 50;
            this.tbCadastroNomeContato.Name = "tbCadastroNomeContato";
            this.tbCadastroNomeContato.Size = new System.Drawing.Size(315, 20);
            this.tbCadastroNomeContato.TabIndex = 9;
            // 
            // labelCadastroEmpresa
            // 
            this.labelCadastroEmpresa.Location = new System.Drawing.Point(9, 19);
            this.labelCadastroEmpresa.Name = "labelCadastroEmpresa";
            this.labelCadastroEmpresa.Size = new System.Drawing.Size(70, 23);
            this.labelCadastroEmpresa.TabIndex = 13;
            this.labelCadastroEmpresa.Text = "Empresa:";
            this.labelCadastroEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCadastroContato
            // 
            this.labelCadastroContato.Location = new System.Drawing.Point(9, 45);
            this.labelCadastroContato.Name = "labelCadastroContato";
            this.labelCadastroContato.Size = new System.Drawing.Size(70, 23);
            this.labelCadastroContato.TabIndex = 21;
            this.labelCadastroContato.Text = "Contato:";
            this.labelCadastroContato.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCadastroTelefone
            // 
            this.labelCadastroTelefone.Location = new System.Drawing.Point(9, 71);
            this.labelCadastroTelefone.Name = "labelCadastroTelefone";
            this.labelCadastroTelefone.Size = new System.Drawing.Size(70, 23);
            this.labelCadastroTelefone.TabIndex = 15;
            this.labelCadastroTelefone.Text = "Telefone:";
            this.labelCadastroTelefone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbCadastroEmpresa
            // 
            this.tbCadastroEmpresa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCadastroEmpresa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbCadastroEmpresa.Location = new System.Drawing.Point(82, 19);
            this.tbCadastroEmpresa.MaxLength = 50;
            this.tbCadastroEmpresa.Name = "tbCadastroEmpresa";
            this.tbCadastroEmpresa.Size = new System.Drawing.Size(315, 20);
            this.tbCadastroEmpresa.TabIndex = 8;
            // 
            // btEnviar
            // 
            this.btEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEnviar.Location = new System.Drawing.Point(241, 122);
            this.btEnviar.Name = "btEnviar";
            this.btEnviar.Size = new System.Drawing.Size(75, 24);
            this.btEnviar.TabIndex = 14;
            this.btEnviar.Text = "&Enviar";
            this.btEnviar.Click += new System.EventHandler(this.btEnviar_Click);
            // 
            // tbCadastroEmail
            // 
            this.tbCadastroEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCadastroEmail.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbCadastroEmail.Location = new System.Drawing.Point(82, 96);
            this.tbCadastroEmail.MaxLength = 40;
            this.tbCadastroEmail.Name = "tbCadastroEmail";
            this.tbCadastroEmail.Size = new System.Drawing.Size(315, 20);
            this.tbCadastroEmail.TabIndex = 13;
            // 
            // buttonCancelarEnvio
            // 
            this.buttonCancelarEnvio.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelarEnvio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancelarEnvio.Location = new System.Drawing.Point(322, 122);
            this.buttonCancelarEnvio.Name = "buttonCancelarEnvio";
            this.buttonCancelarEnvio.Size = new System.Drawing.Size(75, 24);
            this.buttonCancelarEnvio.TabIndex = 15;
            this.buttonCancelarEnvio.Text = "&Cancelar";
            // 
            // radioButtonSim
            // 
            this.radioButtonSim.AutoSize = true;
            this.radioButtonSim.Checked = true;
            this.radioButtonSim.Location = new System.Drawing.Point(227, 8);
            this.radioButtonSim.Name = "radioButtonSim";
            this.radioButtonSim.Size = new System.Drawing.Size(42, 17);
            this.radioButtonSim.TabIndex = 25;
            this.radioButtonSim.TabStop = true;
            this.radioButtonSim.Text = "Sim";
            this.radioButtonSim.UseVisualStyleBackColor = true;
            this.radioButtonSim.CheckedChanged += new System.EventHandler(this.radioButtonSim_CheckedChanged);
            // 
            // radioButtonNao
            // 
            this.radioButtonNao.AutoSize = true;
            this.radioButtonNao.Location = new System.Drawing.Point(287, 8);
            this.radioButtonNao.Name = "radioButtonNao";
            this.radioButtonNao.Size = new System.Drawing.Size(45, 17);
            this.radioButtonNao.TabIndex = 26;
            this.radioButtonNao.Text = "Não";
            this.radioButtonNao.UseVisualStyleBackColor = true;
            this.radioButtonNao.CheckedChanged += new System.EventHandler(this.radioButtonNao_CheckedChanged);
            // 
            // labelTemChaveAtivacao
            // 
            this.labelTemChaveAtivacao.AutoSize = true;
            this.labelTemChaveAtivacao.Location = new System.Drawing.Point(7, 10);
            this.labelTemChaveAtivacao.Name = "labelTemChaveAtivacao";
            this.labelTemChaveAtivacao.Size = new System.Drawing.Size(197, 13);
            this.labelTemChaveAtivacao.TabIndex = 27;
            this.labelTemChaveAtivacao.Text = "Você já possui uma chave de ativação?";
            // 
            // gboxChave3dias
            // 
            this.gboxChave3dias.Controls.Add(this.label2);
            this.gboxChave3dias.Controls.Add(this.label1);
            this.gboxChave3dias.Controls.Add(this.lbValidade);
            this.gboxChave3dias.Controls.Add(this.btContinuar);
            this.gboxChave3dias.Controls.Add(this.btCancelar3dias);
            this.gboxChave3dias.Location = new System.Drawing.Point(118, 383);
            this.gboxChave3dias.Name = "gboxChave3dias";
            this.gboxChave3dias.Size = new System.Drawing.Size(406, 131);
            this.gboxChave3dias.TabIndex = 24;
            this.gboxChave3dias.TabStop = false;
            this.gboxChave3dias.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "+55 81 30811850";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "www.frt.com.br";
            // 
            // lbValidade
            // 
            this.lbValidade.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbValidade.Location = new System.Drawing.Point(12, 31);
            this.lbValidade.Name = "lbValidade";
            this.lbValidade.Size = new System.Drawing.Size(385, 53);
            this.lbValidade.TabIndex = 13;
            this.lbValidade.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btContinuar
            // 
            this.btContinuar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btContinuar.Location = new System.Drawing.Point(241, 97);
            this.btContinuar.Name = "btContinuar";
            this.btContinuar.Size = new System.Drawing.Size(75, 24);
            this.btContinuar.TabIndex = 14;
            this.btContinuar.Text = "&Continuar";
            this.btContinuar.Click += new System.EventHandler(this.btContinuar_Click);
            // 
            // btCancelar3dias
            // 
            this.btCancelar3dias.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancelar3dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancelar3dias.Location = new System.Drawing.Point(322, 97);
            this.btCancelar3dias.Name = "btCancelar3dias";
            this.btCancelar3dias.Size = new System.Drawing.Size(75, 24);
            this.btCancelar3dias.TabIndex = 15;
            this.btCancelar3dias.Text = "&Cancelar";
            // 
            // imageListBandeiras
            // 
            this.imageListBandeiras.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBandeiras.ImageStream")));
            this.imageListBandeiras.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBandeiras.Images.SetKeyName(0, "Afghanistan.png");
            this.imageListBandeiras.Images.SetKeyName(1, "Albania.png");
            this.imageListBandeiras.Images.SetKeyName(2, "Algeria.png");
            this.imageListBandeiras.Images.SetKeyName(3, "American Samoa.png");
            this.imageListBandeiras.Images.SetKeyName(4, "Andorra.png");
            this.imageListBandeiras.Images.SetKeyName(5, "Angola.png");
            this.imageListBandeiras.Images.SetKeyName(6, "Anguilla.png");
            this.imageListBandeiras.Images.SetKeyName(7, "Antarctica.png");
            this.imageListBandeiras.Images.SetKeyName(8, "Antigua & Barbuda.png");
            this.imageListBandeiras.Images.SetKeyName(9, "Argentina.png");
            this.imageListBandeiras.Images.SetKeyName(10, "Armenia.png");
            this.imageListBandeiras.Images.SetKeyName(11, "Aruba.png");
            this.imageListBandeiras.Images.SetKeyName(12, "Australia.png");
            this.imageListBandeiras.Images.SetKeyName(13, "Austria.png");
            this.imageListBandeiras.Images.SetKeyName(14, "Azerbaijan.png");
            this.imageListBandeiras.Images.SetKeyName(15, "Bahamas.png");
            this.imageListBandeiras.Images.SetKeyName(16, "Bahrain.png");
            this.imageListBandeiras.Images.SetKeyName(17, "Bangladesh.png");
            this.imageListBandeiras.Images.SetKeyName(18, "Barbados.png");
            this.imageListBandeiras.Images.SetKeyName(19, "Belarus.png");
            this.imageListBandeiras.Images.SetKeyName(20, "Belgium.png");
            this.imageListBandeiras.Images.SetKeyName(21, "Belize.png");
            this.imageListBandeiras.Images.SetKeyName(22, "Benin.png");
            this.imageListBandeiras.Images.SetKeyName(23, "Bermuda.png");
            this.imageListBandeiras.Images.SetKeyName(24, "Bhutan.png");
            this.imageListBandeiras.Images.SetKeyName(25, "Bolivia.png");
            this.imageListBandeiras.Images.SetKeyName(26, "Bosnia & Herzegovina.png");
            this.imageListBandeiras.Images.SetKeyName(27, "Botswana.png");
            this.imageListBandeiras.Images.SetKeyName(28, "Brazil.png");
            this.imageListBandeiras.Images.SetKeyName(29, "Brunei.png");
            this.imageListBandeiras.Images.SetKeyName(30, "Bulgaria.png");
            this.imageListBandeiras.Images.SetKeyName(31, "Burkina Faso.png");
            this.imageListBandeiras.Images.SetKeyName(32, "Burundi.png");
            this.imageListBandeiras.Images.SetKeyName(33, "Cambodja.png");
            this.imageListBandeiras.Images.SetKeyName(34, "Cameroon.png");
            this.imageListBandeiras.Images.SetKeyName(35, "Canada.png");
            this.imageListBandeiras.Images.SetKeyName(36, "Cape Verde.png");
            this.imageListBandeiras.Images.SetKeyName(37, "Cayman Islands.png");
            this.imageListBandeiras.Images.SetKeyName(38, "Central African Republic.png");
            this.imageListBandeiras.Images.SetKeyName(39, "Chad.png");
            this.imageListBandeiras.Images.SetKeyName(40, "Chile.png");
            this.imageListBandeiras.Images.SetKeyName(41, "China.png");
            this.imageListBandeiras.Images.SetKeyName(42, "CIS.png");
            this.imageListBandeiras.Images.SetKeyName(43, "Colombia.png");
            this.imageListBandeiras.Images.SetKeyName(44, "Comoros.png");
            this.imageListBandeiras.Images.SetKeyName(45, "Cook Islands.png");
            this.imageListBandeiras.Images.SetKeyName(46, "Costa Rica.png");
            this.imageListBandeiras.Images.SetKeyName(47, "Croatia.png");
            this.imageListBandeiras.Images.SetKeyName(48, "Cuba.png");
            this.imageListBandeiras.Images.SetKeyName(49, "Cyprus.png");
            this.imageListBandeiras.Images.SetKeyName(50, "Czech Republic.png");
            this.imageListBandeiras.Images.SetKeyName(51, "Democratic Republic of the Congo.png");
            this.imageListBandeiras.Images.SetKeyName(52, "Denmark.png");
            this.imageListBandeiras.Images.SetKeyName(53, "Djibouti.png");
            this.imageListBandeiras.Images.SetKeyName(54, "Dominica.png");
            this.imageListBandeiras.Images.SetKeyName(55, "Dominican Republic.png");
            this.imageListBandeiras.Images.SetKeyName(56, "Dominican Republic2.png");
            this.imageListBandeiras.Images.SetKeyName(57, "Dominican Republic3.png");
            this.imageListBandeiras.Images.SetKeyName(58, "Ecuador.png");
            this.imageListBandeiras.Images.SetKeyName(59, "Egypt.png");
            this.imageListBandeiras.Images.SetKeyName(60, "El Salvador.png");
            this.imageListBandeiras.Images.SetKeyName(61, "Equatorial Guinea.png");
            this.imageListBandeiras.Images.SetKeyName(62, "Eritrea.png");
            this.imageListBandeiras.Images.SetKeyName(63, "Estonia.png");
            this.imageListBandeiras.Images.SetKeyName(64, "Ethiopia.png");
            this.imageListBandeiras.Images.SetKeyName(65, "Faroes.png");
            this.imageListBandeiras.Images.SetKeyName(66, "Fiji.png");
            this.imageListBandeiras.Images.SetKeyName(67, "Finland.png");
            this.imageListBandeiras.Images.SetKeyName(68, "France.png");
            this.imageListBandeiras.Images.SetKeyName(69, "Gabon.png");
            this.imageListBandeiras.Images.SetKeyName(70, "Gambia.png");
            this.imageListBandeiras.Images.SetKeyName(71, "Georgia.png");
            this.imageListBandeiras.Images.SetKeyName(72, "Germany.png");
            this.imageListBandeiras.Images.SetKeyName(73, "Ghana.png");
            this.imageListBandeiras.Images.SetKeyName(74, "Gibraltar.png");
            this.imageListBandeiras.Images.SetKeyName(75, "Greece.png");
            this.imageListBandeiras.Images.SetKeyName(76, "Greenland.png");
            this.imageListBandeiras.Images.SetKeyName(77, "Grenada.png");
            this.imageListBandeiras.Images.SetKeyName(78, "Guam.png");
            this.imageListBandeiras.Images.SetKeyName(79, "Guatemala.png");
            this.imageListBandeiras.Images.SetKeyName(80, "Guernsey.png");
            this.imageListBandeiras.Images.SetKeyName(81, "Guinea.png");
            this.imageListBandeiras.Images.SetKeyName(82, "Guinea-Bissau.png");
            this.imageListBandeiras.Images.SetKeyName(83, "Guyana.png");
            this.imageListBandeiras.Images.SetKeyName(84, "Haiti.png");
            this.imageListBandeiras.Images.SetKeyName(85, "Honduras.png");
            this.imageListBandeiras.Images.SetKeyName(86, "Hong Kong.png");
            this.imageListBandeiras.Images.SetKeyName(87, "Hungary.png");
            this.imageListBandeiras.Images.SetKeyName(88, "Iceland.png");
            this.imageListBandeiras.Images.SetKeyName(89, "India.png");
            this.imageListBandeiras.Images.SetKeyName(90, "Indonesia.png");
            this.imageListBandeiras.Images.SetKeyName(91, "Iran.png");
            this.imageListBandeiras.Images.SetKeyName(92, "Iraq.png");
            this.imageListBandeiras.Images.SetKeyName(93, "Ireland.png");
            this.imageListBandeiras.Images.SetKeyName(94, "Isle of Man.png");
            this.imageListBandeiras.Images.SetKeyName(95, "Israel.png");
            this.imageListBandeiras.Images.SetKeyName(96, "Italy.png");
            this.imageListBandeiras.Images.SetKeyName(97, "Jamaica.png");
            this.imageListBandeiras.Images.SetKeyName(98, "Japan.png");
            this.imageListBandeiras.Images.SetKeyName(99, "Jersey.png");
            this.imageListBandeiras.Images.SetKeyName(100, "Jordan.png");
            this.imageListBandeiras.Images.SetKeyName(101, "Kazakhstan.png");
            this.imageListBandeiras.Images.SetKeyName(102, "Kenya.png");
            this.imageListBandeiras.Images.SetKeyName(103, "Kiribati.png");
            this.imageListBandeiras.Images.SetKeyName(104, "Kosovo.png");
            this.imageListBandeiras.Images.SetKeyName(105, "Kuwait.png");
            this.imageListBandeiras.Images.SetKeyName(106, "Kyrgyzstan.png");
            this.imageListBandeiras.Images.SetKeyName(107, "Laos.png");
            this.imageListBandeiras.Images.SetKeyName(108, "Latvia.png");
            this.imageListBandeiras.Images.SetKeyName(109, "Lebanon.png");
            this.imageListBandeiras.Images.SetKeyName(110, "Lesotho.png");
            this.imageListBandeiras.Images.SetKeyName(111, "Liberia.png");
            this.imageListBandeiras.Images.SetKeyName(112, "Libya.png");
            this.imageListBandeiras.Images.SetKeyName(113, "Liechtenstein.png");
            this.imageListBandeiras.Images.SetKeyName(114, "Lithuania.png");
            this.imageListBandeiras.Images.SetKeyName(115, "Luxembourg.png");
            this.imageListBandeiras.Images.SetKeyName(116, "Macao.png");
            this.imageListBandeiras.Images.SetKeyName(117, "Macedonia.png");
            this.imageListBandeiras.Images.SetKeyName(118, "Madagascar.png");
            this.imageListBandeiras.Images.SetKeyName(119, "Malawi.png");
            this.imageListBandeiras.Images.SetKeyName(120, "Malaysia.png");
            this.imageListBandeiras.Images.SetKeyName(121, "Maldives.png");
            this.imageListBandeiras.Images.SetKeyName(122, "Mali.png");
            this.imageListBandeiras.Images.SetKeyName(123, "Malta.png");
            this.imageListBandeiras.Images.SetKeyName(124, "Marshall Islands.png");
            this.imageListBandeiras.Images.SetKeyName(125, "Mauritania.png");
            this.imageListBandeiras.Images.SetKeyName(126, "Mauritius.png");
            this.imageListBandeiras.Images.SetKeyName(127, "Mexico.png");
            this.imageListBandeiras.Images.SetKeyName(128, "Micronesia.png");
            this.imageListBandeiras.Images.SetKeyName(129, "Moldova.png");
            this.imageListBandeiras.Images.SetKeyName(130, "Monaco.png");
            this.imageListBandeiras.Images.SetKeyName(131, "Mongolia.png");
            this.imageListBandeiras.Images.SetKeyName(132, "Montenegro.png");
            this.imageListBandeiras.Images.SetKeyName(133, "Montserrat.png");
            this.imageListBandeiras.Images.SetKeyName(134, "Morocco.png");
            this.imageListBandeiras.Images.SetKeyName(135, "Mozambique.png");
            this.imageListBandeiras.Images.SetKeyName(136, "Myanmar(Burma).png");
            this.imageListBandeiras.Images.SetKeyName(137, "Namibia.png");
            this.imageListBandeiras.Images.SetKeyName(138, "Nauru.png");
            this.imageListBandeiras.Images.SetKeyName(139, "Nepal.png");
            this.imageListBandeiras.Images.SetKeyName(140, "Netherlands Antilles.png");
            this.imageListBandeiras.Images.SetKeyName(141, "Netherlands.png");
            this.imageListBandeiras.Images.SetKeyName(142, "New Caledonia.png");
            this.imageListBandeiras.Images.SetKeyName(143, "New Zealand.png");
            this.imageListBandeiras.Images.SetKeyName(144, "Nicaragua.png");
            this.imageListBandeiras.Images.SetKeyName(145, "Niger.png");
            this.imageListBandeiras.Images.SetKeyName(146, "Nigeria.png");
            this.imageListBandeiras.Images.SetKeyName(147, "North Korea.png");
            this.imageListBandeiras.Images.SetKeyName(148, "Norway.png");
            this.imageListBandeiras.Images.SetKeyName(149, "Oman.png");
            this.imageListBandeiras.Images.SetKeyName(150, "Pakistan.png");
            this.imageListBandeiras.Images.SetKeyName(151, "Palau.png");
            this.imageListBandeiras.Images.SetKeyName(152, "Palestine.png");
            this.imageListBandeiras.Images.SetKeyName(153, "Panama.png");
            this.imageListBandeiras.Images.SetKeyName(154, "Papua New Guinea.png");
            this.imageListBandeiras.Images.SetKeyName(155, "Paraguay.png");
            this.imageListBandeiras.Images.SetKeyName(156, "Peru.png");
            this.imageListBandeiras.Images.SetKeyName(157, "Philippines.png");
            this.imageListBandeiras.Images.SetKeyName(158, "Poland.png");
            this.imageListBandeiras.Images.SetKeyName(159, "Portugal.png");
            this.imageListBandeiras.Images.SetKeyName(160, "Puerto Rico.png");
            this.imageListBandeiras.Images.SetKeyName(161, "Puerto Rico2.png");
            this.imageListBandeiras.Images.SetKeyName(162, "Qatar.png");
            this.imageListBandeiras.Images.SetKeyName(163, "Republic of the Congo.png");
            this.imageListBandeiras.Images.SetKeyName(164, "Reunion.png");
            this.imageListBandeiras.Images.SetKeyName(165, "Romania.png");
            this.imageListBandeiras.Images.SetKeyName(166, "Russian Federation.png");
            this.imageListBandeiras.Images.SetKeyName(167, "Rwanda.png");
            this.imageListBandeiras.Images.SetKeyName(168, "Saint Lucia.png");
            this.imageListBandeiras.Images.SetKeyName(169, "Samoa.png");
            this.imageListBandeiras.Images.SetKeyName(170, "San Marino.png");
            this.imageListBandeiras.Images.SetKeyName(171, "Sao Tome & Principe.png");
            this.imageListBandeiras.Images.SetKeyName(172, "Saudi Arabia.png");
            this.imageListBandeiras.Images.SetKeyName(173, "Senegal.png");
            this.imageListBandeiras.Images.SetKeyName(174, "Serbia.png");
            this.imageListBandeiras.Images.SetKeyName(175, "Seyshelles.png");
            this.imageListBandeiras.Images.SetKeyName(176, "Sierra Leone.png");
            this.imageListBandeiras.Images.SetKeyName(177, "Singapore.png");
            this.imageListBandeiras.Images.SetKeyName(178, "Slovakia.png");
            this.imageListBandeiras.Images.SetKeyName(179, "Slovenia.png");
            this.imageListBandeiras.Images.SetKeyName(180, "Solomon Islands.png");
            this.imageListBandeiras.Images.SetKeyName(181, "Somalia.png");
            this.imageListBandeiras.Images.SetKeyName(182, "South Afriica.png");
            this.imageListBandeiras.Images.SetKeyName(183, "South Korea.png");
            this.imageListBandeiras.Images.SetKeyName(184, "Spain.png");
            this.imageListBandeiras.Images.SetKeyName(185, "Sri Lanka.png");
            this.imageListBandeiras.Images.SetKeyName(186, "Sudan.png");
            this.imageListBandeiras.Images.SetKeyName(187, "Suriname.png");
            this.imageListBandeiras.Images.SetKeyName(188, "Swaziland.png");
            this.imageListBandeiras.Images.SetKeyName(189, "Sweden.png");
            this.imageListBandeiras.Images.SetKeyName(190, "Switzerland.png");
            this.imageListBandeiras.Images.SetKeyName(191, "Syria.png");
            this.imageListBandeiras.Images.SetKeyName(192, "Taiwan.png");
            this.imageListBandeiras.Images.SetKeyName(193, "Tajikistan.png");
            this.imageListBandeiras.Images.SetKeyName(194, "Tanzania.png");
            this.imageListBandeiras.Images.SetKeyName(195, "Thailand.png");
            this.imageListBandeiras.Images.SetKeyName(196, "Togo.png");
            this.imageListBandeiras.Images.SetKeyName(197, "Tonga.png");
            this.imageListBandeiras.Images.SetKeyName(198, "Trinidad & Tobago.png");
            this.imageListBandeiras.Images.SetKeyName(199, "Tunisia.png");
            this.imageListBandeiras.Images.SetKeyName(200, "Turkey.png");
            this.imageListBandeiras.Images.SetKeyName(201, "Turkmenistan.png");
            this.imageListBandeiras.Images.SetKeyName(202, "Turks and Caicos Islands.png");
            this.imageListBandeiras.Images.SetKeyName(203, "Tuvalu.png");
            this.imageListBandeiras.Images.SetKeyName(204, "Uganda.png");
            this.imageListBandeiras.Images.SetKeyName(205, "Ukraine.png");
            this.imageListBandeiras.Images.SetKeyName(206, "United Arab Emirates.png");
            this.imageListBandeiras.Images.SetKeyName(207, "United Kingdom(Great Britain).png");
            this.imageListBandeiras.Images.SetKeyName(208, "United States of America.png");
            this.imageListBandeiras.Images.SetKeyName(209, "Uruguay.png");
            this.imageListBandeiras.Images.SetKeyName(210, "Uzbekistan.png");
            this.imageListBandeiras.Images.SetKeyName(211, "Vanutau.png");
            this.imageListBandeiras.Images.SetKeyName(212, "Vatican City.png");
            this.imageListBandeiras.Images.SetKeyName(213, "Venezuela.png");
            this.imageListBandeiras.Images.SetKeyName(214, "Viet Nam.png");
            this.imageListBandeiras.Images.SetKeyName(215, "Western Sahara.png");
            this.imageListBandeiras.Images.SetKeyName(216, "Yemen.png");
            this.imageListBandeiras.Images.SetKeyName(217, "Zambia.png");
            this.imageListBandeiras.Images.SetKeyName(218, "Zimbabwe.png");
            // 
            // FormAtivacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(535, 526);
            this.Controls.Add(this.gboxChave3dias);
            this.Controls.Add(this.labelTemChaveAtivacao);
            this.Controls.Add(this.radioButtonNao);
            this.Controls.Add(this.radioButtonSim);
            this.Controls.Add(this.lbVersao);
            this.Controls.Add(this.lbTexto);
            this.Controls.Add(this.pboxChave);
            this.Controls.Add(this.groupBoxValidarAtivacao);
            this.Controls.Add(this.groupBoxCadastro);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAtivacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ativar Pontos X2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAtivacao_FormClosing);
            this.Load += new System.EventHandler(this.FormAtivacao_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pboxChave)).EndInit();
            this.groupBoxValidarAtivacao.ResumeLayout(false);
            this.groupBoxValidarAtivacao.PerformLayout();
            this.groupBoxCadastro.ResumeLayout(false);
            this.groupBoxCadastro.PerformLayout();
            this.gboxChave3dias.ResumeLayout(false);
            this.gboxChave3dias.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbVersao;
        private System.Windows.Forms.TextBox textBoxChave4;
        private System.Windows.Forms.TextBox textBoxChave3;
        private System.Windows.Forms.TextBox textBoxChave2;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btValidar;
        private System.Windows.Forms.TextBox textBoxChave1;
        private System.Windows.Forms.TextBox textBoxEmpresa;
        private System.Windows.Forms.Label lbChave;
        private System.Windows.Forms.Label lbEmpresa;
        private System.Windows.Forms.Label lbTexto;
        private System.Windows.Forms.PictureBox pboxChave;
        private System.Windows.Forms.Button btGerar;
        private System.Windows.Forms.GroupBox groupBoxValidarAtivacao;
        private System.Windows.Forms.GroupBox groupBoxCadastro;
        private System.Windows.Forms.Label labelCadastroEmail;
        private System.Windows.Forms.TextBox tbCadastroTelefone;
        private System.Windows.Forms.TextBox tbCadastroNomeContato;
        private System.Windows.Forms.Label labelCadastroEmpresa;
        private System.Windows.Forms.Label labelCadastroContato;
        private System.Windows.Forms.Label labelCadastroTelefone;
        private System.Windows.Forms.TextBox tbCadastroEmpresa;
        private System.Windows.Forms.Button btEnviar;
        private System.Windows.Forms.TextBox tbCadastroEmail;
        private System.Windows.Forms.Button buttonCancelarEnvio;
        private System.Windows.Forms.RadioButton radioButtonSim;
        private System.Windows.Forms.RadioButton radioButtonNao;
        private System.Windows.Forms.Label labelTemChaveAtivacao;
        private System.Windows.Forms.GroupBox gboxChave3dias;
        private System.Windows.Forms.Label lbValidade;
        private System.Windows.Forms.Button btContinuar;
        private System.Windows.Forms.Button btCancelar3dias;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboxDDI;
        private System.Windows.Forms.ImageList imageListBandeiras;
        private System.Windows.Forms.TextBox tboxDDD;
        private System.Windows.Forms.Label labelFecharDDD;
        private System.Windows.Forms.Label labelAbrirDDD;
    }
}