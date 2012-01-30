using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class Chirp : SoundGenerator
	{
      protected variable.Slider m_freq = new variable.Slider(0, 10, 1);

      public sbyte Output
      {
         get { return GetValue(); }
      }

      public Chirp(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_output["output"] = Output;
         m_inputs["freq"] = m_freq;
         m_inputs["reset"] = new variable.Set(ResetTime);
      }



		override public sbyte GetValue()
		{
         double freq = (double)m_time * m_freq.m_val;
         sbyte val = (sbyte)(127 * (Math.Sin(2.0 * Math.PI * freq * ((double)(m_time) / (double)m_sampleRate))));
         m_time++;
         return val;
		}
	}
}
