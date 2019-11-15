using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace packmule.ViewModels
{
    class DelegateCommand : ICommand
    {
        private readonly Action<object> executeAction;
        private readonly Func<object, bool> canExecuteAction;

        public DelegateCommand(Action<object> _executeAction, Func<object, bool> _canExecuteAction)
        {
            executeAction = _executeAction;
            canExecuteAction = _canExecuteAction;
        }

        public void Execute(object parameter) => executeAction(parameter);

        public bool CanExecute(object parameter) => canExecuteAction?.Invoke(parameter) ?? true;

        public event EventHandler CanExecuteChanged;

        public void InvokeCanExecuteChanged() => CanExecuteChanged.Invoke(this, EventArgs.Empty);
    }
}
