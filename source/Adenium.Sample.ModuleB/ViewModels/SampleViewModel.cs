using Adenium.ViewModels;

namespace Adenium.Sample.ModuleB.ViewModels
{
    [ViewModel("Adenium.Sample.ModuleB.SampleViewModel")]
    public sealed class SampleViewModel : ViewModel
    {
        public SampleViewModel()
        {
            DisplayName = "Sample B";
        }
    }
}
