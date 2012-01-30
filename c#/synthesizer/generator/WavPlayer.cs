using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace midiplayer
{
   class WavPlayer : SoundGenerator
   {
      byte [] m_hdr;
      sbyte [] m_data;

      protected variable.Slider m_volume = new variable.Slider(0, 128, 128);

      public sbyte Output
      {
         get { return GetValue(); }
      }

      public WavPlayer(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_inputs["volume"] = m_volume;
         m_inputs["filename"] = new SetFilename( Load );
         m_inputs["reset"] = new variable.Set(ResetTime);
         m_output["output"] = Output;
      }

      public void ProcessChunk(BinaryReader br)
      {
         int chunkSize = 0;
         string chunkName = br.ReadChar().ToString() + br.ReadChar().ToString() + br.ReadChar().ToString() + br.ReadChar().ToString();

         if (chunkName == "RIFF")
         {
            chunkSize = br.ReadInt32();
            ProcessChunk(br);
         }
         else if (chunkName == "WAVE")
         {
            ProcessChunk(br);
            ProcessChunk(br);
         }
         else if (chunkName == "fmt ")
         {
            chunkSize = br.ReadInt32();

            int format = br.ReadInt16();
            int NumChannels = br.ReadInt16();
            m_sampleRate = br.ReadUInt32();
            int byteRate = br.ReadInt32();
            int blockAlign = br.ReadInt16();
            int bitsPerSample = br.ReadInt16();
         }
         else if (chunkName == "data")
         {
            chunkSize = br.ReadInt32();

            m_data = new sbyte[chunkSize];
            for (int i = 0; i < chunkSize; i++)
            {
               int v = ((int)(br.ReadByte())) - 128;
               m_data[i] = (sbyte)(v);
            }
         }
      }

      public void Load(string filename)
      {
         FileStream fs = File.OpenRead(filename);
         BinaryReader br = new BinaryReader(fs);


        ProcessChunk(br);

         br.Close();
         fs.Close();
      }


      override public sbyte GetValue()
      {
         if (m_data == null)
            return 0;

         if (m_time >= m_data.Length)
            return 0;

         sbyte val = m_data[m_time];
         if (m_time < m_data.Length)
         {
            m_time++;
         }

         return (sbyte)(((Int16)val * (Int16)m_volume.m_val) / 128);
      }

   }
}
