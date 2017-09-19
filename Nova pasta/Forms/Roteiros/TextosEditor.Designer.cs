namespace LPontos2.Forms.Roteiros
{
    partial class TextosEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextosEditor));
            this.labelIndice = new System.Windows.Forms.Label();
            this.tbIndice = new System.Windows.Forms.TextBox();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.chkTextoAutomatico = new System.Windows.Forms.CheckBox();
            this.tbLabelFrase = new System.Windows.Forms.TextBox();
            this.labelFrase = new System.Windows.Forms.Label();
            this.tbNumero = new System.Windows.Forms.TextBox();
            this.labelNumero = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAgrupar = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.labelFonte = new System.Windows.Forms.ToolStripLabel();
            this.cbFonte = new System.Windows.Forms.ToolStripComboBox();
            this.labelTamanho = new System.Windows.Forms.ToolStripLabel();
            this.cbTamanhoFonte = new System.Windows.Forms.ToolStripComboBox();
            this.btNegrito = new System.Windows.Forms.ToolStripButton();
            this.btItalico = new System.Windows.Forms.ToolStripButton();
            this.btSublinhado = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btMudarFonte = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btAlinharEsquerda = new System.Windows.Forms.ToolStripButton();
            this.btAlinharMeio = new System.Windows.Forms.ToolStripButton();
            this.btAlinharDireita = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btAlinharAbaixo = new System.Windows.Forms.ToolStripButton();
            this.btAlinharCentro = new System.Windows.Forms.ToolStripButton();
            this.btAlinharAcima = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lbNivelSuavizacao = new System.Windows.Forms.ToolStripLabel();
            this.cbNivelSuavizacao = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.lbImage = new System.Windows.Forms.ToolStripLabel();
            this.cbListaImages = new System.Windows.Forms.ToolStripComboBox();
            this.btInsertImage = new System.Windows.Forms.ToolStripButton();
            this.panelModelos = new System.Windows.Forms.Panel();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btSalvar = new System.Windows.Forms.Button();
            this.btContinuar = new System.Windows.Forms.Button();
            this.barraFerramentas = new System.Windows.Forms.ToolStrip();
            this.labelMultiLinha = new System.Windows.Forms.ToolStripLabel();
            this.cbMultinha = new System.Windows.Forms.ToolStripComboBox();
            this.labelModelo = new System.Windows.Forms.ToolStripLabel();
            this.cbModelo = new System.Windows.Forms.ToolStripComboBox();
            this.labelApresentacao = new System.Windows.Forms.ToolStripLabel();
            this.cbApresentacao = new System.Windows.Forms.ToolStripComboBox();
            this.tbTempoApresentacao = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.labelRolagem = new System.Windows.Forms.ToolStripLabel();
            this.tbTempoRolagem = new System.Windows.Forms.ToolStripTextBox();
            this.labelMsRolagem = new System.Windows.Forms.ToolStripLabel();
            this.groupBoxInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.barraFerramentas.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelIndice
            // 
            this.labelIndice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelIndice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIndice.Location = new System.Drawing.Point(6, 26);
            this.labelIndice.Name = "labelIndice";
            this.labelIndice.Size = new System.Drawing.Size(1098, 13);
            this.labelIndice.TabIndex = 1;
            this.labelIndice.Text = "Índice do roteiro:";
            this.labelIndice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbIndice
            // 
            this.tbIndice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIndice.Enabled = false;
            this.tbIndice.Location = new System.Drawing.Point(6, 42);
            this.tbIndice.Name = "tbIndice";
            this.tbIndice.Size = new System.Drawing.Size(1098, 20);
            this.tbIndice.TabIndex = 1;
            this.tbIndice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInfo.Controls.Add(this.chkTextoAutomatico);
            this.groupBoxInfo.Controls.Add(this.tbLabelFrase);
            this.groupBoxInfo.Controls.Add(this.labelFrase);
            this.groupBoxInfo.Controls.Add(this.tbNumero);
            this.groupBoxInfo.Controls.Add(this.labelNumero);
            this.groupBoxInfo.Controls.Add(this.labelIndice);
            this.groupBoxInfo.Controls.Add(this.tbIndice);
            this.groupBoxInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxInfo.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(1110, 159);
            this.groupBoxInfo.TabIndex = 1;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Informações básicas";
            // 
            // chkTextoAutomatico
            // 
            this.chkTextoAutomatico.AutoSize = true;
            this.chkTextoAutomatico.Checked = true;
            this.chkTextoAutomatico.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTextoAutomatico.Location = new System.Drawing.Point(6, 120);
            this.chkTextoAutomatico.Name = "chkTextoAutomatico";
            this.chkTextoAutomatico.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkTextoAutomatico.Size = new System.Drawing.Size(221, 17);
            this.chkTextoAutomatico.TabIndex = 3;
            this.chkTextoAutomatico.Text = "Preencher Texto Automaticamente";
            this.chkTextoAutomatico.UseVisualStyleBackColor = true;
            this.chkTextoAutomatico.CheckedChanged += new System.EventHandler(this.chkTextoAutomatico_CheckedChanged);
            // 
            // tbLabelFrase
            // 
            this.tbLabelFrase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLabelFrase.Enabled = false;
            this.tbLabelFrase.Location = new System.Drawing.Point(233, 117);
            this.tbLabelFrase.MaxLength = 50;
            this.tbLabelFrase.Name = "tbLabelFrase";
            this.tbLabelFrase.Size = new System.Drawing.Size(871, 20);
            this.tbLabelFrase.TabIndex = 4;
            this.tbLabelFrase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbLabelFrase.TextChanged += new System.EventHandler(this.tbLabelFrase_TextChanged);
            // 
            // labelFrase
            // 
            this.labelFrase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFrase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFrase.Location = new System.Drawing.Point(6, 104);
            this.labelFrase.Name = "labelFrase";
            this.labelFrase.Size = new System.Drawing.Size(1098, 13);
            this.labelFrase.TabIndex = 4;
            this.labelFrase.Text = "Texto:";
            this.labelFrase.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbNumero
            // 
            this.tbNumero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNumero.Enabled = false;
            this.tbNumero.Location = new System.Drawing.Point(6, 81);
            this.tbNumero.Name = "tbNumero";
            this.tbNumero.Size = new System.Drawing.Size(1098, 20);
            this.tbNumero.TabIndex = 2;
            this.tbNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelNumero
            // 
            this.labelNumero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNumero.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumero.Location = new System.Drawing.Point(9, 65);
            this.labelNumero.Name = "labelNumero";
            this.labelNumero.Size = new System.Drawing.Size(1095, 13);
            this.labelNumero.TabIndex = 2;
            this.labelNumero.Text = "Número do roteiro:";
            this.labelNumero.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.chkAgrupar);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.panelModelos);
            this.panel1.Controls.Add(this.btCancelar);
            this.panel1.Controls.Add(this.btSalvar);
            this.panel1.Controls.Add(this.btContinuar);
            this.panel1.Controls.Add(this.barraFerramentas);
            this.panel1.Location = new System.Drawing.Point(3, 168);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1110, 500);
            this.panel1.TabIndex = 2;
            // 
            // chkAgrupar
            // 
            this.chkAgrupar.AutoSize = true;
            this.chkAgrupar.BackColor = System.Drawing.SystemColors.Control;
            this.chkAgrupar.Checked = true;
            this.chkAgrupar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAgrupar.Location = new System.Drawing.Point(890, 3);
            this.chkAgrupar.Name = "chkAgrupar";
            this.chkAgrupar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkAgrupar.Size = new System.Drawing.Size(100, 17);
            this.chkAgrupar.TabIndex = 5;
            this.chkAgrupar.Text = "Mesclar Paineis";
            this.chkAgrupar.UseVisualStyleBackColor = false;
            this.chkAgrupar.CheckedChanged += new System.EventHandler(this.chkAgrupar_CheckedChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelFonte,
            this.cbFonte,
            this.labelTamanho,
            this.cbTamanhoFonte,
            this.btNegrito,
            this.btItalico,
            this.btSublinhado,
            this.toolStripSeparator3,
            this.btMudarFonte,
            this.toolStripSeparator4,
            this.btAlinharEsquerda,
            this.btAlinharMeio,
            this.btAlinharDireita,
            this.toolStripSeparator2,
            this.btAlinharAbaixo,
            this.btAlinharCentro,
            this.btAlinharAcima,
            this.toolStripSeparator1,
            this.lbNivelSuavizacao,
            this.cbNivelSuavizacao,
            this.toolStripSeparator5,
            this.lbImage,
            this.cbListaImages,
            this.btInsertImage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1110, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // labelFonte
            // 
            this.labelFonte.Name = "labelFonte";
            this.labelFonte.Size = new System.Drawing.Size(40, 22);
            this.labelFonte.Text = "Fonte:";
            // 
            // cbFonte
            // 
            this.cbFonte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFonte.Name = "cbFonte";
            this.cbFonte.Size = new System.Drawing.Size(200, 25);
            this.cbFonte.SelectedIndexChanged += new System.EventHandler(this.cbFonte_SelectedIndexChanged);
            // 
            // labelTamanho
            // 
            this.labelTamanho.Name = "labelTamanho";
            this.labelTamanho.Size = new System.Drawing.Size(60, 22);
            this.labelTamanho.Text = "Tamanho:";
            // 
            // cbTamanhoFonte
            // 
            this.cbTamanhoFonte.AutoSize = false;
            this.cbTamanhoFonte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTamanhoFonte.Enabled = false;
            this.cbTamanhoFonte.Name = "cbTamanhoFonte";
            this.cbTamanhoFonte.Size = new System.Drawing.Size(45, 23);
            this.cbTamanhoFonte.SelectedIndexChanged += new System.EventHandler(this.cbTamanhoFonte_SelectedIndexChanged);
            // 
            // btNegrito
            // 
            this.btNegrito.BackColor = System.Drawing.SystemColors.Control;
            this.btNegrito.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btNegrito.Enabled = false;
            this.btNegrito.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btNegrito.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btNegrito.Name = "btNegrito";
            this.btNegrito.Size = new System.Drawing.Size(23, 22);
            this.btNegrito.Text = "B";
            this.btNegrito.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btNegrito.Click += new System.EventHandler(this.btNegrito_Click);
            // 
            // btItalico
            // 
            this.btItalico.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btItalico.Enabled = false;
            this.btItalico.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btItalico.Image = ((System.Drawing.Image)(resources.GetObject("btItalico.Image")));
            this.btItalico.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btItalico.Name = "btItalico";
            this.btItalico.Size = new System.Drawing.Size(23, 22);
            this.btItalico.Text = "I";
            this.btItalico.Click += new System.EventHandler(this.btItalico_Click);
            // 
            // btSublinhado
            // 
            this.btSublinhado.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btSublinhado.Enabled = false;
            this.btSublinhado.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSublinhado.Image = ((System.Drawing.Image)(resources.GetObject("btSublinhado.Image")));
            this.btSublinhado.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSublinhado.Name = "btSublinhado";
            this.btSublinhado.Size = new System.Drawing.Size(23, 22);
            this.btSublinhado.Text = "U";
            this.btSublinhado.Click += new System.EventHandler(this.btSublinhado_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btMudarFonte
            // 
            this.btMudarFonte.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btMudarFonte.Image = global::PontosX2.Properties.Resources.style;
            this.btMudarFonte.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btMudarFonte.Name = "btMudarFonte";
            this.btMudarFonte.Size = new System.Drawing.Size(23, 22);
            this.btMudarFonte.Text = "btnChangeFont";
            this.btMudarFonte.Click += new System.EventHandler(this.btMudarFonte_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btAlinharEsquerda
            // 
            this.btAlinharEsquerda.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlinharEsquerda.Image = global::PontosX2.Properties.Resources.shape_align_left;
            this.btAlinharEsquerda.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlinharEsquerda.Name = "btAlinharEsquerda";
            this.btAlinharEsquerda.Size = new System.Drawing.Size(23, 22);
            this.btAlinharEsquerda.Text = "btnAlignLeft";
            this.btAlinharEsquerda.Click += new System.EventHandler(this.btAlinharEsquerda_Click);
            // 
            // btAlinharMeio
            // 
            this.btAlinharMeio.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlinharMeio.Image = global::PontosX2.Properties.Resources.shape_align_center;
            this.btAlinharMeio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlinharMeio.Name = "btAlinharMeio";
            this.btAlinharMeio.Size = new System.Drawing.Size(23, 22);
            this.btAlinharMeio.Text = "btnAlignMiddle";
            this.btAlinharMeio.Click += new System.EventHandler(this.btAlinharMeio_Click);
            // 
            // btAlinharDireita
            // 
            this.btAlinharDireita.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlinharDireita.Image = global::PontosX2.Properties.Resources.shape_align_right;
            this.btAlinharDireita.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlinharDireita.Name = "btAlinharDireita";
            this.btAlinharDireita.Size = new System.Drawing.Size(23, 22);
            this.btAlinharDireita.Text = "btnAlignRight";
            this.btAlinharDireita.Click += new System.EventHandler(this.btAlinharDireita_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btAlinharAbaixo
            // 
            this.btAlinharAbaixo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlinharAbaixo.Image = global::PontosX2.Properties.Resources.shape_align_bottom;
            this.btAlinharAbaixo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlinharAbaixo.Name = "btAlinharAbaixo";
            this.btAlinharAbaixo.Size = new System.Drawing.Size(23, 22);
            this.btAlinharAbaixo.Text = "btnAlignBottom";
            this.btAlinharAbaixo.Click += new System.EventHandler(this.btAlinharAbaixo_Click);
            // 
            // btAlinharCentro
            // 
            this.btAlinharCentro.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlinharCentro.Image = global::PontosX2.Properties.Resources.shape_align_middle;
            this.btAlinharCentro.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlinharCentro.Name = "btAlinharCentro";
            this.btAlinharCentro.Size = new System.Drawing.Size(23, 22);
            this.btAlinharCentro.Text = "btnAlignCenter";
            this.btAlinharCentro.Click += new System.EventHandler(this.btAlinharCentro_Click);
            // 
            // btAlinharAcima
            // 
            this.btAlinharAcima.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlinharAcima.Image = global::PontosX2.Properties.Resources.shape_align_top;
            this.btAlinharAcima.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlinharAcima.Name = "btAlinharAcima";
            this.btAlinharAcima.Size = new System.Drawing.Size(23, 22);
            this.btAlinharAcima.Text = "btnAlignTop";
            this.btAlinharAcima.Click += new System.EventHandler(this.btAlinharAcima_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lbNivelSuavizacao
            // 
            this.lbNivelSuavizacao.Name = "lbNivelSuavizacao";
            this.lbNivelSuavizacao.Size = new System.Drawing.Size(111, 22);
            this.lbNivelSuavizacao.Text = "Nível de Suavização";
            // 
            // cbNivelSuavizacao
            // 
            this.cbNivelSuavizacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNivelSuavizacao.Enabled = false;
            this.cbNivelSuavizacao.Items.AddRange(new object[] {
            "150",
            "160",
            "170",
            "180",
            "190",
            "200",
            "210",
            "220",
            "230",
            "240",
            "250"});
            this.cbNivelSuavizacao.Name = "cbNivelSuavizacao";
            this.cbNivelSuavizacao.Size = new System.Drawing.Size(75, 25);
            this.cbNivelSuavizacao.SelectedIndexChanged += new System.EventHandler(this.cbNivelSuavizacao_SelectedIndexChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // lbImage
            // 
            this.lbImage.Name = "lbImage";
            this.lbImage.Size = new System.Drawing.Size(40, 22);
            this.lbImage.Text = "Image";
            // 
            // cbListaImages
            // 
            this.cbListaImages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbListaImages.Name = "cbListaImages";
            this.cbListaImages.Size = new System.Drawing.Size(110, 25);
            // 
            // btInsertImage
            // 
            this.btInsertImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btInsertImage.Image = ((System.Drawing.Image)(resources.GetObject("btInsertImage.Image")));
            this.btInsertImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btInsertImage.Name = "btInsertImage";
            this.btInsertImage.Size = new System.Drawing.Size(23, 22);
            this.btInsertImage.Text = "toolStripButton1";
            this.btInsertImage.ToolTipText = "Insert Image";
            this.btInsertImage.Click += new System.EventHandler(this.btInsertImage_Click);
            // 
            // panelModelos
            // 
            this.panelModelos.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelModelos.Location = new System.Drawing.Point(295, 156);
            this.panelModelos.Name = "panelModelos";
            this.panelModelos.Size = new System.Drawing.Size(500, 200);
            this.panelModelos.TabIndex = 7;
            // 
            // btCancelar
            // 
            this.btCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelar.Location = new System.Drawing.Point(873, 473);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 8;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btSalvar
            // 
            this.btSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSalvar.Location = new System.Drawing.Point(954, 473);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(75, 23);
            this.btSalvar.TabIndex = 9;
            this.btSalvar.Text = "Salvar";
            this.btSalvar.UseVisualStyleBackColor = true;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // btContinuar
            // 
            this.btContinuar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btContinuar.Location = new System.Drawing.Point(1035, 473);
            this.btContinuar.Name = "btContinuar";
            this.btContinuar.Size = new System.Drawing.Size(75, 23);
            this.btContinuar.TabIndex = 10;
            this.btContinuar.Text = "Continuar";
            this.btContinuar.UseVisualStyleBackColor = true;
            this.btContinuar.Click += new System.EventHandler(this.btContinuar_Click);
            // 
            // barraFerramentas
            // 
            this.barraFerramentas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelMultiLinha,
            this.cbMultinha,
            this.labelModelo,
            this.cbModelo,
            this.labelApresentacao,
            this.cbApresentacao,
            this.tbTempoApresentacao,
            this.toolStripLabel6,
            this.labelRolagem,
            this.tbTempoRolagem,
            this.labelMsRolagem});
            this.barraFerramentas.Location = new System.Drawing.Point(0, 0);
            this.barraFerramentas.Name = "barraFerramentas";
            this.barraFerramentas.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.barraFerramentas.Size = new System.Drawing.Size(1110, 25);
            this.barraFerramentas.TabIndex = 5;
            this.barraFerramentas.Text = "toolStrip1";
            // 
            // labelMultiLinha
            // 
            this.labelMultiLinha.Name = "labelMultiLinha";
            this.labelMultiLinha.Size = new System.Drawing.Size(61, 22);
            this.labelMultiLinha.Text = "Multilinha";
            // 
            // cbMultinha
            // 
            this.cbMultinha.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMultinha.Name = "cbMultinha";
            this.cbMultinha.Size = new System.Drawing.Size(75, 25);
            this.cbMultinha.SelectedIndexChanged += new System.EventHandler(this.cbMultinha_SelectedIndexChanged);
            // 
            // labelModelo
            // 
            this.labelModelo.Name = "labelModelo";
            this.labelModelo.Size = new System.Drawing.Size(51, 22);
            this.labelModelo.Text = "Modelo:";
            // 
            // cbModelo
            // 
            this.cbModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModelo.Items.AddRange(new object[] {
            "Texto",
            "Número + Texto",
            "Texto + Número",
            "Texto Duplo",
            "Número + Texto Duplo",
            "Texto Duplo + Número",
            "Texto Duplo + Texto Duplo"});
            this.cbModelo.Name = "cbModelo";
            this.cbModelo.Size = new System.Drawing.Size(170, 25);
            this.cbModelo.SelectedIndexChanged += new System.EventHandler(this.comboModelo_SelectedIndexChanged);
            // 
            // labelApresentacao
            // 
            this.labelApresentacao.Name = "labelApresentacao";
            this.labelApresentacao.Size = new System.Drawing.Size(82, 22);
            this.labelApresentacao.Text = "Apresentação:";
            // 
            // cbApresentacao
            // 
            this.cbApresentacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbApresentacao.Name = "cbApresentacao";
            this.cbApresentacao.Size = new System.Drawing.Size(200, 25);
            this.cbApresentacao.SelectedIndexChanged += new System.EventHandler(this.cbApresentacao_SelectedIndexChanged);
            // 
            // tbTempoApresentacao
            // 
            this.tbTempoApresentacao.MaxLength = 4;
            this.tbTempoApresentacao.Name = "tbTempoApresentacao";
            this.tbTempoApresentacao.Size = new System.Drawing.Size(30, 25);
            this.tbTempoApresentacao.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTempoApresentacao_KeyPress);
            this.tbTempoApresentacao.TextChanged += new System.EventHandler(this.tbTempoApresentacao_TextChanged);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(35, 22);
            this.toolStripLabel6.Text = "ms    ";
            // 
            // labelRolagem
            // 
            this.labelRolagem.Name = "labelRolagem";
            this.labelRolagem.Size = new System.Drawing.Size(113, 22);
            this.labelRolagem.Text = "Tempo de Rolagem:";
            // 
            // tbTempoRolagem
            // 
            this.tbTempoRolagem.MaxLength = 4;
            this.tbTempoRolagem.Name = "tbTempoRolagem";
            this.tbTempoRolagem.Size = new System.Drawing.Size(30, 25);
            this.tbTempoRolagem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTempoRolagem_KeyPress);
            this.tbTempoRolagem.TextChanged += new System.EventHandler(this.tbTempoRolagem_TextChanged);
            // 
            // labelMsRolagem
            // 
            this.labelMsRolagem.Name = "labelMsRolagem";
            this.labelMsRolagem.Size = new System.Drawing.Size(35, 22);
            this.labelMsRolagem.Text = "ms    ";
            // 
            // TextosEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBoxInfo);
            this.Name = "TextosEditor";
            this.Size = new System.Drawing.Size(1116, 671);
            this.VisibleChanged += new System.EventHandler(this.TextosEditor_VisibleChanged);
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.barraFerramentas.ResumeLayout(false);
            this.barraFerramentas.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelIndice;
        private System.Windows.Forms.TextBox tbIndice;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.TextBox tbNumero;
        private System.Windows.Forms.Label labelNumero;
        private System.Windows.Forms.ToolStrip barraFerramentas;
        private System.Windows.Forms.ToolStripComboBox cbModelo;
        private System.Windows.Forms.ToolStripComboBox cbApresentacao;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripLabel labelModelo;
        private System.Windows.Forms.ToolStripLabel labelApresentacao;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.Button btContinuar;
        private System.Windows.Forms.Panel panelModelos;
        private System.Windows.Forms.ToolStripTextBox tbTempoApresentacao;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel labelRolagem;
        private System.Windows.Forms.ToolStripTextBox tbTempoRolagem;
        private System.Windows.Forms.ToolStripLabel labelMsRolagem;
        private System.Windows.Forms.TextBox tbLabelFrase;
        private System.Windows.Forms.Label labelFrase;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel labelFonte;
        private System.Windows.Forms.ToolStripComboBox cbFonte;
        private System.Windows.Forms.ToolStripLabel labelTamanho;
        private System.Windows.Forms.ToolStripComboBox cbTamanhoFonte;
        private System.Windows.Forms.ToolStripButton btNegrito;
        private System.Windows.Forms.ToolStripButton btItalico;
        private System.Windows.Forms.ToolStripButton btSublinhado;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btMudarFonte;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btAlinharEsquerda;
        private System.Windows.Forms.ToolStripButton btAlinharMeio;
        private System.Windows.Forms.ToolStripButton btAlinharDireita;
        private System.Windows.Forms.ToolStripButton btAlinharAbaixo;
        private System.Windows.Forms.ToolStripButton btAlinharCentro;
        private System.Windows.Forms.ToolStripButton btAlinharAcima;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.CheckBox chkTextoAutomatico;
        private System.Windows.Forms.ToolStripComboBox cbNivelSuavizacao;
        private System.Windows.Forms.CheckBox chkAgrupar;
        private System.Windows.Forms.ToolStripLabel lbNivelSuavizacao;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel lbImage;
        private System.Windows.Forms.ToolStripComboBox cbListaImages;
        private System.Windows.Forms.ToolStripButton btInsertImage;
        private System.Windows.Forms.ToolStripLabel labelMultiLinha;
        private System.Windows.Forms.ToolStripComboBox cbMultinha;
    }
}
