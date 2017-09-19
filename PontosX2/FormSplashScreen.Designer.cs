namespace LPontos2
{
    partial class FormSplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSplashScreen));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labelVersao = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.picMinimizar = new System.Windows.Forms.PictureBox();
            this.picFechar = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFechar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(110, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(220, 34);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(79, 67);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(278, 116);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // labelVersao
            // 
            this.labelVersao.AutoSize = true;
            this.labelVersao.BackColor = System.Drawing.Color.Transparent;
            this.labelVersao.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersao.ForeColor = System.Drawing.Color.White;
            this.labelVersao.Location = new System.Drawing.Point(356, 204);
            this.labelVersao.Name = "labelVersao";
            this.labelVersao.Size = new System.Drawing.Size(72, 16);
            this.labelVersao.TabIndex = 3;
            this.labelVersao.Text = "v 12.1.0";
            this.labelVersao.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelStatus
            // 
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(79, 220);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelStatus.Size = new System.Drawing.Size(349, 21);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "iniciando";
            // 
            // picMinimizar
            // 
            this.picMinimizar.BackColor = System.Drawing.Color.Transparent;
            this.picMinimizar.Image = ((System.Drawing.Image)(resources.GetObject("picMinimizar.Image")));
            this.picMinimizar.Location = new System.Drawing.Point(384, 14);
            this.picMinimizar.Name = "picMinimizar";
            this.picMinimizar.Size = new System.Drawing.Size(16, 16);
            this.picMinimizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picMinimizar.TabIndex = 5;
            this.picMinimizar.TabStop = false;
            this.picMinimizar.Click += new System.EventHandler(this.picMinimizar_Click);
            // 
            // picFechar
            // 
            this.picFechar.BackColor = System.Drawing.Color.Transparent;
            this.picFechar.Image = ((System.Drawing.Image)(resources.GetObject("picFechar.Image")));
            this.picFechar.Location = new System.Drawing.Point(409, 10);
            this.picFechar.Name = "picFechar";
            this.picFechar.Size = new System.Drawing.Size(16, 16);
            this.picFechar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picFechar.TabIndex = 6;
            this.picFechar.TabStop = false;
            this.picFechar.Click += new System.EventHandler(this.picFechar_Click);
            // 
            // FormSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(440, 250);
            this.Controls.Add(this.picFechar);
            this.Controls.Add(this.picMinimizar);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelVersao);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormSplashScreen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSplashScreen_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFechar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelVersao;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.PictureBox picMinimizar;
        private System.Windows.Forms.PictureBox picFechar;
    }
}