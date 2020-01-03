using System;

namespace Layex.Layouts
{
    public sealed class Layout
    {
        public Layout()
        {
            Name = string.Empty;
            ActionItems = new ActionItemCollection();
            ViewModels = new ViewModelCollection();
        }

        public Type Type { get; set; }

        public string Name { get; set; }

        public ViewModelCollection ViewModels { get; set; }

        public ActionItemCollection ActionItems { get; set; }
        
        public void Append(Layout layout)
        {
            ViewModels.Add(layout.ViewModels);
            ActionItems.Add(layout.ActionItems);
        }
    }
}
