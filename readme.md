# FileSemaphore

## Description

FileSemaphore class allow us to pause program execution and wait for a file appear in a given folder with a given name and eventually with a given content.

You can also handle the “unlock” event that contains the filename and the content of the file that has caused the event.

Full class documentation is [here](https://github.com/alfonsocatanzaro/FileSemaphore/blob/master/FileSemaphore.md)!

## Use of '**WaitForUnlock**' method example

```csharp
string semFile = Path.Combine (Directory.GetCurrentDirectory (), "semaphore4.sem");
string content = "specific content";
// Deleting file if exists
if (File.Exists (semFile)) File.Delete (semFile);
FileSemaphore fs = new FileSemaphore (semFile, content);
bool ok = fs.WaitForUnlock (10000, out FileSemaphoreEventArgs eventArgs);
// Here you can write the file manually to unlock the program...
if (ok){
  Console.WriteLine("Unlocked by file");
} else {
  Console.WriteLine("Timeout!");
}
```

## Use of '**Unlock**' event example

```csharp
FileSemaphore fs = new FileSemaphore ("semaphore.sem");
fs.UnLocked += (s, e) => {
    Console.WriteLine ($"t1: semaphore is unlocked with {e.Filename} with '{e.Content}' as content.");
};
fs.Start ();
Thread.Sleep(1000);
File.WriteAllText ("semaphore.sem", "hello semaphore!");
Thread.Sleep(1000);
```
