public interface IEditable
{
#if UNITY_EDITOR
    /// <summary>
    /// DO NOT Use in build!
    /// This Function is only for Editor Mode. 
    /// </summary>
    void EditorSetUp();
#endif
}