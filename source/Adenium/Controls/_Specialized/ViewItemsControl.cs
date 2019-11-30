using Adenium.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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
            ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ItemsViewModel), typeof(ViewItemsControl), new FrameworkPropertyMetadata(OnViewModelPropertyChanged));
            ActiveItemProperty = DependencyProperty.Register("ActiveItem", typeof(ViewItem), typeof(ViewItemsControl), new FrameworkPropertyMetadata(OnActiveItemPropertyChanged));
            ItemsPropertyKey = DependencyProperty.RegisterReadOnly("Items", typeof(IEnumerable<ViewItem>), typeof(ViewItemsControl), new PropertyMetadata(null));
            ItemsProperty = ItemsPropertyKey.DependencyProperty;
        }

        public ViewItemsControl()
        {
            _items = new ObservableCollection<ViewItem>();
            Items = _items;
        }

        public ItemsViewModel ViewModel
        {
            get { return (ItemsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public ViewItem ActiveItem
        {
            get { return (ViewItem)GetValue(ActiveItemProperty); }
            set { SetValue(ActiveItemProperty, value); }
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
            InitializeItems();
            ViewModel.CollectionChanged += OnViewModelCollectionChanged;
            ViewModel.ActivationProcessed += OnViewModelActivationProcessed;
        }

        private void InitializeItems()
        {
            _items.Clear();
            foreach (IViewModel viewModel in ViewModel)
            {
                //ViewItem viewItem = new ViewItem(viewModel);
                //_items.Add(viewItem);
            }
        }

        protected virtual void Uninitialize(ItemsViewModel oldViewModel)
        {
            _items.Clear();
            if (oldViewModel != null)
            {
                oldViewModel.CollectionChanged -= OnViewModelCollectionChanged;
                oldViewModel.ActivationProcessed -= OnViewModelActivationProcessed;
            }
        }

        private void OnViewModelActivationProcessed(object sender, ActivationProcessedEventArgs e)
        {
            if (e.Success)
            {
                ViewItem activeItem = _items.FirstOrDefault(x => ReferenceEquals(x.ViewModel, e.Item));
                if (!ReferenceEquals(ActiveItem, activeItem))
                {
                    ActiveItem = activeItem;
                }
            }
        }

        private void OnViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    int insertIndex = e.NewStartingIndex;
                    foreach (object newItem in e.NewItems)
                    {
                        IViewModel viewModel = (IViewModel)newItem;
                        //ViewItem viewItem = new ViewItem(viewModel);
                        //_items.Insert(insertIndex, viewItem);
                        insertIndex++;
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    throw new NotImplementedException();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    InitializeItems();
                    break;
            }
        }

        private void OnActiveItemChanged()
        {
            IViewModel activeItem = ActiveItem?.ViewModel;
            if (!ReferenceEquals(ViewModel.ActiveItem, activeItem))
            {
                ViewModel.ActiveItem = activeItem;
            }
        }

        private static void OnActiveItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            ViewItemsControl viewItemsControl = (ViewItemsControl)sender;
            viewItemsControl.OnActiveItemChanged();
        }

        private static void OnViewModelPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            ViewItemsControl control = (ViewItemsControl)sender;
            ItemsViewModel oldViewModel = eventArgs.OldValue as ItemsViewModel;
            control.Uninitialize(oldViewModel);
            control.Initialize();
        }
    }
}
