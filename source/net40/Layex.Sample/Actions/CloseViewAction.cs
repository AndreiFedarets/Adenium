using Layex.Actions;

namespace Layex.Sample.Actions
{
    internal sealed class CloseViewAction : CloseViewModelAction
    {
        public CloseViewAction()
            : base(typeof(ViewModels.SampleItemsViewModel))
        {
        }

        public override string DisplayName
        {
            get { return "Close Items View"; }
        }
    }
}