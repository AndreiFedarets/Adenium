using System;

namespace Layex.Layouts
{
    public abstract class ActionItem : ILayoutedItem
    {
        protected ActionItem()
        {
            Order = -1;
        }

        public string Name { get; set; }

        public int Order { get; set; }

        public Type Type { get; set; }
    }
}
