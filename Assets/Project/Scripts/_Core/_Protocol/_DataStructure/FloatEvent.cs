public class FloatEvent : Observable<float>
{
    public FloatEvent() : this(0f) { }
    public FloatEvent(float initialValue)
    {
        value = initialValue;
    }
}
