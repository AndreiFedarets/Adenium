using Adenium.ViewModels;

namespace Adenium.Sample.ModuleA.ViewModels
{
    [ViewModel("Adenium.Sample.ModuleA.Sample")]
    public sealed class SampleViewModel : ViewModel
    {
        public SampleViewModel()
        {
            DisplayName = "Sample A";
        }
    }
}
