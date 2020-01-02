using System;
using System.Collections.Generic;

namespace Layex.Layouts
{
    public class Layout
    {
        public Layout()
        {
            ViewModelCode = string.Empty;
            Items = new List<Item>();
        }

        public List<Item> Items { get; set; }

        public Type Type { get; set; }

        public string ViewModelCode { get; set; }
    }
}
