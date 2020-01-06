using System;
using System.Collections.Generic;

namespace Layex.ViewModels
{
    public interface IItemsViewModel : IViewModel, IEnumerable<IViewModel>
    {
        event EventHandler<ViewModelEventArgs> ItemActivated;

        event EventHandler<ViewModelEventArgs> ItemDeactivated;

        bool ContainsItem(string viewModelName);

        IViewModel GetItem(string viewModel);

        bool ActivateItem(string viewModelName);

        bool ActivateItem<T>(string viewModelName, T param);

        bool ActivateItem<T1, T2>(string viewModelName, T1 param1, T2 param2);

        bool ActivateItem<T1, T2, T3>(string viewModelName, T1 param1, T2 param2, T3 param3);

        void ActivateItem(IViewModel viewModel);

        bool DeactivateItem(string viewModelName, bool close = false);

        void DeactivateItem(IViewModel viewModel, bool close = false);

        void ResetItems();
    }
}
