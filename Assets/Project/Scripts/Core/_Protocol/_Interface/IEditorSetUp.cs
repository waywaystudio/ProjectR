namespace Core
{
    public interface IEditorSetUp
    {
#if UNITY_EDITOR
        void SetUp();
#endif
    }
}