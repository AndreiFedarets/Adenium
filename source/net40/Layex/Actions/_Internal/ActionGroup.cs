using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Layex.Actions
{
    internal sealed class ActionGroup : ReadOnlyCollection<IActionMetadata>, IActionGroup
    {
        private readonly List<IActionGroup> _underyingGroups;

        public ActionGroup(string id)
            : base(new ObservableCollection<IActionMetadata>())
        {
            Id = id;
        }

        public string Id { get; private set; }

        public string DisplayName
        {
            get 
            {
                foreach (IActionMetadata metadata in Items)
                {
                    string displayName = metadata.DisplayName;
                    if (!string.IsNullOrWhiteSpace(displayName))
                    {
                        return displayName;
                    }
                }
                return string.Empty;
            }
        }

        public bool Available
        {
            get { return Items.Any(x => x.Available); }
        }
    }
}
