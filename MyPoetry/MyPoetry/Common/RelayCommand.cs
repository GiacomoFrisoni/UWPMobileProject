using System;

namespace MyPoetry.Common
{
    sealed class RelayCommand : System.Windows.Input.ICommand
    {
        readonly Func<bool> _canExecute;
        readonly Action _execute;

        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public bool CanExecute() { return this.CanExecute(null); }
        public bool CanExecute(object parameter) { return _canExecute == null ? true : _canExecute(); }
        public void Execute() { this.Execute(null); }
        public void Execute(object parameter) { _execute(); }
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    sealed class RelayCommand<TParameter> : System.Windows.Input.ICommand
    {
        readonly Func<TParameter, bool> _canExecute;
        readonly Action<TParameter> _execute;

        public RelayCommand(Action<TParameter> execute) : this(execute, null) { }
        public RelayCommand(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) { return _canExecute == null ? true : _canExecute((TParameter)parameter); }
        public void Execute(object parameter) { _execute((TParameter)parameter); }
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
