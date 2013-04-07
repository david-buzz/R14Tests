using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Redbolts.TestCancelMethod.Workers;

namespace Redbolts.TestCancelMethod
{
    public static class IdleQueue
    {
        private readonly static ConcurrentQueue<string> Queue = new ConcurrentQueue<string>();
        private static int _working = 0;
        private static CancellationTokenSource _cts = new CancellationTokenSource();

        public static void QueueWorker(string workerType)
        {
            Queue.Enqueue(workerType);
        }

        public static void BindToken(CancellationTokenSource cts)
        {
            _cts = cts;
        }

        public static bool IsWorking()
        {
            return (_working == 0);
        }

        public static void OnIdle(object sender, IdlingEventArgs e)
        {
            if (Queue.IsEmpty) return;

            //do work
            var application = (UIApplication)sender;
            if (application == null) return;
            if (application.ActiveUIDocument.Document == null) return;
            Task task = null;
            try
            {
                application.Application.WriteJournalComment(
                    "Main thread id :" + Thread.CurrentThread.ManagedThreadId, false);

                task = new Task(() =>
                                    {
                                        RevitContext.BindToken(_cts.Token);
                                        RevitContext.UIApplication = application;
                                        application.Application.WriteJournalComment(
                                            "worker thread id :" + Thread.CurrentThread.ManagedThreadId, false);
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
            }
            catch (OperationCanceledException ex)
            {
                // YES we cancelled
            }
            catch (AggregateException age)
            {
                
            }
            catch (Exception ex)
            {
                // something else happened
            }
            finally
            {
                var isCancelled = _cts.IsCancellationRequested; 
                _cts = new CancellationTokenSource();
                if (task != null)
                    switch (task.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            RevitContext.OutputNoCancel(isCancelled
                                ? "RanToCompletion and tried to Cancel"
                                : "RanToCompletion");
                            break;

                        case TaskStatus.Faulted:
                            RevitContext.OutputNoCancel(isCancelled
                                ? "Faulted and tried to Cancel"
                                : "Faulted");
                            break;

                        case TaskStatus.Canceled:
                            RevitContext.OutputNoCancel(isCancelled
                                ? "Cancelled and tried to Cancel"
                                : "Cancelled");
                            break;
                    }
                Interlocked.Exchange(ref _working, 0);
                task = null;
                // more work try idle again
                if (!Queue.IsEmpty) e.SetRaiseWithoutDelay();
            }
        }

        public static void Cancel()
        {
            _cts.Cancel(true); 
        }
    }
}