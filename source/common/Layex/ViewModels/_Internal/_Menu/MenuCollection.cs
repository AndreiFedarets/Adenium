using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Layex.ViewModels
{
    internal sealed class MenuCollection : ReadOnlyCollection<Menu>, IMenuCollection, IDisposable
    {
        public MenuCollection()
            : base(new ObservableCollection<Menu>())
        {
        }

        public IMenu this[string id]
        {
            get { return Items.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.Ordinal)); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add {  ((INotifyCollectionChanged)Items).CollectionChanged += value; }
            remove { ((INotifyCollectionChanged)Items).CollectionChanged -= value; }
        }

        public void Add(Menu menu)
        {
            Menu existingMenu = (Menu)this[menu.Id];
            if (existingMenu != null)
            {
                existingMenu.Merge(menu);
            }
            else
            {
                Items.Add(menu);
            }
        }

        public void Dispose()
        {
            foreach (Menu control in Items)
            {
                control.Dispose();
            }
            Items.Clear();
        }

        internal void Initialize(IViewModel ownerViewModel)
        {
            foreach (Menu menu in Items)
            {
                menu.Initialize(ownerViewModel);
            }
        }

        IEnumerator<IMenu> IEnumerable<IMenu>.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
