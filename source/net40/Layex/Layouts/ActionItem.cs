using System;

namespace Layex.Layouts
{
    public abstract class ActionItem : IOrderedtem
    {
        public ActionItem()
        {
            Order = -1;
        }

        public string Name { get; set; }

        public int Order { get; set; }

        public Type Type { get; set; }

        public virtual Actions.ActionItem GetAction(IDependencyContainer dependencyContainer)
        {
            Actions.ActionItem item = (Actions.ActionItem)dependencyContainer.Resolve(Type);
            item.Name = Name;
            item.Order = Order;
            return item;
        }
    }
}
