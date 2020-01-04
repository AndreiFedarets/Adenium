using System;

namespace Layex
{
    public interface IActivator
    {
        object Resolve(Type type);
    }
}
