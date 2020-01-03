using System;
using System.Collections;
using System.Collections.Generic;

namespace Layex.Layouts
{
    public class ItemCollection<TItem> : ICollection<TItem>, ICollection where TItem : ILayoutedItem
    {
        protected readonly Dictionary<string, TItem> Items;

        public ItemCollection()
        {
            Items = new Dictionary<string, TItem>();
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

        public TItem this[string name]
        {
            get { return Items[name]; }
        }
        
        public void Add(TItem item)
        {
            TItem existingItem;
            if (Items.TryGetValue(item.Name, out existingItem))
            {
                if (!HandleAddExisting(existingItem, item))
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                Items.Add(item.Name, item);
            }
        }

        public void Add(ItemCollection<TItem> collection)
        {
            foreach (TItem item in collection)
            {
                Add(item);
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
            return Items.ContainsKey(item.Name);
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
            List<TItem> orderedItems = new List<TItem>(Items.Values);
            orderedItems.Sort(CompareOrdered);
            return orderedItems.GetEnumerator();
        }

        private int CompareOrdered(TItem item1, TItem item2)
        {
            return item1.Order.CompareTo(item2.Order);
        }

        public bool Remove(TItem item)
        {
            return Items.Remove(item.Name);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
