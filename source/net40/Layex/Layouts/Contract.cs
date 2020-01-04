using System;

namespace Layex.Layouts
{
    public sealed class Contract : ILayoutedItem
    {
        private string _name;

        public Type Type { get; set; }

        public int Order { get; set; }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    return Type.FullName;
                }
                return _name;
            }
            set { _name = value; }
        }
    }
}
