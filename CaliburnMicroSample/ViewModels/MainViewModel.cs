namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    public class MainViewModel : Screen
    {
        private SimpleContainer _container => IoC.Get<SimpleContainer>();
        private INavigationService _navigationService => IoC.Get<INavigationService>(key: "ContentFrame");

        private string _filePath = null;

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (Set(ref _filePath, value))
                {
                    //
                }
            }
        }
    }
}
