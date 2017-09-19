using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nucleo;
using Persistencia;
using System.Resources;
using System.Drawing;


namespace Controlador
{
    [Serializable]
    public class Painel
    {
        public int Indice { get; set; }

        public int Altura { get; set; }

        public int Largura { get; set; }

        public List<Mensagem> Mensagens { get; set; }

        public List<Roteiro> Roteiros { get; set; }

        public int AlternanciaSelecionada { get; set; }

        public String FontePath { get; set; }

        public int MensagemSelecionada { get; set; }

        public int MensagemSecundariaSelecionada { get; set; }

        public MensagemEspecial MensagensEspeciais { get; set; }

        public MensagemEmergencia MensagemEmergencia { get; set; }

        public List<Evento> Eventos { get; set; }

        public List<Motorista> Motoristas { get; set; }

        public ResourceManager rm { get; set; } 

        public int MultiLinhas { get; set; }
   
        public List<ItemAlternancia> ListaAlternancias { get; set; }
   
        public byte BrilhoMaximo { get; set; }
        public byte BrilhoMinimo { get; set; }
   
        
        public Painel(ResourceManager rm)
        {
            this.Altura = 16;
            this.Largura = 96;
            this.Roteiros = new List<Roteiro>();
            this.Mensagens = new List<Mensagem>();
            this.MensagensEspeciais = new MensagemEspecial();
            this.MensagemEmergencia = new MensagemEmergencia(rm);
            this.rm = rm;
            this.MensagemSelecionada = 0;
            this.AlternanciaSelecionada = 0;
            this.ListaAlternancias = new List<ItemAlternancia>();
            this.BrilhoMaximo = 100;
            this.BrilhoMinimo = 10;
            this.Eventos = new List<Evento>();
            this.Motoristas = new List<Motorista>();
            this.MultiLinhas = 1;    
        }

        public Painel()
        {
            this.Altura = 16;
            this.Largura = 144;
            this.Roteiros = new List<Roteiro>();
            this.Mensagens = new List<Mensagem>();            
            this.MensagensEspeciais = new MensagemEspecial();
            this.MensagemEmergencia = new MensagemEmergencia();            
            this.MensagemSelecionada = 0;
            this.AlternanciaSelecionada = 0;
            this.ListaAlternancias = new List<ItemAlternancia>();
            this.BrilhoMaximo = 100;
            this.BrilhoMinimo = 10;
            this.Eventos = new List<Evento>();
            this.Motoristas = new List<Motorista>();
            this.MultiLinhas = 1;
        }

        public Painel(ResourceManager rm, Arquivo_RGN regiao)
        {
            this.Altura = 16;
            this.Largura = 96;
            this.Roteiros = new List<Roteiro>();
            this.Mensagens = new List<Mensagem>();            
            this.MensagensEspeciais = new MensagemEspecial(rm, regiao);
            this.MensagemEmergencia = new MensagemEmergencia(rm);
            this.rm = rm;
            this.MensagemSelecionada = 0;
            this.AlternanciaSelecionada = 0;
            this.ListaAlternancias = new List<ItemAlternancia>();
            this.BrilhoMaximo = 100;
            this.BrilhoMinimo = 10;
            this.Eventos = new List<Evento>();
            this.Motoristas = new List<Motorista>();
            this.MultiLinhas = 1;
        }
        
        public Painel(Painel painel_antigo) 
        {
            this.Altura = painel_antigo.Altura;
            this.Largura = painel_antigo.Largura;
            this.rm = painel_antigo.rm;            

            this.Roteiros = new List<Roteiro>();
            foreach (Roteiro r in painel_antigo.Roteiros)
                this.Roteiros.Add(new Roteiro(r,true));

            this.Mensagens = new List<Mensagem>();
            foreach (Mensagem m in painel_antigo.Mensagens)
                this.Mensagens.Add(new Mensagem(m, true));

            this.MensagensEspeciais = new MensagemEspecial(painel_antigo.MensagensEspeciais);
            this.MensagemEmergencia = new MensagemEmergencia(painel_antigo.MensagemEmergencia);
            
            this.AlternanciaSelecionada = painel_antigo.AlternanciaSelecionada;
            this.MensagemSelecionada = painel_antigo.MensagemSelecionada;

            this.ListaAlternancias = new List<ItemAlternancia>();
            foreach (ItemAlternancia item in painel_antigo.ListaAlternancias)
                this.ListaAlternancias.Add(new ItemAlternancia(item));

            this.Eventos = new List<Evento>();
            foreach (Evento e in painel_antigo.Eventos)
                this.Eventos.Add(new Evento(e));

            this.Motoristas = new List<Motorista>();
            foreach (Motorista m in painel_antigo.Motoristas)
                this.Motoristas.Add(new Motorista(m));

            this.BrilhoMaximo = painel_antigo.BrilhoMaximo;
            this.BrilhoMinimo = painel_antigo.BrilhoMinimo;
            this.MultiLinhas = painel_antigo.MultiLinhas;

            this.FontePath = painel_antigo.FontePath;
            this.MensagemSecundariaSelecionada = painel_antigo.MensagemSecundariaSelecionada;
        }

        public void PainelDefault(int numeroMensagens, 
                                  int numeroRoteiros, 
                                  int indicePainel)
        {

            for (int msgs = 0; msgs < numeroMensagens; msgs++)
            {
                Mensagem mTemp = new Mensagem(rm.GetString("MENSAGEM_LABEL").ToString());

                mTemp.ID = msgs+1;
                mTemp.Indice = msgs;
                mTemp.Ascendente = true;
                mTemp.Ordenacao = 0;
                this.Mensagens.Add(mTemp);
            }

            for (int rots = 0; rots < numeroRoteiros; rots++)
            {
                Roteiro rTemp = new Roteiro((rots).ToString("00"));

                rTemp.ID = rots + 1;
                rTemp.Indice = rots;
                rTemp.LabelRoteiro = rm.GetString("ROTEIRO_LABEL") +" "+ rots;
                rTemp.Ascendente = true;
                rTemp.Ordenacao = 0;
                this.Roteiros.Add(rTemp);
            }
            
        }


        public void PainelDefault(int numeroMensagens,
                                  int numeroRoteiros,
                                  int numeroMotoristas,
                                  int indicePainel,
                                  int numeroPaineis)
        {           
            bool isV02 = (indicePainel % 2) == 0;

            for (int msgs = 0; msgs < numeroMensagens; msgs++)
            {
                if (isV02)
                {
                    // DEFINIR VÁRIOS TEMPLATES - Text, Double Text e Double Text + Double Text;
                    // DEFINIR VÁRIOS ALINHAMENTOS
                    // DEFINIR USO DE FONTES frt E TRUETYPE

                    Mensagem mTemp = new Mensagem("Texto Simples");

                    //mTemp.Frases.Add(new Frase("Texto Simples"));
                    mTemp.Frases[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                    mTemp.Frases[0].Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                    mTemp.Frases[0].TextoAutomatico = false;

                    mTemp.ID = msgs + 1;
                    mTemp.Indice = msgs;
                    mTemp.Ascendente = true;
                    mTemp.Ordenacao = 0;
                    mTemp.Frases[0].TipoVideo = Util.Util.TipoVideo.V02;
                    mTemp.Frases[0].Modelo.TipoModelo = Util.Util.TipoModelo.Texto;
                    mTemp.Frases[0].Modelo.Textos[0].LabelTexto = "Texto Simples";
                    mTemp.Frases[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                    mTemp.Frases[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                    mTemp.Frases[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                    mTemp.Frases[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                    mTemp.Frases[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                    this.Mensagens.Add(mTemp);
                }
                else // V04
                {
                    if (this.Mensagens.Count%2 == 0)
                    {
                        Mensagem mTemp = new Mensagem("TextoDuplo");

                        mTemp.Frases[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                        mTemp.Frases[0].Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;

                        mTemp.ID = msgs + 1;
                        mTemp.Indice = msgs;
                        mTemp.Ascendente = true;
                        mTemp.Ordenacao = 0;
                        mTemp.Frases[0].TextoAutomatico = false;
                        mTemp.Frases[0].TipoVideo = Util.Util.TipoVideo.V04;
                        mTemp.Frases[0].Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuplo;
                        mTemp.Frases[0].Modelo.Textos[0].LabelTexto = "Linha cima";
                        mTemp.Frases[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                        mTemp.Frases[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                        mTemp.Frases[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                        mTemp.Frases[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                        mTemp.Frases[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        mTemp.Frases[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                        mTemp.Frases[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                        mTemp.Frases[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                        mTemp.Frases[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                        mTemp.Frases[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                        mTemp.Frases[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        this.Mensagens.Add(mTemp);
                    }
                    else 
                    {                                                                    
                        Mensagem mTemp = new Mensagem("TextoDuploTextoDuplo");                        
                        mTemp.Frases[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                        mTemp.Frases[0].Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuploTextoDuplo;

                        mTemp.ID = msgs + 1;
                        mTemp.Indice = msgs;
                        mTemp.Ascendente = true;
                        mTemp.Ordenacao = 0;
                        mTemp.Frases[0].TextoAutomatico = false;

                        mTemp.Frases[0].Modelo.TipoModelo = Util.Util.TipoModelo.TextoDuploTextoDuplo;
                        mTemp.Frases[0].Modelo.Textos[0].LabelTexto = "Linha cima Esquerda";
                        mTemp.Frases[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                        mTemp.Frases[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                        mTemp.Frases[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                        mTemp.Frases[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                        mTemp.Frases[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        mTemp.Frases[0].Modelo.Textos.Add(new Texto("Linha cima Direita"));
                        mTemp.Frases[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                        mTemp.Frases[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                        mTemp.Frases[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                        mTemp.Frases[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                        mTemp.Frases[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        mTemp.Frases[0].Modelo.Textos.Add(new Texto("Linha baixo esquerda"));
                        mTemp.Frases[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                        mTemp.Frases[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                        mTemp.Frases[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                        mTemp.Frases[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                        mTemp.Frases[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        mTemp.Frases[0].Modelo.Textos.Add(new Texto("Linha baixo Direita"));
                        mTemp.Frases[0].Modelo.Textos[3].Apresentacao = GetRolagemRandomica();
                        mTemp.Frases[0].Modelo.Textos[3].TempoApresentacao = GetTempoApresentacao();
                        mTemp.Frases[0].Modelo.Textos[3].TempoRolagem = GetTempoRolagem();
                        mTemp.Frases[0].Modelo.Textos[3].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                        mTemp.Frases[0].Modelo.Textos[3].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        this.Mensagens.Add(mTemp);
                    }                 
                }
            }

            for (int rots = 0; rots < numeroRoteiros; rots++)
            {
                // DEFINIR VÁRIOS TEMPLATES
                Util.Util.TipoModelo tipoModelo = (Util.Util.TipoModelo)(this.Roteiros.Count % Enum.GetNames(typeof(Util.Util.TipoModelo)).Length);

                //não permitir que paineis menores que altura 26 possuam textos triplos
                if (tipoModelo == Util.Util.TipoModelo.TextoTriplo && this.Altura < 26)
                    tipoModelo = Util.Util.TipoModelo.Texto;

                if (tipoModelo == Util.Util.TipoModelo.TextoTriploNumero && this.Altura < 26)
                    tipoModelo = Util.Util.TipoModelo.TextoNúmero;

                if (tipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo && this.Altura < 26)
                    tipoModelo = Util.Util.TipoModelo.NúmeroTexto;

                Roteiro rTemp = new Roteiro(tipoModelo.ToString());

                rTemp.ID = rots + 1;
                rTemp.Indice = rots;
                rTemp.LabelRoteiro = tipoModelo.ToString() + " " + (rots + 1);
                rTemp.Numero = new Frase((rots + 1).ToString("00"));
                rTemp.Ascendente = true;
                rTemp.Ordenacao = 0;
                // DEFINIR IDA DIFERENTE DE VOLTA
                rTemp.IdaIgualVolta = false;
                // DEFINIR TARIFAS DIFERENTES PARA CADA ROTEIRO
                rTemp.Tarifa = GetTarifaRandomica();

                rTemp.FrasesIda.Add(new Frase("Ida - " + tipoModelo.ToString() + " " + (rots + 1)));
                rTemp.FrasesIda[0].TextoAutomatico = false;
                rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;

                rTemp.FrasesVolta.Add(new Frase("Volta - " + tipoModelo.ToString() + " " + (rots + 1)));
                rTemp.FrasesVolta[0].TextoAutomatico = false;
                rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;

                switch (tipoModelo)
                {
                    case Util.Util.TipoModelo.NúmeroTexto:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = (rots + 1).ToString("00");
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Ida - " + tipoModelo.ToString() + " " + (rots + 1)));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta                          
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = (rots + 1).ToString("00");
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Volta - " + tipoModelo.ToString() + " " + (rots + 1)));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        } break;
                    case Util.Util.TipoModelo.NúmeroTextoDuplo:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = (rots + 1).ToString("00");
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Ida - " + tipoModelo.ToString() + " " + (rots + 1)));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesIda[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = (rots + 1).ToString("00");
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Volta - " + tipoModelo.ToString() + " " + (rots + 1)));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesVolta[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        } break;
                    case Util.Util.TipoModelo.Texto:
                        {                            
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = "Ida - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = "Volta - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        } break;
                    case Util.Util.TipoModelo.TextoDuplo:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = "Ida - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = "Volta - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        } break;
                    case Util.Util.TipoModelo.TextoDuploNúmero:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = "Ida - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto((rots + 1).ToString("00")));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesIda[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = "Volta - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto((rots + 1).ToString("00")));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesVolta[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();
                        } break;
                    case Util.Util.TipoModelo.TextoDuploTextoDuplo:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = "Ida - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha cima Direita"));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha baixo esquerda"));
                            rTemp.FrasesIda[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha baixo Direita"));
                            rTemp.FrasesIda[0].Modelo.Textos[3].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[3].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[3].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[3].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[3].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = "Volta - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha cima Direita"));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha baixo esquerda"));
                            rTemp.FrasesVolta[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha baixo Direita"));
                            rTemp.FrasesVolta[0].Modelo.Textos[3].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        } break;
                    case Util.Util.TipoModelo.TextoNúmero:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = "Ida - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto((rots + 1).ToString("00")));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();                            

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = "Volta - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto((rots + 1).ToString("00")));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        } break;

                    case Util.Util.TipoModelo.TextoTriplo:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;

                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = "Ida - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha meio"));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesIda[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = "Volta - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha meio"));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesVolta[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        }
                        break;

                    case Util.Util.TipoModelo.TextoTriploNumero:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = "Ida - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto((rots + 1).ToString("00")));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha meio"));
                            rTemp.FrasesIda[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesIda[0].Modelo.Textos[3].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[3].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[3].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[3].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[3].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = "Volta - " + tipoModelo.ToString() + " " + (rots + 1);
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto((rots + 1).ToString("00")));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha meio"));
                            rTemp.FrasesVolta[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesVolta[0].Modelo.Textos[3].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        }
                        break;

                    case Util.Util.TipoModelo.NumeroTextoTriplo:
                        {
                            //Frase de Ida
                            rTemp.FrasesIda[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesIda[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesIda[0].Modelo.Textos[0].LabelTexto = (rots + 1).ToString("00");
                            rTemp.FrasesIda[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Ida - " + tipoModelo.ToString() + " " + (rots + 1)));
                            rTemp.FrasesIda[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha meio"));
                            rTemp.FrasesIda[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesIda[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesIda[0].Modelo.Textos[3].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[3].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesIda[0].Modelo.Textos[3].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesIda[0].Modelo.Textos[3].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesIda[0].Modelo.Textos[3].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            //Frase de Volta
                            rTemp.FrasesVolta[0].TipoVideo = (isV02) ? Util.Util.TipoVideo.V02 : Util.Util.TipoVideo.V04;
                            rTemp.FrasesVolta[0].Modelo.TipoModelo = tipoModelo;
                            rTemp.FrasesVolta[0].Modelo.Textos[0].LabelTexto = (rots + 1).ToString("00");
                            rTemp.FrasesVolta[0].Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Volta - " + tipoModelo.ToString() + " " + (rots + 1)));
                            rTemp.FrasesVolta[0].Modelo.Textos[1].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[1].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha meio"));
                            rTemp.FrasesVolta[0].Modelo.Textos[2].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[2].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                            rTemp.FrasesVolta[0].Modelo.Textos.Add(new Texto("Linha baixo"));
                            rTemp.FrasesVolta[0].Modelo.Textos[3].Apresentacao = GetRolagemRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].TempoApresentacao = GetTempoApresentacao();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].TempoRolagem = GetTempoRolagem();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                            rTemp.FrasesVolta[0].Modelo.Textos[3].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                        }
                        break;
                }
                this.Roteiros.Add(rTemp);
                  
                // DEFINIR USO DE FONTES frt E TRUETYPE
                // DEFINIR USO DE DIFERENTES ALINHAMENTOS

            }
            for (int drvs = 0; drvs < numeroMotoristas; drvs++)
            {                                              
                Motorista mTemp = new Motorista((drvs + 1).ToString("00"), "Driver" + (drvs + 1).ToString("000"));              
                mTemp.Ascendente = true;
                mTemp.Ordenacao = 0;

                mTemp.ID.Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                mTemp.ID.Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                mTemp.ID.Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                mTemp.ID.Modelo.Textos[0].FonteWindows = true;
                mTemp.ID.Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                mTemp.ID.Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                mTemp.Nome.Modelo.Textos[0].Apresentacao = GetRolagemRandomica();
                mTemp.Nome.Modelo.Textos[0].TempoApresentacao = GetTempoApresentacao();
                mTemp.Nome.Modelo.Textos[0].TempoRolagem = GetTempoRolagem();
                mTemp.Nome.Modelo.Textos[0].FonteWindows = true;
                mTemp.Nome.Modelo.Textos[0].AlinhamentoH = GetAlinhamentoHorizontalRandomica();
                mTemp.Nome.Modelo.Textos[0].AlinhamentoV = GetAlinhamentoVerticalRandomica();

                this.Motoristas.Add(mTemp);
            }

        }

        private Util.Util.AlinhamentoHorizontal GetAlinhamentoHorizontalRandomica()
        {
            Random r = new Random();
            return (Util.Util.AlinhamentoHorizontal)(r.Next(Enum.GetNames(typeof(Util.Util.AlinhamentoHorizontal)).Length));
        }

        private Util.Util.AlinhamentoVertical GetAlinhamentoVerticalRandomica()
        {
            Random r = new Random();
            return (Util.Util.AlinhamentoVertical)(r.Next(Enum.GetNames(typeof(Util.Util.AlinhamentoVertical)).Length));
        }

        private Util.Util.TipoModelo GetTemplateRandomico(bool isMessage)
        {
            Random r = new Random();            
            if (isMessage)
            {
                switch (r.Next() % 3)
                {
                    case 0: return Util.Util.TipoModelo.Texto;
                    case 1: return Util.Util.TipoModelo.TextoDuplo;
                    case 2: return Util.Util.TipoModelo.TextoDuploTextoDuplo;
                }
            }
            return (Util.Util.TipoModelo)(r.Next(10));
        }

        // DEFINIR VÁRIAS TRANSIÇÕES E TEMPOS DIFERENTES - ROLAGEM E APRESENTAÇÃO
        private Util.Util.Rolagem GetRolagemRandomica()
        {
            Random r = new Random();
            return (Util.Util.Rolagem)(r.Next(Enum.GetNames(typeof(Util.Util.Rolagem)).Length));
        }

        private int GetTarifaRandomica()
        {
            Random r = new Random();
            return r.Next(10, 10000);
        }

        private int GetTempoApresentacao()
        {
            Random r = new Random();
            return r.Next(1000, 9000);
        }

        private int GetTempoRolagem()
        {
            Random r = new Random();
            return r.Next(10, 60);
        }


        public List<Color[,]> PrepararBitMapPainel()
        {
            Color[,] cores = new Color[this.Altura, this.Largura];
            for (int i = cores.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < cores.GetLength(1); j++)
                {
                    cores[i, j] = Color.Black;
                }
            }

            List<Color[,]> listaCores = new List<Color[,]>();
            listaCores.Add(cores);
            return listaCores;
        }

        public void SetResourceManager(ResourceManager resource)
        {
            this.rm = resource;
        }
        
        public void Salvar(String diretorio_raiz, string diretorioFontes, ref bool CancelarEnvioNFX, bool modoApresentacaoDisplayLD6)
        {

            this.SalvarCFG(diretorio_raiz + 
                           Util.Util.DIRETORIO_PAINEL + 
                           this.Indice.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS) + 
                           Util.Util.ARQUIVO_PAINEL_CFG,
                           true, diretorioFontes);

            if (CancelarEnvioNFX)
                return;
            
            this.SalvarAlternancia(diretorio_raiz + Util.Util.DIRETORIO_PAINEL + this.Indice.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS) + Util.Util.ARQUIVO_PAINEL_ALTERNANCIA);

            if (CancelarEnvioNFX)
                return;

            foreach (Mensagem mensagem in this.Mensagens)
            {
                mensagem.Salvar(diretorio_raiz, this.Indice, (uint) this.Altura, (uint) this.Largura);

                if (CancelarEnvioNFX)
                    return;
            }

            foreach (Roteiro roteiro in this.Roteiros)
            {
                roteiro.Salvar(diretorio_raiz, this.Indice, (uint)this.Altura, (uint)this.Largura, modoApresentacaoDisplayLD6);

                if (CancelarEnvioNFX)
                    return;
            }
            foreach (Motorista motorista in this.Motoristas)
            {
                motorista.Salvar(diretorio_raiz, this.Indice, (uint)this.Altura, (uint)this.Largura);

                if (CancelarEnvioNFX)
                    return;
            }

            //Gera arquivos List(listando arquivos de msg e arquivos de rot);
            List<string> msgs = Directory.EnumerateFiles(diretorio_raiz + Util.Util.DIRETORIO_MSGS, Util.Util.ASTERISCO + Util.Util.ARQUIVO_EXT_MSG).ToList();
            List<string> rots = Directory.EnumerateFiles(diretorio_raiz + Util.Util.DIRETORIO_ROTEIROS, Util.Util.ASTERISCO + Util.Util.ARQUIVO_EXT_ROT).ToList();
            List<string> drvs = Directory.EnumerateFiles(diretorio_raiz + Util.Util.DIRETORIO_DRIVERS, Util.Util.ASTERISCO + Util.Util.ARQUIVO_EXT_DRV).ToList();

            Arquivo_LST msg_lst = new Arquivo_LST();

            foreach (string s in msgs)
            {
                msg_lst.listaPaths.Add(Path.GetFileNameWithoutExtension(s));
            }

            if (CancelarEnvioNFX)
                return;

            Arquivo_LST rot_lst = new Arquivo_LST();

            foreach (string s in rots)
            {
                rot_lst.listaPaths.Add(Path.GetFileNameWithoutExtension(s));
            }

            if (CancelarEnvioNFX)
                return;

            Arquivo_LST drv_lst = new Arquivo_LST();

            foreach (string s in drvs)
            {
                drv_lst.listaPaths.Add(Path.GetFileNameWithoutExtension(s));
            }

            if (CancelarEnvioNFX)
                return;

            msg_lst.AtualizarCRC();
            rot_lst.AtualizarCRC();
            drv_lst.AtualizarCRC();

            msg_lst.Salvar(diretorio_raiz + Util.Util.DIRETORIO_MSGS + Util.Util.ARQUIVO_LST_MSGS);

            if (CancelarEnvioNFX)
                return;

            rot_lst.Salvar(diretorio_raiz + Util.Util.DIRETORIO_ROTEIROS + Util.Util.ARQUIVO_LST_ROTEIROS);

            if (CancelarEnvioNFX)
                return;

            drv_lst.Salvar(diretorio_raiz + Util.Util.DIRETORIO_DRIVERS + Util.Util.ARQUIVO_LST_DRIVERS);
        }

        private void SalvarCFG(String diretorio_raiz, Boolean saudacaoPadrao, string diretorioFontes)
        {
            //Painel.cfg
            Arquivo_CFG painel_cfg = new Arquivo_CFG();

            painel_cfg.alternanciaSelecionada = (byte)this.AlternanciaSelecionada;
            painel_cfg.altura = (uint)this.Altura;
            
            string nome_fonte = Path.GetFileNameWithoutExtension(this.FontePath);
            string nomeFonteLocal = String.Empty;

            List<String> arquivos = new List<string>();
            arquivos.AddRange(Directory.EnumerateFiles(diretorioFontes, "*" + Util.Util.ARQUIVO_EXT_FNT, SearchOption.TopDirectoryOnly));

            if (arquivos.IndexOf(diretorioFontes + nome_fonte + Util.Util.ARQUIVO_EXT_FNT) >= 0)
            {
                nomeFonteLocal = arquivos.IndexOf(diretorioFontes + nome_fonte + Util.Util.ARQUIVO_EXT_FNT).ToString() + Util.Util.ARQUIVO_EXT_FNT;
            }
            else
            {
                nomeFonteLocal = "0" + Util.Util.ARQUIVO_EXT_FNT;
            }


            painel_cfg.BrilhoMax = this.BrilhoMaximo;
            painel_cfg.BrilhoMin = this.BrilhoMinimo;
            painel_cfg.fontePath = Util.Util.DIRETORIO_FONTES_FIRMWARE + nomeFonteLocal;//this.FontePath;
            painel_cfg.largura = (uint)this.Largura;
            painel_cfg.mensagemSelecionada = (UInt16)this.MensagemSelecionada;            

            GerarMensagensEspeciais(painel_cfg, Path.GetDirectoryName(diretorio_raiz));

            GerarOpcoesdeApresentacao(painel_cfg);

            painel_cfg.Salvar(diretorio_raiz);

        }

        private void GerarOpcoesdeApresentacao(Arquivo_CFG painel_cfg)
        {
            //HORA DE SAÍDA
            Util.Util.OpcoesApresentacao opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].TempoRolagem;
            opcoes.tempoApresentacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].TempoApresentacao;
            opcoes.animacao = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].Modelo.Textos[0].Apresentacao;

            //Montando a imagem do texto da hora de saída + a imagem da hora de saída para ver se ultrapassa o tamanho do painel. Se ultrapassar, ao montar a imagem é setado alinhamento a esquerda
            Frase fraseTemp = new Frase(this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida]);
            fraseTemp.LabelFrase = fraseTemp.LabelFrase + " " + this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].LabelFrase;
            fraseTemp.Modelo.Textos[0].LabelTexto = fraseTemp.Modelo.Textos[0].LabelTexto + " " + this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].LabelTexto;
            fraseTemp.PrepararMatrizFrase(this.Altura, this.Largura);

            opcoes.alinhamento = (byte)fraseTemp.Modelo.Textos[0].AlinhamentoH;
            painel_cfg.horaSaida = opcoes;


            //TARIFA
            opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa].Modelo.Textos[0].TempoRolagem;
            opcoes.tempoApresentacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa].Modelo.Textos[0].TempoApresentacao;
            opcoes.animacao = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa].Modelo.Textos[0].Apresentacao;
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa].PrepararMatrizFrase(this.Altura, this.Largura);

            opcoes.alinhamento = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Tarifa].Modelo.Textos[0].AlinhamentoH;
            painel_cfg.tarifa = opcoes;


            //TEMPERATURA
            opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura].Modelo.Textos[0].TempoRolagem;
            opcoes.tempoApresentacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura].Modelo.Textos[0].TempoApresentacao;
            opcoes.animacao = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura].Modelo.Textos[0].Apresentacao;
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura].PrepararMatrizFrase(this.Altura, this.Largura);

            opcoes.alinhamento = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Temperatura].Modelo.Textos[0].AlinhamentoH;
            painel_cfg.temperatura = opcoes;


            //VELOCIDADE
            opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade].Modelo.Textos[0].TempoRolagem;
            opcoes.tempoApresentacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade].Modelo.Textos[0].TempoApresentacao;
            opcoes.animacao = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade].Modelo.Textos[0].Apresentacao;  
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade].PrepararMatrizFrase(this.Altura, this.Largura);

            opcoes.alinhamento = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Velocidade].Modelo.Textos[0].AlinhamentoH;
            painel_cfg.velocidade = opcoes;

            
            //SOMENTE HORA
            opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].TempoRolagem;
            opcoes.tempoApresentacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].TempoApresentacao;
            opcoes.animacao = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].Apresentacao;
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].PrepararMatrizFrase(this.Altura, this.Largura);

            opcoes.alinhamento = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SomenteHora].Modelo.Textos[0].AlinhamentoH;
            painel_cfg.somenteHora = opcoes;


            //DATA HORA
            opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].Modelo.Textos[0].TempoRolagem;
            opcoes.tempoApresentacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].Modelo.Textos[0].TempoApresentacao;
            opcoes.animacao = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].Modelo.Textos[0].Apresentacao; 
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].PrepararMatrizFrase(this.Altura, this.Largura);

            opcoes.alinhamento = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHora].Modelo.Textos[0].AlinhamentoH;
            painel_cfg.dataHora = opcoes; 
            

            //HORA TEMPERATURA                        
            opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura].Modelo.Textos[0].TempoRolagem;
            opcoes.tempoApresentacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura].Modelo.Textos[0].TempoApresentacao;
            opcoes.animacao = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura].Modelo.Textos[0].Apresentacao; 
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura].PrepararMatrizFrase(this.Altura, this.Largura);

            opcoes.alinhamento = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura].Modelo.Textos[0].AlinhamentoH;
            painel_cfg.horaTemperatura = opcoes; 
            

            //DATAHORA TEMPERATURA
            opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura].Modelo.Textos[0].TempoRolagem;
            opcoes.tempoApresentacao = (UInt32)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura].Modelo.Textos[0].TempoApresentacao;
            opcoes.animacao = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura].Modelo.Textos[0].Apresentacao;
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura].PrepararMatrizFrase(this.Altura, this.Largura);

            opcoes.alinhamento = (byte)this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura].Modelo.Textos[0].AlinhamentoH;
            painel_cfg.dataHoraTemp = opcoes;
            
        }

        private void GerarMensagensEspeciais(Arquivo_CFG painel_cfg, String diretorio_raiz)
        {

            //Criando video de Bom dia
            Persistencia.Videos.VideoV02 v02Temp = new Persistencia.Videos.VideoV02();
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.BomDia].Salvar(diretorio_raiz + "//", (uint)this.Altura, (uint)this.Largura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de Boa Tarde
            //v02Temp = new Persistencia.Videos.VideoV02();
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.BoaTarde].Salvar(diretorio_raiz + "//", (uint)this.Altura, (uint)this.Largura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de Boa Noite
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.BoaNoite].Salvar(diretorio_raiz + "//", (uint)this.Altura, (uint)this.Largura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de Hora saida
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.HoraSaida].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de simbolo tarifa
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloTarifa].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp)); 

            //Criando video de unidade temperatura
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloTemperatura].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));
           

            //Criando video de unidade velocidade
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloVelocidade].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de segunda
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Segunda].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de terça
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Terca].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de Quarta
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Quarta].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de Quinta
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Quinta].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de Sexta
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Sexta].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de Sabado
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Sabado].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de Domingo
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.Domingo].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de extensao AM
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloAM_Espaco].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));

            //Criando video de extensao PM
            this.MensagensEspeciais.Frases[(int)Util.Util.IndiceMensagensEspeciais.SimboloPM_Ponto].GerarVideo02ApenasImagem(diretorio_raiz + "//", (uint)this.Altura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(new Persistencia.Videos.VideoV02(v02Temp));
            
        }

        /// <summary>
        /// Gera Objetos de vídeo padrão para as saudações padrão.
        /// </summary>
        /// <param name="painel_cfg">Arquivo CFG a ser gerado.</param>
        /// <param name="diretorio_raiz">Diretorio do arquivo CFG sem o nome do arquivo. Apenas o diretório.</param>
        private void GerarSaudacaoPadrao(Arquivo_CFG painel_cfg, String diretorio_raiz)
        {
            Persistencia.Videos.VideoV02 v02Temp = new Persistencia.Videos.VideoV02();

            // BOM DIA
            this.MensagensEspeciais.Frases[0].Salvar(diretorio_raiz + "//", (uint)this.Altura, (uint)this.Largura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
            {
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            }
            Util.Util.sequencial_arquivo_videos++;
            painel_cfg.videosMensagensEspeciais.Add(v02Temp);

            v02Temp = new Persistencia.Videos.VideoV02();

            // BOA TARDE
            this.MensagensEspeciais.Frases[1].Salvar(diretorio_raiz + "//", (uint)this.Altura, (uint)this.Largura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
            {
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            }
            Util.Util.sequencial_arquivo_videos++;
            //painel_cfg.VideosSaudacao.Add(v02Temp);
            painel_cfg.videosMensagensEspeciais.Add(v02Temp);

            // BOA NOITE
            this.MensagensEspeciais.Frases[2].Salvar(diretorio_raiz + "//", (uint)this.Altura, (uint)this.Largura);
            v02Temp.Abrir(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            
            if (File.Exists(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao()))
            {
                File.Delete(diretorio_raiz + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.GetUltimaExtensao());
            }
            Util.Util.sequencial_arquivo_videos++;

            painel_cfg.videosMensagensEspeciais.Add(v02Temp);
        }

        private void SalvarAlternancia(String diretorio_raiz)
        {            
            Arquivo_ALT aalt = new Arquivo_ALT(this.rm);

            aalt.ArquivoNome = diretorio_raiz;
            aalt.listaAlternancias.Clear();

            if (this.ListaAlternancias.Count == 0) // 
            {            
                aalt.CriarAlternanciasDefault();
            }
            else
            {                
                aalt.listaAlternancias.AddRange(this.ListaAlternancias.ToArray());
                aalt.Salvar(diretorio_raiz);
            }
        }

        public void Abrir(String diretorio_raiz)
        {
            //Abrir arquivo de alternância.
            
            //Arquivo_ALT aalt = new Arquivo_ALT();
            
            //Arquivo CFG;
            Arquivo_CFG acfg = new Arquivo_CFG();
            //acfg.CriarConfiguracaoDefault();
            acfg.Abrir(diretorio_raiz + Util.Util.ARQUIVO_CFG);

            this.Altura = (int)acfg.altura;
            this.Largura = (int)acfg.largura;
            this.AlternanciaSelecionada = acfg.alternanciaSelecionada;
            this.FontePath = acfg.fontePath;
            this.MensagemSelecionada = (int)acfg.mensagemSelecionada;
            
            this.CarregaRoteiros(diretorio_raiz);
            this.CarregaMensagens(diretorio_raiz);

            ////Mensagens 
            ////enumera os arquivos .mpt
            //List<string> mpts = Directory.EnumerateFiles(arquivoNome + Util.Util.DIRETORIO_MSGS, "*.mpt").ToList();

            //for (int arquivo = 0; arquivo < mpts.Count; arquivo++)
            //{
            //    this.Mensagens.Add(new Mensagem());
            //    this.Mensagens[arquivo].Id = arquivo; //System.Convert.ToInt32(rpts[arquivo].Substring(0, 3));
            //    this.Mensagens[arquivo].Abrir(arquivoNome + Util.Util.DIRETORIO_MSGS + @"\" + mpts[arquivo]); //abre o rpt.
            //}


        }

        public void CarregaRoteiros(String diretorio_raiz)
        {
            ////O nome do arquivo de RPT é o mesmo nome do arquivo de roteiro a ser carregado.
            //List<string> rpts = Directory.EnumerateFiles(diretorio_raiz + Util.Util.DIRETORIO_ROTEIROS, @"*" + Util.Util.ARQUIVO_EXT_RPT).ToList();
            //List<string> nomeRots = new List<string>();


            ////coleta os rots de acordo como os nomes dos rpts indicam.
            //for (int indiceRPTs = 0; indiceRPTs < rpts.Count; indiceRPTs++)
            //{
            //    String NomeArquivoTemp = String.Empty;
            //    int indice_inicio = diretorio_raiz.Count() + Util.Util.DIRETORIO_ROTEIROS.Count();

            //    NomeArquivoTemp = rpts[indiceRPTs].Substring(indice_inicio,
            //                                                 rpts[indiceRPTs].Count() -
            //                                                 indice_inicio -
            //                                                 Util.Util.ARQUIVO_EXT_RPT.Count()
            //                                                 );

            //    nomeRots.Add(diretorio_raiz +
            //                 Util.Util.DIRETORIO_PAI +
            //                 Util.Util.DIRETORIO_PAI +
            //                 Util.Util.DIRETORIO_ROTEIROS +
            //                 NomeArquivoTemp + Util.Util.ARQUIVO_EXT_ROT);

            //}

            ////Carrega os roteiros dos .rots e adiciona no painel.
            //for (int roteiro = 0; roteiro < nomeRots.Count; roteiro++)
            //{
            //    Roteiro rTemp = new Roteiro();
            //    Arquivo_ROT arot = new Arquivo_ROT();

            //    arot.Abrir(nomeRots[roteiro]);

            //    rTemp.Id = arot.id;
            //    rTemp.Indice = roteiro;
            //    rTemp.IndicePainel = this.Indice;
            //    rTemp.LabelNumero = arot.labelNumero;
            //    rTemp.LabelRoteiroIda = arot.labelRoteiro;
            //    rTemp.LabelRoteiroVolta = arot.labelRoteiro;
            //    rTemp.RPT = rpts[roteiro];

            //    this.Roteiros.Add(rTemp);

            //    //this.Roteiros[roteiro].Abrir(diretorio_raiz);
            //    this.Roteiros[roteiro].AbrirFormatoNovo(diretorio_raiz);
            //}

        }

      
        public void CarregaMensagens(String diretorio_raiz)
        {

            //O nome do arquivo de MPT é o mesmo nome do arquivo de mensagem a ser carregada.
            /*List<string> mpts = Directory.EnumerateFiles(diretorio_raiz + Util.Util.DIRETORIO_MSGS, @"*" + Util.Util.ARQUIVO_EXT_MPT).ToList();
            List<string> nomeMSGs = new List<string>();
=======
            ////O nome do arquivo de MPT é o mesmo nome do arquivo de mensagem a ser carregada.
            //List<string> mpts = Directory.EnumerateFiles(diretorio_raiz + Util.Util.DIRETORIO_MSGS, @"*" + Util.Util.ARQUIVO_EXT_MPT).ToList();
            //List<string> nomeMSGs = new List<string>();
>>>>>>> .r4107

<<<<<<< .mine
            //todo: terminar mensagens de saudação, ainda há coisas a serem definidas por firmware e software.
            //carrega as mensagens de saudações.
           // this.CarregaSalutations(diretorio_raiz);
=======
            ////todo: terminar mensagens de saudação, ainda há coisas a serem definidas por firmware e software.
            ////carrega as mensagens de saudações.
            //this.CarregaSalutations(diretorio_raiz);
>>>>>>> .r4107

            ////coleta os nomes dos msgs de acordo como os nomes que os mpts indicam.
            //for (int indiceMPTs = 0; indiceMPTs < mpts.Count; indiceMPTs++)
            //{
            //    String NomeArquivoTemp = String.Empty;
            //    int indice_inicio = diretorio_raiz.Count() + Util.Util.DIRETORIO_MSGS.Count();

            //    NomeArquivoTemp = mpts[indiceMPTs].Substring(indice_inicio,
            //                                                 mpts[indiceMPTs].Count() -
            //                                                 indice_inicio -
            //                                                 Util.Util.ARQUIVO_EXT_MPT.Count()
            //                                                 );

            //    nomeMSGs.Add(diretorio_raiz +
            //                 Util.Util.DIRETORIO_PAI +
            //                 Util.Util.DIRETORIO_PAI +
            //                 Util.Util.DIRETORIO_MSGS +
            //                 NomeArquivoTemp + Util.Util.ARQUIVO_EXT_MSG);

            //}

            ////todo: ordenar as mensagens para não ocorrer diferenças.
            ////Carrega as mensagens dos .msgs e adiciona no painel.
            //for (int mensagem = 0; mensagem < nomeMSGs.Count; mensagem++)
            //{
            //    Mensagem mTemp = new Mensagem();
            //    Arquivo_MSG amsg = new Arquivo_MSG();

            //    amsg.Abrir(nomeMSGs[mensagem]);

            //    mTemp.ID = amsg.id;
            //    mTemp.Indice = mensagem;
            //    //mTemp.IndicePainel = this.Indice;
            //    mTemp.LabelMensagem = amsg.labelMensagem;
            //    mTemp.MPT = mpts[mensagem];

            //    this.Mensagens.Add(mTemp);

            //    //this.Mensagens[mensagem].Abrir(diretorio_raiz);
            //    this.Mensagens[mensagem].AbrirFormatoNovo(diretorio_raiz);
            //}
            */
        }
    }

    





}
