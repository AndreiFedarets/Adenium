using System;
using Layex.ViewModels;

namespace Layex.Sample.ModuleA.ViewModels
{
    [ViewModel("Layex.Sample.ModuleA.Sample")]
    public sealed class SampleViewModel : ViewModel, Contracts.IDialogContractSource
    {
        private bool _dialogReady;

        public SampleViewModel()
        {
            DisplayName = "Sample A";
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
