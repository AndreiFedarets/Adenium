using Layex.Layouts;
using System.Collections.Generic;

namespace Layex.ViewModels
{
    public interface IItemsViewModel : IViewModel, IEnumerable<IViewModel>
    {
        DisplayMode DisplayMode { get; set; }

        bool ActivateItem(string childCodeName);

        bool DeactivateItem(string childCodeName, bool close = false);

        void ActivateItem(IViewModel viewModel);

        void DeactivateItem(IViewModel viewModel, bool close = false);
    }
}
