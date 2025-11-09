using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WiredBrainCoffee.CustomersApp.Command
{
    public class GenericDelegateCommand<T> : ICommand where T : class
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public GenericDelegateCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute((T?)parameter);


        public void Execute(object? parameter) => _execute((T?)parameter);

    }
}
