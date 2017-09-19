using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PontosX2.Forms.Motorista.Modelo
{
    public partial class TextoMotorista : UserControl
    {
        public TextoMotorista()
        {
            InitializeComponent();
        }

        private void rtbTexto01_TextChanged(object sender, EventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                ((MotoristaEditor)Parent.Parent.Parent).SetTextoSelecionado(rtbTexto01.Text);

                if (Util.Util.VerificaCaracterHindi(rtbTexto01.Text))
                    rtbTexto01.Font = new Font("Arial", 26, FontStyle.Regular);
                else
                    rtbTexto01.Font = new Font("Arial", 36, FontStyle.Regular);
            }
        }

        private void TextoMotorista_Load(object sender, EventArgs e)
        {
            rtbTexto01.SelectionAlignment = HorizontalAlignment.Center;
            /* Comandos para diminuir o efeito de flicker */
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            /* Acima os comandos para diminuir o efeito de flicker */
        }

        private void rtbTexto01_KeyDown(object sender, KeyEventArgs e)
        {
            if (((RichTextBox)sender).ContainsFocus)
            {
                if (((MotoristaEditor)Parent.Parent.Parent).isID)
                    rtbTexto01.MaxLength = 20;
                else
                    rtbTexto01.MaxLength = 150;
            }           
                
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

        public void SetarTexto()
        {
            rtbTexto01.Focus();
        }
    }
}
