namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using Helpers;

    public class SettingViewModel : Screen
    {
        private IEventAggregator _eventAggregator => IoC.Get<IEventAggregator>(nameof(EventAggregator));

        public BindableCollection<string> Languages { get; } = new BindableCollection<string>()
        {
            "简体中文", "English",
        };

        private string _selectedLanguage;

        public string SelectedLanguage
        {
            get => _selectedLanguage ?? (_selectedLanguage = Languages[0]);
            set
            {
                if (Set(ref _selectedLanguage, value))
                {
                    SwitchLanguage(value);
                }
            }
        }

        private void SwitchLanguage(string key)
        {
            switch (key)
            {
                case "简体中文":
                    LanguageHelper.LoadXamlStringsResource("pack://application:,,,/Resources/Strings/zh-Hans-CN.xaml");
                    break;
                case "English":
                    LanguageHelper.LoadXamlStringsResource("pack://application:,,,/Resources/Strings/en-US.xaml");
                    break;
                default:
                    break;
            }
        }

        public void MessageToShell()
        {
            _eventAggregator.PublishOnUIThread(new Models.SimpleMessage(this, "Message from SettingViewModel."));
        }
    }
}
