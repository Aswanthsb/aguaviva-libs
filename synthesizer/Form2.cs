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
   public partial class Form2 : Form
   {
      List<Box> m_boxes = new List<Box>();

      //links
      public class Link
      {
         //[Serializable]
         public Button input;
         public Button output;
      }
      List<Link> m_links = new List<Link>();
      Link m_link = new Link();

      Type[] types = { typeof(SimpleOscillator), typeof(LowPass), typeof(Mixer), typeof(WavPlayer), typeof(Resample), typeof(Chirp), typeof(ScopeForm), typeof(EnvelopeExtractor), typeof(Multiply), typeof(fir), typeof(Speaker), typeof(Scope) };      

      private void Populate()
      {
         System.Windows.Forms.ToolStripMenuItem simpleSinusoidToolStripMenuItem;

         foreach (Type t in types)
         {
            simpleSinusoidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            simpleSinusoidToolStripMenuItem.Name = t.ToString();
            simpleSinusoidToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            simpleSinusoidToolStripMenuItem.Text = t.ToString();
            simpleSinusoidToolStripMenuItem.Tag = t;
            simpleSinusoidToolStripMenuItem.Click += new System.EventHandler(this.simpleSinusoidToolStripMenuItem_Click);

            this.contextMenuStrip1.Items.Add( simpleSinusoidToolStripMenuItem);
         }
      }

      public Form2()
      {
         InitializeComponent();
      }

      void AddLink()
      {
         if (m_link.input != null && m_link.output != null)
         {
            Link p = new Link();

            p.input = m_link.input;
            p.output = m_link.output;

            variable.SetObject _in = (variable.SetObject)p.input.Tag;
            SoundGenerator _out = (SoundGenerator)p.output.Tag;

            _in(_out);

            m_links.Add(p);
            Invalidate();
            m_link.output = null;
            m_link.input = null;
         }
      }

      bool Unlink(Button b)
      {
         for (int i = 0; i < m_links.Count; i++)
         {
            if (m_links[i].input == b || m_links[i].output == b)
            {
               variable.SetObject _in = (variable.SetObject)m_links[i].input.Tag;
               _in(new NullGenerator("", 0));

               m_links.RemoveAt(i);
               Invalidate();
               return true;
            }
         }
         return false;
      }

      public void input_Click(object sender, EventArgs e)
      {
         Button b = (Button)sender;

         if (Unlink(b) == true)
            return;

         m_link.input = b;
         AddLink();
      }

      public void output_Click(object sender, EventArgs e)
      {
         Button b = (Button)sender;

         if (Unlink(b) == true)
            return;

         m_link.output = b;
         AddLink();
      }

      private void Form2_Paint(object sender, PaintEventArgs e)
      {
         Point p = new Point(0, 0);
         Point p0 = new Point();
         Point p1 = new Point();

         foreach (Link l in m_links)
         {
            p0 = (Point)l.output.Size;
            p0.Y /= 2;

            p1 = (Point)l.input.Size;
            p1.Y /= 2;

            p0 = l.output.PointToScreen(p0);
            p1 = l.input.PointToScreen(p1);
            p0 = this.PointToClient(p0);
            p1 = this.PointToClient(p1);

            p0.X = l.output.Parent.Parent.Location.X + l.output.Parent.Width;
            p1.X = l.input.Parent.Parent.Location.X;

            e.Graphics.DrawLine(Pens.Black, p0, p1);
         }
      }


      private void simpleSinusoidToolStripMenuItem_Click(object sender, EventArgs e)
      {
         ToolStripMenuItem s = (ToolStripMenuItem)sender;

         Type t = (Type)(s.Tag);

         SoundGenerator so = (SoundGenerator)System.Activator.CreateInstance(t, new object[] { (string)"Ff", (uint)44100 });

         Box childForm = new Box();
         
         //childForm.Parent = this;
         childForm.Text = "Window ";
         Controls.Add(childForm);

         childForm.MouseMove += new MouseEventHandler(Form2_MouseMove);
         childForm.MouseDown += new MouseEventHandler(Form2_MouseDown);
         childForm.SetMachine(so);
         childForm.Show();

         m_boxes.Add(childForm);

         if (t == typeof(Scope))
         {
            ((MDIParent1)MdiParent).AddScope((Scope)so);
         }

      }

      private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
      {
         e.Cancel = false;
      }

      private void Form2_Load(object sender, EventArgs e)
      {
         Populate();

         FFT.fft f = new FFT.fft(256);

         double[] input = new double[1024];
         double[] output = new double[1024];

         for (int i = 0; i < 1024; i++)
         {
            int n = 1024/256;
            input[i] = Math.Cos( (2.0 * Math.PI * n * i) / 1024 );
         }

         int window = 128;

         double[] phases = new double[window];

         for (int i = 0; i < 10; i++)
         {
            int windowOffset = window;

            f.SetData(input, i * windowOffset );
            f.SetHarmonic(1, i);
            //f.SetWindow();
            f.FFT(-1);

            /*
            for (int b = 0; b < window; b++)
            {
               double p = f.Phase(b);

               double deltaphase = p - phases[b];

               double expectedphase = phases[b] + Math.PI * b;

               double adjustedPhase = expectedphase + p;

               phases[b] = adjustedPhase;
            }
            */


            double p = f.Phase(i);

            f.FFT(1);

            f.GetData(output, i * windowOffset );
         }

         




      }

      private void Form2_MouseClick(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Right)
         {
            contextMenuStrip1.Show(this, e.Location);
         }
      }

      public void Form2_MouseMove(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
            if (sender != this)
            {
               Point p = new Point();

               p = ClickedBox.PointToScreen(this.PointToClient(e.Location));
               Point pp = p;
               p.X = p.X - MouseDownPoint.X;
               p.Y = p.Y - MouseDownPoint.Y;
               ClickedBox.Location = new Point(ClickedBox.Location.X + p.X, ClickedBox.Location.Y + p.Y);

               MouseDownPoint = pp;
            }

            Invalidate();
         }
      }

      Point MouseDownPoint;
      Control ClickedBox;

      private void Form2_MouseDown(object sender, MouseEventArgs e)
      {
         Control ctl = sender as Control;
         if (sender != this)
         {
            ClickedBox = ctl;
            MouseDownPoint = ctl.PointToScreen(this.PointToClient(e.Location));
         }
      }

   }
}
