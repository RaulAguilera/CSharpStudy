using System;
using System.Threading;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopped = false;

            Thread t = new Thread(new ThreadStart(() =>
            {

                while (!stopped)
                {
                    Console.WriteLine("Running...");
                    Thread.Sleep(1000);
                }


            }));

            t.Start();
            Console.WriteLine("Press key to exit");
            Console.ReadKey();

            stopped = true;
            t.Join();

        }

        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("ThreadProc: {0}", i);
                Thread.Sleep(1000);
            }
        }
    }
}

