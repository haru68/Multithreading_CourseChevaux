using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Multithreading_CourseChevaux
{
    class Program
    {
        private int Winner { get; set; }
        private List<int> ArrivalOrderList = new List<int>();
        private Mutex Mutex = new Mutex();

        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
            program.Winner = program.ArrivalOrderList.First();
            Console.WriteLine("The winner is thread n° " + program.Winner);
        }

        private void Run()
        {
            var threadStartDelegate = new ThreadStart(OnThreadStart);
            var threads = new List<Thread>();
            for(int i = 0; i<10; i++)
            {
                var thread = new Thread(threadStartDelegate);
                threads.Add(thread);
            }

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
        }

        private void OnThreadStart()
        {
            var random = new System.Random();
            var executionTime = random.Next(2, 7);
            var timeSpan = TimeSpan.FromSeconds(executionTime);
            Thread.Sleep(timeSpan);
            Mutex.WaitOne();
            ArrivalOrderList.Add(Thread.CurrentThread.ManagedThreadId);
            Mutex.ReleaseMutex();
        }
    }
}
