using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class SampleGenerator : MidiGenerator
   {
      protected UInt16 m_freq;

      public SampleGenerator(string name, uint sampleRate)
         : base(name, sampleRate)
      {
      }

      public sbyte[] m_sampleData;
      public int m_SampleRate;
      public UInt16 m_Length;
      public UInt16 m_Repeat;
      public UInt16 m_RepeatLength;
      private UInt32 m_FixedPoint;

      public void Set( sbyte [] data, int sampleRate)
      {
         m_sampleData = data;
         m_SampleRate = 44100;
      }

      /*
      private void SetTime(UInt32 time)
      {
         m_time = (UInt32)((time * (UInt32)44100)  * (2 * (UInt32)m_period)/ (UInt32)7159090); 
      }
       */ 

      override public sbyte GetValue()
      {
         if (m_freq == 0)
            return 0;

         //UInt32 tttime = (m_time * ( (UInt32)7159090 / m_period ) / (UInt32)44100 );
         m_time += m_FixedPoint>>16;
         m_FixedPoint &= 0x0000ffff;
         m_FixedPoint += (((UInt32)7159090 / m_freq) << 16) / (UInt32)44100;

         //UInt32 time = m_time;

         if (m_Repeat>0 && m_RepeatLength > 0 && m_time == m_Repeat + m_RepeatLength)
         {
            m_time = m_Repeat;
            m_FixedPoint = 0;
         }
         else if (m_time >= m_sampleData.Length)
         {
            if (m_Repeat>0)
            {
               m_time = m_Repeat;
               m_FixedPoint = 0;
            }
            else
            {
               return 0;
            }
         }

         //Console.WriteLine(m_time);


         sbyte val = (sbyte)(m_volume * m_sampleData[m_time] / 64);
         //m_time++;
         return val;
      }
   }
}
