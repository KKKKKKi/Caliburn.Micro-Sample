namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System;
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
        /// languages code dict
        /// </summary>
        private Dictionary<string, string> _langCodeDic;

        /// <summary>
        /// Languages Resource Dict
        /// </summary>
        private Dictionary<string, string> _langDic;

        public SettingViewModel()
        {
            Languages = new BindableCollection<string>();
            _langCodeDic = new Dictionary<string, string>();
            _langDic = new Dictionary<string, string>();

            string langfolder = Environment.CurrentDirectory + "\\Strings";
            DirectoryInfo dir = new DirectoryInfo(langfolder);
            FileInfo[] files = dir.GetFiles("*.xml");  // 获取语言资源文件夹下的所有xml

            for (int i = 0; i < files.Length; i++)
            {
                string path = files[i].FullName;
                XElement lang = XElement.Load(path);
                if (!lang.Name.LocalName.Equals("language"))
                { continue; }
                XAttribute name = lang.Attribute(XName.Get("name"));
                XAttribute tag = lang.Attribute(XName.Get("tag"));
                if (name == null || tag == null)
                { continue; }

                // 加入语言列表
                Languages.Add(name.Value);
                _langCodeDic.Add(tag.Value, name.Value);
                _langDic.Add(name.Value, path);
            }

            // 程序内嵌入简体中文和英文
            // 如果语言资源文件夹下有相应资源
            // 就使用文件夹下资源而不是程序内的
            string zh = "简体中文";
            string en = "English";

            if (!Languages.Contains(zh))
            {
                Languages.Add(zh);
                _langCodeDic.Add("zh-CN", zh);
                _langDic.Add(zh, "pack://application:,,,/Resources/Strings/zh-CN.xml");
            }
            if (!Languages.Contains(en))
            {
                Languages.Add(en);
                _langCodeDic.Add("en-US", en);
                _langDic.Add(en, "pack://application:,,,/Resources/Strings/en-US.xml");
            }
        }

        private string _selectedLanguage;

        /// <summary>
        /// Current Language
        /// <see cref="_selectedLanguage" />
        /// </summary>
        public string SelectedLanguage
        {
            get => _selectedLanguage ?? (_selectedLanguage = "English");
            set
            {
                if (Set(ref _selectedLanguage, value))
                {
                    SwitchLanguage(value);  // 切换语言资源
                }
            }
        }

        private void SwitchLanguage(string key)
        {
            string path = _langDic[key];
            // LanguageHelper.LoadXamlStringsResource($"pack://application:,,,/Resources/Strings/{name}.xaml");  // 加载xaml资源，已经不需要
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
                if (_langCodeDic.ContainsKey(message.Content))
                {
                    SelectedLanguage = _langCodeDic[message.Content];
                }
                else
                {
                    // 如果没有相应的语言资源，那么默认使用 英文
                    SelectedLanguage = "English";
                }
            }
        }

        private string GetLanguageCode(string tvalue)
        {
            foreach (var item in _langCodeDic)
            {
                if (item.Value.Equals(tvalue))
                {
                    return item.Key;
                }
            }

            return "";
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Cleanup()
        {
            _eventAggregator.Unsubscribe(this);  // 取消事件订阅
            // 保存设置
            App.configs.Language = GetLanguageCode(_selectedLanguage);
        }
    }
}
