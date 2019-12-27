using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layex.Layouts
{
    public sealed class ActionItem
    {
        public ActionItem(string typeFullName, int order)
        {
            TypeFullName = typeFullName;
            Order = order;
        }

        public string TypeFullName { get; private set; }

        public Type Type
        {
            get
            {
                Type type = Type.GetType(TypeFullName);
                if (type == null)
                {
                    throw new Exception($"The system cannot find type '{TypeFullName}'");
                }
                return type;
            }
        }

        public int Order { get; private set; }
    }
}
