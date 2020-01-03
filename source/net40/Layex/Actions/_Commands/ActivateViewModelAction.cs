using Layex.ViewModels;

namespace Layex.Actions
{
    public abstract class ActivateViewModelAction : ActionCommand
    {
        private readonly string _viewModelName;

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
