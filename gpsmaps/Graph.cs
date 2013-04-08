using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gpsmaps
{
   public partial class Graph : UserControl
   {
      public delegate void GPSMoveHandler(object sender, int index);

      public event GPSMoveHandler GPSMove;

      Path m_gp;
      PointF [] m_Trans;
      PointF[] m_val;

      public Graph()
      {
         InitializeComponent();
      }

      public void AddPath( Path gp )
      {
         m_gp = gp;
         m_val = m_gp.m_velTime.ToArray();
      }

      private float map( float v, float min, float max, float minout, float maxout )
      {
         float t = (v - min) / (max - min);
         return t * maxout + (1 - t) * minout;
      }

      private void Graph_Paint(object sender, PaintEventArgs e)
      {
         Rectangle r = new Rectangle( 0,+10, Width-1, Height-20 );

         e.Graphics.DrawRectangle(Pens.Red, r);

         

         if (m_val!= null && m_val.Length > 0)
         {
            m_Trans = new PointF[m_val.Length];

            float Ti = m_val[0].X;
            float Tf = m_val[m_val.Length - 1].X;

            for (int i = 0; i < m_val.Length; i++)
            {
               float x = map(m_val[i].X, Ti, Tf, r.Left, r.Right);
               float y = map(m_val[i].Y, 0, 15, r.Bottom, r.Top);
               m_Trans[i].X = x;
               m_Trans[i].Y = y;
            }
            e.Graphics.DrawLines(Pens.Black, m_Trans);
         }         
         

         
      }

      

      private void Graph_MouseMove(object sender, MouseEventArgs e)
      {
         if (m_val!= null && m_val.Length >= 0)
         {
            int i = (int)map(e.X, 0, Width, 0, m_val.Length - 1);


            GPSMove(sender, i);
         }
      }


   }
}
