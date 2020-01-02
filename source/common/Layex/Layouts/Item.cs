using System;

namespace Layex.Layouts
{
    public sealed class Item
    {
        public Item()
        {
            ActivationMode = ActivationMode.OnDemand;
            InstanceMode = InstanceMode.Multiple;
            Closable = true;
        }

        public Type Type { get; set; }

        public int Order { get; set; }

        public ActivationMode ActivationMode { get; set; }

        public InstanceMode InstanceMode { get; set; }

        public bool Closable { get; set; }
    }
}
