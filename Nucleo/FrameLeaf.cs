using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistencia.Videos;


namespace Nucleo
{
    public class FrameLeaf : IFrame
    {
        public FrameLeaf()
        {
            this.PlayList = new List<Persistencia.Videos.IVideo>();
            Altura = 0;
            Largura = 0;
        }

        public List<Persistencia.Videos.IVideo> PlayList { get; set; }

        //indice do frame leaf numa lista de IFrame.
        public int Indice { get; set; }

        public int Altura
        {
            get; set;
        }

        public int Largura
        {
            get; set; }
    
        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Salvar(String arquivoNome)
        {
            foreach (IVideo iv in this.PlayList)
            {
                iv.Salvar(arquivoNome);
            }
        }

        public IFrame Abrir(String arquivoNome)
        {
            FrameLeaf fl = new FrameLeaf();

            

            return fl;
        }

        public void AdicionarVideo(IVideo novoVideo)
        {
            PlayList.Add(novoVideo);
        }

        public void RemoverVideo(IVideo item)
        {
            PlayList.Remove(item);
        }

        public IVideo GetVideo(int indice)
        {
            return PlayList[indice];
        }
    }
}
