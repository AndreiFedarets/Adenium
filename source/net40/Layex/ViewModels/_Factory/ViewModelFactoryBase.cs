using Layex.Layouts;

namespace Layex.ViewModels
{
    public abstract class ViewModelFactoryBase : IViewModelFactory
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

        public abstract IViewModel Create<T>(T param);

        public abstract IViewModel Create<T1, T2>(T1 param1, T2 param2);

        public abstract IViewModel Create<T1, T2, T3>(T1 param1, T2 param2, T3 param3);

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
