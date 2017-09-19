namespace LPontos2
{
    partial class FormPresentationSequence
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPresentationSequence));
            this.lboxTpExibicao = new System.Windows.Forms.ListBox();
            this.lboxTpExibicaoSel = new System.Windows.Forms.ListBox();
            this.btAdicionar = new System.Windows.Forms.Button();
            this.btRemover = new System.Windows.Forms.Button();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btAplicar = new System.Windows.Forms.Button();
            this.gboxAlternancia = new System.Windows.Forms.GroupBox();
            this.tboxFileName = new System.Windows.Forms.TextBox();
            this.btRestaurar = new System.Windows.Forms.Button();
            this.gboxAlternancia.SuspendLayout();
            this.SuspendLayout();
            // 
            // lboxTpExibicao
            // 
            this.lboxTpExibicao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lboxTpExibicao.FormattingEnabled = true;
            this.lboxTpExibicao.Location = new System.Drawing.Point(7, 46);
            this.lboxTpExibicao.Name = "lboxTpExibicao";
            this.lboxTpExibicao.Size = new System.Drawing.Size(189, 197);
            this.lboxTpExibicao.TabIndex = 1;
            this.lboxTpExibicao.SelectedIndexChanged += new System.EventHandler(this.lboxTpExibicao_SelectedIndexChanged);
            // 
            // lboxTpExibicaoSel
            // 
            this.lboxTpExibicaoSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lboxTpExibicaoSel.FormattingEnabled = true;
            this.lboxTpExibicaoSel.Location = new System.Drawing.Point(315, 46);
            this.lboxTpExibicaoSel.Name = "lboxTpExibicaoSel";
            this.lboxTpExibicaoSel.Size = new System.Drawing.Size(183, 197);
            this.lboxTpExibicaoSel.TabIndex = 4;
            this.lboxTpExibicaoSel.SelectedIndexChanged += new System.EventHandler(this.lboxTpExibicaoSel_SelectedIndexChanged);
            // 
            // btAdicionar
            // 
            this.btAdicionar.Location = new System.Drawing.Point(220, 111);
            this.btAdicionar.Name = "btAdicionar";
            this.btAdicionar.Size = new System.Drawing.Size(75, 23);
            this.btAdicionar.TabIndex = 2;
            this.btAdicionar.Text = "Adicionar";
            this.btAdicionar.UseVisualStyleBackColor = true;
            this.btAdicionar.Click += new System.EventHandler(this.btAdicionar_Click);
            // 
            // btRemover
            // 
            this.btRemover.Location = new System.Drawing.Point(220, 140);
            this.btRemover.Name = "btRemover";
            this.btRemover.Size = new System.Drawing.Size(75, 23);
            this.btRemover.TabIndex = 3;
            this.btRemover.Text = "Remover";
            this.btRemover.UseVisualStyleBackColor = true;
            this.btRemover.Click += new System.EventHandler(this.btRemover_Click);
            // 
            // btCancelar
            // 
            this.btCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelar.Location = new System.Drawing.Point(364, 272);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 1;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btAplicar
            // 
            this.btAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btAplicar.Location = new System.Drawing.Point(445, 272);
            this.btAplicar.Name = "btAplicar";
            this.btAplicar.Size = new System.Drawing.Size(75, 23);
            this.btAplicar.TabIndex = 2;
            this.btAplicar.Text = "Aplicar";
            this.btAplicar.UseVisualStyleBackColor = true;
            this.btAplicar.Click += new System.EventHandler(this.btAplicar_Click);
            // 
            // gboxAlternancia
            // 
            this.gboxAlternancia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxAlternancia.Controls.Add(this.tboxFileName);
            this.gboxAlternancia.Controls.Add(this.lboxTpExibicao);
            this.gboxAlternancia.Controls.Add(this.lboxTpExibicaoSel);
            this.gboxAlternancia.Controls.Add(this.btAdicionar);
            this.gboxAlternancia.Controls.Add(this.btRemover);
            this.gboxAlternancia.Location = new System.Drawing.Point(12, 12);
            this.gboxAlternancia.Name = "gboxAlternancia";
            this.gboxAlternancia.Size = new System.Drawing.Size(508, 254);
            this.gboxAlternancia.TabIndex = 0;
            this.gboxAlternancia.TabStop = false;
            this.gboxAlternancia.Text = "Nova Alternância";
            // 
            // tboxFileName
            // 
            this.tboxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxFileName.Location = new System.Drawing.Point(7, 20);
            this.tboxFileName.MaxLength = 32;
            this.tboxFileName.Name = "tboxFileName";
            this.tboxFileName.Size = new System.Drawing.Size(491, 20);
            this.tboxFileName.TabIndex = 0;
            this.tboxFileName.Text = "File Name";
            // 
            // btRestaurar
            // 
            this.btRestaurar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btRestaurar.AutoSize = true;
            this.btRestaurar.Location = new System.Drawing.Point(12, 272);
            this.btRestaurar.Name = "btRestaurar";
            this.btRestaurar.Size = new System.Drawing.Size(100, 23);
            this.btRestaurar.TabIndex = 3;
            this.btRestaurar.Text = "Restaurar Padrão";
            this.btRestaurar.UseVisualStyleBackColor = true;
            this.btRestaurar.Click += new System.EventHandler(this.btRestaurar_Click);
            // 
            // FormPresentationSequence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 305);
            this.Controls.Add(this.btRestaurar);
            this.Controls.Add(this.gboxAlternancia);
            this.Controls.Add(this.btAplicar);
            this.Controls.Add(this.btCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(551, 343);
            this.Name = "FormPresentationSequence";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alternância";
            this.Shown += new System.EventHandler(this.FormPresentationSequence_Shown);
            this.gboxAlternancia.ResumeLayout(false);
            this.gboxAlternancia.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lboxTpExibicao;
        private System.Windows.Forms.ListBox lboxTpExibicaoSel;
        private System.Windows.Forms.Button btAdicionar;
        private System.Windows.Forms.Button btRemover;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btAplicar;
        private System.Windows.Forms.GroupBox gboxAlternancia;
        private System.Windows.Forms.TextBox tboxFileName;
        private System.Windows.Forms.Button btRestaurar;
    }
}