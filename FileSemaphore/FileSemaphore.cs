using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Threading {
    /// <summary>
    /// FileSemaphore class allow you to create a process semaphore that unlock on file event.
    /// </summary>
    public class FileSemaphore : IDisposable {
        private readonly int READ_FILE_MAX_ATTEMPT = 5;
        private readonly int READ_FILE_ATTEMPT_DELAY = 50;
        private readonly int DELETE_FILE_MAX_ATTEMPT = 5;
        private readonly int DELETE_FILE_ATTEMPT_DELAY = 50;

        private readonly string _fileName;
        private readonly string _fileContent;
        private FileSystemWatcher _fileSystemWatcher;
        private readonly ISynchronizeInvoke _syncObject;
        private readonly bool _deleteFileAfterProcess;

        /// <summary>
        /// Create an FileSemaphore class instance
        /// </summary>
        /// <param name="fileName">Filename that will be check</param>
        /// <param name="fileContent">File content that will be match to unlock the semaphore. Empty string allow to unlock the semaphore with any content.</param>
        /// <param name="syncObject">Object to use to invoke event in threadsafe</param>
        /// <param name="deleteFileAfterProcess">Delete file when semaphore is unlocked</param>
        public FileSemaphore (string fileName,
            string fileContent = "",
            ISynchronizeInvoke syncObject = null,
            bool deleteFileAfterProcess = false) {
            _fileName = fileName;
            _fileContent = fileContent;
            _syncObject = syncObject;
            _deleteFileAfterProcess = deleteFileAfterProcess;

            EnsureFullPathFilename (ref _fileName);
        }

        private void EnsureFullPathFilename (ref string filename) {
            if (!filename.Contains (Path.DirectorySeparatorChar) &&
                !filename.Contains (Path.AltDirectorySeparatorChar)) {
                filename = Path.Combine (Directory.GetCurrentDirectory (), filename);
            }
        }

        /// <summary>
        /// Stop current thread and wait for a unlocked event or timeout
        /// </summary>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <param name="eventArgs">Contains unlocked semaphore information if unlocked, null if timeout is reached</param>
        /// <returns>true if an unlocked event has reacher, false when timeout</returns>
        public bool WaitForUnlock (int timeout, out FileSemaphoreEventArgs eventArgs) {

            this.Start ();
            EventWaitHandle waitHandle = new ManualResetEvent (false);
            FileSemaphoreEventArgs capturedEventArgs = null;
            FileSemaphoreEventHandler handler = (s, e) => {
                waitHandle.Set ();
                capturedEventArgs = e;
            };
            this.UnLocked += handler;
            bool ok = waitHandle.WaitOne (timeout);
            eventArgs = ok ? capturedEventArgs : null;
            this.UnLocked -= handler;
            return ok;
        }

        /// <summary>
        /// Stop current thread and wait for a unlocked event or timeout
        /// </summary>
        /// <param name="timeout">Timeout timespan</param>
        /// <param name="eventArgs">Contains unlocked semaphore information if unlocked, null if timeout is reached</param>
        /// <returns>true if an unlocked event has reacher, false when timeout</returns>
        public bool WaitForUnlock (TimeSpan timeout, out FileSemaphoreEventArgs eventArgs) {
            return WaitForUnlock ((int) timeout.TotalMilliseconds, out eventArgs);
        }

        /// <summary>
        /// Stop current thread and wait for a unlocked event or timeout
        /// </summary>
        /// <param name="eventArgs">Contains unlocked semaphore information if unlocked, null if timeout is reached</param>
        /// <returns>true if an unlocked event has reacher, false when timeout</returns>
        public bool WaitForUnlock (out FileSemaphoreEventArgs eventArgs) {
            return WaitForUnlock (-1, out eventArgs);
        }

        /// <summary>
        /// Stop current thread and wait for a unlocked event or timeout
        /// </summary>
        /// <returns>true if an unlocked event has reacher, false when timeout (never in this case)</returns>
        public bool WaitForUnlock () {
            return WaitForUnlock (out FileSemaphoreEventArgs _);
        }

        /// <summary>
        /// Start semaphore checking
        /// </summary>
        public void Start () {
            if (_fileSystemWatcher == null)
                InitfileSystemWatcher ();

            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stop semaphore checking
        /// </summary>
        public void Stop () {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }

        /// <summary>
        /// Event that is fire when the semaphore condition is verified
        /// </summary>
        public event FileSemaphoreEventHandler UnLocked;
        private void OnUnLocked (string filename, string content) {
            UnLocked?.Invoke (this, new FileSemaphoreEventArgs (filename, content));
        }

        private void InitfileSystemWatcher () {
            _fileSystemWatcher = new FileSystemWatcher (
                Path.GetDirectoryName (_fileName),
                Path.GetFileName (_fileName)
            );

            _fileSystemWatcher.NotifyFilter =
                NotifyFilters.LastWrite |
                NotifyFilters.CreationTime |
                NotifyFilters.FileName |
                NotifyFilters.DirectoryName |
                NotifyFilters.Size;

            _fileSystemWatcher.Created += HandleFile;
            _fileSystemWatcher.Renamed += HandleFile;

            if (_syncObject != null)
                _fileSystemWatcher.SynchronizingObject = _syncObject;
            _fileSystemWatcher.IncludeSubdirectories = false;

        }

        private void HandleFile (object sender, FileSystemEventArgs e) {

            if (false == TryReadAllText (_fileName, out string content)) return;
            if (_fileContent != content && !string.IsNullOrEmpty (_fileContent)) return;

            if (_deleteFileAfterProcess)
                TryDeleteFile (_fileName);

            OnUnLocked (_fileName, content);
        }

        private bool TryReadAllText (string fileName, out string fileContent) {
            fileContent = "";
            for (int i = 0; i < READ_FILE_MAX_ATTEMPT; i++) {
                try {
                    fileContent = File.ReadAllText (fileName);
                    return true;
                } catch (Exception) {
                    Thread.Sleep (READ_FILE_ATTEMPT_DELAY);
                }
            }
            return false;
        }

        private bool TryDeleteFile (string fileName) {
            for (int i = 0; i < DELETE_FILE_MAX_ATTEMPT; i++) {
                try {
                    File.Delete (fileName);
                    return true;
                } catch {
                    Thread.Sleep (DELETE_FILE_ATTEMPT_DELAY);
                }
            }
            return false;
        }

        private bool disposedValue;
        /// <summary>
        /// Dispose of the semaphore
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _fileSystemWatcher.Dispose ();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose () {
            Dispose (disposing: true);
            GC.SuppressFinalize (this);
        }

    }

}