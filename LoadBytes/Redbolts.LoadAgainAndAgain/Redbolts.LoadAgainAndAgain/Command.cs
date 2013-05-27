using System.IO;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Redbolts.LoadAgainAndAgain
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var workPath = Path.Combine(directory, "Redbolts.DoSomeWork.dll");
            var assemBytes = File.ReadAllBytes(workPath);
            var assem = Assembly.Load(assemBytes);
            var instance = assem.CreateInstance("Redbolts.DoSomeWork.DoSomething", false) as IExternalCommand;
            return instance != null ? instance.Execute(commandData, ref message, elements) : Result.Failed;
        }
    }
}
