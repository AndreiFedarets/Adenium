using System;

namespace Layex.ViewModels
{
    internal sealed class SingleViewModelFactory : ViewModelFactoryBase
    {
        private IViewModel _viewModel;

        public SingleViewModelFactory(IDependencyContainer container, Layouts.ViewModel layoutItem)
            : base(container, layoutItem)
        {
        }

        public override IViewModel Create()
        {
            if (_viewModel == null)
            {
                _viewModel = Layouts.LayoutActivator.Activate(Container, LayoutItem);
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
