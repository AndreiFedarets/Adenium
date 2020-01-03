using Layex.Extensions;

namespace Layex.Layouts
{
    public sealed class ViewModelCollection : ItemCollection<string, ViewModel>
    {
        protected override string GetItemKey(ViewModel item)
        {
            return ViewModelExtensions.GetViewModelName(item.Type);
        }
    }
}
