namespace LPontos2.Forms.Mensagens
{
    partial class ListarMensagens
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListarMensagens));
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btnIncluir = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.incluirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCopiar = new System.Windows.Forms.Button();
            this.gridMensagens = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMensagens)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExcluir
            // 
            this.btnExcluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcluir.Image = ((System.Drawing.Image)(resources.GetObject("btnExcluir.Image")));
            this.btnExcluir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExcluir.Location = new System.Drawing.Point(596, 70);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(104, 30);
            this.btnExcluir.TabIndex = 19;
            this.btnExcluir.Text = "Excluir";
            this.btnExcluir.UseVisualStyleBackColor = true;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btnIncluir
            // 
            this.btnIncluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncluir.Image = ((System.Drawing.Image)(resources.GetObject("btnIncluir.Image")));
            this.btnIncluir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIncluir.Location = new System.Drawing.Point(596, 34);
            this.btnIncluir.Name = "btnIncluir";
            this.btnIncluir.Size = new System.Drawing.Size(104, 30);
            this.btnIncluir.TabIndex = 18;
            this.btnIncluir.Text = "Incluir";
            this.btnIncluir.UseVisualStyleBackColor = true;
            this.btnIncluir.Click += new System.EventHandler(this.btnIncluir_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.incluirToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.MenuItemCopy});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 70);
            // 
            // incluirToolStripMenuItem
            // 
            this.incluirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("incluirToolStripMenuItem.Image")));
            this.incluirToolStripMenuItem.Name = "incluirToolStripMenuItem";
            this.incluirToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.incluirToolStripMenuItem.Text = "Insert";
            this.incluirToolStripMenuItem.Click += new System.EventHandler(this.btnIncluir_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // MenuItemCopy
            // 
            this.MenuItemCopy.Image = ((System.Drawing.Image)(resources.GetObject("MenuItemCopy.Image")));
            this.MenuItemCopy.Name = "MenuItemCopy";
            this.MenuItemCopy.Size = new System.Drawing.Size(107, 22);
            this.MenuItemCopy.Text = "Copy";
            this.MenuItemCopy.Click += new System.EventHandler(this.MenuItemCopy_Click);
            // 
            // btnCopiar
            // 
            this.btnCopiar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopiar.Image = ((System.Drawing.Image)(resources.GetObject("btnCopiar.Image")));
            this.btnCopiar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopiar.Location = new System.Drawing.Point(596, 106);
            this.btnCopiar.Name = "btnCopiar";
            this.btnCopiar.Size = new System.Drawing.Size(104, 30);
            this.btnCopiar.TabIndex = 20;
            this.btnCopiar.Text = "Copiar";
            this.btnCopiar.UseVisualStyleBackColor = true;
            this.btnCopiar.Click += new System.EventHandler(this.btnCopiar_Click);
            // 
            // gridMensagens
            // 
            this.gridMensagens.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.Rows;
            this.gridMensagens.AllowEditing = false;
            this.gridMensagens.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Nodes;
            this.gridMensagens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMensagens.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            this.gridMensagens.ColumnInfo = resources.GetString("gridMensagens.ColumnInfo");
            this.gridMensagens.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.gridMensagens.ExtendLastCol = true;
            this.gridMensagens.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None;
            this.gridMensagens.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gridMensagens.Location = new System.Drawing.Point(3, 3);
            this.gridMensagens.Name = "gridMensagens";
            this.gridMensagens.Rows.DefaultSize = 19;
            this.gridMensagens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridMensagens.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.gridMensagens.Size = new System.Drawing.Size(729, 355);
            this.gridMensagens.StyleInfo = resources.GetString("gridMensagens.StyleInfo");
            this.gridMensagens.TabIndex = 21;
            this.gridMensagens.Tree.Column = 0;
            this.gridMensagens.BeforeMouseDown += new C1.Win.C1FlexGrid.BeforeMouseDownEventHandler(this.gridMensagens_BeforeMouseDown);
            this.gridMensagens.AfterSort += new C1.Win.C1FlexGrid.SortColEventHandler(this.gridMensagens_AfterSort);
            this.gridMensagens.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridMensagens_DragDrop);
            this.gridMensagens.DragOver += new System.Windows.Forms.DragEventHandler(this.gridMensagens_DragOver);
            this.gridMensagens.DoubleClick += new System.EventHandler(this.gridMensagens_DoubleClick);
            this.gridMensagens.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridMensagens_MouseDown);
            // 
            // ListarMensagens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCopiar);
            this.Controls.Add(this.btnExcluir);
            this.Controls.Add(this.btnIncluir);
            this.Controls.Add(this.gridMensagens);
            this.Name = "ListarMensagens";
            this.Size = new System.Drawing.Size(736, 361);
            this.VisibleChanged += new System.EventHandler(this.ListarMensagens_VisibleChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMensagens)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Button btnIncluir;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem incluirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCopy;
        private System.Windows.Forms.Button btnCopiar;
        private C1.Win.C1FlexGrid.C1FlexGrid gridMensagens;
    }
}
