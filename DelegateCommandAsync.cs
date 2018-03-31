using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSharpLab5
{
    public class DelegateCommandAsync : DelegateCommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
 
        public DelegateCommandAsync(Func<object, Task> executeAsync)
            : this(executeAsync, _ => true )
        {
        }

        public DelegateCommandAsync(Func<object, Task> executeAsync, Predicate<object> canExecute)
            :  base( async _ => { await executeAsync(_); }, canExecute)
        {
        }
    }
}

