using Layex.Actions;

namespace Layex.Sample.ModuleB.Actions
{
    internal sealed class OpenViewAction : ActivateViewModelAction
    {
        public OpenViewAction()
            : base(typeof(ViewModels.SampleViewModel))
        {
        }

        public override string DisplayName
        {
            get { return "Open View B"; }
        }
    }
}