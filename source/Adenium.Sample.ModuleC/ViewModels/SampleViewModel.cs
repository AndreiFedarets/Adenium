using Adenium.ViewModels;

namespace Adenium.Sample.ModuleC.ViewModels
{
    [ViewModel("Adenium.Sample.ModuleC.SampleViewModel")]
    public sealed class SampleViewModel : ViewModel
    {
        public SampleViewModel()
        {
            DisplayName = "Sample C";
        }
    }
}
