namespace Layex.ViewModels
{
    public abstract class DialogViewModel : ViewModel, IDialogViewModel
    {
        public bool? DialogResult { get; set; }
    }
}
