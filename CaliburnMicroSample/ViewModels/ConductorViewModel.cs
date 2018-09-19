namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System.Windows.Media;

    public class ConductorViewModel : Conductor<TabViewModel>.Collection.OneActive
    {
        private int _count = 0;

        public ConductorViewModel()
        {
            Items.CollectionChanged += (s, e) => NotifyOfPropertyChange(() => CanCloseTab);
        }

        protected override void OnInitialize()
        {
            AddTab();
        }

        public void AddTab()
        {
            ActivateItem(new TabViewModel() { DisplayName = $"Tab {_count++}", color = new SolidColorBrush(Colors.LemonChiffon) });
        }

        public void CloseTab()
        {
            DeactivateItem(ActiveItem, close: true);
        }

        public bool CanCloseTab => Items.Count > 1;
    }
}
