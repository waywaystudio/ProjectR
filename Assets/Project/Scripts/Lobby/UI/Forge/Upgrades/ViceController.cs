using System;
using System.Collections.Generic;
using Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Lobby.UI.Forge.Upgrades
{
    public class ViceController : MonoBehaviour, IEditable
    {
        [SerializeField] private EquipmentSlotType slotType;
        [SerializeField] private EnchantType enchantType;
        [SerializeField] private List<ViceBubble> viceBubbleList;
        [SerializeField] private TextMeshProUGUI viceTextMesh;
        [SerializeField] private TextMeshProUGUI valueTextMesh;


        [Button]
        public void OnEthosChanged()
        {
            // Enable & Disable
                // Tier, EnchantType에 따라서 12개의 버블 중 Active와 DeActive설정.
            var equipment = LobbyDirector.UI.Forge.VenturerEquipment(slotType);
            var tier = equipment.Tier;
            var maxBubble = GetMaxBubble(tier);
            
            viceBubbleList.ForEach((bubble, index) =>
            {
                if (maxBubble > index)
                {
                    bubble.Active();
                    bubble.Disable();
                }
                else
                    bubble.DeActive();
            });
            
            
            // Inventory에서 Vice Material 남는 값 받아오기.
            // max 값 잡아주기
            // Tier Max & material Max
            // max value 잡아주기
        }

        public void ChangeEthosValue(float barValue)
        {
            var equipment = LobbyDirector.UI.Forge.VenturerEquipment(slotType);
            var ethosEntity = equipment.GetEnchant(enchantType);
            
            ethosEntity.Value  = (int)barValue;
            valueTextMesh.text = ethosEntity.Value.ToRoman();
        }


        private int GetMaxBubble(int tier)
        {
            Debug.Log($"{enchantType}, {tier}");
            
            return enchantType switch
            {
                EnchantType.None => 0,
                EnchantType.PrimeEnchant => tier switch
                {
                    1 => 6,
                    2 => 9,
                    3 => 12,
                    _ => 0
                },
                EnchantType.SubEnchant => tier switch
                {
                    2 => 3,
                    3 => 6,
                    _ => 0
                },
                EnchantType.ExtraEnchant => tier switch
                {
                    3 => 3,
                    _ => 0
                },
                _ => 0
            };
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            viceTextMesh  = transform.Find("ViceName").Find("Title").GetComponent<TextMeshProUGUI>();
            valueTextMesh = transform.Find("Value").GetComponent<TextMeshProUGUI>();

            GetComponentsInChildren(viceBubbleList);
        }
#endif
    }
}
