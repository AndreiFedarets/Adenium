using System;

namespace Layex.ViewModels
{
    public sealed class ViewModelAttribute : Attribute
    {
        public ViewModelAttribute(string viewModelName)
        {
            ViewModelName = viewModelName;
        }

        public string ViewModelName { get; private set; }

        public static ViewModelAttribute GetAttribute(IViewModel viewModel)
        {
            Type viewModelType = viewModel.GetType();
            return GetAttribute(viewModelType);
        }

        public static ViewModelAttribute GetAttribute(Type viewModelType)
        {
            ViewModelAttribute attribute = (ViewModelAttribute)GetCustomAttribute(viewModelType, typeof(ViewModelAttribute), true);
            return attribute;
        }
    }
}
