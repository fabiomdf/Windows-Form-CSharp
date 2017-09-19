namespace LPontos2.Forms.Roteiros
{
    partial class Roteiros
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Roteiros));
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.tbTarifa = new System.Windows.Forms.TextBox();
            this.labelTarifa = new System.Windows.Forms.Label();
            this.tbNomeRoteiro = new System.Windows.Forms.TextBox();
            this.labelNomeRoteiro = new System.Windows.Forms.Label();
            this.chbRepetir = new System.Windows.Forms.CheckBox();
            this.chbIdaVolta = new System.Windows.Forms.CheckBox();
            this.btnEditarNumero = new System.Windows.Forms.Button();
            this.cbApresentacaoPadrao = new System.Windows.Forms.ComboBox();
            this.labelApresentacaoPadrao = new System.Windows.Forms.Label();
            this.tbNumeroRoteiro = new System.Windows.Forms.TextBox();
            this.labelNumeroRoteiro = new System.Windows.Forms.Label();
            this.labelIndice = new System.Windows.Forms.Label();
            this.tbIndice = new System.Windows.Forms.TextBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tabTextos = new System.Windows.Forms.TabControl();
            this.tabIda = new System.Windows.Forms.TabPage();
            this.btnExcluirFraseIda = new System.Windows.Forms.Button();
            this.btnIncluirFraseIda = new System.Windows.Forms.Button();
            this.lvFrasesIda = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabVolta = new System.Windows.Forms.TabPage();
            this.btnExcluirFraseVolta = new System.Windows.Forms.Button();
            this.btnIncluirFraseVolta = new System.Windows.Forms.Button();
            this.lvFrasesVolta = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.groupBoxInfo.SuspendLayout();
            this.tabTextos.SuspendLayout();
            this.tabIda.SuspendLayout();
            this.tabVolta.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInfo.Controls.Add(this.tbTarifa);
            this.groupBoxInfo.Controls.Add(this.labelTarifa);
            this.groupBoxInfo.Controls.Add(this.tbNomeRoteiro);
            this.groupBoxInfo.Controls.Add(this.labelNomeRoteiro);
            this.groupBoxInfo.Controls.Add(this.chbRepetir);
            this.groupBoxInfo.Controls.Add(this.chbIdaVolta);
            this.groupBoxInfo.Controls.Add(this.btnEditarNumero);
            this.groupBoxInfo.Controls.Add(this.cbApresentacaoPadrao);
            this.groupBoxInfo.Controls.Add(this.labelApresentacaoPadrao);
            this.groupBoxInfo.Controls.Add(this.tbNumeroRoteiro);
            this.groupBoxInfo.Controls.Add(this.labelNumeroRoteiro);
            this.groupBoxInfo.Controls.Add(this.labelIndice);
            this.groupBoxInfo.Controls.Add(this.tbIndice);
            this.groupBoxInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxInfo.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(634, 185);
            this.groupBoxInfo.TabIndex = 0;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Informações básicas";
            // 
            // tbTarifa
            // 
            this.tbTarifa.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbTarifa.Location = new System.Drawing.Point(261, 71);
            this.tbTarifa.MaxLength = 8;
            this.tbTarifa.Name = "tbTarifa";
            this.tbTarifa.Size = new System.Drawing.Size(80, 20);
            this.tbTarifa.TabIndex = 3;
            this.tbTarifa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbTarifa.Click += new System.EventHandler(this.tbTarifa_Click);
            this.tbTarifa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbTarifa_KeyDown);
            this.tbTarifa.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTarifa_KeyPress);
            // 
            // labelTarifa
            // 
            this.labelTarifa.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTarifa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTarifa.Location = new System.Drawing.Point(263, 55);
            this.labelTarifa.Name = "labelTarifa";
            this.labelTarifa.Size = new System.Drawing.Size(78, 13);
            this.labelTarifa.TabIndex = 11;
            this.labelTarifa.Text = "Tarifa:";
            this.labelTarifa.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbNomeRoteiro
            // 
            this.tbNomeRoteiro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNomeRoteiro.Location = new System.Drawing.Point(366, 71);
            this.tbNomeRoteiro.MaxLength = 16;
            this.tbNomeRoteiro.Name = "tbNomeRoteiro";
            this.tbNomeRoteiro.Size = new System.Drawing.Size(262, 20);
            this.tbNomeRoteiro.TabIndex = 4;
            this.tbNomeRoteiro.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbNomeRoteiro.TextChanged += new System.EventHandler(this.tbNomeRoteiro_TextChanged);
            // 
            // labelNomeRoteiro
            // 
            this.labelNomeRoteiro.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelNomeRoteiro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNomeRoteiro.Location = new System.Drawing.Point(283, 55);
            this.labelNomeRoteiro.Name = "labelNomeRoteiro";
            this.labelNomeRoteiro.Size = new System.Drawing.Size(345, 13);
            this.labelNomeRoteiro.TabIndex = 9;
            this.labelNomeRoteiro.Text = "Nome do roteiro:";
            this.labelNomeRoteiro.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chbRepetir
            // 
            this.chbRepetir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chbRepetir.AutoSize = true;
            this.chbRepetir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbRepetir.Location = new System.Drawing.Point(279, 160);
            this.chbRepetir.Name = "chbRepetir";
            this.chbRepetir.Size = new System.Drawing.Size(349, 17);
            this.chbRepetir.TabIndex = 7;
            this.chbRepetir.Text = "Repetir textos inseridos no painel 01 em todos os painéis";
            this.chbRepetir.UseVisualStyleBackColor = true;
            // 
            // chbIdaVolta
            // 
            this.chbIdaVolta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chbIdaVolta.AutoSize = true;
            this.chbIdaVolta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbIdaVolta.Location = new System.Drawing.Point(362, 137);
            this.chbIdaVolta.Name = "chbIdaVolta";
            this.chbIdaVolta.Size = new System.Drawing.Size(266, 17);
            this.chbIdaVolta.TabIndex = 6;
            this.chbIdaVolta.Text = "Os textos de Ida serão iguais dos de Volta";
            this.chbIdaVolta.UseVisualStyleBackColor = true;
            this.chbIdaVolta.Click += new System.EventHandler(this.chbIdaVolta_Click);
            // 
            // btnEditarNumero
            // 
            this.btnEditarNumero.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnEditarNumero.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditarNumero.Location = new System.Drawing.Point(6, 69);
            this.btnEditarNumero.Name = "btnEditarNumero";
            this.btnEditarNumero.Size = new System.Drawing.Size(114, 23);
            this.btnEditarNumero.TabIndex = 1;
            this.btnEditarNumero.Text = "Editar Número";
            this.btnEditarNumero.UseVisualStyleBackColor = true;
            this.btnEditarNumero.Click += new System.EventHandler(this.btnEditarNumero_Click);
            // 
            // cbApresentacaoPadrao
            // 
            this.cbApresentacaoPadrao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbApresentacaoPadrao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbApresentacaoPadrao.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbApresentacaoPadrao.FormattingEnabled = true;
            this.cbApresentacaoPadrao.Location = new System.Drawing.Point(6, 110);
            this.cbApresentacaoPadrao.Name = "cbApresentacaoPadrao";
            this.cbApresentacaoPadrao.Size = new System.Drawing.Size(622, 21);
            this.cbApresentacaoPadrao.TabIndex = 5;
            this.cbApresentacaoPadrao.SelectedIndexChanged += new System.EventHandler(this.cbApresentacaoPadrao_SelectedIndexChanged);
            // 
            // labelApresentacaoPadrao
            // 
            this.labelApresentacaoPadrao.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelApresentacaoPadrao.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApresentacaoPadrao.Location = new System.Drawing.Point(6, 94);
            this.labelApresentacaoPadrao.Name = "labelApresentacaoPadrao";
            this.labelApresentacaoPadrao.Size = new System.Drawing.Size(622, 13);
            this.labelApresentacaoPadrao.TabIndex = 4;
            this.labelApresentacaoPadrao.Text = "Apresentação padrão:";
            this.labelApresentacaoPadrao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbNumeroRoteiro
            // 
            this.tbNumeroRoteiro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbNumeroRoteiro.Location = new System.Drawing.Point(126, 71);
            this.tbNumeroRoteiro.MaxLength = 16;
            this.tbNumeroRoteiro.Name = "tbNumeroRoteiro";
            this.tbNumeroRoteiro.Size = new System.Drawing.Size(112, 20);
            this.tbNumeroRoteiro.TabIndex = 2;
            this.tbNumeroRoteiro.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbNumeroRoteiro.TextChanged += new System.EventHandler(this.tbNumeroRoteiro_TextChanged);
            this.tbNumeroRoteiro.Enter += new System.EventHandler(this.tbNumeroRoteiro_Enter);
            // 
            // labelNumeroRoteiro
            // 
            this.labelNumeroRoteiro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelNumeroRoteiro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumeroRoteiro.Location = new System.Drawing.Point(104, 55);
            this.labelNumeroRoteiro.Name = "labelNumeroRoteiro";
            this.labelNumeroRoteiro.Size = new System.Drawing.Size(134, 13);
            this.labelNumeroRoteiro.TabIndex = 2;
            this.labelNumeroRoteiro.Text = "Número do roteiro:";
            this.labelNumeroRoteiro.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIndice
            // 
            this.labelIndice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelIndice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIndice.Location = new System.Drawing.Point(10, 16);
            this.labelIndice.Name = "labelIndice";
            this.labelIndice.Size = new System.Drawing.Size(618, 13);
            this.labelIndice.TabIndex = 1;
            this.labelIndice.Text = "Índice do roteiro:";
            this.labelIndice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbIndice
            // 
            this.tbIndice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIndice.Location = new System.Drawing.Point(6, 32);
            this.tbIndice.Name = "tbIndice";
            this.tbIndice.ReadOnly = true;
            this.tbIndice.Size = new System.Drawing.Size(622, 20);
            this.tbIndice.TabIndex = 0;
            this.tbIndice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(400, 450);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Sair";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // tabTextos
            // 
            this.tabTextos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabTextos.Controls.Add(this.tabIda);
            this.tabTextos.Controls.Add(this.tabVolta);
            this.tabTextos.Location = new System.Drawing.Point(3, 194);
            this.tabTextos.Name = "tabTextos";
            this.tabTextos.SelectedIndex = 0;
            this.tabTextos.Size = new System.Drawing.Size(634, 250);
            this.tabTextos.TabIndex = 1;
            this.tabTextos.SelectedIndexChanged += new System.EventHandler(this.tabTextos_SelectedIndexChanged);
            // 
            // tabIda
            // 
            this.tabIda.Controls.Add(this.btnExcluirFraseIda);
            this.tabIda.Controls.Add(this.btnIncluirFraseIda);
            this.tabIda.Controls.Add(this.lvFrasesIda);
            this.tabIda.Location = new System.Drawing.Point(4, 22);
            this.tabIda.Name = "tabIda";
            this.tabIda.Padding = new System.Windows.Forms.Padding(3);
            this.tabIda.Size = new System.Drawing.Size(626, 224);
            this.tabIda.TabIndex = 0;
            this.tabIda.Text = "Textos de Ida";
            this.tabIda.UseVisualStyleBackColor = true;
            // 
            // btnExcluirFraseIda
            // 
            this.btnExcluirFraseIda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcluirFraseIda.Image = ((System.Drawing.Image)(resources.GetObject("btnExcluirFraseIda.Image")));
            this.btnExcluirFraseIda.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExcluirFraseIda.Location = new System.Drawing.Point(492, 76);
            this.btnExcluirFraseIda.Name = "btnExcluirFraseIda";
            this.btnExcluirFraseIda.Size = new System.Drawing.Size(104, 30);
            this.btnExcluirFraseIda.TabIndex = 3;
            this.btnExcluirFraseIda.Text = "Excluir";
            this.btnExcluirFraseIda.UseVisualStyleBackColor = true;
            this.btnExcluirFraseIda.Click += new System.EventHandler(this.btnExcluirFraseIda_Click);
            // 
            // btnIncluirFraseIda
            // 
            this.btnIncluirFraseIda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncluirFraseIda.Image = ((System.Drawing.Image)(resources.GetObject("btnIncluirFraseIda.Image")));
            this.btnIncluirFraseIda.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIncluirFraseIda.Location = new System.Drawing.Point(492, 40);
            this.btnIncluirFraseIda.Name = "btnIncluirFraseIda";
            this.btnIncluirFraseIda.Size = new System.Drawing.Size(104, 30);
            this.btnIncluirFraseIda.TabIndex = 2;
            this.btnIncluirFraseIda.Text = "Incluir";
            this.btnIncluirFraseIda.UseVisualStyleBackColor = true;
            this.btnIncluirFraseIda.Click += new System.EventHandler(this.btnIncluirPainel_Click);
            // 
            // lvFrasesIda
            // 
            this.lvFrasesIda.AllowDrop = true;
            this.lvFrasesIda.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFrasesIda.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.lvFrasesIda.FullRowSelect = true;
            this.lvFrasesIda.GridLines = true;
            this.lvFrasesIda.HideSelection = false;
            this.lvFrasesIda.Location = new System.Drawing.Point(6, 6);
            this.lvFrasesIda.MultiSelect = false;
            this.lvFrasesIda.Name = "lvFrasesIda";
            this.lvFrasesIda.Size = new System.Drawing.Size(614, 212);
            this.lvFrasesIda.TabIndex = 13;
            this.lvFrasesIda.UseCompatibleStateImageBehavior = false;
            this.lvFrasesIda.View = System.Windows.Forms.View.Details;
            this.lvFrasesIda.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvFrasesIda_ItemDrag);
            this.lvFrasesIda.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvFrasesIda_ItemSelectionChanged);
            this.lvFrasesIda.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvFrasesIda_DragDrop);
            this.lvFrasesIda.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvFrasesIda_DragEnter);
            this.lvFrasesIda.DoubleClick += new System.EventHandler(this.lvFrasesIda_DoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Índice";
            this.columnHeader2.Width = 75;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Texto";
            this.columnHeader3.Width = 367;
            // 
            // tabVolta
            // 
            this.tabVolta.Controls.Add(this.btnExcluirFraseVolta);
            this.tabVolta.Controls.Add(this.btnIncluirFraseVolta);
            this.tabVolta.Controls.Add(this.lvFrasesVolta);
            this.tabVolta.Location = new System.Drawing.Point(4, 22);
            this.tabVolta.Name = "tabVolta";
            this.tabVolta.Padding = new System.Windows.Forms.Padding(3);
            this.tabVolta.Size = new System.Drawing.Size(626, 224);
            this.tabVolta.TabIndex = 1;
            this.tabVolta.Text = "Textos de Volta";
            this.tabVolta.UseVisualStyleBackColor = true;
            // 
            // btnExcluirFraseVolta
            // 
            this.btnExcluirFraseVolta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcluirFraseVolta.Image = ((System.Drawing.Image)(resources.GetObject("btnExcluirFraseVolta.Image")));
            this.btnExcluirFraseVolta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExcluirFraseVolta.Location = new System.Drawing.Point(492, 76);
            this.btnExcluirFraseVolta.Name = "btnExcluirFraseVolta";
            this.btnExcluirFraseVolta.Size = new System.Drawing.Size(104, 30);
            this.btnExcluirFraseVolta.TabIndex = 5;
            this.btnExcluirFraseVolta.Text = "Excluir";
            this.btnExcluirFraseVolta.UseVisualStyleBackColor = true;
            this.btnExcluirFraseVolta.Click += new System.EventHandler(this.btnExcluirFraseVolta_Click);
            // 
            // btnIncluirFraseVolta
            // 
            this.btnIncluirFraseVolta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncluirFraseVolta.Image = ((System.Drawing.Image)(resources.GetObject("btnIncluirFraseVolta.Image")));
            this.btnIncluirFraseVolta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIncluirFraseVolta.Location = new System.Drawing.Point(492, 40);
            this.btnIncluirFraseVolta.Name = "btnIncluirFraseVolta";
            this.btnIncluirFraseVolta.Size = new System.Drawing.Size(104, 30);
            this.btnIncluirFraseVolta.TabIndex = 4;
            this.btnIncluirFraseVolta.Text = "Incluir";
            this.btnIncluirFraseVolta.UseVisualStyleBackColor = true;
            this.btnIncluirFraseVolta.Click += new System.EventHandler(this.btnIncluirFraseVolta_Click);
            // 
            // lvFrasesVolta
            // 
            this.lvFrasesVolta.AllowDrop = true;
            this.lvFrasesVolta.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFrasesVolta.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.lvFrasesVolta.FullRowSelect = true;
            this.lvFrasesVolta.GridLines = true;
            this.lvFrasesVolta.HideSelection = false;
            this.lvFrasesVolta.Location = new System.Drawing.Point(6, 6);
            this.lvFrasesVolta.MultiSelect = false;
            this.lvFrasesVolta.Name = "lvFrasesVolta";
            this.lvFrasesVolta.Size = new System.Drawing.Size(614, 212);
            this.lvFrasesVolta.TabIndex = 16;
            this.lvFrasesVolta.UseCompatibleStateImageBehavior = false;
            this.lvFrasesVolta.View = System.Windows.Forms.View.Details;
            this.lvFrasesVolta.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvFrasesVolta_ItemDrag);
            this.lvFrasesVolta.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvFrasesVolta_ItemSelectionChanged);
            this.lvFrasesVolta.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvFrasesVolta_DragDrop);
            this.lvFrasesVolta.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvFrasesVolta_DragEnter);
            this.lvFrasesVolta.DoubleClick += new System.EventHandler(this.lvFrasesVolta_DoubleClick);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Índice";
            this.columnHeader5.Width = 75;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Texto";
            this.columnHeader6.Width = 367;
            // 
            // btnAplicar
            // 
            this.btnAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAplicar.Location = new System.Drawing.Point(481, 450);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(75, 23);
            this.btnAplicar.TabIndex = 3;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // btnContinuar
            // 
            this.btnContinuar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinuar.Location = new System.Drawing.Point(562, 450);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(75, 23);
            this.btnContinuar.TabIndex = 4;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Click += new System.EventHandler(this.btContinuar_Click);
            // 
            // Roteiros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.tabTextos);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.groupBoxInfo);
            this.Name = "Roteiros";
            this.Size = new System.Drawing.Size(640, 480);
            this.VisibleChanged += new System.EventHandler(this.Roteiros_VisibleChanged);
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.tabTextos.ResumeLayout(false);
            this.tabIda.ResumeLayout(false);
            this.tabVolta.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.TextBox tbIndice;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label labelApresentacaoPadrao;
        private System.Windows.Forms.TextBox tbNumeroRoteiro;
        private System.Windows.Forms.Label labelNumeroRoteiro;
        private System.Windows.Forms.Label labelIndice;
        private System.Windows.Forms.ComboBox cbApresentacaoPadrao;
        private System.Windows.Forms.Button btnEditarNumero;
        private System.Windows.Forms.CheckBox chbIdaVolta;
        private System.Windows.Forms.TabControl tabTextos;
        private System.Windows.Forms.TabPage tabIda;
        private System.Windows.Forms.TabPage tabVolta;
        private System.Windows.Forms.ListView lvFrasesIda;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnExcluirFraseIda;
        private System.Windows.Forms.Button btnIncluirFraseIda;
        private System.Windows.Forms.CheckBox chbRepetir;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.TextBox tbNomeRoteiro;
        private System.Windows.Forms.Label labelNomeRoteiro;
        private System.Windows.Forms.Button btnExcluirFraseVolta;
        private System.Windows.Forms.Button btnIncluirFraseVolta;
        private System.Windows.Forms.ListView lvFrasesVolta;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label labelTarifa;
        private System.Windows.Forms.TextBox tbTarifa;
        private System.Windows.Forms.Button btnContinuar;
    }
}
