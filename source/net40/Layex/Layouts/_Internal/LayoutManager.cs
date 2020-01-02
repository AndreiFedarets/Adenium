using System;
using System.Collections.Generic;
using System.Linq;

namespace Layex.Layouts
{
    internal sealed class LayoutManager : ILayoutManager
    {
        private readonly ILayoutLocator _layoutLocator;
        private readonly ILayoutReader _layoutReader;
        private readonly Dictionary<string, Layout> _layouts;
        private bool _layoutsLoaded;

        public LayoutManager(ILayoutLocator layoutLocator, ILayoutReader layoutReader)
        {
            _layoutLocator = layoutLocator;
            _layoutReader = layoutReader;
            _layouts = new Dictionary<string, Layout>();
        }

        public Layout GetLayout(string viewModelName)
        {
            LoadLayouts();
            Layout layout;
            if (!_layouts.TryGetValue(viewModelName, out layout))
            {
                //TODO: log warning
                layout = new Layout() { ViewModelName = viewModelName };
            }
            return layout;
        }

        private void LoadLayouts()
        {
            if (_layoutsLoaded)
            {
                return;
            }
            List<Layout> layouts = ReadRawLayouts();
            LoadLayouts(layouts);
            _layoutsLoaded = true;
        }

        private List<Layout> ReadRawLayouts()
        {
            IEnumerable<string> layoutContents = _layoutLocator.LocateLayouts();
            List<Layout> layouts = new List<Layout>();
            foreach (string layoutContent in layoutContents)
            {
                try
                {
                    Application application = _layoutReader.Read(layoutContent);
                    layouts.AddRange(application);
                }
                catch (Exception)
                {
                    //TODO: log exception
                    continue;
                }
            }
            return layouts;
        }

        private void LoadLayouts(IEnumerable<Layout> layouts)
        {
            IEnumerable<IGrouping<string, Layout>> groups = layouts.GroupBy(x => x.ViewModelName);
            foreach (IGrouping<string, Layout> group in groups)
            {
                _layouts[group.Key] = MergeLayouts(group);
            }
        }

        private Layout MergeLayouts(IEnumerable<Layout> layouts)
        {
            Layout layout = new Layout();
            //merge ViewModels
            IEnumerable<ViewModel> viewModels = layouts.SelectMany(x => x.ViewModels);
            layout.ViewModels.AddRange(viewModels);
            layout.ViewModels.Sort((x, y) => x.Order.CompareTo(y.Order));
            //merge Actions
            IEnumerable<Action> actions = layouts.SelectMany(x => x.Actions);
            layout.Actions.AddRange(actions);
            layout.Actions.Sort((x, y) => x.Order.CompareTo(y.Order));
            //merge Contracts
            IEnumerable<Contract> contracts = layouts.SelectMany(x => x.Contracts);
            layout.Contracts.AddRange(contracts);
            return layout;
        }
    }
}
