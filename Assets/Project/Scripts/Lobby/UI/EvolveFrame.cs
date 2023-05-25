using System;
using System.Collections.Generic;
using Common.Equipments;
using Common.PartyCamps;
using TMPro;
using UnityEngine;

namespace Lobby.UI
{
    public class EvolveFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI equipmentTypeText;
        [SerializeField] private List<RelicSelectUI> RelicSelectUIList;

        private EquipmentEntity CurrentEquipment { get; set; }


        public void Initialize()
        {
            var initialClass  = LobbyDirector.UI.Forge.FocusAdventurer;
            var equipmentSlot = LobbyDirector.UI.Forge.FocusSlot;
            
            CurrentEquipment       = PartyCamp.Characters.GetData(initialClass).GetEquipment(equipmentSlot);
            equipmentTypeText.text = equipmentSlot.ToString();
        }
        
        public void NextEquipmentSlot() => LobbyDirector.UI.Forge.FocusSlot = LobbyDirector.UI.Forge.FocusSlot.NextExceptNone();
        public void PrevEquipmentSlot() => LobbyDirector.UI.Forge.FocusSlot = LobbyDirector.UI.Forge.FocusSlot.PrevExceptNone();

        public void ChangeEquipmentSlot()
        {
            // Text Setting
            var currentText       = equipmentTypeText.text;
            var equipmentSlotText = LobbyDirector.UI.Forge.FocusSlot.ToString();

            if (string.Compare(currentText, equipmentSlotText, StringComparison.Ordinal) != 0)
            {
                equipmentTypeText.text = equipmentSlotText;
            }
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren<TextMeshProUGUI>().ForEach(txtComponent =>
            {
                if (txtComponent.gameObject.name == "EquipmentType")
                {
                    equipmentTypeText = txtComponent;
                }
            });

            GetComponentsInChildren(RelicSelectUIList);
        }
#endif
    }
}
