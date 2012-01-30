using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class EnvelopeGenerator : SoundGenerator
   {
      byte m_envelope;
      byte m_step = 0;

      SoundGenerator m_sg;

      protected UInt16 m_freq;
      virtual public void SetFreq(UInt16 per)
      {
         m_freq = per;
      }


      public EnvelopeGenerator( string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_inputs["freq"] = new variable.SetUInt16(SetFreq);
      }

      

      public void SetSoundGenerator(SoundGenerator sg)
      {
         m_sg = sg;
      }

      public void SetEnvelope(byte env)
      {
         m_envelope = env;
         m_time = 0;
         m_step = 0;
      }

      override public sbyte GetValue()
      {
         if (m_envelope == 0xff)
         {
            return m_sg.GetValue();
         }

         sbyte vol=0;

         uint macrostep = (uint)(m_time * m_freq/ m_sampleRate );
         uint step = (uint)(m_time * m_freq * 16 / m_sampleRate) & 0xf;

         m_time++;
         
         /*
           CONT ATT ALT HLD
             0   0   X   X  \_________  0-3 (same as 9)
             0   1   X   X  /_________  4-7 (same as F)
             1   0   0   0  \\\\\\\\\\  8   (Repeating)
             1   0   0   1  \_________  9
             1   0   1   0  \/\/\/\/\/  A   (Repeating)
             1   0   1   1  \"""""""""  B
             1   1   0   0  //////////  C   (Repeating)
             1   1   0   1  /"""""""""  D
             1   1   1   0  /\/\/\/\/\  E   (Repeating)
             1   1   1   1  /_________  F
         */
         
         bool con = (m_envelope & 8) >0;
         bool att = (m_envelope & 4)>0;
         bool alt = (m_envelope & 2)>0;
         bool hld = (m_envelope & 1)>0;

         if ( att == false)
         {
            vol = (sbyte)(16 - step);
         }
         else
         {
            vol = (sbyte)step;
         }

         if ((alt == true) && ((macrostep & 1)>0))
         {
            vol = (sbyte)(16 - vol);
         }

         if (macrostep > 0)
         {
            if (con == false)
            {
               if (m_envelope <= 7)
               {
                  vol = 0;
               }
            }
            else if ( hld == true  )
            {
               if ( m_envelope == 0x9 || m_envelope == 0xf  )
               {
                  vol = 0;
               }
               else if ( m_envelope == 0xb || m_envelope == 0xd )
               {
                  vol = 16;
               }
            }
         }         

         //m_sg.SetVolume((ushort)(vol * 127 / 16));

         return m_sg.GetValue();
         
      }
   }
}
