using System;
using System.Linq;

namespace Adenium.ViewModels
{
    public sealed class ViewModelAttribute : Attribute
    {
        public ViewModelAttribute(string codeName)
        {
            CodeName = codeName;
        }

        public string CodeName { get; private set; }

        public static ViewModelAttribute GetAttribute(IViewModel viewModel)
        {
            Type viewModelType = viewModel.GetType();
            object[] attributes = viewModelType.GetCustomAttributes(typeof(ViewModelAttribute), true);
            ViewModelAttribute attribute = (ViewModelAttribute)attributes.FirstOrDefault();
            return attribute;
        }
    }
}
