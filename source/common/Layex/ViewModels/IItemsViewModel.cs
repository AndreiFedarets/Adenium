using Layex.Layouts;
using System.Collections.Generic;

namespace Layex.ViewModels
{
    public interface IItemsViewModel : IViewModel, IEnumerable<IViewModel>
    {
        DisplayMode DisplayMode { get; set; }
    }
}
