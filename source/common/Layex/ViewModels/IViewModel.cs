using Caliburn.Micro;
using System;

namespace Layex.ViewModels
{
    public interface IViewModel : INotifyPropertyChangedEx, IDisposableEx
    {
        IItemsViewModel Parent { get; }

        event EventHandler<ActivationEventArgs> Activated;

        event EventHandler<DeactivationEventArgs> Deactivated;
    }
}
