using UnityEngine;

public abstract class UniqueScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        instance = null;
        Debug.Log("Runtime Initialize On");
    }
    
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance is not null) return instance;

            var typeObjects = Resources.FindObjectsOfTypeAll<T>();

            switch (typeObjects.Length)
            {
                case 0 :
                {
                    Debug.LogError($"Not Exist {typeof(T)} ScriptableObject in Resources Folder"
                                   + "UniqueScriptableObject does not Create Instance. Return null");
                    return null;
                }
                
                case 1 : instance = typeObjects[0];
                    break;

                default:
                {
                    var sb = new System.Text.StringBuilder();

                    typeObjects.ForEach(uso => sb.Append($"{UnityEditor.AssetDatabase.GetAssetPath(uso)}\n"));
                    
                    Debug.LogError($"More than 1 {typeof(T)} ScriptableObjects in Resources Folder"
                                   + $"Path List of multiple {typeof(T)} : {sb}");

                    return null;
                }
            }

            return instance;
        }
    }
}
