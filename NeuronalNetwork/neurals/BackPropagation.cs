using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class BackPropagation : NeuronalNetwork
   {
      double[] neuronWeightsCorrections;
      double[] neuronDeltas;

      double[] neuronWeightsMomentums;

      override public void Init(double[] input, int[] tp, double[] nw)
      {
         base.Init(input, tp, nw);
         neuronDeltas = new double[neuronValues.Length];
         neuronWeightsCorrections = new double[neuronWeights.Length];

         //initialize momentums
         neuronWeightsMomentums = new double[neuronWeights.Length];
         for (int i = 0; i < neuronWeights.Length; i++)
         {
            neuronWeightsMomentums[i] = 0;
         }
      }

      public double BackPropagate(double[] input, double[] expected)
      {
         double err = ComputeOutputError(expected, 0, expected.Length, neuronValues, neuronLayerReverseOffset[0], topologyReverse[0]);

         for (int i = 0; i < topology.Length - 1; i++)
         {
            CorrectWeights(neuronValues, neuronLayerReverseOffset[i], topologyReverse[i], weightsLayerReverseOffset[i], neuronValues, neuronLayerReverseOffset[i + 1], topologyReverse[i + 1]);
            CorrectNeuronOutputs(neuronLayerReverseOffset[i], topologyReverse[i], weightsLayerReverseOffset[i], neuronLayerReverseOffset[i + 1], topologyReverse[i + 1]);
         }

         CorrectWeights(neuronValues, neuronLayerOffset[0], topology[0], weightsLayerOffset[0], input, 0, input.Length);

         return err;
      }

      double ComputeOutputError(double[] expected, int expected_offset, int expected_len, double[] output, int output_offset, int output_len)
      {
         double total = 0;
         for (int i = 0; i < expected_len; i++)
         {
            double err = (expected[expected_offset + i] - output[output_offset]);
            neuronDeltas[output_offset + i] = prefix(output[output_offset + i]) * err;
            total += Math.Abs(err);
         }
         return total;
      }

      void CorrectWeights(double[] curr, int curr_offset, int curr_len, int weights_offset, double[] prev, int prev_offset, int prev_len)
      {
         for (int i = 0; i < curr_len; i++)
         {
            neuronWeightsCorrections[weights_offset] = neuronWeights[weights_offset] + neuronDeltas[curr_offset + i] * 1;
            weights_offset++;
            for (int j = 0; j < prev_len; j++)
            {
               double corr = neuronWeights[weights_offset] + neuronDeltas[curr_offset + i] * prev[prev_offset + j];
               neuronWeightsCorrections[weights_offset] = corr + (0.8 * neuronWeightsMomentums[weights_offset]);

               neuronWeightsMomentums[weights_offset] = (corr - neuronWeights[weights_offset]);

               weights_offset++;
            }
         }
      }

      void CorrectNeuronOutputs(int source_offset, int source_len, int weights_offset, int target_offset, int target_len)
      {
         for (int i = 0; i < target_len; i++)
         {
            double delta = 0;

            for (int j = 0; j < source_len; j++)
            {
               double d = neuronDeltas[source_offset + j];
               double w = neuronWeightsCorrections[weights_offset + j * (target_len + 1) + (i + 1)];
               delta += d * w;
            }
            neuronDeltas[target_offset + i] = prefix(neuronValues[target_offset + i]) * delta;
         }
      }

      public void UpdateWeights()
      {
         for (int i = 0; i < neuronWeightsCorrections.Length; i++)
         {
            neuronWeights[i] = neuronWeightsCorrections[i];
         }
      }

      override public string GetWeigthInfo(int i)
      {
         return string.Format("{0:#0.000} ({1:#0.000}) ", this.neuronWeights[i], this.neuronWeightsCorrections[i]);
      }

      override public string GetNeuronInfo(int i)
      {
         return string.Format("{0:#0.000} ({1:#0.000})", this.neuronValues[i], this.neuronDeltas[i]);
      }


   }
}
