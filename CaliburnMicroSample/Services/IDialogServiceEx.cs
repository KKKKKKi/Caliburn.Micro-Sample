namespace CaliburnMicroSample.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    public interface IDialogServiceEx : IDialogService
    {
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
        Task<bool> ShowFileDialog(bool isOpenFile, string filter, Window owner, Action<bool, object> callback);
    }
}
