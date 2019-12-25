using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Layex.Layouts
{
    public class Attachment : ReadOnlyCollection<LayoutItem>
    {
        public Attachment(List<LayoutItem> items)
            : base(items)
        {
        }
    }
}
