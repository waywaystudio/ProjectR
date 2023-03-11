using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor.Shortcut
{
    public static class QuickMenu
    {
        private const string SceneDirectory = "Assets/Project/Scenes/";
        private const string SceneExtension = "unity";
        
        [MenuItem("Quick Menu/Scene/Town")]
        private static void ToTown()
        {
            SceneMoveInEditor("Town");
        }
        
        [MenuItem("Quick Menu/Scene/Raid")]
        private static void ToRaid()
        {
            SceneMoveInEditor("Raid");
        }

        private static void SceneMoveInEditor(string sceneName)
        {
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.OpenScene($"{SceneDirectory}{sceneName}.{SceneExtension}");
        }
    }
}
