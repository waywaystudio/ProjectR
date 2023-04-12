using System.Collections.Generic;
using Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lobby.UI
{
    public class SaveLoadFrame : MonoBehaviour
    {
        [SerializeField] private GameObject saveInfoBoxPrefab;
        [SerializeField] private Transform saveInfoElementsHierarchy;

        [ShowInInspector]
        private List<SaveInfoBox> infoBoxList = new();

        [ShowInInspector]
        public void Reload()
        {
            infoBoxList.RemoveAll(box => box == null);
            
            var saveFileList = SaveManager.SaveInfoList;

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
            var saveFileList = SaveManager.SaveInfoList;
            
            saveFileList.ForEach(CreateNewSaveInfoBox);
        }
    }
}
