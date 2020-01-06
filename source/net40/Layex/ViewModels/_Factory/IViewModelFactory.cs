namespace Layex.ViewModels
{
    public interface IViewModelFactory
    {
        bool AutoActivate { get; }

        string Name { get; }

        IViewModel Create();

        IViewModel Create<T>(T param);

        IViewModel Create<T1, T2>(T1 param1, T2 param2);

        IViewModel Create<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
    }
}
