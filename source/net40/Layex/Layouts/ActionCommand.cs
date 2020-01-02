using System.Windows.Data;

namespace Layex.Layouts
{
    public sealed class ActionCommand : Action
    {
        public Binding ParameterBinding { get; set; }
    }
}
