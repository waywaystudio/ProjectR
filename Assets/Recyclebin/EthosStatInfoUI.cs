// using System.Collections.Generic;
// using Common;
// using Common.PartyCamps;
// using Common.UI;
// using UnityEngine;
//
// namespace Lobby.UI.Forge.Adventurers
// {
//     /* Adventurer */
//     public class EthosStatInfoUI : MonoBehaviour, IEditable
//     {
//         [SerializeField] private EthosType ethosType;
//         [SerializeField] private List<ViceInfoUI> viceStatUIList;
//
//         private int Deficiency { get; set; }
//         private int Mean { get; set; }
//         private int Excess { get; set; }
//
//
//         public void OnReloadForge()
//         {
//             if (viceStatUIList.Count != 3)
//             {
//                 Debug.LogError("Require 3 ViceStatUI Objects");
//                 return;
//             }
//             
//             var deficiencyValue  = GetEthosValue(ethosType - 1);
//             var adventurerVirtue = GetEthosValue(ethosType);
//             var excessValue  = GetEthosValue(ethosType + 1);
//
//             // For Optimization
//             if (Deficiency != deficiencyValue)
//             {
//                 Deficiency = deficiencyValue;
//                 viceStatUIList[0].SetVice(ethosType - 1, deficiencyValue);
//             }
//             
//             if (Mean != adventurerVirtue)
//             {
//                 Mean = adventurerVirtue;
//                 viceStatUIList[1].SetVice(ethosType, adventurerVirtue);
//             }
//             
//             if (Excess != excessValue)
//             {
//                 Excess = excessValue;
//                 viceStatUIList[2].SetVice(ethosType + 1, excessValue);
//             }
//         }
//
//
//         private int GetEthosValue(EthosType ethosType)
//         {
//             var targetAdventurer = LobbyDirector.UI.Forge.FocusAdventurer;
//             var value =  PartyCamp.Characters
//                                   .GetData(targetAdventurer)?
//                                   .GetEthosValue(ethosType);
//
//             return value ?? 0;
//         }
//         
//         
//         
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             GetComponentsInChildren(viceStatUIList);
//         }
// #endif
//     }
// }
