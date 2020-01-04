using System;
using System.Collections.Generic;
using System.Linq;

namespace Layex.Layouts
{
    public sealed class LayoutManager : ILayoutManager
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
                layout = new Layout() { Name = viewModelName };
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
            IEnumerable<IGrouping<string, Layout>> layoutGroups = layouts.GroupBy(x => x.Name);
            foreach (IGrouping<string, Layout> layoutsGroup in layoutGroups)
            {
                Layout mergedLayout = new Layout();
                foreach (Layout layout in layoutsGroup)
                {
                    mergedLayout.Append(layout);
                }
                _layouts[layoutsGroup.Key] = mergedLayout;
            }
        }
    }
}
