using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace test
{
   public class NeuronalNetwork
   {
      protected int[] topology;

      protected double[] neuronWeights;
      protected double[] neuronValues;

      protected List<int> weightsLayerOffset = new List<int>();
      protected int[] neuronLayerOffset;

      protected int[] weightsLayerReverseOffset;
      protected int[] neuronLayerReverseOffset;
      protected int[] topologyReverse;

      protected static Random RandomNumber;

      public NeuronalNetwork()
      {
         if (RandomNumber ==null)
         {
            RandomNumber = new Random();
         }
      }

      public virtual void Init(double[] input, int[] tp, double[] nw)
      {
         int totalWeights = 0;
         int totalNeurons = 0;
         int lastLayer = input.Length;

         topology = tp;

         neuronLayerOffset = new int[topology.Length];

         for (int i = 0; i < topology.Length; i++)
         {
            neuronLayerOffset[i] = totalNeurons;
            weightsLayerOffset.Add(totalWeights);

            totalNeurons += topology[i];

            totalWeights += topology[i]; // add the bias weight, one per neuron at that layer
            totalWeights += lastLayer * topology[i];            

            lastLayer = topology[i];
         }

         neuronValues = new double[totalNeurons];

         // reverse offsets to simplify computations
         weightsLayerReverseOffset = new int[weightsLayerOffset.Count];
         for (int i = 0; i < weightsLayerReverseOffset.Length; i++)
         {
            weightsLayerReverseOffset[i] = weightsLayerOffset[weightsLayerReverseOffset.Length - i - 1];
         }

         neuronLayerReverseOffset = new int[neuronLayerOffset.Length];
         for (int i = 0; i < neuronLayerReverseOffset.Length; i++)
         {
            neuronLayerReverseOffset[i] = neuronLayerOffset[neuronLayerReverseOffset.Length - i - 1];
         }

         topologyReverse = new int[topology.Length];
         for (int i = 0; i < topology.Length; i++)
         {
            topologyReverse[i] = topology[topology.Length - i - 1];
         }

         // randomize neuron weights if caller doesnt provide its own weights
         if (nw == null)
         {
            neuronWeights = new double[totalWeights];

            for (int i = 0; i < neuronWeights.Length; i++)
            {
               neuronWeights[i] = RandomNumber.NextDouble()-0.5;
            }
         }
         else
         {
            neuronWeights = nw;
         }

      }

      protected double sigmoid(double x)
      {
         return  1.0 / (1.0 + Math.Exp(-x));
      }

      protected double prefix(double x)
      {
         return  x * (1 - x);
      }

      public void Forward(double[] inputNeurons)
      {
         StepForward(inputNeurons, 0, inputNeurons.Length, 
                     this.neuronWeights, this.weightsLayerOffset[0],
                     this.neuronValues, this.neuronLayerOffset[0], topology[0]);

         for (int i = 0; i < topology.Length - 1; i++)
         {
            StepForward(this.neuronValues, this.neuronLayerOffset[i], topology[i], 
                        this.neuronWeights, this.weightsLayerOffset[i+1],
                        this.neuronValues, this.neuronLayerOffset[i + 1], topology[i + 1]);
         }
      }

      public void Load(double[] input, string file)
      {
         int n;
         StreamReader sr = new StreamReader(file);

         string[] line;
            
         line = sr.ReadLine().Split(',');
         
         n = int.Parse(line[0]);
         int [] tp = new int[n];
         for (int i = 0; i < n; i++)
         {
            tp[i] = int.Parse(line[i + 1]);
         }

         line = sr.ReadLine().Split(',');

         n = int.Parse(line[0]);
         double [] nw = new double[n];
         for (int i = 0; i < n; i++)
         {
            nw[i] = double.Parse(line[i + 1]);
         }

         Init(input, tp, nw);
         sr.Close();
      }

      public void Save(string file)
      {
         StreamWriter sw = new StreamWriter(file);

         sw.Write(topology.Length);
         for (int i = 0; i < topology.Length; i++)
         {
            sw.Write(",");
            sw.Write(topology[i]);
         }
         sw.Write("\n");

         sw.Write(neuronWeights.Length);
         for (int i = 0; i < neuronWeights.Length; i++)
         {
            sw.Write(",");
            sw.Write(neuronWeights[i]);
         }

         sw.Close();
      }

      void StepForward(double[] src, int src_offset, int src_len, double[] weights, int weights_offset, double[] dst, int dst_offset, int dest_len)
      {
         for (int w = 0; w < dest_len; w++)
         {
            double temp = weights[weights_offset++];
            for (int i = 0; i < src_len; i++)
            {
               temp += src[src_offset + i] * weights[weights_offset++];
            }
            dst[dst_offset++] = sigmoid(temp);
         }
      }

      public double GetError(double [] expected)
      {
         double err=0;
         for (int i = 0; i < expected.Length; i++)
         {
            err += Math.Abs(expected[i] - neuronValues[neuronLayerReverseOffset[0] + i]);
         }
         return err;
      }

      public double GetResult(int i)
      {
         return neuronValues[neuronLayerReverseOffset[0]+i];
      }

      public string PrintResults(int layer)
      {
         string tmp = "";
         int offset = 0;

         for (int i = 0; i < layer; i++)
         {
            offset += topology[i];
         }

         if (layer < topology.Length)
         {
            for (int i = 0; i < topology[layer]; i++)
            {
               tmp += neuronValues[offset + i] + " ";
            }
         }

         return tmp;
      }

      public string Print()
      {
         string tmp = "";

         for (int j = 0; j < weightsLayerOffset.Count-1; j++)
         {
            for (int i = this.weightsLayerOffset[j]; i < weightsLayerOffset[j+1]; i++)
            {
               tmp += neuronWeights[ i] + ", ";
            }
            tmp += " | ";
         }

         return tmp;
      }


      public virtual string GetWeigthInfo(int i)
      {
         return string.Format("{0:#0.000}", this.neuronWeights[i]);
      }

      public virtual string GetNeuronInfo(int i)
      {
         return string.Format("{0:#0.000}", this.neuronValues[i]);
      }

      public void Draw(PaintEventArgs e, Control f)
      {
         /*
         if (inputNeurons == null)
         {
            return;
         }

         int nnsize = 30;
         int xsepsize = 300;
         int ysepsize = 150;
         int height = f.Height-40-30;
         int nn = 0;

         float xx = f.Left+40;
         float yy = f.Top+20+30+50;

         e.Graphics.DrawRectangle(Pens.Black, f.Bounds);

         float off = yy + (height - (inputNeurons.Length * ysepsize + nnsize)) / 2;
         for (int i = 0; i < inputNeurons.Length; i++)
         {
            //off = height * i / (inputNeurons.Length+1);
            float y = off + i * ysepsize;
            e.Graphics.DrawEllipse(Pens.Black, xx, y, nnsize, nnsize);
            e.Graphics.DrawString(string.Format("{0:#0.000}", inputNeurons[i]), f.Font, Brushes.Black, xx - 40, y);
         }
         xx += xsepsize;

         for(int j=0;j<topology.Length;j++)
         {
            off = yy + (height - (topology[j] * ysepsize + nnsize)) / 2;
            for(int i=0;i<topology[j];i++)
            {
               float y = off + i * ysepsize;
               e.Graphics.DrawEllipse(Pens.Black, xx , y, nnsize, nnsize);

               //if (j == topology.Length - 1)
               {
                  e.Graphics.DrawString(GetNeuronInfo(this.neuronLayerOffset[j] + i), f.Font, Brushes.Black, xx + nnsize, y);
               }

               int top;
               if (j > 0)
               {
                  top = topology[j - 1];
               }
               else
               {
                  top = inputNeurons.Length;
               }

               nn++;
               float prevoff = yy + (height - (top * ysepsize + nnsize)) / 2;
               for (int k = 0; k < top; k++)
               {
                  float x1 = xx + (-1) * xsepsize + (nnsize);
                  float y1 = prevoff + k * ysepsize + (nnsize / 2);
                  float x2 = xx + (0) * xsepsize;
                  float y2 = off + i * ysepsize + (nnsize / 2);
                  e.Graphics.DrawLine(Pens.LightGray, x1, y1, x2, y2);

                  e.Graphics.DrawString( GetWeigthInfo(nn), f.Font, Brushes.Black, x1 / 4 + x2 * 3 / 4, y1 / 4 + y2 * 3 / 4);
                  nn++;
               }
            }
            xx += xsepsize;
         }
         */

      }
   }
}
