using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistencia;
using Persistencia.Videos;

namespace Controlador
{
    public class Motorista
    {
        public const int Versao = 1;
        public Frase ID { get; set;}
        public Frase Nome { get; set;}
        public int Ordenacao;
        public bool Ascendente;

        public Motorista()
        {
            this.ID = new Frase("");
            this.Nome = new Frase("");
        }

        public Motorista(string ID, string Nome)
        {
            this.ID = new Frase(ID);
            this.Nome = new Frase(Nome);
        }

        public Motorista(Motorista motorista_antigo)
        {
            this.ID = new Frase(motorista_antigo.ID);
            this.Nome = new Frase(motorista_antigo.Nome);
            this.Ascendente = motorista_antigo.Ascendente;
            this.Ordenacao = motorista_antigo.Ordenacao;
        }

        public bool CompararObjetosMotoristas(Motorista m1, Motorista m2)
        {
            bool alterou = false;

            if (m1.ID.CompararObjetosFrase(m1.ID,m2.ID))
                alterou = true;


            if (!alterou)
                if (m1.Nome.CompararObjetosFrase(m1.Nome,m2.Nome))
                    alterou = true;

            return alterou;

        }

        public bool AchouID(Motorista m1, Motorista m2)
        {
            bool achou = false;

            if (m1.ID.LabelFrase == m2.ID.LabelFrase)
                achou = true;

            return achou;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="arquivoNome"></param>
        /// <remarks> Método utilizado para Gerar a configuração que irá para o controlador</remarks>
        public void Salvar(String arquivoNome, int indicePainel, uint altura, uint largura)
        {
            GerarArquivosMotorista(arquivoNome, indicePainel, altura, largura);
        }

        private void GerarArquivosMotorista(string arquivoNome, int indicePainel, uint altura, uint largura)
        {
            //Gera-se os arquivos ROT.
            GerarArquivosDRV(arquivoNome);

            GerarArquivosDPT(arquivoNome, indicePainel, altura, largura);

            Util.Util.sequencial_arquivo_motoristas = Util.Util.sequencial_arquivo_motoristas + 1;
        }
        public void GerarArquivosDRV(String diretorioRaiz)
        {
            Arquivo_DRV adrv = new Arquivo_DRV();

            adrv.id = (ushort)Util.Util.sequencial_arquivo_motoristas;
            adrv.labelIdentificacao = this.ID.LabelFrase;
            adrv.labelNome = this.Nome.LabelFrase;

            adrv.Salvar(diretorioRaiz +
                        Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                        Util.Util.DIRETORIO_DRIVERS +
                        Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                        Util.Util.sequencial_arquivo_motoristas.ToString("X8") +
                        Util.Util.ARQUIVO_EXT_DRV
                        );
        }

        public void GerarArquivosDPT(String arquivoNome, int indicePainel, uint altura, uint largura)
        {
            Arquivo_DPT adpt = new Arquivo_DPT();
            String conteudo_dpt_id = string.Empty;
            String conteudo_dpt_nome = string.Empty;
            
            List<String> DPTContentID = new List<string>();
            List<String> DPTContentNome = new List<string>();

            GerarPlayList(arquivoNome + Util.Util.DIRETORIO_VIDEOS, DPTContentID, DPTContentNome, altura, largura);


            if (DPTContentID.Count > 1)
            {
                conteudo_dpt_id = CriarPlaylist(arquivoNome + Util.Util.DIRETORIO_VIDEOS, DPTContentID);
            }
            else
            {
                conteudo_dpt_id = DPTContentID[0];
            }
            if (DPTContentNome.Count > 1)
            {
                conteudo_dpt_nome = CriarPlaylist(arquivoNome + Util.Util.DIRETORIO_VIDEOS, DPTContentID);
            }
            else
            {
                conteudo_dpt_nome = DPTContentNome[0];
            }

            adpt.pathExibicaoID = Util.Util.TrataDiretorio(conteudo_dpt_id);
            adpt.pathExibicaoNome = Util.Util.TrataDiretorio(conteudo_dpt_nome);
            
            string raiz = arquivoNome;
            arquivoNome = arquivoNome +
                          //Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                          Util.Util.DIRETORIO_PAINEL +
                          //Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                          indicePainel.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS) +
                          Util.Util.ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS +
                          Util.Util.DIRETORIO_DRIVERS;

            adpt.Salvar(arquivoNome);

            adpt.GerarFormatoNovo(arquivoNome, raiz);
        }

        private void GerarPlayList(string arquivoNome, List<string> PlaylistID, List<string> PlaylistNome, uint altura, uint largura)
        {
            PlaylistID.Clear();
            PlaylistNome.Clear();

            string extensao = String.Empty;

            switch (ID.TipoVideo)
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
            ID.Salvar(arquivoNome + "//", altura, largura);
            PlaylistID.Add(arquivoNome + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + extensao);
            Util.Util.sequencial_arquivo_videos++;

            switch (Nome.TipoVideo)
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
            Nome.Salvar(arquivoNome + "//", altura, largura);
            PlaylistNome.Add(arquivoNome + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8") + extensao);
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

    }

}
