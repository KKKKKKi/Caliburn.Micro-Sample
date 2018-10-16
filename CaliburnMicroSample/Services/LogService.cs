namespace CaliburnMicroSample.Services
{
    using NLog;
    using System;
    using System.Threading.Tasks;
    using Helpers;

    public class LogService : ILogService
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public Task Debug(string msg)
        {
            _logger.Debug(msg);

            return TaskHelper.FromResult(true);
        }

        public Task Debug(string msg, params object[] args)
        {
            _logger.Debug(msg, args);

            return TaskHelper.FromResult(true);
        }

        public Task Debug(Exception err, string msg, params object[] args)
        {
            _logger.Debug(err, msg, args);

            return TaskHelper.FromResult(true);
        }

        public Task Error(string msg)
        {
            _logger.Error(msg);
            return TaskHelper.FromResult(true);
        }

        public Task Error(string msg, params object[] args)
        {
            _logger.Error(msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Error(Exception err, string msg, params object[] args)
        {
            _logger.Error(err, msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Fatal(string msg)
        {
            _logger.Fatal(msg);
            return TaskHelper.FromResult(true);
        }

        public Task Fatal(string msg, params object[] args)
        {
            _logger.Fatal(msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Fatal(Exception err, string msg, params object[] args)
        {
            _logger.Fatal(err, msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Info(string msg)
        {
            _logger.Info(msg);
            return TaskHelper.FromResult(true);
        }

        public Task Info(string msg, params object[] args)
        {
            _logger.Info(msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Info(Exception err, string msg, params object[] args)
        {
            _logger.Info(err, msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Trace(string msg)
        {
            _logger.Trace(msg);
            return TaskHelper.FromResult(true);
        }

        public Task Trace(string msg, params object[] args)
        {
            _logger.Trace(msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Trace(Exception err, string msg, params object[] args)
        {
            _logger.Trace(err, msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Warn(string msg)
        {
            _logger.Warn(msg);
            return TaskHelper.FromResult(true);
        }

        public Task Warn(string msg, params object[] args)
        {
            _logger.Warn(msg, args);
            return TaskHelper.FromResult(true);
        }

        public Task Warn(Exception err, string msg, params object[] args)
        {
            _logger.Warn(err, msg, args);
            return TaskHelper.FromResult(true);
        }
    }
}
