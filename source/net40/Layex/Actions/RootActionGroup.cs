using System;

namespace Layex.Actions
{
    public sealed class RootActionGroup : ActionGroup
    {
        public override string DisplayName
        {
            get { return string.Empty; }
        }

        //public override ActionItem this[string fullName]
        //{
        //    get
        //    {
        //        const char namePathSeparator = '.';
        //        if (string.Equals(fullName, string.Empty, StringComparison.Ordinal))
        //        {
        //            return this;
        //        }
        //        if (!fullName.Contains(namePathSeparator.ToString()))
        //        {
        //            return base[fullName];
        //        }
        //        string[] actionPath = fullName.Split(namePathSeparator);
        //        ActionItem currentAction = this;
        //        foreach (string actionName in actionPath)
        //        {
        //            ActionGroup actionGroup = currentAction as ActionGroup;
        //            if (actionGroup == null)
        //            {
        //                return null;
        //            }
        //            currentAction = actionGroup[actionName];
        //        }
        //        return currentAction;
        //    }
        //}
    }
}
