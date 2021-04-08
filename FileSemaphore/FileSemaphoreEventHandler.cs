using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading
{
    /// <summary>
    /// Delegate usend for firing Unlocked event
    /// </summary>
    /// <param name="sender">Object that has fired the event</param>
    /// <param name="eventArgs">Information about the Unlocked event (filename, content)</param>
    public delegate void FileSemaphoreEventHandler(object sender, FileSemaphoreEventArgs eventArgs);
}
