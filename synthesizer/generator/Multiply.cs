using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class Multiply : SoundGenerator
   {
      SoundGenerator m_input0, m_input1;

      public Multiply(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_input0 = new NullGenerator("", 0);
         m_input1 = new NullGenerator("", 0);

         m_inputs["input.0"] = new variable.SetObject(SetInput0);
         m_inputs["input.1"] = new variable.SetObject(SetInput1);
         m_output["output"] = Output;

      }

      public void SetInput0(object o)
      {
         m_input0 = (SoundGenerator)o;
      }

      public void SetInput1(object o)
      {
         m_input1 = (SoundGenerator)o;
      }

      public sbyte Output
      {
         get { return GetValue(); }
      }

      override public sbyte GetValue()
      {
         int a = m_input0.GetValue();
         int b = m_input1.GetValue();

         int v = ((int)a * (int)b) / 100;
         return (sbyte)v;
      }

   }
}
