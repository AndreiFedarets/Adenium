using Layex.ViewModels;
using System.Collections.Generic;

namespace Layex.Layouts
{
    public sealed class CompositeLayoutProvider : ILayoutProvider
    {
        private readonly List<ILayoutProvider> _providers;

        public CompositeLayoutProvider()
        {
            _providers = new List<ILayoutProvider>();
        }

        public Layout GetLayout(IViewModel viewModel)
        {
            Layout mergedLayout = new Layout() { Name = viewModel.Name };
            foreach (ILayoutProvider provider in _providers)
            {
                Layout layout = provider.GetLayout(viewModel);
                mergedLayout.Append(layout);
            }
            return mergedLayout;
        }

        public bool RegisterProvider(ILayoutProvider provider)
        {
            if (!_providers.Contains(provider))
            {
                _providers.Add(provider);
                return true;
            }
            return false;
        }

        public bool UnregisterProvider(ILayoutProvider provider)
        {
            return _providers.Remove(provider);
        }
    }
}
