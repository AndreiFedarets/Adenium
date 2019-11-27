using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Adenium.Layouts
{
    public class Attachment : ReadOnlyCollection<LayoutItem>
    {
        public Attachment(List<LayoutItem> items)
            : base(items)
        {
        }
    }
}
