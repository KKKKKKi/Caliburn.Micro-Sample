﻿namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using Services;

    public class MainViewModel : Screen, ICleanup
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
                    // TODO: File Operate
                }
            }
        }

        public void Cleanup()
        {
            // TODO: Cleanup
        }
    }
}
