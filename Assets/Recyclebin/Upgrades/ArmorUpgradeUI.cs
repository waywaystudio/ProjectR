// using Common;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Lobby.UI.Forge.Upgrades
// {
//     public class ArmorUpgradeUI : MonoBehaviour, IEditable
//     {
//         [SerializeField] private EquipmentSlotType slotType;
//         [SerializeField] private Button button;
//         [SerializeField] private TextMeshProUGUI buttonTextMesh;
//
//
//         public void OnButtonClicked()
//         {
//             var equipment = LobbyDirector.Forge.VenturerArmor();
//
//             if (equipment.Tier == 3 && equipment.Level == 6)
//             {
//                 buttonTextMesh.text = "MAX";
//                 return;
//             }
//
//             buttonTextMesh.text = equipment.Level == 6 ? "Evolve" : "Upgrade";
//
//             equipment.Upgrade();
//         }
//
//
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             button         = GetComponent<Button>();
//             buttonTextMesh = GetComponentInChildren<TextMeshProUGUI>();
//
//             if (button.onClick.GetPersistentEventCount() == 0)
//             {
//                 button.onClick.AddPersistantListenerInEditor(this, "OnButtonClicked");
//             }
//         }
// #endif
//     }
// }
