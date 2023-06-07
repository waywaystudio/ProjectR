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

public interface IEditable<in T>
{
#if UNITY_EDITOR
    /// <summary>
    /// DO NOT Use in build!
    /// This Function is only for Editor Mode. 
    /// </summary>
    void EditorReceive(T data);
#endif
}
