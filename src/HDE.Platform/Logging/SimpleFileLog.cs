using System;

namespace HDE.Platform.Logging
{
    /// <summary>
    /// Logs and outputs information to File
    /// </summary>
    public class SimpleFileLog : HtmlLog
    {
        #region Constructors

        public SimpleFileLog(string logsFolder):
            base(logsFolder, string.Empty)
        {
            LogExtension = ".log";
            LogFooter = string.Empty;
        }

        #endregion

        #region Protected Methods

        protected override void WriteInternal(LoggingEvent loggingEvent, string message)
        {
            string output = string.Format("{1} [{0}]\t{2}", loggingEvent, DateTime.UtcNow.ToString("G"), message);;
            WriteCore(output);
        }

        #endregion
    }
}