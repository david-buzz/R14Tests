using System;
using System.Threading;
using Autodesk.Revit.DB;

namespace Redbolts.TestCancelMethod.Workers
{
    public class FilterNoOutputWorker:IWorker
    {
        public void DoWork()
        {
            RevitContext.Application.WriteJournalComment("Looping", false);
            using (var transaction = new Transaction(RevitContext.Document))
            {
                transaction.Start("looping");
                var fec = new FilteredElementCollector(RevitContext.Document).WhereElementIsNotElementType().OfClass(typeof(Wall));
                var idx = 1;
                foreach (Wall element in fec)
                {
                    idx++;
                    Thread.Sleep(50);
                }
                transaction.RollBack();
            }
        }
    }
}
