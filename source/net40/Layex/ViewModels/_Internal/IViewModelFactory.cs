namespace Layex.ViewModels
{
    internal interface IViewModelFactory
    {
        bool AutoActivate { get; }

        string Name { get; }

        IViewModel Create();
    }
}
