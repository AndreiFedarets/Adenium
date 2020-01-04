namespace Layex.ViewModels
{
    internal sealed class MultiViewModelFactory : ViewModelFactoryBase
    {
        public MultiViewModelFactory(Layouts.ViewModel layoutItem, IDependencyContainer container)
            : base(layoutItem, container)
        {
        }

        public override IViewModel Create()
        {
            return Layouts.LayoutActivator.Activate(LayoutItem, Container);
        }
    }
}
