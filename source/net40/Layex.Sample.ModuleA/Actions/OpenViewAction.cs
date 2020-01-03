using Layex.Actions;

namespace Layex.Sample.ModuleA.Actions
{
    internal sealed class OpenViewAction : ActivateViewModelAction
    {
        public OpenViewAction()
            : base(typeof(ViewModels.SampleViewModel))
        {
        }

        public override string DisplayName
        {
            get { return "Open View A"; }
        }
    }
}