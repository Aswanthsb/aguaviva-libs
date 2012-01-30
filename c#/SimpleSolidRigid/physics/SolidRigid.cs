using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace physics
{
   class SolidRigid
   {
      Vector pos = new Vector(100, 100);
      Vector vel = new Vector();
      Vector acc = new Vector();

      double w, alpha;
      double angle = 0;
      Matrix MatRot = new Matrix();

      public void Integrate()
      {
         pos += vel;
         vel += acc;

         angle += w;
         w += alpha;

         MatRot.Rotation(angle);
      }

      public void ApplyForce(Vector p, Vector force)
      {
         alpha = (p - pos) ^ force;
      }

      public Matrix RotMatrix()
      {
         return MatRot;
      }

      public Vector LocalToWorld(Vector localpos)
      {
         return RotMatrix()*localpos+pos;
      }
      /*
      public Vector WorldToLocal(Vector localpos)
      {
         return RotMatrix() * localpos + pos;
      }
      */

      public void Draw(PaintEventArgs e)
      {
         e.Graphics.ResetTransform();
         e.Graphics.TranslateTransform((float)pos.X, (float)pos.Y);
         e.Graphics.RotateTransform((int)(180.0 * angle / 3.141592));

         e.Graphics.DrawRectangle(Pens.Black, -30, -10, 60, 20);
      }

   }
}
