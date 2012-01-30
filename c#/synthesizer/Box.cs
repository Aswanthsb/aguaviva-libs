using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace midiplayer
{
   public partial class Box : Panel
   {
      SoundGenerator mg_sg;

      public Box()
      {
         InitializeComponent();
      }

      public void SetMachine( SoundGenerator sg )
      {
         int ctrl = 0;

         foreach (KeyValuePair<string, object> pair in sg.m_inputs)
         {
            string t = pair.Value.GetType().ToString();

            if (pair.Value.GetType() == Type.GetType("midiplayer.variable+SetFilename"))
            {
               Button b = new Button();
               b.Margin = new Padding(0, 3, 0, 3);
               b.Tag = sg;
               tableLayoutPanel1.Controls.Add(b, 0, ctrl);
               b.Click += new System.EventHandler(this.input_Click);

               Label l = new Label();
               l.Text = pair.Key;
               l.Dock = DockStyle.Fill;
               tableLayoutPanel1.Controls.Add(l, 1, ctrl);


               TableLayoutPanel tl = new TableLayoutPanel();
               tl.ColumnCount = 2;
               tl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,90F));
               tl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
               tl.Dock = DockStyle.Fill;
               //tl.Bounds = 
               tableLayoutPanel1.Controls.Add(tl, 2, ctrl++);

               TextBox n = new TextBox();
               n.Name = pair.Key;
               n.Dock = DockStyle.Fill;
               n.Tag = pair.Value;               
               tl.Controls.Add(n, 0, 0);

               Button b2 = new Button();
               b2.Text = "...";
               b2.Tag = n;
               b2.Click += new System.EventHandler(this.ChooseFilename_Click);
               
               tl.Controls.Add(b2, 1, 0);
            }
            else if (pair.Value.GetType() == Type.GetType("midiplayer.variable+Slider"))
            {
               Button b = new Button();
               b.Margin = new Padding(0, 3, 0, 3);
               b.Tag = sg;
               tableLayoutPanel1.Controls.Add(b, 0, ctrl);
               b.Click += new System.EventHandler(this.input_Click);

               Label l = new Label();
               l.Text = pair.Key;
               l.Dock = DockStyle.Fill;
               tableLayoutPanel1.Controls.Add(l, 1, ctrl);

               variable.Slider vs = (variable.Slider)pair.Value;
               TrackBar n = new TrackBar();
               n.Name = pair.Key;
               n.SetRange(vs.m_min, vs.m_max);
               n.Value = vs.m_val;
               n.Dock = DockStyle.Fill;
               n.TickStyle = TickStyle.None;
               n.Tag = pair.Value;
               n.Scroll += new System.EventHandler(this.trackBar_Scroll);
               tableLayoutPanel1.Controls.Add(n, 2, ctrl++);
            }
            else if (pair.Value.GetType() == Type.GetType("midiplayer.variable+Set"))
            {
               Button b = new Button();
               b.Tag = sg;
               tableLayoutPanel1.Controls.Add(b, 0, ctrl);
               b.Click += new System.EventHandler(this.input_Click);

               Label l = new Label();
               l.Text = pair.Key;
               l.Dock = DockStyle.Fill;
               tableLayoutPanel1.Controls.Add(l, 1, ctrl);

               Button bb = new Button();
               bb.Tag = pair.Value;
               bb.Dock = DockStyle.Fill;
               bb.Click += new System.EventHandler(this.function_Click);
               tableLayoutPanel1.Controls.Add(bb, 2, ctrl++);               
            }
            else if (pair.Value.GetType() == Type.GetType("midiplayer.variable+SetUInt16"))
            {
               Button b = new Button();
               b.Tag = sg;
               tableLayoutPanel1.Controls.Add(b, 0, ctrl);
               b.Click += new System.EventHandler(this.input_Click);

               Label l = new Label();
               l.Text = pair.Key;
               l.Dock = DockStyle.Fill;
               tableLayoutPanel1.Controls.Add(l, 1, ctrl);

               TrackBar n = new TrackBar();
               n.Name = pair.Key;
               n.SetRange(0,2000);
               n.Value = 100;// (ushort)(pair.Value);
               n.Dock = DockStyle.Fill;
               n.Tag = pair.Value;
               n.Scroll += new System.EventHandler(this.trackBar_Scroll);
               tableLayoutPanel1.Controls.Add(n, 2, ctrl++);               
            }
            else if (pair.Value.GetType() == Type.GetType("System.SByte"))
            {
               Button b = new Button();
               b.Tag = sg;
               tableLayoutPanel1.Controls.Add(b, 0, ctrl);
               b.Click += new System.EventHandler(this.input_Click);


               Label l = new Label();
               l.Text = pair.Key;
               l.Dock = DockStyle.Fill;
               tableLayoutPanel1.Controls.Add(l, 1, ctrl++);
            }
            else
            {
               Button b = new Button();
               b.Tag = pair.Value;
               tableLayoutPanel1.Controls.Add(b, 0, ctrl);
               b.Click += new System.EventHandler(this.input_Click);


               Label l = new Label();
               l.Text = pair.Key;
               l.Dock = DockStyle.Fill;
               tableLayoutPanel1.Controls.Add(l, 1, ctrl++);
            }
         }


         foreach (KeyValuePair<string, object> pair in sg.m_output)
         {
            string t = pair.Value.GetType().ToString();
            if (pair.Value.GetType() == Type.GetType("System.SByte"))
            {
               Label l = new Label();
               l.Text = pair.Key;
               l.Dock = DockStyle.Fill;
               tableLayoutPanel1.Controls.Add(l, 2, ctrl);

               Button b = new Button();
               b.Margin = new Padding(0,3,0,3);
               b.Tag = sg;
               tableLayoutPanel1.Controls.Add(b, 3, ctrl++);
               b.Click += new System.EventHandler(this.output_Click);
            }
               /*
            else
            {
               Label l = new Label();
               l.Text = pair.Key;
               l.Dock = DockStyle.Fill;
               tableLayoutPanel1.Controls.Add(l, 2, ctrl++);

               Button b = new Button();
               b.Tag = sg;
               tableLayoutPanel1.Controls.Add(b, 3, ctrl);
               b.Click += new System.EventHandler(this.input_Click);
            }
                */
         }

         this.Height = (sg.m_inputs.Count + sg.m_output.Count)*20 + 20;
         this.Width = tableLayoutPanel1.Width;

         tableLayoutPanel1.RowCount = sg.m_inputs.Count + sg.m_output.Count;

         for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
         {
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         }

         tableLayoutPanel1.Location = new Point(0, 20);
         tableLayoutPanel1.Height = tableLayoutPanel1.RowCount * 20;
         
      }


      private void ChooseFilename_Click(object sender, EventArgs e)
      {
         OpenFileDialog ofd = new OpenFileDialog();
         DialogResult dr = ofd.ShowDialog();
         if (dr == DialogResult.OK)
         {
            Button b = (Button)sender;
            TextBox tb = (TextBox)(b.Tag);
            tb.Text = ofd.FileName;
            ((variable.SetFilename)(tb.Tag))(ofd.FileName);
         }
      }


      private void function_Click(object sender, EventArgs e)
      {
         Button b = (Button)sender;
         ((variable.Set)(b.Tag))();
      }

      private void input_Click(object sender, EventArgs e)
      {
         ((Form2)Parent).input_Click(sender, e);
      }

      private void output_Click(object sender, EventArgs e)
      {
         ((Form2)Parent).output_Click(sender, e);
      }

      private void trackBar_Scroll(object sender, EventArgs e)
      {
         TrackBar tb = (TrackBar)sender;

         ((variable.Slider)tb.Tag).m_val = (UInt16)tb.Value;
      }

      private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
      {

      }

   }
}
