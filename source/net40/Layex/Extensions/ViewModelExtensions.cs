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
            string codeName = string.Empty;
            if (type != null)
            {
                ViewModelAttribute attribute = ViewModelAttribute.GetAttribute(type);
                if (attribute != null)
                {
                    codeName = attribute.CodeName;
                }
                else
                {
                    codeName = type.FullName;
                }
            }
            return codeName;
        }

        public static bool AreCodeNameEquals(this IViewModel viewModel1, IViewModel viewModel2)
        {
            return AreCodeNameEquals(viewModel1.GetViewModelName(), viewModel2.GetViewModelName());
        }

        public static bool AreCodeNameEquals(this IViewModel viewModel, string codeName)
        {
            return AreCodeNameEquals(viewModel.GetViewModelName(), codeName);
        }

        public static bool AreCodeNameEquals(string codeName1, string codeName2)
        {
            return string.Equals(codeName1, codeName2, StringComparison.Ordinal);
        }
    }
}
