using System.Xaml;

namespace Layex.Layouts
{
    public sealed class XamlLayoutReader : ILayoutReader
    {
        public Application Read(string applicationContent)
        {
            Application application = (Application)XamlServices.Parse(applicationContent);
            return application;
        }
    }
}
