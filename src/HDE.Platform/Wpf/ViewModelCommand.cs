using System;
using System.Windows.Input;

namespace HDE.Platform.Wpf
{
    public abstract class ViewModelCommand<TViewModel> : ICommand
    {
        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
                return false;

            return OnCanExecute((TViewModel)parameter);
        }

        public void Execute(object parameter)
        {
            OnExecute((TViewModel)parameter);
        }

        public void OnCanExecuteChanged()
        {
            var currentCanExecuteChanged = CanExecuteChanged;
            currentCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        protected abstract void OnExecute(TViewModel model);
        protected abstract bool OnCanExecute(TViewModel model);
    }
}