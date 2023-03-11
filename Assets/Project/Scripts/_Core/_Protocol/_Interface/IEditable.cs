public interface IEditable
{
#if UNITY_EDITOR
    void EditorSetUp();
#endif
}