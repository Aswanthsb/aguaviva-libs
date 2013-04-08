using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test
{
   class GeneticTrainer : Trainer
   {
      class Rank : IComparable<Rank>
      {
         public Genetic n;
         public double error;
         public int CompareTo(Rank other)
         {
            if (this.error > other.error)
               return 1;
            if (this.error < other.error)
               return -1;
            
            return 0;
         }

      };
      
      List<Rank> Ranking = new List<Rank>();

      public GeneticTrainer(NeuralTask nt)
      {
         this.neuralTask = nt;
      }

      void AddToRanking(Rank r)
      {
         int index = Ranking.BinarySearch(r);
         if (index < 0)
         {
            Ranking.Insert(~index, r);
         }
         else
         {
            //Ranking.Insert(index, r);
         }
      }

      override public void Init()
      {
         base.Init();

         for (int i = 0; i < 20; i++)
         {
            Rank r = new Rank();
            r.n = new Genetic();
            r.n.Init(neuralTask.GetTrainingSet(0), neuralTask.GetTopology(), null);
            r.error = this.ComputeErrors(r.n);
            AddToRanking(r);
         }

         /*
         for (int i = 0; i < 10; i++)
         {
            for (int j = 0; j < 10; j++)
            {

               Rank r = new Rank();
               r.n = new Genetic();
               r.n.Init(Ranking[i].n, Ranking[j].n);

               r.n.Mutate();
               r.n.Mutate();
               r.n.Mutate();
               r.n.Mutate();

               r.error = this.ComputeErrors(r.n);
               AddToRanking(r);

            }
         }
          */

      }

      override public void TrainNetwork()
      {
         if (Ranking.Count > 40)
         {
            Ranking.RemoveRange(40, Ranking.Count - 40);
         }


         for (int i = 0; i < 40; i++)
         {
            for (int j = 0; j < 40; j++)
            {
               for (int k = 0; k < 10; k++)
               {
                  Rank r = new Rank();
                  r.n = new Genetic();
                  r.n.Init(Ranking[i].n, Ranking[j].n);

                  r.n.Mutate();
                  r.n.Mutate();
                  r.n.Mutate();

                  r.error = this.ComputeErrors(r.n);
                  AddToRanking(r);
               }
               
            }
         }
         
         nn = Ranking[0].n;

         
          
      }

      override public string Print()
      {
         string o = "Genetic training\n";
            
         o += base.Print();

         int count = Ranking.Count;

         if (count > 20)
            count = 20;

         for(int i=0;i<count;i++)
         {
            o += i + ":   " + "  =>  " + string.Format("{0}, ", Ranking[i].error) + "\n";
         }
         o += "\n";

         /*
         for (int i = 0; i < Ranking.Count; i++)
         {
            o += i + ":   " + Ranking[i].n.Print() + "  =>  " + string.Format("{0}, ", Ranking[i].error) + "\n";
         }
          */

         return o;
      }

   }
}
