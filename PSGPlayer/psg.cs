using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PSGPlayer
{

   enum Registers
   {
      AY_AFINE	   = (0),
      AY_ACOARSE	= (1),
      AY_BFINE	   = (2),
      AY_BCOARSE	= (3),
      AY_CFINE	   = (4),
      AY_CCOARSE	= (5),
      AY_NOISEPER	= (6),
      AY_ENABLE	= (7),
      AY_AVOL		= (8),
      AY_BVOL		= (9),
      AY_CVOL		= (10),
      AY_EFINE	   = (11),
      AY_ECOARSE	= (12),
      AY_ESHAPE	= (13),
   }
   
   class AY8910
   {
      const int MAX_OUTPUT = 0x7fff;

      const int STEP = 0x8000;

      int Channel;
	   int SampleRate;
	   int register_latch;
	   byte [] Regs = new byte(16);
	   uint UpdateStep;
	   int PeriodA,PeriodB,PeriodC,PeriodN,PeriodE;
	   int CountA,CountB,CountC,CountN,CountE;
	   uint VolA,VolB,VolC,VolE;
	   byte EnvelopeA,EnvelopeB,EnvelopeC;
	   byte OutputA,OutputB,OutputC,OutputN;
	   sbyte CountEnv;
	   byte Hold,Alternate,Attack,Holding;
	   int RNG;
	   uint [] VolTable = new byte(32);

      void _AYWriteReg(int r, int v)
      {
	      int old;

	      Regs[r] = v;

	      /* A note about the period of tones, noise and envelope: for speed reasons,*/
	      /* we count down from the period to 0, but careful studies of the chip     */
	      /* output prove that it instead counts up from 0 until the counter becomes */
	      /* greater or equal to the period. This is an important difference when the*/
	      /* program is rapidly changing the period to modulate the sound.           */
	      /* To compensate for the difference, when the period is changed we adjust  */
	      /* our internal counter.                                                   */
	      /* Also, note that period = 0 is the same as period = 1. This is mentioned */
	      /* in the YM2203 data sheets. However, this does NOT apply to the Envelope */
	      /* period. In that case, period = 0 is half as period = 1. */
	      switch( r )
	      {
	      case AY_AFINE:
	      case AY_ACOARSE:
		      Regs[AY_ACOARSE] &= 0x0f;
		      old = PeriodA;
		      PeriodA = (Regs[AY_AFINE] + 256 * Regs[AY_ACOARSE]) * UpdateStep;
		      if (PeriodA == 0) PeriodA = UpdateStep;
		      CountA += PeriodA - old;
		      if (CountA <= 0) CountA = 1;
		      break;
	      case AY_BFINE:
	      case AY_BCOARSE:
		      Regs[AY_BCOARSE] &= 0x0f;
		      old = PeriodB;
		      PeriodB = (Regs[AY_BFINE] + 256 * Regs[AY_BCOARSE]) * UpdateStep;
		      if (PeriodB == 0) PeriodB = UpdateStep;
		      CountB += PeriodB - old;
		      if (CountB <= 0) CountB = 1;
		      break;
	      case AY_CFINE:
	      case AY_CCOARSE:
		      Regs[AY_CCOARSE] &= 0x0f;
		      old = PeriodC;
		      PeriodC = (Regs[AY_CFINE] + 256 * Regs[AY_CCOARSE]) * UpdateStep;
		      if (PeriodC == 0) PeriodC = UpdateStep;
		      CountC += PeriodC - old;
		      if (CountC <= 0) CountC = 1;
		      break;
	      case AY_NOISEPER:
		      Regs[AY_NOISEPER] &= 0x1f;
		      old = PeriodN;
		      PeriodN = Regs[AY_NOISEPER] * UpdateStep;
		      if (PeriodN == 0) PeriodN = UpdateStep;
		      CountN += PeriodN - old;
		      if (CountN <= 0) CountN = 1;
		      break;
	      case AY_AVOL:
		      Regs[AY_AVOL] &= 0x1f;
		      EnvelopeA = Regs[AY_AVOL] & 0x10;
		      VolA = EnvelopeA ? VolE : VolTable[Regs[AY_AVOL] ? Regs[AY_AVOL]*2+1 : 0];
		      break;
	      case AY_BVOL:
		      Regs[AY_BVOL] &= 0x1f;
		      EnvelopeB = Regs[AY_BVOL] & 0x10;
		      VolB = EnvelopeB ? VolE : VolTable[Regs[AY_BVOL] ? Regs[AY_BVOL]*2+1 : 0];
		      break;
	      case AY_CVOL:
		      Regs[AY_CVOL] &= 0x1f;
		      EnvelopeC = Regs[AY_CVOL] & 0x10;
		      VolC = EnvelopeC ? VolE : VolTable[Regs[AY_CVOL] ? Regs[AY_CVOL]*2+1 : 0];
		      break;
	      case AY_EFINE:
	      case AY_ECOARSE:
		      old = PeriodE;
		      PeriodE = ((Regs[AY_EFINE] + 256 * Regs[AY_ECOARSE])) * UpdateStep;
		      if (PeriodE == 0) PeriodE = UpdateStep / 2;
		      CountE += PeriodE - old;
		      if (CountE <= 0) CountE = 1;
		      break;
	      case AY_ESHAPE:
		      /* envelope shapes:
		      C AtAlH
		      0 0 x x  \___

		      0 1 x x  /___

		      1 0 0 0  \\\\

		      1 0 0 1  \___

		      1 0 1 0  \/\/
		                ___
		      1 0 1 1  \

		      1 1 0 0  ////
		                ___
		      1 1 0 1  /

		      1 1 1 0  /\/\

		      1 1 1 1  /___

		      The envelope counter on the AY-3-8910 has 16 steps. On the YM2149 it
		      has twice the steps, happening twice as fast. Since the end result is
		      just a smoother curve, we always use the YM2149 behaviour.
		      */
		      Regs[AY_ESHAPE] &= 0x0f;
		      Attack = (Regs[AY_ESHAPE] & 0x04) ? 0x1f : 0x00;
		      if ((Regs[AY_ESHAPE] & 0x08) == 0)
		      {
			      /* if Continue = 0, map the shape to the equivalent one which has Continue = 1 */
			      Hold = 1;
			      Alternate = Attack;
		      }
		      else
		      {
			      Hold = Regs[AY_ESHAPE] & 0x01;
			      Alternate = Regs[AY_ESHAPE] & 0x02;
		      }
		      CountE = PeriodE;
		      CountEnv = 0x1f;
		      Holding = 0;
		      VolE = VolTable[CountEnv ^ Attack];
		      if (EnvelopeA) VolA = VolE;
		      if (EnvelopeB) VolB = VolE;
		      if (EnvelopeC) VolC = VolE;
		      break;
	      }
      }

      /* write a register on AY8910 chip number 'n' */
      void AYWriteReg(int r, int v)
      {

	      if (r > 15) return;

         if (r < 14)
	      {
		      if (r == AY_ESHAPE || Regs[r] != v)
		      {
			      /* update the output buffer before changing the register */
			      stream_update(Channel,0);
		      }
	      }

         _AYWriteReg(r,v);
      }


      static void Update( int [] buffer)
      {
	      int outn;

         int length = buffer.Length;

	      /* The 8910 has three outputs, each output is the mix of one of the three */
	      /* tone generators and of the (single) noise generator. The two are mixed */
	      /* BEFORE going into the DAC. The formula to mix each channel is: */
	      /* (ToneOn | ToneDisable) & (NoiseOn | NoiseDisable). */
	      /* Note that this means that if both tone and noise are disabled, the output */
	      /* is 1, not 0, and can be modulated changing the volume. */


	      /* If the channels are disabled, set their output to 1, and increase the */
	      /* counter, if necessary, so they will not be inverted during this update. */
	      /* Setting the output to 1 is necessary because a disabled channel is locked */
	      /* into the ON state (see above); and it has no effect if the volume is 0. */
	      /* If the volume is 0, increase the counter, but don't touch the output. */
	      if (Regs[AY_ENABLE] & 0x01)
	      {
		      if (CountA <= length*STEP) CountA += length*STEP;
		      OutputA = 1;
	      }
	      else if (Regs[AY_AVOL] == 0)
	      {
		      /* note that I do count += length, NOT count = length + 1. You might think */
		      /* it's the same since the volume is 0, but doing the latter could cause */
		      /* interferencies when the program is rapidly modulating the volume. */
		      if (CountA <= length*STEP) CountA += length*STEP;
	      }
	      if (Regs[AY_ENABLE] & 0x02)
	      {
		      if (CountB <= length*STEP) CountB += length*STEP;
		      OutputB = 1;
	      }
	      else if (Regs[AY_BVOL] == 0)
	      {
		      if (CountB <= length*STEP) CountB += length*STEP;
	      }
	      if (Regs[AY_ENABLE] & 0x04)
	      {
		      if (CountC <= length*STEP) CountC += length*STEP;
		      OutputC = 1;
	      }
	      else if (Regs[AY_CVOL] == 0)
	      {
		      if (CountC <= length*STEP) CountC += length*STEP;
	      }

	      /* for the noise channel we must not touch OutputN - it's also not necessary */
	      /* since we use outn. */
	      if ((Regs[AY_ENABLE] & 0x38) == 0x38)	/* all off */
		      if (CountN <= length*STEP) CountN += length*STEP;

	      outn = (OutputN | Regs[AY_ENABLE]);


	      /* buffering loop */
	      while (length)
	      {
		      int vola,volb,volc;
		      int left;


		      /* vola, volb and volc keep track of how long each square wave stays */
		      /* in the 1 position during the sample period. */
		      vola = volb = volc = 0;

		      left = STEP;
		      do
		      {
			      int nextevent;


			      if (CountN < left) nextevent = CountN;
			      else nextevent = left;

			      if (outn & 0x08)
			      {
				      if (OutputA) vola += CountA;
				      CountA -= nextevent;
				      /* PeriodA is the half period of the square wave. Here, in each */
				      /* loop I add PeriodA twice, so that at the end of the loop the */
				      /* square wave is in the same status (0 or 1) it was at the start. */
				      /* vola is also incremented by PeriodA, since the wave has been 1 */
				      /* exactly half of the time, regardless of the initial position. */
				      /* If we exit the loop in the middle, OutputA has to be inverted */
				      /* and vola incremented only if the exit status of the square */
				      /* wave is 1. */
				      while (CountA <= 0)
				      {
					      CountA += PeriodA;
					      if (CountA > 0)
					      {
						      OutputA ^= 1;
						      if (OutputA) vola += PeriodA;
						      break;
					      }
					      CountA += PeriodA;
					      vola += PeriodA;
				      }
				      if (OutputA) vola -= CountA;
			      }
			      else
			      {
				      CountA -= nextevent;
				      while (CountA <= 0)
				      {
					      CountA += PeriodA;
					      if (CountA > 0)
					      {
						      OutputA ^= 1;
						      break;
					      }
					      CountA += PeriodA;
				      }
			      }

			      if (outn & 0x10)
			      {
				      if (OutputB) volb += CountB;
				      CountB -= nextevent;
				      while (CountB <= 0)
				      {
					      CountB += PeriodB;
					      if (CountB > 0)
					      {
						      OutputB ^= 1;
						      if (OutputB) volb += PeriodB;
						      break;
					      }
					      CountB += PeriodB;
					      volb += PeriodB;
				      }
				      if (OutputB) volb -= CountB;
			      }
			      else
			      {
				      CountB -= nextevent;
				      while (CountB <= 0)
				      {
					      CountB += PeriodB;
					      if (CountB > 0)
					      {
						      OutputB ^= 1;
						      break;
					      }
					      CountB += PeriodB;
				      }
			      }

			      if (outn & 0x20)
			      {
				      if (OutputC) volc += CountC;
				      CountC -= nextevent;
				      while (CountC <= 0)
				      {
					      CountC += PeriodC;
					      if (CountC > 0)
					      {
						      OutputC ^= 1;
						      if (OutputC) volc += PeriodC;
						      break;
					      }
					      CountC += PeriodC;
					      volc += PeriodC;
				      }
				      if (OutputC) volc -= CountC;
			      }
			      else
			      {
				      CountC -= nextevent;
				      while (CountC <= 0)
				      {
					      CountC += PeriodC;
					      if (CountC > 0)
					      {
						      OutputC ^= 1;
						      break;
					      }
					      CountC += PeriodC;
				      }
			      }

			      CountN -= nextevent;
			      if (CountN <= 0)
			      {
				      /* Is noise output going to change? */
				      if ((RNG + 1) & 2)	/* (bit0^bit1)? */
				      {
					      OutputN = ~OutputN;
					      outn = (OutputN | Regs[AY_ENABLE]);
				      }

				      /* The Random Number Generator of the 8910 is a 17-bit shift */
				      /* register. The input to the shift register is bit0 XOR bit2 */
				      /* (bit0 is the output). */

				      /* The following is a fast way to compute bit 17 = bit0^bit2. */
				      /* Instead of doing all the logic operations, we only check */
				      /* bit 0, relying on the fact that after two shifts of the */
				      /* register, what now is bit 2 will become bit 0, and will */
				      /* invert, if necessary, bit 16, which previously was bit 18. */
				      if (RNG & 1) RNG ^= 0x28000;
				      RNG >>= 1;
				      CountN += PeriodN;
			      }

			      left -= nextevent;
		      } while (left > 0);

		      /* update envelope */
		      if (Holding == 0)
		      {
			      CountE -= STEP;
			      if (CountE <= 0)
			      {
				      do
				      {
					      CountEnv--;
					      CountE += PeriodE;
				      } while (CountE <= 0);

				      /* check envelope current position */
				      if (CountEnv < 0)
				      {
					      if (Hold)
					      {
						      if (Alternate)
							      Attack ^= 0x1f;
						      Holding = 1;
						      CountEnv = 0;
					      }
					      else
					      {
						      /* if CountEnv has looped an odd number of times (usually 1), */
						      /* invert the output. */
						      if (Alternate && (CountEnv & 0x20))
 							      Attack ^= 0x1f;

						      CountEnv &= 0x1f;
					      }
				      }

				      VolE = VolTable[CountEnv ^ Attack];
				      /* reload volume */
				      if (EnvelopeA) VolA = VolE;
				      if (EnvelopeB) VolB = VolE;
				      if (EnvelopeC) VolC = VolE;
			      }
		      }

		      *(buf1++) = (vola * VolA) / STEP;
		      *(buf2++) = (volb * VolB) / STEP;
		      *(buf3++) = (volc * VolC) / STEP;

		      length--;
	      }
      }


      void set_clock(int clock)
      {
	      /* the step clock for the tone and noise generators is the chip clock    */
	      /* divided by 8; for the envelope generator of the AY-3-8910, it is half */
	      /* that much (clock/16), but the envelope of the YM2149 goes twice as    */
	      /* fast, therefore again clock/8.                                        */
	      /* Here we calculate the number of steps which happen during one sample  */
	      /* at the given sample rate. No. of events = sample rate / (clock/8).    */
	      /* STEP is a multiplier used to turn the fraction into a fixed point     */
	      /* number.                                                               */
	      UpdateStep = ((float)STEP * SampleRate * 8) / clock;
      }


      void set_volume(int channel,int volume)
      {
	      int ch;

	      for (ch = 0; ch < 3; ch++)
		      if (channel == ch || channel == ALL_8910_CHANNELS)
			      mixer_set_volume(Channel + ch, volume);
      }


      static void build_mixer_table()
      {
	      int i;
	      float outvol;


	      /* calculate the volume->voltage conversion table */
	      /* The AY-3-8910 has 16 levels, in a logarithmic scale (3dB per step) */
	      /* The YM2149 still has 16 levels for the tone generators, but 32 for */
	      /* the envelope generator (1.5dB per step). */
	      outvol = MAX_OUTPUT;
	      for (i = 31;i > 0;i--)
	      {
		      VolTable[i] = outvol + 0.5;	/* round to nearest */

		      outvol /= 1.188502227;	/* = 10 ^ (1.5/20) = 1.5dB */
	      }
	      VolTable[0] = 0;
      }



      void reset()
      {
	      int i;

	      register_latch = 0;
	      RNG = 1;
	      OutputA = 0;
	      OutputB = 0;
	      OutputC = 0;
	      OutputN = 0xff;
	      for (i = 0;i < AY_PORTA;i++)
		      _AYWriteReg(chip,i,0);	/* AYWriteReg() uses the timer system; we cannot */
      								      /* call it at this time because the timer system */
								            /* has not been initialized. */
      }

      int init( int clock,int volume,int sample_rate)
      {

	      //memset(PSG,0,sizeof(struct AY8910));
	      SampleRate = sample_rate;
	      Channel = stream_init_multi(3,name,vol,sample_rate,AY8910Update);


	      AY8910_set_clock(clock);
	      AY8910_reset();

	      return 0;
      }


   }

}









