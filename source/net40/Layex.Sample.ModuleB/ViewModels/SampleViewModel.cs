using Layex.ViewModels;
using System;

namespace Layex.Sample.ModuleB.ViewModels
{
    [ViewModel("Layex.Sample.ModuleB.SampleViewModel")]
    public sealed class SampleViewModel : ViewModel, Contracts.IDialogContractSource
    {
        private bool _dialogReady;

        public SampleViewModel()
        {
            DisplayName = "Sample B";
        }

        public bool DialogReady
        {
            get { return _dialogReady; }
            set
            {
                _dialogReady = value;
                NotifyOfPropertyChange(() => DialogReady);
                ContractSourceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ContractSourceChanged;
    }
}
