namespace Layex.ViewModels
{
    public abstract class DialogItemsViewModel : ItemsViewModel, IDialogViewModel
    {
        public bool? DialogResult { get; set; }
    }
}
