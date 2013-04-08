using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gpsmaps
{
   using System;
   using System.Linq;
   using System.Collections.Generic;
   using System.Text;
   using System.Drawing;
   using System.IO;


   public class Path
   {
      public List<PointF> m_pointsF = new List<PointF>();
      public List<PointF> m_velTime = new List<PointF>();
      
      Point[] m_points;

      public void LoadPath(string filename)
      {
         using (StreamReader sr = new StreamReader(filename))
         {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
               if (line == "start" || line == "end")
                  continue;

               string[] str = line.Split(' ');
               if (str.Length < 5)
                  break;
               PointF p = new PointF();

               p.X = float.Parse(str[4]);
               p.Y = float.Parse(str[3]);



               m_pointsF.Add(p);

               string [] time = str[1].Split(':');
               float secs = float.Parse( time[1] ) * 60 + float.Parse( time[2] );
               float vel = float.Parse( str[5] ) * 1.852f;

               m_velTime.Add(new PointF(secs, vel));
            }
         }

      }

      /*
      public void Set(float scale, float cx, float cy, int mip)
      {
         m_points = new Point[m_pointsF.Count];
         for (int i = 0; i < m_pointsF.Count; i++)
         {
            m_points[i].X = (int)(m_pointsF[i].X * scale + cx);
            m_points[i].Y = (int)(m_pointsF[i].Y * scale + cy);
         }
      }
       */

      public void Set(float scale, float cx, float cy, int mip)
      {
         m_points = new Point[m_pointsF.Count];

         int x = 0;
         int y = 0;

         int j = 0;
         for (int i = 0; i < m_pointsF.Count; i++)
         {
            int xx = (int)(m_pointsF[i].X * scale + cx);
            int yy = (int)(m_pointsF[i].Y * scale + cy);

            if (xx != x && yy != y)
            {
               m_points[j].X = xx;
               m_points[j].Y = yy;
               j++;
               x = xx;
               y = yy;
            }
         }

         Point[] m_shrinked = new Point[j];
         Array.Copy(m_points, m_shrinked, m_shrinked.Length);
         m_points = m_shrinked;
      }

      public void DrawPath(Graphics g, Pen p)
      {
         if (m_points != null)
         {
            g.DrawLines(p, m_points);
         }
      }
   }
}
