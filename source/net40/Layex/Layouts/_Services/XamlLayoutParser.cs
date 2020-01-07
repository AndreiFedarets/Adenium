using System.Xaml;

namespace Layex.Layouts
{
    public static class XamlLayoutParser
    {
        public static Application Parse(string content)
        {
            Application application = (Application)XamlServices.Parse(content);
            return application;
        }
    }
}
