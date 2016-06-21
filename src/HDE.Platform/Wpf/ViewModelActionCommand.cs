using System;

namespace HDE.Platform.Wpf
{
    public class ViewModelActionCommand<TViewModel> : ViewModelCommand<TViewModel>
    {
        #region Constructors

        public ViewModelActionCommand(Action<TViewModel> execute, Predicate<TViewModel> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public ViewModelActionCommand(Action<TViewModel> execute): this(execute, vm=>true)
        {
        }

        #endregion

        #region ICommand Members

        protected override void OnExecute(TViewModel model)
        {
            _execute(model);
        }

        protected override bool OnCanExecute(TViewModel model)
        {
            return _canExecute(model);
        }

        #endregion

        private readonly Action<TViewModel> _execute;
        private readonly Predicate<TViewModel> _canExecute;
    }
}
