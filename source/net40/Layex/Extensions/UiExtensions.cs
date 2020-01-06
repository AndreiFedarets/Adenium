using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Layex.Extensions
{
    public static class UiExtensions
    {
        //public static Panel GetItemsControlPanel(this ItemsControl itemsControl)
        //{
        //    ItemsPresenter itemsPresenter = GetVisualChild<ItemsPresenter>(itemsControl);
        //    Panel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Panel;
        //    return itemsPanel;
        //}

        //public static T GetVisualChild<T>(this DependencyObject parent) where T : Visual
        //{
        //    T child = default(T);
        //    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < numVisuals; i++)
        //    {
        //        Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
        //        child = v as T;
        //        if (child == null)
        //        {
        //            child = GetVisualChild<T>(v);
        //        }
        //        if (child != null)
        //        {
        //            break;
        //        }
        //    }
        //    return child;
        //}

        //public static Point GetPosition(this FrameworkElement element, FrameworkElement relativeTo)
        //{
        //    var positionTransform = element.TransformToAncestor(relativeTo);
        //    var areaPosition = positionTransform.Transform(new Point(0, 0));
        //    return areaPosition;
        //}

        //public static Point GetMiddlePosition(this FrameworkElement element, FrameworkElement relativeTo)
        //{
        //    Point point = element.GetPosition(relativeTo);
        //    point.Y -= relativeTo.ActualHeight / 2;
        //    return point;
        //}

        //public static IList<T> FindChildrenRecursive<T>(this UIElement element) where T : class
        //{
        //    List<T> children = new List<T>();
        //    if (element is T)
        //    {
        //        children.Add(element as T);
        //    }
        //    Panel panel = element as Panel;
        //    if (panel != null)
        //    {
        //        foreach (UIElement panelItem in panel.Children)
        //        {
        //            IList<T> subchildren = panelItem.FindChildrenRecursive<T>();
        //            children.AddRange(subchildren);
        //        }
        //    }
        //    ContentControl contentControl = element as ContentControl;
        //    if (contentControl != null)
        //    {
        //        UIElement visual = contentControl.Content as UIElement;
        //        if (visual != null)
        //        {
        //            IList<T> subchildren = visual.FindChildrenRecursive<T>();
        //            children.AddRange(subchildren);
        //        }
        //    }
        //    return children;
        //}

        public static void ScrollTo(this ItemsControl itemsControl, object item)
        {
            ScrollViewer scrollViewer = null;
            DependencyObject parent = itemsControl;
            while (true)
            {
                parent = VisualTreeHelper.GetChild(parent, 0);
                if (parent == null)
                {
                    return;
                }
                scrollViewer = parent as ScrollViewer;
                if (scrollViewer != null)
                {
                    break;
                }
            }
            int index = itemsControl.Items.IndexOf(item);
            if (index != -1)
            {
                scrollViewer.ScrollToVerticalOffset(index);
            }
        }

        public static void FindVisualChildren<T>(this DependencyObject dependencyObject, List<T> results) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (int i = 0; i < count; i++)
            {
                DependencyObject currentObject = VisualTreeHelper.GetChild(dependencyObject, i);
                T current = currentObject as T;
                if (current != null)
                {
                    results.Add(current);
                }
                FindVisualChildren<T>(currentObject, results);
            }
        }

        public static T FindFirstVisualChild<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (int i = 0; i < count; i++)
            {
                DependencyObject currentObject = VisualTreeHelper.GetChild(dependencyObject, i);
                T current = currentObject as T;
                if (current != null)
                {
                    return current;
                }
                current = FindFirstVisualChild<T>(currentObject);
                if (current != null)
                {
                    return current;
                }
            }
            return null;
        }

        public static T FindFirstChild<T>(this DependencyObject dependencyObject, string name = null) where T : class
        {
            if (dependencyObject is T)
            {
                if (!string.IsNullOrWhiteSpace(name) && dependencyObject is FrameworkElement)
                {
                    FrameworkElement frameworkElement = (FrameworkElement) dependencyObject;
                    if (string.Equals(frameworkElement.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        return dependencyObject as T;
                    }
                }
                else
                {
                    return dependencyObject as T;
                }
            }
            Panel panel = dependencyObject as Panel;
            if (panel != null)
            {
                foreach (UIElement panelItem in panel.Children)
                {
                    T child = panelItem.FindFirstChild<T>(name);
                    if (child != null)
                    {
                        return child;
                    }
                }
            }
            ContentControl contentControl = dependencyObject as ContentControl;
            if (contentControl != null)
            {
                UIElement visual = contentControl.Content as UIElement;
                if (visual != null)
                {
                    T child = visual.FindFirstChild<T>(name);
                    if (child != null)
                    {
                        return child;
                    }
                }
            }
            ItemsControl itemsControl = dependencyObject as ItemsControl;
            if (itemsControl != null)
            {
                foreach (object itemsControlItem in itemsControl.Items)
                {
                    UIElement uiElement = itemsControlItem as UIElement;
                    if (uiElement == null)
                    {
                        continue;
                    }
                    T child = uiElement.FindFirstChild<T>(name);
                    if (child != null)
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        public static T FindParent<T>(this DependencyObject element) where T : class
        {
            if (element == null)
            {
                return default(T);
            }
            DependencyObject childElement = VisualTreeHelper.GetParent(element) ?? LogicalTreeHelper.GetParent(element);
            return childElement.FindParentIntenal<T>();
        }

        private static T FindParentIntenal<T>(this DependencyObject element) where T : class
        {
            if (element == null)
            {
                return default(T);
            }
            if (element is T)
            {
                return element as T;
            }
            DependencyObject childElement = VisualTreeHelper.GetParent(element) ?? LogicalTreeHelper.GetParent(element);
            return childElement.FindParentIntenal<T>();
        }
    }
}
