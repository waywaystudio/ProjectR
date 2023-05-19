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
			[SerializeField] private Single power;
			[SerializeField] private Single health;
			[SerializeField] private Single armor;
			[SerializeField] private String iconFileName;

			public Int32 ID => id;
			public String Name => name;
			public Single Power => power;
			public Single Health => health;
			public Single Armor => armor;
			public String IconFileName => iconFileName;

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
