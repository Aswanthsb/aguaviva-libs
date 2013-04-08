using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace robot
{
    class interpolator
    {
        int[] abs;

        int[] ini;
        int[] fin;
        int[] deltas;

        public delegate void motor(int i, int steps);
        public delegate void next();

        motor mot;
        next nxt;

        readonly object _locker = new object();
        bool go;

        public interpolator(int i, motor m, next n)
        {
            abs = new int[i];
            ini = new int[i];
            fin = new int[i];
            deltas = new int[i];
            
            ini[1] = 750;
            ini[2] = 1380;
            ini[3] = -1330;
            ini[4] = 1330;
            
            mot = new motor(m);
            nxt = new next(n);
            go = false;
        }

        public void SetPoint(int dd1, int dd2, int dd3, int dd4, int dd5)
        {
            fin[0] = dd1;
            fin[1] = dd2;
            fin[2] = dd3;
            fin[3] = dd4;
            fin[4] = dd5;

            go = true;
            new Thread(Interpolate).Start();
                       
        }

        public void Stop()
        {
                go = false;
        }

        private void Interpolate()
        {
            int max = 0;
            for (int i = 0; i < fin.Length; i++)
            {
                if ( max < Math.Abs(fin[i]-ini[i]) )
                    max = Math.Abs(fin[i] - ini[i]);
            }

            if (max == 0)
                return;

            for (int i = 0; i < fin.Length; i++)
            {
                deltas[i] = (fin[i]-ini[i]) * 65536 / max;
                ini[i] *= 65536;
            }

            for (int j = 0; j < max; j++)
            {
                if (go == false)
                    break;

                for (int i = 0; i < fin.Length; i++)
                {
                    ini[i] += deltas[i];
                    mot(i, ini[i] / 65536);
                }

                Thread.Sleep(5);
            }

            for (int i = 0; i < fin.Length; i++)
            {
                ini[i] /= 65536;
            }


            if (nxt != null)
            {
                nxt();
            }
        }

    }
}

