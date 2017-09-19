using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public static class ArrayLDX2
    {
        public static T[] Concat<T>(T[] array1, T[] array2)
        {
            T[] arrayDestino = new T[array1.Length + array2.Length];

            Array.Copy(array1, 0, arrayDestino, 0, array1.Length);
            Array.Copy(array2, 0, arrayDestino, array1.Length, array2.Length);

            return arrayDestino;
        }
        public static unsafe void StringToByteArray(Byte* array, string str, int length)
        {
            if (str.Length < length)
            {
                str = str.PadRight(length, '\0');
            }
            char[] charArray = str.ToCharArray();
            for (int i = 0; i < length; i++)
            {
                array[i] = (Byte)charArray[i];
            }
        }
        public static unsafe string ByteArrayToString(Byte* array, int length)
        {
            string retorno = String.Empty;
            
            for (int i = 0; i < length; i++)
            {
                retorno += (char) array[i];                
            }

            return retorno;
        }
    }
}
