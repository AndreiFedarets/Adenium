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

        public override void DeactivateItem(IViewModel item, bool close = false)
        {
            if (!Items.Contains(item))
            {
                return;
            }
            if (close)
            {
                string itemCodeName = ViewModel.GetCodeName(item.GetType());
                ViewModelItem viewModelItem = _viewModelItems.FindByName(itemCodeName);
                if (viewModelItem != null && !viewModelItem.Closable)
                {
                    return;
                }
            }
            base.DeactivateItem(item, close);
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
            Layout layout = layoutManager.GetLayout(this.GetCodeName());
            _viewModelItems = new ViewModelItemCollection(layout, DependencyContainer);
        }

        private void ActivateItem(ViewModelItem viewModelItem)
        {
            IViewModel viewModel = viewModelItem.GetViewModel();
            ActivateItem(viewModel);
        }
    }
}
