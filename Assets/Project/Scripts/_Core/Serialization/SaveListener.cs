using System.Collections.Generic;
using UnityEngine;

namespace Serialization
{
    public class SaveListener : MonoBehaviour
    {
        [SerializeField] private SaveManager saveManager;
        
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
            saveManager.AddListener(this);
        }
        
        private void OnDisable()
        {
            Save();
            saveManager.RemoveListener(this);
        }

        private void Reset()
        {
#if UNITY_EDITOR
            Finder.TryGetObject(out saveManager);
#endif
        }
    }
}
