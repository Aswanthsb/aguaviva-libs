using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace robot
{
    class ProArm
    {
        static SerialPort serialPort1;
        static byte[] table = { 0x5, 0x6, 0xa, 0x9 };

        static public void Open()
        {
            serialPort1 = new System.IO.Ports.SerialPort( "COM9", 19200, Parity.None, 8, StopBits.One);
            serialPort1.Open();
        }

        static public void Close()
        {
            serialPort1.Close();
        }

        static public void MotorRaw(int i, int v)
        {
            string str = string.Format("Q{0}.", 16*i + v);
            serialPort1.Write(str);
        }

        static public void MotorStep(int i, int step)
        {
            MotorRaw(i, 16*i + table[step & 3]);
        }

        static int[] stepsTo90 = { 750, 750, 900, 900, 900 };

        static public void StepsToAngles(int[] steps, ref double[] angles)
        {
            for(int i=0;i<stepsTo90.Length;i++)
            {
                angles[i] = (steps[i] *90.0) / (double)stepsTo90[i];
            }

        }

    }
}
