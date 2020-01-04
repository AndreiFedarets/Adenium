using System;

namespace Layex.ViewModels
{
    internal sealed class SingleViewModelFactory : ViewModelFactoryBase
    {
        private IViewModel _viewModel;

        public SingleViewModelFactory(Layouts.ViewModel layoutItem, IDependencyContainer container)
            : base(layoutItem, container)
        {
        }

        public override IViewModel Create()
        {
            if (_viewModel == null)
            {
                _viewModel = Layouts.LayoutActivator.Activate(LayoutItem, Container);
                _viewModel.Disposed += OnViewModelDisposed;
            }
            return _viewModel;
        }

        private void OnViewModelDisposed(object sender, EventArgs e)
        {
            _viewModel.Disposed -= OnViewModelDisposed;
            _viewModel = null;
        }
    }
}
