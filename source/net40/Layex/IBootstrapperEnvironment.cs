using System.Collections.Generic;

namespace Layex
{
    public interface IBootstrapperEnvironment
    {
        void AddProbingPath(string path);

        string FindFile(string searchPattern);
    }
}
