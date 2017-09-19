using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using Controlador;

namespace PontosX2
{
    public partial class FormFontEditorOpen : Form
    {
        public string caminhoFonte;

        Fachada fachada = Fachada.Instance;
        ResourceManager rm;

        public FormFontEditorOpen()
        {
            InitializeComponent();

            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        private void AplicaIdioma()
        {
            this.Text = rm.GetString("EDITOR_FONTES_FORM_TEXT");
            this.gboxAbrirFontes.Text = rm.GetString("EDITOR_FONTES_BT_OPEN");
            this.btnAbrir.Text = rm.GetString("EDITOR_FONTES_ABRIR_BT_ABRIR");
            this.btnCancelar.Text = rm.GetString("EDITOR_FONTES_ABRIR_BT_CANCELAR");
        }

        private void FormFontEditorOpen_Load(object sender, EventArgs e)
        {
            CarregarFontesPontos();

            if (cboxFontes.Items.Count > 0)
                cboxFontes.SelectedIndex = 0;
        }

        private void CarregarFontesPontos()
        {
            cboxFontes.Items.Clear();
            cboxFontes.Items.AddRange(fachada.ExibeFontesPontos().ToArray());
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (cboxFontes.SelectedIndex > -1)
            {
                caminhoFonte = fachada.GetDiretorio("fontes") + "\\" + cboxFontes.SelectedItem.ToString() + Util.Util.ARQUIVO_EXT_FNT;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(rm.GetString("EDITOR_FONTES_ABRIR_SEM_FONTES"));
                this.DialogResult = DialogResult.None;
            }
        }
    }
}
