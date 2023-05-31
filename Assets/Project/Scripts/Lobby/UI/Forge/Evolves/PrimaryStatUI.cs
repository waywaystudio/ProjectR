using System.Collections.Generic;
using Common;
using Common.Equipments;
using Common.UI;
using UnityEngine;

namespace Lobby.UI.Forge.Evolves
{
    public class PrimaryStatUI : MonoBehaviour, IEditable
    {
        [SerializeField] private List<StatInfoUI> constStatUIList;
        [SerializeField] private List<MaterialInfoUI> materialUIList;

        public void OnReloadForge()
        {
            constStatUIList.ForEach(statUI => statUI.gameObject.activeSelf.OnTrue(() => statUI.gameObject.SetActive(false)));
            materialUIList.ForEach(materialUI => materialUI.gameObject.activeSelf.OnTrue(() => materialUI.gameObject.SetActive(false)));

            var currentEquipmentEntity = LobbyDirector.UI.Forge.FocusEquipment;
            
            ReloadStat(currentEquipmentEntity);
            ReloadMaterial(currentEquipmentEntity);
        }
        

        private void ReloadStat(EquipmentEntity equipEntity)
        {
            var equipmentSpec = equipEntity.ConstStatSpec;

            equipmentSpec?.IterateOverStats((stat, index) =>
            {
                if (constStatUIList.Count < index) return;
                
                constStatUIList[index].gameObject.SetActive(true);
                constStatUIList[index].SetValue(stat);
            });
        }

        private void ReloadMaterial(EquipmentEntity equipEntity)
        {
            if (materialUIList.Count < 1) return;

            // var focusRelic = LobbyDirector.UI.Forge.FocusRelic;
            // var materialList = EquipmentUtility.RequiredMaterialsForUpgrade(focusRelic, equipEntity.UpgradeLevel);
            
            // materialList.ForEach((ingredient, index) =>
            // {
            //     materialUIList[index].gameObject.SetActive(true);
            //     materialUIList[index].SetInfoUI(ingredient);
            // });
        }

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(constStatUIList);
            GetComponentsInChildren(materialUIList);
        }
#endif
    }
}
