using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace decodebitfield
{
   class stripdata
   {
      public byte[] data;

      public void stringToBytes(string s)
      {
         byte[] o = new byte[s.Length / 4];

         for (int i = 0; i < s.Length; i += 4)
         {
            string bs = s.Substring(i, 4);
            byte b = 0;
            for (int j = 0; j < 4; j++)
            {
               b *= 2;
               if (bs[j] == '1')
                  b |= 1;
            }
            o[i / 4] = b;
         }

         data = o;
      }

      public string ToString()
      {
         string o = "";
         for (int i = 0; i < data.Length; i++)
         {
            o += ((data[i] & 8) == 0) ? "0" : "1";
            o += ((data[i] & 4) == 0) ? "0" : "1";
            o += ((data[i] & 2) == 0) ? "0" : "1";
            o += ((data[i] & 1) == 0) ? "0" : "1";
         }

         return o;
      }


   }
}
