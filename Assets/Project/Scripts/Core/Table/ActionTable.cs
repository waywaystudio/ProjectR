using System;

namespace Core
{
    public class ActionTable : DelegateTable<int, Action>
    {
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}

        /// <summary>
        /// Register Delegate by custom Key. Recommend InstanceID or Data Unique ID.
        /// </summary>
        public void Register(int key, Action action, bool overwrite = false) => TryAdd(key, action, overwrite);

        public void Invoke() => this.ForEach(x => x.Value?.Invoke());
    }
    
    public class ActionTable<T0> : DelegateTable<int, Action<T0>>
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

        public void Invoke(T0 t) => this.ForEach(x => x.Value.Invoke(t));
    }

    public class ActionTable<T0, T1> : DelegateTable<int, Action<T0, T1>>
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
