using Layex.Actions;

namespace Layex.Sample.ModuleC.Actions
{
    internal sealed class OpenViewAction : ActivateViewModelAction
    {
        public OpenViewAction()
            : base(typeof(ViewModels.SampleViewModel))
        {
        }

        public override string DisplayName
        {
            get { return "Open View C"; }
        }
    }
}