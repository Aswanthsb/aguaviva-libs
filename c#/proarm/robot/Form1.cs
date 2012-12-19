using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
namespace robot
{
    public partial class Form1 : Form
    {
        int[] motsteps = new int[5];
        interpolator interpol = new interpolator(6, ProArm.MotorStep, disengageMotors);

        List<Vector3> trace = new List<Vector3>();
        List<Vector3> cands = new List<Vector3>();
        
        List<float> er = new List<float>();

        List<Curve> path = new List<Curve>();

        

        public Form1()
        {
            motsteps[0] = 0;
            motsteps[1] = 750;
            motsteps[2] = 900;
            motsteps[3] = 0;
            /*
            path.Add( new Line(new Vector3(25, 19.8, 0), new Vector3(15, 0, 0)) );
            path.Add( new Line(new Vector3(25, 0, 0), new Vector3(15, 0, 0)) );
            path.Add(new Line(new Vector3(15, 0, 0), new Vector3(15, 19.8, 0)));
            path.Add(new Line(new Vector3(15, 19.8, 0), new Vector3(25, 19.8, 0)));
            */
            //path.Add(new Arc(new Vector3(25, 19.8, 0), new Vector3(25+19.8 , 0, 0), new Vector3(25 , 0, 0)));
            
            //path.Add(new Line( new Vector3(25, 19.8, 0), new Vector3(25, 0, 0)));
            
            //path.Add(new Arc(new Vector3(25, 19.8, 0), new Vector3(25+5 , 14.8, 0), new Vector3(25-5, 14.8-5, 0)));
            path.Add(new Line(new Vector3(25, 19.8, 0), new Vector3(25 -5+15, 14.8, 0)));
            path.Add(new Line(new Vector3(25, 19.8, 0), new Vector3(25 + 5, 14.8, 0)));
            path.Add(new Line(new Vector3(25 + 5, 14.8, 0), new Vector3(25, 0, 0)));
            path.Add(new Line(new Vector3(25, 0, 0), new Vector3(15, 0, 0)));
            path.Add(new Line(new Vector3(15, 0, 0), new Vector3(15, 19.8, 0)));
            path.Add(new Line(new Vector3(15, 19.8, 0), new Vector3(25, 19.8, 0)));
            
            InitializeComponent();
        }

        static void disengageMotors()
        {
            for (int i = 0; i < 6; i++)
            {
                ProArm.MotorRaw(i, 0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        float map(float v, float x0, float x1, float y0, float y1)
        {
            return (v - x0) * (y1 - y0) / (x1 - x0) + y0;
        }

        //90 -> 750
        //12 
        // (22 35)13
        // (19 32)
        //  900 again!
        void ForwardK(int d1, int d2, int d3, int d4, int d5)
        {
            int c3 = (d2 * 240) / 375;
            int c41 = d2 - (d2 * 100) / 750;
            int c42 = d3 - (d3 * 220) / 900;
            int c4 = c41 + c42;

            interpol.SetPoint(d1, d2, d3 + c3, d4 - c4, d5 + c4);
        }

        void DrawRobot(Graphics g, double[] aa)
        {
            double[] l = { 0, 19.8, 15, 10 };
            double[] a = { aa[0], aa[1], -aa[2], aa[3] };
            double x = 0;
            double y = 0;
            double _a = 0;

            g.DrawLine(Pens.Black, (float)x + 0, (float)y + 0, (float)x + 500, (float)y + 0);
            g.DrawLine(Pens.Black, (float)x + 0, (float)y + 0, (float)x + 0, (float)y + 500);

            for (int i = 1; i < a.Length; i++)
            {
                _a += a[i];
                double xx = x + l[i] * Math.Cos(_a * Math.PI / 180.0);
                double yy = y + l[i] * Math.Sin(_a * Math.PI / 180.0);
                g.DrawLine(Pens.Red, (float)x, (float)y, (float)xx, (float)yy);
                
                x = xx;
                y = yy;
            }

            trace.Add(new Vector3( x, y,0));

        }

        void ForwardPos(double [] aa, out Vector3 pos)
        {
            double[] l = { 0, 19.8, 15, 10 };
            double[] a = { aa[0], aa[1], -aa[2], aa[3] };
            double xx = 0;
            double yy = 0;
            double _a = 0;

            for (int i = 1; i < a.Length; i++)
            {
                _a += a[i];
                xx += l[i] * Math.Cos(_a * Math.PI / 180.0);
                yy += l[i] * Math.Sin(_a * Math.PI / 180.0);
            }

            pos = new Vector3(xx, yy, 0);
        }

        List<int[]> posList = new List<int[]>();

        double min_err = 1000000000;
        int [] mout;
        List<int[]> minPath;


        int FindInList(List<int[]> path, int[] pos)
        {
            for( int i=0;i<path.Count;i++)
            {
                int[] p = path[i];

                bool res = true;
                for (int j = 0; j < pos.Length; j++)
                {
                    if (pos[j] != p[j])
                    {
                        res = false;
                        break;
                    }
                }
                if (res == true)
                    return i;
            }
            return -1;
        }

        Vector3 StepsToPos(int[] steps)
        {
            return new Vector3((steps[1]-750)/20.0+25, (steps[2]-900)/20.0+19.8, 0);
            /*
            double[] angles = new double[steps.Length];

            ProArm.StepsToAngles(steps, ref angles);

            Vector3 pos;
            ForwardPos(angles, out pos);
            return pos;
             */
        }

        bool CheckIfTrackContainsEndingPoint( Curve c, List<int[]> steps)
        {
            if (steps == null || steps.Count == 2)
            {
                return false;
            }

            Vector3[] pos = new Vector3[2];

            pos[0] = StepsToPos(steps[0]);
            pos[1] = StepsToPos(steps[1]);
            
            if (    
                    (pos[0]-pos[1]).Magnitude > (pos[0]-c.GetFin()).Magnitude
               )
            {
                return true;
            }

            return false;
        }

        double minimize(int[] m, Curve c, double olderr, int rec)
        {
            double[] angles = new double[m.Length];
            ProArm.StepsToAngles(m, ref angles);
            Vector3 ipos;
            ForwardPos(angles, out ipos);

            int i0 = 0;
            //for (int i0 = -1; i0 < 2; i0++)
            {
                for (int i1 = -1; i1 <= 1; i1++)
                {
                    //int i2 = 0;
                    for (int i2 = -1; i2 <= 1; i2++)
                    {
                        int i3 = 0;
                        //for (int i3 = -1; i3 <= 1; i3++)
                        {

                            int[] stepfin = new [] { (m[0] + i0), (m[1] + i1), (m[2] + i2), (m[3] + i3), m[4]  };

                            if (FindInList( posList, stepfin) >= 0 )
                                continue;

                            Vector3 fpos;
                            ProArm.StepsToAngles(stepfin, ref angles);
                            ForwardPos(angles, out fpos);

                            double err = c.distance(fpos);
                        
                            double totalerr = olderr + err;

                            //if already worse than best then quit this path
                            if (totalerr > min_err)
                            {
                                continue;
                            }

                            posList.Add(stepfin);
                            cands.Add(fpos);
                            er.Add((float)err);

                            if (rec > 0)
                            {
                                minimize(stepfin, c, totalerr, rec - 1);
                            }
                            else
                            {
                                if (totalerr < min_err)
                                {
                                    min_err = totalerr;
                                    mout = posList[1];
                                    minPath = new List<int []>(posList);
                                }

                            }
                            posList.Remove(posList.Last());

                        }
                    }
                }
            }

            return 0;
        }

        bool minimize(int[] m, out int [] res, Curve c)
        {
            cands = new List<Vector3>();
            er = new List<float>();

            min_err = 1000000000;
            mout = null;
            posList.Add(m);
            minimize(m, c, 0, 3);
            posList.Remove(posList.Last());
            res = mout;

            bool b = CheckIfTrackContainsEndingPoint(c, minPath);
            if (b == true)
                return b;

            return b;
        }


        List<int[]> closedSet = new List<int[]>();
        List<int[]> openSet = new List<int[]>();
        

        Dictionary<int[], double> g_score = new Dictionary<int[], double>();
        Dictionary<int[], double> f_score = new Dictionary<int[], double>();
        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>();

        List<int[]> explored = new List<int[]>();

        List<int[]> GenerateNeighbours(int[] m )
        {
            List<int[]> neighbours = new List<int[]>();

            int i0 = 0;
            //for (int i0 = -1; i0 < 2; i0++)
            {
                for (int i1 = -1; i1 <= 1; i1++)
                {
                    //int i2 = 0;
                    for (int i2 = -1; i2 <= 1; i2++)
                    {
                        int i3 = 0;
                        //for (int i3 = -1; i3 <= 1; i3++)
                        {
                            if (i1 == 0 && i2 == 0)
                                continue;

                            int[] stepfin = new [] { (m[0] + i0), (m[1] + i1), (m[2] + i2), (m[3] + i3), m[4]  };

                            int f = FindInList(explored, stepfin);
                            if( f == -1 )
                            {
                                explored.Add(stepfin);
                                f = explored.Count - 1;
                            }

                            neighbours.Add(explored[f]);
                            
                        }
                    }
                }
            }

            return neighbours;
        }
        void DrawAStarPath(Graphics g, Pen p, float scale)
        {

            Font f = new Font(this.Font.FontFamily, (float)10 / scale);

            double min = 1000000;
            Vector3 pmin = new Vector3();

            float r = (float).01;
            for (int j = 0; j < openSet.Count; j++)
            {
                Vector3 pos = StepsToPos(openSet[j]);
                g.DrawEllipse(p, (float)(pos.X-r/2), (float)(pos.Y-r/2), r, r);

                //string str = string.Format("{0:0.0000000}", f_score[openSet[j]]);
                //g.DrawString(str, f, Brushes.White, (float)pos.X, (float)pos.Y);

                if (f_score[openSet[j]] < min)
                {
                    min = f_score[openSet[j]];
                    pmin = pos;
                }
            }
            //string str2 = string.Format("{0:0.0000000}", min);
            //g.DrawString(str2, f, Brushes.Yellow, (float)pmin.X, (float)pmin.Y);



            r *= (float).5;
            for (int j = 0; j < closedSet.Count; j++)
            {
                Vector3 pos = StepsToPos(closedSet[j]);
                g.DrawEllipse(p, (float)(pos.X - r / 2), (float)(pos.Y - r / 2), r, r);
            }



        }

        void AStar(int[] start, Curve c, int iterations)
        {
            if (iterations == 0)
            {
                closedSet = new List<int[]>();
                openSet = new List<int[]>();

                g_score = new Dictionary<int[], double>();
                f_score = new Dictionary<int[], double>();
                cameFrom = new Dictionary<int[], int[]>();

                explored.Add(start);
                openSet.Add(start);
                g_score[start] = 0;
                f_score[start] = g_score[start] + c.distance(StepsToPos(start));
            }

            //for (int i = 0; i < iterations; i++)
            {
                //find node in opennode with minimum score
                int currindex = -1;
                double score = 1000000;
                for(int j=0;j<openSet.Count;j++)
                {
                    if ( f_score[openSet[j]]<score)
                    {
                        currindex = j;
                        score = f_score[openSet[j]];
                    }
                }

                int[] current = openSet[currindex];

                openSet.Remove(current);
                closedSet.Add(current);

                List<int[]> neighbours = GenerateNeighbours(current);
                foreach (int[] neighbour in neighbours)
                {
                    if (closedSet.IndexOf(neighbour) != -1)
                    {
                        continue;
                    }

                    //double tentativeScore = g_score[current] + Math.Abs((c.distance(StepsToPos(current)) - c.distance(StepsToPos(neighbour))));
                    double tentativeScore = g_score[current] + (StepsToPos(current) - StepsToPos(neighbour)).Magnitude;

                    if (openSet.IndexOf(neighbour) == -1 || tentativeScore <= g_score[neighbour])
                    {
                        cameFrom[neighbour] = current;
                        g_score[neighbour] = tentativeScore;
                         f_score[neighbour] = g_score[neighbour] + c.distance(StepsToPos(neighbour));
                         if (openSet.IndexOf(neighbour) == -1)
                         {
                             openSet.Add(neighbour);
                         }                             
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ForwardK(0, 375, 900, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ForwardK(0, 750, 900, 0, 0);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            interpol.Stop();            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            float scale = trackBar1.Value;
            Pen pen = new Pen(Color.Blue, (float)(1.0 / scale));
            Pen penY = new Pen(Color.Yellow, (float)(1.0 / scale));


            double[] angles = new double[5];
            ProArm.StepsToAngles(motsteps, ref angles);

            e.Graphics.TranslateTransform(Width / 2, Height / 2);
            {
                Vector3 tip;
                ForwardPos(angles, out tip);
                //tip.X = 26;
                //tip.Y = 19.8;
                e.Graphics.TranslateTransform((float)tip.X * -scale, (float)tip.Y * scale);
            }

            e.Graphics.ScaleTransform(scale, -scale);

            DrawRobot(e.Graphics, angles);

            DrawAStarPath(e.Graphics, penY, scale);

            foreach (Curve c in path)
            {
                c.Draw(e.Graphics, pen);
            }

            for (int i = 0; i < trace.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.Yellow, (float)trace[i].X, (float)trace[i].Y, 1 / scale, 1 / scale);
            }
                    
            for (int i = 0; i < cands.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.White, (float)cands[i].X, (float)cands[i].Y, 2 / scale, 2 / scale);
                
                string str = string.Format("{0:0.00}", er[i]);
                Font f = new Font(this.Font.FontFamily, (float)10/scale);
                e.Graphics.DrawString(str, f, Brushes.White, (float)cands[i].X, (float)cands[i].Y);
                
            }
                      
            e.Graphics.FillRectangle(Brushes.Red, (float)trace[trace.Count - 1].X, (float)trace[trace.Count - 1].Y, 2 / scale, 2 / scale);

            

            if (minPath != null)
            {
                double[] prev = angles;
                double[] next = null;
                foreach (int[] p in minPath)
                {
                    next = new double[5];
                    ProArm.StepsToAngles(p, ref next);

                    Vector3 x1;
                    ForwardPos(prev, out x1);
                    Vector3 x2;
                    ForwardPos(next, out x2);

                    e.Graphics.DrawLine(pen, (float)x1.X, (float)x1.Y, (float)x2.X, (float)x2.Y);
                    prev = next;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RobotStep();

            Invalidate();
          
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        int cnt = 0;


        void RobotStep()
        {
            /*
            if (cnt >= path.Count)
            {
                return;
            }

            int[] res;
            bool change = minimize(motsteps, out res, path[cnt]);
            motsteps = res;
            int[] res2;
            minimize(motsteps, out res2, path[cnt]);
            
            if (change == true)
            {
                cnt++;
            }

            */
            AStar(motsteps, path[0], cnt++);
        }

        private void step(object sender, EventArgs e)
        {
            RobotStep();
            Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }
    }
}

