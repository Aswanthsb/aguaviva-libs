using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class fir : SoundGenerator
   {
      FFT.fft m_fft = new FFT.fft(1024);

      double[] m_inBuf = new double[512];
      double[] m_outBuf = new double[512];

      int buffer = 0;

      SoundGenerator m_input;
      public void SetInput(object o)
      {
         m_input = (SoundGenerator)o;
      }

      protected variable.Slider m_shift  = new variable.Slider(0,512,0);


      public fir(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_input = new NullGenerator(name, sampleRate);

         m_inputs["shift"] = m_shift;
         m_inputs["input"] = new variable.SetObject(SetInput);
         m_output["output"] = Output;         
      }



      public sbyte Output
      {
         get { return GetValue(); }
      }

      void TransformChunk()
      {
         for (int i = 0; i < 512; i++)
         {
            m_outBuf[i] = m_fft.x[i+512];
         }

         
         for (int i = 0; i < 512; i++)
         {
            m_fft.x[i] = m_inBuf[i];
            m_fft.y[i] = 0;
         }

         for (int i = 0; i < 512; i++)
         {
            double v = (double)m_input.GetValue();
            m_fft.x[i+512] = v;
            m_fft.y[i+512] = 0;

            m_inBuf[i] = v;
         }

         m_fft.SetWindow();
         
         m_fft.FFT(1);

         for (int i = 0; i < 1024; i++)
         {
            int s = m_shift.m_val;

            if (i + s >= 1024)
            {
               m_fft.x[i] = 0;
               m_fft.y[i] = 0;
            }
            else
            {
               m_fft.x[i] = m_fft.x[i + s];
               m_fft.y[i] = m_fft.y[i + s];
            }
         }

         m_fft.FFT(-1);
         
         for (int i = 0; i < 512; i++)
         {
            m_outBuf[i] += m_fft.x[i];
         }
      }


      override public sbyte GetValue()
      {
         if (m_inBuf == null)
            return 0;

         if (buffer == 512)
         {
            TransformChunk();
            buffer = 0;
         }         

         return (sbyte)m_outBuf[buffer++];
      }
   }
}
