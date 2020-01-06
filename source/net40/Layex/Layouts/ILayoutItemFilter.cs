namespace Layex.Layouts
{
    public interface ILayoutItemFilter
    {
        bool CanDisplayItem(ViewModels.IItemsViewModel viewModel);
    }
}
