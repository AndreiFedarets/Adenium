using Adenium.Layouts;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Adenium.ViewModels
{
    public interface IItemsViewModel : IViewModel, IEnumerable<IViewModel>, INotifyCollectionChanged
    {
        IViewModel ActiveItem { get; set; }

        DisplayMode DisplayMode { get; }

        void ActivateItem(IViewModel viewModel);

        void DeactivateItem(IViewModel viewModel, bool close);

        //void DeactivateItems();

        //T FindFirstChild<T>(Func<T, bool> condition = null) where T : IViewModel;
    }

    //public static class ContainerViewModelExtensions
    //{
    //    public static IViewModel ActivateItem(this IContainerViewModel containerViewModel, string childViewModelId)
    //    {
    //        return ViewModelManager.Instance.ActivateItem(containerViewModel, childViewModelId);
    //    }

    //    public static IViewModel ActivateItem<T1>(this IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1)
    //    {
    //        return ViewModelManager.Instance.ActivateItem(containerViewModel, childViewModelId, dependency1);
    //    }

    //    public static IViewModel ActivateItem<T1, T2>(this IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2)
    //    {
    //        return ViewModelManager.Instance.ActivateItem(containerViewModel, childViewModelId, dependency1, dependency2);
    //    }

    //    public static IViewModel ActivateItem<T1, T2, T3>(this IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2, T3 dependency3)
    //    {
    //        return ViewModelManager.Instance.ActivateItem(containerViewModel, childViewModelId, dependency1, dependency2, dependency3);
    //    }
    //}
}
