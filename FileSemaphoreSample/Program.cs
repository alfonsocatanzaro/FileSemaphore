using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileSemaphoreSample {
    class Program {
        static void Main (string[] args) {
            EventWaitHandle wh = new ManualResetEvent (false);
            // this task wait for the semaphore
            Task t1 = Task.Run (() => {
                File.Delete ("semaphore.sem");
                FileSemaphore fs = new FileSemaphore ("semaphore.sem");
                fs.Start ();
                fs.UnLocked += (s, e) => {
                    Console.WriteLine ($"t1: semaphore is unlocked with {e.Filename} with '{e.Content}' as content.");
                };
                wh.WaitOne ();
            });

            // this task unlock the semaphore
            Task t2 = Task.Run (() => {
                Console.WriteLine ("t2: starting");
                Thread.Sleep (2000);
                Console.WriteLine ("t2: writing file");
                File.WriteAllText ("semaphore.sem", "hello semaphore!");
                Console.WriteLine ("t2: file written, end");
            });

            Console.WriteLine ("mainthread: tasks launched, waiting");
            Thread.Sleep (5000);
            Console.WriteLine ("mainthread: ending");
            wh.Set ();

            Task.WaitAll (t1, t2);
            Console.WriteLine ("mainthread: end");

            Console.WriteLine ("--------------------------");

            string semFile = Path.Combine (Directory.GetCurrentDirectory (), "semaphore4.sem");
            string content = "specific content";
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new FileSemaphore (semFile, content);
            Task.Run (() => {
                Console.WriteLine ("Starting writer task");
                Thread.Sleep (2500);
                File.WriteAllText (semFile, content);
                Console.WriteLine ("Writer task has finish");
            });
            Console.WriteLine ("Wait for the semafore");
            bool ok = fs.WaitForUnlock (5000, out FileSemaphoreEventArgs eventArgs);
            Console.WriteLine ($"Ok       = {ok}");
            Console.WriteLine ($"Filename = {eventArgs.Filename}");
            Console.WriteLine ($"Content  = {eventArgs.Content}");

        }
    }
}