using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Redbolts.DoSomeWork
{
    public class DoSomething:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var td = new TaskDialog("Run it") { MainContent = "YIPEE DO DAH!!", CommonButtons = TaskDialogCommonButtons.Close };
            td.Show();
            return Result.Succeeded;
        }
    }
}
