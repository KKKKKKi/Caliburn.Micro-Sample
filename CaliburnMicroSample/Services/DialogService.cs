namespace CaliburnMicroSample.Services
{
    using Microsoft.Win32;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    // using GalaSoft.MvvmLight.Views;

    /// <summary>
    /// An implementation of <see cref="IDialogService"/> allowing
    /// to display simple dialogs to the user. Note that this class
    /// uses the built in Windows Phone dialogs which may or may not
    /// be sufficient for your needs. Using this class is easy
    /// but feel free to develop your own IDialogService implementation
    /// if needed.
    /// </summary>
    /// [ClassInfo(typeof(IDialogService))]
    public class DialogService : IDialogServiceEx
    {
        /// <summary>
        /// Displays information about an error.
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <param name="buttonText">The text shown in the only button
        /// in the dialog box. If left null, the text "OK" will be used.</param>
        /// <param name="afterHideCallback">A callback that should be executed after
        /// the dialog box is closed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in Windows Phone is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            MessageBox.Show(message, title ?? string.Empty, MessageBoxButton.OK);

            afterHideCallback?.Invoke();
            tcs.SetResult(true);

            return tcs.Task;
        }

        /// <summary>
        /// Displays information about an error.
        /// </summary>
        /// <param name="error">The exception of which the message must be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <param name="buttonText">The text shown in the only button
        /// in the dialog box. If left null, the text "OK" will be used.</param>
        /// <param name="afterHideCallback">A callback that should be executed after
        /// the dialog box is closed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in Windows Phone is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            MessageBox.Show(error.Message, title ?? string.Empty, MessageBoxButton.OK);

            afterHideCallback?.Invoke();
            tcs.SetResult(true);

            return tcs.Task;
        }

        /// <summary>
        /// Displays information to the user. The dialog box will have only
        /// one button with the text "OK".
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in Windows Phone is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowMessage(string message, string title)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            ShowMessage(message, title ?? string.Empty, null, null);
            tcs.SetResult(true);
            return tcs.Task;
        }

        /// <summary>
        /// Displays information to the user. The dialog box will have only
        /// one button.
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <param name="buttonText">The text shown in the only button
        /// in the dialog box. If left null, the text "OK" will be used.</param>
        /// <param name="afterHideCallback">A callback that should be executed after
        /// the dialog box is closed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in Windows Phone is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            MessageBox.Show(message, title ?? string.Empty, MessageBoxButton.OK);

            afterHideCallback?.Invoke();

            tcs.SetResult(true);
            return tcs.Task;
        }

        /// <summary>
        /// Displays information to the user. The dialog box will have only
        /// one button.
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <param name="buttonConfirmText">The text shown in the "confirm" button
        /// in the dialog box. If left null, the text "OK" will be used.</param>
        /// <param name="buttonCancelText">The text shown in the "cancel" button
        /// in the dialog box. If left null, the text "Cancel" will be used.</param>
        /// <param name="afterHideCallback">A callback that should be executed after
        /// the dialog box is closed by the user. The callback method will get a boolean
        /// parameter indicating if the "confirm" button (true) or the "cancel" button
        /// (false) was pressed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited. The task will return
        /// true or false depending on the dialog result.</returns>
        /// <remarks>Displaying dialogs in Windows Phone is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            MessageBoxResult result = MessageBox.Show(message, title ?? string.Empty, MessageBoxButton.OKCancel);

            afterHideCallback?.Invoke(result == MessageBoxResult.OK || result == MessageBoxResult.Yes);

            tcs.SetResult(result == MessageBoxResult.OK || result == MessageBoxResult.Yes);
            return tcs.Task;
        }

        /// <summary>
        /// Displays information to the user in a simple dialog box. The dialog box will have only
        /// one button with the text "OK". This method should be used for debugging purposes.
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in Windows Phone is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowMessageBox(string message, string title)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            MessageBox.Show(message, title ?? string.Empty, MessageBoxButton.OK);
            tcs.SetResult(true);
            return tcs.Task;
        }


        /// <summary>
        /// Display fileDialog to the user
        /// </summary>
        /// <param name="isOpenFile">File Dialog is for Open file?</param>
        /// <param name="filter">file extension filter</param>
        /// <param name="owner">the dialog's owner</param>
        /// <param name="callback">A callback that should be executed after
        /// the dialog box is closed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited. The task will return
        /// true or false depending on the dialog result.</returns>
        public Task<bool> ShowFileDialog(bool isOpenFile, string filter, Window owner, Action<bool, object> callback)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (isOpenFile)
            {
                OpenFileDialog dlg = new OpenFileDialog()
                {
                    Filter = filter ?? "All Files (*.*)|*.*",
                };
                bool? result = owner == null ? dlg.ShowDialog() : dlg.ShowDialog(owner);

                callback?.Invoke(result == true, dlg.FileName);

                tcs.SetResult(result == true);
            }
            else
            {
                SaveFileDialog dlg = new SaveFileDialog()
                {
                    Filter = filter ?? "All Files (*.*)|*.*",
                };
                bool? result = owner == null ? dlg.ShowDialog() : dlg.ShowDialog(owner);

                callback?.Invoke(result == true, dlg.FileName);

                tcs.SetResult(result == true);
            }

            return tcs.Task;
        }
    }
}
