namespace Layex.ViewModels
{
    internal abstract class ViewModelFactoryBase : IViewModelFactory
    {
        protected readonly Layouts.ViewModel LayoutItem;
        protected readonly IDependencyContainer Container;

        protected ViewModelFactoryBase(IDependencyContainer container, Layouts.ViewModel layoutItem)
        {
            Container = container;
            LayoutItem = layoutItem;
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

        public static IViewModelFactory CreateFactory(IDependencyContainer container, Layouts.ViewModel layoutItem)
        {
            if (layoutItem.Singleton)
            {
                return new SingleViewModelFactory(container, layoutItem);
            }
            return new MultiViewModelFactory(container, layoutItem);
        }
    }
}
