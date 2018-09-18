namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System.Windows;
    using System.Windows.Controls;
    using Models;
    using Services;

    public class ShellViewModel : Screen, IHandle<SimpleMessage>
    {
        private SimpleContainer _container => IoC.Get<SimpleContainer>();
        private IDialogServiceEx _dialogService => IoC.Get<IDialogServiceEx>(key: nameof(DialogService));
        private IEventAggregator _eventAggregator => IoC.Get<IEventAggregator>(key: nameof(EventAggregator));
        private IWindowManager _windowManager => IoC.Get<IWindowManager>(key: nameof(WindowManager));
        private INavigationService _navigationService;

        public ShellViewModel()
        {
            _eventAggregator.Subscribe(this);
        }

        public void Unsubscribe()
        {
            _eventAggregator.Unsubscribe(this);
        }

        public void GoBack()
        {
            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
        }

        public async void OpenFile()
        {
            await _dialogService.ShowFileDialog(true, "DICOM File (*.dcm)|*.dcm", Application.Current.MainWindow,
                (b, p) => {
                    if (b)
                        _navigationService.For<MainViewModel>()
                        .WithParam(v => v.FilePath, p)
                        .Navigate();
                });
        }

        public async void SaveFile()
        {
            await _dialogService.ShowFileDialog(false, "DICOM File (*.dcm)|*.dcm", Application.Current.MainWindow,
                (b, p) => { });
        }

        public void RegisterFrame(Frame frame)
        {
            _navigationService = new FrameAdapter(frame);

            // _container.Instance(_navigationService);
            // INavigationService _navigationService = IoC.Get<INavigationService>(key: "ContentFrame");
            _container.RegisterInstance(typeof(INavigationService), "ContentFrame", _navigationService);
            _navigationService.NavigateToViewModel(typeof(DragDropViewModel), null);
        }

        public void NavToSettings()
        {
            _navigationService.NavigateToViewModel(typeof(SettingViewModel));
        }

        public void AppExit() => Application.Current.Shutdown();

        public void Handle(SimpleMessage message)
        {
            if (message.Sender is SettingViewModel)
            {
                _windowManager.ShowDialog(new DragDropViewModel());
            }
        }
    }
}
