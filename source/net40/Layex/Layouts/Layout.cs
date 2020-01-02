using System;
using System.Collections.Generic;

namespace Layex.Layouts
{
    public sealed class Layout
    {
        public Layout()
        {
            ViewModelName = string.Empty;
            ViewModels = new List<ViewModel>();
            Actions = new List<Action>();
            Contracts = new List<Contract>();
        }

        public List<ViewModel> ViewModels { get; set; }

        public List<Action> Actions { get; set; }

        public List<Contract> Contracts { get; set; }

        public Type Type { get; set; }

        public string ViewModelName { get; set; }
    }
}
