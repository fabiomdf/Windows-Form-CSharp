using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Evento
    {
        public const int Versao = 1;
        public String Titulo { get; set;}
        public DateTime DataHora { get; set;}
        public Util.Util.TipoOperacaoEvento Operacao { get; set; }
        public UInt16 valorParametro { get; set; }
        public UInt16 valorParametro2 { get; set; }


        public Evento()
        {

        }

        public Evento(string titulo)
        {
            this.Titulo = titulo;
            this.Operacao = Util.Util.TipoOperacaoEvento.SELECAO_MSG_PRINCIPAL;
            this.valorParametro = 0;
            this.valorParametro2 = 0;
            this.DataHora = DateTime.Now.AddHours(1);
        }

        public Evento(Evento evento_antigo) 
        {
            this.Titulo = evento_antigo.Titulo;
            this.DataHora = evento_antigo.DataHora;
            this.Operacao = evento_antigo.Operacao;
            this.valorParametro = evento_antigo.valorParametro;
            this.valorParametro2 = evento_antigo.valorParametro2;
        }

        public bool CompararTodoObjetosEvento(Evento evento1, Evento evento2)
        {
            bool alterou = false;

            if ((evento1.Titulo != evento2.Titulo) || (evento1.DataHora != evento2.DataHora) || (evento1.Operacao != evento2.Operacao) || (evento1.valorParametro != evento2.valorParametro) || (evento1.valorParametro2 != evento2.valorParametro2))
                alterou = true;

            return alterou;
        }

        public bool CompararDataHoraOperacao(Evento evento1, Evento evento2)
        {
            bool alterou = false;

            if ((evento1.DataHora != evento2.DataHora) || (evento1.Operacao != evento2.Operacao))
                alterou = true;

            return alterou;

        }
    }
}
