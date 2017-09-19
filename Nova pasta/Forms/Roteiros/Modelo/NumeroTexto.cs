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
    public partial class NumeroTexto : UserControl
    {
        Fachada fachada = Fachada.Instance;

        String mensagemErro = String.Empty;


        private void AplicaIdioma()
        {
            ResourceManager rm = fachada.carregaIdioma();

            this.rtbText01.Text = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
            this.rtbNumero.Text = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");
            mensagemErro = rm.GetString("TEXTO_ERRO_INSERCAO_TAG");
        }
        public NumeroTexto()
        {
            InitializeComponent();            
        }


        private void rtbNumero_TextChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(0, rtbNumero.Text, false);

                if (Util.Util.VerificaCaracterHindi(rtbNumero.Text))
                    rtbNumero.Font = new Font("Arial", 26, FontStyle.Regular);
                else
                    rtbNumero.Font = new Font("Arial", 36, FontStyle.Regular);
            }
            rtbNumero.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rtbText01_TextChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(1, rtbText01.Text, false);

                if (Util.Util.VerificaCaracterHindi(rtbText01.Text))
                    rtbText01.Font = new Font("Arial", 26, FontStyle.Regular);
                else
                    rtbText01.Font = new Font("Arial", 36, FontStyle.Regular);
            }
            rtbText01.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void NumeroTexto_Load(object sender, EventArgs e)
        {
            rtbNumero.SelectionAlignment = HorizontalAlignment.Center;
            rtbText01.SelectionAlignment = HorizontalAlignment.Center;
            /* Comandos para diminuir o efeito de flicker */
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            /* Acima os comandos para diminuir o efeito de flicker */
        }

        private void rtbNumero_Enter(object sender, EventArgs e)
        {
            ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(0, rtbNumero.Text, true);
        }

        private void rtbText01_Enter(object sender, EventArgs e)
        {
            ((TextosEditor)Parent.Parent.Parent).SetTextoSelecionado(1, rtbText01.Text, true);
        }

        public void CarregarTextos(string numero, string texto)
        {
            rtbNumero.Text = numero;
            rtbText01.Text = texto;

            if (Util.Util.VerificaCaracterHindi(rtbNumero.Text))
                rtbNumero.Font = new Font("Arial", 26, FontStyle.Regular);
            else
                rtbNumero.Font = new Font("Arial", 36, FontStyle.Regular);

            if (Util.Util.VerificaCaracterHindi(rtbText01.Text))
                rtbText01.Font = new Font("Arial", 26, FontStyle.Regular);
            else
                rtbText01.Font = new Font("Arial", 36, FontStyle.Regular);

            rtbNumero.SelectionAlignment = HorizontalAlignment.Center;
            rtbText01.SelectionAlignment = HorizontalAlignment.Center;
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
                            var aux = rtbNumero.SelectionAlignment;
                            textoAnterior = rtbNumero.Text;
                            rtbNumero.Text = rtbNumero.Text.Insert(rtbNumero.SelectionStart, texto);
                            rtbNumero.SelectionAlignment = aux;

                        }
                        break;
                    case 1:
                        {
                            var aux = rtbText01.SelectionAlignment;
                            textoAnterior = rtbText01.Text;
                            rtbText01.Text = rtbText01.Text.Insert(rtbText01.SelectionStart, texto);
                            rtbText01.SelectionAlignment = aux;
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
                            rtbNumero.Text = textoAnterior;
                        }
                        break;
                    case 1:
                        {
                            rtbText01.Text = textoAnterior;
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
                rtbNumero.Focus();
            }
            else
            {
                rtbText01.Focus();
            }
        }
    }
}
