using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DirectX;
using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;
using System.Text;

namespace midiplayer
{
   class Speaker : SoundGenerator
   {
      Boolean m_play;
      Device applicationDevice;
      SecondaryBuffer secondaryBuffer;
      BufferDescription desc;
      byte[] m_rawsamples;
      int m_IndexRawSamples;
      int m_myWrite;
      Form m_form;

      SoundGenerator m_input;
      public void SetInput(object o)
      {
         m_input = (SoundGenerator)o;
      }


      public Speaker(string name, uint sampleRate)
         : base(name, sampleRate)
      {
         m_input = new NullGenerator("null", 44100);
         m_inputs["input"] = new variable.SetObject(SetInput);


         applicationDevice = null;
         secondaryBuffer = null;
      }




      public Boolean Play2
      {
         get { return m_play; }
         set { m_play = value ; }
      }

      public bool Init(Form form)
      {
         m_form = form;

         applicationDevice = new Device();
         applicationDevice.SetCooperativeLevel(form, CooperativeLevel.Normal);

         WaveFormat format = new WaveFormat();
         format.BitsPerSample = 8;
         format.Channels = 1;
         format.BlockAlign = 1;

         format.FormatTag = WaveFormatTag.Pcm;
         format.SamplesPerSecond = (int)m_sampleRate; 
         format.AverageBytesPerSecond = format.SamplesPerSecond * format.BlockAlign;

         // buffer description         
         desc = new BufferDescription(format);
         desc.DeferLocation = true;
         desc.BufferBytes = format.AverageBytesPerSecond;

         // create the buffer         
         secondaryBuffer = new SecondaryBuffer(desc, applicationDevice);

         //generate ramdom data (white noise)
         m_rawsamples = new byte[desc.BufferBytes / 50];

         return true;
      }

      // bytes available in the circular buffer
      public int FreeInBuffer()
      {
         int play = secondaryBuffer.PlayPosition;
         int write = m_myWrite;//secondaryBuffer.WritePosition;
         //int write = secondaryBuffer.WritePosition;

         if ( write > play)
         {
            return desc.BufferBytes - (write - play);
         }
         else
         {
            return play - write;
         }
      }


      public bool CanRender()
      {
         return FreeInBuffer() >= desc.BufferBytes;
      }

      public int RenderSound()
      {
         if (FreeInBuffer() < m_rawsamples.Length)
            return -1;

         if (m_input == null)
            return 0;
         for (int i = 0; i < m_rawsamples.Length; i++)
         {
            m_rawsamples[i] = (byte)((int)m_input.GetValue()+128);
         }

         secondaryBuffer.Write(m_myWrite, m_rawsamples, LockFlag.EntireBuffer);

         m_myWrite += m_rawsamples.Length;

         if (m_myWrite >= desc.BufferBytes)
            m_myWrite -= desc.BufferBytes;

         return 0;
      }

      public int Play()
      {
         secondaryBuffer.Play(0, BufferPlayFlags.Looping);
         m_myWrite = secondaryBuffer.WritePosition;

         return 0;
      }


      public int Stop()
      {
         secondaryBuffer.Stop();
         return 0;
      }

      override public sbyte GetValue()
      {
         return 0;
      }

   }
}
