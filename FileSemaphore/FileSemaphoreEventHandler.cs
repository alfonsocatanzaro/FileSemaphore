using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading
{
    /// <summary>
    /// Delegato per lachio evento semaforo
    /// </summary>
    /// <param name="sender">Oggetto che lancia l'evento</param>
    /// <param name="eventArgs">Contene il Content e il Filename relativo all'evento</param>
    public delegate void FileSemaphoreEventHandler(object sender, FileSemaphoreEventArgs eventArgs);
}
