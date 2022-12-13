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
        /// <param>Unique Custom key for find in Tables
        ///     <name>key</name>
        /// </param>
        public void UnRegister(int key) => ContainsKey(key).OnTrue(() => Remove(key));
    }

    public class ActionTable : DelegateTable<Action>
    {
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}
        
        /// <summary>
        /// Register Delegate by custom Key (like as TryAdd())
        /// </summary>
        /// <param>Unique Custom key for Add in Table (InstanceID Recommend)
        ///     <name>key</name>
        /// </param>
        /// <param>Custom Action
        ///     <name>action</name>
        /// </param>
        public void Register(int key, Action action) => TryAdd(key, action);
        
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
        /// Register Delegate by custom Key (like as TryAdd())
        /// </summary>
        /// <param>Unique Custom key for Add in Table (InstanceID Recommend)
        ///     <name>key</name>
        /// </param>
        /// <param>Custom Action
        ///     <name>action</name>
        /// </param>
        public void Register(int key, Action action) => TryAdd(key, _ => action());
        public void Register(int key, Action<T0> action) => TryAdd(key, action);
        
        public void Invoke(T0 t) => this.ForEach(x => x.Value?.Invoke(t));
    }

    public class ActionTable<T0, T1> : DelegateTable<Action<T0, T1>>
    { 
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}
        
        /// <summary>
        /// Register Delegate by custom Key (like as TryAdd())
        /// </summary>
        /// <param>Unique Custom key for Add in Table (InstanceID Recommend)
        ///     <name>key</name>
        /// </param>
        /// <param>Custom Action
        ///     <name>action</name>
        /// </param>
        public void Register(int key, Action action) => TryAdd(key, (_, _) => action?.Invoke());
        public void Register(int key, Action<T0> action) => TryAdd(key, (t0, _) => action?.Invoke(t0));
        public void Register(int key, Action<T1> action) => TryAdd(key, (_, t1) => action?.Invoke(t1));
        public void Register(int key, Action<T0, T1> action) => TryAdd(key, action);
        
        public void Invoke(T0 t0, T1 t1) => this.ForEach(x => x.Value?.Invoke(t0, t1));
    }
}
