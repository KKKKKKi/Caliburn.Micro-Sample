namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Helpers;
    using Models;
    using Services;
    using Views;
    using System.Windows;

    public class SampleViewModel : Screen, IHandle<SimpleMessage>, ICleanup
    {
        private IEventAggregator _eventAggregator => IoC.Get<IEventAggregator>(key: nameof(EventAggregator));
        private IDialogServiceEx _dialogService => IoC.Get<IDialogServiceEx>(key: nameof(DialogService));

        public SampleViewModel()
        {
            _eventAggregator.Subscribe(this);
        }
        
        protected override void OnActivate()
        {
            Rect rect = SystemInfoHelper.WindowPositon;
            SampleView view = GetView() as SampleView;
            view.Top = rect.Top;
            view.Left = rect.Right;
            view.Height = rect.Height;

            view.Owner = Application.Current.MainWindow;
        }

        public bool CanSayHello(string name) => !string.IsNullOrEmpty(name);
        public async Task SayHelloAsync(string name) => await _dialogService.ShowMessageBox($"Hello {name}!", "Message");

        public async Task CommandWithParamAsync(Button s, object sender, RoutedEventArgs e, object dataContext, object view, object defaultThis)
        {
            string output = "";
            if (sender.Equals(s))
            {
                output += "0";
            }
            if (s.DataContext.Equals(dataContext))
            {
                output += "1";
            }
            if (dataContext is SampleViewModel)
            {
                output += "2";
            }
            if (view is SampleView v)
            {
                output += "3";
            }
            if (defaultThis is SampleViewModel)
            {
                output += "4";
            }
            await _dialogService.ShowMessageBox(s.Name, "Message");
        }

        public void Handle(SimpleMessage message)
        {
            if (message.Sender is ShellView shell)
            {
                SampleView view = GetView() as SampleView;
                view.Top = shell.Top;
                view.Left = shell.Left + shell.Width;
                view.Height = shell.Height;
            }
        }

        public void Cleanup()
        {
            // TODO: Cleanup
            _eventAggregator.Unsubscribe(this);
        }
    }
}
