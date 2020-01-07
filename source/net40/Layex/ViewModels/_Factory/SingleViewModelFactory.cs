using System;

namespace Layex.ViewModels
{
    public sealed class SingleViewModelFactory : ViewModelFactoryBase
    {
        private IViewModel _viewModel;

        public SingleViewModelFactory(Layouts.ViewModel layoutItem, IDependencyContainer container)
            : base(layoutItem, container)
        {
        }

        public override IViewModel Create()
        {
            return Create(Container);
        }

        public override IViewModel Create<T>(T param)
        {
            ForceClose();
            IDependencyContainer childContainer = Container.CreateChildContainer();
            childContainer.RegisterInstance<T>(param);
            return Create(childContainer);
        }

        public override IViewModel Create<T1, T2>(T1 param1, T2 param2)
        {
            ForceClose();
            IDependencyContainer childContainer = Container.CreateChildContainer();
            childContainer.RegisterInstance<T1>(param1);
            childContainer.RegisterInstance<T2>(param2);
            return Create(childContainer);
        }

        public override IViewModel Create<T1, T2, T3>(T1 param1, T2 param2, T3 param3)
        {
            ForceClose();
            IDependencyContainer childContainer = Container.CreateChildContainer();
            childContainer.RegisterInstance<T1>(param1);
            childContainer.RegisterInstance<T2>(param2);
            childContainer.RegisterInstance<T3>(param3);
            return Create(childContainer);
        }

        private void ForceClose()
        {
            if (_viewModel != null)
            {
                _viewModel.Close();
            }
        }

        private IViewModel Create(IDependencyContainer container)
        {
            if (_viewModel == null)
            {
                _viewModel = Layouts.LayoutActivator.Activate(LayoutItem, container);
                _viewModel.Deactivated += OnViewModelDeactivated;
            }
            return _viewModel;
        }

        private void OnViewModelDeactivated(object sender, Caliburn.Micro.DeactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                _viewModel.Deactivated -= OnViewModelDeactivated;
                _viewModel = null;
            }
        }
    }
}
