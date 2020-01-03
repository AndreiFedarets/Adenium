using System;

namespace Layex.Layouts
{
    public sealed class Layout
    {
        public Layout()
        {
            ViewModelName = string.Empty;
            ActionGroups = new  ActionGroupCollection();
            ViewModels = new ViewModelCollection();
        }

        public Type Type { get; set; }

        public string ViewModelName { get; set; }

        public ViewModelCollection ViewModels { get; set; }

        public ActionGroupCollection ActionGroups { get; set; }
        
        public void Append(Layout layout)
        {
            ViewModels.Add(layout.ViewModels);
            ActionGroups.Add(layout.ActionGroups);
        }
    }
}
