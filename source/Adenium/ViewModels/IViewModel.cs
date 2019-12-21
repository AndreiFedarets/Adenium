using Caliburn.Micro;
using System;

namespace Adenium.ViewModels
{
    public interface IViewModel : INotifyPropertyChangedEx, IDisposable
    {
        IItemsViewModel Parent { get; }

        event EventHandler<ActivationEventArgs> Activated;

        event EventHandler<DeactivationEventArgs> Deactivated;
    }
}
