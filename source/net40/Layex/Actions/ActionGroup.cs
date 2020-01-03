using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Layex.ViewModels;

namespace Layex.Actions
{
    public abstract class ActionGroup : ActionItem, IEnumerable<ActionItem>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<ActionItem> _actionItems;

        public ActionGroup()
        {
            _actionItems = new ObservableCollection<ActionItem>();
        }

        public ActionItem this[string fullName]
        {
            get
            {
                if (string.IsNullOrEmpty(fullName))
                {
                    return this;
                }
                const char namePathSeparator = '.';
                int namePathIndex = fullName.IndexOf(namePathSeparator);
                if (namePathIndex < 0)
                {
                    return GetLocalItem(fullName);
                }
                string name = fullName.Substring(0, namePathIndex);
                string path = fullName.Substring(namePathIndex + 1);
                ActionGroup actionGroup = (ActionGroup)GetLocalItem(name);
                return actionGroup[path];
            }
        }

        private ActionItem GetLocalItem(string name)
        {
            return _actionItems.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _actionItems.CollectionChanged += value; }
            remove { _actionItems.CollectionChanged -= value; }
        }

        public void Add(ActionItem action)
        {
            _actionItems.Add(action);
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (ActionItem actionItem in _actionItems)
            {
                actionItem.Dispose();
            }
            _actionItems.Clear();
        }

        public override void AssignContext(IViewModel context)
        {
            foreach (ActionItem actionItem in _actionItems)
            {
                actionItem.AssignContext(context);
            }
            base.AssignContext(context);
        }

        public IEnumerator<ActionItem> GetEnumerator()
        {
            return _actionItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
