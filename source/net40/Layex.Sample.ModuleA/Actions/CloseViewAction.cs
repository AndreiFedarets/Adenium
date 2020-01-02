using Layex.Actions;
using Layex.Extensions;
using Layex.ViewModels;

namespace Layex.Sample.ModuleA.Actions
{
    internal sealed class CloseViewAction : IAction
    {
        public string DisplayName
        {
            get { return "Close View A"; }
        }

        public bool Available
        {
            get { return true; }
        }

        public IViewModel Context { get; set; }

        public void Execute()
        {
            IItemsViewModel itemsViewModel = Context as IItemsViewModel;
            if (itemsViewModel != null)
            {
                string codeName = ViewModelExtensions.GetCodeName<ViewModels.SampleViewModel>();
                itemsViewModel.DeactivateItem(codeName, true);
            }
        }
    }
}
