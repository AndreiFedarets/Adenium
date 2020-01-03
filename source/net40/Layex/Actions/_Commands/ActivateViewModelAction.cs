using Layex.Extensions;
using Layex.ViewModels;
using System;

namespace Layex.Actions
{
    public abstract class ActivateViewModelAction : ActionCommand
    {
        private readonly string _viewModelName;

        protected ActivateViewModelAction(Type viewModelType)
            :this(ViewModelExtensions.GetViewModelDefaultName(viewModelType))
        {
        }

        protected ActivateViewModelAction(string viewModelName)
        {
            _viewModelName = viewModelName;
        }

        public override void Execute(object parameter)
        {
            IItemsViewModel itemsViewModel = Context as IItemsViewModel;
            if (itemsViewModel != null)
            {
                itemsViewModel.ActivateItem(_viewModelName);
            }
        }
    }
}
