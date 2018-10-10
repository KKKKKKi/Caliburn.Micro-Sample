namespace KXControls
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public class Window2D : Window
    {
        /// <summary>
        /// Identifies the IsTitleVisible dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTitleVisibleProperty = 
            DependencyProperty.Register(nameof(IsTitleVisible), typeof(bool), typeof(Window2D), new PropertyMetadata(true));

        public bool IsTitleVisible
        {
            get => (bool)GetValue(IsTitleVisibleProperty);
            set => SetValue(IsTitleVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the IsTitleBarVisible dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTitleBarVisibleProperty =
            DependencyProperty.Register(nameof(IsTitleBarVisible), typeof(bool), typeof(Window2D), new PropertyMetadata(true));

        /// <summary>
        /// Get or set the window titlebar is visible
        /// </summary>
        public bool IsTitleBarVisible
        {
            get => (bool)GetValue(IsTitleBarVisibleProperty);
            set => SetValue(IsTitleBarVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the TitieBarBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarBrushProperty =
            DependencyProperty.Register(nameof(TitleBarBrush), typeof(Brush), typeof(Window2D), new PropertyMetadata(null));

        /// <summary>
        /// get or set the window titlebar brush
        /// </summary>
        public Brush TitleBarBrush
        {
            get => (Brush)GetValue(TitleBarBrushProperty);
            set => SetValue(TitleBarBrushProperty, value);
        }

        public Window2D()
        {
            DefaultStyleKey = typeof(Window2D);
#if NET4
            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.SystemCommands.CloseWindowCommand, OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.ShowSystemMenuCommand, OnShowSystemMenu));
#else
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, OnShowSystemMenu));
#endif
        }

        private void OnCanResizeWindow(object s, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object s, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
#if NET4
            Microsoft.Windows.Shell.SystemCommands.CloseWindow(this);
#else
            SystemCommands.CloseWindow(this);
#endif
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
#if NET4
            Microsoft.Windows.Shell.SystemCommands.MaximizeWindow(this);
#else
            SystemCommands.MaximizeWindow(this);
#endif
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
#if NET4
            Microsoft.Windows.Shell.SystemCommands.MinimizeWindow(this);
#else
            SystemCommands.MinimizeWindow(this);
#endif
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
#if NET4
            Microsoft.Windows.Shell.SystemCommands.RestoreWindow(this);
#else
            SystemCommands.RestoreWindow(this);
#endif
        }

        private void OnShowSystemMenu(object s, ExecutedRoutedEventArgs e)
        {
            if (!(e.OriginalSource is FrameworkElement element))
            {
                return;
            }
            Point point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
#if NET4
            Microsoft.Windows.Shell.SystemCommands.ShowSystemMenu(this, point);
#else
            SystemCommands.ShowSystemMenu(this, point);
#endif
        }
    }
}
