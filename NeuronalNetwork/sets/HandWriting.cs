using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace test
{
   class HandWriting : NeuralTask
   {
      byte[] labels;
      int rows;
      int columns;
      
      int[] topology;

      int Read(BinaryReader b, int i)
      {
         int res = 0;

         while (i-- > 0)
         {
            res <<= 8;
            res |= b.ReadByte();
         }
         return res;
      }

      override public void Init()
      {

         string path2 = "c:\\nn\\train-labels.idx1-ubyte";
         using (BinaryReader b = new BinaryReader(File.Open(path2, FileMode.Open)))
         {
            int mn = Read(b, 4);
            int items = Read(b, 4);
            labels = new byte[items];
            labels = b.ReadBytes(items);
         }

         string path1 = "c:\\nn\\train-images.idx3-ubyte";
         using (BinaryReader b = new BinaryReader(File.Open(path1, FileMode.Open)))
         {
            int mn = Read(b, 4);
            int items = Read(b, 4);
            rows = Read(b, 4);
            columns = Read(b, 4);

            items = 10;

            trainingSet = new double[items][];
            for (int i = 0; i < items; i++)
            {
               Image img = new Image();
               img.Load(columns, rows, b);
               trainingSet[i] = img.ScaleDown().ScaleDown().ImageWithDoubles();
            }
            columns/=4;
            rows /= 4;
         }

         topology = new int[] { trainingSet[0].Length, 30,15,10 };
         expected = new double[trainingSet.Length][];
         for (int i = 0; i < trainingSet.Length; i++)
         {
            expected[i] = new double[10];
            for (int j = 0; j < expected[i].Length; j++)
            {
               expected[i][j] = 0;
            }
            expected[i][labels[i]] = 1;
         }
      }

      override public int[] GetTopology()
      {
         int inputLength = trainingSet[0].Length;
         int outputLength = expected[0].Length;

         return new int[] { inputLength, 5, 3, outputLength };
      }


      public void DrawInput(PaintEventArgs e, int index)
      {
         if (trainingSet == null)
         {
            return;
         }

         for (int k = 0; k < 10; k++)
         {
            for (int i = 0; i < rows; i++)
            {
               for (int j = 0; j < columns; j++)
               {
                  byte col = (byte)((double)255.0 * trainingSet[k][j * (columns) + i]);
                  if (col > 20)
                     e.Graphics.FillRectangle(Brushes.Black, k* 100 + 10 + i * 5, j * 5, 5, 5);
                  else
                     e.Graphics.FillRectangle(Brushes.Gray, k * 100 + 10 + i * 5, j * 5, 5, 5);
               }

               //e.Graphics.DrawString(string.Format("{0}", labels[img]), this.Font, Brushes.Black, 0, 0);
            }
         }
      }       

   }
}
