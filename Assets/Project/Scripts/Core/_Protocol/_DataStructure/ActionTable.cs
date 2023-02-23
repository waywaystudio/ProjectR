using System;
using System.Collections.Generic;

namespace Core
{
    public abstract class ActionTableCore<T> : Dictionary<string, T>
    {
        protected readonly List<string> OnceKeys = new();
        
        protected ActionTableCore() { }
        protected ActionTableCore(int capacity) : base(capacity) { }
        
        public void Unregister(string key) => this.TryRemove(key);

        protected void RemoveOnceActions()
        {
            if (OnceKeys.IsNullOrEmpty()) return;
            
            foreach (var onceKey in OnceKeys) Unregister(onceKey);
            OnceKeys.Clear();
        }
    }

    public class ActionTable : ActionTableCore<Action>
    {
        public ActionTable() { }
        public ActionTable(int capacity) : base(capacity) { }
        
        public void Register(string key, Action action) => TryAdd(key, action);

        public void Register(ActionTable otherTable)
        {
            foreach (var item in otherTable) TryAdd(item.Key, item.Value);
        }

        public void RegisterOnce(string key, Action action)
        {
            if (TryAdd(key, action)) OnceKeys.Add(key);
        }

        public void Invoke()  
        {
            foreach (var action in Values) action?.Invoke();

            RemoveOnceActions();
        }
    }
    
    public class ActionTable<T> : ActionTableCore<Action<T>>
    {
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}

        public void Register(string key, Action action) => TryAdd(key, _ => action());
        public void Register(string key, Action<T> action) => TryAdd(key, action);

        public void Register(ActionTable otherTable)
        {
            foreach (var item in otherTable) TryAdd(item.Key, _ => item.Value.Invoke());
        }

        public void Register(ActionTable<T> otherTable)
        {
            foreach (var item in otherTable) TryAdd(item.Key, item.Value.Invoke);
        } 
        
        public void RegisterOnce(string key, Action action)
        {
            if (TryAdd(key, _ => action())) OnceKeys.Add(key);
        }
        
        public void RegisterOnce(string key, Action<T> action)
        {
            if (TryAdd(key, action)) OnceKeys.Add(key);
        }
        

        public void Invoke(T value)
        {
            foreach (var action in Values) action?.Invoke(value);

            RemoveOnceActions();
        }
    }
    
    /* TODO. 가급적 ActionTable은 One Param에서 끝내보자. 
    public class ActionTable<T0, T1> : ActionTableCore<Action<T0, T1>>
    {
        public ActionTable(){}
        public ActionTable(int capacity) : base(capacity) {}
    
        public void Register(string key, Action action) => TryAdd(key, (_, _) => action());
        public void Register(string key, Action<T0> action) => TryAdd(key, (x, _) => action(x));
        public void Register(string key, Action<T0, T1> action) => TryAdd(key, action);
        
        public void Register(ActionTable otherTable) => 
            otherTable.ForEach(x => TryAdd(x.Key, (_, _) => x.Value.Invoke()));
        
        public void Register(ActionTable<T0> otherTable) => 
            otherTable.ForEach(x => TryAdd(x.Key, (t0, _) => x.Value.Invoke(t0)));
        
        public void Register(ActionTable<T0, T1> otherTable) => 
            otherTable.ForEach(x => TryAdd(x.Key, x.Value));
    
        public void Invoke(T0 value0, T1 value1)
        {
            foreach (var action in Values) action?.Invoke(value0, value1);
        }
    }
    */
}
