// using System.Collections.Generic;
// using Core;
// using MainGame;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace Character.Combat.StatusEffects
// {
//     using Buff;
//     using DeBuff;
//
//
//     // TODO. 버프와 디버프를 C# 클래스 구조로 구성한 것에 대해서, 스스로 의심되는 부분이 많다;
//     public class StatusEffecting : MonoBehaviour
//     {
//         private ICombatTaker taker;
//         private int instanceID;
//
//         [ShowInInspector] public Dictionary<DataIndex, BaseStatusEffect> BuffTable { get; set; } = new();
//         [ShowInInspector] public Dictionary<DataIndex, BaseStatusEffect> DeBuffTable { get; set; } = new();
//
//         public void TryAdd(IActionSender entity)
//         {
//             BaseStatusEffect statusEffect;
//             
//             switch (entity.Provider.ActionCode)
//             {
//                 case DataIndex.BloodDrainBuff : statusEffect = GenerateStatusEffect<BloodDrainBuff>(entity); break;
//                 case DataIndex.CorruptionDeBuff : statusEffect = GenerateStatusEffect<CorruptionDeBuff>(entity); break;
//                 case DataIndex.FireballDeBuff : statusEffect = GenerateStatusEffect<FireballDeBuff>(entity); break;
//                 case DataIndex.FuryBuff : statusEffect = GenerateStatusEffect<FuryBuff>(entity); break;
//                 case DataIndex.RoarDeBuff : statusEffect = GenerateStatusEffect<RoarDeBuff>(entity); break;
//                 
//                 default: return;
//             }
//
//             var targetTable = statusEffect.IsBuff
//                 ? BuffTable
//                 : DeBuffTable;
//             
//             if (targetTable.ContainsKey(entity.Provider.ActionCode))
//             {
//                 // Implement Compare
//                 return;
//             }
//
//             statusEffect.InvokeRoutine = StartCoroutine(statusEffect.MainAction());
//             targetTable.TryAdd(entity.Provider.ActionCode, statusEffect);
//         }
//
//         public void TryRemove(ICombatProvider provider) => TryRemove(provider.ActionCode);
//         public void TryRemove(DataIndex key)
//         {
//             var targetTable = key.ToString().EndsWith("DeBuff")
//                 ? DeBuffTable
//                 : BuffTable;
//             
//             if (!targetTable.TryGetValue(key, out var statusEffect)) return;
//             
//             if (statusEffect.InvokeRoutine != null) 
//                 StopCoroutine(statusEffect.InvokeRoutine);
//
//             targetTable.TryRemove(key);
//         }
//         
//
//         private T GenerateStatusEffect<T>(IOrigin entity) where T : BaseStatusEffect, new()
//         {
//             var statusEffectData = MainData.GetStatusEffect(entity.Provider.ActionCode);
//             
//             return new T
//             {
//                 ActionCode = (DataIndex)statusEffectData.ID,
//                 IsBuff = statusEffectData.IsBuff,
//                 Duration = statusEffectData.Duration,
//                 CombatValue = statusEffectData.CombatValue,
//                 Provider = entity.Provider,
//                 TakerInfo = taker,
//                 Callback = () => TryRemove(entity.Provider),
//             };
//         }
//
//         private void OnEnable()
//         {
//             instanceID = GetInstanceID();
//             
//             taker = GetComponentInParent<ICombatTaker>();
//             taker.OnTakeStatusEffect.Register(instanceID, TryAdd);
//             taker.OnDispelStatusEffect.Register(instanceID, TryRemove);
//         }
//
//         private void OnDisable()
//         {
//             taker.OnTakeStatusEffect.Unregister(instanceID);
//             taker.OnDispelStatusEffect.Unregister(instanceID);
//         }
//     }
// }
