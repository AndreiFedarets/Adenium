using Layex.ViewModels;

namespace Layex.Actions
{
    public interface IAction
    {
        string DisplayName { get; }

        bool Available { get; }

        IViewModel Context { get; set; }

        void Execute();
    }
}
