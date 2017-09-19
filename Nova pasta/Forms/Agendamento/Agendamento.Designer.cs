namespace LPontos2.Forms.Agendamento
{
    partial class Agendamento
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Agendamento));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "20/10/2014 10:00",
            "Agendamento 1"}, -1);
            this.gboxGeral = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelEsquerda = new System.Windows.Forms.Panel();
            this.panelSplit = new System.Windows.Forms.Panel();
            this.lblListaAgendamentos = new System.Windows.Forms.Label();
            this.btRem = new System.Windows.Forms.Button();
            this.lvAgendamentos = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btAdd = new System.Windows.Forms.Button();
            this.panelDireita = new System.Windows.Forms.Panel();
            this.lblOperacao = new System.Windows.Forms.Label();
            this.lblDataHora = new System.Windows.Forms.Label();
            this.tboxNome = new System.Windows.Forms.TextBox();
            this.lblNomeAgendamento = new System.Windows.Forms.Label();
            this.dtData = new System.Windows.Forms.DateTimePicker();
            this.cboxOperacao = new System.Windows.Forms.ComboBox();
            this.dtHora = new System.Windows.Forms.DateTimePicker();
            this.gboxOperacao = new System.Windows.Forms.GroupBox();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.rbVolta = new System.Windows.Forms.RadioButton();
            this.rbIda = new System.Windows.Forms.RadioButton();
            this.dtpHoraSaida = new System.Windows.Forms.DateTimePicker();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btSalvar = new System.Windows.Forms.Button();
            this.gboxGeral.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.panelEsquerda.SuspendLayout();
            this.panelDireita.SuspendLayout();
            this.gboxOperacao.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxGeral
            // 
            this.gboxGeral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxGeral.Controls.Add(this.tableLayoutPanel);
            this.gboxGeral.Location = new System.Drawing.Point(3, 3);
            this.gboxGeral.Name = "gboxGeral";
            this.gboxGeral.Size = new System.Drawing.Size(994, 482);
            this.gboxGeral.TabIndex = 0;
            this.gboxGeral.TabStop = false;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.03925F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.96074F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.panelEsquerda, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panelDireita, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(6, 10);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(982, 466);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // panelEsquerda
            // 
            this.panelEsquerda.Controls.Add(this.panelSplit);
            this.panelEsquerda.Controls.Add(this.lblListaAgendamentos);
            this.panelEsquerda.Controls.Add(this.btRem);
            this.panelEsquerda.Controls.Add(this.lvAgendamentos);
            this.panelEsquerda.Controls.Add(this.btAdd);
            this.panelEsquerda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEsquerda.Location = new System.Drawing.Point(3, 3);
            this.panelEsquerda.Name = "panelEsquerda";
            this.panelEsquerda.Size = new System.Drawing.Size(387, 460);
            this.panelEsquerda.TabIndex = 3;
            // 
            // panelSplit
            // 
            this.panelSplit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSplit.Location = new System.Drawing.Point(383, 3);
            this.panelSplit.Name = "panelSplit";
            this.panelSplit.Size = new System.Drawing.Size(1, 461);
            this.panelSplit.TabIndex = 13;
            // 
            // lblListaAgendamentos
            // 
            this.lblListaAgendamentos.AutoSize = true;
            this.lblListaAgendamentos.Location = new System.Drawing.Point(3, 3);
            this.lblListaAgendamentos.Name = "lblListaAgendamentos";
            this.lblListaAgendamentos.Size = new System.Drawing.Size(118, 13);
            this.lblListaAgendamentos.TabIndex = 12;
            this.lblListaAgendamentos.Text = "Lista de Agendamentos";
            // 
            // btRem
            // 
            this.btRem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btRem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btRem.Image = ((System.Drawing.Image)(resources.GetObject("btRem.Image")));
            this.btRem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btRem.Location = new System.Drawing.Point(176, 433);
            this.btRem.Name = "btRem";
            this.btRem.Size = new System.Drawing.Size(95, 24);
            this.btRem.TabIndex = 1;
            this.btRem.Text = "Excluir";
            this.btRem.UseVisualStyleBackColor = true;
            this.btRem.Click += new System.EventHandler(this.btRem_Click);
            // 
            // lvAgendamentos
            // 
            this.lvAgendamentos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAgendamentos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvAgendamentos.FullRowSelect = true;
            this.lvAgendamentos.GridLines = true;
            this.lvAgendamentos.HideSelection = false;
            this.lvAgendamentos.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvAgendamentos.Location = new System.Drawing.Point(6, 20);
            this.lvAgendamentos.MultiSelect = false;
            this.lvAgendamentos.Name = "lvAgendamentos";
            this.lvAgendamentos.Size = new System.Drawing.Size(366, 407);
            this.lvAgendamentos.TabIndex = 0;
            this.lvAgendamentos.TabStop = false;
            this.lvAgendamentos.UseCompatibleStateImageBehavior = false;
            this.lvAgendamentos.View = System.Windows.Forms.View.Details;
            this.lvAgendamentos.Click += new System.EventHandler(this.lvAgendamentos_Click);
            this.lvAgendamentos.DoubleClick += new System.EventHandler(this.lvAgendamentos_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Data e Hora";
            this.columnHeader1.Width = 130;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Nome";
            this.columnHeader2.Width = 170;
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btAdd.BackgroundImage")));
            this.btAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btAdd.Location = new System.Drawing.Point(277, 433);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(95, 24);
            this.btAdd.TabIndex = 2;
            this.btAdd.Text = "Adicionar";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // panelDireita
            // 
            this.panelDireita.Controls.Add(this.lblOperacao);
            this.panelDireita.Controls.Add(this.lblDataHora);
            this.panelDireita.Controls.Add(this.tboxNome);
            this.panelDireita.Controls.Add(this.lblNomeAgendamento);
            this.panelDireita.Controls.Add(this.dtData);
            this.panelDireita.Controls.Add(this.cboxOperacao);
            this.panelDireita.Controls.Add(this.dtHora);
            this.panelDireita.Controls.Add(this.gboxOperacao);
            this.panelDireita.Controls.Add(this.btCancelar);
            this.panelDireita.Controls.Add(this.btSalvar);
            this.panelDireita.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDireita.Location = new System.Drawing.Point(396, 3);
            this.panelDireita.Name = "panelDireita";
            this.panelDireita.Size = new System.Drawing.Size(583, 460);
            this.panelDireita.TabIndex = 4;
            // 
            // lblOperacao
            // 
            this.lblOperacao.AutoSize = true;
            this.lblOperacao.Location = new System.Drawing.Point(3, 56);
            this.lblOperacao.Name = "lblOperacao";
            this.lblOperacao.Size = new System.Drawing.Size(54, 13);
            this.lblOperacao.TabIndex = 23;
            this.lblOperacao.Text = "Operação";
            // 
            // lblDataHora
            // 
            this.lblDataHora.AutoSize = true;
            this.lblDataHora.Location = new System.Drawing.Point(318, 3);
            this.lblDataHora.Name = "lblDataHora";
            this.lblDataHora.Size = new System.Drawing.Size(117, 13);
            this.lblDataHora.TabIndex = 22;
            this.lblDataHora.Text = "Data e Hora do Evento";
            // 
            // tboxNome
            // 
            this.tboxNome.Location = new System.Drawing.Point(6, 20);
            this.tboxNome.MaxLength = 50;
            this.tboxNome.Name = "tboxNome";
            this.tboxNome.Size = new System.Drawing.Size(275, 20);
            this.tboxNome.TabIndex = 0;
            // 
            // lblNomeAgendamento
            // 
            this.lblNomeAgendamento.AutoSize = true;
            this.lblNomeAgendamento.Location = new System.Drawing.Point(3, 3);
            this.lblNomeAgendamento.Name = "lblNomeAgendamento";
            this.lblNomeAgendamento.Size = new System.Drawing.Size(119, 13);
            this.lblNomeAgendamento.TabIndex = 17;
            this.lblNomeAgendamento.Text = "Nome do Agendamento";
            // 
            // dtData
            // 
            this.dtData.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtData.Location = new System.Drawing.Point(321, 20);
            this.dtData.Name = "dtData";
            this.dtData.Size = new System.Drawing.Size(98, 20);
            this.dtData.TabIndex = 1;
            // 
            // cboxOperacao
            // 
            this.cboxOperacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxOperacao.FormattingEnabled = true;
            this.cboxOperacao.Location = new System.Drawing.Point(6, 72);
            this.cboxOperacao.Name = "cboxOperacao";
            this.cboxOperacao.Size = new System.Drawing.Size(275, 21);
            this.cboxOperacao.TabIndex = 3;
            this.cboxOperacao.SelectedIndexChanged += new System.EventHandler(this.cboxOperacao_SelectedIndexChanged);
            // 
            // dtHora
            // 
            this.dtHora.CustomFormat = "HH:mm";
            this.dtHora.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtHora.Location = new System.Drawing.Point(425, 20);
            this.dtHora.Name = "dtHora";
            this.dtHora.ShowUpDown = true;
            this.dtHora.Size = new System.Drawing.Size(54, 20);
            this.dtHora.TabIndex = 2;
            this.dtHora.Value = new System.DateTime(2015, 10, 2, 8, 0, 0, 0);
            // 
            // gboxOperacao
            // 
            this.gboxOperacao.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxOperacao.Controls.Add(this.listView);
            this.gboxOperacao.Controls.Add(this.comboBox);
            this.gboxOperacao.Controls.Add(this.rbVolta);
            this.gboxOperacao.Controls.Add(this.rbIda);
            this.gboxOperacao.Controls.Add(this.dtpHoraSaida);
            this.gboxOperacao.Location = new System.Drawing.Point(3, 115);
            this.gboxOperacao.Name = "gboxOperacao";
            this.gboxOperacao.Size = new System.Drawing.Size(577, 312);
            this.gboxOperacao.TabIndex = 4;
            this.gboxOperacao.TabStop = false;
            this.gboxOperacao.Text = "Operação";
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(6, 57);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(565, 249);
            this.listView.TabIndex = 6;
            this.listView.TabStop = false;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView_ColumnWidthChanging);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Index";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Departure Text";
            this.columnHeader4.Width = 200;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Return Text";
            this.columnHeader5.Width = 200;
            // 
            // comboBox
            // 
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(178, 24);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(209, 21);
            this.comboBox.TabIndex = 3;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // rbVolta
            // 
            this.rbVolta.AutoSize = true;
            this.rbVolta.Location = new System.Drawing.Point(52, 25);
            this.rbVolta.Name = "rbVolta";
            this.rbVolta.Size = new System.Drawing.Size(49, 17);
            this.rbVolta.TabIndex = 1;
            this.rbVolta.TabStop = true;
            this.rbVolta.Text = "Volta";
            this.rbVolta.UseVisualStyleBackColor = true;
            this.rbVolta.CheckedChanged += new System.EventHandler(this.rbVolta_CheckedChanged);
            // 
            // rbIda
            // 
            this.rbIda.AutoSize = true;
            this.rbIda.Location = new System.Drawing.Point(6, 25);
            this.rbIda.Name = "rbIda";
            this.rbIda.Size = new System.Drawing.Size(40, 17);
            this.rbIda.TabIndex = 0;
            this.rbIda.TabStop = true;
            this.rbIda.Text = "Ida";
            this.rbIda.UseVisualStyleBackColor = true;
            this.rbIda.CheckedChanged += new System.EventHandler(this.rbIda_CheckedChanged);
            // 
            // dtpHoraSaida
            // 
            this.dtpHoraSaida.CustomFormat = "HH:mm";
            this.dtpHoraSaida.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoraSaida.Location = new System.Drawing.Point(107, 25);
            this.dtpHoraSaida.Name = "dtpHoraSaida";
            this.dtpHoraSaida.ShowUpDown = true;
            this.dtpHoraSaida.Size = new System.Drawing.Size(65, 20);
            this.dtpHoraSaida.TabIndex = 2;
            // 
            // btCancelar
            // 
            this.btCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelar.Location = new System.Drawing.Point(425, 433);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 24);
            this.btCancelar.TabIndex = 4;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btSalvar
            // 
            this.btSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSalvar.Location = new System.Drawing.Point(505, 433);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(75, 24);
            this.btSalvar.TabIndex = 5;
            this.btSalvar.Text = "Salvar";
            this.btSalvar.UseVisualStyleBackColor = true;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // Agendamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboxGeral);
            this.Name = "Agendamento";
            this.Size = new System.Drawing.Size(1000, 488);
            this.ClientSizeChanged += new System.EventHandler(this.Agendamento_ClientSizeChanged);
            this.VisibleChanged += new System.EventHandler(this.Agendamento_VisibleChanged);
            this.gboxGeral.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.panelEsquerda.ResumeLayout(false);
            this.panelEsquerda.PerformLayout();
            this.panelDireita.ResumeLayout(false);
            this.panelDireita.PerformLayout();
            this.gboxOperacao.ResumeLayout(false);
            this.gboxOperacao.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxGeral;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panelEsquerda;
        private System.Windows.Forms.Label lblListaAgendamentos;
        private System.Windows.Forms.Button btRem;
        private System.Windows.Forms.ListView lvAgendamentos;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Panel panelDireita;
        private System.Windows.Forms.Label lblOperacao;
        private System.Windows.Forms.Label lblDataHora;
        private System.Windows.Forms.TextBox tboxNome;
        private System.Windows.Forms.Label lblNomeAgendamento;
        private System.Windows.Forms.DateTimePicker dtData;
        private System.Windows.Forms.ComboBox cboxOperacao;
        private System.Windows.Forms.DateTimePicker dtHora;
        private System.Windows.Forms.GroupBox gboxOperacao;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.RadioButton rbVolta;
        private System.Windows.Forms.RadioButton rbIda;
        private System.Windows.Forms.DateTimePicker dtpHoraSaida;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.Panel panelSplit;

    }
}
