namespace CaliburnMicroSample.Log
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Timers;

    public class LogClient
    {
        private static LogClient _instance;
        private string _logfile;
        private string _logfolder;
        private Queue<LogEntry> _logEntries;
        private object _logEntriesLock = new object();
        private Timer _logTimer = new Timer();
        private bool _isInitialized;
        private int _archiveAboveSize = 5242880;  // 5MB
        private int _maxArchiveFiles = 3;
        
        public static LogClient Instance => _instance ?? (_instance = new LogClient());
        public static string Logfile => Instance._logfile;

        public static void Info(
            string message, 
            object[] args, 
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Instance.AddLogEntry(LogLevel.Info, message, args, sourceFilePath, memberName, lineNumber);
        }

        public static void Warning(
            string message,
            object[] args,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Instance.AddLogEntry(LogLevel.Warning, message, args, sourceFilePath, memberName, lineNumber);
        }

        public static void Error(
            string message,
            object[] args,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Instance.AddLogEntry(LogLevel.Error, message, args, sourceFilePath, memberName, lineNumber);
        }

        private LogClient()
        {
            _logfolder = "";
            _logfile = "";
            _logEntries = new Queue<LogEntry>();
            _logTimer.Interval = 25;
            _logTimer.Elapsed += (_, __) =>
            {
                _logTimer.Stop();

                lock (_logEntriesLock)
                {
                    TryWrite(_logEntries.Dequeue());
                    _logTimer.Start();
                }
            };
        }

        public static void Initialize(int archiveAboveSize, int maxArchiveFiles)
        {
            if (LogClient.Instance._isInitialized) return;
            LogClient.Instance._isInitialized = true;
            LogClient.Instance._archiveAboveSize = archiveAboveSize;
            LogClient.Instance._maxArchiveFiles = maxArchiveFiles;
        }

        private void AddLogEntry(LogLevel level, string message, object[] args, string callerFilePath, string callerMemberName, int lineNumber)
        {
            this._logTimer.Stop();

            try
            {
                if (args != null) message = string.Format(message, args);

                string levelDescription = string.Empty;

                switch (level)
                {
                    case LogLevel.Info:
                        levelDescription = "Info";
                        break;
                    case LogLevel.Warning:
                        levelDescription = "Warning";
                        break;
                    case LogLevel.Error:
                        levelDescription = "Error";
                        break;
                    default:
                        levelDescription = "Error";
                        break;
                }

                lock (_logEntriesLock)
                {
                    this._logEntries.Enqueue(new LogEntry
                    {
                        TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        Level = levelDescription,
                        CallerFilePath = callerFilePath,
                        CallerMemberName = callerMemberName,
                        Message = message,
                        CallerLineNumber = lineNumber
                    });
                }
            }
            catch (Exception)
            {
                // A failure to should never crash the application
            }

            this._logTimer.Start();
        }

        private void TryWrite(LogEntry entry)
        {
            try
            {
                // If the log directory doesn't exist, create it.
                if (!Directory.Exists(this._logfolder)) Directory.CreateDirectory(this._logfolder);

                // If the log file doesn't exist, this also creates it.
                bool isWriteSuccess = false;

                while (!isWriteSuccess)
                {
                    try
                    {
                        using (StreamWriter sw = File.AppendText(this._logfile))
                        {
                            sw.WriteLine($"{entry.TimeStamp}|{entry.Level}|{Path.GetFileNameWithoutExtension(entry.CallerFilePath)}.{entry.CallerMemberName}|{entry.CallerLineNumber}|{entry.Message}");
                        }
                        isWriteSuccess = true;
                    }
                    catch (Exception)
                    {
                    }
                }

                // Rotate the log file
                try
                {
                    this.RotateLogfile();
                }
                catch (Exception)
                {
                }

                // Delete archived log files
                try
                {
                    this.DeleteArchives();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void RotateLogfile()
        {
            var fi = new FileInfo(this._logfile);

            if (fi.Length >= this._archiveAboveSize)
            {
                string archivedLogfile = Path.Combine(Path.GetDirectoryName(this._logfile), Path.GetFileNameWithoutExtension(this._logfile) + DateTime.Now.ToString(" (yyyy-MM-dd HH.mm.ss.fff)") + ".log");
                File.Move(this._logfile, archivedLogfile);
            }
        }

        private void DeleteArchives()
        {
            var di = new DirectoryInfo(this._logfolder);

            FileInfo[] files = di.GetFiles().OrderBy(p => p.CreationTime).ToArray();

            while (files.Length > this._maxArchiveFiles + 1)
            {
                FileInfo fi = files.FirstOrDefault();
                if (fi != null) File.Delete(fi.FullName);
                files = di.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            }
        }

        public static string GetAllExceptions(Exception ex)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Exception:");
            sb.AppendLine(ex.ToString());
            sb.AppendLine("");
            sb.AppendLine("Stack trace:");
            sb.AppendLine(ex.StackTrace);

            int innerExceptionCounter = 0;

            while (ex.InnerException != null)
            {
                innerExceptionCounter += 1;
                sb.AppendLine("Inner Exception " + innerExceptionCounter + ":");
                sb.AppendLine(ex.InnerException.ToString());
                ex = ex.InnerException;
            }

            return sb.ToString();
        }
    }
}
