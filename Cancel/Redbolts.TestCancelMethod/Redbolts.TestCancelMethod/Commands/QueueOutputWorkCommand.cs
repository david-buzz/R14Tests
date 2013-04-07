using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Redbolts.TestCancelMethod.Composition;
using Redbolts.TestCancelMethod.Workers;

namespace Redbolts.TestCancelMethod.Commands
{
    [Button(RibbonConstants.Tab, RibbonConstants.Panel, "QueueWorkCmd", "Queue Output Work", LargeImage = "Redbolts.TestCancelMethod.Images.RevitR14.png")]
    [Transaction(TransactionMode.Manual)]
    public class QueueOutputWorkCommand:IExternalCommand
    {
        private static readonly string Fw = typeof(FilterWorker).FullName;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            IdleQueue.QueueWorker(Fw);
            return Result.Succeeded;
        }
    }
}