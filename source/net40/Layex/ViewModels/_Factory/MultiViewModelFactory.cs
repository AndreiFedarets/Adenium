namespace Layex.ViewModels
{
    public sealed class MultiViewModelFactory : ViewModelFactoryBase
    {
        public MultiViewModelFactory(Layouts.ViewModel layoutItem)
            : base(layoutItem)
        {
        }

        public override IViewModel Create(IDependencyContainer container, bool forceCreate)
        {
            return Layouts.LayoutActivator.Activate(LayoutItem, container);
        }
    }
}
