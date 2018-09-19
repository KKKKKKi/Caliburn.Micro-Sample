namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System.Windows;
    using System.Windows.Controls;
    using Services;

    public class DragDropViewModel : Screen
    {
        private SimpleContainer _container => IoC.Get<SimpleContainer>();
        // private IDialogServiceEx _dialogService => IoC.Get<IDialogServiceEx>(key: nameof(DialogService));
        // 使用构造方法
        private IDialogServiceEx _dialogService;
        private INavigationService _navigationService => IoC.Get<INavigationService>(key: "ContentFrame");

        public DragDropViewModel(IDialogServiceEx dialogService)
        {
            _dialogService = dialogService;
        }

        public void OnDragEnter(Grid s, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }

        public void OnDragOver(Grid s, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }

        public async void OnFileDrop(Grid s, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();

                if (fileName.ToUpper().EndsWith(".DCM"))
                {
                    _navigationService.For<MainViewModel>()
                        .WithParam(v => v.FilePath, fileName)
                        .Navigate();
                }
                else
                {
                    await _dialogService.ShowError("Invalid File", "Error", null, () => { });
                }
            }
        }
    }
}
