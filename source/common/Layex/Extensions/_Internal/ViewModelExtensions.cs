using Layex.ViewModels;
using System;

namespace Layex.Extensions
{
    internal static class ViewModelExtensions
    {
        public static string GetCodeName(this IViewModel viewModel)
        {
            string codeName;
            ViewModelAttribute attribute = ViewModelAttribute.GetAttribute(viewModel);
            if (attribute != null)
            {
                codeName = attribute.CodeName;
            }
            else
            {
                codeName = viewModel.GetType().FullName;
            }
            return codeName.ToLowerInvariant();
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
            return string.Equals(codeName1, codeName2, StringComparison.InvariantCulture);
        }
    }
}
