using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controlador
{
    public class celula
    {
        //              <ObjetoCelula Altura=11 Largura=24 AlinhametoHorizontal=Centro AlinhametoVertical=Centro> 032 </ObjetoCelula>
        //              <ObjetoCelula Altura=11 Largura=24 AlinhametoHorizontal=Centro AlinhametoVertical=Centro> IDA </ObjetoCelula>
        //              <ObjetoCelula Altura=11 Largura=24 AlinhametoHorizontal=Centro AlinhametoVertical=Centro> SETUBAL </ObjetoCelula>
        //              <ObjetoCelula Altura=11 Largura=24 AlinhametoHorizontal=Centro AlinhametoVertical=Centro> PRINCIPE </ObjetoCelula>    
        public byte Colspan = 1;
        public byte Rowspan = 1; // Nos casos de Texto Duplo mais Simples ou o inverso, deverá ser no valor 2.
        public int altura = 0;
        public int largura = 0;
        public int AlinhamentoHorizontal = 0;
        public int AlinhamentoVertical = 0;
        public string Texto = "Marcus Vinicius";
        public string Body = "<Font=\"\\fonte01.fnt\"> Marcus</Font> <Font=\"\\fonte02.fnt\">Vinicius </font>";

        public List<byte> PixelBytes = new List<byte>();
        public celula()
        {
            for (byte i = 0; i < 100; i++)
            {
                PixelBytes.Add(i);
            }
            Texto = "Marcus Vinicius";
        }
    }
}
