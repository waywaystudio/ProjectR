// using System;
// using UnityEngine;
//
// namespace Core
// {
//     public class BoolTable : DelegateTable<int, Func<bool>>
//     {
//         public void Register(int key, Func<bool> predicate)
//         {
//             if (ContainsKey(key)) Debug.LogWarning($"Key is already Exist. key:{key}");
//
//             TryAdd(key, predicate);
//         }
//
//         public bool IsAllTrue()
//         {
//             foreach (var item in this) 
//                 if (!item.Value.Invoke()) return false;
//
//             return true;
//         }
//
//         public bool IsAllFalse()
//         {
//             foreach (var item in this) 
//                 if (item.Value.Invoke()) return false;
//
//             return true;
//         }
//
//         public bool HasTrue(out Func<bool> source)
//         {
//             foreach (var item in this)
//             {
//                 if (!item.Value.Invoke()) continue;
//                 
//                 source = item.Value;
//                 return true;
//             }
//
//             source = null;
//             return false;
//         }
//
//         public bool HasTrue()
//         {
//             foreach (var item in this) 
//                 if (item.Value.Invoke()) return true;
//
//             return false;
//         }
//
//         public bool HasFalse(out Func<bool> source)
//         {
//             foreach (var item in this)
//             {
//                 if (item.Value.Invoke()) continue;
//                 
//                 source = item.Value;
//                 return true;
//             }
//
//             source = null;
//             return false;
//         }
//
//         public bool HasFalse()
//         {
//             foreach (var item in this) 
//                 if (!item.Value.Invoke()) return true;
//
//             return false;
//         }
//
//         public int TrueCount()
//         {
//             var result = 0;
//
//             foreach (var item in this)
//             {
//                 if (item.Value.Invoke()) result++;
//             }
//
//             return result;
//         }
//
//         public int FalseCount()
//         {
//             var result = 0;
//
//             foreach (var item in this)
//             {
//                 if (!item.Value.Invoke()) result++;
//             }
//
//             return result;
//         }
//     }
// }
