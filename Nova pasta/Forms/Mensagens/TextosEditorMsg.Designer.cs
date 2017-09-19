namespace LPontos2.Forms.Mensagens
{
    partial class TextosEditorMsg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextosEditorMsg));
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.chkTextoAutomatico = new System.Windows.Forms.CheckBox();
            this.tbLabelFrase = new System.Windows.Forms.TextBox();
            this.labelTexto = new System.Windows.Forms.Label();
            this.labelIndiceMensagem = new System.Windows.Forms.Label();
            this.tbIndice = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAgrupar = new System.Windows.Forms.CheckBox();
            this.panelModelos = new System.Windows.Forms.Panel();
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
            this.barraFerramentas = new System.Windows.Forms.ToolStrip();
            this.labelModelo = new System.Windows.Forms.ToolStripLabel();
            this.cbModelo = new System.Windows.Forms.ToolStripComboBox();
            this.labelApresentacao = new System.Windows.Forms.ToolStripLabel();
            this.cbApresentacao = new System.Windows.Forms.ToolStripComboBox();
            this.tbTempoApresentacao = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.labelRolagem = new System.Windows.Forms.ToolStripLabel();
            this.tbTempoRolagem = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.groupBoxInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.barraFerramentas.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInfo.Controls.Add(this.chkTextoAutomatico);
            this.groupBoxInfo.Controls.Add(this.tbLabelFrase);
            this.groupBoxInfo.Controls.Add(this.labelTexto);
            this.groupBoxInfo.Controls.Add(this.labelIndiceMensagem);
            this.groupBoxInfo.Controls.Add(this.tbIndice);
            this.groupBoxInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxInfo.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(1096, 111);
            this.groupBoxInfo.TabIndex = 1;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Informações básicas";
            // 
            // chkTextoAutomatico
            // 
            this.chkTextoAutomatico.AutoSize = true;
            this.chkTextoAutomatico.Checked = true;
            this.chkTextoAutomatico.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTextoAutomatico.Location = new System.Drawing.Point(6, 77);
            this.chkTextoAutomatico.Name = "chkTextoAutomatico";
            this.chkTextoAutomatico.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkTextoAutomatico.Size = new System.Drawing.Size(221, 17);
            this.chkTextoAutomatico.TabIndex = 2;
            this.chkTextoAutomatico.Text = "Preencher Texto Automaticamente";
            this.chkTextoAutomatico.UseVisualStyleBackColor = true;
            this.chkTextoAutomatico.CheckedChanged += new System.EventHandler(this.chkTextoAutomatico_CheckedChanged);
            // 
            // tbLabelFrase
            // 
            this.tbLabelFrase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLabelFrase.Enabled = false;
            this.tbLabelFrase.Location = new System.Drawing.Point(233, 75);
            this.tbLabelFrase.MaxLength = 50;
            this.tbLabelFrase.Name = "tbLabelFrase";
            this.tbLabelFrase.Size = new System.Drawing.Size(856, 20);
            this.tbLabelFrase.TabIndex = 3;
            this.tbLabelFrase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbLabelFrase.TextChanged += new System.EventHandler(this.tbLabelFrase_TextChanged);
            // 
            // labelTexto
            // 
            this.labelTexto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTexto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTexto.Location = new System.Drawing.Point(9, 59);
            this.labelTexto.Name = "labelTexto";
            this.labelTexto.Size = new System.Drawing.Size(1080, 13);
            this.labelTexto.TabIndex = 9;
            this.labelTexto.Text = "Texto:";
            this.labelTexto.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIndiceMensagem
            // 
            this.labelIndiceMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelIndiceMensagem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIndiceMensagem.Location = new System.Drawing.Point(6, 16);
            this.labelIndiceMensagem.Name = "labelIndiceMensagem";
            this.labelIndiceMensagem.Size = new System.Drawing.Size(1084, 12);
            this.labelIndiceMensagem.TabIndex = 6;
            this.labelIndiceMensagem.Text = "Índice da mensagem:";
            this.labelIndiceMensagem.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbIndice
            // 
            this.tbIndice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIndice.Enabled = false;
            this.tbIndice.Location = new System.Drawing.Point(6, 32);
            this.tbIndice.Name = "tbIndice";
            this.tbIndice.Size = new System.Drawing.Size(1084, 20);
            this.tbIndice.TabIndex = 1;
            this.tbIndice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.chkAgrupar);
            this.panel1.Controls.Add(this.panelModelos);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.barraFerramentas);
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnSalvar);
            this.panel1.Controls.Add(this.btnContinuar);
            this.panel1.Location = new System.Drawing.Point(3, 120);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1096, 357);
            this.panel1.TabIndex = 4;
            // 
            // chkAgrupar
            // 
            this.chkAgrupar.AutoSize = true;
            this.chkAgrupar.BackColor = System.Drawing.SystemColors.Control;
            this.chkAgrupar.Checked = true;
            this.chkAgrupar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAgrupar.Location = new System.Drawing.Point(776, 5);
            this.chkAgrupar.Name = "chkAgrupar";
            this.chkAgrupar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkAgrupar.Size = new System.Drawing.Size(100, 17);
            this.chkAgrupar.TabIndex = 3;
            this.chkAgrupar.Text = "Mesclar Paineis";
            this.chkAgrupar.UseVisualStyleBackColor = false;
            this.chkAgrupar.CheckedChanged += new System.EventHandler(this.chkAgrupar_CheckedChanged);
            // 
            // panelModelos
            // 
            this.panelModelos.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelModelos.Location = new System.Drawing.Point(302, 85);
            this.panelModelos.Name = "panelModelos";
            this.panelModelos.Size = new System.Drawing.Size(500, 208);
            this.panelModelos.TabIndex = 5;
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
            this.toolStrip1.Size = new System.Drawing.Size(1096, 25);
            this.toolStrip1.TabIndex = 4;
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
            this.cbNivelSuavizacao.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
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
            // barraFerramentas
            // 
            this.barraFerramentas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelModelo,
            this.cbModelo,
            this.labelApresentacao,
            this.cbApresentacao,
            this.tbTempoApresentacao,
            this.toolStripLabel6,
            this.labelRolagem,
            this.tbTempoRolagem,
            this.toolStripLabel7});
            this.barraFerramentas.Location = new System.Drawing.Point(0, 0);
            this.barraFerramentas.Name = "barraFerramentas";
            this.barraFerramentas.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.barraFerramentas.Size = new System.Drawing.Size(1096, 25);
            this.barraFerramentas.TabIndex = 2;
            this.barraFerramentas.Text = "toolStrip1";
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
            this.cbModelo.Name = "cbModelo";
            this.cbModelo.Size = new System.Drawing.Size(170, 25);
            this.cbModelo.SelectedIndexChanged += new System.EventHandler(this.cbModelo_SelectedIndexChanged);
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
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(35, 22);
            this.toolStripLabel7.Text = "ms    ";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(859, 330);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalvar.Location = new System.Drawing.Point(940, 330);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 7;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnContinuar
            // 
            this.btnContinuar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinuar.Location = new System.Drawing.Point(1021, 330);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(75, 23);
            this.btnContinuar.TabIndex = 8;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // TextosEditorMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.panel1);
            this.Name = "TextosEditorMsg";
            this.Size = new System.Drawing.Size(1102, 480);
            this.VisibleChanged += new System.EventHandler(this.TextosEditorMsg_VisibleChanged);
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

        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelIndiceMensagem;
        private System.Windows.Forms.TextBox tbIndice;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnContinuar;
        private System.Windows.Forms.CheckBox chkTextoAutomatico;
        private System.Windows.Forms.TextBox tbLabelFrase;
        private System.Windows.Forms.Label labelTexto;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btAlinharAbaixo;
        private System.Windows.Forms.ToolStripButton btAlinharCentro;
        private System.Windows.Forms.ToolStripButton btAlinharAcima;
        private System.Windows.Forms.ToolStrip barraFerramentas;
        private System.Windows.Forms.ToolStripLabel labelModelo;
        private System.Windows.Forms.ToolStripComboBox cbModelo;
        private System.Windows.Forms.ToolStripLabel labelApresentacao;
        private System.Windows.Forms.ToolStripComboBox cbApresentacao;
        private System.Windows.Forms.ToolStripTextBox tbTempoApresentacao;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel labelRolagem;
        private System.Windows.Forms.ToolStripTextBox tbTempoRolagem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.Panel panelModelos;
        private System.Windows.Forms.CheckBox chkAgrupar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox cbNivelSuavizacao;
        private System.Windows.Forms.ToolStripLabel lbNivelSuavizacao;
        private System.Windows.Forms.ToolStripLabel lbImage;
        private System.Windows.Forms.ToolStripComboBox cbListaImages;
        private System.Windows.Forms.ToolStripButton btInsertImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;

    }
}
