using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class Mixer : SoundGenerator
   {
      public SoundGenerator[] m_SoundGenList = new SoundGenerator[4];

      public Mixer(string name, uint sampleRate)
         : base(name, sampleRate)      
      {
         for(int i=0;i<4;i++)
         {
            SoundGenerator sg = new NullGenerator("gg",5);
            m_SoundGenList[i] = sg;
         }

         m_inputs["input.0"] = new variable.SetObject(SetInput0);
         m_inputs["input.1"] = new variable.SetObject(SetInput1);
         m_inputs["input.2"] = new variable.SetObject(SetInput2);
         m_inputs["input.3"] = new variable.SetObject(SetInput3);

         m_output["output"] = Output;
      }

      public void SetInput0(object o)
      {
         m_SoundGenList[0] = (SoundGenerator)o;
      }

      public void SetInput1(object o)
      {
         m_SoundGenList[1] = (SoundGenerator)o;
      }

      public void SetInput2(object o)
      {
         m_SoundGenList[2] = (SoundGenerator)o;
      }

      public void SetInput3(object o)
      {
         m_SoundGenList[3] = (SoundGenerator)o;
      }

      public sbyte Output
      {
         get { return GetValue(); }
      }
      
      override public sbyte GetValue()
      {
         // mix all channels
         Int32 res = 0;
         for (int channels = 0; channels < m_SoundGenList.Length; channels++)
         {
            sbyte val = m_SoundGenList[channels].GetValue();
            //Console.Write( val+"    ");
            res += val;
         }

         //Console.WriteLine();
         if (m_SoundGenList.Length == 0)
            return 0;
         return (sbyte)(res / m_SoundGenList.Length);
      }
   }
}
