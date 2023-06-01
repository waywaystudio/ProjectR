// using System.Collections.Generic;
// using Common.Equipments;
// using Common.UI;
// using UnityEngine;
//
// namespace Lobby.UI.Forge.Evolves
// {
//     /* Equipment's */
//     public class ViceStatUI : MonoBehaviour, IEditable
//     {
//         [SerializeField] private List<ViceInfoUI> viceStatUIList;
//         [SerializeField] private List<MaterialInfoUI> materialUIList;
//         
//         public void OnReloadForge()
//         {
//             viceStatUIList.ForEach(statUI => statUI.gameObject.activeSelf.OnTrue(() => statUI.gameObject.SetActive(false)));
//             materialUIList.ForEach(materialUI => materialUI.gameObject.activeSelf.OnTrue(() => materialUI.gameObject.SetActive(false)));
//
//             var currentEquipmentEntity = LobbyDirector.UI.Forge.FocusEquipment;
//             // var focusRelic = LobbyDirector.UI.Forge.FocusRelic;
//             var tier = currentEquipmentEntity.Tier;
//             
//             ReloadEthos(currentEquipmentEntity);
//             ReloadMaterial(tier);
//         }
//
//
//         private void ReloadEthos(EquipmentEntity equipEntity)
//         {
//             // var equipmentSpec = equipEntity.GetRelic(relicType).EthosSpec;
//             //
//             // equipmentSpec?.IterateOverStats((ethos, index) =>
//             // {
//             //     if (viceStatUIList.Count < index) return;
//             //     
//             //     viceStatUIList[index].gameObject.SetActive(true);
//             //     viceStatUIList[index].SetVice(ethos);
//             // });
//         }
//         private void ReloadMaterial(int tier)
//         {
//             if (materialUIList.Count < 2 || tier == 0)
//             {
//                 return;
//             }
//
//             // var firstMaterial  = MaterialType.None;
//             // var secondMaterial = MaterialType.None;
//             //
//             // switch (tier)
//             // {
//             //     case 1:
//             //     {
//             //         firstMaterial  = MaterialType.ViciousShard;
//             //         secondMaterial = MaterialType.VirtuousShard;
//             //         break;
//             //     }
//             //     case 2:
//             //     {
//             //         firstMaterial  = MaterialType.ViciousStone;
//             //         secondMaterial = MaterialType.VirtuousStone;
//             //         break;
//             //     }
//             //     case 3:
//             //     {
//             //         firstMaterial  = MaterialType.ViciousCrystal;
//             //         secondMaterial = MaterialType.VirtuousCrystal;
//             //         break;
//             //     }
//             // }
//             //
//             // materialUIList[0].gameObject.SetActive(true);
//             // materialUIList[1].gameObject.SetActive(true);
//             // materialUIList[0].SetInfoUI(firstMaterial, "5");
//             // materialUIList[1].SetInfoUI(secondMaterial, "5");
//         }
//         
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             GetComponentsInChildren(viceStatUIList);
//             GetComponentsInChildren(materialUIList);
//         }
// #endif
//     }
// }
