using System.Collections.Generic;
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
        [SerializeField] private List<Savable> savableList;
        // [SerializeField] private List<ES3File> saveFileList;
        // private ES3File currentSlot;

        public void Save()
        {
            
        }

        public void Load()
        {
            
        }
    }
}
