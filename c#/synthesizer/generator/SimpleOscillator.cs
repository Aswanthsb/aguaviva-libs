using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class SimpleOscillator : SoundGenerator
	{
      protected variable.Slider m_freq = new variable.Slider(0, 22000, 1000);
      protected variable.Slider m_volume = new variable.Slider(0, 128, 120);

      public sbyte Output
      {
         get { return GetValue(); }
      }

      public SimpleOscillator(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_inputs["volume"] = m_volume;
         m_inputs["freq"] = m_freq;
         m_output["output"] = Output;
      }


		public override sbyte GetValue()
		{
         sbyte val = (sbyte)(m_volume.m_val * (Math.Sin(2.0 * Math.PI * m_freq.m_val * ((double)(m_time) / (double)m_sampleRate))));
         m_time++;
         return val;
		}
	}
}
