using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading {
    
    /// <summary>
    /// Contains information about filesemaphore unlock event
    /// </summary>
    public class FileSemaphoreEventArgs : EventArgs {

        /// <summary>
        /// Content of the file that has unlocked the semaphore
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Filename that has unlocked the semaphore
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Create a FileSemaphoreEventArgs object
        /// </summary>
        /// <param name="filename">Filename that has unlocked the semaphore</param>
        /// <param name="content">Content of the file that has unlocked the semaphore</param>
        public FileSemaphoreEventArgs (string filename, string content) {
            this.Content = content;
            this.Filename = filename;
        }
    }
}