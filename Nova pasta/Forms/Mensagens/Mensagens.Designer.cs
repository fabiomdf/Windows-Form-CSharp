namespace LPontos2.Forms.Mensagens
{
    partial class Mensagens
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mensagens));
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.tboxNomeMensagem = new System.Windows.Forms.TextBox();
            this.lblNomeMensagem = new System.Windows.Forms.Label();
            this.labelIndiceMensagem = new System.Windows.Forms.Label();
            this.chbRepetir = new System.Windows.Forms.CheckBox();
            this.cbApresentacaoPadrao = new System.Windows.Forms.ComboBox();
            this.labelApresentaçãoPadrão = new System.Windows.Forms.Label();
            this.tbIndice = new System.Windows.Forms.TextBox();
            this.tabTextos = new System.Windows.Forms.TabControl();
            this.tabIda = new System.Windows.Forms.TabPage();
            this.btnExcluirPainel = new System.Windows.Forms.Button();
            this.btnIncluirPainel = new System.Windows.Forms.Button();
            this.listViewMensagens = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.groupBoxInfo.SuspendLayout();
            this.tabTextos.SuspendLayout();
            this.tabIda.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInfo.Controls.Add(this.tboxNomeMensagem);
            this.groupBoxInfo.Controls.Add(this.lblNomeMensagem);
            this.groupBoxInfo.Controls.Add(this.labelIndiceMensagem);
            this.groupBoxInfo.Controls.Add(this.chbRepetir);
            this.groupBoxInfo.Controls.Add(this.cbApresentacaoPadrao);
            this.groupBoxInfo.Controls.Add(this.labelApresentaçãoPadrão);
            this.groupBoxInfo.Controls.Add(this.tbIndice);
            this.groupBoxInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxInfo.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(634, 173);
            this.groupBoxInfo.TabIndex = 0;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Informações básicas";
            // 
            // tboxNomeMensagem
            // 
            this.tboxNomeMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxNomeMensagem.Location = new System.Drawing.Point(6, 108);
            this.tboxNomeMensagem.MaxLength = 16;
            this.tboxNomeMensagem.Name = "tboxNomeMensagem";
            this.tboxNomeMensagem.Size = new System.Drawing.Size(622, 20);
            this.tboxNomeMensagem.TabIndex = 2;
            this.tboxNomeMensagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxNomeMensagem.TextChanged += new System.EventHandler(this.tboxNomeMensagem_TextChanged);
            // 
            // lblNomeMensagem
            // 
            this.lblNomeMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNomeMensagem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeMensagem.Location = new System.Drawing.Point(6, 92);
            this.lblNomeMensagem.Name = "lblNomeMensagem";
            this.lblNomeMensagem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblNomeMensagem.Size = new System.Drawing.Size(622, 13);
            this.lblNomeMensagem.TabIndex = 16;
            this.lblNomeMensagem.Text = "Indice Mensagem";
            this.lblNomeMensagem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelIndiceMensagem
            // 
            this.labelIndiceMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelIndiceMensagem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIndiceMensagem.Location = new System.Drawing.Point(6, 13);
            this.labelIndiceMensagem.Name = "labelIndiceMensagem";
            this.labelIndiceMensagem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelIndiceMensagem.Size = new System.Drawing.Size(622, 13);
            this.labelIndiceMensagem.TabIndex = 14;
            this.labelIndiceMensagem.Text = "Indice Mensagem";
            this.labelIndiceMensagem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbRepetir
            // 
            this.chbRepetir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chbRepetir.AutoSize = true;
            this.chbRepetir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbRepetir.Location = new System.Drawing.Point(279, 134);
            this.chbRepetir.Name = "chbRepetir";
            this.chbRepetir.Size = new System.Drawing.Size(349, 17);
            this.chbRepetir.TabIndex = 3;
            this.chbRepetir.Text = "Repetir textos inseridos no painel 01 em todos os painéis";
            this.chbRepetir.UseVisualStyleBackColor = true;
            // 
            // cbApresentacaoPadrao
            // 
            this.cbApresentacaoPadrao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbApresentacaoPadrao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbApresentacaoPadrao.FormattingEnabled = true;
            this.cbApresentacaoPadrao.Location = new System.Drawing.Point(6, 68);
            this.cbApresentacaoPadrao.Name = "cbApresentacaoPadrao";
            this.cbApresentacaoPadrao.Size = new System.Drawing.Size(622, 21);
            this.cbApresentacaoPadrao.TabIndex = 1;
            this.cbApresentacaoPadrao.SelectedIndexChanged += new System.EventHandler(this.cbApresentacaoPadrao_SelectedIndexChanged);
            // 
            // labelApresentaçãoPadrão
            // 
            this.labelApresentaçãoPadrão.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelApresentaçãoPadrão.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApresentaçãoPadrão.Location = new System.Drawing.Point(6, 52);
            this.labelApresentaçãoPadrão.Name = "labelApresentaçãoPadrão";
            this.labelApresentaçãoPadrão.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelApresentaçãoPadrão.Size = new System.Drawing.Size(622, 13);
            this.labelApresentaçãoPadrão.TabIndex = 11;
            this.labelApresentaçãoPadrão.Text = "Apresentação padrão";
            this.labelApresentaçãoPadrão.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbIndice
            // 
            this.tbIndice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIndice.Location = new System.Drawing.Point(6, 30);
            this.tbIndice.Name = "tbIndice";
            this.tbIndice.ReadOnly = true;
            this.tbIndice.Size = new System.Drawing.Size(622, 20);
            this.tbIndice.TabIndex = 0;
            this.tbIndice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabTextos
            // 
            this.tabTextos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabTextos.Controls.Add(this.tabIda);
            this.tabTextos.Location = new System.Drawing.Point(3, 182);
            this.tabTextos.Name = "tabTextos";
            this.tabTextos.SelectedIndex = 0;
            this.tabTextos.Size = new System.Drawing.Size(634, 262);
            this.tabTextos.TabIndex = 1;
            // 
            // tabIda
            // 
            this.tabIda.Controls.Add(this.btnExcluirPainel);
            this.tabIda.Controls.Add(this.btnIncluirPainel);
            this.tabIda.Controls.Add(this.listViewMensagens);
            this.tabIda.Location = new System.Drawing.Point(4, 22);
            this.tabIda.Name = "tabIda";
            this.tabIda.Padding = new System.Windows.Forms.Padding(3);
            this.tabIda.Size = new System.Drawing.Size(626, 236);
            this.tabIda.TabIndex = 0;
            this.tabIda.Text = "Textos de Ida";
            this.tabIda.UseVisualStyleBackColor = true;
            // 
            // btnExcluirPainel
            // 
            this.btnExcluirPainel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcluirPainel.Image = ((System.Drawing.Image)(resources.GetObject("btnExcluirPainel.Image")));
            this.btnExcluirPainel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExcluirPainel.Location = new System.Drawing.Point(492, 76);
            this.btnExcluirPainel.Name = "btnExcluirPainel";
            this.btnExcluirPainel.Size = new System.Drawing.Size(104, 30);
            this.btnExcluirPainel.TabIndex = 2;
            this.btnExcluirPainel.Text = "Excluir";
            this.btnExcluirPainel.UseVisualStyleBackColor = true;
            this.btnExcluirPainel.Click += new System.EventHandler(this.btnExcluirPainel_Click);
            // 
            // btnIncluirPainel
            // 
            this.btnIncluirPainel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncluirPainel.Image = ((System.Drawing.Image)(resources.GetObject("btnIncluirPainel.Image")));
            this.btnIncluirPainel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIncluirPainel.Location = new System.Drawing.Point(492, 40);
            this.btnIncluirPainel.Name = "btnIncluirPainel";
            this.btnIncluirPainel.Size = new System.Drawing.Size(104, 30);
            this.btnIncluirPainel.TabIndex = 1;
            this.btnIncluirPainel.Text = "Incluir";
            this.btnIncluirPainel.UseVisualStyleBackColor = true;
            this.btnIncluirPainel.Click += new System.EventHandler(this.btnIncluirPainel_Click);
            // 
            // listViewMensagens
            // 
            this.listViewMensagens.AllowDrop = true;
            this.listViewMensagens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewMensagens.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.listViewMensagens.FullRowSelect = true;
            this.listViewMensagens.GridLines = true;
            this.listViewMensagens.HideSelection = false;
            this.listViewMensagens.Location = new System.Drawing.Point(6, 6);
            this.listViewMensagens.MultiSelect = false;
            this.listViewMensagens.Name = "listViewMensagens";
            this.listViewMensagens.Size = new System.Drawing.Size(614, 224);
            this.listViewMensagens.TabIndex = 0;
            this.listViewMensagens.UseCompatibleStateImageBehavior = false;
            this.listViewMensagens.View = System.Windows.Forms.View.Details;
            this.listViewMensagens.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewMensagens_ItemDrag);
            this.listViewMensagens.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewMensagens_ItemSelectionChanged);
            this.listViewMensagens.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewMensagens_DragDrop);
            this.listViewMensagens.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewMensagens_DragEnter);
            this.listViewMensagens.DoubleClick += new System.EventHandler(this.listViewMensagens_DoubleClick);
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
            // btnAplicar
            // 
            this.btnAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAplicar.Location = new System.Drawing.Point(481, 450);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(75, 23);
            this.btnAplicar.TabIndex = 3;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(400, 450);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnContinuar
            // 
            this.btnContinuar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinuar.Location = new System.Drawing.Point(562, 450);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(75, 23);
            this.btnContinuar.TabIndex = 4;
            this.btnContinuar.Text = "Aplicar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // Mensagens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.tabTextos);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.groupBoxInfo);
            this.Name = "Mensagens";
            this.Size = new System.Drawing.Size(640, 480);
            this.VisibleChanged += new System.EventHandler(this.Mensagens_VisibleChanged);
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.tabTextos.ResumeLayout(false);
            this.tabIda.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.CheckBox chbRepetir;
        private System.Windows.Forms.ComboBox cbApresentacaoPadrao;
        private System.Windows.Forms.Label labelApresentaçãoPadrão;
        private System.Windows.Forms.TextBox tbIndice;
        private System.Windows.Forms.TabControl tabTextos;
        private System.Windows.Forms.TabPage tabIda;
        private System.Windows.Forms.Button btnExcluirPainel;
        private System.Windows.Forms.Button btnIncluirPainel;
        private System.Windows.Forms.ListView listViewMensagens;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label labelIndiceMensagem;
        private System.Windows.Forms.Label lblNomeMensagem;
        private System.Windows.Forms.TextBox tboxNomeMensagem;
        private System.Windows.Forms.Button btnContinuar;
    }
}
