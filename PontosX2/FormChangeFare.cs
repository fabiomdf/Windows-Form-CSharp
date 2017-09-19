using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Controlador;

namespace PontosX2
{
    public partial class FormChangeFare : Form
    {
        #region Propriedades

        Fachada fachada = Fachada.Instance;
        ResourceManager rm;
        string strTodasTarifas = "";
        string strTarifaDe = "";
        string strTarifaPara = "";
        public int Tarifa { get; set; }
        public int TarifaDe { get; set; }
        public int TarifaPara { get; set; }
        public bool TodasTarifas { get; set; }

        #endregion

        #region Construtor

        public FormChangeFare()
        {
            InitializeComponent();

            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        #endregion

        #region Globalização

        private void AplicaIdioma()
        {
            this.Text = rm.GetString("MUDAR_TARIFA_TEXT");
            this.rbTodasTarifas.Text = rm.GetString("MUDAR_TARIFA_TODAS_TARIFAS_LABEL");
            this.rbTarifaEspecifica.Text = rm.GetString("MUDAR_TARIFA_TARIFA_ESPECIFICA_LABEL");
            this.labelDe.Text = rm.GetString("MUDAR_TARIFA_DE_LABEL"); ;
            this.labelPara.Text = rm.GetString("MUDAR_TARIFA_PARA_LABEL"); ;
            this.btCancel.Text = rm.GetString("MUDAR_TARIFA_BUTTON_CANCELAR");
            this.btApply.Text = rm.GetString("MUDAR_TARIFA_BUTTON_APLICAR");
        }

        #endregion

        #region Entrando no Form

        private void FormChangeFare_Shown(object sender, EventArgs e)
        {
            //tboxFare.SelectionStart = tboxFare.Text.Length + 1;

            rbTodasTarifas.Checked = true;
        }

        #endregion

        #region Alteração GUI

        private void btApply_Click(object sender, EventArgs e)
        {
            
            TodasTarifas = rbTodasTarifas.Checked;
            
            //Se for alterar todas as tarifas
            if (rbTodasTarifas.Checked)
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("MUDAR_TARIFA_MBOX_APLICAR"), rm.GetString("MUDAR_TARIFA_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (tboxFare.Text != "")
                    {
                        try
                        {
                            Tarifa = Convert.ToInt32(tboxFare.Text.Replace(".", string.Empty));
                            this.DialogResult = DialogResult.OK;
                        }
                        catch
                        {
                            tboxFare.Text = "0.00";
                            Tarifa = 0;
                            this.DialogResult = DialogResult.None;
                        }
                    }
                    else
                    {
                        tboxFare.Text = "0.00";
                        Tarifa = 0;
                        this.DialogResult = DialogResult.OK;
                    }

                }
                else
                    this.DialogResult = DialogResult.None;
            }
            else // Se for alterar a tarifa com um valor especifo para outro valor
            {
                if (DialogResult.Yes == MessageBox.Show(rm.GetString("MUDAR_TARIFA_MBOX_APLICAR_TARIFA_ESPECIFICA"), rm.GetString("MUDAR_TARIFA_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    if (tboxDe.Text != "" && tboxPara.Text != "")
                    {
                        try
                        {
                            TarifaDe = Convert.ToInt32(tboxDe.Text.Replace(".", string.Empty));
                            TarifaPara = Convert.ToInt32(tboxPara.Text.Replace(".", string.Empty));
                            this.DialogResult = DialogResult.OK;
                        }
                        catch
                        {
                            tboxDe.Text = "0.00";
                            TarifaDe = 0;
                            tboxPara.Text = "0.00";
                            TarifaPara = 0;
                            this.DialogResult = DialogResult.None;
                        }
                    }
                    else
                    {
                        tboxDe.Text = "0.00";
                        TarifaDe = 0;
                        tboxPara.Text = "0.00";
                        TarifaPara = 0;
                        this.DialogResult = DialogResult.OK;
                    }

                }
                else
                    this.DialogResult = DialogResult.None;
            }
        }

        private void tboxFare_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;            
        }

        private bool IsNumeric(int Val)
        {
            return ((Val >= 48 && Val <= 57) || (Val == 8) || (Val == 46));
        }

        private void tboxFare_KeyDown(object sender, KeyEventArgs e)
        {
            int KeyCode = 0;

            //transformando o numpad em algarismos
            switch (e.KeyValue)
            {
                case 96: KeyCode = 48; break;
                case 97: KeyCode = 49; break;
                case 98: KeyCode = 50; break;
                case 99: KeyCode = 51; break;
                case 100: KeyCode = 52; break;
                case 101: KeyCode = 53; break; 
                case 102: KeyCode = 54; break; 
                case 103: KeyCode = 55; break; 
                case 104: KeyCode = 56; break;
                case 105: KeyCode = 57; break; 
            }

            if (KeyCode==0)
                KeyCode = e.KeyValue;

            if (!IsNumeric(KeyCode))            
            {
                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = true;
            }

            //Definindo o tamaho máximo de 9 caracteres
            if (strTodasTarifas.Length == 8 && (!(KeyCode == 8 || KeyCode == 46)))
            {
                e.Handled = true;
                return;
            }

            //Se o texto for tamanho 0 também não deve permitir digitar 0
            if (strTodasTarifas.Length == 0 && KeyCode == 48)
            {
                e.Handled = true;
                return;
            }

            if (((KeyCode == 8) || (KeyCode == 46)) && (strTodasTarifas.Length > 0))
            {
                strTodasTarifas = strTodasTarifas.Substring(0, strTodasTarifas.Length - 1);
            }
            else if (!((KeyCode == 8) || (KeyCode == 46)))
            {
                strTodasTarifas = strTodasTarifas + Convert.ToChar(KeyCode);
            }

            if (strTodasTarifas.Length == 0)
            {
                tboxFare.Text = "0.00";
            }

            if (strTodasTarifas.Length == 1)
            {
                tboxFare.Text = "0.0" + strTodasTarifas;
            }
            else
            {
                if (strTodasTarifas.Length == 2)
                {
                    tboxFare.Text = "0." + strTodasTarifas;
                }
                else
                {
                    if (strTodasTarifas.Length > 2)
                    {
                        tboxFare.Text = strTodasTarifas.Substring(0, strTodasTarifas.Length - 2) + "." +
                                        strTodasTarifas.Substring(strTodasTarifas.Length - 2);
                    }
                }
            }

            tboxFare.SelectionStart = tboxFare.Text.Length + 1;
        }

        private void tboxFare_Click(object sender, EventArgs e)
        {
            tboxFare.SelectionStart = tboxFare.Text.Length + 1;
        }

        private void rbTodasTarifas_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTodasTarifas.Checked)
            {
                labelDe.Visible = false;
                labelPara.Visible = false;
                tboxDe.Visible = false;
                tboxPara.Visible = false;

                tboxFare.Visible = true;
                tboxFare.Select();
            }
            else
            {
                labelDe.Visible = true;
                labelPara.Visible = true;
                tboxDe.Visible = true;
                tboxPara.Visible = true;
                tboxDe.Location = new Point(labelDe.Location.X + labelDe.Width + 2, tboxDe.Location.Y);
                tboxPara.Location = new Point(labelPara.Location.X + labelPara.Width + 2, tboxPara.Location.Y);

                tboxFare.Visible = false;

                tboxDe.Select();
            }
        }

        private void tboxDe_KeyDown(object sender, KeyEventArgs e)
        {
            int KeyCode = 0;

            //transformando o numpad em algarismos
            switch (e.KeyValue)
            {
                case 96: KeyCode = 48; break;
                case 97: KeyCode = 49; break;
                case 98: KeyCode = 50; break;
                case 99: KeyCode = 51; break;
                case 100: KeyCode = 52; break;
                case 101: KeyCode = 53; break;
                case 102: KeyCode = 54; break;
                case 103: KeyCode = 55; break;
                case 104: KeyCode = 56; break;
                case 105: KeyCode = 57; break;
            }

            if (KeyCode == 0)
                KeyCode = e.KeyValue;

            if (!IsNumeric(KeyCode))
            {
                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = true;
            }

            //Definindo o tamaho máximo de 9 caracteres
            if (strTarifaDe.Length == 8 && (!(KeyCode == 8 || KeyCode == 46)))
            {
                e.Handled = true;
                return;
            }

            //Se o texto for tamanho 0 também não deve permitir digitar 0
            if (strTarifaDe.Length == 0 && KeyCode == 48)
            {
                e.Handled = true;
                return;
            }

            if (((KeyCode == 8) || (KeyCode == 46)) && (strTarifaDe.Length > 0))
            {
                strTarifaDe = strTarifaDe.Substring(0, strTarifaDe.Length - 1);
            }
            else if (!((KeyCode == 8) || (KeyCode == 46)))
            {
                strTarifaDe = strTarifaDe + Convert.ToChar(KeyCode);
            }

            if (strTarifaDe.Length == 0)
            {
                tboxDe.Text = "0.00";
            }

            if (strTarifaDe.Length == 1)
            {
                tboxDe.Text = "0.0" + strTarifaDe;
            }
            else
            {
                if (strTarifaDe.Length == 2)
                {
                    tboxDe.Text = "0." + strTarifaDe;
                }
                else
                {
                    if (strTarifaDe.Length > 2)
                    {
                        tboxDe.Text = strTarifaDe.Substring(0, strTarifaDe.Length - 2) + "." +
                                        strTarifaDe.Substring(strTarifaDe.Length - 2);
                    }
                }
            }

            tboxDe.SelectionStart = tboxDe.Text.Length + 1;
        }

        private void tboxPara_KeyDown(object sender, KeyEventArgs e)
        {
            int KeyCode = 0;

            //transformando o numpad em algarismos
            switch (e.KeyValue)
            {
                case 96: KeyCode = 48; break;
                case 97: KeyCode = 49; break;
                case 98: KeyCode = 50; break;
                case 99: KeyCode = 51; break;
                case 100: KeyCode = 52; break;
                case 101: KeyCode = 53; break;
                case 102: KeyCode = 54; break;
                case 103: KeyCode = 55; break;
                case 104: KeyCode = 56; break;
                case 105: KeyCode = 57; break;
            }

            if (KeyCode == 0)
                KeyCode = e.KeyValue;

            if (!IsNumeric(KeyCode))
            {
                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = true;
            }

            //Definindo o tamaho máximo de 9 caracteres
            if (strTarifaPara.Length == 8 && (!(KeyCode == 8 || KeyCode == 46)))
            {
                e.Handled = true;
                return;
            }

            //Se o texto for tamanho 0 também não deve permitir digitar 0
            if (strTarifaPara.Length == 0 && KeyCode == 48)
            {
                e.Handled = true;
                return;
            }

            if (((KeyCode == 8) || (KeyCode == 46)) && (strTarifaPara.Length > 0))
            {
                strTarifaPara = strTarifaPara.Substring(0, strTarifaPara.Length - 1);
            }
            else if (!((KeyCode == 8) || (KeyCode == 46)))
            {
                strTarifaPara = strTarifaPara + Convert.ToChar(KeyCode);
            }

            if (strTarifaPara.Length == 0)
            {
                tboxPara.Text = "0.00";
            }

            if (strTarifaPara.Length == 1)
            {
                tboxPara.Text = "0.0" + strTarifaPara;
            }
            else
            {
                if (strTarifaPara.Length == 2)
                {
                    tboxPara.Text = "0." + strTarifaPara;
                }
                else
                {
                    if (strTarifaPara.Length > 2)
                    {
                        tboxPara.Text = strTarifaPara.Substring(0, strTarifaPara.Length - 2) + "." +
                                        strTarifaPara.Substring(strTarifaPara.Length - 2);
                    }
                }
            }

            tboxPara.SelectionStart = tboxPara.Text.Length + 1;
        }

        private void tboxDe_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void tboxPara_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void tboxDe_Click(object sender, EventArgs e)
        {
            tboxDe.SelectionStart = tboxDe.Text.Length + 1;
        }

        private void tboxPara_Click(object sender, EventArgs e)
        {
            tboxPara.SelectionStart = tboxPara.Text.Length + 1;
        }

        #endregion
    }
}
