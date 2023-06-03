using System.Collections.Generic;
using Common;
using Common.Equipments;
using Common.PartyCamps;
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

        private EquipmentEntity CurrentEquipment => LobbyDirector.UI.Forge.VenturerEquipment(slotType);

        public void GetNextVice()
        {
            var currentVice = CurrentEquipment.GetEnchantedEthos(enchantType);
            var nextVice = currentVice.EthosType.GetNextVice();
            
            // DisEnchant Current Vice;
            CurrentEquipment.DisEnchant(enchantType);
            
            // Set Text
            CurrentEquipment.Enchant(enchantType, nextVice, 0);
            viceTextMesh.text  = nextVice.ToString();
            valueTextMesh.text = "";
        }

        public void GetPrevVice()
        {
            var currentVice = CurrentEquipment.GetEnchantedEthos(enchantType);
            var prevVice = currentVice.EthosType.GetPrevVice();
            
            // DisEnchant Current Vice;
            CurrentEquipment.DisEnchant(enchantType);
            
            // Set Text
            CurrentEquipment.Enchant(enchantType, prevVice, 0);
            viceTextMesh.text  = prevVice.ToString();
            valueTextMesh.text = "";
        }

        public void OnEthosValueChanged(int value)
        {
            
        }


        [Button]
        public void OnEthosChanged()
        {
            var tier = CurrentEquipment.Tier;
            var maxBubble = GetMaxBubble(tier); // Max Able to Active Bubble Count
            var currentVice = CurrentEquipment.GetEnchantedEthos(enchantType);
            var currentViceValue = currentVice.Value;
            var targetMaterial = currentVice.EthosType.ConvertToViceMaterial();
            var remainViceProgress = PartyCamp.Inventories.GetMaterialCount(targetMaterial);
            
            // Bubble Setting
            viceBubbleList.ForEach((bubble, index) =>
            {
                // ex.
                // Able To Active 9
                // Value 5
                // Able to On 6
                // Disable To On 3
                if (index >= maxBubble)
                {
                    bubble.DeActive();
                    return;
                }

                if (maxBubble > index)
                {
                    bubble.Active();
                    
                    if (currentViceValue > index)
                    {
                        bubble.TurnOn();
                    }
                    else
                    {
                        bubble.Disable();
                    }
                }
            });
            
            // Set Title
            var viceValue = 0;

            if (CurrentEquipment.PrimeVice == null || CurrentEquipment.PrimeVice.EthosType == EthosType.None)
            {
                viceTextMesh.text  = "Not Enchanted";
                valueTextMesh.text = "";
                viceBubbleList.ForEach(bubble => bubble.DeActive());
            }
            else
            {
                viceTextMesh.text  = CurrentEquipment.PrimeVice.EthosType.ToString();
                viceValue          = CurrentEquipment.PrimeVice.Value;
                valueTextMesh.text = viceValue.ToRoman();
            }
            
            // Bubble Activity
            viceBubbleList.ForEach((bubble, index) =>
            {
                if (viceValue > index) bubble.TurnOn();
                else
                {
                    if (bubble.IsActive)
                    {
                        bubble.TurnOff();
                    }
                }
            });

            // Inventory에서 Vice Material 남는 값 받아오기.
            // max 값 잡아주기
            // Tier Max & material Max
            // max value 잡아주기
        }

        public void ChangeEthosValue(float barValue)
        {
            var equipment = LobbyDirector.UI.Forge.VenturerEquipment(slotType);
            var ethosEntity = equipment.GetEnchantedEthos(enchantType);
            
            ethosEntity.Value  = (int)barValue;
            valueTextMesh.text = ethosEntity.Value.ToRoman();
        }


        private int GetMaxBubble(int tier)
        {
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
