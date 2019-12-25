using System;

namespace Layex.ViewModels
{
    public interface IDisposableEx : IDisposable
    {
        event EventHandler Disposed;
    }
}
