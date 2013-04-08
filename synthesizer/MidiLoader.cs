using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace midiplayer
{
	class MidiLoader
	{
		public int m_time;

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

		static UInt32 GetUInt32(BinaryReader br)
		{
			return GetUIntN(br, 4);
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


		static public List<MidiChannel> channelList = new List<MidiChannel>();
		static public List<SimpleOscillator> SimpleGenList = new List<SimpleOscillator>();
      static public List<SoundGenerator> SoundGenList = new List<SoundGenerator>();

		public void Load(string filename )
		{
			FileStream fs = File.OpenRead(filename);
			BinaryReader br = new BinaryReader(fs);

			UInt16 tracks=0;
			for (int i=0;;i++)
			{
				string name = GetString(br, 4);
				UInt32 size = GetUInt32(br);

				if (name == "MThd")
				{
					UInt16 format = GetUInt16(br);
					tracks = GetUInt16(br);
					UInt16 time = GetUInt16(br);
				}
				else if (name == "MTrk")
				{
					SimpleOscillator sg = new SimpleOscillator("simple"+i.ToString(),44100);
					SoundGenList.Add(sg);
					channelList.Add(new MidiChannel(br.ReadBytes((int)size), sg));
					tracks--;
					if (tracks == 0)
						break;
				}
			}
			
			br.Close();
			fs.Close();

			Reset();
		}



		public void Reset()
		{
			m_time = 0;

			for (int j = 0; j < channelList.Count; j++)
			{
				channelList[j].Reset();
			}
		}

		public String Play( Player pl )
		{
			String tmp="";

         tmp += String.Format("{0,-5} ", m_time);

         for (int j = 0; j < channelList.Count; j++)
         {
            tmp += String.Format(" | ");
            tmp += channelList[j].DoTrack(m_time);
         }
         m_time += 32;

         //pl.RenderSound(SoundGenList);

			return tmp;
		}
	}
}
