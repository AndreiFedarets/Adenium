using Layex.Extensions;
using Layex.ViewModels;
using System;

namespace Layex.Actions
{
    public abstract class CloseViewModelAction : ActionCommand
    {
        private readonly string _viewModelName;

        protected CloseViewModelAction(Type viewModelType)
            : this(ViewModelExtensions.GetViewModelDefaultName(viewModelType))
        {
        }

        protected CloseViewModelAction(string viewModelName)
        {
            _viewModelName = viewModelName;
        }

        public override bool CanExecute(object parameter)
        {
            IItemsViewModel itemsViewModel = Context as IItemsViewModel;
            if (itemsViewModel != null)
            {
                return itemsViewModel.ContainsItem(_viewModelName);
            }
            return false;
        }

        public override void Execute(object parameter)
        {
            IItemsViewModel itemsViewModel = Context as IItemsViewModel;
            if (itemsViewModel != null)
            {
                itemsViewModel.DeactivateItem(_viewModelName, true);
            }
        }

        protected override void OnContextAssigned()
        {
            base.OnContextAssigned();
            IItemsViewModel itemsViewModel = Context as IItemsViewModel;
            if (itemsViewModel != null)
            {
                itemsViewModel.ItemActivated += OnItemsChanged;
                itemsViewModel.ItemDeactivated += OnItemsChanged;
            }
        }

        private void OnItemsChanged(object sender, EventArgs e)
        {
            NotifyOfCanExecuteChanged();
        }

        public override void Dispose()
        {
            IItemsViewModel itemsViewModel = Context as IItemsViewModel;
            if (itemsViewModel != null)
            {
                itemsViewModel.ItemActivated -= OnItemsChanged;
                itemsViewModel.ItemDeactivated -= OnItemsChanged;
            }
        }
    }
}
