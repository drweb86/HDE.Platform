//Taken from MSDN article by Stephen Cleary https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
using System;
using System.Windows.Input;

namespace HDE.Platform.Wpf.Commands
{
    public sealed class DelegateCommand : ICommand
    {
        private readonly Action _command;

        public DelegateCommand(Action command)
        {
            _command = command;
        }

        public void Execute(object parameter)
        {
            _command();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}