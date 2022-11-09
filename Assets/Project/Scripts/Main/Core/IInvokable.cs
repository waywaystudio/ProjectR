namespace Main
{
    public interface IInvokable
    {
        void Invoke();
    }

    public interface IInvokable<in T>
    {
        void Invoke(T value);
    }
    
    public interface IInvokable<in T0, in T1>
    {
        void Invoke(T0 value1, T1 value2);
    }
}
