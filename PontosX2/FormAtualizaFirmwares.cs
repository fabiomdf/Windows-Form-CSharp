using Controlador;
using Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PontosX2
{
    public partial class FormAtualizaFirmwares : Form
    {
        #region Constantes

        private const int COLUNA_PRODUTO = 0;
        private const int COLUNA_VERSAO = 1;
        private const int COLUNA_TIPO = 2;
        private const int COLUNA_DIRETORIO = 3;
        private const int COLUNA_VERSAO_ARQUIVO = 4;
        private const int COLUNA_ELLIPSIS = 5;
        private const int COLUNA_UPDATES = 6;
        private const int COLUNA_PERCENTUAL = 7;

        #endregion Constantes

        #region Propriedades

        ResourceManager rm;

        Fachada fachada;

        List<Arquivo_FIR.IdentificacaoProduto> lista = new List<Arquivo_FIR.IdentificacaoProduto>();

        List<WebClient> webList;

        int indiceLinhaDownload = 0;

        #endregion Propriedades

        #region Construtor
        public FormAtualizaFirmwares()
        {
            InitializeComponent();
        }
        #endregion Construtor

        #region Eventos do Form

        private void dataGridViewAtualizarFirmware_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == COLUNA_ELLIPSIS)
            {
                string nomeArquivo = String.Empty;
                if (e.RowIndex == 0)
                    return;
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    openFileDialog.Filter = rm.GetString("FILTRO_FIRMWARES");
                    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        nomeArquivo = openFileDialog.FileName;
                        Arquivo_FIR.IdentificacaoProduto id = fachada.IdentificarProduto(nomeArquivo);
                        if (id.nomeProduto == (dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_PRODUTO].Value as string))
                        {
                            if (id.versao.CompareTo(new Version(dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_VERSAO].Value.ToString())) >= 0)
                            {
                                dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_DIRETORIO].Value = nomeArquivo;
                                dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_VERSAO_ARQUIVO].Value = id.versao.ToString();
                            }
                            else
                            {
                                MessageBox.Show(this, rm.GetString("UPDATE_FIRMWARE_VERSION_ERROR") + id.versao + ")", rm.GetString("MAIN_MENU_MBOX_OPEN_ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if ((id.nomeProduto.Equals(Util.Util.NOME_FIRMWARE_TURBUS)) && (dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_PRODUTO].Value as string).Equals(Util.Util.NOME_FIRMWARE_CONTROLADOR))
                        {
                            dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_PRODUTO].Value = id.nomeProduto;
                            dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_VERSAO].Value = id.versao.ToString();
                            dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_DIRETORIO].Value = nomeArquivo;
                            dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_VERSAO_ARQUIVO].Value = id.versao.ToString();
                        }
                        else
                        {
                            MessageBox.Show(this, rm.GetString("UPDATE_FIRMWARE_PRODUCT_ERROR") + id.nomeProduto + ")", rm.GetString("MAIN_MENU_MBOX_OPEN_ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch
                {

                }
            }
            else if (e.ColumnIndex == COLUNA_UPDATES)
            {
                indiceLinhaDownload = e.RowIndex;
                string produto = dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_PRODUTO].Value.ToString();
                Version versao = new Version(dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_VERSAO].Value.ToString());
                string sw_fw = (produto == Application.ProductName) ? "SW" : "FW";
                string familia = versao.Major.ToString();

                string url = fachada.VerificarAtualizarSistemas(familia, produto, sw_fw, versao.ToString());

                

                string filename = "\\" + produto + "_" + familia + "_" + sw_fw + "_" + versao.ToString().Replace(".", "_");

                if (!String.IsNullOrEmpty(url))
                {
                    dataGridViewAtualizarFirmware.Rows[e.RowIndex].Cells[COLUNA_DIRETORIO].Value = url;
                    Uri uri = new Uri(url);
                    
                    webList[indiceLinhaDownload].DownloadFileAsync(new Uri(url), Fachada.diretorio_downloads + "\\" + filename + Path.GetExtension(uri.PathAndQuery));
                }
                else
                {
                    dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_PERCENTUAL].Value = rm.GetString("UPDATE_FIRMWARE_MSG_ATUALIZADO");
                }            
                
            }
        }
        private void quandoDownloadCompleto(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            indiceLinhaDownload = webList.IndexOf(sender as WebClient);

            dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_PERCENTUAL].Value =  "100 % Completed";
            if (!String.IsNullOrEmpty(dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_DIRETORIO].Value.ToString()))
            {
                Uri uri = new Uri(dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_DIRETORIO].Value.ToString());
                if (uri.PathAndQuery.EndsWith(".msi"))
                {
                    string produto = dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_PRODUTO].Value.ToString();
                    Version versao = new Version(dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_VERSAO].Value.ToString());
                    string sw_fw = (produto == Application.ProductName) ? "SW" : "FW";
                    string familia = versao.Major.ToString();

                    string filename = "\\" + produto + "_" + familia + "_" + sw_fw + "_" + versao.ToString().Replace(".", "_");

                    Process.Start(Fachada.diretorio_downloads + "\\" + filename + Path.GetExtension(uri.PathAndQuery));

                    Application.Exit();
                }
                else if (uri.PathAndQuery.EndsWith(".fir"))
                {
                    // Copiar para firmware
                    string[] arquivos = Directory.GetFiles(Fachada.diretorio_downloads, "*.fir");
                    foreach (string arquivo in arquivos)
                    {
                        if (arquivo.Contains("ControladorPontos"))
                        {
                            File.Copy(arquivo, Fachada.diretorio_fir + "\\firmware.fir", true);
                        }
                        else if (arquivo.Contains("PainelMultilinhas"))
                        {
                            File.Copy(arquivo, Fachada.diretorio_fir + "\\multilin.fir", true);
                        }
                        else if (arquivo.Contains("PainelMultplex2vias"))
                        {
                            File.Copy(arquivo, Fachada.diretorio_fir + "\\mux2vias.fir", true);
                        }
                        else if (arquivo.Contains("PainelMultiplex2x13"))
                        {
                            File.Copy(arquivo, Fachada.diretorio_fir + "\\mux_2x13.fir", true);
                        }
                        else if (arquivo.Contains("PainelMultiplex2x8"))
                        {
                            File.Copy(arquivo, Fachada.diretorio_fir + "\\mux_2x8.fir", true);
                        }
                        else if (arquivo.Contains("PainelPontos"))
                        {
                            File.Copy(arquivo, Fachada.diretorio_fir + "\\painel.fir", true);
                        }
                        else if (arquivo.Contains("ControladorTurbus"))
                        {
                            File.Copy(arquivo, Fachada.diretorio_fir + "\\firmware.fir", true);
                        }
                        File.Delete(arquivo);
                    }
                    


                }

                
            }
        }
        private void quandoProgressoDownloadMudar(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            indiceLinhaDownload = webList.IndexOf(sender as WebClient);

            dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_PERCENTUAL].Value = e.ProgressPercentage + " % ";
        }
        private void FormAtualizaFirmwares_Load(object sender, EventArgs e)
        {
            fachada = Fachada.Instance;

            rm = fachada.carregaIdioma();
            dataGridViewAtualizarFirmware.Rows.Clear();
            lista = fachada.CarregarListaFirmwares();

            //this.Text = rm.GetString("UPDATE_FIRMWARE_FORM_TITLE");
            this.Text = rm.GetString("MAIN_MENU_HELP_VERIFICAR_ATUALIZACAO");

            btExportar.Text = rm.GetString("UPDATE_FIRMWARE_BUTTON_EXPORTAR");
            btAplicar.Text = rm.GetString("UPDATE_FIRMWARE_BUTTON_APLICAR");
            btCancelar.Text = rm.GetString("UPDATE_FIRMWARE_BUTTON_CANCELAR");

            buttonDownloadAll.Text = rm.GetString("UPDATE_FIRMWARE_DOWNLOAD_ALL");
            dataGridViewAtualizarFirmware.Columns[COLUNA_PRODUTO].HeaderText = rm.GetString("UPDATE_FIRMWARE_HEADER_PRODUTO");
            dataGridViewAtualizarFirmware.Columns[COLUNA_VERSAO].HeaderText = rm.GetString("UPDATE_FIRMWARE_HEADER_LOCAL");
            dataGridViewAtualizarFirmware.Columns[COLUNA_DIRETORIO].HeaderText = rm.GetString("UPDATE_FIRMWARE_HEADER_DIRETORIO");
            dataGridViewAtualizarFirmware.Columns[COLUNA_VERSAO_ARQUIVO].HeaderText = rm.GetString("UPDATE_FIRMWARE_HEADER_ARQUIVO");


            dataGridViewAtualizarFirmware.Columns[COLUNA_PERCENTUAL].HeaderText = rm.GetString("UPDATE_FIRMWARE_HEADER_PERCENTUAL");
            dataGridViewAtualizarFirmware.Columns[COLUNA_TIPO].HeaderText = rm.GetString("UPDATE_FIRMWARE_HEADER_TIPO");             

            // Alterando o alinhamento das colunas de versão.
            this.dataGridViewAtualizarFirmware.Columns[COLUNA_VERSAO].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewAtualizarFirmware.Columns[COLUNA_VERSAO_ARQUIVO].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            webList = new List<WebClient>();

            DataGridViewRow linha = (DataGridViewRow)dataGridViewAtualizarFirmware.Rows[0].Clone();

            linha.Cells[COLUNA_PRODUTO].Value = Application.ProductName;
            linha.Cells[COLUNA_VERSAO].Value = Application.ProductVersion;
            linha.Cells[COLUNA_TIPO].Value = "Software";
            linha.Cells[COLUNA_ELLIPSIS].Value = "...";            
            linha.Cells[COLUNA_UPDATES].Value = rm.GetString("UPDATE_FIRMWARE_HEADER_CHECK_UPDATES");
            dataGridViewAtualizarFirmware.Rows.Add(linha);

            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(quandoDownloadCompleto);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(quandoProgressoDownloadMudar);
            webList.Add(webClient);

            for (int i = 0; i < lista.Count; i++)
            {
                linha = (DataGridViewRow)dataGridViewAtualizarFirmware.Rows[0].Clone();                
                             
                linha.Cells[COLUNA_PRODUTO].Value = lista[i].nomeProduto;
                linha.Cells[COLUNA_VERSAO].Value = lista[i].versao.ToString();
                linha.Cells[COLUNA_TIPO].Value = "Firmware";
                linha.Cells[COLUNA_ELLIPSIS].Value = "...";
                linha.Cells[COLUNA_UPDATES].Value = rm.GetString("UPDATE_FIRMWARE_HEADER_CHECK_UPDATES");
                dataGridViewAtualizarFirmware.Rows.Add(linha);

                webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(quandoDownloadCompleto);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(quandoProgressoDownloadMudar);
                webList.Add(webClient);
            }            
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 1; i < dataGridViewAtualizarFirmware.Rows.Count; i++)
                {
                    if ((null != dataGridViewAtualizarFirmware.Rows[i].Cells[COLUNA_DIRETORIO].Value) && (!String.IsNullOrEmpty(dataGridViewAtualizarFirmware.Rows[i].Cells[COLUNA_DIRETORIO].Value.ToString())))
                    {
                        string origem = @"" + dataGridViewAtualizarFirmware.Rows[i].Cells[COLUNA_DIRETORIO].Value.ToString();
                        // Abrir a origem e verificar se é arquivo da TURBUS
                        Arquivo_FIR af = new Arquivo_FIR();
                        Arquivo_FIR.IdentificacaoProduto produto = af.IdentificarProduto(origem);
                        if (produto.nomeProduto.Contains(Util.Util.NOME_FIRMWARE_TURBUS))
                        {
                            fachada.CopiarFirmwareTurbusParaProgramData(origem);
                        }
                        else
                        {
                            fachada.CopiarFirmwareParaProgramData(origem);
                        }                        

                        
                    }
                }

                MessageBox.Show(this, rm.GetString("UPDATE_FIRMWARE_UPDATE_SUCCESS"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this, rm.GetString("UPDATE_FIRMWARE_UPDATE_ERROR"), rm.GetString("MAIN_MENU_MBOX_OPEN_ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btExportar_Click(object sender, EventArgs e)
        {
            try
            {
                // O usuário escolhe o destino que provavelmente será um Pendrive
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

                if (Util.Util.IdentificarDispositivoUSB() == string.Empty)
                    folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                else
                    folderBrowserDialog.SelectedPath = Util.Util.IdentificarDispositivoUSB();

                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string destino = @"" + folderBrowserDialog.SelectedPath;

                    fachada.CopiarFirmwareProgramDataParaDiretorio(destino);
                }
                MessageBox.Show(this, rm.GetString("UPDATE_FIRMWARE_UPDATE_SUCCESS"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this, rm.GetString("UPDATE_FIRMWARE_UPDATE_ERROR"), rm.GetString("MAIN_MENU_MBOX_OPEN_ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDownloadAll_Click(object sender, EventArgs e)
        {
            foreach (WebClient web in webList)
            {
                indiceLinhaDownload = webList.IndexOf(web);
                string produto = dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_PRODUTO].Value.ToString();
                Version versao = new Version(dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_VERSAO].Value.ToString());
                string sw_fw = (dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_TIPO].Value.ToString() == "Software") ? "SW" : "FW";
                string familia = versao.Major.ToString();

                string url = fachada.VerificarAtualizarSistemas(familia, produto, sw_fw, versao.ToString());



                string filename = "\\" + produto + "_" + familia + "_" + sw_fw + "_" + versao.ToString().Replace(".", "_");

                if (!String.IsNullOrEmpty(url))
                {
                    dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_DIRETORIO].Value = url;
                    Uri uri = new Uri(url);

                    webList[indiceLinhaDownload].DownloadFileAsync(new Uri(url), Fachada.diretorio_downloads + "\\" + filename + Path.GetExtension(uri.PathAndQuery));
                }
                else
                {
                    dataGridViewAtualizarFirmware.Rows[indiceLinhaDownload].Cells[COLUNA_PERCENTUAL].Value = rm.GetString("UPDATE_FIRMWARE_MSG_ATUALIZADO");
                    
                }

            }
        }
        #endregion Eventos do Form


    }
}
