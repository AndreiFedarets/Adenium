using Adenium.Extensions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Adenium.Controls
{
    public class ViewGridControl : ItemsControl
    {
        static ViewGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewGridControl), new FrameworkPropertyMetadata(typeof(ViewGridControl)));
        }

        public ViewGridControl()
        {
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                Panel p1 = (Panel)typeof(MultiSelector).InvokeMember("ItemsHost", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance, null, this, null);
                Panel p2 = this.GetItemsControlPanel();
                bool eq = ReferenceEquals(p1, p2);
            }
            catch (System.Exception)
            {
                
            }
        }
    }
}
