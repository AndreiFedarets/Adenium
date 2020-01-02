namespace Layex.Contracts
{
    public interface IDialogContractConsumer : IContractConsumer
    {
        void OnReadyChanged(bool ready);
    }
}
