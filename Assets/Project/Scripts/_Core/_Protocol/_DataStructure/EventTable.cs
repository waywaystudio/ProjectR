using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventTable
{
    [SerializeField] private UnityEvent persistantEvent;

    public ActionTable ActionTable { get; } = new();

    public void Invoke()
    {
        ActionTable.Invoke();
        persistantEvent?.Invoke();
    }

    public void Add(string key, Action action) => ActionTable.Register(key, action);
    public void Remove(string key) => ActionTable.Remove(key);
}

// [Serializable]
// public class EventTable<T> : ActionTable<T>
// {
//     [SerializeField] private UnityEvent<T> persistantEvent;
//     
//     public override void Invoke(T value)
//     {
//         base.Invoke(value);
//         
//         persistantEvent?.Invoke(value);
//     }
// }
//
// [Serializable]
// public class EventTable<T0, T1> : ActionTable<T0, T1>
// {
//     [SerializeField] private UnityEvent<T0, T1> persistantEvent;
//     
//     public override void Invoke(T0 value1, T1 value2)
//     {
//         base.Invoke(value1, value2);
//         
//         persistantEvent?.Invoke(value1, value2);
//     }
// }
