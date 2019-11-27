namespace Adenium.Layouting
{
    public sealed class DefaultLayoutProvider : ILayoutProvider
    {
        public void ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
        }

        public string GetLayout(IViewModel targetViewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(targetViewModel);
        }
    }
}
