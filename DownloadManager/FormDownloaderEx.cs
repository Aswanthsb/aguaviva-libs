using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.IO;

namespace WindowsFormsApplication1
{
 
   public class FormDownloaderEx : Form
   {
      
      static int totalActiveDownloads = 0;

      const int BUFFER_SIZE = 1024;
      const int DefaultTimeout = 5000; // 2 minutes timeout

      /// <summary>
      /// Progress callback, called when we've read another packet of data.
      /// It get's the GUIs thread and calls pd 
      /// </summary>
      private void Progress(ProgressDelegate pd, DownloadState ds)
      {
         if (this.InvokeRequired)
         {
            ProgressFormDelegate del = new ProgressFormDelegate(Progress);
            this.Invoke(del, new object[] { pd, ds });
         }
         else
         {
            pd(ds);
         }
      }

      // Abort the request if the timer fires.
      private static void TimeoutCallback(object state, bool timedOut)
      {
         if (timedOut)
         {
            HttpWebRequest request = state as HttpWebRequest;
            if (request != null)
            {
               //this will generate an exception
               request.Abort();
            }
         }
      }

      public DownloadState Download(string url, ProgressDelegate pd)
      {
         RequestState myRequestState = new RequestState();

         try
         {
            // Create a HttpWebrequest object to the desired URL. 
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            // Create an instance of the RequestState and assign the previous myHttpWebRequest
            // object to its request field.  
            myRequestState.request = myHttpWebRequest;
            myRequestState.progFormCB = new ProgressFormDelegate(Progress);
            myRequestState.progCB = new ProgressDelegate(pd);

            // Start the asynchronous request.
            IAsyncResult result = (IAsyncResult)myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);

            // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

            // Release the HttpWebResponse resource.
            totalActiveDownloads++;

            return (DownloadState)myRequestState;
         }
         catch (WebException e)
         {
            myRequestState.error = e;
            myRequestState.progFormCB(myRequestState.progCB, myRequestState);
            //Console.WriteLine("\nMain Exception raised!");
            //Console.WriteLine("\nMessage:{0}", e.Message);
            //Console.WriteLine("\nStatus:{0}", e.Status);
            //Console.WriteLine("Press any key to continue..........");
         }
         /*
         catch (Exception e)
         {
            Console.WriteLine("\nMain Exception raised!");
            Console.WriteLine("Source :{0} ", e.Source);
            Console.WriteLine("Message :{0} ", e.Message);
            Console.WriteLine("Press any key to continue..........");
            //Console.Read();
         }
         */

         return null;
      }

      private static void RespCallback(IAsyncResult asynchronousResult)
      {
         RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;

         try
         {
            // State of request is asynchronous.            
            HttpWebRequest myHttpWebRequest = myRequestState.request;
            myRequestState.response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);

            // Read the response into a Stream object.
            Stream responseStream = myRequestState.response.GetResponseStream();
            myRequestState.streamResponse = responseStream;

            myRequestState.BufferRead = new byte[myRequestState.response.ContentLength];

            myRequestState.progFormCB(myRequestState.progCB, myRequestState);

            int Dataleft = (int)myRequestState.DataLeft();
            if (Dataleft >= BUFFER_SIZE)
               Dataleft = BUFFER_SIZE;

            // Begin the Reading of the contents of the HTML page and print it to the console.
            IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, myRequestState.bytesRead, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
            return;
         }
         catch (WebException e)
         {
            myRequestState.error = e;
            myRequestState.progFormCB(myRequestState.progCB, myRequestState);
            //Console.WriteLine("\nRespCallback Exception raised!");
            //Console.WriteLine("\nMessage:{0}", e.Message);
            //Console.WriteLine("\nStatus:{0}", e.Status);
         }
         totalActiveDownloads--;
      }

      private static void ReadCallBack(IAsyncResult asyncResult)
      {
         RequestState myRequestState = (RequestState)asyncResult.AsyncState;
         try
         {
            Stream responseStream = myRequestState.streamResponse;
            int read = responseStream.EndRead(asyncResult);
            // Read the HTML page and then print it to the console.
            if (read > 0)
            {
               myRequestState.bytesRead += read;
               myRequestState.progFormCB(myRequestState.progCB, myRequestState);

               int Dataleft = (int)myRequestState.DataLeft();
               if (Dataleft >= BUFFER_SIZE)
                  Dataleft = BUFFER_SIZE;

               IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.BufferRead, myRequestState.bytesRead, Dataleft, new AsyncCallback(ReadCallBack), myRequestState);
               return;
            }
            else
            {
               responseStream.Close();
            }

         }
         catch (WebException e)
         {
            myRequestState.error = e;
            myRequestState.progFormCB(myRequestState.progCB, myRequestState);
            //Console.WriteLine("\nReadCallBack Exception raised!");
            //Console.WriteLine("\nMessage:{0}", e.Message);
            //Console.WriteLine("\nStatus:{0}", e.Status);
         }
         totalActiveDownloads--;
      }
   }

}
