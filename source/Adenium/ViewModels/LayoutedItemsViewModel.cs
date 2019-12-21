using Adenium.Extensions;
using Adenium.Layouts;
using System.Collections.Generic;
using System.Linq;

namespace Adenium.ViewModels
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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivateStartupViewModels();
        }

        protected override void Configure()
        {
            base.Configure();
            ILayoutManager layoutManager = DependencyContainer.Resolve<ILayoutManager>();
            Layout layout = layoutManager.LoadLayout(this.GetCodeName());
            DisplayMode = layout.DisplayMode;
            _viewModelItems = new ViewModelItemCollection(layout, DependencyContainer);
        }

        private void ActivateStartupViewModels()
        {
            IEnumerable<ViewModelItem> startupViewModelItems = _viewModelItems.Where(x => x.ActivationMode == ActivationMode.OnStartup);
            foreach (ViewModelItem viewModelItem in startupViewModelItems)
            {
                ActivateItem(viewModelItem);
            }
            ActiveItem = Items.FirstOrDefault();
        }

        private void ActivateItem(ViewModelItem viewModelItem)
        {
            IViewModel viewModel = viewModelItem.GetViewModel();
            ActivateItem(viewModel);
        }
    }
}
