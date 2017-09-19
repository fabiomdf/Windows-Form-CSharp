using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistencia.Videos;

namespace Persistencia
{
    public static class UtilPersistencia
    {
        public static List<Video> GravaVideo(List<Video> lVideo, List<Video> vOld)
        {
            foreach (Video v in vOld)
            {
                if (v is VideoV01)
                    lVideo.Add(new VideoV01((VideoV01)v));
                else if (v is VideoV02)
                    lVideo.Add(new VideoV02((VideoV02)v));
            }

            return lVideo;
        }
    }
}
