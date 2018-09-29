namespace CaliburnMicroSample.Helpers
{
    using System;
    using System.Windows;

    public static class SystemInfoHelper
    {
        public static string ApplicationName => System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static string ApplicationVersion => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static OperatingSystem OSVersion => Environment.OSVersion;
        public static string MachineName => Environment.MachineName;
        public static string UserName => Environment.UserName;
        public static string UserDomainName => Environment.UserDomainName;
        public static Version DotNETVersion => Environment.Version;
        public static int ProcessorCount => Environment.ProcessorCount;
        public static Rect WindowPositon => Application.Current.MainWindow.RestoreBounds;
        public static double ScreenWidth => SystemParameters.PrimaryScreenWidth;
        public static double ScreenHeight => SystemParameters.PrimaryScreenHeight;
        public static double WorkAreaWidth => SystemParameters.WorkArea.Width;
        public static double WorkAreaHeight => SystemParameters.WorkArea.Height;
    }
}
