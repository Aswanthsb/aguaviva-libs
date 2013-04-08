using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   class ModChannel : MyStream
   {
      List<SampleGenerator> m_Samples;
      SampleGenerator m_CurrentSample = null;
      UInt16 m_CurrentPeriod;
      UInt16 m_CurrentEffect;
      ModLoader m_ModGlobals;
      UInt16 m_Volume = 64;

      public ModChannel(byte[] str, ModLoader mod)
         : base(str)
      {
         m_ModGlobals = mod;
         m_Samples = m_ModGlobals.m_Samples;
      }

      string[] note = { "C-", "C#", "D-", "D#", "E-", "F-", "F#", "G-", "G#", "A-", "A#", "B-" };
      UInt16[] Octaves = { 1712,1616,1525,1440,1357,1281,1209,1141,1077,1017, 961, 907,
                           856, 808, 762, 720, 678, 640, 604, 570, 538, 508, 480, 453,
                           428, 404, 381, 360, 339, 320, 302, 285, 269, 254, 240, 226,
                           214, 202, 190, 180, 170, 160, 151, 143, 135, 127, 120, 113,
                           107, 101,  95,  90,  85,  80,  76,  71,  67,  64,  60,  57};

      public string PeriodToNote(UInt16 period)
      {
         for (int i = 0; i < Octaves.Length; i++)
         {
            if (Octaves[i] == period)
               return note[i % 12] + (2+i / 12).ToString();
         }
         return "caca";
      }


      public String DoTrack(int time, Int32[] outData)
      {
         string str="";

         UInt32 val = GetDoubleWord();
         if (val != 0)
         {
            UInt16 CurrentPeriod = (UInt16)((val & 0x0fff0000) >> 16);
            {
               if (CurrentPeriod != 0)
               {
                  m_CurrentPeriod = CurrentPeriod;
                  str += PeriodToNote( CurrentPeriod)+".";
               }
               else
               {
                  str += "....";
               }
            }
            
            byte instrument = (byte)(((val & 0xf0000000) >> 28) | ((val & 0x0000f000) >> 12));
            if (instrument != 0)
            {
               m_CurrentSample = m_Samples[instrument - 1];
               m_CurrentSample.SetTime(0);
               str += instrument.ToString("X2");
            }
            else
            {
               str += "..";
            }

            UInt16 CurrentEffect = (UInt16)(val & 0x00000fff);
            if (CurrentEffect != 0)
            {
               m_CurrentEffect = CurrentEffect;
               str += CurrentEffect.ToString("X3");
            }
            else
            {
               str += "...";
            }

         }
         else
         {
            str = "........."; 
         }

         if (m_CurrentSample == null)
            return str;

         m_CurrentSample.SetFreq( (UInt16)(2 * m_CurrentPeriod) );
         int tempo = outData.Length;

         int effectType = (m_CurrentEffect & 0xf00) >> 8;
         int x = (m_CurrentEffect & 0xf0) >> 4;
         int y = (m_CurrentEffect & 0xf);


         if (effectType == 99) //arpeggio
         {
            int div = tempo / 4;
            int i;
            for (i = 0; i < div; i++)
            {
               outData[i] += m_CurrentSample.GetValue();
            }

            for (; i < 2*div; i++)
            {
               m_CurrentSample.SetTime(0);
               m_CurrentSample.SetFreq((UInt16)(m_CurrentPeriod + x));
               outData[i] += m_CurrentSample.GetValue();
            }

            for (; i < 3 * div; i++)
            {
               m_CurrentSample.SetTime(0);
               m_CurrentSample.SetFreq((UInt16)(m_CurrentPeriod + y));
               outData[i] += m_CurrentSample.GetValue();
            }

            for (; i < tempo; i++)
            {
               m_CurrentSample.SetTime(0);
               m_CurrentSample.SetFreq(m_CurrentPeriod);
               outData[i] += m_CurrentSample.GetValue();
            }

         }
         else if (effectType == 99)  //sample offset
         {
            m_CurrentSample.SetTime((UInt32)(x * 4096 + y * 256));

            for (int i = 0; i < tempo; i++)
            {
               outData[i] += m_CurrentSample.GetValue();
            }
         }
         else if (effectType == 0xa)  //volumeslide
         {
            for (UInt16 i = 0; i < tempo; i++)
            {
               UInt16 tick = (UInt16)(6 * i / tempo);

               Int16 vol = 0;
               if (x > 0)
                  vol = (Int16)(m_Volume + tick * x);
               else
                  vol = (Int16)(m_Volume - tick * y);

               if (vol < 0) vol = 0;
               if (vol > 64) vol = 64;

               m_Volume = (UInt16)vol;


               m_CurrentSample.SetVolume((UInt16)vol);

               outData[i] += m_CurrentSample.GetValue();
            }

            if (x > 0)
               m_Volume = (UInt16)(m_Volume + 6 * x);
            else
               m_Volume = (UInt16)(m_Volume - 6 * y);

            m_CurrentEffect = (UInt16)(0xc00 + m_Volume);
            return str;
         }
         else if (effectType == 0xc) //volume
         {
            m_Volume = (UInt16)(x * 16 + y);
         }
         else if (effectType == 0xe) //volume
         {
            if (x == 0xd)
            {
               m_CurrentSample.SetVolume(m_Volume);
               for (int i = 0; i < tempo; i++)
               {
                  UInt16 tick = (UInt16)(6 * i / tempo);

                  if (tick >= y)
                  {
                     outData[i] += m_CurrentSample.GetValue();
                  }
               }
               return str;
            }
         }
         else if (effectType == 0xf)  //set speed
         {
            byte z = (byte)(x * 16 + y);

            if (z <= 32)
            {
               m_ModGlobals.m_ticksPerDivision = z;
            }
            else
            {
               m_ModGlobals.m_BeatsPerMinute = z;
            }
         }

         m_CurrentSample.SetVolume(m_Volume);
         for (int i = 0; i < tempo; i++)
         {
            outData[i] += m_CurrentSample.GetValue();
         }

         return str;
      }
   }
}
