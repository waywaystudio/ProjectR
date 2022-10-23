using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Singleton;

namespace Wayway.Engine.Save
{
    // SaveManager;
    // SaveEvent, LoadEvent; : GameEvent;
    // SaveEventListener;
    // ISavable (for inherit to Class);
    public class SaveManager : MonoSingleton<SaveManager>
    {
        [FolderPath] 
        [SerializeField] private string defaultSavePath = "Project/Data/Save";
        [SerializeField] private List<Savable> savableList;

#if UNITY_EDITOR
        [FolderPath]
        [SerializeField] private string dataPath = "Assets/Project/Data/Event/Serialization/";

        public static string DataPath => Instance.dataPath;
#endif
    }
}
