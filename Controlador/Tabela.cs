using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Controlador
{
    public class Tabela
    {
        public Util.Util.FormatoTipoCelula formato;

        public celula texto1;
        public celula texto2;
        public celula texto3;
        public celula texto4;

        public Util.Util.AlinhamentoHorizontal alinhamentoHorizontal;
        public Util.Util.AlinhamentoVertical alinhamentoVertical;

        // As propriedades de dimensão, são referentes ao tamanho do Painel. Determinando a dimensão máxima.
        public int Altura = 0;
        public int Largura = 0;

        // Mesmo array de bytes de Vídeo 01 e 02.
        public byte[] PixelBytes;

        public Tabela()
        {
            // TODO: Implementar os construtores
            switch (formato)
            {
                case Util.Util.FormatoTipoCelula.TextoSimples:
                    break;

                case Util.Util.FormatoTipoCelula.TextoSimplesSimples:
                    break;

                case Util.Util.FormatoTipoCelula.TextoSimplesDuplo:
                    break;

                case Util.Util.FormatoTipoCelula.TextoDuploSimples:
                    break;

                case Util.Util.FormatoTipoCelula.TextoDuploDuplo:
                    break;
            }
        }
    }
}
