//Taken from MSDN article by Stephen Cleary https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
using System.Threading.Tasks;
using System.Windows.Input;

namespace HDE.Platform.Wpf.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}