namespace LPontos2.Forms.Roteiros.Modelo
{
    partial class TextoTriplo
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
            this.rtbTexto02 = new System.Windows.Forms.RichTextBox();
            this.rtbTexto01 = new System.Windows.Forms.RichTextBox();
            this.rtbTexto03 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbTexto02
            // 
            this.rtbTexto02.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtbTexto02.Font = new System.Drawing.Font("Arial", 36.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbTexto02.Location = new System.Drawing.Point(3, 69);
            this.rtbTexto02.Multiline = false;
            this.rtbTexto02.Name = "rtbTexto02";
            this.rtbTexto02.Size = new System.Drawing.Size(494, 60);
            this.rtbTexto02.TabIndex = 1;
            this.rtbTexto02.Text = "Texto 02";
            this.rtbTexto02.TextChanged += new System.EventHandler(this.rtbTexto02_TextChanged);
            this.rtbTexto02.Enter += new System.EventHandler(this.rtbTexto02_Enter);
            // 
            // rtbTexto01
            // 
            this.rtbTexto01.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtbTexto01.Font = new System.Drawing.Font("Arial", 36.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbTexto01.Location = new System.Drawing.Point(3, 3);
            this.rtbTexto01.Multiline = false;
            this.rtbTexto01.Name = "rtbTexto01";
            this.rtbTexto01.Size = new System.Drawing.Size(494, 60);
            this.rtbTexto01.TabIndex = 0;
            this.rtbTexto01.Text = "Texto 01";
            this.rtbTexto01.TextChanged += new System.EventHandler(this.rtbTexto01_TextChanged);
            this.rtbTexto01.Enter += new System.EventHandler(this.rtbTexto01_Enter);
            // 
            // rtbTexto03
            // 
            this.rtbTexto03.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtbTexto03.Font = new System.Drawing.Font("Arial", 36.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbTexto03.Location = new System.Drawing.Point(3, 135);
            this.rtbTexto03.Multiline = false;
            this.rtbTexto03.Name = "rtbTexto03";
            this.rtbTexto03.Size = new System.Drawing.Size(494, 60);
            this.rtbTexto03.TabIndex = 2;
            this.rtbTexto03.Text = "Texto 03";
            this.rtbTexto03.TextChanged += new System.EventHandler(this.rtbTexto03_TextChanged);
            this.rtbTexto03.Enter += new System.EventHandler(this.rtbTexto03_Enter);
            // 
            // TextoTriplo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtbTexto03);
            this.Controls.Add(this.rtbTexto02);
            this.Controls.Add(this.rtbTexto01);
            this.Name = "TextoTriplo";
            this.Size = new System.Drawing.Size(500, 200);
            this.Load += new System.EventHandler(this.TextoTriplo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbTexto02;
        private System.Windows.Forms.RichTextBox rtbTexto01;
        private System.Windows.Forms.RichTextBox rtbTexto03;
    }
}
