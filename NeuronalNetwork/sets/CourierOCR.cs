using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace test
{
   class CourierOCR : NeuralTaskEx
   {

      override public void Init()
      {
         int length = 'E' - 'A';

         this.trainingSet = new double[length][];
         this.expected = new double[length][];

         Bitmap bmp = new Bitmap(8, 10, PixelFormat.Format24bppRgb);
         Graphics gBmp = Graphics.FromImage(bmp);
         Font f = new Font("Courier New", 10);
         Rectangle r = new Rectangle(0, 0, 8, 10);

         for (int c = 0; c < length; c++)
         {
            gBmp.FillRectangle(Brushes.Black, r);
            string str = Convert.ToChar('A' + c).ToString();
            gBmp.DrawString(str, f, Brushes.White, -2, -2);
            BitmapData data = bmp.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
               this.trainingSet[c] = new double[10 * 10];
               for (int y = 0; y < data.Height; y++)
               {
                  for (int x = 0; x < data.Height; x++)
                  {
                     byte v = ((byte*)data.Scan0.ToPointer())[x*3 + y * data.Stride];
                     trainingSet[c][x + y * 10] = ((double)v)/255.0;

                  }
               }
            }
            bmp.UnlockBits(data);

            this.expected[c] = new double[length];
            this.expected[c][c] = 1.0;
         }

         base.Init();
      }   

      override public int[] GetTopology()
      {
         int inputLength = trainingSet[0].Length;
         int outputLength = expected[0].Length;

         return new int[] { inputLength, 50, 25, outputLength };
      }

   }
}
