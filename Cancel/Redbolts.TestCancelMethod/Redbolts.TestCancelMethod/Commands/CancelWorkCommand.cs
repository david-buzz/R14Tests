using System;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Redbolts.TestCancelMethod.Composition;
using Redbolts.TestCancelMethod.UI;

namespace Redbolts.TestCancelMethod.Commands
{
    [Button(RibbonConstants.Tab, RibbonConstants.Panel, "CancelCmd", "Cancel Work UI", PanelIndex = 1, LargeImage = "Redbolts.TestCancelMethod.Images.RevitR14.png")]
    [Transaction(TransactionMode.Manual)]
    public class CancelWorkUICommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDel = new ThreadStart(() =>
            {
                var window = new CancelWindow();
                var outputCallBack = new OutputCallback(window.RecordOutput);
                RevitContext.BindOutput(outputCallBack);
                window.ShowDialog();
            });
            var t = new Thread(uiDel);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            return Result.Succeeded;
        }
    }
}