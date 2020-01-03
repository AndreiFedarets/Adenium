using System;
using System.Windows.Input;

namespace Layex.Actions
{
    public abstract class ActionCommand : ActionItem, ICommand
    {
        public event EventHandler CanExecuteChanged;

        public override bool Available
        {
            get { return base.Available; }
            protected set
            {
                base.Available = value;
                NotifyOfCanExecuteChanged();
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return Available;
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
