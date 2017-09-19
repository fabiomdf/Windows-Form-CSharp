namespace LPontos2.Forms.Motorista
{
    partial class Motorista
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Motorista));
            this.lvMotoristas = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Nome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gboxMotorista = new System.Windows.Forms.GroupBox();
            this.btAplicar = new System.Windows.Forms.Button();
            this.btCancelar = new System.Windows.Forms.Button();
            this.tboxNome = new System.Windows.Forms.TextBox();
            this.tboxID = new System.Windows.Forms.TextBox();
            this.btEditarNome = new System.Windows.Forms.Button();
            this.btEditarID = new System.Windows.Forms.Button();
            this.btRem = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.gboxMotorista.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvMotoristas
            // 
            this.lvMotoristas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMotoristas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.Nome});
            this.lvMotoristas.FullRowSelect = true;
            this.lvMotoristas.GridLines = true;
            this.lvMotoristas.HideSelection = false;
            this.lvMotoristas.Location = new System.Drawing.Point(3, 121);
            this.lvMotoristas.MultiSelect = false;
            this.lvMotoristas.Name = "lvMotoristas";
            this.lvMotoristas.Size = new System.Drawing.Size(994, 364);
            this.lvMotoristas.TabIndex = 0;
            this.lvMotoristas.UseCompatibleStateImageBehavior = false;
            this.lvMotoristas.View = System.Windows.Forms.View.Details;
            this.lvMotoristas.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMotoristas_ColumnClick);
            this.lvMotoristas.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvMotoristas_ItemSelectionChanged);
            this.lvMotoristas.DoubleClick += new System.EventHandler(this.lvMotoristas_DoubleClick);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 180;
            // 
            // Nome
            // 
            this.Nome.Text = "Nome";
            this.Nome.Width = 450;
            // 
            // gboxMotorista
            // 
            this.gboxMotorista.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxMotorista.Controls.Add(this.btAplicar);
            this.gboxMotorista.Controls.Add(this.btCancelar);
            this.gboxMotorista.Controls.Add(this.tboxNome);
            this.gboxMotorista.Controls.Add(this.tboxID);
            this.gboxMotorista.Controls.Add(this.btEditarNome);
            this.gboxMotorista.Controls.Add(this.btEditarID);
            this.gboxMotorista.Location = new System.Drawing.Point(3, 3);
            this.gboxMotorista.Name = "gboxMotorista";
            this.gboxMotorista.Size = new System.Drawing.Size(994, 112);
            this.gboxMotorista.TabIndex = 3;
            this.gboxMotorista.TabStop = false;
            this.gboxMotorista.Text = "Informações do Motorista";
            // 
            // btAplicar
            // 
            this.btAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAplicar.Location = new System.Drawing.Point(913, 82);
            this.btAplicar.Name = "btAplicar";
            this.btAplicar.Size = new System.Drawing.Size(75, 23);
            this.btAplicar.TabIndex = 5;
            this.btAplicar.Text = "Aplicar";
            this.btAplicar.UseVisualStyleBackColor = true;
            this.btAplicar.Click += new System.EventHandler(this.btAplicar_Click);
            // 
            // btCancelar
            // 
            this.btCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelar.Location = new System.Drawing.Point(832, 82);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 4;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // tboxNome
            // 
            this.tboxNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxNome.Location = new System.Drawing.Point(104, 52);
            this.tboxNome.MaxLength = 150;
            this.tboxNome.Name = "tboxNome";
            this.tboxNome.Size = new System.Drawing.Size(884, 20);
            this.tboxNome.TabIndex = 3;
            this.tboxNome.TextChanged += new System.EventHandler(this.tboxNome_TextChanged);
            this.tboxNome.Enter += new System.EventHandler(this.tboxNome_Enter);
            // 
            // tboxID
            // 
            this.tboxID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxID.Location = new System.Drawing.Point(104, 23);
            this.tboxID.MaxLength = 20;
            this.tboxID.Name = "tboxID";
            this.tboxID.Size = new System.Drawing.Size(884, 20);
            this.tboxID.TabIndex = 2;
            this.tboxID.TextChanged += new System.EventHandler(this.tboxID_TextChanged);
            this.tboxID.Enter += new System.EventHandler(this.tboxID_Enter);
            // 
            // btEditarNome
            // 
            this.btEditarNome.Location = new System.Drawing.Point(7, 50);
            this.btEditarNome.Name = "btEditarNome";
            this.btEditarNome.Size = new System.Drawing.Size(91, 23);
            this.btEditarNome.TabIndex = 1;
            this.btEditarNome.Text = "Editar Nome";
            this.btEditarNome.UseVisualStyleBackColor = true;
            this.btEditarNome.Click += new System.EventHandler(this.btEditarNome_Click);
            // 
            // btEditarID
            // 
            this.btEditarID.Location = new System.Drawing.Point(7, 21);
            this.btEditarID.Name = "btEditarID";
            this.btEditarID.Size = new System.Drawing.Size(91, 23);
            this.btEditarID.TabIndex = 0;
            this.btEditarID.Text = "Editar ID";
            this.btEditarID.UseVisualStyleBackColor = true;
            this.btEditarID.Click += new System.EventHandler(this.btEditarID_Click);
            // 
            // btRem
            // 
            this.btRem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btRem.Image = ((System.Drawing.Image)(resources.GetObject("btRem.Image")));
            this.btRem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btRem.Location = new System.Drawing.Point(870, 202);
            this.btRem.Name = "btRem";
            this.btRem.Size = new System.Drawing.Size(104, 30);
            this.btRem.TabIndex = 2;
            this.btRem.Text = "Remover";
            this.btRem.UseVisualStyleBackColor = true;
            this.btRem.Click += new System.EventHandler(this.btRem_Click);
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btAdd.Image = ((System.Drawing.Image)(resources.GetObject("btAdd.Image")));
            this.btAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAdd.Location = new System.Drawing.Point(870, 166);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(104, 30);
            this.btAdd.TabIndex = 1;
            this.btAdd.Text = "Adicionar";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // Motorista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboxMotorista);
            this.Controls.Add(this.btRem);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.lvMotoristas);
            this.Name = "Motorista";
            this.Size = new System.Drawing.Size(1000, 488);
            this.gboxMotorista.ResumeLayout(false);
            this.gboxMotorista.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvMotoristas;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader Nome;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btRem;
        private System.Windows.Forms.GroupBox gboxMotorista;
        private System.Windows.Forms.Button btAplicar;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.TextBox tboxNome;
        private System.Windows.Forms.TextBox tboxID;
        private System.Windows.Forms.Button btEditarNome;
        private System.Windows.Forms.Button btEditarID;
    }
}
