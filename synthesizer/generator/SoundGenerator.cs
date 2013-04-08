using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace midiplayer
{

   public class SoundGenerator : variable
   {
      protected string m_name;
      public UInt32 m_time;      
      protected bool m_enabled = true;
      protected uint m_sampleRate = 0;

      public SoundGenerator()
      {
         m_name = "NoName";
         m_sampleRate = 0;
      }

      public SoundGenerator(string name, uint sampleRate)
      {
         m_name = name;
         m_sampleRate = sampleRate;
      }

      public uint GetSampleRate()
      {
         return m_sampleRate;
      }

      public void ResetTime()
      {
         m_time = 0;
      }


      public void Enable(bool state)
      {
         m_enabled = state;
      }

      public virtual sbyte GetValue()
      {
         return 0;
      }


   }
}

