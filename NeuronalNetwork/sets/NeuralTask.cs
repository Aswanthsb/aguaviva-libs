using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class NeuralTask
   {
      protected double[][] trainingSet;
      protected double[][] expected;

      virtual public void Init() 
      {
      }

      virtual public int [] GetTopology()
      {
         return null;
      }

      virtual public int GetSetLength()
      {
         return this.trainingSet.Length;
      }

      virtual public double[] GetTrainingSet(int i)
      {
         return this.trainingSet[i];
      }

      virtual public double[] GetExpected(int i)
      {
         return this.expected[i];
      }

      virtual public string Print() 
      {
         return "none";
      }

   }
}
