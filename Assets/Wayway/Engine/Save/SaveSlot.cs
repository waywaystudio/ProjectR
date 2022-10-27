using System;
using UnityEngine;

namespace Wayway.Engine.Save
{
    [Serializable]
    public class SaveSlot : ISavable
    {
        [SerializeField] private string saveName;
        [SerializeField] private int testInt;

        public string SaveName => saveName;
        public int TestInt => testInt;
        public void Save()
        {
            SaveManager.Save("SaveName", SaveName);
            SaveManager.Save("TestInt", TestInt);
        }

        public void Load()
        {
            saveName = SaveManager.Load<string>("SaveName");
            testInt = SaveManager.Load<int>("TestInt");
        }
    }
}
