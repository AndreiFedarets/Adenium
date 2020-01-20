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

        public abstract IViewModel Create(IDependencyContainer container);

        public abstract IViewModel Create<T>(IDependencyContainer container, T param);

        public abstract IViewModel Create<T1, T2>(IDependencyContainer container, T1 param1, T2 param2);

        public abstract IViewModel Create<T1, T2, T3>(IDependencyContainer container, T1 param1, T2 param2, T3 param3);

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
