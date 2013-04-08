using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace gpsmaps
{
   public partial class Form1 : Form
   {
      string m_mapsPath = "c:\\";// "\\Tarjeta de almacenamiento\\mapas\\";
      string m_logsPath = "c:\\";// "\\Tarjeta de almacenamiento\\mapas\\";


      void Loader()
      {
         string filename = "L:\\Users\\raguaviv\\Desktop\\gpslogs\\201008201052550000.gpx";
         //string filename = "L:\\Users\\raguaviv\\Desktop\\gpslogs\\201007211825070000";
         //string filename = openFileDialog1.FileName;
         try
         {
            Path gp = new Path();

            gp.LoadPath(filename);
            pictureBox1.AddPath(gp);
            graph1.AddPath(gp);

         }
         catch (Exception ex)
         {
            MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
         }
      }


      public Form1()
      {
         InitializeComponent();

         Loader();
         pictureBox1.SetCacheDir(m_mapsPath);
         //string[] theSerialPortNames = System.IO.Ports.SerialPort.GetPortNames();
         //serialPort1.Open();
         
      }

      private void trackBar1_Scroll(object sender, EventArgs e)
      {
         pictureBox1.SetZoom(trackBar1.Value);

         Redraw();
      }

      private void Redraw()
      {
         pictureBox1.Invalidate();
      }

      float m_longitude = 48.8411f;
      float m_latitude = 2.356567f;

      private void pictureBox1_Paint(object sender, PaintEventArgs e)
      {
         label2.Text = m_latitude.ToString() + " " + m_longitude.ToString();
      }


      string m_LogFilename;
      List<string> m_Log = new List<string>();

      void LogData(string str)
      {
         m_Log.Add(str);
         if (m_Log.Count > 30)
         {
            LogSave();
         }
      }

      void LogSave()
      {
         StreamWriter LogStreamWriter = new StreamWriter(m_logsPath + m_LogFilename, true, Encoding.ASCII);
         foreach (string s in m_Log)
         {
            LogStreamWriter.WriteLine(s);
         }
         LogStreamWriter.Close();
         m_Log.Clear();
      }

      private void Logging_CheckedChanged(object sender, EventArgs e)
      {
         if (Logging.Checked == false)
         {
            LogData("stop");
            LogSave();
         }
         else 
         {
            m_LogFilename = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            FileStream fs = new FileStream(m_logsPath + m_LogFilename, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();

            LogData("start");

         }
      }

      private void button1_Click(object sender, EventArgs e)
      {
         Redraw();
      }


      private void graph1_GPSMove_1(object sender, int idex)
      {
         pictureBox1.SetPosFromIndex(idex);
      }

   }
}

