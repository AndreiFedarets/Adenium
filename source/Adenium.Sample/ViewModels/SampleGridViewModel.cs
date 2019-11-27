using Adenium.ViewModels;

namespace Adenium.Sample.ViewModels
{
    [ViewModel("Adenium.Sample.Grid")]
    public sealed class SampleGridViewModel : ItemsViewModel
    {
        public SampleGridViewModel()
        {
            DisplayName = "Grid Sample";
        }
    }
}
