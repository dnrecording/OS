using System;
using System.Threading;

namespace OS_Sync_01
{
    class Program{
        private static string x = "";
        private static int exitflag = 0;

        private static object _Lock = new object();

        static void ThReadX(){
            lock(_Lock){
            while(exitflag==0)
            Console.WriteLine("X = {0}", x);
            }
        }

        static void ThWriteX(){
            string xx;
            while(exitflag == 0){
                Console.Write("Input: ");
                xx = Console.ReadLine();
                lock(_Lock){
                if(xx=="exit"){
                    exitflag = 1;
                }
                else{
                    x = xx;
                }
                }
            }
        }

        static void Main(string[] args){
            Thread A = new Thread(ThReadX);
            Thread B = new Thread(ThWriteX);

            A.Start();
            B.Start();
        }
    }
}