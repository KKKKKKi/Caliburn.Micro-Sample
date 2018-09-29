namespace CaliburnMicroSample.Design
{
    using Caliburn.Micro;
    using System.Windows.Media;
    using ViewModels;

    public class ConductorViewModel : Conductor<TabViewModel>.Collection.OneActive
    {
        public ConductorViewModel()
        {
            ActivateItem(new TabViewModel()
            {
                DisplayName = "Deactivate Item",
                color = new SolidColorBrush(Colors.LimeGreen),
            });
            ActivateItem(new TabViewModel()
            {
                DisplayName = "Activate Item",
                color = new SolidColorBrush(Colors.LimeGreen),
            });
        }
    }
}
