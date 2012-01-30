using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace physics
{
   class Vector
   {
      private double _x, _y;

      public Vector() { _x = 0; _y = 0; }
      public Vector(double x, double y) { _x = x; _y = y; }

      public double X
      {
         get { return _x; }
         set { _x = value; }
      }

      public double Y
      {
         get { return _y; }
         set { _y = value; }
      }

      public static double operator ^(Vector v1, Vector v2)
      {
         return v1.X * v2.Y - v1.Y * v2.X;
      }

      public static Vector operator +(Vector v1, Vector v2)
      {
         return new Vector(v1.X + v2.X, v1.Y + v2.Y);
      }

      public static Vector operator -(Vector v1, Vector v2)
      {
         return new Vector(v1.X - v2.X, v1.Y - v2.Y);
      }

      public static double operator *(Vector v1, Vector v2)
      {
         return (v1.X * v2.X + v1.Y * v2.Y);
      }

      public double Distance()
      {
         return Math.Sqrt(_x * _x + _y * _y );
      }

      public Vector Normalize()
      {
         return this / this.Distance();
      }


      public static Vector operator *(Vector v1, double scale)
      {
         return new Vector(v1.X * scale, v1.Y * scale);
      }

      public static Vector operator /(Vector v1, double scale)
      {
         return new Vector(v1.X / scale, v1.Y / scale);
      }

      public static Vector operator -(Vector v)
      {
         return new Vector(-v.X, -v.Y);
      }


   }
}
