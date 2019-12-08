using Adenium.Extensions;
using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Adenium.Controls
{
    public class ViewGridControl : ItemsControl
    {
        private const int CellSideLength = 50;

        private static readonly DependencyProperty RowsCountProperty;
        private static readonly DependencyProperty ColumnsCountProperty;


        static ViewGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewGridControl), new FrameworkPropertyMetadata(typeof(ViewGridControl)));
            RowsCountProperty = DependencyProperty.Register("RowsCount", typeof(int), typeof(ViewGridControl), new PropertyMetadata());
            ColumnsCountProperty = DependencyProperty.Register("ColumnsCount", typeof(int), typeof(ViewGridControl), new PropertyMetadata());
        }

        public ViewGridControl()
        {
            //Using Loaded event because only after that ItemsHost become available
            Loaded += OnControlLoaded;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public int RowsCount
        {
            get { return (int)GetValue(RowsCountProperty); }
            set { SetValue(RowsCountProperty, value); }
        }

        public int ColumnsCount
        {
            get { return (int)GetValue(ColumnsCountProperty); }
            set { SetValue(ColumnsCountProperty, value); }
        }

        private Grid Panel
        {
            get
            {
                try
                {
                    return (Grid)typeof(MultiSelector).InvokeMember("ItemsHost", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance, null, this, null);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
