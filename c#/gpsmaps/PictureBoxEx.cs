using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace gpsmaps
{
   public partial class PictureBoxEx : Control
   {
      Bitmap m_bmpError = null;

      string m_mapsPath = "\\Tarjeta de almacenamiento\\mapas\\";
      Pen m_penRed = new Pen(Color.Red,1);
      Pen m_penBlack = new Pen(Color.Black);

      MouseEventArgs m_mouse;
      float m_MouseX = 0;
      float m_MouseY = 0;

      float m_latitude = 50.85189f;
      float m_longitude = 4.36482838f;
      int m_zoom = 5;

      Path m_gp;

      public PictureBoxEx()
      {
         InitializeComponent();
      }

      public void SetCacheDir(string cache)
      {
         m_mapsPath = cache;
      }


      byte[] GetData(string Url)
      {
         System.Net.WebRequest myReq = System.Net.WebRequest.Create(Url); //DownloadData function from here
         //myReq.Timeout = 1000;
         WebResponse myResp = myReq.GetResponse();
         Stream stream = myResp.GetResponseStream();

         byte[] buffer = new byte[myResp.ContentLength];
         int bytes = 0;

         while (bytes < buffer.Length)
         {
            bytes += stream.Read(buffer, bytes, buffer.Length - bytes);
         }

         return buffer;
      }

      Dictionary<string, Bitmap> m_cache = new Dictionary<string, Bitmap>();

      public Bitmap GetOpenStreetMapTile(int tileX, int tileY, int zoom)
      {
         Bitmap bmp = null;

         string file = string.Format("{0}_{1}_{2}.png", zoom, tileX, tileY);

         // was it cached?
         if (m_cache.TryGetValue(file, out bmp) == true)
            return bmp;

         //go get the tile
         if (File.Exists(m_mapsPath + file) == true)
         {
            try
            {
               bmp = new Bitmap(m_mapsPath + file);
            }
            catch
            {
            }
         }
         else
         {
            try
            {
               string url = string.Format("{0}/{1}/{2}.png", zoom, (int)tileX, (int)tileY);

               byte[] data = GetData("http://c.tile.openstreetmap.org/" + url);

               if (data != null)
               {
                  try
                  {
                     FileStream fileStream = new FileStream(m_mapsPath + file, FileMode.Create);
                     fileStream.Write(data, 0, (int)data.Length);
                     fileStream.Close();
                  }
                  catch
                  {
                  }

                  MemoryStream ms = new MemoryStream(data);
                  bmp = new Bitmap(ms);
               }
            }
            catch
            {
               if (m_bmpError == null)
               {
                  m_bmpError = new Bitmap(m_mapsPath + "error.png");
               }
               bmp = m_bmpError;
            }
         }

         m_cache[file] = bmp;

         //label1.Text = m_cache.Keys.Count.ToString();

         return bmp;
      }

      public void GeoToScreen(double lat, double lon, ref PointF p)
      {
         p.X = (float)((lon + 180.0) / 360.0);
         p.Y = (float)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) + 1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0);
      }

      public void GetOpenStreetMap(Graphics g, double lat, double lon, int zoom)
      {
         PointF tile = new PointF();
         GeoToScreen(lat, lon, ref tile);

         tile.X *= (1 << zoom);
         tile.Y *= (1 << zoom);

         tile.X -= m_MouseX / 256;
         tile.Y -= m_MouseY / 256;

         int tileIntX = (int)Math.Floor(tile.X);
         int tileIntY = (int)Math.Floor(tile.Y);

         int tileFracX = (int)((tile.X - tileIntX) * 256);
         int tileFracY = (int)((tile.Y - tileIntY) * 256);

         int centerX = (int)( - tileFracX + Width / 2);
         int centerY = (int)( - tileFracY + Height / 2);

         int minX = tileIntX - (int)Math.Ceiling((float)centerX / 256);
         int maxX = tileIntX + (int)Math.Ceiling((float)(Width - (centerX + 256)) / 256);

         int minY = tileIntY - (int)Math.Ceiling((float)centerY / 256);
         int maxY = tileIntY + (int)Math.Ceiling((float)(Height - (centerY + 256)) / 256);

         for (int y = minY; y <= maxY; y++)
         {
            for (int x = minX; x <= maxX; x++)
            {
               if (x >= 0 && y>=0 && ((1<<zoom)<x) && ((1<<zoom)<y) )
                  GetOpenStreetMapTile((int)x, (int)y, zoom);
            }
         }

         for (int y = minY; y <= maxY; y++)
         {
            for (int x = minX; x <= maxX; x++)
            {
               if (x < 0 || y < 0)
                  continue;

               if ((x >= (1 << zoom)) || (y >= (1 << zoom)))
                  continue;

               Bitmap bmp = GetOpenStreetMapTile((int)x, (int)y, zoom);

               int fx = (x - tileIntX) * 256;
               int fy = (y - tileIntY) * 256;

               g.DrawImage(bmp, fx + centerX, fy + centerY);
               //g.DrawRectangle(m_penBlack, fx + centerX, fy + centerY, 256, 256);
            }
         }

         g.DrawRectangle(m_penRed, (Width / 2 - 5 + (int)m_MouseX), Height / 2 - 5 + (int)m_MouseY, 10, 10);

         g.DrawRectangle(m_penBlack, Width / 2 - 5, Height / 2 - 5, 10, 10);

         if ( m_gp != null )
         {
            m_gp.DrawPath( g, m_penRed );
         }
/*
         if ( m_points != null )
         {
            g.TranslateTransform(Width / 2 - tileX * 256, Height / 2 - tileY * 256);
            g.DrawLines(m_penRed, m_points);
            g.ResetTransform();
         }
 */ 
      }

      private Bitmap m_backBuffer;
      Graphics m_g;
      protected override void OnPaint(PaintEventArgs pe)
      {
         if (m_backBuffer == null)
         {
            m_backBuffer = new Bitmap(Width, Height);
            m_g = Graphics.FromImage(m_backBuffer);  
         }

         GetOpenStreetMap(m_g, m_latitude, m_longitude, m_zoom);
         pe.Graphics.DrawImageUnscaled(m_backBuffer, 0, 0);

         // Calling the base class OnPaint
         base.OnPaint(pe);
      }

      private void PictureBoxEx_SizeChanged(object sender, EventArgs e)
      {
         if (m_backBuffer != null)
         {
            m_g.Dispose();
            m_backBuffer.Dispose();
            m_backBuffer = null;
         }
      }


      public void SetPos(float lon, float lat)
      {
         m_longitude = lon;
         m_latitude = lat;
         PreTransform();
      }

      public void SetPosFromIndex(int i)
      {
         m_longitude = m_gp.m_pointsF[i].Y;
         m_latitude = m_gp.m_pointsF[i].X;
         PreTransform();
      }

      public void SetOffset(float X, float Y)
      {
         m_MouseX = X;
         m_MouseY = Y;
         PreTransform();
      }

      public bool SetZoom( int zoom )
      {
         if (zoom > 18)
            return false;

         int zoomDelta = m_zoom - zoom;
         if (zoomDelta > 0)
         {
            m_MouseX /= 1 << (zoomDelta);
            m_MouseY /= 1 << (zoomDelta);
         }
         else
         {
            m_MouseX *= 1 << (-zoomDelta);
            m_MouseY *= 1 << (-zoomDelta);
         }
         m_zoom = zoom;

         PreTransform();
         return true;
      }

      public void PreTransform()
      {
         if (m_gp == null)
            return;

         PointF p = new PointF();
         GeoToScreen(m_latitude, m_longitude, ref p);

         p.X *= 256 * (1 << m_zoom);
         p.Y *= 256 * (1 << m_zoom);

         p.X -= m_MouseX;
         p.Y -= m_MouseY;

         float z = 256 * (1 << m_zoom);

         float xx = Width / 2 - p.X;
         float yy = Height / 2 - p.Y;

         m_gp.Set(z, xx, yy, 0);
      }

      public void AddPath(Path gp)
      {
         //normalize
         for (int i = 0; i < gp.m_pointsF.Count; i++)
         {
            PointF p = gp.m_pointsF[i];
            GeoToScreen(gp.m_pointsF[i].X, gp.m_pointsF[i].Y, ref p);

            gp.m_pointsF[i] = p;
         }
         m_gp = gp;
      }

      protected override void OnPaintBackground(PaintEventArgs e)
      {
         //Do nothing
      }

      private void PictureBoxEx_MouseDown(object sender, MouseEventArgs e)
      {
         m_mouse = e;
      }

      private void PictureBoxEx_MouseMove(object sender, MouseEventArgs e)
      {
         if (m_mouse != null && e.Button == MouseButtons.Left)
         {
            m_MouseX += (e.X - m_mouse.X);
            m_MouseY += (e.Y - m_mouse.Y);
            m_mouse = e;
            PreTransform();
            Invalidate();
         }
         else
         {
            m_mouse = e;
         }
      }




   }
}
