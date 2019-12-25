using Layex.ViewModels;

namespace Layex.Sample.ModuleA.ViewModels
{
    [ViewModel("Layex.Sample.ModuleA.Sample")]
    public sealed class SampleViewModel : ViewModel
    {
        public SampleViewModel()
        {
            DisplayName = "Sample A";
        }
    }
}
