namespace Layex.ViewModels
{
    public abstract class ViewModelFactoryBase : IViewModelFactory
    {
        protected readonly Layouts.ViewModel LayoutItem;

        protected ViewModelFactoryBase(Layouts.ViewModel layoutItem)
        {
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

        public abstract IViewModel Create(IDependencyContainer container, bool forceCreate);

        public static IViewModelFactory CreateFactory(Layouts.ViewModel layoutItem)
        {
            if (layoutItem.Singleton)
            {
                return new SingleViewModelFactory(layoutItem);
            }
            return new MultiViewModelFactory(layoutItem);
        }
    }
}
