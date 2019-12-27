namespace Layex.Actions
{
    public interface IActionGroup
    {
        string DisplayName { get; }

        bool Available { get; }
    }
}
