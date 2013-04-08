using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class SimplePSG : SoundGenerator
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


      public SimplePSG(string name, uint sampleRate)
         : base(name, sampleRate)
      {
      }

      bool m_reallyEnabled = true;

      override public sbyte GetValue()
      {
         sbyte val=0;

         m_time += m_freq;

         if (m_time > m_sampleRate)
            m_time -= m_sampleRate;

         if (m_time < m_sampleRate/2)
            val = 1;

         if (val > 0)
         {
            val = (sbyte)m_volume;
         }
         else
         {
            val = 0;
         }

         if (m_enabled != m_reallyEnabled)
         {
            if (m_enabled == false)
            {
               if (val == 0)
               {
                  m_reallyEnabled = false;      
               }
            }
            else
            {
               m_time = 0;
               m_reallyEnabled = true;
            }
         }

         if (m_reallyEnabled == false)
         {
            val = 0;
         }


         return val;
      }
   }
}
