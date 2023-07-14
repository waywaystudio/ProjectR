using System;
using UnityEngine.InputSystem;

namespace Inputs
{
    using Context = InputAction.CallbackContext;
    
    public class InputActionTable
    {
        public ActionTable<Context> Start { get; } = new();
        public ActionTable<Context> Cancel { get; } = new();
        public ActionTable<Context> Perform { get; } = new();

        public void AddStart(string key, Action<Context> action) => Start.Add(key, action);
        public void AddStart(string key, Action action) => Start.Add(key, action);
        public void RemoveStart(string key) => Start.Remove(key);
        
        public void AddCancel(string key, Action<Context> action) => Cancel.Add(key, action);
        public void AddCancel(string key, Action action) => Cancel.Add(key, action);
        public void RemoveCancel(string key) => Cancel.Remove(key);

        public void AddPerform(string key, Action<Context> action) => Perform.Add(key, action);
        public void AddPerform(string key, Action action) => Perform.Add(key, action);
        public void RemovePerform(string key) => Perform.Remove(key);

        /// <summary>
        /// Do Not use this method Directly. Designed for Input Interaction.
        /// </summary>
        public void StartInvoke(Context context)=> Start.Invoke(context);
        
        /// <summary>
        /// Do Not use this method Directly. Designed for Input Interaction.
        /// </summary>
        public void CancelInvoke(Context context)=> Cancel.Invoke(context);
        
        /// <summary>
        /// Do Not use this method Directly. Designed for Input Interaction.
        /// </summary>
        public void PerformInvoke(Context context)=> Perform.Invoke(context);
    }
}
