using Adenium.Layouts;
using System;

namespace Adenium.ViewModels
{
    internal class ViewModelItem
    {
        private readonly IDependencyContainer _container;
        private readonly LayoutItem _layoutItem;
        private IViewModel _viewModel;

        public ViewModelItem(LayoutItem layoutItem, IDependencyContainer container)
        {
            _layoutItem = layoutItem;
            _container = container;
        }

        public ActivationMode ActivationMode
        {
            get { return _layoutItem.ActivationMode; }
        }

        public IViewModel GetViewModel()
        {
            switch (_layoutItem.InstanceMode)
            {
                case InstanceMode.Single:
                    if (_viewModel == null)
                    {
                        _viewModel = CreateViewModel();
                    }
                    return _viewModel;
                case InstanceMode.Multiple:
                    return CreateViewModel();
                default:
                    throw new NotImplementedException();
            }
        }

        private IViewModel CreateViewModel()
        {
            Type viewModelType = _layoutItem.Type;
            IViewModel viewModel = (IViewModel)_container.Resolve(viewModelType);
            return viewModel;
        }
    }
}
