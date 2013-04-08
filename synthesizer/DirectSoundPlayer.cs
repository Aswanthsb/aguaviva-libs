using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;

using Microsoft.DirectX.DirectSound;
using System.Windows.Forms;

namespace midiplayer
{
   class DirectSoundPlayer : Player
   {
      Device applicationDevice;

      SecondaryBuffer secondaryBuffer;
      BufferDescription desc;

      byte[] m_rawsamples;
      int m_IndexRawSamples;

      int m_myWrite;

      Form m_form;

      public DirectSoundPlayer()
      {
         applicationDevice = null;
         secondaryBuffer = null;
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
         format.SamplesPerSecond = 44100; //sampling frequency of your data;   
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
      override public int FreeInBuffer()
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


      override public bool CanRender()
      {
         return FreeInBuffer() >= desc.BufferBytes;
      }

      override public int RenderSound(SoundGenerator SoundGenList)
      {

         for (int i = 0; i < m_rawsamples.Length; i++)
         {
            m_rawsamples[i] = (byte)SoundGenList.GetValue();
         }

         secondaryBuffer.Write(m_myWrite, m_rawsamples, LockFlag.EntireBuffer);

         m_myWrite += m_rawsamples.Length;

         if (m_myWrite >= desc.BufferBytes)
            m_myWrite -= desc.BufferBytes;

         return 0;
      }

      override public int RenderSound(Int32[] data, Int32 division)
      {
         for (int i = 0; i < data.Length; i++)
         {
            m_rawsamples[m_IndexRawSamples++] = (byte)(data[i] / division);

            if (m_IndexRawSamples == m_rawsamples.Length)
            {
               m_IndexRawSamples = 0;

               secondaryBuffer.Write(m_myWrite, m_rawsamples, LockFlag.FromWriteCursor );//.EntireBuffer);

               m_myWrite += m_rawsamples.Length;

               if (m_myWrite >= desc.BufferBytes)
                  m_myWrite -= desc.BufferBytes;
            }
         }

         return 0;
      }

      override public int Play()
      {
         secondaryBuffer.Play(0, BufferPlayFlags.Looping);
         m_myWrite = secondaryBuffer.WritePosition;

         return 0;
      }


      override public int Stop()
      {
         secondaryBuffer.Stop();
         return 0;
      }
   }
}
