namespace LPontos2
{
    partial class FormLanguagesAndRegions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLanguagesAndRegions));
            this.tabControlRGNeIdiomas = new System.Windows.Forms.TabControl();
            this.tabPageRgn = new System.Windows.Forms.TabPage();
            this.comboBoxNomeArquivoRGN = new System.Windows.Forms.ComboBox();
            this.gBoxFormatoHora = new System.Windows.Forms.GroupBox();
            this.lblOpcaoAmPm_Ponto = new System.Windows.Forms.Label();
            this.rbPonto = new System.Windows.Forms.RadioButton();
            this.rbAmPm = new System.Windows.Forms.RadioButton();
            this.cbHora = new System.Windows.Forms.ComboBox();
            this.gBoxVelocidade = new System.Windows.Forms.GroupBox();
            this.cbVelocidade = new System.Windows.Forms.ComboBox();
            this.gBoxMoeda = new System.Windows.Forms.GroupBox();
            this.cbMoeda = new System.Windows.Forms.ComboBox();
            this.gBoxSeparadorDec = new System.Windows.Forms.GroupBox();
            this.cbDecimal = new System.Windows.Forms.ComboBox();
            this.gBoxTemperatura = new System.Windows.Forms.GroupBox();
            this.cbTemperatura = new System.Windows.Forms.ComboBox();
            this.tabPageIdiomas = new System.Windows.Forms.TabPage();
            this.labelNovoTexto = new System.Windows.Forms.Label();
            this.buttonEditar = new System.Windows.Forms.Button();
            this.textBoxEditar = new System.Windows.Forms.TextBox();
            this.listBoxEditaFrases = new System.Windows.Forms.ListBox();
            this.listBoxMenuFrases = new System.Windows.Forms.ListBox();
            this.cbArquivos = new System.Windows.Forms.ComboBox();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.btRestaurar = new System.Windows.Forms.Button();
            this.tabControlRGNeIdiomas.SuspendLayout();
            this.tabPageRgn.SuspendLayout();
            this.gBoxFormatoHora.SuspendLayout();
            this.gBoxVelocidade.SuspendLayout();
            this.gBoxMoeda.SuspendLayout();
            this.gBoxSeparadorDec.SuspendLayout();
            this.gBoxTemperatura.SuspendLayout();
            this.tabPageIdiomas.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlRGNeIdiomas
            // 
            this.tabControlRGNeIdiomas.Controls.Add(this.tabPageRgn);
            this.tabControlRGNeIdiomas.Controls.Add(this.tabPageIdiomas);
            this.tabControlRGNeIdiomas.Location = new System.Drawing.Point(12, 12);
            this.tabControlRGNeIdiomas.Name = "tabControlRGNeIdiomas";
            this.tabControlRGNeIdiomas.SelectedIndex = 0;
            this.tabControlRGNeIdiomas.Size = new System.Drawing.Size(508, 564);
            this.tabControlRGNeIdiomas.TabIndex = 0;
            this.tabControlRGNeIdiomas.SelectedIndexChanged += new System.EventHandler(this.tabControlRGNeIdiomas_SelectedIndexChanged);
            // 
            // tabPageRgn
            // 
            this.tabPageRgn.Controls.Add(this.comboBoxNomeArquivoRGN);
            this.tabPageRgn.Controls.Add(this.gBoxFormatoHora);
            this.tabPageRgn.Controls.Add(this.gBoxVelocidade);
            this.tabPageRgn.Controls.Add(this.gBoxMoeda);
            this.tabPageRgn.Controls.Add(this.gBoxSeparadorDec);
            this.tabPageRgn.Controls.Add(this.gBoxTemperatura);
            this.tabPageRgn.Location = new System.Drawing.Point(4, 22);
            this.tabPageRgn.Name = "tabPageRgn";
            this.tabPageRgn.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRgn.Size = new System.Drawing.Size(500, 538);
            this.tabPageRgn.TabIndex = 0;
            this.tabPageRgn.Text = "Configurações Regionais";
            this.tabPageRgn.UseVisualStyleBackColor = true;
            // 
            // comboBoxNomeArquivoRGN
            // 
            this.comboBoxNomeArquivoRGN.FormattingEnabled = true;
            this.comboBoxNomeArquivoRGN.Location = new System.Drawing.Point(13, 19);
            this.comboBoxNomeArquivoRGN.MaxLength = 16;
            this.comboBoxNomeArquivoRGN.Name = "comboBoxNomeArquivoRGN";
            this.comboBoxNomeArquivoRGN.Size = new System.Drawing.Size(471, 21);
            this.comboBoxNomeArquivoRGN.TabIndex = 1;
            this.comboBoxNomeArquivoRGN.Text = "Nome do Arquivo";
            // 
            // gBoxFormatoHora
            // 
            this.gBoxFormatoHora.Controls.Add(this.lblOpcaoAmPm_Ponto);
            this.gBoxFormatoHora.Controls.Add(this.rbPonto);
            this.gBoxFormatoHora.Controls.Add(this.rbAmPm);
            this.gBoxFormatoHora.Controls.Add(this.cbHora);
            this.gBoxFormatoHora.Location = new System.Drawing.Point(13, 53);
            this.gBoxFormatoHora.Name = "gBoxFormatoHora";
            this.gBoxFormatoHora.Size = new System.Drawing.Size(473, 78);
            this.gBoxFormatoHora.TabIndex = 1;
            this.gBoxFormatoHora.TabStop = false;
            this.gBoxFormatoHora.Text = "Formatos de Hora";
            // 
            // lblOpcaoAmPm_Ponto
            // 
            this.lblOpcaoAmPm_Ponto.Location = new System.Drawing.Point(281, 41);
            this.lblOpcaoAmPm_Ponto.Name = "lblOpcaoAmPm_Ponto";
            this.lblOpcaoAmPm_Ponto.Size = new System.Drawing.Size(85, 14);
            this.lblOpcaoAmPm_Ponto.TabIndex = 5;
            this.lblOpcaoAmPm_Ponto.Text = "Usar:";
            this.lblOpcaoAmPm_Ponto.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOpcaoAmPm_Ponto.Visible = false;
            // 
            // rbPonto
            // 
            this.rbPonto.AutoSize = true;
            this.rbPonto.Location = new System.Drawing.Point(440, 39);
            this.rbPonto.Name = "rbPonto";
            this.rbPonto.Size = new System.Drawing.Size(28, 17);
            this.rbPonto.TabIndex = 2;
            this.rbPonto.TabStop = true;
            this.rbPonto.Text = ".";
            this.rbPonto.UseVisualStyleBackColor = true;
            this.rbPonto.Visible = false;
            this.rbPonto.CheckedChanged += new System.EventHandler(this.rbPonto_CheckedChanged);
            // 
            // rbAmPm
            // 
            this.rbAmPm.AutoSize = true;
            this.rbAmPm.Location = new System.Drawing.Point(372, 39);
            this.rbAmPm.Name = "rbAmPm";
            this.rbAmPm.Size = new System.Drawing.Size(60, 17);
            this.rbAmPm.TabIndex = 1;
            this.rbAmPm.TabStop = true;
            this.rbAmPm.Text = "Am/Pm";
            this.rbAmPm.UseVisualStyleBackColor = true;
            this.rbAmPm.Visible = false;
            this.rbAmPm.CheckedChanged += new System.EventHandler(this.rbAmPm_CheckedChanged);
            // 
            // cbHora
            // 
            this.cbHora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHora.FormattingEnabled = true;
            this.cbHora.Items.AddRange(new object[] {
            "24h",
            "AM/PM"});
            this.cbHora.Location = new System.Drawing.Point(11, 35);
            this.cbHora.Name = "cbHora";
            this.cbHora.Size = new System.Drawing.Size(263, 21);
            this.cbHora.TabIndex = 0;
            this.cbHora.SelectedIndexChanged += new System.EventHandler(this.cbHora_SelectedIndexChanged);
            // 
            // gBoxVelocidade
            // 
            this.gBoxVelocidade.Controls.Add(this.cbVelocidade);
            this.gBoxVelocidade.Location = new System.Drawing.Point(13, 149);
            this.gBoxVelocidade.Name = "gBoxVelocidade";
            this.gBoxVelocidade.Size = new System.Drawing.Size(473, 78);
            this.gBoxVelocidade.TabIndex = 2;
            this.gBoxVelocidade.TabStop = false;
            this.gBoxVelocidade.Text = "Velocidade";
            // 
            // cbVelocidade
            // 
            this.cbVelocidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVelocidade.FormattingEnabled = true;
            this.cbVelocidade.Items.AddRange(new object[] {
            "KM/h",
            "MPh"});
            this.cbVelocidade.Location = new System.Drawing.Point(11, 35);
            this.cbVelocidade.Name = "cbVelocidade";
            this.cbVelocidade.Size = new System.Drawing.Size(456, 21);
            this.cbVelocidade.TabIndex = 0;
            this.cbVelocidade.SelectedIndexChanged += new System.EventHandler(this.cbVelocidade_SelectedIndexChanged);
            // 
            // gBoxMoeda
            // 
            this.gBoxMoeda.Controls.Add(this.cbMoeda);
            this.gBoxMoeda.Location = new System.Drawing.Point(13, 245);
            this.gBoxMoeda.Name = "gBoxMoeda";
            this.gBoxMoeda.Size = new System.Drawing.Size(473, 78);
            this.gBoxMoeda.TabIndex = 3;
            this.gBoxMoeda.TabStop = false;
            this.gBoxMoeda.Text = "Moeda";
            // 
            // cbMoeda
            // 
            this.cbMoeda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMoeda.FormattingEnabled = true;
            this.cbMoeda.Items.AddRange(new object[] {
            "Real",
            "Dólar",
            "Peso"});
            this.cbMoeda.Location = new System.Drawing.Point(11, 34);
            this.cbMoeda.Name = "cbMoeda";
            this.cbMoeda.Size = new System.Drawing.Size(456, 21);
            this.cbMoeda.TabIndex = 0;
            this.cbMoeda.SelectedIndexChanged += new System.EventHandler(this.cbMoeda_SelectedIndexChanged);
            // 
            // gBoxSeparadorDec
            // 
            this.gBoxSeparadorDec.Controls.Add(this.cbDecimal);
            this.gBoxSeparadorDec.Location = new System.Drawing.Point(13, 437);
            this.gBoxSeparadorDec.Name = "gBoxSeparadorDec";
            this.gBoxSeparadorDec.Size = new System.Drawing.Size(473, 78);
            this.gBoxSeparadorDec.TabIndex = 6;
            this.gBoxSeparadorDec.TabStop = false;
            this.gBoxSeparadorDec.Text = "Separador Decimal";
            // 
            // cbDecimal
            // 
            this.cbDecimal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDecimal.FormattingEnabled = true;
            this.cbDecimal.Items.AddRange(new object[] {
            "Vírgula (,)",
            "Ponto  (.)"});
            this.cbDecimal.Location = new System.Drawing.Point(11, 33);
            this.cbDecimal.Name = "cbDecimal";
            this.cbDecimal.Size = new System.Drawing.Size(456, 21);
            this.cbDecimal.TabIndex = 0;
            this.cbDecimal.SelectedIndexChanged += new System.EventHandler(this.cbDecimal_SelectedIndexChanged);
            // 
            // gBoxTemperatura
            // 
            this.gBoxTemperatura.Controls.Add(this.cbTemperatura);
            this.gBoxTemperatura.Location = new System.Drawing.Point(13, 341);
            this.gBoxTemperatura.Name = "gBoxTemperatura";
            this.gBoxTemperatura.Size = new System.Drawing.Size(473, 78);
            this.gBoxTemperatura.TabIndex = 5;
            this.gBoxTemperatura.TabStop = false;
            this.gBoxTemperatura.Text = "Temperatura";
            // 
            // cbTemperatura
            // 
            this.cbTemperatura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTemperatura.FormattingEnabled = true;
            this.cbTemperatura.Items.AddRange(new object[] {
            "Celsius",
            "Fahrenheit"});
            this.cbTemperatura.Location = new System.Drawing.Point(11, 37);
            this.cbTemperatura.Name = "cbTemperatura";
            this.cbTemperatura.Size = new System.Drawing.Size(456, 21);
            this.cbTemperatura.TabIndex = 0;
            this.cbTemperatura.SelectedIndexChanged += new System.EventHandler(this.cbTemperatura_SelectedIndexChanged);
            // 
            // tabPageIdiomas
            // 
            this.tabPageIdiomas.Controls.Add(this.labelNovoTexto);
            this.tabPageIdiomas.Controls.Add(this.buttonEditar);
            this.tabPageIdiomas.Controls.Add(this.textBoxEditar);
            this.tabPageIdiomas.Controls.Add(this.listBoxEditaFrases);
            this.tabPageIdiomas.Controls.Add(this.listBoxMenuFrases);
            this.tabPageIdiomas.Controls.Add(this.cbArquivos);
            this.tabPageIdiomas.Location = new System.Drawing.Point(4, 22);
            this.tabPageIdiomas.Name = "tabPageIdiomas";
            this.tabPageIdiomas.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIdiomas.Size = new System.Drawing.Size(500, 538);
            this.tabPageIdiomas.TabIndex = 1;
            this.tabPageIdiomas.Text = "Configurações de Idiomas";
            this.tabPageIdiomas.UseVisualStyleBackColor = true;
            this.tabPageIdiomas.Click += new System.EventHandler(this.tabPageIdiomas_Click);
            // 
            // labelNovoTexto
            // 
            this.labelNovoTexto.AutoSize = true;
            this.labelNovoTexto.Location = new System.Drawing.Point(6, 512);
            this.labelNovoTexto.Name = "labelNovoTexto";
            this.labelNovoTexto.Size = new System.Drawing.Size(66, 13);
            this.labelNovoTexto.TabIndex = 27;
            this.labelNovoTexto.Text = "Novo Texto:";
            // 
            // buttonEditar
            // 
            this.buttonEditar.Location = new System.Drawing.Point(415, 507);
            this.buttonEditar.Name = "buttonEditar";
            this.buttonEditar.Size = new System.Drawing.Size(75, 23);
            this.buttonEditar.TabIndex = 4;
            this.buttonEditar.Text = "Editar Texto";
            this.buttonEditar.UseVisualStyleBackColor = true;
            this.buttonEditar.Click += new System.EventHandler(this.buttonEditar_Click);
            // 
            // textBoxEditar
            // 
            this.textBoxEditar.Location = new System.Drawing.Point(78, 509);
            this.textBoxEditar.MaxLength = 16;
            this.textBoxEditar.Name = "textBoxEditar";
            this.textBoxEditar.Size = new System.Drawing.Size(331, 20);
            this.textBoxEditar.TabIndex = 3;
            // 
            // listBoxEditaFrases
            // 
            this.listBoxEditaFrases.Enabled = false;
            this.listBoxEditaFrases.FormattingEnabled = true;
            this.listBoxEditaFrases.Location = new System.Drawing.Point(248, 42);
            this.listBoxEditaFrases.Name = "listBoxEditaFrases";
            this.listBoxEditaFrases.Size = new System.Drawing.Size(242, 459);
            this.listBoxEditaFrases.TabIndex = 2;
            this.listBoxEditaFrases.Click += new System.EventHandler(this.listBoxEditaFrases_SelectedIndexChanged);
            this.listBoxEditaFrases.SelectedIndexChanged += new System.EventHandler(this.listBoxEditaFrases_SelectedIndexChanged);
            // 
            // listBoxMenuFrases
            // 
            this.listBoxMenuFrases.FormattingEnabled = true;
            this.listBoxMenuFrases.Location = new System.Drawing.Point(6, 42);
            this.listBoxMenuFrases.Name = "listBoxMenuFrases";
            this.listBoxMenuFrases.Size = new System.Drawing.Size(236, 459);
            this.listBoxMenuFrases.TabIndex = 1;
            this.listBoxMenuFrases.Click += new System.EventHandler(this.listBoxMenuFrases_SelectedIndexChanged);
            this.listBoxMenuFrases.SelectedIndexChanged += new System.EventHandler(this.listBoxMenuFrases_SelectedIndexChanged);
            // 
            // cbArquivos
            // 
            this.cbArquivos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbArquivos.FormattingEnabled = true;
            this.cbArquivos.Location = new System.Drawing.Point(6, 15);
            this.cbArquivos.Name = "cbArquivos";
            this.cbArquivos.Size = new System.Drawing.Size(484, 21);
            this.cbArquivos.TabIndex = 0;
            this.cbArquivos.SelectedIndexChanged += new System.EventHandler(this.cbArquivos_SelectedIndexChanged);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Location = new System.Drawing.Point(288, 589);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(113, 23);
            this.buttonCancelar.TabIndex = 8;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(407, 589);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(113, 23);
            this.buttonOk.TabIndex = 9;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // btRestaurar
            // 
            this.btRestaurar.AutoSize = true;
            this.btRestaurar.Location = new System.Drawing.Point(12, 589);
            this.btRestaurar.Name = "btRestaurar";
            this.btRestaurar.Size = new System.Drawing.Size(113, 23);
            this.btRestaurar.TabIndex = 7;
            this.btRestaurar.Text = "Restaurar Padrão";
            this.btRestaurar.UseVisualStyleBackColor = true;
            this.btRestaurar.Visible = false;
            this.btRestaurar.Click += new System.EventHandler(this.btRestaurar_Click);
            // 
            // FormLanguagesAndRegions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 624);
            this.Controls.Add(this.btRestaurar);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.tabControlRGNeIdiomas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLanguagesAndRegions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Línguas e Regiões";
            this.Load += new System.EventHandler(this.FormLanguagesAndRegions_Load);
            this.tabControlRGNeIdiomas.ResumeLayout(false);
            this.tabPageRgn.ResumeLayout(false);
            this.gBoxFormatoHora.ResumeLayout(false);
            this.gBoxFormatoHora.PerformLayout();
            this.gBoxVelocidade.ResumeLayout(false);
            this.gBoxMoeda.ResumeLayout(false);
            this.gBoxSeparadorDec.ResumeLayout(false);
            this.gBoxTemperatura.ResumeLayout(false);
            this.tabPageIdiomas.ResumeLayout(false);
            this.tabPageIdiomas.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlRGNeIdiomas;
        private System.Windows.Forms.TabPage tabPageRgn;
        private System.Windows.Forms.TabPage tabPageIdiomas;
        private System.Windows.Forms.GroupBox gBoxFormatoHora;
        private System.Windows.Forms.GroupBox gBoxVelocidade;
        private System.Windows.Forms.GroupBox gBoxMoeda;
        private System.Windows.Forms.GroupBox gBoxSeparadorDec;
        private System.Windows.Forms.GroupBox gBoxTemperatura;
        private System.Windows.Forms.Button buttonCancelar;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ComboBox cbHora;
        private System.Windows.Forms.ComboBox cbVelocidade;
        private System.Windows.Forms.ComboBox cbMoeda;
        private System.Windows.Forms.ComboBox cbDecimal;
        private System.Windows.Forms.ComboBox cbTemperatura;
        private System.Windows.Forms.ComboBox cbArquivos;
        private System.Windows.Forms.ListBox listBoxMenuFrases;
        private System.Windows.Forms.ListBox listBoxEditaFrases;
        private System.Windows.Forms.Label labelNovoTexto;
        private System.Windows.Forms.Button buttonEditar;
        private System.Windows.Forms.TextBox textBoxEditar;
        private System.Windows.Forms.ComboBox comboBoxNomeArquivoRGN;
        private System.Windows.Forms.RadioButton rbPonto;
        private System.Windows.Forms.RadioButton rbAmPm;
        private System.Windows.Forms.Label lblOpcaoAmPm_Ponto;
        private System.Windows.Forms.Button btRestaurar;
    }
}