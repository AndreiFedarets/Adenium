using Layex.Actions;
using Layex.ViewModels;

namespace Layex.Sample.Actions
{
    public sealed class ResetItemsAction : ActionCommandBase
    {
        public override string DisplayName
        {
            get { return "Reset Items"; }
        }

        public override void Execute(object parameter)
        {
            IItemsViewModel viewModel = Context as IItemsViewModel;
            if (viewModel != null)
            {
                viewModel.ResetItems();
            }
        }
    }
}
