using System;
using System.Globalization;

namespace HDE.Platform.Logging
{
    /// <summary>
    /// Handles most functional of ILog interface.
    /// </summary>
    public abstract class LogBase : ILog, IDisposable
    {
        #region Fields

        protected string _logFile;

        #endregion

        #region Properties

        public string LogFile
        {
            get { return _logFile; }
        }

        public bool IsOpened { get; private set; }
        public bool IsErrorsLogged { get; set; }
        public bool IsWarningsLogged { get; set; }

        #endregion

        #region Protected Methods

        protected abstract void OpenInternal();
        protected abstract void CloseInternal();
        protected abstract void WriteInternal(LoggingEvent loggingEvent, string message);

        #endregion

        #region Public Methods

        public void Close()
        {
            if (IsOpened)
            {
                CloseInternal();
                IsOpened = false;
            }
        }

        public void Open()
        {
            if (IsOpened)
            {
                throw new LogException("already opened!");
            }

            OpenInternal();
            IsOpened = true;
        }

        public void Write(LoggingEvent loggingEvent, string message, params object[] arguments)
        {
            Write(loggingEvent, string.Format(CultureInfo.CurrentCulture, message, arguments));
        }

        public void Write(LoggingEvent loggingEvent, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                preprocessLogEntry(loggingEvent, message);
                WriteInternal(loggingEvent, message);
            }
        }

        public void Debug(string message)
        {
            Write(LoggingEvent.Debug, message);
        }

        public void Debug(string message, params object[] args)
        {
            Write(LoggingEvent.Debug, message, args);
        }

        public void Info(string message)
        {
            Write(LoggingEvent.Info, message);
        }

        public void Info(string message, params object[] args)
        {
            Write(LoggingEvent.Info, message, args);
        }

        public void Error(Exception unhandledException)
        {
            if (unhandledException != null)
            {
                Error(unhandledException.ToString());
            }
        }

        public void Error(string message)
        {
            Write(LoggingEvent.Error, message);
        }

        public void Error(string message, params object[] args)
        {
            Write(LoggingEvent.Error, message, args);
        }

        public void Warning(string message)
        {
            Write(LoggingEvent.Warning, message);
        }

        public void Warning(string message, params object[] args)
        {
            Write(LoggingEvent.Warning, message, args);
        }

        public void Dispose()
        {
            if (IsOpened)
            {
                Close();
            }
        }

        #endregion

        #region Private Methods

        private void preprocessLogEntry(LoggingEvent loggingEvent, string message)
        {
            if (!IsOpened)
            {
                System.Diagnostics.Debug.WriteLine("Not opened");
                System.Diagnostics.Debug.WriteLine(Environment.StackTrace);
                throw new LogException("log is closed");
            }

            if (loggingEvent == LoggingEvent.Error)
            {
                IsErrorsLogged = true;
            }

            if (loggingEvent == LoggingEvent.Warning)
            {
                IsWarningsLogged = true;
            }

            System.Diagnostics.Debug.Write(string.Format("[{0}] ", loggingEvent));
            System.Diagnostics.Debug.WriteLine(message);
        }

        #endregion
    }
}
