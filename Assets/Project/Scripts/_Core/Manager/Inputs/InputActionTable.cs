using System;
using UnityEngine.InputSystem;

namespace Inputs
{
    using Context = InputAction.CallbackContext;
    
    public class InputActionTable
    {
        private ConditionTable Condition { get; set; } = new();
        private ActionTable<Context> ContextAction { get; } = new();

        public void AddCondition(string key, Func<bool> condition) => Condition.Add(key, condition);
        public void RemoveCondition(string key) => Condition.Remove(key);
        public void Add(string key, Action<Context> action) => ContextAction.Add(key, action);
        public void Add(string key, Action action) => ContextAction.Add(key, action);
        public void Remove(string key) => ContextAction.Remove(key);

        /// <summary>
        /// Do Not use this method Directly. Designed for Input Interaction.
        /// </summary>
        public void Invoke(Context context)
        {
            if (Condition.IsAllTrue)
            {
                ContextAction.Invoke(context);
            }
        }
    }
}
