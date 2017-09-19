using Persistencia.Videos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Reflection;
using Globalization;

namespace Controlador
{
    public class MensagemEmergencia
    {
        public List<Frase> Frases;
        public ResourceManager rm;
        
        public MensagemEmergencia(ResourceManager rm)
        {
            string texto = rm.GetString("MENSAGENS_ESPECIAIS_EMERGENCIA");
            Frases = new List<Frase>();

            if (Directory.Exists(Util.Util.diretorio_NSS))
            {
                texto = rm.GetString("MENSAGENS_ESPECIAIS_STOP");
            }
            Frase f = new Frase(texto);
            f.Indice = 0;
            f.LabelFrase = texto;
            Frases.Add(f);

            this.rm = rm;
        }

        public MensagemEmergencia()
        {            
            Frases = new List<Frase>();
            ResourceManager rm = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
            string texto = rm.GetString("MENSAGENS_ESPECIAIS_EMERGENCIA");
            if (Directory.Exists(Util.Util.diretorio_NSS))
            {
                texto = rm.GetString("MENSAGENS_ESPECIAIS_STOP");
            }
            Frase f = new Frase(texto);
            f.Indice = 0;
            f.LabelFrase = texto;
            Frases.Add(f);

            this.rm = rm;
        }

        public MensagemEmergencia(MensagemEmergencia mensagem_antiga)
        {

            this.Frases = new List<Frase>();
            this.rm = mensagem_antiga.rm;

            foreach (Frase f in mensagem_antiga.Frases)
            {
                this.Frases.Add(new Frase(f));
            }
            
        }

        public bool CompararObjetosMensagem(MensagemEmergencia mensagem1, MensagemEmergencia mensagem2)
        {
            bool alterou = false;

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

        public void GerarPlaylist(uint altura, uint largura, int indicePainel)
        {
            // indicePainel igual 0xff significa que não tem NSS na configuração.
            if (indicePainel == 0xff)
                indicePainel = 0;

            string diretorio_raiz = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string arquivo_temporario_MsgEmerg = diretorio_raiz +
                                     Util.Util.DIRETORIO_APP +
                                     Util.Util.DIRETORIO_TEMPORARIO +
                                     Util.Util.DIRETORIO_PAINEL +
                                     indicePainel.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS) + 
                                     Util.Util.ARQUIVO_PLS_EMERG;

            string diretorio_temporario_Videos = diretorio_raiz +
                                     Util.Util.DIRETORIO_APP +
                                     Util.Util.DIRETORIO_TEMPORARIO +
                                     Util.Util.DIRETORIO_PAINEL +
                                     indicePainel.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS);                         
            List<String> lArquivosMensagens = new List<string>();

            int quantidadeFrases = Frases.Count;
            int i = 0;
            foreach (Frase f in Frases)
            {
                f.Salvar(diretorio_temporario_Videos + "//", altura, largura);
                lArquivosMensagens.Add(diretorio_temporario_Videos + "//" + Util.Util.sequencial_arquivo_videos.ToString("X8")+Util.Util.ARQUIVO_EXT_V02);
                Util.Util.sequencial_arquivo_videos++;                
            }

            #region Playlist da Mensagem de Emergência
            Arquivo_PLS playlist = new Arquivo_PLS(diretorio_raiz);

            playlist.Default();
            playlist.listaPaths.Clear();

            if (lArquivosMensagens.Count > 0)
            {
                foreach (String arquivo in lArquivosMensagens)
                {
                    String nomeArquivoTemp = arquivo.Substring(diretorio_raiz.Count());
                    nomeArquivoTemp = Util.Util.TrataDiretorio(nomeArquivoTemp);

                    if (nomeArquivoTemp.Contains('/'))
                    {
                        int indice = nomeArquivoTemp.IndexOf('/');
                    }

                    playlist.listaPaths.Add(nomeArquivoTemp.Substring(1)); //tira a barra do início.
                }

                playlist.QTDArquivos = lArquivosMensagens.Count;
                playlist.Salvar(arquivo_temporario_MsgEmerg, true);
            }
            #endregion Playlist da Mensagem de Emergência

            // Ao final, apagamos os arquivos temporários gerados
            Util.Util.sequencial_arquivo_videos = 0;

            foreach (Frase f in Frases)
            {
                if (File.Exists(diretorio_temporario_Videos + "//" +Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.ARQUIVO_EXT_V02))
                {
                    File.Delete(diretorio_temporario_Videos + "//" +Util.Util.sequencial_arquivo_videos.ToString("X8") + Util.Util.ARQUIVO_EXT_V02);
                }
                Util.Util.sequencial_arquivo_videos++;
            }
            Util.Util.sequencial_arquivo_videos = 0;
           // #region Primeira Frase da Emergência
            //String nome_arquivo = diretorio_raiz + DIRETORIO_VIDEOS + @"\emerg1";

            //Persistencia.Videos.VideoV02 videoMensagem = new Persistencia.Videos.VideoV02();

            //videoMensagem.Altura = (uint)this.Paineis[0].Altura;
            //videoMensagem.Largura = (uint)this.Paineis[0].Largura;

            //videoMensagem.animacao = (byte)this.Paineis[0].MensagemEmergencia.Frases[0].Modelo.Textos[0].Apresentacao;

            //videoMensagem.tempoRolagem = Convert.ToUInt32(this.Paineis[0].MensagemEmergencia.Frases[0].Modelo.Textos[0].TempoRolagem);
            //videoMensagem.tempoApresentacao = Convert.ToUInt32(this.Paineis[0].MensagemEmergencia.Frases[0].Modelo.Textos[0].TempoApresentacao);

            ////preparaPixelBytes(ref videoMensagem, this.Paineis[0].MensagemEmergencia.Frases[0].Modelo.Textos[0],
            ////                                  null,
            ////                                  null,
            ////                                  null,
            ////                                  0,
            ////                                  Util.Util.TipoModelo.Texto,
            ////                                  Util.Util.TipoVideo.V02);

            //videoMensagem.Largura = (uint)this.Paineis[0].Largura;
            //videoMensagem.Salvar(nome_arquivo, true);
            ////sequencial_arquivo_V02 = sequencial_arquivo_V02 + 1;
            //lArquivosMensagens.Add(nome_arquivo + Util.Util.ARQUIVO_EXT_V02);
            //#endregion  Primeira Frase da Emergência

            //#region  Segunda Frase da Perigo
            //nome_arquivo = diretorio_raiz + DIRETORIO_VIDEOS + @"\emerg1";

            //videoMensagem = new Persistencia.Videos.VideoV02();

            //videoMensagem.Altura = (uint)this.Paineis[0].Altura;
            //videoMensagem.Largura = (uint)this.Paineis[0].Largura;

            //videoMensagem.animacao = (byte)this.Paineis[0].MensagemEmergencia.Frases[0].Modelo.Textos[0].Apresentacao;

            //videoMensagem.tempoRolagem = Convert.ToUInt32(this.Paineis[0].MensagemEmergencia.Frases[0].Modelo.Textos[0].TempoRolagem);
            //videoMensagem.tempoApresentacao = Convert.ToUInt32(this.Paineis[0].MensagemEmergencia.Frases[0].Modelo.Textos[0].TempoApresentacao);

            ////preparaPixelBytes(ref videoMensagem, this.Paineis[0].MensagemEmergencia.Frases[1].Modelo.Textos[0],
            ////                                  null,
            ////                                  null,
            ////                                  null,
            ////                                  0,
            ////                                  Util.Util.TipoModelo.Texto,
            ////                                  Util.Util.TipoVideo.V02);

            //videoMensagem.Largura = (uint)this.Paineis[0].Largura;
            //videoMensagem.Salvar(nome_arquivo, true);
            ////sequencial_arquivo_V02 = sequencial_arquivo_V02 + 1;
            //lArquivosMensagens.Add(nome_arquivo + Util.Util.ARQUIVO_EXT_V02);
            //#endregion  Segunda Frase da Perigo

            
        }
    }
}
