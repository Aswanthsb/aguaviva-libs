using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class Seno : NeuralTask
   {
      Random r = new Random();

      override public void Init()
      {
         trainingSet = new double[10000][];
         expected = new double[trainingSet.Length][];

         int inputLength = 10;

         for (int i = 0; i < trainingSet.Length; i++)
         {
            trainingSet[i] = new double[inputLength];
            
            expected[i] = new double[1];

            if ((i & 1) == 1)
            {
               double off = Math.PI * r.NextDouble();

               for (int t = 0; t < inputLength; t++)
               {
                  double alfa = (t * 2 * Math.PI) / (inputLength);
                  trainingSet[i][t] = Math.Cos(alfa + off);
               }

               expected[i][0] = 1;
            }
            else
            {
               for (int t = 0; t < inputLength; t++)
               {
                  trainingSet[i][t] = r.NextDouble() * 2 - 1;
               }
               expected[i][0] = 0;
            }
         }

      }

      override public int[] GetTopology()
      {
         int inputLength = trainingSet[0].Length;
         int outputLength = expected[0].Length;

         return new int[] { inputLength, 5, 3, outputLength };
      }

   }
}
