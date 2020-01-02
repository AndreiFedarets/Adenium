using System.Linq;

namespace Layex.Layouts
{
    internal sealed class SmartLayoutReader : ILayoutReader
    {
        private readonly ILayoutReader[] _readers;

        public SmartLayoutReader()
        {
            _readers = new ILayoutReader[] { new XmlLayoutReader(), new JsonLayoutReader() };
        }

        public bool SupportsContent(string applicationContent)
        {
            return FindLayoutReader(applicationContent) != null;
        }

        public Application Read(string applicationContent)
        {
            ILayoutReader layoutReader = FindLayoutReader(applicationContent);
            return layoutReader.Read(applicationContent);
        }

        private ILayoutReader FindLayoutReader(string layoutContent)
        {
            return _readers.FirstOrDefault(x => x.SupportsContent(layoutContent));
        }
    }
}
