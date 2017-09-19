using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Util;
using System.Resources;
using Persistencia.Erros;


namespace Persistencia
{    
    public class Arquivo_LPK_V02 // : IArquivo
    {
        private const int NSS_LPK_SIZE = 192; // Tamanho do arquivo de Idiomas do NSS (APP)
        private const int QTD_FRASES = 104;
        private const int TAM_MAX_FRASE = 16;

        public unsafe struct FormatoIdioma
        {
            public Byte versao;
            public fixed Byte reservado[3];
            public fixed Byte nome[TAM_MAX_FRASE];
            public fixed Byte reservado2[42];
            public UInt16 crc;
            public fixed Byte texto[TAM_MAX_FRASE * QTD_FRASES];
        }


        public enum ErroArquivo
        {
            SUCESSO,
            ERRO_CRC,
            ERRO_TAMANHO_ARQUIVO
        }

        //public enum TextoID
        //{
        //    RECONHECENDO_USB = 0,
        //    DISPOSITIVO_INCOMPATIVEL = 2,
        //    FUNCAO_BLOQUEADA = 4,
        //    AGUARDE = 6,
        //    COLOQUE_PENDRIVE = 7,
        //    VERSAO = 9,
        //    ROTEIROS,
        //    MENSAGENS,
        //    NUMERO_SERIE,
        //    PAINEL,
        //    AJUSTAR_PARTIDA,
        //    SENTIDO,
        //    IDA,
        //    VOLTA,
        //    IDENTIFICANDO,
        //    FALHA,
        //    SUCESSO,
        //    NOVA_CONFIG = SUCESSO + 2,
        //    AJUSTAR_RELOGIO,
        //    SELEC_REGIAO,
        //    CONFIGURACOES,
        //    IDENTIF_PAINEIS,
        //    COLETAR_DUMP,
        //    FORMAT_PENDRIVE,
        //    DOM_SEG_TER_QUA_QUI,
        //    SEX_SAB,
        //    MENSAGEM,
        //    SELECIONA_PAINEL,
        //    ROTEIRO,
        //    AUTOMATICO,
        //    PAINEIS = AUTOMATICO + 2,
        //    DESEJA_FORMATAR,
        //    ATUALIZANDO = DESEJA_FORMATAR + 2,
        //    REMOVA_PENDRIVE,
        //    RESETANDO,
        //    CARREGUE_CONFIG,
        //    APAGAR_ARQUIVOS = CARREGUE_CONFIG + 2,
        //    SINCRONIZANDO,
        //    PAINEL_NAO_DETECTADO,
        //    OPCOES_AVANCADAS = PAINEL_NAO_DETECTADO + 2,
        //    COLETANDO,
        //    COLETAR_LOG,
        //    PAINEL_COM_DEFEITO,
        //    SIM_NAO = PAINEL_COM_DEFEITO + 2,
        //    DESEJA_DESBLOQUEAR_FUNCAO,
        //    SENHA = DESEJA_DESBLOQUEAR_FUNCAO + 2,
        //    SENHA_INCORRETA,
        //    CONTROLADOR = SENHA_INCORRETA + 2,
        //    TODOS_DISPOSITIVOS,
        //    TODOS_PAINEIS,
        //    SISTEMA_ARQUIVO_DEFEITUOSO,
        //    ACENDER_PAINEIS = SISTEMA_ARQUIVO_DEFEITUOSO + 2,
        //    SENHA_ANTI_FURTO_INCORRETA
        //}
        public int versao;
        public List<string> texto;
        public string nome;
        public UInt16 crc;
        public ResourceManager rm;


        public Arquivo_LPK_V02(ResourceManager rm) 
        {
            this.versao = 2;
            nome = Util.Util.NOME_IDIOMA;
            texto = new List<string>();
            this.rm = rm;
            texto.AddRange(CarregarFrasesDefault());            
        }

        public string[] CarregarFrasesDefault()
        {
            return CarregarFrases(this.rm);
            //switch (idioma)
            //{
            //    case Util.Util.Lingua.Portugues: return CarregarFrasesDefaultPortugues();                    
                    
            //    case Util.Util.Lingua.Espanhol: return CarregarFrasesDefaultEspanhol();

            //    case Util.Util.Lingua.Ingles: return CarregarFrasesDefaultIngles();
                      
            //    case Util.Util.Lingua.Frances: return CarregarFrasesDefaultFrances();

            //default: return CarregarFrasesDefaultPortugues();                    
            //}
        }

        private string[] CarregarFrasesDefaultIngles()
        {
            List<string> textoLocal = new List<string>();
            textoLocal.Add("   DETECTING    ");
            textoLocal.Add("   USB DEVICE   ");
            textoLocal.Add("  IMCOMPATIBLE  ");
            textoLocal.Add("     DEVICE     ");
            textoLocal.Add("    BLOCKED     ");
            textoLocal.Add("    FUNCTION    ");
            textoLocal.Add("WAIT            ");
            textoLocal.Add("     INSERT     ");
            textoLocal.Add("   FLASH DRIVE  ");
            textoLocal.Add("VERSION         ");
            textoLocal.Add("ROUTE           ");
            textoLocal.Add("MESSAGES        ");
            textoLocal.Add("SERIAL NUMBER   ");
            textoLocal.Add("DISPLAY         ");
            textoLocal.Add("ADJUST DEP. TIME");
            textoLocal.Add("DIRECTION       ");
            textoLocal.Add("GOING           ");
            textoLocal.Add("COMING          ");
            textoLocal.Add("IDENTIFYING     ");
            textoLocal.Add("      FAIL      ");
            textoLocal.Add("     SUCCESS    ");
            textoLocal.Add("                ");
            textoLocal.Add("NEW CONFIG.     ");
            textoLocal.Add("ADJUST CLOCK    ");
            textoLocal.Add("SELECT REGION   ");
            textoLocal.Add("CONFIG. SUMMARY ");
            textoLocal.Add("PAIR            ");
            textoLocal.Add("COLECT DUMP     ");
            textoLocal.Add("FORMAT FLASH DRV");
            textoLocal.Add("SUNMONTUEWEDTHU ");
            textoLocal.Add("FRISAT          ");
            textoLocal.Add("MESSAGE         ");
            textoLocal.Add("SELECT DISPLAY  ");
            textoLocal.Add("ROUTE           ");
            textoLocal.Add("Automatic?      ");
            textoLocal.Add("                ");
            textoLocal.Add("DISPLAYS        ");
            textoLocal.Add("Are you sure to ");
            textoLocal.Add("format?         ");
            textoLocal.Add("UPDATING        ");
            textoLocal.Add("REMOVE DEVICE   ");
            textoLocal.Add("    RESETING    ");
            textoLocal.Add("  Load a new    ");
            textoLocal.Add("configuration.  ");
            textoLocal.Add("ERASE CONFIG.   ");
            textoLocal.Add("SYNCHRONIZING   ");
            textoLocal.Add("Display xx      ");
            textoLocal.Add("wasn't detected ");
            textoLocal.Add("ADVANCED OPTIONS");
            textoLocal.Add("COLETING        ");
            textoLocal.Add("COLECT LOG     ");
            textoLocal.Add("Malfunction of  ");
            textoLocal.Add("Display xx      ");
            textoLocal.Add("YES     NO      ");
            textoLocal.Add("Do you want to  ");
            textoLocal.Add("unlock function?");
            textoLocal.Add("PASSWORD        ");
            textoLocal.Add("ACCESS DENIED   ");
            textoLocal.Add("                ");
            textoLocal.Add("CONTROLLER      ");
            textoLocal.Add("ALL DEVICES     ");
            textoLocal.Add("ALL DISPLAYS    ");
            textoLocal.Add("MALFUNCTION     ");
            textoLocal.Add("FILE SYSTEM     ");
            textoLocal.Add("TURN DISPLAYS ON");
            textoLocal.Add("CONFIG. CAN'T BE");
            textoLocal.Add("LOADED          ");
            //////////////////////////
            textoLocal.Add("DEP.:".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("SUN".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MON".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("TUE".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("WED".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("THU".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("FRI".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("SAT".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("FACTORY SETTING ");
            textoLocal.Add("COMMUNICATION   ");
            textoLocal.Add("FAIL            ");
            textoLocal.Add("NEW CONFIG.     ");
            textoLocal.Add("INVALID         ");
            textoLocal.Add("PAIR            ");
            textoLocal.Add("INCOMPATIBLE    ");
            textoLocal.Add("TIME ON         ");
            textoLocal.Add("TIME OFF        ");
            textoLocal.Add("INDEFINITELY    ");
            textoLocal.Add("NSS NOT         ");
            textoLocal.Add("DETECTED        ");
            /* Adição de mais 13 frases incluindo os modos de teste */
            textoLocal.Add("File not found".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("SecondaryMessage".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Main Message ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Rename folder:".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Automatic ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Rows ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Columns ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("All Leds Off ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("All Leds On ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Text".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Turn test off".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("TEST MODE".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add(" NSS ".PadRight(TAM_MAX_FRASE, '\0'));

            // NENHUM_ARQUIVO, MENSAGEM_SECUNDARIA, MENSAGEM_PRINCIPAL, NOMEIE_PASTA, TESTE_AUTOMATICO, LINHAS, COLUNAS, APAGADO, ACESO, TEXTO, DESLIGAR_TESTE, MODO_TESTE, APP
            return textoLocal.ToArray();
        }
        private string[] CarregarFrasesDefaultPortugues()
        {
            List<string>  textoLocal = new List<string>();
            textoLocal.Add("  RECONHECENDO  ");
            textoLocal.Add("DISPOSITIVO USB ");
            textoLocal.Add("   DISPOSITIVO  ");
            textoLocal.Add("  IMCOMPATÍVEL  ");
            textoLocal.Add("     FUNÇÃO     ");
            textoLocal.Add("   BLOQUEADA    ");
            textoLocal.Add("AGUARDE         ");
            textoLocal.Add("    COLOQUE O   ");
            textoLocal.Add("    PENDRIVE    ");
            textoLocal.Add("VERSÃO          ");
            textoLocal.Add("ROTEIROS        ");
            textoLocal.Add("MENSAGENS       ");
            textoLocal.Add("NUMERO DE SÉRIE ");
            textoLocal.Add("PAINEL          ");
            textoLocal.Add("AJUSTAR PARTIDA ");
            textoLocal.Add("SENTIDO         ");
            textoLocal.Add("IDA             ");
            textoLocal.Add("VOLTA           ");
            textoLocal.Add("IDENTIFICANDO   ");
            textoLocal.Add("      FALHA     ");
            textoLocal.Add("     SUCESSO    ");
            textoLocal.Add("                ");
            textoLocal.Add("NOVA CONFIG.    ");
            textoLocal.Add("AJUSTAR RELOGIO ");
            textoLocal.Add("SELEC. REGIÃO   ");
            textoLocal.Add("CONFIGURAÇÕES   ");
            textoLocal.Add("EMPARELHAR      ");
            textoLocal.Add("COLETAR DUMP    ");
            textoLocal.Add("FORMAT. PENDRIVE");
            textoLocal.Add("DOMSEGTERQUAQUI ");
            textoLocal.Add("SEXSAB          ");
            textoLocal.Add("MENSAGEM        ");
            textoLocal.Add("SELECIONA PAINEL");
            textoLocal.Add("ROTEIRO         ");
            textoLocal.Add("Automático?     ");
            textoLocal.Add("                ");
            textoLocal.Add("PAINEIS         ");
            textoLocal.Add("Tem certeza em  ");
            textoLocal.Add("formatar?       ");
            textoLocal.Add("ATUALIZANDO     ");
            textoLocal.Add("REMOVA PENDRIVE ");
            textoLocal.Add("    RESETANDO   ");
            textoLocal.Add("  Carregue uma  ");
            textoLocal.Add("  nova config.  ");
            textoLocal.Add("APAGAR ARQUIVOS ");
            textoLocal.Add("SINCRONIZANDO   ");
            textoLocal.Add("Painel xx não   ");
            textoLocal.Add("foi detectado   ");
            textoLocal.Add("OPÇÕES AVANÇADAS");
            textoLocal.Add("COLETANDO       ");
            textoLocal.Add("COLETAR LOG     ");
            textoLocal.Add("Painel xx está  ");
            textoLocal.Add("com defeito     ");
            textoLocal.Add("SIM     NÃO     ");
            textoLocal.Add("Desbloquear a   ");
            textoLocal.Add("função?         ");
            textoLocal.Add("SENHA           ");
            textoLocal.Add("SENHA INCORRETA ");
            textoLocal.Add("                ");
            textoLocal.Add("CONTROLADOR     ");
            textoLocal.Add("TODOS DISP.     ");
            textoLocal.Add("TODOS PAINEIS   ");
            textoLocal.Add("SISTEMA ARQUIVO ");
            textoLocal.Add("DEFEITUOSO      ");
            textoLocal.Add("ACENDER PAINEIS ");
            textoLocal.Add("CONFIG. NÃO PODE");
            textoLocal.Add("SER CARREGADA   ");
            textoLocal.Add("SAIDA:".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("DOM".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("SEG".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("TER".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("QUA".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("QUI".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("SEX".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("SAB".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("CONFIG. FÁBRICA ");
            textoLocal.Add("FALHA DE        ");
            textoLocal.Add("COMUNICAÇÃO     ");
            textoLocal.Add("NOVA CONFIG.    ");
            textoLocal.Add("INVÁLIDA        ");
            textoLocal.Add("EMPARELHAMENTO  ");
            textoLocal.Add("INCOMPATÍVEL    ");
            textoLocal.Add("TEMPO ACESO     ");
            textoLocal.Add("TEMPO APAGADO   ");
            textoLocal.Add("INDEFINIDAMENTE ");
            textoLocal.Add("NSS não foi     ");
            textoLocal.Add("detectado       ");
            /* Adição de mais 13 frases incluindo os modos de teste */
            textoLocal.Add("Nenhum arquivo".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MSG SECUNDÁRIA ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MSG PRINCIPAL".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Nomeie a pasta: ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Automático ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Linhas ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Colunas ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Apagado ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Aceso ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Texto".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Desligar Teste".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Modo Teste".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add(" APP ".PadRight(TAM_MAX_FRASE, '\0'));


            return textoLocal.ToArray();
        }
        private string[] CarregarFrasesDefaultFrances()
        {
            List<string> textoLocal = new List<string>();
            textoLocal.Add("  RECONHECENDO  ");
            textoLocal.Add("DISPOSITIVO USB ");
            textoLocal.Add("   DISPOSITIVO  ");
            textoLocal.Add("  IMCOMPATÍVEL  ");
            textoLocal.Add("     FUNÇÃO     ");
            textoLocal.Add("   BLOQUEADA    ");
            textoLocal.Add("AGUARDE         ");
            textoLocal.Add("    COLOQUE O   ");
            textoLocal.Add("    PENDRIVE    ");
            textoLocal.Add("VERSÃO          ");
            textoLocal.Add("ROTEIROS        ");
            textoLocal.Add("MENSAGENS       ");
            textoLocal.Add("NUMERO DE SÉRIE ");
            textoLocal.Add("PAINEL          ");
            textoLocal.Add("AJUSTAR PARTIDA ");
            textoLocal.Add("SENTIDO         ");
            textoLocal.Add("IDA             ");
            textoLocal.Add("VOLTA           ");
            textoLocal.Add("IDENTIFICANDO   ");
            textoLocal.Add("      FALHA     ");
            textoLocal.Add("     SUCESSO    ");
            textoLocal.Add("                ");
            textoLocal.Add("NOVA CONFIG.    ");
            textoLocal.Add("AJUSTAR RELOGIO ");
            textoLocal.Add("SELEC. REGIÃO   ");
            textoLocal.Add("CONFIGURAÇÕES   ");
            textoLocal.Add("EMPARELHAR      ");
            textoLocal.Add("COLETAR DUMP    ");
            textoLocal.Add("FORMAT. PENDRIVE");
            textoLocal.Add("DOMSEGTERQUAQUI ");
            textoLocal.Add("SEXSAB          ");
            textoLocal.Add("MENSAGEM        ");
            textoLocal.Add("SELECIONA PAINEL");
            textoLocal.Add("ROTEIRO         ");
            textoLocal.Add("Automático?     ");
            textoLocal.Add("                ");
            textoLocal.Add("PAINEIS         ");
            textoLocal.Add("Tem certeza em  ");
            textoLocal.Add("formatar?       ");
            textoLocal.Add("ATUALIZANDO     ");
            textoLocal.Add("REMOVA PENDRIVE ");
            textoLocal.Add("    RESETANDO   ");
            textoLocal.Add("  Carregue uma  ");
            textoLocal.Add("  nova config.  ");
            textoLocal.Add("APAGAR ARQUIVOS ");
            textoLocal.Add("SINCRONIZANDO   ");
            textoLocal.Add("Painel xx não   ");
            textoLocal.Add("foi detectado   ");
            textoLocal.Add("OPÇÕES AVANÇADAS");
            textoLocal.Add("COLETANDO       ");
            textoLocal.Add("COLETAR LOG     ");
            textoLocal.Add("Painel xx está  ");
            textoLocal.Add("com defeito     ");
            textoLocal.Add("SIM     NÃO     ");
            textoLocal.Add("Desbloquear a   ");
            textoLocal.Add("função?         ");
            textoLocal.Add("SENHA           ");
            textoLocal.Add("SENHA INCORRETA ");
            textoLocal.Add("                ");
            textoLocal.Add("CONTROLADOR     ");
            textoLocal.Add("TODOS DISP.     ");
            textoLocal.Add("TODOS PAINEIS   ");
            textoLocal.Add("SISTEMA ARQUIVO ");
            textoLocal.Add("DEFEITUOSO      ");
            textoLocal.Add("ACENDER PAINEIS ");
            textoLocal.Add("CONFIG. NÃO PODE");
            textoLocal.Add("SER CARREGADA   ");
            textoLocal.Add("SAIDA:".PadRight(16, '\0'));
            textoLocal.Add("DOM".PadRight(16, '\0'));
            textoLocal.Add("SEG".PadRight(16, '\0'));
            textoLocal.Add("TER".PadRight(16, '\0'));
            textoLocal.Add("QUA".PadRight(16, '\0'));
            textoLocal.Add("QUI".PadRight(16, '\0'));
            textoLocal.Add("SEX".PadRight(16, '\0'));
            textoLocal.Add("SAB".PadRight(16, '\0'));
            textoLocal.Add("CONFIG. FÁBRICA ");
            textoLocal.Add("FALHA DE        ");
            textoLocal.Add("COMUNICAÇÃO     ");
            textoLocal.Add("NOVA CONFIG.    ");
            textoLocal.Add("INVÁLIDA        ");
            textoLocal.Add("EMPARELHAMENTO  ");
            textoLocal.Add("INCOMPATÍVEL    ");
            textoLocal.Add("TEMPO ACESO     ");
            textoLocal.Add("TEMPO APAGADO   ");
            textoLocal.Add("INDEFINIDAMENTE ");
            textoLocal.Add("NSS não foi     ");
            textoLocal.Add("detectado       ");
            /* Adição de mais 13 frases incluindo os modos de teste */
            textoLocal.Add("Nenhum arquivo".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MSG SECUNDÁRIA ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MSG PRINCIPAL".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Nomeie a pasta: ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Automático ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Linhas ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Colunas ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Apagado ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Aceso ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Texto".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Desligar Teste".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("Modo Teste".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add(" APP ".PadRight(TAM_MAX_FRASE, '\0'));

            return textoLocal.ToArray();
        }
        private string[] CarregarFrasesDefaultEspanhol()
        {
            List<string> textoLocal = new List<string>();
            textoLocal.Add("  RECONOCIENDO  ");
            textoLocal.Add("DISPOSITIVO USB ");
            textoLocal.Add("   DISPOSITIVO  ");
            textoLocal.Add("  INCOMPATIBLE  ");
            textoLocal.Add("    FUNCIÓN     ");
            textoLocal.Add("   BLOQUEADO    ");
            textoLocal.Add("ESPERE          ");
            textoLocal.Add("    INSERTA     ");
            textoLocal.Add("DISPOSITIVO USB ");
            textoLocal.Add("VERSIÓN         ");
            textoLocal.Add("RUTAS           ");
            textoLocal.Add("MENSAJES        ");
            textoLocal.Add("NUNERO DE SERIE ");
            textoLocal.Add("PANEL           ");
            textoLocal.Add("AJUSTA SALIDA   ");
            textoLocal.Add("SENTIDO         ");
            textoLocal.Add("IDA             ");
            textoLocal.Add("VUELTA          ");
            textoLocal.Add("IDENTIFICANDO   ");
            textoLocal.Add("      FALLA     ");
            textoLocal.Add("      ÉXITO     ");
            textoLocal.Add("                ");
            textoLocal.Add("NUEVA CONFIG.    ");
            textoLocal.Add("AJUSTAR RELOJ   ");
            textoLocal.Add("SELEC. REGIÓN   ");
            textoLocal.Add("CONFIGURAGIÓN   ");
            textoLocal.Add("EMPAREJAR       ");
            textoLocal.Add("VOLCADO DE MEM  ");
            textoLocal.Add("FORMAT. PENDRIVE");
            textoLocal.Add("DOMLUNMARMIEJUE ");
            textoLocal.Add("VIESAB          ");
            textoLocal.Add("MENSAJE         ");
            textoLocal.Add("SELECCIONA PANEL");
            textoLocal.Add("RUTAS           ");
            textoLocal.Add("AUTOMÁTICO?     ");
            textoLocal.Add("                ");
            textoLocal.Add("PANELES         ");
            textoLocal.Add("ESTA SEGURO     ");
            textoLocal.Add("FOMATEAR ?      ");
            textoLocal.Add("ACTUALIZANDO    ");
            textoLocal.Add("RETIRE PENDRIVE ");
            textoLocal.Add("   RESETEANDO   ");
            textoLocal.Add("CARGUE UNA NUEVA");
            textoLocal.Add("CONFIGURACIÓN   ");
            textoLocal.Add("BORRAR ARCHIVOS ");
            textoLocal.Add("SINCRONIZANDO   ");
            textoLocal.Add("PANEL   XX  NO  ");
            textoLocal.Add("FUE DETECTADO   ");
            textoLocal.Add("OPCIONESAVANZADAS");
            textoLocal.Add("GUARDANDO       ");
            textoLocal.Add("GUARDAR LOG     ");
            textoLocal.Add("PANEL  XX ESTÁ  ");
            textoLocal.Add("CON DEFECTO     ");
            textoLocal.Add("SI      NO      ");
            textoLocal.Add("DESBLOQUEAR A   ");
            textoLocal.Add("FUNCIÓN?        ");
            textoLocal.Add("CONTRASEÑA      ");
            textoLocal.Add("CONTRASEÑAINCORR");
            textoLocal.Add("                ");
            textoLocal.Add("CONTROLADOR     ");
            textoLocal.Add("TODOS DISP.     ");
            textoLocal.Add("TODOS PANELES   ");
            textoLocal.Add("SISTEMA ARCHIVO ");
            textoLocal.Add("DEFECTUOSO      ");
            textoLocal.Add("ENCENDER PANELES");
            textoLocal.Add("CONFIGURAGIÓN NO");
            textoLocal.Add("PUEDE SER CARGAD");
            textoLocal.Add("SALIDA:".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("DOM".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("LUN".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MAR".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MIE".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("JUE".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("VIE".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("SAB".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("CONFIG. FÁBRICA ");
            textoLocal.Add("HOJA DE         ");
            textoLocal.Add("COMUNICACIÓN    ");
            textoLocal.Add("NUEVA CONFIG.   ");
            textoLocal.Add("INVALIDA        ");
            textoLocal.Add("APARAJAMIENTO   ");
            textoLocal.Add("INCOMPATIBLE    ");
            textoLocal.Add("TIEMPO ENCENDIDO");
            textoLocal.Add("TIEMPO APAGADO  ");
            textoLocal.Add("INDEFINIDAMENTE ");
            textoLocal.Add("NSS não foi     ");
            textoLocal.Add("detectado       ");
            /* Adição de mais 13 frases incluindo os modos de teste */
            textoLocal.Add("NO HAY ARCHIVO".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MSJ SECUNDÁRIA ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MSJ PRINCIPAL".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("NOMBRE CARPETA".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("AUTOMÁTICO ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("LINEAS ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("COLUMNAS ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("APAGADO ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("ACCESO ".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("TEXTO".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("INCENDE PRUEBA".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("MODO DE PRUEBA".PadRight(TAM_MAX_FRASE, '\0'));
            textoLocal.Add("APP ".PadRight(TAM_MAX_FRASE, '\0'));

            return textoLocal.ToArray();
        }

        private string[] CarregarFrases(System.Resources.ResourceManager rm)
        {
            List<string> textoLocal = new List<string>();

            for (int i = 0; i< QTD_FRASES; i++)
            {
                textoLocal.Add(rm.GetString("TEXTO_"+i.ToString()+"_ARQUIVO_LPK").PadRight(TAM_MAX_FRASE, '\0'));
            }
            
            return textoLocal.ToArray();
        }

        public unsafe void stringToByteArray(Byte* array, List<string> listaFrases, int length)
        {
            int indiceFrase = 0;
            foreach (var str in listaFrases)
            {                
                char[] charArray = str.PadRight(length, '\0').ToCharArray();
                for (int i = 0; i < length; i++)
                {
                    array[(indiceFrase*TAM_MAX_FRASE) + i] = (Byte) charArray[i];
                }
                indiceFrase++;
            }
        }
        public void FromBytesToFormatoIdioma(Byte[] dados)
        {            
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoIdioma* idioma = (FormatoIdioma*)pSrc;

                    this.versao = idioma->versao;
                    this.nome = String.Empty;
                    for (int i = 0; i < TAM_MAX_FRASE; i++)
                    {                        
                        this.nome += (char)idioma->nome[i];
                    }
                    this.texto.Clear();
                    for (int indiceFrase = 0; indiceFrase < QTD_FRASES; indiceFrase++)
                    {
                        String Aux = string.Empty;
                        for (int i = 0; i < TAM_MAX_FRASE; i++)
                        {
                            Aux += (char)idioma->texto[(indiceFrase*TAM_MAX_FRASE) + i];
                        }
                        this.texto.Add(Aux);
                    }                    
                }               
            }
        }
        public Byte[] toByteArray()
        {
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoIdioma)];

                FormatoIdioma idioma = new FormatoIdioma();
                idioma.versao = (Byte)this.versao;
            }
            unsafe
            {
                Byte[] resultado = new Byte[sizeof(FormatoIdioma)];

                fixed (byte* pSrc = resultado)
                {
                    FormatoIdioma* idioma = (FormatoIdioma*)pSrc;

                    idioma->versao = (Byte)this.versao;
                    this.nome = this.nome.PadRight(16, '\0');

                    for (int i = 0; i < this.nome.Length; i++)
                    {
                        idioma->nome[i] = (byte)this.nome[i];
                    }

                    stringToByteArray(idioma->texto, this.texto, 16);
                    idioma->crc = this.crc;

                }

                return resultado;
            }

        }

        public void Salvar(string arquivoNome)//, object controlador)
        {            
            AtualizarCRC();
            byte[] dados = this.toByteArray();
            FileStream fs = File.Create(arquivoNome);
            fs.Write(dados, 0, dados.Length);
            fs.Close();
        }

        public object Abrir(string arquivoNome)
        {
            if (VerificarIntegridade(arquivoNome))
            {
                unsafe
                {
                    FileStream fs = File.OpenRead(arquivoNome);
                    byte[] dados = new byte[(int)fs.Length];
                    fs.Read(dados, 0, dados.Length);
                    fs.Close();

                    FromBytesToFormatoIdioma(dados);
                    return this;
                }
            }
            return null;

        }

        public void RestaurarVersao()
        {
            //apenas abriu sem verificar a versão do arquivo do usuário e sem repassar para as classes inferiores porque é a versão inicial
            this.Abrir(this.nome);

            //se houvesse classes de versões anteriores 

            //// Recuperando o ultimo LPK
            //Arquivo_LPK_V01 LPKRestaurado = new Arquivo_LPK_V01(rm);
            //LPKRestaurado.nome = arquivoNome;
            //LPKRestaurado.RestaurarVersao();

            ////Adicionando as alterações da versão 2 ao lpk v1 recuperado

            ////copiando as informações do arquivo do usuário para a nova versão
            //this.rm = LPKRestaurado.rm;
            //this.nome = LPKRestaurado.nome;
            //this.texto = LPKRestaurado.texto;

            ////adicionando as novas informações
            //this.texto.Add(rm.GetString("TEXTO_104_ARQUIVO_LPK").PadRight(TAM_MAX_FRASE, '\0'));
        }


        public unsafe void AtualizarCRC()
        {
            Byte[] dados = toByteArray();

            fixed (byte* pSrc = dados)
            {
                FormatoIdioma* parametros = (FormatoIdioma*)pSrc;

                this.crc = CalcularCRC(dados);
            }
        }
         public bool VerificarIntegridade(string arquivoNome)
        {
            bool resposta = false;
            FileStream fs = File.OpenRead(arquivoNome);
            byte[] dados = new byte[(int)fs.Length];
            fs.Read(dados, 0, dados.Length);
            resposta = VerificarCRC(dados);
            if (!resposta)
            {
                throw new CRCFileException("CRC file error.");
            }
            resposta = VerificarTamanhoArquivo(fs);
            if (!resposta)
            {
                throw new SizeFileException("Size file error.");
            }

            fs.Close();
            return resposta;
        }
        private unsafe UInt16 CalcularCRC(Byte[] dados)
        {
            int tamanho;

            Byte[] dadosCRC = new byte[dados.Length - sizeof(UInt16)];

            fixed (byte* pSrc = dados)
            {
                FormatoIdioma* parametros = (FormatoIdioma*)pSrc;

                Array.Copy(dados, 0, dadosCRC, 0, (int)&parametros->crc - (int)pSrc);
                Array.Copy(dados, ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)), dadosCRC,
                           (int)&parametros->crc - (int)pSrc,
                           dados.Length - ((int)&parametros->crc - (int)pSrc + sizeof(UInt16)));

                return CRC16CCITT.Calcular(dadosCRC);
            }
        }

        public bool VerificarCRC(byte[] dados)
        {
            unsafe
            {
                fixed (byte* pSrc = dados)
                {
                    FormatoIdioma* parametros = (FormatoIdioma*)pSrc;

                    return (parametros->crc == CalcularCRC(dados));
                }
            }
        }

        public bool VerificarTamanhoArquivo(FileStream fs)
        {
            bool resposta = false;
            if (fs.Length == NSS_LPK_SIZE)
            {
                return true;
            }
            unsafe
            {
                resposta = (fs.Length >= sizeof(FormatoIdioma));
            }
            return resposta;
        }
    }
}
