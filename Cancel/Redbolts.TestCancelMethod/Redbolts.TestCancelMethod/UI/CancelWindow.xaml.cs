using System;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Redbolts.TestCancelMethod.UI
{
    /// <summary>
    /// Interaction logic for CancelWindow.xaml
    /// </summary>
    public partial class CancelWindow : Window
    {
        public CancelWindow()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            RevitContext.BindOutput(new RevitContext.DummyCallback());
            Close();
        }

        private async void CancelClick(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                if (IdleQueue.IsWorking()) return;
                IdleQueue.Cancel();
            }); 
        }

        public void RecordOutput(string message)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,(Action)(() =>
                                                            {
                                                                MessageLabel.Content = message;
                                                            }));           
        }
    }
}
