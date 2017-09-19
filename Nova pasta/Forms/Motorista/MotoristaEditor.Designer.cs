namespace LPontos2.Forms.Motorista
{
    partial class MotoristaEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MotoristaEditor));
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.tbNome = new System.Windows.Forms.TextBox();
            this.labelNome = new System.Windows.Forms.Label();
            this.labelID = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
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
            this.barraFerramentas = new System.Windows.Forms.ToolStrip();
            this.labelApresentacao = new System.Windows.Forms.ToolStripLabel();
            this.cbApresentacao = new System.Windows.Forms.ToolStripComboBox();
            this.tbTempoApresentacao = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.labelRolagem = new System.Windows.Forms.ToolStripLabel();
            this.tbTempoRolagem = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelModelos = new System.Windows.Forms.Panel();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btSalvar = new System.Windows.Forms.Button();
            this.groupBoxInfo.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.barraFerramentas.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInfo.Controls.Add(this.tbNome);
            this.groupBoxInfo.Controls.Add(this.labelNome);
            this.groupBoxInfo.Controls.Add(this.labelID);
            this.groupBoxInfo.Controls.Add(this.tbID);
            this.groupBoxInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxInfo.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(1110, 116);
            this.groupBoxInfo.TabIndex = 3;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Informações básicas";
            // 
            // tbNome
            // 
            this.tbNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNome.Enabled = false;
            this.tbNome.Location = new System.Drawing.Point(6, 81);
            this.tbNome.Name = "tbNome";
            this.tbNome.Size = new System.Drawing.Size(1098, 20);
            this.tbNome.TabIndex = 3;
            this.tbNome.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelNome
            // 
            this.labelNome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNome.Location = new System.Drawing.Point(9, 65);
            this.labelNome.Name = "labelNome";
            this.labelNome.Size = new System.Drawing.Size(1095, 13);
            this.labelNome.TabIndex = 2;
            this.labelNome.Text = "Número do roteiro:";
            this.labelNome.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelID
            // 
            this.labelID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelID.Location = new System.Drawing.Point(6, 26);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(1098, 13);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "Índice do roteiro:";
            this.labelID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbID
            // 
            this.tbID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(6, 42);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(1098, 20);
            this.tbID.TabIndex = 0;
            this.tbID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.cbNivelSuavizacao});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1110, 25);
            this.toolStrip1.TabIndex = 7;
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
            // barraFerramentas
            // 
            this.barraFerramentas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.barraFerramentas.Size = new System.Drawing.Size(1110, 25);
            this.barraFerramentas.TabIndex = 1;
            this.barraFerramentas.Text = "toolStrip1";
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
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.panelModelos);
            this.panel1.Controls.Add(this.btCancelar);
            this.panel1.Controls.Add(this.btSalvar);
            this.panel1.Controls.Add(this.barraFerramentas);
            this.panel1.Location = new System.Drawing.Point(3, 125);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1110, 399);
            this.panel1.TabIndex = 4;
            // 
            // panelModelos
            // 
            this.panelModelos.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelModelos.Location = new System.Drawing.Point(295, 105);
            this.panelModelos.Name = "panelModelos";
            this.panelModelos.Size = new System.Drawing.Size(500, 200);
            this.panelModelos.TabIndex = 6;
            // 
            // btCancelar
            // 
            this.btCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelar.Location = new System.Drawing.Point(951, 373);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 5;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btSalvar
            // 
            this.btSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSalvar.Location = new System.Drawing.Point(1032, 373);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(75, 23);
            this.btSalvar.TabIndex = 3;
            this.btSalvar.Text = "Salvar";
            this.btSalvar.UseVisualStyleBackColor = true;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // MotoristaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.panel1);
            this.Name = "MotoristaEditor";
            this.Size = new System.Drawing.Size(1116, 527);
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.barraFerramentas.ResumeLayout(false);
            this.barraFerramentas.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.TextBox tbNome;
        private System.Windows.Forms.Label labelNome;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.TextBox tbID;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lbNivelSuavizacao;
        private System.Windows.Forms.ToolStripComboBox cbNivelSuavizacao;
        private System.Windows.Forms.ToolStrip barraFerramentas;
        private System.Windows.Forms.ToolStripLabel labelApresentacao;
        private System.Windows.Forms.ToolStripComboBox cbApresentacao;
        private System.Windows.Forms.ToolStripTextBox tbTempoApresentacao;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel labelRolagem;
        private System.Windows.Forms.ToolStripTextBox tbTempoRolagem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelModelos;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btSalvar;
    }
}
