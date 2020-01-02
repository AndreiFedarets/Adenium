using System;

namespace Layex.ViewModels
{
    public interface IMenuControl : IDisposable
    {
        string Id { get; }

        void Invalidate();
    }
}
