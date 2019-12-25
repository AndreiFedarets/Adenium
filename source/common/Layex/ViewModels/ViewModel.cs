using System;
using Caliburn.Micro;

namespace Layex.ViewModels
{
    public abstract class ViewModel : Screen, IViewModel
    {
        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }

        public event EventHandler Disposed;

        public virtual void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

    }
}
