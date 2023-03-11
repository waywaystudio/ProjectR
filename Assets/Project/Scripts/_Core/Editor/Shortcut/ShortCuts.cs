using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Editor.Shortcut
{
    public static class ShortCuts
    {
        private const string PrefabPath = "Assets/Project/Prefabs";
        private const string AdventurerPrefabFolder = "Adventurer";
        private const string SkillPrefabFolder = "Skill";
        
        // Alt + c
        [Shortcut("Clear Console", KeyCode.C, ShortcutModifiers.Alt)]
        public static void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");

            if (method != null) 
                method.Invoke(new object(), null);
        }

        [Shortcut("HalfWork", KeyCode.F11)]
        public static void HalfWorkLayout()
        {
            EditorApplication.ExecuteMenuItem("Window/Layouts/HalfWork");
        }
        
        [Shortcut("FullWork", KeyCode.F12)]
        public static void FullWorkLayout()
        {
            EditorApplication.ExecuteMenuItem("Window/Layouts/FullWork");
        }
        
        [MenuItem("Quick Menu/Prefab/Adventurer")]
        public static void AdventurerPrefabs()
        {
            var guids = AssetDatabase.FindAssets($"t:Folder, {AdventurerPrefabFolder}", new []{PrefabPath});
            
            foreach (var guid in guids)
            {
                var path         = AssetDatabase.GUIDToAssetPath(guid);
                var prefabFolder = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                Selection.activeObject = prefabFolder;
                EditorGUIUtility.PingObject(prefabFolder);
                break;
            }
        }
        
        [MenuItem("Quick Menu/Prefab/Skill")]
        public static void SkillPrefabs()
        {
            var guids = AssetDatabase.FindAssets($"t:Folder, {SkillPrefabFolder}", new []{PrefabPath});
            
            foreach (var guid in guids)
            {
                var path         = AssetDatabase.GUIDToAssetPath(guid);
                var prefabFolder = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                Selection.activeObject = prefabFolder;
                EditorGUIUtility.PingObject(prefabFolder);
                break;
            }
        }
    }
}


