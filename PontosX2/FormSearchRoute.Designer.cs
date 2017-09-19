namespace LPontos2
{
    partial class FormSearchRoute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSearchRoute));
            this.btFind = new System.Windows.Forms.Button();
            this.lbFindWhat = new System.Windows.Forms.Label();
            this.tbFindWhat = new System.Windows.Forms.TextBox();
            this.gbFindOptions = new System.Windows.Forms.GroupBox();
            this.cbMatchWord = new System.Windows.Forms.CheckBox();
            this.cbIgnoreCase = new System.Windows.Forms.CheckBox();
            this.btCancelar = new System.Windows.Forms.Button();
            this.cbTexts = new System.Windows.Forms.CheckBox();
            this.gbFindOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btFind
            // 
            this.btFind.Location = new System.Drawing.Point(160, 145);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(75, 23);
            this.btFind.TabIndex = 0;
            this.btFind.Text = "Find";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.button1_Click);
            this.btFind.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btFind_KeyUp);
            // 
            // lbFindWhat
            // 
            this.lbFindWhat.AutoSize = true;
            this.lbFindWhat.Location = new System.Drawing.Point(9, 9);
            this.lbFindWhat.Name = "lbFindWhat";
            this.lbFindWhat.Size = new System.Drawing.Size(56, 13);
            this.lbFindWhat.TabIndex = 1;
            this.lbFindWhat.Text = "Find what:";
            // 
            // tbFindWhat
            // 
            this.tbFindWhat.Location = new System.Drawing.Point(12, 25);
            this.tbFindWhat.Name = "tbFindWhat";
            this.tbFindWhat.Size = new System.Drawing.Size(223, 20);
            this.tbFindWhat.TabIndex = 2;
            // 
            // gbFindOptions
            // 
            this.gbFindOptions.Controls.Add(this.cbTexts);
            this.gbFindOptions.Controls.Add(this.cbMatchWord);
            this.gbFindOptions.Controls.Add(this.cbIgnoreCase);
            this.gbFindOptions.Location = new System.Drawing.Point(12, 51);
            this.gbFindOptions.Name = "gbFindOptions";
            this.gbFindOptions.Size = new System.Drawing.Size(223, 88);
            this.gbFindOptions.TabIndex = 3;
            this.gbFindOptions.TabStop = false;
            this.gbFindOptions.Text = " Find Options";
            // 
            // cbMatchWord
            // 
            this.cbMatchWord.AutoSize = true;
            this.cbMatchWord.Location = new System.Drawing.Point(7, 43);
            this.cbMatchWord.Name = "cbMatchWord";
            this.cbMatchWord.Size = new System.Drawing.Size(113, 17);
            this.cbMatchWord.TabIndex = 1;
            this.cbMatchWord.Text = "Match whole word";
            this.cbMatchWord.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreCase
            // 
            this.cbIgnoreCase.AutoSize = true;
            this.cbIgnoreCase.Location = new System.Drawing.Point(7, 20);
            this.cbIgnoreCase.Name = "cbIgnoreCase";
            this.cbIgnoreCase.Size = new System.Drawing.Size(83, 17);
            this.cbIgnoreCase.TabIndex = 0;
            this.cbIgnoreCase.Text = "Ignore Case";
            this.cbIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(80, 145);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(76, 23);
            this.btCancelar.TabIndex = 4;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbTexts
            // 
            this.cbTexts.AutoSize = true;
            this.cbTexts.Location = new System.Drawing.Point(7, 66);
            this.cbTexts.Name = "cbTexts";
            this.cbTexts.Size = new System.Drawing.Size(86, 17);
            this.cbTexts.TabIndex = 2;
            this.cbTexts.Text = "Find on texts";
            this.cbTexts.UseVisualStyleBackColor = true;
            // 
            // FormSearchRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 178);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.gbFindOptions);
            this.Controls.Add(this.tbFindWhat);
            this.Controls.Add(this.lbFindWhat);
            this.Controls.Add(this.btFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSearchRoute";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Route";
            this.Shown += new System.EventHandler(this.FormSearchRoute_Shown);
            this.gbFindOptions.ResumeLayout(false);
            this.gbFindOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Label lbFindWhat;
        private System.Windows.Forms.TextBox tbFindWhat;
        private System.Windows.Forms.GroupBox gbFindOptions;
        private System.Windows.Forms.CheckBox cbMatchWord;
        private System.Windows.Forms.CheckBox cbIgnoreCase;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.CheckBox cbTexts;
    }
}