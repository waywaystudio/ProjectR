using System;

namespace Core
{
    public static class DelegateExtension
    {
        public static void AddUniquely(this Action action, Action newDelegate)
        {
            /* GPT Recommend 
            if (action == null || newDelegate == null)
            {
                return;
            }
            
            var delegates = action.GetInvocationList();
            
            if (delegates.Any(del => del == (Delegate)newDelegate))
            {
                return;
            } */
            
            action -= newDelegate;
            action += newDelegate;
        }
    }
}
