using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace physics
{
   public partial class Form1 : Form
   {
      SolidRigid r = new SolidRigid();

      public Form1()
      {
         InitializeComponent();
      }

      private void DrawVector(PaintEventArgs e, Pen p, Vector a, Vector b)
      {
         e.Graphics.DrawLine(p, (float)a.X, (float)a.Y, (float)b.X, (float)b.Y);
      }

      private void spring(SolidRigid r , Vector local, Vector world)
      {
         Vector vv = r.LocalToWorld( local );

         double distance = (world - vv).Distance();

         Vector direction = (world - vv) / distance;

         r.ApplyForce(vv, direction * (.000001 * distance));
      }

      private void pictureBox1_Paint(object sender, PaintEventArgs e)
      {
         Vector s = new Vector(100, 80);
         Vector v = new Vector( 20, 0 );

         spring(r, v, s);

         Vector vv = r.LocalToWorld(v);
         DrawVector(e, Pens.Black, vv, s);
         //DrawVector(e, Pens.Red, vv, vv + dir * 10);

         e.Graphics.DrawLine(Pens.Beige, 100, 100,  (float)vv.X,  (float)vv.Y);


         r.Draw(e);

      }

      private void timer1_Tick(object sender, EventArgs e)
      {
         Vector v = new Vector( .000001, 0 );
         Vector f = new Vector( .000001, 0 );
         //ApplyForce( v,f );
         r.Integrate();

         pictureBox1.Invalidate();
      }
   }
}
