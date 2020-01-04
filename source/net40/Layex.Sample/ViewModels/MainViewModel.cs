using Layex.ViewModels;

namespace Layex.Sample.ViewModels
{
    [ViewModel("Layex.Sample.Main")]
    public class MainViewModel : ViewModel
    {
        private readonly IViewModelManager _viewModelManager;

        public MainViewModel(IViewModelManager viewModelManager)
        {
            _viewModelManager = viewModelManager;
            DisplayName = "Main";
        }

        public void OpenSampleItemsFull()
        {
            _viewModelManager.Activate("Layex.Sample.LayoutedItems.Full");
        }

        public void OpenSampleItemsMini()
        {
            _viewModelManager.Activate("Layex.Sample.LayoutedItems.Mini");
        }
    }
}
