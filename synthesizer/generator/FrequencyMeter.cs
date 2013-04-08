using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class FrequencyMeter : SoundGenerator
   {
      sbyte m_oldVal;
      uint m_oldTime;

      SoundGenerator m_input;
      public void SetInput(object o)
      {
         m_input = (SoundGenerator)o;
      }


      public FrequencyMeter(string name, uint sampleRate)
         : base(name, sampleRate)
      {
      }

      override public sbyte GetValue()
      {
         sbyte val = m_input.GetValue();

         if (m_oldVal < 0 && val >= 0)
         {
            float freq = (float)m_sampleRate/ (float)(m_time - m_oldTime);
            Console.WriteLine(freq);

            m_oldTime = m_time;
         }

         m_oldVal = val;

         m_time++;
         return val;
      }


   }
}
