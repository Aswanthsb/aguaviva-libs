using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace midiplayer
{
	public partial class Form1 : Form
	{
		Player pl;
      KonamiLoader ml;
      DirectSoundPlayer dpl;

      sbyte[] data = new sbyte[10000];

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
		}

		void PlayerPoll()
		{
         
         while (pl.FreeInBuffer() >= 4410)
			{            
				listBox1.Items.Insert(0, ml.Play(pl));
			}
         


		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			PlayerPoll();
		}

		private void button1_CheckedChanged(object sender, EventArgs e)
		{
			if (button1.Checked)
			{
				//WavPlayer wpl = new WavPlayer();
				//wpl.Init("c:\\cancion.bin");
				pl = dpl;

            

				//ml = new ModLoader();
            //ml.LoadMod("c:\\python25\\aladdin.mod");
            //ml.LoadMod("c:\\python25\\midlalta.mod");

            //ml = new MidiLoader();
				//ml.Load("c:\\python25\\smb1-Theme.mid");
				//ml.Load("kungfu.mid");
            //ml.Load("mario.mid");
				//ml.Load("c:\\python25\\moo_moo_farm.mid");
				//PlayerPoll();

            //ml = new PSGLoader();

            //ml.Load("kings.psg");
            //ml.Load("soccer.psg");
            //ml.Load("log.psg");

            ml = new KonamiLoader();
            ml.Load("king.rom");
            
				pl.Play();

            for (int i = 0; i < 100000; i++)
            {
               data[i] = ml.GetValue();
            }
            Invalidate();
            //listBox1.Items.Insert(0, ml.PlaySample(pl, 7));
				
				/*
				for(int i=0;i<20000;i++)
					ml.Play(pl);
				 */
				timer1.Enabled = true;
				timer2.Enabled = true;
			}
			else
			{
				timer1.Enabled = false;
				if (pl != null)
					pl.Stop();
			}
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			timer1.Interval = trackBar1.Value;
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			//progressBar1.Value = pl.BufferDepth();
		}

      private void Form1_Load(object sender, EventArgs e)
      {
         dpl = new DirectSoundPlayer();
         dpl.Init(this);
         ml = new KonamiLoader();
         ml.Load("king.rom");
         pl = dpl;

         pl.Play();
         timer1.Enabled = true;
         timer2.Enabled = true;

      }

      private void button2_Click(object sender, EventArgs e)
      {

      }

      private void Form1_Paint(object sender, PaintEventArgs e)
      {
         int d = 3;
         int t = 0;
         for (int i = 0; i < data.Length-d; i+=d)
         {
            e.Graphics.DrawLine(Pens.Black,t,data[i]+128,t+1,data[i+d]+128);
            t++;
         }
      }
   }
}
