namespace CaliburnMicroSample.Views
{
    using System;
    using System.Windows.Interop;
    using Helpers;
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

            // Add Hook
            // HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            // source.AddHook(Vm.WndProc);

            HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(new HwndSourceHook(Vm.WndProc));
        }
    }
}
