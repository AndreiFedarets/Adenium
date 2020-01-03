using Caliburn.Micro;
using System;

namespace Layex.ViewModels
{
    public interface IViewModel : INotifyPropertyChangedEx, IDisposableEx
    {
        string Name { get; }

        bool Locked { get; set; }

        IItemsViewModel Parent { get; }

        event EventHandler<ActivationEventArgs> Activated;

        event EventHandler<DeactivationEventArgs> Deactivated;

        void Activate();

        void Close();
    }
}
