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
    public partial class FormReleaseNotes : Form
    {
        Fachada fachada = Fachada.Instance;
        public ResourceManager rm;

        public FormReleaseNotes()
        {
            InitializeComponent();

            //Globalizacao
            rm = fachada.carregaIdioma();

            AplicarIdioma();
        }

        private void AplicarIdioma()
        {
            this.Text = rm.GetString("FORM_REALEASE_NOTES_NOME");
        }

        private void FormReleaseNotes_Load(object sender, EventArgs e)
        {
            switch (fachada.GetIdiomaFachada())
            {
                case Util.Util.Lingua.Portugues:
                    CarregarReleasePortugues();
                    break;
                case Util.Util.Lingua.Ingles:
                    CarregarReleaseIngles();
                    break;
                case Util.Util.Lingua.Espanhol:
                    CarregarReleaseEspanhol();
                    break;
                default:
                    CarregarReleaseIngles();
                    break;
            }
            
        }


        private void CarregarReleasePortugues()
        {
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(" Versão 12.4.2\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Data da Versão: 25/08/2017 \n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Testados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.6.2\n");
            richTextBox1.AppendText("       • Painel: 12.5.3\n");
            richTextBox1.AppendText("       • Painel Multiplex 16: 12.0.3\n");
            richTextBox1.AppendText("       • Painel Multiplex 26: 12.0.3\n");
            richTextBox1.AppendText("       • Painel Multilinhas: 12.0.3\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Funcionalidades:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Liga / Desliga porta USB; \n");
            richTextBox1.AppendText("       • Bloqueio para Formatar Pendrive; \n");
            richTextBox1.AppendText("       • Correção dos problemas levantados na versão 12.4.1\n");
            richTextBox1.AppendText("           - Correção no release notes ;\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");

            richTextBox1.AppendText(" Versão 12.4.1\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Data da Versão: 25/08/2017 \n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Testados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.6.2\n");
            richTextBox1.AppendText("       • Painel: 12.5.3\n");
            richTextBox1.AppendText("       • Painel Multiplex 16: 12.0.3\n");
            richTextBox1.AppendText("       • Painel Multiplex 26: 12.0.3\n");
            richTextBox1.AppendText("       • Painel Multilinhas: 12.0.3\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Funcionalidades:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Liga / Desliga porta USB; \n");
            richTextBox1.AppendText("       • Formatar Pendrive; \n");
            richTextBox1.AppendText("       • Embaralhar painéis após um tempo determinado; \n");
            richTextBox1.AppendText("       • Integração com o validador; \n");            
            richTextBox1.AppendText("       • Atualização dos arquivos de firmware na aplicação via pendrive; \n");
            richTextBox1.AppendText("       • Correção dos problemas levantados na versão 12.3.5\n");
            richTextBox1.AppendText("           - Correção nas fontes ;\n");
            richTextBox1.AppendText("           - Correção na ativação ;\n");
            richTextBox1.AppendText("           - Correção no release notes ;\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");


            richTextBox1.AppendText(" Versão 12.3.6\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Data da Versão: 22/05/2017 \n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Testados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.5.4\n");
            richTextBox1.AppendText("       • Painel: 12.5.2\n");
            richTextBox1.AppendText("       • Painel Multiplex 16: 12.0.2\n");
            richTextBox1.AppendText("       • Painel Multiplex 26: 12.0.2\n");
            richTextBox1.AppendText("       • Painel Multilinhas: 12.0.2\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Funcionalidades:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Habilita modo apresentação do APP;\n");
            richTextBox1.AppendText("       • Atualização dos arquivos de firmware na aplicação; \n");            
            richTextBox1.AppendText("       • Correção dos problemas levantados na versão 12.3.5\n");
            richTextBox1.AppendText("           - Correção na geração do arquivo de configuração (NFX) quando vazio;\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(" Versão 12.3.5\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Data da Versão: 16/05/2017 \n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Testados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.5.4\n");
            richTextBox1.AppendText("       • Painel: 12.5.2\n");
            richTextBox1.AppendText("       • Painel Multiplex 16: 12.0.2\n");
            richTextBox1.AppendText("       • Painel Multiplex 26: 12.0.2\n");
            richTextBox1.AppendText("       • Painel Multilinhas: 12.0.2\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Funcionalidades:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Integração com o APP;\n");
            richTextBox1.AppendText("       • Modificação na imagem do controlador na aba de transmissão;\n");
            richTextBox1.AppendText("       • Adição da impressão dos textos de roteiros e mensagens nos relatórios;\n");
            richTextBox1.AppendText("       • Criação do copiar e colar no editor de fontes;\n");
            richTextBox1.AppendText("       • Verificação da região na abertura de arquivos .ldx2 recarregando as mensagens especiais;\n");
            richTextBox1.AppendText("       • Adição da seleção de roteiros e mensagens;\n");
            richTextBox1.AppendText("       • Textos de ida difere de textos de volta;\n");
            richTextBox1.AppendText("       • Manter indices, apresentações e alinhamentos conforme o arquivo do Pontos 6;\n");
            richTextBox1.AppendText("       • Opção de reposicionar os roteiros e as mensagens;\n");
            richTextBox1.AppendText("       • Correção dos problemas levantados na versão 12.3.4\n");
            richTextBox1.AppendText("           - Correção no referenciamento dos roteiros e mensagens no agendamento;\n");
            richTextBox1.AppendText("           - Correção na seleção dos paineis APP;\n");
            richTextBox1.AppendText("           - Correção na seleção das alternâncias nos painéis;\n");
            richTextBox1.AppendText("           - Correção na edição dos agendamentos.\n");
            richTextBox1.AppendText("           - Correção na edição das fontes.\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(" Versão 12.3.4\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Data da Versão: 28/12/2016\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Testados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.5.1\n");
            richTextBox1.AppendText("       • Painel: 12.5.1\n");
            richTextBox1.AppendText("       • Painel Multiplex 16: 12.0.1\n");
            richTextBox1.AppendText("       • Painel Multiplex 26: 12.0.1\n");
            richTextBox1.AppendText("       • Painel Multilinhas: 12.0.1\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Funcionalidades:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Ajuste no redimensionamento dos textos;\n");
            richTextBox1.AppendText("       • Adição da funcionalidade de cópia de painel, mensagens e roteiros;\n");
            richTextBox1.AppendText("       • Correção no alinhamento do editor de fontes;\n");
            richTextBox1.AppendText("       • Reordenação dos textos de roteiros e mensagens;\n");
            richTextBox1.AppendText("       • Modificação no redimensionamento da largura do caractere no editor de fontes;\n");
            richTextBox1.AppendText("       • Adição da ultima versão do firmware no instalador do software;\n");
            richTextBox1.AppendText("       • Exibição de mensagem ao usuário na abertura de arquivos LDX2 corrompidos;\n");
            richTextBox1.AppendText("       • Atualização da logomarca da FRT no software;\n");
            richTextBox1.AppendText("       • Expanção / Contração dos textos internos dos roteiros e das mensagens;\n");
            richTextBox1.AppendText("       • Edição dos textos de roteiros e mensagens ao clicar duas vezes na tela de listagem;\n");
            richTextBox1.AppendText("       • Criação do release notes;\n");
            richTextBox1.AppendText("       • Adição de busca das mensagens;\n");
            richTextBox1.AppendText("       • Adição de busca dos textos dos roteiros e das mensagens;\n");
            richTextBox1.AppendText("       • Correção dos problemas levantados na versão 12.3.3\n");
            richTextBox1.AppendText("           - Correção na abertura de arquivos recentes;\n");
            richTextBox1.AppendText("           - Correção na abertura de arquivos LDX2;\n");
            richTextBox1.AppendText("           - Seleção de idioma de acordo com o escolhido pelo usuário durante o processo de instalação;\n");
            richTextBox1.AppendText("           - Correção na ativação do software.\n");

            richTextBox1.DeselectAll();

        }

        private void CarregarReleaseIngles()
        {
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(" Build 12.4.2\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText("  Release Date: Aug 25, 2017 \n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Tested Firmware:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controller: 12.6.1\n");
            richTextBox1.AppendText("       • Sign: 12.5.3\n");
            richTextBox1.AppendText("       • Multiplex Sign 16: 12.0.3\n");
            richTextBox1.AppendText("       • Multiplex Sign 26: 12.0.3\n");
            richTextBox1.AppendText("       • Multilines Sign: 12.0.3\n");


            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Features:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Turn USB on/off; \n");
            richTextBox1.AppendText("       • Format Pendrive; \n");
            richTextBox1.AppendText("       • Bug fixes raised on version 12.4.1\n");
            richTextBox1.AppendText("           - Correction on release notes ;\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(" Build 12.4.1\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText("  Release Date: Aug 25, 2017 \n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Tested Firmware:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controller: 12.6.1\n");
            richTextBox1.AppendText("       • Sign: 12.5.3\n");
            richTextBox1.AppendText("       • Multiplex Sign 16: 12.0.3\n");
            richTextBox1.AppendText("       • Multiplex Sign 26: 12.0.3\n");
            richTextBox1.AppendText("       • Multilines Sign: 12.0.3\n");

            
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Features:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Turn USB on/off; \n");
            richTextBox1.AppendText("       • Format Pendrive; \n");
            richTextBox1.AppendText("       • Mix up signs after timeout; \n");
            richTextBox1.AppendText("       • Integration with validator; \n");
            richTextBox1.AppendText("       • Update firmware files by USB device; \n");
            richTextBox1.AppendText("       • Bug fixes raised on version 12.3.5\n");
            richTextBox1.AppendText("           - Correction on fonts;\n");
            richTextBox1.AppendText("           - Correction on activation ;\n");
            richTextBox1.AppendText("           - Correction on release notes ;\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");


            richTextBox1.AppendText(" Build 12.3.5\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Release Date: May 16, 2017\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Tested Firmware:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controller: 12.5.4\n");
            richTextBox1.AppendText("       • Sign: 12.5.2\n");
            richTextBox1.AppendText("       • Multiplex Sign 16: 12.0.2\n");
            richTextBox1.AppendText("       • Multiplex Sign 26: 12.0.2\n");
            richTextBox1.AppendText("       • Multilines Sign: 12.0.2\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Features:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • APP integration;\n");
            richTextBox1.AppendText("       • Modification of the controller image on transmission tab;\n");
            richTextBox1.AppendText("       • Addition of itineraries and messages texts on reports;\n");
            richTextBox1.AppendText("       • Addition of copy and paste on Font Editor;\n");
            richTextBox1.AppendText("       • Region check when opening .ldx2 files reloading the special messages;\n");
            richTextBox1.AppendText("       • Addition of itineraries and message selection;\n");
            richTextBox1.AppendText("       • Departure texts differ from return texts;\n");
            richTextBox1.AppendText("       • Maintain indices, presentations and alignments as the Pontos 6 file;\n");
            richTextBox1.AppendText("       • Option to reposition itinerary and messages;\n");
            richTextBox1.AppendText("       • Bug fixes raised on version 12.3.4\n");
            richTextBox1.AppendText("           - Correction of itineraries and messages references on schedule;\n");
            richTextBox1.AppendText("           - Correction on APP signs selection;\n");
            richTextBox1.AppendText("           - Correction of presentation sequence selection on signs;\n");
            richTextBox1.AppendText("           - Correction on schedule editing.\n");
            richTextBox1.AppendText("           - Correction on font editor.\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(" Build 12.3.4\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 12, FontStyle.Bold);

            richTextBox1.AppendText(" Release Date: 28/12/2016\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Tested Firmware:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controller: 12.5.1\n");
            richTextBox1.AppendText("       • Sign: 12.5.1\n");
            richTextBox1.AppendText("       • Multiplex Sign 16: 12.0.1\n");
            richTextBox1.AppendText("       • Multiplex Sign 26: 12.0.1\n");
            richTextBox1.AppendText("       • Multilines Sign: 12.0.1\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Features:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Adjustment on text resizing;\n");
            richTextBox1.AppendText("       • Addition copy functionality from sign, messages, and itineraries;\n");
            richTextBox1.AppendText("       • Correction on Font Editor Alignment;\n");
            richTextBox1.AppendText("       • Reordering texts from itineraries and messages;\n");
            richTextBox1.AppendText("       • Modification on character width scaling on Font Editor;\n");
            richTextBox1.AppendText("       • Addition of the latest firmware version on software installer;\n");
            richTextBox1.AppendText("       • Message display to user opening corrupted LDX2 files;\n");
            richTextBox1.AppendText("       • Updating FRT logo on software;\n");
            richTextBox1.AppendText("       • Expansion / Contraction of the internal texts of itineraries and messages;\n");
            richTextBox1.AppendText("       • Editing itineraries and messages by double-clicking on the listing screen;\n");
            richTextBox1.AppendText("       • Creation of the release notes;\n");
            richTextBox1.AppendText("       • Adding message search;\n");
            richTextBox1.AppendText("       • Adding text search for itineraries and messages;\n");
            richTextBox1.AppendText("       • Bug fixes raised on version 12.3.3\n");
            richTextBox1.AppendText("           - Correction opening recent files;\n");
            richTextBox1.AppendText("           - Correction opening LDX2 files;\n");
            richTextBox1.AppendText("           - Language selection as chosen by user during the installation process;\n");
            richTextBox1.AppendText("           - Correction on software activation.\n");

            richTextBox1.DeselectAll();

        }

        private void CarregarReleaseEspanhol()
        {
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(" Versión 12.4.2\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Fecha de Versíon: 25/08/2017 \n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Probados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.6.1\n");
            richTextBox1.AppendText("       • Panel: 12.5.3\n");
            richTextBox1.AppendText("       • Panel Multiplex 16: 12.0.3\n");
            richTextBox1.AppendText("       • Panel Multiplex 26: 12.0.3\n");
            richTextBox1.AppendText("       • Panel Multilíneas: 12.0.3\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Características:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Encender / apagar USB; \n");
            richTextBox1.AppendText("       • Formatear memoria USB; \n");
            richTextBox1.AppendText("       • Corrección de problemas planteados en la versión 12.4.1\n");
            richTextBox1.AppendText("           - Corrección de las notas de la versión ;\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");
            
            richTextBox1.AppendText(" Versión 12.4.1\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Fecha de Versíon: 25/08/2017 \n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Probados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.6.1\n");
            richTextBox1.AppendText("       • Panel: 12.5.3\n");
            richTextBox1.AppendText("       • Panel Multiplex 16: 12.0.3\n");
            richTextBox1.AppendText("       • Panel Multiplex 26: 12.0.3\n");
            richTextBox1.AppendText("       • Panel Multilíneas: 12.0.3\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Características:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Encender / apagar USB; \n");
            richTextBox1.AppendText("       • Formatear memoria USB; \n");
            richTextBox1.AppendText("       • Embarajar paneles despos de un tiempo determinado; \n");
            richTextBox1.AppendText("       • Integración con el validador; \n");
            richTextBox1.AppendText("       • Actualización de los archivos de firmware; \n");
            richTextBox1.AppendText("       • Corrección de problemas planteados en la versión 12.3.5\n");
            richTextBox1.AppendText("           - Corrección en fuentes ;\n");
            richTextBox1.AppendText("           - Corrección en activación ;\n");
            richTextBox1.AppendText("           - Corrección de las notas de la versión ;\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");


            richTextBox1.AppendText(" Versión 12.3.5\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 10, FontStyle.Bold);

            richTextBox1.AppendText(" Fecha de Versíon: 16/05/2017\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Probados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.5.4\n");
            richTextBox1.AppendText("       • Panel: 12.5.2 \n");
            richTextBox1.AppendText("       • Panel Multiplex 16: 12.0.2\n");
            richTextBox1.AppendText("       • Panel Multiplex 26: 12.0.2\n");
            richTextBox1.AppendText("       • Panel Multilíneas: 12.0.2\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Características:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Integración con APP;\n");
            richTextBox1.AppendText("       • Modificación de la imagen del controlador en la lengüeta de transmisión;\n");
            richTextBox1.AppendText("       • Adición de textos de rutas y mensajes en los informes;\n");
            richTextBox1.AppendText("       • Adición de copiar y pegar en el editor de fuentes;\n");
            richTextBox1.AppendText("       • Verificación de la región al abrir archivos .ldx2 recargando las mensajes especiales;\n");
            richTextBox1.AppendText("       • Adición de la selección de rutas y de mensajes;\n");
            richTextBox1.AppendText("       • Los textos de ida difieren de los textos de vuelta;\n");
            richTextBox1.AppendText("       • Mantener indices, presentaciones y alineaciones según el archivo de Pontos 6;\n");
            richTextBox1.AppendText("       • Opción de reposicionar las rutas y los mensajes;\n");
            richTextBox1.AppendText("       • Corrección de problemas planteados en la versión 12.3.4\n");
            richTextBox1.AppendText("           - Corrección en las referencias de rutas y mensajes en las programaciones;\n");
            richTextBox1.AppendText("           - Corrección en la selección de paneles APP;\n");
            richTextBox1.AppendText("           - Corrección en la selección de presentación de los paneles;\n");
            richTextBox1.AppendText("           - Corrección en la edición de las programaciones.\n");
            richTextBox1.AppendText("           - Corrección en el editor de fuente.\n");

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText(" Versión 12.3.4\n");
            SetarFonte(richTextBox1.Lines.Length - 2, 12, FontStyle.Bold);

            richTextBox1.AppendText(" Fecha de Versíon: 28/12/2016\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Italic);

            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Firmwares Probados:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Controlador: 12.5.1\n");
            richTextBox1.AppendText("       • Panel: 12.5.1\n");
            richTextBox1.AppendText("       • Panel Multiplex 16: 12.0.1\n");
            richTextBox1.AppendText("       • Panel Multiplex 26: 12.0.1\n");
            richTextBox1.AppendText("       • Panel Multilíneas: 12.0.1\n");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("   Características:\n");
            SetarFonte(richTextBox1.Lines.Length - 2, richTextBox1.Font.Size, FontStyle.Bold);

            richTextBox1.AppendText("       • Ajuste de redimensionamiento de texto;\n");
            richTextBox1.AppendText("       • Adición de función de copia de paneles, mensajes y rutas;\n");
            richTextBox1.AppendText("       • Corrección en la alineación del editor de fuentes;\n");
            richTextBox1.AppendText("       • Reordenación de los textos de rutas y mensajes;\n");
            richTextBox1.AppendText("       • Modificación de la escala de anchura de caracteres en Editor de Fuentes;\n");
            richTextBox1.AppendText("       • Adición de la versión de firmware más reciente en el instalador de software;\n");
            richTextBox1.AppendText("       • Exhibición de mensaje al usuario que abre archivos LDX2 dañados;\n");
            richTextBox1.AppendText("       • Actualización del logotipo de FRT en el software;\n");
            richTextBox1.AppendText("       • Expansión / Contracción de los textos internos de rutas y mensajes;\n");
            richTextBox1.AppendText("       • Edición de itinerarios y mensajes haciendo doble clic en la pantalla del listado;\n");
            richTextBox1.AppendText("       • Creación de las notas de la versión;\n");
            richTextBox1.AppendText("       • Adición de búsqueda de mensajes;\n");
            richTextBox1.AppendText("       • Adición de búsqueda de texto para rutas y mensajes;\n");
            richTextBox1.AppendText("       • Corrección de problemas planteados en la versión 12.3.3\n");
            richTextBox1.AppendText("           - Corrección de apertura de archivos recientes;\n");
            richTextBox1.AppendText("           - Corrección de apertura de archivos LDX2;\n");
            richTextBox1.AppendText("           - Selección de idioma elegida por el usuario durante el proceso de instalación;\n");
            richTextBox1.AppendText("           - Corrección de la activación del software.\n");

            richTextBox1.DeselectAll();

        }

        private void SetarFonte(int index, float size ,FontStyle estilo)
        {
            string linha = richTextBox1.Lines[index];
            richTextBox1.Select(richTextBox1.GetFirstCharIndexFromLine(index), linha.Length);
            richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.Font.Name, size, estilo);
        }


    }
}
