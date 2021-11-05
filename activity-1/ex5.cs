using System;
using System.Threading;
namespace ex5
{
    class Program
    {
        private static string x = "";
        private static int exitflag = 0;
        private static object _Lock = new object();
        static void ThReadX()
        {
            String i = Thread.CurrentThread.Name;
            lock (_Lock)
            {
                while (exitflag == 0)
                {
                    Monitor.Wait(_Lock);
                    if (exitflag == 1) break;
                    Console.WriteLine("***Thread {0} : X = {1}***", i, x);
                }
                Console.WriteLine("---Thread {0} exit---", i);
            }}
        static void ThWriteX()
        {
            string xx;
            while (exitflag == 0)
            {
                lock (_Lock)
                {
                    Monitor.Pulse(_Lock);
                    Console.Write("Input: ");
                    xx = Console.ReadLine();
                    if (xx == "exit")
                    {
                        exitflag = 1;
                        Monitor.PulseAll(_Lock);
                    }
                    else
                    {x = xx;}
                }}}
        static void Main(string[] args)
        {
            Thread A = new Thread(new ThreadStart(ThWriteX));
            Thread B = new Thread(new ThreadStart(ThReadX));
            Thread C = new Thread(new ThreadStart(ThReadX));
            Thread D = new Thread(new ThreadStart(ThReadX));
            B.Name = "1";
            C.Name = "2";
            D.Name = "3";
            A.Start();
            B.Start();
            C.Start();
            D.Start();
        }}
}