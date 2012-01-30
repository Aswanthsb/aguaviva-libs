using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{
   public delegate void ProgressDelegate( DownloadState ds );
   public delegate void ProgressFormDelegate(ProgressDelegate pd, RequestState ds);

   public class DownloadState
   {
      // This class stores the State of the request.
      public byte[] BufferRead;
      public int bytesRead;           // # bytes read during current transfer

      public WebException error;

      public DownloadState()
      {
         bytesRead = 0;
         BufferRead = null;
      }
      public int Progress()
      {
         return (100 * bytesRead) / BufferRead.Length;
      }
      public int DataLeft()
      {
         return BufferRead.Length - bytesRead;
      }
      public virtual void Abort()
      {
      }
   }

   public class RequestState : DownloadState
   {
      public HttpWebRequest request;
      public HttpWebResponse response;
      public Stream streamResponse;
      public Control ctrl;
      public DownloadManager downloadManager;
      public ProgressDelegate progCB;
      public ProgressFormDelegate progFormCB;
      public RequestState()
      {
         request = null;
         streamResponse = null;
      }
      public override void Abort()
      {
         request.Abort();
      }
   }

}
