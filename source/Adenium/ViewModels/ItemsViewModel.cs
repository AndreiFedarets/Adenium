using Adenium.Contracts;
using Adenium.Layouts;
using Caliburn.Micro;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Adenium.ViewModels
{
    public abstract class ItemsViewModel : Conductor<IViewModel>.Collection.OneActive, IItemsViewModel
    {
        private readonly ContractCollection _contracts;

        public ItemsViewModel(DisplayMode displayMode)
        {
            DisplayMode = displayMode;
            _contracts = new ContractCollection(this);
        }

        public ItemsViewModel()
            : this(DisplayMode.Tab)
        {
        }

        public virtual DisplayMode DisplayMode { get; private set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { Items.CollectionChanged += value; }
            remove { Items.CollectionChanged -= value; }
        }

        public override void ActivateItem(IViewModel item)
        {
            base.ActivateItem(item);
            _contracts.RegisterItem(item);
        }

        public override void DeactivateItem(IViewModel item, bool close)
        {
            if (close)
            {
                _contracts.UnregisterItem(item);
            }
            base.DeactivateItem(item, close);
        }

        public virtual void Dispose()
        {
            _contracts.Dispose();
        }

        public IEnumerator<IViewModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
