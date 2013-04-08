using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class NeuralTaskEx : NeuralTask
   {
      Random rnd = new Random();

      double[] random;
      double[] randomExpected;

      override public void Init()
      {
         random = new double[trainingSet[0].Length];
         randomExpected = new double[expected[0].Length];
      }

      double[] GetRandomPattern()
      {
         for (int y = 0; y < 10 * 10; y++)
         {
            random[y] = rnd.NextDouble();
         }

         return random;
      }

      override public int GetSetLength()
      {
         return this.trainingSet.Length * 2;
      }

      override public double[] GetTrainingSet(int i)
      {
         if ((i & 1) == 0)
         {
            return this.trainingSet[i / 2];
         }
         else
         {
            return GetRandomPattern();
         }
      }

      override public double[] GetExpected(int i)
      {
         if ((i & 1) == 0)
         {
            return this.expected[i / 2];
         }
         else
         {
            return randomExpected;
         }
      }


   }
}
