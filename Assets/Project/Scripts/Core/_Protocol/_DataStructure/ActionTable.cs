using System;
using System.Collections.Generic;

namespace Core
{
    public abstract class ActionTableCore<T> : Dictionary<string, T>
    {
        protected ActionTableCore() { }
        protected ActionTableCore(int capacity) : base(capacity) { }

        public void Unregister(string key) => this.TryRemove(key);

        protected void TryRegister(string key, T value) => TryAdd(key, value);
    }

    public class ActionTable : ActionTableCore<Action>
    {
        public ActionTable() { }
        public ActionTable(int capacity) : base(capacity) { }

        public void Register(string key, Action action) => TryRegister(key, action);
        
        public void Invoke() => this.ForEach(x => x.Value?.Invoke());
    }
    
    public class ActionTable<T> : ActionTableCore<Action<T>>
    {
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}

        public void Register(string key, Action action) => TryRegister(key, _ => action());
        public void Register(string key, Action<T> action) => TryRegister(key, action);
        
        public void Invoke(T value) => this.ForEach(x => x.Value?.Invoke(value));
    }
    
    public class ActionTable<T0, T1> : ActionTableCore<Action<T0, T1>>
    {
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}

        public void Register(string key, Action action) => TryRegister(key, (_, _) => action());
        public void Register(string key, Action<T0> action) => TryRegister(key, (x, _) => action(x));
        public void Register(string key, Action<T0, T1> action) => TryRegister(key, action);

        public void Invoke(T0 value0, T1 value1) => this.ForEach(x => x.Value?.Invoke(value0, value1));
    }
}
