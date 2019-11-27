namespace Adenium.ViewModels
{
    [ViewModel("Application")]
    internal sealed class ApplicationViewModel : LayoutedItemsViewModel
    {
        public ApplicationViewModel(IDependencyContainer dependencyContainer)
        {
            SetupContainer(dependencyContainer);
        }

        public void Initialize()
        {
            OnInitialize();
        }
    }
}
