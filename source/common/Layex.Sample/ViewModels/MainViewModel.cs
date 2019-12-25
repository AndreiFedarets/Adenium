using Layex.ViewModels;

namespace Layex.Sample.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IViewModelManager _viewModelManager;

        public MainViewModel(IViewModelManager viewModelManager)
        {
            _viewModelManager = viewModelManager;
            DisplayName = "Main";
        }

        public void OpenSampleItemsView()
        {
            _viewModelManager.Activate("Layex.Sample.LayoutedItems");
        }
    }
}
