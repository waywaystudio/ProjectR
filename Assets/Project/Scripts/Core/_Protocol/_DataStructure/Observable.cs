using System;
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
                OnValueChanged.Invoke(value);
            }
        }

        public virtual void Register(string key, Action action) => Register(key, _ => action());
        public virtual void Register(string key, Action<T> action)
        {
            OnValueChanged.Register(key, action);
        }

        public void Unregister(string key) => OnValueChanged.Unregister(key);
    }

    public class FloatEvent : Observable<float>
    {
        private float min, max;

        public override float Value
        {
            get => value;
            set
            {
                var clampedValue = Mathf.Clamp(value, min, max);
                
                if (Math.Abs(this.value - clampedValue) < 0.0001f) return;
                
                this.value = clampedValue;
                OnValueChanged.Invoke(clampedValue);
            }
        }

        public FloatEvent() { }
        public FloatEvent(float min, float max) { SetClamp(min, max);}

        public void SetClamp(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
