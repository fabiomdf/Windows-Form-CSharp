using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Persistencia.Videos;
using Persistencia;
using ImportacaoLDX;
using Mensagem = Controlador.Mensagem;
using Painel = Controlador.Painel;
using System.Resources;
using Globalization;
using System.Reflection;

namespace LDX2

{
    public class ImportacaoLDX
    {
        private const string DIRETORIO_FONTES = @"\fontes";
        private const string DIRETORIO_IDIOMAS = @"\idiomas";
        private const string DIRETORIO_MSGS = @"\msgs";
        private const string DIRETORIO_PAINEIS = @"\paineis";
        private const string DIRETORIO_REGIOES = @"\regioes";
        private const string DIRETORIO_VIDEOS = @"\videos";
        private const string DIRETORIO_ROTEIROS = @"\roteiros";

        private const string ARQUIVO_FIX = @"\param.fix";
        private const string ARQUIVO_VAR = @"\param.var";
        private const string ARQUIVO_LST_MSGS = @"\msgs.lst";
        private const string ARQUIVO_LST_ROTEIROS = @"\roteiros.lst";
        private const string ARQUIVO_ALT = @"\altern.alt";
        private const string ARQUIVO_CFG = @"\painel.cfg";

        private const string DIRETORIO_PAINEL_MSGS = @"\msgs";
        private const string DIRETORIO_PAINEL_ROTEIROS = @"\roteiros";

        private static UInt64 sequencial_arquivo_V01 = 0;
        private static UInt64 sequencial_arquivo_V02 = 0;
        private static UInt64 sequencial_arquivo_PLS = 0;
        private static UInt64 sequencial_arquivo_roteiros = 0;
        private static UInt64 sequencial_arquivo_mensagens = 0;

        List<Painel> Paineis = new List<Painel>();

        ParserLDX parser = new ParserLDX(); 

        public ImportacaoLDX()
        {

        }
        public string DiretorioPrincipal = string.Empty;
        public string arquivoLDX = string.Empty;

        public void ImportarProgramacao(string arquivoOrigem, string diretorioDestino)
        {            
            DiretorioPrincipal = diretorioDestino;
            arquivoLDX = arquivoOrigem;        
            parser.CarregarControlador(arquivoOrigem);
            int qtd = parser.quantidadePaineis;
            Console.WriteLine("Number of Displays = " + qtd.ToString());
            CriaDiretorios(qtd);
            Console.WriteLine("Copying fnt files...");
            CopiarArquivosFonte(diretorioDestino);
            Console.WriteLine("Creating Temporary files...");
            for (int i = 0; i < qtd; i++)
            {
                WriteFiles(i);
            }
        }

        private void CopiarArquivosFonte(string diretorioDestino)
        {
            string caminhoFontes = @"C:\teste2\fontes\";
            Console.WriteLine(Environment.CurrentDirectory + "\\LDX2\\CaminhoFontes.inf");
            if (File.Exists(Environment.CurrentDirectory + "\\LDX2\\CaminhoFontes.inf"))
            {
                TextReader tr = File.OpenText(Environment.CurrentDirectory + "\\LDX2\\CaminhoFontes.inf");
                string caminho = tr.ReadLine();
                if (Directory.Exists(caminho))
                {
                    caminhoFontes = caminho;
                }
                tr.Close();
            }

            Console.WriteLine(caminhoFontes);
            try
            {
                foreach (var file in Directory.GetFiles(caminhoFontes/*Util.Util.DIRETORIO_ARQUIVOS_FONTE_TEMP*/))
                    File.Copy(file, Path.Combine(diretorioDestino + DIRETORIO_FONTES, Path.GetFileName(file)),true);
            }
            catch (Exception e)
            {
                return;
            }
        }


        private void ImportarParamentrosVariaveis(int quantidadePaineis)
        {
            //VAR
            Arquivo_VAR arvar = new Arquivo_VAR();
            arvar.roteiro = parser.control.Painel[0].RoteiroSelecionado;
            arvar.Salvar(DiretorioPrincipal + ARQUIVO_VAR);
        }

        private void ImportarParametrosFixos(int quantidadePaineis)
        {
            ////strings usadas em saudaçães.
            //string dir_bom_dia = Util.Util.DIRETORIO_VIDEOS + @"bdia";
            //string dir_boa_tarde = Util.Util.DIRETORIO_VIDEOS + @"btar";
            //string dir_boa_noite = Util.Util.DIRETORIO_VIDEOS + @"bnoi";

            //VideoV01 BDv01 = new VideoV01();
            //// BOM DIA
            //BDv01.texto = parser.control.FrasesFixasLCD[42];
            //BDv01.tempoApresentacao = parser.control.TempoMensagem;
            //BDv01.tempoRolagem = parser.control.TempoRolagem;
            //BDv01.Salvar(DiretorioPrincipal + dir_bom_dia, true);

            //VideoV01 BTv01 = new VideoV01();
            //// BOA TARDE
            //BTv01.texto = parser.control.FrasesFixasLCD[43];
            //BTv01.tempoApresentacao = parser.control.TempoMensagem;
            //BTv01.tempoRolagem = parser.control.TempoRolagem;
            //BTv01.Salvar(DiretorioPrincipal + dir_boa_tarde, true);

            //VideoV01 BNv01 = new VideoV01();

            //BNv01.texto = parser.control.FrasesFixasLCD[44];
            //BNv01.tempoApresentacao = parser.control.TempoMensagem;
            //BNv01.tempoRolagem = parser.control.TempoRolagem;
            //BNv01.Salvar(DiretorioPrincipal + dir_boa_noite, true);

            //FIX
            Arquivo_FIX arfix = new Arquivo_FIX();
            arfix.CriarParametrosFixosPadrao();
            arfix.qntPaineis = (UInt16)quantidadePaineis;

            //dir_bom_dia = Util.Util.TrataDiretorio(dir_bom_dia) + Util.Util.ARQUIVO_EXT_V01;
            //dir_boa_tarde = Util.Util.TrataDiretorio(dir_boa_tarde) + Util.Util.ARQUIVO_EXT_V01;
            //dir_boa_noite = Util.Util.TrataDiretorio(dir_boa_noite) + Util.Util.ARQUIVO_EXT_V01;

            arfix.horaBomDia = parser.control.HoraInicioDia;
            arfix.horaBoaTarde = parser.control.HoraInicioTarde;
            arfix.horaBoaNoite = parser.control.HoraInicioNoite;
            //arfix.labelBomDia = Encoding.ASCII.GetBytes(parser.control.FrasesFixasLCD[42].PadRight(20, '\0'));
            //arfix.labelBoaTarde = Encoding.ASCII.GetBytes(parser.control.FrasesFixasLCD[43].PadRight(20, '\0'));
            //arfix.labelBoaNoite = Encoding.ASCII.GetBytes(parser.control.FrasesFixasLCD[44].PadRight(20, '\0'));
            
            //Util.Util.OpcoesApresentacao opcoes = new Util.Util.OpcoesApresentacao();
            //opcoes.intervaloAnimacao = parser.control.TempoRolagem;
            //opcoes.tempoApresentacao = parser.control.TempoMensagem;
            //opcoes.animacao = 0;
            //opcoes.alinhamento = 2; // 2 = Centralizado            

            //arfix.horaSaida = opcoes;
            //arfix.dataHora = opcoes;
            //arfix.tarifa = opcoes;
            //arfix.somenteHora = opcoes;
            //arfix.temperatura = opcoes;
            //arfix.horaTemperatura = opcoes;

            arfix.Salvar(DiretorioPrincipal + ARQUIVO_FIX);
        }
        private void CriaDiretorios(int quantidadePaineis)
        {
            Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_FONTES);
            Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_IDIOMAS);
            Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_MSGS);
            Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_PAINEIS);
            Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_REGIOES);
            Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_VIDEOS);
            Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_ROTEIROS);

            CriarDiretoriosPaineis(quantidadePaineis);
        }

        public ResourceManager carregaIdioma()
        {
            ResourceManager rmStrings = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));

            //switch (this.lingua)
            //{
            //    case Util.Util.Lingua.Ingles:
                    rmStrings = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
            //        break;
            //    case Util.Util.Lingua.Portugues:
            //        rmStrings = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
            //        break;
            //    case Util.Util.Lingua.Espanhol:
            //        rmStrings = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
            //        break;
            //    case Util.Util.Lingua.Frances:
            //        rmStrings = new ResourceManager("Globalization.Frances", Assembly.GetAssembly(typeof(ResLibraryClass)));
            //        break;
            //}


            return rmStrings;

        }
        private void CriarDiretoriosPaineis(int quantidadePaineis)
        {
            for (int i = 0; i < quantidadePaineis; i++)
            {
                this.Paineis.Add(new Painel(carregaIdioma()));

                string diretorio_painel = @"\" + i.ToString("d2");

                Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_PAINEIS + diretorio_painel);
                Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_PAINEIS + diretorio_painel + DIRETORIO_PAINEL_MSGS);
                Directory.CreateDirectory(DiretorioPrincipal + DIRETORIO_PAINEIS + diretorio_painel + DIRETORIO_PAINEL_ROTEIROS);

                //Arquivo ALT
                Arquivo_ALT aalt = new Arquivo_ALT();

                aalt.ArquivoNome = DiretorioPrincipal + DIRETORIO_PAINEIS + diretorio_painel + ARQUIVO_ALT;
                aalt.CriarAlternanciasDefault();                
            }

            CriarRegiao();
            CriarIdioma();

            ImportarParametrosFixos(quantidadePaineis);

            ImportarParamentrosVariaveis(quantidadePaineis);
        }
        private string RetornaDiretorioArquivoFonteDefaultParaArquivoCFG(int altura, int largura)
        {
            List<string> dir_fontes = Directory.EnumerateFiles(DiretorioPrincipal + DIRETORIO_FONTES, "*.fnt").ToList();

            foreach (string nome_fonte in dir_fontes)
            {
                Arquivo_FNT fonte = new Arquivo_FNT();

                fonte.Abrir(nome_fonte);

                if (altura == fonte.Altura)
                    return nome_fonte;

            }

            return null;
        }
        private void WriteFiles(int indicePainel)
        {
            int numPaineis = 1;

            Arquivo_CFG acfg = new Arquivo_CFG();

            acfg.CriarConfiguracaoDefault();

            acfg.altura = (uint) System.Convert.ToInt32(parser.control.Painel[indicePainel].Altura);
            acfg.largura = (uint) System.Convert.ToInt32(parser.control.Painel[indicePainel].Largura);
            String fonte_default = RetornaDiretorioArquivoFonteDefaultParaArquivoCFG(System.Convert.ToInt32(parser.control.Painel[indicePainel].Altura), 0);

            if (fonte_default == null)
                acfg.fontePath = Util.Util.TrataDiretorio(DIRETORIO_FONTES + @"\FONTE8.FNT").Substring(1);
            else acfg.fontePath = Util.Util.TrataDiretorio(fonte_default.Substring(DiretorioPrincipal.Count()).Substring(1));

            acfg.mensagemSelecionada = parser.control.Painel[indicePainel].MensagemSelecionada;

            //VideoV01 bdia = new VideoV01();
            //bdia.Abrir(DiretorioPrincipal + Util.Util.DIRETORIO_VIDEOS + @"bdia.v01");
            //acfg.VideosSaudacao[0] = bdia;

            //VideoV01 btar = new VideoV01();
            //btar.Abrir(DiretorioPrincipal + Util.Util.DIRETORIO_VIDEOS + @"btar.v01");
            //acfg.VideosSaudacao[1] = btar;

            //VideoV01 bnoi = new VideoV01();
            //bnoi.Abrir(DiretorioPrincipal + Util.Util.DIRETORIO_VIDEOS + @"bnoi.v01");
            //acfg.VideosSaudacao[2] = bnoi;

            acfg.Salvar(DiretorioPrincipal + DIRETORIO_PAINEIS + @"\" + indicePainel.ToString("d2") + ARQUIVO_CFG);
            
            //carrega uma bela de uma fonte default
            Arquivo_FNT afnt = new Arquivo_FNT();

            afnt.Abrir(DiretorioPrincipal + acfg.fontePath);
            
            SalvaMensagens(indicePainel);
            SalvaRoteiros(indicePainel);

            CriaRotLst();
            CriaMsgLst();

        }

        private void MensagensV01(Mensagem m)
        {
            ////v01s de mensagens.
            //VideoV01 _mv01 = new VideoV01();

            //_mv01.animacao = (byte)m.Rolagem;
            //_mv01.tempoRolagem = parser.control.TempoRolagem;
            //_mv01.tempoApresentacao = parser.control.TempoRoteiro[0];

            //_mv01.texto = m.LabelMensagem;
            //_mv01.Salvar(DiretorioPrincipal + DIRETORIO_VIDEOS + @"\" + sequencial_arquivo_V01.ToString("X8"));

            ////salvando mpts de mensagens.
            //Arquivo_MPT ampt = new Arquivo_MPT();

            //ampt.idMensagem = (ushort)m.Id;
            //ampt.pathVideo = Util.Util.TrataDiretorio(DIRETORIO_VIDEOS +
            //                                          @"\" +
            //                                          sequencial_arquivo_V01.ToString("X8") +
            //                                          Util.Util.ARQUIVO_EXT_V01).Substring(1); //O substring de 1 é para tirar a primeira barra.


            //for (int i = 0; i < this.Paineis.Count; i++)
            //{
            //    ampt.Salvar(DiretorioPrincipal +
            //                DIRETORIO_PAINEIS +
            //                @"\" +
            //                i.ToString("X2") +
            //                DIRETORIO_PAINEL_MSGS +
            //                @"\" +
            //                sequencial_arquivo_mensagens.ToString("X8"));
            //    ampt.GerarFormatoNovo(DiretorioPrincipal + DIRETORIO_PAINEIS + @"\" + i.ToString("d2") +
            //                    DIRETORIO_PAINEL_MSGS + @"\" + sequencial_arquivo_mensagens.ToString("X8"), DiretorioPrincipal);
            //}

            //sequencial_arquivo_mensagens = sequencial_arquivo_mensagens + 1;
            //sequencial_arquivo_V01 = sequencial_arquivo_V01 + 1;

        }
        private void SalvaMensagens(int indicePainel)
        {
            List<String> lArquivosMensagens = new List<string>();
            //sequencial_arquivo_mensagens = 0;
            Util.Util.sequencial_arquivo_mensagens = 0;
            //salvando mensagens.
            if (indicePainel == 0)
            {
                GerarMensagemEmergencia();
            }
            for (int msg = 1; msg < parser.control.QtdMensagens; msg++)
            {
                int quantidadeFrases = parser.RetornarQuantidadeFrases(indicePainel, msg);
                Controlador.Mensagem m = new Controlador.Mensagem();
                
                lArquivosMensagens.Clear();

                for (int indiceFrase = 0; indiceFrase < quantidadeFrases; indiceFrase++)
                {
                    if (indicePainel == 0)
                    {
                        m.ID = msg;
                        // m.IndicePainel = indicePainel;
                        m.LabelMensagem = parser.RetornaLabelMensagem(indicePainel, msg, indiceFrase, 0);
                        //m.Rolagem = (Util.Util.Rolagem) parser.RetornaRolagemMensagem(indicePainel, msg, indiceFrase);

                        //m.TempoApresentacao = parser.control.TempoMensagem.ToString("0");
                        //m.TempoRolagem = parser.control.TempoRolagem.ToString("0");

                        m.Salvar(DiretorioPrincipal, indicePainel, (uint)parser.control.Painel[indicePainel].Altura, (uint)parser.control.Painel[indicePainel].Largura);
                    }

                    String nome_arquivo = DiretorioPrincipal + DIRETORIO_VIDEOS + @"\" +
                                          sequencial_arquivo_V02.ToString("X8");

                    Persistencia.Videos.VideoV02 videoMensagem = new Persistencia.Videos.VideoV02();

                    videoMensagem.Altura = parser.control.Painel[indicePainel].Altura;
                    videoMensagem.Largura = parser.control.Painel[indicePainel].Largura;

                    videoMensagem.animacao = (byte) parser.RetornaRolagemMensagem(indicePainel, msg, indiceFrase);

                    videoMensagem.tempoRolagem = parser.control.TempoRolagem;
                    videoMensagem.tempoApresentacao = parser.control.TempoRoteiro[0];

                    videoMensagem.pixelBytes = parser.RetornarPixelBytesMensagens(indicePainel, msg, indiceFrase);
                    videoMensagem.Largura = (uint)parser.RetornarLarguraPixelBytesMensagens(indicePainel, msg, indiceFrase);
                    videoMensagem.Salvar(nome_arquivo, true);
                    sequencial_arquivo_V02 = sequencial_arquivo_V02 + 1;
                    lArquivosMensagens.Add(nome_arquivo + Util.Util.ARQUIVO_EXT_V02);
                }

                Arquivo_MPT armtTemp = new Arquivo_MPT();

                armtTemp.CriarMensagemPathPadrao();
                armtTemp.idMensagem = System.Convert.ToUInt16(m.ID.ToString("00"));

                CriaPlaylist(lArquivosMensagens);
                armtTemp.pathVideo = Util.Util.DIRETORIO_VIDEOS_FIRMWARE + sequencial_arquivo_PLS.ToString("X8") +
                                     @".pls";
                sequencial_arquivo_PLS++;

                armtTemp.pathVideo = Util.Util.TrataDiretorio(armtTemp.pathVideo);

                armtTemp.Salvar(DiretorioPrincipal + DIRETORIO_PAINEIS + @"\" + indicePainel.ToString("d2") +
                                DIRETORIO_PAINEL_MSGS + @"\");

                armtTemp.GerarFormatoNovo(DiretorioPrincipal + DIRETORIO_PAINEIS + @"\" + indicePainel.ToString("d2") +
                                DIRETORIO_PAINEL_MSGS + @"\", DiretorioPrincipal);

                Util.Util.sequencial_arquivo_mensagens = Util.Util.sequencial_arquivo_mensagens + 1;
            }
        }

        private void GerarMensagemEmergencia()
        {            
            List<String> lArquivosMensagens = new List<string>();            

            int quantidadeFrases = parser.RetornarQuantidadeFrases(0, 0);
            
            for (int indiceFrase = 0; indiceFrase < quantidadeFrases; indiceFrase++)
            {

                String nome_arquivo = DiretorioPrincipal + DIRETORIO_VIDEOS + @"\" + sequencial_arquivo_V02.ToString("X8");

                Persistencia.Videos.VideoV02 videoMensagem = new Persistencia.Videos.VideoV02();

                videoMensagem.Altura = parser.control.Painel[0].Altura;
                videoMensagem.Largura = parser.control.Painel[0].Largura;

                videoMensagem.animacao = (byte)parser.RetornaRolagemMensagem(0, 0, indiceFrase);

                videoMensagem.tempoRolagem = parser.control.TempoRolagem;
                videoMensagem.tempoApresentacao = parser.control.TempoMensagem;

                videoMensagem.pixelBytes = parser.RetornarPixelBytesMensagens(0, 0, indiceFrase);
                videoMensagem.Largura = (uint)parser.RetornarLarguraPixelBytesMensagens(0, 0, indiceFrase);
                videoMensagem.Salvar(nome_arquivo, true);
                sequencial_arquivo_V02 = sequencial_arquivo_V02 + 1;
                lArquivosMensagens.Add(nome_arquivo + Util.Util.ARQUIVO_EXT_V02);
            }

            //Arquivo_LST playlist = new Arquivo_LST();
            Arquivo_PLS playlist = new Arquivo_PLS(DiretorioPrincipal);

            playlist.Default();
            playlist.listaPaths.Clear();

            if (lArquivosMensagens.Count > 0)
            {
                foreach (String nome_arquivo in lArquivosMensagens)
                {
                    String nomeArquivoTemp = nome_arquivo.Substring(DiretorioPrincipal.Count());
                    nomeArquivoTemp = Util.Util.TrataDiretorio(nomeArquivoTemp);

                    if (nomeArquivoTemp.Contains('/'))
                    {
                        int indice = nomeArquivoTemp.IndexOf('/');
                    }

                    playlist.listaPaths.Add(nomeArquivoTemp.Substring(1)); //tira a barra do início.
                }

                playlist.QTDArquivos = lArquivosMensagens.Count;
                playlist.Salvar(DiretorioPrincipal + DIRETORIO_PAINEIS +  @"\00\emerg");               
            }
        }
        private void CriaMsgLst()
        {
            List<string> msgs = Directory.EnumerateFiles(DiretorioPrincipal + DIRETORIO_MSGS, "*.msg").ToList();

            Arquivo_LST alst = new Arquivo_LST();
            alst.Default();
            alst.listaPaths.Clear();
            alst.qtdArquivos = System.Convert.ToUInt32(msgs.Count);

            foreach (string s in msgs)
            {
                //alst.listaPaths.Add(s);
                alst.listaPaths.Add(s.Substring(DiretorioPrincipal.Length + DIRETORIO_MSGS.Length, s.Length - (DiretorioPrincipal.Length + DIRETORIO_MSGS.Length)).Replace("\\", String.Empty).Replace(Util.Util.ARQUIVO_EXT_MSG, String.Empty));
            }

            alst.AtualizarCRC();
            alst.Salvar(DiretorioPrincipal + DIRETORIO_MSGS + ARQUIVO_LST_MSGS);
        }
        private void SalvaRoteiros(int indicePainel)
        {
            int numRoteiros = parser.control.QtdRoteiros;
            int count = 0 + indicePainel;            
            List<String> lArquivosNumero = new List<string>();
            List<String> lArquivosIda = new List<string>();
            List<String> lArquivosVolta = new List<string>();

            // Reinicia o sequencial do arquivo de roteiros devido a mudança de Painéis.
            sequencial_arquivo_roteiros = 0;

            for (int j = 0; j < numRoteiros; j++)
            {
                lArquivosNumero.Clear();
                lArquivosIda.Clear();
                lArquivosVolta.Clear();
                Controlador.Roteiro r = new Controlador.Roteiro();
                r.ID = count;
                //RandomizaId(id);
                r.ID = r.ID + 1;
                //Verifica o count de texto. caso tenha mais de um texto, deve-se
                //proceder de uma forma diferente: continua-se criando os .v01 .v02 etc..
                //mas tem de se construir um arquivo .pls(que conterá os nomes desses vídeos)
                //que será apontado por um arquivo path(.mpt ou .rpt) e que será gravado no mesmo diretório de videos(O arquivo .pls).
                //O arquivo .pls é considerado um arquivo de vídeo.
                #region Numero do Roteiro
                
                String nome_arquivo = DiretorioPrincipal + DIRETORIO_VIDEOS + @"\" + sequencial_arquivo_V02.ToString("X8");

                Persistencia.Videos.VideoV02 videoNumero = new Persistencia.Videos.VideoV02();

                videoNumero.Altura = parser.control.Painel[indicePainel].Altura;
                videoNumero.Largura = parser.control.Painel[indicePainel].Largura;

                videoNumero.animacao = (byte) parser.RetornaRolagemNumeroRoteiro(indicePainel, j);

                videoNumero.tempoRolagem = parser.control.TempoRolagem;
                videoNumero.tempoApresentacao = parser.control.TempoRoteiro[0];

                videoNumero.pixelBytes = parser.RetornarPixelBytesNumeroRoteiro(indicePainel, j);
                videoNumero.Largura = (uint) parser.RetornarLarguraPixelBytesNumeroRoteiro(indicePainel, j);
                videoNumero.Salvar(nome_arquivo, true);
                sequencial_arquivo_V02 = sequencial_arquivo_V02 + 1;
                lArquivosNumero.Add(nome_arquivo + Util.Util.ARQUIVO_EXT_V02);
                
                #endregion

                #region Ida
                int QuantidadeFrases = parser.RetornarQuantidadeFrases(indicePainel, j, true);

                for (int ntexto = 0; ntexto < QuantidadeFrases; ntexto++)
                {
                    count = count + 1;

                    nome_arquivo = DiretorioPrincipal + DIRETORIO_VIDEOS + @"\" + sequencial_arquivo_V02.ToString("X8");

                    Persistencia.Videos.VideoV02 v02 = new Persistencia.Videos.VideoV02();

                    v02.Altura = parser.control.Painel[indicePainel].Altura;
                    v02.Largura = parser.control.Painel[indicePainel].Largura;

                    v02.animacao = (byte) parser.RetornaRolagemRoteiro(indicePainel, j, ntexto, true);

                    v02.tempoRolagem = parser.control.TempoRolagem;

                    v02.tempoApresentacao = parser.control.TempoRoteiro[0];

                    v02.pixelBytes = parser.RetornarPixelBytesRoteiros(indicePainel, j, ntexto, true);
                    v02.Largura = (uint)parser.RetornarLarguraPixelBytesRoteiros(indicePainel, j, ntexto, true);

                    v02.Salvar(nome_arquivo, true);
                    sequencial_arquivo_V02 = sequencial_arquivo_V02 + 1;
                    lArquivosIda.Add(nome_arquivo + Util.Util.ARQUIVO_EXT_V02);

                    Persistencia.Videos.VideoV04 v04 = new VideoV04();

                    v04.listaVideos = new List<IVideo>();
                    v04.listaVideos.Add(v02);
                    v04.listaVideos.Add(v02);



                }

                #endregion

                #region Volta
                QuantidadeFrases = parser.RetornarQuantidadeFrases(indicePainel, j, false);

                for (int ntexto = 0; ntexto < QuantidadeFrases; ntexto++)
                {
                    count = count + 1;

                    nome_arquivo = DiretorioPrincipal + DIRETORIO_VIDEOS + @"\" + sequencial_arquivo_V02.ToString("X8");

                    Persistencia.Videos.VideoV02 v02 = new Persistencia.Videos.VideoV02();

                    v02.Altura = parser.control.Painel[indicePainel].Altura;
                    v02.Largura = parser.control.Painel[indicePainel].Largura;

                    v02.animacao = (byte)parser.RetornaRolagemRoteiro(indicePainel, j, ntexto, false);

                    v02.tempoRolagem = parser.control.TempoRolagem;

                    v02.tempoApresentacao = parser.control.TempoRoteiro[0];

                    v02.pixelBytes = parser.RetornarPixelBytesRoteiros(indicePainel, j, ntexto, false);
                    v02.Largura = (uint)parser.RetornarLarguraPixelBytesRoteiros(indicePainel, j, ntexto, false);

                    v02.Salvar(nome_arquivo, true);
                    sequencial_arquivo_V02 = sequencial_arquivo_V02 + 1;
                    lArquivosVolta.Add(nome_arquivo + Util.Util.ARQUIVO_EXT_V02);
                }

                #endregion

                SalvaROT(r.ID.ToString("00"), indicePainel, j, 0, 0, true);
                
                Arquivo_RPT arptTemp = new Arquivo_RPT();

                arptTemp.CriarRoteirosPathPadrao();
                arptTemp.idRoteiro = System.Convert.ToUInt16(r.ID.ToString("00"));
                
                CriaPlaylist(lArquivosIda);
                arptTemp.pathExibicaoIda = Util.Util.DIRETORIO_VIDEOS_FIRMWARE + sequencial_arquivo_PLS.ToString("X8") + @".pls";
                sequencial_arquivo_PLS++;

                CriaPlaylist(lArquivosVolta);
                arptTemp.pathExibicaoVolta = Util.Util.DIRETORIO_VIDEOS_FIRMWARE + sequencial_arquivo_PLS.ToString("X8") + @".pls";
                sequencial_arquivo_PLS++;

                CriaPlaylist(lArquivosNumero);
                arptTemp.pathNumeroRoteiro = Util.Util.DIRETORIO_VIDEOS_FIRMWARE + sequencial_arquivo_PLS.ToString("X8") + @".pls";
                sequencial_arquivo_PLS++;

                arptTemp.pathExibicaoIda = Util.Util.TrataDiretorio(arptTemp.pathExibicaoIda);
                arptTemp.pathExibicaoVolta = Util.Util.TrataDiretorio(arptTemp.pathExibicaoVolta);
                arptTemp.pathNumeroRoteiro = Util.Util.TrataDiretorio(arptTemp.pathNumeroRoteiro);

                arptTemp.Salvar(DiretorioPrincipal + DIRETORIO_PAINEIS + @"\" + indicePainel.ToString("d2") +
                                DIRETORIO_PAINEL_ROTEIROS + @"\" + sequencial_arquivo_roteiros.ToString("X8"));

                arptTemp.GerarFormatoNovo(DiretorioPrincipal + DIRETORIO_PAINEIS + @"\" + indicePainel.ToString("d2") + DIRETORIO_PAINEL_ROTEIROS + @"\" + sequencial_arquivo_roteiros.ToString("X8") + ".rpt", DiretorioPrincipal);

                sequencial_arquivo_roteiros = sequencial_arquivo_roteiros + 1;
            }
        }
       

        private void CriaRotLst()
        {
            //******
            List<string> rots = Directory.EnumerateFiles(DiretorioPrincipal + DIRETORIO_ROTEIROS, "*.rot").ToList();

            Arquivo_LST alst_roteiros = new Arquivo_LST();
            alst_roteiros.Default();
            alst_roteiros.listaPaths.Clear();
            alst_roteiros.qtdArquivos = System.Convert.ToUInt32(rots.Count);

            foreach (string s in rots)
            {
                alst_roteiros.listaPaths.Add(s.Substring(DiretorioPrincipal.Length + DIRETORIO_ROTEIROS.Length,
                                                         s.Length -
                                                         (DiretorioPrincipal.Length + DIRETORIO_ROTEIROS.Length)).Replace("\\", String.Empty).Replace(Util.Util.ARQUIVO_EXT_ROT, String.Empty));
            }

            alst_roteiros.AtualizarCRC();
            alst_roteiros.Salvar(DiretorioPrincipal + DIRETORIO_ROTEIROS + ARQUIVO_LST_ROTEIROS);
        }
        private void SalvaROT(String id_roteiro, int indicePainel, int indiceRoteiro, int indiceFrase, int indiceImagem, bool ida)
        {
            // Salva apenas se for o painel Principal
            if (indicePainel != 0)
            {
                return;
            }
            //=============================================
            //Arquivo ROT
            Arquivo_ROT arotTemp = new Arquivo_ROT();

            arotTemp.CriarRoteirosPadrao();
            arotTemp.id = System.Convert.ToUInt16(id_roteiro);
            arotTemp.labelNumero = parser.RetornarLabelNumero(0, indiceRoteiro, indiceImagem);
            arotTemp.labelRoteiro = parser.RetornarLabelRoteiro(0, indiceRoteiro, indiceFrase, indiceImagem, ida);

            arotTemp.Salvar(DiretorioPrincipal + DIRETORIO_ROTEIROS + @"\" + sequencial_arquivo_roteiros.ToString("X8") + ".rot");

            //**************************************************
        }

        /// <summary>
        /// Cria o arquivo RPT para todos os painéis.
        /// </summary>
        /// <param name="id_roteiro">id do roteiro.</param>
        private void CriaRPT(string id_roteiro, int indicePainel)
        {
            Arquivo_RPT arptTemp = new Arquivo_RPT();

            arptTemp.CriarRoteirosPathPadrao();
            arptTemp.idRoteiro = System.Convert.ToUInt16(id_roteiro);

            arptTemp.pathExibicaoIda = Util.Util.DIRETORIO_VIDEOS_FIRMWARE + sequencial_arquivo_V01.ToString("X8") + @".pls";

            arptTemp.pathExibicaoVolta = Util.Util.DIRETORIO_VIDEOS_FIRMWARE + sequencial_arquivo_V01.ToString("X8") + @".pls";

            arptTemp.pathNumeroRoteiro = Util.Util.DIRETORIO_VIDEOS_FIRMWARE + sequencial_arquivo_V01.ToString("X8") + @".pls";

            arptTemp.pathExibicaoIda = Util.Util.TrataDiretorio(arptTemp.pathExibicaoIda);
            arptTemp.pathExibicaoVolta = Util.Util.TrataDiretorio(arptTemp.pathExibicaoVolta);
            arptTemp.pathNumeroRoteiro = Util.Util.TrataDiretorio(arptTemp.pathNumeroRoteiro);

            arptTemp.Salvar(DiretorioPrincipal + DIRETORIO_PAINEIS + @"\" +indicePainel.ToString("d2") + DIRETORIO_PAINEL_ROTEIROS + @"\" +sequencial_arquivo_roteiros.ToString("X8") +".rpt");

            arptTemp.GerarFormatoNovo(DiretorioPrincipal + DIRETORIO_PAINEIS + @"\" + indicePainel.ToString("d2") + DIRETORIO_PAINEL_ROTEIROS + @"\" + sequencial_arquivo_roteiros.ToString("X8") + ".rpt", DiretorioPrincipal);

            sequencial_arquivo_V01 = sequencial_arquivo_V01 + 1;
            sequencial_arquivo_roteiros = sequencial_arquivo_roteiros + 1;

        }
        private void CriaPlaylist(List<String> ListaArquivos)
        {
            //Arquivo_LST playlist = new Arquivo_LST();
            Arquivo_PLS playlist = new Arquivo_PLS(DiretorioPrincipal);

            playlist.Default();
            playlist.listaPaths.Clear();

            if (ListaArquivos.Count > 0)
            {
                foreach (String nome_arquivo in ListaArquivos)
                {
                    String nomeArquivoTemp = nome_arquivo.Substring(DiretorioPrincipal.Count());
                    nomeArquivoTemp = Util.Util.TrataDiretorio(nomeArquivoTemp);

                    if (nomeArquivoTemp.Contains('/'))
                    {
                        int indice = nomeArquivoTemp.IndexOf('/');
                    }

                    playlist.listaPaths.Add(nomeArquivoTemp.Substring(1)); //tira a barra do início.
                }

                playlist.QTDArquivos = ListaArquivos.Count;
                playlist.Salvar(DiretorioPrincipal + DIRETORIO_VIDEOS + @"\" + sequencial_arquivo_PLS.ToString("X8"));
            }
        }
        private void CriarIdioma()
        {
            ResourceManager rmStrings = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
            this.lingua = Util.Util.Lingua.Ingles;

            switch (this.lingua)
            {
                case Util.Util.Lingua.Ingles:
                    rmStrings = new ResourceManager("Globalization.English", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Portugues:
                    rmStrings = new ResourceManager("Globalization.Português", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Espanhol:
                    rmStrings = new ResourceManager("Globalization.Espanhol", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
                case Util.Util.Lingua.Frances:
                    rmStrings = new ResourceManager("Globalization.Frances", Assembly.GetAssembly(typeof(ResLibraryClass)));
                    break;
            }

            Arquivo_LPK alpk = new Arquivo_LPK(rmStrings);
            alpk.Salvar(DiretorioPrincipal + DIRETORIO_IDIOMAS + @"\ptbr.lpk");
        }

        private void CriarRegiao()
        {
            Arquivo_RGN argn = new Arquivo_RGN();

            argn.CriarRegiaoPadrao();

            argn.Salvar(DiretorioPrincipal + DIRETORIO_REGIOES + @"\brasil.RGN");

            Arquivo_LST alst = new Arquivo_LST();

            alst.listaPaths.Add(@"brasil");

            alst.Salvar(DiretorioPrincipal + DIRETORIO_REGIOES + @"\regioes.lst");
        }

        public Util.Util.Lingua lingua { get; set; }
    }
}
