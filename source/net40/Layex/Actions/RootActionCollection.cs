using System;

namespace Layex.Actions
{
    public sealed class RootActionCollection : ActionCollectionBase
    {
        public override ActionBase this[string fullName]
        {
            get
            {
                const char namePathSeparator = '.';
                if (string.Equals(fullName, string.Empty, StringComparison.Ordinal))
                {
                    return this;
                }
                if (!fullName.Contains(namePathSeparator.ToString()))
                {
                    return base[fullName];
                }
                string[] actionPath = fullName.Split(namePathSeparator);
                ActionBase currentAction = this;
                foreach (string actionName in actionPath)
                {
                    ActionCollectionBase actionGroup = currentAction as ActionCollectionBase;
                    if (actionGroup == null)
                    {
                        return null;
                    }
                    currentAction = actionGroup[actionName];
                }
                return currentAction;
            }
        }
    }
}
