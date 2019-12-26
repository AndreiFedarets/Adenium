using System.Collections.Generic;

namespace Layex.ViewModels
{
    public interface IMenuCollection : IEnumerable<IMenu>
    {
        IMenu this[string id] { get; }
    }
}
