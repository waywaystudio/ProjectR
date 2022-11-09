namespace Main.Input
{
    public interface IEventModel
    {
        void Register();
        void Unregister();
        void Invoke();
    }
}
