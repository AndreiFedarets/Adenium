using System;

namespace Adenium.Layouts
{
    public sealed class LayoutItem
    {
        public LayoutItem(string typeFullName, ActivationMode activationMode, InstanceMode instanceMode, int order)
        {
            TypeFullName = typeFullName;
            ActivationMode = activationMode;
            InstanceMode = instanceMode;
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

        public ActivationMode ActivationMode { get; private set; }

        public InstanceMode InstanceMode { get; private set; }
    }
}
