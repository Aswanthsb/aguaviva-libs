using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
	abstract class Player
	{
		
		abstract public bool CanRender();
		abstract public int RenderSound(SoundGenerator SoundGenList);
      abstract public int RenderSound(Int32 [] data, Int32 division);

		abstract public int Play();
		abstract public int Stop();
      abstract public int FreeInBuffer();
	}
}
