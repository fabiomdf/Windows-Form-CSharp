using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucleo
{
    public class FrameSet : IFrame
    {
        public List<IFrame> Children
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Pontos
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        //indice do frameSet numa lista de IFrame.
        public int Indice { get; set; }

        public Orientacao Orientacao
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    
        public void Play()
        {
            foreach (IFrame child in Children)
            {
                child.Play();
            }
        }

        public void Stop()
        {
            foreach (IFrame child in Children)
            {
                child.Stop();
            }
        }

        public void Clear()
        {
            foreach (IFrame child in Children)
            {
                child.Clear();
            }
        }

        public void Salvar(String arquivoNome)
        {
            foreach (IFrame child in Children)
            {
                child.Salvar(arquivoNome);
            }
        }

        public IFrame Abrir(String arquivoNome)
        {
            foreach (IFrame child in Children)
            {
                child.Abrir(arquivoNome);
            }

            return Children[0];
            //return null;
        }

        public void AdicionarFrame(IFrame novoFrame)
        {
            Children.Add(novoFrame);
        }

        public void RemoverFrame(IFrame item)
        {
            Children.Remove(item);
        }

        public IFrame GetChild(int indice)
        {
            return Children[indice];
        }
    }
}
