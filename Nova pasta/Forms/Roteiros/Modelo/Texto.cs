using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controlador;
using System.Resources;

namespace PontosX2.Forms.Roteiros.Modelo
{
    public partial class Texto : UserControl
    {
        Fachada fachada = Fachada.Instance;

        String mensagemErro = String.Empty;

        private void AplicaIdioma()
        {
            ResourceManager rm = fachada.carregaIdioma();
            this.rtbTexto01.Text = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
            mensagemErro = rm.GetString("TEXTO_ERRO_INSERCAO_TAG");
        }

        public Texto()
        {
            InitializeComponent();
            AplicaIdioma();
        }

        private void rtbTexto01_TextChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                if (Parent.Parent.Parent is TextosEditor)
                {
                    ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(0, rtbTexto01.Text, false);
                }
                else
                {
                    ((NumEditor)Parent.Parent.Parent).SetTextoSelecionado(rtbTexto01.Text);
                }

                if (Util.Util.VerificaCaracterHindi(rtbTexto01.Text))
                    rtbTexto01.Font = new Font("Arial", 26, FontStyle.Regular);
                else
                    rtbTexto01.Font = new Font("Arial", 36, FontStyle.Regular);
            }
            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void Texto_Load(object sender, EventArgs e)
        {
            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;
            /* Comandos para diminuir o efeito de flicker */
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            /* Acima os comandos para diminuir o efeito de flicker */
        }

        private void rtbTexto01_Enter(object sender, EventArgs e)
        {
            if (Parent.Parent.Parent is TextosEditor)
                ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(0, rtbTexto01.Text, true);

        }

        public void CarregarTextos(string texto)
        {
            rtbTexto01.Text = texto;

            if (Util.Util.VerificaCaracterHindi(rtbTexto01.Text))
                rtbTexto01.Font = new Font("Arial", 26, FontStyle.Regular);
            else
                rtbTexto01.Font = new Font("Arial", 36, FontStyle.Regular);

            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;                
        }

        private void rtbTexto01_KeyDown(object sender, KeyEventArgs e)
        {
            if(((RichTextBox)sender).ContainsFocus)
            {
                if (Parent.Parent.Parent is TextosEditor)
                    rtbTexto01.MaxLength = 255;
                else
                    rtbTexto01.MaxLength = 16;
            }
        }

        public void InserirTexto(string texto)
        {
            string textoAnterior = String.Empty;

            try
            {
                var aux = rtbTexto01.SelectionAlignment;
                textoAnterior = rtbTexto01.Text;
                rtbTexto01.Text = rtbTexto01.Text.Insert(rtbTexto01.SelectionStart, texto);
                rtbTexto01.SelectionAlignment = aux;

            }
            catch
            {
                MessageBox.Show(this, mensagemErro, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                rtbTexto01.Text = textoAnterior;

                ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(0, textoAnterior, false);
                rtbTexto01.Focus();
            }

        }

        public void SetarFocus()
        {
            rtbTexto01.Focus();
        }

    }
}
