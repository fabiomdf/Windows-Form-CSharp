using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nucleo;
using Persistencia;
using System.Security.Cryptography;

namespace Controlador
{
    public class ParametrosFixos
    {
        // Quantidade de funções pré-definidas pelo FIRMWARE do controlador
        public const int QUANTIDADE_FUNCOES_DEFINIDAS = Util.Util.QUANTIDADE_DE_FUNCOES_BLOQUEAVEIS;

        // Quantidade de perifericos na rede
        public const int QUANTIDADE_PERIFERICOS_NA_REDE = 4;

        public ParametrosFixos()
        {

            Util.Util.OpcoesApresentacao opcoes = new Util.Util.OpcoesApresentacao();
            opcoes.intervaloAnimacao = 26;
            opcoes.tempoApresentacao = 2000;
            opcoes.animacao = 0; // Fixa
            opcoes.alinhamento = 2; // 2 = Centralizado  

            this.HoraInicioDia = 4;
            this.HoraInicioTarde = 12;
            this.HoraInicioNoite = 18;

            this.horaSaida = opcoes;
            this.dataHora = opcoes;
            this.tarifa = opcoes;
            this.somenteHora = opcoes;
            this.temperatura = opcoes;
            this.horaTemperatura = opcoes;

            this.SenhaAcessoEspecial = string.Empty;
            this.SenhaAntiRoubo = string.Empty;

            this.BloqueioFuncoes = new bool[QUANTIDADE_FUNCOES_DEFINIDAS];

            this.PerifericosNaRede = new bool[QUANTIDADE_PERIFERICOS_NA_REDE];

            this.PaineisAPP = new List<int>();

            this.MinutosInverterLED = 0;

            this.ModoApresentacaoDisplayLD6 = false;

        }

        // Construtor de cópia.
        public ParametrosFixos(ParametrosFixos OldValue)
        {
            BloqueioFuncoes = OldValue.BloqueioFuncoes;

            HoraInicioDia = OldValue.HoraInicioDia;

            HoraInicioTarde = OldValue.HoraInicioTarde;

            HoraInicioNoite = OldValue.HoraInicioNoite;

            QtdPaineis = OldValue.QtdPaineis;

            somenteHora = OldValue.somenteHora;
            dataHora = OldValue.dataHora;
            horaSaida = OldValue.horaSaida;
            temperatura = OldValue.temperatura;
            tarifa = OldValue.tarifa;
            horaTemperatura = OldValue.horaTemperatura;
            PaineisAPP = OldValue.PaineisAPP;

            MinutosInverterLED = OldValue.MinutosInverterLED;

            PerifericosNaRede = OldValue.PerifericosNaRede;

            this.ModoApresentacaoDisplayLD6 = (null == OldValue.ModoApresentacaoDisplayLD6)?false: OldValue.ModoApresentacaoDisplayLD6;

        }
        public List<int> PaineisAPP { get; set; }

        public byte MinutosInverterLED { get; set; }

        public bool[] BloqueioFuncoes  { get; set; }

        public bool[] PerifericosNaRede { get; set; }

        public int HoraInicioDia  { get; set; }

        public int HoraInicioTarde { get; set; }

        public int HoraInicioNoite  { get; set; }

        public int QtdPaineis  { get; set; }

        public String SenhaAntiRoubo { get; set; }

        public String SenhaAcessoEspecial { get; set; }

        public bool HabilitaSenha { get; set; }

        public bool ModoApresentacaoDisplayLD6 { get; set; }

        public bool HabilitaLock { get; set; }

        public bool serialAnexo { get; set; }

        public byte baudRate { get; set; }
        public UInt16 timeoutFalhaRede { get; set; }

        public Util.Util.OpcoesApresentacao somenteHora;
        public Util.Util.OpcoesApresentacao dataHora;
        public Util.Util.OpcoesApresentacao horaSaida;
        public Util.Util.OpcoesApresentacao temperatura;
        public Util.Util.OpcoesApresentacao tarifa;
        public Util.Util.OpcoesApresentacao horaTemperatura;


        public void GerarArquivoFIX(String diretorio_raiz)
        {
            Arquivo_FIX arfix = new Arquivo_FIX();
            arfix.CriarParametrosFixosPadrao();

            for (int i = 0; i < BloqueioFuncoes.Length; i++)
            {
                arfix.BloqueioDeFuncoes[i] = BloqueioFuncoes[i];
            }

            for(int i=0;i< PerifericosNaRede.Length;i++)
            {
                arfix.Perifericos[i] = PerifericosNaRede[i];
            }

            for(int i = 0; i < arfix.PaineisAPP.Length; i++)
                for(int j=0; j < this.PaineisAPP.Count; j++)
                    if (PaineisAPP[j] == i)
                    {
                        arfix.PaineisAPP[i] = true;
                        break;
                    }
                
            arfix.funcoes.funcoes1 = arfix.ConvertToUInt32();
            arfix.senhaAntiFurto = Util.AtivacaoSenha.ConverterSenhaNumero(SenhaAntiRoubo);
            arfix.senhaAcessoEspecial = Util.AtivacaoSenha.ConverterSenhaNumero(SenhaAcessoEspecial);
            arfix.ativaSenhaAntiFurto = HabilitaSenha;
            arfix.ativaLock = HabilitaLock;
            arfix.tempoInverterLed = MinutosInverterLED;
            arfix.baudRate = baudRate;
            arfix.tempoAnimaSemComunicacao = timeoutFalhaRede;
            
            //arfix.painelAPP = (byte)PainelAPP_NSS;
            arfix.painelAPP = arfix.ConvertPaineisToUint32();

            arfix.horaBomDia = (byte)HoraInicioDia;
            arfix.horaBoaTarde = (byte)HoraInicioTarde;
            arfix.horaBoaNoite = (byte)HoraInicioNoite;

            arfix.qntPaineis = (uint)QtdPaineis;

            arfix.perifericoNaRede = arfix.ConvertToUint16();

            arfix.Salvar(diretorio_raiz + Util.Util.ARQUIVO_FIX);
        }
        


        
    }
}
