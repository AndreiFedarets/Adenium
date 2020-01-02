using System;

namespace Layex.Actions
{
    public abstract class ActionBase : Caliburn.Micro.PropertyChangedBase, IDisposable
    {
        private bool _available;

        protected ActionBase()
        {
            _available = true;
        }

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

        internal string Name { get; set; }

        internal string CollectionName { get; set; }

        internal int Order { get; set; }

        public virtual void Dispose()
        {

        }

        internal void AssignContext(ViewModels.IViewModel context)
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
