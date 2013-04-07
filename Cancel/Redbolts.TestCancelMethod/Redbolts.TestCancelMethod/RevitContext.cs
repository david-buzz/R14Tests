using System.Threading;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Redbolts.TestCancelMethod.UI;

namespace Redbolts.TestCancelMethod
{
    public static class RevitContext
    {
        private static UIApplication _uiApplication;
        private static CancellationToken _token;
        private static IOutputCallback _output = new DummyCallback();

        internal static UIApplication UIApplication
        {
            get
            {
                _token.ThrowIfCancellationRequested();
                return _uiApplication;
            }
            set
            {
                _token.ThrowIfCancellationRequested();
                _uiApplication = value;
            }
        }

        internal static void BindToken(CancellationToken token)
        {
            _token = token;
            _token.ThrowIfCancellationRequested();
        }

        internal static void BindOutput(IOutputCallback output)
        {
            _output = output;
        }

        public static Application Application
        {
            get { return UIApplication.Application; }
        }

        public static Document Document
        {
            get { return UIApplication.ActiveUIDocument.Document; }
        }

        public static void Output(string message)
        {
            _token.ThrowIfCancellationRequested();
            _output.Record(message);
        }

        internal static void OutputNoCancel(string message)
        {
            _output.Record(message);
        }

        internal class DummyCallback : IOutputCallback
        {
            public void Record(string message)
            {
            }
        }
    }
}