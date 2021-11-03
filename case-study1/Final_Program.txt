using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01
{
    class Program
    {
        static int DataSize = 1000000000;
        static int num_threads = 64;
        static byte[] Data_Global = new byte[DataSize];
        static long[] Sum_Global = new long[num_threads];

        static long Sum_Last = 0;

        static void myWorkerThread(int startIndex, int endIndex, int SumID)
        {
            int j;
            for (j = startIndex; j <= endIndex; j++)
            {
                sum(SumID, j);
            }
        }

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
        static void sum(int sumID, int index)
        {
            if (Data_Global[index] % 2 == 0)
            {
                Sum_Global[sumID] -= Data_Global[index];
            }
            else if (Data_Global[index] % 3 == 0)
            {
                Sum_Global[sumID] += (Data_Global[index] * 2);
            }
            else if (Data_Global[index] % 5 == 0)
            {
                Sum_Global[sumID] += (Data_Global[index] / 2);
            }
            else if (Data_Global[index] % 7 == 0)
            {
                Sum_Global[sumID] += (Data_Global[index] / 3);
            }
            Data_Global[index] = 0;
        }
        public static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int i, y;

            //Read data from file
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

            //Start
            Console.Write("\n\nWorking...");
            sw.Start();
            // make threads from num_threads
            Thread[] myThreads = new Thread[num_threads];
            //calculate index for each threads
            int tempCalIndex = DataSize / num_threads;
            int[] start = new int[num_threads];
            int[] end = new int[num_threads];
            int[] ThreadIndex = new int[num_threads];
            int calculateIndex; //faster loop
            for (calculateIndex = 0; calculateIndex < num_threads; calculateIndex++)
            {
                ThreadIndex[calculateIndex] = calculateIndex;
                start[calculateIndex] = tempCalIndex * calculateIndex;
                end[calculateIndex] = (tempCalIndex * (calculateIndex + 1)) - 1;
            }
            //creating thread
            for (calculateIndex = 0; calculateIndex < num_threads - 1; calculateIndex++)
            { //the other threads
                int damnIndex = calculateIndex;
                myThreads[damnIndex] = new Thread(() => myWorkerThread(start[damnIndex], end[damnIndex], damnIndex));
            }
            //last thread, for the remaining of the divinding number until the DataSize
            myThreads[num_threads - 1] = new Thread(() => myWorkerThread(start[num_threads - 1], DataSize - 1, num_threads - 1));
            // start all threads 
            int joining;
            for (joining = 0; joining < num_threads; joining++)
            {
                myThreads[joining].Start();
            }
            //wait for all threads to complete
            for (joining = 0; joining < num_threads; joining++)
            {
                myThreads[joining].Join();
            }
            // sum all result, do last. all threads need to complete.
            for (i = 0; i < num_threads; i++)
                Sum_Last += Sum_Global[i];

            sw.Stop();
            Console.WriteLine("Done.");

            //Result
            Console.WriteLine("Summation result: {0}", Sum_Last);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
