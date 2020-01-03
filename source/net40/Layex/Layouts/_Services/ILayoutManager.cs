namespace Layex.Layouts
{
    public interface ILayoutManager
    {
        Layout GetLayout(string viewModelName);
    }
}
