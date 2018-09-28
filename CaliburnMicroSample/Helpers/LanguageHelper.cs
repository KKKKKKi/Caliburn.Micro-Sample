namespace CaliburnMicroSample.Helpers
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// 动态加载语言资源
    /// </summary>
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
            if (!(Application.Current.TryFindResource("Strings") is XmlDataProvider provider))
                return;

            provider.Source = new Uri(resourceName, UriKind.RelativeOrAbsolute);

            provider.Refresh();
        }
    }
}
