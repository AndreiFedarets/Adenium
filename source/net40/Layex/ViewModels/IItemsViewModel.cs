using System;
using System.Collections.Generic;

namespace Layex.ViewModels
{
    public interface IItemsViewModel : IViewModel, IEnumerable<IViewModel>
    {
        event EventHandler ItemActivated;

        event EventHandler ItemDeactivated;

        bool ActivateItem(string viewModelName);

        bool DeactivateItem(string viewModelName, bool close = false);

        void ActivateItem(IViewModel viewModel);

        void DeactivateItem(IViewModel viewModel, bool close = false);

        bool ContainsItem(string viewModelName);

        void ResetItems();
    }
}
