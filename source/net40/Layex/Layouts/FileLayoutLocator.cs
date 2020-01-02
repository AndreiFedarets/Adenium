using System.Collections.Generic;
using System.IO;

namespace Layex.Layouts
{
    public sealed class FileLayoutLocator : ILayoutLocator
    {
        private readonly IBootstrapperEnvironment _environment;

        public FileLayoutLocator(IBootstrapperEnvironment environment)
        {
            _environment = environment;
        }

        public IEnumerable<string> LocateLayouts()
        {
            const string searchPattern = "*.layout.xaml";
            IEnumerable<string> layoutFiles = _environment.FindFiles(searchPattern);
            foreach (string layoutFile in layoutFiles)
            {
                yield return File.ReadAllText(layoutFile);
            }
        }
    }
}
