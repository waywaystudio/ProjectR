#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

public static class Finder
{
    /// <summary>
    /// Find Object By object Name
    /// </summary>
    /// <returns></returns>
    public static bool TryGetObject<T>(out T value, bool showDebug = false) where T : Object =>
        TryGetObject("Assets", $"t:{typeof(T).Name}", out value, showDebug);
    
    /// <summary>
    /// Find Object By object Name
    /// </summary>
    /// <returns></returns>
    public static bool TryGetObject<T>(string fileName, out T value, bool showDebug = false) where T : Object =>
        TryGetObject("Assets", fileName, out value, showDebug);

    /// <summary>
    /// Find Object By object Name
    /// </summary>
    /// <returns></returns>
    public static bool TryGetObject<T>(string directory, string filter, out T value, bool showDebug = false) where T : Object
    {
        var gUIDs = AssetDatabase.FindAssets(filter, new [] { directory });
        
        switch (gUIDs.Length)
        {
            case 0:
            {
                if (showDebug)
                    Debug.LogError($"Can't Find in {filter} object in Project");

                value = null;
                return false;
            }
            case 1:
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(gUIDs[0]);
                value = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;

                return true;
            }
            default:
            {
                var stringBuilder = new StringBuilder();
                var assetPath = string.Empty;
            
                foreach (var t in gUIDs)
                {
                    assetPath = AssetDatabase.GUIDToAssetPath(t);
                    stringBuilder.Append(assetPath);
                }
            
                if (showDebug)
                    Debug.Log($"{filter} Objects in Project List : " +
                              $"{stringBuilder}." +
                              "return First Searched Object");
            
                value = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
                return true;
            }
        }
    }

    /// <summary>
    /// Get Object List by Class Name
    /// </summary>
    /// <returns></returns>
    public static bool TryGetObjectList<T>(out List<T> value, bool showDebug = false) where T : Object
        => TryGetObjectList("Assets", $"t:{typeof(T).Name}", out value, showDebug);
    
    /// <summary>
    /// Get Object List by FileName
    /// </summary>
    /// <returns></returns>
    public static bool TryGetObjectList<T>(string fileName, out List<T> value, bool showDebug = false)
        where T : Object => TryGetObjectList("Assets", fileName, out value, showDebug);
    
    /// <summary>
    /// Get Object List by UnityFilter in Directory
    /// </summary>
    /// <returns></returns>
    public static bool TryGetObjectList<T>(string directory, string filter, out List<T> value, bool showDebug = false)
        where T : Object
    {
        var gUIDs = AssetDatabase.FindAssets(filter, new [] { directory });

        switch (gUIDs.Length)
        {
            case 0:
            {
                if (showDebug)
                    Debug.LogError($"Can't Find in {filter} object in Project");

                value = null;
                return false;
            }
            default:
            {
                var stringBuilder = new StringBuilder();

                value = new List<T>();
            
                foreach (var t in gUIDs)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(t);
                    
                    value.Add(AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T);
                    stringBuilder.Append(assetPath);
                    stringBuilder.AppendLine();
                }
            
                if (showDebug)
                    Debug.Log($"{filter} Objects in Project List : " +
                              $"{stringBuilder}");

                return true;
            }
        }
    }

    /// <summary>
    /// Find Object by FilePath
    /// </summary>
    /// <returns></returns>
    public static bool TryGetObjectByFilePath<T>(string filePath, out T value, bool showDebug = false) 
        where T : Object
    {
        var result = AssetDatabase.LoadAssetAtPath<T>(filePath);
        
        if (showDebug)
            Debug.Log($"FilePath : {filePath} isExist ? : {result is not null}");

        if (result is not null)
        {
            value = result;
            return true;
        }

        value = null;
        return false;
    }

    /// <summary>
    /// Create Prefab at specific Path
    /// </summary>
    /// <param name="originalPrefab">Original prefab</param>
    /// <param name="directoryPath">Set Directory ex.Assets/Project/Data </param>
    /// <param name="prefabName">Set PrefabName ex.MyPrefab</param>
    public static GameObject CreatePrefab(GameObject originalPrefab, string directoryPath, string prefabName)
    {
        var prefab = Object.Instantiate(originalPrefab);
        var result = PrefabUtility.SaveAsPrefabAsset(prefab, $"{directoryPath}/{prefabName}.prefab");
            
        Object.DestroyImmediate(prefab);

        return result;
    }
    
    /// <summary>
    /// Get Prefab Which Included Specific MonoBehaviour
    /// </summary>
    /// <param name="directory">searching target directory</param>
    /// <typeparam name="T">Only inherited MonoBehaviour class</typeparam>
    /// <returns>First Searched Prefab</returns>
    public static GameObject GetPrefabInclude<T>(string directory = "Assets/") where T : MonoBehaviour
    {
        var gUidList = AssetDatabase.FindAssets($"t:GameObject,", new[] {directory});

        return gUidList.Select(AssetDatabase.GUIDToAssetPath)
                       .Select(assetPath => AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject)
                       .FirstOrDefault(asset => asset != null && asset.GetComponent<T>() != null);
    }
    
    /// <summary>
    /// Get PrefabPath Which Included Specific MonoBehaviour 
    /// </summary>
    /// <param name="directory">searching target directory</param>
    /// <typeparam name="T">Only inherited MonoBehaviour class</typeparam>
    /// <returns>First Searched Prefab Path</returns>
    public static string GetPrefabPathInclude<T>(string directory = "Assets/") where T : MonoBehaviour
    {
        var prefab = GetPrefabInclude<T>();

        return AssetDatabase.GetAssetPath(prefab);
    }

    /// <summary>
    /// Create ScriptableObject at path
    /// </summary>
    public static T CreateScriptableObject<T>(string directory, string defaultName, bool showDebug = false) where T : ScriptableObject
    {
        if (directory.IsNullOrEmpty())
        {
            if (showDebug)
                Debug.LogError("at least one of a Parameter value is <b><color=red>NULL!</color></b>");
            
            return null;
        }
        
        if (string.IsNullOrEmpty(defaultName))
        {
            defaultName = typeof(T).Name;
            
            if (showDebug)
                Debug.Log($"name Created by automatically <b><color=green>{typeof(T).FullName}</color></b>");
        }

        var result = ScriptableObject.CreateInstance<T>();
        var uniquePath = AssetDatabase.GenerateUniqueAssetPath($"{directory}/{defaultName}.asset");

        AssetDatabase.CreateAsset(result, uniquePath);

        if (showDebug)
            Debug.Log($"Create <b><color=green>{uniquePath}</color></b> Scriptable Object");

        return result;
    }
    
    /// <summary>
    /// Create ScriptableObject at path
    /// </summary>
    public static ScriptableObject CreateScriptableObject(string directory, string className, string defaultName, bool showDebug = false)
    {
        if (directory.IsNullOrEmpty())
        {
            if (showDebug)
                Debug.LogError("at least one of a Parameter value is <b><color=red>NULL!</color></b>");
            
            return null;
        }
        
        if (string.IsNullOrEmpty(defaultName))
        {
            defaultName = "ScriptableObject";
            
            if (showDebug)
                Debug.Log($"name Created by automatically <b><color=green>{defaultName}</color></b>");
        }

        var result = ScriptableObject.CreateInstance(className);
        var uniquePath = AssetDatabase.GenerateUniqueAssetPath($"{directory}/{defaultName}.asset");

        AssetDatabase.CreateAsset(result, uniquePath);

        if (showDebug)
            Debug.Log($"Create <b><color=green>{uniquePath}</color></b> Scriptable Object");

        return result;
    }
}
#endif
