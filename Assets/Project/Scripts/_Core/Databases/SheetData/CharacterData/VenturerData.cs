/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.CharacterData
{    
    public partial class VenturerData : DataObject<VenturerData.Venturer>
    {
        [Serializable]
        public class Venturer : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private String characterName;
			[SerializeField] private String baseRole;
			[SerializeField] private Single defaultDamage;
			[SerializeField] private Single basePower;
			[SerializeField] private Single health;
			[SerializeField] private Single maxResource;
			[SerializeField] private Single moveSpeed;
			[SerializeField] private Single critical;
			[SerializeField] private Single haste;
			[SerializeField] private Single armor;
			[SerializeField] private List<Int32> defaultSkills;
			[SerializeField] private List<Int32> initialEquipments;

			public Int32 ID => id;
			public String Name => name;
			public String CharacterName => characterName;
			public String BaseRole => baseRole;
			public Single DefaultDamage => defaultDamage;
			public Single BasePower => basePower;
			public Single Health => health;
			public Single MaxResource => maxResource;
			public Single MoveSpeed => moveSpeed;
			public Single Critical => critical;
			public Single Haste => haste;
			public Single Armor => armor;
			public List<Int32> DefaultSkills => defaultSkills;
			public List<Int32> InitialEquipments => initialEquipments;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1BNf_4jZonJqd1ocWNGQoQDKNGtS_vtVy7H5Lmr8PVt0";
        public readonly string SpreadSheetName = "CharacterData";
        public readonly string WorkSheetName = "Venturer";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Venturer>("CharacterData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Venturer");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
