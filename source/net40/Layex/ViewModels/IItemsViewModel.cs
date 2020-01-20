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

        IViewModel ActivateItem(string viewModelName);

        IViewModel ActivateItem<T>(string viewModelName, T param);

        IViewModel ActivateItem<T1, T2>(string viewModelName, T1 param1, T2 param2);

        IViewModel ActivateItem<T1, T2, T3>(string viewModelName, T1 param1, T2 param2, T3 param3);

        TViewModel ActivateItem<TViewModel>() where TViewModel : IViewModel;

        TViewModel ActivateItem<TViewModel, T>(T param) where TViewModel : IViewModel;

        TViewModel ActivateItem<TViewModel, T1, T2>(T1 param1, T2 param2) where TViewModel : IViewModel;

        TViewModel ActivateItem<TViewModel, T1, T2, T3>(T1 param1, T2 param2, T3 param3) where TViewModel : IViewModel;

        void ActivateItem(IViewModel viewModel);

        bool DeactivateItem(string viewModelName, bool close = false);

        void DeactivateItem(IViewModel viewModel, bool close = false);

        void ResetItems();
    }
}
