using System;

namespace Layex.Actions
{
    public abstract class ActionItem : Caliburn.Micro.PropertyChangedBase, IDisposable
    {
        private bool _available;
        private string _name;

        protected ActionItem()
        {
            _available = true;
        }

        public string Name { get; internal protected set; }

        public int Order { get; internal protected set; }

        public virtual string DisplayName
        {
            get { return GetType().Name; }
        }

        public virtual bool Available
        {
            get { return _available; }
            protected set
            {
                _available = value;
                NotifyOfPropertyChange(() => Available);
            }
        }

        public ViewModels.IViewModel Context { get; private set; }

        public virtual void Dispose()
        {

        }

        public virtual void AssignContext(ViewModels.IViewModel context)
        {
            Context = context;
            OnContextAssigned();
            NotifyOfPropertyChange(() => Context);
        }

        protected virtual void OnContextAssigned()
        {
            
        }
    }
}
