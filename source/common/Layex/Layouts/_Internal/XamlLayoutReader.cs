using System.Xaml;

namespace Layex.Layouts
{
    internal sealed class XamlLayoutReader : ILayoutReader
    {
        public Application Read(string applicationContent)
        {
            Application application = (Application)XamlServices.Parse(applicationContent);
            return application;
        }
    }
}
