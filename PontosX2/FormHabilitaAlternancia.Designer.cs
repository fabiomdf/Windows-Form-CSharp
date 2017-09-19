namespace LPontos2
{
    partial class FormHabilitaAlternancia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHabilitaAlternancia));
            this.chklistboxAlternancias = new System.Windows.Forms.CheckedListBox();
            this.lbAlternancia = new System.Windows.Forms.Label();
            this.btFechar = new System.Windows.Forms.Button();
            this.btAplicar = new System.Windows.Forms.Button();
            this.btRestaurarPadroes = new System.Windows.Forms.Button();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chklistboxAlternancias
            // 
            this.chklistboxAlternancias.CheckOnClick = true;
            this.chklistboxAlternancias.FormattingEnabled = true;
            this.chklistboxAlternancias.Items.AddRange(new object[] {
            "Apresentar somente o roteiro",
            "Apresentar somente número do roteiro",
            "Apresentar somente mensagem",
            "Apresentar somente hora",
            "Apresentar o roteiro alternando com Mensagem",
            "Apresentar o roteiro alternando com Saudação",
            "Apresentar o roteiro alternando com Data Hora",
            "Apresentar o roteiro alternando com Hora Partida",
            "Apresentar o roteiro alternando com Mensagem e Saudação",
            "Apresentar o roteiro alternando com Mensagem e Hora Partida",
            "Apresentar o roteiro alternando com Mensagem e Data Hora",
            "Apresentar o roteiro alternando com Tarifa",
            "Apresentar a mensagem alternando com hora",
            "Apresentar a mensagem alternando com hora e temperatura",
            "Apenas Temperatura",
            "Apenas Velocidade",
            "Alterna roteiro com velocidade",
            "Alterna mensagem com velocidade",
            "Alterna roteiro com temperatura",
            "Alterna mensagem com temperatura",
            "Alterna roteiro com hora e temperatura (Separados)",
            "Alterna mensagem com hora e temperatura (Separados)"});
            this.chklistboxAlternancias.Location = new System.Drawing.Point(12, 28);
            this.chklistboxAlternancias.Name = "chklistboxAlternancias";
            this.chklistboxAlternancias.Size = new System.Drawing.Size(440, 349);
            this.chklistboxAlternancias.TabIndex = 14;
            // 
            // lbAlternancia
            // 
            this.lbAlternancia.AutoSize = true;
            this.lbAlternancia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbAlternancia.Location = new System.Drawing.Point(9, 8);
            this.lbAlternancia.Name = "lbAlternancia";
            this.lbAlternancia.Size = new System.Drawing.Size(262, 13);
            this.lbAlternancia.TabIndex = 12;
            this.lbAlternancia.Text = "Bloqueia o acesso a algumas funções de alternância. ";
            // 
            // btFechar
            // 
            this.btFechar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btFechar.Location = new System.Drawing.Point(312, 403);
            this.btFechar.Name = "btFechar";
            this.btFechar.Size = new System.Drawing.Size(67, 23);
            this.btFechar.TabIndex = 11;
            this.btFechar.Text = "Fechar";
            this.btFechar.UseVisualStyleBackColor = true;
            this.btFechar.Click += new System.EventHandler(this.btFechar_Click);
            // 
            // btAplicar
            // 
            this.btAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btAplicar.Location = new System.Drawing.Point(385, 403);
            this.btAplicar.Name = "btAplicar";
            this.btAplicar.Size = new System.Drawing.Size(67, 23);
            this.btAplicar.TabIndex = 10;
            this.btAplicar.Text = "Aplicar";
            this.btAplicar.UseVisualStyleBackColor = true;
            this.btAplicar.Click += new System.EventHandler(this.btAplicar_Click);
            // 
            // btRestaurarPadroes
            // 
            this.btRestaurarPadroes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btRestaurarPadroes.AutoSize = true;
            this.btRestaurarPadroes.Location = new System.Drawing.Point(12, 403);
            this.btRestaurarPadroes.Name = "btRestaurarPadroes";
            this.btRestaurarPadroes.Size = new System.Drawing.Size(65, 23);
            this.btRestaurarPadroes.TabIndex = 9;
            this.btRestaurarPadroes.Text = "Restaurar";
            this.btRestaurarPadroes.UseVisualStyleBackColor = true;
            this.btRestaurarPadroes.Click += new System.EventHandler(this.btRestaurarPadroes_Click);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(15, 380);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(93, 17);
            this.chkAll.TabIndex = 15;
            this.chkAll.Text = "Checar Todos";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.Click += new System.EventHandler(this.chkAll_Click);
            // 
            // FormHabilitaAlternancia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 431);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.chklistboxAlternancias);
            this.Controls.Add(this.lbAlternancia);
            this.Controls.Add(this.btFechar);
            this.Controls.Add(this.btAplicar);
            this.Controls.Add(this.btRestaurarPadroes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHabilitaAlternancia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Habilitar ou bloquear funções de alternância.";
            this.Load += new System.EventHandler(this.FormHabilitaAlternancia_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chklistboxAlternancias;
        private System.Windows.Forms.Label lbAlternancia;
        private System.Windows.Forms.Button btFechar;
        private System.Windows.Forms.Button btAplicar;
        private System.Windows.Forms.Button btRestaurarPadroes;
        private System.Windows.Forms.CheckBox chkAll;
    }
}