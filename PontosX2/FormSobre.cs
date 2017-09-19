using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Threading;
using Controlador;
using System.Resources;
//using FRT.Pontos.Util;

namespace PontosX2
{
    public partial class FormSobre : Form
    {
        Fachada fachada = Fachada.Instance;
        public ResourceManager rm;

        public FormSobre()
        {
            InitializeComponent();

            //Globalizacao
            rm = fachada.carregaIdioma();

            AplicarIdioma();

        }

   
        private void AplicarIdioma()
        {
            Version versionAplicacao = new Version(Application.ProductVersion);

            this.listView1.Items[0].Text = rm.GetString("SOBRE_LISTVIEW_LINHA_1");
            this.listView1.Items[1].Text = rm.GetString("SOBRE_LISTVIEW_LINHA_2");
            this.listView1.Items[2].Text = rm.GetString("SOBRE_LISTVIEW_LINHA_3");

            this.listView1.Items[0].SubItems[1].Text = Application.ProductName;
            this.listView1.Items[1].SubItems[1].Text = versionAplicacao.ToString(3);
            this.listView1.Items[2].SubItems[1].Text = fachada.GetLicenciadoPara();

            lblFRT.Text = Application.CompanyName;

            this.Text = rm.GetString("SOBRE_FORM_TEXT");
            this.listView1.Columns[0].Text = rm.GetString("SOBRE_LISTVIEW_COLUNA_TITULO");
            this.listView1.Columns[1].Text = rm.GetString("SOBRE_LISTVIEW_COLUNA_DESCRICAO");

            string[] pp = this.richTextBox1.Lines;
            pp[0] = rm.GetString("SOBRE_RICHTEXT_GERENCIA");
            pp[3] = rm.GetString("SOBRE_RICHTEXT_DESENVOLVIMENTO");
            pp[8] = rm.GetString("SOBRE_RICHTEXT_FIRMWARE");
            this.richTextBox1.Text = String.Empty;
            this.richTextBox1.Lines = pp;
        }
     

      
    }
}
