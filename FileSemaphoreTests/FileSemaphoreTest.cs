using System;
using System.IO;
using Xunit;
using System.Threading;

namespace FileSemaphoreTests
{

    public class FileSemaphoreTest
    {
        const int TIMEOUT = 500;

        [Fact]
        public void FileSemaphore_ShouldUnlock_OnOnSpecificFileAndAllContent()
        {
            string semFile = Path.Combine(Directory.GetCurrentDirectory(), "semaphore1.sem");
            if (File.Exists(semFile)) File.Delete(semFile);
            FileSemaphore fs = new FileSemaphore(semFile);
            fs.Start();
            SemaphoreSlim signal = new SemaphoreSlim(0, 1);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) =>
            {
                signal.Release();
                eventArgs = e;
            };

            File.WriteAllText(semFile, "content");
            bool ok = signal.Wait(TIMEOUT);
            Assert.True(ok);
            Assert.NotNull(eventArgs);
            Assert.Equal("content", eventArgs.Content);
            Assert.Equal(semFile, eventArgs.Filename);
        }

        [Fact]
        public void FileSemaphore_ShouldntUnlock_WhenNoFileWasWritten()
        {
            string semFile = Path.Combine(Directory.GetCurrentDirectory(), "semaphore2.sem");
            if (File.Exists(semFile)) File.Delete(semFile);
            FileSemaphore fs = new FileSemaphore(semFile);
            fs.Start();
            SemaphoreSlim signal = new SemaphoreSlim(0, 1);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) =>
            {
                signal.Release();
                eventArgs = e;
            };

            bool ok = signal.Wait(TIMEOUT);
            Assert.False(ok);
        }

        [Fact]
        public void FileSemaphore_ShouldUnlock_OnOnSpecificFileAndSpecificContent()
        {
            string semFile = Path.Combine(Directory.GetCurrentDirectory(), "semaphore2.sem");
            if (File.Exists(semFile)) File.Delete(semFile);
            FileSemaphore fs = new FileSemaphore(semFile, fileContent: "specific content");
            fs.Start();
            SemaphoreSlim signal = new SemaphoreSlim(0, 1);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) =>
            {
                signal.Release();
                eventArgs = e;
            };
            File.WriteAllText(semFile, "specific content");

            bool ok = signal.Wait(TIMEOUT);
            Assert.True(ok);
            Assert.NotNull(eventArgs);
            Assert.Equal("specific content", eventArgs.Content);
            Assert.Equal(semFile, eventArgs.Filename);
        }

        [Fact]
        public void FileSemaphore_ShouldNotUnlock_OnOnSpecificFileAndDifferentContent()
        {
            string semFile = Path.Combine(Directory.GetCurrentDirectory(), "semaphore3.sem");
            if (File.Exists(semFile)) File.Delete(semFile);
            FileSemaphore fs = new FileSemaphore(semFile, "specific content");
            fs.Start();
            SemaphoreSlim signal = new SemaphoreSlim(0, 1);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) =>
            {
                signal.Release();
                eventArgs = e;
            };
            File.WriteAllText(semFile, "Different cointent");

            bool ok = signal.Wait(TIMEOUT);
            Assert.False(ok);
        }

        [Fact(Skip = "This test fails due to FileSystemWatcher(.net standard 2.0) bug")]
        public void FileSemaphore_ShouldUnlock_OnOnGroupFileAndAllContent()
        {
            string semFile = Path.Combine(Directory.GetCurrentDirectory(), "sem???.sem");
            foreach (var f in Directory.GetFiles(Directory.GetCurrentDirectory(), "sem???.sem"))
                File.Delete(f);

            FileSemaphore fs = new FileSemaphore(semFile);
            fs.Start();
            SemaphoreSlim signal = new SemaphoreSlim(0, 1);
            FileSemaphoreEventArgs eventArgs = null;
            fs.UnLocked += (s, e) =>
            {
                signal.Release();
                eventArgs = e;
            };
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "sem123.sem"), "content");

            bool ok = signal.Wait(5000);
            Assert.True(ok);
            Assert.NotNull(eventArgs);
            Assert.Equal("content", eventArgs.Content);
            Assert.Equal(semFile, eventArgs.Filename);

            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "sem001_noCatch_.sem"), "content");
            signal.Wait(TIMEOUT);
            bool ok1 = signal.Wait(TIMEOUT);
            Assert.False(ok1);

        }

    }
}
