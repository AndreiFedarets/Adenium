using System;

namespace Layex.ViewModels
{
    internal sealed class MenuControlHandlerLazy : MenuControlHandlerBase
    {
        private readonly Lazy<IMenuControlHandler> _handler;
        private readonly Type _controlHandlerType;
        private readonly IDependencyContainer _container;

        public MenuControlHandlerLazy(Type controlHandlerType, IDependencyContainer container)
        {
            _controlHandlerType = controlHandlerType;
            _container = container;
            _handler = new Lazy<IMenuControlHandler>(CreateControlHandler);
        }

        public override void OnControlAttached(IMenuControl control)
        {
            _handler.Value.OnControlAttached(control);
        }

        public override void OnViewModelAttached(IViewModel ownerViewModel)
        {
            _handler.Value.OnViewModelAttached(ownerViewModel);
        }

        public override bool GetVisible()
        {
            return _handler.Value.GetVisible();
        }

        public override bool GetEnabled()
        {
            return _handler.Value.GetEnabled();
        }

        public override void OnAction()
        {
            _handler.Value.OnAction();
        }

        public override string GetText()
        {
            return _handler.Value.GetText();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_handler.IsValueCreated)
            {
                _handler.Value.Dispose();
            }
        }

        private IMenuControlHandler CreateControlHandler()
        {
            return (IMenuControlHandler)_container.Resolve(_controlHandlerType);
        }
    }
}
