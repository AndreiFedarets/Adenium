using System;

namespace Layex.Layouts
{
    public static class LayoutActivator
    {
        public static Actions.ActionItem Activate(IDependencyContainer container, ActionItem item)
        {
            if (item is ActionCommand)
            {
                return Activate(container, (ActionCommand)item);
            }
            if (item is ActionGroup)
            {
                return Activate(container, (ActionGroup)item);
            }
            throw new NotSupportedException();
        }

        public static Actions.ActionCommand Activate(IDependencyContainer container, ActionCommand item)
        {
            Actions.ActionCommand command = (Actions.ActionCommand)container.Resolve(item.Type);
            command.Name = item.Name;
            command.Order = item.Order;
            return command;
        }

        public static Actions.ActionGroup Activate(IDependencyContainer container, ActionGroup item)
        {
            Type type = item.Type;
            Actions.ActionGroup group;
            if (type == null)
            {
                group = new Actions.ActionGroup();
            }
            else
            {
                group = (Actions.ActionGroup)container.Resolve(item.Type);
            }
            group.Name = item.Name;
            group.Order = item.Order;
            foreach (ActionItem childItem in item.Items)
            {
                Actions.ActionItem actionItem = Activate(container, childItem);
                group.Add(actionItem);
            }
            return group;
        }

        public static ViewModels.IViewModel Activate(IDependencyContainer container, ViewModel item)
        {
            ViewModels.IViewModel viewModel = (ViewModels.IViewModel)container.Resolve(item.Type);
            viewModel.Locked = item.Locked;
            ILayoutedItem layoutedItem = viewModel as ILayoutedItem;
            if (layoutedItem != null)
            {
                layoutedItem.Order = item.Order;
                layoutedItem.Name = item.Name;
            }
            return viewModel;
        }
    }
}
