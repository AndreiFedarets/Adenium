namespace Layex.ViewModels
{
    public interface IViewModelManager
    {
        IViewModel Activate(params string[] names);

        IViewModel Activate(string viewModelName);

        IViewModel Activate<T>(string viewModelName, T param);

        IViewModel Activate<T1, T2>(string viewModelName, T1 param1, T2 param2);

        IViewModel Activate<T1, T2, T3>(string viewModelName, T1 param1, T2 param2, T3 param3);

        TViewModel Create<TViewModel>() where TViewModel : IViewModel;

        TViewModel Create<TViewModel, T>(T param) where TViewModel : IViewModel;

        TViewModel Create<TViewModel, T1, T2>(T1 param1, T2 param2) where TViewModel : IViewModel;

        TViewModel Create<TViewModel, T1, T2, T3>(T1 param1, T2 param2, T3 param3) where TViewModel : IViewModel;

        TViewModel Activate<TViewModel>() where TViewModel : IViewModel;

        TViewModel Activate<TViewModel, T>(T param) where TViewModel : IViewModel;

        TViewModel Activate<TViewModel, T1, T2>(T1 param1, T2 param2) where TViewModel : IViewModel;

        TViewModel Activate<TViewModel, T1, T2, T3>(T1 param1, T2 param2, T3 param3) where TViewModel : IViewModel;

        //void ShowWindow(IViewModel viewModel);

        //bool? ShowDialog(IViewModel viewModel);

        //void ShowPopup(IViewModel viewModel);

        //IViewModel CreateViewModel(Type viewModelType);

        //IViewModel CreateViewModel(Type viewModelType, IContainerViewModel parentViewModel);

        //IViewModel CreateViewModel<T1>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1);

        //IViewModel CreateViewModel<T1, T2>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2);

        //IViewModel CreateViewModel<T1, T2, T3>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2, T3 dependency3);

        //void BuildViewModelLayout(IViewModel viewModel);

        //void ResetViewModelLayout(IViewModel viewModel);

        //IViewModel ActivateItem(IContainerViewModel containerViewModel, string childViewModelId);

        //IViewModel ActivateItem<T1>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1);

        //IViewModel ActivateItem<T1, T2>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2);

        //IViewModel ActivateItem<T1, T2, T3>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2, T3 dependency3);
    }

    //public static class ViewModelManagerExtensions
    //{
    //    // CreateViewModel
    //    public static TViewModel CreateViewModel<TViewModel>(this IViewModelManager manager) where TViewModel : IViewModel
    //    {
    //        return (TViewModel)manager.CreateViewModel<object, object, object>(typeof(TViewModel), null, null, null, null);
    //    }

    //    public static TViewModel CreateViewModel<TViewModel>(this IViewModelManager manager, IContainerViewModel parentViewModel) where TViewModel : IViewModel
    //    {
    //        return (TViewModel)manager.CreateViewModel(typeof(TViewModel), parentViewModel);
    //    }

    //    public static TViewModel CreateViewModel<TViewModel, T1>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1) where TViewModel : IViewModel
    //    {
    //        return (TViewModel)manager.CreateViewModel(typeof(TViewModel), parentViewModel, dependency1);
    //    }

    //    public static TViewModel CreateViewModel<TViewModel, T1, T2>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2) where TViewModel : IViewModel
    //    {
    //        return (TViewModel)manager.CreateViewModel(typeof(TViewModel), parentViewModel, dependency1, dependency2);
    //    }

    //    public static TViewModel CreateViewModel<TViewModel, T1, T2, T3>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2, T3 dependency3) where TViewModel : IViewModel
    //    {
    //        return (TViewModel)manager.CreateViewModel(typeof(TViewModel), parentViewModel, dependency1, dependency2, dependency3);
    //    }

    //    //ShowDialog
    //    public static bool? ShowDialog<TViewModel>(this IViewModelManager manager) where TViewModel : IViewModel
    //    {
    //        TViewModel viewModel = manager.CreateViewModel<TViewModel>();
    //        return manager.ShowDialog(viewModel);
    //    }

    //    public static bool? ShowDialog<TViewModel>(this IViewModelManager manager, IContainerViewModel parentViewModel) where TViewModel : IViewModel
    //    {
    //        TViewModel viewModel = manager.CreateViewModel<TViewModel>(parentViewModel);
    //        return manager.ShowDialog(viewModel);
    //    }

    //    public static bool? ShowDialog<TViewModel, T1>(this IViewModelManager manager, IContainerViewModel parentViewModel,  T1 dependency1) where TViewModel : IViewModel
    //    {
    //        TViewModel viewModel = manager.CreateViewModel<TViewModel, T1>(parentViewModel, dependency1);
    //        return manager.ShowDialog(viewModel);
    //    }

    //    public static bool? ShowDialog<TViewModel, T1, T2>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2) where TViewModel : IViewModel
    //    {
    //        TViewModel viewModel = manager.CreateViewModel<TViewModel, T1, T2>(parentViewModel, dependency1, dependency2);
    //        return manager.ShowDialog(viewModel);
    //    }

    //    public static bool? ShowDialog<TViewModel, T1, T2, T3>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2, T3 dependency3) where TViewModel : IViewModel
    //    {
    //        TViewModel viewModel = manager.CreateViewModel<TViewModel, T1, T2, T3>(parentViewModel, dependency1, dependency2, dependency3);
    //        return manager.ShowDialog(viewModel);
    //    }
    //}
}
