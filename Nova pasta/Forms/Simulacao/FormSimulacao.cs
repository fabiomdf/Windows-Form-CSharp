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
using Persistencia;
using System.Threading;
using System.Diagnostics;
using Util;

namespace PontosX2.Forms.Simulacao
{
    public partial class FormSimulacao : Form
    {
        
        Fachada fachada = Fachada.Instance;
        public int controladorSelecionado;
        public int painelSelecionado;
        public bool isSimulacao;
        public List<Frase> listaFrasesRoteiroPainelMultilinhas;
        ResourceManager rm;

        const int QTD_THREADS_PAINEL = 9;

        List<Forms.Simulacao.PainelSimulacao> listaUserControlPaineis = new List<Forms.Simulacao.PainelSimulacao>();
        
        List<Painel> listaPaineis = new List<Painel>();
        bool Cancelar = false;

        List<BackgroundWorker> listaBgw;
        List<AutoResetEvent> listaEsperarApresentacao;
       

        public FormSimulacao()
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            InitializeComponent();

            rm = fachada.carregaIdioma();

            listaBgw = new List<BackgroundWorker>();
            listaEsperarApresentacao = new List<AutoResetEvent>();
            listaFrasesRoteiroPainelMultilinhas = new List<Frase>();

            this.Text = rm.GetString("SIMULACAO_PAINEL_FORM_TEXT");

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);            

        }

        private void MontarPaineis()
        {
            int altura = 5;
            int largura = 0;

            
            if (isSimulacao)
                //Se estiver sendo chamado da tela de simulação, é para simular todos os paineis
                listaPaineis = fachada.CarregarPaineis(controladorSelecionado);
            else
                //se estiver sendo chamada da tela de roteiro em painel multinhas
                listaPaineis.Add(fachada.CarregarPainel(controladorSelecionado, painelSelecionado));

            listaUserControlPaineis.Clear();

            foreach(Painel p in listaPaineis)
            {

                Forms.Simulacao.PainelSimulacao painelUserControl = new Forms.Simulacao.PainelSimulacao();
                painelUserControl.painel = p;

                if (p.MultiLinhas==1)
                    painelUserControl.label.Text = rm.GetString("SIMULACAO_PAINEL_LABEL_PAINEL") + " " + (p.Indice + 1).ToString("d3") + " " + p.Altura.ToString("d2") + " X " + p.Largura.ToString("d2");
                else
                    painelUserControl.label.Text = rm.GetString("SIMULACAO_PAINEL_LABEL_PAINEL") + " " + (p.Indice + 1).ToString("d3") + " " + (p.Altura / p.MultiLinhas).ToString("d2") + " X " + p.Largura.ToString("d2") +
                                    " (" + rm.GetString("ARQUIVO_PAINEL_MULTI_LINHAS") + " " + p.MultiLinhas + "x)";

                painelUserControl.MontarPainel();


                this.Controls.Add(painelUserControl);

                painelUserControl.Location = new Point(0, altura);

                altura = altura + painelUserControl.Height + 10;

                if (largura < painelUserControl.Width)
                    largura = painelUserControl.Width;

                listaUserControlPaineis.Add(painelUserControl);
            }

            
            if (altura > 700)
            {
                this.Height = 700;
                this.Width = largura + 60;
            }
            else
            { 
                this.Height = altura + 35;
                this.Width = largura + 20;
            }

            for(int i=0; i<listaUserControlPaineis.Count;i++)
            {
                listaUserControlPaineis[i].Location = new Point(this.Width / 2 - (listaUserControlPaineis[i].Width / 2) - 7 , listaUserControlPaineis[i].Location.Y);
            }

        }

        private void FormSimulacao_Shown(object sender, EventArgs e)
        {
            MontarPaineis();

            System.Drawing.Rectangle workingRectangle = Screen.GetWorkingArea(this);
            this.Location = new Point((workingRectangle.Width / 2 - this.Width / 2), (workingRectangle.Height / 2 - this.Height / 2));

            IniciarSimulacao();
        }

        private void IniciarSimulacao()
        {

            listaBgw.Clear();

            for(int i=0;i<listaUserControlPaineis.Count;i++)
            {

                //Background worker de cada painel
                BackgroundWorker worker = new BackgroundWorker();

                worker.WorkerSupportsCancellation = true;
                worker.WorkerReportsProgress = true;
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.listaBgw_DoWork);
                listaBgw.Add(worker);

                //adicionando os eventos de timers para cada painel
                AutoResetEvent timer = new AutoResetEvent(false);             
                listaEsperarApresentacao.Add(timer);

                //Montando os backgroundworkers para cada painel
                for (int j = 0; j < QTD_THREADS_PAINEL - 1; j++) 
                {
                    BackgroundWorker worker2 = new BackgroundWorker();

                    worker2.WorkerSupportsCancellation = true;
                    worker2.WorkerReportsProgress = true;
                    worker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.listaBgwVideos_DoWork);
                    worker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.listaBgwVideos_RunWorkerCompleted);

                    listaBgw.Add(worker2);

                    //adicionando evento de timer para cada subpainel
                    timer = new AutoResetEvent(false);
                    listaEsperarApresentacao.Add(timer);
                }

                listaBgw[(i* QTD_THREADS_PAINEL)].RunWorkerAsync();               
            }
        }


        //cada apresentação de subpainel completa
        private void listaBgwVideos_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int indicebgw = listaBgw.IndexOf((BackgroundWorker)sender);

            int indicePainel = indicebgw / QTD_THREADS_PAINEL;

            listaEsperarApresentacao[indicebgw].Set();
        }

        //apresentando cada texto do subpainel
        private void listaBgwVideos_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            int indicebgw = listaBgw.IndexOf((BackgroundWorker)sender);

            int indicePainel = indicebgw / QTD_THREADS_PAINEL;

            int indiceVideo = (indicebgw % QTD_THREADS_PAINEL) - 1;

            Texto texto = (Texto)e.Argument;

            listaUserControlPaineis[indicePainel].ApresentarTexto(indiceVideo, texto);
             
        }

        //background worker do painel
        private void listaBgw_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            int indicebgw = listaBgw.IndexOf((BackgroundWorker)sender);
            int indicePainel = indicebgw / QTD_THREADS_PAINEL;
           

            List<Frase> listaFrases = new List<Frase>();
            Stopwatch s = new Stopwatch();


            if (isSimulacao)
            {
                listaFrases = MontarListaFrasesAlternancia(listaFrases, fachada.CarregarPainel(controladorSelecionado, indicePainel));

                for (int i = 0; i < listaFrases.Count && (!Cancelar); i++)
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, indicePainel), fachada.GetLarguraPainel(controladorSelecionado, indicePainel), listaFrases[i]);

                //inicia o relogio se for para inverter o LED 
                if (fachada.RetornarMinutosInverterLED(controladorSelecionado) != 0)
                    s.Start();
            }
            else
            {
                listaFrases = listaFrasesRoteiroPainelMultilinhas;

                for (int i = 0; i < listaFrases.Count && (!Cancelar); i++)
                    fachada.PreparaBitMapFrase(fachada.GetAlturaPainel(controladorSelecionado, painelSelecionado), fachada.GetLarguraPainel(controladorSelecionado, painelSelecionado), listaFrases[i]);
            }

            while (!Cancelar)
            {
                //Controle para inversão dos LEDs apenas se for na simulação
                if (isSimulacao)
                {
                    if (fachada.RetornarMinutosInverterLED(controladorSelecionado) != 0)
                    {
                        if (s.ElapsedMilliseconds > (fachada.RetornarMinutosInverterLED(controladorSelecionado) * 60000))
                        {
                            for (int i = 0; i < listaFrases.Count && (!Cancelar); i++)
                            {
                                fachada.InveterLEDBitMapFrase(listaFrases[i]);

                                foreach (Texto t in listaFrases[i].Modelo.Textos)
                                    t.InverterLed = !t.InverterLed;
                            }

                            s.Restart();
                        }
                    }
                }

                for (int i = 0; i<listaFrases.Count && (!Cancelar);i++ )
                {
                    Frase f = new Frase(listaFrases[i]);

                    if (!isSimulacao && listaFrases.Count > 1)
                    { 
                        //atualizar o item selecionado na lista de frases do roteiro
                        MethodInvoker alterarItemSelecionadoListView = delegate
                        {
                            ((Arquivo)this.Owner.ActiveMdiChild).SelecionarItemListRoteiros(i);
                        };

                        ((Arquivo)this.Owner.ActiveMdiChild).Invoke(alterarItemSelecionadoListView);
                    }


                    MethodInvoker invoker = delegate
                    {
                        listaUserControlPaineis[indicePainel].ReposicionarVideoMapNoPainel(f,(listaPaineis[indicePainel].MultiLinhas==1?false:true));
                    };


                    listaUserControlPaineis[indicePainel].Invoke(invoker);

                    if (f.TipoVideo == Util.Util.TipoVideo.V02)
                    {
                        listaBgw[(indicebgw + 1)].RunWorkerAsync(f.Modelo.Textos[0]);
                        listaEsperarApresentacao[(indicebgw + 1)].WaitOne();
                    }

                    if (f.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
                        {
                            if (listaPaineis[indicePainel].MultiLinhas == 1)
                            {
                                listaBgw[(indicebgw + 1)].RunWorkerAsync(f.Modelo.Textos[0]);
                                listaEsperarApresentacao[(indicebgw + 1)].WaitOne();
                            }
                            else
                            {
                                for(int z=0; z<listaPaineis[indicePainel].MultiLinhas;z++)
                                    listaBgw[(indicebgw + 1 + z)].RunWorkerAsync(f.Modelo.Textos[z]);

                                for (int z = 0; z < listaPaineis[indicePainel].MultiLinhas; z++)
                                    listaEsperarApresentacao[(indicebgw + 1 + z)].WaitOne();
                            }
                        }

                        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo || f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero)
                        {
                            listaBgw[(indicebgw + 1)].RunWorkerAsync(f.Modelo.Textos[0]);
                            listaBgw[(indicebgw + 2)].RunWorkerAsync(f.Modelo.Textos[1]);
                            listaEsperarApresentacao[(indicebgw + 1)].WaitOne();
                            listaEsperarApresentacao[(indicebgw + 2)].WaitOne();
                        }

                        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo || f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero || f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
                        {
                            listaBgw[(indicebgw + 1)].RunWorkerAsync(f.Modelo.Textos[0]);
                            listaBgw[(indicebgw + 2)].RunWorkerAsync(f.Modelo.Textos[1]);
                            listaBgw[(indicebgw + 3)].RunWorkerAsync(f.Modelo.Textos[2]);
                            listaEsperarApresentacao[(indicebgw + 1)].WaitOne();
                            listaEsperarApresentacao[(indicebgw + 2)].WaitOne();
                            listaEsperarApresentacao[(indicebgw + 3)].WaitOne();
                        }

                        if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo || f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero || f.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
                        {
                            listaBgw[(indicebgw + 1)].RunWorkerAsync(f.Modelo.Textos[0]);
                            listaBgw[(indicebgw + 2)].RunWorkerAsync(f.Modelo.Textos[1]);
                            listaBgw[(indicebgw + 3)].RunWorkerAsync(f.Modelo.Textos[2]);
                            listaBgw[(indicebgw + 4)].RunWorkerAsync(f.Modelo.Textos[3]);
                            listaEsperarApresentacao[(indicebgw + 1)].WaitOne();
                            listaEsperarApresentacao[(indicebgw + 2)].WaitOne();
                            listaEsperarApresentacao[(indicebgw + 3)].WaitOne();
                            listaEsperarApresentacao[(indicebgw + 4)].WaitOne();
                        }
                    }
                }
            }
        }

        private List<Frase> MontarListaFrasesAlternancia(List<Frase> lista, Painel p)
        {

            int motoristaSelecionado = fachada.RetornarParamVariavelMotoristaSelecionado(controladorSelecionado); ;
            int roteiroSelecionado = fachada.RetornarParamVariavelRoteiroSelecionado(controladorSelecionado);
            int mensagemSelecionada = p.MensagemSelecionada;
            int mensagemSecSelecionada = p.MensagemSecundariaSelecionada;
            bool SentidoIdaRoteiro = fachada.RetornarParamVariavelSentidaIda(controladorSelecionado);
            Frase fraseTemp;

            foreach (TipoExibicao tpExibicao in p.ListaAlternancias[p.AlternanciaSelecionada].Exibicoes)
            {
                switch (tpExibicao)
                {
                    #region Exibir Roteiro

                    case TipoExibicao.EXIBICAO_ROTEIRO :                        
                        
                        if (SentidoIdaRoteiro)
                        {
                            if (p.Roteiros[roteiroSelecionado].FrasesIda.Count==0 )
                                lista.Add(new Frase(p.Roteiros[roteiroSelecionado].Numero));
                            else
                                foreach(Frase f in p.Roteiros[roteiroSelecionado].FrasesIda)
                                    lista.Add(new Frase(f));
                        }
                        else
                        {

                            //se o roteiro estiver marcado como os textos de ida sendo iguais aos de volta deve exibir os textos de ida
                            if (p.Roteiros[roteiroSelecionado].IdaIgualVolta)
                            {
                                if (p.Roteiros[roteiroSelecionado].FrasesIda.Count == 0)
                                    lista.Add(new Frase(p.Roteiros[roteiroSelecionado].Numero));
                                else
                                    foreach (Frase f in p.Roteiros[roteiroSelecionado].FrasesIda)
                                        lista.Add(new Frase(f));
                            }

                            //se o roteiro estiver marcado como textos de ida e volta diferentes
                            if (!p.Roteiros[roteiroSelecionado].IdaIgualVolta)
                            {
                                if (p.Roteiros[roteiroSelecionado].FrasesVolta.Count == 0)
                                    lista.Add(new Frase(p.Roteiros[roteiroSelecionado].Numero));
                                else
                                    foreach (Frase f in p.Roteiros[roteiroSelecionado].FrasesVolta)
                                        lista.Add(new Frase(f));
                            }
                        }

                        break;

                    #endregion

                    #region Exibir Numero

                    case TipoExibicao.EXIBICAO_NUMERO :

                        lista.Add(new Frase(p.Roteiros[roteiroSelecionado].Numero));

                        break;

                    #endregion

                    #region Mensagem

                    case TipoExibicao.EXIBICAO_MENSAGEM :

                        foreach(Frase f in p.Mensagens[mensagemSelecionada].Frases)
                            lista.Add(new Frase(f));

                        break;

                    #endregion

                    #region Hora Saida

                    case TipoExibicao.EXIBICAO_HORA_SAIDA :

                            fraseTemp = new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida]);
                            fraseTemp.LabelFrase = fraseTemp.LabelFrase + " " + p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].LabelFrase;
                            fraseTemp.Modelo.Textos[0].LabelTexto = fraseTemp.Modelo.Textos[0].LabelTexto + " " + p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;

                            lista.Add(new Frase(fraseTemp));

                        break;

                    #endregion

                    #region Exibir Hora

                    case TipoExibicao.EXIBICAO_HORA :

                        lista.Add(new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora]));
                        break;

                    #endregion

                    #region Exibir Data Hora
                    
                    case TipoExibicao.EXIBICAO_DATA_HORA :

                        lista.Add(new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora]));
                        break;

                    #endregion

                    #region Tarifa
                    case TipoExibicao.EXIBICAO_TARIFA :
                        Frase fTarifa = new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa]);

                        string tarifaInteiro = (p.Roteiros[roteiroSelecionado].Tarifa <= 99 ? '0'.ToString() : p.Roteiros[roteiroSelecionado].Tarifa.ToString().Substring(0, p.Roteiros[roteiroSelecionado].Tarifa.ToString().Length - 2));
                        string tarifaDecimal = (p.Roteiros[roteiroSelecionado].Tarifa <= 99 ? p.Roteiros[roteiroSelecionado].Tarifa.ToString("00") : p.Roteiros[roteiroSelecionado].Tarifa.ToString().Substring(p.Roteiros[roteiroSelecionado].Tarifa.ToString().Length - 2, 2));

                        fTarifa.Modelo.Textos[0].LabelTexto = p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloTarifa].Modelo.Textos[0].LabelTexto + " " + tarifaInteiro + p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloSeparadorDecimal].Modelo.Textos[0].LabelTexto + tarifaDecimal;
                        fTarifa.LabelFrase = p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloTarifa].Modelo.Textos[0].LabelTexto + " " + tarifaInteiro + p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloSeparadorDecimal].Modelo.Textos[0].LabelTexto + tarifaDecimal;

                        lista.Add(new Frase(fTarifa));
                        break;
                    #endregion

                    #region Saudacao
                    
                    case TipoExibicao.EXIBICAO_SAUDACAO :

                        if (DateTime.Now.Hour >= fachada.RetornarHoraBomDia(controladorSelecionado) && DateTime.Now.Hour < fachada.RetornarHoraBoaTarde(controladorSelecionado))
                            lista.Add(new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.BomDia]));
                        else
                        { 
                            if (DateTime.Now.Hour >= fachada.RetornarHoraBoaTarde(controladorSelecionado) && DateTime.Now.Hour < fachada.RetornarHoraBoaNoite(controladorSelecionado))
                                lista.Add(new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.BoaTarde]));
                            else
                            { 
                                if (DateTime.Now.Hour >= fachada.RetornarHoraBoaNoite(controladorSelecionado))
                                    lista.Add(new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.BoaNoite]));
                            }
                        }
                        break;

                    #endregion

                    #region Temperatura

                    case TipoExibicao.EXIBICAO_TEMPERATURA :

                        lista.Add(new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura]));
                        break;

                    #endregion

                    #region Hora e Temperatura

                    case TipoExibicao.EXIBICAO_HORA_E_TEMPERATURA :
                        lista.Add(new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura]));
                        break;

                    #endregion

                    #region Velocidade
                    
                    case TipoExibicao.EXIBICAO_VELOCIDADE :
                        lista.Add(new Frase(p.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade]));
                        break;
                    
                    #endregion

                    #region Mensagem Secundaria

                    case TipoExibicao.EXIBICAO_MENSAGEM_SECUNDARIA:

                        foreach (Frase f in p.Mensagens[mensagemSecSelecionada].Frases)
                            lista.Add(new Frase(f));

                        break;

                    #endregion

                    #region ID do Motorista

                    case TipoExibicao.EXIBICAO_ID_MOTORISTA:
                        if (p.Motoristas.Count>0)
                            lista.Add(new Frase(p.Motoristas[motoristaSelecionado].ID));
                        break;

                    #endregion

                    #region Nome do Motorista

                    case TipoExibicao.EXIBICAO_NOME_MOTORISTA:
                        if (p.Motoristas.Count > 0)
                            lista.Add(new Frase(p.Motoristas[motoristaSelecionado].Nome));
                        break;

                    #endregion
                }
            }

            return lista;
        }
       
        private void FormSimulacao_FormClosing(object sender, FormClosingEventArgs e)
        {

            for (int j = 0; j < listaUserControlPaineis.Count; j++)
            {
                listaUserControlPaineis[j].PararSimulacao();
            }

            Cancelar = true;

            for (int z = 0; z < listaEsperarApresentacao.Count; z++)
                listaEsperarApresentacao[z].Set();

            int i=0;
            while (i<listaBgw.Count)
            {
                while(listaBgw[i].IsBusy)
                {
                   //Do events é usado para atualizar o status do backgroundworker
                   Application.DoEvents();
                }
                i = i + QTD_THREADS_PAINEL;
            }

            if (!isSimulacao)
            {
                MethodInvoker invoker = delegate
                {
                    ((Arquivo)this.Owner.ActiveMdiChild).CancelarApresentacaoPainelMultiLinhas();
                };

                ((Arquivo)this.Owner.ActiveMdiChild).Invoke(invoker);
            }

        }

    }
}
