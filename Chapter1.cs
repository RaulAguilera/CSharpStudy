using System;
using System.Threading;

namespace TestProject
{
    public static class Chapter1
    {


        public static void RunThread()
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

        public static void RunTask()
        {


        }



    }
}