using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace midiplayer
{/*
	class WavPlayer : Player
	{
		BinaryWriter m_br;
		FileStream m_fs;
		string m_name;

		public int Init( String name )
		{
			m_name = name;
			return 0;
		}

		override public bool CanRender()
		{
			return true;
		}

		override public int BufferDepth()
		{
			return 2000;
		}

		override public int RenderSound(List<SimpleOscillator> SoundGenList)
		{
			for (int i = 0; i < 44100 / 20; i++)
			{
				UInt32 res = 0;
				for (int s = 1; s < SoundGenList.Count; s++)
				{
					res += SoundGenList[s].GetValue();
				}

				byte val = (byte)(res / SoundGenList.Count-1);

				m_br.Write(val);
			}

			return 0;
		}
   */
//		override public void SetTempo(UInt32 tempo) { /*m_tempo = tempo;*/ }
/*
		override public int Play()
		{
			m_fs = File.Create(m_name);
			m_br = new BinaryWriter(m_fs);

			return 0;
		}

		override public int Stop()
		{
			m_br.Close();
			m_fs.Close();

			return 0;
		}

	}
*/
}
