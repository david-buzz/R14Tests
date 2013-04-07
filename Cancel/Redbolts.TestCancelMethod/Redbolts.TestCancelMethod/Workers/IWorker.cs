using System;
using System.Threading;
using Autodesk.Revit.DB;

namespace Redbolts.TestCancelMethod.Workers
{
    public interface IWorker
    {
        void DoWork();
    }
}