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

        public virtual void Register(Action action) => Register(action.ToString(), _ => action());
        public virtual void Register(string key, Action action) => Register(key, _ => action());
        public virtual void Register(Action<T> action) => Register(action.ToString(), action);
        public virtual void Register(string key, Action<T> action)
        {
            OnValueChanged.Register(key, action);
        }

        public void Unregister(Action action) => Unregister(action.ToString());
        public void Unregister(string key) => OnValueChanged.Unregister(key);
    }

    public class FloatEvent : Observable<float>
    {
        protected float Min = float.NegativeInfinity;
        protected float Max = float.PositiveInfinity;

        public override float Value
        {
            get => value;
            set
            {
                if (Math.Abs(this.value - value) < 0.000001f) return;
                
                var clampedValue = Mathf.Clamp(value, Min, Max);
                this.value = clampedValue;
                
                OnValueChanged.Invoke(clampedValue);
            }
        }

        public FloatEvent() { }
        public FloatEvent(float min, float max) { SetClamp(min, max);}

        public void SetClamp(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}
