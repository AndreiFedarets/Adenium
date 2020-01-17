using Layex.Extensions;
using Layex.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Layex.Views
{
    public static class ViewManager
    {
        public const string ViewContentPartName = "ViewContent";
        private static readonly Func<object, DependencyObject, object, UIElement> LocateForModel;
        private static readonly Func<Type, DependencyObject, object, Type> LocateTypeForModelType;

        private static readonly Func<Type, UIElement> GetOrCreateViewType;
        private static readonly Func<string, string> ModifyModelTypeAtDesignTime;
        private static readonly Func<string, object, IEnumerable<string>> TransformName;
        private static readonly Func<Type, Type, string> DeterminePackUriFromType;
        private static readonly Func<Type, DependencyObject, object, UIElement> LocateForModelType;

        static ViewManager()
        {
            LocateForModel = ViewLocator.LocateForModel;
            ViewLocator.LocateForModel = LocateForModelPrivate;
            LocateTypeForModelType = ViewLocator.LocateTypeForModelType;
            ViewLocator.LocateTypeForModelType = LocateTypeForModelTypeInternal;


            DeterminePackUriFromType = ViewLocator.DeterminePackUriFromType;
            ViewLocator.DeterminePackUriFromType = DeterminePackUriFromTypeInternal;

            LocateForModelType = ViewLocator.LocateForModelType;
            ViewLocator.LocateForModelType = LocateForModelTypeInternal;

            TransformName = ViewLocator.TransformName;
            ViewLocator.TransformName = TransformNameInternal;

            ModifyModelTypeAtDesignTime = ViewLocator.ModifyModelTypeAtDesignTime;
            ViewLocator.ModifyModelTypeAtDesignTime = ModifyModelTypeAtDesignTimeInternal;

            GetOrCreateViewType = ViewLocator.GetOrCreateViewType;
            ViewLocator.GetOrCreateViewType = GetOrCreateViewTypeInternal;
        }

        public static void Initialize()
        {

        }

        private static UIElement GetOrCreateViewTypeInternal(Type type)
        {
            UIElement result = GetOrCreateViewType(type);
            return result;
        }

        private static string ModifyModelTypeAtDesignTimeInternal(string type)
        {
            string result = ModifyModelTypeAtDesignTime(type);
            return result;
        }

        private static IEnumerable<string> TransformNameInternal(string name, object @object)
        {
            IEnumerable<string> result = TransformName(name, @object);
            return result;
        }

        private static UIElement LocateForModelTypeInternal(Type type, DependencyObject dependencyObject, object @object)
        {
            UIElement result = LocateForModelType(type, dependencyObject, @object);
            return result;
        }

        private static string DeterminePackUriFromTypeInternal(Type type1, Type type2)
        {
            return DeterminePackUriFromType(type1, type2);
        }

        private static Type LocateTypeForModelTypeInternal(Type type, DependencyObject dependencyObject, object @object)
        {
            object[] attributes = type.GetCustomAttributes(typeof(ViewModelAttribute), false);
            ViewModelAttribute attribute = (ViewModelAttribute)attributes.FirstOrDefault();
            Type viewType = null;
            if (attribute != null && !string.IsNullOrWhiteSpace(attribute.ViewType))
            {
                viewType = Type.GetType(attribute.ViewType);
            }
            viewType = viewType ?? LocateTypeForModelType(type, dependencyObject, @object);
            return viewType;
        }

        private static UIElement LocateForModelPrivate(object viewModel, DependencyObject dependencyObject, object context)
        {
            EnsureViewModelAssembly(viewModel);
            UIElement view = LocateForModel(viewModel, dependencyObject, context);
            return view;
        }

        public static UIElement LocateViewForViewModel(IViewModel viewModel)
        {
            return LocateForModelPrivate(viewModel, null, null);
        }

        //public static UIElement LocateViewForModel(object viewModel)
        //{
        //    UIElement viewElement = ViewLocator.LocateForModel(viewModel, null, null);
        //    View view = viewElement as View;
        //    if (view == null)
        //    {
        //        return viewElement;
        //    }
        //    ViewModelBinder.Bind(viewModel, view, null);
        //    return view;
        //}

        public static ContentControl FindViewContent(View view)
        {
            ContentControl contentControl = view.FindFirstChild<ContentControl>(ViewContentPartName);
            if (contentControl == null)
            {
                contentControl = view;
            }
            return contentControl;
        }

        private static void EnsureViewModelAssembly(object viewModel)
        {
            Type viewModelType = viewModel.GetType();
            Assembly assembly = viewModelType.Assembly;
            if (!AssemblySource.Instance.Contains(assembly))
            {
                AssemblySource.Instance.Add(assembly);
            }
        }
    }
}
