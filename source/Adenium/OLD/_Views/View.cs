using System;
using System.Windows;
using System.Windows.Controls;

namespace Adenium
{
    public class View : UserControl
    {
        public static readonly DependencyProperty DisplayPanelProperty;
        public static readonly DependencyProperty ViewPositionProperty;
        public static readonly DependencyProperty AttachToProperty;
        private IViewBehaviorExtension _behaviorExtension;

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
            DisplayPanelProperty = DependencyProperty.Register("DisplayPanel", typeof(bool), typeof(View), new FrameworkPropertyMetadata(false));
            ViewPositionProperty = DependencyProperty.Register("ViewPosition", typeof(Position), typeof(View), new FrameworkPropertyMetadata(Position.Default));
            AttachToProperty = DependencyProperty.Register("AttachTo", typeof(string), typeof(View), new FrameworkPropertyMetadata(string.Empty));
        }

        public View()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
            DataContextChanged += OnDataContextChanged;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)DataContext; }
        }

        public bool DisplayPanel
        {
            get { return (bool)GetValue(DisplayPanelProperty); }
            set { SetValue(DisplayPanelProperty, value); }
        }

        public Position ViewPosition
        {
            get { return (Position)GetValue(ViewPositionProperty); }
            set { SetValue(ViewPositionProperty, value); }
        }

        public string AttachTo
        {
            get { return (string)GetValue(AttachToProperty); }
            set { SetValue(AttachToProperty, value); }
        }

        private void UpdateBehaviorExtension()
        {
            if (_behaviorExtension != null)
            {
                _behaviorExtension.Dispose();
            }
            if (DataContext is TabViewModel)
            {
                _behaviorExtension = new TabViewBehaviorExtension(this, (TabViewModel)DataContext);
            }
            else if (DataContext is GridViewModel)
            {
                _behaviorExtension = new GridViewBehaviorExtension(this, (GridViewModel)DataContext);
            }
            else if (DataContext is ViewModel)
            {
                _behaviorExtension = new ContentViewBehaviorExtension();
            }
            else
            {
                throw new NotSupportedException("This type of DataContext is not supported");
            }
            _behaviorExtension.Initialize();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            View view = (View) sender;
            view.UpdateBehaviorExtension();
        }
    }
}
