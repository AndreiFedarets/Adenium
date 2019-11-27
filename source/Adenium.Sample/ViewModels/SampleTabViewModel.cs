using Adenium.ViewModels;

namespace Adenium.Sample.ViewModels
{
    [ViewModel("Adenium.Sample.Tab")]
    public sealed class SampleTabViewModel : ItemsViewModel
    {
        public SampleTabViewModel()
        {
            DisplayName = "Tab Sample";
        }
    }
}
