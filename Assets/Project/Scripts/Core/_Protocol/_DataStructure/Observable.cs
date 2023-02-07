using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class Observable<T>
    {
        [SerializeField] protected T value;

        protected OldActionTable<T> OnValueChanged { get; set; } = new();

        public virtual T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public void Register(int key, Action<T> action)
        {
            OnValueChanged ??= new OldActionTable<T>();
            OnValueChanged.Register(key, action);
        }
        public void Register(int key, Action action)
        {
            OnValueChanged ??= new OldActionTable<T>();
            OnValueChanged.Register(key, _ => action());
        }

        public void RegisterUniquely(Action<T> action) => Register(0, action);
        public void RegisterUniquely(Action action) => Register(0, action);
        
        public void Unregister(int key) => OnValueChanged?.Unregister(key);
    }
}
