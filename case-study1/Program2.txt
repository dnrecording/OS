// test with 2 threads
// 3 seconds faster 

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace case_study1
{
    class Program
    {
        static byte[] Data_Global = new byte[1000000000];
        static long[] Sum_Global = new long[2];
        static int G_index = 0;

        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                Data_Global = (byte[])bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }

        // static void sum()
        // {
        //     if (Data_Global[G_index] % 2 == 0)
        //     {
        //         Sum_Global -= Data_Global[G_index];
        //     }
        //     else if (Data_Global[G_index] % 3 == 0)
        //     {
        //         Sum_Global += (Data_Global[G_index] * 2);
        //     }
        //     else if (Data_Global[G_index] % 5 == 0)
        //     {
        //         Sum_Global += (Data_Global[G_index] / 2);
        //     }
        //     else if (Data_Global[G_index] % 7 == 0)
        //     {
        //         Sum_Global += (Data_Global[G_index] / 3);
        //     }
        //     Data_Global[G_index] = 0;
        //     G_index++;
        // }

        static void sum2(int index_sum, int index_val)
        {
            if (Data_Global[index_val] % 2 == 0)
            {
                Sum_Global[index_sum] -= Data_Global[index_val];
            }
            else if (Data_Global[index_val] % 3 == 0)
            {
                Sum_Global[index_sum] += (Data_Global[index_val] * 2);
            }
            else if (Data_Global[index_val] % 5 == 0)
            {
                Sum_Global[index_sum] += (Data_Global[index_val] / 2);
            }
            else if (Data_Global[index_val] % 7 == 0)
            {
                Sum_Global[index_sum] += (Data_Global[index_val] / 3);
            }
            Data_Global[index_val] = 0;
            //G_index++;
        }

        static void loopandsum()
        {
            for (int i = 0; i < 500000000; i++)
            sum2(0,i);
        }

        static void loopandsum2()
        {
            for (int j = 500000000; j < 1000000000; j++)
            sum2(1,j);
        }

        public static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int i, y;

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            /* Start */
            Console.Write("\n\nWorking...");

            Thread t1 = new Thread(loopandsum);
            Thread t2 = new Thread(loopandsum2);

            sw.Start();
            //    for (i = 0; i < 1000000000; i++)
            //         sum();

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Sum_Global[0] += Sum_Global[1];

            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global[0]);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
