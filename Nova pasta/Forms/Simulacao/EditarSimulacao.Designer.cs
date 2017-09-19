namespace LPontos2.Forms.Simulacao
{
    partial class EditorSimulacao
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gboxRoteiro = new System.Windows.Forms.GroupBox();
            this.lblTextoRoteiro = new System.Windows.Forms.Label();
            this.rbVolta = new System.Windows.Forms.RadioButton();
            this.rbIda = new System.Windows.Forms.RadioButton();
            this.cboxRoteiros = new System.Windows.Forms.ComboBox();
            this.lvRoteiros = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gboxMensagem = new System.Windows.Forms.GroupBox();
            this.cboxMensagens = new System.Windows.Forms.ComboBox();
            this.lvMensagens = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btSimular = new System.Windows.Forms.Button();
            this.gboxSaudacao = new System.Windows.Forms.GroupBox();
            this.labelMinbNoite = new System.Windows.Forms.Label();
            this.labelMinbTarde = new System.Windows.Forms.Label();
            this.labelMinbDia = new System.Windows.Forms.Label();
            this.labelBoaNoite = new System.Windows.Forms.Label();
            this.labelBoaTarde = new System.Windows.Forms.Label();
            this.labelBomDia = new System.Windows.Forms.Label();
            this.tboxBoaNoite = new System.Windows.Forms.TextBox();
            this.tboxBoaTarde = new System.Windows.Forms.TextBox();
            this.tboxBomDia = new System.Windows.Forms.TextBox();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.gboxSecMensagem = new System.Windows.Forms.GroupBox();
            this.cboxSecMensagem = new System.Windows.Forms.ComboBox();
            this.lvSecMensagens = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gboxInverterLed = new System.Windows.Forms.GroupBox();
            this.lblMinInverterLed = new System.Windows.Forms.Label();
            this.lblInverterLed = new System.Windows.Forms.Label();
            this.tboxMinInverterLed = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gboxMotorista = new System.Windows.Forms.GroupBox();
            this.cboxMotorista = new System.Windows.Forms.ComboBox();
            this.gboxAlternancia = new System.Windows.Forms.GroupBox();
            this.btBloquearAlternancia = new System.Windows.Forms.Button();
            this.cboxAlternancia = new System.Windows.Forms.ComboBox();
            this.groupBoxTimeOutSemComunicacao = new System.Windows.Forms.GroupBox();
            this.labelTimeOutSegundos = new System.Windows.Forms.Label();
            this.labelTimeoutFalhaRede = new System.Windows.Forms.Label();
            this.textBoxTimeoutFalhaRede = new System.Windows.Forms.TextBox();
            this.groupBoxBaudrate = new System.Windows.Forms.GroupBox();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.groupBoxTurnOffUSB = new System.Windows.Forms.GroupBox();
            this.checkBoxTurnUSBOff = new System.Windows.Forms.CheckBox();
            this.gboxRoteiro.SuspendLayout();
            this.gboxMensagem.SuspendLayout();
            this.gboxSaudacao.SuspendLayout();
            this.gboxSecMensagem.SuspendLayout();
            this.gboxInverterLed.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gboxMotorista.SuspendLayout();
            this.gboxAlternancia.SuspendLayout();
            this.groupBoxTimeOutSemComunicacao.SuspendLayout();
            this.groupBoxBaudrate.SuspendLayout();
            this.groupBoxTurnOffUSB.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxRoteiro
            // 
            this.gboxRoteiro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxRoteiro.Controls.Add(this.lblTextoRoteiro);
            this.gboxRoteiro.Controls.Add(this.rbVolta);
            this.gboxRoteiro.Controls.Add(this.rbIda);
            this.gboxRoteiro.Controls.Add(this.cboxRoteiros);
            this.gboxRoteiro.Controls.Add(this.lvRoteiros);
            this.gboxRoteiro.Location = new System.Drawing.Point(3, 3);
            this.gboxRoteiro.Name = "gboxRoteiro";
            this.gboxRoteiro.Size = new System.Drawing.Size(327, 323);
            this.gboxRoteiro.TabIndex = 2;
            this.gboxRoteiro.TabStop = false;
            this.gboxRoteiro.Text = "Roteiros";
            // 
            // lblTextoRoteiro
            // 
            this.lblTextoRoteiro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextoRoteiro.ForeColor = System.Drawing.Color.Red;
            this.lblTextoRoteiro.Location = new System.Drawing.Point(134, 49);
            this.lblTextoRoteiro.Name = "lblTextoRoteiro";
            this.lblTextoRoteiro.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTextoRoteiro.Size = new System.Drawing.Size(188, 15);
            this.lblTextoRoteiro.TabIndex = 6;
            this.lblTextoRoteiro.Text = "Textos de salida/retorno diferentes";
            // 
            // rbVolta
            // 
            this.rbVolta.AutoSize = true;
            this.rbVolta.Location = new System.Drawing.Point(83, 47);
            this.rbVolta.Name = "rbVolta";
            this.rbVolta.Size = new System.Drawing.Size(49, 17);
            this.rbVolta.TabIndex = 5;
            this.rbVolta.TabStop = true;
            this.rbVolta.Text = "Volta";
            this.rbVolta.UseVisualStyleBackColor = true;
            this.rbVolta.CheckedChanged += new System.EventHandler(this.rbVolta_CheckedChanged);
            // 
            // rbIda
            // 
            this.rbIda.AutoSize = true;
            this.rbIda.Location = new System.Drawing.Point(7, 47);
            this.rbIda.Name = "rbIda";
            this.rbIda.Size = new System.Drawing.Size(40, 17);
            this.rbIda.TabIndex = 4;
            this.rbIda.TabStop = true;
            this.rbIda.Text = "Ida";
            this.rbIda.UseVisualStyleBackColor = true;
            this.rbIda.CheckedChanged += new System.EventHandler(this.rbIda_CheckedChanged);
            // 
            // cboxRoteiros
            // 
            this.cboxRoteiros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxRoteiros.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRoteiros.FormattingEnabled = true;
            this.cboxRoteiros.Location = new System.Drawing.Point(6, 19);
            this.cboxRoteiros.Name = "cboxRoteiros";
            this.cboxRoteiros.Size = new System.Drawing.Size(315, 21);
            this.cboxRoteiros.TabIndex = 3;
            this.cboxRoteiros.SelectedIndexChanged += new System.EventHandler(this.cboxRoteiros_SelectedIndexChanged);
            // 
            // lvRoteiros
            // 
            this.lvRoteiros.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvRoteiros.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvRoteiros.FullRowSelect = true;
            this.lvRoteiros.GridLines = true;
            this.lvRoteiros.Location = new System.Drawing.Point(5, 70);
            this.lvRoteiros.Name = "lvRoteiros";
            this.lvRoteiros.Size = new System.Drawing.Size(316, 247);
            this.lvRoteiros.TabIndex = 0;
            this.lvRoteiros.UseCompatibleStateImageBehavior = false;
            this.lvRoteiros.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Indice";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Texto";
            this.columnHeader2.Width = 210;
            // 
            // gboxMensagem
            // 
            this.gboxMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxMensagem.Controls.Add(this.cboxMensagens);
            this.gboxMensagem.Controls.Add(this.lvMensagens);
            this.gboxMensagem.Location = new System.Drawing.Point(336, 3);
            this.gboxMensagem.Name = "gboxMensagem";
            this.gboxMensagem.Size = new System.Drawing.Size(327, 323);
            this.gboxMensagem.TabIndex = 3;
            this.gboxMensagem.TabStop = false;
            this.gboxMensagem.Text = "Mensagem";
            // 
            // cboxMensagens
            // 
            this.cboxMensagens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxMensagens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMensagens.FormattingEnabled = true;
            this.cboxMensagens.Location = new System.Drawing.Point(7, 19);
            this.cboxMensagens.Name = "cboxMensagens";
            this.cboxMensagens.Size = new System.Drawing.Size(314, 21);
            this.cboxMensagens.TabIndex = 2;
            this.cboxMensagens.SelectedIndexChanged += new System.EventHandler(this.cboxMensagens_SelectedIndexChanged);
            // 
            // lvMensagens
            // 
            this.lvMensagens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMensagens.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvMensagens.GridLines = true;
            this.lvMensagens.Location = new System.Drawing.Point(7, 70);
            this.lvMensagens.Name = "lvMensagens";
            this.lvMensagens.Size = new System.Drawing.Size(314, 247);
            this.lvMensagens.TabIndex = 1;
            this.lvMensagens.UseCompatibleStateImageBehavior = false;
            this.lvMensagens.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Indice";
            this.columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Texto";
            this.columnHeader4.Width = 210;
            // 
            // btSimular
            // 
            this.btSimular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSimular.Location = new System.Drawing.Point(922, 460);
            this.btSimular.Name = "btSimular";
            this.btSimular.Size = new System.Drawing.Size(75, 23);
            this.btSimular.TabIndex = 4;
            this.btSimular.Text = "Simular";
            this.btSimular.UseVisualStyleBackColor = true;
            this.btSimular.Click += new System.EventHandler(this.btSimular_Click);
            // 
            // gboxSaudacao
            // 
            this.gboxSaudacao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxSaudacao.Controls.Add(this.labelMinbNoite);
            this.gboxSaudacao.Controls.Add(this.labelMinbTarde);
            this.gboxSaudacao.Controls.Add(this.labelMinbDia);
            this.gboxSaudacao.Controls.Add(this.labelBoaNoite);
            this.gboxSaudacao.Controls.Add(this.labelBoaTarde);
            this.gboxSaudacao.Controls.Add(this.labelBomDia);
            this.gboxSaudacao.Controls.Add(this.tboxBoaNoite);
            this.gboxSaudacao.Controls.Add(this.tboxBoaTarde);
            this.gboxSaudacao.Controls.Add(this.tboxBomDia);
            this.gboxSaudacao.Location = new System.Drawing.Point(559, 3);
            this.gboxSaudacao.Name = "gboxSaudacao";
            this.gboxSaudacao.Size = new System.Drawing.Size(438, 56);
            this.gboxSaudacao.TabIndex = 1;
            this.gboxSaudacao.TabStop = false;
            this.gboxSaudacao.Text = "Hora da Saudação";
            // 
            // labelMinbNoite
            // 
            this.labelMinbNoite.AutoSize = true;
            this.labelMinbNoite.Location = new System.Drawing.Point(390, 27);
            this.labelMinbNoite.Name = "labelMinbNoite";
            this.labelMinbNoite.Size = new System.Drawing.Size(22, 13);
            this.labelMinbNoite.TabIndex = 8;
            this.labelMinbNoite.Text = ":00";
            // 
            // labelMinbTarde
            // 
            this.labelMinbTarde.AutoSize = true;
            this.labelMinbTarde.Location = new System.Drawing.Point(243, 27);
            this.labelMinbTarde.Name = "labelMinbTarde";
            this.labelMinbTarde.Size = new System.Drawing.Size(22, 13);
            this.labelMinbTarde.TabIndex = 7;
            this.labelMinbTarde.Text = ":00";
            // 
            // labelMinbDia
            // 
            this.labelMinbDia.AutoSize = true;
            this.labelMinbDia.Location = new System.Drawing.Point(85, 27);
            this.labelMinbDia.Name = "labelMinbDia";
            this.labelMinbDia.Size = new System.Drawing.Size(22, 13);
            this.labelMinbDia.TabIndex = 6;
            this.labelMinbDia.Text = ":00";
            // 
            // labelBoaNoite
            // 
            this.labelBoaNoite.AutoSize = true;
            this.labelBoaNoite.Location = new System.Drawing.Point(304, 27);
            this.labelBoaNoite.Name = "labelBoaNoite";
            this.labelBoaNoite.Size = new System.Drawing.Size(54, 13);
            this.labelBoaNoite.TabIndex = 5;
            this.labelBoaNoite.Text = "Boa Noite";
            // 
            // labelBoaTarde
            // 
            this.labelBoaTarde.AutoSize = true;
            this.labelBoaTarde.Location = new System.Drawing.Point(154, 27);
            this.labelBoaTarde.Name = "labelBoaTarde";
            this.labelBoaTarde.Size = new System.Drawing.Size(57, 13);
            this.labelBoaTarde.TabIndex = 4;
            this.labelBoaTarde.Text = "Boa Tarde";
            // 
            // labelBomDia
            // 
            this.labelBomDia.AutoSize = true;
            this.labelBomDia.Location = new System.Drawing.Point(6, 27);
            this.labelBomDia.Name = "labelBomDia";
            this.labelBomDia.Size = new System.Drawing.Size(47, 13);
            this.labelBomDia.TabIndex = 3;
            this.labelBomDia.Text = "Bom Dia";
            // 
            // tboxBoaNoite
            // 
            this.tboxBoaNoite.Location = new System.Drawing.Point(364, 24);
            this.tboxBoaNoite.MaxLength = 2;
            this.tboxBoaNoite.Name = "tboxBoaNoite";
            this.tboxBoaNoite.Size = new System.Drawing.Size(18, 20);
            this.tboxBoaNoite.TabIndex = 2;
            this.tboxBoaNoite.Text = "18";
            this.tboxBoaNoite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxBoaNoite.Click += new System.EventHandler(this.tboxBoaNoite_Click);
            this.tboxBoaNoite.Enter += new System.EventHandler(this.tboxBoaNoite_Enter);
            this.tboxBoaNoite.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxBoaNoite_KeyDown);
            this.tboxBoaNoite.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxBoaNoite_KeyPress);
            // 
            // tboxBoaTarde
            // 
            this.tboxBoaTarde.Location = new System.Drawing.Point(217, 24);
            this.tboxBoaTarde.MaxLength = 2;
            this.tboxBoaTarde.Name = "tboxBoaTarde";
            this.tboxBoaTarde.Size = new System.Drawing.Size(18, 20);
            this.tboxBoaTarde.TabIndex = 1;
            this.tboxBoaTarde.Text = "12";
            this.tboxBoaTarde.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxBoaTarde.Click += new System.EventHandler(this.tboxBoaTarde_Click);
            this.tboxBoaTarde.Enter += new System.EventHandler(this.tboxBoaTarde_Enter);
            this.tboxBoaTarde.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxBoaTarde_KeyDown);
            this.tboxBoaTarde.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxBoaTarde_KeyPress);
            // 
            // tboxBomDia
            // 
            this.tboxBomDia.Location = new System.Drawing.Point(59, 24);
            this.tboxBomDia.MaxLength = 2;
            this.tboxBomDia.Name = "tboxBomDia";
            this.tboxBomDia.Size = new System.Drawing.Size(18, 20);
            this.tboxBomDia.TabIndex = 0;
            this.tboxBomDia.Text = "23";
            this.tboxBomDia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxBomDia.Click += new System.EventHandler(this.tboxBomDia_Click);
            this.tboxBomDia.Enter += new System.EventHandler(this.tboxBomDia_Enter);
            this.tboxBomDia.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxBomDia_KeyDown);
            this.tboxBomDia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxBomDia_KeyPress);
            // 
            // btnAplicar
            // 
            this.btnAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAplicar.Location = new System.Drawing.Point(842, 460);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(75, 23);
            this.btnAplicar.TabIndex = 5;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // gboxSecMensagem
            // 
            this.gboxSecMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxSecMensagem.Controls.Add(this.cboxSecMensagem);
            this.gboxSecMensagem.Controls.Add(this.lvSecMensagens);
            this.gboxSecMensagem.Location = new System.Drawing.Point(669, 3);
            this.gboxSecMensagem.Name = "gboxSecMensagem";
            this.gboxSecMensagem.Size = new System.Drawing.Size(328, 323);
            this.gboxSecMensagem.TabIndex = 4;
            this.gboxSecMensagem.TabStop = false;
            this.gboxSecMensagem.Text = "Mensagem Secundário";
            // 
            // cboxSecMensagem
            // 
            this.cboxSecMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxSecMensagem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSecMensagem.FormattingEnabled = true;
            this.cboxSecMensagem.Location = new System.Drawing.Point(7, 19);
            this.cboxSecMensagem.Name = "cboxSecMensagem";
            this.cboxSecMensagem.Size = new System.Drawing.Size(315, 21);
            this.cboxSecMensagem.TabIndex = 2;
            this.cboxSecMensagem.SelectedIndexChanged += new System.EventHandler(this.cboxSecMensagem_SelectedIndexChanged);
            // 
            // lvSecMensagens
            // 
            this.lvSecMensagens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSecMensagens.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.lvSecMensagens.GridLines = true;
            this.lvSecMensagens.Location = new System.Drawing.Point(7, 70);
            this.lvSecMensagens.Name = "lvSecMensagens";
            this.lvSecMensagens.Size = new System.Drawing.Size(315, 247);
            this.lvSecMensagens.TabIndex = 1;
            this.lvSecMensagens.UseCompatibleStateImageBehavior = false;
            this.lvSecMensagens.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Indice";
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Texto";
            this.columnHeader6.Width = 210;
            // 
            // gboxInverterLed
            // 
            this.gboxInverterLed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxInverterLed.Controls.Add(this.lblMinInverterLed);
            this.gboxInverterLed.Controls.Add(this.lblInverterLed);
            this.gboxInverterLed.Controls.Add(this.tboxMinInverterLed);
            this.gboxInverterLed.Location = new System.Drawing.Point(336, 3);
            this.gboxInverterLed.Name = "gboxInverterLed";
            this.gboxInverterLed.Size = new System.Drawing.Size(217, 56);
            this.gboxInverterLed.TabIndex = 1;
            this.gboxInverterLed.TabStop = false;
            this.gboxInverterLed.Text = "Painel";
            // 
            // lblMinInverterLed
            // 
            this.lblMinInverterLed.AutoSize = true;
            this.lblMinInverterLed.Location = new System.Drawing.Point(131, 27);
            this.lblMinInverterLed.Name = "lblMinInverterLed";
            this.lblMinInverterLed.Size = new System.Drawing.Size(50, 13);
            this.lblMinInverterLed.TabIndex = 9;
            this.lblMinInverterLed.Text = "(Minutos)";
            // 
            // lblInverterLed
            // 
            this.lblInverterLed.AutoSize = true;
            this.lblInverterLed.Location = new System.Drawing.Point(8, 27);
            this.lblInverterLed.Name = "lblInverterLed";
            this.lblInverterLed.Size = new System.Drawing.Size(127, 13);
            this.lblInverterLed.TabIndex = 8;
            this.lblInverterLed.Text = "Tempo para Inverter LED";
            // 
            // tboxMinInverterLed
            // 
            this.tboxMinInverterLed.Location = new System.Drawing.Point(107, 24);
            this.tboxMinInverterLed.MaxLength = 3;
            this.tboxMinInverterLed.Name = "tboxMinInverterLed";
            this.tboxMinInverterLed.Size = new System.Drawing.Size(24, 20);
            this.tboxMinInverterLed.TabIndex = 7;
            this.tboxMinInverterLed.Text = "0";
            this.tboxMinInverterLed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxMinInverterLed.TextChanged += new System.EventHandler(this.tboxMinInverterLed_TextChanged);
            this.tboxMinInverterLed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxMinInverterLed_KeyPress);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanel1.Controls.Add(this.gboxRoteiro, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gboxMensagem, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gboxSecMensagem, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 125);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 329);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.34F));
            this.tableLayoutPanel2.Controls.Add(this.gboxSaudacao, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.gboxInverterLed, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.gboxMotorista, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 62);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1000, 62);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // gboxMotorista
            // 
            this.gboxMotorista.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxMotorista.Controls.Add(this.cboxMotorista);
            this.gboxMotorista.Location = new System.Drawing.Point(3, 3);
            this.gboxMotorista.Name = "gboxMotorista";
            this.gboxMotorista.Size = new System.Drawing.Size(327, 56);
            this.gboxMotorista.TabIndex = 2;
            this.gboxMotorista.TabStop = false;
            this.gboxMotorista.Text = "Motorista";
            // 
            // cboxMotorista
            // 
            this.cboxMotorista.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxMotorista.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMotorista.FormattingEnabled = true;
            this.cboxMotorista.Location = new System.Drawing.Point(8, 24);
            this.cboxMotorista.Name = "cboxMotorista";
            this.cboxMotorista.Size = new System.Drawing.Size(313, 21);
            this.cboxMotorista.TabIndex = 0;
            // 
            // gboxAlternancia
            // 
            this.gboxAlternancia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxAlternancia.Controls.Add(this.btBloquearAlternancia);
            this.gboxAlternancia.Controls.Add(this.cboxAlternancia);
            this.gboxAlternancia.Location = new System.Drawing.Point(3, 3);
            this.gboxAlternancia.Name = "gboxAlternancia";
            this.gboxAlternancia.Size = new System.Drawing.Size(327, 56);
            this.gboxAlternancia.TabIndex = 7;
            this.gboxAlternancia.TabStop = false;
            this.gboxAlternancia.Text = "Alternancia";
            // 
            // btBloquearAlternancia
            // 
            this.btBloquearAlternancia.Location = new System.Drawing.Point(295, 18);
            this.btBloquearAlternancia.Name = "btBloquearAlternancia";
            this.btBloquearAlternancia.Size = new System.Drawing.Size(26, 23);
            this.btBloquearAlternancia.TabIndex = 1;
            this.btBloquearAlternancia.Text = "...";
            this.btBloquearAlternancia.UseVisualStyleBackColor = true;
            this.btBloquearAlternancia.Click += new System.EventHandler(this.btBloquearAlternancia_Click);
            // 
            // cboxAlternancia
            // 
            this.cboxAlternancia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxAlternancia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxAlternancia.FormattingEnabled = true;
            this.cboxAlternancia.Location = new System.Drawing.Point(6, 19);
            this.cboxAlternancia.Name = "cboxAlternancia";
            this.cboxAlternancia.Size = new System.Drawing.Size(289, 21);
            this.cboxAlternancia.TabIndex = 0;
            // 
            // groupBoxTimeOutSemComunicacao
            // 
            this.groupBoxTimeOutSemComunicacao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTimeOutSemComunicacao.Controls.Add(this.labelTimeOutSegundos);
            this.groupBoxTimeOutSemComunicacao.Controls.Add(this.labelTimeoutFalhaRede);
            this.groupBoxTimeOutSemComunicacao.Controls.Add(this.textBoxTimeoutFalhaRede);
            this.groupBoxTimeOutSemComunicacao.Location = new System.Drawing.Point(559, 3);
            this.groupBoxTimeOutSemComunicacao.Name = "groupBoxTimeOutSemComunicacao";
            this.groupBoxTimeOutSemComunicacao.Size = new System.Drawing.Size(278, 56);
            this.groupBoxTimeOutSemComunicacao.TabIndex = 10;
            this.groupBoxTimeOutSemComunicacao.TabStop = false;
            this.groupBoxTimeOutSemComunicacao.Text = " Quando perder comunicação com o controlador ";
            // 
            // labelTimeOutSegundos
            // 
            this.labelTimeOutSegundos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimeOutSegundos.AutoSize = true;
            this.labelTimeOutSegundos.Location = new System.Drawing.Point(211, 27);
            this.labelTimeOutSegundos.Name = "labelTimeOutSegundos";
            this.labelTimeOutSegundos.Size = new System.Drawing.Size(61, 13);
            this.labelTimeOutSegundos.TabIndex = 9;
            this.labelTimeOutSegundos.Text = "(Segundos)";
            // 
            // labelTimeoutFalhaRede
            // 
            this.labelTimeoutFalhaRede.AutoSize = true;
            this.labelTimeoutFalhaRede.Location = new System.Drawing.Point(6, 27);
            this.labelTimeoutFalhaRede.Name = "labelTimeoutFalhaRede";
            this.labelTimeoutFalhaRede.Size = new System.Drawing.Size(130, 13);
            this.labelTimeoutFalhaRede.TabIndex = 8;
            this.labelTimeoutFalhaRede.Text = "Embaralhar os painéis em ";
            // 
            // textBoxTimeoutFalhaRede
            // 
            this.textBoxTimeoutFalhaRede.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTimeoutFalhaRede.Location = new System.Drawing.Point(174, 24);
            this.textBoxTimeoutFalhaRede.MaxLength = 3;
            this.textBoxTimeoutFalhaRede.Name = "textBoxTimeoutFalhaRede";
            this.textBoxTimeoutFalhaRede.Size = new System.Drawing.Size(24, 20);
            this.textBoxTimeoutFalhaRede.TabIndex = 7;
            this.textBoxTimeoutFalhaRede.Text = "0";
            this.textBoxTimeoutFalhaRede.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxTimeoutFalhaRede.TextChanged += new System.EventHandler(this.textBoxTimeoutFalhaRede_TextChanged);
            this.textBoxTimeoutFalhaRede.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTimeoutFalhaRede_KeyPress);
            // 
            // groupBoxBaudrate
            // 
            this.groupBoxBaudrate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBaudrate.Controls.Add(this.comboBoxBaudRate);
            this.groupBoxBaudrate.Location = new System.Drawing.Point(336, 3);
            this.groupBoxBaudrate.Name = "groupBoxBaudrate";
            this.groupBoxBaudrate.Size = new System.Drawing.Size(217, 56);
            this.groupBoxBaudrate.TabIndex = 3;
            this.groupBoxBaudrate.TabStop = false;
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] {
            "Ninbus",
            "Validador"});
            this.comboBoxBaudRate.Location = new System.Drawing.Point(7, 19);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(203, 21);
            this.comboBoxBaudRate.TabIndex = 0;
            this.comboBoxBaudRate.SelectedIndexChanged += new System.EventHandler(this.comboBoxBaudRate_SelectedIndexChanged);
            // 
            // groupBoxTurnOffUSB
            // 
            this.groupBoxTurnOffUSB.Controls.Add(this.checkBoxTurnUSBOff);
            this.groupBoxTurnOffUSB.Location = new System.Drawing.Point(844, 4);
            this.groupBoxTurnOffUSB.Name = "groupBoxTurnOffUSB";
            this.groupBoxTurnOffUSB.Size = new System.Drawing.Size(153, 55);
            this.groupBoxTurnOffUSB.TabIndex = 11;
            this.groupBoxTurnOffUSB.TabStop = false;
            this.groupBoxTurnOffUSB.Text = " USB On / Off ";
            // 
            // checkBoxTurnUSBOff
            // 
            this.checkBoxTurnUSBOff.AutoSize = true;
            this.checkBoxTurnUSBOff.Location = new System.Drawing.Point(6, 25);
            this.checkBoxTurnUSBOff.Name = "checkBoxTurnUSBOff";
            this.checkBoxTurnUSBOff.Size = new System.Drawing.Size(90, 17);
            this.checkBoxTurnUSBOff.TabIndex = 0;
            this.checkBoxTurnUSBOff.Text = "Turn USB Off";
            this.checkBoxTurnUSBOff.UseVisualStyleBackColor = true;
            this.checkBoxTurnUSBOff.CheckedChanged += new System.EventHandler(this.checkBoxTurnUSBOff_CheckedChanged);
            // 
            // EditorSimulacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTurnOffUSB);
            this.Controls.Add(this.groupBoxBaudrate);
            this.Controls.Add(this.groupBoxTimeOutSemComunicacao);
            this.Controls.Add(this.gboxAlternancia);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btSimular);
            this.Name = "EditorSimulacao";
            this.Size = new System.Drawing.Size(1000, 488);
            this.ClientSizeChanged += new System.EventHandler(this.EditorSimulacao_ClientSizeChanged);
            this.VisibleChanged += new System.EventHandler(this.EditarSimulacao_VisibleChanged);
            this.gboxRoteiro.ResumeLayout(false);
            this.gboxRoteiro.PerformLayout();
            this.gboxMensagem.ResumeLayout(false);
            this.gboxSaudacao.ResumeLayout(false);
            this.gboxSaudacao.PerformLayout();
            this.gboxSecMensagem.ResumeLayout(false);
            this.gboxInverterLed.ResumeLayout(false);
            this.gboxInverterLed.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.gboxMotorista.ResumeLayout(false);
            this.gboxAlternancia.ResumeLayout(false);
            this.groupBoxTimeOutSemComunicacao.ResumeLayout(false);
            this.groupBoxTimeOutSemComunicacao.PerformLayout();
            this.groupBoxBaudrate.ResumeLayout(false);
            this.groupBoxTurnOffUSB.ResumeLayout(false);
            this.groupBoxTurnOffUSB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxRoteiro;
        private System.Windows.Forms.RadioButton rbVolta;
        private System.Windows.Forms.RadioButton rbIda;
        private System.Windows.Forms.ComboBox cboxRoteiros;
        private System.Windows.Forms.ListView lvRoteiros;
        private System.Windows.Forms.GroupBox gboxMensagem;
        private System.Windows.Forms.ComboBox cboxMensagens;
        private System.Windows.Forms.ListView lvMensagens;
        private System.Windows.Forms.Button btSimular;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox gboxSaudacao;
        private System.Windows.Forms.Label labelBoaNoite;
        private System.Windows.Forms.Label labelBoaTarde;
        private System.Windows.Forms.Label labelBomDia;
        private System.Windows.Forms.TextBox tboxBoaNoite;
        private System.Windows.Forms.TextBox tboxBoaTarde;
        private System.Windows.Forms.TextBox tboxBomDia;
        private System.Windows.Forms.Label labelMinbNoite;
        private System.Windows.Forms.Label labelMinbTarde;
        private System.Windows.Forms.Label labelMinbDia;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.GroupBox gboxSecMensagem;
        private System.Windows.Forms.ComboBox cboxSecMensagem;
        private System.Windows.Forms.ListView lvSecMensagens;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.GroupBox gboxInverterLed;
        private System.Windows.Forms.Label lblMinInverterLed;
        private System.Windows.Forms.Label lblInverterLed;
        private System.Windows.Forms.TextBox tboxMinInverterLed;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox gboxMotorista;
        private System.Windows.Forms.ComboBox cboxMotorista;
        private System.Windows.Forms.GroupBox gboxAlternancia;
        private System.Windows.Forms.Button btBloquearAlternancia;
        private System.Windows.Forms.ComboBox cboxAlternancia;
        private System.Windows.Forms.Label lblTextoRoteiro;
        private System.Windows.Forms.GroupBox groupBoxTimeOutSemComunicacao;
        private System.Windows.Forms.Label labelTimeOutSegundos;
        private System.Windows.Forms.Label labelTimeoutFalhaRede;
        private System.Windows.Forms.TextBox textBoxTimeoutFalhaRede;
        private System.Windows.Forms.GroupBox groupBoxBaudrate;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.GroupBox groupBoxTurnOffUSB;
        private System.Windows.Forms.CheckBox checkBoxTurnUSBOff;
    }
}
