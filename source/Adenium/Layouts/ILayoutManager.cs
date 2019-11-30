using Adenium.ViewModels;

namespace Adenium.Layouts
{
    public interface ILayoutManager
    {
        //TODO: replace parameter with string codeName
        Layout LoadLayout(LayoutedItemsViewModel itemsViewModel);
    }
}
