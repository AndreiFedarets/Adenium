using Layex.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Layex.Layouts
{
    public sealed class FileSystemLayoutProvider : ILayoutProvider
    {
        private const string LayoutFileExtension = ".layout.xaml";
        private readonly Dictionary<string, List<Layout>> _layouts;

        public FileSystemLayoutProvider()
            : this(AppDomain.CurrentDomain.BaseDirectory)
        {
        }

        public FileSystemLayoutProvider(string baseDirectory)
        {
            _layouts = LoadLayouts(baseDirectory);
        }

        private Dictionary<string, List<Layout>> LoadLayouts(string baseDirectory)
        {
            string[] applicationFiles = Directory.GetFiles(baseDirectory, "*" + LayoutFileExtension, SearchOption.AllDirectories);
            IEnumerable<string> contents = applicationFiles.Select(x => File.ReadAllText(x));
            IEnumerable<Application> applications = contents.Select(x => XamlLayoutParser.Parse(x));
            IEnumerable<IGrouping<string, Layout>> groups = applications.SelectMany(x => x).GroupBy(x => x.Name);
            Dictionary<string, List<Layout>> layouts = new Dictionary<string, List<Layout>>();
            foreach (IGrouping<string, Layout> group in groups)
            {
                layouts[group.Key] = new List<Layout>(group);
            }
            return layouts;
        }

        public Layout GetLayout(IViewModel viewModel)
        {
            Layout mergedLayout = new Layout() { Name = viewModel.Name };
            List<Layout> layouts;
            if (!_layouts.TryGetValue(viewModel.Name, out layouts))
            {
                return mergedLayout;
            }
            foreach (Layout layout in layouts)
            {
                mergedLayout.Append(layout);
            }
            return mergedLayout;
        }
    }
}
