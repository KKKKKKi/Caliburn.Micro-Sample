namespace CaliburnMicroSample.Helpers
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public static class LanguageHelper
    {
        public static void LoadXamlStringsResource(string resourceName)
        {
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
            {
                Source = new Uri(resourceName, UriKind.RelativeOrAbsolute)
            };
        }

        public static void LoadXmlStringsResource(string resourceName)
        {
            XmlDataProvider provider = Application.Current.TryFindResource("Strings") as XmlDataProvider;

            if (provider == null)
                return;

            provider.Source = new Uri(resourceName);

            provider.Refresh();
        }
    }
}
