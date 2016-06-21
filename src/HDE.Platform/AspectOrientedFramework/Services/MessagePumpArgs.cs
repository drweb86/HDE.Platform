using System;

namespace HDE.Platform.AspectOrientedFramework.Services
{
    public class MessagePumpArgs: EventArgs
    {
        #region Properties

        public string To { get; private set; }
        public string Subject { get; private set; }
        public object[] Body { get; private set; }

        #endregion

        #region Constructors

        public MessagePumpArgs(string to,
                               string subject,
                               object[] body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }

        #endregion
    }
}