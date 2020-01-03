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

        public override Actions.ActionItem GetAction(IDependencyContainer dependencyContainer)
        {
            if (Type == null)
            {
                Type = typeof(Actions.RootActionGroup);
            }
            Actions.ActionGroup group = (Actions.ActionGroup)base.GetAction(dependencyContainer);
            foreach (ActionItem item in Items)
            {
                Actions.ActionItem actionItem = item.GetAction(dependencyContainer);
                group.Add(actionItem);
            }
            return group;
        }
    }
}
