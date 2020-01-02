using System.Collections.Generic;

namespace Layex.Layouts
{
    public interface ILayoutLocator
    {
        IEnumerable<string> LocateLayouts();
    }
}
