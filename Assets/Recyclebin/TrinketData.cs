// /*     ===== Do not touch this. Auto Generated Code. =====    */
// /*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
// //     ReSharper disable BuiltInTypeReferenceStyle
// //     ReSharper disable PartialTypeWithSinglePart
// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
//
// namespace Databases.SheetData.EquipmentData
// {    
//     public partial class TrinketData : DataObject<TrinketData.Trinket>
//     {
//         [Serializable]
//         public class Trinket : IIdentifier
//         {
// 			[SerializeField] private Int32 id;
// 			[SerializeField] private String name;
// 			[SerializeField] private Int32 equipable;
// 			[SerializeField] private Single power;
// 			[SerializeField] private Single health;
// 			[SerializeField] private Single criticalChance;
// 			[SerializeField] private Single criticalDamage;
// 			[SerializeField] private Single haste;
// 			[SerializeField] private Int32 extraSkillCode;
// 			[SerializeField] private Single extraPower;
// 			[SerializeField] private String iconFileName;
//
// 			public Int32 ID => id;
// 			public String Name => name;
// 			public Int32 Equipable => equipable;
// 			public Single Power => power;
// 			public Single Health => health;
// 			public Single CriticalChance => criticalChance;
// 			public Single CriticalDamage => criticalDamage;
// 			public Single Haste => haste;
// 			public Int32 ExtraSkillCode => extraSkillCode;
// 			public Single ExtraPower => extraPower;
// 			public String IconFileName => iconFileName;
//
//         }
//         
// #region Editor Functions.
//     #if UNITY_EDITOR
//         public readonly string SpreadSheetID = "11-gKd8b56WeoWl9uSpXNDpf1nA6tlQ6gPkm5Ky5LuC0";
//         public readonly string SpreadSheetName = "EquipmentData";
//         public readonly string WorkSheetName = "Trinket";    
//   
//         private void LoadFromJson()
//         {
//     
//             List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
//                 .LoadFromJson<Trinket>("EquipmentData"); 
//         }
//         
//         private void LoadFromGoogleSpreadSheet()
//         {
//             UnityGoogleSheet.Editor.Core.UgsExplorer
//                 .ParseSpreadSheet(SpreadSheetID, "Trinket");
//
//             LoadFromJson();
//             UnityEditor.EditorUtility.SetDirty(this);
//             UnityEditor.AssetDatabase.Refresh();
//         }
//
//     #endif
// #endregion
//     }
// }
