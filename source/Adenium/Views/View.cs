using Adenium.Controls;
using Adenium.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Adenium.Views
{
    public class View : UserControl
    {
        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
        }

        public View()
        {
            DataContextChanged += OnDataContextChanged;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)DataContext; }
        }

        private void RenderContent()
        {
            IItemsViewModel itemsViewModel = ViewModel as IItemsViewModel;
            if (itemsViewModel != null)
            {
                ContentControl contentControl = ViewManager.FindViewContent(this);
                switch (itemsViewModel.Layout.DisplayMode)
                {
                    case Layouts.DisplayMode.Tab:
                        contentControl.Content = new ViewTabControl() { ViewModel = itemsViewModel };
                        break;
                    case Layouts.DisplayMode.Grid:
                        contentControl.Content = new ViewGridControl() { ViewModel = itemsViewModel };
                        break;
                    default:
                        throw new NotSupportedException($"DisplayMode.'{itemsViewModel.Layout.DisplayMode}' value is not supported");
                }
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            View view = (View) sender;
            view.RenderContent();
        }
    }
}
