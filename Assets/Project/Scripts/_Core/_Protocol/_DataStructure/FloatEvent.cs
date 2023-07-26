using System;

public class FloatEvent : Observable<float>
{
    public FloatEvent() : this(0f) { }
    public FloatEvent(float initialValue)
    {
        value = initialValue;
    }
    
    public override float Value
    {
        get => value;
        set
        {
            if (Math.Abs(this.value - value) < 0.000001f)
            {
                this.value = value;
                return;
            }
            
            this.value = value;
            OnValueChanged.Invoke(value);
        }
    }
}
