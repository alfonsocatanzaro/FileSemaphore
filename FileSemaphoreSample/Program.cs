using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileSemaphoreSample
{
    class Program
    {
        static void Main(string[] args)
        {
            EventWaitHandle wh = new ManualResetEvent(false);
            EventWaitHandle t2Ready = new ManualResetEvent(false);
            EventWaitHandle t1Ready = new ManualResetEvent(false);
            // this task wait for the semaphore
            Task t1 = Task.Run(() =>
            {
                File.Delete("semaphore.sem");
                FileSemaphore fs = new FileSemaphore("semaphore.sem");
                fs.Start();
                fs.UnLocked += (s, e) => {
                    Console.WriteLine($"t1: semaphore is unlocked with {e.Filename} with '{e.Content}' as content.");
                };
                t1Ready.Set();
                wh.WaitOne();
            });


            // this task unlock the semaphore
            Task t2 = Task.Run(() =>
            {
                Console.WriteLine("t2: starting");
                Thread.Sleep(2000);
                Console.WriteLine("t2: writing file");
                File.WriteAllText("semaphore.sem", "hello semaphore!");
                Console.WriteLine("t2: file written, end");
            });

            Console.WriteLine("mainthread: tasks launched, waiting");
            Thread.Sleep(5000);
            Console.WriteLine("mainthread: ending");
            wh.Set();

            Task.WaitAll(t1,t2);
            Console.WriteLine("mainthread: end");

        }
    }
}
