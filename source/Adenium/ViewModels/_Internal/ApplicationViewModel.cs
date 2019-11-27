namespace Adenium.ViewModels
{
    internal sealed class ApplicationViewModel : LayoutedItemsViewModel
    {
        public ApplicationViewModel(IDependencyContainer dependencyContainer)
        {
            InitializeContainer(dependencyContainer);
        }

        public void Initialize()
        {
            OnInitialize();
        }
    }
}
