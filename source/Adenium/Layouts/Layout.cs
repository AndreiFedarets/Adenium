using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Adenium.Layouts
{
    public class Layout : ReadOnlyCollection<LayoutItem>
    {
        public Layout(DisplayMode displayMode, List<LayoutItem> items)
            : base(items)
        {
            DisplayMode = displayMode;
        }

        public Layout()
            : this(DisplayMode.Tab, new List<LayoutItem>())
        {
        }

        public DisplayMode DisplayMode { get; set; }
    }
}
