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
    public virtual void AddListener(string key, Action<T> action) => OnValueChanged.Register(key, action);

    public void RemoveListener(Action action) => RemoveListener(action.ToString());
    public void RemoveListener(string key) => OnValueChanged.Unregister(key);
}

public class FloatEvent : Observable<float>
{
    private const float Tolerance = 0.0000001f;
        
    protected float Min = float.NegativeInfinity;
    protected float Max = float.PositiveInfinity;

    public override float Value
    {
        get => value;
        set
        {
            if (Math.Abs(this.value - value) < Tolerance) return;
                
            var clampedValue = Mathf.Clamp(value, Min, Max);
            this.value = clampedValue;
                
            OnValueChanged.Invoke(this.value);
        }
    }

    public FloatEvent() : this(float.NegativeInfinity, float.PositiveInfinity) { }
    public FloatEvent(float coolTime) : this(float.NegativeInfinity, coolTime) { }
    public FloatEvent(float min, float max)
    {
        value = 0f;
        SetClamp(min, max);
    }

    public void SetClamp(float min, float max)
    {
        Min = min;
        Max = max;
    }
}