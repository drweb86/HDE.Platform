using System;
using System.Text;

namespace HDE.Platform.Logging
{
    public static class SevenZipLogHelper
    {
        /// <summary>
        /// Grabs output from console and converts it readable format, writes to log
        /// </summary>
        public static void HandleOutput(
            ILog log, 
            string consoleOutput, 
            bool finishedSuccessfully, 
            
            bool onlyErrors,
            bool destinationLogIsConsole,
            bool loggingAppIsConsole)
        {
            Encoding initialEncoding = Encoding.GetEncoding("cp866");
            Encoding targetEncoding = Encoding.Default;
            // packer encodings

            if (destinationLogIsConsole)
            {
                initialEncoding = targetEncoding;
            }
            else
            {
                if (loggingAppIsConsole)
                {
                    targetEncoding = initialEncoding;
                }
                else
                {
                    targetEncoding = Encoding.Default;
                }
            }


            // Preparation of string to process
            string DestinationString = convertPackerOutputInLogEncoding(
                initialEncoding,
                targetEncoding,
                consoleOutput);
            DestinationString = DestinationString.Replace("\r", string.Empty);
            string[] data = DestinationString.Split(new Char[] { '\n' });// errors /r - remains
            for (int i = 0; i < data.Length; i++)
                data[i] = data[i].Trim();// Trim required because output from archiver is bad

            // if it is a low log level
            if (onlyErrors)
            {
                // we do output only warnings and errors
                // from 7-zip output
                if (!finishedSuccessfully)
                {
                    bool outputedSomething = false;
                    bool doOutputFlag = false;

                    for (int i = 0; i < data.Length; i++)
                    {
                        // we cannot log empty strings
                        if (!string.IsNullOrEmpty(data[i]))
                        {
                            if (data[i].StartsWith("WARNINGS for files:", StringComparison.CurrentCulture))
                                doOutputFlag = true;

                            if (doOutputFlag)
                            {
                                if ((data[i] != "----------------") && (data[i] != "WARNINGS for files:"))
                                {
                                    outputedSomething = true;
                                    log.Error(data[i]);
                                }
                            }
                        }
                    }

                    // if we didn't output anything - than we 
                    // possibly missed something or a non-standard
                    // message occured
                    if (!outputedSomething)
                        outputPackerMessageHelper(log, data, LoggingEvent.Error);
                }
            }
            else
            {
                // in all other log types we should output all
                // 7-zip output entirely
                if (finishedSuccessfully)
                    outputPackerMessageHelper(log, data, LoggingEvent.Info);
                else
                    outputPackerMessageHelper(log, data, LoggingEvent.Error);
            }
        }

        #region Private Methods

        private static string convertPackerOutputInLogEncoding(
            Encoding fromEncoding,
            Encoding targetEncoding,
            string packerOutput)
        {
            Decoder dec = fromEncoding.GetDecoder();
            byte[] ba = targetEncoding.GetBytes(packerOutput);
            int len = dec.GetCharCount(ba, 0, ba.Length);
            char[] ca = new char[len];
            dec.GetChars(ba, 0, ba.Length, ca, 0);
            return new string(ca);
        }

        private static void outputPackerMessageHelper(ILog log, string[] data, LoggingEvent putInLogAs)
        {
            for (int i = 0; i < data.Length; i++)
                if (!string.IsNullOrEmpty(data[i]))
                    log.Write(putInLogAs, data[i]);
        }

        #endregion
    }
}