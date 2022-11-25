using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Shortcut
{
    public static class ShortCuts
    {
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
    }
}


