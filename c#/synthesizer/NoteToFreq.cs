﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
   public class NoteToFreq
   {
      static double[] Cps =                                  /* Default hertz table*/
		{
			8.176, 8.662, 9.177, 9.723, 10.301, 10.913,
			11.562, 12.250, 12.978, 13.750, 14.568, 15.534,
			16.352, 17.324, 18.354, 19.455, 20.602, 21.827,
			23.125, 24.500, 25.957, 27.500, 29.135, 30.868,
			32.703, 34.648, 36.708, 38.891, 41.203, 43.654,
			46.249, 48.999, 51.913, 55.000, 58.270, 61.735,
			65.406, 69.296, 73.416, 77.782, 82.407, 87.307,
			92.499, 97.999, 103.826, 110.000, 116.541, 123.471,
			130.813, 138.591, 146.832, 155.563, 164.814, 174.614,
			184.997, 195.998, 207.652, 220.000, 233.082, 246.942,
			261.626, 277.183, 293.665, 311.127, 329.628, 349.228,
			369.994, 391.995, 415.305, 440.000, 466.164, 493.883,
			523.251, 554.365, 587.330, 622.254, 659.255, 698.456,
			739.989, 783.991, 830.609, 880.000, 932.328, 987.767,
			1046.502, 1108.731, 1174.659, 1244.508, 1318.510, 1396.913,
			1479.978, 1567.982, 1661.219, 1760.000, 1864.655, 1975.533,
			2093.005, 2217.461, 2349.318, 2489.016, 2637.020, 2793.826,
			2959.955, 3135.963, 3322.438, 3520.000, 3729.310, 3951.066,
			4186.009, 4434.922, 4698.636, 4978.032, 5274.041, 5587.652,
			5919.911, 6271.927, 6644.875, 7040.000, 7458.620, 7902.133,
			8372.018, 8869.844, 9397.273, 9956.063, 10548.08, 11175.30,
			11839.82, 12543.85
		};

      static string [] notes = { "C","C#","D", "D#","E","G","G#","A","A#","B" };

      public double Translate(string str)
      {
         if (str.Length > 3 || str.Length<2)
         {
            return 0;
         }

         int octave = int.Parse(str[str.Length-1].ToString());

         if (octave >= 9)
            return 0;

         string note = str.Substring(0,str.Length-1);

         for(int i=0;i<notes.Length;i++)
         {
            if ( notes[i] == note )
            {
               return Cps[21 + 12*octave+ i];
            }
         }
         
         return 0;
      }
   }
}
