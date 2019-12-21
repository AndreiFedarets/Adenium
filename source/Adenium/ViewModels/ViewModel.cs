using Caliburn.Micro;

namespace Adenium.ViewModels
{
    public abstract class ViewModel : Screen, IViewModel
    {
        public virtual void Dispose()
        {
            
        }

        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }
    }
}
