using Controlador;
using Persistencia;
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
    public partial class FormHabilitaAlternancia : Form
    {
        Arquivo_ALT alternancia;
        Fachada fachada = Fachada.Instance;
        public List<ItemAlternancia> ListaAlternancias = new List<ItemAlternancia>();
        public int controladorSelecionado;
        public int painelSelecionado;
        
        ResourceManager rm;

        public FormHabilitaAlternancia()
        {
            InitializeComponent();
            alternancia = fachada.CarregarAlternancia();
            AplicarIdioma();            
        }

        private void AplicarIdioma()
        {
            rm = fachada.carregaIdioma();
            this.lbAlternancia.Text = rm.GetString("TEXTO_ALTERNANCIA_Texto");
            this.Text = rm.GetString("TEXTO_ALTERNANCIA_Caption");
            this.btAplicar.Text = rm.GetString("TEXTO_ALTERNANCIA_btAplicar");
            this.btFechar.Text = rm.GetString("TEXTO_ALTERNANCIA_btFechar");
            this.btRestaurarPadroes.Text = rm.GetString("TEXTO_ALTERNANCIA_btRestoreDefault");
            this.chkAll.Text = rm.GetString("TEXTO_ALTERNANCIA_chkAll");
        }
        private void popularCheckBoxAlternancia()
        {
            chklistboxAlternancias.Items.Clear();

            for (int i = 0; i < alternancia.listaAlternancias.Count; i++ )
            {
                chklistboxAlternancias.Items.Add(alternancia.listaAlternancias[i].NomeAlternancia);
                for (int j = 0; j < ListaAlternancias.Count; j++)
                {
                    if (alternancia.listaAlternancias[i].NomeAlternancia == ListaAlternancias[j].NomeAlternancia)
                        chklistboxAlternancias.SetItemChecked(chklistboxAlternancias.Items.Count - 1, true);
                }
            }

            if (chklistboxAlternancias.CheckedIndices.Count == chklistboxAlternancias.Items.Count)
                chkAll.Checked = true;
           
        }

        private void btFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormHabilitaAlternancia_Load(object sender, EventArgs e)
        {
            CarregarListaAlternanciaFachada();
        }

        private void btRestaurarPadroes_Click(object sender, EventArgs e)
        {
            CarregarListaPadrao();
        }

        private void CarregarListaAlternanciaFachada()
        {
            ListaAlternancias = fachada.CarregarAlternanciaPainel(controladorSelecionado, painelSelecionado);
            popularCheckBoxAlternancia();
        }

        private void CarregarListaPadrao()
        {
            for (int i = 0; i < chklistboxAlternancias.Items.Count; i++)
                if (i>=Util.Util.INDICE_ALTERNANCIA_PADRAO_PAINEL)
                    chklistboxAlternancias.SetItemChecked(i, false);
                else
                    chklistboxAlternancias.SetItemChecked(i, true);

            if (chklistboxAlternancias.CheckedIndices.Count == chklistboxAlternancias.Items.Count)
                chkAll.Checked = true;
            else
                chkAll.Checked = false;
        }

        private void btAplicar_Click(object sender, EventArgs e)
        {
            if (chklistboxAlternancias.CheckedIndices.Count>0)
            {
                int quantidadeAgendamentos = fachada.QuantidadeAgendamentosAlternancia(controladorSelecionado, painelSelecionado);
                if (quantidadeAgendamentos == 0)
                    Aplicar();
                else
                {
                    string msg;
                    if (quantidadeAgendamentos == 1)
                        msg = rm.GetString("TEXTO_ALTERNANCIA_MBOX_EXCLUIR_AGENDAMENTO1") + " " + quantidadeAgendamentos + " " + rm.GetString("TEXTO_ALTERNANCIA_MBOX_EXCLUIR_AGENDAMENTO2_S");
                    else
                        msg = rm.GetString("TEXTO_ALTERNANCIA_MBOX_EXCLUIR_AGENDAMENTO1") + " " + quantidadeAgendamentos + " " + rm.GetString("TEXTO_ALTERNANCIA_MBOX_EXCLUIR_AGENDAMENTO2_P");

                    if (DialogResult.Yes == MessageBox.Show(msg, rm.GetString("EDITAR_SIMULACAO_GBOX_ALTERNANCIA"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        fachada.ExcluirEventoAlternancia(controladorSelecionado, painelSelecionado);
                        Aplicar();
                    }
                }
            }
            else
            {
                MessageBox.Show(rm.GetString("TEXTO_ALTERNANCIA_MBOX"));
            }
        }


        private void chkAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chklistboxAlternancias.Items.Count; i++)
                chklistboxAlternancias.SetItemChecked(i, chkAll.Checked);

        }

        private void Aplicar()
        {
            ListaAlternancias.Clear();
            for (int i = 0; i < chklistboxAlternancias.CheckedIndices.Count; i++)
            {
                int indice = chklistboxAlternancias.CheckedIndices[i];
                ListaAlternancias.Add(new ItemAlternancia(alternancia.listaAlternancias[indice]));
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
