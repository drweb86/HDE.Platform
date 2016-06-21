using System;

namespace HDE.Platform.AspectOrientedFramework.Services
{
    public class MessagePump : IMessagePump
    {
        public void SendMessage(string to, string subject, params object[] body)
        {
            if (OnMessageReceived != null)
            {
                OnMessageReceived(null, new MessagePumpArgs(to, subject, body));
            }
        }

        public event EventHandler<MessagePumpArgs> OnMessageReceived;
    }
}