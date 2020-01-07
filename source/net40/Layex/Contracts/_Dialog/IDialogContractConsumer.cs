namespace Layex.Contracts
{
    [EnableContract(typeof(DialogContract))]
    public interface IDialogContractConsumer : IContractConsumer
    {
        void OnReadyChanged(bool ready);
    }
}
