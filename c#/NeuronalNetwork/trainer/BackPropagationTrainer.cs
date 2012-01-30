using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class BackPropagationTrainer : Trainer
   {
      public BackPropagationTrainer(NeuralTask nt)
      {
         this.neuralTask = nt;
      }

      override public void Init()
      {
         base.Init();

         nn = new BackPropagation();

         nn.Init(neuralTask.GetTrainingSet(0), neuralTask.GetTopology(), null);
      }

      override public void TrainNetwork()
      {
         BackPropagation bp = (BackPropagation)nn;

         iter++;
         int i = iter % neuralTask.GetSetLength();

         bp.Forward(neuralTask.GetTrainingSet(i));

         bp.BackPropagate(neuralTask.GetTrainingSet(i), neuralTask.GetExpected(i));
         bp.UpdateWeights();
      }

   }
}
