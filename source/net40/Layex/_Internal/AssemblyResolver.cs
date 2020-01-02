using System;
using System.Reflection;

namespace Layex
{
    internal sealed class AssemblyResolver
    {
        private readonly IBootstrapperEnvironment _environment;

        public AssemblyResolver(IBootstrapperEnvironment environment)
        {
            _environment = environment;
        }

        public void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnCurrentDomainAssemblyResolve;
        }

        private Assembly OnCurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;
            try
            {
                AssemblyName name = new AssemblyName(args.Name);
                string fileName = name.Name + ".dll";
                string fileFullName = _environment.FindFile(fileName);
                if (!string.IsNullOrEmpty(fileFullName))
                {
                    assembly = Assembly.LoadFile(fileFullName);
                }
            }
            catch (Exception)
            {
                
            }
            return assembly;
        }
    }
}
