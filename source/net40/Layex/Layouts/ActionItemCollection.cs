using System;

namespace Layex.Layouts
{
    public sealed class ActionItemCollection : ItemCollection<string, ActionItem>
    {
        protected override string GetItemKey(ActionItem item)
        {
            return item.Name;
        }

        protected override bool HandleAddExisting(ActionItem existingItem, ActionItem newItem)
        {
            if (existingItem.GetType() != newItem.GetType() || existingItem is ActionCommand)
            {
                return false;
            }
            ActionGroup existingGroup = (ActionGroup)existingItem;
            ActionGroup newGroup = (ActionGroup)newItem;
            if (newGroup.Type != null)
            {
                existingGroup.Type = newGroup.Type;
                existingGroup.Order = newGroup.Order;
            }
            existingGroup.Items.Add(newGroup.Items);
            return true;
        }
    }
}
