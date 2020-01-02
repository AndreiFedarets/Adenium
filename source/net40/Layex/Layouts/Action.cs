using System;

namespace Layex.Layouts
{
    public abstract class Action
    {
        protected Action()
        {
            CollectionName = string.Empty;
        }

        public string Name { get; set; }

        public string CollectionName { get; set; }

        public int Order { get; set; }

        public Type Type { get; set; }

        internal Actions.ActionBase GetAction(IDependencyContainer dependencyContainer, ViewModels.IViewModel context)
        {
            Actions.ActionBase actionBase = (Actions.ActionBase)dependencyContainer.Resolve(Type);
            actionBase.Name = Name;
            actionBase.CollectionName = CollectionName;
            actionBase.Order = Order;
            actionBase.AssignContext(context);
            return actionBase;
        }
    }
}
