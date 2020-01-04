namespace Layex.Contracts
{
    public interface IDialogContractSource : IContractSource
    {
        bool DialogReady { get; }
    }
}
