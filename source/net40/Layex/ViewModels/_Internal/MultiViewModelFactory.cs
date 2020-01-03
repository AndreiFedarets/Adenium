namespace Layex.ViewModels
{
    internal sealed class MultiViewModelFactory : ViewModelFactoryBase
    {
        public MultiViewModelFactory(IDependencyContainer container, Layouts.ViewModel layoutItem)
            : base(container, layoutItem)
        {
        }

        public override IViewModel Create()
        {
            return Layouts.LayoutActivator.Activate(Container, LayoutItem);
        }
    }
}
