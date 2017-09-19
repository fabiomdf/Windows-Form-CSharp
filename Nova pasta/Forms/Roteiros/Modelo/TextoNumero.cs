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
    public partial class TextoNumero : UserControl
    {

        Fachada fachada = Fachada.Instance;

        String mensagemErro = String.Empty;

        
        private void AplicaIdioma()
        {
            ResourceManager rm = fachada.carregaIdioma();

            this.rtbTexto01.Text = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
            this.rtbNumero.Text = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
            mensagemErro = rm.GetString("TEXTO_ERRO_INSERCAO_TAG");
        }


        public TextoNumero()
        {
            InitializeComponent();
            AplicaIdioma();

        }


        private void rtbTexto01_TextChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(0, rtbTexto01.Text, false);

                if (Util.Util.VerificaCaracterHindi(rtbTexto01.Text))
                    rtbTexto01.Font = new Font("Arial", 26, FontStyle.Regular);
                else
                    rtbTexto01.Font = new Font("Arial", 36, FontStyle.Regular);
            }
        }

        private void rtbNumero_TextChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(1, rtbNumero.Text, false);

                if (Util.Util.VerificaCaracterHindi(rtbNumero.Text))
                    rtbNumero.Font = new Font("Arial", 26, FontStyle.Regular);
                else
                    rtbNumero.Font = new Font("Arial", 36, FontStyle.Regular);
            }
        }

        private void TextoNumero_Load(object sender, EventArgs e)
        {
            rtbNumero.SelectionAlignment = HorizontalAlignment.Center;
            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;
            /* Comandos para diminuir o efeito de flicker */
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            /* Acima os comandos para diminuir o efeito de flicker */
        }

        private void rtbTexto01_Enter(object sender, EventArgs e)
        {
            ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(0, rtbTexto01.Text, true);
        }

        private void rtbNumero_Enter(object sender, EventArgs e)
        {
            ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(1, rtbNumero.Text, true);
        }

        public void CarregarTextos(string numero, string texto)
        {
            rtbNumero.Text = numero;
            rtbTexto01.Text = texto;

            if (Util.Util.VerificaCaracterHindi(rtbNumero.Text))
                rtbNumero.Font = new Font("Arial", 26, FontStyle.Regular);
            else
                rtbNumero.Font = new Font("Arial", 36, FontStyle.Regular);

            if (Util.Util.VerificaCaracterHindi(rtbTexto01.Text))
                rtbTexto01.Font = new Font("Arial", 26, FontStyle.Regular);
            else
                rtbTexto01.Font = new Font("Arial", 36, FontStyle.Regular);

            rtbNumero.SelectionAlignment = HorizontalAlignment.Center;
            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;
        }         

        internal void InserirTexto(int textoSelecionado, string texto)
        {
            string textoAnterior = String.Empty;

            try
            {
                switch (textoSelecionado)
                {
                    case 0:
                        {
                            var aux = rtbTexto01.SelectionAlignment;
                            textoAnterior = rtbTexto01.Text;
                            rtbTexto01.Text = rtbTexto01.Text.Insert(rtbTexto01.SelectionStart, texto);
                            rtbTexto01.SelectionAlignment = aux;
                        }
                        break;
                    case 1:
                        {
                            var aux = rtbNumero.SelectionAlignment;
                            textoAnterior = rtbNumero.Text;
                            rtbNumero.Text = rtbNumero.Text.Insert(rtbNumero.SelectionStart, texto);
                            rtbNumero.SelectionAlignment = aux;
                        }
                        break;                    
                }
            }
            catch
            {
                MessageBox.Show(this, mensagemErro, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                switch (textoSelecionado)
                {
                    case 0:
                        {
                            rtbTexto01.Text = textoAnterior;
                        }
                        break;
                    case 1:
                        {
                            rtbNumero.Text = textoAnterior;
                        }
                        break;                   
                }

                ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(textoSelecionado, textoAnterior, false);
                Focus(textoSelecionado);
            }
        }


        internal void Focus(int textoSelecionado)
        {
            if (textoSelecionado == 0)
            {
                rtbTexto01.Focus();
            }
            else
            {
                rtbNumero.Focus();
            }
        }
    }
}
