using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class Resample : SoundGenerator
   {
      int m_deltaTime;

      variable.Slider m_baseFreq = new variable.Slider(0, 3000, 1000);
      variable.Slider m_playbackFreq = new variable.Slider(0, 3000, 1000);

      SoundGenerator m_input;

      public sbyte Output
      {
         get { return GetValue(); }
      }

      public Resample(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_inputs["base freq"] = m_baseFreq;
         m_inputs["freq"] = m_playbackFreq;
         m_output["output"] = Output;
         m_inputs["input"] = new variable.SetObject(SetInput);

         m_input = new NullGenerator("",0);
      }

      public void SetInput(object i)
      {         
         m_input = (SoundGenerator)i;

         m_deltaTime = (int)(65536 * ((float)(m_input.GetSampleRate() / (float)GetSampleRate())));
      }

      void UpdateDelta()
      {
         m_deltaTime = (int)(65536 * ((float)m_playbackFreq.m_val / (float)m_baseFreq.m_val) * ((float)(m_input.GetSampleRate() / (float)GetSampleRate())));
      }


      private int lerp(int y0, int y1, int t0, int t1, int t)
      {
         if (t0 == t1)
            return 0;
         return y0 + ((t - t0) * (y1 - y0)) / (t1 - t0);
      }

      sbyte m_nextValue;
      sbyte m_currValue;
      int m_err;

      public sbyte UpSample()
      {

         int val = 0;
         if (m_time == 0)
         {
            m_currValue = m_input.GetValue();
            m_nextValue = m_input.GetValue();

            m_time = 0;
            m_err = 0;
         }

         if ((m_err >> 16) > 0)
         {
            m_currValue = m_nextValue;
            m_nextValue = m_input.GetValue();
            m_err &= 0xffff;
         }

         val = lerp(m_currValue, m_nextValue, 0, 1 << 16, m_err);

         //Console.WriteLine( "{0:D2} {1:D2}", m_currValue ,val);

         m_time++;
         m_err += m_deltaTime;

         return (sbyte)val;
      }


      int m_timeErr;
      int m_valErr;

      public sbyte DownSample()
      {
         if (m_time == 0)
         {
            m_valErr = 0;
            m_timeErr = 0;
         }

         int val = m_valErr;
         int time = m_deltaTime - m_timeErr;

         for (int i = 0; i < (time >> 16); i++)
         {
            sbyte v = m_input.GetValue();

            val += ((int)v << 16);            
         }


         m_timeErr = (time & 0xffff);

         if (m_timeErr > 0)
         {
            sbyte v = m_input.GetValue();
            val += v * m_timeErr;

            m_timeErr = (0xffff - m_timeErr);
            m_valErr = v * m_timeErr;
         }
         else
         {
            m_timeErr = 0;
            m_valErr = 0;
         }


         m_time++;

         return (sbyte)(val / m_deltaTime);
      }

      override public sbyte GetValue()
      {
         if (m_input == null)
            return 0;

         UpdateDelta();

         if (m_deltaTime == 0x10000)
         {
            return m_input.GetValue();
         }
         else if ( (m_deltaTime>>16) ==0 )
         {
            return UpSample();
         }
         else
         {
            return DownSample();
         }
      }

   }
}

