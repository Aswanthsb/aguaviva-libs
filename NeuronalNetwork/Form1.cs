using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;

namespace test
{
   public partial class Form1 : Form
   {
      Thread oThread;
      public delegate void AddListItem();
      AddListItem mydelegate;

      //Trainer tr = new BackPropagationTrainer( new Seno() );
      //Trainer tr = new BackPropagationTrainer(new XOR());
      //Trainer tr = new GeneticTrainer(new XOR());
      Trainer tr = new BackPropagationTrainer(new CourierOCR());
      //Trainer tr = new BackPropagationTrainer(new HandWriting());

      public Form1()
      {
         InitializeComponent();

         tr.Init();

         mydelegate = new AddListItem(AddProgress);

      }

      private void Form1_Load(object sender, EventArgs e)
      {
         oThread = new Thread(new ThreadStart(this.Xor));
         oThread.Priority = ThreadPriority.Highest;//.AboveNormal;
         oThread.Start();
      }


      private void Form1_FormClosing(object sender, FormClosingEventArgs e)
      {
      }

      int iteracciones = 1000;
      TimeSpan timeItTook;
      void Xor()
      {
         mydelegate = new AddListItem(AddProgress);
         
         for (int j = 0; j < 10000; j++)
         {
            DateTime start = DateTime.Now;
            for (int i = 0; i < iteracciones; i++)
            {
               tr.TrainNetwork();
            }
            timeItTook = DateTime.Now - start;

            if (iteracciones == 100000)
            {
               //iteracciones = 1000 * iteracciones / timeItTook.Milliseconds;
            }

            try
            {
               Invoke(mydelegate);
               Invalidate();
            }
            catch
            {
               return;
            }
         }         
      }
      
      public void AddProgress()
      {
         string o = "";

         o = string.Format("time {0}\r\n", timeItTook.Milliseconds);
         o += string.Format("iteracciones {0}\r\n", tr.iter);
         if (timeItTook.Milliseconds > 0)
         {
            o += string.Format("i/s {0}\r\n", 1000 * tr.iter / timeItTook.Milliseconds);
         }
         o += tr.Print();

         this.textBox1.Text = string.Join( "\r\n", o.Split('\n'));
      }

      private void Form1_Paint(object sender, PaintEventArgs e)
      {
         //job.GetNN().Draw(e, this.panel1);
         //job.DrawInput(e,0);

         /*
         Bitmap bmp = new Bitmap(8, 10, PixelFormat.Format24bppRgb);
         Graphics gBmp = Graphics.FromImage(bmp);
         Font f = new Font("Courier New", 10);
         Rectangle r = new Rectangle(0, 0, 8, 10);

         for (int i = 0; i < 25; i++)
         {
            gBmp.FillRectangle(Brushes.White, r);
            string str = Convert.ToChar('A'+i).ToString();
            gBmp.DrawString(str, f, Brushes.Black, -2, -2);

            e.Graphics.DrawImage(bmp, i*15, 0);
         }
          * */

      }



   }
}
