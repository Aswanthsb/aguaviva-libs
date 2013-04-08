using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class NullGenerator : SoundGenerator
   {
      public NullGenerator(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         
      }

      public override sbyte GetValue()
      {
         return 0;
      }


   }
}
