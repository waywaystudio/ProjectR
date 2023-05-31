/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorScriptableObject.cs'  */
//     ReSharper disable BuiltInTypeReferenceStyle
//     ReSharper disable PartialTypeWithSinglePart
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Databases.SheetData.ContentData
{    
    public partial class UpgradesData : DataObject<UpgradesData.Upgrades>
    {
        [Serializable]
        public class Upgrades : IIdentifier
        {
			[SerializeField] private Int32 id;
			[SerializeField] private String name;
			[SerializeField] private Int32 maxLevel;
			[SerializeField] private List<Int32> levelList;

			public Int32 ID => id;
			public String Name => name;
			public Int32 MaxLevel => maxLevel;
			public List<Int32> LevelList => levelList;

        }
        
#region Editor Functions.
    #if UNITY_EDITOR
        public readonly string SpreadSheetID = "1yO5sJqxMvySDiihls5pwiHQWoJGysrT7LBmL16HhHRM";
        public readonly string SpreadSheetName = "ContentData";
        public readonly string WorkSheetName = "Upgrades";    
  
        private void LoadFromJson()
        {
    
            List = UnityGoogleSheet.Editor.Core.UgsEditorUtility
                .LoadFromJson<Upgrades>("ContentData"); 
        }
        
        private void LoadFromGoogleSpreadSheet()
        {
            UnityGoogleSheet.Editor.Core.UgsExplorer
                .ParseSpreadSheet(SpreadSheetID, "Upgrades");

            LoadFromJson();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.Refresh();
        }

    #endif
#endregion
    }
}
