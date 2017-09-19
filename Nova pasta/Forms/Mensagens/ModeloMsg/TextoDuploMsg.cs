using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using Controlador;

namespace PontosX2.Forms.Mensagens.ModeloMsg
{
    public partial class TextoDuploMsg : UserControl
    {
        Fachada fachada = Fachada.Instance;

        String mensagemErro = String.Empty;

        public TextoDuploMsg()
        {
            InitializeComponent();
            AplicaIdioma();
        }

        private void AplicaIdioma()
        {
            ResourceManager rm = fachada.carregaIdioma();

            this.rtbTexto01.Text = rm.GetString("USER_CONTROL_MODELOS_TEXTO_01");
            this.rtbTexto02.Text = rm.GetString("USER_CONTROL_MODELOS_TEXTO_02");

            mensagemErro = rm.GetString("TEXTO_ERRO_INSERCAO_TAG");
        }

        private void rtbTexto01_TextChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                ((TextosEditorMsg)Parent.Parent.Parent).SetTextoSelecionado(0, rtbTexto01.Text, false);


                if (Util.Util.VerificaCaracterHindi(rtbTexto01.Text))
                    rtbTexto01.Font = new Font("Arial", 26, FontStyle.Regular);
                else
                    rtbTexto01.Font = new Font("Arial", 36, FontStyle.Regular);
            }
            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;
        }
        private void rtbTexto02_TextChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                ((TextosEditorMsg)Parent.Parent.Parent).SetTextoSelecionado(1, rtbTexto02.Text, false);


                if (Util.Util.VerificaCaracterHindi(rtbTexto02.Text))
                    rtbTexto02.Font = new Font("Arial", 26, FontStyle.Regular);
                else
                    rtbTexto02.Font = new Font("Arial", 36, FontStyle.Regular);
            }
            rtbTexto02.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rtbTexto01_Enter(object sender, EventArgs e)
        {
            ((TextosEditorMsg)Parent.Parent.Parent).SetTextoSelecionado(0, rtbTexto01.Text, true);
        }

        private void rtbTexto02_Enter(object sender, EventArgs e)
        {
            ((TextosEditorMsg)Parent.Parent.Parent).SetTextoSelecionado(1, rtbTexto02.Text, true);
        }

        private void TextoDuploMsg_Load(object sender, EventArgs e)
        {
            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;
            rtbTexto02.SelectionAlignment = HorizontalAlignment.Center;
            /* Comandos para diminuir o efeito de flicker */
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            /* Acima os comandos para diminuir o efeito de flicker */
        }


        public void CarregarTextos(string texto1, string texto2)
        {

            rtbTexto01.Text = texto1;
            rtbTexto02.Text = texto2;

            if (Util.Util.VerificaCaracterHindi(rtbTexto01.Text))
                rtbTexto01.Font = new Font("Arial", 26, FontStyle.Regular);
            else
                rtbTexto01.Font = new Font("Arial", 36, FontStyle.Regular);

            if (Util.Util.VerificaCaracterHindi(rtbTexto02.Text))
                rtbTexto02.Font = new Font("Arial", 26, FontStyle.Regular);
            else
                rtbTexto02.Font = new Font("Arial", 36, FontStyle.Regular);

            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;
            rtbTexto02.SelectionAlignment = HorizontalAlignment.Center;
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
                            var aux = rtbTexto02.SelectionAlignment;
                            textoAnterior = rtbTexto02.Text;
                            rtbTexto02.Text = rtbTexto02.Text.Insert(rtbTexto02.SelectionStart, texto);
                            rtbTexto02.SelectionAlignment = aux;
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
                            rtbTexto02.Text = textoAnterior;
                        }
                        break;                   
                }

                ((TextosEditorMsg)Parent.Parent.Parent).SetTextoSelecionado(textoSelecionado, textoAnterior, false);
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
                rtbTexto02.Focus();
            }
        }
    }
}
