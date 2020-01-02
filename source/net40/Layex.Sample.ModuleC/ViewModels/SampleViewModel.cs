using Layex.ViewModels;

namespace Layex.Sample.ModuleC.ViewModels
{
    [ViewModel("Layex.Sample.ModuleC.SampleViewModel")]
    public sealed class SampleViewModel : ViewModel
    {
        public SampleViewModel()
        {
            DisplayName = "Sample C";
        }
    }
}
