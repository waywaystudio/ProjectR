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

        [MenuItem("Quick Menu/Assets/SaveManager")]
        public static void GetSaveManager() => GetResources("SaveManager");

        [MenuItem("Quick Menu/Prefab/Adventurer/Knight")]
        public static void GetKnightPrefab() => GetAdventurerPrefab("Knight");
        
        [MenuItem("Quick Menu/Prefab/Adventurer/Rogue")]
        public static void GetRoguePrefab() => GetAdventurerPrefab("Rogue");
        
        [MenuItem("Quick Menu/Prefab/Adventurer/Hunter")]
        public static void GetHunterPrefab() => GetAdventurerPrefab("Hunter");
        
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
        
        
        private static void GetAdventurerPrefab(string className)
        {
            var adventurerPrefabDirectory = $"{PrefabPath}/Character/{AdventurerPrefabFolder}";

            var gUIDs = AssetDatabase.FindAssets($"{className}", new[] { adventurerPrefabDirectory });

            foreach (var guid in gUIDs)
            {
                var path         = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                
                EditorUtility.OpenPropertyEditor(prefab);
                break;
            }
        }
        
        private static void GetResources(string resourceName)
        {
            var targetResource = Resources.Load(resourceName);
            
            EditorUtility.OpenPropertyEditor(targetResource);
        }
    }
}


