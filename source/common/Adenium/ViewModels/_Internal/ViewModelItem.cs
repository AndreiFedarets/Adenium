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

        public string CodeName
        {
            get
            {
                Type viewModelType = _layoutItem.Type;
                string codeName = string.Empty;
                if (viewModelType != null)
                {
                    ViewModelAttribute attribute = ViewModelAttribute.GetAttribute(viewModelType);
                    if (attribute != null)
                    {
                        codeName = attribute.CodeName;
                    }
                }
                return codeName;
            }
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
                        _viewModel.Disposed += OnSingleViewModelDisposed;
                    }
                    return _viewModel;
                case InstanceMode.Multiple:
                    return CreateViewModel();
                default:
                    throw new NotImplementedException();
            }
        }

        private void OnSingleViewModelDisposed(object sender, EventArgs e)
        {
            _viewModel.Disposed -= OnSingleViewModelDisposed;
            _viewModel = null;
        }

        private IViewModel CreateViewModel()
        {
            Type viewModelType = _layoutItem.Type;
            IViewModel viewModel = (IViewModel)_container.Resolve(viewModelType);
            return viewModel;
        }
    }
}
