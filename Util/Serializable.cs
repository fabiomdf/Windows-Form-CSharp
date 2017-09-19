using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public abstract class Serializable
    {
        public static unsafe void StringToByteArray(Byte* array, string str, int length)
        {
            if (str.Length < length)
            {
                str = str.PadRight(length, '\0');
            }
            char[] charArray = new char[length];
                charArray = str.ToCharArray();
            for (int i = 0; i < length; i++)
            {
                array[i] = (Byte)charArray[i];
            }
        }

        public abstract Byte[] ToByteArray();
    }
}
