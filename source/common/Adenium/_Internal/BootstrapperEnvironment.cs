using System;
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
            AddProbingPath(AppDomain.CurrentDomain.BaseDirectory);
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
            lock (_probingPaths)
            {
                foreach (string probingPath in _probingPaths)
                {
                    string[] files = Directory.GetFiles(probingPath, searchPattern, SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        return files[0];
                    }
                }
                return string.Empty;
            }
        }

        public IEnumerable<string> FindFiles(string searchPattern)
        {
            List<string> files = new List<string>();
            lock (_probingPaths)
            {
                foreach (string probingPath in _probingPaths)
                {
                    files.AddRange(Directory.GetFiles(probingPath, searchPattern, SearchOption.AllDirectories));
                }
            }
            return files;
        }
    }
}
