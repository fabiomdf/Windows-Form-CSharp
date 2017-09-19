using Persistencia.Videos;
using Persistencia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Resources;

namespace Controlador
{
    public class MensagemEspecial
    {
        public List<Frase> Frases;
        public ResourceManager rm;
        public Arquivo_RGN regiao;
        public int QTD_FRASES = 25;

        public MensagemEspecial()
        {
            Frases = new List<Frase>();
        }

        public MensagemEspecial(ResourceManager rm, Arquivo_RGN regiao)
        {
            Frases = new List<Frase>();

            Frase f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_BOM_DIA"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.BomDia;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_BOM_DIA");
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_BOA_TARDE"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.BoaTarde;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_BOA_TARDE");
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_BOA_NOITE"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.BoaNoite;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_BOA_NOITE");
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_DOMINGO"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Domingo;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_DOMINGO");
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_SEGUNDA"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Segunda;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_SEGUNDA");
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_TERCA"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Terca;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_TERCA");
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_QUARTA"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Quarta;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_QUARTA");
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_QUINTA"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Quinta;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_QUINTA");
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_SEXTA"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Sexta;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_SEXTA");
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_SABADO"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Sabado;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_SABADO");
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            string hora;
            string formatoHora;
            string formatoAmEspaco = "";
            string formatoPmPonto = "";
            if (regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
            {
                if (regiao.opcaoAmPm_Ponto == (byte)Util.Util.OpcaoAmPm_Ponto.EXIBIR_AM_PM)
                {
                    formatoHora = " pm";
                    formatoPmPonto = " pm";
                    formatoAmEspaco = " am";
                }
                else
                { 
                    formatoHora = ".";
                    formatoPmPonto = ".";
                    formatoAmEspaco = " ";
                }

                hora = DateTime.Now.ToString("hh:mm");
            }
            else
            {
                hora = DateTime.Now.ToString("HH:mm");
                formatoHora = "";
            }

            hora = hora + formatoHora;

            f = new Frase(hora);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.SomenteHora;
            f.LabelFrase = hora;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            string data;
            if (regiao.formatoDataHora == (byte)Util.Util.FormatoDataHora.FORMATO_AM_PM)
                data = DateTime.Now.ToString("MM/dd/yyyy");
            else
                data = DateTime.Now.ToString("dd/MM/yyyy");

            int dia = (int)DateTime.Now.DayOfWeek;
            string diaSemana = "";
            switch (dia) 
            {
                case 0: diaSemana = rm.GetString("MENSAGENS_ESPECIAIS_DOMINGO"); break;
                case 1: diaSemana = rm.GetString("MENSAGENS_ESPECIAIS_SEGUNDA"); break;
                case 2: diaSemana = rm.GetString("MENSAGENS_ESPECIAIS_TERCA");   break;
                case 3: diaSemana = rm.GetString("MENSAGENS_ESPECIAIS_QUARTA");  break;
                case 4: diaSemana = rm.GetString("MENSAGENS_ESPECIAIS_QUINTA");  break;
                case 5: diaSemana = rm.GetString("MENSAGENS_ESPECIAIS_SEXTA");   break;
                case 6: diaSemana = rm.GetString("MENSAGENS_ESPECIAIS_SABADO");  break;
            }

            f = new Frase(diaSemana + " " + data +" "+ hora);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.DataHora;
            f.LabelFrase = diaSemana + " " + data + " " + hora;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(rm.GetString("MENSAGENS_ESPECIAIS_LABEL_SAIDA"));
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.HoraSaida;
            f.LabelFrase = rm.GetString("MENSAGENS_ESPECIAIS_LABEL_SAIDA");
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            string unidade_Temperatura;
            string temperatura;

            if (regiao.unidadeTemperatura == (byte)Util.Util.UnidadeTemperatura.UNIDADE_CELSIUS)
            {
                unidade_Temperatura = "°C";
                temperatura = "30 " + unidade_Temperatura;
            }
            else
            { 
                unidade_Temperatura = "°F";
                temperatura = "86 " + unidade_Temperatura;
            }

            f = new Frase(temperatura);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Temperatura;
            f.LabelFrase = temperatura;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);


            string unidade_Velocidade;
            string velocidade;
            if (regiao.unidadeVelocidade == (byte)Util.Util.UnidadeVelocidade.UNIDADE_KMpH)
            {
                unidade_Velocidade = rm.GetString("MENSAGENS_ESPECIAIS_QUILOMETROS_POR_HORA"); 
                velocidade = "60 " + unidade_Velocidade;
            }
            else
            {
                unidade_Velocidade = rm.GetString("MENSAGENS_ESPECIAIS_MILHAS_POR_HORA"); 
                velocidade = "35 " + unidade_Velocidade;
            }

            f = new Frase(velocidade);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Velocidade;
            f.LabelFrase = velocidade;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            string moeda="";
            string separador = "";
            string valorMoeda = "";
            if (regiao.separadorDecimal == Encoding.ASCII.GetBytes(".")[0])
                separador = ".";
            else
                separador = ",";

            switch (regiao.moeda) 
            {
                case (byte)Util.Util.Moeda.MOEDA_DOLAR: moeda = "$";
                                                        valorMoeda = "$ 1" + separador + "00";
                                                        break;
                case (byte)Util.Util.Moeda.MOEDA_REAL: moeda = "R$";
                                                       valorMoeda = "R$ 3" + separador + "00";
                                                       break;
                case (byte)Util.Util.Moeda.MOEDA_PESO: moeda = "$";
                                                       valorMoeda = "$ 5" + separador + "00";
                                                       break;
                case (byte)Util.Util.Moeda.MOEDA_EURO: moeda = "€";
                                                       valorMoeda = "€ 1" + separador + "00";
                                                       break;

            }

            f = new Frase(valorMoeda);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.Tarifa;
            f.LabelFrase = valorMoeda;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(hora + " " + temperatura);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.HoraTemperatura;
            f.LabelFrase = hora + " " + temperatura;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(data+ " " + hora + " " + temperatura);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.DataHoraTemperatura;
            f.LabelFrase = data + " " + hora + " " + temperatura;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase("0123456789/ "+moeda+" "+unidade_Temperatura+" "+unidade_Velocidade);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.FraseFontePainel;
            f.LabelFrase = "0123456789/ " + moeda + " " + unidade_Temperatura + " " + unidade_Velocidade;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(separador);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.SimboloSeparadorDecimal;
            f.LabelFrase = separador;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(unidade_Velocidade);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.SimboloVelocidade;
            f.LabelFrase = unidade_Velocidade;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(unidade_Temperatura);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.SimboloTemperatura;
            f.LabelFrase = unidade_Temperatura;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);
            
            f = new Frase(moeda);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.SimboloTarifa;
            f.LabelFrase = moeda;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(formatoAmEspaco);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.SimboloAM_Espaco;
            f.LabelFrase = formatoAmEspaco;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            f = new Frase(formatoPmPonto);
            f.Indice = (int)Util.Util.IndiceMensagensEspeciais.SimboloPM_Ponto;
            f.LabelFrase = formatoPmPonto;
            f.Modelo.Textos[0].AlinhamentoV = Util.Util.AlinhamentoVertical.Baixo;
            Frases.Add(f);

            this.rm = rm;
            this.regiao = new Arquivo_RGN(regiao);
        }

        public MensagemEspecial(MensagemEspecial mensagem_antiga)
        {

            this.Frases = new List<Frase>();
            this.rm = mensagem_antiga.rm;
            this.regiao = mensagem_antiga.regiao;

            foreach (Frase f in mensagem_antiga.Frases)
            {
                this.Frases.Add(new Frase(f));
            }
            
        }

        public bool CompararObjetosMensagem(MensagemEspecial mensagem1, MensagemEspecial mensagem2)
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

        public void GerarPlaylist(uint altura, uint largura)
        {
            string diretorio_raiz = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string arquivo_temporario_MsgEmerg = diretorio_raiz +
                                     Util.Util.DIRETORIO_APP +
                                     Util.Util.DIRETORIO_TEMPORARIO +
                                     Util.Util.DIRETORIO_PAINEL +
                                     0.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS) +
                                     Util.Util.ARQUIVO_PLS_EMERG;

            string diretorio_temporario_Videos = diretorio_raiz +
                                     Util.Util.DIRETORIO_APP +
                                     Util.Util.DIRETORIO_TEMPORARIO +
                                     Util.Util.DIRETORIO_PAINEL +
                                     0.ToString(Util.Util.ARQUIVO_PAINEL_NUMEROALGS);
            List<String> lArquivosMensagens = new List<string>();

            int quantidadeFrases = Frases.Count;
            int i = 0;
            foreach (Frase f in Frases)
            {
                f.Salvar(diretorio_temporario_Videos + "//" + i.ToString(), altura, largura);
                lArquivosMensagens.Add(diretorio_temporario_Videos + "//" + i.ToString());
                i++;
            }

            #region Playlist da Mensagem de Emergência
            Arquivo_PLS playlist = new Arquivo_PLS(arquivo_temporario_MsgEmerg);

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
                playlist.Salvar(arquivo_temporario_MsgEmerg);
            }
            #endregion Playlist da Mensagem de Emergência

            // Ao final, apagamos os arquivos temporários gerados
            i = 0;
            foreach (Frase f in Frases)
            {
                if (File.Exists(diretorio_temporario_Videos + "//" + i.ToString()))
                {
                    File.Delete(diretorio_temporario_Videos + "//" + i.ToString());
                }
                i++;
            }
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
