namespace CaliburnMicroSample
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Input;  // Key Bindings Convertion
    using Models;
    using Services;
    using ViewModels;

    public class AppBootstrapper : BootstrapperBase, ICleanup
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public AppBootstrapper()
        {
            Initialize();
            // 加载设置
            LoadConfigs();
        }

        protected override void Configure()
        {
            _container.Instance<SimpleContainer>(_container);

            _container.Singleton<IWindowManager, WindowManager>(key: nameof(WindowManager))
                .Singleton<IEventAggregator, EventAggregator>(key: nameof(EventAggregator))
                .Singleton<IDialogServiceEx, DialogService>(key: nameof(DialogService));

            _container.Singleton<ShellViewModel>(key: nameof(ShellViewModel))
                .PerRequest<DragDropViewModel>(key: nameof(DragDropViewModel))
                .Singleton<MainViewModel>(key: nameof(MainViewModel))
                .Singleton<SettingViewModel>(key: nameof(SettingViewModel))
                .Singleton<ConductorViewModel>(key: nameof(ConductorViewModel))
                .Singleton<SampleViewModel>(key: nameof(SampleViewModel))
                .Singleton<WinMsgViewModel>(key: nameof(WinMsgViewModel));

            // Key Bindings Convertion
            /*********************************************************************************************/
            Func<DependencyObject, string, System.Windows.Interactivity.TriggerBase> defaultCreateTrigger = Parser.CreateTrigger;

            Parser.CreateTrigger = (target, triggerText) =>
            {
                if (triggerText == null)
                    return defaultCreateTrigger(target, null);

                string triggerDetail = triggerText.Replace("[", string.Empty).Replace("]", string.Empty);

                string[] splits = triggerDetail.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                switch (splits[0])
                {
                    case "Key":
                        Key key = (Key)Enum.Parse(typeof(Key), splits[1], true);
                        return new KeyTrigger { Key = key };

                    case "Gesture":
                        MultiKeyGesture mkg = (MultiKeyGesture)new MultiKeyGestureConverter().ConvertFrom(splits[1]);
                        return new KeyTrigger { Modifiers = mkg.KeySequences[0].Modifiers, Key = mkg.KeySequences[0].Keys[0] };

                    default: break;
                }

                return defaultCreateTrigger(target, triggerText);
            };

            MessageBinder.SpecialValues.Add("$sender", ctx => ctx.Source);
            /*********************************************************************************************/
        }

        protected override void OnStartup(object s, StartupEventArgs e)
        {
            NotifyLoadLanguage();  // Notify Settings Load Language
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void OnExit(object s, EventArgs e)
        {
            Cleanup();
            SaveConfigs();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        /// <summary>
        /// 全局错误处理
        /// </summary>
        /// <param name="s">sender</param>
        /// <param name="e">event args</param>
        protected override void OnUnhandledException(object s, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message, "(^_−)≡★", MessageBoxButton.OK);
        }

        #region configs
        /* configs */
        /*********************************************************************************************/
        
        private void LoadConfigs()
        {
            App.configs = Configs.LoadConfigs();
        }

        private void SaveConfigs()
        {
            App.configs.SaveConfigs();
        }

        // 给设置发送消息
        // 加载语言资源文件
        private void NotifyLoadLanguage()
        {
            IEventAggregator eventAggregator = IoC.Get<IEventAggregator>(nameof(EventAggregator));
            eventAggregator.Subscribe(IoC.Get<SettingViewModel>(nameof(SettingViewModel)));
            eventAggregator.PublishOnUIThread(new SimpleMessage(this, "", App.configs.Language));
        }
        /*********************************************************************************************/
        #endregion
        
        /// <summary>
        /// Cleanup
        /// </summary>
        public void Cleanup()
        {
            IoC.Get<ShellViewModel>(nameof(ShellViewModel)).Cleanup();
            IoC.Get<DragDropViewModel>(nameof(DragDropViewModel)).Cleanup();
            IoC.Get<MainViewModel>(nameof(MainViewModel)).Cleanup();
            IoC.Get<SettingViewModel>(nameof(SettingViewModel)).Cleanup();
            IoC.Get<ConductorViewModel>(nameof(ConductorViewModel)).Cleanup();
            IoC.Get<SampleViewModel>(nameof(SampleViewModel)).Cleanup();
            IoC.Get<WinMsgViewModel>(nameof(WinMsgViewModel)).Cleanup();
        }
    }
}
