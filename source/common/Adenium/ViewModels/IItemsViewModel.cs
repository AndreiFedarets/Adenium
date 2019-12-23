using Adenium.Layouts;
using System.Collections.Generic;

namespace Adenium.ViewModels
{
    public interface IItemsViewModel : IViewModel, IEnumerable<IViewModel>
    {
        DisplayMode DisplayMode { get; set; }
    }
}
