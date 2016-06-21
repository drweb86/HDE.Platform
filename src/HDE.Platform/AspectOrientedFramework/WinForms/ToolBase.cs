using System;
using System.Windows.Forms;
using HDE.Platform.AspectOrientedFramework.Services;
using HDE.Platform.Collections;
using HDE.Platform.Logging;

namespace HDE.Platform.AspectOrientedFramework.WinForms
{
    public class ToolBase : ITool
    {
        #region Properties

        protected TabControl TabControl { get; private set; }
        protected MenuStrip MainMenu { get; private set; }
        protected ILog Log { get; private set; }
        protected ReadOnlyDictionary<object, object> CommonServices { get; private set; }
        protected string ToolName { get; private set; }

        #endregion

        #region Fields

        private IMessagePump _messagePump;

        #endregion

        public virtual void Assign(ILog log, string toolName, TabControl tabControl, MenuStrip mainMenu, ReadOnlyDictionary<object, object> commonServices)
        {
            Log = log;
            ToolName = toolName;
            TabControl = tabControl;
            MainMenu = mainMenu;
            CommonServices = commonServices;

            if (CommonServices.ContainsKey(typeof(IMessagePump)))
            {
                _messagePump = (IMessagePump)CommonServices[typeof(IMessagePump)];
                _messagePump.OnMessageReceived += OnMessageReceived;
            }
            else
            {
                log.Info("IMessagePump is missing. Inter-tool communication feature will be unavailable.");
            }
        }

        public virtual void Activate()
        {
        }

        public virtual void Dispose()
        {
            if (_messagePump != null)
            {
                _messagePump.OnMessageReceived -= OnMessageReceived;
            }
        }

        public virtual void ApplyChange(string subject, params object[] body)
        {
            ;
        }

        #region Messaging

        protected void SendMessage(string to, string subject, params object[] args)
        {
            if (_messagePump != null)
            {
                _messagePump.SendMessage(to, subject, args);
            }
        }

        protected virtual void OnMessageReceived(object sender, MessagePumpArgs e)
        {
            if (string.Compare(e.To, ToolName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                OnMessageProcess(e.Subject, e.Body);
            }
        }

        protected virtual void OnMessageProcess(string subject, params object[] body)
        {
            ;
        }

        #endregion
    }
}