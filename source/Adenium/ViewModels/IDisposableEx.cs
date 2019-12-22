using System;

namespace Adenium.ViewModels
{
    public interface IDisposableEx : IDisposable
    {
        event EventHandler Disposed;
    }
}
