using System;

namespace Redbolts.TestCancelMethod.UI
{
    public class OutputCallback : IOutputCallback
    {
        private readonly Action<string> _messageAction;

        public OutputCallback(Action<string> action)
        {
            _messageAction = action;
        }

        public void Record(string message)
        {
            _messageAction(message);
        }
    }
}