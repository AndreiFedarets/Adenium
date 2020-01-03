using Layex.ViewModels;
using System;

namespace Layex.Extensions
{
    public static class ViewModelExtensions
    {
        public static string GetViewModelName(this IViewModel viewModel)
        {
            return GetViewModelName(viewModel.GetType());
        }

        public static string GetViewModelName<T>() where T : IViewModel
        {
            return GetViewModelName(typeof(T));
        }

        public static string GetViewModelName(Type type)
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
