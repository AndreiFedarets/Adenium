using System;

namespace Layex.ViewModels
{
    internal class ViewModelItem
    {
        private readonly IDependencyContainer _container;
        private readonly Layouts.Item _layoutItem;
        private IViewModel _viewModel;

        public ViewModelItem(Layouts.Item layoutItem, IDependencyContainer container)
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

        public bool Closable
        {
            get { return _layoutItem.Closable; }
        }

        public Layouts.ActivationMode ActivationMode
        {
            get { return _layoutItem.ActivationMode; }
        }

        public IViewModel GetViewModel()
        {
            IViewModel viewModel;
            switch (_layoutItem.InstanceMode)
            {
                case Layouts.InstanceMode.Single:
                    if (_viewModel == null)
                    {
                        _viewModel = CreateViewModel();
                        _viewModel.Disposed += OnSingleViewModelDisposed;
                    }
                    viewModel = _viewModel;
                    break;
                case Layouts.InstanceMode.Multiple:
                    viewModel = CreateViewModel();
                    break;
                default:
                    throw new NotImplementedException();
            }
            //if (viewModel != null && _layoutItem.IsStatic.HasValue)
            //{
            //    viewModel.IsStatic = _layoutItem.IsStatic.Value;
            //}
            return viewModel;
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
