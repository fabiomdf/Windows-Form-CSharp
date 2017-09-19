using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;
using System.Text.RegularExpressions;
using Persistencia;
using Persistencia.Videos;
using Persistencia.NSS;
using System.Resources;

namespace Memorias
{
    public class NandFFS
    {
        // Constantes relacionadas a memória que será utilizada MT29F

        #region Constantes

        private const uint TAMANHO_PAGINA = 2048;
        private const uint TAMANHO_CI = 64;
       // private const uint TAMANHO_CI_NEW = 16;
        private const uint PAGINAS_POR_BLOCO = 64;
        private const uint NUMERO_BLOCOS = 2048;
        private const long TAMANHO_TOTAL = (TAMANHO_PAGINA + TAMANHO_CI)*PAGINAS_POR_BLOCO*NUMERO_BLOCOS;
        private const uint TAMANHO_CABECALHO = 4;

        private string nomeArquivoDestino = "teste.nfs";
        public bool isOld = false;

        #endregion Constantes

        private FileStream fs;
        private List<byte> dados;

        private FileChunk[] listaFileChunk;
        private ChunkTags[] listaFileChunkTags;
        private DirChunk[] listaDirChunk;
        private ChunkTags[] listaDirChunkTags;
        private string[] diretorios;
        private string[] arquivos;
        public List<string> listaDiretoriosArquivos;       
                
        private uint GetParentIdFileChunk(string diretorio)
        {
            bool existe = false;
            string pai = Directory.GetParent(diretorio).FullName;
            uint resposta = (uint) listaDiretoriosArquivos.IndexOf(pai);
            for (int i = 0; i < listaDiretoriosArquivos.Count; i++ )
            {
                listaDiretoriosArquivos[i] = listaDiretoriosArquivos[i].Replace("\\\\", "\\");
            }

            if (resposta == 0xffffffff)
            {
                resposta = 0;
            }
            return resposta;            
        }

   
        private static uint GetParentIdDirChunk(string diretorio, DirChunk[] listaDirChunk, ChunkTags[] listaChunkTags)
        {
            string pai = Directory.GetParent(diretorio).Name;
            try
            {
                for (int i = 0; i < listaDirChunk.Length; i++)
                {
                    if (listaDirChunk[i].name == pai.PadRight(8, '\0').Substring(0, 8))
                    {
                        return listaChunkTags[i].objectId;                        
                    }
                }
            }
            catch
            {
            }
            return 0;
        }
        private String[] CarregarListaArquivos(string diretorioRaiz)
        {
            List<String> listaArquivos = new List<string>();

            // ARQUIVOS DO LDX2
            var searchPatternLDX2 = new Regex(@"$(?<=\.(fix|var|sch|fir|opt|fnt|rgn|lpk|alt|cfg|lst|msg|rot|drv|mpt|rpt|dpt|pls))", RegexOptions.IgnoreCase);
            string[] filesLDX2 = Directory.GetFiles(diretorioRaiz, "*.*", SearchOption.AllDirectories).Where(f => searchPatternLDX2.IsMatch(f)).ToArray();
            listaArquivos.AddRange(filesLDX2);
            // Arquivo relacionado de mensagem de emergência.
            //listaArquivos.AddRange(Directory.GetFiles(diretorioRaiz, "*emerg.pls", SearchOption.AllDirectories));
            
            // ARQUIVOS NSS - Obs.: Alguns arquivos já são iguais aos arquivos LDX2            
            var searchPatternNSS = new Regex(@"$(?<=\.(wav|bin|v01|pli|crc|plv|pla|pls|b12|v02|nfx))", RegexOptions.IgnoreCase);
            string[] filesNSS = Directory.GetFiles(diretorioRaiz, "*.*", SearchOption.AllDirectories).Where(f => searchPatternNSS.IsMatch(f)).ToArray();
            listaArquivos.AddRange(filesNSS);

            listaArquivos.AddRange(Directory.GetFiles(diretorioRaiz, "nss.nfs", SearchOption.AllDirectories));
            listaArquivos.AddRange(Directory.GetFiles(diretorioRaiz, Util.Util.ASTERISCO + Util.Util.ARQUIVO_EXT_LDX2_MAIUSCULO, SearchOption.AllDirectories));
            return listaArquivos.ToArray();
        }
        public void VerificarNandFlashFileSystem(string arquivoOrigem, ResourceManager rm)
        {
#if !DEBUG
            return;
#else
            string diretorioRaiz = (Directory.Exists("C:\\TempNSS\\")) ? "C:\\TempNSS2\\" : "C:\\TempNSS\\";
            string ultimoStatus = String.Empty;
            try
            {

                ultimoStatus = "GerarDiretoriosFAT";
                GerarDiretoriosFAT(arquivoOrigem, diretorioRaiz);

                if (arquivoOrigem.EndsWith(Util.Util.ARQUIVO_B12) || arquivoOrigem.EndsWith(Util.Util.ARQUIVO_LDNFX))
                {
                    #region Verificar Firmwares
                    String[] firmwares = Directory.GetFiles(diretorioRaiz, "*.fir", SearchOption.AllDirectories);
                    foreach (String firmware in firmwares)
                    {
                        ultimoStatus = "Verificando Integridade do Firmware: " + firmware;
                        Arquivo_FIR arquivo = new Arquivo_FIR();
                        arquivo.Abrir(firmware);
                    }
                    String[] optionFirmwares = Directory.GetFiles(diretorioRaiz, "*.opt", SearchOption.AllDirectories);
                    foreach (String option in optionFirmwares)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos Options de Firmwares: " + option;
                        Arquivo_OPT arquivo = new Arquivo_OPT();
                        arquivo.Abrir(option);
                    }
                    #endregion Verificar Firmwares
                    #region Parametros Fixos
                    String[] parametrosFixos = Directory.GetFiles(diretorioRaiz, "*.fix", SearchOption.AllDirectories);
                    foreach (String param in parametrosFixos)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Parametros Fixos: " + param;
                        Arquivo_FIX arquivo = new Arquivo_FIX();
                        arquivo.Abrir(param);
                    }
                    #endregion Parametros Fixos
                    #region Parametros Variaveis
                    String[] parametrosVariaveis = Directory.GetFiles(diretorioRaiz, "*.var", SearchOption.AllDirectories);
                    foreach (String param in parametrosVariaveis)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Parametros Variaveis: " + param;
                        Arquivo_VAR arquivo = new Arquivo_VAR();
                        arquivo.Abrir(param);
                    }
                    #endregion Parametros Variaveis
                    #region Fontes (FNT)
                    String[] fontes = Directory.GetFiles(diretorioRaiz, "*.fnt", SearchOption.AllDirectories);
                    foreach (String fonte in fontes)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Fonte: " + fonte;
                        Arquivo_FNT arquivo = new Arquivo_FNT();
                        arquivo.Abrir(fonte);
                    }
                    #endregion Fontes (FNT)
                    #region  Arquivos de Idioma (LPK)
                    String[] idiomas = Directory.GetFiles(diretorioRaiz, "*.lpk", SearchOption.AllDirectories);
                    foreach (String idioma in idiomas)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Idioma: " + idioma;
                        Arquivo_LPK arquivo = new Arquivo_LPK(rm);
                        arquivo.Abrir(idioma);
                    }
                    #endregion  Arquivos de Idioma (LPK)
                    #region  Listas de Mensagem, Roteiro, idioma e Regiao (LST)
                    String[] listas = Directory.GetFiles(diretorioRaiz, "*.lst", SearchOption.AllDirectories);
                    foreach (String lista in listas)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Listas: " + lista;
                        Arquivo_LST arquivo = new Arquivo_LST();
                        arquivo.Abrir(lista);
                    }
                    #endregion  Listas de Mensagem, Roteiro, idioma e Regiao (LST)
                    #region  Mensagens (MSG)
                    String[] mensagens = Directory.GetFiles(diretorioRaiz, "*.msg", SearchOption.AllDirectories);
                    foreach (String mensagem in mensagens)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Mensagens (MSG) : " + mensagem;
                        Arquivo_MSG arquivo = new Arquivo_MSG();
                        arquivo.Abrir(mensagem);
                    }
                    #endregion  Mensagens (MSG)
                    #region  Roteiros (ROT)
                    String[] roteiros = Directory.GetFiles(diretorioRaiz, "*.rot", SearchOption.AllDirectories);
                    foreach (String roteiro in roteiros)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Roteiros (ROT) : " + roteiro;
                        Arquivo_ROT arquivo = new Arquivo_ROT();
                        arquivo.Abrir(roteiro);
                    }
                    #endregion  Roteiros (ROT)
                    #region  Drivers (DRV)
                    String[] motoristas = Directory.GetFiles(diretorioRaiz, "*.drv", SearchOption.AllDirectories);
                    foreach (String motorista in motoristas)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Motoristas (DRV) : " + motorista;
                        Arquivo_DRV arquivo = new Arquivo_DRV();
                        arquivo.Abrir(motorista);
                    }
                    #endregion  Drivers (DRV)
                    #region  Configurações de Painéis (CFG)
                    String[] configs = Directory.GetFiles(diretorioRaiz, "*.cfg", SearchOption.AllDirectories);
                    foreach (String config in configs)
                    {
                        if (config.Contains("\\FRT_APP\\"))
                        {
                            // TODO: Implementar verificar arquivo cfg do APP, pois os arquivos são diferentes.
                            continue;
                        }
                        ultimoStatus = "Verificando Integridade dos arquivos de Configurações de Painéis (CFG) : " + config;
                        Arquivo_CFG arquivo = new Arquivo_CFG();
                        arquivo.Abrir(config);
                    }
                    #endregion  Configurações de Painéis (CFG)
                    #region  Alternâncias de Painéis (ALT)
                    String[] alternancias = Directory.GetFiles(diretorioRaiz, "*.alt", SearchOption.AllDirectories);
                    foreach (String alternancia in alternancias)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Alternâncias de Painéis (ALT) : " + alternancia;
                        Arquivo_ALT arquivo = new Arquivo_ALT();
                        arquivo.Abrir(alternancia);
                    }
                    #endregion  Alternâncias de Painéis (ALT)
                    #region Regioes (RGN)
                    String[] regioes = Directory.GetFiles(diretorioRaiz, "*.rgn", SearchOption.AllDirectories);
                    foreach (String regiao in regioes)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de regiões (RGN) : " + regiao;
                        Arquivo_RGN arquivo = new Arquivo_RGN();
                        arquivo.Abrir(regiao);
                    }
                    #endregion Regioes (RGN)
                    #region  Mensagens (MPT)
                    String[] mensagensPath = Directory.GetFiles(diretorioRaiz, "*.mpt", SearchOption.AllDirectories);
                    foreach (String mensagem in mensagensPath)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Mensagens (MPT) : " + mensagem;
                        Arquivo_MPT arquivo = new Arquivo_MPT();
                        arquivo.Abrir(mensagem);
                    }
                    #endregion  Mensagens (MPT)
                    #region  Roteiros (RPT)
                    String[] roteirosPath = Directory.GetFiles(diretorioRaiz, "*.rpt", SearchOption.AllDirectories);
                    foreach (String roteiro in roteirosPath)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Roteiros (RPT) : " + roteiro;
                        Arquivo_RPT arquivo = new Arquivo_RPT();
                        arquivo.Abrir(roteiro);
                    }
                    #endregion  Roteiros (RPT)
                    #region  Motoristas (DPT)
                    String[] motoristasPath = Directory.GetFiles(diretorioRaiz, "*.dpt", SearchOption.AllDirectories);
                    foreach (String motorista in motoristasPath)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Motoristas (DPT) : " + motorista;
                        Arquivo_DPT arquivo = new Arquivo_DPT();
                        arquivo.Abrir(motorista);
                    }
                    #endregion  Roteiros (RPT)
                    #region  Playlist de Vídeos (PLS)
                    String[] VideosPLS = Directory.GetFiles(diretorioRaiz, "*.pls", SearchOption.AllDirectories);
                    foreach (String video in VideosPLS)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Playlist de Vídeos: " + video;
                        Arquivo_PLS arquivo = new Arquivo_PLS();
                        arquivo.Abrir(video);
                    }
                    #endregion  Playlist de Vídeos (PLS)
                    String[] VideosV01 = Directory.GetFiles(diretorioRaiz, "*.v01", SearchOption.AllDirectories);
                    foreach (String video in VideosV01)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Vídeos V01: " + video;
                        VideoV01 arquivo = new VideoV01();
                        arquivo.Abrir(video);
                    }

                    String[] VideosV02 = Directory.GetFiles(diretorioRaiz, "*.v02", SearchOption.AllDirectories);
                    foreach (String video in VideosV02)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Vídeos V02: " + video;
                        VideoV02 arquivo = new VideoV02();
                        arquivo.Abrir(video);
                    }

                    String[] VideosV04 = Directory.GetFiles(diretorioRaiz, "*.v04", SearchOption.AllDirectories);
                    foreach (String video in VideosV04)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Vídeos V04: " + video;
                        VideoV04 arquivo = new VideoV04();
                        arquivo.Abrir(video);
                    }
                }
                else if (arquivoOrigem.EndsWith(Util.Util.ARQUIVO_NSS)|| arquivoOrigem.EndsWith(Util.Util.ARQUIVO_NFX))
                {
                    #region Verificar Arquivos do NSS
                    #region Verificar Firmwares
                    String[] firmwares = Directory.GetFiles(diretorioRaiz, "*.fir", SearchOption.AllDirectories);
                    foreach (String firmware in firmwares)
                    {
                        ultimoStatus = "Verificando Integridade do Firmware: " + firmware;
                        Arquivo_FIR arquivo = new Arquivo_FIR();
                        arquivo.Abrir(firmware);
                    }
                    String[] optionFirmwares = Directory.GetFiles(diretorioRaiz, "*.opt", SearchOption.AllDirectories);
                    foreach (String option in optionFirmwares)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos Options de Firmwares: " + option;
                        Arquivo_OPT arquivo = new Arquivo_OPT();
                        arquivo.Abrir(option);
                    }
                    #endregion Verificar Firmwares

                    #region Fontes (FNT)
                    String[] fontes = Directory.GetFiles(diretorioRaiz, "*.fnt", SearchOption.AllDirectories);
                    foreach (String fonte in fontes)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Fonte: " + fonte;
                        Arquivo_FNT arquivo = new Arquivo_FNT();
                        arquivo.Abrir(fonte);
                    }
                    #endregion Fontes (FNT)
                                        
                    String[] waves = Directory.GetFiles(diretorioRaiz, "*.wav", SearchOption.AllDirectories);
                    foreach (String wave in waves)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Audio WAV: " + wave;
                        NSS_Arquivo_WAV arquivo = new NSS_Arquivo_WAV();
                        if (!arquivo.VerificarFormatoWavePCM(wave))
                        {
                            throw new NotImplementedException();
                        }

                    }

                    String[] VideosV02 = Directory.GetFiles(diretorioRaiz, "*.v02", SearchOption.AllDirectories);
                    foreach (String video in VideosV02)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Vídeos V02: " + video;
                        VideoV02 arquivo = new VideoV02();
                        arquivo.Abrir(video);
                    }

                    #region  Playlist de Audios (PLA)
                    String[] audiosPLA = Directory.GetFiles(diretorioRaiz, "*.pla", SearchOption.AllDirectories);
                    foreach (String audio in audiosPLA)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Playlist de Audios: " + audio;
                        Arquivo_PLS arquivo = new Arquivo_PLS();
                        arquivo.Abrir(audio);
                    }
                    #endregion  Playlist de Vídeos (PLS)
                    #region  Playlist de Vídeos (PLS)
                    String[] VideosPLS = Directory.GetFiles(diretorioRaiz, "*.pls", SearchOption.AllDirectories);
                    foreach (String video in VideosPLS)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Playlist de Vídeos: " + video;
                        Arquivo_PLS arquivo = new Arquivo_PLS();
                        arquivo.Abrir(video);
                    }
                    #endregion  Playlist de Vídeos (PLS)

                    String[] binarios = Directory.GetFiles(diretorioRaiz, "*.bin", SearchOption.AllDirectories);
                    foreach (String binario in binarios)
                    {                     
                        ultimoStatus = "Verificando Integridade dos arquivos de BIN: " + binario;
                        
                        byte[] buffer = File.ReadAllBytes(binario);
                        List<byte> bufferSemCRC = new List<byte>();
                        bufferSemCRC.AddRange(buffer);
                        // Posiçao do CRC 30 e 31
                        bufferSemCRC.RemoveAt(30);
                        bufferSemCRC.RemoveAt(30);
                        ushort crc = Util.CRC16CCITT.Calcular(bufferSemCRC.ToArray());
                        if (crc != BitConverter.ToUInt16(buffer, 30))
                        {
                            throw new NotImplementedException();
                        }                        
                    }
                    /*                                         
                     Lista.crc
                     painel.cfg
                     config.cfg
                     idioma.lpk                        
                     */
                    String[] listaCRC = Directory.GetFiles(diretorioRaiz, "Lista.crc", SearchOption.AllDirectories);
                    foreach (String crc in listaCRC)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de Lista.crc. ";
                       // NSS_Arquivo_ListaCRC arquivo = new NSS_Arquivo_ListaCRC();
                       // arquivo.Abrir(crc);
                    }

                    String[] painelCFG = Directory.GetFiles(diretorioRaiz, "painel.cfg", SearchOption.AllDirectories);
                    foreach (String painel in painelCFG)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de painel.cfg. ";
                        byte[] buffer = File.ReadAllBytes(painel);
                        List<byte> bufferSemCRC = new List<byte>();
                        bufferSemCRC.AddRange(buffer);
                        // Posiçao do CRC 30 e 31
                        bufferSemCRC.RemoveAt(62);
                        bufferSemCRC.RemoveAt(62);
                        ushort crc = Util.CRC16CCITT.Calcular(bufferSemCRC.ToArray());
                        if (crc != BitConverter.ToUInt16(buffer, 62))
                        {
                            throw new NotImplementedException();
                        }   
                    }


                    String[] configCFG = Directory.GetFiles(diretorioRaiz, "config.cfg", SearchOption.AllDirectories);
                    foreach (String config in configCFG)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de config.cfg. ";
                        byte[] buffer = File.ReadAllBytes(config);
                        List<byte> bufferSemCRC = new List<byte>();
                        bufferSemCRC.AddRange(buffer);
                        // Posiçao do CRC 30 e 31
                        bufferSemCRC.RemoveAt(62);
                        bufferSemCRC.RemoveAt(62);
                        ushort crc = Util.CRC16CCITT.Calcular(bufferSemCRC.ToArray());
                        if (crc != BitConverter.ToUInt16(buffer, 62))
                        {
                            throw new NotImplementedException();
                        }   
                    }

                    String[] idiomaLPK = Directory.GetFiles(diretorioRaiz, "idioma.lpk", SearchOption.AllDirectories);
                    foreach (String idioma in idiomaLPK)
                    {
                        ultimoStatus = "Verificando Integridade dos arquivos de idioma.lpk. ";
                        NSS_Arquivo_LPK arquivo = new NSS_Arquivo_LPK();
                        byte[] buffer = File.ReadAllBytes(idioma);
                        List<byte> bufferSemCRC = new List<byte>();
                        bufferSemCRC.AddRange(buffer);
                        // Posiçao do CRC 30 e 31
                        bufferSemCRC.RemoveAt(62);
                        bufferSemCRC.RemoveAt(62);
                        ushort crc = Util.CRC16CCITT.Calcular(bufferSemCRC.ToArray());
                        if (crc != BitConverter.ToUInt16(buffer, 62))
                        {
                            throw new NotImplementedException();
                        }     
                    }

                    #endregion Verificar Arquivos do NSS

                }
            }
            catch
            {
                throw new Exception("ERRO AO VERIFICAR NANDFFS: " + ultimoStatus);
            }
            finally
            {
                Directory.Delete(diretorioRaiz, true);
            }
            #endif
        }
        public void GerarDiretoriosFAT(string arquivoOrigem, string diretorioRaiz)
        {
            if (!isOld)
            {
                GerarDiretoriosFATOtimizado(arquivoOrigem, diretorioRaiz);
                return;
            }
            List<String> Diretorios = new List<string>();
            if (!File.Exists(arquivoOrigem))
            {
                throw new FileNotFoundException("This file " + arquivoOrigem + " was not found.");
            }
            if (!Directory.Exists(diretorioRaiz))
            {
                Directory.CreateDirectory(diretorioRaiz);
            }
            FileStream fNand = File.OpenRead(arquivoOrigem);
            byte[] buffer = new byte[fNand.Length];

            PaginaNandFlash[] listaPaginas = new PaginaNandFlash[fNand.Length / 2112];

            for (int i = 0; i < buffer.Length; i++)
            {
                fNand.Read(buffer, i, 1);
            }
            fNand.Close();
            for (int i = 0; i < listaPaginas.Length; i++)
            {
                byte[] bufferLocal = new byte[TAMANHO_PAGINA + TAMANHO_CI];
                Array.Copy(buffer, i * 2112, bufferLocal, 0, 2112);
                listaPaginas[i] = new PaginaNandFlash();
                listaPaginas[i].LoadFromBytes(bufferLocal);
                if (listaPaginas[i].CI[0] == 0)
                {
                    Diretorios.Add(Encoding.ASCII.GetString(listaPaginas[i].Dados, 0, 8).Replace("\0", String.Empty));
                }


            }
            List<byte> dados = new List<byte>();
            for (int i = 1; i < listaPaginas.Length; i++)
            {
                if (listaPaginas[i].CI[0] == (byte)(TipoObjeto.DIR_OBJECT))
                {
                    string diretorioCriado = Encoding.ASCII.GetString(listaPaginas[i].Dados, 0, 8).Replace("\0", String.Empty);
                    int parentId = listaPaginas[i].CI[4];
                    int objectId = i;
                    while (parentId != 0)
                    {
                        diretorioCriado = Encoding.ASCII.GetString(listaPaginas[parentId].Dados, 0, 8).Replace("\0", String.Empty) + "\\" + diretorioCriado;

                        parentId = listaPaginas[parentId].CI[4];
                    }
                    if (parentId == 0)
                    {
                        diretorioCriado = diretorioRaiz + "\\" + diretorioCriado;
                        Directory.CreateDirectory(diretorioCriado);
                    }
                }
                if (listaPaginas[i].CI[0] == (byte)(TipoObjeto.DATA_OBJECT))
                {
                    dados.AddRange(listaPaginas[i].Dados);
                }
                if (listaPaginas[i].CI[0] == (byte)(TipoObjeto.FILE_OBJECT))
                {
                    string nomeArquivo = Encoding.ASCII.GetString(listaPaginas[i].Dados, 0, 8).Replace("\0", String.Empty);
                    string extensao = Encoding.ASCII.GetString(listaPaginas[i].Dados, 8, 3).Replace("\0", String.Empty);
                    UInt32 tamanho = BitConverter.ToUInt32(listaPaginas[i].Dados, 12);
                    int parentId = listaPaginas[i].CI[4];
                    int objectId = i;

                    while (parentId != 0)
                    {
                        nomeArquivo = Encoding.ASCII.GetString(listaPaginas[parentId].Dados, 0, 8).Replace("\0", String.Empty) + "\\" + nomeArquivo;

                        parentId = listaPaginas[parentId].CI[4];
                    }
                    if (parentId == 0)
                    {
                        byte[] dadosLocal = new byte[tamanho];

                        for (int j = 0; j < tamanho; j++)
                        {
                            dadosLocal[j] = dados[j];
                        }

                        nomeArquivo = diretorioRaiz + "\\" + nomeArquivo + "." + extensao;
                        // FILE          
                        File.WriteAllBytes(nomeArquivo, dadosLocal);

                        // Limpa a coleção de dados;
                        dados.Clear();

                        if (nomeArquivo.EndsWith(Util.Util.ARQUIVO_NSS))
                        {
                            GerarDiretoriosFAT(nomeArquivo, diretorioRaiz + "\\" + "FRT_APP");
                        }
                        if (nomeArquivo.EndsWith(Util.Util.ARQUIVO_NFX))
                        {
                            GerarDiretoriosFAT(nomeArquivo, diretorioRaiz + "\\" + "FRT_APP");
                        }
                    }
                }
            }     


                  
        }

        public void GerarDiretoriosFATOtimizado(string arquivoOrigem, string diretorioRaiz)
        {
            StringBuilder status = new StringBuilder();
            List<String> Diretorios = new List<string>();
            if (!File.Exists(arquivoOrigem))
            {
                throw new FileNotFoundException("This file " + arquivoOrigem + " was not found.");
            }
            status.AppendLine("1");
            if (!Directory.Exists(diretorioRaiz))
            {
                Directory.CreateDirectory(diretorioRaiz);
            }
            FileStream fNand = File.OpenRead(arquivoOrigem);
            byte[] buffer = new byte[fNand.Length];

            int tamanhoPaginas = (int)fNand.Length / 2112;

            tamanhoPaginas += 1;

            List<PaginaNandFlash> listaPaginas = new List<PaginaNandFlash>();
            // PaginaNandFlash[] listaPaginas = new PaginaNandFlash[tamanhoPaginas];

            for (int i = 0; i < buffer.Length; i++)
            {
                fNand.Read(buffer, i, 1);
            }
            fNand.Close();
            int indiceCI = 0;
            int indicePagina = (int)TAMANHO_CI;
            // Preparar as páginas
            for (int i = 0; i < buffer.Length; i++)
            {
                byte[] bufferLocal = new byte[TAMANHO_PAGINA + TAMANHO_CI];
                byte[] bufferCI = new byte[TAMANHO_CI];
                byte[] bufferDados = new byte[TAMANHO_PAGINA];

                Array.Copy(buffer, indiceCI, bufferCI, 0, TAMANHO_CI);

                int tamanhoPagina = BitConverter.ToUInt16(bufferCI, 8);

                Array.Copy(buffer, indicePagina, bufferDados, 0, tamanhoPagina);

                indiceCI += (int)(TAMANHO_CI + tamanhoPagina);
                indicePagina += (int)(TAMANHO_CI + tamanhoPagina);

                Array.Copy(bufferDados, 0, bufferLocal, 0, TAMANHO_PAGINA);
                Array.Copy(bufferCI, 0, bufferLocal, TAMANHO_PAGINA, TAMANHO_CI);

                PaginaNandFlash pagina = new PaginaNandFlash();

                pagina.CI = bufferCI;
                pagina.Dados = bufferDados;

                listaPaginas.Add(pagina);

                i = indiceCI - 1;
            }



            for (int i = 0; i < listaPaginas.Count; i++)
            {
                if (listaPaginas[i].CI[0] == 0)
                {
                    Diretorios.Add(Encoding.ASCII.GetString(listaPaginas[i].Dados, 0, 8).Replace("\0", String.Empty));
                }

            }
            List<byte> dados = new List<byte>();
            for (int i = 1; i < listaPaginas.Count; i++)
            {
                if (listaPaginas[i].CI[0] == (byte)(TipoObjeto.DIR_OBJECT))
                {
                    string diretorioCriado = Encoding.ASCII.GetString(listaPaginas[i].Dados, 0, 8).Replace("\0", String.Empty);
                    int parentId = listaPaginas[i].CI[4];
                    int objectId = i;
                    while (parentId != 0)
                    {
                        diretorioCriado = Encoding.ASCII.GetString(listaPaginas[parentId].Dados, 0, 8).Replace("\0", String.Empty) + "\\" + diretorioCriado;

                        parentId = listaPaginas[parentId].CI[4];
                    }
                    if (parentId == 0)
                    {
                        diretorioCriado = diretorioRaiz + "\\" + diretorioCriado;
                        Directory.CreateDirectory(diretorioCriado);
                    }
                }
                if (listaPaginas[i].CI[0] == (byte)(TipoObjeto.DATA_OBJECT))
                {
                    dados.AddRange(listaPaginas[i].Dados);
                }
                if (listaPaginas[i].CI[0] == (byte)(TipoObjeto.FILE_OBJECT))
                {
                    string nomeArquivo = Encoding.ASCII.GetString(listaPaginas[i].Dados, 0, 8).Replace("\0", String.Empty);
                    string extensao = Encoding.ASCII.GetString(listaPaginas[i].Dados, 8, 3).Replace("\0", String.Empty);
                    UInt32 tamanho = BitConverter.ToUInt32(listaPaginas[i].Dados, 12);
                    int parentId = listaPaginas[i].CI[4];
                    int objectId = i;

                    while (parentId != 0)
                    {
                        nomeArquivo = Encoding.ASCII.GetString(listaPaginas[parentId].Dados, 0, 8).Replace("\0", String.Empty) + "\\" + nomeArquivo;

                        parentId = listaPaginas[parentId].CI[4];
                    }
                    if ((parentId == 0) || extensao == "LDX2")
                    {
                        byte[] dadosLocal = new byte[tamanho];

                        for (int j = 0; j < tamanho; j++)
                        {
                            dadosLocal[j] = dados[j];
                        }

                        nomeArquivo = diretorioRaiz + "\\" + nomeArquivo + "." + extensao;
                        // FILE          
                        File.WriteAllBytes(nomeArquivo, dadosLocal);
                        status.AppendLine(nomeArquivo);
                        // Limpa a coleção de dados;
                        dados.Clear();
                    }
                }

            }
        }
        public void GerarArquivoNandOLD(string diretorioRaiz, string arquivoDestino)
        {
            int bytesValidos = 0;
            listaDiretoriosArquivos = new List<string>();
            List<String> listaArquivos = new List<string>();

            listaDiretoriosArquivos.Add(diretorioRaiz);
            
            diretorioRaiz = diretorioRaiz + "\\";

            // NÃO REMOVER - pois irá implicar no mal funcionamento do Firmware.
            diretorioRaiz = diretorioRaiz.Replace("\\\\", "\\");

            diretorios = Directory.GetDirectories(diretorioRaiz, "*.*", SearchOption.AllDirectories);

            //listaArquivos.AddRange(Directory.GetFiles(diretorioRaiz, "*.*", SearchOption.AllDirectories));
            /*             
                1.	Os diretórios devem ser salvos primeiro;
                2.	O diretório raiz deve ser o primeiro;
                3.	Sub-diretórios devem ser salvos após os diretórios pais;
                4.	Arquivos da raiz devem ser salvos em seguida;
                5.	Em seguida, salvar arquivos de tipos não muito frequentes, por exemplo: *.fnt, *.alt, *.cfg, *.lst. A quantidade de arquivos destes tipos não cresce com o número de roteiros e mensagens;
                6.	Em seguida, salvar arquivos *.msg;
                7.	Depois, salvar arquivos *.rot;
                8.	Por fim, salvar arquivos *.mpt e *.rpt.             
             */
            listaArquivos.AddRange(CarregarListaArquivos(diretorioRaiz));
            
            arquivos = listaArquivos.ToArray();
                        
            listaDiretoriosArquivos.AddRange(diretorios);
            listaDiretoriosArquivos.AddRange(arquivos);

            GerarDirChunks(diretorioRaiz);
            GerarFileChunks(diretorioRaiz);

            if (File.Exists(arquivoDestino))
            {
                File.Delete(arquivoDestino);
            }

            FileStream fs = File.Create(arquivoDestino);            

            for (int i = 0; i < listaDirChunk.Length; i++)
            {
                byte[] dados = listaDirChunk[i].toBytes();
                bytesValidos = dados.Length;                
                Array.Resize(ref dados, (int)TAMANHO_PAGINA);
                fs.Write(dados, 0, dados.Length);
                listaDirChunkTags[i].objectId = (uint) ((fs.Length - TAMANHO_PAGINA)/2112);
                listaDirChunkTags[i].chunkLength = (UInt16)bytesValidos;
                dados = listaDirChunkTags[i].toBytes();
                Array.Resize(ref dados, (int)TAMANHO_CI);
                fs.Write(dados, 0, dados.Length);
            }
            for (int i = 0; i < listaFileChunk.Length; i++)
            {
                FileStream fs1 = File.OpenRead(arquivos[i]);
                byte[] buffer = new byte[fs1.Length];
                fs1.Read(buffer, 0, buffer.Length);
                fs1.Close();

                if (buffer.Length <= TAMANHO_PAGINA)
                {
                    bytesValidos = buffer.Length;

                    Array.Resize(ref buffer, (int)TAMANHO_PAGINA);
                    fs.Write(buffer, 0, buffer.Length);
                    listaFileChunkTags[i].objectId = (uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                    listaFileChunkTags[i].objectType = TipoObjeto.DATA_OBJECT;
                    listaFileChunkTags[i].chunkLength = (UInt16)bytesValidos;
                    byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();

                    Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);
                    fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);
                }
                else
                {
                    int qntPaginasNecessarias = (int)((buffer.Length / TAMANHO_PAGINA) + (buffer.Length % TAMANHO_PAGINA == 0 ? 0 : 1));

                    for (int j = 0; j < qntPaginasNecessarias; j++)
                    {
                        if ((j + 1 == qntPaginasNecessarias) && (buffer.Length % TAMANHO_PAGINA != 0))
                        {
                            byte[] dadosLocal = new byte[TAMANHO_PAGINA];
                            bytesValidos = dadosLocal.Length;
                            Array.Copy(buffer, (int) (j*TAMANHO_PAGINA), dadosLocal, 0, (int) (buffer.Length%TAMANHO_PAGINA));
                            fs.Write(dadosLocal, 0, dadosLocal.Length);
                        }
                        else
                        {
                            bytesValidos = (UInt16)TAMANHO_PAGINA;
                            fs.Write(buffer, (int)(j * TAMANHO_PAGINA), (int)TAMANHO_PAGINA);
                        }
                        listaFileChunkTags[i].objectId = (uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                        listaFileChunkTags[i].objectType = TipoObjeto.DATA_OBJECT;
                        listaFileChunkTags[i].chunkLength = (UInt16)bytesValidos;
                        byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();
                        Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);

                        fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);
                    }

                }

                byte[] dados = listaFileChunk[i].toBytes();
                bytesValidos = dados.Length;
                Array.Resize(ref dados, (int)TAMANHO_PAGINA);
                fs.Write(dados, 0, dados.Length);
                {
                    listaFileChunkTags[i].objectId = (uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                    listaFileChunkTags[i].objectType = TipoObjeto.FILE_OBJECT;
                    listaFileChunkTags[i].chunkLength = (UInt16) bytesValidos;
                    byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();
                    Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);
                    fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);
                }                
            }
            if (fs.Length % 2112 != 0)
            {
                throw new Exception("Tamanho Inválido do arquivo gerado!!! ");
            }
            fs.Close();
        }
        public void GerarArquivoNand(string diretorioRaiz, string arquivoDestino)
        {
            int bytesValidos = 0;
            listaDiretoriosArquivos = new List<string>();
            List<String> listaArquivos = new List<string>();

            listaDiretoriosArquivos.Add(diretorioRaiz);

            diretorioRaiz = diretorioRaiz + "\\";

            // NÃO REMOVER - pois irá implicar no mal funcionamento do Firmware.
            diretorioRaiz = diretorioRaiz.Replace("\\\\", "\\");

            diretorios = Directory.GetDirectories(diretorioRaiz, "*.*", SearchOption.AllDirectories);

            //listaArquivos.AddRange(Directory.GetFiles(diretorioRaiz, "*.*", SearchOption.AllDirectories));
            /*             
                1.	Os diretórios devem ser salvos primeiro;
                2.	O diretório raiz deve ser o primeiro;
                3.	Sub-diretórios devem ser salvos após os diretórios pais;
                4.	Arquivos da raiz devem ser salvos em seguida;
                5.	Em seguida, salvar arquivos de tipos não muito frequentes, por exemplo: *.fnt, *.alt, *.cfg, *.lst. A quantidade de arquivos destes tipos não cresce com o número de roteiros e mensagens;
                6.	Em seguida, salvar arquivos *.msg;
                7.	Depois, salvar arquivos *.rot;
                8.	Por fim, salvar arquivos *.mpt e *.rpt.             
             */
            listaArquivos.AddRange(CarregarListaArquivos(diretorioRaiz));

            arquivos = listaArquivos.ToArray();

            listaDiretoriosArquivos.AddRange(diretorios);
            listaDiretoriosArquivos.AddRange(arquivos);

            GerarDirChunks(diretorioRaiz);
            GerarFileChunks(diretorioRaiz);

            if (File.Exists(arquivoDestino))
            {
                File.Delete(arquivoDestino);
            }

            FileStream fs = File.Create(arquivoDestino);

            for (int i = 0; i < listaDirChunk.Length; i++)
            {
                byte[] dados = listaDirChunk[i].toBytes();
                bytesValidos = dados.Length;
                Array.Resize(ref dados, (int)TAMANHO_PAGINA);
                fs.Write(dados, 0, dados.Length);
                listaDirChunkTags[i].objectId = (uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                listaDirChunkTags[i].chunkLength = (UInt16)bytesValidos;
                dados = listaDirChunkTags[i].toBytes();
                Array.Resize(ref dados, (int)TAMANHO_CI);
                fs.Write(dados, 0, dados.Length);
            }
            for (int i = 0; i < listaFileChunk.Length; i++)
            {
                FileStream fs1 = File.OpenRead(arquivos[i]);
                byte[] buffer = new byte[fs1.Length];
                fs1.Read(buffer, 0, buffer.Length);
                fs1.Close();

                if (buffer.Length <= TAMANHO_PAGINA)
                {
                    bytesValidos = buffer.Length;

                    Array.Resize(ref buffer, (int)TAMANHO_PAGINA);
                    fs.Write(buffer, 0, buffer.Length);
                    listaFileChunkTags[i].objectId = (uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                    listaFileChunkTags[i].objectType = TipoObjeto.DATA_OBJECT;
                    listaFileChunkTags[i].chunkLength = (UInt16)bytesValidos;
                    byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();

                    Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);
                    fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);
                }
                else
                {
                    int qntPaginasNecessarias = (int)((buffer.Length / TAMANHO_PAGINA) + (buffer.Length % TAMANHO_PAGINA == 0 ? 0 : 1));

                    for (int j = 0; j < qntPaginasNecessarias; j++)
                    {
                        if ((j + 1 == qntPaginasNecessarias) && (buffer.Length % TAMANHO_PAGINA != 0))
                        {
                            byte[] dadosLocal = new byte[TAMANHO_PAGINA];
                            bytesValidos = dadosLocal.Length;
                            Array.Copy(buffer, (int)(j * TAMANHO_PAGINA), dadosLocal, 0, (int)(buffer.Length % TAMANHO_PAGINA));
                            fs.Write(dadosLocal, 0, dadosLocal.Length);
                        }
                        else
                        {
                            bytesValidos = (UInt16)TAMANHO_PAGINA;
                            fs.Write(buffer, (int)(j * TAMANHO_PAGINA), (int)TAMANHO_PAGINA);
                        }
                        listaFileChunkTags[i].objectId = (uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                        listaFileChunkTags[i].objectType = TipoObjeto.DATA_OBJECT;
                        listaFileChunkTags[i].chunkLength = (UInt16)bytesValidos;
                        byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();
                        Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);

                        fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);
                    }

                }

                byte[] dados = listaFileChunk[i].toBytes();
                bytesValidos = dados.Length;
                Array.Resize(ref dados, (int)TAMANHO_PAGINA);
                fs.Write(dados, 0, dados.Length);
                {
                    listaFileChunkTags[i].objectId = (uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                    listaFileChunkTags[i].objectType = TipoObjeto.FILE_OBJECT;
                    listaFileChunkTags[i].chunkLength = (UInt16)bytesValidos;
                    byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();
                    Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);
                    fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);
                }
            }
            if (fs.Length % 2112 != 0)
            {
                throw new Exception("Tamanho Inválido do arquivo gerado!!! ");
            }
            fs.Close();            
        }
        public void GerarArquivoNandOtimizado(string diretorioRaiz, string arquivoDestino)
        {
            if (isOld)
            {
                GerarArquivoNand(diretorioRaiz, arquivoDestino);
                return;
            }
            else
            {
                if (arquivoDestino.EndsWith(Util.Util.ARQUIVO_B12))
                {
                    arquivoDestino = arquivoDestino.Replace(Util.Util.ARQUIVO_B12, Util.Util.ARQUIVO_LDNFX);
                }
                if (arquivoDestino.EndsWith(Util.Util.ARQUIVO_NSS))
                {
                    arquivoDestino = arquivoDestino.Replace(Util.Util.ARQUIVO_NSS, Util.Util.ARQUIVO_NFX);
                }
            }
            uint objectID = 0; 
            int bytesValidos = 0;
            listaDiretoriosArquivos = new List<string>();
            List<String> listaArquivos = new List<string>();

            listaDiretoriosArquivos.Add(diretorioRaiz);

            diretorioRaiz = diretorioRaiz + "\\";

            // NÃO REMOVER - pois irá implicar no mal funcionamento do Firmware.
            diretorioRaiz = diretorioRaiz.Replace("\\\\", "\\");

            diretorios = Directory.GetDirectories(diretorioRaiz, "*.*", SearchOption.AllDirectories);

            //listaArquivos.AddRange(Directory.GetFiles(diretorioRaiz, "*.*", SearchOption.AllDirectories));
            /*             
                1.	Os diretórios devem ser salvos primeiro;
                2.	O diretório raiz deve ser o primeiro;
                3.	Sub-diretórios devem ser salvos após os diretórios pais;
                4.	Arquivos da raiz devem ser salvos em seguida;
                5.	Em seguida, salvar arquivos de tipos não muito frequentes, por exemplo: *.fnt, *.alt, *.cfg, *.lst. A quantidade de arquivos destes tipos não cresce com o número de roteiros e mensagens;
                6.	Em seguida, salvar arquivos *.msg;
                7.	Depois, salvar arquivos *.rot;
                8.	Por fim, salvar arquivos *.mpt e *.rpt.             
             */
            listaArquivos.AddRange(CarregarListaArquivos(diretorioRaiz));

            arquivos = listaArquivos.ToArray();

            listaDiretoriosArquivos.AddRange(diretorios);
            listaDiretoriosArquivos.AddRange(arquivos);

            GerarDirChunks(diretorioRaiz);
            GerarFileChunks(diretorioRaiz);

            if (File.Exists(arquivoDestino))
            {
                File.Delete(arquivoDestino);
            }

            FileStream fs = File.Create(arquivoDestino);

            for (int i = 0; i < listaDirChunk.Length; i++)
            {
                byte[] dados = listaDirChunk[i].toBytes();
                bytesValidos = dados.Length;

                listaDirChunkTags[i].objectId = objectID++; //(uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                listaDirChunkTags[i].chunkLength = (UInt16)bytesValidos;
                byte[] dadosChunkTags = listaDirChunkTags[i].toBytes();
                Array.Resize(ref dadosChunkTags, (int)TAMANHO_CI);
                fs.Write(dadosChunkTags, 0, dadosChunkTags.Length);

                dados = listaDirChunk[i].toBytes();
                //Array.Resize(ref dados, (int)TAMANHO_PAGINA);
                fs.Write(dados, 0, dados.Length);                
            }
            for (int i = 0; i < listaFileChunk.Length; i++)
            {
                FileStream fs1 = File.OpenRead(arquivos[i]);
                byte[] buffer = new byte[fs1.Length];
                fs1.Read(buffer, 0, buffer.Length);
                fs1.Close();

                if (buffer.Length <= TAMANHO_PAGINA)
                {
                    bytesValidos = buffer.Length;

                    listaFileChunkTags[i].objectId = objectID++; //(uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                    listaFileChunkTags[i].objectType = TipoObjeto.DATA_OBJECT;
                    listaFileChunkTags[i].chunkLength = (UInt16)bytesValidos;
                    byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();

                    Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);
                    fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);


                    //Array.Resize(ref buffer, (int)TAMANHO_PAGINA);
                    fs.Write(buffer, 0, buffer.Length);
                    
                }
                else
                {
                    int qntPaginasNecessarias = (int)((buffer.Length / TAMANHO_PAGINA) + (buffer.Length % TAMANHO_PAGINA == 0 ? 0 : 1));

                    for (int j = 0; j < qntPaginasNecessarias; j++)
                    {
                        listaFileChunkTags[i].objectId = objectID++; //(uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                        listaFileChunkTags[i].objectType = TipoObjeto.DATA_OBJECT;
                        listaFileChunkTags[i].chunkLength = (UInt16)TAMANHO_PAGINA;
                        byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();
                        Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);

                        fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);

                        if ((j + 1 == qntPaginasNecessarias) && (buffer.Length % TAMANHO_PAGINA != 0))
                        {
                            byte[] dadosLocal = new byte[TAMANHO_PAGINA];
                            bytesValidos = dadosLocal.Length;
                            Array.Copy(buffer, (int)(j * TAMANHO_PAGINA), dadosLocal, 0, (int)(buffer.Length % TAMANHO_PAGINA));
                            fs.Write(dadosLocal, 0, dadosLocal.Length);
                        }
                        else
                        {
                            bytesValidos = (UInt16)TAMANHO_PAGINA;
                            fs.Write(buffer, (int)(j * TAMANHO_PAGINA), (int)TAMANHO_PAGINA);
                        }
                       
                    }

                }

                byte[] dados = listaFileChunk[i].toBytes();
                bytesValidos = dados.Length;

                {
                    listaFileChunkTags[i].objectId = objectID++; //(uint)((fs.Length - TAMANHO_PAGINA) / 2112);
                    listaFileChunkTags[i].objectType = TipoObjeto.FILE_OBJECT;
                    listaFileChunkTags[i].chunkLength = (UInt16)bytesValidos;
                    byte[] dadosFileChunk = listaFileChunkTags[i].toBytes();
                    Array.Resize(ref dadosFileChunk, (int)TAMANHO_CI);
                    fs.Write(dadosFileChunk, 0, dadosFileChunk.Length);
                }

               // Array.Resize(ref dados, (int)TAMANHO_PAGINA);
                fs.Write(dados, 0, dados.Length);
                
            }
            if ((isOld)&&(fs.Length % 2112 != 0))
            {
                throw new Exception("Tamanho Inválido do arquivo gerado!!! ");
            }
            fs.Close();

        }
        private void GerarHeader(string arquivoNome)
        {
            List<byte> lista = new List<byte>();
            FileStream fs = File.Open(arquivoNome, FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            byte[] cabecalho = new byte[TAMANHO_CABECALHO]; // TODO: Alterar o tamanho do cabeçalho
            fs.Read(buffer, 0, buffer.Length);
            lista.AddRange(buffer);
            fs.Close();

            File.Delete(arquivoNome);

            fs = File.Create(arquivoNome);
            // Montar Cabeçalho
            cabecalho[0] = 1;  // Versão
            cabecalho[1] = 66; // Extensão B12
            cabecalho[2] = 49; // Extensão B12
            cabecalho[3] = 50; // Extensão B12
            
            lista.InsertRange(0, cabecalho);

            ushort crc = CRC16CCITT.Calcular(lista.ToArray());
            Array.Resize(ref cabecalho, (int)TAMANHO_CABECALHO + sizeof(ushort));
            // Insere o CRC 16 CCITT
            lista.InsertRange((int)TAMANHO_CABECALHO, BitConverter.GetBytes(crc));
            
            buffer = lista.ToArray();

            // Escrever Cabeçalho
            fs.Write(buffer, 0, buffer.Length);

            fs.Close();
        }

        private void GerarFileChunks(string diretorioRaiz)
        {
           // arquivos = Directory.GetFiles(diretorioRaiz, "*.*", SearchOption.AllDirectories);
            listaFileChunk = new FileChunk[arquivos.Length];
            listaFileChunkTags = new ChunkTags[arquivos.Length];

            for (int i = 0; i < listaFileChunk.Length; i++)
            {
                FileInfo fi = new FileInfo(arquivos[i]);

                listaFileChunk[i] = new FileChunk();
                listaFileChunk[i].extension = Path.GetExtension(arquivos[i]).Substring(1);
                listaFileChunk[i].name = Path.GetFileNameWithoutExtension(arquivos[i]).PadRight(8, '\0').Substring(0, 8);
                listaFileChunk[i].size = (uint)fi.Length;

                listaFileChunkTags[i] = new ChunkTags();
                listaFileChunkTags[i].objectType = TipoObjeto.FILE_OBJECT;
                listaFileChunkTags[i].parentId = GetParentIdFileChunk(arquivos[i]);//, listaDirChunk, listaFileChunk, listaDirChunkTags, listaFileChunkTags);
                listaFileChunkTags[i].objectId = (uint)(i + listaDirChunk.Length);
                listaFileChunkTags[i].depth = GetDepth(arquivos[i], diretorioRaiz);

            }
        }

        private byte GetDepth(string caminho, string diretorioRaiz)
        {
            string temp = caminho.Replace(diretorioRaiz, String.Empty);
            string[] partesDiretorio = temp.Split(new char[1] { '\\' });

            return (byte)(partesDiretorio.Length);

        }

        private void GerarDirChunks(string diretorioRaiz)
        {
            diretorios = Directory.GetDirectories(diretorioRaiz, "*.*", SearchOption.AllDirectories);
            listaDirChunk = new DirChunk[diretorios.Length + 1];
            listaDirChunkTags = new ChunkTags[diretorios.Length + 1];

            string[] partesDiretorioRaiz = diretorioRaiz.Split(new char[1] { '\\' });

            // Cria o Chunk do diretório raiz
            listaDirChunk[0] = new DirChunk();
            listaDirChunk[0].name = String.Empty.PadRight(8, '\0');
            listaDirChunkTags[0] = new ChunkTags();
            listaDirChunkTags[0].objectType = TipoObjeto.DIR_OBJECT;
            listaDirChunkTags[0].parentId = 0;
            listaDirChunkTags[0].objectId = 0;
            listaDirChunkTags[0].depth = 0;


            for (int i = 1; i < listaDirChunk.Length; i++)
            {
                string[] partesDiretorio = diretorios[i - 1].Split(new char[1] { '\\' });
                listaDirChunk[i] = new DirChunk();
                listaDirChunk[i].name = partesDiretorio[partesDiretorio.Length - 1].PadRight(8, '\0').Substring(0, 8);
                listaDirChunkTags[i] = new ChunkTags();
                listaDirChunkTags[i].objectType = TipoObjeto.DIR_OBJECT;
                listaDirChunkTags[i].parentId = NandFFS.GetParentIdDirChunk(diretorios[i - 1], listaDirChunk, listaDirChunkTags);
                listaDirChunkTags[i].objectId = (uint)i;
                listaDirChunkTags[i].depth = GetDepth(diretorios[i - 1], diretorioRaiz);
            }
        }
    }
}
