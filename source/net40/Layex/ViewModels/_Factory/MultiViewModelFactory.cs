namespace Layex.ViewModels
{
    public sealed class MultiViewModelFactory : ViewModelFactoryBase
    {
        public MultiViewModelFactory(Layouts.ViewModel layoutItem, IDependencyContainer container)
            : base(layoutItem, container)
        {
        }

        public override IViewModel Create()
        {
            return Layouts.LayoutActivator.Activate(LayoutItem, Container);
        }

        public override IViewModel Create<T>(T param)
        {
            IDependencyContainer childContainer = Container.CreateChildContainer();
            childContainer.RegisterInstance<T>(param);
            return Layouts.LayoutActivator.Activate(LayoutItem, childContainer);
        }

        public override IViewModel Create<T1, T2>(T1 param1, T2 param2)
        {
            IDependencyContainer childContainer = Container.CreateChildContainer();
            childContainer.RegisterInstance<T1>(param1);
            childContainer.RegisterInstance<T2>(param2);
            return Layouts.LayoutActivator.Activate(LayoutItem, childContainer);
        }

        public override IViewModel Create<T1, T2, T3>(T1 param1, T2 param2, T3 param3)
        {
            IDependencyContainer childContainer = Container.CreateChildContainer();
            childContainer.RegisterInstance<T1>(param1);
            childContainer.RegisterInstance<T2>(param2);
            childContainer.RegisterInstance<T3>(param3);
            return Layouts.LayoutActivator.Activate(LayoutItem, childContainer);
        }
    }
}
