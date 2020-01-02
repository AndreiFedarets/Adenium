using System;

namespace Layex.Layouts
{
    internal sealed class JsonLayoutReader : ILayoutReader
    {
        public bool SupportsContent(string applicationContent)
        {
            return false;
        }

        public Application Read(string applicationContent)
        {
            throw new NotImplementedException();
        }
    }
}
