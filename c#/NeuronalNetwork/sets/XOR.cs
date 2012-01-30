using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class XOR : NeuralTask
   {
      override public void Init()
      {
         trainingSet = new double[4][];
         expected = new double[trainingSet.Length][];

         for (int i = 0; i < 4; i++)
         {
            trainingSet[i] = new double[2];
            byte i1 = (byte)(i & 1);
            byte i2 = (byte)((i & 2) >> 1);
            trainingSet[i][0] = (double)i1;
            trainingSet[i][1] = (double)i2;

            expected[i] = new double[1];
            expected[i][0] = i1 ^ i2;
         }
      }

      override public int[] GetTopology()
      {
         int inputLength = trainingSet[0].Length;
         int outputLength = expected[0].Length;

         return new int[] { inputLength, 2, outputLength };
      }

/*
      override public string Print()
      {
         string o = "";
         for (int j = 0; j < 4; j++) 
         {
            n.Forward(trainingSet[j]);

            for (int i = 0; i < input.Length; i++)
            {
               o += input[i] + ", ";
            }
            o += "   =>" + expected[0];
            o+= "     " + n.GetResult(0) + "\r\n";
         }
         return o;
      }
*/ 
   }
}
