using System;
using System.Windows.Forms;
using HDE.Platform.Collections;
using HDE.Platform.Logging;

namespace HDE.Platform.AspectOrientedFramework.WinForms
{
    public interface ITool : IDisposable
    {
        void Assign(ILog log, string toolName, TabControl tabControl, MenuStrip mainMenu, ReadOnlyDictionary<object, object> commonServices);

        void Activate();

        void ApplyChange(string subject, params object[] body);
    }
}
