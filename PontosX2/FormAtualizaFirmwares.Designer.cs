namespace LPontos2
{
    partial class FormAtualizaFirmwares
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewAtualizarFirmware = new System.Windows.Forms.DataGridView();
            this.ColumnProduto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVersaoLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDiretorio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVersaoArquivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColElipsys = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Download = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Percentual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btAplicar = new System.Windows.Forms.Button();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btExportar = new System.Windows.Forms.Button();
            this.buttonDownloadAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAtualizarFirmware)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewAtualizarFirmware
            // 
            this.dataGridViewAtualizarFirmware.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAtualizarFirmware.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnProduto,
            this.ColumnVersaoLocal,
            this.Tipo,
            this.ColumnDiretorio,
            this.ColumnVersaoArquivo,
            this.ColElipsys,
            this.Download,
            this.Percentual});
            this.dataGridViewAtualizarFirmware.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewAtualizarFirmware.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewAtualizarFirmware.MultiSelect = false;
            this.dataGridViewAtualizarFirmware.Name = "dataGridViewAtualizarFirmware";
            this.dataGridViewAtualizarFirmware.RowHeadersVisible = false;
            this.dataGridViewAtualizarFirmware.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewAtualizarFirmware.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewAtualizarFirmware.ShowCellErrors = false;
            this.dataGridViewAtualizarFirmware.ShowEditingIcon = false;
            this.dataGridViewAtualizarFirmware.Size = new System.Drawing.Size(1009, 177);
            this.dataGridViewAtualizarFirmware.TabIndex = 0;
            this.dataGridViewAtualizarFirmware.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAtualizarFirmware_CellContentClick);
            // 
            // ColumnProduto
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColumnProduto.DefaultCellStyle = dataGridViewCellStyle11;
            this.ColumnProduto.HeaderText = "Produto";
            this.ColumnProduto.Name = "ColumnProduto";
            this.ColumnProduto.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnProduto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnProduto.Width = 150;
            // 
            // ColumnVersaoLocal
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnVersaoLocal.DefaultCellStyle = dataGridViewCellStyle12;
            this.ColumnVersaoLocal.HeaderText = "Versão Local";
            this.ColumnVersaoLocal.Name = "ColumnVersaoLocal";
            this.ColumnVersaoLocal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnVersaoLocal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnVersaoLocal.Width = 80;
            // 
            // Tipo
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Tipo.DefaultCellStyle = dataGridViewCellStyle13;
            this.Tipo.HeaderText = "Software/Firmware";
            this.Tipo.Name = "Tipo";
            this.Tipo.ReadOnly = true;
            // 
            // ColumnDiretorio
            // 
            this.ColumnDiretorio.HeaderText = "Diretório do arquivo";
            this.ColumnDiretorio.Name = "ColumnDiretorio";
            this.ColumnDiretorio.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnDiretorio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDiretorio.Width = 342;
            // 
            // ColumnVersaoArquivo
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnVersaoArquivo.DefaultCellStyle = dataGridViewCellStyle14;
            this.ColumnVersaoArquivo.HeaderText = "Versão arquivo";
            this.ColumnVersaoArquivo.Name = "ColumnVersaoArquivo";
            this.ColumnVersaoArquivo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnVersaoArquivo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnVersaoArquivo.Width = 110;
            // 
            // ColElipsys
            // 
            this.ColElipsys.HeaderText = "";
            this.ColElipsys.Name = "ColElipsys";
            this.ColElipsys.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColElipsys.Text = "...";
            this.ColElipsys.Width = 26;
            // 
            // Download
            // 
            this.Download.HeaderText = "";
            this.Download.Name = "Download";
            this.Download.Text = "Check Updates";
            // 
            // Percentual
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Percentual.DefaultCellStyle = dataGridViewCellStyle15;
            this.Percentual.HeaderText = "Percentual";
            this.Percentual.Name = "Percentual";
            this.Percentual.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Percentual.Width = 80;
            // 
            // btAplicar
            // 
            this.btAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAplicar.Location = new System.Drawing.Point(847, 182);
            this.btAplicar.Name = "btAplicar";
            this.btAplicar.Size = new System.Drawing.Size(75, 23);
            this.btAplicar.TabIndex = 1;
            this.btAplicar.Text = "Aplicar";
            this.btAplicar.UseVisualStyleBackColor = true;
            this.btAplicar.Click += new System.EventHandler(this.btAplicar_Click);
            // 
            // btCancelar
            // 
            this.btCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelar.Location = new System.Drawing.Point(928, 182);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 2;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btExportar
            // 
            this.btExportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExportar.Location = new System.Drawing.Point(766, 182);
            this.btExportar.Name = "btExportar";
            this.btExportar.Size = new System.Drawing.Size(75, 23);
            this.btExportar.TabIndex = 3;
            this.btExportar.Text = "Exportar";
            this.btExportar.UseVisualStyleBackColor = true;
            this.btExportar.Click += new System.EventHandler(this.btExportar_Click);
            // 
            // buttonDownloadAll
            // 
            this.buttonDownloadAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadAll.Location = new System.Drawing.Point(650, 182);
            this.buttonDownloadAll.Name = "buttonDownloadAll";
            this.buttonDownloadAll.Size = new System.Drawing.Size(110, 23);
            this.buttonDownloadAll.TabIndex = 4;
            this.buttonDownloadAll.Text = "Download All";
            this.buttonDownloadAll.UseVisualStyleBackColor = true;
            this.buttonDownloadAll.Click += new System.EventHandler(this.buttonDownloadAll_Click);
            // 
            // FormAtualizaFirmwares
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 210);
            this.ControlBox = false;
            this.Controls.Add(this.buttonDownloadAll);
            this.Controls.Add(this.btExportar);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.btAplicar);
            this.Controls.Add(this.dataGridViewAtualizarFirmware);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormAtualizaFirmwares";
            this.Text = "Atualizar Firmwares";
            this.Load += new System.EventHandler(this.FormAtualizaFirmwares_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAtualizarFirmware)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewAtualizarFirmware;
        private System.Windows.Forms.Button btAplicar;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btExportar;
        private System.Windows.Forms.Button buttonDownloadAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProduto;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVersaoLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDiretorio;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVersaoArquivo;
        private System.Windows.Forms.DataGridViewButtonColumn ColElipsys;
        private System.Windows.Forms.DataGridViewButtonColumn Download;
        private System.Windows.Forms.DataGridViewTextBoxColumn Percentual;
    }
}