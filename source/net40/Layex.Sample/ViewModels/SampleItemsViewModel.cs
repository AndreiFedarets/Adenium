using Layex.Layouts;
using Layex.ViewModels;
using Layex.Views;
using System.Collections.Generic;

namespace Layex.Sample.ViewModels
{
    [ViewModel("Layex.Sample.LayoutedItems")]
    public sealed class SampleItemsViewModel : LayoutedItemsViewModel
    {
        public SampleItemsViewModel()
        {
            DisplayName = "Layouted Items Sample";
            DisplayModes = new[] { DisplayMode.Grid, DisplayMode.Tab };
        }

        public IEnumerable<DisplayMode> DisplayModes { get; }
    }
}
