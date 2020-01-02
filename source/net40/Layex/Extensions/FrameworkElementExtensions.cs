using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Layex.Extensions
{
    public static class FrameworkElementExtensions
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

        public static T FindFirstChild<T>(this UIElement element, string name = null) where T : class
        {
            if (element is T)
            {
                if (!string.IsNullOrWhiteSpace(name) && element is FrameworkElement)
                {
                    FrameworkElement frameworkElement = (FrameworkElement) element;
                    if (string.Equals(frameworkElement.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        return element as T;
                    }
                }
                else
                {
                    return element as T;
                }
            }
            Panel panel = element as Panel;
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
            ContentControl contentControl = element as ContentControl;
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
            ItemsControl itemsControl = element as ItemsControl;
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
