namespace Layex.Layouts
{
    public interface ILayoutManager
    {
        Layout LoadLayout(string viewModelCodeName);
    }
}
