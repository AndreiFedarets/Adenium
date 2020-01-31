namespace Layex.ViewModels
{
    public interface IViewModelFactory
    {
        bool AutoActivate { get; }

        string Name { get; }

        IViewModel Create(IDependencyContainer container, bool forceCreate);
    }
}
