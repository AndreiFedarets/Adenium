using Adenium.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class ViewItemsControl : ContentControl
    {
        public static readonly DependencyProperty ViewModelProperty;
        public static readonly DependencyProperty ActiveItemProperty;
        public static readonly DependencyProperty ItemsProperty;
        private static readonly DependencyPropertyKey ItemsPropertyKey;

        private readonly ObservableCollection<ViewItem> _items;

        static ViewItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewItemsControl), new FrameworkPropertyMetadata(typeof(ViewItemsControl)));
            ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IItemsViewModel), typeof(ViewItemsControl), new FrameworkPropertyMetadata(OnViewModelPropertyChanged));
            ActiveItemProperty = DependencyProperty.Register("ActiveItem", typeof(ViewItem), typeof(ViewTabControl), new FrameworkPropertyMetadata(OnActiveItemPropertyChanged));
            ItemsPropertyKey = DependencyProperty.RegisterReadOnly("Items", typeof(IEnumerable<ViewItem>), typeof(ViewItemsControl), new PropertyMetadata(null));
            ItemsProperty = ItemsPropertyKey.DependencyProperty;
        }

        public ViewItemsControl()
        {
            _items = new ObservableCollection<ViewItem>();
            Items = _items;
        }

        public IItemsViewModel ViewModel
        {
            get { return (IItemsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public IEnumerable<ViewItem> Items
        {
            get { return (IEnumerable<ViewItem>)GetValue(ItemsProperty); }
            private set { SetValue(ItemsPropertyKey, value); }
        }

        protected virtual void Initialize()
        {
            if (ViewModel == null)
            {
                return;
            }
            foreach (IViewModel viewModel in ViewModel)
            {
                ViewItem viewItem = new ViewItem() { ViewModel = viewModel };
                _items.Add(viewItem);
            }
            ViewModel.CollectionChanged += OnViewModelCollectionChanged;
        }

        protected virtual void Uninitialize(IItemsViewModel oldViewModel)
        {
            if (oldViewModel == null)
            {
                return;
            }
            oldViewModel.CollectionChanged -= OnViewModelCollectionChanged;
            _items.Clear();
        }

        private void OnViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }
        
        private static void OnActiveItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            //ViewTabControl viewTabControl = (ViewTabControl)sender;
           // viewTabControl.OnActiveItemChanged((ViewItem)eventArgs.NewValue, (ViewItem)eventArgs.OldValue);
        }

        private static void OnViewModelPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            ViewItemsControl control = (ViewItemsControl)sender;
            IItemsViewModel oldViewModel = eventArgs.OldValue as IItemsViewModel;
            control.Uninitialize(oldViewModel);
            control.Initialize();
        }
    }
}
