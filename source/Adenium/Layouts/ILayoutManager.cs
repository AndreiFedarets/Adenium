using Adenium.ViewModels;

namespace Adenium.Layouts
{
    public interface ILayoutManager
    {
        Layout LoadLayout(IItemsViewModel itemsViewModel);
    }
}
