using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Core
{
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
                OnValueChanged?.Invoke(value);
            }
        }

        public void Register(int key, Action<T> action) => OnValueChanged.Register(key, action);
        public void Unregister(int key) => OnValueChanged.Unregister(key);
    }

    public class ObservableFloat : Observable<float>
    {
        public ObservableFloat() : this(float.MinValue, float.MaxValue) {}
        public ObservableFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        
        private readonly float min;
        private readonly float max;
        
        public override float Value
        {
            get => value;
            set
            {
                if (value < min) this.value = min;
                if (value > max) this.value = max;
                
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        
        public static ObservableFloat operator +(ObservableFloat x, float y)
        {
            x.value += y;
            return x;
        }
    }
}
