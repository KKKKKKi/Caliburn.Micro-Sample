namespace CaliburnMicroSample.Views
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using ViewModels;

    /// <summary>
    /// WinMsgView.xaml 的交互逻辑
    /// </summary>
    public partial class WinMsgView
    {
        public WinMsgViewModel Vm => DataContext as WinMsgViewModel;

        public WinMsgView()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(Vm.WndProc);
        }
    }
}
