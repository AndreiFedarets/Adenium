using System.Windows.Markup;

namespace Layex.Layouts
{
    [ContentProperty("Items")]
    public sealed class ActionGroup : ActionItem
    {
        public ActionGroup()
        {
            Items = new ActionItemCollection();
        }

        public ActionItemCollection Items { get; private set; }
    }
}
