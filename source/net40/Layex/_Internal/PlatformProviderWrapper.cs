using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Layex
{
    internal sealed class PlatformProviderWrapper : IPlatformProvider
    {
        private IPlatformProvider _underlyingPlatformProvider;

        private PlatformProviderWrapper(IPlatformProvider underlyingPlatformProvider)
        {
            _underlyingPlatformProvider = underlyingPlatformProvider;
        }

        public static void Initialize()
        {
            PlatformProvider.Current = new PlatformProviderWrapper(PlatformProvider.Current);
        }

        public bool InDesignMode
        {
            get { return _underlyingPlatformProvider.InDesignMode; }
        }

        public void BeginOnUIThread(System.Action action)
        {
            _underlyingPlatformProvider.BeginOnUIThread(action);
        }

        public void ExecuteOnFirstLoad(object view, Action<object> handler)
        {
            _underlyingPlatformProvider.ExecuteOnFirstLoad(view, handler);
        }

        public void ExecuteOnLayoutUpdated(object view, Action<object> handler)
        {
            _underlyingPlatformProvider.ExecuteOnLayoutUpdated(view, handler);
        }

        public object GetFirstNonGeneratedView(object view)
        {
            return _underlyingPlatformProvider.GetFirstNonGeneratedView(view);
        }

        public System.Action GetViewCloseAction(object viewModel, ICollection<object> views, bool? dialogResult)
        {
            System.Action closeAction = _underlyingPlatformProvider.GetViewCloseAction(viewModel, views, dialogResult);
            return () =>
            {
                ViewModels.IDialogViewModel dialogViewModel = viewModel as ViewModels.IDialogViewModel;
                if (dialogViewModel != null)
                {
                    dialogViewModel.DialogResult = dialogResult;
                }
                closeAction();
            };
        }

        public void OnUIThread(System.Action action)
        {
            _underlyingPlatformProvider.OnUIThread(action);
        }

        public Task OnUIThreadAsync(System.Action action)
        {
            return OnUIThreadAsync(action);
        }
    }
}
