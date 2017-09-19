using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controlador;
using System.Resources;

namespace PontosX2
{
    public partial class FormPerifericoAnexo : Form
    {
        Fachada fachada = Fachada.Instance;
        ResourceManager rm;

        bool[] perifericos;
        public int controladorSelecionado;

        public FormPerifericoAnexo()
        {
            InitializeComponent();

            //Globalização
            rm = fachada.carregaIdioma();
            AplicaIdioma();
        }

        private void AplicaIdioma()
        {
            this.Text = rm.GetString("FORM_PERIFERICO_NA_REDE_NOME");
            gboxPerifericos.Text = rm.GetString("FORM_PERIFERICO_NA_REDE_GBOX");
            chkCatraca.Text = rm.GetString("FORM_PERIFERICO_NA_REDE_CHECK_CATRACA");
            chkTemp.Text = rm.GetString("FORM_PERIFERICO_NA_REDE_CHECK_TEMPERATURA");
            chkVelocidade.Text = rm.GetString("FORM_PERIFERICO_NA_REDE_CHECK_VELOCIDADE");
            chkApp.Text = rm.GetString("FORM_PERIFERICO_NA_REDE_CHECK_APP");
            btCancelar.Text = rm.GetString("FORM_PERIFERICO_NA_REDE_BT_CANCELAR");
            btAplicar.Text = rm.GetString("FORM_PERIFERICO_NA_REDE_BT_APLICAR");
        }

        private void FormPerifericoAnexo_Shown(object sender, EventArgs e)
        {
            perifericos = fachada.RetornarPerifericos(controladorSelecionado);
            carregarPaineis();

            chkCatraca.Checked = perifericos[(int)Util.Util.IndicePerifericosNaRede.Catraca];
            chkVelocidade.Checked = perifericos[(int)Util.Util.IndicePerifericosNaRede.Velocidade];
            chkTemp.Checked = perifericos[(int)Util.Util.IndicePerifericosNaRede.Temperatura];
            chkApp.Checked = perifericos[(int)Util.Util.IndicePerifericosNaRede.APP];

            //cbPainel.SelectedIndex = (fachada.RetornarPainelAPP_NSS(controladorSelecionado)==255? cbPainel.Items.Count-1 : fachada.RetornarPainelAPP_NSS(controladorSelecionado));
            HabilitarListaPaineis(chkApp.Checked);
            ChecarListaPaineis(fachada.RetornarPaineisAPP(controladorSelecionado));

        }

        private void ChecarListaPaineis(List<int> paineis)
        {
            foreach (int i in paineis)
                cbListPaineis.SetItemChecked(i, true);
        }

        private void carregarPaineis()
        {
            cbListPaineis.Items.Clear();

            string labelPainel;

            foreach (Painel painel in fachada.CarregarPaineis(controladorSelecionado))
            {

                if (painel.MultiLinhas > 1)
                {
                    labelPainel = rm.GetString("ARQUIVO_PAINEL") + " " + (painel.Indice + 1).ToString("d3") + " " + (painel.Altura / painel.MultiLinhas).ToString("d2") + " X " + painel.Largura.ToString("d2") +
                                    " (" + rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS") + " " + painel.MultiLinhas + "x)";
                }
                else
                {
                    labelPainel = rm.GetString("ARQUIVO_PAINEL") + " " + (painel.Indice + 1).ToString("d3") + " " + painel.Altura.ToString("d2") + " X " + painel.Largura.ToString("d2");                    
                }

                cbListPaineis.Items.Add(labelPainel);

            }

        }

        private void btAplicar_Click(object sender, EventArgs e)
        {
            if (chkApp.Checked)
                if (cbListPaineis.CheckedIndices.Count == 0)
                {
                    MessageBox.Show(rm.GetString("FORM_PERIFERICO_MBOX_PAINEL"));
                    return;
                }

            perifericos[(int)Util.Util.IndicePerifericosNaRede.Catraca] = chkCatraca.Checked;
            perifericos[(int)Util.Util.IndicePerifericosNaRede.Velocidade] = chkVelocidade.Checked;
            perifericos[(int)Util.Util.IndicePerifericosNaRede.Temperatura] = chkTemp.Checked;
            perifericos[(int)Util.Util.IndicePerifericosNaRede.APP] = chkApp.Checked;


            if (chkApp.Checked)
                fachada.SetarPaineisAPP(controladorSelecionado, PaineisSelecionados());

            this.Close();
        }


        private List<int> PaineisSelecionados()
        {
            List<int> paineis = new List<int>();

            foreach(int i in cbListPaineis.CheckedIndices)
            {
                paineis.Add(i);
            }

            return paineis;
        }

        private void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkApp.Checked)
                HabilitarListaPaineis(true);
            else
                HabilitarListaPaineis(false);

        }

        private void HabilitarListaPaineis(bool habilitar)
        {
            if (habilitar)
            {
                cbListPaineis.Visible = true;
                this.Height = 264;
                this.gboxPerifericos.Height = 174;
            }
            else
            {
                cbListPaineis.Visible = false;
                this.Height = 204;
                this.gboxPerifericos.Height = 114;
            }
        }

        private void cbListPaineis_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (((CheckedListBox)sender).ContainsFocus)
            {
                if (fachada.GetMultiLinhas(controladorSelecionado, e.Index) > 1)
                {
                    MessageBox.Show(rm.GetString("FORM_PERIFERICO_MBOX_PAINEL_MULTILINHAS"));
                    e.NewValue = CheckState.Unchecked;
                }
            }
        }
    }
}
