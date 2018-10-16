namespace CaliburnMicroSample.Services
{
    using System;
    using System.Threading.Tasks;

    public interface ILogService
    {
        Task Trace(string msg);
        Task Trace(string msg, params object[] args);
        Task Trace(Exception err, string msg, params object[] args);
        Task Debug(string msg);
        Task Debug(string msg, params object[] args);
        Task Debug(Exception err, string msg, params object[] args);
        Task Info(string msg);
        Task Info(string msg, params object[] args);
        Task Info(Exception err, string msg, params object[] args);
        Task Warn(string msg);
        Task Warn(string msg, params object[] args);
        Task Warn(Exception err, string msg, params object[] args);
        Task Error(string msg);
        Task Error(string msg, params object[] args);
        Task Error(Exception err, string msg, params object[] args);
        Task Fatal(string msg);
        Task Fatal(string msg, params object[] args);
        Task Fatal(Exception err, string msg, params object[] args);
    }
}
