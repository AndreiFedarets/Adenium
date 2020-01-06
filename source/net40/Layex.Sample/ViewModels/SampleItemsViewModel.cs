using Layex.ViewModels;
using Layex.Views;
using System.Collections.Generic;

namespace Layex.Sample.ViewModels
{
    public sealed class SampleItemsViewModel : ItemsViewModel, IDialogViewModel
    {
        public SampleItemsViewModel()
        {
            DisplayName = "Layouted Items Sample";
            DisplayModes = new[] { DisplayMode.Grid, DisplayMode.Tab };
        }

        public IEnumerable<DisplayMode> DisplayModes { get; }

        public bool DialogReady { get; private set; }

        public bool? DialogResult
        {
            get { return DialogReady; }
        }

        public void OnReadyChanged(bool ready)
        {
            DialogReady = ready;
            NotifyOfPropertyChange(() => DialogReady);
        }
    }
}
