using Adenium.Layouting;
using System;

namespace Adenium
{
    public interface IViewModel : IDisposable
    {
        string CodeName { get; }

        string DisplayName { get; }

        IContainerViewModel Parent { get; }

        IContainerViewModel LogicalParent { get; }

        IMenuCollection Menus { get; }
    }
}
