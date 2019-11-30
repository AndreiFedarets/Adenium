using Caliburn.Micro;

namespace Adenium.ViewModels
{
    [ViewModel("Application")]
    internal sealed class ApplicationViewModel : LayoutedItemsViewModel
    {
        private readonly IWindowManager _windowManager;

        public ApplicationViewModel(IWindowManager windowManager, IDependencyContainer dependencyContainer)
        {
            _windowManager = windowManager;
            ((IRequireDependencyContainer)this).Configure(dependencyContainer);
        }

        public void Initialize()
        {
            OnInitialize();
        }

        public override void ActivateItem(IViewModel item)
        {
            item.Deactivated += OnItemDeactivated;
            base.ActivateItem(item);
            _windowManager.ShowWindow(item);
        }

        private void OnItemDeactivated(object sender, DeactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                IViewModel viewModel = (IViewModel)sender;
                viewModel.Deactivated -= OnItemDeactivated;
                Items.Remove(viewModel);
            }
        }
    }
}
