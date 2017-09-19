using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Persistencia;
using Controlador;
using System.Resources;
using System.IO;
using Util;

namespace PontosX2
{
    public partial class FormPresentationSequence : Form
    {


        #region Propriedades e Atributos

        //Acesso à fachada
        Fachada fachada = Fachada.Instance;
        ResourceManager rm;

        Arquivo_ALT alternancia;
        ItemAlternancia itemAlternancia;

        #endregion

        #region Construtor

        public FormPresentationSequence()
        {
            InitializeComponent();

            //Globalização
            rm = fachada.carregaIdioma();
            AplicaIdioma();

            popularTiposExibicao();

            itemAlternancia = new ItemAlternancia();
        }

        #endregion

        #region Globalização

        private void AplicaIdioma()
        {
            this.Text = rm.GetString("CRIAR_ALTERNANCIA_FORM_LABEL");
            this.gboxAlternancia.Text = rm.GetString("CRIAR_ALTERNANCIA_GBOX_LABEL");
            this.tboxFileName.Text = rm.GetString("CRIAR_ALTERNANCIA_TBOX_FILENAME");
            this.btAdicionar.Text = rm.GetString("CRIAR_ALTERNANCIA_BUTTON_ADICIONAR");
            this.btRemover.Text = rm.GetString("CRIAR_ALTERNANCIA_BUTTON_REMOVER");
            this.btCancelar.Text = rm.GetString("CRIAR_ALTERNANCIA_BUTTON_CANCELAR");
            this.btAplicar.Text = rm.GetString("CRIAR_ALTERNANCIA_BUTTON_APLICAR");
            this.btRestaurar.Text = rm.GetString("CRIAR_ALTERNANCIA_BUTTON_RESTAURAR");

            System.Windows.Forms.ToolTip toolTipbtRestaurar = new System.Windows.Forms.ToolTip();
            toolTipbtRestaurar.SetToolTip(btRestaurar, rm.GetString("CRIAR_ALTERNANCIA_BUTTON_RESTAURAR_TOOLTIP"));

        }

        #endregion

        #region Alteração GUI

        private void lboxTpExibicaoSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListBox)sender).Focused)
                lboxTpExibicao.SelectedItems.Clear();
        }

        private void lboxTpExibicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListBox)sender).Focused)
                lboxTpExibicaoSel.SelectedItems.Clear();
        }

        private void btRestaurar_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(rm.GetString("CRIAR_ALTERNANCIA_BUTTON_RESTAURAR_MBOX_CONFIRM"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            {
                try
                {
                    fachada.RestaurarArquivoAlternancia();
                    MessageBox.Show(rm.GetString("CRIAR_ALTERNANCIA_BUTTON_RESTAURAR_MBOX"));
                }
                catch
                {
                    MessageBox.Show(rm.GetString("CRIAR_ALTERNANCIA_BUTTON_RESTAURAR_MBOX_EXCEPTION"), "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void popularTiposExibicao()
        {
            lboxTpExibicao.Items.Clear();
            lboxTpExibicao.Items.AddRange(fachada.CarregarTiposExibicao().ToArray());
        }

        private void btAplicar_Click(object sender, EventArgs e)
        {
            if (tboxFileName.Text!="")
            {
                if (lboxTpExibicaoSel.Items.Count>0)
                { 
                    salvarAlternancia();
                    limparCampos();                    
                }
                else
                {
                    MessageBox.Show(rm.GetString("CRIAR_ALTERNANCIA_MBOX_UM_TIPO_EXIBICAO"));
                }
            }
            else
            {
                MessageBox.Show(rm.GetString("CRIAR_ALTERNANCIA_MBOX_NOME_ALTERNANCIA"));
                tboxFileName.Select();
            }
        }

        private void limparCampos()
        {
            lboxTpExibicao.SelectedItems.Clear();
            tboxFileName.Text = this.tboxFileName.Text = rm.GetString("CRIAR_ALTERNANCIA_TBOX_FILENAME");
            lboxTpExibicaoSel.Items.Clear();
            itemAlternancia.Exibicoes.Clear();
            itemAlternancia.NomeAlternancia = "";
            tboxFileName.Select();
        }

        private void salvarAlternancia()
        {
            //Carregando arquivo de alternancia
            alternancia = fachada.CarregarAlternancia();

            itemAlternancia.NomeAlternancia = tboxFileName.Text;

            alternancia.listaAlternancias.Add(itemAlternancia);
            
            //Salvando arquivo de alternancia
            fachada.SalvarAlternancia(alternancia);

            MessageBox.Show(rm.GetString("CRIAR_ALTERNANCIA_MBOX_SALVAR"));
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormPresentationSequence_Shown(object sender, EventArgs e)
        {
            tboxFileName.Select();
        }

        private void btAdicionar_Click(object sender, EventArgs e)
        {
            if (lboxTpExibicao.SelectedItems.Count > 0)
            {
                if (lboxTpExibicaoSel.Items.Count==7)
                {
                    MessageBox.Show(rm.GetString("CRIAR_ALTERNANCIA_MBOX_SETE_TIPO_EXIBICAO"));
                    lboxTpExibicao.SelectedItems.Clear();
                }
                else
                { 
                    lboxTpExibicaoSel.Items.Add(lboxTpExibicao.Items[lboxTpExibicao.SelectedIndex].ToString());
                    itemAlternancia.Exibicoes.Add((TipoExibicao)lboxTpExibicao.SelectedIndex);
                }
            }
        }

        private void btRemover_Click(object sender, EventArgs e)
        {
            if (lboxTpExibicaoSel.SelectedItems.Count > 0)
            {
                int itemSelecionado = lboxTpExibicaoSel.SelectedIndex;
                itemAlternancia.Exibicoes.RemoveAt(itemSelecionado);
                lboxTpExibicaoSel.Items.RemoveAt(itemSelecionado);

                if (lboxTpExibicaoSel.Items.Count > itemSelecionado)
                {
                    lboxTpExibicaoSel.SelectedIndex = itemSelecionado;
                }
                else
                    lboxTpExibicaoSel.SelectedIndex = lboxTpExibicaoSel.Items.Count - 1;
                
            }
        }

        #endregion


    }
}
