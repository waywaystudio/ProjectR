using System;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class FunctionTable<TResult> : DelegateTable<int, Func<TResult>>
    {
        public FunctionTable(){}
        public FunctionTable(int capacity) : base(capacity) {}

        /// <summary>
        /// Register Delegate by custom Key. Recommend InstanceID or Data Unique ID.
        /// </summary>
        public void Register(int key, Func<TResult> function)
        {
            if (Count < 1) TryAdd(key, function);
            else Debug.LogError("FunctionTable able to storage just 1 function.");
        }
        
        public TResult Invoke() => this.FirstOrDefault().Value.Invoke();
    }
   
    public class FunctionTable<T0, TResult> : DelegateTable<int, Func<T0, TResult>>
    {
        public FunctionTable(){}
        public FunctionTable(int capacity) : base(capacity) {}

        public void Register(int key, Func<T0, TResult> function)
        {
            if (Count < 1) TryAdd(key, function);
            else Debug.LogError("FunctionTable able to storage just 1 function.");
        }
        
        public TResult Invoke(T0 t) => this.FirstOrDefault().Value.Invoke(t);
    }
        
    public class FunctionTable<T1, T2, TResult> : DelegateTable<int, Func<T1, T2, TResult>>
    {
        public FunctionTable(){}
        public FunctionTable(int capacity) : base(capacity) {}

        public void Register(int key, Func<T1, T2, TResult> function)
        {
            if (Count < 1) TryAdd(key, function);
            else Debug.LogError("FunctionTable able to storage just 1 function.");
        }
        
        public TResult Invoke(T1 t1, T2 t2) => this.FirstOrDefault().Value.Invoke(t1, t2);
    }
}