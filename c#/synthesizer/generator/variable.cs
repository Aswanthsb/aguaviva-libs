using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace midiplayer
{

   public class variable
   {
      [Serializable]

      public class Slider
      {
         public UInt16 m_min;
         public UInt16 m_max;
         public UInt16 m_val;

         public Slider(UInt16 min, UInt16 max, UInt16 val)
         {
            m_min = min;
            m_max = max;
            m_val = val;
         }
      }

      public delegate void SetUInt16(UInt16 v);
      public delegate void SetObject(object v);
      public delegate void Set();
      public delegate void SetFilename(string s);
      public delegate sbyte SetOutput();

      [NonSerialized]
      public Dictionary<string, object> m_inputs = new Dictionary<string, object>();
      public Dictionary<string, object> m_output = new Dictionary<string, object>();

      public void AddInput( string name, SetUInt16 del)
      {
         m_inputs[name] = new SetUInt16( del );
      }

   }
}
