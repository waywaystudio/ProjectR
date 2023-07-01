using System;
using UnityEngine;

[Serializable]
public class Observable<T>
{
    [SerializeField] protected T value;

    protected ActionTable<T> OnValueChanged { get; } = new();

    public virtual T Value
    {
        get => value;
        set
        {
            this.value = value;
            OnValueChanged.Invoke(value);
        }
    }

    public virtual void AddListener(Action action) => AddListener(action.ToString(), _ => action());
    public virtual void AddListener(string key, Action action) => AddListener(key, _ => action());
    public virtual void AddListener(Action<T> action) => AddListener(action.ToString(), action);
    public virtual void AddListener(string key, Action<T> action) => OnValueChanged.Add(key, action);

    public void RemoveListener(Action action) => RemoveListener(action.ToString());
    public void RemoveListener(string key) => OnValueChanged.Remove(key);
}

