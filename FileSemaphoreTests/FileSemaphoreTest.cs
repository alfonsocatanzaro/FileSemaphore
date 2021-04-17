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
            string semFile = "semaphore1.sem";
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
            string semFile = "semaphore2.sem";
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
            string semFile = "semaphore2.sem";
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
            string semFile = "semaphore3.sem";
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
            string semFile =  "semaphore4.sem";
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
            string semFile = "semaphore5.sem";
            string content = "specific content";
            if (File.Exists (semFile)) File.Delete (semFile);
            FileSemaphore fs = new FileSemaphore (semFile, content);
            bool ok = fs.WaitForUnlock (TIMEOUT, out FileSemaphoreEventArgs eventArgs);
            Assert.False (ok);
            Assert.Null (eventArgs);
        }

        [Theory]
        [InlineData ("sem???.sem", "sem123.sem", "content")]
        [InlineData ("filesem.*", "filesem.001", "content")]
        [InlineData ("AAA.A??", "AAA.A01", "content")]
        public void FileSemaphore_ShouldUnlock_OnGroupFileAndContent (
            string fileNameFilter,
            string semaphoreFile,
            string semaphoreContent
        ) {
            string semFile = Path.Combine (Directory.GetCurrentDirectory (), fileNameFilter);
            foreach (var f in Directory.GetFiles (Directory.GetCurrentDirectory (), fileNameFilter))
                File.Delete (f);

            FileSemaphore fs = new FileSemaphore (semFile);
            fs.Start ();
            EventWaitHandle signal = new ManualResetEvent (false);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) => {
                signal.Set ();
                eventArgs = e;
            };
            File.WriteAllText (Path.Combine (Directory.GetCurrentDirectory (), semaphoreFile), semaphoreContent);

            bool ok = signal.WaitOne (5000);
            Assert.True (ok);
            Assert.NotNull (eventArgs);
            Assert.Equal (semaphoreContent, eventArgs.Content);
            Assert.Equal (semFile, eventArgs.Filename);
            // signal.Reset ();
            // File.WriteAllText (Path.Combine (Directory.GetCurrentDirectory (), "sem001_noCatch_.sem"), "content");
            // bool ok1 = signal.WaitOne (TIMEOUT);
            // Assert.False (ok1);

        }

    }
}