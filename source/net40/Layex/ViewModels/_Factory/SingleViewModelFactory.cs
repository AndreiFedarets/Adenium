namespace Layex.ViewModels
{
    public sealed class SingleViewModelFactory : ViewModelFactoryBase
    {
        private IViewModel _viewModel;

        public SingleViewModelFactory(Layouts.ViewModel layoutItem)
            : base(layoutItem)
        {
        }

        public override IViewModel Create(IDependencyContainer container)
        {
            return CreateInternal(container);
        }

        public override IViewModel Create<T>(IDependencyContainer container, T param)
        {
            ForceClose();
            container.RegisterInstance<T>(param);
            return CreateInternal(container);
        }

        public override IViewModel Create<T1, T2>(IDependencyContainer container, T1 param1, T2 param2)
        {
            ForceClose();
            container.RegisterInstance<T1>(param1);
            container.RegisterInstance<T2>(param2);
            return CreateInternal(container);
        }

        public override IViewModel Create<T1, T2, T3>(IDependencyContainer container, T1 param1, T2 param2, T3 param3)
        {
            ForceClose();
            container.RegisterInstance<T1>(param1);
            container.RegisterInstance<T2>(param2);
            container.RegisterInstance<T3>(param3);
            return CreateInternal(container);
        }

        private void ForceClose()
        {
            if (_viewModel != null)
            {
                _viewModel.Close();
            }
        }

        private IViewModel CreateInternal(IDependencyContainer container)
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
