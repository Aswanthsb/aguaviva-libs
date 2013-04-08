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
   public partial class ScopeForm : Form
   {
      Mixer m = new Mixer("dfd", 44100);

      public ScopeForm()
      {
         InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         ((Form2)MdiParent).input_Click(sender, e);
      }

      private void button2_Click(object sender, EventArgs e)
      {
         ((Form2)MdiParent).input_Click(sender, e);

      }

      private void Trigger()
      {
         sbyte dataRedLast = 0;
         sbyte dataBlueLast = 0;

         for (int i = 0; i < 1000; i++)
         {
            sbyte dataRed = m.m_SoundGenList[0].GetValue();
            sbyte dataBlue = m.m_SoundGenList[1].GetValue();


            if (dataRedLast < 0 && dataRed >= 0)
               break;

            dataRedLast = dataRed;
         }
      }


      sbyte [] m_dataRed = new sbyte[1000];
      sbyte[] m_dataBlue = new sbyte[1000];

      private void GetBuffer()
      {
         //for (int i = 0; i < m_dataRed.Length; i++)
         XOscillo.DataBlock db = new XOscillo.DataBlock();
         db.Alloc(44100);

         List<Scope> sc = ((MDIParent1)MdiParent).GetScopes();

         db.m_sampleRate = 44100;
         db.m_stride = 2;
         db.m_channels = 2;

         for (int i = 0; i < 44100; )
         {
            db.m_Buffer[i++] = (byte)(127 + (int)sc[0].GetValue());
            db.m_Buffer[i++] = 0;// (byte)(127 + (int)m.m_SoundGenList[1].GetValue());
         }

         this.graphControl1.SetScopeData(db);
         this.graphControl1.SetSecondsPerDivision(.001f);
      }


      private void Scope_Paint(object sender, PaintEventArgs e)
      {

         /*
         int y = Height / 2;

         for (int i = 1; i < 1000; i ++)
         {
            e.Graphics.DrawLine(Pens.Red, i, (int)m_dataRed[i-1] + y, i + 1, (int)m_dataRed[i] + y);
            e.Graphics.DrawLine(Pens.Blue, i, (int)m_dataBlue[i-1] + y, i + 1, (int)m_dataBlue[i] + y);
         }
          */
      }

      private void Scope_Load(object sender, EventArgs e)
      {
         button1.Tag = m.m_inputs["input.0"];
         button2.Tag = m.m_inputs["input.1"];
      }

      private void button3_Click(object sender, EventArgs e)
      {
         GetBuffer();
         Invalidate();
      }

      private void checkBox2_CheckedChanged(object sender, EventArgs e)
      {
         

         if (checkBox2.Checked == true)
         {
            if (checkBox1.Checked)
            {
               Trigger();
            }
            GetBuffer();
            Invalidate();
         }      
      }

      private void checkBox3_CheckedChanged(object sender, EventArgs e)
      {
         this.graphControl1.DrawFFT(checkBox3.Checked);
      }

   }
}
