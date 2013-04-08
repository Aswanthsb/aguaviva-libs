using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
   public partial class Form1 : FormDownloader
   {
      DownloadManager dw = new DownloadManager();

      public Form1()
      {
         InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         DownloadState ds = dw.Download(urlBox.Text, this, new ProgressDelegate(Progress1));
      }

      public void Progress1( DownloadState ds )
      {
         if ( ds.error != null )
         {
            label1.Text = ds.error.Status + "  "+ ds.error.Message;
            return;
         }

         progressBar1.Maximum = (int)ds.BufferRead.Length;
         progressBar1.Value = ds.bytesRead;
         label1.Text = ds.Progress().ToString();

         if ( ds.DataLeft() == 0 )
         {
            dw.Download(urlBox.Text, this, Progress2);
         }
      }

      public void Progress2( DownloadState ds )
      {
         if (ds.error != null)
         {
            label2.Text = ds.error.Status + "  " + ds.error.Message;
            return;
         }

         progressBar2.Maximum = (int)ds.BufferRead.Length;
         progressBar2.Value = ds.bytesRead;
         label2.Text = ds.Progress().ToString();

         if (ds.Progress() == 50)
         {
            ds.Abort();
         }
      }

      private void button2_Click(object sender, EventArgs e)
      {
         dw.AbortAll();
      }
   }
}
