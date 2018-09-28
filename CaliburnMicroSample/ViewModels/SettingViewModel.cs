namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Helpers;
    using Models;

    public class SettingViewModel : Screen, Services.ICleanup, IHandle<SimpleMessage>
    {
        private IEventAggregator _eventAggregator => IoC.Get<IEventAggregator>(nameof(EventAggregator));

        /// <summary>
        /// Languages List
        /// </summary>
        public BindableCollection<string> Languages { get; private set; }

        /// <summary>
        /// Languages Resource Dict
        /// </summary>
        private Dictionary<string, string> _langDic;

        public SettingViewModel()
        {
            if (Execute.InDesignMode)
            {
                Languages = new BindableCollection<string>() { "语言" };
                _selectedLanguage = Languages[0];
            }
            else
            {
                Languages = new BindableCollection<string>();
                _langDic = new Dictionary<string, string>();

                string langfolder = System.Environment.CurrentDirectory + "\\Strings";
                DirectoryInfo dir = new DirectoryInfo(langfolder);
                FileInfo[] files = dir.GetFiles("*.xml");  // 获取语言资源文件夹下的所有xml

                for (int i = 0; i < files.Length; i++)
                {
                    string path = files[i].FullName;
                    XElement lang = XElement.Load(path);
                    if (!lang.Name.LocalName.Equals("language"))
                    { continue; }
                    XAttribute name = lang.Attribute(XName.Get("name"));
                    if (name == null)
                    { continue; }

                    // 加入语言列表
                    Languages.Add(name.Value);
                    _langDic.Add(name.Value, path);
                }
            }
        }

        private string _selectedLanguage;

        /// <summary>
        /// Current Language
        /// <see cref="_selectedLanguage" />
        /// </summary>
        public string SelectedLanguage
        {
            get => _selectedLanguage ?? (_selectedLanguage = App.configs.Language);
            set
            {
                if (Set(ref _selectedLanguage, value))
                {
                    SwitchLanguage(value);  // 切换语言资源
                    App.configs.Language = value;  // 更改应用配置
                }
            }
        }

        private void SwitchLanguage(string key)
        {
            string path = _langDic[key];
            // LanguageHelper.LoadXamlStringsResource($"pack://application:,,,/Resources/Strings/{name}.xaml");
            LanguageHelper.LoadXmlStringsResource(path);  // 加载xml资源文件
        }

        public void MessageToShell()
        {
            _eventAggregator.PublishOnUIThread(new SimpleMessage(this, "メル", "Message from SettingViewModel."));
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="message">SimpleMessage</param>
        public void Handle(SimpleMessage message)
        {
            if (message.Sender is AppBootstrapper)
            {
                // 如果没有语言资源
                // 就从程序内加载资源文件
                if (_langDic.Count == 0 || Languages.Count == 0)
                {
                    Languages = new BindableCollection<string>() { "简体中文" };
                    // _langDic = new Dictionary<string, string>() { { "简体中文", "zh-Hans-CN" } };
                    _selectedLanguage = Languages[0];
                    App.configs.Language = _selectedLanguage;
                    // LanguageHelper.LoadXamlStringsResource("pack://application:,,,/Resources/Strings/zh-Hans-CN.xaml");
                    LanguageHelper.LoadXmlStringsResource("pack://application:,,,/Resources/Strings/zh-Hans-CN.xml");
                    return;
                }

                if (_langDic.ContainsKey(message.Content))
                {
                    SelectedLanguage = message.Content;
                }
                else
                {
                    // 如果没有相应的语言资源，那么选用已有资源文件的第一个
                    SelectedLanguage = Languages[0];
                }
            }
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Cleanup()
        {
            _eventAggregator.Unsubscribe(this);  // 取消事件订阅
        }
    }
}
