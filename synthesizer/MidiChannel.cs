using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
	class MidiChannel : MyStream
	{
		UInt32 m_NextNote;
		UInt32 m_time;
		bool gettime;
		SimpleOscillator m_SoundGen;

		public MidiChannel(byte [] str, SimpleOscillator sg)
			: base( str )
		{
			gettime = true;
			m_time = 0;
			m_NextNote = 0;
			m_SoundGen = sg;
		}

		public String DoTrack(int time)
		{
			String tmp = "";

			if ( EOS() )
				return "  -----  ";

			if (gettime)
			{
				for (; ; )
				{
					if ( EOS() )
						return "  -----  ";

					m_time = GetVariableLength();
					if (m_time == 0)
						DoTrack2();
					else
						break;

				}
				m_NextNote += m_time;
				gettime = false;
			}

			if (time > m_NextNote)
			{
				tmp += DoTrack2();
				gettime = true;
			}
			else
			{
				return "         ";
			}

			return tmp;
		}

		public String DoTrack2()
		{
			String tmp = "";

			Byte valch = GetByte();

			//Console.Write(" *{0,-3} ", m_time);

			if (valch == 0xff)
			{
				byte type = GetByte();
				byte typesize = GetByte();
				//Console.Write("{0} {1} ", valch, type);

				if (type == 0x58)
				{
					GetByte();
					GetByte();
					GetByte();
					GetByte();

				}
				else if (type == 0x47)
				{
					string str = GetString(typesize);
					//Console.Write("'{0}'", str);
				}
				else if (type == 0x3)
				{
					string str = GetString(typesize);
					//Console.Write("'{0}'", str);
				}
				else
				{
					GetString(typesize);
				}

			}
			else
			{
				Byte evento = (Byte)(valch >> 4);
				Byte channel = (Byte)(valch & 0xf);

				Byte param1 = GetByte();

				Byte param2 = 0;

				switch (evento)
				{
					case 0x8:
						param2 = GetByte();
						tmp = String.Format("Off {0,2} {1,2}", param1, param2);
						//m_SoundGen.SetVolume(0);
						return tmp;
					case 0x9:
						param2 = GetByte();
						tmp = String.Format("On  {0,2} {1,2}", param1, param2);
						m_SoundGen.ResetTime();
						//m_SoundGen.Se.SetFreqFromMidiNote( param1 );
						//m_SoundGen.SetVolume(param2);
						return tmp;
					case 0xa:
						//Console.Write("Aft {0} {1}", param1, param2);
						break;
					case 0xb:
						param2 = GetByte();
						//Console.Write("Ctr {0} {1}", param1, param2);
						break;
					case 0xc:
						//Console.Write("Chnl Aft {0}->{1}", channel + 1, param1);
						break;
					case 0xd:
						param2 = GetByte();
						//Console.Write("Pich Bnd", param1, param2);
						break;
					default:
						return String.Format("{0,2}  {1,2} {2,2}", evento, param1, param2);
				}

			}

			return "        ";

		}


	}
}

