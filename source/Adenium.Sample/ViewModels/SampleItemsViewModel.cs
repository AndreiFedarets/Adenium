using Adenium.Layouts;
using Adenium.ViewModels;
using System.Collections.Generic;

namespace Adenium.Sample.ViewModels
{
    [ViewModel("Adenium.Sample.LayoutedItems")]
    public sealed class SampleItemsViewModel : LayoutedItemsViewModel
    {
        public SampleItemsViewModel()
        {
            DisplayName = "Layouted Items Sample";
            AvailableDisplayModes = new[] { DisplayMode.Grid, DisplayMode.Tab };
        }

        public IEnumerable<DisplayMode> AvailableDisplayModes { get; }
    }
}
