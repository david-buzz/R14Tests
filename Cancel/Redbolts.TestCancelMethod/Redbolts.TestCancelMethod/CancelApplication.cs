using Autodesk.Revit.UI;
using Redbolts.TestCancelMethod.Composition;

namespace Redbolts.TestCancelMethod
{
    public class CancelApplication:IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            var container = CommandContainer.Instance();
            if (container.Valid)
            {
                container.BuildRibbon(application);
            }
            application.Idling += IdleQueue.OnIdle;
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            application.Idling -= IdleQueue.OnIdle;
            return Result.Succeeded;
        }
    }
}
