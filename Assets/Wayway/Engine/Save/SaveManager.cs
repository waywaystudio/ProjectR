using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Singleton;
// ReSharper disable NotAccessedField.Local

#pragma warning disable CS0414

namespace Wayway.Engine.Save
{
    // SaveManager;
    // SaveEvent, LoadEvent; : GameEvent;
    // SaveEventListener;
    // ISavable (for inherit to Class);
    public class SaveManager : MonoSingleton<SaveManager>
    {
        [FolderPath] 
        [SerializeField] private string defaultSavePath = 
#if UNITY_EDITOR
                "Project/Data/Save";
#else
                Application.persistentDataPath;
#endif
        [SerializeField] private List<Savable> savableList;

        private string current;
        private const string DefaultSaveFileName = "Save";
        private const string SaveFileExtension = "es3";

        public static void Save()
        {
            
        }

        public static void Load()
        {
            
        }

        public void SaveOriginalToDest(string original, string dest)
        {
            
        }

        public void SaveCurrentToDest(string dest)
        {
            
        }

        private string GetSaveFileName(int index)
        {
            return $"{DefaultSaveFileName}:{index:###}.{SaveFileExtension}";
        }

        private string GetSaveFileFullPath(string saveName)
        {
            return $"{defaultSavePath}/{saveName}.{SaveFileExtension}";
        }
    }
}
