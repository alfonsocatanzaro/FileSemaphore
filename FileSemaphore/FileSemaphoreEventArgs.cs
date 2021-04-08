using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading
{
    /// <summary>
    /// Contiene le informazioni che hanno fatto scattare il semaforo
    /// </summary>
    public class FileSemaphoreEventArgs : EventArgs
    {
        /// <summary>
        /// Contenuto letto dal file semaforo
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Nome del file che ha fatto scattare il semaforo
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Costruisce un oggetto FileSemaphoreEventArgs
        /// </summary>
        /// <param name="filename">Nome del file che ha fatto scattare il semaforo</param>
        /// <param name="content">Contenuto letto dal file semaforo</param>
        public FileSemaphoreEventArgs(string filename, string content)
        {
            this.Content = content;
            this.Filename = filename;
        }
    }
}
