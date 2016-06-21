using System;

namespace HDE.Platform.Logging
{
	/// <summary>
	/// Logs output to console. Works with dos output from archiver
	/// </summary>
	public sealed class ConsoleLog: LogBase
	{
        protected override void WriteInternal(LoggingEvent loggingEvent, string message)
        {
            ConsoleColor previousColor = Console.ForegroundColor;

            switch (loggingEvent)
            {
                case LoggingEvent.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LoggingEvent.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LoggingEvent.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LoggingEvent.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine(message);

            Console.ForegroundColor = previousColor;
        }

	    protected override void OpenInternal() { }
        protected override void CloseInternal() { }
	}
}
