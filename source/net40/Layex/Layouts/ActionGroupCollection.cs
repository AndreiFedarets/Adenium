namespace Layex.Layouts
{
    public sealed class ActionGroupCollection : ItemCollection<string, ActionGroup>
    {
        protected override string GetItemKey(ActionGroup item)
        {
            return item.Name;
        }

        protected override bool HandleAddExisting(ActionGroup existingItem, ActionGroup newItem)
        {
            if (newItem.Type != null)
            {
                existingItem.Type = newItem.Type;
                existingItem.Order = newItem.Order;
            }
            existingItem.Items.Add(newItem.Items);
            return true;
        }
    }
}
