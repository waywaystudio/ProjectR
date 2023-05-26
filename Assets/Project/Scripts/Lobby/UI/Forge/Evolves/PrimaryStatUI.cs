using System.Collections.Generic;
using Common;
using Common.Equipments;
using Common.PartyCamps;
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
            
            var adventurer             = LobbyDirector.UI.Forge.FocusAdventurer;
            var slot                   = LobbyDirector.UI.Forge.FocusSlot;
            var currentEquipmentEntity = PartyCamp.Characters.GetAdventurerEquipment(adventurer, slot);
            var tier                   = currentEquipmentEntity.Tier;
            
            ReloadStat(currentEquipmentEntity);
            ReloadMaterial(tier);
        }
        

        private void ReloadStat(EquipmentEntity equipEntity)
        {
            var equipmentSpec = equipEntity.ConstStatSpec + equipEntity.UpgradeStatSpec;

            equipmentSpec?.IterateOverStats((stat, index) =>
            {
                if (constStatUIList.Count < index) return;
                
                constStatUIList[index].gameObject.SetActive(true);
                constStatUIList[index].SetValue(stat);
            });
        }

        private void ReloadMaterial(int tier)
        {
            if (materialUIList.Count < 1 || tier == 0)
            {
                return;
            }

            var profitMaterialType = tier switch
            {
                1 => MaterialType.ViciousShard,
                2 => MaterialType.ViciousStone,
                3 => MaterialType.ViciousCrystal,
                _ => MaterialType.None,
            };

            materialUIList[0].gameObject.SetActive(true);
            materialUIList[0].SetInfoUI(profitMaterialType, "5");
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
