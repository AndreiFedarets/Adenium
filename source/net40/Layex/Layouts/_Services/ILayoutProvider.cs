namespace Layex.Layouts
{
    public interface ILayoutProvider
    {
        Layout GetLayout(ViewModels.IViewModel viewModel);
    }
}
