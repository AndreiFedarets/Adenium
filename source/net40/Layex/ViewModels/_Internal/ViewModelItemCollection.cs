using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Layex.ViewModels
{
    internal sealed class ViewModelItemCollection : ReadOnlyCollection<ViewModelItem>
    {
        public ViewModelItemCollection(Layouts.Layout layout, IDependencyContainer container)
            : base(BuildViewModelItems(layout, container))
        {
        }

        public ViewModelItem FindByName(string viewModelName)
        {
            foreach (ViewModelItem item in Items)
            {
                if (string.Equals(item.CodeName, viewModelName, StringComparison.Ordinal))
                {
                    return item;
                }
            }
            return null;
        }

        private static List<ViewModelItem> BuildViewModelItems(Layouts.Layout layout, IDependencyContainer container)
        {
            List<ViewModelItem> viewModelItems = new List<ViewModelItem>();
            foreach (Layouts.Item layoutItem in layout.Items)
            {
                viewModelItems.Add(new ViewModelItem(layoutItem, container));
            }
            return viewModelItems;
        }
    }
}
