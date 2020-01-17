using Layex.Extensions;
using System;

namespace Layex.Layouts
{
    public sealed class ViewModel : ILayoutedItem
    {
        private string _name;
        
        public Type Type { get; set; }

        public Type FilterType { get; set; }

        public int Order { get; set; }

        public bool AutoActivate { get; set; }

        public bool Singleton { get; set; }

        public bool Locked { get; set; }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    return ViewModels.ViewModelExtensions.GetViewModelDefaultName(Type);
                }
                return _name;
            }
            set { _name = value; }
        }
    }
}
