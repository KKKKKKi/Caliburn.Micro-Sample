namespace CaliburnMicroSample
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Input;  // Key Bindings Convertion
    using Services;
    using ViewModels;

    public class AppBootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public AppBootstrapper()
        {
            Initialize();
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
                .PerRequest<ConductorViewModel>(key: nameof(ConductorViewModel));

            // Key Bindings Convertion
            var defaultCreateTrigger = Parser.CreateTrigger;

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
                        MultiKeyGesture mkg = (MultiKeyGesture)(new MultiKeyGestureConverter()).ConvertFrom(splits[1]);
                        return new KeyTrigger { Modifiers = mkg.KeySequences[0].Modifiers, Key = mkg.KeySequences[0].Keys[0] };

                    default: break;
                }

                return defaultCreateTrigger(target, triggerText);
            };
        }

        protected override void OnStartup(object s, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void OnExit(object s, EventArgs e)
        {
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
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK);
        }
    }
}
