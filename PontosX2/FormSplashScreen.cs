using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Controlador;
using System.Resources;

namespace PontosX2
{
    public partial class FormSplashScreen : Form
    {
        public bool fecharSplashUsuario = false;
        Fachada fachada = Fachada.Instance;

        public FormSplashScreen()
        {
            InitializeComponent();

            this.ClientSize = this.BackgroundImage.Size;

            this.picFechar.SizeMode = PictureBoxSizeMode.CenterImage;

            Version versionAplicacao = new Version(Application.ProductVersion);
            this.labelVersao.Text = "v " + versionAplicacao.ToString(3);
        }

        private void FormSplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!fecharSplashUsuario)
            {
                while (this.Opacity != 0)
                {
                    this.Opacity -= 0.05;
                    Thread.Sleep(40);
                }
            }
        }

        public void SetStatus(string status)
        {
            labelStatus.Text = status;
        }

        public bool AbrirArquivo(string arquivo)
        {
            bool retorno = true;
            if (arquivo != null)
            {
                fachada.Abrir(arquivo);

                if (fachada.QuantidadeControladores() > 0)
                {
                    //Checa todas as fontes do controlador
                    if (!fachada.ChecarFontesControlador(fachada.QuantidadeControladores() - 1))
                        retorno = false;                 
                }
            }
            return retorno;
        }

        private void picFechar_Click(object sender, EventArgs e)
        {
            fecharSplashUsuario = true;
        }

        private void picMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
