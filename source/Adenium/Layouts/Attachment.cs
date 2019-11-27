using System.Collections.Generic;

namespace Adenium.Layouts
{
    public class Attachment
    {
        public Attachment(List<LayoutItem> items)
        {
            Items = items;
        }

        public List<LayoutItem> Items { get; private set; }
    }
}
