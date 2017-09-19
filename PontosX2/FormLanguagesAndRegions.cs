using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Controlador;
using System.Globalization;
using System.Resources;
using System.Reflection;
using Globalization;


namespace PontosX2
{
    public partial class FormLanguagesAndRegions : Form
    {
        #region Proprieadades
        //Lista de possíveis nomes para arquivos.(para facilitar a entrada de texto para o usuário.)
        List<String> ListaDeNomesParaArquivos = new List<String>();

        //========================================================================================//
        //regiões
        //========================================================================================//
        Util.Util.FormatoDataHora fdatahora = Util.Util.FormatoDataHora.FORMATO_24H;
        Util.Util.OpcaoAmPm_Ponto fopcaoAmPm_Ponto = Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
        Util.Util.UnidadeVelocidade uvelocidade = Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
        Util.Util.UnidadeTemperatura utemperatura = Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
        Util.Util.SeparadorDecimal sdecimal = Util.Util.SeparadorDecimal.VIRGULA;
        Util.Util.Moeda moeda = Util.Util.Moeda.MOEDA_REAL;
        String Nome = String.Empty;

        //========================================================================================//
        //idiomas
        //========================================================================================//
        
        //Lista as frases default (menu de frases)
        Persistencia.Arquivo_LPK alpkDefault;
        Persistencia.Arquivo_LPK alpkEditado;  

        int selectedIndex = 0;
        //========================================================================================//

        //Acesso à fachada
        Fachada fachada = Fachada.Instance;
        ResourceManager rm;
        #endregion

        #region Construtor
        public FormLanguagesAndRegions()
        {
            InitializeComponent();

            //Globalização
            rm = fachada.carregaIdioma();
            AplicaIdioma();
            
            alpkDefault = new Persistencia.Arquivo_LPK(rm);
            alpkEditado = new Persistencia.Arquivo_LPK(rm);

            //Popular DataHora
            PopularDataHora();

            //Popular Velocidade
            PopularVelocidade();

            //Popular Moeda
            PopularMoeda();

            //Popular Temperatura
            PopularTemperatura();

            //Popular Separador Decimal
            PopularDecimal();
        }
        #endregion

        #region Globalização

        private void AplicaIdioma()
        {
            this.tabPageRgn.Text = rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_TEXT");
            this.gBoxFormatoHora.Text = rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_TEXT");
            this.lblOpcaoAmPm_Ponto.Text = rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_OPCAO_AM_PM_PONTO");
            this.gBoxMoeda.Text = rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_MOEDA");
            this.gBoxSeparadorDec.Text = rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_SEPARADOR_DECIMAL");
            this.gBoxTemperatura.Text = rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_TEMPERATURA");
            this.gBoxVelocidade.Text = rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_VELOCIDADE");

            this.tabPageIdiomas.Text = rm.GetString("REGIOES_IDIOMAS_TAB_IDIOMAS_TEXT");
            this.buttonEditar.Text = rm.GetString("REGIOES_IDIOMAS_TAB_IDIOMAS_BTN_EDITAR");
            this.labelNovoTexto.Text = rm.GetString("REGIOES_IDIOMAS_TAB_IDIOMAS_NOVO_TEXTO");

            this.buttonCancelar.Text = rm.GetString("REGIOES_IDIOMAS_TAB_BTN_CANCELAR");
            this.buttonOk.Text = rm.GetString("REGIOES_IDIOMAS_TAB_BTN_OK");
            this.btRestaurar.Text = rm.GetString("REGIOES_IDIOMAS_TAB_BTN_RESTAURAR");

            System.Windows.Forms.ToolTip toolTipbtRestaurar = new System.Windows.Forms.ToolTip();
            toolTipbtRestaurar.SetToolTip(btRestaurar, rm.GetString("REGIOES_IDIOMAS_TAB_BTN_RESTAURAR_TOOLTIP"));

            this.Text = rm.GetString("REGIOES_IDIOMAS_TEXT");

        }

        #endregion

        #region REGIOES

        private void SalvaArquivoRGN()
        {
            Persistencia.Arquivo_RGN argn = new Persistencia.Arquivo_RGN();
            string idiomaPath="";

            argn.formatoDataHora = (byte)this.fdatahora;
            argn.opcaoAmPm_Ponto = (byte)this.fopcaoAmPm_Ponto;
            argn.unidadeVelocidade = (byte)this.uvelocidade;
            argn.unidadeTemperatura = (byte)this.utemperatura;
            argn.separadorDecimal = (this.sdecimal==Util.Util.SeparadorDecimal.VIRGULA?Encoding.ASCII.GetBytes(",")[0]:Encoding.ASCII.GetBytes(".")[0]);
            argn.moeda = (byte)this.moeda;
            if (cbArquivos.SelectedIndex == -1)
            {
                switch(fachada.GetIdioma())
                {
                    case Util.Util.Lingua.Espanhol:  idiomaPath = Util.Util.LPK_ES_ES; break;
                    case Util.Util.Lingua.Ingles:    idiomaPath = Util.Util.LPK_EN_US; break;
                    case Util.Util.Lingua.Portugues: idiomaPath = Util.Util.LPK_PT_BR; break;
                }
            }
            else
                idiomaPath = cbArquivos.SelectedItem.ToString();

            idiomaPath = Util.Util.TrataDiretorio(Util.Util.DIRETORIO_IDIOMAS + idiomaPath + Util.Util.ARQUIVO_EXT_LPK);
            argn.idiomaPath = Encoding.ASCII.GetBytes(idiomaPath.Substring(1).PadRight(64,'\0'));
            if (comboBoxNomeArquivoRGN.Text.Length>16)
                this.Nome = Util.Util.RemoveSpecialCharacters(comboBoxNomeArquivoRGN.Text.Substring(0,16),"_");
            else
                this.Nome = Util.Util.RemoveSpecialCharacters(comboBoxNomeArquivoRGN.Text, "_");
            argn.nome = Encoding.ASCII.GetBytes(this.Nome.PadRight(16, ' '));

            try
            {
                fachada.SalvarRGN(argn);
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_SALVAR"));
            }
            catch
            {
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_SALVAR_EXECAO"));
            }


        }
               

        #endregion

        #region IDIOMAS

        private void LinguaSelecionada()
        {
            switch((string)cbArquivos.SelectedItem)
            {
                case Util.Util.LPK_EN_US:
                    alpkDefault.nome = Util.Util.LPK_EN_US;
                    alpkDefault.rm = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    alpkEditado.nome = Util.Util.LPK_EN_US;
                    alpkEditado.rm = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.LPK_ES_ES:
                    alpkDefault.nome = Util.Util.LPK_ES_ES;
                    alpkDefault.rm = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    alpkEditado.nome = Util.Util.LPK_ES_ES;
                    alpkEditado.rm = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.LPK_PT_BR:
                    alpkDefault.nome = Util.Util.LPK_PT_BR;
                    alpkDefault.rm = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    alpkEditado.nome = Util.Util.LPK_PT_BR;
                    alpkEditado.rm = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
            }
        }

        //private void ListaArquivos()
        //{
        //    //Lista os arquivos existentes.
        //    cbArquivos.Items.Clear();
        //    ListaLPKs();

        //}

        /// <summary>
        /// Lista as disponibilidades de regiões.
        /// </summary>
        private void ListaArquivosLPK()
        {
            try
            {
                cbArquivos.Items.Clear();
                cbArquivos.Items.AddRange(fachada.ListaLPKs().ToArray());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SalvaArquivoLPK()
        {
            try
            {
                fachada.SalvarLPK(alpkEditado);
                alpkEditado.nome = alpkEditado.nome.TrimEnd('\0');
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_IDIOMAS_SALVAR"));
            }
            catch
            {
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_IDIOMAS_SALVAR_EXECAO"));
            }

        }
             

        private void listBoxMenuFrases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxEditaFrases.Items.Count == listBoxMenuFrases.Items.Count)
            {
                listBoxEditaFrases.SelectedIndex = listBoxMenuFrases.SelectedIndex;
                this.selectedIndex = listBoxEditaFrases.SelectedIndex;

            }
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            if (listBoxEditaFrases.SelectedItems.Count > 0 && listBoxMenuFrases.SelectedItems.Count > 0)
            {
                listBoxEditaFrases.Items[listBoxEditaFrases.SelectedIndex] = textBoxEditar.Text;
                this.alpkEditado.texto[listBoxEditaFrases.SelectedIndex] = (textBoxEditar.Text);
            }
        }

        private void listBoxEditaFrases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxEditaFrases.Items.Count == listBoxMenuFrases.Items.Count)
            {
                if (listBoxMenuFrases.SelectedIndex < 0)
                    listBoxMenuFrases.SelectedIndex = this.selectedIndex;
                textBoxEditar.Text = listBoxEditaFrases.Items[listBoxMenuFrases.SelectedIndex].ToString();
            }
        }


        private void tabPageIdiomas_Click(object sender, EventArgs e)
        {
        }

        private void cbArquivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Focused)
            {
                //setando o resource manager de acordo com a lingua selecionada
                LinguaSelecionada();

                //remontando a lista default
                ListaLpkDefault();

                //remontando a lista de frases editadas
                ListaLpkEditado();

                if (listBoxMenuFrases.SelectedItems.Count > 0)
                    listBoxEditaFrases.SelectedIndex = listBoxMenuFrases.SelectedIndex;
            }
        }

        private void ListaLpkDefault()
        {
            //verificando se existia itemselecionado
            int itemSelecionado = -1;
            if (listBoxMenuFrases.SelectedItems.Count > 0)
                itemSelecionado = listBoxMenuFrases.SelectedIndex;

            //Lista as frases default de acordo com o idioma escolhida pelo usuário
            listBoxMenuFrases.Items.Clear();
            alpkDefault.texto.Clear();
            alpkDefault.texto.AddRange(fachada.CarregarFrasesLpkDefault(alpkDefault));
            listBoxMenuFrases.Items.AddRange(alpkDefault.texto.ToArray());

            //reselecionando o item selecionado após troca de lingua
            if (itemSelecionado != -1)
                listBoxMenuFrases.SelectedIndex = itemSelecionado;
        }

        private void ListaLpkEditado()
        {
            alpkEditado.texto.Clear();
            alpkEditado.texto = fachada.ExibeLPK((string)cbArquivos.SelectedItem, alpkEditado);
            alpkEditado.nome = alpkEditado.nome.TrimEnd('\0');
            listBoxEditaFrases.Items.Clear();
            listBoxEditaFrases.Items.AddRange(this.alpkEditado.texto.ToArray());
        }



        #endregion

        #region Botoes

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (tabControlRGNeIdiomas.SelectedTab == tabPageRgn)
            {
                if (VerificarCamposRegiao())
                    this.SalvaArquivoRGN();
            }

            if (tabControlRGNeIdiomas.SelectedTab == tabPageIdiomas)
            {
                SalvaArquivoLPK();
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Boolean VerificarCamposRegiao()
        {
            bool verificou = true;

            if (comboBoxNomeArquivoRGN.Text=="")
            {
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_MBOX_NOME"));
                comboBoxNomeArquivoRGN.Select();
                return false;
            }

            if (cbHora.SelectedIndex == -1)
            {
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_MBOX_HORA"));
                cbHora.Select();
                return false;
            }

            if (cbVelocidade.SelectedIndex == -1)
            {
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_MBOX_VELOCIDADE"));
                cbVelocidade.Select();
                return false;
            }

            if (cbMoeda.SelectedIndex == -1)
            {
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_MBOX_MOEDA"));
                cbMoeda.Select();
                return false;
            }

            if (cbTemperatura.SelectedIndex == -1)
            {
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_MBOX_TEMPERATURA"));
                cbTemperatura.Select();
                return false;
            }

            if (cbDecimal.SelectedIndex == -1)
            {
                MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_REGIOES_MBOX_DECIMAL"));
                cbDecimal.Select();
                return false;
            }

            return verificou;
        }

        #endregion

        #region AlterarGUI

        private void cbHora_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbHora.SelectedIndex)
            {
                case 0: this.fdatahora = Util.Util.FormatoDataHora.FORMATO_24H;
                        this.fopcaoAmPm_Ponto = Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
                        rbAmPm.Visible = false;
                        rbPonto.Visible = false;
                        lblOpcaoAmPm_Ponto.Visible = false;
                        break;
                case 1: this.fdatahora = Util.Util.FormatoDataHora.FORMATO_AM_PM;
                        this.fopcaoAmPm_Ponto = Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
                        rbAmPm.Visible = true;
                        rbPonto.Visible = true;
                        lblOpcaoAmPm_Ponto.Visible = true;
                        rbPonto.Checked = true;                        
                    break;
            }
        }

        private void cbVelocidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbVelocidade.SelectedIndex)
            {
                case 0: this.uvelocidade = Util.Util.UnidadeVelocidade.UNIDADE_KMpH;
                    break;
                case 1: this.uvelocidade = Util.Util.UnidadeVelocidade.UNIDADE_MPH;
                    break;
            }

        }

        private void cbMoeda_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbMoeda.SelectedIndex)
            {
                case 0: this.moeda = Util.Util.Moeda.MOEDA_REAL;
                    break;
                case 1: this.moeda = Util.Util.Moeda.MOEDA_DOLAR;
                    break;
                case 2: this.moeda = Util.Util.Moeda.MOEDA_PESO;
                    break;
                case 3: this.moeda = Util.Util.Moeda.MOEDA_EURO;
                    break;
            }

        }

        private void cbTemperatura_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbTemperatura.SelectedIndex)
            {
                case 0: this.utemperatura = Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS;
                    break;
                case 1: this.utemperatura = Util.Util.UnidadeTemperatura.UNIDADE_FAHRENHEIT;
                    break;
            }

        }

        private void cbDecimal_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbDecimal.SelectedIndex)
            {
                case 0: this.sdecimal = Util.Util.SeparadorDecimal.VIRGULA;
                    break;
                case 1: this.sdecimal = Util.Util.SeparadorDecimal.PONTO;
                    break;
            }

        }

        private void GetCulture()
        {

            // get culture names
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                 string specName = "(none)";
                 try { specName = CultureInfo.CreateSpecificCulture(ci.Name).Name; } catch { }
                 //this.ListaDeNomesParaArquivos.Add(String.Format("{0,-12}{1,-12}{2}", ci.Name, specName, ci.EnglishName));
                 //this.ListaDeNomesParaArquivos.Add(ci.EnglishName + " - " + ci.Name); //mais legível!!!
                 string teste = Util.Util.RemoveSpecialCharacters(ci.EnglishName, "_");
                 if (teste.Substring(teste.Length - 1) == "_")
                     teste = teste.Remove(teste.Length - 1);
                 this.ListaDeNomesParaArquivos.Add(teste);
            }

            this.ListaDeNomesParaArquivos.Sort(); // sort by name

            //Insere nas combos. não está diretamente no primeiro laço por causa do sort;
            foreach(String arquivo in this.ListaDeNomesParaArquivos)
            {
                //this.comboBoxNomeArquivo.Items.Add(arquivo);
                this.comboBoxNomeArquivoRGN.Items.Add(arquivo);
            }

           // this.comboBoxNomeArquivo.Text = rm.GetString("REGIOES_IDIOMAS_COMBOS_ARQUIVOS");
            this.comboBoxNomeArquivoRGN.Text = rm.GetString("REGIOES_IDIOMAS_COMBOS_ARQUIVOS");
            //// write to console
            //Console.WriteLine("CULTURE SPEC.CULTURE ENGLISH NAME");
            //Console.WriteLine("--------------------------------------------------------------");
            //foreach (string str in list)
            // Console.WriteLine(str);
        }



        private void FormLanguagesAndRegions_Load(object sender, EventArgs e)
        {
            //carrega os nomes default para os arquivos.
            this.GetCulture();
        }

        private void PopularDataHora()
        {
            cbHora.Items.Clear();
            cbHora.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_DATAHORA_24H"));
            cbHora.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_DATAHORA_AMPM"));
        }

        private void PopularVelocidade()
        {
            cbVelocidade.Items.Clear();
            cbVelocidade.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_VELOCIDADE_KMH"));
            cbVelocidade.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_VELOCIDADE_MPH"));
        }

        private void PopularMoeda()
        {
            cbMoeda.Items.Clear();
            cbMoeda.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_MOEDA_REAL"));
            cbMoeda.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_MOEDA_DOLAR"));
            cbMoeda.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_MOEDA_PESO"));
            cbMoeda.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_MOEDA_EURO"));
        }

        private void PopularTemperatura()
        {
            cbTemperatura.Items.Clear();
            cbTemperatura.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_TEMPERATURA_CELSIUS"));
            cbTemperatura.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_TEMPERATURA_FAHRENHEIT"));
        }

        private void PopularDecimal()
        {
            cbDecimal.Items.Clear();
            cbDecimal.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_DECIMAL_VIRGULA"));
            cbDecimal.Items.Add(rm.GetString("REGIOES_IDIOMAS_COMBO_DECIMAL_PONTO"));
        }


        private void rbAmPm_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).ContainsFocus)
                if (rbAmPm.Checked)
                    fopcaoAmPm_Ponto = Util.Util.OpcaoAmPm_Ponto.EXIBIR_AM_PM;
        }

        private void rbPonto_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).ContainsFocus)
                if (rbPonto.Checked)
                    fopcaoAmPm_Ponto = Util.Util.OpcaoAmPm_Ponto.EXIBIR_PONTO;
        }

        #endregion

        private void btRestaurar_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_BTN_RESTAURAR_MBOX_CONFIRM"), rm.GetString("ARQUIVO_MBOX_TYPE"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            {
                try
                {
                    fachada.SalvarLPK(alpkDefault);
                    MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_BTN_RESTAURAR_MBOX"));

                    //setando o resource manager de acordo com a lingua selecionada
                    LinguaSelecionada();

                    //remontando a lista default
                    ListaLpkDefault();

                    //remontando a lista de frases editadas
                    ListaLpkEditado();

                    //selecionando o indice após a restuaração
                    listBoxEditaFrases.SelectedIndex = listBoxMenuFrases.SelectedIndex;
                    this.selectedIndex = listBoxEditaFrases.SelectedIndex;

                }
                catch
                {
                    MessageBox.Show(rm.GetString("REGIOES_IDIOMAS_TAB_BTN_RESTAURAR_MBOX_EXCEPTION"), "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void tabControlRGNeIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlRGNeIdiomas.SelectedTab == tabPageRgn)
            {
                btRestaurar.Visible = false;
            }

            if (tabControlRGNeIdiomas.SelectedTab == tabPageIdiomas)
            {
                ListaArquivosLPK();

                switch (fachada.GetIdioma())
                {
                    case Util.Util.Lingua.Ingles:
                        cbArquivos.SelectedItem = Util.Util.LPK_EN_US;
                        break;
                    case Util.Util.Lingua.Portugues:
                        cbArquivos.SelectedItem = Util.Util.LPK_PT_BR;
                        break;
                    case Util.Util.Lingua.Espanhol:
                        cbArquivos.SelectedItem = Util.Util.LPK_ES_ES;
                        break;
                }

                //setando o resource manager e o nome de alpkdefault e alpkeditado
                LinguaSelecionada();

                //remontando a lista default
                ListaLpkDefault();

                //remontando a lista de frases editadas
                ListaLpkEditado();

                btRestaurar.Visible = true;
            }

        }

    }
}
