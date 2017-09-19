using System;
using System.Text;
using System.Drawing;

//TODO Implementar cálculo direto do tamanho máximo da fonte em pixels evitando o método iterativo

namespace Nucleo
{
    public class TextImage
    {
        private String text; //texto que será utilizado na conversão
        private Color textColor; //cor do texto
        private Color backColor; //cor do background
        private Font font; //font do texto
        private Bitmap bitmap; //bitmap que contém a conversão
        private Boolean invalid; //flag que identifica se o bitmap precisa redesenhado
        public int BinaryThreshold;

        private const int defaultThreshold = 200; //threshold padrao da binarização

        public TextImage(String text, Font font, Color textColor, Color backColor, int binaryThreshold)
        {
            this.text = text;
            this.font = font;
            this.textColor = textColor;
            this.backColor = backColor;
            this.invalid = true;
            this.BinaryThreshold = binaryThreshold;
        }

        public TextImage(String text, Font font, Color textColor, Color backColor)
            : this(text, font, textColor, backColor, defaultThreshold)
        {
        }


        public TextImage(String text, Font font, int binaryThreshold)
            : this(text, font, Color.FromArgb(0, 0, 0), Color.FromArgb(255, 255, 255), binaryThreshold)
        {

        }

        public TextImage(String text, Font font)
            : this(text, font, Color.FromArgb(0, 0, 0), Color.FromArgb(255, 255, 255), defaultThreshold)
        {

        }

        public TextImage(String text)
            : this(text, new Font("Arial", 16))
        {

        }


        public String Text
        {
            get { return text; }
            set 
            { 
                text = value;
                invalid = true;
            }
        }

        public Font Font
        {
            get { return font; }
            set 
            {
                font = value;
                invalid = true;
            }
        }
        

        public Color TextColor
        {
            get { return textColor; }
            set 
            {
                textColor = value;
                invalid = true;
            }
        }
        

        public Color BackColor
        {
            get { return backColor; }
            set 
            {
                backColor = value;
                invalid = true;
            }
        }

        public Bitmap getBitmap()
        {
            if (invalid)
            {
                //caso algum parâmetro tenha sido alterado, redesenha o bitmap
                this.bitmap = MakeTextBitmap(this.text, this.font, this.textColor, this.backColor, this.BinaryThreshold);
                invalid = false;
            }

            //caso contrário, retornamos a vers
            return this.bitmap;
        }

        //gera o bitmap a partir do texto
        public static Bitmap MakeTextBitmap(String text, Font font, Color textColor, Color backColor)
        {
            return MakeTextBitmap(text, font, textColor, backColor);
        }

        //gera o bitmap a partir do texto
        public static Bitmap MakeTextBitmap(String text, Font font, Color textColor, Color backColor, int binaryThreshold = 200)
        {
            
            Bitmap textImage;
            float fontSize = font.Size + 1;

            //enquanto o texto gerado não se adequar ao tamanho da fonte, decrementamos o tamanho da fonte e geramos novamente
            do
            {
                fontSize--;
                
               //converte o texto em imagem
               textImage = new Bitmap(DrawText(text, new Font(font.FontFamily, fontSize, font.Style), textColor, backColor));

               //binariza a image
               textImage = Binarize(textImage, binaryThreshold);

               if (!string.IsNullOrEmpty(text))
               {
                   int countEsquerda = 0;
                   int countDireita = 0;
                   if (text.StartsWith(" "))
                   {
                       countEsquerda = text.Length - text.TrimStart().Length;
                   }
                   if (text.EndsWith(" "))
                   {
                       countDireita = text.Length - text.TrimEnd().Length;
                   }
                   //if 
                   {
                       //corta as regiões brancas
                       textImage = Crop(textImage, countEsquerda, countDireita);
                   }

               }

            } while (textImage.Height > font.Size);

            return textImage;
        }

        public static Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            //cria uma imagem para que possamos medir o tamanho da string
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            int countEsquerda = 0;
            int countDireita = 0;
            if (text.StartsWith(" "))
            {
                countEsquerda = text.Length - text.TrimStart().Length;
            }
            if (text.EndsWith(" "))
            {
                countDireita = text.Length - text.TrimEnd().Length;
            }


            //mede o tamanho da string
            SizeF textSize = drawing.MeasureString(text, font);

            if (String.IsNullOrEmpty(text))
                return img; 

            //libera a imagem utilizada para medir o tamanho
            img.Dispose();
            drawing.Dispose();
            int tamanhoMaisEspacos = (int)textSize.Width;
            tamanhoMaisEspacos += countEsquerda;
            tamanhoMaisEspacos += countDireita;


            if( font.Underline)
            {
                tamanhoMaisEspacos += 3;
            }
            //cria uma imagem com o tamanho correto
            img = new Bitmap(tamanhoMaisEspacos, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //desenha o background
            drawing.Clear(backColor);

            //escreve o texto
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }

        public static Bitmap Binarize(Bitmap original, int threshold)
        {
            //cria uma imagem com o mesmo tamanho da original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //obtém o pixel da imagem original
                    Color originalColor = original.GetPixel(i, j);

                    //cria a versão em escala cinza do pixel original
                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                        + (originalColor.B * .11));

                    //binariza o pixel obedecendo o threshold
                    if (grayScale > threshold)
                    {
                        //caso o valor do pixel seja maior que o threshold, o pixel é branco
                        grayScale = 255;
                    }
                    else
                    {
                        //caso contrário, o pixel é preto
                        grayScale = 0;
                    }
  
                    //cria a cor de acordo com a binzarização
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    //atribui o novo valor do pixel na imagem nova
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }

        public static Bitmap Crop(Bitmap bmp, int espacosEsquerda = 0, int espacosDireita = 0)
        {
            //obtém o tamanho da imagem
            int w = bmp.Width;
            int h = bmp.Height;

            //função que verifica se um linha é completamente branca
            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
                    if (bmp.GetPixel(i, row).R != 255)
                        return false;
                return true;
            };

            //função que verifica se uma coluna é completamente branca
            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                    if (bmp.GetPixel(col, i).R != 255)
                        return false;
                return true;
            };

            //obtém o valor do pixel limitante mais alto a ser removido
            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            //obtém o valor do pixel limitante mais baixo a ser removido
            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }

            int leftmost = 0, rightmost = w;

            //obtém o valor do pixel limitante mais a esquerda a ser removido

            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }
            
            
            //obtém o valor do pixel limiteante mais a direta a ser removido
            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            

            if (rightmost == 0) rightmost = w;
            if (bottommost == 0) bottommost = h;

            rightmost += espacosDireita;
            leftmost -= espacosEsquerda;

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            leftmost++;
            topmost++;

            croppedHeight--;
            croppedWidth--;

            try
            {
                if (croppedWidth == 0 || croppedHeight == 0)
                {
                    return bmp;
                }
                else
                {
                    var target = new Bitmap(croppedWidth, croppedHeight);
                    using (Graphics g = Graphics.FromImage(target))
                    {
                         g.DrawImage(bmp,
                          new RectangleF(0, 0, croppedWidth, croppedHeight),
                          //new RectangleF(0, 0, croppedWidth, croppedHeight),
                          new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                          GraphicsUnit.Pixel);
                    }
                    return target;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Os valores são topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                  ex);
            }
        }

    }
}
