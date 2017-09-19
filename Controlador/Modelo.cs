using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controlador
{
    public class Modelo
    {
        public List<Texto> Textos { get; set; }
        public Util.Util.TipoModelo TipoModelo { get; set; }

        public Modelo() 
        {
            this.Textos = new List<Texto>();
            this.TipoModelo = Util.Util.TipoModelo.NúmeroTexto;
        }

        public Modelo(string numeroLabel)
        {
            this.Textos = new List<Texto>();
            this.Textos.Add(new Texto(numeroLabel));
            this.TipoModelo = Util.Util.TipoModelo.Texto;
        }

        public Modelo(Modelo modelo_antigo)
        {
            this.Textos = new List<Texto>();

            foreach (Texto t in modelo_antigo.Textos) 
            {
                this.Textos.Add(new Texto(t));
            }
            this.TipoModelo = modelo_antigo.TipoModelo;
        }

        public bool CompararObjetosModelo(Modelo modelo1, Modelo modelo2)
        {
            bool alterou = false;
            if (modelo1.TipoModelo != modelo2.TipoModelo)
                alterou = true;

            if (modelo1.Textos.Count!=modelo2.Textos.Count)
                alterou = true;
            
            if (!alterou)
            {
                //os dois modelos tem a mesma quantidade de textos
                for(int i=0; i<modelo1.Textos.Count;i++)
                {
                    if (modelo1.Textos[i].CompararObjetosTexto(modelo1.Textos[i], modelo2.Textos[i]))
                    { 
                        alterou = true;
                        break;
                    }
                }
            }

            return alterou;
        }
    }
}
