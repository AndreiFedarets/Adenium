namespace Layex.ViewModels
{
    internal sealed class MenuItem : MenuControl
    {
        private string _text;

        public MenuItem(string id)
            : base(id)
        {
            _text = string.Empty;
            Children = new MenuControlCollection();
        }

        public string Text
        {
            get { return _text; }
            private set
            {
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public IMenuControlCollection Children { get; private set; }

        public void OnAction()
        {
            Handler.OnAction();
        }

        public override void Invalidate()
        {
            base.Invalidate();
            if (IsVisible)
            {
                Text = Handler.GetText();
            }
            Children.Invalidate();
        }

        internal override void Initialize(IViewModel ownerViewModel)
        {
            base.Initialize(ownerViewModel);
            foreach (MenuControl control in _collection)
            {
                control.Initialize(ownerViewModel);
            }
        }

        internal override void Merge(MenuControl control)
        {
            base.Merge(control);
            MenuControlCollection controlCollection = control as MenuControlCollection;
            if (controlCollection != null)
            {
                foreach (MenuControl childControl in controlCollection.Cast<MenuControl>())
                {
                    Add(childControl);
                }
            }
        }
    }
}
