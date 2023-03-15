// using System.Collections.Generic;
// using Character.Adventurers;
// using Common;
// using UnityEngine;
//
// namespace Character.ActionFrames.AdventurerFrame
// {
//     public class StatusEffectWindow : MonoBehaviour, IEditable
//     {
//         [SerializeField] private List<StatusEffectSymbol> deBuffActionSlotList;
//         [SerializeField] private List<StatusEffectSymbol> buffActionSlotList;
//
//         private Adventurer adventurer;
//
//
//         public void Initialize(Adventurer adventurer)
//         {
//             this.adventurer = adventurer;
//             
//             adventurer.OnTakeBuff.Register("RegisterBuffUI", RegisterBuffUI);
//             adventurer.OnTakeDeBuff.Register("RegisterDeBuffUI", RegisterDeBuffUI);
//         }
//
//         public void Focus()
//         {
//             
//         }
//
//         public void Release()
//         {
//             
//         }
//         
//
//         private void RegisterBuffUI(StatusEffectEntity entity)
//         {
//             if (entity.IsOverride) return;
//             if (!TryGetEmptyBuff(out var symbol))
//             {
//                 Debug.LogWarning($"Not Enough Buff Symbol Object. Add more buffer or pooling");
//                 return;
//             }
//
//             // symbol.Register(entity.Effect);
//         }
//
//         private void RegisterDeBuffUI(StatusEffectEntity entity)
//         {
//             if (entity.IsOverride) return;
//             if (!TryGetEmptyDeBuff(out var symbol))
//             {
//                 Debug.LogWarning($"Not Enough DeBuff Symbol Object. Add more buffer or pooling");
//                 return;
//             }
//
//             // symbol.Register(entity.Effect);
//         }
//         
//         private void UpdateStatusEffect()
//         {
//             // Add ab.StatusEffects
//             adventurer.DynamicStatEntry.BuffTable.Values.ForEach(effect =>
//             {
//                 if (!TryGetEmptyBuff(out var buffSlot)) return;
//                 
//                 // buffSlot.Register(effect);
//             });
//             
//             // Add ab.StatusEffects
//             adventurer.DynamicStatEntry.DeBuffTable.Values.ForEach(effect =>
//             {
//                 if (!TryGetEmptyDeBuff(out var deBuffSlot)) return;
//                 
//                 // deBuffSlot.Register(effect);
//             });
//         }
//             
//         
//         private bool TryGetEmptyBuff(out StatusEffectSymbol buffSlot) => TryGetEmptySymbol(StatusEffectType.Buff, out buffSlot);
//         private bool TryGetEmptyDeBuff(out StatusEffectSymbol deBuffSlot) => TryGetEmptySymbol(StatusEffectType.DeBuff, out deBuffSlot);
//         private bool TryGetEmptySymbol(StatusEffectType statusType, out StatusEffectSymbol symbol)
//         {
//             var targetList = statusType == StatusEffectType.Buff
//                 ? buffActionSlotList
//                 : deBuffActionSlotList;
//             
//             symbol = targetList.Find(item => !item.isActiveAndEnabled);
//
//             return symbol is not null;
//         }
//
//
//         private void OnEnable()
//         {
//             adventurer = GetComponentInParent<Adventurer>();
//         }
//
//         private void OnDisable()
//         {
//             Release();
//         }
//
//
// #if UNITY_EDITOR
//         public void EditorSetUp()
//         {
//             GetComponentsInChildren(true, deBuffActionSlotList);
//             GetComponentsInChildren(true, buffActionSlotList);
//         }
// #endif
//     }
// }