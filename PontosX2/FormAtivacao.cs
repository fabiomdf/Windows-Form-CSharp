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
using System.Text.RegularExpressions;
using System.Net.Http;

namespace PontosX2
{
    public partial class FormAtivacao : Form
    {
        Fachada fachada = Fachada.Instance;
        ResourceManager rm;

        public FormAtivacao(bool exibeBotaoGeradorChave = false)
        {
            InitializeComponent();
            rm = fachada.carregaIdioma();
            btGerar.Visible = exibeBotaoGeradorChave;
        }

        private void buttonValidar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos("Validar"))
            {

                string chaveInformada = textBoxChave1.Text + textBoxChave2.Text + textBoxChave3.Text + textBoxChave4.Text;

                if (!fachada.ValidarChaveAtivacao(textBoxEmpresa.Text, Application.ProductName, Application.ProductVersion.Substring(0, 2), chaveInformada))
                    MessageBox.Show(rm.GetString("ATIVACAO_CHAVE_INVALIDA"));
                else
                {
                    fachada.GerarArquivoAtivacao(textBoxEmpresa.Text, Application.ProductName, Application.ProductVersion.Substring(0, 2), chaveInformada);
                    fachada.SetLicenciadoPara(textBoxEmpresa.Text);
                    DialogResult = DialogResult.OK;
                }
            }

        }

        private void FormAtivacao_Load(object sender, EventArgs e)
        {
            CarregarIdioma();
            PosicionarComponentes();
        }

        private void PosicionarComponentes()
        {
            this.Height = 290;
            this.Width = 555;

            groupBoxCadastro.Location = new Point(118,61);
            groupBoxValidarAtivacao.Location = new Point(118, 61);
            gboxChave3dias.Location = new Point(118, 61);

            gboxExibido();

            radioButtonSim.Location = new Point(labelTemChaveAtivacao.Location.X + labelTemChaveAtivacao.Width + 10, radioButtonSim.Location.Y);
            radioButtonNao.Location = new Point(radioButtonSim.Location.X + radioButtonSim.Width + 10, radioButtonNao.Location.Y);
        }

        private void CarregarIdioma()
        {
            Version versionAplicacao = new Version(Application.ProductVersion);

            this.Text = rm.GetString("ATIVACAO_CAPTION");

            lbTexto.Text = rm.GetString("ATIVACAO_TEXTO");
            lbEmpresa.Text = rm.GetString("ATIVACAO_EMPRESA");
            lbChave.Text = rm.GetString("ATIVACAO_CHAVE");
            labelTemChaveAtivacao.Text = rm.GetString("ATIVACAO_TEMCHAVEATIVACAO");
            lbVersao.Text = rm.GetString("ATIVACAO_VERSAO") + " " + versionAplicacao.ToString(3);
            labelCadastroEmpresa.Text = rm.GetString("ATIVACAO_EMPRESA");
            labelCadastroContato.Text = rm.GetString("ATIVACAO_CONTATO");
            labelCadastroTelefone.Text = rm.GetString("ATIVACAO_TELEFONE");
            labelCadastroEmail.Text = rm.GetString("ATIVACAO_EMAIL");
            
            btValidar.Text = rm.GetString("ATIVACAO_BUTTON_APLICAR");
            btContinuar.Text = rm.GetString("ATIVACAO_BUTTON_CONTINUAR");
            btEnviar.Text = rm.GetString("ATIVACAO_BUTTON_ENVIAR");

            buttonCancelarEnvio.Text = rm.GetString("ATIVACAO_BUTTON_CANCELAR");
            btCancelar.Text = rm.GetString("ATIVACAO_BUTTON_CANCELAR");            
            btCancelar3dias.Text = rm.GetString("ATIVACAO_BUTTON_CANCELAR");

            radioButtonSim.Text = rm.GetString("ATIVACAO_SIM");
            radioButtonNao.Text = rm.GetString("ATIVACAO_NAO");

        }

        private void CursorWait()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.UseWaitCursor = true;
        }

        private void CursorDefault()
        {
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }

        private void btGerar_Click(object sender, EventArgs e)
        {
            string chaveGerada = fachada.GerarChaveAtivacao(textBoxEmpresa.Text, Application.ProductName ,Application.ProductVersion.Substring(0, 2));

            textBoxChave1.Text = chaveGerada.Substring(0, 16);
            textBoxChave2.Text = chaveGerada.Substring(16, 16);
            textBoxChave3.Text = chaveGerada.Substring(32, 16);
            textBoxChave4.Text = chaveGerada.Substring(48, 16);
        }

        private void btEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos("Cadastro"))
                {
                    CursorWait();

                    fachada.CadastrarChaveAtivacao(tbCadastroEmpresa.Text, Application.ProductVersion.Substring(0, 2), Application.ProductName, tbCadastroNomeContato.Text, cboxDDI.SelectedItem.ToString() + " " +tboxDDD.Text + " " + tbCadastroTelefone.Text, tbCadastroEmail.Text);
                    fachada.CriarArquivoTrial(tbCadastroEmpresa.Text, Application.ProductVersion.Substring(0, 2), Application.ProductName, tbCadastroNomeContato.Text, cboxDDI.SelectedItem.ToString() + " " + tboxDDD.Text + " " + tbCadastroTelefone.Text, tbCadastroEmail.Text);

                    CursorDefault();

                    MessageBox.Show(rm.GetString("ATIVACAO_BUTTON_ENVIAR_SUCESSO") + " " + Util.Util.DIAS_CHAVE_TRIAL + " " + rm.GetString("ATIVACAO_MENSAGEM_DIAS"));
                    gboxExibido();
                }
            }
            catch (HttpRequestException ex)
            {
                fachada.CriarArquivoTrial(tbCadastroEmpresa.Text, Application.ProductVersion.Substring(0, 2), Application.ProductName, tbCadastroNomeContato.Text, cboxDDI.SelectedItem.ToString() + " " + tboxDDD.Text + " " + tbCadastroTelefone.Text, tbCadastroEmail.Text);

                CursorDefault();

                MessageBox.Show(rm.GetString("ATIVACAO_BUTTON_ENVIAR_CATCH") + " " + Util.Util.DIAS_CHAVE_TRIAL + " " + rm.GetString("ATIVACAO_MENSAGEM_DIAS"));           
                gboxExibido();
            }
            catch (EntryPointNotFoundException ex)
            {
                fachada.CriarArquivoTrial(tbCadastroEmpresa.Text, Application.ProductVersion.Substring(0, 2), Application.ProductName, tbCadastroNomeContato.Text, cboxDDI.SelectedItem.ToString() + " " + tboxDDD.Text + " " + tbCadastroTelefone.Text, tbCadastroEmail.Text);

                CursorDefault();

                MessageBox.Show(rm.GetString("ATIVACAO_BUTTON_ENVIAR_CATCH_EMPRESA") + " " + Util.Util.DIAS_CHAVE_TRIAL + " " + rm.GetString("ATIVACAO_MENSAGEM_DIAS"));
                gboxExibido();
            }

        }

        private bool ValidarCampos(string opcao)
        {
            switch (opcao)
            {
                case "Cadastro":
                    if (tbCadastroEmpresa.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_EMPRESA"));
                        tbCadastroEmpresa.Focus();
                        return false;
                    }
                    if (tbCadastroNomeContato.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_CONTATO"));
                        tbCadastroNomeContato.Focus();
                        return false;
                    }
                    if (tboxDDD.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_DDD"));
                        tboxDDD.Focus();
                        return false;
                    }
                    if (tbCadastroTelefone.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_TELEFONE"));
                        tbCadastroTelefone.Focus();
                        return false;
                    }
                    if (tbCadastroEmail.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_EMAIL_VAZIO"));
                        tbCadastroEmail.Focus();
                        return false;
                    }
                    if (!validarEmail(tbCadastroEmail.Text))
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_EMAIL"));
                        tbCadastroEmail.Focus();
                        return false;
                    }

                    break;

                case "Validar":
                    if (textBoxEmpresa.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_EMPRESA"));
                        textBoxEmpresa.Focus();
                        return false;
                    }
                    if (textBoxChave1.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_CHAVE"));
                        textBoxChave1.Focus();
                        return false;
                    }
                    if (textBoxChave2.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_CHAVE"));
                        textBoxChave2.Focus();
                        return false;
                    }
                    if (textBoxChave3.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_CHAVE"));
                        textBoxChave3.Focus();
                        return false;
                    }
                    if (textBoxChave4.Text == "")
                    {
                        MessageBox.Show(rm.GetString("ATIVACAO_CADASTRO_VALIDACAO_CHAVE"));
                        textBoxChave4.Focus();
                        return false;
                    }
                    break;
            }

            return true;
        }

        private void radioButtonSim_CheckedChanged(object sender, EventArgs e)
        {
            gboxExibido();
        }

        private void radioButtonNao_CheckedChanged(object sender, EventArgs e)
        {
            gboxExibido();
        }

       
        

        private void gboxExibido()
        {

            if (radioButtonSim.Checked)
            {
                groupBoxValidarAtivacao.Visible = true;
                gboxChave3dias.Visible = false;
                groupBoxCadastro.Visible = false;

                lbTexto.Text = rm.GetString("ATIVACAO_TEXTO_ATIVACAO");
            }
            else
            {
                if (fachada.arquivoTrialCriado())
                {
                    gboxChave3dias.Visible = true;
                    groupBoxCadastro.Visible = false;
                    groupBoxValidarAtivacao.Visible = false;

                    lbTexto.Text = "";
                    if (fachada.SituacaoChave() == Util.Util.ValidacaoAtivacao.EmAtivacao)
                    {
                        int dias = fachada.DiasExpirarChaveTrial();
                        if (dias > 1)
                            lbValidade.Text = rm.GetString("ATIVACAO_PERIODO_AVALIACAO") + " " + dias + " " + rm.GetString("ATIVACAO_MENSAGEM_DIAS");
                        else
                            lbValidade.Text = rm.GetString("ATIVACAO_PERIODO_AVALIACAO") + " " + dias + " " + rm.GetString("ATIVACAO_MENSAGEM_DIA");
                    }
                    else
                    {
                        string[] msg = rm.GetString("ATIVACAO_PERIODO_AVALIACAO_EXPIRADO").Split('.');
                        lbValidade.Text = msg[0] + ".\n"+ msg[1] + ".";
                        
                        btContinuar.Visible = false;
                    }

                }
                else
                {                    
                    groupBoxCadastro.Visible = true;
                    gboxChave3dias.Visible = false;
                    groupBoxValidarAtivacao.Visible = false;

                    lbTexto.Text = rm.GetString("ATIVACAO_TEXTO_CADASTRO");

                    switch (fachada.GetIdiomaFachada())
                    {
                        case Util.Util.Lingua.Ingles:
                            cboxDDI.SelectedIndex = 208;
                            break;

                        case Util.Util.Lingua.Portugues:
                            cboxDDI.SelectedIndex = 28;
                            break;

                        case Util.Util.Lingua.Espanhol:
                            cboxDDI.SelectedIndex = 184;
                            break;

                        case Util.Util.Lingua.Frances:
                            cboxDDI.SelectedIndex = 68;
                            break;

                        case Util.Util.Lingua.Italiano:
                            cboxDDI.SelectedIndex = 96;
                            break;

                        default:
                            cboxDDI.SelectedIndex = 208;
                            break;
                    }
                }
                    


            }

            posicionarImagem();
        }


        private bool validarEmail(string email)
        {

            Regex regExpEmail = new Regex("^[A-Za-z0-9](([_.-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([.-]?[a-zA-Z0-9]+)*)([.][A-Za-z]{2,4})$");
            Match match = regExpEmail.Match(email);

            if (match.Success)
                return true;
            else
                return false;
        }

        private void posicionarImagem()
        {
            int posicao = 0;
            if (groupBoxValidarAtivacao.Visible)
                posicao = groupBoxValidarAtivacao.Height / 2 + groupBoxValidarAtivacao.Location.Y;

            if (groupBoxCadastro.Visible)
                posicao = groupBoxCadastro.Height / 2 + groupBoxCadastro.Location.Y;

            if (gboxChave3dias.Visible)
                posicao = gboxChave3dias.Height / 2 + gboxChave3dias.Location.Y;

            pboxChave.Location = new Point(pboxChave.Location.X, (posicao - pboxChave.Height/2));

        }


        private void btContinuar_Click(object sender, EventArgs e)
        {
            // this.Close();
            DialogResult = DialogResult.OK;
        }

        private void FormAtivacao_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                DialogResult = DialogResult.Cancel;
        }

        private void cboxDDI_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index > -1 && imageListBandeiras.Images.Count >= e.Index)
            {
                //for image
                e.Graphics.DrawImage(imageListBandeiras.Images[e.Index], new PointF(e.Bounds.X, e.Bounds.Y));
                // for text
                e.Graphics.DrawString(cboxDDI.Items[e.Index].ToString(), cboxDDI.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 16, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height));
            }
        }

        private void tboxDDD_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbCadastroTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBoxChave1_TextChanged(object sender, EventArgs e)
        {
            String temporaria = textBoxChave1.Text;

            if (temporaria.Length >= 16)
            {
                textBoxChave1.Text = temporaria.Substring(0, 16);
                textBoxChave2.Focus();
                textBoxChave2.Select(0, 0);
            }

            if (temporaria.Length >= 32)
            {
                textBoxChave2.Text = temporaria.Substring(16, 16);
                textBoxChave3.Focus();
                textBoxChave3.Select(0, 0);
            }
            if (temporaria.Length >= 48)
            {
                textBoxChave3.Text = temporaria.Substring(32, 16);
                textBoxChave4.Focus();
                textBoxChave4.Select(0, 0);
            }
            if (temporaria.Length >= 64)
            {
                textBoxChave4.Text = temporaria.Substring(48, 16);
                btValidar.Focus();
            }

        }

        private void textBoxChave2_TextChanged(object sender, EventArgs e)
        {
            String temporaria = textBoxChave2.Text;

            if (temporaria.Length >= 16)
            {
                textBoxChave2.Text = temporaria.Substring(0, 16);
                textBoxChave3.Focus();
                textBoxChave3.Select(0, 0);
            }
            if (temporaria.Length >= 32)
            {
                textBoxChave3.Text = temporaria.Substring(16, 16);
                textBoxChave4.Focus();
                textBoxChave4.Select(0, 0);
            }
            if (temporaria.Length >= 48)
            {
                textBoxChave4.Text = temporaria.Substring(32, 16);
                btValidar.Focus();
            }
        }

        private void textBoxChave3_TextChanged(object sender, EventArgs e)
        {
            String temporaria = textBoxChave3.Text;

            if (temporaria.Length >= 16)
            {
                textBoxChave3.Text = temporaria.Substring(0, 16);
                textBoxChave4.Focus();
                textBoxChave4.Select(0, 0);
            }
            if (temporaria.Length >= 32)
            {
                textBoxChave4.Text = temporaria.Substring(16, 16);
                btValidar.Focus();
            }

        }

        private void textBoxChave4_TextChanged(object sender, EventArgs e)
        {
            String temporaria = textBoxChave4.Text;
            if (temporaria.Length >= 16)
            {
                textBoxChave4.Text = temporaria.Substring(0, 16);
                btValidar.Focus();
            }
        }


    }
}
