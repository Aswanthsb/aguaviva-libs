﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace decodebitfield
{
   public partial class Form1 : Form
   {
      List<string> strs = new List<string>();
      byte[][] nibblesList;

      public void identicos4()
      {
         nibblesList = new byte[4][];

         /*
         nibblesList[0] = stringToBytes("1011000001111001100010001010101011011110110111110111100110011010111111111111111111011100110111010001101111101111011101010101010101010101111110101010101000101110011010100100001101110100000110111111001100110001011001101011");
         nibblesList[1] = stringToBytes("1011100101110001100110011101110100111111001101110110000100011101011101110111011100111011001100110000010111110111011000100010001000100010011111011101110111001111111011011010010001101010000001010111010001000000111011101011");
         nibblesList[2] = stringToBytes("1011000001111101110011001110111010011010100110110011110111011110101110111011101110011000100110010101111110101011001100010001000100010001101111101110111001101010001011100000011100110000010111111011011101110101001000101011");
         nibblesList[3] = stringToBytes("1011000011100000100010001100110000101110001001100111000000001100011001100110011000101010001000100001010011100110011100110011001100110011011011001100110011011110111111001011010101111011000101000110010101010001111111111011");
         */

         /*
         nibblesList[0] = stringToBytes("01101110111010101010010010000100000000010110");
         nibblesList[1] = stringToBytes("10000000000001000100101001101010111011111000");
         nibblesList[2] = stringToBytes("00001000100011001100001011100010011001110000");
         nibblesList[3] = stringToBytes("10100010001001100110100001001000110011011010");
          */

         nibblesList[0] = stringToBytes("0101011111010110111011101010101001001000010000000001011010100110110011001100110010000000100010111000110101111111111010111011101110111011111001000100010001010110011101000011110111110011100111001110110111011001011101110101");
         nibblesList[1] = stringToBytes("0000010000001000000000000100010010100110101011101111100010100110110011001100110010000000100011001111101000001000100111001100110011001100100100110011001100100001000000110100101010000100111010111001101010101110000000000000");
         nibblesList[2] = stringToBytes("0000110100010000100010001100110000101110001001100111000010100110110011001100110010000000100011011110101100011001100011011101110111011101100000100010001000110000000100100101101110010101111110101000101110111111000100010000");
         nibblesList[3] = stringToBytes("1110001111011010001000100110011010000100100011001101101010100110110011001100110010000000100010001011111001001100110110011001100110011001110001100110011001110100010101100001111111010001101111101100111111111011010101011110");

         PrintVertical(nibblesList);

         int max = 0;

         byte[] key = new byte[4];

         byte[][] tables = new byte[4][];
         for (byte i = 0; i < 16; i++)
         {
            for (byte j = 0; j < 16; j++)
            {
               for (byte k = 0; k < 16; k++)
               {
                  for (byte l = 0; l < 16; l++)
                  {
                     tables[0] = encodeXor(nibblesList[0], 3, 11, i);
                     tables[1] = encodeXor(nibblesList[1], 3, 11, j);
                     tables[2] = encodeXor(nibblesList[2], 3, 11, k);
                     tables[3] = encodeXor(nibblesList[3], 3, 11, l);

                     int v = compare(tables, 4);

                     if (v >= max)
                     {
                        max = v;
                        key[0] = i;
                        key[1] = j;
                        key[2] = k;
                        key[3] = l;
                        //textBox1.Text += max.ToString() + "\r\n";
                     }
                     if (v == 11)
                     {
                        textBox1.Text += "--------------\r\n";
                        textBox1.Text += ">" + key[0].ToString() + "   " + key[1].ToString() + "   " + key[2].ToString() + "   " + key[3].ToString() + "\r\n";
                        textBox1.Text += max.ToString() + "\r\n";
                        PrintVertical(tables);

                     }
                  }
               }
            }
         }
         /*
         textBox1.Text += max.ToString() + "\r\n";
         textBox1.Text += key[0].ToString() + "   " + key[1].ToString() + "   " + key[2].ToString() + "   " + key[3].ToString() + "\r\n";

         for( int i=0;i<4;i++)
            tables[i] = encodeXor(nibblesList[i], key[i]);

         Print(tables);
         */
      }
      /*
      public void nuevos()
      {
         nibblesList = new byte[2][];
         nibblesList[0] = stringToBytes("1011000010111111111111111111111100010001000100010001000100010001000100010001000101011101010101010101010101010101010101010000101010101010101010101010101010111000100010111100001000001100011000110001001000100110100010001011");
         // 10 viajes
         nibblesList[1] = stringToBytes("1011111110011010101010101010101000110011001100110011001100110011001100110011101110110001100110011001100110011001100110011110001000000000000000000000000001110101101110001010011100000101101000010011001000100110100010001011");
         Print(nibblesList);

         textBox1.Text += "--------------\r\n";

         byte[][] tables = new byte[2][];
         tables[0] = encodeXor(nibblesList[0], nibblesList[0][21]);
         tables[1] = encodeXor(nibblesList[1], nibblesList[1][21]);
         Print(tables);

         textBox1.Text += "--------------\r\n";


         byte[][] tables2 = new byte[2][];
         tables2[0] = encodeXor(tables[0], 13);
         tables2[1] = encodeXor(tables[1], 15);
         Print(tables2);
    
      
      }
*/
      public void nuevos()
      {

         List<string> strs = new List<string>();
         /*
         //serial number: 034.880.959     <---- this a number all the tickets have, please see attached picture!
         //punched on:    260108-1603-835   <------ this is what the obliterator prints on the ticket when you punch it.
         strs.Add("1101001100110100011001101010100000000100100001011000011110111111111111111111111111000100010001000100010000101010100011001110111000100011010101100110011001100010001000011111001110011001111100000001001100110010011101101101");

         //serial number: 064.802.813
         //punched on:    130809-1532-m00
         strs.Add("1101111111111000101010100110010111100010000111100001110000000100010001000100010001111111111111111111111100110101010100011100001000100011011001010101010101010110000100011111011110110101010010111010100010001001000100001101");

         //serial number: 043.471.026
         //punched on:    130809-2211-m42
         strs.Add("1101010101010010000000001100111001110000101010100111010110011101110111011101110111100110011001100110011010101100110010000011101101100111000100100011011000100111011011010011101110011010100101100111010101010100101001101101");
         
         //serial number: 064.157.428
         //punched on:    140809-1655-679
         strs.Add("1101101110111100111011100010000110101000011100011110101001100010001000100010001000011001100110011001100101011111111110111000101101010100000100100010001000100110101011010011111100111111100001110110010001000101011011111101");

         //serial number: 064.875.476
         //punched on:    140809-1905-m42
         strs.Add("1101101010100100000000000011111100101001011110100111010101100100010001000100010010001001100110011001100110101111111111010001001001111111010110011001100110011011011111011010101111110011111100001000110011000100110110011101");

         //serial number: 049.127.916
         //punched on:    140809-1906-m42
         strs.Add("1101001000101100100010001011111100011010000010000110010101100100010001000100010010001001100110011001100110101111111111010001001001001100010010001000100010001010011000000111011000101110001011010101000100011001100001101101");
         


         //180210-2300-M42
         strs.Add("1011000001111001100010001010101011011110110111110111100110011010111111111111111111011100110111010001101111101111011101010101010101010101111110101010101000101110011010100100001101110100000110111111001100110001011001101011");
         strs.Add("1011100101110001100110011101110100111111001101110110000100011101011101110111011100111011001100110000010111110111011000100010001000100010011111011101110111001111111011011010010001101010000001010111010001000000111011101011");
         strs.Add("1011000001111101110011001110111010011010100110110011110111011110101110111011101110011000100110010101111110101011001100010001000100010001101111101110111001101010001011100000011100110000010111111011011101110101001000101011");
         strs.Add("1011000011100000100010001100110000101110001001100111000000001100011001100110011000101010001000100001010011100110011100110011001100110011011011001100110011011110111111001011010101111011000101000110010101010001111111111011");


         // 220210-2301-M42 -->  2301 2302 2303 2304 2305
         strs.Add("1011011010001101010101010001000111110011111110111010110101011001001100110011001101111111011101100101000010100010001101100110011001100110001110011001100110001011101010011110000000101110010000010011000000000100101010101011");
         strs.Add("1011100110110111111111111011101101011001010100010000011100111111010101010101010100011001000100110000010111110111011000110011001100110011011011001100110011011110111111001011010101111011000101000110010101010001111111111011");
         strs.Add("1011100100111000000000000100010010100110101011101111100001001000001000100010001001101110011001010110001110010001000001010101010101010101000010101010101010111000100110101101001100011101011100100000001100110111100110011011");
         strs.Add("1011111110110011101110111111111100011101000101010100001100011101011101110111011100111011001101110100000110110011001001110111011101110111001010001000100010011010101110001111000100111111010100000010000100010101101110111011");
         strs.Add("1011011010101011001100110111011110010101100111011100101100011101011101110111011100111011001101100101000010100010001101100110011001100110001110011001100110001011101010011110000000101110010000010011000000000100101010101011");

         // 230210-0001-M42  -->0001 0002 0003 005
         strs.Add("1011011000011011001100110111011110010101100111011100101100110011001100110011001101111111011101100110011011000100011100100010001000100010011111011101110111001111111011011010010001101010000001010111010001000000111011101011");
         strs.Add("1011111100100110011101110101010100100001001000001000011001000100010001000100010001100111011000100010001001110110101000000000000000000000101011111111111101111011001111110001011000100001010011101010011001100100001100111011");
         strs.Add("1011111100111000100110011011101111001111110011100110100010111011101110111011101110011000100101010101010100000001110101110111011101110111110110001000100000001100010010000110000101010110001110011101000100010011010001001011");
         strs.Add("1011111110101100010001000000000011100010111010101011110001100110011001100110011000101010001001110111011111010101011000110011001100110011011011001100110011011110111111001011010101111011000101000110010101010001111111111011");

         //230210-2154-M42
         strs.Add("1011000011010110111011101010101001001000010000000001011000000111110111011101110110010001100111110001010011100110010100000000000000000000010111111111111111101101110011111000011001001000001001110101011001100010110011001011");

         //230210-2155-M42
         strs.Add("1011100111001000000000000100010010100110101011101111100001100001101110111011101111110111111110000110001110010001001001110111011101110111001010001000100010011010101110001111000100111111010100000010000100010101101110111011");

         //230210-2157-M42
         strs.Add("1011111110110011001000100000000001110100011101011101001110100100000100010001000100110010001110101101011100100011111101010101010101010101111110101010101000101110011010100100001101110100000110111111001100110001011001101011");

         //230210-2201-M42
         strs.Add("1011011010101101010101010001000111110011111110111010110101011101011101110111011100111011001100100011011011000100011100100010001000100010011111011101110111001111111011011010010001101010000001010111010001000000111011101011");
         */
         //
         strs.Add("1011011000010000011101110101010111001011000100111011010111010011111011101101110011001001100011010011100011011111000111011100001000110011111001000111011110011101001001000000101101011111000011011001000100010011010001001011");
         strs.Add("1011100111100111011101110111111111111011101110111011101101111001000100010001010101000110011001010010101100010001011000100010001000100010001111111111111110011001100010101010101010101010101010101110111111111011101110111011");
         strs.Add("1011111100101111010001000110011011111111110111100010110000110000010101000100010101010000000111101000001101100111101000000000000000000000111011011001100101110011010001010001101001001110000111001000000000000010010101011011");
         strs.Add("1011011001101101111011101010101000111101101010011000111100011101011100010001100110010011101100010111101000001100011100010001000100010001011010101000100011111101001110111001010000110110100100100000000100010101101110111011");
         strs.Add("1011111110100101000000000100010011010001110110011000111111000001101011111101010101011111011101011001001010000000011101100110011001100110000111011111111110001010010011001110001101000001111001010111011001100010110011001011");
         strs.Add("1011000010100000101110111111111101100111111110010101101101000001110011001101110011001001100001100011000001010110111100110011001100110011110111101010101001000000011101100010100101111101001011111011001100110001011001101011");
         strs.Add("1011100110110111111111111011001101101010011000100011010011101101000100010001100111010001100111000000101100011001111110101010101010101010100000110011001101110100101110001110000010001000111101010011000000000100101010101011");
         strs.Add("1011111100101001001100110111011111101101011100011101001110011001110100110000000100010100010100111010000101000110110001100110011001100110100010111111111100010101001000110111110000101000011110101110011001100100001100111011");
         strs.Add("1011000010110110001100110111011111100010111010101011110001011001010011101110011001101100010000100011100000100110000100010001000100010001011010101000100011111101001110111001010000110110100100100000000100010101101110111011");
         strs.Add("1011111101001010111111111011101101101010011000100011010010100010000010011101010111010001100100011001010011101010110011111111111111111111101101010111011101110111011111111000111110100000011100110101010001000000111011101011");
         strs.Add("1011100111100010100010001010101010011110111111010101101110000110110100111000101010001101110000100001010000010011000101010101010101010101101110001100110010100011010001010110011010111100101111011000000000000010010101011011");
         strs.Add("1011011001000010101010101110111000100110001001100110011000110000110011001100010001001110011011000000001110010001000000010101100100010001000100010001000101111100001001100011100010011101101101110101100101000010110011001011");
         strs.Add("1011100101111110110111011001100111100000001100000011010000111010011101010101010111010101110100001001000100110111110011001100110011001100101101110101010100010100101000100100110101010111010110100110011101110011110111011011");
         strs.Add("1011111100100111011001100100010000010110010001101110000010000110001100110011001000110000000100000111110010011000000101010101010101010101000111001100110011101100010010000001011001001100001111011011011101110101001000101011");
         strs.Add("1011111100001100000100010101010111001000100010001010001110001001001011011101010101011111011100110010100100111111011101010101010101010101001011101100110010111001011111111101000001110010110101100100010101010001111111111011");
         strs.Add("1011111101110011001100110011001110111111111111111111111100011101011101110111001100110001000101100101100000100110000001010101010101010101010010001000100010101010101110011001100110011001100110011101110011001000100010001011");

         int tickets = strs.Count;

         nibblesList = new byte[tickets][];
         for( int i=0;i<tickets;i++)
            nibblesList[i] = stringToBytes(strs[i]);

         for (int i = 0; i < tickets; i++)
         {
            byte lrc = 0;
            for (int j = 2; j < nibblesList[i].Length - 1; j++)
            {
               lrc = (byte)(lrc ^ nibblesList[i][j]);
            }
            textBox1.Text += lrc.ToString() + " ";
         }
         textBox1.Text += "\r\n";

         PrintVertical(nibblesList);
         textBox1.Text += "--------------\r\n";
         textBox1.Text += "keys\r\n";

         for (int i = 0; i < tickets; i++)
         {
            textBox1.Text += nibblesList[i][21].ToString() + "   ";
         }
         textBox1.Text += "\r\n";
   

         textBox1.Text += "--------------\r\n";

         byte[][] tables = new byte[tickets][];
         for (int i = 0; i < tickets; i++)
         {
            tables[i] = encodeXor(nibblesList[i], nibblesList[i][21]);
         }

         //PrintVertical(tables);

         textBox1.Text += "--------------\r\n";
         /*
         //08 01 01 01 08 01 08 08 08 08 08 08 08 01 01 08
         string s =  "1111000101111111001";
         //FindBitsAcross(tables, s);

         for (int i = 0; i < strs[0].Length; i++)
         {
            bool t = true;

            for (int j = 0; j < tickets; j++)
            {
               //string ss = strs[j];//
               string ss = bytesToString(tables[j]);

               if (ss[i] != s[j])
               {
                  t = false;
                  break;
               }
            }

            if (t)
            {
               textBox1.Text += (i ).ToString() + "   ";
            }
         }
         
         textBox1.Text += "\r\n";
         */
         for (int i = 0; i < tickets; i++)
         {
            tables[i] = encodeXor(nibblesList[i], nibblesList[i][21]);
            if (tables[i][20] != 1)
               tables[i] = reverseBits(tables[i]);
            tables[i] = encodeXor(tables[i], 3, 11, tables[i][3]);
            tables[i] = encodeXor(tables[i], 23, 6, tables[i][23]);
            tables[i] = encodeXor(tables[i], 29, 25, tables[i][29]);
         }

         //tables[i] = encodeXor(tables[i], 3, 29, tables[i][3]);

         {
            PrintVertical(tables);
         }

         /*
         textBox1.Text += "--------------\r\n";
         byte[][] tables2 = new byte[tickets][];
         tables2[0] = encodeXor(tables[0], 13);
         tables2[1] = encodeXor(tables[1], 15);
         Print(tables2);
         */

      }

      public Form1()
      {
         InitializeComponent();
         //identicos4();
         nuevos();
      }

      public byte computeLRC( byte [] data )
      {
         byte lrc = 0;
         for (int j = 0; j < data.Length ; j++)
         {
            lrc = (byte)(lrc ^ data[j]);
         }

         return lrc;
      }

      public string bytesToString( byte[] b )
      {
         string o = "";
         for (int i = 0; i < b.Length; i++)
         {
            o += ((b[i] & 8) == 0) ? "0" : "1";
            o += ((b[i] & 4) == 0) ? "0" : "1";
            o += ((b[i] & 2) == 0) ? "0" : "1";
            o += ((b[i] & 1) == 0) ? "0" : "1";
         }

         return o;
      }


      public byte[] stringToBytes(string s)
      {
         byte[] o = new byte[s.Length/4];

         for (int i = 0; i < s.Length; i += 4)
         {
            string bs = s.Substring(i, 4);
            byte b = 0;
            for (int j = 0; j < 4; j++)
            {
               b *= 2;
               if (bs[j] == '1')
                  b |= 1;
            }
            o[i / 4] = b;
         }

         return o;
      }
      /*
      public void FindBitsAcross( byte[][] tbl , string s )
      {
         for (int i = 0; i < tbl[0].Length; i++)
         {
            bool t = true;

            for (int j = 0; j < tbl.Length; j++)
            {
               string ss = bytesToString(tbl[j]);

               if (ss[i] != s[j])
               {
                  t = false;
                  break;
               }
            }

            if (t)
            {
               textBox1.Text += (i ).ToString() + "   ";
            }
         }
      }
       */


      public byte[] encodeXor(byte[] l, byte key)
      {
         return encodeXor(l, 0, l.Length, key);
      }

      public byte[] encodeXor(byte[] l, int ini, int len, byte key)
      {
         byte[] o = new byte[ l.Length ];
         for (int i = 0; i < l.Length; i++)
         {
            if ( i>= ini && i<ini+len )
               o[i] = (byte)(l[i] ^ key);
            else
               o[i] = l[i];
         }
         return o;
      }

      public byte[] reverseBits(byte[] b)
      {
         byte[] o = new byte[b.Length];
         for (int i = 0; i < b.Length; i++)
         {
            o[i] = 0;
            if ( (b[i] & 1) == 1)
               o[i] += 8;

            if ((b[i] & 2) == 2)
               o[i] += 4;

            if ((b[i] & 4) == 4)
               o[i] += 2;

            if ((b[i] & 8) == 8)
               o[i] += 1;
         }
         return o;
      }

      public int compare(byte[][] a, int k)
      {
         int res = 0;
         for (int j = 0; j < a[0].Length; j++)
         {
            byte b = a[0][j];
            bool cmp = true;
            for (int i = 1; i < k; i++)
            {
               if (b != a[i][j])
               {
                  cmp = false;
                  break;
               }
            }
            if (cmp == true)
               res++;
         }

         return res;
      }

      public void  PrintVertical(byte[][] a)
      {
         for (int i = 0; i < a[0].Length; i++)
         {
            for (int j = 0; j < a.Length; j++)
            {
               textBox1.Text += string.Format("{0:00} ", a[j][i]);
            }

            textBox1.Text += "\r\n";
         }
      }

      public void PrintHorizontalBin(byte[][] a)
      {
         for (int i = 0; i < a.Length; i++)
         {
            textBox1.Text += bytesToString(a[i]);

            textBox1.Text += "\r\n";
         }
      }

      private void listBox_SelectedIndexChanged(object sender, EventArgs e)
      {

      }
   }
}
