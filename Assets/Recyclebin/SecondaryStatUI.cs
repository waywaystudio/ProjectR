// using System.Collections.Generic;
// using Common.Equipments;
// using Common.UI;
// using UnityEngine;
//
// namespace Lobby.UI.Forge.Evolves
// {
//     public class SecondaryStatUI : MonoBehaviour, IEditable
//     {
//         [SerializeField] private List<StatInfoUI> dynamicStatUIList;
//         [SerializeField] private List<MaterialInfoUI> materialUIList;
//         
//         public void OnReloadForge()
//         {
//             dynamicStatUIList.ForEach(statUI => statUI.gameObject.activeSelf.OnTrue(() => statUI.gameObject.SetActive(false)));
//             materialUIList.ForEach(materialUI => materialUI.gameObject.activeSelf.OnTrue(() => materialUI.gameObject.SetActive(false)));
//             
//             // var focusRelic = LobbyDirector.UI.Forge.FocusRelic;
//             var currentEquipmentEntity = LobbyDirector.UI.Forge.FocusEquipment;
//             var tier                   = currentEquipmentEntity.Tier;
//             
//             ReloadStat(currentEquipmentEntity);
//             ReloadMaterial(tier);
//         }
//         
//         private void ReloadStat(EquipmentEntity equipEntity)
//         {
//             // var equipmentSpec = equipEntity.GetRelic(relicType).StatSpec;
//             //
//             // equipmentSpec?.IterateOverStats((stat, index) =>
//             // {
//             //     if (dynamicStatUIList.Count < index) return;
//             //     
//             //     dynamicStatUIList[index].gameObject.SetActive(true);
//             //     dynamicStatUIList[index].SetValue(stat);
//             // });
//         }
//
//         private void ReloadMaterial(int tier)
//         {
//             if (materialUIList.Count < 1 || tier == 0)
//             {
//                 return;
//             }
//
//             // var profitMaterialType = tier switch
//             // {
//             //     1 => MaterialType.VirtuousShard,
//             //     2 => MaterialType.VirtuousStone,
//             //     3 => MaterialType.VirtuousCrystal,
//             //     _ => MaterialType.None,
//             // };
//             //
//             // materialUIList[0].gameObject.SetActive(true);
//             // materialUIList[0].SetInfoUI(profitMaterialType, "5");
//         }
//         
//         
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             GetComponentsInChildren(dynamicStatUIList);
//             GetComponentsInChildren(materialUIList);
//         }
// #endif
//     }
// }
