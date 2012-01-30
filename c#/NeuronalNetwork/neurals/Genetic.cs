using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class Genetic : NeuronalNetwork
   {
      public UInt64[] GetGens()
      {
         UInt64 [] genes = new UInt64[neuronWeights.Length];
         for(int i=0;i<neuronWeights.Length;i++)
         {
            genes[i] = (UInt64)BitConverter.DoubleToInt64Bits(neuronWeights[i]);
         }
         return genes;
      }

      public void SetGens(Int64[] gens)
      {
         UInt64[] genes = new UInt64[neuronWeights.Length];
         for (int i = 0; i < neuronWeights.Length; i++)
         {
            neuronWeights[i] = BitConverter.Int64BitsToDouble((Int64)genes[i]);
         }
      }

      public double MixDoubles(int splitPoint, double f1, double f2)
      {
         /*
         UInt64 mask = (UInt64)((1 << splitPoint) - 1);
         UInt64 p1 = (UInt64)BitConverter.DoubleToInt64Bits(f1) & mask;
         UInt64 p2 = (UInt64)BitConverter.DoubleToInt64Bits(f2) & ~mask;
         double f = BitConverter.Int64BitsToDouble((Int64)(p1 | p2));
         */
         return (f1*splitPoint +  f2*(64-splitPoint))/64;
      }

      public void Init(Genetic n1, Genetic n2)
      {
         this.neuronWeights = new double[n1.neuronWeights.Length];
         this.topology = n1.topology;
         this.neuronLayerOffset = n1.neuronLayerOffset;
         this.weightsLayerOffset = n1.weightsLayerOffset;
         this.neuronLayerReverseOffset = n1.neuronLayerReverseOffset;
         this.topologyReverse = n1.topologyReverse;
         this.neuronValues = new double[n1.neuronValues.Length];

         int splitpoint = (int)(RandomNumber.NextDouble() * (n1.neuronWeights.Length - 1) * 64);

         int point = splitpoint / 64;
         int intrapoint = splitpoint % 64;

         int index = 0;
         for (; index < point; index++)
         {
            neuronWeights[index] = n1.neuronWeights[index];
         }

         neuronWeights[index] = MixDoubles(intrapoint, n1.neuronWeights[index],n2.neuronWeights[index]);
         index++;

         for (; index < neuronWeights.Length; index++)
         {
            neuronWeights[index] = n2.neuronWeights[index];
         }
      }

      public void Mutate()
      {         
         int index = (int)(RandomNumber.NextDouble() * (neuronWeights.Length-1));
         double muta = RandomNumber.NextDouble() * .0000000001;
         neuronWeights[index] += muta;
         /*
         int splitpoint = (int)(RandomNumber.NextDouble() * neuronWeights.Length * 64);

         int point = splitpoint / 64;
         int intrapoint = splitpoint % 64;

         UInt64 mask = (UInt64)((1 << intrapoint));

         UInt64 bin = (UInt64)BitConverter.DoubleToInt64Bits(neuronWeights[point]);

         if (bin == 0)
         {
            bin |= mask;            
         }
         else
         {
            bin &= ~mask;
         }

         neuronWeights[point] = BitConverter.Int64BitsToDouble((Int64)bin);
          */
      }

   }
}
