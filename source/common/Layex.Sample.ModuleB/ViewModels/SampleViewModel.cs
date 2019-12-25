using Layex.ViewModels;

namespace Layex.Sample.ModuleB.ViewModels
{
    [ViewModel("Layex.Sample.ModuleB.SampleViewModel")]
    public sealed class SampleViewModel : ViewModel
    {
        public SampleViewModel()
        {
            DisplayName = "Sample B";
        }
    }
}
