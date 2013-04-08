using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class EnvelopeExtractor : SoundGenerator
   {
      sbyte m_val;

      int m_timeErr;

      SoundGenerator m_input;
      public void SetInput(object o)
      {
         m_input = (SoundGenerator)o;
      }

      public EnvelopeExtractor(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_inputs["env"] = new variable.SetUInt16(SetEnver);
         m_inputs["input"] = new variable.SetObject(SetInput);
         m_output["output"] = Output;
      }

      public sbyte Output
      {
         get { return GetValue(); }
      }

      protected int m_enver;
      public void SetEnver(UInt16 enver)
      {
         m_enver = (int)(enver * (1 << 16) / 1000);
      }

      override public sbyte GetValue()
      {
         if (m_input == null)
            return 0;

         sbyte val = m_input.GetValue();
         if (val < 0)
            val *= -1;

         if ( val > m_val )
         {
            m_val = val;
         }
         else
         {
            m_timeErr += m_enver;;

            if ((m_timeErr>>16) > 0)
            {
               m_val--;

               m_timeErr &= 0xffff;
            }
         }



         return m_val;
      }

   }
}
