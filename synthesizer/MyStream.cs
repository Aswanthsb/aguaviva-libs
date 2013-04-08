using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace midiplayer
{
	class MyStream
	{
		public byte [] data;
		public int pointer;

		public MyStream(byte[] str)
		{
			data = str;
			pointer = 0;
		}

		public byte GetByte()
		{
         if (pointer >= data.Length)
            pointer = 0;
			return data[pointer++];
		}

      public UInt16 GetWord()
      {
         return (UInt16)(((UInt16)(GetByte()) << 8) | (UInt16)GetByte());
      }

      public UInt32 GetDoubleWord()
      {
         return (UInt32)( ( GetWord() << 16 ) | ( GetWord() ));
      }


		public String GetString(int length)
		{
			string tmp = "";
			for (int i = 0; i < length;i++ )
				tmp += (char)data[pointer++];
			return tmp;
		}

		public UInt32 GetVariableLength()
		{
			UInt32 res = 0;
			UInt32 size = 0;
			while (true)
			{
				size++;
				res <<= 7;
				Byte b = GetByte();
				if ((b & 128) > 0)
				{
					res |= ((UInt32)b & 0x7f);
				}
				else
				{
					res |= b;
					break;
				}

			};

			return res;
		}

		public void Reset()
		{
			pointer = 0;
		}

		public bool EOS()
		{
			return (pointer >= data.Length);
		}
	}
}
