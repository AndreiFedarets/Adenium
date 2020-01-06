namespace Layex.ViewModels
{
    [Contracts.EnableContract(typeof(Contracts.DialogContract))]
    public interface IDialogViewModel : IViewModel, Contracts.IDialogContractConsumer
    {
        bool? DialogResult { get; }
    }
}
