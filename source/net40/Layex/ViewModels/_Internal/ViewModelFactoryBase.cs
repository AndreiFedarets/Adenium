namespace Layex.ViewModels
{
    internal abstract class ViewModelFactoryBase : IViewModelFactory
    {
        protected readonly Layouts.ViewModel LayoutItem;
        protected readonly IDependencyContainer Container;

        protected ViewModelFactoryBase(Layouts.ViewModel layoutItem, IDependencyContainer container)
        {
            LayoutItem = layoutItem;
            Container = container;
        }

        public bool AutoActivate
        {
            get { return LayoutItem.AutoActivate; }
        }

        public string Name
        {
            get { return LayoutItem.Name; }
        }

        public abstract IViewModel Create();

        public static IViewModelFactory CreateFactory(Layouts.ViewModel layoutItem, IDependencyContainer container)
        {
            if (layoutItem.Singleton)
            {
                return new SingleViewModelFactory(layoutItem, container);
            }
            return new MultiViewModelFactory(layoutItem, container);
        }
    }
}
