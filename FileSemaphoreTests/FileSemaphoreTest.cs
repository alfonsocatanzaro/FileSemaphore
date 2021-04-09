using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FileSemaphoreTests {

    public class FileSemaphoreTest {
        const int TIMEOUT = 500;
        const int HALF_TIMEOUT = TIMEOUT / 2;

        [Fact]
        public void FileSemaphore_ShouldUnlock_OnOnSpecificFileAndAllContent () {
            string semFile = Path.Combine (Directory.GetCurrentDirectory (), "semaphore1.sem");
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new FileSemaphore (semFile);
            fs.Start ();
            EventWaitHandle signal = new ManualResetEvent (false);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) => {
                signal.Set ();
                eventArgs = e;
            };

            File.WriteAllText (semFile, "content");
            bool ok = signal.WaitOne (TIMEOUT);
            Assert.True (ok);
            Assert.NotNull (eventArgs);
            Assert.Equal ("content", eventArgs.Content);
            Assert.Equal (semFile, eventArgs.Filename);
        }

        [Fact]
        public void FileSemaphore_ShouldntUnlock_WhenNoFileWasWritten () {
            string semFile = Path.Combine (Directory.GetCurrentDirectory (), "semaphore2.sem");
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new FileSemaphore (semFile);
            fs.Start ();
            EventWaitHandle signal = new ManualResetEvent (false);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) => {
                signal.Set ();
                eventArgs = e;
            };

            bool ok = signal.WaitOne (TIMEOUT);
            Assert.False (ok);
        }

        [Fact]
        public void FileSemaphore_ShouldUnlock_OnOnSpecificFileAndSpecificContent () {
            string semFile = Path.Combine (Directory.GetCurrentDirectory (), "semaphore2.sem");
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new FileSemaphore (semFile, fileContent: "specific content");
            fs.Start ();
            EventWaitHandle signal = new ManualResetEvent (false);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) => {
                signal.Set ();
                eventArgs = e;
            };
            File.WriteAllText (semFile, "specific content");

            bool ok = signal.WaitOne (TIMEOUT);
            Assert.True (ok);
            Assert.NotNull (eventArgs);
            Assert.Equal ("specific content", eventArgs.Content);
            Assert.Equal (semFile, eventArgs.Filename);
        }

        [Fact]
        public void FileSemaphore_ShouldNotUnlock_OnOnSpecificFileAndDifferentContent () {
            string semFile = Path.Combine (Directory.GetCurrentDirectory (), "semaphore3.sem");
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new FileSemaphore (semFile, "specific content");
            fs.Start ();
            EventWaitHandle signal = new ManualResetEvent (false);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) => {
                signal.Set ();
                eventArgs = e;
            };
            File.WriteAllText (semFile, "Different cointent");

            bool ok = signal.WaitOne (TIMEOUT);
            Assert.False (ok);
        }

        [Fact]
        public void FileSemaphore_WaitForUnlockShouldReturnTrueAndEventArgs () {
            string semFile = Path.Combine (Directory.GetCurrentDirectory (), "semaphore4.sem");
            string content = "specific content";
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new FileSemaphore (semFile, content);
            Task.Run (() => {
                Thread.Sleep (HALF_TIMEOUT);
                File.WriteAllText (semFile, content);
            });
            bool ok = fs.WaitForUnlock (TIMEOUT, out FileSemaphoreEventArgs eventArgs);
            Assert.True (ok);
            Assert.NotNull (eventArgs);
            Assert.Equal (semFile, eventArgs.Filename);
            Assert.Equal (content, eventArgs.Content);
        }

        [Fact]
        public void FileSemaphore_WaitForUnlockShouldReturnFalseAndEventArgsNullOnTimeout () {
            string semFile = Path.Combine (Directory.GetCurrentDirectory (), "semaphore5.sem");
            string content = "specific content";
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new FileSemaphore (semFile, content);
            bool ok = fs.WaitForUnlock (TIMEOUT, out FileSemaphoreEventArgs eventArgs);
            Assert.False (ok);
            Assert.Null (eventArgs);
        }

        [Fact (Skip = "This test fails due to FileSystemWatcher(.net standard 2.0) bug")]
        public void FileSemaphore_ShouldUnlock_OnOnGroupFileAndAllContent () {
            string semFile = Path.Combine (Directory.GetCurrentDirectory (), "sem???.sem");
            foreach (var f in Directory.GetFiles (Directory.GetCurrentDirectory (), "sem???.sem"))
                File.Delete (f);

            FileSemaphore fs = new FileSemaphore (semFile);
            fs.Start ();
            EventWaitHandle signal = new ManualResetEvent (false);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) => {
                signal.Set ();
                eventArgs = e;
            };
            File.WriteAllText (Path.Combine (Directory.GetCurrentDirectory (), "sem123.sem"), "content");

            bool ok = signal.WaitOne (5000);
            Assert.True (ok);
            Assert.NotNull (eventArgs);
            Assert.Equal ("content", eventArgs.Content);
            Assert.Equal (semFile, eventArgs.Filename);
            signal.Reset ();
            File.WriteAllText (Path.Combine (Directory.GetCurrentDirectory (), "sem001_noCatch_.sem"), "content");
            bool ok1 = signal.WaitOne (TIMEOUT);
            Assert.False (ok1);

        }

    }
}