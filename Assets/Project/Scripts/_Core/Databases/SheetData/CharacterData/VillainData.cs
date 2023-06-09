/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.CharacterData
{    
    public partial class VillainData : DataObject<VillainData.Villain>
    {
        [Serializable]
        public class Villain : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private String fullName;
			[SerializeField] private String subName;
			[SerializeField] private String backgroundNarrative;
			[SerializeField] private Single defaultDamage;
			[SerializeField] private Single power;
			[SerializeField] private Single health;
			[SerializeField] private Single maxResource;
			[SerializeField] private Single moveSpeed;
			[SerializeField] private Single critical;
			[SerializeField] private Single haste;
			[SerializeField] private Single armor;
			[SerializeField] private List<Int32> defaultSkills;
			[SerializeField] private String iconFileName;

			public Int32 ID => id;
			public String Name => name;
			public String FullName => fullName;
			public String SubName => subName;
			public String BackgroundNarrative => backgroundNarrative;
			public Single DefaultDamage => defaultDamage;
			public Single Power => power;
			public Single Health => health;
			public Single MaxResource => maxResource;
			public Single MoveSpeed => moveSpeed;
			public Single Critical => critical;
			public Single Haste => haste;
			public Single Armor => armor;
			public List<Int32> DefaultSkills => defaultSkills;
			public String IconFileName => iconFileName;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1BNf_4jZonJqd1ocWNGQoQDKNGtS_vtVy7H5Lmr8PVt0";
        public readonly string SpreadSheetName = "CharacterData";
        public readonly string WorkSheetName = "Villain";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Villain>("CharacterData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Villain");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
