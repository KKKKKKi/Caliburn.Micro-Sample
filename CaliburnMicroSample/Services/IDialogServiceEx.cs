namespace CaliburnMicroSample.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    public interface IDialogServiceEx : IDialogService
    {
        Task<bool> ShowFileDialog(bool isOpenFile, string filter, Window owner, Action<bool, object> callback);
    }
}
