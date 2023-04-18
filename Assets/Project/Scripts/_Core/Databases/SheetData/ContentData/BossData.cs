/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.ContentData
{    
    public partial class BossData : DataObject<BossData.Boss>
    {
        [Serializable]
        public class Boss : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private Single power;
			[SerializeField] private Single health;
			[SerializeField] private Single maxResource;
			[SerializeField] private Single moveSpeed;
			[SerializeField] private Single critical;
			[SerializeField] private Single haste;
			[SerializeField] private Single armor;

			public Int32 ID => id;
			public String Name => name;
			public Single Power => power;
			public Single Health => health;
			public Single MaxResource => maxResource;
			public Single MoveSpeed => moveSpeed;
			public Single Critical => critical;
			public Single Haste => haste;
			public Single Armor => armor;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public readonly string SpreadSheetName = "ContentData";
        public readonly string WorkSheetName = "Boss";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Boss>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Boss");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
