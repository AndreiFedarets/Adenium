using Layex.ViewModels;
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

        public bool SupportsContentType(string layoutContent)
        {
            return FindLayoutReader(layoutContent) != null;
        }

        public Layout ReadLayout(string layoutContent)
        {
            ILayoutReader layoutReader = FindLayoutReader(layoutContent);
            return layoutReader.ReadLayout(layoutContent);
        }

        public Attachment ReadAttachment(string attachmentContent)
        {
            ILayoutReader layoutReader = FindLayoutReader(attachmentContent);
            return layoutReader.ReadAttachment(attachmentContent);
        }

        private ILayoutReader FindLayoutReader(string layoutContent)
        {
            return _readers.FirstOrDefault(x => x.SupportsContentType(layoutContent));
        }
    }
}
