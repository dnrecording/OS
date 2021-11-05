﻿using System;
using System.Threading;

namespace case_study2
{
    class Thread_safe_buffer
    {
        static int[] TSBuffer = new int[10] {-5,-5,-5,-5,-5,-5,-5,-5,-5,-5};
        static int Front = 0;
        static int Back = 0;
        static int Count = 0;
        static object _Lock = new object();
        static object _Lock2 = new object();

        static void EnQueue(int eq)
        {
            TSBuffer[Back] = eq;
            Back++;
            Back %= 10;
            Count += 1;
            //Console.WriteLine("Push: {0} , Count: {1}", eq, Count);
        }

        static int DeQueue()
        {
            int x = 0;
            x = TSBuffer[Front];
            Front++;
            Front %= 10;
            Count -= 1;
            return x;
        }

        static void th01()
        {
            int i;
            for (i = 1; i < 51; i++)
            {
                lock(_Lock){
                while (Count >= 10);
                EnQueue(i);
                Thread.Sleep(5);
                }
            }
        }

        static void th011()
        {
            int i;
            
            for (i = 100; i < 151; i++)
            {
                lock(_Lock){
                while (Count >= 10);
                EnQueue(i);
                Thread.Sleep(5);
            }
            }
        }


        static void th02(object t)
        {
            int i;
            int j;
            for (i = 0; i < 60; i++)
            {
                Monitor.Enter(_Lock2);
                while (Count <= 0);
                j = DeQueue();
                Console.WriteLine("Pop: {0} , thread: {1} , Count: {2}", j, t, Count);
                //Console.WriteLine("j={0}, thread:{1}", j, t);
                Thread.Sleep(100);
                Monitor.Exit(_Lock2);
            }
        }
        static void Main(string[] args)
        {
            Thread t1 = new Thread(th01);
            Thread t11 = new Thread(th011);
            Thread t2 = new Thread(th02);
            Thread t21 = new Thread(th02);
            Thread t22 = new Thread(th02);

            t1.Start();
            t11.Start();
            t2.Start(1);
            t21.Start(2);
            t22.Start(3);

            
        }
    }
}
