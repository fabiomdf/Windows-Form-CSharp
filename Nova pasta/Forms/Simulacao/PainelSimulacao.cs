using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controlador;
using System.Threading;

namespace PontosX2.Forms.Simulacao
{
    public partial class PainelSimulacao : UserControl
    {

        List<VideoBitmap.VideoBitmap> listaVideoBitMap = new List<VideoBitmap.VideoBitmap>();
        public GroupBox gBox = new GroupBox();
        public Panel panelVideoBitmap = new Panel();
        public Label label = new Label();
        Fachada fachada = Fachada.Instance;        
        Frase fraseAnterior = null;
        public Painel painel;
     

        public PainelSimulacao()
        {
            InitializeComponent();            
            gBox.Location = new Point(5, 0);
            panelVideoBitmap.BorderStyle = BorderStyle.Fixed3D;
            label.AutoSize = true;
            gBox.Controls.Add(label);
            gBox.Controls.Add(panelVideoBitmap);
            this.Controls.Add(gBox);
        }

        public void SetarPainelVisivel(bool visible)
        {
            MethodInvoker invoker = delegate
            {
                panelVideoBitmap.Visible = visible;
                //listaUserControlPaineis[indicePainel].SetarPainelVisivel(true);
                //listaUserControlPaineis[indicePainel].Visible = true;
            };
            panelVideoBitmap.Invoke(invoker);
        }

        public void MontarPainel()
        {

            listaVideoBitMap.Clear();
            listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(painel.Altura, painel.Largura));

            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
            listaVideoBitMap[0].Location = new Point(0, 0);

            panelVideoBitmap.Width = listaVideoBitMap[0].Width + 3;
            panelVideoBitmap.Height = listaVideoBitMap[0].Height + 3;

            gBox.Height = panelVideoBitmap.Height + 22;
            gBox.Width = panelVideoBitmap.Width + 15;

            panelVideoBitmap.Location = new Point(gBox.Width / 2 - panelVideoBitmap.Size.Width / 2, gBox.Height / 2 - ((panelVideoBitmap.Size.Height / 2) - 5));
            label.Location = new Point(gBox.Width / 2 - label.Size.Width / 2, 0);

            this.Width = gBox.Width + 10;
            this.Height = gBox.Height;

        }

        public void ReposicionarVideoMapNoPainel(Frase f, bool multiLinhas)
        {
            bool alterou = VerificarAlteracaoPaineisBitmap(fraseAnterior, f);

            fraseAnterior = new Frase(f);

            if (alterou)
            {
                listaVideoBitMap.Clear();
                panelVideoBitmap.Controls.Clear();
            } 
                
            if (f.TipoVideo == Util.Util.TipoVideo.V02)
            {
                if (alterou)
                { 
                    listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                    panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                    listaVideoBitMap[0].Location = new Point(0, 0);
                        
                    listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                }

                listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
            }
            
            if (f.TipoVideo == Util.Util.TipoVideo.V04)
            {
                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.Texto)
                {
                    if (alterou)
                    {
                        if (!multiLinhas)
                        {
                            listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                            panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                            listaVideoBitMap[0].Location = new Point(0, 0);

                            listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        }
                        else
                        {
                            for(int i = 0; i < f.Modelo.Textos.Count; i++)
                            {
                                listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[i].listaBitMap[0].GetLength(0), f.Modelo.Textos[i].listaBitMap[0].GetLength(1)));
                                panelVideoBitmap.Controls.Add(listaVideoBitMap[i]);
                                if (i==0)
                                    listaVideoBitMap[i].Location = new Point(0, 0);
                                else
                                    listaVideoBitMap[i].Location = new Point(0, listaVideoBitMap[i-1].Location.Y + listaVideoBitMap[i-1].Height);
                                listaVideoBitMap[i].Apaga(0, f.Modelo.Textos[i].InverterLed ? Color.Yellow : Color.Black);
                            }
                        }
                    }

                    if (!multiLinhas)
                        listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    else
                    {
                        for (int i = 0; i < f.Modelo.Textos.Count; i++)
                            listaVideoBitMap[i].listaBitMap = f.Modelo.Textos[i].listaBitMap;
                    }
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTexto || f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoNúmero)
                {

                    if (alterou)
                    { 
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);

                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);

                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuplo)
                {
                    if (alterou)
                    { 
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(0, listaVideoBitMap[0].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;                       
                }


                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NúmeroTextoDuplo)
                {
                    if (alterou)
                    { 
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploNúmero)
                {

                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));

                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);

                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);

                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;  
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoDuploTextoDuplo)
                {
                    if (alterou)
                    { 

                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[3].listaBitMap[0].GetLength(0), f.Modelo.Textos[3].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);
                        listaVideoBitMap[3].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[3].Apaga(0, f.Modelo.Textos[3].InverterLed ? Color.Yellow : Color.Black);

                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[3].listaBitMap = f.Modelo.Textos[3].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriplo)
                {
                    if (alterou)
                    {
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(0, listaVideoBitMap[0].Height);
                        listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height + listaVideoBitMap[1].Height);

                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);

                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
                {
                    if (alterou)
                    {

                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[3].listaBitMap[0].GetLength(0), f.Modelo.Textos[3].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                        listaVideoBitMap[3].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height + listaVideoBitMap[2].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[3].Apaga(0, f.Modelo.Textos[3].InverterLed ? Color.Yellow : Color.Black);

                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[3].listaBitMap = f.Modelo.Textos[3].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.NumeroTextoTriplo)
                {
                    if (alterou)
                    {

                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[3].listaBitMap[0].GetLength(0), f.Modelo.Textos[3].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height);
                        listaVideoBitMap[3].Location = new Point(listaVideoBitMap[0].Width, listaVideoBitMap[1].Height + listaVideoBitMap[2].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[3].Apaga(0, f.Modelo.Textos[3].InverterLed ? Color.Yellow : Color.Black);

                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[3].listaBitMap = f.Modelo.Textos[3].listaBitMap;
                }

                if (f.Modelo.TipoModelo == Util.Util.TipoModelo.TextoTriploNumero)
                {
                    if (alterou)
                    {

                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[0].listaBitMap[0].GetLength(0), f.Modelo.Textos[0].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[1].listaBitMap[0].GetLength(0), f.Modelo.Textos[1].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[2].listaBitMap[0].GetLength(0), f.Modelo.Textos[2].listaBitMap[0].GetLength(1)));
                        listaVideoBitMap.Add(new VideoBitmap.VideoBitmap(f.Modelo.Textos[3].listaBitMap[0].GetLength(0), f.Modelo.Textos[3].listaBitMap[0].GetLength(1)));
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[0]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[1]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[2]);
                        panelVideoBitmap.Controls.Add(listaVideoBitMap[3]);
                        listaVideoBitMap[0].Location = new Point(0, 0);
                        listaVideoBitMap[1].Location = new Point(listaVideoBitMap[0].Width, 0);
                        listaVideoBitMap[2].Location = new Point(0, listaVideoBitMap[0].Height);
                        listaVideoBitMap[3].Location = new Point(0, listaVideoBitMap[0].Height + listaVideoBitMap[2].Height);
                        listaVideoBitMap[0].Apaga(0, f.Modelo.Textos[0].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[1].Apaga(0, f.Modelo.Textos[1].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[2].Apaga(0, f.Modelo.Textos[2].InverterLed ? Color.Yellow : Color.Black);
                        listaVideoBitMap[3].Apaga(0, f.Modelo.Textos[3].InverterLed ? Color.Yellow : Color.Black);

                    }

                    listaVideoBitMap[0].listaBitMap = f.Modelo.Textos[0].listaBitMap;
                    listaVideoBitMap[1].listaBitMap = f.Modelo.Textos[1].listaBitMap;
                    listaVideoBitMap[2].listaBitMap = f.Modelo.Textos[2].listaBitMap;
                    listaVideoBitMap[3].listaBitMap = f.Modelo.Textos[3].listaBitMap;
                }
            }
        }


        private bool VerificarAlteracaoPaineisBitmap(Frase fraseAnterior, Frase fraseAtual)
        {
            //verificação se foi alterado algum painel
            bool alterou = false;

            if (fraseAnterior != null)
            {
                if (fraseAnterior.TipoVideo != fraseAtual.TipoVideo)
                    alterou = true;

                if (!alterou)
                {
                    if (fraseAtual.TipoVideo == Util.Util.TipoVideo.V04)
                    {
                        if (fraseAnterior.Modelo.TipoModelo != fraseAtual.Modelo.TipoModelo)
                            alterou = true;
                        else
                        {
                            for (int i = 0; i < fraseAtual.Modelo.Textos.Count; i++)
                            {
                                if (fraseAtual.Modelo.Textos[i].listaBitMap[0].GetLength(0) != fraseAnterior.Modelo.Textos[i].listaBitMap[0].GetLength(0) || fraseAtual.Modelo.Textos[i].listaBitMap[0].GetLength(1) != fraseAnterior.Modelo.Textos[i].listaBitMap[0].GetLength(1))
                                {
                                    alterou = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
                alterou = true;

            return alterou;
        }

        public void ApresentarTexto(int indice, Texto t)
        {
            listaVideoBitMap[indice].Apresentar(t);
        }

        public void PararSimulacao()
        {
            for (int i = 0; i < listaVideoBitMap.Count; i++)
                listaVideoBitMap[i].Cancelar = true;
        }
    }
}