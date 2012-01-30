using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace midiplayer
{
   class ModLoader
   {
      public List<SampleGenerator> m_Samples = new List<SampleGenerator>();
      List<Pattern> m_Patterns = new List<Pattern>();
      int m_currentPattern;
      int m_currentRow;
      Int32[] m_outData = new Int32[44100/(8)];
      byte[] m_PatternTable;

      public byte m_BeatsPerMinute = 125;
      public byte m_ticksPerDivision = 6;

      static UInt32 GetUIntN(BinaryReader br, uint size)
      {
         UInt32 v = 0;
         for (int i = 0; i < size; i++)
         {
            v <<= 8;
            v |= br.ReadByte();
         }

         return v;
      }

      static UInt16 GetUInt16(BinaryReader br)
      {
         return (UInt16)GetUIntN(br, 2);
      }


      static string GetString(BinaryReader br, UInt32 n)
      {
         byte[] hdr = br.ReadBytes((int)n);
         string tmp = "";
         foreach (char i in hdr)
            tmp += i;
         return tmp;
      }

      static sbyte [] ReadSBytes(BinaryReader br, UInt32 n)
      {
         sbyte[] hdr = new sbyte[n];
         for (int i = 0; i < n;i++ )
         {
            hdr[i] = br.ReadSByte();
         }
            
         return hdr;
      }


      public void LoadMod(string filename)
      {
         FileStream fs = File.OpenRead(filename);
         BinaryReader br = new BinaryReader(fs);

         string name = GetString(br, 20);

         for (int i = 0; i < 31; i++)
         {
            string samplename = GetString(br, 22);

            SampleGenerator sam = new SampleGenerator(samplename, 44100);

            
            UInt16 length = (UInt16) ( GetUInt16(br) * 2 );
            sbyte fintune = br.ReadSByte();
            byte volume = br.ReadByte();
            UInt16 StartRepeat = (UInt16)( GetUInt16(br) * 2 );
            UInt16 RepeatLength = (UInt16)( GetUInt16(br) * 2 );

            sam.m_Length = length;
            sam.m_Repeat = StartRepeat;
            sam.m_RepeatLength = RepeatLength;
            
            m_Samples.Add(sam);

            Console.WriteLine(samplename);
         }

         byte SongPositions = br.ReadByte();
         byte SetTo127 = br.ReadByte();

         m_PatternTable = br.ReadBytes(128);

         byte numberOfPatterns = 0;
         for (int i = 0; i < m_PatternTable.Length; i++)
         {
            if (m_PatternTable[i] > numberOfPatterns)
               numberOfPatterns = m_PatternTable[i];
         }
         numberOfPatterns++;

         m_currentPattern = 0;

         string nameDoc = GetString(br, 4);

         for(int i=0;i<numberOfPatterns;i++)
         {
            Pattern pat= new Pattern();

            byte[] data = br.ReadBytes(1024);

            for (int channel = 0; channel < 4; channel++)
            {
               pat.m_Channels.Add( new ModChannel( new byte[64*4] , this ) );
            }


            for (int channel = 0; channel < 4; channel++)
            {            
               for (int j = 0; j < 64; j++)
               {
                  for (int ii = 0; ii < 4; ii++) 
                  {
                     pat.m_Channels[channel].data[j*4 + ii] = data[channel *4 + j*4*4 + ii];
                  }
               }
            }

            m_Patterns.Add(pat);
         }

         for (int i = 0; i < 31; i++)
         {

            m_Samples[i].m_sampleData = ReadSBytes(br, m_Samples[i].m_Length);
         }


         

         br.Close();
         fs.Close();
         
         //Reset();
      }

      public String PlaySample(Player pl, UInt16 period, int sample)
      {

         string str = pl.FreeInBuffer() + "   " + m_outData.Length+"  "+m_Samples[sample].m_Length.ToString() + ":";

         int chunks = 0;

         m_Samples[sample].SetFreq(period);

         while ( pl.FreeInBuffer() > m_outData.Length)
         {
            str += m_Samples[sample].m_time.ToString() + ",";

            Console.Write(pl.FreeInBuffer() + " , " );

            for (int i = 0; i < m_outData.Length; i++)
            {
               m_outData[i] = 128 + m_Samples[sample].GetValue();
            }


            pl.RenderSound(m_outData, 1);
            chunks++;
         }

         Console.WriteLine(chunks + "  "+str);

         return str;
      }

      public bool CanRender(Player pl)
      {
         return pl.FreeInBuffer() > m_outData.Length;
      }

      public String Play(Player pl)
      {
         //return PlaySample(pl, 428, 3);

         string str = "";

         str+= m_currentRow.ToString("X2") + ":";
         
         for (int i = 0; i < m_outData.Length; i++)
         {
            m_outData[i] = 0;
         }
          

         m_currentRow++;

         if (m_currentRow < 64)
         {
            str += m_Patterns[m_PatternTable[m_currentPattern]].Play(m_outData);
         }
         else
         {
            m_currentRow = 0;
            m_currentPattern++;
         }

         for (int i = 0; i < m_outData.Length; i++)
         {
            m_outData[i] += 4*128;
         }


         str += "\n";
         
         pl.RenderSound(m_outData, 4);

         return str;
      }

   }
}
