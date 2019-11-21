using Adenium.Sample.ViewModels;
using System.Windows;

namespace Adenium.Sample
{
    public sealed class Bootstrapper : Adenium.Bootstrapper
    {
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
