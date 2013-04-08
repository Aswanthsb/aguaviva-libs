using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class NoiseGenerator : SoundGenerator
   {
      protected UInt16 m_freq;
      virtual public void SetFreq(UInt16 per)
      {
         m_freq = per;
      }

      protected UInt16 m_volume;
      public void SetVolume(UInt16 volume)
      {
         m_volume = volume;
      }

      sbyte[] noisetable = new sbyte[1024];

      Random ramdom = new Random();

      public NoiseGenerator(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         for (int i = 0; i < noisetable.Length; i++)
         {
            noisetable[i] = (sbyte)ramdom.Next(0, 2);
         }
      }

      override public sbyte GetValue()
      {
         sbyte val = 0;

         m_time += m_freq;
         if (m_time > m_sampleRate)
            m_time -= m_sampleRate;

         val = noisetable[m_time / noisetable.Length];

         if (val > 0)
         {
            val = (sbyte)m_volume;
         }
         else
         {
            val = 0;
         }
         return val;
      }   

   }
}
