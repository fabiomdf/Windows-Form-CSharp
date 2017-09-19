namespace LPontos2
{
    partial class FormFontEditorOpen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFontEditorOpen));
            this.gboxAbrirFontes = new System.Windows.Forms.GroupBox();
            this.cboxFontes = new System.Windows.Forms.ComboBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.gboxAbrirFontes.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxAbrirFontes
            // 
            this.gboxAbrirFontes.Controls.Add(this.btnAbrir);
            this.gboxAbrirFontes.Controls.Add(this.btnCancelar);
            this.gboxAbrirFontes.Controls.Add(this.cboxFontes);
            this.gboxAbrirFontes.Location = new System.Drawing.Point(5, 2);
            this.gboxAbrirFontes.Name = "gboxAbrirFontes";
            this.gboxAbrirFontes.Size = new System.Drawing.Size(285, 85);
            this.gboxAbrirFontes.TabIndex = 0;
            this.gboxAbrirFontes.TabStop = false;
            this.gboxAbrirFontes.Text = "Abrir Fonte";
            // 
            // cboxFontes
            // 
            this.cboxFontes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxFontes.FormattingEnabled = true;
            this.cboxFontes.Location = new System.Drawing.Point(7, 19);
            this.cboxFontes.Name = "cboxFontes";
            this.cboxFontes.Size = new System.Drawing.Size(272, 21);
            this.cboxFontes.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(123, 53);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnAbrir
            // 
            this.btnAbrir.Location = new System.Drawing.Point(204, 53);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(75, 23);
            this.btnAbrir.TabIndex = 2;
            this.btnAbrir.Text = "Abrir";
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // FormFontEditorOpen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 90);
            this.Controls.Add(this.gboxAbrirFontes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFontEditorOpen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editor de Fontes";
            this.Load += new System.EventHandler(this.FormFontEditorOpen_Load);
            this.gboxAbrirFontes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxAbrirFontes;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.ComboBox cboxFontes;
    }
}