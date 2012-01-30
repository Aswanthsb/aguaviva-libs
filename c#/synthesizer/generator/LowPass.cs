using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class LowPass : SoundGenerator
   {
      sbyte m_oldOut = 0;

      SoundGenerator m_input;
      public void SetInput(object o)
      {
         m_input = (SoundGenerator)o;
      }

      protected variable.Slider m_freq  = new variable.Slider(0,22000,1000);

      public LowPass(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_input = new NullGenerator(name, sampleRate);

         m_inputs["cut off freq"] = m_freq;
         m_inputs["input"] = new variable.SetObject(SetInput);
         m_output["output"] = Output;
         
      }



      public sbyte Output
      {
         get { return GetValue(); }
      }

      override public sbyte GetValue()
      {
         double rc = 1.0/(2.0*Math.PI* m_freq.m_val);
         double alfa = 1.0 / (1.0 + rc * m_sampleRate);

         sbyte val = 0;
         if ( m_input != null)
            val = m_input.GetValue();

         m_oldOut = (sbyte)(m_oldOut + (sbyte)(alfa * ((double)( val - m_oldOut))));


         return m_oldOut;
      }
   }
}
