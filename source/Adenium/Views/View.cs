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
            ItemsViewModel itemsViewModel = ViewModel as ItemsViewModel;
            if (itemsViewModel != null)
            {
                ContentControl contentControl = ViewManager.FindViewContent(this);
                switch (itemsViewModel.DisplayMode)
                {
                    case Layouts.DisplayMode.Tab:
                        contentControl.Content = new ViewTabControl() { DataContext = itemsViewModel };
                        break;
                    case Layouts.DisplayMode.Grid:
                        contentControl.Content = new ViewGridControl() { DataContext = itemsViewModel };
                        break;
                    default:
                        throw new NotSupportedException($"DisplayMode.'{itemsViewModel.DisplayMode}' value is not supported");
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
