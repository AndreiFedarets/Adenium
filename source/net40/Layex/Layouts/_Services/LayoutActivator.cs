using System;

namespace Layex.Layouts
{
    public static class LayoutActivator
    {
        public static Actions.ActionItem Activate(ActionItem item, IDependencyContainer container)
        {
            if (item is ActionCommand)
            {
                return Activate((ActionCommand)item, container);
            }
            if (item is ActionGroup)
            {
                return Activate((ActionGroup)item, container);
            }
            throw new NotSupportedException();
        }

        public static Actions.ActionCommand Activate(ActionCommand item, IDependencyContainer container)
        {
            Actions.ActionCommand command = (Actions.ActionCommand)container.Resolve(item.Type);
            command.Name = item.Name;
            command.Order = item.Order;
            return command;
        }

        public static Actions.ActionGroup Activate(ActionGroup item, IDependencyContainer container)
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
                Actions.ActionItem actionItem = Activate(childItem, container);
                group.Add(actionItem);
            }
            return group;
        }

        public static ViewModels.IViewModel Activate(ViewModel item, IDependencyContainer container)
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

        public static Contracts.IContract Activate(Contract item, IDependencyContainer container)
        {
            Contracts.IContract contract = (Contracts.IContract)container.Resolve(item.Type);
            return contract;
        }

        public static bool CanDisplayItem(ViewModel layoutItem, ViewModels.IItemsViewModel parentViewModel, IDependencyContainer container)
        {
            if (layoutItem.FilterType == null)
            {
                return true;
            }
            ILayoutItemFilter filter = (ILayoutItemFilter)container.Resolve(layoutItem.FilterType);
            return filter.CanDisplayItem(parentViewModel);
        }
    }
}
