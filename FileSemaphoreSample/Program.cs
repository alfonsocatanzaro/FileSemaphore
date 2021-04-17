using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileSemaphoreSample {
    class Program {
        static void Main (string[] args) {
            EventSample ();
            Console.WriteLine ("--------------------------");
            WaitForUnlockSample ();
            Console.ReadLine();
        }

        private static void EventSample () {
            EventWaitHandle wh = new ManualResetEvent (false);
            // this task wait for the semaphore
            Task t1 = Task.Run (() => {
                File.Delete ("semaphore000.sem");
                File.Delete ("semaphoreAAA.sem");
                File.Delete ("Another.sem");
                FileSemaphore fs = new("semaphore???.sem");
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
                File.WriteAllText ("semaphore000.sem", "hello semaphore000!");
                File.WriteAllText("semaphoreAAA.sem", "hello semaphoreAAA!");
                File.WriteAllText("Another.sem", "this file doesn't catch");
                Console.WriteLine ("t2: files written, end");
            });

            Console.WriteLine ("mainthread: tasks launched, waiting");
            Thread.Sleep (5000);
            Console.WriteLine ("mainthread: ending");
            wh.Set ();

            Task.WaitAll (t1, t2);
            Console.WriteLine ("mainthread: end");
        }

        private static void WaitForUnlockSample () {
            string semFile =  "semaphore4.sem";
            string content = "specific content";
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new(semFile, content);
            Task.Run (() => {
                Console.WriteLine ("Starting writer task");
                Thread.Sleep (2500);
                File.WriteAllText (semFile, content);
                Console.WriteLine ("Writer task has finish");
            });
            Console.WriteLine ("Wait for the semaphore");
            bool ok = fs.WaitForUnlock (5000, out FileSemaphoreEventArgs eventArgs);
            Console.WriteLine ($"Ok       = {ok}");
            Console.WriteLine ($"Filename = {eventArgs.Filename}");
            Console.WriteLine ($"Content  = {eventArgs.Content}");
        }


    }
}