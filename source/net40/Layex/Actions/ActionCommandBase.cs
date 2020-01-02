using System;
using System.Windows.Input;

namespace Layex.Actions
{
    public abstract class ActionCommandBase : ActionBase, ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {

        }

        protected override void OnContextAssigned()
        {
            base.OnContextAssigned();
            NotifyOfCanExecuteChanged();
        }

        protected void NotifyOfCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
