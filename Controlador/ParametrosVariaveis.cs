using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controlador
{
    public class ParametrosVariaveis
    {

        public ParametrosVariaveis()
        {
            RoteiroSelecionado = 0;
            HoraSaida = 0;
            MinutosSaida = 0;
            RegiaoSelecionada = 0;
            motoristaSelecionado = 0;
            SentidoIda = true;
        }

        public ParametrosVariaveis(ParametrosVariaveis OldValue)
        {
            RoteiroSelecionado = OldValue.RoteiroSelecionado;
            HoraSaida = OldValue.HoraSaida;
            MinutosSaida = OldValue.MinutosSaida;
            RegiaoSelecionada = OldValue.RegiaoSelecionada;
            SentidoIda = OldValue.SentidoIda;
            TurnOffUSB = OldValue.TurnOffUSB;
            motoristaSelecionado = OldValue.motoristaSelecionado;
        }

        public UInt16 motoristaSelecionado 
        { 
            get; set; 
        }
        public bool TurnOffUSB
        {
            get; set;
        }
        public UInt32 RoteiroSelecionado
        {
            get; set;
        }
        public Byte HoraSaida
        {
            get; set;
        }
        public Byte MinutosSaida
        {
            get; set;
        }
        public Byte RegiaoSelecionada
        {
            get; set;
        }
        public bool SentidoIda
        {
            get; set;
        }
        public void GerarArquivoVAR(String diretorio_raiz)
        {
            Arquivo_VAR avar = new Arquivo_VAR();

            avar.horaSaida = HoraSaida;
            avar.minutosSaida = MinutosSaida;
            avar.regiao = RegiaoSelecionada;
            avar.roteiro = RoteiroSelecionado;
            avar.sentidoIda = SentidoIda;
            avar.regiao = this.RegiaoSelecionada;
            avar.motorista = this.motoristaSelecionado;
            avar.TurnOffUSB = this.TurnOffUSB;
            avar.Salvar(diretorio_raiz + Util.Util.ARQUIVO_VAR);
        }



    }
}
