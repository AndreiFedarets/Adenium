namespace Layex.ViewModels
{
    public sealed class MultiViewModelFactory : ViewModelFactoryBase
    {
        public MultiViewModelFactory(Layouts.ViewModel layoutItem)
            : base(layoutItem)
        {
        }

        public override IViewModel Create(IDependencyContainer container)
        {
            return Layouts.LayoutActivator.Activate(LayoutItem, container);
        }

        public override IViewModel Create<T>(IDependencyContainer container, T param)
        {
            container.RegisterInstance<T>(param);
            return Layouts.LayoutActivator.Activate(LayoutItem, container);
        }

        public override IViewModel Create<T1, T2>(IDependencyContainer container, T1 param1, T2 param2)
        {
            container.RegisterInstance<T1>(param1);
            container.RegisterInstance<T2>(param2);
            return Layouts.LayoutActivator.Activate(LayoutItem, container);
        }

        public override IViewModel Create<T1, T2, T3>(IDependencyContainer container, T1 param1, T2 param2, T3 param3)
        {
            container.RegisterInstance<T1>(param1);
            container.RegisterInstance<T2>(param2);
            container.RegisterInstance<T3>(param3);
            return Layouts.LayoutActivator.Activate(LayoutItem, container);
        }
    }
}
