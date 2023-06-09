// using Common;
// using Sirenix.OdinInspector;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Lobby.UI.Forge.Upgrades
// {
//     public class ViceController : MonoBehaviour, IEditable
//     {
//         [SerializeField] private EquipmentSlotType slotType;
//         [SerializeField] private EnchantType enchantType;
//         [SerializeField] private Slider viceSlider;
//         [SerializeField] private TextMeshProUGUI viceTextMesh;
//         [SerializeField] private TextMeshProUGUI valueTextMesh;
//         // [SerializeField] private List<ViceBubble> viceBubbleList;
//
//         private IEquipment CurrentEquipment => LobbyDirector.Forge.VenturerArmor();
//
//         public void GetNextVice()
//         {
//             // var currentVice = CurrentEquipment.GetEnchantedEthos(enchantType);
//             // var nextVice = currentVice.EthosType.GetNextVice();
//             //
//             // // DisEnchant Current Vice;
//             // CurrentEquipment.DisEnchant(enchantType);
//             //
//             // // Set Text
//             // CurrentEquipment.Enchant(enchantType, nextVice, 0);
//             // viceTextMesh.text  = nextVice.ToString();
//             // valueTextMesh.text = "";
//         }
//
//         public void GetPrevVice()
//         {
//             // var currentVice = CurrentEquipment.GetEnchantedEthos(enchantType);
//             // var prevVice = currentVice.EthosType.GetPrevVice();
//             //
//             // // DisEnchant Current Vice;
//             // CurrentEquipment.DisEnchant(enchantType);
//             //
//             // // Set Text
//             // CurrentEquipment.Enchant(enchantType, prevVice, 0);
//             // viceTextMesh.text  = prevVice.ToString();
//             // valueTextMesh.text = "";
//         }
//
//         public void OnEthosValueChanged(int value)
//         {
//             
//         }
//
//
//         [Button]
//         public void OnEthosChanged()
//         {
//             // var tier = CurrentEquipment.Tier;
//             // var maxBubble = GetMaxBubble(tier); // Max Able to Active Bubble Count
//             // var currentVice = CurrentEquipment.GetEnchantedEthos(enchantType);
//             // var currentViceValue = currentVice.Value;
//             // var targetMaterial = currentVice.EthosType.ConvertToViceMaterial();
//             // var remainViceProgress = PartyCamp.Inventories.GetMaterialCount(targetMaterial);
//             //
//             // // Bubble Setting
//             // viceBubbleList.ForEach((bubble, index) =>
//             // {
//             //     // ex.
//             //     // Able To Active 9
//             //     // Value 5
//             //     // Able to On 6
//             //     // Disable To On 3
//             //     if (index >= maxBubble)
//             //     {
//             //         bubble.DeActive();
//             //         return;
//             //     }
//             //
//             //     if (maxBubble > index)
//             //     {
//             //         bubble.Active();
//             //         
//             //         if (currentViceValue > index)
//             //         {
//             //             bubble.TurnOn();
//             //         }
//             //         else
//             //         {
//             //             bubble.Disable();
//             //         }
//             //     }
//             // });
//             //
//             // // Set Title
//             // var viceValue = 0;
//             //
//             // if (CurrentEquipment.PrimeVice == null || CurrentEquipment.PrimeVice.EthosType == EthosType.None)
//             // {
//             //     viceTextMesh.text  = "Not Enchanted";
//             //     valueTextMesh.text = "";
//             //     viceBubbleList.ForEach(bubble => bubble.DeActive());
//             // }
//             // else
//             // {
//             //     viceTextMesh.text  = CurrentEquipment.PrimeVice.EthosType.ToString();
//             //     viceValue          = CurrentEquipment.PrimeVice.Value;
//             //     valueTextMesh.text = viceValue.ToRoman();
//             // }
//             //
//             // // Bubble Activity
//             // viceBubbleList.ForEach((bubble, index) =>
//             // {
//             //     if (viceValue > index) bubble.TurnOn();
//             //     else
//             //     {
//             //         if (bubble.IsActive)
//             //         {
//             //             bubble.TurnOff();
//             //         }
//             //     }
//             // });
//         }
//
//
//         private int GetMaxBubble(int tier)
//         {
//             return enchantType switch
//             {
//                 EnchantType.None => 0,
//                 EnchantType.PrimeEnchant => tier switch
//                 {
//                     1 => 6,
//                     2 => 9,
//                     3 => 12,
//                     _ => 0
//                 },
//                 EnchantType.SubEnchant => tier switch
//                 {
//                     2 => 3,
//                     3 => 6,
//                     _ => 0
//                 },
//                 EnchantType.ExtraEnchant => tier switch
//                 {
//                     3 => 3,
//                     _ => 0
//                 },
//                 _ => 0
//             };
//         }
//
//
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             viceSlider    = GetComponentInChildren<Slider>();
//             viceTextMesh  = transform.Find("ViceName").Find("Title").GetComponent<TextMeshProUGUI>();
//             valueTextMesh = transform.Find("ViceSlider").Find("Value").GetComponent<TextMeshProUGUI>();
//
//             // GetComponentsInChildren(viceBubbleList);
//         }
// #endif
//     }
// }
