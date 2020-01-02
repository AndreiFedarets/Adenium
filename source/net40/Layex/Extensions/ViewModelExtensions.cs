using Layex.ViewModels;
using System;

namespace Layex.Extensions
{
    public static class ViewModelExtensions
    {
        public static string GetCodeName(this IViewModel viewModel)
        {
            return GetCodeName(viewModel.GetType());
        }

        public static string GetCodeName<T>() where T : IViewModel
        {
            return GetCodeName(typeof(T));
        }

        public static string GetCodeName(Type type)
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
            return AreCodeNameEquals(viewModel1.GetCodeName(), viewModel2.GetCodeName());
        }

        public static bool AreCodeNameEquals(this IViewModel viewModel, string codeName)
        {
            return AreCodeNameEquals(viewModel.GetCodeName(), codeName);
        }

        public static bool AreCodeNameEquals(string codeName1, string codeName2)
        {
            return string.Equals(codeName1, codeName2, StringComparison.Ordinal);
        }
    }
}
