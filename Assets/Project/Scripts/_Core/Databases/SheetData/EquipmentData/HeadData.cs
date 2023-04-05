/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.EquipmentData
{    
    public partial class HeadData : DataObject<HeadData.Head>
    {
        [Serializable]
        public class Head : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private Int32 equipable;
			[SerializeField] private Single power;
			[SerializeField] private Single health;
			[SerializeField] private Single criticalChance;
			[SerializeField] private Single criticalDamage;
			[SerializeField] private Single haste;
			[SerializeField] private Single armor;
			[SerializeField] private Int32 extraSkillCode;
			[SerializeField] private Single extraPower;

			public Int32 ID => id;
			public String Name => name;
			public Int32 Equipable => equipable;
			public Single Power => power;
			public Single Health => health;
			public Single CriticalChance => criticalChance;
			public Single CriticalDamage => criticalDamage;
			public Single Haste => haste;
			public Single Armor => armor;
			public Int32 ExtraSkillCode => extraSkillCode;
			public Single ExtraPower => extraPower;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "11-gKd8b56WeoWl9uSpXNDpf1nA6tlQ6gPkm5Ky5LuC0";
        public readonly string SpreadSheetName = "EquipmentData";
        public readonly string WorkSheetName = "Head";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Head>("EquipmentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Head");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
