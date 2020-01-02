using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Layex.Actions
{
    public abstract class ActionCollectionBase : ActionBase, IEnumerable<ActionBase>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<ActionBase> _actions;

        public ActionCollectionBase()
        {
            _actions = new ObservableCollection<ActionBase>();
        }

        public virtual ActionBase this[string name]
        {
            get { return _actions.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal)); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _actions.CollectionChanged += value; }
            remove { _actions.CollectionChanged -= value; }
        }

        public IEnumerator<ActionBase> GetEnumerator()
        {
            return _actions.GetEnumerator();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (ActionBase action in _actions)
            {
                action.Dispose();
            }
            _actions.Clear();
        }

        internal void Add(ActionBase action)
        {
            _actions.Add(action);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
