<a name='assembly'></a>
# FileSemaphore

## Contents

- [FileSemaphore](#T-System-Threading-FileSemaphore 'System.Threading.FileSemaphore')
  - [#ctor(fileName,fileContent,syncObject,deleteFileAfterProcess)](#M-System-Threading-FileSemaphore-#ctor-System-String,System-String,System-ComponentModel-ISynchronizeInvoke,System-Boolean- 'System.Threading.FileSemaphore.#ctor(System.String,System.String,System.ComponentModel.ISynchronizeInvoke,System.Boolean)')
  - [Dispose(disposing)](#M-System-Threading-FileSemaphore-Dispose-System-Boolean- 'System.Threading.FileSemaphore.Dispose(System.Boolean)')
  - [Dispose()](#M-System-Threading-FileSemaphore-Dispose 'System.Threading.FileSemaphore.Dispose')
  - [Start()](#M-System-Threading-FileSemaphore-Start 'System.Threading.FileSemaphore.Start')
  - [Stop()](#M-System-Threading-FileSemaphore-Stop 'System.Threading.FileSemaphore.Stop')
- [FileSemaphoreEventArgs](#T-System-Threading-FileSemaphoreEventArgs 'System.Threading.FileSemaphoreEventArgs')
  - [#ctor(filename,content)](#M-System-Threading-FileSemaphoreEventArgs-#ctor-System-String,System-String- 'System.Threading.FileSemaphoreEventArgs.#ctor(System.String,System.String)')
  - [Content](#P-System-Threading-FileSemaphoreEventArgs-Content 'System.Threading.FileSemaphoreEventArgs.Content')
  - [Filename](#P-System-Threading-FileSemaphoreEventArgs-Filename 'System.Threading.FileSemaphoreEventArgs.Filename')
- [FileSemaphoreEventHandler](#T-System-Threading-FileSemaphoreEventHandler 'System.Threading.FileSemaphoreEventHandler')

<a name='T-System-Threading-FileSemaphore'></a>
## FileSemaphore `type`

##### Namespace

System.Threading

##### Summary

Consente di creare un semaforo basato su file

<a name='M-System-Threading-FileSemaphore-#ctor-System-String,System-String,System-ComponentModel-ISynchronizeInvoke,System-Boolean-'></a>
### #ctor(fileName,fileContent,syncObject,deleteFileAfterProcess) `constructor`

##### Summary

Crea un oggetto FileSemaphore

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fileName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Nome (completo di percorso) del file da controllare |
| fileContent | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Contenuto da controllare per far scattare il semaforo; vuoto scatta a qualunque contenuto |
| syncObject | [System.ComponentModel.ISynchronizeInvoke](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ComponentModel.ISynchronizeInvoke 'System.ComponentModel.ISynchronizeInvoke') | Oggetto per la la syncronizzaione del crossthreading |
| deleteFileAfterProcess | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Cancella il file semafoto quando processato |

<a name='M-System-Threading-FileSemaphore-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Dispose dell'oggetto semaforo

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| disposing | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-System-Threading-FileSemaphore-Dispose'></a>
### Dispose() `method`

##### Summary

Dispose

##### Parameters

This method has no parameters.

<a name='M-System-Threading-FileSemaphore-Start'></a>
### Start() `method`

##### Summary

Avvio il controllo del semaforo

##### Parameters

This method has no parameters.

<a name='M-System-Threading-FileSemaphore-Stop'></a>
### Stop() `method`

##### Summary

Ferma il controllo del semaforo

##### Parameters

This method has no parameters.

<a name='T-System-Threading-FileSemaphoreEventArgs'></a>
## FileSemaphoreEventArgs `type`

##### Namespace

System.Threading

##### Summary

Contiene le informazioni che hanno fatto scattare il semaforo

<a name='M-System-Threading-FileSemaphoreEventArgs-#ctor-System-String,System-String-'></a>
### #ctor(filename,content) `constructor`

##### Summary

Costruisce un oggetto FileSemaphoreEventArgs

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| filename | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Nome del file che ha fatto scattare il semaforo |
| content | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Contenuto letto dal file semaforo |

<a name='P-System-Threading-FileSemaphoreEventArgs-Content'></a>
### Content `property`

##### Summary

Contenuto letto dal file semaforo

<a name='P-System-Threading-FileSemaphoreEventArgs-Filename'></a>
### Filename `property`

##### Summary

Nome del file che ha fatto scattare il semaforo

<a name='T-System-Threading-FileSemaphoreEventHandler'></a>
## FileSemaphoreEventHandler `type`

##### Namespace

System.Threading

##### Summary

Delegato per lachio evento semaforo

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [T:System.Threading.FileSemaphoreEventHandler](#T-T-System-Threading-FileSemaphoreEventHandler 'T:System.Threading.FileSemaphoreEventHandler') | Oggetto che lancia l'evento |
