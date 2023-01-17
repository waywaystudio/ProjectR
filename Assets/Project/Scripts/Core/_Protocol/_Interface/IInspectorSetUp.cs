namespace Core
{
    public interface IInspectorSetUp
    {
#if UNITY_EDITOR
        void SetUp();
#endif
    }
}