using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace interferences
{
   public partial class Form1 : Form
   {
      System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(512, 512);

      float time = 0;
      float SoundSpeed = 343.2f; // meters/second
      float PixelsPerMeter = 128; 

      public Form1()
      {
         InitializeComponent();
      }

      private void Form1_Load(object sender, EventArgs e)
      {
      }

      private double Wave(float t, float freq, float x, float y)
      {
         double d = Math.Sqrt(x * x + y * y) / PixelsPerMeter;
         //double d = x / PixelsPerMeter;

         return Math.Sin(2 * Math.PI * freq *( t - (d/SoundSpeed) ));
      }

      public void Calculate()
      {
         Graphics g = Graphics.FromImage(bmp);
         g.FillRectangle(Brushes.Black, 0, 0, 512, 512);

         System.Drawing.Imaging.BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); 

         /*
         float f1 =1;
         float.TryParse(textBox1.Text, out f1);

         float f2 = 1;
         float.TryParse(textBox2.Text, out f2);
         */

         float xx = 256;
         float yy = 256;
         

         int stride = bmData.Stride;
         System.IntPtr Scan0 = bmData.Scan0;
         unsafe
         {
               byte* p = (byte*)(void*)Scan0;
               int nOffset = stride - bmp.Width * 3;
               int nWidth = bmp.Width * 3;


               for (int y = 0; y < bmp.Height; ++y)
               {
                  for (int x = 0; x < bmp.Width; ++x)
                  {
                     bool t = true;
                     for (int f1 = 1000; f1 < 5000; f1+=1000)
                     {
                        for (int f2 = 1000; f2 < 5000; f2+=1000)
                        {
                           double val = Wave(time, f1, xx - 128, yy) + 
                                        Wave(time, f2, xx - 128 * 3, yy);

                           int b = 0;
                           b += (int)(128 * Wave(time, f1, x - 128, y));
                           b += (int)(128 * Wave(time, f2, x - 128 * 3, y));
                           b /= 2;

                           if (val * b < 0)
                           {
                              t = false;
                              break;
                           }
                        }
                     }                  
                     if ( t == true)
                        p[2] = 255;


                     if (x == xx && y == yy)
                     {
                        p[0] = 255;
                        p[1] = 255;
                     }

                     p += 3;

                  }
                  p += nOffset;
               }
            }
         

         bmp.UnlockBits(bmData);


         Invalidate();
      }


      private void Form1_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.DrawImage(bmp, 0, 0);
         e.Graphics.DrawString(string.Format("{0} seconds", time), this.Font, Brushes.LightGreen, 0, 0);

      }

      private void timer1_Tick(object sender, EventArgs e)
      {
         time += 0.005f;
         Calculate();
      }

      private void textBox1_TextChanged(object sender, EventArgs e)
      {

      }

      private void textBox2_TextChanged(object sender, EventArgs e)
      {

      }
   }
}
