using System;

namespace HDE.Platform.Logging
{
    /// <summary>
    /// Provides output to several logs.
    /// </summary>
    public class QueueLog: LogBase
    {
        #region Logs

        private readonly ILog[] _logs;

        #endregion

        #region Constructors

        public QueueLog(
            params ILog[] logs)
        {
            _logs = logs;
        }

        #endregion

        #region Protected Methods

        protected override void OpenInternal()
        {
            Array.ForEach(_logs, log=>log.Open());
        }

        protected override void CloseInternal()
        {
            Array.ForEach(_logs, log => log.Close());
        }

        protected override void WriteInternal(LoggingEvent loggingEvent, string message)
        {
            Array.ForEach(_logs, log => log.Write(loggingEvent, message));
        }

        #endregion
    }
}
