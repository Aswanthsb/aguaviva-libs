using System;
using System.Collections.Generic;
using System.Text;

namespace FFT
{
   class fft
   {
      public double[] x;
      public double[] y;

      public fft(int length)
      {
         x = new double[ length ];
         y = new double[ length ];
      }

      public void SetData(byte[] data, int offset, int length)
      {
         for (int i = 0; i < length; i++)
         {
            x[i] = (double)data[offset + i];
            y[i] = 0;
         }
      }

      public void SetData( double [] data, int offset )
      {
         for(int i=0;i<x.Length;i++)
         {
            x[i] = (double)data[offset + i];
            y[i] = 0;
         }
      }

      public void GetData(double[] data, int offset)
      {
         for (int i = 0; i < x.Length; i++)
         {
            data[offset + i] += x[i];
         }
      }


      public void SetHarmonic(int n, double phase)
      {
         for (int i = 0; i < x.Length; i++)
         {
            x[i] = (double)Math.Cos( phase + ( 2 * Math.PI * i * n ) / x.Length );
            y[i] = 0;
         }
      }


      public void SetWindow()
      {
         for (int i = 0; i < x.Length; i++)
         {
            double h = 0.5f * (1.0 - Math.Cos((double)(2.0 * Math.PI * i) / (double)(x.Length)));
            x[i] = x[i]*h;            
         }
      }

      public double Power(int i)
      {
         return Math.Sqrt(x[i] * x[i] + y[i] * y[i]);
      }


      double smbAtan2(double x, double y)
      {
        double signx;
        if (x > 0.0) signx = 1.0;  
        else signx = -1.0;
        
        if (x == 0.0) return 0.0;
        if (y == 0.0) return signx * Math.PI / 2.0;
        
        return Math.Atan2(x, y);
      }

      public double Phase(int i)
      {
         return smbAtan2(y[i], x[i]);
         /*
         if (x[i] > 0)
         {
            if (y[i] > 0)
            {
               return Math.Atan2(y[i], x[i]);
            }
            else
            {
               return 2.0*Math.PI - Math.Atan2(-y[i], x[i]) ;
            }
         }
         else
         {
            if (y[i] > 0)
            {
               return Math.PI - Math.Atan2(y[i], -x[i]);
            }
            else
            {
               return Math.PI + Math.Atan2(-y[i], -x[i]);
            }
         }
          */
      }

      /*
         This computes an in-place complex-to-complex FFT 
         x and y are the real and imaginary arrays of 2^m points.
         dir =  1 gives forward transform
         dir = -1 gives reverse transform 
      */
      public bool FFT( int dir )
      {
         long n,i,i1,j,k,i2,l,l1,l2;
         double c1,c2,tx,ty,t1,t2,u1,u2,z;

         if (x.Length != y.Length)
            return false;

         long m = 8;// x.Length;

         /* Calculate the number of points */
         n = 1;
         for (i=0;i<m;i++) 
            n *= 2;

         /* Do the bit reversal */
         i2 = n >> 1;
         j = 0;
         for (i=0;i<n-1;i++) {
            if (i < j) {
               tx = x[i];
               ty = y[i];
               x[i] = x[j];
               y[i] = y[j];
               x[j] = tx;
               y[j] = ty;
            }
            k = i2;
            while (k <= j) {
               j -= k;
               k >>= 1;
            }
            j += k;
         }

         /* Compute the FFT */
         c1 = -1.0; 
         c2 = 0.0;
         l2 = 1;
         for (l=0;l<m;l++) {
            l1 = l2;
            l2 <<= 1;
            u1 = 1.0; 
            u2 = 0.0;
            for (j=0;j<l1;j++) {
               for (i=j;i<n;i+=l2) {
                  i1 = i + l1;
                  t1 = u1 * x[i1] - u2 * y[i1];
                  t2 = u1 * y[i1] + u2 * x[i1];
                  x[i1] = x[i] - t1; 
                  y[i1] = y[i] - t2;
                  x[i] += t1;
                  y[i] += t2;
               }
               z =  u1 * c1 - u2 * c2;
               u2 = u1 * c2 + u2 * c1;
               u1 = z;
            }
            c2 = Math.Sqrt((1.0 - c1) / 2.0);
            if (dir == 1) 
               c2 = -c2;
            c1 = Math.Sqrt((1.0 + c1) / 2.0);
         }

         /* Scaling for forward transform */
         if (dir == 1) {
            for (i=0;i<n;i++) {
               x[i] /= n;
               y[i] /= n;
            }
         }
         
         return true;
      }

   }
}
