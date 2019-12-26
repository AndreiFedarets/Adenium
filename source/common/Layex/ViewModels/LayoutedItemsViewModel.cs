using Layex.Extensions;
using Layex.Layouts;
using System.Collections.Generic;
using System.Linq;

namespace Layex.ViewModels
{
    public abstract class LayoutedItemsViewModel : ItemsViewModel
    {
        private ViewModelItemCollection _viewModelItems;

        public override bool ActivateItem(string childCodeName)
        {
            if (base.ActivateItem(childCodeName))
            {
                return true;
            }
            ViewModelItem viewModelItem = _viewModelItems.FindByName(childCodeName);
            if (viewModelItem == null)
            {
                return false;
            }
            ActivateItem(viewModelItem);
            return true;
        }

        public void ResetItems()
        {
            IViewModel activeItem = ActiveItem;
            IEnumerable<ViewModelItem> startupViewModelItems = _viewModelItems.Where(x => x.ActivationMode == ActivationMode.OnStartup);
            foreach (ViewModelItem viewModelItem in startupViewModelItems)
            {
                ActivateItem(viewModelItem);
            }
            if (activeItem == null && Items.Any())
            {
                activeItem = Items.First();
            }
            if (activeItem != null)
            {
                ActiveItem = activeItem;
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ResetItems();
        }

        protected override void Configure()
        {
            base.Configure();
            ILayoutManager layoutManager = DependencyContainer.Resolve<ILayoutManager>();
            Layout layout = layoutManager.LoadLayout(this.GetCodeName());
            DisplayMode = layout.DisplayMode;
            _viewModelItems = new ViewModelItemCollection(layout, DependencyContainer);
        }

        private void ActivateItem(ViewModelItem viewModelItem)
        {
            IViewModel viewModel = viewModelItem.GetViewModel();
            ActivateItem(viewModel);
        }
    }
}
