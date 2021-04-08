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

FileSemaphore class allow you to create a process semaphore that unlock on file event.

<a name='M-System-Threading-FileSemaphore-#ctor-System-String,System-String,System-ComponentModel-ISynchronizeInvoke,System-Boolean-'></a>
### #ctor(fileName,fileContent,syncObject,deleteFileAfterProcess) `constructor`

##### Summary

Create an FileSemaphore class instance

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fileName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Filename that will be check |
| fileContent | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | File content that will be match to unlock the semaphore. Empty string allow to unlock the semaphore with any content. |
| syncObject | [System.ComponentModel.ISynchronizeInvoke](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ComponentModel.ISynchronizeInvoke 'System.ComponentModel.ISynchronizeInvoke') | Object to use to invoke event in threadsafe |
| deleteFileAfterProcess | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Delete file when semaphore is unlocked |

<a name='M-System-Threading-FileSemaphore-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Dispose of the semaphore

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

Start semaphore checking

##### Parameters

This method has no parameters.

<a name='M-System-Threading-FileSemaphore-Stop'></a>
### Stop() `method`

##### Summary

Stop semaphore checking

##### Parameters

This method has no parameters.

<a name='T-System-Threading-FileSemaphoreEventArgs'></a>
## FileSemaphoreEventArgs `type`

##### Namespace

System.Threading

##### Summary

Contains information about filesemaphore unlock event

<a name='M-System-Threading-FileSemaphoreEventArgs-#ctor-System-String,System-String-'></a>
### #ctor(filename,content) `constructor`

##### Summary

Create a FileSemaphoreEventArgs object

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| filename | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Filename that has unlocked the semaphore |
| content | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Content of the file that has unlocked the semaphore |

<a name='P-System-Threading-FileSemaphoreEventArgs-Content'></a>
### Content `property`

##### Summary

Content of the file that has unlocked the semaphore

<a name='P-System-Threading-FileSemaphoreEventArgs-Filename'></a>
### Filename `property`

##### Summary

Filename that has unlocked the semaphore

<a name='T-System-Threading-FileSemaphoreEventHandler'></a>
## FileSemaphoreEventHandler `type`

##### Namespace

System.Threading

##### Summary

Delegate usend for firing Unlocked event

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [T:System.Threading.FileSemaphoreEventHandler](#T-T-System-Threading-FileSemaphoreEventHandler 'T:System.Threading.FileSemaphoreEventHandler') | Object that has fired the event |
