namespace Core
{
    public interface IEventModel : IInvokable
    {
        // + Invoke();
        void Register();
        void Unregister();
    }
}
