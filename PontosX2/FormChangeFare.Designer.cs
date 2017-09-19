namespace LPontos2
{
    partial class FormChangeFare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChangeFare));
            this.gboxFare = new System.Windows.Forms.GroupBox();
            this.labelPara = new System.Windows.Forms.Label();
            this.labelDe = new System.Windows.Forms.Label();
            this.tboxPara = new System.Windows.Forms.TextBox();
            this.tboxDe = new System.Windows.Forms.TextBox();
            this.rbTarifaEspecifica = new System.Windows.Forms.RadioButton();
            this.rbTodasTarifas = new System.Windows.Forms.RadioButton();
            this.tboxFare = new System.Windows.Forms.TextBox();
            this.btApply = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.gboxFare.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxFare
            // 
            this.gboxFare.Controls.Add(this.labelPara);
            this.gboxFare.Controls.Add(this.labelDe);
            this.gboxFare.Controls.Add(this.tboxPara);
            this.gboxFare.Controls.Add(this.tboxDe);
            this.gboxFare.Controls.Add(this.rbTarifaEspecifica);
            this.gboxFare.Controls.Add(this.rbTodasTarifas);
            this.gboxFare.Controls.Add(this.tboxFare);
            this.gboxFare.Controls.Add(this.btApply);
            this.gboxFare.Controls.Add(this.btCancel);
            this.gboxFare.Location = new System.Drawing.Point(12, 3);
            this.gboxFare.Name = "gboxFare";
            this.gboxFare.Size = new System.Drawing.Size(370, 134);
            this.gboxFare.TabIndex = 0;
            this.gboxFare.TabStop = false;
            // 
            // labelPara
            // 
            this.labelPara.AutoSize = true;
            this.labelPara.Location = new System.Drawing.Point(160, 73);
            this.labelPara.Name = "labelPara";
            this.labelPara.Size = new System.Drawing.Size(32, 13);
            this.labelPara.TabIndex = 8;
            this.labelPara.Text = "Para:";
            // 
            // labelDe
            // 
            this.labelDe.AutoSize = true;
            this.labelDe.Location = new System.Drawing.Point(13, 73);
            this.labelDe.Name = "labelDe";
            this.labelDe.Size = new System.Drawing.Size(24, 13);
            this.labelDe.TabIndex = 7;
            this.labelDe.Text = "De:";
            // 
            // tboxPara
            // 
            this.tboxPara.Location = new System.Drawing.Point(198, 70);
            this.tboxPara.Name = "tboxPara";
            this.tboxPara.Size = new System.Drawing.Size(100, 20);
            this.tboxPara.TabIndex = 6;
            this.tboxPara.Text = "0.00";
            this.tboxPara.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxPara.Click += new System.EventHandler(this.tboxPara_Click);
            this.tboxPara.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxPara_KeyDown);
            this.tboxPara.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxPara_KeyPress);
            // 
            // tboxDe
            // 
            this.tboxDe.Location = new System.Drawing.Point(43, 70);
            this.tboxDe.Name = "tboxDe";
            this.tboxDe.Size = new System.Drawing.Size(100, 20);
            this.tboxDe.TabIndex = 5;
            this.tboxDe.Text = "0.00";
            this.tboxDe.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxDe.Click += new System.EventHandler(this.tboxDe_Click);
            this.tboxDe.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxDe_KeyDown);
            this.tboxDe.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxDe_KeyPress);
            // 
            // rbTarifaEspecifica
            // 
            this.rbTarifaEspecifica.AutoSize = true;
            this.rbTarifaEspecifica.Location = new System.Drawing.Point(15, 42);
            this.rbTarifaEspecifica.Name = "rbTarifaEspecifica";
            this.rbTarifaEspecifica.Size = new System.Drawing.Size(85, 17);
            this.rbTarifaEspecifica.TabIndex = 4;
            this.rbTarifaEspecifica.TabStop = true;
            this.rbTarifaEspecifica.Text = "radioButton2";
            this.rbTarifaEspecifica.UseVisualStyleBackColor = true;
            // 
            // rbTodasTarifas
            // 
            this.rbTodasTarifas.AutoSize = true;
            this.rbTodasTarifas.Location = new System.Drawing.Point(15, 19);
            this.rbTodasTarifas.Name = "rbTodasTarifas";
            this.rbTodasTarifas.Size = new System.Drawing.Size(85, 17);
            this.rbTodasTarifas.TabIndex = 3;
            this.rbTodasTarifas.TabStop = true;
            this.rbTodasTarifas.Text = "radioButton1";
            this.rbTodasTarifas.UseVisualStyleBackColor = true;
            this.rbTodasTarifas.CheckedChanged += new System.EventHandler(this.rbTodasTarifas_CheckedChanged);
            // 
            // tboxFare
            // 
            this.tboxFare.Location = new System.Drawing.Point(15, 70);
            this.tboxFare.MaxLength = 9;
            this.tboxFare.Name = "tboxFare";
            this.tboxFare.Size = new System.Drawing.Size(169, 20);
            this.tboxFare.TabIndex = 0;
            this.tboxFare.Text = "0.00";
            this.tboxFare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxFare.Click += new System.EventHandler(this.tboxFare_Click);
            this.tboxFare.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxFare_KeyDown);
            this.tboxFare.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxFare_KeyPress);
            // 
            // btApply
            // 
            this.btApply.Location = new System.Drawing.Point(289, 101);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(75, 23);
            this.btApply.TabIndex = 2;
            this.btApply.Text = "Aplicar";
            this.btApply.UseVisualStyleBackColor = true;
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(208, 101);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancelar";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // FormChangeFare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 145);
            this.Controls.Add(this.gboxFare);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChangeFare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tarifa";
            this.Shown += new System.EventHandler(this.FormChangeFare_Shown);
            this.gboxFare.ResumeLayout(false);
            this.gboxFare.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxFare;
        private System.Windows.Forms.TextBox tboxFare;
        private System.Windows.Forms.Button btApply;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label labelPara;
        private System.Windows.Forms.Label labelDe;
        private System.Windows.Forms.TextBox tboxPara;
        private System.Windows.Forms.TextBox tboxDe;
        private System.Windows.Forms.RadioButton rbTarifaEspecifica;
        private System.Windows.Forms.RadioButton rbTodasTarifas;
    }
}