using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;


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
            string x = "";

            Task<string> t = Task.Run(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    x += "*";
                    Thread.Sleep(100);
                }
                return x;
            });

            Console.WriteLine(t.Result);  //Accessing the task result produces the same effect as Wait()
            //t.Wait();
        }

        public static void RunTaskWithChildren()
        {

            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];

                new Task(() => results[0] = 0, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2, TaskCreationOptions.AttachedToParent).Start();

                return results;

            });

            var finalTask = parent.ContinueWith(parentTask =>
            {

                foreach (int i in parentTask.Result)
                    Console.WriteLine(i);

            });

            finalTask.Wait();
            Console.ReadKey();
        }

        public static void RunTaskWithChildrenFactory()
        {

            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];

                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);

                tf.StartNew(() => results[0] = 0);
                tf.StartNew(() => results[1] = 1);
                tf.StartNew(() => results[2] = 2);
                return results;
            });

            var finalTask = parent.ContinueWith(parentTask =>
            {
                foreach (int i in parentTask.Result)
                    Console.WriteLine(i);
            });

            finalTask.Wait();
            Console.ReadKey();
        }

        public static void RunTaskWaitAll()
        {
            Task[] tasks = new Task[3];
            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("1");
                return 1;
            });
            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 2;
            });
            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("3");
                return 3;
            }
            );
            Task.WaitAll(tasks);

        }

        public static void RunTaskWaiAny()
        {
            Task<int>[] tasks = new Task<int>[3];

            tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
            tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
            tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });
            while (tasks.Length > 0)
            {
                int i = Task.WaitAny(tasks);
                Task<int> completedTask = tasks[i];
                Console.WriteLine(completedTask.Result);
                var temp = tasks.ToList();
                temp.RemoveAt(i);
                tasks = temp.ToArray();
            }
        }


        public static void ParallelForForEach()
        {

            Parallel.For(0, 10, i =>
            {
                Thread.Sleep(1000);
            });

            var numbers = Enumerable.Range(0, 10);

            Parallel.ForEach(numbers, i =>
            {
                Thread.Sleep(1000);
            });
        }

        public static void ParallelBreak()
        {
            ParallelLoopResult result = Parallel.For(0, 1000, (int i, ParallelLoopState loopState) =>
            {
                if (i == 500)
                {
                    Console.WriteLine("Breaking loop");
                    loopState.Break();
                }
                return;
            });

        }

        public static void AsyncAwait()
        {
            string result = DownloadContent().Result;
        }

        public static async Task<string> DownloadContent()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://www.microsoft.com");
                return result;
            }
        }

    }
}