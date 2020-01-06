using System;

namespace Layex.ViewModels
{
    public sealed class ViewModelAttribute : Attribute
    {
        public ViewModelAttribute(string viewModelName)
            : this(viewModelName, string.Empty)
        {
        }

        public ViewModelAttribute(string viewModelName, string viewType)
        {
            ViewModelName = viewModelName;
            ViewType = viewType;
        }

        public string ViewModelName { get; private set; }

        public string ViewType { get; private set; }

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
