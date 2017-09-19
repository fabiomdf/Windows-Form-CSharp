namespace LPontos2
{
    partial class FormPerifericoAnexo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPerifericoAnexo));
            this.btCancelar = new System.Windows.Forms.Button();
            this.btAplicar = new System.Windows.Forms.Button();
            this.gboxPerifericos = new System.Windows.Forms.GroupBox();
            this.cbListPaineis = new System.Windows.Forms.CheckedListBox();
            this.chkTemp = new System.Windows.Forms.CheckBox();
            this.chkApp = new System.Windows.Forms.CheckBox();
            this.chkVelocidade = new System.Windows.Forms.CheckBox();
            this.chkCatraca = new System.Windows.Forms.CheckBox();
            this.gboxPerifericos.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancelar
            // 
            this.btCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancelar.Location = new System.Drawing.Point(228, 192);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 21;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            // 
            // btAplicar
            // 
            this.btAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btAplicar.Location = new System.Drawing.Point(309, 192);
            this.btAplicar.Name = "btAplicar";
            this.btAplicar.Size = new System.Drawing.Size(75, 23);
            this.btAplicar.TabIndex = 22;
            this.btAplicar.Text = "Aplicar";
            this.btAplicar.UseVisualStyleBackColor = true;
            this.btAplicar.Click += new System.EventHandler(this.btAplicar_Click);
            // 
            // gboxPerifericos
            // 
            this.gboxPerifericos.Controls.Add(this.cbListPaineis);
            this.gboxPerifericos.Controls.Add(this.chkTemp);
            this.gboxPerifericos.Controls.Add(this.chkApp);
            this.gboxPerifericos.Controls.Add(this.chkVelocidade);
            this.gboxPerifericos.Controls.Add(this.chkCatraca);
            this.gboxPerifericos.Location = new System.Drawing.Point(12, 12);
            this.gboxPerifericos.Name = "gboxPerifericos";
            this.gboxPerifericos.Size = new System.Drawing.Size(372, 174);
            this.gboxPerifericos.TabIndex = 25;
            this.gboxPerifericos.TabStop = false;
            this.gboxPerifericos.Text = "Selecione os periféricos em anexo";
            // 
            // cbListPaineis
            // 
            this.cbListPaineis.CheckOnClick = true;
            this.cbListPaineis.FormattingEnabled = true;
            this.cbListPaineis.Items.AddRange(new object[] {
            "Painel 1",
            "Painel 2",
            "Painel 3",
            "Painel 4",
            "Painel 5"});
            this.cbListPaineis.Location = new System.Drawing.Point(59, 88);
            this.cbListPaineis.Name = "cbListPaineis";
            this.cbListPaineis.Size = new System.Drawing.Size(307, 79);
            this.cbListPaineis.TabIndex = 29;
            this.cbListPaineis.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbListPaineis_ItemCheck);
            // 
            // chkTemp
            // 
            this.chkTemp.AutoSize = true;
            this.chkTemp.Location = new System.Drawing.Point(6, 65);
            this.chkTemp.Name = "chkTemp";
            this.chkTemp.Size = new System.Drawing.Size(137, 17);
            this.chkTemp.TabIndex = 28;
            this.chkTemp.Text = "Sensor de Temperatura";
            this.chkTemp.UseVisualStyleBackColor = true;
            // 
            // chkApp
            // 
            this.chkApp.AutoSize = true;
            this.chkApp.Location = new System.Drawing.Point(6, 88);
            this.chkApp.Name = "chkApp";
            this.chkApp.Size = new System.Drawing.Size(47, 17);
            this.chkApp.TabIndex = 27;
            this.chkApp.Text = "APP";
            this.chkApp.UseVisualStyleBackColor = true;
            this.chkApp.CheckedChanged += new System.EventHandler(this.chkApp_CheckedChanged);
            // 
            // chkVelocidade
            // 
            this.chkVelocidade.AutoSize = true;
            this.chkVelocidade.Location = new System.Drawing.Point(6, 42);
            this.chkVelocidade.Name = "chkVelocidade";
            this.chkVelocidade.Size = new System.Drawing.Size(130, 17);
            this.chkVelocidade.TabIndex = 26;
            this.chkVelocidade.Text = "Sensor de Velocidade";
            this.chkVelocidade.UseVisualStyleBackColor = true;
            // 
            // chkCatraca
            // 
            this.chkCatraca.AutoSize = true;
            this.chkCatraca.Location = new System.Drawing.Point(6, 19);
            this.chkCatraca.Name = "chkCatraca";
            this.chkCatraca.Size = new System.Drawing.Size(63, 17);
            this.chkCatraca.TabIndex = 25;
            this.chkCatraca.Text = "Catraca";
            this.chkCatraca.UseVisualStyleBackColor = true;
            // 
            // FormPerifericoAnexo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 225);
            this.Controls.Add(this.gboxPerifericos);
            this.Controls.Add(this.btAplicar);
            this.Controls.Add(this.btCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPerifericoAnexo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Periféricos";
            this.Shown += new System.EventHandler(this.FormPerifericoAnexo_Shown);
            this.gboxPerifericos.ResumeLayout(false);
            this.gboxPerifericos.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btAplicar;
        private System.Windows.Forms.GroupBox gboxPerifericos;
        private System.Windows.Forms.CheckBox chkTemp;
        private System.Windows.Forms.CheckBox chkApp;
        private System.Windows.Forms.CheckBox chkVelocidade;
        private System.Windows.Forms.CheckBox chkCatraca;
        private System.Windows.Forms.CheckedListBox cbListPaineis;
    }
}