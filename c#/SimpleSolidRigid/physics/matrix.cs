using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace physics
{
   class Matrix
   {
      private double _x1, _y1;
      private double _x2, _y2;

      public double x1 { get { return _x1; } set { _x1 = value; } }
      public double y1 { get { return _y1; } set { _y1 = value; } }
      public double x2 { get { return _x2; } set { _x2 = value; } }
      public double y2 { get { return _y2; } set { _y2 = value; } }

      public void Rotation(double a)
      {
         _x1 = Math.Cos(a); _y1 = -Math.Sin(a);
         _x2 = Math.Sin(a); _y2 = Math.Cos(a);
      }

      public static Vector operator * (Matrix m, Vector v)
      {
         return new Vector( m._x1 * v.X + m._y1 * v.Y, m._x2 * v.X + m._y2 * v.Y );
      }


   }
}
