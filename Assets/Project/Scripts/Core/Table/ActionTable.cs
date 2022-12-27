using System;
using System.Collections.Generic;

namespace Core
{
    public class DelegateTable<T> : Dictionary<int, T> where T : Delegate
    {
        protected DelegateTable(){}
        protected DelegateTable(int capacity) : base(capacity) {}

        /// <summary>
        /// Unregister Delegate by custom Key (like as Remove())
        /// </summary>
        public void Unregister(int key) => ContainsKey(key).OnTrue(() => Remove(key));
        public void UnregisterAll() => Clear();

        protected void TryAdd(int key, T value, bool overwrite)
        {
            if (!TryAdd(key, value) && overwrite) this[key] = value;
        }
    }

    public class ActionTable : DelegateTable<Action>
    {
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}

        /// <summary>
        /// Register Delegate by custom Key. Recommend InstanceID or Data Unique ID.
        /// </summary>
        public void Register(int key, Action action, bool overwrite = false) => TryAdd(key, action, overwrite);

        /// <summary>
        /// Invoke All of Table Action
        /// </summary>
        public void Invoke() => this.ForEach(x => x.Value?.Invoke());
    }
    
    public class ActionTable<T0> : DelegateTable<Action<T0>>
    {
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}

        /// <summary>
        /// Register None-Parameter Action. auto generate lambda expression.
        /// </summary>
        public void Register(int key, Action action, bool overwrite = false) => TryAdd(key, _ => action());
        
        /// <summary>
        /// Register Delegate by custom Key. Recommend InstanceID or Data Unique ID.
        /// </summary>
        public void Register(int key, Action<T0> action, bool overwrite = false) => TryAdd(key, action, overwrite);

        public void Invoke(T0 t)
        {
            this.ForEach(x =>
            {
                x.Value.Invoke(t);
            });
        }
    }

    public class ActionTable<T0, T1> : DelegateTable<Action<T0, T1>>
    { 
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}

        /// <summary>
        /// Register None-Parameter Action. auto generate lambda expression.
        /// </summary>
        public void Register(int key, Action action, bool overwrite = false) => TryAdd(key, (_, _) => action(), overwrite);
        
        /// <summary>
        /// Register Former Parameter Action. auto generate lambda expression.
        /// </summary>
        public void Register(int key, Action<T0> action, bool overwrite = false) => TryAdd(key, (t0, _) => action(t0), overwrite);
        
        /// <summary>
        /// Register later Parameter Action. auto generate lambda expression.
        /// </summary>
        public void Register(int key, Action<T1> action, bool overwrite = false) => TryAdd(key, (_, t1) => action(t1), overwrite);

        /// <summary>
        /// Register Delegate by custom Key. Recommend InstanceID or Data Unique ID.
        /// </summary>
        public void Register(int key, Action<T0, T1> action, bool overwrite = false) => TryAdd(key, action, overwrite);

        public void Invoke(T0 t0, T1 t1) => this.ForEach(x => x.Value?.Invoke(t0, t1));
    }
}
