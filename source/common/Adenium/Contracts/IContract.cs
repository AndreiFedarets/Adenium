using System;

namespace Adenium.Contracts
{
    public interface IContract : IDisposable
    {
        void Register(object item);

        void Unregister(object item);
    }

    public interface IContract<TSource, TConsumer> : IContract
    {

    }
}
