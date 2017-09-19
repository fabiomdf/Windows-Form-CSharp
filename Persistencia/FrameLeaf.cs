using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistencia.Videos;

namespace Persistencia
{
    public class FrameLeaf : IFrame
    {
        public List<Video> PlayList { get; set; }
        private int altura;
        private int largura;

        public FrameLeaf()
        {
            this.PlayList = new List<Video>();
            Altura = 0;
            Largura = 0;
        }

        public FrameLeaf(FrameLeaf oldValue)
        {
            this.altura = oldValue.altura;
            this.largura = oldValue.largura;

            this.PlayList = UtilPersistencia.GravaVideo(this.PlayList, oldValue.PlayList);
        }

        public int Altura
        {
            get
            {
                return this.altura;
            }
            set
            {
                this.altura = value;
            }
        }

        public int Largura
        {
            get
            {
                return this.largura;
            }
            set
            {
                this.largura = value;
            }
        }
    
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

        public void AdicionarVideo(Video novoVideo)
        {
            PlayList.Add(novoVideo);
        }

        public void RemoverVideo(Video item)
        {
            PlayList.Remove(item);
        }

        public Video GetVideo(int indice)
        {
            return PlayList[indice];
        }

        //private List<Video> GravaVideo(List<Video> lVideo, List<Video> vOld)
        //{
        //    foreach (Video v in vOld)
        //    {
        //        if (v is VideoV01)
        //            lVideo.Add(new VideoV01((VideoV01)v));
        //        else if (v is VideoV02)
        //            lVideo.Add(new VideoV02((VideoV02)v));
        //    }

        //    return lVideo;
        //}

    }
}
