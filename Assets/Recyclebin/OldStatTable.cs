// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace Common
// {
//     [ShowInInspector]
//     public class OldStatTable : Dictionary<StatType, StatValueTable>
//     {
//         public OldStatTable() : this(1) { }
//         public OldStatTable(int capacity) : base(capacity) { }
//
//         public float Power { get; set; }
//         public float Critical { get; set; }
//         public float Haste { get; set; }
//         public float MaxHp { get; set; }
//         public float MaxResource { get; set; }
//         public float MoveSpeed { get; set; }
//         public float Armor { get; set; }
//
//         public void Register(DataIndex key, StatValue statEntity, bool overwrite)
//         {
//             if (!ContainsKey(statEntity.StatCode))
//             {
//                 /* 순서 엄청 중요하다. 
//                  1. assign callback, 
//                  2. table add, 
//                  3. valueTable Register */
//                 var statValueTable = new StatValueTable { OnResultChanged = () => Recalculation(statEntity.StatCode) };
//
//                 Add(statEntity.StatCode, statValueTable);
//                 statValueTable.Register(key, statEntity, overwrite);
//             }
//             else
//                 this[statEntity.StatCode].Register(key, statEntity, overwrite);
//         }
//         
//         public void Register(DataIndex key, StatValue statEntity)
//         {
//             if (!ContainsKey(statEntity.StatCode))
//             {
//                 var statValueTable = new StatValueTable { OnResultChanged = () => Recalculation(statEntity.StatCode) };
//
//                 Add(statEntity.StatCode, statValueTable);
//                 statValueTable.Register(key, statEntity);
//             }
//             else
//                 this[statEntity.StatCode].Register(key, statEntity);
//         }
//         
//
//         public void Unregister(DataIndex key, StatValue statValue) => Unregister(key, statValue.StatCode);
//         public void Unregister(DataIndex key, StatType statCode)
//         {
//             if (ContainsKey(statCode)) this[statCode].Unregister(key);
//         }
//
//         public void UnionWith(OldStatTable target, bool overwrite = true)
//         {
//             target.ForEach(statEntry =>
//             {
//                 statEntry.Value.ForEach(statEntityTable =>
//                 {
//                     Register(statEntityTable.Key, statEntityTable.Value, overwrite);
//                 });
//             });
//         }
//         
//
//         private void Recalculation(StatType statCode)
//         {
//             var result = this[statCode].Result;
//             
//             switch (statCode)
//             {
//                 case StatType.Power :       Power       = result; break;
//                 case StatType.CriticalChance :    Critical    = result; break;
//                 case StatType.Haste :       Haste       = result; break;
//                 case StatType.MaxHp:        MaxHp       = result; break;
//                 case StatType.MaxResource : MaxResource = result; break;
//                 case StatType.MoveSpeed :   MoveSpeed   = result; break;
//                 case StatType.Armor :       Armor       = result; break;
//                 case StatType.None:
//                 {
//                     Debug.LogError($"statCode Missing. Input:{StatType.None}");
//                     return;
//                 }
//                 default:
//                 {
//                     Debug.LogError($"statCode Missing. Input:{statCode}");
//                     return;
//                 }
//             }
//         }
//     }
// }