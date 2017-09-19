using Controlador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PontosX2
{
    public partial class FormSearchRoute : Form
    {
        public bool IgnoreCase = false;
        public bool MatchWord = false;
        public string FindWhat = String.Empty;
        public bool isRoteiro;
        public int startIndex;
        private ResourceManager rm;

        public FormSearchRoute()
        {
            InitializeComponent();            
        }

        private void CarregarIdioma()
        {

            Fachada fachada = Fachada.Instance;
            rm = fachada.carregaIdioma();

            if (isRoteiro)
                this.Text = rm.GetString("FIND_CAPTION_ROTA");
            else
                this.Text = rm.GetString("FIND_CAPTION_MENSAGEM");

            this.lbFindWhat.Text = rm.GetString("FIND_WHAT");
            this.gbFindOptions.Text = rm.GetString("FIND_OPTION");            
            
            this.cbIgnoreCase.Text = rm.GetString("FIND_IGNORECASE");
            this.cbMatchWord.Text = rm.GetString("FIND_MATCHWORD");
            this.cbTexts.Text = rm.GetString("FIND_TEXTS");

            this.btFind.Text = rm.GetString("FIND_BUTTONFIND");
            this.btCancelar.Text = rm.GetString("FIND_CANCEL");            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Localizar();
        }

        private void Localizar()
        {
            startIndex = ((MDIPrincipal)(this.Owner)).Localizar(isRoteiro, startIndex, tbFindWhat.Text, cbMatchWord.Checked, cbIgnoreCase.Checked, cbTexts.Checked);
            if (startIndex < 0)
            {
                startIndex = 0;
                if (isRoteiro)
                    MessageBox.Show(rm.GetString("FIND_BUTTONFIND_MBOX_ROTEIRO"));
                else
                    MessageBox.Show(rm.GetString("FIND_BUTTONFIND_MBOX_MENSAGEM"));
            }
            else
            {
                startIndex = startIndex + 1;
                int quantidade = 0;
                if (isRoteiro)
                    quantidade = ((MDIPrincipal)(this.Owner)).QuantidadeRoteiros();
                else
                    quantidade = ((MDIPrincipal)(this.Owner)).QuantidadeMensagens();

                if (startIndex >= quantidade)
                    startIndex = 0;
            }
        }

        private void FormSearchRoute_Shown(object sender, EventArgs e)
        {
            CarregarIdioma();

            startIndex = 0;
        }

        private void btFind_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                Localizar();
            }
        }
    }
}

