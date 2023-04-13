using System.Collections.Generic;
using UnityEngine;

namespace Serialization
{
    public class SaveListener : MonoBehaviour
    {
        private readonly List<ISavable> savableList = new();

        private List<ISavable> SavableList
        {
            get
            {
                if (savableList.IsNullOrEmpty()) 
                    GetComponents(savableList);

                return savableList;
            }
        }

        public void Save() => SavableList.ForEach(savable => savable.Save());
        public void Load() => SavableList.ForEach(savable => savable.Load());

        private void OnEnable()
        {
            Load();
            SaveManager.Instance.AddListener(this);
        }
        
        private void OnDisable()
        {
            Save();
            SaveManager.Instance.RemoveListener(this);
        }
    }
}
