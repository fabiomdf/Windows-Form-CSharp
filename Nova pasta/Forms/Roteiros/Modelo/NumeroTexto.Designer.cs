namespace LPontos2.Forms.Roteiros.Modelo
{
    partial class NumeroTexto
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
            this.rtbNumero = new System.Windows.Forms.RichTextBox();
            this.rtbText01 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbNumero
            // 
            this.rtbNumero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtbNumero.Font = new System.Drawing.Font("Arial", 36.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbNumero.Location = new System.Drawing.Point(3, 70);
            this.rtbNumero.Multiline = false;
            this.rtbNumero.Name = "rtbNumero";
            this.rtbNumero.Size = new System.Drawing.Size(150, 64);
            this.rtbNumero.TabIndex = 1;
            this.rtbNumero.Text = "N";
            this.rtbNumero.TextChanged += new System.EventHandler(this.rtbNumero_TextChanged);
            this.rtbNumero.Enter += new System.EventHandler(this.rtbNumero_Enter);
            // 
            // rtbText01
            // 
            this.rtbText01.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtbText01.Font = new System.Drawing.Font("Arial", 36.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbText01.Location = new System.Drawing.Point(159, 70);
            this.rtbText01.Multiline = false;
            this.rtbText01.Name = "rtbText01";
            this.rtbText01.Size = new System.Drawing.Size(338, 64);
            this.rtbText01.TabIndex = 2;
            this.rtbText01.Text = "Texto 01";
            this.rtbText01.TextChanged += new System.EventHandler(this.rtbText01_TextChanged);
            this.rtbText01.Enter += new System.EventHandler(this.rtbText01_Enter);
            // 
            // NumeroTexto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbNumero);
            this.Controls.Add(this.rtbText01);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NumeroTexto";
            this.Size = new System.Drawing.Size(500, 215);
            this.Load += new System.EventHandler(this.NumeroTexto_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbNumero;
        private System.Windows.Forms.RichTextBox rtbText01;
    }
}
