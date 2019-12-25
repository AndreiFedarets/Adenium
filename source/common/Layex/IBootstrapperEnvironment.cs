using System.Collections.Generic;

namespace Layex
{
    public interface IBootstrapperEnvironment
    {
        void AddProbingPath(string path);

        IEnumerable<string> FindFiles(string searchPattern);

        string FindFile(string searchPattern);
    }
}
