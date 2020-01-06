using Layex.ViewModels;
using System;

namespace Layex.Extensions
{
    public static class ViewModelExtensions
    {
        public static string GetViewModelDefaultName<T>()
        {
            return GetViewModelDefaultName(typeof(T));
        }

        public static string GetViewModelDefaultName(Type type)
        {
            string viewModelName = string.Empty;
            if (type != null)
            {
                ViewModelAttribute attribute = ViewModelAttribute.GetAttribute(type);
                if (attribute != null)
                {
                    viewModelName = attribute.ViewModelName;
                }
                else
                {
                    viewModelName = type.FullName;
                }
            }
            return viewModelName;
        }
    }
}
