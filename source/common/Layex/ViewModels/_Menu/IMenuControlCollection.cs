using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Layex.ViewModels
{
    public interface IMenuControlCollection : IEnumerable<IMenuControl>, INotifyCollectionChanged, IDisposable
    {
        IMenuControl this[string id] { get; }

        void Invalidate();
    }
}
