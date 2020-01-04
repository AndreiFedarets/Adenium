using Layex.ViewModels;
using Layex.Views;
using System.Collections.Generic;

namespace Layex.Sample.ViewModels
{
    [Contracts.EnableContract(typeof(Contracts.DialogContract))]
    public sealed class SampleItemsViewModel : ItemsViewModel, Contracts.IDialogContractConsumer
    {
        public SampleItemsViewModel()
        {
            DisplayName = "Layouted Items Sample";
            DisplayModes = new[] { DisplayMode.Grid, DisplayMode.Tab };
        }

        public IEnumerable<DisplayMode> DisplayModes { get; }

        public bool DialogReady { get; private set; }

        public void OnReadyChanged(bool ready)
        {
            DialogReady = ready;
            NotifyOfPropertyChange(() => DialogReady);
        }
    }
}
