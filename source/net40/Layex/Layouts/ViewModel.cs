using Layex.Extensions;
using System;

namespace Layex.Layouts
{
    public sealed class ViewModel : IOrderedtem
    {
        private ViewModels.IViewModel _viewModel;
        
        public Type Type { get; set; }

        public int Order { get; set; }

        public bool AutoActivate { get; set; }

        public bool Singleton { get; set; }

        public bool Locked { get; set; }

        public string ViewModelName
        {
            get { return ViewModelExtensions.GetViewModelName(Type); }
        }

        internal ViewModels.IViewModel GetViewModel(IDependencyContainer container)
        {
            ViewModels.IViewModel viewModel;
            if (Singleton)
            {
                if (_viewModel == null)
                {
                    _viewModel = CreateViewModel(container);
                    _viewModel.Disposed += OnViewModelDisposed;
                }
                viewModel = _viewModel;
            }
            else
            {
                viewModel = CreateViewModel(container);
            }
            return viewModel;
        }

        private void OnViewModelDisposed(object sender, EventArgs e)
        {
            _viewModel.Disposed -= OnViewModelDisposed;
            _viewModel = null;
        }

        private ViewModels.IViewModel CreateViewModel(IDependencyContainer container)
        {
            ViewModels.IViewModel viewModel = (ViewModels.IViewModel)container.Resolve(Type);
            ViewModels.ViewModel viewModelItem = viewModel as ViewModels.ViewModel;
            if (viewModelItem != null)
            {
                viewModelItem.Order = Order;
                viewModelItem.Locked = Locked;
            }
            return viewModel;
        }
    }
}
