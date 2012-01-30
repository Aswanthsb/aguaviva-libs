using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class Trainer
   {
      public int iter = 0;

      protected double[] errors;
      protected double totalError;

      protected NeuralTask neuralTask;

      public NeuronalNetwork nn;

      public Trainer()
      {
      }

      public Trainer(NeuralTask nt)
      {
         this.neuralTask = nt;
      }

      virtual public void Init()
      {
         neuralTask.Init();

         errors = new double[this.neuralTask.GetSetLength()];
      }

      virtual public void TrainNetwork()
      {
      }

      virtual public double ComputeErrors(NeuronalNetwork n)
      {
         if (n == null)
            n = nn;

         totalError = 0;
         for (int j = 0; j < this.neuralTask.GetSetLength(); j++)
         {
            n.Forward(neuralTask.GetTrainingSet(j));
            errors[j] = n.GetError(neuralTask.GetExpected(j));
            totalError += errors[j];
         }

         return totalError;
      }

      public void GetErrorAndVariance(double[] errors, int len, out double averageError, out double variance)
      {
         double err;
         err = 0;
         for (int j = 0; j < len; j++)
         {
            err += errors[j];
         }
         averageError = err / len;

         double averageErrorSquare = averageError * averageError;

         err = 0;
         for (int j = 0; j < len; j++)
         {
            err += errors[j] * errors[j] - averageErrorSquare;
         }
         variance = err;
      }

      virtual public string Print()
      {
         string o = "";

         ComputeErrors(null);

         for (int j = 0; j < 1000; j++)
         {
            if (j >= neuralTask.GetSetLength())
            {
               break;
            }

            nn.Forward(neuralTask.GetTrainingSet(j));

            o += string.Format("{0:0#} : ",j);
            for (int i = 0; i < neuralTask.GetTrainingSet(0).Length; i++)
            {
               //o += (Math.Sign(neuralTask.GetTrainingSet()[j][i]) >= 0 ? " " : "-") + string.Format("{0:#0.00}, ", Math.Abs(neuralTask.GetTrainingSet()[j][i]));
            }

            o += "   =>";

            for (int i = 0; i < neuralTask.GetExpected(j).Length; i++)
            {
               o += neuralTask.GetExpected(j)[i] +", ";
            }
            o += "     " + errors[j] + "\n";
         }

         double averageError;
         double variance;
         GetErrorAndVariance(errors, errors.Length, out averageError, out variance);

         o += "Average error:  " + averageError + "\n";
         o += "Variance:       " + variance + "\n";

         //n.Save("c:\\cacas.txt");
         return o;
      }

   }
}
