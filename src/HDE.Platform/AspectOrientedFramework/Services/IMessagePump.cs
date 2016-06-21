using System;

namespace HDE.Platform.AspectOrientedFramework.Services
{
    public interface IMessagePump
    {
        void SendMessage(string to, string subject, params object[] body);
        event EventHandler<MessagePumpArgs> OnMessageReceived;
    }
}