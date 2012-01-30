using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace midiplayer
{
   class PSGLoader 
   {
		public int m_time;
      
      protected UInt16 m_freq;
      virtual public void SetFreq(UInt16 per)
      {
         m_freq = per;
      }

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


//		List<byte> channelList = new List<byte>();

      List<EnvelopeGenerator> EnvelopeGen = new List<EnvelopeGenerator>();
      List<SimplePSG> PsgGenList = new List<SimplePSG>();
      List<SoundGenerator> SoundGenList = new List<SoundGenerator>();

      Mixer mixer;
      LowPass lp;

      byte [] song;

      public void TestEnvelopes()
      {
         EnvelopeGenerator[] leeg = new EnvelopeGenerator[16];
         for (byte i = 0; i < leeg.Length; i++)
         {
            leeg[i] = new EnvelopeGenerator("env",100);
            leeg[i].SetFreq(1);
            leeg[i].SetEnvelope(i);
         }

         for (int i = 0; i < 100; i++)
         {
            Console.Write(i.ToString("D3") + "     ");
            for (int j = 0; j < leeg.Length; j++)
            {

               Console.Write(leeg[j].GetValue().ToString("D2") + "   ");
            }
            Console.WriteLine();
         }
      }

		public void Load(string filename )
		{
			FileStream fs = File.OpenRead(filename);
			BinaryReader br = new BinaryReader(fs);

         string name = GetString(br, 16);

         song = br.ReadBytes(100000 * 14);

         for (int i = 0; i < 3; i++)
         {
            SimplePSG sp = new SimplePSG("Channel"+i.ToString() ,44100);
            EnvelopeGenerator eg = new EnvelopeGenerator("Env"+i.ToString(), 44100);

            eg.SetSoundGenerator(sp);

            PsgGenList.Add(sp);
            EnvelopeGen.Add(eg);

            SoundGenList.Add(eg);
         }
         //SoundGenList.Add(new NoiseGenerator(44100));
         /*
         mixer = new Mixer("Mixer", 44100);
         mixer.SetInputs(SoundGen);

         lp = new LowPass("LowPass", 44100);
         lp.SetFreq(400);
         lp.SetInput(mixer);
         */
         br.Close();
			fs.Close();
         Reset();
		}

		public void Reset()
		{
			m_time = 0;
		}

      byte GetReg(int i)
      {
         if (m_time + i * 2 + 1 > song.Length)
            m_time = 0;

         return song[m_time + i*2 +1];
      }

      bool b = true;
		public String Play( Player pl )
      {

         return "";
      }

      string cacas(Player pl)
      {
         String tmp = m_time.ToString("D5");
/*
         if (b)
         {
            PsgGenList[0].SetFreq(1000);
            EnvelopeGen[0].SetEnvelope(0xc);
            EnvelopeGen[0].SetFreq(1);
            b = false;
         }
         pl.RenderSound(mixer);

         return tmp;
*/
         /*
         {
            ushort f = GetReg(6);
            if (f != 0)
            {
               f = (ushort)((3579545 / 32) / f);
            }
            SoundGenList[3].SetFreq(f);


            if ((GetReg(7) >> 3) > 0)
            {
               //SoundGenList[i].Enable((GetReg(7) & 1 << i) == 0);
               SoundGenList[3].SetVolume(10);
            }

            tmp += String.Format(" | {0,4} {1,2}", f, 10);
         }
          */
         /*
         // Set envelope
         ushort envelopeFreq = 0;
         {
            byte fl = GetReg(0xb);
            byte fh = GetReg(0xc);
            envelopeFreq = (ushort)(fh * 256 + fl);

            if (envelopeFreq != 0)
            {
               envelopeFreq = (ushort)((3579545 / 32) / envelopeFreq);
            }
         }

         for (int i = 0; i < 3; i++)
         {
            byte fl = GetReg(2 * i);
            byte fh = GetReg(2 * i + 1);
            ushort freq = (ushort)(fh * 256 + fl);

            if (freq != 0)
            {
               freq = (ushort)((3579545 / 32) / freq);
            }

            byte vol = GetReg(8 + i);

            PsgGenList[i].SetFreq(freq);

            tmp += String.Format(" | {0,4} {1,2}", freq, vol);

            if (vol == 16)
            {
               EnvelopeGen[i].SetEnvelope(GetReg(0xd));
               EnvelopeGen[i].SetFreq(envelopeFreq);
            }
            else
            {
               PsgGenList[i].SetVolume((ushort)(vol * 8));
               EnvelopeGen[i].SetEnvelope(0xff);               
            }

            PsgGenList[i].Enable(true);//(GetReg(7) & 1 << i) == 0);
         }

         m_time += 14 * 2 + 1;

         */
         
         pl.RenderSound(mixer);

         return tmp;
      }
	}
}
