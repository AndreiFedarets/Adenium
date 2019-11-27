using Adenium.Layouts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Adenium.ViewModels
{
    internal sealed class ViewModelItemCollection : ReadOnlyCollection<ViewModelItem>
    {
        public ViewModelItemCollection(IEnumerable<LayoutItem> layoutItems, IDependencyContainer container)
            : base(BuildViewModelItems(layoutItems, container))
        {
        }

        private static List<ViewModelItem> BuildViewModelItems(IEnumerable<LayoutItem> layoutItems, IDependencyContainer container)
        {
            List<ViewModelItem> viewModelItems = new List<ViewModelItem>();
            foreach (LayoutItem layoutItem in layoutItems)
            {
                viewModelItems.Add(new ViewModelItem(layoutItem, container));
            }
            return viewModelItems;
        }
    }
}
