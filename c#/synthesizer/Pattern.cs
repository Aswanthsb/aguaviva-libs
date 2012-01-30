using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class Pattern
   {
      SoundGenerator m_sg;


      List<List<string>> m_values;

      NoteToFreq n2f = new NoteToFreq();

      public Pattern()
      {
         
      }

      public void init( SoundGenerator sg, int length )
      {
         m_sg = sg;
         m_values = new List<List<string>>();

         foreach (KeyValuePair<string, object> pair in m_sg.m_inputs)
         {
            m_values.Add(new List<string>(length));
         }
      }

      public bool SetValue( int time, int row, string value )
      {
         int i = 0;
         foreach (KeyValuePair<string, object> pair in m_sg.m_inputs)
         {
            if ( i==row)
            {
               m_values[time][i] = value;
               return true;
            }
         }

         return false;
      }

      public void Play(int time)
      {
         int i = 0;
         foreach (KeyValuePair<string, object> pair in m_sg.m_inputs)
         {
            string value = m_values[time][i];

            if (pair.Key == "freq")
            {
               ((variable.Slider)(pair.Value)).m_val = (ushort)n2f.Translate(value);
            }
            else if (pair.Key == "volume")
            {
               ((variable.Slider)(pair.Value)).m_val = (ushort)int.Parse(value);
            }            
         }
      }

   }
}
