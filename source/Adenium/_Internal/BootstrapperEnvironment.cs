using System.Collections.Generic;
using System.IO;

namespace Adenium
{
    internal sealed class BootstrapperEnvironment : IBootstrapperEnvironment
    {
        private readonly List<string> _probingPaths;

        public BootstrapperEnvironment()
        {
            _probingPaths = new List<string>();
        }

        public void AddProbingPath(string path)
        {
            path = Path.GetFullPath(path).ToLowerInvariant();
            lock (_probingPaths)
            {
                if (!_probingPaths.Contains(path))
                {
                    _probingPaths.Add(path);
                }
            }
        }

        public string FindFile(string searchPattern)
        {
            List<string> probingPaths = GetProbingPathsCopy();
            foreach (string probingPath in probingPaths)
            {
                string[] files = Directory.GetFiles(probingPath, searchPattern);
                if (files.Length > 0)
                {
                    return files[0];
                }
            }
            return string.Empty;
        }

        public IEnumerable<string> FindFiles(string searchPattern)
        {
            List<string> probingPaths = GetProbingPathsCopy();
            List<string> files = new List<string>();
            foreach (string probingPath in probingPaths)
            {
                files.AddRange(Directory.GetFiles(probingPath, searchPattern));
            }
            return files;
        }

        private List<string> GetProbingPathsCopy()
        {
            List<string> probingPaths;
            lock (_probingPaths)
            {
                probingPaths = new List<string>(_probingPaths);
            }
            return probingPaths;
        }

    }
}
