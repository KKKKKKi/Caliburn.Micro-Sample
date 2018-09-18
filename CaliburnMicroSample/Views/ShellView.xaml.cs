namespace CaliburnMicroSample.Views
{
    /// <summary>
    /// ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void MenuButton_Click(object s, System.Windows.RoutedEventArgs e)
        {
            MenuPopup.IsOpen = !MenuPopup.IsOpen;
        }
    }
}
