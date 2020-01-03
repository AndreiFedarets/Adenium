using System;
using System.Collections;
using System.Collections.Generic;

namespace Layex.Layouts
{
    public abstract class ItemCollection<TKey, TItem> : ICollection<TItem>, ICollection
    {
        protected readonly Dictionary<TKey, TItem> Items;

        public ItemCollection()
        {
            Items = new Dictionary<TKey, TItem>();
        }

        public bool IsOrdered
        {
            get { return typeof(IOrderedtem).IsAssignableFrom(typeof(TItem)); }
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public TItem this[TKey key]
        {
            get { return Items[key]; }
        }

        protected abstract TKey GetItemKey(TItem item);

        public void Add(TItem item)
        {
            AddInternal(item);
        }

        public void Add(ItemCollection<TKey, TItem> collection)
        {
            foreach (TItem item in collection)
            {
                AddInternal(item);
            }
        }

        protected void AddInternal(TItem item)
        {
            TKey itemKey = GetItemKey(item);
            if (Items.ContainsKey(itemKey))
            {
                TItem existingItem = this[itemKey];
                if (!HandleAddExisting(existingItem, item))
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                Items.Add(itemKey, item);
            }
        }

        protected virtual bool HandleAddExisting(TItem existingItem, TItem newItem)
        {
            return false;
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(TItem item)
        {
            TKey itemKey = GetItemKey(item);
            return Items.ContainsKey(itemKey);
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            IEnumerable<TItem> items = Items.Values;
            if (IsOrdered)
            {
                List<TItem> orderedItems = new List<TItem>(items);
                orderedItems.Sort(CompareOrdered);
                items = orderedItems;
            }
            return items.GetEnumerator();
        }

        private int CompareOrdered(TItem item1, TItem item2)
        {
            return ((IOrderedtem)item1).Order.CompareTo(((IOrderedtem)item2).Order);
        }

        public bool Remove(TItem item)
        {
            TKey itemKey = GetItemKey(item);
            return Items.Remove(itemKey);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
