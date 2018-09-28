namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System.Windows.Media;

    public class TabViewModel : Screen, Services.ICleanup
    {
        public BindableCollection<string> Messages { get; } = new BindableCollection<string>();

        public Brush color { get; set; } = new SolidColorBrush(Colors.LemonChiffon);

        protected override void OnInitialize()
        {
            Messages.Add($"{DisplayName} Initialized");
        }

        protected override void OnActivate()
        {
            Messages.Add($"{DisplayName} Activated");
        }

        protected override void OnDeactivate(bool close)
        {
            Messages.Add($"{DisplayName} Deactivated, close: {close}");
        }

        public void Cleanup()
        {
            // TODO: Cleanup
        }
    }
}
