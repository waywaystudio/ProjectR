// using System.Collections;
// using Character.Combat;
// using Core;
// using UnityEngine;
//
// namespace Character.Combat.StatusEffects.Buff
// {
//     public class FuryBuff : StatusEffectObject
//     {
//         [SerializeField] private HasteValue hasteValue;
//
//         protected override IEnumerator Effectuate()
//         {
//             WaitBuffer = new WaitForSeconds(Duration);
//
//             Provider.StatTable.Register(ActionCode, hasteValue);
//
//             yield return WaitBuffer;
//
//             Provider.StatTable.Unregister(ActionCode, hasteValue);
//             Complete();
//         }
//         
//         
// #if UNITY_EDITOR
//         public override void SetUp()
//         {
//             base.SetUp();
//
//             hasteValue.Value = CombatValue;
//         }
// #endif
//     }
// }
