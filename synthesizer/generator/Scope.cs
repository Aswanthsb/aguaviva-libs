using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   public class Scope : SoundGenerator
   {

      SoundGenerator m_input;
      public void SetInput(object o)
      {
         m_input = (SoundGenerator)o;
      }

      public Scope(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_input = new NullGenerator(name, sampleRate);

         m_inputs["input"] = new variable.SetObject(SetInput);
      }

      public sbyte Output
      {
         get { return GetValue(); }
      }

      override public sbyte GetValue()
      {
         return m_input.GetValue();
      }
   }
}
