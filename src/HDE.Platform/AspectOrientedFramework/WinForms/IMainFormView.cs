using System.Windows.Forms;

namespace HDE.Platform.AspectOrientedFramework.WinForms
{
    public interface IMainFormView
    {
        TabControl TabControl { get; }
        MenuStrip MainMenu { get; }
        void SetController(object controller);
    }
}