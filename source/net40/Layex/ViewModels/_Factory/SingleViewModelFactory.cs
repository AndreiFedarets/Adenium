namespace Layex.ViewModels
{
    public sealed class SingleViewModelFactory : ViewModelFactoryBase
    {
        private IViewModel _viewModel;

        public SingleViewModelFactory(Layouts.ViewModel layoutItem)
            : base(layoutItem)
        {
        }

        public override IViewModel Create(IDependencyContainer container, bool forceCreate)
        {
            if (forceCreate)
            {
                ForceClose();
            }
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
