using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Layex.ViewModels
{
    internal sealed class MenuControlCollection : ReadOnlyCollection<IMenuControl>, IMenuControlCollection
    {
        public MenuControlCollection()
            : base (new ObservableCollection<IMenuControl>())
        {
        }

        public IMenuControl this[string id]
        {
            get { return Items.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.Ordinal)); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { ((INotifyCollectionChanged)Items).CollectionChanged += value; }
            remove { ((INotifyCollectionChanged)Items).CollectionChanged -= value; }
        }

        internal void Add(MenuControl control)
        {
            MenuControl existingControl = (MenuControl)this[control.Id];
            if (existingControl != null)
            {
                existingControl.Merge(control);
            }
            else
            {
                Items.Add(control);
            }
        }

        public void Invalidate()
        {
            foreach (IMenuControl control in Items)
            {
                control.Invalidate();
            }
        }

        public void Dispose()
        {
            foreach (IMenuControl control in Items)
            {
                control.Dispose();
            }
            Items.Clear();
        }

        //internal override void Initialize(IViewModel ownerViewModel)
        //{
        //    base.Initialize(ownerViewModel);
        //    foreach (MenuControl control in _collection)
        //    {
        //        control.Initialize(ownerViewModel);
        //    }
        //}

        //internal override void Merge(MenuControl control)
        //{
        //    base.Merge(control);
        //    MenuControlCollection controlCollection = control as MenuControlCollection;
        //    if (controlCollection != null)
        //    {
        //        foreach (MenuControl childControl in controlCollection.Cast<MenuControl>())
        //        {
        //            Add(childControl);
        //        }
        //    }
        //}
    }
}
