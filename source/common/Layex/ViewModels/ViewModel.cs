using System;
using Caliburn.Micro;

namespace Layex.ViewModels
{
    public abstract class ViewModel : Screen, IViewModel
    {
        private bool _isStatic;

        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }

        public bool IsStatic
        {
            get { return _isStatic; }
            set
            {
                _isStatic = value;
                NotifyOfPropertyChange(() => IsStatic);
            }
        }

        public event EventHandler Disposed;

        public virtual void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
