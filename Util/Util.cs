using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;
using System.Diagnostics;
using System.Management;
using System.ServiceProcess;
using IWshRuntimeLibrary;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;


namespace Util
{
    public static class Util
    {
        public enum EnvioRoteiroAPP : int
        {
            NaoTemRotaAPP = 0,
            EnviarRotaAPP = 1,
            NaoEnviarRotaAPP = 2
        }

        public enum IndicePerifericosNaRede : int
        {
            Catraca = 0,
            Velocidade = 1,
            Temperatura = 2,
            APP = 3
        }

        public enum TipoOperacaoEvento : byte
        {
            SELECAO_ROTEIRO,
            SELECAO_IDA_VOLTA,
            SELECAO_ALTERNANCIA,
            SELECAO_MSG_PRINCIPAL,
            SELECAO_MSG_SECUNDARIA,
            ALTERACAO_HORA_SAIDA
        }

        public enum IndiceMensagensEspeciais : int
        {
            BomDia = 0,
            BoaTarde = 1,
            BoaNoite = 2,
            Domingo = 3,
            Segunda = 4,
            Terca = 5,
            Quarta = 6,
            Quinta = 7,
            Sexta = 8,
            Sabado = 9,
            SomenteHora = 10,
            DataHora = 11,
            HoraSaida = 12,
            Temperatura = 13,
            Velocidade = 14,
            Tarifa = 15,
            HoraTemperatura = 16,
            DataHoraTemperatura = 17,
            FraseFontePainel = 18,
            SimboloSeparadorDecimal = 19,
            SimboloVelocidade = 20,
            SimboloTemperatura = 21,
            SimboloTarifa = 22,
            SimboloAM_Espaco = 23,
            SimboloPM_Ponto = 24
            
        }

        public enum Lingua
        {
            Portugues = 01,
            Espanhol = 02,
            Ingles = 03,
            Frances = 04,
            Italiano = 05
        }
        //========================================================
        //Sequenciais para geração de arquivos pelo controlador.
        //devem ser zeradas após execução de Controlador.Salvar();
        //========================================================
        //public static UInt64 sequencial_arquivo_V01 = 0;
        //public static UInt64 sequencial_arquivo_V02 = 0;
        public static UInt64 sequencial_arquivo_videos = 0;
        public static UInt64 sequencial_arquivo_roteiros = 0;
        public static UInt64 sequencial_arquivo_motoristas = 0;
        public static UInt64 sequencial_arquivo_mensagens = 0;
        //==========================================================

        //==========================================================
        //variável para comunicação com as classes de peristência.
        //é por aqui que se sabe qual a extensão de vídeo está sendo gravada no momento.
        //essa variável é alterada pelas funções de 'salvar' das classes de vídeos.
        //e lida pelas classes mais altas para criação de arquivos RPTs(para se saber qual a última extensão de vídeo gravada).
        //todo: verificar melhor forma de implementar essa comunicação.
        //==========================================================
        private static TipoVideo _ultimaExtensao = TipoVideo.V01;
        //==========================================================

        //=====================================================================================================
        //diretório raiz(diretório desktop) onde o controlador será gravado. 
        //O controlador sempre atualiza o valor dessa variável quando vai gravar seus arquivos.

        public static string DIRETORIO_RAIZ = @"C:\teste2\";

        //=====================================================================================================

        public const string DIRETORIO_APP = @"\LDX12\";
        public const string DIRETORIO_ARQUIVOS_FONTE_TEMP = @"C:\Projetos\EXEMPLOS\arquivos-formatos\arquivos-formatos\exemplo\fontes\";
        public const string DIRETORIO_PAINEL = @"\paineis\";
        public const string ARQUIVO_VAR = @"\param.var";
        public const string ARQUIVO_FIX = @"\param.fix";
        public const string ARQUIVO_NSS = @"\nss.nfs";
        public const string ARQUIVO_B12 = @"\ld.b12";
        public const string ARQUIVO_NFX = @"\nss.nfx";
        public const string ARQUIVO_LDNFX = @"\ld.nfx";
        public const string ARQUIVO_TEMP_LDX2 = @"\TEMP.LDX2";
        public const string ARQUIVO_TEMP_LDX = @"\TEMP.LDX";
        public const string DIRETORIO_ARQUIVOS_ROTA = @"\APP\ROTAS\";

        public const string DIRETORIO_FONTES = @"\fontes\";
        public const string DIRETORIO_FONTES_FIRMWARE = @"/fontes/";
        public const string DIRETORIO_NSS = @"\FRT_APP\";
        public const string DIRETORIO_TEMPORARIO = @"\temp\";
        public const string DIRETORIO_IDIOMAS = @"\idiomas\";
        public const string DIRETORIO_MSGS = @"\msgs\";
        public const string DIRETORIO_REGIOES = @"\regioes\";
        public const string DIRETORIO_VIDEOS = @"\videos\";
        public const string DIRETORIO_VIDEOS_FIRMWARE = @"videos/";
        public const string DIRETORIO_ROTEIROS = @"\roteiros\";
        public const string DIRETORIO_DRIVERS = @"\drivers\";
        public const string DIRETORIO_FIRMWARE = @"\firmware\";
        public const string DIRETORIO_DOWNLOADS = @"\downloads\";
        public const string DIRETORIO_BITMAP = @"\Bitmaps\";
        public const string DIRETORIO_REPORTS = @"\Reports\";
        public const string DIRETORIO_ALT = @"\alternancia\";
        public const string DIRETORIO_IDIOMA_GUI = @"\idiomaGUI\";
        public const string DIRETORIO_NFX = @"\NFX\";
        public const string DIRETORIO_APP_NFX = @"ldx2\";


        public const string ARQUIVO_PLS_EMERG = @"\emerg";        
        public const string ARQUIVO_LST_MSGS = @"\msgs.lst";
        public const string ARQUIVO_LST_ROTEIROS = @"\roteiros.lst";
        public const string ARQUIVO_LST_DRIVERS = @"\drivers.lst";
        public const string ARQUIVO_LST_RGN = @"\regioes.lst";
        public const string ARQUIVO_LST_LPK = @"\idiomas.lst";
        public const string ARQUIVO_ALT = @"\altern.alt";
        public const string ARQUIVO_CFG = @"\painel.cfg";

        public const string diretorio_painel_msgs = @"\msgs";
        public const string diretorio_painel_roteiros = @"\roteiros";
        public const string diretorio_painel_drivers = @"\drivers";

        
        public const string FILTRO_ARQUIVO_IMAGEM = @"Bitmap(*.BMP)|*.BMP";
        
        public const string ARQUIVO_PAINEL_NUMEROALGS = @"d2";
        public const string ARQUIVO_MENSAGEM_NUMEROALGS = @"d3";
        public const string ARQUIVO_NUMERO_NUMEROALGS = @"d3";
        public const string ARQUIVO_ROTEIRO_NUMEROALGS = @"d3";
        public const string ARQUIVO_FRAME_NUMEROALGS = @"d3";
        public const string ARQUIVO_VIDEO_NUMEROALGS = @"d3";
        public const string ARQUIVO_SEPARADOR_DIRETORIO = @"/";
        public const string ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS = @"\";

        public const string ASTERISCO = "*";
        public const string ARQUIVO_EXT_ROT = @".rot";
        public const string ARQUIVO_EXT_MSG = @".msg";
        public const string ARQUIVO_EXT_DRV = @".drv";
        public const string ARQUIVO_EXT_MPT = @".mpt";
        public const string ARQUIVO_EXT_RPT = @".rpt";
        public const string ARQUIVO_EXT_DPT = @".dpt";
        public const string ARQUIVO_EXT_V01 = @".v01";
        public const string ARQUIVO_EXT_V02 = @".v02";
        public const string ARQUIVO_EXT_V03 = @".v03";
        public const string ARQUIVO_EXT_V04 = @".v04";
        public const string ARQUIVO_EXT_RGN = @".rgn";
        public const string ARQUIVO_EXT_LPK = @".lpk";
        public const string ARQUIVO_EXT_FNT = @".fnt";
        public const string ARQUIVO_EXT_PLS = @".pls";
        public const string ARQUIVO_EXT_LST = @".lst";
        public const string ARQUIVO_EXT_ALT = @".alt";
        public const string ARQUIVO_EXT_FIR = @".fir";
        public const string ARQUIVO_EXT_OPT = @".opt";
        public const string ARQUIVO_EXT_BMP = @".bmp";
        public const string ARQUIVO_EXT_LDX = @".ldx";
        public const string ARQUIVO_EXT_LDX2_MINUSCULO = @".ldx2";
        public const string ARQUIVO_EXT_LDX2_MAIUSCULO = @".LDX2";
        public const string ARQUIVO_EXT_ATALHO = @".lnk";
        public const string ARQUIVO_EXT_EXE = @".exe";
        public const string ARQUIVO_EXT_ROTA = @".rota";

        public const string ARQUIVO_PAINEL_CFG = @"\painel.cfg";
        public const string ARQUIVO_PAINEL_ALTERNANCIA = @"\altern.alt";
        public const string ARQUIVO_AGENDAMENTO = @"\agenda.sch";
        public const string ARQUIVO_ATIVACAO = @"\done.atv";
        public const string ARQUIVO_ATIVACAO_3DIAS = @"\three.atv";
        public const string ARQUIVO_LIBERACAO = @"\liberacao.pos";

        public const string ARQUIVO_FIR_LDX2 = @"\firmware.fir";
        public const string ARQUIVO_OPT_LDX2 = @"\firmware.opt";
        public const string ARQUIVO_FIR_PAINEL = @"\painel.fir";
        public const string ARQUIVO_OPT_PAINEL = @"\painel.opt";

        public const string ARQUIVO_IDIOMA_GUI = @"\idioma.inf";

        public const string ARQUIVO_VERSAO = @"\versao.inf";

        public const string ARQUIVO_RELATORIO = @"\report.html";

        public const string ARQUIVO_IDIOMA = @"\EN-US.lpk";
        public const string ARQUIVO_REGIAO = @"\EN-US.rgn";
        public const string NOME_REGIAO = @"EN-US";

        public const string NOME_IDIOMA = @"EN-US";

        public const string LPK_EN_US = @"EN-US";
        public const string LPK_PT_BR = @"PT-BR";
        public const string LPK_ES_ES = @"ES-ES";

        public const string RGN_EN_US = @"EN-US";
        public const string RGN_PT_BR = @"PT-BR";
        public const string RGN_ES_ES = @"ES-ES";

        public const int ARQUIVO_LST_PATH_SIZE = 8;
        public const string DIRETORIO_PAI = @"\..";

        public const int CRCPosition1 = 62;
        public const int CRCPosition2 = 63;

        public const string ABRE_TAG = "{{:";
        public const string FECHA_TAG = ":}}";

        //definições para arquivo excel de teste.
        public const int inicio_infos_linha = 5;
        public const int tabela_tarifa_indice_coluna = 2;
        public const string celula_painel_altura = "B2";
        public const string celula_painel_largura = "D2";
        
        public const int indice_coluna_mensagem_texto = 1;
        public const int indice_coluna_mensagem_rolagem = 2;
        public const int indice_coluna_numero_roteiro = 3;
        public const int indice_coluna_largura_numero = 4;
        public const int indice_coluna_fonte_numero = 5;
        public const int indice_coluna_texto = 6;
        public const int indice_coluna_fonte = 7;
        public const int indice_coluna_rolagem = 8;
        //public const string indice_coluna_texto_roteiro = "5";
        
        //definições para arquivos de saudaçães.
        public const string ARQUIVO_BOM_DIA_V01 = "bdia";
        public const string ARQUIVO_BOM_TARDE_V01 = "btar";
        public const string ARQUIVO_BOM_NOITE_V01 = "bnoi";

        public static UInt32 OffSetArquivo = 0;        


        //fonte default.
        public const string FONTE_DEFAULT_5 = "FRT 05x06 Regular x";
        public const string FONTE_DEFAULT_7 = "FRT 07x06 Regular x";
        public const string FONTE_DEFAULT_8 = "FRT 08x06 Bold x";
        public const string FONTE_DEFAULT_11 = "FRT 11x07 Bold x";
        public const string FONTE_DEFAULT_13 = "FRT 13x08 Bold";
        public const string FONTE_DEFAULT_14 = "FRT 14x10 Bold x";
        public const string FONTE_DEFAULT_16 = "FRT 16x10 Bold x";
        public const string FONTE_DEFAULT_26 = "FRT 26x16 Bold x";
        public const string FONTE_DEFAULT_LDX12 = "FRT 16x10 Bold x";
        public const string ALTURA_FONTE_DEFAULT_LDX12 = "16";
        public const string FONTE_DEFAULT_WINDOWS = "Arial";
        public const string ALTURA_FONTE_DEFAULT_WINDOWS = "16";
        public const int ALTURA_MAXIMA_PAINEL = 16;

        public const int ALTURA_MINIMA_FONTE_TRUETYPE = 5;
        public const int ALTURA_MAXIMA_FONTE_TRUETYPE = 30;


        //editor de fonte
        public const string EDITOR_FONTE_ALTURA_FONTE = "16";
        public const string EDITOR_FONTE_LARGURA_FONTE = "16";

        //indice da alternancia padrao a ser exibida e carregada para cada painel
        public const int INDICE_ALTERNANCIA_PADRAO_PAINEL = 12;
        public const int INDICE_ALTERNANCIA_PADRAO_PAINEL_MULTI_LINHAS = 1;
        public static string diretorio_NSS = @"C:\ProgramData\LDX12\FRT_APP";

        public const int DIAS_CHAVE_TRIAL = 3;

        //tamanho minimo da janela de arquivo
        public const int ALTURA_MINIMA_JANELA_ARQUIVO = 768;
        public const int LARGURA_MINIMA_JANELA_ARQUIVO = 1032;
        public const int ALTURA_MINIMA_JANELA_PRINCIPAL = 820;
        public const int LARGURA_MINIMA_JANELA_PRINCIPAL = 1035;

        //nome aplicativo ninbus
        public const string NOME_APP_NINBUS = @"Ninbus";
        public const string NOME_ARQUIVO_NINBUS = @"NinbusApplication";
        public const string NOME_ARQUIVO_NINBUS_SERVER = @"NinbusServerApplication";

        //nome firmware produto
        public const string NOME_FIRMWARE_CONTROLADOR = "ControladorPontos";
        public const string NOME_FIRMWARE_TURBUS = "ControladorTurbus";

        //nome aplicativo app
        public const string NOME_APP_APP = @"APP";
        public const string NOME_ARQUIVO_APP = @"APP";

        //altura painel com texto triplo
        public const int ALTURA_PAINEL_TEXTO_TRIPLO = 26;
        public const int ALTURA_PAINEL_TEXTO_TRIPLO1 = 8;
        public const int ALTURA_PAINEL_TEXTO_TRIPLO2 = 9;
        public const int ALTURA_PAINEL_TEXTO_TRIPLO3 = 9;

        //valores minimos da altura e largura de textos v04
        public const int ALTURA_MINIMA_TEXTOS_V04 = 2;
        public const int LARGURA_MINIMA_TEXTOS_V04 = 5;

        //variavel de comunicação com o APP
        public const int RF_ATUALIZARROTA = 0xA123;

        #region REGIONAIS
        //Enums de configurações regionais.
        //========================================================================================
        public enum FormatoDataHora
        {
            FORMATO_24H,
            FORMATO_AM_PM
        }

        public enum OpcaoAmPm_Ponto
        {
            EXIBIR_AM_PM,
            EXIBIR_PONTO
        }

        public enum UnidadeVelocidade
        {
            UNIDADE_KMpH,
            UNIDADE_MPH
        }

        public enum UnidadeTemperatura
        {
            UNIDADE_CELSIUS,
            UNIDADE_FAHRENHEIT
        }

        public enum Moeda
        {
            MOEDA_REAL,
            MOEDA_DOLAR,
            MOEDA_PESO,
            MOEDA_EURO
        }

        public enum SeparadorDecimal
        {
            VIRGULA,
            PONTO
        }
        //========================================================================================
        #endregion


        public enum ValidacaoAtivacao
        {
            Ativado = 0,
            Expirado = 1,
            EmAtivacao = 2
        }

        //PLS (arquivo de playlist também é considerado um tipo video)
        public enum TipoVideo: byte
        {
            V01 = 0,
            V02 = 1,
            V03 = 2,
            V04 = 3,
            PLS = 4
        }

        public enum AlinhamentoVertical : byte
        {
            Centro = 0,
            Cima = 1,
            Baixo = 2,

        }


        public enum AlinhamentoHorizontal : byte
        {            
            Direita = 0,
            Esquerda = 1,
            Centralizado = 2
        }

        public enum FormatoTipoCelula
        {
            TextoSimples = 0,
            TextoSimplesSimples = 1,
            TextoSimplesDuplo = 2,
            TextoDuploSimples = 3,
            TextoDuploDuplo = 4
            // TODO: FIXME Próximos a implementar
            //TextoSimplesTriplo = 5,
            //TextoTriploSimples = 6
        }

        public static string retornarAtributoLinha(TextReader fileInput, string atributo)
        {
            string linha = "";
            string strPalavra = "";
            string resultado = "";
            //if (fileInput.Peek().Equals(-1))
            //{
            //    for (int i = 0; i < _contadorLinhas; i++)
            //    {
            //        fileInput.ReadLine();
            //    }
            //}
            while (fileInput.Peek() > -1)
            {
                linha = fileInput.ReadLine();
                // _contadorLinhas++;
                for (int i = atributo.Length; i < linha.Length; i++)
                {
                    strPalavra = linha.Substring(0, i);
                    if (strPalavra.Equals(atributo + "="))
                    {
                        resultado = linha.Substring(i, linha.Length - i);
                        return resultado.TrimStart();
                    }
                }
            }

            return resultado;
        }
        public static bool VerificarExistenciaVelocidade(string arquivoNome)
        {
            TextReader input = new StreamReader(arquivoNome, Encoding.UTF8, true);
            bool resposta = false;
            try
            {
                if (!retornarAtributoLinha(input, "FrasesFixasLCD[70]").Equals(String.Empty))
                {
                    resposta = true;
                }
                else
                {
                    resposta = false;
                }
            }
            catch
            {
                resposta = false;
            }

            input.Close();
            return resposta;
        }

        public static bool EjectUnit(String unit)
        {
            String[] parties;
            if (unit.Contains(":"))
            {
                parties = unit.Split(':');
                unit = parties[0];
                if (unit.ToLower().Equals("c"))
                    return true;
            }
            
            USBEject eject = new USBEject(unit);

            return eject.Eject();
        }
        public static string IdentificarDispositivoUSB()
        {
            String retorno = String.Empty;
            String[] diskArray;
            String driveNumber;
            String driveLetter;

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");

            foreach (ManagementObject dm in searcher1.Get())
            {
                diskArray = null;
                driveLetter = dm.GetPropertyValue("Dependent").ToString().Substring(dm.GetPropertyValue("Dependent").ToString().Length - 3, 2);
                diskArray = dm.GetPropertyValue("Antecedent").ToString().Split(',');
                driveNumber = diskArray[0].Remove(0, diskArray[0].Length - 2).Trim();

                if (driveNumber[0].Equals('#'))
                {
                    driveNumber = driveNumber.Remove(0, 1);
                }

                /* This is where we get the drive serial */
                ManagementObjectSearcher disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                foreach (ManagementObject disk in disks.Get())
                {
                    if (disk["Name"].ToString() == ("\\\\.\\PHYSICALDRIVE" + driveNumber) && disk["InterfaceType"].ToString() == "USB")
                    {
                        String validar = disk["Model"].ToString();

                        if (!validar.Contains("SD/MMC"))
                        {
                            retorno = driveLetter;
                            break;
                        }
                    }
                }
            }

            return retorno;
        }
        public enum TipoModelo : byte
        {
            Texto = 0,
            NúmeroTexto = 1,
            TextoNúmero = 2,
            TextoDuplo = 3,
            NúmeroTextoDuplo = 4,
            TextoDuploNúmero = 5,
            TextoDuploTextoDuplo = 6,
            TextoTriplo = 7,
            NumeroTextoTriplo = 8,
            TextoTriploNumero = 9
        }

        public enum Rolagem : byte
        {
            Fixa = 0,            
            Rolagem_Continua_Esquerda = 1,           
            Rolagem_Paginada_Esquerda = 2,            
            Rolagem_Continua2_Esquerda = 3,            
            Rolagem_Cima = 4,           
            Rolagem_Baixo = 5,            
            Surgimento_Fora_Horizontal = 6,            
            Surgimento_Fora_Vertical = 7,            
            Surgimento_Fora_Ambos = 8,            
            Surgimento_Dentro_Horizontal = 9,           
            Surgimento_Dentro_Vertical = 10,            
            Surgimento_Dentro_Ambos = 11,            
            Rolagem_Continua3_Esquerda = 12
        }
       
        public unsafe struct OpcoesApresentacao
        {
            public Byte animacao;  //0 = direita, 1 esquerda, 2 centralizado
            public Byte alinhamento;
            public fixed Byte reservado[2];
            public UInt32 intervaloAnimacao;
            public UInt32 tempoApresentacao;
        }
        public static string SubstituirString(String textoOriginal, String oldValue, String newValue, int qtdOcorrencias)
        {
            string resposta = textoOriginal;
            var regex = new Regex(Regex.Escape(oldValue));
            resposta = regex.Replace(resposta, newValue, qtdOcorrencias);

            return resposta;
        }
        //Métodos acessores para acesso a UltimaExtensao;
        public static String GetUltimaExtensao()
        {
            return "." + _ultimaExtensao.ToString().ToLower();
        }

        public static void SetUltimaExtensao(TipoVideo _tv = TipoVideo.V01)
        {
            _ultimaExtensao = _tv;
        }


        /// <summary>
        /// Zera os valores dos sequenciais para serem utilizados numa nova geração de arquivos.
        /// </summary>
        public static void LimpaSequenciais()
        {
            sequencial_arquivo_videos = 0;
            sequencial_arquivo_roteiros = 0;
            sequencial_arquivo_mensagens = 0;
            sequencial_arquivo_motoristas = 0;

        }

        /// <summary>
        /// Retorna um tipo vídeo a partir da extensão.
        /// </summary>
        /// <param name="_tv">String contendo a extensão do arquivo de vídeo.</param>
        /// <returns></returns>
        public static TipoVideo RetornaTipoVideo(String _tv)
        {
            switch (_tv)
            {
                case ".v01": return TipoVideo.V01;
                case ".v02": return TipoVideo.V02;
                case ".v03": return TipoVideo.V03;
                default: return TipoVideo.V01;
            }

        }

        public static string RemoveSpecialCharacters(string text, string replacement)
        {
            //string pattern = @"(?i)[^0-9a-záéíóúàèìòùâêîôûãõç\s]";
            string pattern = "[^0-9a-zA-Z-]+";
            Regex rgx = new Regex(pattern);
            return rgx.Replace(text, replacement);
        }

        /// <summary>
        /// Cria o diretório usado para uso do aplicativo.
        /// </summary>
        public static void CriaDiretorioAppData()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LDX12\"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LDX12\");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LDX12\regioes");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LDX12\fontes");
            }

        }



        /// <summary>
        /// Retorna o diretorio base de uma string contendo nome de arquivo.
        /// </summary>
        /// <param name="ArquivoNome">Nome completo do arquivo.</param>
        /// <returns>String contendo diretorio do arquivo.</returns>
        public static String RetornaDiretorio(String ArquivoNome)
        {
            int indice_ultima_barra = -1;

            indice_ultima_barra = ArquivoNome.LastIndexOf(ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS);

            return ArquivoNome.Substring(0, indice_ultima_barra + 1);
        }

        /// <summary>
        /// Retorna o nome do arquivo sem o diretorio.
        /// </summary>
        /// <param name="ArquivoNome">Nome completo do arquivo.</param>
        /// <returns>String contendo o nome do arquivo.</returns>
        public static String RetornaNomeArquivo(String ArquivoNome)
        {
            int indice_ultima_barra = -1;

            indice_ultima_barra = ArquivoNome.LastIndexOf(ARQUIVO_SEPARADOR_DIRETORIO_WINDOWS);

            return ArquivoNome.Substring(indice_ultima_barra + 1);
        }


        

        /// <summary>
        /// Retorna o valor da rolagem associado a um byte.
        /// </summary>
        /// <param name="rolagem">Byte representativo da rolagem.</param>
        /// <returns>Retorna o byte como valor de Util.Rolagem.</returns>
        public static Util.Rolagem RetornaRolagem(Byte rolagem)
        {
            switch (rolagem)
            {
               /* case 0:
                    return Util.Rolagem.Fixa;
                case 1:
                    return Util.Rolagem.Contínua;
                case 2:
                    return Util.Rolagem.Paginada;
                case 3:
                    return Util.Rolagem.Contínua2;
                case 4:
                    return Util.Rolagem.Subindo;
                case 5:
                    return Util.Rolagem.Descendo;
                default:
                    return Util.Rolagem.Fixa; */

                case 0:
                    return Util.Rolagem.Fixa;
                case 1:
                    return Util.Rolagem.Rolagem_Continua_Esquerda;
                case 2:
                    return Util.Rolagem.Rolagem_Paginada_Esquerda;
                case 3:
                    return Util.Rolagem.Rolagem_Continua2_Esquerda;
                case 4:
                    return Util.Rolagem.Rolagem_Cima;
                case 5:
                    return Util.Rolagem.Rolagem_Baixo;
                case 6:
                    return Util.Rolagem.Surgimento_Fora_Horizontal;
                case 7:
                    return Util.Rolagem.Surgimento_Fora_Vertical;
                case 8:
                    return Util.Rolagem.Surgimento_Fora_Ambos;
                case 9:
                    return Util.Rolagem.Surgimento_Dentro_Horizontal;
                case 10:
                    return Util.Rolagem.Surgimento_Dentro_Vertical;
                case 11:
                    return Util.Rolagem.Surgimento_Dentro_Ambos;
                case 12:
                    return Util.Rolagem.Rolagem_Continua3_Esquerda;
                default:
                    return Util.Rolagem.Fixa;
            }
        }

        /// <summary>
        /// Recebe a formatação coletada do arquivo excel. caso inválida, retorna centralizado.
        /// </summary>
        /// <param name="texto">valor coletado na posição do alinhamento na string do excel.</param>
        /// <returns>valor do alinhamento.</returns>
        public static byte RetornaAlinhamentoHorizontal(String alinhamento)
        {
            byte Result = 0;

            try
            {
                Result = System.Convert.ToByte(alinhamento);
            }
            catch (Exception)
            {
                Result = (byte)Util.AlinhamentoHorizontal.Centralizado;
            }

            return Result;
        }
        public static AlinhamentoHorizontal RetornaAlinhamentoHorizontalLD6(String alinhamento)
        {
            AlinhamentoHorizontal Result = AlinhamentoHorizontal.Centralizado;

            switch (alinhamento)
            {
                case "Esquerda":
                    Result = Util.AlinhamentoHorizontal.Esquerda;
                    break;
                case "Centralizado":
                    Result = AlinhamentoHorizontal.Centralizado;
                    break;
                case "Direita":
                    Result = AlinhamentoHorizontal.Direita;                        
                    break;
            }

            return Result;
        }
        public static AlinhamentoVertical RetornaAlinhamentoVerticalLD6(String alinhamento)
        {
            AlinhamentoVertical Result = AlinhamentoVertical.Centro;


            switch (alinhamento)
            {
                case "Abaixo":
                    Result = Util.AlinhamentoVertical.Baixo;
                    break;
                case "Centralizado":
                    Result = AlinhamentoVertical.Centro;
                    break;
                case "Acima":
                    Result = AlinhamentoVertical.Cima;
                    break;
            }

            return Result;
        }
        public static Rolagem RetornaRolagemLD6(String rolagem)
        {
            Rolagem Result = Rolagem.Fixa;

            switch (rolagem)
            {
                case "Nenhuma":
                    Result = Util.Rolagem.Fixa;
                    break;
                case "Continua":
                    Result = Rolagem.Rolagem_Continua_Esquerda;
                    break;
                case "Paginada":
                    Result = Rolagem.Rolagem_Paginada_Esquerda;
                    break;
                case "Continua2":
                    Result = Util.Rolagem.Rolagem_Continua2_Esquerda;
                    break;
                case "Continua3":
                    Result = Rolagem.Rolagem_Continua3_Esquerda;
                    break;                
            }
           
            return Result;
        }
        /// <summary>
        /// Retorna nomes dos arquivos fonte disponíveis no diretório especificado.
        /// </summary>
        /// <param name="diretorio">diretorio a ser verificado.</param>
        /// <returns>Lista com o nome completo do arquivo fonte.</returns>
        public static List<String> RetornaNomesArquivosFonteDisponiveis(String diretorio)
        {
            return null;
        }


        /// <summary>
        /// Recebe a formatação coletada do arquivo excel. caso inválida, retorna centralizado.
        /// </summary>
        /// <param name="alinhamentoExcel">valor coletado na posição do alinhamento na string do excel.</param>
        /// <returns>valor do alinhamento.</returns>
        public static byte RetornaAlinhamentoVertical(String alinhamentoExcel)
        {
            byte Result = 0;

            try
            {
                Result = System.Convert.ToByte(alinhamentoExcel);
            }
            catch (Exception)
            {
                Result = (byte)Util.AlinhamentoVertical.Centro;
            }

            return Result;
        }

        /// <summary>
        /// Verifica se o usuario quer criar o texto num arquivo v02
        /// </summary>
        /// <param name="texto">String contendo um texto, caso comece com "v02" retorna TRUE. FALSE caso contrário.</param>
        /// <returns></returns>
        public static bool VerificaV02(String texto)
        {
            if (texto.Length >= 4)
                if (texto.Substring(0, 4).Equals(".v02")) return true;

            return false;
        }

        /// <summary>
        /// Inverte as barras e remove os \0 para ser usado como diretorio de acesso aos arquivos de v01.
        /// </summary>
        /// <param name="diretorio">diretorio a ser tratado.</param>
        /// <returns></returns>
        public static String TrataDiretorio(String diretorio)
        {
            try
            {
                Int32 indice_limite = diretorio.IndexOfAny("\0".ToCharArray());

                if (indice_limite != -1)
                {
                    string temp = diretorio.Substring(0, indice_limite);
                    
                    return temp.Replace(@"\", @"/");
                }

                return diretorio.Replace(@"\", @"/");
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }



        /// <summary>
        /// Prepara um diretório de firmware para ser usado em ambiente windows.
        /// </summary>
        /// <param name="diretorio">diretorio a ser tratado.</param>
        /// <returns></returns>
        public static String TrataDiretorioFWParaWindows(String diretorio)
        {
            try
            {
                Int32 indice_limite = diretorio.IndexOfAny("\0".ToCharArray());

                if (indice_limite != -1)
                {
                    string temp = diretorio.Substring(0, indice_limite);

                    return temp.Replace(@"/", @"\");
                }

                return diretorio.Replace(@"/", @"\");
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        //Verifica caracter HINDI
        public static bool VerificaCaracterHindi(string texto)
        {
            bool hindi = false;

            foreach (char _char in texto)
            {
                if (Convert.ToInt32(_char) > 255)
                {
                    hindi = true;
                    break;
                }
            }
            return hindi;
        }


        /// <summary>
        /// Verifica se é uma hora válida
        /// </summary>
        /// <param name="hora"> Byte que representa a hora</param>
        /// <returns></returns>
        public static bool ValidarHora(byte hora)
        {
            return ((hora >= 0) & (hora < 24));
        }
        /// <summary>
        /// Verifica se é um minuto válido
        /// </summary>
        /// <param name="minuto"> Byte que representa o minuto</param>
        /// <returns></returns>
        public static bool ValidarMinuto(byte minuto)
        {
            return ((minuto >= 0) & (minuto < 60));
        }

        /// <summary>
        /// PadRight array de bytes.
        /// </summary>
        /// <param name="array">array a ser padeado com zeros.</param>
        /// <returns> array padeado com tamanho especificado.</returns>
        public static Byte[] PadRight(Byte[] array, int length)
        {
            Byte[] temp = new byte[length];

            for (int a = 0; a < array.Count(); a++)
                temp[a] = array[a];

            for (int i = array.Count() - 1; i < temp.Count(); i++)
            {
                temp[i] = 0;
            }

            return temp;
        }

        // O parametro deve vir com 2 bytes de CRC
        public static Byte[] CalcularCRC2(Byte[] dados)
        {
            byte[] dadosSemCRC = new byte[dados.Length - 2];
            byte[] dadosTemp;

            dadosSemCRC = GetBytesTemp(dados, CRCPosition1, CRCPosition2);
            dadosTemp = UpdateCRCValue(dados, dadosSemCRC, CRCPosition1, CRCPosition2);

            return dadosTemp;
        }


        public static bool VerificarStringVazio(byte[] texto)
        {
            for (int i = 0; i < texto.Length; i++)
            {
                if (texto[i] != 0 & texto[i] != 32)
                {
                    return false;
                }
            }
            return true;
        }


        public static Byte[] GetBytesTemp(Byte[] dados, int CRCPosition1, int CRCPosition2)
        {
            Byte[] dadosTemp = new byte[dados.Length - 2];

            for (int i = 0, j = 0; j < dados.Length; j++)
            {
                if (j == CRCPosition1 || j == CRCPosition2)
                {
                    continue;
                }

                dadosTemp[i] = dados[j];

                i++;
            }

            return dadosTemp;
        }

        public static Byte[] UpdateCRCValue(Byte[] dados, Byte[] dadosTemp, int CRCPosition1, int CRCPosition2)
        {
            ushort crc = CRC16CCITT.Calcular(dadosTemp);

            Byte[] dadosCRC = BitConverter.GetBytes(crc);

            dados[CRCPosition1] = dadosCRC[0];
            dados[CRCPosition2] = dadosCRC[1];

            return dados;
        }

        public static bool releaseComObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
                return true;
            }
            catch (Exception ex)
            {
                obj = null;                
            }
            finally
            {
                GC.Collect();
            }

            return false;
        }


        public static bool ValidarPath(string diretorioPrincipal, byte[] file)
        {
            string caminho = diretorioPrincipal + Encoding.ASCII.GetString(file).Replace('/', '\\').Replace("\0", string.Empty);
            // Verifica se o tamanho do nome do arquivo é compatível com o firmware
            if ((String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(caminho))) || (Path.GetFileNameWithoutExtension(caminho).Length > 8))
                return false;
            return System.IO.File.Exists(caminho.Replace("\0", string.Empty));
        }
        public static bool ValidarPath(string caminho)
        {
            string caminhoTratado = caminho.Replace('/', '\\').Replace("\0", string.Empty);
            // Verifica se o tamanho do nome do arquivo é compatível com o firmware
            if ((String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(caminhoTratado))) || (Path.GetFileNameWithoutExtension(caminhoTratado).Length > 8))
                return false;
            return System.IO.File.Exists(caminhoTratado.Replace("\0", string.Empty));
        }
        public static void CopiarArquivos(String diretorio_raiz, String diretorio_destino, String extensao)
        {
            List<String> arquivos = new List<string>();

            // André -> 10-12-2014
            if (!Directory.Exists(diretorio_destino))
                Directory.CreateDirectory(diretorio_destino);

            arquivos.AddRange(Directory.EnumerateFiles(diretorio_raiz, "*" + extensao));

            foreach (string s in arquivos)
            {
                if (Path.GetFileNameWithoutExtension(s).Length > 8)
                {
                    System.IO.File.Copy(s, diretorio_destino + @"\" + Path.GetFileNameWithoutExtension(s).Substring(0, 8) + Path.GetExtension(s), true);
                }
                else
                {
                    System.IO.File.Copy(s, diretorio_destino + @"\" + Path.GetFileNameWithoutExtension(s) + Path.GetExtension(s), true);
                }
            }

            //GerarArquivosRegiaoLST(diretorio_raiz, diretorio_destino);
            //File.Copy(diretorio_raiz + @"\regioes.lst", diretorio_destino + '\\' + Path.GetFileName(@"regioes.lst"), true);  
        }
        /// <summary>
        /// Método utilizado para gerar um array com os arquivos recentes determinados pelo filtro
        /// </summary>
        /// <param name="filtro"> O filtro deve ser a extensão do arquivo + extensão do atalho (lnk) conforme o exemplo. </param>
        /// <returns> Conjunto dos arquivos recentes utilizados </returns>
        /// <remarks> Exemplo: GetArquivosRecentes("*.ldx.lnk"); </remarks>
        public static String[] GetArquivosRecentes()
        {
            List<String> retorno = new List<String>();
            string caminhoOrigem = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
             string[] arquivos = Directory.GetFiles(caminhoOrigem, "*.lnk", SearchOption.TopDirectoryOnly).Where(x => new FileInfo(x).LastWriteTime.Date >= (DateTime.Today.Date.AddDays(-30))).ToArray();
            //string[] arquivos = Directory.GetFiles(caminhoOrigem, ASTERISCO + ARQUIVO_EXT_ATALHO, SearchOption.TopDirectoryOnly);
 
            foreach (string fi in arquivos)
            {
                string arquivo = ExtrairCaminhoReal(fi);
                if (arquivo.Contains(ARQUIVO_EXT_LDX2_MAIUSCULO)||arquivo.Contains(ARQUIVO_EXT_LDX2_MINUSCULO))
                    if (System.IO.File.Exists(arquivo))
                        retorno.Add(arquivo);
            }

            return retorno.ToArray();
        }

        public static bool ExisteNinbusInstalado(out string caminho)
        {
            bool isNinbusInstalled = false;
            caminho = string.Empty;

            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        try
                        {
                            string displayName = (subkey.GetValue("DisplayName")).ToString();
                            if (displayName.Equals(NOME_APP_NINBUS))
                            {
                                isNinbusInstalled = true;
                                caminho = subkey.GetValue("InstallLocation").ToString();
                                break;
                            }
                        }
                        catch
                        { }
                    }
                }
            }

            return isNinbusInstalled;
        }

        public static bool ExisteAPPInstalado(out string caminho)
        {
            bool isAPPInstalled = false;
            caminho = string.Empty;

            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        try
                        {
                            string displayName = (subkey.GetValue("DisplayName")).ToString();
                            if (displayName.Equals(NOME_APP_APP))
                            {
                                isAPPInstalled = true;
                                caminho = subkey.GetValue("InstallLocation").ToString();
                                break;
                            }
                        }
                        catch
                        { }
                    }
                }
            }

            return isAPPInstalled;
        }


        public static string ExtrairCaminhoReal(String atalho)
        {
            if (System.IO.File.Exists(atalho))
            {
                WshShell shell = new WshShell(); //Create a new WshShell Interface
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(atalho); //Link the interface to our shortcut

                return link.TargetPath;
            }
            else
            {
                return String.Empty;
            }
        }

        #region Registro de LOGs

        public static String TrackerLog = "tracker.log";
                   
        public static readonly Boolean registrarLog = true; /*Para o teste de campo do Pontos*/

        public static Queue<String> filaEventos = new Queue<string>();

        public static void escreverEvento(String evento)
        {
            lock (filaEventos)
            {
                filaEventos.Enqueue(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " | " + evento);
            }
        }

        public static Thread threadGerarLog = new Thread(EscreverEventosArquivo);
        
        // Quantidade de funções pré-definidas pelo FIRMWARE do controlador
        public const int QUANTIDADE_DE_FUNCOES_BLOQUEAVEIS = 21;

        public static void EscreverEventosArquivo()
        {
            string evento = string.Empty;
            while (true)
            {
                lock (filaEventos)
                {
                    while (filaEventos.Count > 0)
                    {
                        evento = filaEventos.Dequeue();
                        if (registrarLog)
                        {
                            TextWriter mem = new StreamWriter(Util.TrackerLog, true, Encoding.Default);
                            try
                            {
                                mem.WriteLine(evento);
                            }
                            catch
                            {
                            }
                            finally
                            {
                                mem.Close();
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
        #endregion Registro de LOGs  
    }
}
