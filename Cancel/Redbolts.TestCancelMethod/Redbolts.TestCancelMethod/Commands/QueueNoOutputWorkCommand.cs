using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Redbolts.TestCancelMethod.Composition;
using Redbolts.TestCancelMethod.Workers;

namespace Redbolts.TestCancelMethod.Commands
{
    [Button(RibbonConstants.Tab, RibbonConstants.Panel, "QueueNoOutputWorkCmd", "Queue NoOutput Work", LargeImage = "Redbolts.TestCancelMethod.Images.RevitR14.png")]
    [Transaction(TransactionMode.Manual)]
    public class QueueNoOutputWorkCommand : IExternalCommand
    {
        private static readonly string Sw = typeof(FilterNoOutputWorker).FullName;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            IdleQueue.QueueWorker(Sw);
            return Result.Succeeded;
        }
    }
}