namespace Layex.ViewModels
{
    public class ViewModelManager : IViewModelManager
    {
        private readonly IDependencyContainer _container;
        private readonly ApplicationViewModel _applicationViewModel;

        public ViewModelManager(ApplicationViewModel applicationViewModel, IDependencyContainer container)
        {
            _applicationViewModel = applicationViewModel;
            _container = container;
        }

        public IViewModel Activate(params string[] names)
        {
            IViewModel currentViewModel = _applicationViewModel;
            foreach (string viewModelName in names)
            {
                ItemsViewModel itemsViewModel = currentViewModel as ItemsViewModel;
                if (itemsViewModel == null)
                {
                    return null;
                }
                currentViewModel = TryActivate(itemsViewModel, viewModelName);
            }
            return currentViewModel;
        }

        public IViewModel Activate(string viewModelName)
        {
            return _applicationViewModel.ActivateItem(viewModelName);
        }

        public IViewModel Activate<T>(string viewModelName, T param)
        {
            return _applicationViewModel.ActivateItem<T>(viewModelName, param);
        }

        public IViewModel Activate<T1, T2>(string viewModelName, T1 param1, T2 param2)
        {
            return _applicationViewModel.ActivateItem<T1, T2>(viewModelName, param1, param2);
        }

        public IViewModel Activate<T1, T2, T3>(string viewModelName, T1 param1, T2 param2, T3 param3)
        {
            return _applicationViewModel.ActivateItem<T1, T2, T3>(viewModelName, param1, param2, param3);
        }

        private IViewModel TryActivate(ItemsViewModel itemsViewModel, string viewModelName)
        {
            return itemsViewModel.ActivateItem(viewModelName);
        }

        public TViewModel Create<TViewModel>() where TViewModel : IViewModel
        {
            IDependencyContainer childContainer = _container.CreateChildContainer();
            return childContainer.Resolve<TViewModel>();
        }

        public TViewModel Create<TViewModel, T>(T param) where TViewModel : IViewModel
        {
            IDependencyContainer childContainer = _container.CreateChildContainer();
            childContainer.RegisterInstance<T>(param);
            return childContainer.Resolve<TViewModel>();
        }

        public TViewModel Create<TViewModel, T1, T2>(T1 param1, T2 param2) where TViewModel : IViewModel
        {
            IDependencyContainer childContainer = _container.CreateChildContainer();
            childContainer.RegisterInstance<T1>(param1);
            childContainer.RegisterInstance<T2>(param2);
            return childContainer.Resolve<TViewModel>();
        }

        public TViewModel Create<TViewModel, T1, T2, T3>(T1 param1, T2 param2, T3 param3) where TViewModel : IViewModel
        {
            IDependencyContainer childContainer = _container.CreateChildContainer();
            childContainer.RegisterInstance<T1>(param1);
            childContainer.RegisterInstance<T2>(param2);
            childContainer.RegisterInstance<T3>(param3);
            return childContainer.Resolve<TViewModel>();
        }

        public TViewModel Activate<TViewModel>() where TViewModel : IViewModel
        {
            return _applicationViewModel.ActivateItem<TViewModel>();
        }

        public TViewModel Activate<TViewModel, T>(T param) where TViewModel : IViewModel
        {
            return _applicationViewModel.ActivateItem<TViewModel, T>(param);
        }

        public TViewModel Activate<TViewModel, T1, T2>(T1 param1, T2 param2) where TViewModel : IViewModel
        {
            return _applicationViewModel.ActivateItem<TViewModel, T1, T2>(param1, param2);
        }

        public TViewModel Activate<TViewModel, T1, T2, T3>(T1 param1, T2 param2, T3 param3) where TViewModel : IViewModel
        {
            return _applicationViewModel.ActivateItem<TViewModel, T1, T2, T3>(param1, param2, param3);
        }
    }
}
