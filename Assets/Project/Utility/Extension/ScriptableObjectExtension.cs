#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wayway.Engine;

namespace ScriptExtension
{
    public static class ScriptableObjectExtension
    {
        /// <summary>
        /// [Editor Function] Get ScriptableObject.asset Object;
        /// </summary>
        /// <param name="so"></param>
        /// <typeparam name="T">inherited class type</typeparam>
        /// <returns>T :: ScriptableObject</returns>
        public static T GetAsset<T>(this ScriptableObject so) where T : ScriptableObject => GetAsset<T>(so, "");
        
        /// <summary>
        /// [Editor Function] Get ScriptableObject.asset Object;
        /// </summary>
        /// <param name="so"></param>
        /// <param name="folderPath">searching path</param>
        /// <typeparam name="T">inherited class type</typeparam>
        /// <returns>T :: ScriptableObject</returns>
        public static T GetAsset<T>(this ScriptableObject so, string folderPath) where T : ScriptableObject 
            => GetAsset<T>(so, folderPath, typeof(T).Name);
        
        /// <summary>
        /// [Editor Function] Get ScriptableObject.asset Object;
        /// </summary>
        /// <param name="so"></param>
        /// <param name="folderPath">searching path</param>
        /// <param name="className">target object class Name</param>
        /// <typeparam name="T">inherited class type</typeparam>
        /// <returns>T :: ScriptableObject</returns>
        public static T GetAsset<T>(this ScriptableObject so, string folderPath, string className) where T : ScriptableObject
        {
            folderPath = string.IsNullOrEmpty(folderPath) ? "Assets" : folderPath;
            className = $"t:{typeof(T).FullName}, {className}";

            var gUIDs = AssetDatabase.FindAssets(className, new [] { folderPath });
            if (gUIDs.Length == 0)
            {
                Debug.LogWarning($"Can't Find {className} class in {folderPath}");
                return null;
            }
            
            var targetIndex = 0;
            
            if (gUIDs.Length > 1)
            {
                for (var i = 0; i < gUIDs.Length; ++i)
                {
                    var assetName = AssetDatabase.GUIDToAssetPath(gUIDs[i])
                                                      .Replace(folderPath, "")
                                                      .Replace(".asset", "");

                    if (assetName != className) continue;
                    targetIndex = i;
                        
                    Debug.LogWarning($"Multiple Found :: count : {gUIDs.Length} keyWord : {className} \n " +
                                     $"Select Asset by Exactly SameName :: {assetName}");
                }
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(gUIDs[targetIndex]);
            var result = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
            
            return result;
        }

        public static List<T> GetAssetList<T>(this ScriptableObject so, string folderPath) where T : ScriptableObject =>
            GetAssetList<T>(so, folderPath, "");
        
        /// <summary>
        /// [Editor Function] Get ScriptableObject.asset Objects
        /// </summary>
        /// <param name="so"></param>
        /// <param name="folderPath">searching path</param>
        /// <param name="className">target object class Name</param>
        /// <typeparam name="T">inherited class type</typeparam>
        /// <returns>T type ScriptableObjects</returns>
        public static List<T> GetAssetList<T>(this ScriptableObject so, string folderPath, string className)
            where T : ScriptableObject
        {
            var result = new List<T>();

            folderPath = string.IsNullOrEmpty(folderPath) ? "Assets" : folderPath;
            className = $"t:{typeof(T).FullName}, {className}";

            var gUIDs = AssetDatabase.FindAssets(className, new [] { folderPath });

            gUIDs.ForEach(x =>
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(x);
                var data = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;

                if (!result.Contains(data))
                    result.Add(data);
            });

            return result;
        }
    }
}
#endif
