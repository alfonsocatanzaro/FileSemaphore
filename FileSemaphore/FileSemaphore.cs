using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Threading
{
    /// <summary>
    /// Consente di creare un semaforo basato su file
    /// </summary>
    public class FileSemaphore : IDisposable
    {
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
        /// Crea un oggetto FileSemaphore
        /// </summary>
        /// <param name="fileName">Nome (completo di percorso) del file da controllare</param>
        /// <param name="fileContent">Contenuto da controllare per far scattare il semaforo; vuoto scatta a qualunque contenuto</param>
        /// <param name="syncObject">Oggetto per la la syncronizzaione del crossthreading</param>
        /// <param name="deleteFileAfterProcess">Cancella il file semafoto quando processato</param>
        public FileSemaphore(string fileName,
            string fileContent = "",
            ISynchronizeInvoke syncObject = null,
            bool deleteFileAfterProcess = false)
        {
            this._fileName = fileName;
            this._fileContent = fileContent;
            this._syncObject = syncObject;
            this._deleteFileAfterProcess = deleteFileAfterProcess;
        }

        /// <summary>
        /// Avvio il controllo del semaforo
        /// </summary>
        public void Start()
        {
            if (_fileSystemWatcher == null)
                InitfileSystemWatcher();

            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Ferma il controllo del semaforo
        /// </summary>
        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }

        /// <summary>
        /// Evento che viene lanciato quando le condizioni del semaforo si verificano
        /// </summary>
        public event FileSemaphoreEventHandler UnLocked;
        private void OnUnLocked(string filename, string content)
        {
            UnLocked?.Invoke(this, new FileSemaphoreEventArgs(filename, content));
        }


        private void InitfileSystemWatcher()
        {
            _fileSystemWatcher = new FileSystemWatcher(
               Path.GetDirectoryName(_fileName),
               Path.GetFileName(_fileName)
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

        private void HandleFile(object sender, FileSystemEventArgs e)
        {

            if (false == TryReadAllText(_fileName, out string content)) return;
            if (_fileContent != content && !string.IsNullOrEmpty(_fileContent)) return;


            if (_deleteFileAfterProcess)
                TryDeleteFile(_fileName);

            OnUnLocked(_fileName, content);
        }

        private bool TryReadAllText(string fileName, out string fileContent)
        {
            fileContent = "";
            for (int i = 0; i < READ_FILE_MAX_ATTEMPT; i++)
            {
                try
                {
                    fileContent = File.ReadAllText(fileName);
                    return true;
                }
                catch (Exception)
                {
                    Thread.Sleep(READ_FILE_ATTEMPT_DELAY);
                }
            }
            return false;
        }

        private bool TryDeleteFile(string fileName)
        {
            for (int i = 0; i < DELETE_FILE_MAX_ATTEMPT; i++)
            {
                try
                {
                    File.Delete(fileName);
                    return true;
                }
                catch
                {
                    Thread.Sleep(DELETE_FILE_ATTEMPT_DELAY);
                }
            }
            return false;
        }


        private bool disposedValue;
        /// <summary>
        /// Dispose dell'oggetto semaforo
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _fileSystemWatcher.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }

}
