using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



 //s ound effect
 //02 02 8D 7F68 03 0C 0C 03 00 01 00 01 01  
 //02 02 8D 7F84 02 0C 0C 03 00 01 00 01 01  
 //00 01 00 7D3A 00 00 00 00 00 00 00 00 00  
 //50 03 A8 01 00 00 00 B8 0C 0C 00 00 00 00 


namespace midiplayer
{
   class Channel
   {
      public byte ix_0;
      public byte ix_1;
      public byte song;
      public int ix_3_4;
      public byte ix_5;
      public byte ix_6;
      public byte volume;
      public byte ix_8;
      public byte ix_9;
      public byte ix_a;
      public byte ix_b;
      public byte ix_c;
      public byte ix_d;
   };
   
   class KonamiLoader
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


      void Mixer( byte c, byte d )
      {
	      //Mixer:   ld     a,c        
	      //7B22:   cp     #05        
	      //7B24:   ret    nz 
          if ( c == 5 )
	      {        
		      //7B25:   dec    d          
		      //7B26:   jr     z,#7b2c    
		      //7B28:   ld     a,#9c      
		      //7B2A:   jr     #7b2e      
		      //7B2C:   ld     a,#b8      
		      //7B2E:   ld     (#e03a),a  ;   set mixer control
		      //7B31:   ld     e,a        
		      //7B32:   ld     a,#07      
		      //7B34:   jp     #_WRTPSG     

		      if ( d == 0 )
		      {
			      //mute tone just noise
               writePSG(7, 0x9c); 
		      }
		      else
		      {
			      // 
               writePSG(7, 0xb8); 
		      }
       
	      }	
      }
      /*
      7B37:   ld     a,(#e03a)  
      7B3A:   call   #7b2e      
      */
      


      Channel [] channel = new Channel[3];

      Channel ch = null;
      byte [] mem;

      void UpdateMusic()
      {
	      c = 1;

	      for (byte b = 0;b<3;b++)
	      {
            ch = channel[b];
      		
		      byte a = ch.song;
     		
		      if ( a == 1 )
		      {
			      //RoutineA(  );
		      } 
            else if ( a != 0 )
		      {
			      RoutineB(  );
		      }

		      c +=2;
	      }
      } 

      byte[] psgreg;

      void writePSG( byte reg, byte val )
      {
         psgreg[reg] = val;
      }

      void write_PSG_val2( byte reg, byte v1, byte v2 )
      {
         writePSG(reg, v1);
         writePSG((byte)(reg - 1), v2);
      }

      byte a, b, c, d, e;
      int hl;

      void RoutineB(  )
      {
      init:

         if ( (a & (1<<6)) == 0)
	      {
		      Mixer( c, 1 );
	      }
       
	      if ( (sbyte)ch.song >= 0 )
	      {    
		      ch.ix_0--;
            if (ch.ix_0 == 0)
            {
               if (asm_7ba4_7c2a() == true)
               {
                  goto init;
               }
               else
                  return;
            }
         }
         else
         {
            //7c2d
            ch.ix_0--;

            if (ch.ix_0 == 0)
            {
               if (asm_7ba4_7c2a())
                  goto init;
               else
                  return;
               //writePSG((byte)((c >> 1) + 0x88), e);
            }
            
            ch.ix_8--;

      	   //7c36
            if (ch.ix_8 == ch.ix_0)
            {
               //7c3e
               if (ch.ix_8 > ch.ix_d)
               {
                  return;
               }
            }
            else
            {
               //7c47
               ch.ix_8--;
            }

            //7c4a
            if (ch.volume > 1)
            {
               ch.volume--;
               //7c53
               writePSG((byte)((c >> 1) + 0x8), ch.volume);
            }
         }

		
	      return;
      }

      void asm_7af7_7b1b()
      {
         hl++;
         a = (byte)(ch.ix_9 + 1);

         if (a == mem[hl])
         {
            hl += 2;
            ch.ix_9 = 0;
            hl++; ch.ix_3_4 = hl;
         }
         else
         {
            if (a > (sbyte)(mem[hl]))
            {
               a--;
            }
            ch.ix_9 = a;

            hl++;
            ch.ix_3_4 = mem[hl] ; //ok?????????????????????????????????
            hl++;
            ch.ix_3_4 += mem[hl]*256;
            ch.ix_3_4 -= 0x4000;
         }

         ch.ix_0++;
      }

      bool asm_7ba4_7c2a()
      {
         hl = ch.ix_3_4; //0x7e0b
         a = mem[hl];  // music command

         if (a == 0xfe)
         {
            asm_7af7_7b1b();

            //goto init
            return true;
         }

         if (a >= 0xfe)
         {
            asm_7c1a_7c27();
            //7c53
            writePSG((byte)((c >> 1) + 8), 0);
            //inc channel and stuff
         }
         else
         {
            asm_7bb2_7c15(); 
         }

         e = 0;
         return false;
      }

      void asm_7bc0_7bc6()
      {
         ch.ix_1 = (byte)(a & 0x0f);

         hl++;
         a = mem[hl];
      }

      void asm_7bce_7be9()
      {
         e = (byte)(mem[hl] & 0x1f);
         hl++;

         b = mem[hl];

         

         if ( (mem[hl] & (1<<4)) == 0 )
         {
            e-=10;
         }

         b &= (byte)(0xff^(1<<4));

         hl--;

         writePSG(6, e);  //noise period   				

         Mixer( c, 0 );

         hl++;      				
      }

      void asm_7bf0_7bf4()
      {
         a = mem[hl];

         hl++; ch.ix_3_4 = hl;
         a = b;
      }

      void asm_7bf7_7c15()
      {
         d = (byte)((mem[hl] & 0xf0) ^ mem[hl]);
         hl++;
         e = mem[hl];

         hl++; ch.ix_3_4 = hl;

         write_PSG_val2(c, d, e);

         a = (byte)((mem[hl] & 0xf0) >> 4);

         asm_7c0b_7c15();

      }

      void asm_7bb2_7c15()
      {
         if ((ch.song & (1 << 7)) == 0)
         {
            //7bb9
            if ((a & 0xf0) == 0x20)
            {
               asm_7bc0_7bc6();
            }

            //7bc7
            b = a;

            //7bc8
            if ((mem[hl] & 0xf) == 0x10)
            {
               //noise period
               asm_7bce_7be9(); 
            }

            //7bea
            if ((ch.song & (1 << 6)) == 0)
            {               
               asm_7bf7_7c15();
               // 7c53
               writePSG((byte)((c >> 1) + 8), (byte)(b>>4));
               return;

            }
            else
            {
               asm_7bf0_7bf4();
            }

         }
         //7c5b
         asm_7c5b_();
      }

      void asm_7c0b_7c15()
      {
         ch.ix_0 = ch.ix_1;
         ch.ix_8 = (byte)(ch.ix_c + ch.ix_1);
      }

      void asm_7c1a_7c27()
      {
         ch.ix_9 = 0;
         ch.ix_b = 0;
         Mixer(c, 1);
         ch.song = 0;
      }

      void asm_7c6e_7c7e()
      {
         ch.ix_6 = (byte)(mem[hl] & 0xf);
         hl++;

         ch.ix_c = mem[hl];
         hl++;

         ch.ix_d = mem[hl];
         hl++;
      }

      void asm_7c8f_7cd9()
      {
         ch.ix_5 = a;
         hl++;
      }

      void asm_7c5b_()
      {
      more:
         a = mem[hl];
	      if ( (mem[hl] & 0xf0) == 0xd0 )
	      {      		
		      ch.ix_a = (byte)(mem[hl] & 0xf);      		
		      hl++;
	      }
	      if ( mem[hl] >= 0xf0  )
	      {
            asm_7c6e_7c7e();
	      }
	      if ( mem[hl] >= 0xe0 )
	      {
		      if ( (byte)((mem[hl] & 0xf) & (1<<3)) != 0 )
		      { 

               ch.ix_b = (byte)(mem[hl] & 0xf);
               hl++;
      			
			      //7C8D:   jr     #7c5b      
               goto more;
		      }
            //7c8f
            ch.ix_5 = (byte)(mem[hl] & 0xf);
            hl++;
            

	      }

         //7c94
         b = (byte)(mem[hl] & 0xf);

         a = ch.ix_a;
         if (b != 0)
         {
            a += (byte)((b) * a);
         }
         //7ca1
         ch.ix_1 = a;
         a = mem[hl];
         hl++; ch.ix_3_4 = hl;

         a = (byte)(((a & 0xf0) >> 4));
         
         b = a;

         if (a != 0xc)
         {
            ch.volume = ch.ix_6;
         }
         else
         {
            ch.volume = 0;
         }

         //7cb9
         asm_7c0b_7c15();

         //7c53
         writePSG((byte)((c >> 1) + 8), ch.volume);

         //7CBC:   ld     a,b        
         //7CBD:   ld     hl,#7cea   
         //7CC0:   call   #4010	
         //7CC3:   ld     l,(hl)     
         //7CC4:   ld     h,#00      

         hl = mem[0x3cea + b];



         if (ch.ix_5 != 0)
         {
            hl <<= (ch.ix_5);
         }

         if ((ch.ix_b != 0))
         {
            hl++;
         }

         write_PSG_val2(c, (byte)((hl & 0xff00) >> 8), (byte)((hl & 0xff)));
      }

      //20
      void SetSong(byte song)
      {
         byte ss = (byte)(song & 0x3f);

         int ini = 0;
         int cnt = 2;

         if (ss < 0xb)
         {
            cnt = 1;
            ini = 2;
         }
         else if (ss < 0x11)
         {
            ini = 0;
            cnt = 2;
         }
         else
         {
            cnt = 3;
            ini = 0;
         }


         hl = 0xe02e - 0x4000;
/*         
         if ((byte)(mem[ hl] & 0x3f) > (byte)(song & 0x3f))
         {
            return;
         }
 */        

         int de = (byte)(ss * 2) + (0x7cf4 - 0x4000);

         hl--;

         //7adc
         for (int i = ini; i < ini + cnt; i++)
         {
            channel[i].ix_0 = 1;
            channel[i].ix_1 = 1;
            channel[i].song = song;
            channel[i].ix_3_4 = (mem[de+1] * 256 + mem[de ]) -0x4000;
            de += 2;
            channel[i].ix_9 = 0;
         }

         //UpdateMusic();
      }

      //		List<byte> channelList = new List<byte>();

      List<EnvelopeGenerator> EnvelopeGen = new List<EnvelopeGenerator>();
      List<SimplePSG> PsgGenList = new List<SimplePSG>();
      List<SoundGenerator> SoundGenList = new List<SoundGenerator>();

      Mixer mixer;
      LowPass lp;
      Resample rs;
      FrequencyMeter fm;
      WavPlayer wp;
      EnvelopeExtractor ee;
      Multiply mm;

      byte[] song;


      public string DumpStructs()
      {
         string str = "";

         for (int i = 0; i < 3; i++)
         {
            str += channel[i].ix_0.ToString("x2") + " ";
            str += channel[i].ix_1.ToString("x2") + " ";
            str += channel[i].song.ToString("x2") + " ";
            str += (channel[i].ix_3_4 + 0x4000).ToString("x4") + " ";
            str += channel[i].ix_5.ToString("x2") + " ";
            str += channel[i].ix_6.ToString("x2") + " ";
            str += channel[i].volume.ToString("x2") + " ";
            str += channel[i].ix_8.ToString("x2") + " ";
            str += channel[i].ix_9.ToString("x2") + " ";
            str += channel[i].ix_a.ToString("x2") + " ";
            str += channel[i].ix_b.ToString("x2") + " ";
            str += channel[i].ix_c.ToString("x2") + " ";
            str += channel[i].ix_d.ToString("x2") + " ";
            str += " |  ";
         }
         return str;
      }
      
      public void Load(string filename)
      {
         /*
         EnvelopeGenerator [] leeg = new EnvelopeGenerator[16];
         for (byte i = 0; i < leeg.Length; i++)
         {
            leeg[i] = new EnvelopeGenerator(100);
            leeg[i].SetFreq(1);
            leeg[i].SetEnvelope(i);
         }

         for (int i = 0; i < 100; i++)
         {
            Console.Write(i.ToString("D3") + "     ");
            for (int j = 0; j < leeg.Length; j++)
            {
               
               Console.Write(leeg[j].GetValue().ToString("D2")+"   ");
            }
            Console.WriteLine();
         }
         */

         FileStream fs = File.OpenRead(filename);
         BinaryReader br = new BinaryReader(fs);

         mem = br.ReadBytes(100000 * 14);

         psgreg = new byte[16];
         channel[0] = new Channel();
         channel[1] = new Channel();
         channel[2] = new Channel();

         //87
         //20 mute
         //87 aparece enemigo
         //8b ingame music
         //8d abre puerta
         //91 fanfare
         //94 musica final
         //97 intro music
         //9a final game
         //1d muerte

         SetSong(0x20);

         for (int i = 0; i < 3; i++)
         {
            SimplePSG sp = new SimplePSG("Channel" + i.ToString(), 44100);
            EnvelopeGenerator eg = new EnvelopeGenerator("Env" + i.ToString(), 44100);

            eg.SetSoundGenerator(sp);

            PsgGenList.Add(sp);
            EnvelopeGen.Add(eg);

            SoundGenList.Add(eg);
         }
         //SoundGenList.Add(new NoiseGenerator(44100));
         /*
         mixer = new Mixer("Mixer", 44100);
         mixer.SetInputs(SoundGenList);

         lp = new LowPass("LowPass", 44100);
         lp.SetFreq(400);
         lp.SetInput(mixer);
         */
         {
            SimpleOscillator so1 = new SimpleOscillator("simple", 44100 );
            SimpleOscillator so2 = new SimpleOscillator("simple", 44100 );
            
            wp = new WavPlayer("pp", 12000);
            wp.Load("formant.wav");            

            rs = new Resample("Resample", 44100);
            rs.SetInput( wp );
            /*
            ee = new EnvelopeExtractor("ff", 44100);
            ee.SetInput(rs);
            */
            //mm = new Multiply("mm", 44100);
            //mm.SetInput1(rs);
            //mm.SetInput2(so1);
            //fm = new FrequencyMeter("fr", 44100);
            //fm.SetInput(rs);

         }

         br.Close();
         fs.Close();
         Reset();
      }

      public sbyte GetValue()
      {
         return rs.GetValue();
      }


      public void Reset()
      {
         m_time = 0;
      }

      byte GetReg(int i)
      {
         //if (m_time + i * 2 + 1 > song.Length)
         //   m_time = 0;

         return psgreg[i];
      }

      


      public String Play(Player pl)
      {

         String tmp = (m_time ).ToString("X4");
         
         bool dumpregs = !true;

         if (m_time == 0x2)
         {
            SetSong(0x97);
         }
         if (m_time == 0x15c)
         {
            SetSong(0x8b);
         }
         if (m_time == 0x17c)
         {
            SetSong(0x87);
         }
         if (m_time == 0x502)
         {
            SetSong(0x1d);
         }
         if (m_time == 0x5d4)
         {
            SetSong(0x8d);
         }
         if (m_time == 0x52A)
         {
            Console.Write("caca");
         }

         if (dumpregs == true)
         {
            Console.Write(tmp + ": " + DumpStructs());

         }
         UpdateMusic();
         if ( dumpregs == true )
         {         
            for (int i = 0; i < 13; i++)
               Console.Write(psgreg[i].ToString("x2") + " ");

            Console.WriteLine();
         }
         

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

            PsgGenList[i].Enable((GetReg(7) & 1 << i) == 0);
         }
         */
         m_time += 1;



         //pl.RenderSound(mixer);
         pl.RenderSound(rs);

         return tmp;
      }

   }
}
