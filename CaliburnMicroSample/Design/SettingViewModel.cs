namespace CaliburnMicroSample.Design
{
    using Caliburn.Micro;

    public class SettingViewModel : Screen
    {
        public SettingViewModel()
        {
            Languages = new BindableCollection<string>() { "语言列表" };
            SelectedLanguage = Languages[0];
        }

        /// <summary>
        /// Languages List
        /// </summary>
        public BindableCollection<string> Languages { get; private set; }

        public string SelectedLanguage { get; private set; }
    }
}
