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
   public partial class PatternEditor : Form
   {
      int time = 0;
      List<Pattern> m_sg;

      public PatternEditor()
      {
         InitializeComponent();
      }

      private void listView1_SelectedIndexChanged(object sender, EventArgs e)
      {
         
      }
      
      public void UpdateMachines()
      {
         List<System.Windows.Forms.ColumnHeader> ColumnsList = new List<System.Windows.Forms.ColumnHeader>();

         {
            System.Windows.Forms.ColumnHeader ch = new System.Windows.Forms.ColumnHeader();
            ch.Text = "step";
            ColumnsList.Add(ch);
         }
/*
         foreach (SoundGenerator sg in m_sg)
         {
            foreach (KeyValuePair<string, object> pair in sg.m_inputs)
            {
               string t = pair.Value.GetType().ToString();
               System.Windows.Forms.ColumnHeader ch = new System.Windows.Forms.ColumnHeader();

               if (pair.Value.GetType() == Type.GetType("midiplayer.variable+Slider"))
               {
                  ch.Text = pair.Key.ToString();
               }
               else if (pair.Value.GetType() == Type.GetType("midiplayer.variable+Set"))
               {
                  ch.Text = pair.Key.ToString();
               }
               else if (pair.Value.GetType() == Type.GetType("midiplayer.variable+SetUInt16"))
               {
                  ch.Text = pair.Key.ToString();
               }
               else if (pair.Value.GetType() == Type.GetType("System.SByte"))
               {
                  ch.Text = pair.Key.ToString();
               }
               else
               {
                  ch.Text = pair.Key.ToString();
               }
               ColumnsList.Add(ch);
            }
         }
 */ 
         this.listView1.Columns.AddRange(ColumnsList.ToArray());

         string[] notes = { "C", "C#", "D", "D#", "E", "G", "G#", "A", "A#", "B" };

         for (int i = 0; i < 10; i++)
         {
            ListViewItem l = new ListViewItem(i.ToString());

            l.SubItems.Add("4");           
            for(int j=0;j<10;j++)
            {
               l.SubItems.Add("4");  
               //string s = (j%10).ToString()+"4";
               l.SubItems.Add(notes[i % 10]+"4");
            }
            listView1.Items.Add(l);
         }

      }

      private void PatternEditor_Load(object sender, EventArgs e)
      {
         UpdateMachines();
      }


      private void checkBox1_CheckedChanged(object sender, EventArgs e)
      {
         timer1.Enabled = checkBox1.Checked;
      }

      private void timer1_Tick(object sender, EventArgs e)      
      {
         int item = 1;
         listView1.SelectedIndices.Clear();
         listView1.SelectedIndices.Add(time);
         /*
         foreach (SoundGenerator sg in m_sg)
         {
            foreach (KeyValuePair<string, object> pair in sg.m_inputs)
            {
               if (pair.Value.GetType() == Type.GetType("midiplayer.variable+Slider"))
               {
                  if (pair.Key == "freq")
                  {
                     variable.Slider s = (variable.Slider)(pair.Value);

                     string note = listView1.Items[time].SubItems[item].Text;

                     s.m_val = (ushort)n2f.Translate(note);
                  }
                  else if (pair.Key == "volume")
                  {
                     variable.Slider s = (variable.Slider)(pair.Value);

                     string vol = listView1.Items[time].SubItems[item].Text;

                     s.m_val = (ushort)int.Parse(vol);
                  }
               }

               item++;

            }
         }
         */
         time++;
         if (time >= listView1.Items.Count)
         {
            time = 0;
         }


      }

      private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
      {

      }

      private void button1_Click(object sender, EventArgs e)
      {

      }


   }
}
