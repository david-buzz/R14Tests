#Cancel Test

This application is not specific to R14. I need to be able to cancel a worker method that takes no parameters and not my code. However, I couldn't find a way to make it work in all situations. This is the fudge. 

Using Task's running synchronously allows me to exploit the continuation options using what is a very nice .NET API, but is actually not required for this cancelling of workers.

*Note: According to the TaskScheduler docs it is possible the current scheduler wouldn't be used. However, all my testing to date shows the task runs on the revit thread. So I'm not sure what situations the TaskScheduler would use the default..... And probably Autodesk would tell you not to use Tasks like this...*

##Code
There are 3 revit commands. 'CancelWorkUICommand' starts a window on another thread and pushing the Cancel button sets the 'CancellationTokenSource' to cancel.'QueueOutputWorkCommand' and 'QueueNoOutputWorkCommand' are used to queue work to be done.

'RevitContext' checks the CancellationToken as often as the properties are accessed and throws if cancellation is requested. So it's not close to being an ideal solution and depending on how the worker code is written it is entirely possible the method would not be cancelled before completion as 'QueueNoOutputWorkCommand' shows.

The IdleQueue bit of code that executes the worker method:

    task = new Task(() =>
    {
        RevitContext.BindToken(_cts.Token);
        RevitContext.UIApplication = application;
        application.Application.WriteJournalComment("worker thread id :" + Thread.CurrentThread.ManagedThreadId, false);
        Interlocked.Exchange(ref _working, 1); //working
        string worker;
        if (!Queue.TryDequeue(out worker)) return;
        if (String.IsNullOrEmpty(worker)) return;

        var workerType = Type.GetType(worker);
        if (workerType == null) return;
        var workerObject = (IWorker)Activator.CreateInstance(workerType);
        workerObject.DoWork();
    },_cts.Token);
    task.RunSynchronously(TaskScheduler.Current);
    task.Wait(_cts.Token);
