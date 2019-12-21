using System;
using Adenium.ViewModels;

namespace Adenium.Sample.ViewModels
{
    public class MainViewModel : LayoutedItemsViewModel
    {
        private readonly IViewModelManager _viewModelManager;

        public MainViewModel(IViewModelManager viewModelManager)
        {
            _viewModelManager = viewModelManager;
            DisplayName = "Main View";
        }
        
        public void OpenGridView()
        {
            ActivateItem("Adenium.Sample.Grid");
        }

        public void OpenTabView()
        {
            ActivateItem("Adenium.Sample.Tab");
        }
    }
}
