namespace Layex.Layouts
{
    public sealed class ActionCollection : Action
    {
        public ActionCollection()
        {
            Type = typeof(Actions.DefaultActionCollection);
        }
    }
}
