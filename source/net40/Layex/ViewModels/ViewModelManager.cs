namespace Layex.ViewModels
{
    public class ViewModelManager : IViewModelManager
    {
        private readonly ApplicationViewModel _applicationViewModel;

        public ViewModelManager(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
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
    }
}
