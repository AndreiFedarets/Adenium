using Caliburn.Micro;
using System;

namespace Adenium.ViewModels
{
    public interface IViewModel : IDisposable
    {
        event EventHandler<ActivationEventArgs> Activated;

        event EventHandler<DeactivationEventArgs> Deactivated;
    }
}
