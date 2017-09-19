using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistencia;
using Persistencia.Videos;
using Util;
//using FrameLeaf = Nucleo.FrameLeaf;
//using IFrame = Nucleo.IFrame;

namespace Controlador
{
    public class Mensagem
    {
        public int ID { get; set; }
        public int Indice { get; set; }
        public string LabelMensagem { get; set; }
        public List<Frase> Frases { get; set; }
        public int Ordenacao;
        public bool Ascendente;

        public Mensagem()
        {
            this.Frases = new List<Frase>();
        }

        public Mensagem(string labelMensagem)
        {
            this.LabelMensagem = labelMensagem;
            this.Frases = new List<Frase>();
            Frases.Add(new Frase(labelMensagem));
        }


        public Mensagem(Mensagem mensagem_antiga, bool copiarFrases)
        {

            this.Frases = new List<Frase>(); 

            this.ID = mensagem_antiga.ID;
            this.Indice = mensagem_antiga.Indice;
            this.LabelMensagem = mensagem_antiga.LabelMensagem;
            this.Ordenacao = mensagem_antiga.Ordenacao;
            this.Ascendente = mensagem_antiga.Ascendente;

            if (copiarFrases)
            {
                foreach (Frase f in mensagem_antiga.Frases)
                {
                    this.Frases.Add(new Frase(f));
                }
            }
        }

        public void MoverFrases(int posicaoInicial, int posicaoFinal)
        {
            Frase aux = this.Frases[posicaoFinal];
            this.Frases[posicaoFinal] = this.Frases[posicaoInicial];
            this.Frases[posicaoInicial] = aux;
        }

        public bool CompararObjetosMensagem(Mensagem mensagem1, Mensagem mensagem2)
        {
            bool alterou = false;

            if ((mensagem1.ID != mensagem2.ID) || (mensagem1.Indice != mensagem2.Indice) || (mensagem1.LabelMensagem != mensagem2.LabelMensagem))
                alterou = true;

            if (!alterou)
                if (mensagem1.Frases.Count != mensagem2.Frases.Count)
                    alterou = true;

            if (!alterou)
            {
                //os dois roteiros tem a mesma quantidade de frases
                for (int i = 0; i < mensagem1.Frases.Count; i++)
                {
                    if (mensagem1.Frases[i].CompararObjetosFrase(mensagem1.Frases[i], mensagem2.Frases[i]))
                    {
                        alterou = true;
                        break;
                    }
                }
            }

            return alterou;
        }
        private void GerarArquivosMensagem(string arquivoNome, int indicePainel, uint altura, uint largura)
        {
            //Gera-se os arquivos ROT.
            GerarArquivosMSG(arquivoNome);

            GerarArquivosMPT(arquivoNome, indicePainel, String.Empty, altura, largura);

            Util.Util.sequencial_arquivo_mensagens = Util.Util.sequencial_arquivo_mensagens + 1;
        }
        public void Salvar(String arquivoNome, int indicePainel, uint altura, uint largura)
        {
           GerarArquivosMensagem(arquivoNome, indicePainel, altura, largura);
        }

        /// <summary>
        /// Abre a mensagem com o formato novo de MPT.
        /// </summary>
        /// <param name="diretorioRaiz">Diretorio do Arquivo MPT</param>
        public void AbrirFormatoNovo(String diretorioRaiz)
        {
            //Arquivo_MPT ampt = new Arquivo_MPT();

            //ampt.AbrirFormatoNovo(this.MPT);

            //for (int video = 0; video < ampt.Videos.Count; video++)
            //{
            //    FrameLeaf fl = new FrameLeaf();

            //    fl.AdicionarVideo(ampt.Videos[video]);

            //    this.Frame.Add(fl);
            //}

        }


        public void Abrir(String diretorioRaiz)
        {
            //IVideo tempVideo = null;
            //FrameLeaf flTempMsg = new FrameLeaf();
            //Arquivo_MPT ampt = new Arquivo_MPT();

            //ampt.Abrir(this.MPT);

            //InsereVideos(this.Frame, diretorioRaiz +
            //                            Util.Util.DIRETORIO_PAI +
            //                            Util.Util.DIRETORIO_PAI +
            //                            Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
            //                            Util.Util.TrataDiretorioFWParaWindows(ampt.pathVideo));


            //////verificar o diretorio de path video.
            ////if (Util.Util.RetornaTipoVideoInterno(ampt.pathVideo) != Util.Util.TipoVideo.PLS)
            ////{
            ////    tempVideo = CarregaVideo(ampt.pathVideo);
            ////}
            ////else
            ////{
            ////    Arquivo_PLS pls = new Arquivo_PLS();
            ////    pls.Abrir(ampt.pathVideo);

            ////    foreach (String s in pls.listaPaths)
            ////    {
            ////        flTempMsg = new FrameLeaf();
            ////        flTempMsg.PlayList.Add(CarregaVideo(s));
            ////        this.Frame.Add(flTempMsg);
            ////    }

            ////    return;
            ////}

            ////flTempMsg.PlayList.Add(tempVideo);
            ////this.Frame.Add(flTempMsg);
        }

        /// <summary>
        /// Realiza a leitura de um arquivo de vídeo ou playlist e grava numa lista de frames.
        /// Essa função existe no código de mensagens E roteiros (cópia não controlada).
        /// </summary>
        /// <param name="ListaDeFrames">Lista de frames onde ficarão os vídeos.</param>
        /// <param name="diretorio">Diretório do arquivo de vídeo.</param>
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


        //private IVideo CarregaVideo(String nomeArquivo)
        //{
        //    Util.Util.TipoVideo tv = Util.Util.RetornaTipoVideoInterno(nomeArquivo);

        //    IVideo iv = null;

        //    //pôr esse switch numa função separada para ser chamada pela função de 
        //    //carregar pls.
        //    switch (tv)
        //    {
        //        case Util.Util.TipoVideo.V01:
        //            {
        //                VideoV01 v01 = new VideoV01();
        //                v01.Abrir(nomeArquivo);

        //                return v01;
        //            }
        //            break;

        //        case Util.Util.TipoVideo.V02:
        //            {
        //                VideoV02 v02 = new VideoV02();
        //                v02.Abrir(nomeArquivo);

        //                return v02;
        //            }
        //            break;

        //        case Util.Util.TipoVideo.V03:
        //            break;

        //        case Util.Util.TipoVideo.V04:
        //            break;

        //        #region PLS
        //        //era pra ficar nessa função, mas achei inconveniente preparar
        //        //uma função recursiva agora.
        //        //case Util.Util.TipoVideo.PLS:
        //        //    {
        //        //        Arquivo_PLS please = new Arquivo_PLS();

        //        //        please.Abrir(nomeArquivo);

        //        //        foreach (String s in please.listaPaths)
        //        //        {
        //        //            iv.Add(CarregaVideo(s));
        //        //        }
        //        //    }
        //        //    break;
        //        #endregion
        //    }

        //    return iv;
        //}

        
        private String CriarPlaylist(String diretorio, List<String> ListaArquivos)
        {
            String arquivoPls = diretorio /*+ Util.Util.DIRETORIO_VIDEOS */+ Util.Util.sequencial_arquivo_videos.ToString("X8");

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
                
                playlist.listaPaths.AddRange(ListaArquivos.ToArray());
                playlist.QTDArquivos = ListaArquivos.Count;
                playlist.Salvar(arquivoPls, true);
            }

            Util.Util.sequencial_arquivo_videos = Util.Util.sequencial_arquivo_videos + 1;
            
            return arquivoPls;
        }

        public void GerarArquivosMPT(String diretorioRaiz, int indicePainel, String pathVideo, uint altura, uint largura)
        {
            String nome_arquivo = string.Empty;
            Arquivo_MPT ampt = new Arquivo_MPT();
            List<String> RPTContentFrase = new List<string>();
            String conteudo_mpt = string.Empty;

            GerarPlayList(diretorioRaiz + Util.Util.DIRETORIO_VIDEOS, RPTContentFrase, altura, largura);
                                  

            if (RPTContentFrase.Count > 1)
            {
                conteudo_mpt = CriarPlaylist(diretorioRaiz + Util.Util.DIRETORIO_VIDEOS
                    //Util.Util.DIRETORIO_PAI +
                    //Util.Util.DIRETORIO_PAI +
                    //Util.Util.DIRETORIO_PAI +
                    //Util.Util.DIRETORIO_PAI
                                                , RPTContentFrase) + Util.Util.ARQUIVO_EXT_PLS;
            }
            else
            {
                conteudo_mpt = RPTContentFrase[0];
            }


            ampt.pathVideo = Util.Util.TrataDiretorio(conteudo_mpt);

            nome_arquivo = diretorioRaiz +
                           Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                           Util.Util.DIRETORIO_PAINEL +
                           Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                           indicePainel.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS) +
                           Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                           Util.Util.DIRETORIO_MSGS;

            ampt.Salvar(nome_arquivo);
            ampt.GerarFormatoNovo(nome_arquivo, diretorioRaiz);
        }

        private void GerarPlayList(string arquivoNome, List<string> PlaylistFrase, uint altura, uint largura)
        {
            string extensao = String.Empty;
            foreach (Frase f in Frases)
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
                PlaylistFrase.Add(arquivoNome + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + extensao);
                Util.Util.sequencial_arquivo_videos++;
            }

        }

        public void GerarArquivosMSG(String diretorioRaiz)
        {
            Arquivo_MSG amsg = new Arquivo_MSG();

            amsg.id = (ushort)this.ID;
            amsg.labelMensagem = this.LabelMensagem;

            amsg.Salvar(diretorioRaiz +
                        Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                        Util.Util.DIRETORIO_MSGS +
                        Util.Util.sequencial_arquivo_mensagens.ToString("X8") +
                        Util.Util.ARQUIVO_EXT_MSG);
        }
    }
}
