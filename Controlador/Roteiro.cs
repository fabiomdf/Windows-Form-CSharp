using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nucleo;
using Persistencia;
using Persistencia.Videos;
using Util;
//using FrameLeaf = Nucleo.FrameLeaf;

namespace Controlador
{
    public class Roteiro
    {

        public int ID { get; set;}
        public int Indice { get; set;}
        public Frase Numero { get; set;}
        public bool IdaIgualVolta { get; set;}
        public string LabelRoteiro { get; set;}
        public int Tarifa { get; set;} 
        public List<Frase> FrasesIda { get; set; }
        public List<Frase> FrasesVolta { get; set; }
        public int Ordenacao = 255;
        public bool Ascendente;
        public Util.Util.EnvioRoteiroAPP EnvioRoteiroAPP { get; set; }


        public Roteiro()
        {
            this.FrasesIda = new List<Frase>();
            this.FrasesVolta = new List<Frase>();
            this.Numero = new Frase("00");
            this.IdaIgualVolta = true;
            this.Tarifa = 0;
            this.EnvioRoteiroAPP = Util.Util.EnvioRoteiroAPP.NaoTemRotaAPP;
        }

        public Roteiro(string numero)
        {
            this.FrasesIda = new List<Frase>();
            this.FrasesVolta = new List<Frase>();
            this.Numero = new Frase(numero);
            this.IdaIgualVolta = true;
            this.Tarifa = 0;
            this.EnvioRoteiroAPP = Util.Util.EnvioRoteiroAPP.NaoTemRotaAPP;
        }

        
        public Roteiro(Roteiro roteiro_antigo, bool copiarFrases)
        {
            
            this.FrasesIda = new List<Frase>();
            this.FrasesVolta = new List<Frase>();
            
            this.ID = roteiro_antigo.ID;
            this.Indice = roteiro_antigo.Indice;
            this.IdaIgualVolta = roteiro_antigo.IdaIgualVolta;
            this.LabelRoteiro = roteiro_antigo.LabelRoteiro;
            this.Tarifa = roteiro_antigo.Tarifa;
            this.Ordenacao = roteiro_antigo.Ordenacao;
            this.Ascendente = roteiro_antigo.Ascendente;
            this.EnvioRoteiroAPP = roteiro_antigo.EnvioRoteiroAPP;
            this.Numero = new Frase(roteiro_antigo.Numero);            

            if (copiarFrases) 
            { 
                foreach (Frase f in roteiro_antigo.FrasesIda)
                {
                    this.FrasesIda.Add(new Frase(f));
                }
                foreach (Frase f in roteiro_antigo.FrasesVolta)
                {
                    this.FrasesVolta.Add(new Frase(f));
                }
            }
        }

        public void MoverFrases(bool isFraseIda,int posicaoInicial, int posicaoFinal)
        {
            if (isFraseIda)
            {
                Frase aux = this.FrasesIda[posicaoFinal];
                this.FrasesIda[posicaoFinal] = this.FrasesIda[posicaoInicial];
                this.FrasesIda[posicaoInicial] = aux;
            }
            else
            {
                Frase aux = this.FrasesVolta[posicaoFinal];
                this.FrasesVolta[posicaoFinal] = this.FrasesVolta[posicaoInicial];
                this.FrasesVolta[posicaoInicial] = aux;
            }
        }

        public bool CompararObjetosRoteiro(Roteiro roteiro1, Roteiro roteiro2)
        {
            bool alterou = false;

            if ((roteiro1.ID != roteiro2.ID) || (roteiro1.Indice != roteiro2.Indice) || (roteiro1.LabelRoteiro != roteiro2.LabelRoteiro) ||
                (roteiro1.IdaIgualVolta != roteiro2.IdaIgualVolta) || (roteiro1.Tarifa != roteiro2.Tarifa) || roteiro1.EnvioRoteiroAPP != roteiro2.EnvioRoteiroAPP)
                alterou = true;

            if (!alterou)
            {
                if (roteiro1.Numero.CompararObjetosFrase(roteiro1.Numero,roteiro2.Numero))
                    alterou = true;
            }

            if (!alterou)
                if (roteiro1.FrasesIda.Count!=roteiro2.FrasesIda.Count)
                    alterou = true;

            if (!alterou)
                if (roteiro1.FrasesVolta.Count!=roteiro2.FrasesVolta.Count)
                    alterou = true;

            if (!alterou)
            {
                //os dois roteiros tem a mesma quantidade de frases
                for(int i=0; i<roteiro1.FrasesIda.Count;i++)
                {
                    if (roteiro1.FrasesIda[i].CompararObjetosFrase(roteiro1.FrasesIda[i],roteiro2.FrasesIda[i]))
                    { 
                        alterou = true;
                        break;
                    }
                }
            }

            if (!alterou)
            {
                //os dois roteiros tem a mesma quantidade de frases
                for(int i=0; i<roteiro1.FrasesVolta.Count;i++)
                {
                    if (roteiro1.FrasesVolta[i].CompararObjetosFrase(roteiro1.FrasesVolta[i],roteiro2.FrasesVolta[i]))
                    { 
                        alterou = true;
                        break;
                    }
                }
            }
                    
            return alterou;
        }


        public void GerarArquivosROT(String diretorioRaiz, bool modoApresentacaoDisplayLD6, bool idaIgualVolta)
        {
            Arquivo_ROT arot = new Arquivo_ROT();

            //arot.CriarRoteirosPadrao();
            arot.id = (ushort)this.ID;
            arot.labelNumero = this.Numero.LabelFrase;
            arot.labelRoteiro = this.LabelRoteiro;
            arot.labelRoteiroVolta = this.LabelRoteiro;
            if (modoApresentacaoDisplayLD6)
            {
                //arot.labelRoteiro = (string.IsNullOrEmpty(this.FrasesIda[0].LabelFrase))?this.Numero.LabelFrase: this.FrasesIda[0].LabelFrase;
                if ((null != this.FrasesIda) && this.FrasesIda.Count > 0)
                {
                    arot.labelRoteiro = (string.IsNullOrEmpty(this.FrasesIda[0].LabelFrase)) ? this.Numero.LabelFrase : this.FrasesIda[0].LabelFrase;
                }
                else
                {
                    arot.labelRoteiro = this.Numero.LabelFrase;
                }


                if (idaIgualVolta)
                {
                    arot.labelRoteiroVolta = arot.labelRoteiro;
                }
                else
                {
                    if ((null != this.FrasesVolta) && this.FrasesVolta.Count > 0)
                    {
                        arot.labelRoteiroVolta = (string.IsNullOrEmpty(this.FrasesVolta[0].LabelFrase)) ? this.Numero.LabelFrase : this.FrasesVolta[0].LabelFrase;
                    }
                    else
                    {
                        arot.labelRoteiroVolta = this.Numero.LabelFrase;
                    }
                }
               
            }

            arot.tarifa = (UInt32)this.Tarifa;

            arot.Salvar(diretorioRaiz + 
                        Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                        Util.Util.DIRETORIO_ROTEIROS +
                        Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                        Util.Util.sequencial_arquivo_roteiros.ToString("X8") +
                        Util.Util.ARQUIVO_EXT_ROT
                        );
        }

        public void GerarArquivosRPT(String arquivoNome, int indicePainel, uint altura, uint largura)
        {
            Arquivo_RPT arpt = new Arquivo_RPT();
            String conteudo_rpt_ida = string.Empty;
            String conteudo_rpt_volta = string.Empty;
            String conteudo_rpt_numero = string.Empty;

            List<String> RPTContentIda = new List<string>();
            List<String> RPTContentVolta = new List<string>();
            List<String> RPTContentNumero = new List<string>();

            GerarPlayList(arquivoNome + Util.Util.DIRETORIO_VIDEOS, RPTContentIda, RPTContentVolta, RPTContentNumero, altura, largura);


            if (RPTContentIda.Count > 1)
            {
                conteudo_rpt_ida = CriarPlaylist(arquivoNome + Util.Util.DIRETORIO_VIDEOS, RPTContentIda);
            }
            else
            {
                conteudo_rpt_ida = RPTContentIda[0];
            }
            if (IdaIgualVolta)
            { conteudo_rpt_volta = conteudo_rpt_ida; }
            else
            {
                if (RPTContentVolta.Count > 1)
                {
                    conteudo_rpt_volta = CriarPlaylist(arquivoNome + Util.Util.DIRETORIO_VIDEOS, RPTContentVolta);
                }
                else
                {
                    conteudo_rpt_volta = RPTContentVolta[0];
                }
            }
            conteudo_rpt_numero = RPTContentNumero[0];
            

            arpt.pathExibicaoIda = Util.Util.TrataDiretorio(conteudo_rpt_ida);
            arpt.pathExibicaoVolta = Util.Util.TrataDiretorio(conteudo_rpt_volta);
            arpt.pathNumeroRoteiro = Util.Util.TrataDiretorio(conteudo_rpt_numero);
            string raiz = arquivoNome;
            arquivoNome = arquivoNome +
                          Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                          Util.Util.DIRETORIO_PAINEL +
                          Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                          indicePainel.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS) +
                          Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                          Util.Util.DIRETORIO_ROTEIROS;

            arpt.Salvar(arquivoNome);

            arpt.GerarFormatoNovo(arquivoNome, raiz);//+
                                                //Util.Util.DIRETORIO_PAI +
                                                //Util.Util.DIRETORIO_PAI +
                                                //Util.Util.DIRETORIO_PAI +
                                                //Util.Util.DIRETORIO_PAI);
        }
        
        private void GerarPlayList(string arquivoNome, List<string> PlaylistIda, List<string> PlaylistVolta, List<string> PlaylistNumero, uint altura, uint largura)
        {
            PlaylistIda.Clear();
            PlaylistNumero.Clear();
            PlaylistVolta.Clear();

            string extensao = String.Empty;
            foreach (Frase f in FrasesIda)
            {                
                switch (f.TipoVideo)
                {
                    case Util.Util.TipoVideo.PLS: extensao = Util.Util.ARQUIVO_EXT_PLS;
                        break;
                    case Util.Util.TipoVideo.V01: extensao = Util.Util.ARQUIVO_EXT_V01;
                        break;
                    case Util.Util.TipoVideo.V02: extensao = Util.Util.ARQUIVO_EXT_V02;
                        break;
                    case Util.Util.TipoVideo.V03: extensao = Util.Util.ARQUIVO_EXT_V03;
                        break;
                    case Util.Util.TipoVideo.V04: extensao = Util.Util.ARQUIVO_EXT_V04;
                        break;
                }
                f.Salvar(arquivoNome + "//", altura, largura);
                PlaylistIda.Add(arquivoNome + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + extensao);
                Util.Util.sequencial_arquivo_videos++;
            }
            foreach (Frase f in FrasesVolta)
            {
                switch (f.TipoVideo)
                {
                    case Util.Util.TipoVideo.PLS: extensao = Util.Util.ARQUIVO_EXT_PLS;
                        break;
                    case Util.Util.TipoVideo.V01: extensao = Util.Util.ARQUIVO_EXT_V01;
                        break;
                    case Util.Util.TipoVideo.V02: extensao = Util.Util.ARQUIVO_EXT_V02;
                        break;
                    case Util.Util.TipoVideo.V03: extensao = Util.Util.ARQUIVO_EXT_V03;
                        break;
                    case Util.Util.TipoVideo.V04: extensao = Util.Util.ARQUIVO_EXT_V04;
                        break;
                }
                f.Salvar(arquivoNome + "//", altura, largura);
                PlaylistVolta.Add(arquivoNome + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + extensao);
                Util.Util.sequencial_arquivo_videos++;
            }            
            
            switch (this.Numero.TipoVideo)
            {
                case Util.Util.TipoVideo.PLS: extensao = Util.Util.ARQUIVO_EXT_PLS;
                    break;
                case Util.Util.TipoVideo.V01: extensao = Util.Util.ARQUIVO_EXT_V01;
                    break;
                case Util.Util.TipoVideo.V02: extensao = Util.Util.ARQUIVO_EXT_V02;
                    break;
                case Util.Util.TipoVideo.V03: extensao = Util.Util.ARQUIVO_EXT_V03;
                    break;
                case Util.Util.TipoVideo.V04: extensao = Util.Util.ARQUIVO_EXT_V04;
                    break;
            }
            this.Numero.Salvar(arquivoNome + "//", altura, largura);
            PlaylistNumero.Add(arquivoNome + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + extensao);
            
            if (FrasesIda.Count == 0)
            {
                PlaylistIda.Add(arquivoNome + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + extensao);    
            }
            if (this.IdaIgualVolta)
            {
                PlaylistVolta.AddRange(PlaylistIda.ToArray());
            }
            else
            {
                if (FrasesVolta.Count == 0)
                {
                    PlaylistVolta.Add(arquivoNome + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + extensao);
                }
            }
            Util.Util.sequencial_arquivo_videos++;
        }
        

        private String CriarPlaylist(String diretorio, List<String> ListaArquivos)
        {
            String arquivoPls = diretorio + 
                                  //Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS+
                                  //Util.Util.DIRETORIO_VIDEOS +
                                  Util.Util.sequencial_arquivo_videos.ToString("X8");

            //Arquivo_LST playlist = new Arquivo_LST();
            Arquivo_PLS playlist = new Arquivo_PLS(diretorio);
            playlist.Default();
            playlist.listaPaths.Clear();

            if (ListaArquivos.Count > 0)
            {
                //foreach (String nome_arquivo in ListaArquivos)
                //{
                //    playlist.listaPaths.Add(nome_arquivo.Substring(1)); //tira a barra do início.
                //}

                playlist.QTDArquivos = ListaArquivos.Count;
                playlist.listaPaths.AddRange(ListaArquivos.ToArray());
                playlist.Salvar(arquivoPls, true);
            }

            Util.Util.sequencial_arquivo_videos = Util.Util.sequencial_arquivo_videos + 1;

            return Util.Util.DIRETORIO_VIDEOS + Util.Util.RetornaNomeArquivo(arquivoPls) + Util.Util.ARQUIVO_EXT_PLS;           
        }


        public void AbrirFormatoNovo(String diretorioRaiz)
        {
            //Arquivo_RPT arpt = new Arquivo_RPT();

            //arpt.AbrirFormatoNovo(this.RPT);
   
            ////todo: verificar se os videos vão ficar no RPT mesmo ou embutidas no roteiro.
            //for(int video = 0; video < arpt.lVideosIda.Count; video ++)
            //{
            //    FrameLeaf fl = new FrameLeaf();

            //    fl.AdicionarVideo(arpt.lVideosIda[video]);

            //    this.FrameIda.Add(fl);
            //}

            //for (int video = 0; video < arpt.lVideosVolta.Count; video++)
            //{
            //    FrameLeaf fl = new FrameLeaf();

            //    fl.AdicionarVideo(arpt.lVideosVolta[video]);

            //    this.FrameVolta.Add(fl);
            //}

            //for (int video = 0; video < arpt.lVideosNumero.Count; video++)
            //{
            //    FrameLeaf fl = new FrameLeaf();

            //    fl.AdicionarVideo(arpt.lVideosNumero[video]);

            //    this.FrameNumero.Add(fl);
            //}

        }


        public void Abrir(String diretorioRaiz)
        {
            //Arquivo_RPT arpt = new Arquivo_RPT();

            //arpt.Abrir(this.RPT);

            //InsereVideos(this.FrameIda, diretorioRaiz + 
            //                            Util.Util.DIRETORIO_PAI + 
            //                            Util.Util.DIRETORIO_PAI + 
            //                            Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
            //                            Util.Util.TrataDiretorioFWParaWindows(
            //                            arpt.pathExibicaoIda));

            //InsereVideos(this.FrameVolta, diretorioRaiz + 
            //                            Util.Util.DIRETORIO_PAI + 
            //                            Util.Util.DIRETORIO_PAI + 
            //                            Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
            //                            Util.Util.TrataDiretorioFWParaWindows(
            //                            arpt.pathExibicaoVolta));

            //InsereVideos(this.FrameNumero, diretorioRaiz + 
            //                            Util.Util.DIRETORIO_PAI + 
            //                            Util.Util.DIRETORIO_PAI + 
            //                            Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
            //                            Util.Util.TrataDiretorioFWParaWindows(
            //                            arpt.pathNumeroRoteiro));

            /*
            //verificar o diretorio de path video.
            if (Util.Util.RetornaTipoVideoInterno(arpt.pathExibicaoIda) != Util.Util.TipoVideo.PLS)
            {
                tempVideoIda = CarregaVideo(arpt.pathExibicaoIda);
            }
            else
            {
                Arquivo_PLS pls = new Arquivo_PLS();
                pls.Abrir(arpt.pathExibicaoIda);

                foreach (String s in pls.listaPaths)
                {
                    flTempIda = new FrameLeaf();
                    flTempIda.PlayList.Add(CarregaVideo(s));
                    this.FrameIda.Add(flTempIda);
                }

                return;
            }

            flTempIda.PlayList.Add(tempVideoIda);
            this.FrameIda.Add(flTempIda);
            */





            //Persistencia.Videos.IVideo ivIda = new Persistencia.Videos.VideoTexto();
            //Persistencia.Videos.IVideo ivVolta = new Persistencia.Videos.VideoTexto();
            //Persistencia.Videos.IVideo ivNumro = new Persistencia.Videos.VideoTexto();
            
            //String temp = Util.Util.TrataDiretorio(arpt.pathExibicaoIda);

            //tratar diretorios.
            //ivIda = ivIda.Abrir(diretorioRaiz + Util.Util.DIRETORIO_PAI + Util.Util.DIRETORIO_PAI + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + Util.Util.TrataDiretorioFWParaWindows(arpt.pathExibicaoIda));
            //ivVolta = ivVolta.Abrir(diretorioRaiz + Util.Util.DIRETORIO_PAI + Util.Util.DIRETORIO_PAI + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + Util.Util.TrataDiretorioFWParaWindows(arpt.pathExibicaoVolta));
            //ivNumro = ivNumro.Abrir(diretorioRaiz + Util.Util.DIRETORIO_PAI + Util.Util.DIRETORIO_PAI + Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS + Util.Util.TrataDiretorioFWParaWindows(arpt.pathNumeroRoteiro));
            /**/
            //FrameLeaf flTempIda = new FrameLeaf();
                
            //flTempIda.PlayList.Add(ivIda);
            //this.FrameIda.Add(flTempIda);

            ////FrameLeaf flTempVolta = new FrameLeaf();

            //flTempVolta.AdicionarVideo(ivVolta);
            //this.FrameVolta.Add(flTempVolta);

            ////FrameLeaf flTempNumero = new FrameLeaf();

            //flTempNumero.AdicionarVideo(ivNumro);
            //this.FrameNumero.Add(flTempNumero);
            /**/

            /*

            for (int frame = 0; frame < this.FrameIda.Count; frame++)
            {
                this.FrameIda[frame].Abrir(diretorioRaiz + Util.Util.DIRETORIO_VIDEOS + @"\" + frame.ToString("d3") + "-" + this.LabelRoteiroIda.Replace(":", "_") + @".ida.v01");
            }

            for (int frame = 0; frame < this.FrameVolta.Count; frame++)
            {
                this.FrameVolta[frame].Abrir(diretorioRaiz + Util.Util.DIRETORIO_VIDEOS + @"\" + frame.ToString("d3") + "-" + this.LabelRoteiroVolta.Replace(":", "_") + @".volta.v01");
            }

            for (int frame = 0; frame < this.FrameNumero.Count; frame++)
            {
                this.FrameNumero[frame].Abrir(diretorioRaiz + Util.Util.DIRETORIO_VIDEOS + @"\" + frame.ToString("d3") + "-" + this.LabelNumero.Replace(":", "_") + @".num.v01");
            }
            */

        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="arquivoNome"></param>
        /// <remarks> Método utilizado para Gerar a configuração que irá para o controlador</remarks>
        public void Salvar(String arquivoNome, int indicePainel, uint altura, uint largura, bool modoApresentacaoDisplayLD6)
        {            
            GerarArquivosRoteiro(arquivoNome, indicePainel, altura, largura, modoApresentacaoDisplayLD6);
        }

        private void GerarArquivosRoteiro(string arquivoNome, int indicePainel, uint altura, uint largura, bool modoApresentacaoDisplayLD6)
        {            
            //Gera-se os arquivos ROT.
            if (indicePainel == 0)
            {
                GerarArquivosROT(arquivoNome, modoApresentacaoDisplayLD6, this.IdaIgualVolta);
            }

            GerarArquivosRPT(arquivoNome, indicePainel, altura, largura);            
            Util.Util.sequencial_arquivo_roteiros = Util.Util.sequencial_arquivo_roteiros + 1;
        }

        


        ///// <summary>
        ///// Recebe um diretório e faz a leitura do arquivo de vídeo.
        ///// </summary>
        ///// <param name="nomeArquivo">Diretório local contendo o nome do arquivo de vídeo.</param>
        ///// <returns>Objeto vídeo apontado por interface IVideo.</returns>
        //private IVideo CarregaVideo(String nomeArquivo)
        //{
        //    Util.Util.TipoVideo tv = Util.Util.RetornaTipoVideoInterno(nomeArquivo);

        //    IVideo iv = null;

        //    ////pôr esse switch numa função separada para ser chamada pela função de 
        //    ////carregar pls.
        //    //switch (tv)
        //    //{
        //    //    case Util.Util.TipoVideo.V01:
        //    //        {
        //    //            VideoV01 v01 = new VideoV01();
        //    //            v01.Abrir(nomeArquivo);

        //    //            return v01;
        //    //        }
        //    //        break;

        //    //    case Util.Util.TipoVideo.V02:
        //    //        {
        //    //            Persistencia.Videos.VideoV02 v02 = new Persistencia.Videos.VideoV02();
        //    //            v02.Abrir(nomeArquivo);

        //    //            return v02;
        //    //        }
        //    //        break;

        //    //    case Util.Util.TipoVideo.V03:
        //    //        break;

        //    //    case Util.Util.TipoVideo.V04:
        //    //        break;

        //    //    #region PLS
        //    //    //era pra ficar nessa função, mas achei inconveniente preparar
        //    //    //uma função recursiva agora.
        //    //    //case Util.Util.TipoVideo.PLS:
        //    //    //    {
        //    //    //        Arquivo_PLS please = new Arquivo_PLS();

        //    //    //        please.Abrir(nomeArquivo);

        //    //    //        foreach (String s in please.listaPaths)
        //    //    //        {
        //    //    //            iv.Add(CarregaVideo(s));
        //    //    //        }
        //    //    //    }
        //    //    //    break;
        //    //    #endregion
        //    //}

        //    return iv;
        //}

        ///// <summary>
        ///// Realiza a leitura de um arquivo de vídeo ou playlist e grava numa lista de frames.
        ///// </summary>
        ///// <param name="ListaDeFrames">Lista de frames onde ficarão os vídeos.</param>
        ///// <param name="diretorio">Diretório do arquivo de vídeo.</param>
        //private void InsereVideos(List<IFrame> ListaDeFrames, String diretorio)
        //{
        //    //IVideo tempVideo;
        //    //FrameLeaf flTemp = new FrameLeaf();
        //    //String diretorio_raiz = Util.Util.RetornaDiretorio(diretorio);


        //    ////verificar o diretorio de path video.
        //    //if (Util.Util.RetornaTipoVideoInterno(diretorio) != Util.Util.TipoVideo.PLS)
        //    //{
        //    //    tempVideo = CarregaVideo(diretorio);
        //    //}
        //    //else
        //    //{
        //    //    Arquivo_PLS pls = new Arquivo_PLS(diretorio_raiz);
        //    //    pls.Abrir(diretorio);

        //    //    foreach (String s in pls.listaPaths)
        //    //    {
        //    //        flTemp = new FrameLeaf();
        //    //        flTemp.PlayList.Add(CarregaVideo(diretorio_raiz + Util.Util.RetornaNomeArquivo(Util.Util.TrataDiretorioFWParaWindows(s))));
        //    //        ListaDeFrames.Add(flTemp);
        //    //    }

        //    //    return;
        //    //}

        //    //flTemp.PlayList.Add(tempVideo);
        //    //ListaDeFrames.Add(flTemp);
   
        //}
    }
}
