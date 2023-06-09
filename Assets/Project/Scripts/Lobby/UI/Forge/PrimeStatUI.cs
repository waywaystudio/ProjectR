using System.Collections.Generic;
using Common;
using Common.UI;
using UnityEngine;

namespace Lobby.UI.Forge
{
    public class PrimeStatUI : MonoBehaviour, IEditable
    {
        [SerializeField] private EquipmentSlotType slotType;
        [SerializeField] private List<StatInfoUI> constStatUIList;

        public void OnReloadForge()
        {
            constStatUIList.ForEach(statUI => statUI.gameObject.activeSelf.OnTrue(() => statUI.gameObject.SetActive(false)));

            var targetEquipment = slotType == EquipmentSlotType.Weapon
                ? LobbyDirector.Forge.VenturerWeapon()
                : LobbyDirector.Forge.VenturerArmor();
            
            ReloadStat(targetEquipment);
        }
        

        private void ReloadStat(IEquipment equipEntity)
        {
            var equipmentSpec = equipEntity.StatSpec;

            equipmentSpec?.IterateOverStats((stat, index) =>
            {
                if (constStatUIList.Count < index) return;
                
                constStatUIList[index].gameObject.SetActive(true);
                constStatUIList[index].SetValue(stat);
            });
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(constStatUIList);
        }
#endif
    }
}
