using Layex.Actions;

namespace Layex.Sample.Actions
{
    internal sealed class MainGroup : IActionGroup
    {
        public string DisplayName
        {
            get { return "Main"; }
        }

        public bool Available
        {
            get { return true; }
        }
    }
}
