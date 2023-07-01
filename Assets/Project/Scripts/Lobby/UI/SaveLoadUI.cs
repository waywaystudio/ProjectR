using System.Collections.Generic;
using Lobby.UI.SaveLoad;
using Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lobby.UI
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private GameObject saveInfoBoxPrefab;
        [SerializeField] private Transform saveInfoElementsHierarchy;

        [ShowInInspector]
        private List<SaveInfoBox> infoBoxList = new();

        [ShowInInspector]
        public void Reload()
        {
            infoBoxList.RemoveAll(box => box is null);
            
            var saveFileList = saveManager.SaveInfoList;

            saveFileList.ForEach((saveFile, index) => 
            {
                if (infoBoxList.Count > index) infoBoxList[index].Initialize(saveFile);
                else
                    CreateNewSaveInfoBox(saveFile);
            });
        }


        private void CreateNewSaveInfoBox(SaveInfo saveFileInformation)
        {
            var infoBox = Instantiate(saveInfoBoxPrefab, saveInfoElementsHierarchy);
            
            if (!infoBox.TryGetComponent(out SaveInfoBox saveInfoBehaviour))
            {
                Debug.LogError($"Not Exist {nameof(SaveInfoBox)} Script In {saveInfoBoxPrefab.name}");
                return;
            }
            
            saveInfoBehaviour.Initialize(saveFileInformation);
            infoBoxList.AddUniquely(saveInfoBehaviour);
        }
        
        private void Awake()
        {
            var saveFileList = saveManager.SaveInfoList;
            
            saveFileList.ForEach(CreateNewSaveInfoBox);
        }
    }
}
