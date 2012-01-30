using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace test
{
   class Image
   {
      int width;
      int height;
      byte[] data;

      public void Load(int w,int h,BinaryReader b)
      {
         width = w;
         height = h;
         data = b.ReadBytes(w*h);
      }

      public Image ScaleDown()
      {
         Image img = new Image();
         img.width = width / 2;
         img.height = height / 2;

         img.data = new byte[width / 2 * height / 2];

         for (int i = 0; i < img.height; i++)
         {
            int ii = 2 * i;
            for (int j = 0; j < img.width; j++)
            {
               int jj = 2 * j;
               img.data[j * img.width + i] = (byte)((data[jj * width + ii] +
                                                 data[jj * width + (ii + 1)] +
                                                 data[(jj + 1) * width + ii] +
                                                 data[(jj + 1) * width + (ii + 1)]) / 4);
            }
         }

         return img;
      }

      public double [] ImageWithDoubles()
      {
         double [] res = new double[width * height];
         for (int i = 0; i < res.Length; i++)
         {
            res[i] = ((double)data[i]) / (double)255;
         }

         return res;
      }
   }
}
