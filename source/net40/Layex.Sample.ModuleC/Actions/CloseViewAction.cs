using Layex.Actions;

namespace Layex.Sample.ModuleC.Actions
{
    internal sealed class CloseViewAction : CloseViewModelAction
    {
        public CloseViewAction()
            : base(typeof(ViewModels.SampleViewModel))
        {
        }

        public override string DisplayName
        {
            get { return "Close View C"; }
        }
    }
}