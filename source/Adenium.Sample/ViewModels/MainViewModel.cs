using Adenium.ViewModels;
using System.Windows.Input;

namespace Adenium.Sample.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IViewModelManager _viewModelManager;

        public MainViewModel(IViewModelManager viewModelManager)
        {
            _viewModelManager = viewModelManager;
            DisplayName = "Main View";
            OpenGridViewCommand = new SyncCommand(OpenGridView);
            OpenTabViewCommand = new SyncCommand(OpenTabView);
        }

        public ICommand OpenGridViewCommand { get; private set; }

        public ICommand OpenTabViewCommand { get; private set; }

        private void OpenGridView()
        {
            _viewModelManager.ShowDialog<SampleGridViewModel>();
        }

        private void OpenTabView()
        {
            _viewModelManager.ShowDialog<SampleTabViewModel>();
        }
    }
}
